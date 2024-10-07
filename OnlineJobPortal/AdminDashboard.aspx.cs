using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Web.UI;

namespace OnlineJobPortal
{
    public partial class AdminDashboard : Page
    {
        // Connection string from web.config
        private string connectionString = ConfigurationManager.ConnectionStrings["OnlineJobPortalDB"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Load admin name and statistics on first load
                LoadAdminName();
                LoadDashboardStats();
            }
        }

        private void LoadAdminName()
        {
            // Assuming the admin's UserID is stored in session after login
            if (Session["AdminID"] != null)
            {
                int adminId = Convert.ToInt32(Session["AdminID"]);
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string query = "SELECT Username FROM [User] WHERE UserID = @UserID";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@UserID", adminId);
                        conn.Open();

                        // Execute the query and retrieve the username
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

        private void LoadDashboardStats()
        {
            // Replace these methods with actual data retrieval logic
            lblTotalUsers.Text = GetTotalUsers().ToString();
            lblTotalJobPostings.Text = GetTotalJobPostings().ToString();
            lblTotalApplications.Text = GetTotalApplications().ToString();
            lblPendingApplications.Text = GetPendingApplications().ToString();
        }

        private int GetTotalUsers()
        {
            // Your logic to get total users
            return 100; // Example static value
        }

        private int GetTotalJobPostings()
        {
            // Your logic to get total job postings
            return 50; // Example static value
        }

        private int GetTotalApplications()
        {
            // Your logic to get total applications
            return 75; // Example static value
        }

        private int GetPendingApplications()
        {
            // Your logic to get pending applications
            return 20; // Example static value
        }

        protected void lnkViewProfile_Click(object sender, EventArgs e)
        {
            // Redirect to profile page
            Response.Redirect("AdminProfile.aspx");
        }

        protected void lnkInbox_Click(object sender, EventArgs e)
        {
            Response.Redirect("ChangeAdminPassword.aspx");
        }

        protected void lnkLogout_Click(object sender, EventArgs e)
        {
            // Clear session and redirect to login page
            Session.Clear();
            Response.Redirect("AdminLogin.aspx");
        }

        protected void btnManageJobseeker_Click(object sender, EventArgs e)
        {
            // Redirect to Manage Jobseeker page
            Response.Redirect("ManageJobseeker.aspx");
        }
    }
}