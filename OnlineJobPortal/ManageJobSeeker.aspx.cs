using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace OnlineJobPortal
{
    public partial class ManageJobSeeker : Page
    {
        private string connectionString = ConfigurationManager.ConnectionStrings["OnlineJobPortalDB"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadJobSeekerData();
            }
        }

        private void LoadJobSeekerData()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "SELECT js.JobSeekerID, u.Username, js.FirstName, js.LastName, js.DateOfBirth, js.Gender, js.City, js.State FROM JobSeeker js INNER JOIN [User] u ON js.UserID = u.UserID";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    GridViewJobSeekers.DataSource = dt;
                    GridViewJobSeekers.DataBind();
                }
            }
        }

        protected void GridViewJobSeekers_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            // Get the JobSeekerID from the DataKeys collection
            int jobSeekerId = Convert.ToInt32(GridViewJobSeekers.DataKeys[e.RowIndex].Value);

            // Connect to the database
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                // Prepare the delete query
                string deleteJobSeekerQuery = "DELETE FROM JobSeeker WHERE JobSeekerID = @JobSeekerID";
                using (SqlCommand cmd = new SqlCommand(deleteJobSeekerQuery, conn))
                {
                    cmd.Parameters.AddWithValue("@JobSeekerID", jobSeekerId);
                    cmd.ExecuteNonQuery(); // Execute the delete query for JobSeeker
                }

                // Optionally, you can reload data to reflect changes
                LoadJobSeekerData();
            }
        }


        protected void lnkLogout_Click(object sender, EventArgs e)
        {
            Session.Clear();
            Response.Redirect("AdminLogin.aspx");
        }
    }
}
