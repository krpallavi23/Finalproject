using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Web.UI;

namespace OnlineJobPortal
{
    public partial class AdminLogin : Page
    {
        // Connection string from Web.config
        private string connectionString = ConfigurationManager.ConnectionStrings["OnlineJobPortalDB"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Clear any previous login session
                Session["AdminID"] = null;
            }
        }

        protected void btnAdminLogin_Click(object sender, EventArgs e)
        {
            // Retrieve the entered email and password
            string email = txtAdminEmail.Text.Trim();
            string password = txtAdminPassword.Text.Trim();

            // Check if both fields are filled
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                lblMessage.Text = "Please enter both email and password.";
                return;
            }

            // Query to check if the credentials are correct and the user is an Admin
            string query = @"SELECT UserID, Password 
                             FROM [User] 
                             WHERE Email = @Email AND UserType = 'Admin'";

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        // Set command timeout to 60 seconds
                        cmd.CommandTimeout = 60;

                        // Adding parameters to the SQL query
                        cmd.Parameters.AddWithValue("@Email", email);

                        conn.Open();
                        SqlDataReader reader = cmd.ExecuteReader();

                        if (reader.HasRows)
                        {
                            reader.Read();

                            // Get the stored password
                            string storedPassword = reader["Password"].ToString();

                            // Compare the entered password with the stored password
                            if (password == storedPassword)
                            {
                                // Get the UserID
                                int userID = Convert.ToInt32(reader["UserID"]);

                                // Store the UserID in session
                                Session["AdminID"] = userID;

                                // Redirect to the admin dashboard
                                Response.Redirect("AdminDashboard.aspx");
                            }
                            else
                            {
                                lblMessage.Text = "Invalid email or password. Please try again.";
                            }
                        }
                        else
                        {
                            // Display error message if login fails
                            lblMessage.Text = "Invalid email or password. Please try again.";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = "An error occurred: " + ex.Message; // For debugging purposes
                // Optionally log the error
            }
        }
    }
}
