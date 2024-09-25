using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Web.UI;

namespace OnlineJobPortal
{
    public partial class EmployerLogin : Page
    {
        // Connection string from Web.config
        private string connectionString = ConfigurationManager.ConnectionStrings["OnlineJobPortalDB"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Clear any previous login session
                Session["EmployerID"] = null;
            }
        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            // Retrieve the entered email and password
            string email = txtEmail.Text.Trim();
            string password = txtPassword.Text.Trim();

            // Check if both fields are filled
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                lblMessage.Text = "Please enter both email and password.";
                return;
            }

            // Query to check if the credentials are correct and the user is an Employer
            string query = @"SELECT e.EmployerID, u.UserType 
                             FROM [User] u
                             INNER JOIN Employer e ON u.UserID = e.UserID
                             WHERE u.Email = @Email AND u.Password = @Password AND u.UserType = 'Employer'";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    // Set command timeout to 60 seconds
                    cmd.CommandTimeout = 60;  // Increase the timeout to 60 seconds

                    // Adding parameters to the SQL query
                    cmd.Parameters.AddWithValue("@Email", email);
                    cmd.Parameters.AddWithValue("@Password", password);  // Ensure password hashing if used

                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.HasRows)
                    {
                        reader.Read();

                        // Get the EmployerID
                        int employerID = Convert.ToInt32(reader["EmployerID"]);

                        // Store the EmployerID in session
                        Session["EmployerID"] = employerID;

                        // Redirect to the employer dashboard
                        Response.Redirect("EmployerDashboard.aspx");
                    }
                    else
                    {
                        // Display error message if login fails
                        lblMessage.Text = "Invalid email or password. Please try again.";
                    }
                }
            }
        }
    }
}
