using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI.WebControls;
using System.Web.UI;

namespace OnlineJobPortal
{
    public partial class ManageJobsActive : System.Web.UI.Page
    {
        // Connection string from Web.config
        string connectionString = ConfigurationManager.ConnectionStrings["OnlineJobPortalDB"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindJobs();
                LoadJobSeekers(); // Load job seekers for chat functionality
            }
        }

        /// <summary>
        /// Binds the GridView with active jobs of the logged-in employer.
        /// </summary>
        private void BindJobs()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                int employerId = GetEmployerID(); // Retrieve EmployerID from Session

                string query = @"
                    SELECT 
                        JobID, Title, Description, Location, Salary, ApplicationDeadline, 
                        JobType, Status, JobCategory, RequiredYearsOfExperience 
                    FROM 
                        JobPosting 
                    WHERE 
                        EmployerID = @EmployerID AND Status = 'Active'"; // Only active jobs

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

        /// <summary>
        /// Handles the RowEditing event to enable edit mode for the selected row.
        /// </summary>
        protected void gvJobs_RowEditing(object sender, GridViewEditEventArgs e)
        {
            gvJobs.EditIndex = e.NewEditIndex;
            BindJobs();
        }

        /// <summary>
        /// Handles the RowCancelingEdit event to cancel edit mode.
        /// </summary>
        protected void gvJobs_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            gvJobs.EditIndex = -1;
            BindJobs();
        }

        /// <summary>
        /// Handles the RowUpdating event to update job details in the database.
        /// </summary>
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
                lblSuccess.Text = "Job updated successfully.";
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

        /// <summary>
        /// Handles the RowCommand event for custom button commands.
        /// </summary>
        protected void gvJobs_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "ProfileMatch" ||
                e.CommandName == "Shortlisted" ||
                e.CommandName == "Interviewed" ||
                e.CommandName == "Accepted" ||
                e.CommandName == "Rejected")
            {
                int jobId = Convert.ToInt32(e.CommandArgument);

                string redirectUrl = "";

                switch (e.CommandName)
                {
                    case "ProfileMatch":
                        redirectUrl = $"MatchingCandidates.aspx?JobID={jobId}";
                        break;
                    case "Shortlisted":
                        redirectUrl = $"ShortlistedCandidates.aspx?JobID={jobId}";
                        break;
                    case "Interviewed":
                        redirectUrl = $"InterviewedCandidates.aspx?JobID={jobId}";
                        break;
                    case "Accepted":
                        redirectUrl = $"AcceptedCandidates.aspx?JobID={jobId}";
                        break;
                    case "Rejected":
                        redirectUrl = $"RejectedCandidates.aspx?JobID={jobId}";
                        break;
                }

                if (!string.IsNullOrEmpty(redirectUrl))
                {
                    Response.Redirect(redirectUrl);
                }
            }
        }

        /// <summary>
        /// Handles the RowDataBound event to set selected values in dropdowns during edit mode.
        /// </summary>
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

        /// <summary>
        /// Retrieves the EmployerID from the session.
        /// </summary>
        /// <returns>EmployerID as integer</returns>
        private int GetEmployerID()
        {
            if (Session["EmployerID"] != null)
            {
                return Convert.ToInt32(Session["EmployerID"]);
            }
            else
            {
                // Redirect to login or handle accordingly
                Response.Redirect("EmployerLogin.aspx");
                return 0; // This line will not be reached due to redirection
            }
        }

        /// <summary>
        /// Loads job seekers into the dropdown for chat functionality.
        /// </summary>
        private void LoadJobSeekers()
        {
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
        /// Handles the Send Message button click event in the chatbox.
        /// </summary>
        protected void btnSendMessage_Click(object sender, EventArgs e)
        {
            lblMessage.Text = "";
            if (Session["EmployerID"] == null)
            {
                lblMessage.Text = "Session expired. Please log in again.";
                Response.Redirect("EmployerLogin.aspx");
                return;
            }

            if (!int.TryParse(Session["EmployerID"].ToString(), out int employerID))
            {
                lblMessage.Text = "Invalid session data.";
                Response.Redirect("EmployerLogin.aspx");
                return;
            }

            if (string.IsNullOrEmpty(ddlJobSeekers.SelectedValue) || string.IsNullOrEmpty(txtChatMessage.Text.Trim()))
            {
                lblMessage.Text = "Please select a job seeker and enter a message.";
                return;
            }

            if (!int.TryParse(ddlJobSeekers.SelectedValue, out int jobSeekerID))
            {
                lblMessage.Text = "Invalid job seeker selection.";
                return;
            }

            string messageContent = txtChatMessage.Text.Trim();

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

        /// <summary>
        /// Loads chat messages from the database for the logged-in employer.
        /// </summary>
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

        protected void gvJobs_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
