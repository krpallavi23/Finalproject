using System;
using System.Data.SqlClient;
using System.Configuration;
using System.IO;

namespace OnlineJobPortal
{
    public partial class JobSeekerSignUp : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void BtnSignUp_Click(object sender, EventArgs e)
        {
            // Retrieve data from input fields
            string firstName = FirstNameInput.Text;
            string lastName = LastNameInput.Text;
            string dateOfBirth = DateOfBirthInput.Text;
            string gender = GenderInput.SelectedValue;
            string addressLine1 = AddressLine1Input.Text;
            string addressLine2 = AddressLine2Input.Text;
            string city = CityInput.Text;
            string state = StateInput.Text;
            string country = CountryInput.Text;
            string postalCode = PostalCodeInput.Text;
            string email = EmailInput.Text;
            string password = PasswordInput.Text; // Plain password
            int yearsOfExperience = int.TryParse(YearsOfExperienceInput.Text, out int experience) ? experience : 0;

            string connectionString = ConfigurationManager.ConnectionStrings["OnlineJobPortalDB"].ConnectionString;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                // SQL query to insert into User table and then into JobSeeker table
                string query = @"
                    INSERT INTO [User] (Username, Password, Email, UserType)
                    VALUES (@Username, @Password, @Email, 'JobSeeker'); 
                    
                    DECLARE @UserID INT = SCOPE_IDENTITY();

                    INSERT INTO JobSeeker (UserID, FirstName, LastName, DateOfBirth, Gender, AddressLine1, AddressLine2, City, State, Country, PostalCode, YearsOfExperience)
                    VALUES (@UserID, @FirstName, @LastName, @DateOfBirth, @Gender, @AddressLine1, @AddressLine2, @City, @State, @Country, @PostalCode, @YearsOfExperience)";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Username", firstName + " " + lastName); // Assuming full name is used as username
                    command.Parameters.AddWithValue("@Password", password); // Plain password
                    command.Parameters.AddWithValue("@Email", email);
                    command.Parameters.AddWithValue("@FirstName", firstName);
                    command.Parameters.AddWithValue("@LastName", lastName);
                    command.Parameters.AddWithValue("@DateOfBirth", dateOfBirth);
                    command.Parameters.AddWithValue("@Gender", gender);
                    command.Parameters.AddWithValue("@AddressLine1", addressLine1);
                    command.Parameters.AddWithValue("@AddressLine2", addressLine2);
                    command.Parameters.AddWithValue("@City", city);
                    command.Parameters.AddWithValue("@State", state);
                    command.Parameters.AddWithValue("@Country", country);
                    command.Parameters.AddWithValue("@PostalCode", postalCode);
                    command.Parameters.AddWithValue("@YearsOfExperience", yearsOfExperience);

                    try
                    {
                        connection.Open();
                        command.ExecuteNonQuery();
                    }
                    catch (SqlException ex)
                    {
                        // Handle specific SQL exceptions (e.g., duplicate email)
                        // Log the error or show a message to the user
                    }
                    finally
                    {
                        connection.Close();
                    }
                }
            }

            // Handle resume file upload
            if (ResumeUpload.HasFile)
            {
                string fileName = Path.GetFileName(ResumeUpload.PostedFile.FileName);
                string filePath = Server.MapPath("~/Resumes/") + fileName;
                ResumeUpload.SaveAs(filePath);

                // Update JobSeeker table with resume path
                UpdateResumePath(email, fileName);
            }

            // Redirect to a success page or display a message
            Response.Redirect("JobSeekerDashboard.aspx");
        }

        private void UpdateResumePath(string email, string resumeFileName)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["OnlineJobPortalDB"].ConnectionString;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = @"
                    UPDATE JobSeeker
                    SET ResumePath = @ResumePath
                    WHERE UserID = (SELECT UserID FROM [User] WHERE Email = @Email)";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ResumePath", "~/Resumes/" + resumeFileName);
                    command.Parameters.AddWithValue("@Email", email);

                    try
                    {
                        connection.Open();
                        command.ExecuteNonQuery();
                    }
                    catch (SqlException ex)
                    {
                        // Handle errors
                    }
                    finally
                    {
                        connection.Close();
                    }
                }
            }
        }
    }
}
