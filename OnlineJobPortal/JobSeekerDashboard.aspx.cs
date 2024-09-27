using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace OnlineJobPortal
{
    public partial class JobSeekerDashboard : Page
    {
        // Retrieve the connection string from Web.config
        private readonly string connectionString = ConfigurationManager.ConnectionStrings["OnlineJobPortalDB"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Existing checks for authentication
                if (Session["JobSeekerID"] == null)
                {
                    Response.Redirect("JobSeekerLogin.aspx");
                }
                else
                {
                    LoadJobSeekerDetails();
                    LoadDashboardStatistics();
                    LoadJobPostings(); // Load job postings here
                }
            }
        }



        /// <summary>
        /// Loads job seeker details such as name.
        /// </summary>
        private void LoadJobSeekerDetails()
        {
            int jobSeekerID = Convert.ToInt32(Session["JobSeekerID"]);

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "SELECT FirstName, LastName FROM JobSeeker WHERE JobSeekerID = @JobSeekerID";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@JobSeekerID", jobSeekerID);

                    try
                    {
                        conn.Open();
                        SqlDataReader reader = cmd.ExecuteReader();
                        if (reader.Read())
                        {
                            lblJobSeekerName.Text = $"{reader["FirstName"]} {reader["LastName"]}";
                            // Optionally, load profile picture if stored in the database
                        }
                        reader.Close();
                    }
                    catch (Exception ex)
                    {
                        // Log the exception (implement logging as per your project standards)
                        lblMessage.Text = "An error occurred while loading your details.";
                    }
                }
            }
        }

        /// <summary>
        /// Loads various statistics for the dashboard.
        /// </summary>
        private void LoadDashboardStatistics()
        {
            int jobSeekerID = Convert.ToInt32(Session["JobSeekerID"]);

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();

                    // 1. Total Applications
                    string totalApplicationsQuery = "SELECT COUNT(*) FROM JobApplication WHERE JobSeekerID = @JobSeekerID";
                    using (SqlCommand cmd = new SqlCommand(totalApplicationsQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@JobSeekerID", jobSeekerID);
                        int totalApplications = (int)cmd.ExecuteScalar();
                        lblTotalApplications.Text = totalApplications.ToString();
                    }

                    // 2. Active Jobs (Total Active Job Postings)
                    string activeJobsQuery = "SELECT COUNT(*) FROM JobPosting WHERE Status = 'Active'";
                    using (SqlCommand cmd = new SqlCommand(activeJobsQuery, conn))
                    {
                        int activeJobs = (int)cmd.ExecuteScalar();
                        lblActiveJobs.Text = activeJobs.ToString();
                    }

                    // 3. Pending Applications (Applications with Status 'Applied' or 'Interviewed')
                    string pendingApplicationsQuery = "SELECT COUNT(*) FROM JobApplication WHERE JobSeekerID = @JobSeekerID AND Status IN ('Applied', 'Interviewed')";
                    using (SqlCommand cmd = new SqlCommand(pendingApplicationsQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@JobSeekerID", jobSeekerID);
                        int pendingApplications = (int)cmd.ExecuteScalar();
                        lblPendingApplications.Text = pendingApplications.ToString();
                    }

                    // 4. Messages (Total Messages Received)
                    string messagesQuery = "SELECT COUNT(*) FROM ChatMessages WHERE JobSeekerID = @JobSeekerID";
                    using (SqlCommand cmd = new SqlCommand(messagesQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@JobSeekerID", jobSeekerID);
                        int totalMessages = (int)cmd.ExecuteScalar();
                        lblMessages.Text = totalMessages.ToString();
                    }

                    // Add more statistics as needed
                }
                catch (Exception ex)
                {
                    // Log the exception (implement logging as per your project standards)
                    lblMessage.Text = "An error occurred while loading dashboard statistics.";
                }
            }
        }

        /// <summary>
        /// Handles the click event for viewing the job seeker's profile.
        /// </summary>
        protected void lnkViewProfile_Click(object sender, EventArgs e)
        {
            Response.Redirect("JobSeekerProfile.aspx");
        }

        /// <summary>
        /// Handles the click event for accessing the inbox.
        /// </summary>
        protected void lnkInbox_Click(object sender, EventArgs e)
        {
            Response.Redirect("JobSeekerInbox.aspx");
        }

        /// <summary>
        /// Handles the click event for logging out the job seeker.
        /// </summary>
        protected void lnkLogout_Click(object sender, EventArgs e)
        {
            Session.Abandon();
            Response.Redirect("JobSeekerLogin.aspx");
        }

        /// <summary>
        /// Handles the click event for sending chat messages.
        /// </summary>

        private void LoadJobPostings()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "SELECT JobID, Title FROM JobPosting WHERE Status = 'Active'";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();

                    ddlJobPostings.DataSource = reader; // Change this to your dropdown ID
                    ddlJobPostings.DataTextField = "Title"; // Display job titles
                    ddlJobPostings.DataValueField = "JobID"; // Store job IDs
                    ddlJobPostings.DataBind();

                    ddlJobPostings.Items.Insert(0, new ListItem("-- Select Job Posting --", ""));
                }
            }
        }

        private int GetEmployerIDFromJobPosting(int jobPostingID)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "SELECT EmployerID FROM JobPosting WHERE JobID = @JobID";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@JobID", jobPostingID);
                    conn.Open();
                    return (int)cmd.ExecuteScalar();
                }
            }
        }


        private int GetSelectedJobPostingID()
        {
            if (!string.IsNullOrEmpty(ddlJobPostings.SelectedValue))
            {
                return Convert.ToInt32(ddlJobPostings.SelectedValue);
            }
            return 0;
        }

        private void LoadChatMessages()
        {
            int jobSeekerID = Convert.ToInt32(Session["JobSeekerID"]);

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = @"
            SELECT CM.Message, CM.MessageTime, E.CompanyName
            FROM ChatMessages CM
            INNER JOIN Employer E ON CM.EmployerID = E.EmployerID
            WHERE CM.JobSeekerID = @JobSeekerID
            ORDER BY CM.MessageTime DESC";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@JobSeekerID", jobSeekerID);

                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    DataTable messagesTable = new DataTable();
                    adapter.Fill(messagesTable);

                    rptChatMessages.DataSource = messagesTable;
                    rptChatMessages.DataBind();
                }
            }
        }

        protected void btnSendMessage_Click(object sender, EventArgs e)
        {
            int jobSeekerID = Convert.ToInt32(Session["JobSeekerID"]);
            int jobPostingID = GetSelectedJobPostingID(); // Update this method to fetch the JobID
            string messageContent = txtChatMessage.Text.Trim();

            if (jobPostingID == 0)
            {
                lblMessage.Text = "Please select a job posting.";
                return;
            }

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
                    cmd.Parameters.AddWithValue("@EmployerID", GetEmployerIDFromJobPosting(jobPostingID)); // Implement this method
                    cmd.Parameters.AddWithValue("@Message", messageContent);
                    cmd.Parameters.AddWithValue("@MessageTime", DateTime.Now);

                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }

            txtChatMessage.Text = "";
            lblMessage.Text = "Message sent successfully.";
            LoadChatMessages();


        }
    }
}