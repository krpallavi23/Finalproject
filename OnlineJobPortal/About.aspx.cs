using System;
using System.Web.UI;

namespace OnlineJobPortal
{
    public partial class About : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // This is where you can load any dynamic content if needed.
                LoadTeamMembers();
                LoadJobBoardStats();
            }
        }

        private void LoadTeamMembers()
        {
            // This method can be used to load team members from a database or other source
            // For demonstration, we're just going to use static data

            // Example: Retrieve and bind team member data to the UI elements
            // You can replace this with actual data retrieval logic
        }

        private void LoadJobBoardStats()
        {
            // This method can be used to load job board statistics from a database
            // Example: Retrieve stats and update the UI elements accordingly
        }
    }
}
