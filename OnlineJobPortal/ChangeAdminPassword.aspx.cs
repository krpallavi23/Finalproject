using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Web.UI;

namespace OnlineJobPortal
{
    public partial class ChangeAdminPassword : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadAdminName(); // Load the admin's username
            }
        }

        private void LoadAdminName()
        {
            if (Session["AdminID"] != null)
            {
                int adminID = Convert.ToInt32(Session["AdminID"]);
                string connectionString = ConfigurationManager.ConnectionStrings["OnlineJobPortalDB"].ConnectionString;

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    string query = "SELECT Username FROM [User] WHERE UserID = @UserID";
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@UserID", adminID);

                    connection.Open();
                    string username = command.ExecuteScalar() as string;

                    if (!string.IsNullOrEmpty(username))
                    {
                        lblAdminName.Text = username; // Display the admin's username
                    }
                    else
                    {
                        lblAdminName.Text = "User"; // Fallback if username not found
                    }
                }
            }
            else
            {
                lblAdminName.Text = "User"; // Default display if no admin ID in session
            }
        }

        protected void btnChangePassword_Click(object sender, EventArgs e)
        {
            string oldPassword = txtOldPassword.Text.Trim();
            string newPassword = txtNewPassword.Text.Trim();
            string confirmPassword = txtConfirmPassword.Text.Trim();

            if (newPassword != confirmPassword)
            {
                lblMessage.Text = "New passwords do not match.";
                return;
            }

            if (Session["AdminID"] == null)
            {
                lblMessage.Text = "User session has expired. Please log in again.";
                return;
            }

            string connectionString = ConfigurationManager.ConnectionStrings["OnlineJobPortalDB"].ConnectionString;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                int adminID = Convert.ToInt32(Session["AdminID"]);

                // Transaction
                using (SqlTransaction transaction = connection.BeginTransaction())
                {
                    try
                    {
                        // Verify the old password
                        string query = "SELECT Password FROM [User] WHERE UserID = @UserID";
                        SqlCommand command = new SqlCommand(query, connection, transaction);
                        command.Parameters.AddWithValue("@UserID", adminID);
                        string storedPassword = command.ExecuteScalar() as string;

                        // Here you should hash oldPassword before comparing it
                        if (storedPassword != oldPassword) // Replace with hashed comparison
                        {
                            lblMessage.Text = "Old password is incorrect.";
                            return;
                        }

                        // Update the password
                        string updateQuery = "UPDATE [User] SET Password = @NewPassword WHERE UserID = @UserID";
                        SqlCommand updateCommand = new SqlCommand(updateQuery, connection, transaction);
                        updateCommand.Parameters.AddWithValue("@NewPassword", newPassword); // Hash this as well
                        updateCommand.Parameters.AddWithValue("@UserID", adminID);

                        int rowsAffected = updateCommand.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            transaction.Commit();
                            lblMessage.Text = "Password changed successfully.";
                        }
                        else
                        {
                            lblMessage.Text = "Could not update password. Please try again.";
                        }
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                        lblMessage.Text = "An error occurred while changing your password. Please try again.";
                    }
                }
            }
        }

        protected void lnkViewProfile_Click(object sender, EventArgs e)
        {
            Response.Redirect("AdminProfile.aspx");
        }

        protected void lnkLogout_Click(object sender, EventArgs e)
        {
            Session.Abandon();
            Response.Redirect("AdminLogin.aspx");
        }
    }
}
