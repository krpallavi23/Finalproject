
using System;
using System.Web.UI;

namespace OnlineJobPortal
{
    public partial class AdminDashboard : Page
    {
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
            // Assume you get the admin's name from session or database
            lblAdminName.Text = "Admin"; // Replace with actual admin name retrieval
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