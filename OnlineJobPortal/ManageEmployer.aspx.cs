using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebGrease.Activities;

namespace OnlineJobPortal
{
    public partial class ManageEmployer : Page
    {
        private string connectionString = ConfigurationManager.ConnectionStrings["OnlineJobPortalDB"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadEmployerData();
                LoadAdminName();
            }
        }

        private void LoadEmployerData()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string query = @"
                        SELECT e.EmployerID, u.Username, e.CompanyName, e.Email, e.ContactPerson, e.ContactNumber 
                        FROM Employer e 
                        INNER JOIN [User] u ON e.UserID = u.UserID";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        DataTable dt = new DataTable();
                        da.Fill(dt);
                        GridViewEmployers.DataSource = dt;
                        GridViewEmployers.DataBind();
                    }
                }
            }
            catch (Exception ex)
            {
                // Log the error
            }
        }

        private void LoadAdminName()
        {
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

                        object result = cmd.ExecuteScalar();
                        lblAdminName.Text = result?.ToString() ?? "Admin"; // Fallback if user not found
                    }
                }
            }
            else
            {
                lblAdminName.Text = "Admin"; // Default display if no admin ID in session
            }
        }

        protected void GridViewEmployers_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                int employerId = Convert.ToInt32(GridViewEmployers.DataKeys[e.RowIndex].Value);

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    using (var transaction = conn.BeginTransaction())
                    {
                        try
                        {
                            // Check for references in other tables
                            string checkReferencesQuery = @"
                                SELECT COUNT(*) 
                                FROM JobPosting 
                                WHERE EmployerID = @EmployerID";

                            using (SqlCommand checkCmd = new SqlCommand(checkReferencesQuery, conn, transaction))
                            {
                                checkCmd.Parameters.AddWithValue("@EmployerID", employerId);
                                int count = (int)checkCmd.ExecuteScalar();

                                if (count > 0)
                                {
                                    // Inform the user that the employer cannot be deleted
                                    // This could be a label on the page to show a message
                                    lblError.Text = "Cannot delete this employer, as there are job postings associated with them.";
                                    return;
                                }
                            }

                            // Delete from User and Employer tables
                            string deleteQuery = @"
                                DELETE FROM [User] WHERE UserID = 
                                (SELECT UserID FROM Employer WHERE EmployerID = @EmployerID);
                                DELETE FROM Employer WHERE EmployerID = @EmployerID;";

                            using (SqlCommand cmd = new SqlCommand(deleteQuery, conn, transaction))
                            {
                                cmd.Parameters.AddWithValue("@EmployerID", employerId);
                                cmd.ExecuteNonQuery();
                            }

                            // Commit transaction
                            transaction.Commit();
                        }
                        catch (Exception)
                        {
                            transaction.Rollback();
                            throw; // Rethrow the exception to be caught by the outer catch
                        }
                    }
                }

                LoadEmployerData(); // Reload data after deletion
            }
            catch (Exception ex)
            {
                // Log the error and show a friendly message
                lblError.Text = "An error occurred while deleting the employer.";
            }
        }

        protected void lnkLogout_Click(object sender, EventArgs e)
        {
            Session.Clear();
            Response.Redirect("AdminLogin.aspx");
        }

        protected void lnkViewProfile_Click(object sender, EventArgs e)
        {
            Response.Redirect("AdminProfile.aspx");
        }
    }
}