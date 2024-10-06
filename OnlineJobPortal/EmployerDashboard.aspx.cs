using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace OnlineJobPortal
{
    public partial class EmployerDashboard : Page
    {
        // Retrieve the connection string from Web.config
        private readonly string connectionString = ConfigurationManager.ConnectionStrings["OnlineJobPortalDB"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Authenticate Employer
                if (Session["EmployerID"] == null)
                {
                    Response.Redirect("EmployerLogin.aspx");
                }
                else
                {
                    LoadEmployerDetails();
                    LoadDashboardStatistics();
                    LoadJobSeekers(); // Load job seekers for the chat dropdown
                    LoadChatMessages(); // Load existing chat messages
                }
            }
        }

        /// <summary>
        /// Loads employer details such as company name.
        /// </summary>
        private void LoadEmployerDetails()
        {
            if (Session["EmployerID"] == null)
            {
                lblMessage.Text = "User session expired. Please log in again.";
                Response.Redirect("EmployerLogin.aspx");
                return;
            }

            // Safely parse EmployerID from session
            if (!int.TryParse(Session["EmployerID"].ToString(), out int employerID))
            {
                lblMessage.Text = "Invalid session data. Please log in again.";
                Response.Redirect("EmployerLogin.aspx");
                return;
            }

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "SELECT CompanyName FROM Employer WHERE EmployerID = @EmployerID";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@EmployerID", employerID);

                    try
                    {
                        conn.Open();
                        SqlDataReader reader = cmd.ExecuteReader();
                        if (reader.Read())
                        {
                            lblEmployerName.Text = reader["CompanyName"].ToString();
                        }
                        else
                        {
                            lblEmployerName.Text = "Company";
                        }
                        reader.Close();
                    }
                    catch (SqlException sqlEx)
                    {
                        // Log the exception details (implement logging as per your project standards)
                        System.Diagnostics.Trace.TraceError($"SQL Error in LoadEmployerDetails: {sqlEx.Message}");
                        lblMessage.Text = "A database error occurred while loading your details.";
                    }
                    catch (Exception ex)
                    {
                        // Log the exception details
                        System.Diagnostics.Trace.TraceError($"Error in LoadEmployerDetails: {ex.Message}");
                        lblMessage.Text = "An unexpected error occurred while loading your details.";
                    }
                }
            }
        }

        /// <summary>
        /// Loads various statistics for the dashboard.
        /// </summary>
        private void LoadDashboardStatistics()
        {
            if (!int.TryParse(Session["EmployerID"].ToString(), out int employerID))
            {
                lblMessage.Text = "Invalid session data. Please log in again.";
                Response.Redirect("EmployerLogin.aspx");
                return;
            }

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();

                    // 1. Total Job Postings
                    string totalJobsQuery = "SELECT COUNT(*) FROM JobPosting WHERE EmployerID = @EmployerID";
                    using (SqlCommand cmd = new SqlCommand(totalJobsQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@EmployerID", employerID);
                        int totalJobs = (int)cmd.ExecuteScalar();
                        lblTotalJobPostings.Text = totalJobs.ToString();
                    }

                    // 2. Active Jobs
                    string activeJobsQuery = "SELECT COUNT(*) FROM JobPosting WHERE EmployerID = @EmployerID AND Status = 'Active'";
                    using (SqlCommand cmd = new SqlCommand(activeJobsQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@EmployerID", employerID);
                        int activeJobs = (int)cmd.ExecuteScalar();
                        lblActiveJobs.Text = activeJobs.ToString();
                    }

                    // 3. Inactive Jobs
                    string inactiveJobsQuery = "SELECT COUNT(*) FROM JobPosting WHERE EmployerID = @EmployerID AND Status = 'Inactive'";
                    using (SqlCommand cmd = new SqlCommand(inactiveJobsQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@EmployerID", employerID);
                        int inactiveJobs = (int)cmd.ExecuteScalar();
                        lblInactiveJobs.Text = inactiveJobs.ToString();
                    }

                    // 4.Total Candidates Selected
                    string messagesQuery = "SELECT COUNT(*) AS TotalCandidatesSelected\r\nFROM JobPosting JP\r\nINNER JOIN Application A ON JP.JobPostingID = A.JobPostingID\r\nWHERE JP.EmployerID = @EmployerID\r\n  AND A.Status = 'Accepted';\r\n";
                    using (SqlCommand cmd = new SqlCommand(messagesQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@EmployerID", employerID);
                        int totalMessages = (int)cmd.ExecuteScalar();
                        lblMessages.Text = totalMessages.ToString();
                    }

                    // Add more statistics as needed
                }
                catch (SqlException sqlEx)
                {
                    // Log the exception details
                    System.Diagnostics.Trace.TraceError($"SQL Error in LoadDashboardStatistics: {sqlEx.Message}");
                    lblMessage.Text = "A database error occurred while loading dashboard statistics.";
                }
                catch (Exception ex)
                {
                    // Log the exception details
                    System.Diagnostics.Trace.TraceError($"Error in LoadDashboardStatistics: {ex.Message}");
                    lblMessage.Text = "An unexpected error occurred while loading dashboard statistics.";
                }
            }
        }

        /// <summary>
        /// Loads the list of job seekers for the chat functionality.
        /// </summary>
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

        /// <summary>
        /// Handles the click event for viewing the employer's profile.
        /// </summary>
        protected void lnkViewProfile_Click(object sender, EventArgs e)
        {
            Response.Redirect("EmployerProfile.aspx");
        }

        /// <summary>
        /// Handles the click event for accessing the inbox.
        /// </summary>
        protected void lnkInbox_Click(object sender, EventArgs e)
        {
            Response.Redirect("ChangeEmployerPassword.aspx");
        }

        /// <summary>
        /// Handles the click event for logging out the employer.
        /// </summary>
        protected void lnkLogout_Click(object sender, EventArgs e)
        {
            Session.Abandon();
            Response.Redirect("EmployerLogin.aspx");
        }

        /// <summary>
        /// Handles the Send Message button click event.
        /// </summary>
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
