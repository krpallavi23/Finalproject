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

            if (Session["JobSeekerID"] == null)
            {
                lblStatusMessage.Text = "You need to log in to view your applied jobs.";
                return;
            }

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string query = "SELECT ja.JobID, jp.Title AS JobTitle, e.CompanyName, ja.ApplicationDate, ja.Status " +
                                   "FROM JobApplication ja " +
                                   "JOIN JobPosting jp ON ja.JobID = jp.JobID " +
                                   "JOIN Employer e ON jp.EmployerID = e.EmployerID " +
                                   "WHERE ja.JobSeekerID = @JobSeekerID";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@JobSeekerID", Session["JobSeekerID"]);

                        using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                        {
                            DataTable dt = new DataTable();
                            sda.Fill(dt);

                            if (dt.Rows.Count > 0)
                            {
                                gvAppliedJobs.DataSource = dt;
                                gvAppliedJobs.DataBind();
                            }
                            else
                            {
                                lblStatusMessage.Text = "No applied jobs found.";
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                lblStatusMessage.Text = "An error occurred while fetching the applied jobs: " + ex.Message;
            }
        }


        protected void gvAppliedJobs_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvAppliedJobs.PageIndex = e.NewPageIndex;
            BindAppliedJobs();
        }

        protected void lnkViewProfile_Click(object sender, EventArgs e)
        {
            Response.Redirect("JobSeekerProfile.aspx"); // Redirect to profile page
        }

        protected void lnkLogout_Click(object sender, EventArgs e)
        {
            Session.Clear();
            Response.Redirect("JobseekerLogin.aspx");
        }
    }
}
