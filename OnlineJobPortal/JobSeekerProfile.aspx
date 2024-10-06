<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="JobSeekerProfile.aspx.cs" Inherits="OnlineJobPortal.JobSeekerProfile" %>

<!DOCTYPE html>
<html lang="en" dir="ltr">
<head runat="server">
    <!-- Title -->
    <title>Job-Seeker Dashboard</title>

    <!-- Meta -->
    <meta charset="utf-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge" />
    <meta name="author" content="DexignZone" />
    <meta name="robots" content="" />
    <meta name="keywords" content="job portal, job seeker dashboard, Bootstrap HTML template, job listings, applications, user-friendly interface, powerful functionalities" />
    <meta name="description" content="Qerza is a versatile job portal dashboard for job seekers powered by Bootstrap HTML. Explore job listings, manage applications, and enhance your job search process. User-friendly interface with powerful functionalities. Job portal, job seeker dashboard, Bootstrap HTML template." />
    <meta property="og:title" content="Qerza - Job Portal Job Seeker Dashboard | Bootstrap HTML Template" />
    <meta property="og:description" content="Qerza is a versatile job portal dashboard for job seekers powered by Bootstrap HTML. Explore job listings, manage applications, and enhance your job search process. User-friendly interface with powerful functionalities. Job portal, job seeker dashboard, Bootstrap HTML template." />
    <meta property="og:image" content="xhtml/social-image.png" />
    <meta name="format-detection" content="telephone=no" />

    <!-- Mobile Specific -->
    <meta name="viewport" content="width=device-width, initial-scale=1" />

    <!-- Favicon icon -->
    <link rel="icon" type="image/png" href="xhtml/images/logo.svg" />

    <!-- CSS Files -->
    <link href="xhtml/vendor/jqvmap/css/jqvmap.min.css" rel="stylesheet" />
    <link rel="stylesheet" href="xhtml/vendor/chartist/css/chartist.min.css" />
    <link href="xhtml/vendor/bootstrap-select/dist/css/bootstrap-select.min.css" rel="stylesheet" />
    <link href="xhtml/css/style.css" rel="stylesheet" />
    <!-- Fonts -->
    <link href="https://fonts.googleapis.com/css2?family=Poppins:wght@100;200;300;400;500;600;700;800;900&amp;family=Roboto:wght@100;300;400;500;700;900&amp;display=swap" rel="stylesheet" />
