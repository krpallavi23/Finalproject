using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace OnlineJobPortal
{
    public partial class SearchJobSeekers : Page
    {
        // Retrieve the connection string from Web.config
        private readonly string connectionString = ConfigurationManager.ConnectionStrings["OnlineJobPortalDB"].ConnectionString;

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
                    PopulateSearchFilters();
                    LoadJobSeekers();
                    LoadJobSeekersForChat();
                    LoadChatMessages();
                }
            }
        }

        /// <summary>
        /// Loads employer details such as name.
        /// </summary>
        private void LoadEmployerDetails()
        {
            if (Session["EmployerID"] == null)
            {
                lblStatus.Text = "User session expired. Please log in again.";
                Response.Redirect("EmployerLogin.aspx");
                return;
            }

            // Safely parse EmployerID from session
            if (!int.TryParse(Session["EmployerID"].ToString(), out int employerID))
            {
                lblStatus.Text = "Invalid session data. Please log in again.";
                Response.Redirect("EmployerLogin.aspx");
                return;
            }

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "SELECT CompanyName, ContactPerson FROM Employer WHERE EmployerID = @EmployerID";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@EmployerID", employerID);

                    try
                    {
                        conn.Open();
                        SqlDataReader reader = cmd.ExecuteReader();
                        if (reader.Read())
                        {
                            lblEmployerName.Text = $"{reader["CompanyName"]} - {reader["ContactPerson"]}";
                        }
                        else
                        {
                            lblEmployerName.Text = "Employer";
                        }
                        reader.Close();
                    }
                    catch (SqlException sqlEx)
                    {
                        // Log the exception details
                        System.Diagnostics.Trace.TraceError($"SQL Error in LoadEmployerDetails: {sqlEx.Message}");
                        lblStatus.Text = "A database error occurred while loading your details.";
                    }
                    catch (Exception ex)
                    {
                        // Log the exception details
                        System.Diagnostics.Trace.TraceError($"Error in LoadEmployerDetails: {ex.Message}");
                        lblStatus.Text = "An unexpected error occurred while loading your details.";
                    }
                }
            }
        }

        /// <summary>
        /// Populates search filters: Location and Experience.
        /// </summary>
        private void PopulateSearchFilters()
        {
            PopulateLocations();
            // Experience levels are predefined in the ASPX DropDownList
        }

        /// <summary>
        /// Populates the Location DropDownList.
        /// </summary>
        private void PopulateLocations()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "SELECT DISTINCT City FROM JobSeeker ORDER BY City";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    try
                    {
                        conn.Open();
                        SqlDataReader reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            string city = reader["City"].ToString();
                            ddlLocation.Items.Add(new ListItem(city, city));
                        }
                        reader.Close();
                    }
                    catch (SqlException sqlEx)
                    {
                        System.Diagnostics.Trace.TraceError($"SQL Error in PopulateLocations: {sqlEx.Message}");
                        lblStatus.Text = "A database error occurred while loading locations.";
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Trace.TraceError($"Error in PopulateLocations: {ex.Message}");
                        lblStatus.Text = "An unexpected error occurred while loading locations.";
                    }
                }
            }
        }

        /// <summary>
        /// Loads job seekers based on search filters.
        /// </summary>
        private void LoadJobSeekers()
        {
            string skills = txtSkills.Text.Trim();
            string location = ddlLocation.SelectedValue;
            string experience = ddlExperience.SelectedValue;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = @"
                    SELECT 
                        JS.JobSeekerID, 
                        JS.FirstName, 
                        JS.LastName, 
                        ISNULL(STRING_AGG(ST.SkillName, ', '), '') AS Skills, 
                        JS.City AS Location, 
                        JS.YearsOfExperience
                    FROM JobSeeker JS
                    LEFT JOIN JobSeekerSkill JSS ON JS.JobSeekerID = JSS.JobSeekerID
                    LEFT JOIN SkillTag ST ON JSS.SkillTagID = ST.SkillTagID
                    WHERE 1=1";

                // Append filters based on user input
                if (!string.IsNullOrEmpty(skills))
                {
                    // Assuming skills are comma-separated and we want JobSeekers having all specified skills
                    string[] skillArray = skills.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                    for (int i = 0; i < skillArray.Length; i++)
                    {
                        // Use EXISTS to ensure the JobSeeker has each skill
                        query += $" AND EXISTS (SELECT 1 FROM JobSeekerSkill JSS{i} INNER JOIN SkillTag ST{i} ON JSS{i}.SkillTagID = ST{i}.SkillTagID WHERE JSS{i}.JobSeekerID = JS.JobSeekerID AND ST{i}.SkillName LIKE @Skill{i})";
                    }
                }

                if (!string.IsNullOrEmpty(location))
                {
                    query += " AND JS.City = @Location";
                }

                if (!string.IsNullOrEmpty(experience))
                {
                    // Handle experience ranges
                    if (experience.Contains("+"))
                    {
                        // e.g., "5+"
                        if (int.TryParse(experience.Replace("+", ""), out int minExp))
                        {
                            query += " AND JS.YearsOfExperience >= @MinExperience";
                        }
                    }
                    else
                    {
                        // e.g., "0-1", "2-3", "4-5"
                        string[] expRange = experience.Split('-');
                        if (expRange.Length == 2)
                        {
                            if (int.TryParse(expRange[0], out int minExp) && int.TryParse(expRange[1], out int maxExp))
                            {
                                query += " AND JS.YearsOfExperience BETWEEN @MinExperience AND @MaxExperience";
                            }
                        }
                    }
                }

                query += " GROUP BY JS.JobSeekerID, JS.FirstName, JS.LastName, JS.City, JS.YearsOfExperience";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    // Add parameters to prevent SQL injection
                    if (!string.IsNullOrEmpty(skills))
                    {
                        string[] skillArray = skills.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                        for (int i = 0; i < skillArray.Length; i++)
                        {
                            cmd.Parameters.AddWithValue($"@Skill{i}", $"%{skillArray[i].Trim()}%");
                        }
                    }
                    if (!string.IsNullOrEmpty(location))
                    {
                        cmd.Parameters.AddWithValue("@Location", location);
                    }
                    if (!string.IsNullOrEmpty(experience))
                    {
                        if (experience.Contains("+"))
                        {
                            int minExp = int.Parse(experience.Replace("+", ""));
                            cmd.Parameters.AddWithValue("@MinExperience", minExp);
                        }
                        else
                        {
                            string[] expRange = experience.Split('-');
                            if (expRange.Length == 2)
                            {
                                int minExp = int.Parse(expRange[0]);
                                int maxExp = int.Parse(expRange[1]);
                                cmd.Parameters.AddWithValue("@MinExperience", minExp);
                                cmd.Parameters.AddWithValue("@MaxExperience", maxExp);
                            }
                        }
                    }

                    try
                    {
                        conn.Open();
                        SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                        DataTable seekersTable = new DataTable();
                        adapter.Fill(seekersTable);

                        if (seekersTable.Rows.Count > 0)
                        {
                            ViewState["JobSeekers"] = seekersTable; // Store data for sorting
                            gvJobSeekers.DataSource = seekersTable;
                            gvJobSeekers.DataBind();
                            lblStatus.Text = $"Found {seekersTable.Rows.Count} job seeker(s).";
                        }
                        else
                        {
                            gvJobSeekers.DataSource = null;
                            gvJobSeekers.DataBind();
                            lblStatus.Text = "No job seekers found matching your criteria.";
                        }
                    }
                    catch (SqlException sqlEx)
                    {
                        // Log the exception details (optional)
                        System.Diagnostics.Trace.TraceError($"SQL Error in LoadJobSeekers: {sqlEx.Message}\nStack Trace: {sqlEx.StackTrace}");

                        // Display the detailed error message
                        lblStatus.Text = $"A database error occurred while loading job seekers: {sqlEx.Message}";
                    }
                    catch (Exception ex)
                    {
                        // Log the exception details (optional)
                        System.Diagnostics.Trace.TraceError($"Error in LoadJobSeekers: {ex.Message}\nStack Trace: {ex.StackTrace}");

                        // Display the detailed error message
                        lblStatus.Text = $"An unexpected error occurred while loading job seekers: {ex.Message}";
                    }
                }
            }
        }

        /// <summary>
        /// Loads job seekers into the chat dropdown.
        /// </summary>
        private void LoadJobSeekersForChat()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "SELECT JobSeekerID, FirstName + ' ' + LastName AS FullName FROM JobSeeker WHERE 1=1";

                // Optionally, filter only active job seekers if a status field exists
                // e.g., WHERE JS.Status = 'Active'

                query += " ORDER BY FirstName, LastName";

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
                            ddlJobSeekers.Items.Add(new ListItem(reader["FullName"].ToString(), reader["JobSeekerID"].ToString()));
                        }
                        reader.Close();
                    }
                    catch (SqlException sqlEx)
                    {
                        System.Diagnostics.Trace.TraceError($"SQL Error in LoadJobSeekersForChat: {sqlEx.Message}");
                        lblChatMessage.Text = "A database error occurred while loading job seekers for chat.";
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Trace.TraceError($"Error in LoadJobSeekersForChat: {ex.Message}");
                        lblChatMessage.Text = "An unexpected error occurred while loading job seekers for chat.";
                    }
                }
            }
        }

        /// <summary>
        /// Loads chat messages between the employer and the selected job seeker.
        /// </summary>
        private void LoadChatMessages()
        {
            if (ddlJobSeekers.SelectedValue == "")
            {
                rptChatMessages.DataSource = null;
                rptChatMessages.DataBind();
                return;
            }

            string selectedJobSeekerID = ddlJobSeekers.SelectedValue;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = @"
                    SELECT CM.Message, CM.MessageTime, JS.FirstName + ' ' + JS.LastName AS JobSeekerName
                    FROM ChatMessages CM
                    INNER JOIN JobSeeker JS ON CM.JobSeekerID = JS.JobSeekerID
                    WHERE CM.EmployerID = @EmployerID AND CM.JobSeekerID = @JobSeekerID
                    ORDER BY CM.MessageTime ASC";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@EmployerID", Session["EmployerID"]);
                    cmd.Parameters.AddWithValue("@JobSeekerID", selectedJobSeekerID);

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
                        lblChatMessage.Text = "A database error occurred while loading chat messages.";
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Trace.TraceError($"Error in LoadChatMessages: {ex.Message}");
                        lblChatMessage.Text = "An unexpected error occurred while loading chat messages.";
                    }
                }
            }
        }

        /// <summary>
        /// Handles the TextChanged event for the Skills TextBox.
        /// </summary>
        protected void txtSkills_TextChanged(object sender, EventArgs e)
        {
            LoadJobSeekers();
        }

        /// <summary>
        /// Handles the SelectedIndexChanged event for the Location DropDownList.
        /// </summary>
        protected void ddlLocation_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadJobSeekers();
        }

        /// <summary>
        /// Handles the SelectedIndexChanged event for the Experience DropDownList.
        /// </summary>
        protected void ddlExperience_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadJobSeekers();
        }

        /// <summary>
        /// Handles the Search button click event.
        /// </summary>
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            LoadJobSeekers();
        }

        /// <summary>
        /// Handles paging in the GridView.
        /// </summary>
        protected void gvJobSeekers_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvJobSeekers.PageIndex = e.NewPageIndex;
            LoadJobSeekers();
        }

        /// <summary>
        /// Handles sorting in the GridView.
        /// </summary>
        protected void gvJobSeekers_Sorting(object sender, GridViewSortEventArgs e)
        {
            DataTable seekersTable = ViewState["JobSeekers"] as DataTable;
            if (seekersTable != null)
            {
                DataView dv = new DataView(seekersTable);
                string sortDirection = "ASC";

                // Toggle sort direction
                if (ViewState["SortDirection"] != null && ViewState["SortDirection"].ToString() == "ASC")
                {
                    sortDirection = "DESC";
                }
                ViewState["SortDirection"] = sortDirection;

                dv.Sort = e.SortExpression + " " + sortDirection;
                gvJobSeekers.DataSource = dv;
                gvJobSeekers.DataBind();
            }
        }

        /// <summary>
        /// Handles the Message button click event in the GridView.
        /// </summary>
        protected void btnMessage_Click(object sender, EventArgs e)
        {
            Button messageButton = sender as Button;
            if (messageButton != null)
            {
                string jobSeekerID = messageButton.CommandArgument;
                ddlJobSeekers.SelectedValue = jobSeekerID;
                LoadChatMessages();
                // Optionally, scroll to the chatbox or open it
                ClientScript.RegisterStartupScript(this.GetType(), "OpenChatBox", "toggleChatBox();", true);
            }
        }

        /// <summary>
        /// Handles the Send Message button click event.
        /// </summary>
        protected void btnSendMessage_Click(object sender, EventArgs e)
        {
            if (ddlJobSeekers.SelectedValue == "")
            {
                lblChatMessage.Text = "Please select a job seeker to chat with.";
                return;
            }

            string messageContent = txtChatMessage.Text.Trim();
            if (string.IsNullOrEmpty(messageContent))
            {
                lblChatMessage.Text = "Please enter a message.";
                return;
            }

            if (!int.TryParse(Session["EmployerID"].ToString(), out int employerID))
            {
                lblChatMessage.Text = "Invalid session data. Please log in again.";
                Response.Redirect("EmployerLogin.aspx");
                return;
            }

            if (!int.TryParse(ddlJobSeekers.SelectedValue, out int jobSeekerID))
            {
                lblChatMessage.Text = "Invalid Job Seeker selected.";
                return;
            }

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = @"
                    INSERT INTO ChatMessages (EmployerID, JobSeekerID, Message, MessageTime)
                    VALUES (@EmployerID, @JobSeekerID, @Message, @MessageTime)";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@EmployerID", employerID);
                    cmd.Parameters.AddWithValue("@JobSeekerID", jobSeekerID);
                    cmd.Parameters.AddWithValue("@Message", messageContent);
                    cmd.Parameters.AddWithValue("@MessageTime", DateTime.Now);

                    try
                    {
                        conn.Open();
                        cmd.ExecuteNonQuery();

                        txtChatMessage.Text = "";
                        lblChatMessage.Text = "Message sent successfully.";
                        LoadChatMessages();
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
            }
        }

        /// <summary>
        /// Handles the View Profile LinkButton click event.
        /// </summary>
        protected void lnkViewProfile_Click(object sender, EventArgs e)
        {
            Response.Redirect("EmployerProfile.aspx");
        }

        /// <summary>
        /// Handles the Inbox LinkButton click event.
        /// </summary>
        protected void lnkInbox_Click(object sender, EventArgs e)
        {
            Response.Redirect("ChangeEmployerPassword.aspx");
        }

        /// <summary>
        /// Handles the Logout LinkButton click event.
        /// </summary>
        protected void lnkLogout_Click(object sender, EventArgs e)
        {
            Session.Abandon();
            Response.Redirect("EmployerLogin.aspx");
        }
    }
}
