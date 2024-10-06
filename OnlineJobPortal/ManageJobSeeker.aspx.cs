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
                LoadAdminName(); // Load the admin's username
            }
        }

        private void LoadJobSeekerData()
        {
            try
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
            catch (Exception ex)
            {
                // Log the error and display an error message
                lblErrorMessage.Text = "An error occurred while loading job seekers: " + ex.Message;
            }
        }

        private void LoadAdminName()
        {
            if (Session["AdminID"] != null)
            {
                int adminId = Convert.ToInt32(Session["AdminID"]);
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string query = "SELECT Username FROM [User] WHERE UserID = @UserID";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = adminId;
                        conn.Open();

                        object result = cmd.ExecuteScalar();
                        if (result != null)
                        {
                            lblAdminName.Text = result.ToString(); // Set the label to the admin's username
                        }
                        else
                        {
                            lblAdminName.Text = "Admin"; // Fallback if user not found
                        }
                    }
                }
            }
            else
            {
                lblAdminName.Text = "Admin"; // Default display if no admin ID in session
            }
        }

        protected void GridViewJobSeekers_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                int jobSeekerId = Convert.ToInt32(GridViewJobSeekers.DataKeys[e.RowIndex].Value);

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    // First, get the UserID of the JobSeeker
                    int userId;
                    string getUserIdQuery = "SELECT UserID FROM JobSeeker WHERE JobSeekerID = @JobSeekerID";
                    using (SqlCommand cmd = new SqlCommand(getUserIdQuery, conn))
                    {
                        cmd.Parameters.Add("@JobSeekerID", SqlDbType.Int).Value = jobSeekerId;
                        userId = (int)cmd.ExecuteScalar();
                    }

                    // Delete related chat messages
                    string deleteChatMessagesQuery = "DELETE FROM ChatMessages WHERE JobSeekerID = @JobSeekerID";
                    using (SqlCommand cmd = new SqlCommand(deleteChatMessagesQuery, conn))
                    {
                        cmd.Parameters.Add("@JobSeekerID", SqlDbType.Int).Value = jobSeekerId;
                        cmd.ExecuteNonQuery();
                    }

                    // Delete the JobSeeker
                    string deleteJobSeekerQuery = "DELETE FROM JobSeeker WHERE JobSeekerID = @JobSeekerID";
                    using (SqlCommand cmd = new SqlCommand(deleteJobSeekerQuery, conn))
                    {
                        cmd.Parameters.Add("@JobSeekerID", SqlDbType.Int).Value = jobSeekerId;
                        cmd.ExecuteNonQuery();
                    }

                    // Delete the User
                    string deleteUserQuery = "DELETE FROM [User] WHERE UserID = @UserID";
                    using (SqlCommand cmd = new SqlCommand(deleteUserQuery, conn))
                    {
                        cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = userId;
                        cmd.ExecuteNonQuery();
                    }
                }

                LoadJobSeekerData(); // Refresh the GridView
            }
            catch (Exception ex)
            {
                lblErrorMessage.Text = "An error occurred while deleting the job seeker: " + ex.Message;
            }
        }



        protected void lnkLogout_Click(object sender, EventArgs e)
        {
            Session.Clear();
            Response.Redirect("AdminLogin.aspx");
        }

        protected void lnkViewProfile_Click(object sender, EventArgs e)
        {
            // Redirect to profile page
            Response.Redirect("AdminProfile.aspx");
        }
    }
}