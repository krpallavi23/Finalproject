using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;

namespace OnlineJobPortal
{
    public partial class EmployerDashboard : Page
    {
        // Connection string from Web.config
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
                    LoadEmployerName();
                    LoadDashboardStatistics();
                    LoadJobsPostedOverTime();
                    LoadCandidatesMatched();
                    LoadNotifications();
                    LoadServerStats();
                }
            }
        }

        private void LoadEmployerName()
        {
            int employerID = Convert.ToInt32(Session["EmployerID"]);
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "SELECT CompanyName FROM Employer WHERE EmployerID = @EmployerID";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@EmployerID", employerID);

                conn.Open();
                lblEmployerName.Text = cmd.ExecuteScalar()?.ToString() ?? "Employer";
            }
        }

        private void LoadDashboardStatistics()
        {
            int employerID = Convert.ToInt32(Session["EmployerID"]);

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                // Total Jobs Posted
                string totalJobsQuery = "SELECT COUNT(*) FROM JobPosting WHERE EmployerID = @EmployerID";
                SqlCommand totalJobsCmd = new SqlCommand(totalJobsQuery, conn);
                totalJobsCmd.Parameters.AddWithValue("@EmployerID", employerID);
                lblTotalJobs.Text = totalJobsCmd.ExecuteScalar().ToString();

                // Candidates Shortlisted (Assuming Status 'Shortlisted' in JobApplication)
                string candidatesShortlistedQuery = @"
                    SELECT COUNT(*) FROM JobApplication
                    WHERE JobID IN (SELECT JobID FROM JobPosting WHERE EmployerID = @EmployerID)
                    AND Status = 'Shortlisted'";
                SqlCommand candidatesShortlistedCmd = new SqlCommand(candidatesShortlistedQuery, conn);
                candidatesShortlistedCmd.Parameters.AddWithValue("@EmployerID", employerID);
                lblCandidatesShortlisted.Text = candidatesShortlistedCmd.ExecuteScalar().ToString();

                // Pending Reviews (Assuming Status 'Pending' in JobApplication)
                string pendingReviewsQuery = @"
                    SELECT COUNT(*) FROM JobApplication
                    WHERE JobID IN (SELECT JobID FROM JobPosting WHERE EmployerID = @EmployerID)
                    AND Status = 'Pending'";
                SqlCommand pendingReviewsCmd = new SqlCommand(pendingReviewsQuery, conn);
                pendingReviewsCmd.Parameters.AddWithValue("@EmployerID", employerID);
                lblPendingReviews.Text = pendingReviewsCmd.ExecuteScalar().ToString();

                // Interviews Scheduled (Assuming Status 'Interview Scheduled' in JobApplication)
                string interviewsScheduledQuery = @"
                    SELECT COUNT(*) FROM JobApplication
                    WHERE JobID IN (SELECT JobID FROM JobPosting WHERE EmployerID = @EmployerID)
                    AND Status = 'Interview Scheduled'";
                SqlCommand interviewsScheduledCmd = new SqlCommand(interviewsScheduledQuery, conn);
                interviewsScheduledCmd.Parameters.AddWithValue("@EmployerID", employerID);
                lblInterviewsScheduled.Text = interviewsScheduledCmd.ExecuteScalar().ToString();
            }
        }

        private void LoadJobsPostedOverTime()
        {
            int employerID = Convert.ToInt32(Session["EmployerID"]);

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
                WHERE EmployerID = @EmployerID
                GROUP BY YEAR(ApplicationDeadline), MONTH(ApplicationDeadline), DATENAME(MONTH, ApplicationDeadline)
            ),
            JobsWithChange AS (
                SELECT
                    mj.Year,
                    mj.MonthNumber,
                    mj.[Month],
                    mj.JobsPosted,
                    LAG(mj.JobsPosted) OVER (ORDER BY mj.Year, mj.MonthNumber) AS PreviousJobsPosted
                FROM MonthlyJobs mj
            )
            SELECT
                [Month],
                JobsPosted,
                CASE
                    WHEN PreviousJobsPosted IS NULL THEN 0
                    ELSE ((CAST(JobsPosted AS FLOAT) - PreviousJobsPosted) / PreviousJobsPosted) * 100
                END AS ChangePercentage
            FROM JobsWithChange
            ORDER BY Year, MonthNumber";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@EmployerID", employerID);

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
            int employerID = Convert.ToInt32(Session["EmployerID"]);

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = @"
            WITH DepartmentCandidates AS (
                SELECT
                    jp.JobCategory AS Department,
                    COUNT(ja.ApplicationID) AS CandidatesMatched
                FROM JobPosting jp
                LEFT JOIN JobApplication ja ON jp.JobID = ja.JobID
                WHERE jp.EmployerID = @EmployerID
                GROUP BY jp.JobCategory
            ),
            CandidatesWithChange AS (
                SELECT
                    dc.Department,
                    dc.CandidatesMatched,
                    LAG(dc.CandidatesMatched) OVER (ORDER BY dc.Department) AS PreviousCandidatesMatched
                FROM DepartmentCandidates dc
            )
            SELECT
                Department,
                CandidatesMatched,
                CASE
                    WHEN PreviousCandidatesMatched IS NULL THEN 0
                    ELSE ((CAST(CandidatesMatched AS FLOAT) - PreviousCandidatesMatched) / PreviousCandidatesMatched) * 100
                END AS ChangePercentage
            FROM CandidatesWithChange
            ORDER BY Department";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@EmployerID", employerID);

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
            int employerID = Convert.ToInt32(Session["EmployerID"]);

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = @"
                    SELECT TOP 5
                        'New application from ' + js.FirstName + ' ' + js.LastName AS NotificationText,
                        ja.ApplicationDate AS [Date]
                    FROM JobApplication ja
                    INNER JOIN JobPosting jp ON ja.JobID = jp.JobID
                    INNER JOIN JobSeeker js ON ja.JobSeekerID = js.JobSeekerID
                    WHERE jp.EmployerID = @EmployerID
                    ORDER BY ja.ApplicationDate DESC";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@EmployerID", employerID);

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
            // Placeholder values; replace with actual server monitoring code
            lblServerLoad.Text = "45%";
            divServerLoadProgress.Style["width"] = "45%";

            lblDiskSpace.Text = "70%";
            divDiskSpaceProgress.Style["width"] = "70%";

            lblMemoryUsage.Text = "60%";
            divMemoryUsageProgress.Style["width"] = "60%";
        }

        protected void imgProfile_Click(object sender, ImageClickEventArgs e)
        {
            // Toggle profile menu visibility
            pnlProfileMenu.Visible = !pnlProfileMenu.Visible;
        }

        protected void lnkEditProfile_Click(object sender, EventArgs e)
        {
            Response.Redirect("EditEmployerProfile.aspx");
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
            // Implement sorting logic
            DataTable dt = gvJobsPosted.DataSource as DataTable;
            if (dt != null)
            {
                dt.DefaultView.Sort = e.SortExpression;
                gvJobsPosted.DataSource = dt;
                gvJobsPosted.DataBind();
            }
        }

        protected void gvCandidatesMatched_Sorting(object sender, System.Web.UI.WebControls.GridViewSortEventArgs e)
        {
            // Implement sorting logic
            DataTable dt = gvCandidatesMatched.DataSource as DataTable;
            if (dt != null)
            {
                dt.DefaultView.Sort = e.SortExpression;
                gvCandidatesMatched.DataSource = dt;
                gvCandidatesMatched.DataBind();
            }
        }

        protected void btnSend_Click(object sender, EventArgs e)
        {
            // Implement chat functionality
            if (!string.IsNullOrEmpty(txtChat.Text.Trim()))
            {
                int employerID = Convert.ToInt32(Session["EmployerID"]);
                int jobSeekerID = 0; // Set this based on your chat implementation

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string query = @"
                        INSERT INTO ChatMessages (EmployerID, JobSeekerID, Message)
                        VALUES (@EmployerID, @JobSeekerID, @Message)";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@EmployerID", employerID);
                    cmd.Parameters.AddWithValue("@JobSeekerID", jobSeekerID);
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
            int employerID = Convert.ToInt32(Session["EmployerID"]);
            int jobSeekerID = 0; // Set this based on your chat implementation

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = @"
                    SELECT Message, MessageTime AS [Time]
                    FROM ChatMessages
                    WHERE EmployerID = @EmployerID AND JobSeekerID = @JobSeekerID
                    ORDER BY MessageTime DESC";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@EmployerID", employerID);
                cmd.Parameters.AddWithValue("@JobSeekerID", jobSeekerID);

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
