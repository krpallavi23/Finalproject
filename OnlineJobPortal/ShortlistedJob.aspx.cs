using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace OnlineJobPortal
{
    public partial class ShortlistedJob : System.Web.UI.Page
    {
        // Connection string from Web.config
        private string connectionString = ConfigurationManager.ConnectionStrings["OnlineJobPortalDB"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindJobApplications();
                BindDashboardWidgets();
                BindJobPostingsDropdown();
                BindChatMessages();
            }
        }

        /// <summary>
        /// Binds the GridView with shortlisted job applications.
        /// </summary>
        private void BindJobApplications()
        {
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    string query = @"SELECT ja.ApplicationID, jp.Title AS JobTitle, emp.CompanyName, ja.ApplicationDate, ja.Status 
                                     FROM JobApplication ja
                                     INNER JOIN JobPosting jp ON ja.JobID = jp.JobID
                                     INNER JOIN Employer emp ON jp.EmployerID = emp.EmployerID
                                     WHERE ja.JobSeekerID = @JobSeekerID AND ja.Status = 'Shortlisted'
                                     ORDER BY ja.ApplicationDate DESC";

                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@JobSeekerID", GetCurrentJobSeekerID());

                        using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                        {
                            DataTable dt = new DataTable();
                            sda.Fill(dt);
                            gvShortlistedJobs.DataSource = dt;
                            gvShortlistedJobs.DataBind();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Log the exception
                lblMessage.ForeColor = System.Drawing.Color.Red;
                lblMessage.Text = "An error occurred while loading your shortlisted jobs. Please try again later.";
            }
        }

        /// <summary>
        /// Handles paging for the GridView.
        /// </summary>
        protected void gvShortlistedJobs_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvShortlistedJobs.PageIndex = e.NewPageIndex;
            BindJobApplications();
        }

        /// <summary>
        /// Handles row commands such as viewing details.
        /// </summary>
        protected void gvShortlistedJobs_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "ViewDetails")
            {
                string applicationId = e.CommandArgument.ToString();
                // Redirect to a details page with the ApplicationID as query parameter
                Response.Redirect($"ApplicationDetails.aspx?ApplicationID={applicationId}");
            }
        }

        /// <summary>
        /// Retrieves the current logged-in JobSeeker's ID from the session.
        /// </summary>
        /// <returns>JobSeekerID as integer</returns>
        private int GetCurrentJobSeekerID()
        {
            // Implement your logic to retrieve the current logged-in JobSeeker's ID
            // For example, from session:
            if (Session["JobSeekerID"] != null)
            {
                return Convert.ToInt32(Session["JobSeekerID"]);
            }
            else
            {
                // Handle the case when the user is not logged in
                Response.Redirect("Home.aspx");
                return 0;
            }
        }

        /// <summary>
        /// Binds the dashboard widgets with relevant data.
        /// </summary>
        private void BindDashboardWidgets()
        {
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();

                    // Total Applications
                    string totalAppsQuery = @"SELECT COUNT(*) FROM JobApplication WHERE JobSeekerID = @JobSeekerID";
                    using (SqlCommand cmd = new SqlCommand(totalAppsQuery, con))
                    {
                        cmd.Parameters.AddWithValue("@JobSeekerID", GetCurrentJobSeekerID());
                        lblTotalApplications.Text = cmd.ExecuteScalar().ToString();
                    }

                    // Active Jobs
                    string activeJobsQuery = @"SELECT COUNT(DISTINCT JobID) 
                                               FROM JobApplication 
                                               WHERE JobSeekerID = @JobSeekerID AND Status IN ('Applied', 'Shortlisted', 'Selected')";
                    using (SqlCommand cmd = new SqlCommand(activeJobsQuery, con))
                    {
                        cmd.Parameters.AddWithValue("@JobSeekerID", GetCurrentJobSeekerID());
                        lblActiveJobs.Text = cmd.ExecuteScalar().ToString();
                    }

                    // Pending Applications
                    string pendingAppsQuery = @"SELECT COUNT(*) FROM JobApplication 
                                                WHERE JobSeekerID = @JobSeekerID AND Status = 'Pending'";
                    using (SqlCommand cmd = new SqlCommand(pendingAppsQuery, con))
                    {
                        cmd.Parameters.AddWithValue("@JobSeekerID", GetCurrentJobSeekerID());
                        lblPendingApplications.Text = cmd.ExecuteScalar().ToString();
                    }

                    // Messages
                    string messagesQuery = @"SELECT COUNT(*) FROM ChatMessages 
                                             WHERE JobSeekerID = @JobSeekerID";
                    using (SqlCommand cmd = new SqlCommand(messagesQuery, con))
                    {
                        cmd.Parameters.AddWithValue("@JobSeekerID", GetCurrentJobSeekerID());
                        lblMessages.Text = cmd.ExecuteScalar().ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                // Log the exception
                lblMessage.ForeColor = System.Drawing.Color.Red;
                lblMessage.Text = "An error occurred while loading dashboard data. Please try again later.";
            }
        }

        /// <summary>
        /// Binds the job postings dropdown with available job postings.
        /// </summary>
        private void BindJobPostingsDropdown()
        {
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    string query = @"SELECT JobPostingID, Title FROM JobPosting 
                                     WHERE EmployerID IN (SELECT EmployerID FROM Employer)"; // Adjust the WHERE clause as needed

                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        con.Open();
                        using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                        {
                            DataTable dt = new DataTable();
                            sda.Fill(dt);
                            ddlJobPostings.DataSource = dt;
                            ddlJobPostings.DataTextField = "Title";
                            ddlJobPostings.DataValueField = "JobPostingID";
                            ddlJobPostings.DataBind();
                            ddlJobPostings.Items.Insert(0, new ListItem("-- Select Job Posting --", ""));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Log the exception
                lblMessage.ForeColor = System.Drawing.Color.Red;
                lblMessage.Text = "An error occurred while loading job postings. Please try again later.";
            }
        }

        /// <summary>
        /// Handles sending a message related to a job posting.
        /// </summary>
        protected void btnSendMessage_Click(object sender, EventArgs e)
        {
            string selectedJobPosting = ddlJobPostings.SelectedValue;
            string message = txtChatMessage.Text.Trim();

            if (string.IsNullOrEmpty(selectedJobPosting))
            {
                lblMessage.Text = "Please select a job posting.";
                return;
            }

            if (string.IsNullOrEmpty(message))
            {
                lblMessage.Text = "Please enter a message.";
                return;
            }

            // Implement your message sending logic here
            // For example, insert the message into the ChatMessages table

            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    string insertQuery = @"INSERT INTO ChatMessages (EmployerID, JobSeekerID, Message, MessageTime) 
                                           VALUES (@EmployerID, @JobSeekerID, @Message, @MessageTime)";

                    using (SqlCommand cmd = new SqlCommand(insertQuery, con))
                    {
                        // Assuming each JobPosting is associated with an Employer
                        cmd.Parameters.AddWithValue("@EmployerID", GetEmployerIDByJobPostingID(selectedJobPosting));
                        cmd.Parameters.AddWithValue("@JobSeekerID", GetCurrentJobSeekerID());
                        cmd.Parameters.AddWithValue("@Message", message);
                        cmd.Parameters.AddWithValue("@MessageTime", DateTime.Now);

                        con.Open();
                        cmd.ExecuteNonQuery();
                    }
                }

                lblMessage.ForeColor = System.Drawing.Color.Green;
                lblMessage.Text = "Message sent successfully.";
                txtChatMessage.Text = string.Empty;

                // Optionally, refresh the chat messages
                BindChatMessages();
            }
            catch (Exception ex)
            {
                // Log the exception
                lblMessage.ForeColor = System.Drawing.Color.Red;
                lblMessage.Text = "An error occurred while sending your message. Please try again later.";
            }
        }

        /// <summary>
        /// Retrieves the EmployerID associated with a given JobPostingID.
        /// </summary>
        private int GetEmployerIDByJobPostingID(string jobPostingID)
        {
            int employerID = 0;
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = @"SELECT EmployerID FROM JobPosting WHERE JobID = @JobID";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@JobID", jobPostingID);
                    con.Open();
                    object result = cmd.ExecuteScalar();
                    if (result != null)
                    {
                        employerID = Convert.ToInt32(result);
                    }
                }
            }
            return employerID;
        }

        /// <summary>
        /// Binds the chat messages to the repeater control.
        /// </summary>
        private void BindChatMessages()
        {
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    string chatQuery = @"SELECT cm.Message, cm.MessageTime, emp.CompanyName 
                                         FROM ChatMessages cm
                                         INNER JOIN Employer emp ON cm.EmployerID = emp.EmployerID
                                         WHERE cm.JobSeekerID = @JobSeekerID 
                                         ORDER BY cm.MessageTime DESC";

                    using (SqlCommand cmd = new SqlCommand(chatQuery, con))
                    {
                        cmd.Parameters.AddWithValue("@JobSeekerID", GetCurrentJobSeekerID());

                        using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                        {
                            DataTable dt = new DataTable();
                            sda.Fill(dt);
                            rptChatMessages.DataSource = dt;
                            rptChatMessages.DataBind();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Log the exception
                // Optionally, display a message or handle accordingly
            }
        }

        /// <summary>
        /// Redirects to the Job Seeker's profile page.
        /// </summary>
        protected void lnkViewProfile_Click(object sender, EventArgs e)
        {
            Response.Redirect("JobSeekerProfile.aspx");
        }

        /// <summary>
        /// Handles the logout process by clearing the session and redirecting to the login page.
        /// </summary>
        protected void lnkLogout_Click(object sender, EventArgs e)
        {
            // Implement your logout logic here
            Session.Clear();
            Response.Redirect("Home.aspx");
        }

        /// <summary>
        /// Ensures that chat messages are bound every time the page is rendered.
        /// </summary>
        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            // Ensure chat messages are bound before rendering
            BindChatMessages();
        }
    }
}
