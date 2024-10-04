using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;

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
                // Log the error and show a friendly message
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
                    string deleteEmployerQuery = "DELETE FROM Employer WHERE EmployerID = @EmployerID; DELETE FROM [User] WHERE UserID = (SELECT UserID FROM Employer WHERE EmployerID = @EmployerID)";
                    using (SqlCommand cmd = new SqlCommand(deleteEmployerQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@EmployerID", employerId);
                        cmd.ExecuteNonQuery();
                    }
                }
                LoadEmployerData(); // Reload data after deletion
            }
            catch (Exception ex)
            {
                // Log the error and show a friendly message
            }
        }

        protected void lnkLogout_Click(object sender, EventArgs e)
        {
            Session.Clear();
            Response.Redirect("AdminLogin.aspx");
        }
    }
}
