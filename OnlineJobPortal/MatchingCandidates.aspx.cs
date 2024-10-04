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
            int jobId;
            if (!IsPostBack)
            {
                if (Request.QueryString["JobID"] != null && int.TryParse(Request.QueryString["JobID"], out jobId))
                {
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

        private void LoadMatchingCandidates(int jobId)
        {
            // List to hold matching candidates
            List<MatchingCandidate> matchingCandidates = new List<MatchingCandidate>();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                // Step 1: Get Job requirements
                JobRequirements jobRequirements = GetJobRequirements(conn, jobId);

                // Step 2: Get all candidates
                List<Candidate> candidates = GetAllCandidates(conn);

                // Step 3: For each candidate, compute matching percentage
                foreach (var candidate in candidates)
                {
                    double matchingPercentage = ComputeMatchingPercentage(jobRequirements, candidate);

                    if (matchingPercentage >= 40)
                    {
                        matchingCandidates.Add(new MatchingCandidate
                        {
                            Candidate = candidate,
                            MatchingPercentage = matchingPercentage
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
                jobReq.RequiredExperience = Convert.ToInt32(cmd.ExecuteScalar());
            }

            return jobReq;
        }

        private List<Candidate> GetAllCandidates(SqlConnection conn)
        {
            List<Candidate> candidates = new List<Candidate>();

            // Get JobSeekerID and YearsOfExperience from JobSeeker
            string queryCandidates = @"SELECT JobSeekerID, FirstName, LastName, YearsOfExperience, ResumePath FROM JobSeeker";
            using (SqlCommand cmd = new SqlCommand(queryCandidates, conn))
            {
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Candidate candidate = new Candidate();
                        candidate.JobSeekerID = reader.GetInt32(reader.GetOrdinal("JobSeekerID"));
                        candidate.FirstName = reader.GetString(reader.GetOrdinal("FirstName"));
                        candidate.LastName = reader.GetString(reader.GetOrdinal("LastName"));
                        candidate.YearsOfExperience = reader.IsDBNull(reader.GetOrdinal("YearsOfExperience")) ? 0 : reader.GetInt32(reader.GetOrdinal("YearsOfExperience"));
                        candidate.ResumePath = reader.IsDBNull(reader.GetOrdinal("ResumePath")) ? "" : reader.GetString(reader.GetOrdinal("ResumePath"));

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
                    // Start file download
                    Response.ContentType = "application/octet-stream";
                    Response.AppendHeader("Content-Disposition", "attachment; filename=" + System.IO.Path.GetFileName(resumePath));
                    Response.TransmitFile(Server.MapPath(resumePath));
                    Response.End();
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

                // Send email to candidate
                SendEmailToCandidate(jobSeekerID);

                // Provide feedback
                lblSuccessMessage.Text = "Email sent to candidate.";
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
                    resumePath = cmd.ExecuteScalar() as string;
                }
            }
            return resumePath;
        }

        private void SendEmailToCandidate(int jobSeekerID)
        {
            string candidateEmail = GetCandidateEmail(jobSeekerID);

            if (!string.IsNullOrEmpty(candidateEmail))
            {
                try
                {
                    System.Net.Mail.MailMessage mail = new System.Net.Mail.MailMessage();
                    mail.To.Add(candidateEmail);
                    mail.From = new System.Net.Mail.MailAddress("yourcompany@example.com");
                    mail.Subject = "Job Selection";
                    mail.Body = "Congratulations! You have been selected for the job. Please contact us for further details.";

                    System.Net.Mail.SmtpClient smtp = new System.Net.Mail.SmtpClient();
                    smtp.Host = "smtp.example.com"; // Your SMTP server
                    smtp.Port = 25; // Your SMTP port
                    smtp.UseDefaultCredentials = false;
                    smtp.Credentials = new System.Net.NetworkCredential("username", "password"); // Your SMTP credentials
                    smtp.EnableSsl = true;

                    smtp.Send(mail);

                    lblMessage.Text = "Email sent successfully to " + candidateEmail;
                }
                catch (Exception ex)
                {
                    lblError.Text = "Error sending email: " + ex.Message;
                }
            }
            else
            {
                lblError.Text = "Candidate email not found.";
            }
        }

        private string GetCandidateEmail(int jobSeekerID)
        {
            string email = "";
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = @"SELECT u.Email FROM [User] u
                                 INNER JOIN JobSeeker js ON u.UserID = js.UserID
                                 WHERE js.JobSeekerID = @JobSeekerID";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@JobSeekerID", jobSeekerID);
                    email = cmd.ExecuteScalar() as string;
                }
            }
            return email;
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
                        lblMessage.Text = "An error occurred while sending the message.";
                        lblMessage.CssClass = "mt-2 text-danger";
                        // Log error
                    }
                }
            }
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
        }

        protected void lnkViewProfile_Click(object sender, EventArgs e)
        {
            Response.Redirect("EmployerProfile.aspx");
        }

        protected void lnkInbox_Click(object sender, EventArgs e)
        {
            Response.Redirect("EmployerInbox.aspx");
        }

        protected void lnkLogout_Click(object sender, EventArgs e)
        {
            Session.Abandon();
            Response.Redirect("EmployerLogin.aspx");
        }
    }
}
