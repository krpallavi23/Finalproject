using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace OnlineJobPortal
{
    public partial class PostJob : Page
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
                    PopulateDepartments();
                    PopulateEmploymentTypes();
                    LoadDegrees();
                    LoadSkills();
                    InitializeDegreeRepeater();
                    InitializeSkillRepeater();
                    LoadJobSeekers(); // For the chat functionality
                }
            }
        }

        [Serializable]
        public class DegreeItem
        {
            public string DegreeID { get; set; }
            public string DegreeName { get; set; }
        }

        [Serializable]
        public class SkillItem
        {
            public string SkillTagID { get; set; }
            public string SkillName { get; set; }
        }

        /// <summary>
        /// Loads employer details such as company name and contact person.
        /// </summary>
        private void LoadEmployerDetails()
        {
            if (Session["EmployerID"] == null)
            {
                lblStatus.Text = "User session expired. Please log in again.";
                lblStatus.CssClass = "mt-2 text-danger";
                Response.Redirect("EmployerLogin.aspx");
                return;
            }

            // Safely parse EmployerID from session
            if (!int.TryParse(Session["EmployerID"].ToString(), out int employerID))
            {
                lblStatus.Text = "Invalid session data. Please log in again.";
                lblStatus.CssClass = "mt-2 text-danger";
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
                    catch (Exception ex)
                    {
                        // Log error
                        lblStatus.Text = "An error occurred while loading employer details.";
                        lblStatus.CssClass = "mt-2 text-danger";
                    }
                }
            }
        }

        /// <summary>
        /// Populates the Department DropDownList.
        /// </summary>
        private void PopulateDepartments()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "SELECT DISTINCT JobCategory FROM JobPosting ORDER BY JobCategory";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    try
                    {
                        conn.Open();
                        SqlDataReader reader = cmd.ExecuteReader();
                        ddlDepartment.Items.Clear();
                        ddlDepartment.Items.Add(new ListItem("-- Select Department --", ""));
                        while (reader.Read())
                        {
                            string department = reader["JobCategory"].ToString();
                            ddlDepartment.Items.Add(new ListItem(department, department));
                        }
                        reader.Close();
                    }
                    catch (Exception ex)
                    {
                        // Log error
                        lblStatus.Text = "An error occurred while loading departments.";
                        lblStatus.CssClass = "mt-2 text-danger";
                    }
                }
            }
        }

        /// <summary>
        /// Populates the Employment Type DropDownList.
        /// </summary>
        private void PopulateEmploymentTypes()
        {
            // Employment types are already defined in the .aspx file
            // This method can be used if you prefer to populate them dynamically
        }

        /// <summary>
        /// Loads degrees into ViewState and initializes the Repeater.
        /// </summary>
        private void LoadDegrees()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "SELECT DegreeID, DegreeName FROM Degree ORDER BY DegreeName";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    try
                    {
                        conn.Open();
                        SqlDataReader reader = cmd.ExecuteReader();
                        List<DegreeItem> degreeItems = new List<DegreeItem>();
                        while (reader.Read())
                        {
                            degreeItems.Add(new DegreeItem
                            {
                                DegreeID = reader["DegreeID"].ToString(),
                                DegreeName = reader["DegreeName"].ToString()
                            });
                        }
                        reader.Close();

                        // Check if degrees are loaded
                        if (degreeItems.Count > 0)
                        {
                            ViewState["Degrees"] = degreeItems;
                        }
                        else
                        {
                            lblStatus.Text = "No degrees found in the database.";
                            lblStatus.CssClass = "mt-2 text-warning";
                        }
                    }
                    catch (Exception ex)
                    {
                        // Log error
                        lblStatus.Text = "An error occurred while loading degrees: " + ex.Message;
                        lblStatus.CssClass = "mt-2 text-danger";
                    }
                }
            }
        }


        /// <summary>
        /// Loads skills into ViewState.
        /// </summary>
        private void LoadSkills()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "SELECT SkillTagID, SkillName FROM SkillTag ORDER BY SkillName";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    try
                    {
                        conn.Open();
                        SqlDataReader reader = cmd.ExecuteReader();
                        List<SkillItem> skillItems = new List<SkillItem>();
                        while (reader.Read())
                        {
                            skillItems.Add(new SkillItem
                            {
                                SkillTagID = reader["SkillTagID"].ToString(),
                                SkillName = reader["SkillName"].ToString()
                            });
                        }
                        reader.Close();
                        ViewState["Skills"] = skillItems;
                    }
                    catch (Exception ex)
                    {
                        // Log error
                    }
                }
            }
        }

        /// <summary>
        /// Initializes the Degree Requirements Repeater.
        /// </summary>
        private void InitializeDegreeRepeater()
        {
            List<DegreeRequirement> degrees = new List<DegreeRequirement>
            {
                new DegreeRequirement()
            };
            rptDegrees.DataSource = degrees;
            rptDegrees.DataBind();
        }

        /// <summary>
        /// Initializes the Skill Requirements Repeater.
        /// </summary>
        private void InitializeSkillRepeater()
        {
            List<SkillRequirement> skills = new List<SkillRequirement>
            {
                new SkillRequirement()
            };
            rptSkills.DataSource = skills;
            rptSkills.DataBind();
        }

        /// <summary>
        /// Populates degree dropdowns within the Repeater during ItemDataBound.
        /// </summary>
        protected void rptDegrees_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                DropDownList ddlDegree = e.Item.FindControl("ddlDegree") as DropDownList;
                if (ddlDegree != null)
                {
                    // Retrieve degrees from ViewState
                    var degrees = ViewState["Degrees"] as List<DegreeItem>;

                    // Check if degrees are available
                    if (degrees != null && degrees.Count > 0)
                    {
                        ddlDegree.DataSource = degrees;
                        ddlDegree.DataTextField = "DegreeName";
                        ddlDegree.DataValueField = "DegreeID";
                        ddlDegree.DataBind();

                        // Add a placeholder option
                        ddlDegree.Items.Insert(0, new ListItem("-- Select Degree --", ""));

                        // Set selected value if available
                        DegreeRequirement dataItem = e.Item.DataItem as DegreeRequirement;
                        if (!string.IsNullOrEmpty(dataItem.DegreeID))
                        {
                            ddlDegree.SelectedValue = dataItem.DegreeID;
                        }
                    }
                    else
                    {
                        // Display message if no degrees are available
                        lblStatus.Text = "No degrees available to display.";
                        lblStatus.CssClass = "mt-2 text-warning";
                    }
                }
            }
        }

        /// <summary>
        /// Populates skill dropdowns within the Repeater during ItemDataBound.
        /// </summary>
        protected void rptSkills_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                DropDownList ddlSkill = e.Item.FindControl("ddlSkill") as DropDownList;
                if (ddlSkill != null)
                {
                    // Populate skills
                    var skills = ViewState["Skills"] as List<SkillItem>;
                    ddlSkill.DataSource = skills;
                    ddlSkill.DataTextField = "SkillName";
                    ddlSkill.DataValueField = "SkillTagID";
                    ddlSkill.DataBind();

                    // Add a placeholder option
                    ddlSkill.Items.Insert(0, new ListItem("-- Select Skill --", ""));

                    // Set selected value if available
                    SkillRequirement dataItem = e.Item.DataItem as SkillRequirement;
                    if (!string.IsNullOrEmpty(dataItem.SkillTagID))
                    {
                        ddlSkill.SelectedValue = dataItem.SkillTagID;
                    }
                }

                // Expertise Dropdown (DropDownList)
                DropDownList ddlExpertise = e.Item.FindControl("ddlExpertise") as DropDownList;
                if (ddlExpertise != null)
                {
                    // Set selected value if available
                    SkillRequirement dataItem = e.Item.DataItem as SkillRequirement;
                    if (!string.IsNullOrEmpty(dataItem.SkillLevel))
                    {
                        ddlExpertise.SelectedValue = dataItem.SkillLevel;
                    }
                }
            }
        }

        /// <summary>
        /// Adds a new degree entry to the Repeater.
        /// </summary>
        protected void btnAddDegree_Click(object sender, EventArgs e)
        {
            List<DegreeRequirement> degrees = GetDegreeRequirements();
            degrees.Add(new DegreeRequirement());
            rptDegrees.DataSource = degrees;
            rptDegrees.DataBind();
        }

        /// <summary>
        /// Adds a new skill entry to the Repeater.
        /// </summary>
        protected void btnAddSkill_Click(object sender, EventArgs e)
        {
            List<SkillRequirement> skills = GetSkillRequirements();
            skills.Add(new SkillRequirement());
            rptSkills.DataSource = skills;
            rptSkills.DataBind();
        }

        protected void rptDegrees_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "RemoveDegree")
            {
                List<DegreeRequirement> degrees = GetDegreeRequirements();
                if (degrees.Count > 1)
                {
                    degrees.RemoveAt(Convert.ToInt32(e.CommandArgument));
                    rptDegrees.DataSource = degrees;
                    rptDegrees.DataBind();
                }
            }
        }

        protected void rptSkills_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "RemoveSkill")
            {
                List<SkillRequirement> skills = GetSkillRequirements();
                if (skills.Count > 1)
                {
                    skills.RemoveAt(Convert.ToInt32(e.CommandArgument));
                    rptSkills.DataSource = skills;
                    rptSkills.DataBind();
                }
            }
        }

        /// <summary>
        /// Retrieves degree requirements from the Repeater.
        /// </summary>
        private List<DegreeRequirement> GetDegreeRequirements()
        {
            List<DegreeRequirement> degrees = new List<DegreeRequirement>();
            foreach (RepeaterItem item in rptDegrees.Items)
            {
                DropDownList ddlDegree = item.FindControl("ddlDegree") as DropDownList;
                TextBox txtMinPercentage = item.FindControl("txtMinPercentage") as TextBox;

                if (ddlDegree != null && txtMinPercentage != null)
                {
                    string degreeID = ddlDegree.SelectedValue;
                    decimal? minPerc = null;
                    if (decimal.TryParse(txtMinPercentage.Text.Trim(), out decimal parsedPerc))
                    {
                        minPerc = parsedPerc;
                    }

                    degrees.Add(new DegreeRequirement
                    {
                        DegreeID = degreeID,
                        MinimumPercentage = minPerc
                    });
                }
            }
            return degrees;
        }

        /// <summary>
        /// Retrieves skill requirements from the Repeater.
        /// </summary>
        private List<SkillRequirement> GetSkillRequirements()
        {
            List<SkillRequirement> skills = new List<SkillRequirement>();
            foreach (RepeaterItem item in rptSkills.Items)
            {
                DropDownList ddlSkill = item.FindControl("ddlSkill") as DropDownList;
                DropDownList ddlExpertise = item.FindControl("ddlExpertise") as DropDownList;

                if (ddlSkill != null && ddlExpertise != null)
                {
                    string skillTagID = ddlSkill.SelectedValue;
                    string skillLevel = ddlExpertise.SelectedValue;

                    skills.Add(new SkillRequirement
                    {
                        SkillTagID = skillTagID,
                        SkillLevel = skillLevel
                    });
                }
            }
            return skills;
        }

        /// <summary>
        /// Handles the Post Job button click event.
        /// Inserts a new job posting into the database along with degree and skill requirements.
        /// </summary>
        protected void btnPostJob_Click(object sender, EventArgs e)
        {
            // Validate main job fields
            if (string.IsNullOrEmpty(txtJobTitle.Text.Trim()) ||
                string.IsNullOrEmpty(txtDescription.Text.Trim()) ||
                string.IsNullOrEmpty(txtLocation.Text.Trim()) ||
                string.IsNullOrEmpty(txtSalary.Text.Trim()) ||
                string.IsNullOrEmpty(txtDeadline.Text.Trim()) ||
                string.IsNullOrEmpty(ddlEmploymentType.SelectedValue) ||
                string.IsNullOrEmpty(ddlDepartment.SelectedValue))
            {
                lblStatus.Text = "Please fill in all required fields.";
                lblStatus.CssClass = "mt-2 text-danger";
                return;
            }

            // Parse EmployerID from session
            if (!int.TryParse(Session["EmployerID"].ToString(), out int employerID))
            {
                lblStatus.Text = "Invalid session data. Please log in again.";
                lblStatus.CssClass = "mt-2 text-danger";
                Response.Redirect("EmployerLogin.aspx");
                return;
            }

            // Parse salary
            if (!decimal.TryParse(txtSalary.Text.Trim(), out decimal salary))
            {
                lblStatus.Text = "Invalid salary format.";
                lblStatus.CssClass = "mt-2 text-danger";
                return;
            }

            // Parse deadline
            if (!DateTime.TryParse(txtDeadline.Text.Trim(), out DateTime deadline))
            {
                lblStatus.Text = "Invalid deadline date.";
                lblStatus.CssClass = "mt-2 text-danger";
                return;
            }

            // Begin database transaction
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlTransaction transaction = conn.BeginTransaction();
                int insertedJobID = 0;

                try
                {
                    // Insert into JobPosting
                    string insertJobQuery = @"
                        INSERT INTO JobPosting 
                            (EmployerID, Title, Description, Location, Salary, ApplicationDeadline, JobType, Status, JobCategory, RequiredYearsOfExperience)
                        VALUES 
                            (@EmployerID, @JobTitle, @Description, @Location, @Salary, @ApplicationDeadline, @JobType, @Status, @JobCategory, @RequiredYearsOfExperience);
                        SELECT CAST(scope_identity() AS int);";

                    using (SqlCommand cmdJob = new SqlCommand(insertJobQuery, conn, transaction))
                    {
                        cmdJob.Parameters.AddWithValue("@EmployerID", employerID);
                        cmdJob.Parameters.AddWithValue("@JobTitle", txtJobTitle.Text.Trim());
                        cmdJob.Parameters.AddWithValue("@Description", txtDescription.Text.Trim());
                        cmdJob.Parameters.AddWithValue("@Location", txtLocation.Text.Trim());
                        cmdJob.Parameters.AddWithValue("@Salary", salary);
                        cmdJob.Parameters.AddWithValue("@ApplicationDeadline", deadline);
                        cmdJob.Parameters.AddWithValue("@JobType", ddlEmploymentType.SelectedValue);
                        cmdJob.Parameters.AddWithValue("@Status", "Active"); // Default status
                        cmdJob.Parameters.AddWithValue("@JobCategory", ddlDepartment.SelectedValue);
                        cmdJob.Parameters.AddWithValue("@RequiredYearsOfExperience", 0); // Assuming 0 for simplicity

                        // Execute and get the inserted JobID
                        insertedJobID = (int)cmdJob.ExecuteScalar();
                    }

                    // Insert Degree Requirements
                    var degreeEntries = GetDegreeRequirements();
                    foreach (var degree in degreeEntries)
                    {
                        if (!string.IsNullOrEmpty(degree.DegreeID) && degree.MinimumPercentage.HasValue)
                        {
                            string insertDegreeQuery = @"
                                INSERT INTO JobPostingDegree (JobID, DegreeID, MinimumPercentage)
                                VALUES (@JobID, @DegreeID, @MinimumPercentage)";

                            using (SqlCommand cmdDegree = new SqlCommand(insertDegreeQuery, conn, transaction))
                            {
                                cmdDegree.Parameters.AddWithValue("@JobID", insertedJobID);
                                cmdDegree.Parameters.AddWithValue("@DegreeID", degree.DegreeID);
                                cmdDegree.Parameters.AddWithValue("@MinimumPercentage", degree.MinimumPercentage.Value);
                                cmdDegree.ExecuteNonQuery();
                            }
                        }
                    }

                    // Insert Skill Requirements
                    var skillEntries = GetSkillRequirements();
                    foreach (var skill in skillEntries)
                    {
                        if (!string.IsNullOrEmpty(skill.SkillTagID) && !string.IsNullOrEmpty(skill.SkillLevel))
                        {
                            string insertSkillQuery = @"
                                INSERT INTO JobSkill (JobID, SkillTagID, SkillLevel)
                                VALUES (@JobID, @SkillTagID, @SkillLevel)";

                            using (SqlCommand cmdSkill = new SqlCommand(insertSkillQuery, conn, transaction))
                            {
                                cmdSkill.Parameters.AddWithValue("@JobID", insertedJobID);
                                cmdSkill.Parameters.AddWithValue("@SkillTagID", skill.SkillTagID);
                                cmdSkill.Parameters.AddWithValue("@SkillLevel", int.Parse(skill.SkillLevel));
                                cmdSkill.ExecuteNonQuery();
                            }
                        }
                    }

                    // Commit transaction
                    transaction.Commit();

                    lblStatus.Text = "Job posted successfully!";
                    lblStatus.CssClass = "mt-2 text-success";
                    ClearJobForm();
                    // Optionally, refresh any grids or lists displaying job postings
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    lblStatus.Text = "An error occurred while posting the job.";
                    lblStatus.CssClass = "mt-2 text-danger";
                    // Log error
                }
            }
        }

        /// <summary>
        /// Clears the job posting form fields.
        /// </summary>
        private void ClearJobForm()
        {
            txtJobTitle.Text = "";
            txtDescription.Text = "";
            txtLocation.Text = "";
            txtSalary.Text = "";
            txtDeadline.Text = "";
            ddlEmploymentType.SelectedIndex = 0;
            ddlDepartment.SelectedIndex = 0;

            // Reset Degree Requirements Repeater
            InitializeDegreeRepeater();

            // Reset Skill Requirements Repeater
            InitializeSkillRepeater();
        }

        // Define DegreeRequirement and SkillRequirement classes
        public class DegreeRequirement
        {
            public string DegreeID { get; set; }
            public decimal? MinimumPercentage { get; set; }
        }

        public class SkillRequirement
        {
            public string SkillTagID { get; set; }
            public string SkillLevel { get; set; }
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

        /// <summary>
        /// Loads job seekers into the dropdown for messaging.
        /// </summary>
        private void LoadJobSeekers()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "SELECT JobSeekerID, FirstName + ' ' + LastName AS FullName FROM JobSeeker ORDER BY FullName";
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
                    catch (Exception ex)
                    {
                        // Log error
                        lblChatMessage.Text = "An error occurred while loading job seekers.";
                        lblChatMessage.CssClass = "mt-2 text-danger";
                    }
                }
            }
        }

        /// <summary>
        /// Handles the Send Message button click event.
        /// </summary>
        protected void btnSendMessage_Click(object sender, EventArgs e)
        {
            if (Session["EmployerID"] == null)
            {
                lblChatMessage.Text = "Session expired. Please log in again.";
                lblChatMessage.CssClass = "mt-2 text-danger";
                return;
            }

            if (!int.TryParse(Session["EmployerID"].ToString(), out int employerID))
            {
                lblChatMessage.Text = "Invalid session data.";
                lblChatMessage.CssClass = "mt-2 text-danger";
                return;
            }

            if (string.IsNullOrEmpty(ddlJobSeekers.SelectedValue) || string.IsNullOrEmpty(txtChatMessage.Text.Trim()))
            {
                lblChatMessage.Text = "Please select a job seeker and enter a message.";
                lblChatMessage.CssClass = "mt-2 text-danger";
                return;
            }

            if (!int.TryParse(ddlJobSeekers.SelectedValue, out int jobSeekerID))
            {
                lblChatMessage.Text = "Invalid job seeker selection.";
                lblChatMessage.CssClass = "mt-2 text-danger";
                return;
            }

            string messageContent = txtChatMessage.Text.Trim();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = @"
                    INSERT INTO ChatMessages 
                        (EmployerID, JobSeekerID, Message, MessageTime)
                    VALUES 
                        (@EmployerID, @JobSeekerID, @MessageContent, @MessageTime)";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@EmployerID", employerID);
                    cmd.Parameters.AddWithValue("@JobSeekerID", jobSeekerID);
                    cmd.Parameters.AddWithValue("@MessageContent", messageContent);
                    cmd.Parameters.AddWithValue("@MessageTime", DateTime.Now);

                    try
                    {
                        conn.Open();
                        cmd.ExecuteNonQuery();

                        lblChatMessage.Text = "Message sent successfully!";
                        lblChatMessage.CssClass = "mt-2 text-success";
                        txtChatMessage.Text = "";
                    }
                    catch (Exception ex)
                    {
                        lblChatMessage.Text = "An error occurred while sending the message.";
                        lblChatMessage.CssClass = "mt-2 text-danger";
                        // Log error
                    }
                }
            }
        }
    }
}
