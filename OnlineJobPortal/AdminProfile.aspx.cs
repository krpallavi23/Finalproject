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
        private string connectionString = ConfigurationManager.ConnectionStrings["OnlineJobPortalDB"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadAdminData();
            }
        }

        private void LoadAdminData()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "SELECT * FROM [User] WHERE UserType = 'Admin'";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    GridViewAdmins.DataSource = dt;
                    GridViewAdmins.DataBind();
                }
            }
        }

        protected void GridViewAdmins_RowEditing(object sender, GridViewEditEventArgs e)
        {
            GridViewAdmins.EditIndex = e.NewEditIndex;
            LoadAdminData();
        }

        protected void GridViewAdmins_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            GridViewAdmins.EditIndex = -1;
            LoadAdminData();
        }

        protected void GridViewAdmins_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            // Retrieve the UserID from DataKeys
            int userId = Convert.ToInt32(GridViewAdmins.DataKeys[e.RowIndex].Value);

            // Ensure the correct controls are accessed
            string username = ((TextBox)GridViewAdmins.Rows[e.RowIndex].FindControl("TextBoxUsername")).Text;
            string email = ((TextBox)GridViewAdmins.Rows[e.RowIndex].FindControl("TextBoxEmail")).Text;

            // Assume UserType is a DropDownList; make sure to define it in the GridView markup
            string userType = ((DropDownList)GridViewAdmins.Rows[e.RowIndex].FindControl("UserTypeDropDown")).SelectedValue;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "UPDATE [User] SET Username = @Username, Email = @Email, UserType = @UserType WHERE UserID = @UserID";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Username", username);
                    cmd.Parameters.AddWithValue("@Email", email);
                    cmd.Parameters.AddWithValue("@UserType", userType);
                    cmd.Parameters.AddWithValue("@UserID", userId);

                    conn.Open();
                    cmd.ExecuteNonQuery();
                    conn.Close();
                }
            }

            GridViewAdmins.EditIndex = -1;
            LoadAdminData(); // Refresh the GridView data
        }

        protected void lnkViewDashboard_Click(object sender, EventArgs e)
        {
            Response.Redirect("AdminDashboard.aspx");
        }


        protected void lnkLogout_Click(object sender, EventArgs e)
        {
            Session.Clear();
            Response.Redirect("AdminLogin.aspx");
        }
    }
}
