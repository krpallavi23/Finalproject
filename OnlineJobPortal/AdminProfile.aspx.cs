using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace OnlineJobPortal
{
    public partial class AdminProfile : Page
    {
        // Connection string from web.config
        private string connectionString = ConfigurationManager.ConnectionStrings["OnlineJobPortalDB"].ConnectionString;

        // Page Load event - Load admin data when the page is first loaded
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadAdminData();  // Load admin data to GridView on first load
            }
        }

        // Load admin data from the database into GridView
        private void LoadAdminData()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "SELECT * FROM [User] WHERE UserType = 'Admin'";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);  // Fill DataTable with result from database
                    GridViewAdmins.DataSource = dt;  // Bind DataTable to GridView
                    GridViewAdmins.DataBind();  // Re-bind GridView
                }
            }
        }

        // Row editing event - puts the GridView in edit mode for the selected row
        protected void GridViewAdmins_RowEditing(object sender, GridViewEditEventArgs e)
        {
            GridViewAdmins.EditIndex = e.NewEditIndex;  // Set the index of the row to be edited
            LoadAdminData();  // Reload the GridView to show edit controls
        }

        // Row canceling edit event - cancels edit mode for the GridView
        protected void GridViewAdmins_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            GridViewAdmins.EditIndex = -1;  // Exit edit mode
            LoadAdminData();  // Reload data to revert back to normal mode
        }

        // Row updating event - called when a row is updated
        protected void GridViewAdmins_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            // Retrieve the UserID from the DataKeys collection
            int userId = Convert.ToInt32(GridViewAdmins.DataKeys[e.RowIndex].Value);

            // Get the new values from the textboxes/dropdownlist in the edited row
            string username = ((TextBox)GridViewAdmins.Rows[e.RowIndex].FindControl("TextBoxUsername")).Text;
            string email = ((TextBox)GridViewAdmins.Rows[e.RowIndex].FindControl("TextBoxEmail")).Text;
            string userType = ((DropDownList)GridViewAdmins.Rows[e.RowIndex].FindControl("UserTypeDropDown")).SelectedValue;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                // SQL update query to modify the admin's information
                string query = "UPDATE [User] SET Username = @Username, Email = @Email, UserType = @UserType WHERE UserID = @UserID";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    // Add parameters to avoid SQL injection
                    cmd.Parameters.AddWithValue("@Username", username);
                    cmd.Parameters.AddWithValue("@Email", email);
                    cmd.Parameters.AddWithValue("@UserType", userType);
                    cmd.Parameters.AddWithValue("@UserID", userId);

                    conn.Open();  // Open connection to the database
                    cmd.ExecuteNonQuery();  // Execute the update query
                    conn.Close();  // Close the connection
                }
            }

            GridViewAdmins.EditIndex = -1;  // Exit edit mode
            LoadAdminData();  // Reload data to reflect the changes
        }

        // Event handler for viewing the dashboard
        protected void lnkViewDashboard_Click(object sender, EventArgs e)
        {
            Response.Redirect("AdminDashboard.aspx");  // Redirect to the dashboard page
        }

        // Event handler for logging out the admin
        protected void lnkLogout_Click(object sender, EventArgs e)
        {
            Session.Clear();  // Clear the session to log out
            Response.Redirect("AdminLogin.aspx");  // Redirect to the login page
        }
    }
}
