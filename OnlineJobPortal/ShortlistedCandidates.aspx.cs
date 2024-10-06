using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Net.Mail;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace OnlineJobPortal
{
    public partial class ShortlistedCandidates : Page
    {
        // Retrieve the connection string from Web.config
        private readonly string connectionString = ConfigurationManager.ConnectionStrings["OnlineJobPortalDB"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Validate and retrieve JobID from QueryString
                if (Request.QueryString["JobID"] != null && int.TryParse(Request.QueryString["JobID"], out int jobId))
                {
                    // Store JobID in ViewState for later use
                    ViewState["JobID"] = jobId;

                    // Load job details
                    LoadJobDetails(jobId);

                    // Load shortlisted candidates
                    LoadShortlistedCandidates(jobId);

                    // Load job seekers into the dropdown for emailing
                    LoadJobSeekers();
                }
                else
                {
                    // Handle error - JobID not provided or invalid
                    lblError.Text = "Invalid Job ID.";
                }
            }
        }

        /// <summary>
        /// Loads the details of the selected job and displays them on the page.
        /// </summary>
        /// <param name="jobId">The ID of the job to load details for.</param>
        private void LoadJobDetails(int jobId)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = @"SELECT Title, Description, Location, Salary, JobType, JobCategory, RequiredYearsOfExperience, ApplicationDeadline
                                 FROM JobPosting
                                 WHERE JobID = @JobID";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@JobID", jobId);
                    try
                    {
                        conn.Open();
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                lblJobTitle.Text = reader["Title"].ToString();
                                lblJobDescription.Text = reader["Description"].ToString();
                                lblJobLocation.Text = reader["Location"].ToString();
                                lblJobSalary.Text = Convert.ToDecimal(reader["Salary"]).ToString("N2");
                                lblJobType.Text = reader["JobType"].ToString();
                                lblJobCategory.Text = reader["JobCategory"].ToString();
                                lblJobExperience.Text = reader["RequiredYearsOfExperience"].ToString();
                                lblJobDeadline.Text = Convert.ToDateTime(reader["ApplicationDeadline"]).ToString("MMMM dd, yyyy");
                            }
                            else
                            {
                                lblError.Text = "Job details not found.";
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        // Log the exception and display an error message
                        lblError.Text = "An error occurred while loading job details: " + ex.Message;
                        // Consider logging ex.Message to a file or logging system
                    }
                }
            }
        }

        /// <summary>
        /// Retrieves the current JobID from ViewState.
        /// </summary>
        /// <returns>The current JobID.</returns>
        private int GetCurrentJobId()
        {
            if (ViewState["JobID"] != null && int.TryParse(ViewState["JobID"].ToString(), out int jobId))
            {
                return jobId;
            }
            throw new InvalidOperationException("Job ID is not available.");
        }

        /// <summary>
        /// Loads and binds the list of shortlisted candidates to the GridView.
        /// </summary>
        /// <param name="jobId">The ID of the job to load candidates for.</param>
        private void LoadShortlistedCandidates(int jobId)
        {
            List<MatchingCandidate> shortlistedCandidates = new List<MatchingCandidate>();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                // Step 1: Get Job requirements
                JobRequirements jobRequirements = GetJobRequirements(conn, jobId);

                // Step 2: Get all shortlisted candidates
                List<Candidate> candidates = GetShortlistedCandidates(conn, jobId); // Only shortlisted

                // Step 3: For each candidate, compute matching percentage
                foreach (var candidate in candidates)
                {
                    double matchingPercentage = ComputeMatchingPercentage(jobRequirements, candidate);

                    if (matchingPercentage >=0)
                    {
                        // Get application status
                        string status = GetApplicationStatus(conn, jobId, candidate.JobSeekerID);

                        shortlistedCandidates.Add(new MatchingCandidate
                        {
                            Candidate = candidate,
                            MatchingPercentage = matchingPercentage,
                            ApplicationStatus = status
                        });
                    }
                }

                // Step 4: Order candidates by matching percentage descending
                shortlistedCandidates.Sort((x, y) => y.MatchingPercentage.CompareTo(x.MatchingPercentage));

                // Step 5: Bind to GridView
                gvMatchingCandidates.DataSource = shortlistedCandidates;
                gvMatchingCandidates.DataBind();
            }
        }

        /// <summary>
        /// Retrieves the current application status of a candidate.
        /// </summary>
        /// <param name="conn">The SQL connection.</param>
        /// <param name="jobId">The JobID.</param>
        /// <param name="jobSeekerID">The JobSeekerID.</param>
        /// <returns>The application status.</returns>
        private string GetApplicationStatus(SqlConnection conn, int jobId, int jobSeekerID)
        {
            try
            {
                string status = "Applied"; // Default status

                string query = @"SELECT Status FROM JobApplication 
                                 WHERE JobID = @JobID AND JobSeekerID = @JobSeekerID";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@JobID", jobId);
                    cmd.Parameters.AddWithValue("@JobSeekerID", jobSeekerID);
                    object result = cmd.ExecuteScalar();
                    if (result != null && result != DBNull.Value)
                    {
                        status = result.ToString();
                    }
                }

                return status;
            }
            catch (Exception ex)
            {
                // Log the exception
                System.Diagnostics.Debug.WriteLine($"Error retrieving application status: {ex.Message}");
                return "Error";
            }
        }

        /// <summary>
        /// Retrieves the job requirements including skills, degrees, and experience.
        /// </summary>
        /// <param name="conn">The SQL connection.</param>
        /// <param name="jobId">The JobID.</param>
        /// <returns>A JobRequirements object containing the requirements.</returns>
        private JobRequirements GetJobRequirements(SqlConnection conn, int jobId)
        {
            JobRequirements jobReq = new JobRequirements();

            // Get required skills
            jobReq.RequiredSkills = new List<int>(); // List of SkillTagID

            string querySkills = @"SELECT SkillTagID FROM JobSkill WHERE JobID = @JobID";
            using (SqlCommand cmd = new SqlCommand(querySkills, conn))
            {
                cmd.Parameters.AddWithValue("@JobID", jobId);
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int skillTagID = reader.GetInt32(0);
                        jobReq.RequiredSkills.Add(skillTagID);
                    }
                }
            }

            // Get required degrees
            jobReq.RequiredDegrees = new List<int>(); // List of DegreeID

            string queryDegrees = @"SELECT DegreeID FROM JobPostingDegree WHERE JobID = @JobID";
            using (SqlCommand cmd = new SqlCommand(queryDegrees, conn))
            {
                cmd.Parameters.AddWithValue("@JobID", jobId);
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int degreeID = reader.GetInt32(0);
                        jobReq.RequiredDegrees.Add(degreeID);
                    }
                }
            }

            // Get required experience
            string queryExperience = @"SELECT RequiredYearsOfExperience FROM JobPosting WHERE JobID = @JobID";
            using (SqlCommand cmd = new SqlCommand(queryExperience, conn))
            {
                cmd.Parameters.AddWithValue("@JobID", jobId);
                object result = cmd.ExecuteScalar();
                if (result != null && result != DBNull.Value)
                {
                    jobReq.RequiredExperience = Convert.ToInt32(result);
                }
                else
                {
                    jobReq.RequiredExperience = 0; // Default to 0 if not found
                }
            }

            return jobReq;
        }

        /// <summary>
        /// Retrieves a list of candidates who are shortlisted for the specified job.
        /// </summary>
        /// <param name="conn">The SQL connection.</param>
        /// <param name="jobId">The JobID.</param>
        /// <returns>A list of Candidate objects.</returns>
        private List<Candidate> GetShortlistedCandidates(SqlConnection conn, int jobId)
        {
            List<Candidate> candidates = new List<Candidate>();

            // Get JobSeekerID and YearsOfExperience from JobSeeker where status is 'Shortlisted'
            string queryCandidates = @"SELECT JS.JobSeekerID, JS.FirstName, JS.LastName, JS.YearsOfExperience, JS.ResumePath
                                       FROM JobSeeker JS
                                       INNER JOIN JobApplication JA ON JS.JobSeekerID = JA.JobSeekerID
                                       WHERE JA.JobID = @JobID AND JA.Status = 'Shortlisted'";
            using (SqlCommand cmd = new SqlCommand(queryCandidates, conn))
            {
                cmd.Parameters.AddWithValue("@JobID", jobId);
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Candidate candidate = new Candidate
                        {
                            JobSeekerID = reader.GetInt32(reader.GetOrdinal("JobSeekerID")),
                            FirstName = reader["FirstName"].ToString(),
                            LastName = reader["LastName"].ToString(),
                            YearsOfExperience = reader.IsDBNull(reader.GetOrdinal("YearsOfExperience")) ? 0 : reader.GetInt32(reader.GetOrdinal("YearsOfExperience")),
                            ResumePath = reader.IsDBNull(reader.GetOrdinal("ResumePath")) ? "" : reader["ResumePath"].ToString(),
                            SkillTagIDs = new List<int>(),
                            DegreeIDs = new List<int>()
                        };

                        candidates.Add(candidate);
                    }
                }
            }

            // For each candidate, get their skills and degrees
            foreach (var candidate in candidates)
            {
                // Get skills
                string querySkills = @"SELECT SkillTagID FROM JobSeekerSkill WHERE JobSeekerID = @JobSeekerID";
                using (SqlCommand cmd = new SqlCommand(querySkills, conn))
                {
                    cmd.Parameters.AddWithValue("@JobSeekerID", candidate.JobSeekerID);
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            candidate.SkillTagIDs.Add(reader.GetInt32(0));
                        }
                    }
                }

                // Get degrees
                string queryDegrees = @"SELECT DegreeID FROM AcademicDetails WHERE JobSeekerID = @JobSeekerID";
                using (SqlCommand cmd = new SqlCommand(queryDegrees, conn))
                {
                    cmd.Parameters.AddWithValue("@JobSeekerID", candidate.JobSeekerID);
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            candidate.DegreeIDs.Add(reader.GetInt32(0));
                        }
                    }
                }
            }

            return candidates;
        }

        /// <summary>
        /// Computes the matching percentage for a candidate based on job requirements.
        /// </summary>
        /// <param name="jobReq">The job requirements.</param>
        /// <param name="candidate">The candidate.</param>
        /// <returns>The matching percentage.</returns>
        private double ComputeMatchingPercentage(JobRequirements jobReq, Candidate candidate)
        {
            // Weights for different criteria
            double skillsWeight = 0.5;
            double degreesWeight = 0.3;
            double experienceWeight = 0.2;

            double skillsMatchPercentage = 0;
            double degreesMatchPercentage = 0;
            double experienceMatchPercentage = 0;

            // Calculate Skills Match
            if (jobReq.RequiredSkills.Count > 0)
            {
                int matchingSkills = 0;
                foreach (var requiredSkill in jobReq.RequiredSkills)
                {
                    if (candidate.SkillTagIDs.Contains(requiredSkill))
                    {
                        matchingSkills++;
                    }
                }
                skillsMatchPercentage = ((double)matchingSkills / jobReq.RequiredSkills.Count) * 100;
            }
            else
            {
                skillsMatchPercentage = 100; // No skills required
            }

            // Calculate Degrees Match
            if (jobReq.RequiredDegrees.Count > 0)
            {
                int matchingDegrees = 0;
                foreach (var requiredDegree in jobReq.RequiredDegrees)
                {
                    if (candidate.DegreeIDs.Contains(requiredDegree))
                    {
                        matchingDegrees++;
                    }
                }
                degreesMatchPercentage = ((double)matchingDegrees / jobReq.RequiredDegrees.Count) * 100;
            }
            else
            {
                degreesMatchPercentage = 100; // No degrees required
            }

            // Calculate Experience Match
            if (jobReq.RequiredExperience > 0)
            {
                double experienceRatio = ((double)candidate.YearsOfExperience / jobReq.RequiredExperience) * 100;
                experienceRatio = experienceRatio > 100 ? 100 : experienceRatio; // Cap at 100%
                experienceMatchPercentage = experienceRatio;
            }
            else
            {
                experienceMatchPercentage = 100; // No experience required
            }

            // Compute overall matching percentage based on weights
            double matchingPercentage = (skillsMatchPercentage * skillsWeight) +
                                        (degreesMatchPercentage * degreesWeight) +
                                        (experienceMatchPercentage * experienceWeight);

            return matchingPercentage;
        }

        /// <summary>
        /// Handles the RowCommand event of the GridView to manage candidate actions.
        /// </summary>
        /// <param name="sender">The GridView sender.</param>
        /// <param name="e">The GridViewCommandEventArgs.</param>
        protected void gvMatchingCandidates_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            // Clear previous messages
            lblError.Text = "";
            lblSuccessMessage.Text = "";
            lblEmailStatus.Text = "";

            // Validate and retrieve JobSeekerID from CommandArgument
            if (!int.TryParse(e.CommandArgument.ToString(), out int jobSeekerID))
            {
                lblError.Text = "Invalid Job Seeker ID.";
                return;
            }

            // Retrieve current JobID
            int jobId = GetCurrentJobId();

            switch (e.CommandName)
            {
                case "DownloadResume":
                    DownloadResume(jobSeekerID);
                    break;

                case "ViewProfile":
                    // Redirect to FullProfile.aspx with JobSeekerID
                    Response.Redirect($"FullProfile.aspx?JobSeekerID={jobSeekerID}");
                    break;

                case "InterviewCandidate":
                    // Update application status to 'Interviewed'
                    if (UpdateCandidateStatus(jobSeekerID, jobId, "Interviewed"))
                    {
                        lblSuccessMessage.Text = "Candidate status updated to Interviewed.";
                        // Refresh the GridView to reflect changes
                        LoadShortlistedCandidates(jobId);
                    }
                    else
                    {
                        lblError.Text = "An error occurred while updating the candidate's status.";
                    }
                    break;

                case "RejectCandidate":
                    // Update application status to 'Rejected'
                    if (UpdateCandidateStatus(jobSeekerID, jobId, "Rejected"))
                    {
                        lblSuccessMessage.Text = "Candidate has been rejected.";
                        // Refresh the GridView to reflect changes
                        LoadShortlistedCandidates(jobId);
                        // Optionally, redirect to RejectedCandidates.aspx or handle accordingly
                    }
                    else
                    {
                        lblError.Text = "An error occurred while rejecting the candidate.";
                    }
                    break;

                default:
                    break;
            }
        }

        /// <summary>
        /// Downloads the resume of the specified candidate.
        /// </summary>
        /// <param name="jobSeekerID">The JobSeekerID.</param>
        private void DownloadResume(int jobSeekerID)
        {
            string resumePath = GetResumePath(jobSeekerID);

            if (!string.IsNullOrEmpty(resumePath))
            {
                try
                {
                    // Ensure the file exists
                    string fullPath = Server.MapPath(resumePath);
                    if (System.IO.File.Exists(fullPath))
                    {
                        // Initiate the file download
                        Response.ContentType = "application/octet-stream";
                        Response.AppendHeader("Content-Disposition", "attachment; filename=" + System.IO.Path.GetFileName(resumePath));
                        Response.TransmitFile(fullPath);
                        Response.End();
                    }
                    else
                    {
                        lblError.Text = "Resume file does not exist.";
                    }
                }
                catch (Exception ex)
                {
                    lblError.Text = "An error occurred while downloading the resume: " + ex.Message;
                    // Log the exception as needed
                    System.Diagnostics.Debug.WriteLine($"Error downloading resume: {ex.Message}");
                }
            }
            else
            {
                lblError.Text = "Resume not available.";
            }
        }

        /// <summary>
        /// Retrieves the resume path of a candidate from the database.
        /// </summary>
        /// <param name="jobSeekerID">The JobSeekerID.</param>
        /// <returns>The resume path.</returns>
        private string GetResumePath(int jobSeekerID)
        {
            string resumePath = "";
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = @"SELECT ResumePath FROM JobSeeker WHERE JobSeekerID = @JobSeekerID";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@JobSeekerID", jobSeekerID);
                    object result = cmd.ExecuteScalar();
                    resumePath = result as string;
                }
            }
            return resumePath;
        }

        /// <summary>
        /// Updates the application status of a candidate.
        /// </summary>
        /// <param name="jobSeekerID">The JobSeekerID.</param>
        /// <param name="jobId">The JobID.</param>
        /// <param name="newStatus">The new status to set.</param>
        /// <returns>True if update is successful; otherwise, false.</returns>
        private bool UpdateCandidateStatus(int jobSeekerID, int jobId, string newStatus)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = @"UPDATE JobApplication 
                                 SET Status = @Status 
                                 WHERE JobID = @JobID AND JobSeekerID = @JobSeekerID";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Status", newStatus);
                    cmd.Parameters.AddWithValue("@JobID", jobId);
                    cmd.Parameters.AddWithValue("@JobSeekerID", jobSeekerID);

                    try
                    {
                        conn.Open();
                        int rowsAffected = cmd.ExecuteNonQuery();
                        return rowsAffected > 0;
                    }
                    catch (Exception ex)
                    {
                        // Log the exception
                        System.Diagnostics.Debug.WriteLine($"Error updating candidate status: {ex.Message}");
                        lblError.Text = "An error occurred while updating the candidate's status: " + ex.Message;
                        return false;
                    }
                }
            }
        }

        /// <summary>
        /// Loads job seekers into the dropdown for emailing.
        /// </summary>
        private void LoadJobSeekers()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "SELECT JobSeekerID, FirstName + ' ' + LastName AS FullName FROM JobSeeker ORDER BY FullName";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    try
                    {
                        conn.Open();
                        SqlDataReader reader = cmd.ExecuteReader();
                        ddlJobSeekers.Items.Clear();
                        ddlJobSeekers.Items.Add(new ListItem("-- Select Job Seeker --", ""));
                        while (reader.Read())
                        {
                            ddlJobSeekers.Items.Add(new ListItem(reader["FullName"].ToString(), reader["JobSeekerID"].ToString()));
                        }
                        reader.Close();
                    }
                    catch (Exception ex)
                    {
                        // Log error
                        lblMessage.Text = "An error occurred while loading job seekers.";
                        lblMessage.CssClass = "mt-2 text-danger";
                        System.Diagnostics.Debug.WriteLine($"Error loading job seekers: {ex.Message}");
                    }
                }
            }
        }

        /// <summary>
        /// Handles the Send Email button click event to email all shortlisted candidates.
        /// </summary>
        protected void btnSendEmail_Click(object sender, EventArgs e)
        {
            lblEmailStatus.Text = "";
            string emailSubject = txtEmailSubject.Text.Trim();
            string emailBody = txtEmailContent.Text.Trim();

            // Validate input
            if (string.IsNullOrEmpty(emailSubject) || string.IsNullOrEmpty(emailBody))
            {
                lblEmailStatus.Text = "Please enter both subject and message.";
                lblEmailStatus.CssClass = "text-danger";
                return;
            }

            int jobId = GetCurrentJobId();

            // Retrieve emails of all shortlisted candidates
            List<string> candidateEmails = GetShortlistedCandidateEmails(jobId);

            if (candidateEmails.Count == 0)
            {
                lblEmailStatus.Text = "No candidates to email.";
                lblEmailStatus.CssClass = "text-warning";
                return;
            }

            try
            {
                // Send email to each candidate
                foreach (string email in candidateEmails)
                {
                    SendEmail(email, emailSubject, emailBody);
                }

                lblEmailStatus.Text = "Email sent successfully to all candidates.";
                lblEmailStatus.CssClass = "text-success";
                txtEmailSubject.Text = "";
                txtEmailContent.Text = "";
            }
            catch (Exception ex)
            {
                lblEmailStatus.Text = "An error occurred while sending emails: " + ex.Message;
                lblEmailStatus.CssClass = "text-danger";
                // Log error
                System.Diagnostics.Debug.WriteLine($"Error sending emails: {ex.Message}");
            }
        }

        /// <summary>
        /// Retrieves the email addresses of all shortlisted candidates for the specified job.
        /// </summary>
        /// <param name="jobId">The JobID.</param>
        /// <returns>A list of email addresses.</returns>
        private List<string> GetShortlistedCandidateEmails(int jobId)
        {
            List<string> emails = new List<string>();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = @"SELECT U.Email
                                 FROM [User] U
                                 INNER JOIN JobSeeker JS ON U.UserID = JS.UserID
                                 INNER JOIN JobApplication JA ON JS.JobSeekerID = JA.JobSeekerID
                                 WHERE JA.JobID = @JobID AND JA.Status = 'Shortlisted'";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@JobID", jobId);
                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string email = reader["Email"].ToString();
                            if (!string.IsNullOrEmpty(email))
                            {
                                emails.Add(email);
                            }
                        }
                    }
                }
            }

            return emails;
        }

        /// <summary>
        /// Sends an email to the specified recipient.
        /// </summary>
        /// <param name="toEmail">The recipient's email address.</param>
        /// <param name="subject">The email subject.</param>
        /// <param name="body">The email body.</param>
        private void SendEmail(string toEmail, string subject, string body)
        {
            // Configure your SMTP client here
            // Replace the placeholders with your actual SMTP server details and credentials
            MailMessage mail = new MailMessage();
            mail.To.Add(toEmail);
            mail.Subject = subject;
            mail.Body = body;
            mail.IsBodyHtml = false;

            // Set the sender's email address and credentials
            mail.From = new MailAddress("youremail@example.com"); // Replace with your email

            SmtpClient smtp = new SmtpClient("smtp.example.com"); // Replace with your SMTP server
            smtp.Port = 587; // Replace with your SMTP port (e.g., 587 for TLS)
            smtp.Credentials = new System.Net.NetworkCredential("youremail@example.com", "yourpassword"); // Replace with your credentials
            smtp.EnableSsl = true; // Enable SSL if required

            smtp.Send(mail);
        }

        /// <summary>
        /// Handles the Back button click event to navigate back to ManageJobsActive.aspx.
        /// </summary>
        protected void btnBack_Click(object sender, EventArgs e)
        {
            // Redirect back to ManageJobsActive.aspx
            Response.Redirect("ManageJobsActive.aspx");
        }

        // Helper Classes
        /// <summary>
        /// Represents the job requirements.
        /// </summary>
        public class JobRequirements
        {
            public List<int> RequiredSkills { get; set; }
            public List<int> RequiredDegrees { get; set; }
            public int RequiredExperience { get; set; }
        }

        /// <summary>
        /// Represents a candidate.
        /// </summary>
        public class Candidate
        {
            public int JobSeekerID { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public int YearsOfExperience { get; set; }
            public string ResumePath { get; set; }
            public List<int> SkillTagIDs { get; set; }
            public List<int> DegreeIDs { get; set; }
        }

        /// <summary>
        /// Represents a candidate along with their matching percentage and application status.
        /// </summary>
        public class MatchingCandidate
        {
            public Candidate Candidate { get; set; }
            public double MatchingPercentage { get; set; }
            public string ApplicationStatus { get; set; }
        }

        // User Profile and Logout Event Handlers
        protected void lnkViewProfile_Click(object sender, EventArgs e)
        {
            Response.Redirect("EmployerProfile.aspx");
        }

        protected void lnkInbox_Click(object sender, EventArgs e)
        {
            Response.Redirect("ChangeEmployerPassword.aspx");
        }

        protected void lnkLogout_Click(object sender, EventArgs e)
        {
            Session.Abandon();
            Response.Redirect("EmployerLogin.aspx");
        }

        /// <summary>
        /// Handles the RowDataBound event of the GridView to customize row appearance.
        /// </summary>
        protected void gvMatchingCandidates_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                // Retrieve the current data item
                var dataItem = (MatchingCandidate)e.Row.DataItem;

                // Example: Change row color based on application status
                if (dataItem != null)
                {
                    switch (dataItem.ApplicationStatus)
                    {
                        case "Interviewed":
                            e.Row.BackColor = System.Drawing.Color.LightYellow;
                            break;
                        case "Rejected":
                            e.Row.BackColor = System.Drawing.Color.LightCoral;
                            break;
                        case "Shortlisted":
                            e.Row.BackColor = System.Drawing.Color.LightGreen;
                            break;
                        default:
                            break;
                    }
                }
            }
        }
        

        /// <summary>
        /// Loads chat messages for the employer.
        /// </summary>
        private void LoadChatMessages()
        {
            if (!int.TryParse(Session["EmployerID"].ToString(), out int employerID))
            {
                lblMessage.Text = "Invalid session data. Please log in again.";
                Response.Redirect("EmployerLogin.aspx");
                return;
            }

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = @"
                    SELECT CM.Message, CM.MessageTime, JS.FirstName, JS.LastName, JS.ProfilePicturePath
                    FROM ChatMessages CM
                    INNER JOIN JobSeeker JS ON CM.JobSeekerID = JS.JobSeekerID
                    WHERE CM.EmployerID = @EmployerID
                    ORDER BY CM.MessageTime DESC";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@EmployerID", employerID);

                    try
                    {
                        conn.Open();
                        SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                        DataTable messagesTable = new DataTable();
                        adapter.Fill(messagesTable);

                        rptChatMessages.DataSource = messagesTable;
                        rptChatMessages.DataBind();
                    }
                    catch (SqlException sqlEx)
                    {
                        System.Diagnostics.Trace.TraceError($"SQL Error in LoadChatMessages: {sqlEx.Message}");
                        lblMessage.Text = "A database error occurred while loading messages.";
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Trace.TraceError($"Error in LoadChatMessages: {ex.Message}");
                        lblMessage.Text = "An unexpected error occurred while loading messages.";
                    }
                }
            }
        }
        protected void btnSendMessage_Click(object sender, EventArgs e)
        {
            if (!int.TryParse(Session["EmployerID"].ToString(), out int employerID))
            {
                lblMessage.Text = "Invalid session data. Please log in again.";
                Response.Redirect("EmployerLogin.aspx");
                return;
            }

            if (!int.TryParse(ddlJobSeekers.SelectedValue, out int jobSeekerID) || jobSeekerID == 0)
            {
                lblMessage.Text = "Please select a job seeker to send a message.";
                return;
            }

            string messageContent = txtChatMessage.Text.Trim();

            if (string.IsNullOrEmpty(messageContent))
            {
                lblMessage.Text = "Please enter a message.";
                return;
            }

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = @"
                    INSERT INTO ChatMessages (JobSeekerID, EmployerID, Message, MessageTime)
                    VALUES (@JobSeekerID, @EmployerID, @Message, @MessageTime)";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@JobSeekerID", jobSeekerID);
                    cmd.Parameters.AddWithValue("@EmployerID", employerID);
                    cmd.Parameters.AddWithValue("@Message", messageContent);
                    cmd.Parameters.AddWithValue("@MessageTime", DateTime.Now);

                    try
                    {
                        conn.Open();
                        cmd.ExecuteNonQuery();

                        txtChatMessage.Text = "";
                        lblMessage.Text = "Message sent successfully.";
                        LoadChatMessages();
                    }
                    catch (SqlException sqlEx)
                    {
                        System.Diagnostics.Trace.TraceError($"SQL Error in btnSendMessage_Click: {sqlEx.Message}");
                        lblMessage.Text = "A database error occurred while sending your message.";
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Trace.TraceError($"Error in btnSendMessage_Click: {ex.Message}");
                        lblMessage.Text = "An unexpected error occurred while sending your message.";
                    }
                }
            }
        }
    }
}
