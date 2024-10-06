using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace OnlineJobPortal
{
    public partial class MatchingCandidates : Page
    {
        // Retrieve the connection string from Web.config
        private readonly string connectionString = ConfigurationManager.ConnectionStrings["OnlineJobPortalDB"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Request.QueryString["JobID"] != null && int.TryParse(Request.QueryString["JobID"], out int jobId))
                {
                    // Store JobID in ViewState for later use
                    ViewState["JobID"] = jobId;

                    // Load job details
                    LoadJobDetails(jobId);

                    // Load matching candidates
                    LoadMatchingCandidates(jobId);

                    // Load job seekers into the dropdown
                    LoadJobSeekers();
                }
                else
                {
                    // Handle error - JobID not provided or invalid
                    lblError.Text = "Invalid Job ID.";
                }
            }
        }

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
                        // Log the exception
                        // For demonstration, display the error message
                        lblError.Text = "An error occurred while loading job details: " + ex.Message;
                    }
                }
            }
        }

        private int GetCurrentJobId()
        {
            if (ViewState["JobID"] != null && int.TryParse(ViewState["JobID"].ToString(), out int jobId))
            {
                return jobId;
            }
            throw new InvalidOperationException("Job ID is not available.");
        }

        private void LoadMatchingCandidates(int jobId)
        {
            // Existing implementation remains unchanged
            // ...
            List<MatchingCandidate> matchingCandidates = new List<MatchingCandidate>();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                // Step 1: Get Job requirements
                JobRequirements jobRequirements = GetJobRequirements(conn, jobId);

                // Step 2: Get all candidates
                List<Candidate> candidates = GetAllCandidates(conn, jobId); // Pass jobId to filter applications

                // Step 3: For each candidate, compute matching percentage
                foreach (var candidate in candidates)
                {
                    double matchingPercentage = ComputeMatchingPercentage(jobRequirements, candidate);

                    if (matchingPercentage >0)
                    {
                        // Get application status
                        string status = GetApplicationStatus(conn, jobId, candidate.JobSeekerID);

                        matchingCandidates.Add(new MatchingCandidate
                        {
                            Candidate = candidate,
                            MatchingPercentage = matchingPercentage,
                            ApplicationStatus = status
                        });
                    }
                }

                // Step 4: Order candidates by matching percentage descending
                matchingCandidates.Sort((x, y) => y.MatchingPercentage.CompareTo(x.MatchingPercentage));

                // Step 5: Bind to GridView
                gvMatchingCandidates.DataSource = matchingCandidates;
                gvMatchingCandidates.DataBind();
            }
        }

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
                // Log the exception (e.g., to a file, event log, etc.)
                // For demonstration, we'll set the status to "Error" and log to Debug
                System.Diagnostics.Debug.WriteLine($"Error retrieving application status: {ex.Message}");
                return "Error";
            }
        }

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

        private List<Candidate> GetAllCandidates(SqlConnection conn, int jobId)
        {
            List<Candidate> candidates = new List<Candidate>();

            // Get JobSeekerID and YearsOfExperience from JobSeeker
            string queryCandidates = @"SELECT JS.JobSeekerID, JS.FirstName, JS.LastName, JS.YearsOfExperience, JS.ResumePath
                                       FROM JobSeeker JS
                                       INNER JOIN JobApplication JA ON JS.JobSeekerID = JA.JobSeekerID
                                       WHERE JA.JobID = @JobID AND JA.Status != 'Rejected'"; // Optionally exclude rejected applications
            using (SqlCommand cmd = new SqlCommand(queryCandidates, conn))
            {
                cmd.Parameters.AddWithValue("@JobID", jobId);
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Candidate candidate = new Candidate();
                        candidate.JobSeekerID = reader.GetInt32(reader.GetOrdinal("JobSeekerID"));
                        candidate.FirstName = reader["FirstName"].ToString();
                        candidate.LastName = reader["LastName"].ToString();
                        candidate.YearsOfExperience = reader.IsDBNull(reader.GetOrdinal("YearsOfExperience")) ? 0 : reader.GetInt32(reader.GetOrdinal("YearsOfExperience"));
                        candidate.ResumePath = reader.IsDBNull(reader.GetOrdinal("ResumePath")) ? "" : reader["ResumePath"].ToString();

                        candidates.Add(candidate);
                    }
                }
            }

            // For each candidate, get their skills and degrees
            foreach (var candidate in candidates)
            {
                // Get skills
                candidate.SkillTagIDs = new List<int>();
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
                candidate.DegreeIDs = new List<int>();
                string queryDegrees = @"SELECT DegreeID
                                        FROM AcademicDetails
                                        WHERE JobSeekerID = @JobSeekerID";
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

        private double ComputeMatchingPercentage(JobRequirements jobReq, Candidate candidate)
        {
            // Weights
            double skillsWeight = 0.5;
            double degreesWeight = 0.3;
            double experienceWeight = 0.2;

            double skillsMatchPercentage = 0;
            double degreesMatchPercentage = 0;
            double experienceMatchPercentage = 0;

            // Skills Match
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

            // Degrees Match
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

            // Experience Match
            if (jobReq.RequiredExperience > 0)
            {
                double experienceRatio = ((double)candidate.YearsOfExperience / jobReq.RequiredExperience) * 100;
                if (experienceRatio > 100) experienceRatio = 100;
                experienceMatchPercentage = experienceRatio;
            }
            else
            {
                experienceMatchPercentage = 100; // No experience required
            }

            // Compute overall matching percentage
            double matchingPercentage = (skillsMatchPercentage * skillsWeight) + (degreesMatchPercentage * degreesWeight) + (experienceMatchPercentage * experienceWeight);

            return matchingPercentage;
        }

        protected void gvMatchingCandidates_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            lblError.Text = "";
            lblSuccessMessage.Text = "";

            if (e.CommandName == "DownloadResume")
            {
                int jobSeekerID = Convert.ToInt32(e.CommandArgument);

                // Get the resume path from database
                string resumePath = GetResumePath(jobSeekerID);

                if (!string.IsNullOrEmpty(resumePath))
                {
                    try
                    {
                        // Ensure the file exists
                        string fullPath = Server.MapPath(resumePath);
                        if (System.IO.File.Exists(fullPath))
                        {
                            // Start file download
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
                        // Log error
                    }
                }
                else
                {
                    lblError.Text = "Resume not available.";
                }
            }
            else if (e.CommandName == "ViewProfile")
            {
                int jobSeekerID = Convert.ToInt32(e.CommandArgument);
                // Redirect to FullProfile.aspx with JobSeekerID
                Response.Redirect("FullProfile.aspx?JobSeekerID=" + jobSeekerID);
            }
            else if (e.CommandName == "SelectCandidate")
            {
                int jobSeekerID = Convert.ToInt32(e.CommandArgument);
                int jobId = GetCurrentJobId(); // Retrieve JobID from ViewState

                // Update application status to 'Shortlisted'
                bool updateSuccess = UpdateCandidateStatus(jobSeekerID, jobId, "Shortlisted");

                if (updateSuccess)
                {
                    lblSuccessMessage.Text = "Candidate shortlisted successfully.";

                    // Rebind the GridView to reflect changes
                    LoadMatchingCandidates(jobId);
                }
                else
                {
                    lblError.Text = "An error occurred while shortlisting the candidate.";
                }
            }
        }

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
                        // Log the exception (not shown here for brevity)
                        System.Diagnostics.Debug.WriteLine($"Error updating candidate status: {ex.Message}");
                        lblError.Text = "An error occurred while shortlisting the candidate: " + ex.Message;
                        return false;
                    }
                }
            }
        }

        /// <summary>
        /// Loads job seekers into the dropdown for messaging.
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
                    }
                }
            }
        }

        /// <summary>
        /// Handles the Send Message button click event.
        /// </summary>
        protected void btnSendMessage_Click(object sender, EventArgs e)
        {
            lblMessage.Text = "";
            if (Session["EmployerID"] == null)
            {
                lblMessage.Text = "Session expired. Please log in again.";
                lblMessage.CssClass = "mt-2 text-danger";
                return;
            }

            if (!int.TryParse(Session["EmployerID"].ToString(), out int employerID))
            {
                lblMessage.Text = "Invalid session data.";
                lblMessage.CssClass = "mt-2 text-danger";
                return;
            }

            if (string.IsNullOrEmpty(ddlJobSeekers.SelectedValue) || string.IsNullOrEmpty(txtChatMessage.Text.Trim()))
            {
                lblMessage.Text = "Please select a job seeker and enter a message.";
                lblMessage.CssClass = "mt-2 text-danger";
                return;
            }

            if (!int.TryParse(ddlJobSeekers.SelectedValue, out int jobSeekerID))
            {
                lblMessage.Text = "Invalid job seeker selection.";
                lblMessage.CssClass = "mt-2 text-danger";
                return;
            }

            string messageContent = txtChatMessage.Text.Trim();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = @"
                    INSERT INTO ChatMessages 
                        (EmployerID, JobSeekerID, Message, MessageTime)
                    VALUES 
                        (@EmployerID, @JobSeekerID, @MessageContent, @MessageTime)";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@EmployerID", employerID);
                    cmd.Parameters.AddWithValue("@JobSeekerID", jobSeekerID);
                    cmd.Parameters.AddWithValue("@MessageContent", messageContent);
                    cmd.Parameters.AddWithValue("@MessageTime", DateTime.Now);

                    try
                    {
                        conn.Open();
                        cmd.ExecuteNonQuery();

                        lblMessage.Text = "Message sent successfully!";
                        lblMessage.CssClass = "mt-2 text-success";
                        txtChatMessage.Text = "";
                    }
                    catch (Exception ex)
                    {
                        lblMessage.Text = "An error occurred while sending the message: " + ex.Message;
                        lblMessage.CssClass = "mt-2 text-danger";
                        // Log error
                        System.Diagnostics.Debug.WriteLine($"Error sending message: {ex.Message}");
                    }
                }
            }
        }

        // Back Button Click Event Handler
        protected void btnBack_Click(object sender, EventArgs e)
        {
            // Redirect back to ManageJobsActive.aspx
            Response.Redirect("ManageJobsActive.aspx");
        }

        // Helper Classes
        public class JobRequirements
        {
            public List<int> RequiredSkills { get; set; }
            public List<int> RequiredDegrees { get; set; }
            public int RequiredExperience { get; set; }
        }

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

        public class MatchingCandidate
        {
            public Candidate Candidate { get; set; }
            public double MatchingPercentage { get; set; }
            public string ApplicationStatus { get; set; } // New Property
        }

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

        protected void gvMatchingCandidates_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                // Retrieve the current data item
                var dataItem = (MatchingCandidate)e.Row.DataItem;

                // Find the PlaceHolders
                PlaceHolder phSelect = (PlaceHolder)e.Row.FindControl("phSelect");
                PlaceHolder phSelected = (PlaceHolder)e.Row.FindControl("phSelected");

                if (dataItem != null)
                {
                    if (dataItem.ApplicationStatus == "Shortlisted")
                    {
                        phSelect.Visible = false;
                        phSelected.Visible = true;
                    }
                    else
                    {
                        phSelect.Visible = true;
                        phSelected.Visible = false;
                    }
                }
            }
        }
    }
}
