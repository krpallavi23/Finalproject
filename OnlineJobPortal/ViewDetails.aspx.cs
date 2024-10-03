using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace OnlineJobPortal
{
    public partial class ViewDetails : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                lblMessage.Visible = false;
                // Check if JobID is provided in the query string
                if (Request.QueryString["JobID"] != null)
                {
                    int jobId;
                    if (int.TryParse(Request.QueryString["JobID"], out jobId))
                    {
                        LoadJobDetails(jobId);
                    }
                    else
                    {
                        lblTitle.Text = "Invalid Job ID.";
                    }
                }
                else
                {
                    lblTitle.Text = "Job ID is required.";
                }
            }
        }

        private void LoadJobDetails(int jobId)
        {
            string query = "SELECT jp.Title, e.CompanyName, jp.Location, jp.Salary, jp.JobType, jp.Description, jp.ApplicationDeadline " +
                           "FROM JobPosting jp " +
                           "JOIN Employer e ON jp.EmployerID = e.EmployerID " +
                           "WHERE jp.JobID = @jobId AND jp.Status = 'Active'";

            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["OnlineJobPortalDB"].ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@jobId", jobId);
                    conn.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            lblTitle.Text = reader["Title"].ToString();
                            lblCompany.Text = reader["CompanyName"].ToString();
                            lblLocation.Text = reader["Location"].ToString();
                            lblSalary.Text = "$" + reader["Salary"].ToString();
                            lblJobType.Text = reader["JobType"].ToString();
                            lblDescription.Text = reader["Description"].ToString();
                            lblDeadline.Text = "Application Deadline: " + Convert.ToDateTime(reader["ApplicationDeadline"]).ToShortDateString();
                        }
                        else
                        {
                            lblTitle.Text = "Job not found or is no longer active.";
                        }
                    }
                }
            }
        }

        protected void btnApply_Click(object sender, EventArgs e)
        {
            // Get the JobID from the query string (should already be available)
            lblMessage.Text = "Applied successfully!";
            lblMessage.Visible = true;
        }

        // Redirect to the profile page
        protected void lnkViewProfile_Click(object sender, EventArgs e)
        {
            Response.Redirect("JobSeekerProfile.aspx"); // Update with your profile page URL
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
            Response.Redirect("JobSeekerLogin.aspx"); // Update with your login page URL
        }
    }
}