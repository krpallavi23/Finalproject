using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace OnlineJobPortal
{
    public partial class ShortlistedJobs : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindShortlistedJobs();
            }
        }

        private void BindShortlistedJobs()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["OnlineJobPortalDB"].ConnectionString;

            // Check if the JobSeekerID is in session
            if (Session["JobSeekerID"] == null)
            {
                lblStatus.Text = "You need to log in to view your shortlisted jobs.";
                return;
            }

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string query = "SELECT JobID, JobTitle, CompanyName, Location, JobType FROM JobApplications WHERE JobSeekerID = @JobSeekerID AND Status = 'Shortlisted'";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@JobSeekerID", Session["JobSeekerID"]);
                        SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);

                        gvShortlistedJobs.DataSource = dt;
                        gvShortlistedJobs.DataBind();
                    }
                }
            }
            catch (Exception ex)
            {
                lblStatus.Text = "Error: " + ex.Message;
            }
        }

        protected void gvShortlistedJobs_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvShortlistedJobs.PageIndex = e.NewPageIndex;
            BindShortlistedJobs();
        }

        protected void btnRemove_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            int jobId = Convert.ToInt32(btn.CommandArgument);
            RemoveShortlistedJob(jobId);
            BindShortlistedJobs();
        }

        private void RemoveShortlistedJob(int jobId)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["OnlineJobPortalDB"].ConnectionString;

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string query = "DELETE FROM JobApplications WHERE JobID = @JobID AND JobSeekerID = @JobSeekerID AND Status = 'Shortlisted'";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@JobID", jobId);
                        cmd.Parameters.AddWithValue("@JobSeekerID", Session["JobSeekerID"]);
                        conn.Open();
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                lblStatus.Text = "Error: " + ex.Message;
            }
        }

        protected void lnkViewProfile_Click(object sender, EventArgs e)
        {
            Response.Redirect("ViewProfile.aspx"); // Redirect to the profile page
        }

        protected void lnkInbox_Click(object sender, EventArgs e)
        {
            Response.Redirect("Inbox.aspx"); // Redirect to the inbox page
        }

        protected void lnkLogout_Click(object sender, EventArgs e)
        {
            // Implement logout logic
            Session.Clear();
            Response.Redirect("Login.aspx"); // Redirect to login page
        }
    }
}
