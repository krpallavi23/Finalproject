using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace OnlineJobPortal
{
    public partial class JobSeekerProfile : Page
    {
        // Connection string from Web.config
        private string connectionString = ConfigurationManager.ConnectionStrings["OnlineJobPortalDB"].ConnectionString;

        // Properties to hold dynamic lists for Academic Details and Skills
        private List<JobSeekerAcademicDetail> JobSeekerAcademicDetails
        {
            get
            {
                if (ViewState["JobSeekerAcademicDetails"] == null)
                {
                    ViewState["JobSeekerAcademicDetails"] = new List<JobSeekerAcademicDetail>();
                }
                return (List<JobSeekerAcademicDetail>)ViewState["JobSeekerAcademicDetails"];
            }
            set
            {
                ViewState["JobSeekerAcademicDetails"] = value;
            }
        }

        private List<JobSeekerSkill> JobSeekerSkills
        {
            get
            {
                if (ViewState["JobSeekerSkills"] == null)
                {
                    ViewState["JobSeekerSkills"] = new List<JobSeekerSkill>();
                }
                return (List<JobSeekerSkill>)ViewState["JobSeekerSkills"];
            }
            set
            {
                ViewState["JobSeekerSkills"] = value;
            }
        }

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
                    LoadJobSeekerProfile(); // Load the profile details
                    LoadAcademicDetails(); // Load existing academic details
                    LoadSkillRequirements(); // Load existing skill requirements
                }
            }
        }

        // Method to load the Job Seeker's profile details
        private void LoadJobSeekerProfile()
        {
            int jobSeekerID = Convert.ToInt32(Session["JobSeekerID"]);

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = @"SELECT FirstName, LastName, DateOfBirth, Gender, YearsOfExperience, 
                                    AddressLine1, AddressLine2, City, State, Country, PostalCode 
                                 FROM JobSeeker 
                                 WHERE JobSeekerID = @JobSeekerID";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@JobSeekerID", jobSeekerID);
                    conn.Open();

                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        // Set labels with profile details
                        lblFirstName.Text = reader["FirstName"].ToString();
                        lblLastName.Text = reader["LastName"].ToString();
                        lblDateOfBirth.Text = Convert.ToDateTime(reader["DateOfBirth"]).ToString("yyyy-MM-dd");
                        lblGender.Text = reader["Gender"].ToString();
                        lblExperience.Text = reader["YearsOfExperience"].ToString();
                        lblAddressLine1.Text = reader["AddressLine1"].ToString();
                        lblAddressLine2.Text = reader["AddressLine2"].ToString();
                        lblCity.Text = reader["City"].ToString();
                        lblState.Text = reader["State"].ToString();
                        lblCountry.Text = reader["Country"].ToString();
                        lblPostalCode.Text = reader["PostalCode"].ToString();

                        // Set textboxes with profile details for editing
                        txtFirstName.Text = reader["FirstName"].ToString();
                        txtLastName.Text = reader["LastName"].ToString();
                        txtDateOfBirth.Text = Convert.ToDateTime(reader["DateOfBirth"]).ToString("yyyy-MM-dd");
                        ddlGender.SelectedValue = reader["Gender"].ToString();
                        txtExperience.Text = reader["YearsOfExperience"].ToString();
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
                        lblMessage.CssClass = "text-danger";
                        lblMessage.Visible = true;
                    }
                    reader.Close();
                }
            }
        }

        // Handle Edit button click
        protected void btnEdit_Click(object sender, EventArgs e)
        {
            ToggleEditMode(true);
        }

        // Handle Cancel button click
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            LoadJobSeekerProfile(); // Reload original details
            ToggleEditMode(false);
            lblMessage.Visible = false;
        }

        // Toggle between view and edit modes
        private void ToggleEditMode(bool isEditMode)
        {
            if (isEditMode)
            {
                // Show textboxes and hide labels
                lblFirstName.Visible = false;
                txtFirstName.Visible = true;

                lblLastName.Visible = false;
                txtLastName.Visible = true;

                lblDateOfBirth.Visible = false;
                txtDateOfBirth.Visible = true;

                lblGender.Visible = false;
                ddlGender.Visible = true;

                lblExperience.Visible = false;
                txtExperience.Visible = true;

                lblAddressLine1.Visible = false;
                txtAddressLine1.Visible = true;

                lblAddressLine2.Visible = false;
                txtAddressLine2.Visible = true;

                lblCity.Visible = false;
                txtCity.Visible = true;

                lblState.Visible = false;
                txtState.Visible = true;

                lblCountry.Visible = false;
                txtCountry.Visible = true;

                lblPostalCode.Visible = false;
                txtPostalCode.Visible = true;

                // Show Update and Cancel buttons, hide Edit button
                btnEdit.Visible = false;
                btnUpdateProfile.Visible = true;
                btnCancel.Visible = true;
            }
            else
            {
                // Hide textboxes and show labels
                lblFirstName.Visible = true;
                txtFirstName.Visible = false;

                lblLastName.Visible = true;
                txtLastName.Visible = false;

                lblDateOfBirth.Visible = true;
                txtDateOfBirth.Visible = false;

                lblGender.Visible = true;
                ddlGender.Visible = false;

                lblExperience.Visible = true;
                txtExperience.Visible = false;

                lblAddressLine1.Visible = true;
                txtAddressLine1.Visible = false;

                lblAddressLine2.Visible = true;
                txtAddressLine2.Visible = false;

                lblCity.Visible = true;
                txtCity.Visible = false;

                lblState.Visible = true;
                txtState.Visible = false;

                lblCountry.Visible = true;
                txtCountry.Visible = false;

                lblPostalCode.Visible = true;
                txtPostalCode.Visible = false;

                // Show Edit button, hide Update and Cancel buttons
                btnEdit.Visible = true;
                btnUpdateProfile.Visible = false;
                btnCancel.Visible = false;
            }
        }

        // Handle Update Profile button click
        protected void btnUpdateProfile_Click(object sender, EventArgs e)
        {
            int jobSeekerID = Convert.ToInt32(Session["JobSeekerID"]);

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = @"UPDATE JobSeeker 
                                 SET FirstName = @FirstName, 
                                     LastName = @LastName, 
                                     DateOfBirth = @DateOfBirth, 
                                     Gender = @Gender, 
                                     YearsOfExperience = @YearsOfExperience, 
                                     AddressLine1 = @AddressLine1, 
                                     AddressLine2 = @AddressLine2, 
                                     City = @City, 
                                     State = @State, 
                                     Country = @Country, 
                                     PostalCode = @PostalCode 
                                 WHERE JobSeekerID = @JobSeekerID";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@FirstName", txtFirstName.Text.Trim());
                    cmd.Parameters.AddWithValue("@LastName", txtLastName.Text.Trim());
                    cmd.Parameters.AddWithValue("@DateOfBirth", Convert.ToDateTime(txtDateOfBirth.Text.Trim()));
                    cmd.Parameters.AddWithValue("@Gender", ddlGender.SelectedValue);
                    cmd.Parameters.AddWithValue("@YearsOfExperience", Convert.ToInt32(txtExperience.Text.Trim()));
                    cmd.Parameters.AddWithValue("@AddressLine1", txtAddressLine1.Text.Trim());
                    cmd.Parameters.AddWithValue("@AddressLine2", txtAddressLine2.Text.Trim());
                    cmd.Parameters.AddWithValue("@City", txtCity.Text.Trim());
                    cmd.Parameters.AddWithValue("@State", txtState.Text.Trim());
                    cmd.Parameters.AddWithValue("@Country", txtCountry.Text.Trim());
                    cmd.Parameters.AddWithValue("@PostalCode", txtPostalCode.Text.Trim());
                    cmd.Parameters.AddWithValue("@JobSeekerID", jobSeekerID);

                    conn.Open();
                    int rowsAffected = cmd.ExecuteNonQuery();
                    conn.Close();

                    if (rowsAffected > 0)
                    {
                        lblMessage.Text = "Profile updated successfully.";
                        lblMessage.CssClass = "text-success";
                        lblMessage.Visible = true;

                        LoadJobSeekerProfile(); // Reload updated details
                        ToggleEditMode(false);
                    }
                    else
                    {
                        lblMessage.Text = "Profile update failed.";
                        lblMessage.CssClass = "text-danger";
                        lblMessage.Visible = true;
                    }
                }
            }
        }

        // Load existing Academic Details
        private void LoadAcademicDetails()
        {
            int jobSeekerID = Convert.ToInt32(Session["JobSeekerID"]);
            List<JobSeekerAcademicDetail> academicDetails = new List<JobSeekerAcademicDetail>();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = @"SELECT DegreeID, EducationLevel, Marks, BoardOrUniversity, YearOfCompletion 
                                 FROM AcademicDetails 
                                 WHERE JobSeekerID = @JobSeekerID";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@JobSeekerID", jobSeekerID);
                    conn.Open();

                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        academicDetails.Add(new JobSeekerAcademicDetail
                        {
                            DegreeID = Convert.ToInt32(reader["DegreeID"]),
                            EducationLevel = reader["EducationLevel"].ToString(),
                            Marks = reader["Marks"].ToString(),
                            BoardOrUniversity = reader["BoardOrUniversity"].ToString(),
                            YearOfCompletion = reader["YearOfCompletion"].ToString()
                        });
                    }
                    reader.Close();
                }
            }

            JobSeekerAcademicDetails = academicDetails;
            rptAcademicDetails.DataSource = JobSeekerAcademicDetails;
            rptAcademicDetails.DataBind();
        }

        // Load existing Skill Requirements
        private void LoadSkillRequirements()
        {
            int jobSeekerID = Convert.ToInt32(Session["JobSeekerID"]);
            List<JobSeekerSkill> skills = new List<JobSeekerSkill>();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = @"SELECT SkillTagID, SkillLevel 
                                 FROM JobSeekerSkill 
                                 WHERE JobSeekerID = @JobSeekerID";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@JobSeekerID", jobSeekerID);
                    conn.Open();

                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        skills.Add(new JobSeekerSkill
                        {
                            SkillTagID = Convert.ToInt32(reader["SkillTagID"]),
                            SkillLevel = reader["SkillLevel"].ToString()
                        });
                    }
                    reader.Close();
                }
            }

            JobSeekerSkills = skills;
            rptSkills.DataSource = JobSeekerSkills;
            rptSkills.DataBind();
        }

        // Handle Add Degree button click
        protected void btnAddDegree_Click(object sender, EventArgs e)
        {
            JobSeekerAcademicDetails.Add(new JobSeekerAcademicDetail());
            rptAcademicDetails.DataSource = JobSeekerAcademicDetails;
            rptAcademicDetails.DataBind();
        }

        // Handle Remove Degree command
        protected void rptAcademicDetails_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "RemoveDegree")
            {
                int index = Convert.ToInt32(e.CommandArgument);
                if (index >= 0 && index < JobSeekerAcademicDetails.Count)
                {
                    JobSeekerAcademicDetails.RemoveAt(index);
                    rptAcademicDetails.DataSource = JobSeekerAcademicDetails;
                    rptAcademicDetails.DataBind();
                }
            }
        }

        // Handle ItemDataBound for Academic Details Repeater
        protected void rptAcademicDetails_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                JobSeekerAcademicDetail academicDetail = (JobSeekerAcademicDetail)e.Item.DataItem;
                DropDownList ddlDegree = (DropDownList)e.Item.FindControl("ddlDegree");
                DropDownList ddlEducationLevel = (DropDownList)e.Item.FindControl("ddlEducationLevel");
                TextBox txtMarks = (TextBox)e.Item.FindControl("txtMarks");
                TextBox txtBoardUniversity = (TextBox)e.Item.FindControl("txtBoardUniversity");
                TextBox txtYearOfCompletion = (TextBox)e.Item.FindControl("txtYearOfCompletion");

                if (ddlDegree != null)
                {
                    BindDegreeDropdown(ddlDegree);
                    if (academicDetail.DegreeID > 0)
                    {
                        ddlDegree.SelectedValue = academicDetail.DegreeID.ToString();
                    }
                }

                if (ddlEducationLevel != null)
                {
                    if (!string.IsNullOrEmpty(academicDetail.EducationLevel))
                    {
                        ddlEducationLevel.SelectedValue = academicDetail.EducationLevel;
                    }
                }

                if (txtMarks != null)
                {
                    txtMarks.Text = academicDetail.Marks;
                }

                if (txtBoardUniversity != null)
                {
                    txtBoardUniversity.Text = academicDetail.BoardOrUniversity;
                }

                if (txtYearOfCompletion != null)
                {
                    txtYearOfCompletion.Text = academicDetail.YearOfCompletion;
                }
            }
        }

        // Bind Degree dropdown for a specific DropDownList
        private void BindDegreeDropdown(DropDownList ddl)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = "SELECT DegreeID, DegreeName FROM Degree";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    ddl.DataSource = reader;
                    ddl.DataTextField = "DegreeName";
                    ddl.DataValueField = "DegreeID";
                    ddl.DataBind();
                    ddl.Items.Insert(0, new ListItem("Select Degree", ""));
                    con.Close();
                }
            }
        }

        // Handle Add Skill button click
        protected void btnAddSkill_Click(object sender, EventArgs e)
        {
            JobSeekerSkills.Add(new JobSeekerSkill());
            rptSkills.DataSource = JobSeekerSkills;
            rptSkills.DataBind();
        }

        // Handle Remove Skill command
        protected void rptSkills_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "RemoveSkill")
            {
                int index = Convert.ToInt32(e.CommandArgument);
                if (index >= 0 && index < JobSeekerSkills.Count)
                {
                    JobSeekerSkills.RemoveAt(index);
                    rptSkills.DataSource = JobSeekerSkills;
                    rptSkills.DataBind();
                }
            }
        }

        // Handle ItemDataBound for Skills Repeater
        protected void rptSkills_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                JobSeekerSkill skill = (JobSeekerSkill)e.Item.DataItem;
                DropDownList ddlSkill = (DropDownList)e.Item.FindControl("ddlSkill");
                DropDownList ddlSkillLevel = (DropDownList)e.Item.FindControl("ddlSkillLevel");

                if (ddlSkill != null)
                {
                    BindSkillDropdown(ddlSkill);
                    if (skill.SkillTagID > 0)
                    {
                        ddlSkill.SelectedValue = skill.SkillTagID.ToString();
                    }
                }

                if (ddlSkillLevel != null)
                {
                    if (!string.IsNullOrEmpty(skill.SkillLevel))
                    {
                        ddlSkillLevel.SelectedValue = skill.SkillLevel;
                    }
                }
            }
        }

        // Bind Skill dropdown for a specific DropDownList
        private void BindSkillDropdown(DropDownList ddl)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = "SELECT SkillTagID, SkillName FROM SkillTag";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    ddl.DataSource = reader;
                    ddl.DataTextField = "SkillName";
                    ddl.DataValueField = "SkillTagID";
                    ddl.DataBind();
                    ddl.Items.Insert(0, new ListItem("Select Skill", ""));
                    con.Close();
                }
            }
        }

        // Override OnPreRender to save Degrees and Skills before rendering
        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            // Save Academic Details from Repeater
            List<JobSeekerAcademicDetail> academicDetails = new List<JobSeekerAcademicDetail>();
            foreach (RepeaterItem item in rptAcademicDetails.Items)
            {
                if (item.ItemType == ListItemType.Item || item.ItemType == ListItemType.AlternatingItem)
                {
                    DropDownList ddlDegree = (DropDownList)item.FindControl("ddlDegree");
                    DropDownList ddlEducationLevel = (DropDownList)item.FindControl("ddlEducationLevel");
                    TextBox txtMarks = (TextBox)item.FindControl("txtMarks");
                    TextBox txtBoardUniversity = (TextBox)item.FindControl("txtBoardUniversity");
                    TextBox txtYearOfCompletion = (TextBox)item.FindControl("txtYearOfCompletion");

                    JobSeekerAcademicDetail academicDetail = new JobSeekerAcademicDetail
                    {
                        DegreeID = ddlDegree != null && !string.IsNullOrEmpty(ddlDegree.SelectedValue) ? Convert.ToInt32(ddlDegree.SelectedValue) : 0,
                        EducationLevel = ddlEducationLevel != null ? ddlEducationLevel.SelectedValue : string.Empty,
                        Marks = txtMarks != null ? txtMarks.Text.Trim() : string.Empty,
                        BoardOrUniversity = txtBoardUniversity != null ? txtBoardUniversity.Text.Trim() : string.Empty,
                        YearOfCompletion = txtYearOfCompletion != null ? txtYearOfCompletion.Text.Trim() : string.Empty
                    };
                    academicDetails.Add(academicDetail);
                }
            }
            JobSeekerAcademicDetails = academicDetails;

            // Save Skills from Repeater
            List<JobSeekerSkill> skills = new List<JobSeekerSkill>();
            foreach (RepeaterItem item in rptSkills.Items)
            {
                if (item.ItemType == ListItemType.Item || item.ItemType == ListItemType.AlternatingItem)
                {
                    DropDownList ddlSkill = (DropDownList)item.FindControl("ddlSkill");
                    DropDownList ddlSkillLevel = (DropDownList)item.FindControl("ddlSkillLevel");

                    JobSeekerSkill skill = new JobSeekerSkill
                    {
                        SkillTagID = ddlSkill != null && !string.IsNullOrEmpty(ddlSkill.SelectedValue) ? Convert.ToInt32(ddlSkill.SelectedValue) : 0,
                        SkillLevel = ddlSkillLevel != null ? ddlSkillLevel.SelectedValue : string.Empty
                    };
                    skills.Add(skill);
                }
            }
            JobSeekerSkills = skills;
        }

        // Handle Save Academic and Skill Requirements button click
        protected void btnSaveRequirements_Click(object sender, EventArgs e)
        {
            int jobSeekerID = Convert.ToInt32(Session["JobSeekerID"]);

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlTransaction transaction = conn.BeginTransaction();

                try
                {
                    // ----- Handle Academic Details -----
                    // Delete existing Academic Details
                    string deleteAcademic = "DELETE FROM AcademicDetails WHERE JobSeekerID = @JobSeekerID";
                    using (SqlCommand cmd = new SqlCommand(deleteAcademic, conn, transaction))
                    {
                        cmd.Parameters.AddWithValue("@JobSeekerID", jobSeekerID);
                        cmd.ExecuteNonQuery();
                    }

                    // Insert updated Academic Details
                    string insertAcademic = @"INSERT INTO AcademicDetails (JobSeekerID, DegreeID, EducationLevel, Marks, BoardOrUniversity, YearOfCompletion) 
                                              VALUES (@JobSeekerID, @DegreeID, @EducationLevel, @Marks, @BoardOrUniversity, @YearOfCompletion)";
                    foreach (var academic in JobSeekerAcademicDetails)
                    {
                        if (academic.DegreeID > 0 && !string.IsNullOrEmpty(academic.EducationLevel) && !string.IsNullOrEmpty(academic.Marks))
                        {
                            using (SqlCommand cmd = new SqlCommand(insertAcademic, conn, transaction))
                            {
                                cmd.Parameters.AddWithValue("@JobSeekerID", jobSeekerID);
                                cmd.Parameters.AddWithValue("@DegreeID", academic.DegreeID);
                                cmd.Parameters.AddWithValue("@EducationLevel", academic.EducationLevel);
                                cmd.Parameters.AddWithValue("@Marks", Convert.ToDecimal(academic.Marks));
                                cmd.Parameters.AddWithValue("@BoardOrUniversity", string.IsNullOrEmpty(academic.BoardOrUniversity) ? (object)DBNull.Value : academic.BoardOrUniversity);
                                cmd.Parameters.AddWithValue("@YearOfCompletion", string.IsNullOrEmpty(academic.YearOfCompletion) ? (object)DBNull.Value : academic.YearOfCompletion);
                                cmd.ExecuteNonQuery();
                            }
                        }
                    }

                    // ----- Handle Skill Requirements -----
                    // Delete existing Skills
                    string deleteSkills = "DELETE FROM JobSeekerSkill WHERE JobSeekerID = @JobSeekerID";
                    using (SqlCommand cmdSkill = new SqlCommand(deleteSkills, conn, transaction))
                    {
                        cmdSkill.Parameters.AddWithValue("@JobSeekerID", jobSeekerID);
                        cmdSkill.ExecuteNonQuery();
                    }

                    // Insert updated Skills
                    string insertSkill = @"INSERT INTO JobSeekerSkill (JobSeekerID, SkillTagID, SkillLevel) 
                                           VALUES (@JobSeekerID, @SkillTagID, @SkillLevel)";
                    foreach (var skill in JobSeekerSkills)
                    {
                        if (skill.SkillTagID > 0 && !string.IsNullOrEmpty(skill.SkillLevel))
                        {
                            using (SqlCommand cmd = new SqlCommand(insertSkill, conn, transaction))
                            {
                                cmd.Parameters.AddWithValue("@JobSeekerID", jobSeekerID);
                                cmd.Parameters.AddWithValue("@SkillTagID", skill.SkillTagID);
                                cmd.Parameters.AddWithValue("@SkillLevel", Convert.ToInt32(skill.SkillLevel));
                                cmd.ExecuteNonQuery();
                            }
                        }
                    }

                    transaction.Commit();
                    lblMessage.Text = "Academic and Skill requirements saved successfully.";
                    lblMessage.CssClass = "text-success";
                    lblMessage.Visible = true;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    lblMessage.Text = "Error saving requirements: " + ex.Message;
                    lblMessage.CssClass = "text-danger";
                    lblMessage.Visible = true;
                }
            }
        }

        // Handle Dashboard link click


        protected void lnkViewProfile_Click(object sender, EventArgs e)
        {
            Response.Redirect("JobSeekerProfile.aspx");
        }

        /// <summary>
        /// Handles the click event for accessing the inbox.
        /// </summary>
        protected void lnkInbox_Click(object sender, EventArgs e)
        {
            Response.Redirect("ChangeJobSeekerPassword.aspx");
        }

        /// <summary>
        /// Handles the click event for logging out the job seeker.
        /// </summary>
        protected void lnkLogout_Click(object sender, EventArgs e)
        {
            Session.Abandon();
            Response.Redirect("JobSeekerLogin.aspx");
        }

    }

    // Helper classes to represent Academic Detail and Skill
    [Serializable]
    public class JobSeekerAcademicDetail
    {
        public int DegreeID { get; set; }
        public string EducationLevel { get; set; }
        public string Marks { get; set; }
        public string BoardOrUniversity { get; set; }
        public string YearOfCompletion { get; set; }
    }

    [Serializable]
    public class JobSeekerSkill
    {
        public int SkillTagID { get; set; }
        public string SkillLevel { get; set; }
    }
}
