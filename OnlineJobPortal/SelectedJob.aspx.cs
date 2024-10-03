using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace OnlineJobPortal
{
    public partial class SelectedJob : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadSelectedJobListings(null, null, null); // Initial load with no filters
            }
        }

        private void LoadSelectedJobListings(string keyword, string location, string jobType)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["OnlineJobPortalDB"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("sp_GetSelectedJobs", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@Keyword", string.IsNullOrEmpty(keyword) ? (object)DBNull.Value : keyword);
                    cmd.Parameters.AddWithValue("@Location", string.IsNullOrEmpty(location) ? (object)DBNull.Value : location);
                    cmd.Parameters.AddWithValue("@JobType", string.IsNullOrEmpty(jobType) ? (object)DBNull.Value : jobType);

                    try
                    {
                        conn.Open();
                        SqlDataReader reader = cmd.ExecuteReader();
                        gvSelectedJobListings.DataSource = reader;
                        gvSelectedJobListings.DataBind();
                    }
                    catch (Exception ex)
                    {
                        lblStatus.Text = "An error occurred: " + ex.Message;
                    }
                }
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            string keyword = txtKeyword.Text.Trim();
            string location = ddlLocation.SelectedValue;
            string jobType = ddlJobType.SelectedValue;

            LoadSelectedJobListings(keyword, location, jobType);
        }

        protected void gvSelectedJobListings_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvSelectedJobListings.PageIndex = e.NewPageIndex;
            LoadSelectedJobListings(txtKeyword.Text.Trim(), ddlLocation.SelectedValue, ddlJobType.SelectedValue);
        }

        protected void btnApply_Click(object sender, EventArgs e)
        {
            Button btnApply = (Button)sender;
            string jobId = btnApply.CommandArgument;

            // Implement your apply logic here
            lblStatus.Text = "You have applied for Job ID: " + jobId;
        }

        // Add this method for profile view
        protected void lnkViewProfile_Click(object sender, EventArgs e)
        {
            // Redirect to the profile page
            Response.Redirect("JobSeekerProfile.aspx");
        }

        // Add this method for logout functionality
        protected void lnkLogout_Click(object sender, EventArgs e)
        {
            // Implement your logout logic here (e.g., clearing session)
            Session.Abandon();
            Response.Redirect("JobSeekerLogin.aspx"); // Redirect to the login page
        }
    }
}
