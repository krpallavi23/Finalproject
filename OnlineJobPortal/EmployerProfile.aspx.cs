using System;
using System.Data.SqlClient;
using System.Configuration;

namespace OnlineJobPortal
{
    public partial class EmployerProfile : System.Web.UI.Page
    {
        // Database connection string from Web.config
        private string connectionString = ConfigurationManager.ConnectionStrings["OnlineJobPortalDB"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Fetch employer profile data when the page is loaded for the first time
                LoadEmployerProfile();
            }
        }

        private void LoadEmployerProfile()
        {
            // Retrieve employerID from session
            if (Session["EmployerID"] != null)
            {
                int employerID = Convert.ToInt32(Session["EmployerID"]);

                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    string query = "SELECT * FROM Employer WHERE EmployerID = @EmployerID";
                    SqlCommand cmd = new SqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@EmployerID", employerID);

                    try
                    {
                        con.Open();
                        SqlDataReader reader = cmd.ExecuteReader();
                        if (reader.Read())
                        {
                            // Populate the fields with data from the database
                            txtCompanyName.Text = reader["CompanyName"].ToString();
                            txtContactPerson.Text = reader["ContactPerson"].ToString();
                            txtEmail.Text = reader["Email"].ToString();
                            txtContactNumber.Text = reader["ContactNumber"].ToString();
                            txtWebsite.Text = reader["Website"].ToString();
                            txtAddressLine1.Text = reader["AddressLine1"].ToString();
                            txtAddressLine2.Text = reader["AddressLine2"].ToString();
                            txtCity.Text = reader["City"].ToString();
                            txtState.Text = reader["State"].ToString();
                            txtCountry.Text = reader["Country"].ToString();
                            txtPostalCode.Text = reader["PostalCode"].ToString();

                            // Set the employer name to the label
                            lblEmployerName.Text = reader["CompanyName"].ToString(); // Assuming CompanyName is the employer's name
                        }
                    }
                    catch (Exception ex)
                    {
                        lblMessage.Text = "Error loading profile: " + ex.Message;
                        lblMessage.Visible = true;
                    }
                }
            }
            else
            {
                // Redirect to login page if EmployerID is not found in session
                Response.Redirect("Home.aspx");
            }
        }

        protected void btnUpdateProfile_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                UpdateEmployerProfile();
            }
        }

        private void UpdateEmployerProfile()
        {
            // Retrieve employerID from session
            if (Session["EmployerID"] != null)
            {
                int employerID = Convert.ToInt32(Session["EmployerID"]);

                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    string query = @"UPDATE Employer
                                    SET CompanyName = @CompanyName,
                                        ContactPerson = @ContactPerson,
                                        Email = @Email,
                                        ContactNumber = @ContactNumber,
                                        Website = @Website,
                                        AddressLine1 = @AddressLine1,
                                        AddressLine2 = @AddressLine2,
                                        City = @City,
                                        State = @State,
                                        Country = @Country,
                                        PostalCode = @PostalCode
                                    WHERE EmployerID = @EmployerID";

                    SqlCommand cmd = new SqlCommand(query, con);

                    // Adding parameters to prevent SQL Injection
                    cmd.Parameters.AddWithValue("@CompanyName", txtCompanyName.Text);
                    cmd.Parameters.AddWithValue("@ContactPerson", txtContactPerson.Text);
                    cmd.Parameters.AddWithValue("@Email", txtEmail.Text);
                    cmd.Parameters.AddWithValue("@ContactNumber", txtContactNumber.Text);
                    cmd.Parameters.AddWithValue("@Website", txtWebsite.Text);
                    cmd.Parameters.AddWithValue("@AddressLine1", txtAddressLine1.Text);
                    cmd.Parameters.AddWithValue("@AddressLine2", txtAddressLine2.Text);
                    cmd.Parameters.AddWithValue("@City", txtCity.Text);
                    cmd.Parameters.AddWithValue("@State", txtState.Text);
                    cmd.Parameters.AddWithValue("@Country", txtCountry.Text);
                    cmd.Parameters.AddWithValue("@PostalCode", txtPostalCode.Text);
                    cmd.Parameters.AddWithValue("@EmployerID", employerID);

                    try
                    {
                        con.Open();
                        int rowsAffected = cmd.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            lblMessage.Text = "Profile updated successfully!";
                            lblMessage.CssClass = "text-success";
                        }
                        else
                        {
                            lblMessage.Text = "Error updating profile.";
                            lblMessage.CssClass = "text-danger";
                        }
                        lblMessage.Visible = true;
                    }
                    catch (Exception ex)
                    {
                        lblMessage.Text = "Error updating profile: " + ex.Message;
                        lblMessage.CssClass = "text-danger";
                        lblMessage.Visible = true;
                    }
                }
            }
            else
            {
                // Redirect to login page if EmployerID is not found in session
                Response.Redirect("Home.aspx");
            }
        }
        /// </summary>
        protected void lnkViewProfile_Click(object sender, EventArgs e)
        {
            Response.Redirect("EmployerProfile.aspx");
        }

        /// <summary>
        /// Handles the click event for accessing the inbox.
        /// </summary>
        protected void lnkInbox_Click(object sender, EventArgs e)
        {
            Response.Redirect("ChangeEmployerPassword.aspx");
        }

        /// <summary>
        /// Handles the click event for logging out the employer.
        /// </summary>
        

        protected void lnkViewDashboard_Click(object sender, EventArgs e)
        {
            // Redirect to Employer Dashboard
            Response.Redirect("EmployerDashboard.aspx");
        }

        protected void lnkLogout_Click(object sender, EventArgs e)
        {
            // Implement Logout logic (e.g., clearing session)
            Session.Clear(); // Clear session data
            Response.Redirect("EmployerLogin.aspx");
        }
    }
}
