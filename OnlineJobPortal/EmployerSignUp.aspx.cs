using System;
using System.Data.SqlClient;
using System.Configuration;

namespace OnlineJobPortal
{
    public partial class EmployerSignUp : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void BtnSignUp_Click(object sender, EventArgs e)
        {
            // Retrieve data from input fields
            string companyName = CompanyName.Text;
            string addressLine1 = AddressLine1Input.Text;
            string addressLine2 = AddressLine2Input.Text;
            string city = CityInput.Text;
            string state = StateInput.Text;
            string country = CountryInput.Text;
            string postalCode = PostalCodeInput.Text;
            string email = EmailInput.Text;
            string password = PasswordInput.Text; // Plain password
            string contactPerson = ContactPersonInput.Text;
            string contactNumber = ContactNumberInput.Text;

            string connectionString = ConfigurationManager.ConnectionStrings["OnlineJobPortalDB"].ConnectionString;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                // SQL query to insert into User table and then into Employer table
                string query = @"
                    INSERT INTO [User] (Username, Password, Email, UserType)
                    VALUES (@Username, @Password, @Email, 'Employer'); 
                    
                    DECLARE @UserID INT = SCOPE_IDENTITY();

                    INSERT INTO Employer (UserID, CompanyName, AddressLine1, AddressLine2, City, State, Country, PostalCode, Email, ContactPerson, ContactNumber)
                    VALUES (@UserID, @CompanyName, @AddressLine1, @AddressLine2, @City, @State, @Country, @PostalCode, @Email, @ContactPerson, @ContactNumber)";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Username", companyName); // Assuming company name is used as username
                    command.Parameters.AddWithValue("@Password", password); // Plain password
                    command.Parameters.AddWithValue("@Email", email);
                    command.Parameters.AddWithValue("@CompanyName", companyName);
                    command.Parameters.AddWithValue("@AddressLine1", addressLine1);
                    command.Parameters.AddWithValue("@AddressLine2", addressLine2);
                    command.Parameters.AddWithValue("@City", city);
                    command.Parameters.AddWithValue("@State", state);
                    command.Parameters.AddWithValue("@Country", country);
                    command.Parameters.AddWithValue("@PostalCode", postalCode);
                    command.Parameters.AddWithValue("@ContactPerson", contactPerson);
                    command.Parameters.AddWithValue("@ContactNumber", contactNumber);

                    try
                    {
                        connection.Open();
                        command.ExecuteNonQuery();
                    }
                    catch (SqlException ex)
                    {
                        // Handle specific SQL exceptions (e.g., duplicate email)
                        // Log the error or show a message to the user
                        // For example, you might redirect to an error page or show a message
                    }
                    finally
                    {
                        connection.Close();
                    }
                }
            }

            // Redirect to a success page or display a message
            Response.Redirect("EmployerDashboard.aspx");
        }
    }
}
