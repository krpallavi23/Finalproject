using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace OnlineJobPortal
{
    public partial class EmployerDashboard : System.Web.UI.Page
    {
        // Connection string from Web.config
        private string connectionString = ConfigurationManager.ConnectionStrings["OnlineJobPortalDB"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Check if the employer is logged in
                if (Session["EmployerID"] == null)
                {
                    Response.Redirect("EmployerLogin.aspx");
                }
                else
                {
                    int employerID = Convert.ToInt32(Session["EmployerID"]);
                    LoadEmployerName(employerID);
                    LoadDashboardData(employerID);
                    PopulateNotifications(employerID);
                    LoadChatMessages(employerID);
                }
            }
        }

        /// <summary>
        /// Loads the employer's name and displays a welcome message.
        /// </summary>
        /// <param name="employerID">The EmployerID from the session.</param>
        private void LoadEmployerName(int employerID)
        {
            string query = @"SELECT e.CompanyName, e.ContactPerson
                             FROM Employer e
                             WHERE e.EmployerID = @EmployerID";

            using (SqlConnection conn = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@EmployerID", employerID);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    string companyName = reader["CompanyName"].ToString();
                    string contactPerson = reader["ContactPerson"].ToString();
                    lblEmployerName.Text = $"{contactPerson} from {companyName}";
                }
                conn.Close();
            }
        }

        /// <summary>
        /// Loads dashboard metrics such as total jobs, shortlisted candidates, etc.
        /// </summary>
        /// <param name="employerID">The EmployerID from the session.</param>
        private void LoadDashboardData(int employerID)
        {
            // Queries to retrieve metrics
            string totalJobsQuery = @"SELECT COUNT(*) FROM JobPosting WHERE EmployerID = @EmployerID AND Status = 'Active'";
            string shortlistedCandidatesQuery = @"SELECT COUNT(DISTINCT ja.JobSeekerID)
                                                  FROM JobApplication ja
                                                  INNER JOIN JobPosting jp ON ja.JobID = jp.JobID
                                                  WHERE jp.EmployerID = @EmployerID AND ja.Status = 'Interviewed'";
            string pendingReviewsQuery = @"SELECT COUNT(*) FROM JobApplication ja
                                          INNER JOIN JobPosting jp ON ja.JobID = jp.JobID
                                          WHERE jp.EmployerID = @EmployerID AND ja.Status = 'Applied'";
            string interviewsScheduledQuery = @"SELECT COUNT(*) FROM JobApplication ja
                                               INNER JOIN JobPosting jp ON ja.JobID = jp.JobID
                                               WHERE jp.EmployerID = @EmployerID AND ja.Status = 'Interviewed'";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                // Total Jobs Posted
                using (SqlCommand cmd = new SqlCommand(totalJobsQuery, conn))
                {
                    cmd.Parameters.AddWithValue("@EmployerID", employerID);
                    int totalJobs = (int)cmd.ExecuteScalar();
                    lblTotalJobs.Text = totalJobs.ToString();
                    // Assuming totalJobs is the baseline (100%)
                    UpdateProgressBar("TotalJobs", 100); // Adjust based on your logic
                    lblTotalJobsPercent.Text = "100%";
                }

                // Candidates Shortlisted
                using (SqlCommand cmd = new SqlCommand(shortlistedCandidatesQuery, conn))
                {
                    cmd.Parameters.AddWithValue("@EmployerID", employerID);
                    int shortlisted = (int)cmd.ExecuteScalar();
                    lblCandidatesShortlisted.Text = shortlisted.ToString();
                    // Assuming max shortlisted is 200 for percentage
                    int percent = CalculatePercentage(shortlisted, 200);
                    UpdateProgressBar("CandidatesShortlisted", percent);
                    lblCandidatesShortlistedPercent.Text = $"{percent}%";
                }

                // Pending Reviews
                using (SqlCommand cmd = new SqlCommand(pendingReviewsQuery, conn))
                {
                    cmd.Parameters.AddWithValue("@EmployerID", employerID);
                    int pending = (int)cmd.ExecuteScalar();
                    lblPendingReviews.Text = pending.ToString();
                    // Assuming max pending is 50 for percentage
                    int percent = CalculatePercentage(pending, 50);
                    UpdateProgressBar("PendingReviews", percent);
                    lblPendingReviewsPercent.Text = $"{percent}%";
                }

                // Interviews Scheduled
                using (SqlCommand cmd = new SqlCommand(interviewsScheduledQuery, conn))
                {
                    cmd.Parameters.AddWithValue("@EmployerID", employerID);
                    int interviews = (int)cmd.ExecuteScalar();
                    lblInterviewsScheduled.Text = interviews.ToString();
                    // Assuming max interviews is 30 for percentage
                    int percent = CalculatePercentage(interviews, 30);
                    UpdateProgressBar("InterviewsScheduled", percent);
                    lblInterviewsScheduledPercent.Text = $"{percent}%";
                }

                conn.Close();
            }

            // Load additional data for GridViews
            BindJobsPostedOverTime(employerID);
            BindCandidatesMatched(employerID);
            LoadServerStats();
        }

        /// <summary>
        /// Binds data to the Jobs Posted Over Time GridView.
        /// </summary>
        /// <param name="employerID">The EmployerID from the session.</param>
        private void BindJobsPostedOverTime(int employerID)
        {
            string query = @"SELECT DATENAME(month, ApplicationDeadline) AS Month, COUNT(*) AS JobsPosted
                             FROM JobPosting
                             WHERE EmployerID = @EmployerID
                             GROUP BY DATENAME(month, ApplicationDeadline), MONTH(ApplicationDeadline)
                             ORDER BY MONTH(ApplicationDeadline)";

            DataTable dt = new DataTable();

            using (SqlConnection conn = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand(query, conn))
            using (SqlDataAdapter da = new SqlDataAdapter(cmd))
            {
                cmd.Parameters.AddWithValue("@EmployerID", employerID);
                da.Fill(dt);
            }

            gvJobsPosted.DataSource = dt;
            gvJobsPosted.DataBind();
        }

        /// <summary>
        /// Binds data to the Candidates Matched GridView.
        /// </summary>
        /// <param name="employerID">The EmployerID from the session.</param>
        private void BindCandidatesMatched(int employerID)
        {
            string query = @"SELECT jt.SkillName, COUNT(jsk.JobSeekerID) AS CandidatesMatched
                             FROM JobPosting jp
                             INNER JOIN JobSkill js ON jp.JobID = js.JobID
                             INNER JOIN JobSeekerSkill jsk ON js.SkillTagID = jsk.SkillTagID
                             INNER JOIN SkillTag jt ON js.SkillTagID = jt.SkillTagID
                             WHERE jp.EmployerID = @EmployerID
                             GROUP BY jt.SkillName";

            DataTable dt = new DataTable();

            using (SqlConnection conn = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand(query, conn))
            using (SqlDataAdapter da = new SqlDataAdapter(cmd))
            {
                cmd.Parameters.AddWithValue("@EmployerID", employerID);
                da.Fill(dt);
            }

            gvCandidatesMatched.DataSource = dt;
            gvCandidatesMatched.DataBind();
        }

        /// <summary>
        /// Populates the notifications list with relevant notifications for the employer.
        /// </summary>
        /// <param name="employerID">The EmployerID from the session.</param>
        private void PopulateNotifications(int employerID)
        {
            string query = @"SELECT TOP 5 
                                'New application for job: ' + jp.Title AS Notification,
                                CONVERT(VARCHAR, ja.ApplicationDate, 101) AS Date
                             FROM JobApplication ja
                             INNER JOIN JobPosting jp ON ja.JobID = jp.JobID
                             WHERE jp.EmployerID = @EmployerID AND ja.Status = 'Applied'
                             ORDER BY ja.ApplicationDate DESC";

            DataTable dt = new DataTable();

            using (SqlConnection conn = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand(query, conn))
            using (SqlDataAdapter da = new SqlDataAdapter(cmd))
            {
                cmd.Parameters.AddWithValue("@EmployerID", employerID);
                da.Fill(dt);
            }

            if (dt.Rows.Count > 0)
            {
                rptNotifications.DataSource = dt;
                rptNotifications.DataBind();
            }
            else
            {
                // Show a default message if no notifications
                DataTable dtDefault = new DataTable();
                dtDefault.Columns.Add("Notification");
                dtDefault.Columns.Add("Date");
                DataRow dr = dtDefault.NewRow();
                dr["Notification"] = "No new notifications.";
                dr["Date"] = "";
                dtDefault.Rows.Add(dr);
                rptNotifications.DataSource = dtDefault;
                rptNotifications.DataBind();
            }
        }

        /// <summary>
        /// Binds chat messages to the chat Repeater control.
        /// </summary>
        /// <param name="employerID">The EmployerID from the session.</param>
        private void LoadChatMessages(int employerID)
        {
            // Placeholder: Implement actual chat retrieval logic
            // For demonstration, we'll add some dummy messages

            DataTable dt = new DataTable();
            dt.Columns.Add("Message");
            dt.Columns.Add("Time");

            // Example data
            dt.Rows.Add("Welcome to the Employer Dashboard!", DateTime.Now.AddMinutes(-30).ToString("hh:mm tt"));
            dt.Rows.Add("Feel free to post new jobs and manage your candidates.", DateTime.Now.AddMinutes(-20).ToString("hh:mm tt"));

            rptChatMessages.DataSource = dt;
            rptChatMessages.DataBind();
        }

        /// <summary>
        /// Updates the Bootstrap progress bar based on the type and percentage.
        /// </summary>
        /// <param name="progressType">Type of progress (e.g., TotalJobs).</param>
        /// <param name="percent">Percentage to set.</param>
        private void UpdateProgressBar(string progressType, int percent)
        {
            percent = percent > 100 ? 100 : percent;
            switch (progressType)
            {
                case "TotalJobs":
                    divTotalJobsProgress.Style["width"] = $"{percent}%";
                    divTotalJobsProgress.Attributes["aria-valuenow"] = percent.ToString();
                    lblTotalJobsPercent.Text = $"{percent}%";
                    break;
                case "CandidatesShortlisted":
                    divCandidatesShortlistedProgress.Style["width"] = $"{percent}%";
                    divCandidatesShortlistedProgress.Attributes["aria-valuenow"] = percent.ToString();
                    lblCandidatesShortlistedPercent.Text = $"{percent}%";
                    break;
                case "PendingReviews":
                    divPendingReviewsProgress.Style["width"] = $"{percent}%";
                    divPendingReviewsProgress.Attributes["aria-valuenow"] = percent.ToString();
                    lblPendingReviewsPercent.Text = $"{percent}%";
                    break;
                case "InterviewsScheduled":
                    divInterviewsScheduledProgress.Style["width"] = $"{percent}%";
                    divInterviewsScheduledProgress.Attributes["aria-valuenow"] = percent.ToString();
                    lblInterviewsScheduledPercent.Text = $"{percent}%";
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Calculates the percentage based on actual and maximum values.
        /// </summary>
        /// <param name="actual">Actual value.</param>
        /// <param name="max">Maximum value for calculation.</param>
        /// <returns>Calculated percentage.</returns>
        private int CalculatePercentage(int actual, int max)
        {
            if (max == 0) return 0;
            int percent = (actual * 100) / max;
            return percent > 100 ? 100 : percent;
        }

        /// <summary>
        /// Populates the server stats progress bars.
        /// </summary>
        private void LoadServerStats()
        {
            // Example values; replace with actual server stats retrieval logic
            int serverLoad = GetServerLoad(); // Implement actual logic
            int diskSpace = GetDiskSpace();   // Implement actual logic
            int memoryUsage = GetMemoryUsage(); // Implement actual logic

            // Update Server Load
            divServerLoadProgress.Style["width"] = $"{serverLoad}%";
            divServerLoadProgress.Attributes["aria-valuenow"] = serverLoad.ToString();
            lblServerLoad.Text = $"{serverLoad}%";

            // Update Disk Space
            divDiskSpaceProgress.Style["width"] = $"{diskSpace}%";
            divDiskSpaceProgress.Attributes["aria-valuenow"] = diskSpace.ToString();
            lblDiskSpace.Text = $"{diskSpace}%";

            // Update Memory Usage
            divMemoryUsageProgress.Style["width"] = $"{memoryUsage}%";
            divMemoryUsageProgress.Attributes["aria-valuenow"] = memoryUsage.ToString();
            lblMemoryUsage.Text = $"{memoryUsage}%";
        }

        /// <summary>
        /// Retrieves the server load percentage.
        /// </summary>
        /// <returns>Server load percentage.</returns>
        private int GetServerLoad()
        {
            // TODO: Implement actual server load retrieval logic
            // For demonstration, return a dummy value
            return 65;
        }

        /// <summary>
        /// Retrieves the disk space usage percentage.
        /// </summary>
        /// <returns>Disk space usage percentage.</returns>
        private int GetDiskSpace()
        {
            // TODO: Implement actual disk space retrieval logic
            // For demonstration, return a dummy value
            return 80;
        }

        /// <summary>
        /// Retrieves the memory usage percentage.
        /// </summary>
        /// <returns>Memory usage percentage.</returns>
        private int GetMemoryUsage()
        {
            // TODO: Implement actual memory usage retrieval logic
            // For demonstration, return a dummy value
            return 55;
        }

        #region GridView Sorting

        protected void gvJobsPosted_Sorting(object sender, GridViewSortEventArgs e)
        {
            int employerID = Convert.ToInt32(Session["EmployerID"]);
            DataTable dt = GetJobsPostedOverTime(employerID);

            if (dt != null)
            {
                DataView dv = new DataView(dt);
                string sortDirection = GetSortDirection("JobsPostedOverTimeSortDirection", "JobsPostedOverTimeSortExpression", e.SortExpression);
                dv.Sort = e.SortExpression + " " + sortDirection;
                gvJobsPosted.DataSource = dv;
                gvJobsPosted.DataBind();
            }
        }

        protected void gvCandidatesMatched_Sorting(object sender, GridViewSortEventArgs e)
        {
            int employerID = Convert.ToInt32(Session["EmployerID"]);
            DataTable dt = GetCandidatesMatched(employerID);

            if (dt != null)
            {
                DataView dv = new DataView(dt);
                string sortDirection = GetSortDirection("CandidatesMatchedSortDirection", "CandidatesMatchedSortExpression", e.SortExpression);
                dv.Sort = e.SortExpression + " " + sortDirection;
                gvCandidatesMatched.DataSource = dv;
                gvCandidatesMatched.DataBind();
            }
        }

        /// <summary>
        /// Retrieves the Jobs Posted Over Time data.
        /// </summary>
        /// <param name="employerID">The EmployerID from the session.</param>
        /// <returns>DataTable with Jobs Posted Over Time data.</returns>
        private DataTable GetJobsPostedOverTime(int employerID)
        {
            string query = @"SELECT DATENAME(month, ApplicationDeadline) AS Month, COUNT(*) AS JobsPosted
                             FROM JobPosting
                             WHERE EmployerID = @EmployerID
                             GROUP BY DATENAME(month, ApplicationDeadline), MONTH(ApplicationDeadline)
                             ORDER BY MONTH(ApplicationDeadline)";

            DataTable dt = new DataTable();

            using (SqlConnection conn = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand(query, conn))
            using (SqlDataAdapter da = new SqlDataAdapter(cmd))
            {
                cmd.Parameters.AddWithValue("@EmployerID", employerID);
                da.Fill(dt);
            }

            return dt;
        }

        /// <summary>
        /// Retrieves the Candidates Matched data.
        /// </summary>
        /// <param name="employerID">The EmployerID from the session.</param>
        /// <returns>DataTable with Candidates Matched data.</returns>
        private DataTable GetCandidatesMatched(int employerID)
        {
            string query = @"SELECT jt.SkillName, COUNT(jsk.JobSeekerID) AS CandidatesMatched
                             FROM JobPosting jp
                             INNER JOIN JobSkill js ON jp.JobID = js.JobID
                             INNER JOIN JobSeekerSkill jsk ON js.SkillTagID = jsk.SkillTagID
                             INNER JOIN SkillTag jt ON js.SkillTagID = jt.SkillTagID
                             WHERE jp.EmployerID = @EmployerID
                             GROUP BY jt.SkillName";

            DataTable dt = new DataTable();

            using (SqlConnection conn = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand(query, conn))
            using (SqlDataAdapter da = new SqlDataAdapter(cmd))
            {
                cmd.Parameters.AddWithValue("@EmployerID", employerID);
                da.Fill(dt);
            }

            return dt;
        }

        /// <summary>
        /// Retrieves the sort direction for GridView columns.
        /// </summary>
        /// <param name="sortDirectionKey">ViewState key for sort direction.</param>
        /// <param name="sortExpressionKey">ViewState key for sort expression.</param>
        /// <param name="currentSortExpression">Current sort expression.</param>
        /// <returns>Sort direction as string ("ASC" or "DESC").</returns>
        private string GetSortDirection(string sortDirectionKey, string sortExpressionKey, string currentSortExpression)
        {
            string sortDirection = "ASC";
            string previousSortExpression = ViewState[sortExpressionKey] as string;

            if (previousSortExpression != null)
            {
                if (previousSortExpression == currentSortExpression)
                {
                    string lastDirection = ViewState[sortDirectionKey] as string;
                    if ((lastDirection != null) && (lastDirection == "ASC"))
                    {
                        sortDirection = "DESC";
                    }
                }
            }

            ViewState[sortDirectionKey] = sortDirection;
            ViewState[sortExpressionKey] = currentSortExpression;

            return sortDirection;
        }

        #endregion

        /// <summary>
        /// Populates the chat messages Repeater control.
        /// </summary>
        /// <param name="employerID">The EmployerID from the session.</param>
        private void LoadChatMessages(int employerID)
        {
            // Placeholder: Implement actual chat retrieval logic
            // For demonstration, we'll add some dummy messages

            string query = @"SELECT Message, CONVERT(VARCHAR, MessageTime, 101) AS Time
                             FROM ChatMessages
                             WHERE EmployerID = @EmployerID
                             ORDER BY MessageTime DESC";

            DataTable dt = new DataTable();

            using (SqlConnection conn = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand(query, conn))
            using (SqlDataAdapter da = new SqlDataAdapter(cmd))
            {
                cmd.Parameters.AddWithValue("@EmployerID", employerID);
                da.Fill(dt);
            }

            if (dt.Rows.Count > 0)
            {
                rptChatMessages.DataSource = dt;
                rptChatMessages.DataBind();
            }
            else
            {
                // Show a default message if no chat messages
                DataTable dtDefault = new DataTable();
                dtDefault.Columns.Add("Message");
                dtDefault.Columns.Add("Time");
                DataRow dr = dtDefault.NewRow();
                dr["Message"] = "No chat messages yet.";
                dr["Time"] = "";
                dtDefault.Rows.Add(dr);
                rptChatMessages.DataSource = dtDefault;
                rptChatMessages.DataBind();
            }
        }

        protected void imgProfile_Click(object sender, EventArgs e)
        {
            pnlProfileMenu.Visible = !pnlProfileMenu.Visible;
        }

        protected void lnkEditProfile_Click(object sender, EventArgs e)
        {
            Response.Redirect("EditEmployerProfile.aspx");
        }

        protected void lnkSettings_Click(object sender, EventArgs e)
        {
            Response.Redirect("EmployerSettings.aspx");
        }

        protected void lnkLogout_Click(object sender, EventArgs e)
        {
            Session.Abandon();
            Response.Redirect("EmployerLogin.aspx");
        }

        protected void btnSend_Click(object sender, EventArgs e)
        {
            string message = txtChat.Text.Trim();
            if (!string.IsNullOrEmpty(message))
            {
                // Insert the message into the database
                InsertChatMessage(Convert.ToInt32(Session["EmployerID"]), message);

                // Reload chat messages
                LoadChatMessages(Convert.ToInt32(Session["EmployerID"]));

                txtChat.Text = string.Empty;
            }
        }

        /// <summary>
        /// Inserts a new chat message into the database.
        /// </summary>
        /// <param name="employerID">The EmployerID from the session.</param>
        /// <param name="message">The chat message.</param>
        private void InsertChatMessage(int employerID, string message)
        {
            string query = @"INSERT INTO ChatMessages (EmployerID, Message, MessageTime)
                             VALUES (@EmployerID, @Message, @MessageTime)";

            using (SqlConnection conn = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@EmployerID", employerID);
                cmd.Parameters.AddWithValue("@Message", message);
                cmd.Parameters.AddWithValue("@MessageTime", DateTime.Now);
                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();
            }
        }

        /// <summary>
        /// Overrides the OnPreRender event to load server stats after all other data is loaded.
        /// </summary>
        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            // Server stats are loaded within LoadDashboardData
            // If you have separate logic, you can call it here
        }
    }
}
