using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Web.UI;

namespace OnlineJobPortal
{
    public partial class JobSeekerProfile : Page
    {
        // Connection string from Web.config
        private string connectionString = ConfigurationManager.ConnectionStrings["OnlineJobPortalDB"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Check if the JobSeekerID session variable is set
                if (Session["JobSeekerID"] == null)
                {
                    Response.Redirect("JobSeekerLogin.aspx");
                }
                else
                {
                    LoadJobSeekerProfile(); // Load the profile if the user is logged in
                }
            }
        }

        private void LoadJobSeekerProfile()
        {
            int jobSeekerID = Convert.ToInt32(Session["JobSeekerID"]);

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "SELECT FirstName, LastName, DateOfBirth, Gender, YearsOfExperience, AddressLine1, AddressLine2, City, State, Country, PostalCode FROM JobSeeker WHERE JobSeekerID = @JobSeekerID";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@JobSeekerID", jobSeekerID);
                    conn.Open();

                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        txtFirstName.Text = reader["FirstName"].ToString();
                        txtLastName.Text = reader["LastName"].ToString();
                        txtDateOfBirth.Text = Convert.ToDateTime(reader["DateOfBirth"]).ToString("yyyy-MM-dd");
                        ddlGender.SelectedValue = reader["Gender"].ToString();
                        txtExperience.Text = reader["YearsOfExperience"].ToString(); // Updated to match the column name
                        txtAddressLine1.Text = reader["AddressLine1"].ToString();
                        txtAddressLine2.Text = reader["AddressLine2"].ToString();
                        txtCity.Text = reader["City"].ToString();
                        txtState.Text = reader["State"].ToString();
                        txtCountry.Text = reader["Country"].ToString();
                        txtPostalCode.Text = reader["PostalCode"].ToString();
                    }
                    else
                    {
                        lblMessage.Text = "Profile not found.";
                        lblMessage.Visible = true;
                    }
                    reader.Close();
                }
            }
        }

        protected void btnUpdateProfile_Click(object sender, EventArgs e)
        {
            int jobSeekerID = Convert.ToInt32(Session["JobSeekerID"]);

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "UPDATE JobSeeker SET FirstName = @FirstName, LastName = @LastName, DateOfBirth = @DateOfBirth, Gender = @Gender, YearsOfExperience = @YearsOfExperience, AddressLine1 = @AddressLine1, AddressLine2 = @AddressLine2, City = @City, State = @State, Country = @Country, PostalCode = @PostalCode WHERE JobSeekerID = @JobSeekerID";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@FirstName", txtFirstName.Text.Trim());
                    cmd.Parameters.AddWithValue("@LastName", txtLastName.Text.Trim());
                    cmd.Parameters.AddWithValue("@DateOfBirth", Convert.ToDateTime(txtDateOfBirth.Text.Trim()));
                    cmd.Parameters.AddWithValue("@Gender", ddlGender.SelectedValue);
                    cmd.Parameters.AddWithValue("@YearsOfExperience", Convert.ToInt32(txtExperience.Text.Trim())); // Updated to match the column name
                    cmd.Parameters.AddWithValue("@AddressLine1", txtAddressLine1.Text.Trim());
                    cmd.Parameters.AddWithValue("@AddressLine2", txtAddressLine2.Text.Trim());
                    cmd.Parameters.AddWithValue("@City", txtCity.Text.Trim());
                    cmd.Parameters.AddWithValue("@State", txtState.Text.Trim());
                    cmd.Parameters.AddWithValue("@Country", txtCountry.Text.Trim());
                    cmd.Parameters.AddWithValue("@PostalCode", txtPostalCode.Text.Trim());
                    cmd.Parameters.AddWithValue("@JobSeekerID", jobSeekerID);

                    conn.Open();
                    int rowsAffected = cmd.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        lblMessage.Text = "Profile updated successfully.";
                        lblMessage.CssClass = "text-success";
                    }
                    else
                    {
                        lblMessage.Text = "Profile update failed.";
                        lblMessage.CssClass = "text-danger";
                    }
                    lblMessage.Visible = true;
                }
            }
        }

        protected void lnkViewDashboard_Click(object sender, EventArgs e)
        {
            Response.Redirect("JobSeekerDashboard.aspx"); // Redirect to the dashboard
        }

        protected void lnkLogout_Click(object sender, EventArgs e)
        {
            Session.Clear(); // Clear the session
            Response.Redirect("JobSeekerLogin.aspx"); // Redirect to the login page
        }
    }
}
