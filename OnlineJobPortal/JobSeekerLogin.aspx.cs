using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace OnlineJobPortal
{
    public partial class JobSeekerLogin : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // Any code to run on page load
        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            // Implement login logic here
            string email = txtEmail.Text;
            string password = txtPassword.Text;

            // Example logic: Validate user credentials
            if (IsValidUser(email, password)) // Replace with your validation logic
            {
                // Redirect to another page or show a success message
                Response.Redirect("JobSeekerDashbord.aspx"); // Redirect to a dashboard or home page
            }
            else
            {
                lblMessage.Text = "Invalid email or password.";
            }
        }

        private bool IsValidUser(string email, string password)
        {
            // Replace this with your actual user validation logic
            // For example, check against a database
            // Here we'll use a simple hardcoded check for demonstration
            return email == "test@example.com" && password == "password123"; // Example credentials
        }
    }
}