</head>
<body data-typography="poppins" data-theme-version="light" data-layout="vertical" data-nav-headerbg="color_1"
    data-headerbg="color_1" data-sidebar-style="mini" data-sidebarbg="color_1" data-sidebar-position="fixed"
    data-header-position="fixed" data-container="wide" direction="ltr" data-primary="color_1">
    <div id="preloader" style="display: none;">
        <div class="sk-three-bounce">
            <div class="sk-child sk-bounce1"></div>
            <div class="sk-child sk-bounce2"></div>
            <div class="sk-child sk-bounce3"></div>
        </div>
    </div>

    <!-- Main wrapper start -->
    <form id="form1" runat="server">
        <div id="Div1" class="show">
            <!-- Nav header start -->
            <div class="nav-header">
                <a href="JobSeekerDashboard.aspx" class="brand-logo">
                    <!-- Add your logo here -->
                    <img src="xhtml/images/newme3.png" alt="Logo" />
                </a>
                <div class="nav-control">
                    <div class="hamburger">
                        <span class="line"></span>
                        <span class="line"></span>
                        <span class="line"></span>
                    </div>
                </div>
            </div>

            <!-- Chat box start -->
            <div class="chatbox">
                <div class="chatbox-content">
                    <!-- Send a Message Section -->
                    <div class="card">
                        <div class="card-header">
                            <h5>Send a Message</h5>
                        </div>
                        <div class="card-body">
                            <!-- Job Posting Selection Dropdown -->
                            <div class="mb-3">
                                <label for="ddlJobPostings" class="form-label">Select Job Posting</label>
                                <asp:DropDownList ID="ddlJobPostings" runat="server" CssClass="form-select">
                                </asp:DropDownList>
                            </div>

                            <!-- Chat Message TextBox -->
                            <div class="mb-3">
                                <label for="txtChatMessage" class="form-label">Your Message</label>
                                <asp:TextBox ID="txtChatMessage" runat="server" TextMode="MultiLine" Rows="4"
                                    CssClass="form-control"></asp:TextBox>
                            </div>
                            <!-- Send Button -->
                            <asp:Button ID="btnSendMessage" runat="server" Text="Send" CssClass="btn btn-primary"
                                 />
                            <!-- Message Label -->
                            <asp:Label ID="Label1" runat="server" ForeColor="Red" CssClass="mt-2"></asp:Label>
                        </div>
                    </div>

                    <!-- End of Send a Message Section -->

                    <!-- Chat Messages Section -->
                    <div class="card mt-4">
                        <div class="card-header">
                            <h5>Your Messages</h5>
                        </div>
                        <div class="card-body">
                            <!-- Repeater for Chat Messages -->
                            <asp:Repeater ID="rptChatMessages" runat="server">
                                <ItemTemplate>
                                    <div>
                                        <strong><%# Eval("CompanyName") %>:</strong>
                                        <span><%# Eval("Message") %></span>
                                        <small><%# Eval("MessageTime", "{0:MM/dd/yyyy hh:mm tt}") %></small>
                                    </div>
                                </ItemTemplate>
                            </asp:Repeater>
                        </div>
                    </div>
                    <!-- End of Chat Messages Section -->
                </div>
            </div>
            <!-- Chat box End -->

            <div class="header">
                <div class="header-content">
                    <nav class="navbar navbar-expand">
                        <div class="collapse navbar-collapse justify-content-between">
                            <div class="header-left">
                                <div class="dashboard_bar">
                                    Dashboard
                                </div>
                            </div>
                            <ul class="navbar-nav header-right">

                                <!-- Theme mode toggle -->
                                <li class="nav-item dropdown notification_dropdown">
                                    <a class="nav-link bell dz-theme-mode p-0" href="javascript:void(0);">
                                        <i id="icon-light" class="fas fa-sun"></i>
                                        <i id="icon-dark" class="fas fa-moon"></i>
                                    </a>
                                </li>
                                <!-- Message -->
                                <li class="nav-item dropdown notification_dropdown">
                                    <a class="nav-link bell bell-link" href="javascript:void(0)">
                                        <svg width="28" height="28" viewBox="0 0 28 28" fill="none" xmlns="http://www.w3.org/2000/svg">
                                            <path d="M22.4605 3.84888H5.31688C4.64748 3.84961 4.00571 4.11586 3.53237 4.58919C3.05903 5.06253 2.79279 5.7043 2.79205 6.3737V18.1562C2.79279 18.8256 3.05903 19.4674 3.53237 19.9407C4.00571 20.4141 4.64748 20.6803 5.31688 20.6811C5.54005 20.6812 5.75404 20.7699 5.91184 20.9277C6.06964 21.0855 6.15836 21.2995 6.15849 21.5227V23.3168C6.15849 23.6215 6.24118 23.9204 6.39774 24.1818C6.5543 24.4431 6.77886 24.6571 7.04747 24.8009C7.31608 24.9446 7.61867 25.0128 7.92298 24.9981C8.22729 24.9834 8.52189 24.8863 8.77539 24.7173L14.6173 20.8224C14.7554 20.7299 14.918 20.6807 15.0842 20.6811H19.187C19.7383 20.68 20.2743 20.4994 20.7137 20.1664C21.1531 19.8335 21.4721 19.3664 21.6222 18.8359L24.8966 7.05011C24.9999 6.67481 25.0152 6.28074 24.9414 5.89856C24.8675 5.51637 24.7064 5.15639 24.4707 4.84663C24.235 4.53687 23.931 4.28568 23.5823 4.11263C23.2336 3.93957 22.8497 3.84931 22.4605 3.84888ZM23.2733 6.60304L20.0006 18.3847C19.95 18.5614 19.8432 18.7168 19.6964 18.8275C19.5496 18.9381 19.3708 18.9979 19.187 18.9978H15.0842C14.5856 18.9972 14.0981 19.1448 13.6837 19.4219L7.84171 23.3168V21.5227C7.84097 20.8533 7.57473 20.2115 7.10139 19.7382C6.62805 19.2648 5.98628 18.9986 5.31688 18.9978C5.09371 18.9977 4.87972 18.909 4.72192 18.7512C4.56412 18.5934 4.4754 18.3794 4.47527 18.1562V6.3737C4.4754 6.15054 4.56412 5.93655 4.72192 5.77874C4.87972 5.62094 5.09371 5.53223 5.31688 5.5321H22.4605C22.5905 5.53243 22.7188 5.56277 22.8353 5.62076C22.9517 5.67875 23.0532 5.76283 23.1318 5.86646C23.2105 5.97008 23.2642 6.09045 23.2887 6.21821C23.3132 6.34597 23.308 6.47766 23.2733 6.60304Z" fill="#3E4954"></path>
                                            <path d="M7.84173 11.4233H12.0498C12.273 11.4233 12.4871 11.3347 12.6449 11.1768C12.8027 11.019 12.8914 10.8049 12.8914 10.5817C12.8914 10.3585 12.8027 10.1444 12.6449 9.98661C12.4871 9.82878 12.273 9.74011 12.0498 9.74011H7.84173C7.61852 9.74011 7.40446 9.82878 7.24662 9.98661C7.08879 10.1444 7.00012 10.3585 7.00012 10.5817C7.00012 10.8049 7.08879 11.019 7.24662 11.1768C7.40446 11.3347 7.61852 11.4233 7.84173 11.4233Z" fill="#3E4954"></path>
                                            <path d="M15.4162 13.1066H7.84173C7.61852 13.1066 7.40446 13.1952 7.24662 13.3531C7.08879 13.5109 7.00012 13.725 7.00012 13.9482C7.00012 14.1714 7.08879 14.3855 7.24662 14.5433C7.40446 14.7011 7.61852 14.7898 7.84173 14.7898H15.4162C15.6394 14.7898 15.8535 14.7011 16.0113 14.5433C16.1692 14.3855 16.2578 14.1714 16.2578 13.9482C16.2578 13.725 16.1692 13.5109 16.0113 13.3531C15.8535 13.1952 15.6394 13.1066 15.4162 13.1066Z" fill="#3E4954"></path>
                                        </svg>
                                        <span class="badge light text-white bg-primary">3</span>
                                    </a>
                                </li>
                                <!-- User Profile Dropdown -->
                                <li class="nav-item dropdown header-profile">
                                    <a class="nav-link" href="javascript:void(0)" role="button" data-bs-toggle="dropdown">
                                        <div class="header-info">
                                            <span class="text-black"><strong>
                                                <asp:Label ID="lblJobSeekerName" runat="server" Text="John Doe"></asp:Label></strong></span>
                                            <p class="fs-12 mb-0">Job Seeker</p>
                                        </div>
                                        <img src="xhtml/images/profile/pro_1.png" width="20" alt="Profile" />
                                    </a>
                                    <div class="dropdown-menu dropdown-menu-end">
                                        <asp:LinkButton ID="lnkViewProfile" runat="server" CssClass="dropdown-item" OnClick="lnkViewProfile_Click">
                                            <i class="fas fa-user"></i><span class="ms-2">Profile</span>
                                        </asp:LinkButton>
                                        <asp:LinkButton ID="lnkInbox" runat="server" CssClass="dropdown-item" OnClick="lnkInbox_Click">
                                            <i class="fas fa-envelope"></i><span class="ms-2">Change Password</span>
                                        </asp:LinkButton>
                                        <asp:LinkButton ID="lnkLogout" runat="server" CssClass="dropdown-item" OnClick="lnkLogout_Click">
                                            <i class="fas fa-sign-out-alt"></i><span class="ms-2">Logout</span>
                                        </asp:LinkButton>
                                    </div>
                                </li>
                            </ul>
                        </div>
                    </nav>
                </div>
            </div>
            <!-- Header end -->

            <!-- Sidebar start -->
            <div class="deznav">
                <div class="deznav-scroll">
                    <ul class="metismenu" id="menu">
                        <li>
                            <a href="SearchJob.aspx" class="ai-icon">
                                <i class="flaticon-381-search"></i>
                                <span class="nav-text">Search Jobs</span>
                            </a>
                        </li>
                        <li>
                            <a class="has-arrow ai-icon" href="javascript:void(0);" aria-expanded="false">
                                <i class="flaticon-381-briefcase"></i>
                                <span class="nav-text">Job Application</span>
                            </a>
                            <ul aria-expanded="false">
                                <li><a href="AllJobApplications.aspx?status=All">All</a></li>
                                <li><a href="AppliedJob.aspx?status=Applied">Applied</a></li>
                                <li><a href="ShortlistedJob.aspx?status=Shortlisted">Shortlisted</a></li>
                                <li><a href="SelectedJob.aspx?status=Selected">Selected</a></li>
                                <li><a href="RejectedJobs.aspx?status=Rejected">Rejected</a></li>
                            </ul>
                        </li>
                    </ul>
                </div>
            </div>
            <!-- Sidebar end -->
            <!-- Main Content Start -->
            <div class="content-body">
                <div class="container-fluid">
                    <h2 class="text-center mb-3">Job Seeker Profile</h2>
                    <asp:Label ID="lblMessage" runat="server" CssClass="text-danger text-center mb-3" Visible="false"></asp:Label>

                    <!-- Job Seeker Profile Panel -->
                    <div class="card mb-3">
                        <div class="card-header">
                            <h5 class="card-title mb-0">Personal Details</h5>
                        </div>
                        <div class="card-body">
                            <div class="row g-2">
                                <!-- First Name -->
                                <div class="col-md-6">
                                    <label class="form-label">First Name</label>
                                    <asp:Label ID="lblFirstName" runat="server" CssClass="form-control-plaintext"></asp:Label>
                                    <asp:TextBox ID="txtFirstName" runat="server" CssClass="form-control d-none"></asp:TextBox>
                                </div>
                                <!-- Last Name -->
                                <div class="col-md-6">
                                    <label class="form-label">Last Name</label>
                                    <asp:Label ID="lblLastName" runat="server" CssClass="form-control-plaintext"></asp:Label>
                                    <asp:TextBox ID="txtLastName" runat="server" CssClass="form-control d-none"></asp:TextBox>
                                </div>
                                <!-- Date of Birth -->
                                <div class="col-md-6">
                                    <label class="form-label">Date of Birth</label>
                                    <asp:Label ID="lblDateOfBirth" runat="server" CssClass="form-control-plaintext"></asp:Label>
                                    <asp:TextBox ID="txtDateOfBirth" runat="server" CssClass="form-control d-none" type="date"></asp:TextBox>
                                </div>
                                <!-- Gender -->
                                <div class="col-md-6">
                                    <label class="form-label">Gender</label>
                                    <asp:Label ID="lblGender" runat="server" CssClass="form-control-plaintext"></asp:Label>
                                    <asp:DropDownList ID="ddlGender" runat="server" CssClass="form-select d-none">
                                        <asp:ListItem Value="">-- Select Gender --</asp:ListItem>
                                        <asp:ListItem Value="Male">Male</asp:ListItem>
                                        <asp:ListItem Value="Female">Female</asp:ListItem>
                                        <asp:ListItem Value="Other">Other</asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                                <!-- Years of Experience -->
                                <div class="col-md-6">
                                    <label class="form-label">Years of Experience</label>
                                    <asp:Label ID="lblExperience" runat="server" CssClass="form-control-plaintext"></asp:Label>
                                    <asp:TextBox ID="txtExperience" runat="server" CssClass="form-control d-none" type="number" min="0"></asp:TextBox>
                                </div>
                                <!-- Address Line 1 -->
                                <div class="col-md-6">
                                    <label class="form-label">Address Line 1</label>
                                    <asp:Label ID="lblAddressLine1" runat="server" CssClass="form-control-plaintext"></asp:Label>
                                    <asp:TextBox ID="txtAddressLine1" runat="server" CssClass="form-control d-none"></asp:TextBox>
                                </div>
                                <!-- Address Line 2 -->
                                <div class="col-md-6">
                                    <label class="form-label">Address Line 2</label>
                                    <asp:Label ID="lblAddressLine2" runat="server" CssClass="form-control-plaintext"></asp:Label>
                                    <asp:TextBox ID="txtAddressLine2" runat="server" CssClass="form-control d-none"></asp:TextBox>
                                </div>
                                <!-- City -->
                                <div class="col-md-4">
                                    <label class="form-label">City</label>
                                    <asp:Label ID="lblCity" runat="server" CssClass="form-control-plaintext"></asp:Label>
                                    <asp:TextBox ID="txtCity" runat="server" CssClass="form-control d-none"></asp:TextBox>
                                </div>
                                <!-- State -->
                                <div class="col-md-4">
                                    <label class="form-label">State</label>
                                    <asp:Label ID="lblState" runat="server" CssClass="form-control-plaintext"></asp:Label>
                                    <asp:TextBox ID="txtState" runat="server" CssClass="form-control d-none"></asp:TextBox>
                                </div>
                                <!-- Country -->
                                <div class="col-md-4">
                                    <label class="form-label">Country</label>
                                    <asp:Label ID="lblCountry" runat="server" CssClass="form-control-plaintext"></asp:Label>
                                    <asp:TextBox ID="txtCountry" runat="server" CssClass="form-control d-none"></asp:TextBox>
                                </div>
                                <!-- Postal Code -->
                                <div class="col-md-6">
                                    <label class="form-label">Postal Code</label>
                                    <asp:Label ID="lblPostalCode" runat="server" CssClass="form-control-plaintext"></asp:Label>
                                    <asp:TextBox ID="txtPostalCode" runat="server" CssClass="form-control d-none"></asp:TextBox>
                                </div>
                                <!-- Add more job seeker detail fields as necessary -->
                            </div>
                        </div>
                        <div class="card-footer p-2">
                            <asp:Button ID="btnEdit" runat="server" Text="Edit" CssClass="btn btn-primary btn-sm me-2" OnClick="btnEdit_Click" />
                            <asp:Button ID="btnUpdateProfile" runat="server" Text="Update" CssClass="btn btn-success btn-sm d-none me-2" OnClick="btnUpdateProfile_Click" />
                            <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="btn btn-secondary btn-sm d-none" OnClick="btnCancel_Click" />
                        </div>
                    </div>

                    <!-- Academic Details Section -->
                    <div class="card mb-3">
                        <div class="card-header d-flex justify-content-between align-items-center">
                            <span class="fw-bold">Academic Details</span>
                            <asp:Button ID="btnAddDegree" runat="server" Text="Add Degree" CssClass="btn btn-secondary btn-sm" OnClick="btnAddDegree_Click" />
                        </div>
                        <div class="card-body p-2">
                            <asp:Repeater ID="rptAcademicDetails" runat="server" OnItemDataBound="rptAcademicDetails_ItemDataBound" OnItemCommand="rptAcademicDetails_ItemCommand">
                                <ItemTemplate>
                                    <div class="academic-entry row g-2 mb-2">
                                        <div class="col-md-3">
                                            <asp:DropDownList ID="ddlDegree" runat="server" CssClass="form-select degree-dropdown">
                                                <asp:ListItem Value="">-- Select Degree --</asp:ListItem>
                                            </asp:DropDownList>
                                        </div>
                                        <div class="col-md-3">
                                            <asp:DropDownList ID="ddlEducationLevel" runat="server" CssClass="form-select education-level-dropdown">
                                                <asp:ListItem Value="">-- Select Education Level --</asp:ListItem>
                                                <asp:ListItem Value="10th">10th</asp:ListItem>
                                                <asp:ListItem Value="12th">12th</asp:ListItem>
                                                <asp:ListItem Value="Graduation">Graduation</asp:ListItem>
                                                <asp:ListItem Value="Post-Graduation">Post-Graduation</asp:ListItem>
                                            </asp:DropDownList>
                                        </div>
                                        <div class="col-md-2">
                                            <asp:TextBox ID="txtMarks" runat="server" CssClass="form-control" Placeholder="Marks (%)" type="number" step="0.01" min="0"></asp:TextBox>
                                        </div>
                                        <div class="col-md-3">
                                            <asp:TextBox ID="txtBoardUniversity" runat="server" CssClass="form-control" Placeholder="Board/University"></asp:TextBox>
                                        </div>
                                        <div class="col-md-1 d-flex align-items-center">
                                            <asp:LinkButton ID="btnRemoveDegree" runat="server" Text="Remove" CssClass="btn btn-danger btn-sm remove-degree" CommandName="RemoveDegree" CommandArgument='<%# Container.ItemIndex %>' OnClientClick="return confirm('Are you sure you want to remove this academic detail?');"></asp:LinkButton>
                                        </div>
                                        <div class="col-md-2">
                                            <asp:TextBox ID="txtYearOfCompletion" runat="server" CssClass="form-control" Placeholder="Year" type="number" min="1900" max="2100"></asp:TextBox>
                                        </div>
                                    </div>
                                </ItemTemplate>
                            </asp:Repeater>
                        </div>
                    </div>

                    <!-- Skill Requirements Section -->
                    <div class="card mb-3">
                        <div class="card-header d-flex justify-content-between align-items-center">
                            <span class="fw-bold">Skill Requirements</span>
                            <asp:Button ID="btnAddSkill" runat="server" Text="Add Skill" CssClass="btn btn-secondary btn-sm" OnClick="btnAddSkill_Click" />
                        </div>
                        <div class="card-body p-2">
                            <asp:Repeater ID="rptSkills" runat="server" OnItemDataBound="rptSkills_ItemDataBound" OnItemCommand="rptSkills_ItemCommand">
                                <ItemTemplate>
                                    <div class="skill-entry row g-2 mb-2">
                                        <div class="col-md-6">
                                            <asp:DropDownList ID="ddlSkill" runat="server" CssClass="form-select skill-dropdown">
                                                <asp:ListItem Value="">-- Select Skill --</asp:ListItem>
                                            </asp:DropDownList>
                                        </div>
                                        <div class="col-md-4">
                                            <asp:DropDownList ID="ddlSkillLevel" runat="server" CssClass="form-select skill-level-dropdown">
                                                <asp:ListItem Value="">-- Select Skill Level --</asp:ListItem>
                                                <asp:ListItem Value="1">Novice</asp:ListItem>
                                                <asp:ListItem Value="2">Beginner</asp:ListItem>
                                                <asp:ListItem Value="3">Competent</asp:ListItem>
                                                <asp:ListItem Value="4">Proficient</asp:ListItem>
                                                <asp:ListItem Value="5">Expert</asp:ListItem>
                                            </asp:DropDownList>
                                        </div>
                                        <div class="col-md-2 d-flex align-items-center">
                                            <asp:LinkButton ID="btnRemoveSkill" runat="server" Text="Remove" CssClass="btn btn-danger btn-sm remove-skill" CommandName="RemoveSkill" CommandArgument='<%# Container.ItemIndex %>' OnClientClick="return confirm('Are you sure you want to remove this skill?');"></asp:LinkButton>
                                        </div>
                                    </div>
                                </ItemTemplate>
                            </asp:Repeater>
                        </div>
                    </div>

                    <!-- Save Academic and Skill Requirements -->
                    <div class="mb-3">
                        <asp:Button ID="btnSaveRequirements" runat="server" Text="Save Academic and Skill Requirements" CssClass="btn btn-primary btn-sm" OnClick="btnSaveRequirements_Click" />
                    </div>

                    <!-- Dashboard and Logout Links -->
                    <div class="mb-3">
                        <asp:LinkButton ID="lnkViewDashboard" runat="server" CssClass="btn btn-info btn-sm me-2" OnClick="lnkViewDashboard_Click">View Dashboard</asp:LinkButton>
                        <asp:LinkButton ID="LinkButton1" runat="server" CssClass="btn btn-warning btn-sm" OnClick="lnkLogout_Click">Logout</asp:LinkButton>
                    </div>
                </div>
            </div>
            <!-- Main Content End -->



        </div>
    </form>
    <!-- Main wrapper end -->

    <!-- Scripts -->
    <script src="https://ajax.googleapis.com/ajax/libs/jquery/3.6.0/jquery.min.js"></script>
    <script src="xhtml/vendor/global/global.min.js"></script>
    <script src="xhtml/vendor/bootstrap-select/dist/js/bootstrap-select.min.js"></script>
    <script src="xhtml/vendor/chart-js/chart.bundle.min.js"></script>
    <script src="xhtml/js/custom.min.js"></script>
    <script src="xhtml/js/deznav-init.js"></script>
    <script src="xhtml/js/demo.js"></script>
</body>
</html>
