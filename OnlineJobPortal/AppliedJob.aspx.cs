using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace OnlineJobPortal
{
    public partial class AppliedJob : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindAppliedJobs();
            }
        }

        private void BindAppliedJobs()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["OnlineJobPortalDB"].ConnectionString;

            // Check if the JobSeekerID is in session
            if (Session["JobSeekerID"] == null)
            {
                lblStatusMessage.Text = "You need to log in to view your applied jobs.";
                return; // Exit the method if the session variable is not set
            }

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string query = "SELECT JobID, JobTitle, CompanyName, ApplicationDate, Status FROM JobApplications WHERE JobSeekerID = @JobSeekerID";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@JobSeekerID", Session["JobSeekerID"]);

                        using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                        {
                            DataTable dt = new DataTable();
                            sda.Fill(dt);
                            gvAppliedJobs.DataSource = dt;
                            gvAppliedJobs.DataBind();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                lblStatusMessage.Text = "An error occurred while fetching the applied jobs.";
                // Optionally log the error
            }
        }

        protected void gvAppliedJobs_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvAppliedJobs.PageIndex = e.NewPageIndex;
            BindAppliedJobs();
        }

        protected void lnkViewProfile_Click(object sender, EventArgs e)
        {
            // Handle profile view logic
        }

        protected void lnkLogout_Click(object sender, EventArgs e)
        {
            // Handle logout logic
            Session.Clear();
            Response.Redirect("Login.aspx");
        }
    }
}
