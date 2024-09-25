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
        private readonly string connectionString = ConfigurationManager.ConnectionStrings["OnlineJobPortalDB"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["JobSeekerID"] == null)
                {
                    Response.Redirect("JobSeekerLogin.aspx");
                }
                else
                {
                    LoadJobSeekerName();
                    LoadDashboardStatistics();
                    LoadJobsPostedOverTime();
                    LoadCandidatesMatched();
                    LoadNotifications();
                    LoadServerStats();
                }
            }
        }

        private void LoadJobSeekerName()
        {
            int jobSeekerID = Convert.ToInt32(Session["JobSeekerID"]);
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "SELECT FirstName, LastName FROM JobSeeker WHERE JobSeekerID = @JobSeekerID";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@JobSeekerID", jobSeekerID);

                conn.Open();
                lblJobSeekerName.Text = cmd.ExecuteScalar()?.ToString() ?? "JobSeeker";
            }
        }

        private void LoadDashboardStatistics()
        {
            int jobSeekerID = Convert.ToInt32(Session["JobSeekerID"]);

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                lblTotalJobs.Text = GetCount(conn, "JobPosting", jobSeekerID);
                lblCandidatesShortlisted.Text = GetCandidatesCount(conn, jobSeekerID, "Shortlisted");
                lblPendingReviews.Text = GetCandidatesCount(conn, jobSeekerID, "Pending");
                lblInterviewsScheduled.Text = GetCandidatesCount(conn, jobSeekerID, "Interview Scheduled");
            }
        }

        private string GetCount(SqlConnection conn, string tableName, int jobSeekerID)
        {
            string query = $"SELECT COUNT(*) FROM {tableName} WHERE JobSeekerID = @JobSeekerID";
            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@JobSeekerID", jobSeekerID);
            return cmd.ExecuteScalar().ToString();
        }

        private string GetCandidatesCount(SqlConnection conn, int jobSeekerID, string status)
        {
            string query = @"
                SELECT COUNT(*) FROM JobApplication
                WHERE JobID IN (SELECT JobID FROM JobPosting WHERE JobSeekerID = @JobSeekerID)
                AND Status = @Status";
            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@JobSeekerID", jobSeekerID);
            cmd.Parameters.AddWithValue("@Status", status);
            return cmd.ExecuteScalar().ToString();
        }

        private void LoadJobsPostedOverTime()
        {
            int jobSeekerID = Convert.ToInt32(Session["JobSeekerID"]);

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = @"
                    WITH MonthlyJobs AS (
                        SELECT
                            YEAR(ApplicationDeadline) AS Year,
                            MONTH(ApplicationDeadline) AS MonthNumber,
                            DATENAME(MONTH, ApplicationDeadline) AS [Month],
                            COUNT(*) AS JobsPosted
                        FROM JobPosting
                        WHERE JobSeekerID = @JobSeekerID
                        GROUP BY YEAR(ApplicationDeadline), MONTH(ApplicationDeadline), DATENAME(MONTH, ApplicationDeadline)
                    )
                    SELECT [Month], JobsPosted
                    FROM MonthlyJobs
                    ORDER BY Year, MonthNumber";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@JobSeekerID", jobSeekerID);

                conn.Open();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                gvJobsPosted.DataSource = dt;
                gvJobsPosted.DataBind();
            }
        }

        private void LoadCandidatesMatched()
        {
            int jobSeekerID = Convert.ToInt32(Session["JobSeekerID"]);

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = @"
                    WITH DepartmentCandidates AS (
                        SELECT
                            jp.JobCategory AS Department,
                            COUNT(ja.ApplicationID) AS CandidatesMatched
                        FROM JobPosting jp
                        LEFT JOIN JobApplication ja ON jp.JobID = ja.JobID
                        WHERE jp.JobSeekerID = @JobSeekerID
                        GROUP BY jp.JobCategory
                    )
                    SELECT Department, CandidatesMatched
                    FROM DepartmentCandidates
                    ORDER BY Department";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@JobSeekerID", jobSeekerID);

                conn.Open();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                gvCandidatesMatched.DataSource = dt;
                gvCandidatesMatched.DataBind();
            }
        }

        private void LoadNotifications()
        {
            int jobSeekerID = Convert.ToInt32(Session["JobSeekerID"]);

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = @"
                    SELECT TOP 5
                        'New application from ' + js.FirstName + ' ' + js.LastName AS NotificationText,
                        ja.ApplicationDate AS [Date]
                    FROM JobApplication ja
                    INNER JOIN JobPosting jp ON ja.JobID = jp.JobID
                    INNER JOIN JobSeeker js ON ja.JobSeekerID = js.JobSeekerID
                    WHERE jp.JobSeekerID = @JobSeekerID
                    ORDER BY ja.ApplicationDate DESC";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@JobSeekerID", jobSeekerID);

                conn.Open();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                rptNotifications.DataSource = dt;
                rptNotifications.DataBind();
            }
        }

        private void LoadServerStats()
        {
            lblServerLoad.Text = "45%";
            divServerLoadProgress.Style["width"] = "45%";

            lblDiskSpace.Text = "70%";
            divDiskSpaceProgress.Style["width"] = "70%";

            lblMemoryUsage.Text = "60%";
            divMemoryUsageProgress.Style["width"] = "60%";
        }

        protected void imgProfile_Click(object sender, ImageClickEventArgs e)
        {
            pnlProfileMenu.Visible = !pnlProfileMenu.Visible;
        }

        protected void lnkEditProfile_Click(object sender, EventArgs e)
        {
            Response.Redirect("EditJobSeekerProfile.aspx");
        }

        protected void lnkSettings_Click(object sender, EventArgs e)
        {
            Response.Redirect("Settings.aspx");
        }

        protected void lnkLogout_Click(object sender, EventArgs e)
        {
            Session.Abandon();
            Response.Redirect("Login.aspx");
        }

        protected void gvJobsPosted_Sorting(object sender, System.Web.UI.WebControls.GridViewSortEventArgs e)
        {
            SortGridView(gvJobsPosted, e.SortExpression);
        }

        protected void gvCandidatesMatched_Sorting(object sender, System.Web.UI.WebControls.GridViewSortEventArgs e)
        {
            SortGridView(gvCandidatesMatched, e.SortExpression);
        }

        private void SortGridView(GridView gridView, string sortExpression)
        {
            DataTable dt = gridView.DataSource as DataTable;
            if (dt != null)
            {
                dt.DefaultView.Sort = sortExpression;
                gridView.DataSource = dt;
                gridView.DataBind();
            }
        }

        protected void btnSend_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtChat.Text.Trim()))
            {
                int jobSeekerID = Convert.ToInt32(Session["JobSeekerID"]);
                int recipientID = 0; // Set this based on your chat implementation

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string query = @"
                        INSERT INTO ChatMessages (JobSeekerID, RecipientID, Message)
                        VALUES (@JobSeekerID, @RecipientID, @Message)";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@JobSeekerID", jobSeekerID);
                    cmd.Parameters.AddWithValue("@RecipientID", recipientID);
                    cmd.Parameters.AddWithValue("@Message", txtChat.Text.Trim());

                    conn.Open();
                    cmd.ExecuteNonQuery();
                }

                txtChat.Text = string.Empty;
                LoadChatMessages();
            }
        }

        private void LoadChatMessages()
        {
            int jobSeekerID = Convert.ToInt32(Session["JobSeekerID"]);
            int recipientID = 0; // Set this based on your chat implementation

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = @"
                    SELECT Message, MessageTime AS [Time]
                    FROM ChatMessages
                    WHERE JobSeekerID = @JobSeekerID AND RecipientID = @RecipientID
                    ORDER BY MessageTime DESC";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@JobSeekerID", jobSeekerID);
                cmd.Parameters.AddWithValue("@RecipientID", recipientID);

                conn.Open();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                rptChatMessages.DataSource = dt;
                rptChatMessages.DataBind();
            }
        }
    }
}
