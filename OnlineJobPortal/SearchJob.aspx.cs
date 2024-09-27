using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Collections.Generic;
using System.Web.UI.WebControls;

namespace OnlineJobPortal
{
    public partial class SearchJob : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Load all job listings initially
                LoadJobListings(string.Empty, string.Empty, string.Empty);
                 // Load job categories into dropdown
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            // Get the input values from the user
            string keyword = txtKeyword.Text.Trim();
            string location = ddlLocation.SelectedValue;
            string jobType = ddlJobType.SelectedValue;
            // Get selected job category

            // Load job listings based on the search criteria
            LoadJobListings(keyword, location, jobType);
        }

        private void LoadJobListings(string keyword, string location, string jobType)
        {
            // Base query for fetching job listings
            string query = "SELECT jp.JobID, jp.Title, e.CompanyName, jp.Location, jp.JobType, jp.JobCategory " +
                           "FROM JobPosting jp " +
                           "JOIN Employer e ON jp.EmployerID = e.EmployerID " + // Ensure correct foreign key reference
                           "WHERE jp.Status = 'Active'";

            List<string> filters = new List<string>();

            // Add filters based on user input
            if (!string.IsNullOrWhiteSpace(keyword))
            {
                filters.Add("jp.Title LIKE @keyword");
            }
            if (!string.IsNullOrWhiteSpace(location))
            {
                filters.Add("jp.Location = @location");
            }
            if (!string.IsNullOrWhiteSpace(jobType))
            {
                filters.Add("jp.JobType = @jobType");
            }

            // Append filters to the query if any
            if (filters.Count > 0)
            {
                query += " AND " + string.Join(" AND ", filters);
            }

            // Execute the query and bind results to the GridView
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["OnlineJobPortalDB"].ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    // Add parameters to the command
                    if (!string.IsNullOrWhiteSpace(keyword))
                    {
                        cmd.Parameters.AddWithValue("@keyword", "%" + keyword + "%");
                    }
                    if (!string.IsNullOrWhiteSpace(location))
                    {
                        cmd.Parameters.AddWithValue("@location", location);
                    }
                    if (!string.IsNullOrWhiteSpace(jobType))
                    {
                        cmd.Parameters.AddWithValue("@jobType", jobType);
                    }
                  

                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        DataTable dt = new DataTable();
                        dt.Load(reader);
                        gvJobListings.DataSource = dt;
                        gvJobListings.DataBind();
                    }
                }
            }

            // Update the status label with the number of results found
            lblStatus.Text = $"Found {gvJobListings.Rows.Count} job(s) matching your criteria.";
        }

     

        protected void gvJobListings_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvJobListings.PageIndex = e.NewPageIndex;
            btnSearch_Click(sender, e); // Re-load listings on page change
        }

        // Redirect to the profile page
        protected void lnkViewProfile_Click(object sender, EventArgs e)
        {
            Response.Redirect("Profile.aspx"); // Update with your profile page URL
        }

        // Redirect to the inbox page
        protected void lnkInbox_Click(object sender, EventArgs e)
        {
            Response.Redirect("Inbox.aspx"); // Update with your inbox page URL
        }

        // Clear session and redirect to login page
        protected void lnkLogout_Click(object sender, EventArgs e)
        {
            Session.Clear();
            Response.Redirect("Login.aspx"); // Update with your login page URL
        }

        // Apply for the job
        protected void btnApply_Click(object sender, EventArgs e)
        {
            // Get the JobID from the CommandArgument
            Button btnApply = (Button)sender;
            string jobId = btnApply.CommandArgument;

            // Redirect to the job application page (update this with your actual page)
            Response.Redirect($"JobApplication.aspx?JobID={jobId}");
        }
    }
}
