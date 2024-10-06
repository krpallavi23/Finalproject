using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Web.UI;

namespace OnlineJobPortal
{
    public partial class ChangeJobSeekerPassword : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadJobSeekerName();
            }
        }

        // Method to load the Job Seeker's name based on JobSeekerID from session
        private void LoadJobSeekerName()
        {
            int jobSeekerID = Convert.ToInt32(Session["JobSeekerID"]); // Assuming JobSeekerID is stored in the session

            // Your connection string (adjust if needed)
            string connectionString = ConfigurationManager.ConnectionStrings["OnlineJobPortalDB"].ConnectionString;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT FirstName, LastName FROM JobSeeker WHERE JobSeekerID = @JobSeekerID";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@JobSeekerID", jobSeekerID);

                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    // Fetch the FirstName and LastName from the database
                    string firstName = reader["FirstName"].ToString();
                    string lastName = reader["LastName"].ToString();

                    // Set the label to display the full name
                    lblJobSeekerName.Text = $"{firstName} {lastName}";
                }

                reader.Close();
            }
        }

        // Change Password logic
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

            string connectionString = ConfigurationManager.ConnectionStrings["OnlineJobPortalDB"].ConnectionString;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                // Get the UserID of the currently logged-in JobSeeker
                int jobSeekerID = Convert.ToInt32(Session["JobSeekerID"]);

                // Step 1: Get UserID from JobSeeker table
                string userIdQuery = "SELECT UserID FROM JobSeeker WHERE JobSeekerID = @JobSeekerID";
                SqlCommand userIdCommand = new SqlCommand(userIdQuery, connection);
                userIdCommand.Parameters.AddWithValue("@JobSeekerID", jobSeekerID);

                int userID = (int)(userIdCommand.ExecuteScalar() ?? 0);

                // Step 2: Verify the old password
                string query = "SELECT Password FROM [User] WHERE UserID = @UserID";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@UserID", userID);

                string storedPassword = command.ExecuteScalar() as string;

                // Step 3: Check if the old password is correct
                if (storedPassword != oldPassword)
                {
                    lblMessage.Text = "Old password is incorrect.";
                    return;
                }

                // Step 4: Update the password
                string updateQuery = "UPDATE [User] SET Password = @NewPassword WHERE UserID = @UserID";
                SqlCommand updateCommand = new SqlCommand(updateQuery, connection);
                updateCommand.Parameters.AddWithValue("@NewPassword", newPassword);
                updateCommand.Parameters.AddWithValue("@UserID", userID);

                // Step 5: Execute the update
                int rowsAffected = updateCommand.ExecuteNonQuery();

                if (rowsAffected > 0)
                {
                    lblMessage.Text = "Password changed successfully.";
                }
                else
                {
                    lblMessage.Text = "Error updating password.";
                }
            }
        }

        // Link for viewing the profile
        protected void lnkViewProfile_Click(object sender, EventArgs e)
        {
            Response.Redirect("JobSeekerProfile.aspx");
        }

        // Link for logging out
        protected void lnkLogout_Click(object sender, EventArgs e)
        {
            Session.Abandon();
            Response.Redirect("JobSeekerLogin.aspx");
        }
    }
}
