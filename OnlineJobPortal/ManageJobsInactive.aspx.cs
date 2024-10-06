using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Web.UI.WebControls;
using System.Web.UI;

namespace OnlineJobPortal
{
    public partial class ManageJobsInactive : System.Web.UI.Page
    {
        // Connection string from Web.config
        string connectionString = ConfigurationManager.ConnectionStrings["OnlineJobPortalDB"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindJobs();
                LoadJobSeekers(); // Load job seekers for the chatbox
            }
        }

        private void BindJobs()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                int employerId = GetEmployerID(); // Implement this method to retrieve EmployerID from Session or other means

                string query = @"
                    SELECT 
                        JobID, Title, Description, Location, Salary, ApplicationDeadline, 
                        JobType, Status, JobCategory, RequiredYearsOfExperience 
                    FROM 
                        JobPosting 
                    WHERE 
                        EmployerID = @EmployerID AND Status = 'Inactive'";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@EmployerID", employerId);
                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        DataTable dt = new DataTable();
                        da.Fill(dt);
                        gvJobs.DataSource = dt;
                        gvJobs.DataBind();
                    }
                }
            }
        }

        protected void gvJobs_RowEditing(object sender, GridViewEditEventArgs e)
        {
            gvJobs.EditIndex = e.NewEditIndex;
            BindJobs();
        }

        protected void gvJobs_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            gvJobs.EditIndex = -1;
            BindJobs();
        }

        protected void gvJobs_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            int jobId = Convert.ToInt32(gvJobs.DataKeys[e.RowIndex].Value);
            GridViewRow row = gvJobs.Rows[e.RowIndex];

            // Retrieve all editable fields using FindControl
            TextBox txtTitle = (TextBox)row.FindControl("txtTitle");
            TextBox txtDescription = (TextBox)row.FindControl("txtDescription");
            TextBox txtLocation = (TextBox)row.FindControl("txtLocation");
            TextBox txtSalary = (TextBox)row.FindControl("txtSalary");
            TextBox txtApplicationDeadline = (TextBox)row.FindControl("txtApplicationDeadline");
            DropDownList ddlJobType = (DropDownList)row.FindControl("ddlJobType");
            DropDownList ddlStatus = (DropDownList)row.FindControl("ddlStatus");
            TextBox txtJobCategory = (TextBox)row.FindControl("txtJobCategory");
            TextBox txtRequiredExperience = (TextBox)row.FindControl("txtRequiredExperience");

            // Validate and parse inputs
            string title = txtTitle.Text.Trim();
            string description = txtDescription.Text.Trim();
            string location = txtLocation.Text.Trim();

            decimal salary;
            if (!decimal.TryParse(txtSalary.Text.Trim(), out salary))
            {
                lblError.Text = "Invalid salary format.";
                return;
            }

            DateTime applicationDeadline;
            if (!DateTime.TryParse(txtApplicationDeadline.Text.Trim(), out applicationDeadline))
            {
                lblError.Text = "Invalid date format for Application Deadline.";
                return;
            }

            string jobType = ddlJobType.SelectedValue;
            string status = ddlStatus.SelectedValue;
            string jobCategory = txtJobCategory.Text.Trim();

            int requiredExperience;
            if (!int.TryParse(txtRequiredExperience.Text.Trim(), out requiredExperience))
            {
                lblError.Text = "Invalid number format for Required Years of Experience.";
                return;
            }

            // Update the job in the database
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string updateQuery = @"
                        UPDATE JobPosting 
                        SET 
                            Title = @Title, 
                            Description = @Description, 
                            Location = @Location, 
                            Salary = @Salary, 
                            ApplicationDeadline = @ApplicationDeadline, 
                            JobType = @JobType, 
                            Status = @Status, 
                            JobCategory = @JobCategory, 
                            RequiredYearsOfExperience = @RequiredYearsOfExperience 
                        WHERE 
                            JobID = @JobID";

                    using (SqlCommand cmd = new SqlCommand(updateQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@Title", title);
                        cmd.Parameters.AddWithValue("@Description", description);
                        cmd.Parameters.AddWithValue("@Location", location);
                        cmd.Parameters.AddWithValue("@Salary", salary);
                        cmd.Parameters.AddWithValue("@ApplicationDeadline", applicationDeadline);
                        cmd.Parameters.AddWithValue("@JobType", jobType);
                        cmd.Parameters.AddWithValue("@Status", status);
                        cmd.Parameters.AddWithValue("@JobCategory", jobCategory);
                        cmd.Parameters.AddWithValue("@RequiredYearsOfExperience", requiredExperience);
                        cmd.Parameters.AddWithValue("@JobID", jobId);

                        conn.Open();
                        cmd.ExecuteNonQuery();
                        conn.Close();
                    }
                }

                lblError.Text = ""; // Clear any previous errors
                gvJobs.EditIndex = -1;
                BindJobs();
            }
            catch (SqlException ex)
            {
                lblError.Text = "An error occurred while updating the job: " + ex.Message;
            }
            catch (Exception ex)
            {
                lblError.Text = "An unexpected error occurred: " + ex.Message;
            }
        }

        protected void gvJobs_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "ProfileMatch")
            {
                int jobId = Convert.ToInt32(e.CommandArgument);
                // Redirect to MatchingCandidates.aspx with JobID as query parameter
                Response.Redirect($"MatchingCandidates.aspx?JobID={jobId}");
            }
        }

        protected void gvJobs_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            // Ensure this is a data row
            if (e.Row.RowType == DataControlRowType.DataRow && gvJobs.EditIndex == e.Row.RowIndex)
            {
                // Set the selected value for JobType dropdown
                DropDownList ddlJobType = (DropDownList)e.Row.FindControl("ddlJobType");
                string jobType = DataBinder.Eval(e.Row.DataItem, "JobType").ToString();
                if (ddlJobType.Items.FindByValue(jobType) != null)
                {
                    ddlJobType.SelectedValue = jobType;
                }

                // Set the selected value for Status dropdown
                DropDownList ddlStatus = (DropDownList)e.Row.FindControl("ddlStatus");
                string status = DataBinder.Eval(e.Row.DataItem, "Status").ToString();
                if (ddlStatus.Items.FindByValue(status) != null)
                {
                    ddlStatus.SelectedValue = status;
                }
            }
        }

        private int GetEmployerID()
        {
            // Implement logic to retrieve EmployerID from Session or other means
            if (Session["EmployerID"] != null)
            {
                return Convert.ToInt32(Session["EmployerID"]);
            }
            else
            {
                // Redirect to login or handle accordingly
                Response.Redirect("Login.aspx");
                return 0;
            }
        }

        // Event handler for Profile Match button
        // Already handled via RowCommand

        // Event handlers for user profile dropdown
        protected void lnkViewProfile_Click(object sender, EventArgs e)
        {
            Response.Redirect("EmployerProfile.aspx");
        }

        protected void lnkInbox_Click(object sender, EventArgs e)
        {
            Response.Redirect("Inbox.aspx");
        }

        protected void lnkLogout_Click(object sender, EventArgs e)
        {
            // Clear session and redirect to Login page
            Session.Clear();
            Response.Redirect("Login.aspx");
        }

        protected void btnSendMessage_Click(object sender, EventArgs e)
        {
            if (Session["EmployerID"] == null)
            {
                lblMessage.Text = "User session expired. Please log in again.";
                Response.Redirect("EmployerLogin.aspx");
                return;
            }

            if (!int.TryParse(Session["EmployerID"].ToString(), out int employerID))
            {
                lblMessage.Text = "Invalid session data. Please log in again.";
                Response.Redirect("EmployerLogin.aspx");
                return;
            }

            if (!int.TryParse(ddlJobSeekers.SelectedValue, out int jobSeekerID) || jobSeekerID == 0)
            {
                lblMessage.Text = "Please select a job seeker to send a message.";
                return;
            }

            string messageContent = txtChatMessage.Text.Trim();

            if (string.IsNullOrEmpty(messageContent))
            {
                lblMessage.Text = "Please enter a message.";
                return;
            }

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

                    try
                    {
                        conn.Open();
                        cmd.ExecuteNonQuery();

                        txtChatMessage.Text = "";
                        lblMessage.Text = "Message sent successfully.";
                        LoadChatMessages();
                    }
                    catch (SqlException sqlEx)
                    {
                        System.Diagnostics.Trace.TraceError($"SQL Error in btnSendMessage_Click: {sqlEx.Message}");
                        lblMessage.Text = "A database error occurred while sending your message.";
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Trace.TraceError($"Error in btnSendMessage_Click: {ex.Message}");
                        lblMessage.Text = "An unexpected error occurred while sending your message.";
                    }
                }
            }
        }

        private void LoadChatMessages()
        {
            if (Session["EmployerID"] == null)
            {
                lblMessage.Text = "User session expired. Please log in again.";
                Response.Redirect("EmployerLogin.aspx");
                return;
            }

            if (!int.TryParse(Session["EmployerID"].ToString(), out int employerID))
            {
                lblMessage.Text = "Invalid session data. Please log in again.";
                Response.Redirect("EmployerLogin.aspx");
                return;
            }

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = @"
                    SELECT CM.Message, CM.MessageTime, JS.FirstName, JS.LastName, JS.ResumePath AS ProfilePicturePath
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
                        lblMessage.Text = "A database error occurred while loading messages.";
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Trace.TraceError($"Error in LoadChatMessages: {ex.Message}");
                        lblMessage.Text = "An unexpected error occurred while loading messages.";
                    }
                }
            }
        }

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
    }
}
