using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace OnlineJobPortal
{
    public partial class RejectedJob : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindRejectedJobListings();
                lblJobSeekerName.Text = "John Doe"; // Replace with actual job seeker name from session or DB
            }
        }

        private void BindRejectedJobListings()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["OnlineJobPortalDB"].ConnectionString;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                // Ensure the table name is correct
                string query = "SELECT JobID, JobTitle, CompanyName, Location, JobType FROM dbo.YourCorrectTableName WHERE Status = 'Rejected' AND JobSeekerID = @JobSeekerID";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@JobSeekerID", GetJobSeekerID()); // Ensure this method returns the correct ID

                    SqlDataAdapter adapter = new SqlDataAdapter(command);
                    DataTable dt = new DataTable();
                    try
                    {
                        connection.Open();
                        adapter.Fill(dt);
                        gvRejectedJobListings.DataSource = dt;
                        gvRejectedJobListings.DataBind();
                    }
                    catch (SqlException ex)
                    {
                        // Log the exception or show an error message
                        lblStatus.Text = "Error retrieving job listings: " + ex.Message;
                    }
                }
            }
        }

        protected void gvRejectedJobListings_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvRejectedJobListings.PageIndex = e.NewPageIndex;
            BindRejectedJobListings();
        }

        protected void btnReapply_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            int jobId = Convert.ToInt32(btn.CommandArgument);
            Response.Redirect($"JobApplication.aspx?JobID={jobId}");
        }

        protected void lnkViewProfile_Click(object sender, EventArgs e)
        {
            Response.Redirect("Profile.aspx");
        }

        protected void lnkLogout_Click(object sender, EventArgs e)
        {
            Session.Clear();
            Response.Redirect("Login.aspx");
        }

        private int GetJobSeekerID()
        {
            // Retrieve the Job Seeker ID from session or another source
            return Convert.ToInt32(Session["JobSeekerID"]);
        }
    }
}
