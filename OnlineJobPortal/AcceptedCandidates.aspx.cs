using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Net;
using System.Net.Mail;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace OnlineJobPortal
{
    public partial class AcceptedCandidates : Page
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

                    // Load accepted candidates
                    LoadAcceptedCandidates(jobId);
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

        private void LoadAcceptedCandidates(int jobId)
        {
            List<MatchingCandidate> acceptedCandidates = new List<MatchingCandidate>();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = @"SELECT JS.JobSeekerID, JS.FirstName, JS.LastName, JS.YearsOfExperience, JS.ResumePath, JA.Status
                                 FROM JobSeeker JS
                                 INNER JOIN JobApplication JA ON JS.JobSeekerID = JA.JobSeekerID
                                 WHERE JA.JobID = @JobID AND JA.Status = 'Accepted'";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@JobID", jobId);
                    try
                    {
                        conn.Open();
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
                                    ResumePath = reader.IsDBNull(reader.GetOrdinal("ResumePath")) ? "" : reader["ResumePath"].ToString()
                                };

                                string status = reader["Status"].ToString();

                                acceptedCandidates.Add(new MatchingCandidate
                                {
                                    Candidate = candidate,
                                    MatchingPercentage = CalculateMatchingPercentage(jobId, candidate.JobSeekerID),
                                    ApplicationStatus = status
                                });
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        lblError.Text = "An error occurred while loading candidates: " + ex.Message;
                        return;
                    }
                }
            }

            gvAcceptedCandidates.DataSource = acceptedCandidates;
            gvAcceptedCandidates.DataBind();
        }

        private double CalculateMatchingPercentage(int jobId, int jobSeekerID)
        {
            // Implement your matching percentage calculation logic here
            // For demonstration, we'll return a random value between 40 and 100
            Random rnd = new Random();
            return rnd.Next(40, 101);
        }

        protected void gvAcceptedCandidates_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            lblError.Text = "";
            lblSuccessMessage.Text = "";

            if (e.CommandArgument == null)
            {
                lblError.Text = "Invalid command argument.";
                return;
            }

            if (!int.TryParse(e.CommandArgument.ToString(), out int jobSeekerID))
            {
                lblError.Text = "Invalid Job Seeker ID.";
                return;
            }

            int jobId = GetCurrentJobId();

            switch (e.CommandName)
            {
                case "DownloadResume":
                    DownloadResume(jobSeekerID);
                    break;
                case "ViewProfile":
                    ViewProfile(jobSeekerID);
                    break;
                default:
                    lblError.Text = "Unknown command.";
                    break;
            }
        }

        private void DownloadResume(int jobSeekerID)
        {
            string resumePath = GetResumePath(jobSeekerID);

            if (!string.IsNullOrEmpty(resumePath))
            {
                try
                {
                    string fullPath = Server.MapPath(resumePath);
                    if (System.IO.File.Exists(fullPath))
                    {
                        Response.Clear();
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
                }
            }
            else
            {
                lblError.Text = "Resume not available.";
            }
        }

        private string GetResumePath(int jobSeekerID)
        {
            string resumePath = "";
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = @"SELECT ResumePath FROM JobSeeker WHERE JobSeekerID = @JobSeekerID";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@JobSeekerID", jobSeekerID);
                    try
                    {
                        conn.Open();
                        object result = cmd.ExecuteScalar();
                        resumePath = result as string;
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine($"Error retrieving resume path: {ex.Message}");
                    }
                }
            }
            return resumePath;
        }

        private void ViewProfile(int jobSeekerID)
        {
            Response.Redirect("FullProfile.aspx?JobSeekerID=" + jobSeekerID);
        }

        /// <summary>
        /// Handles the Send Email button click event.
        /// </summary>
        protected void btnSendEmail_Click(object sender, EventArgs e)
        {
            lblEmailStatus.Text = "";
            lblEmailStatus.CssClass = "";

            string emailSubject = txtEmailSubject.Text.Trim();
            string emailBody = txtEmailContent.Text.Trim();

            if (string.IsNullOrEmpty(emailSubject) || string.IsNullOrEmpty(emailBody))
            {
                lblEmailStatus.Text = "Please enter both subject and message.";
                lblEmailStatus.CssClass = "text-danger";
                return;
            }

            int jobId = GetCurrentJobId();
            List<string> candidateEmails = GetCandidateEmails(jobId);

            if (candidateEmails.Count == 0)
            {
                lblEmailStatus.Text = "No candidates to email.";
                lblEmailStatus.CssClass = "text-warning";
                return;
            }

            try
            {
                // Configure SMTP client
                SmtpClient smtp = new SmtpClient
                {
                    Host = ConfigurationManager.AppSettings["SmtpHost"], // e.g., "smtp.gmail.com"
                    Port = int.Parse(ConfigurationManager.AppSettings["SmtpPort"]), // e.g., 587
                    EnableSsl = bool.Parse(ConfigurationManager.AppSettings["EnableSsl"]), // e.g., true
                    Credentials = new NetworkCredential(
                        ConfigurationManager.AppSettings["EmailUsername"], // Your email
                        ConfigurationManager.AppSettings["EmailPassword"]) // Your password
                };

                MailMessage mail = new MailMessage
                {
                    From = new MailAddress(ConfigurationManager.AppSettings["EmailUsername"], "Your Company Name"),
                    Subject = emailSubject,
                    Body = emailBody,
                    IsBodyHtml = false
                };

                // Add recipients to BCC to protect privacy
                foreach (string email in candidateEmails)
                {
                    if (!string.IsNullOrEmpty(email))
                    {
                        mail.Bcc.Add(email);
                    }
                }

                // Optionally, add a visible recipient (like yourself) or omit the To field
                mail.To.Add(ConfigurationManager.AppSettings["EmailUsername"]);

                smtp.Send(mail);

                lblEmailStatus.Text = "Emails sent successfully to all candidates.";
                lblEmailStatus.CssClass = "text-success";

                // Clear the email fields
                txtEmailSubject.Text = "";
                txtEmailContent.Text = "";
            }
            catch (Exception ex)
            {
                lblEmailStatus.Text = "An error occurred while sending emails: " + ex.Message;
                lblEmailStatus.CssClass = "text-danger";
                // Log the exception
                System.Diagnostics.Debug.WriteLine($"Error sending emails: {ex.Message}");
            }
        }


        private List<string> GetCandidateEmails(int jobId)
        {
            List<string> emails = new List<string>();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = @"SELECT U.Email
                                 FROM [User] U
                                 INNER JOIN JobSeeker JS ON U.UserID = JS.UserID
                                 INNER JOIN JobApplication JA ON JS.JobSeekerID = JA.JobSeekerID
                                 WHERE JA.JobID = @JobID AND JA.Status = 'Accepted'";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@JobID", jobId);
                    try
                    {
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
                    catch (Exception ex)
                    {
                        lblEmailStatus.Text = "An error occurred while retrieving candidate emails: " + ex.Message;
                        lblEmailStatus.CssClass = "text-danger";
                        System.Diagnostics.Debug.WriteLine($"Error retrieving emails: {ex.Message}");
                    }
                }
            }

            return emails;
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

        protected void gvAcceptedCandidates_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                // Retrieve the current data item
                var dataItem = (MatchingCandidate)e.Row.DataItem;

                // Find the Buttons
                Button btnDownloadResume = (Button)e.Row.FindControl("btnDownloadResume");
                Button btnViewProfile = (Button)e.Row.FindControl("btnViewProfile");

                if (dataItem != null)
                {
                    // Optionally, disable buttons based on status or other criteria
                    // For example, if resume is not available, disable the button
                    if (string.IsNullOrEmpty(dataItem.Candidate.ResumePath))
                    {
                        btnDownloadResume.Enabled = false;
                        btnDownloadResume.Text = "No Resume";
                        btnDownloadResume.CssClass = "btn btn-secondary btn-sm disabled";
                    }
                }
            }
        }
        private void LoadJobSeekers()
        {
            if (Session["EmployerID"] == null)
            {
                lblMessage.Text = "User session expired. Please log in again.";
                Response.Redirect("EmployerLogin.aspx");
                return;
            }

            // Safely parse EmployerID from session
            if (!int.TryParse(Session["EmployerID"].ToString(), out int currentEmployerID))
            {
                lblMessage.Text = "Invalid session data. Please log in again.";
                Response.Redirect("EmployerLogin.aspx");
                return;
            }

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "SELECT JobSeekerID, FirstName, LastName FROM JobSeeker ORDER BY FirstName, LastName";
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
                            string jobSeekerName = $"{reader["FirstName"]} {reader["LastName"]}";
                            ddlJobSeekers.Items.Add(new ListItem(jobSeekerName, reader["JobSeekerID"].ToString()));
                        }
                        reader.Close();
                    }
                    catch (SqlException sqlEx)
                    {
                        System.Diagnostics.Trace.TraceError($"SQL Error in LoadJobSeekers: {sqlEx.Message}");
                        lblMessage.Text = "A database error occurred while loading job seekers.";
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Trace.TraceError($"Error in LoadJobSeekers: {ex.Message}");
                        lblMessage.Text = "An unexpected error occurred while loading job seekers.";
                    }
                }
            }
        }
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
