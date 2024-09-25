using System;
using System.Data;
using System.Configuration;
using System.Data.SqlClient;
using System.Web.UI;

namespace OnlineJobPortal
{
    public partial class Home : Page
    {
        // Retrieve the connection string from Web.config
        private string connectionString = ConfigurationManager.ConnectionStrings["OnlineJobPortalDB"]?.ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (string.IsNullOrEmpty(connectionString))
                {
                    // Connection string is missing or incorrectly named
                    Response.Write("<script>alert('Database connection string is not configured properly.');</script>");
                    return;
                }

                PopulateRegionDropdown();
                PopulateJobTypeDropdown();
                LoadJobListings();
            }
        }

        // Event handler for Search button click
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            string jobTitle = txtJobTitle.Text.Trim();
            string region = ddlRegion.SelectedValue;
            string jobType = ddlJobType.SelectedValue;

            LoadJobListings(jobTitle, region, jobType);
        }

        // Method to populate the Region dropdown from the database
        private void PopulateRegionDropdown()
        {
            string query = "SELECT DISTINCT Location FROM JobPosting WHERE Status = 'Active' ORDER BY Location";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    try
                    {
                        conn.Open();
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            ddlRegion.DataSource = reader;
                            ddlRegion.DataTextField = "Location";
                            ddlRegion.DataValueField = "Location";
                            ddlRegion.DataBind();
                        }
                        // Insert default item at the top
                        ddlRegion.Items.Insert(0, new System.Web.UI.WebControls.ListItem("Select Region", ""));
                    }
                    catch (Exception ex)
                    {
                        // Handle exception (e.g., log error)
                        Response.Write($"<script>alert('Error loading regions: {ex.Message}');</script>");
                    }
                }
            }
        }

        // Method to populate the Job Type dropdown from the database
        private void PopulateJobTypeDropdown()
        {
            string query = "SELECT DISTINCT JobType FROM JobPosting WHERE Status = 'Active' ORDER BY JobType";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    try
                    {
                        conn.Open();
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            ddlJobType.DataSource = reader;
                            ddlJobType.DataTextField = "JobType";
                            ddlJobType.DataValueField = "JobType";
                            ddlJobType.DataBind();
                        }
                        // Insert default item at the top
                        ddlJobType.Items.Insert(0, new System.Web.UI.WebControls.ListItem("Select Job Type", ""));
                    }
                    catch (Exception ex)
                    {
                        // Handle exception (e.g., log error)
                        Response.Write($"<script>alert('Error loading job types: {ex.Message}');</script>");
                    }
                }
            }
        }

        // Method to load job listings, optionally with search filters
        private void LoadJobListings(string jobTitle = "", string region = "", string jobType = "")
        {
            DataTable dt = new DataTable();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                // Build the query with optional filters
                string query = @"SELECT JP.JobID, JP.Title, E.CompanyName, JP.Location, JP.JobType,
                                        CASE 
                                            WHEN JP.JobType = 'Full-Time' THEN 'success'
                                            WHEN JP.JobType = 'Part-Time' THEN 'danger'
                                            WHEN JP.JobType = 'Contract' THEN 'primary'
                                            WHEN JP.JobType = 'Freelance' THEN 'warning'
                                            ELSE 'secondary'
                                        END AS JobTypeBadgeClass
                                 FROM JobPosting JP
                                 INNER JOIN Employer E ON JP.EmployerID = E.EmployerID
                                 WHERE JP.Status = 'Active'";

                // Add filters based on user input
                if (!string.IsNullOrEmpty(jobTitle))
                {
                    query += " AND JP.Title LIKE @JobTitle";
                }

                if (!string.IsNullOrEmpty(region))
                {
                    query += " AND JP.Location = @Region";
                }

                if (!string.IsNullOrEmpty(jobType))
                {
                    query += " AND JP.JobType = @JobType";
                }

                query += " ORDER BY JP.ApplicationDeadline DESC";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    // Add parameters to prevent SQL Injection
                    if (!string.IsNullOrEmpty(jobTitle))
                    {
                        cmd.Parameters.AddWithValue("@JobTitle", "%" + jobTitle + "%");
                    }

                    if (!string.IsNullOrEmpty(region))
                    {
                        cmd.Parameters.AddWithValue("@Region", region);
                    }

                    if (!string.IsNullOrEmpty(jobType))
                    {
                        cmd.Parameters.AddWithValue("@JobType", jobType);
                    }

                    try
                    {
                        conn.Open();
                        using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                        {
                            da.Fill(dt);
                        }
                    }
                    catch (Exception ex)
                    {
                        // Handle exception (e.g., log error)
                        Response.Write($"<script>alert('Error loading job listings: {ex.Message}');</script>");
                        return;
                    }
                }
            }

            // Bind the DataTable to the Repeater
            rptJobListings.DataSource = dt;
            rptJobListings.DataBind();
        }
    }
}
