using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace OnlineJobPortal
{
    public partial class ChangeEmployerPassword : Page
    {
        private string connectionString = ConfigurationManager.ConnectionStrings["OnlineJobPortalDB"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Authenticate Employer
                if (Session["EmployerID"] == null)
                {
                    Response.Redirect("EmployerLogin.aspx");
                }
                else
                {
                    LoadEmployerDetails();
                    LoadJobSeekers(); // Load job seekers for the chat dropdown
                    LoadChatMessages(); // Load existing chat messages
                }
            }
        }

        /// <summary>
        /// Loads employer details such as company name.
        /// </summary>
        private void LoadEmployerDetails()
        {
            if (Session["EmployerID"] == null)
            {
                lblMessage.Text = "User session expired. Please log in again.";
                Response.Redirect("EmployerLogin.aspx");
                return;
            }

            // Safely parse EmployerID from session
            if (!int.TryParse(Session["EmployerID"].ToString(), out int employerID))
            {
                lblMessage.Text = "Invalid session data. Please log in again.";
                Response.Redirect("EmployerLogin.aspx");
                return;
            }

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "SELECT CompanyName FROM Employer WHERE EmployerID = @EmployerID";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@EmployerID", employerID);

                    try
                    {
                        conn.Open();
                        SqlDataReader reader = cmd.ExecuteReader();
                        if (reader.Read())
                        {
                            lblEmployerName.Text = reader["CompanyName"].ToString();
                        }
                        else
                        {
                            lblEmployerName.Text = "Company";
                        }
                        reader.Close();
                    }
                    catch (SqlException sqlEx)
                    {
                        // Log the exception details (implement logging as per your project standards)
                        System.Diagnostics.Trace.TraceError($"SQL Error in LoadEmployerDetails: {sqlEx.Message}");
                        lblMessage.Text = "A database error occurred while loading your details.";
                    }
                    catch (Exception ex)
                    {
                        // Log the exception details
                        System.Diagnostics.Trace.TraceError($"Error in LoadEmployerDetails: {ex.Message}");
                        lblMessage.Text = "An unexpected error occurred while loading your details.";
                    }
                }
            }
        }

        /// <summary>
        /// Loads job seekers into the dropdown list for the chat functionality.
        /// </summary>
        private void LoadJobSeekers()
        {
            if (Session["EmployerID"] == null)
            {
                lblMessage.Text = "User session expired. Please log in again.";
                Response.Redirect("EmployerLogin.aspx");
                return;
            }

            // Safely parse EmployerID from session
            if (!int.TryParse(Session["EmployerID"].ToString(), out int currentEmployerID))
            {
                lblMessage.Text = "Invalid session data. Please log in again.";
                Response.Redirect("EmployerLogin.aspx");
                return;
            }

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "SELECT JobSeekerID, FirstName, LastName FROM JobSeeker ORDER BY FirstName, LastName";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    try
                    {
                        conn.Open();
                        SqlDataReader reader = cmd.ExecuteReader();

                        ddlJobSeekers.Items.Clear();
                        ddlJobSeekers.Items.Add(new ListItem("-- Select Job Seeker --", ""));
                        while (reader.Read())
                        {
                            string jobSeekerName = $"{reader["FirstName"]} {reader["LastName"]}";
                            ddlJobSeekers.Items.Add(new ListItem(jobSeekerName, reader["JobSeekerID"].ToString()));
                        }
                        reader.Close();
                    }
                    catch (SqlException sqlEx)
                    {
                        System.Diagnostics.Trace.TraceError($"SQL Error in LoadJobSeekers: {sqlEx.Message}");
                        lblMessage.Text = "A database error occurred while loading job seekers.";
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Trace.TraceError($"Error in LoadJobSeekers: {ex.Message}");
                        lblMessage.Text = "An unexpected error occurred while loading job seekers.";
                    }
                }
            }
        }

        /// <summary>
        /// Loads chat messages for the employer.
        /// </summary>
        private void LoadChatMessages()
        {
            if (!int.TryParse(Session["EmployerID"].ToString(), out int employerID))
            {
                lblMessage.Text = "Invalid session data. Please log in again.";
                Response.Redirect("EmployerLogin.aspx");
                return;
            }

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = @"
                    SELECT CM.Message, CM.MessageTime, JS.FirstName, JS.LastName, JS.ProfilePicturePath
                    FROM ChatMessages CM
                    INNER JOIN JobSeeker JS ON CM.JobSeekerID = JS.JobSeekerID
                    WHERE CM.EmployerID = @EmployerID
                    ORDER BY CM.MessageTime DESC";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@EmployerID", employerID);

                    try
                    {
                        conn.Open();
                        SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                        DataTable messagesTable = new DataTable();
                        adapter.Fill(messagesTable);

                        rptChatMessages.DataSource = messagesTable;
                        rptChatMessages.DataBind();
                    }
                    catch (SqlException sqlEx)
                    {
                        System.Diagnostics.Trace.TraceError($"SQL Error in LoadChatMessages: {sqlEx.Message}");
                        //lblChatMessage.Text = "A database error occurred while loading messages.";
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Trace.TraceError($"Error in LoadChatMessages: {ex.Message}");
                       // lblChatMessage.Text = "An unexpected error occurred while loading messages.";
                    }
                }
            }
        }

        /// <summary>
        /// Handles the Change Password button click event.
        /// </summary>
        protected void btnChangePassword_Click(object sender, EventArgs e)
        {
            string oldPassword = txtOldPassword.Text.Trim();
            string newPassword = txtNewPassword.Text.Trim();
            string confirmPassword = txtConfirmPassword.Text.Trim();

            if (string.IsNullOrEmpty(oldPassword) || string.IsNullOrEmpty(newPassword) || string.IsNullOrEmpty(confirmPassword))
            {
                lblMessage.Text = "All fields are required.";
                return;
            }

            if (newPassword != confirmPassword)
            {
                lblMessage.Text = "New passwords do not match.";
                return;
            }

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    if (!int.TryParse(Session["EmployerID"].ToString(), out int employerID))
                    {
                        lblMessage.Text = "Session expired. Please log in again.";
                        Response.Redirect("EmployerLogin.aspx");
                        return;
                    }

                    // Get UserID from Employer table
                    string userIdQuery = "SELECT UserID FROM Employer WHERE EmployerID = @EmployerID";
                    SqlCommand userIdCommand = new SqlCommand(userIdQuery, connection);
                    userIdCommand.Parameters.AddWithValue("@EmployerID", employerID);

                    object result = userIdCommand.ExecuteScalar();
                    if (result == null)
                    {
                        lblMessage.Text = "Employer not found.";
                        return;
                    }

                    int userID = Convert.ToInt32(result);

                    // Verify the old password
                    string query = "SELECT Password FROM [User] WHERE UserID = @UserID";
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@UserID", userID);

                    string storedPassword = command.ExecuteScalar() as string;

                    if (storedPassword != oldPassword)
                    {
                        lblMessage.Text = "Old password is incorrect.";
                        return;
                    }

                    // Update the password
                    string updateQuery = "UPDATE [User] SET Password = @NewPassword WHERE UserID = @UserID";
                    SqlCommand updateCommand = new SqlCommand(updateQuery, connection);
                    updateCommand.Parameters.AddWithValue("@NewPassword", newPassword); // Consider hashing
                    updateCommand.Parameters.AddWithValue("@UserID", userID);

                    int rowsAffected = updateCommand.ExecuteNonQuery();

                    lblMessage.Text = rowsAffected > 0 ? "Password changed successfully." : "Error updating password.";
                }
            }
            catch (Exception ex)
            {
                // Log the exception (consider a logging framework)
                System.Diagnostics.Trace.TraceError($"Error in btnChangePassword_Click: {ex.Message}");
                lblMessage.Text = "An error occurred while changing the password. Please try again.";
            }
        }

        /// <summary>
        /// Handles the Send Message button click event.
        /// </summary>
        protected void btnSendMessage_Click(object sender, EventArgs e)
        {
            if (!int.TryParse(Session["EmployerID"].ToString(), out int employerID))
            {
                lblChatMessage.Text = "Invalid session data. Please log in again.";
                Response.Redirect("EmployerLogin.aspx");
                return;
            }

            if (!int.TryParse(ddlJobSeekers.SelectedValue, out int jobSeekerID) || jobSeekerID == 0)
            {
                lblChatMessage.Text = "Please select a job seeker to send a message.";
                return;
            }

            string messageContent = txtChatMessage.Text.Trim();

            if (string.IsNullOrEmpty(messageContent))
            {
                lblChatMessage.Text = "Please enter a message.";
                return;
            }

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string query = @"
                        INSERT INTO ChatMessages (JobSeekerID, EmployerID, Message, MessageTime)
                        VALUES (@JobSeekerID, @EmployerID, @Message, @MessageTime)";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@JobSeekerID", jobSeekerID);
                        cmd.Parameters.AddWithValue("@EmployerID", employerID);
                        cmd.Parameters.AddWithValue("@Message", messageContent);
                        cmd.Parameters.AddWithValue("@MessageTime", DateTime.Now);

                        conn.Open();
                        cmd.ExecuteNonQuery();

                        txtChatMessage.Text = "";
                        lblChatMessage.Text = "Message sent successfully.";
                        LoadChatMessages();
                    }
                }
            }
            catch (SqlException sqlEx)
            {
                System.Diagnostics.Trace.TraceError($"SQL Error in btnSendMessage_Click: {sqlEx.Message}");
                lblChatMessage.Text = "A database error occurred while sending your message.";
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.TraceError($"Error in btnSendMessage_Click: {ex.Message}");
                lblChatMessage.Text = "An unexpected error occurred while sending your message.";
            }
        }

        /// <summary>
        /// Handles the click event for viewing the employer's profile.
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
        protected void lnkLogout_Click(object sender, EventArgs e)
        {
            Session.Abandon();
            Response.Redirect("EmployerLogin.aspx");
        }
    }
}
