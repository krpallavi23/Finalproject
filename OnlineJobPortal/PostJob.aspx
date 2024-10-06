<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PostJob.aspx.cs" Inherits="OnlineJobPortal.PostJob" %>

<!DOCTYPE html>
<html lang="en" dir="ltr">
<head runat="server">
    <!-- Title -->
    <title>Online Job Portal - Post a Job</title>

    <!-- Meta -->
    <meta charset="utf-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />

    <!-- CSS Files -->
    <link href="xhtml/vendor/jqvmap/css/jqvmap.min.css" rel="stylesheet" />
    <link rel="stylesheet" href="xhtml/vendor/chartist/css/chartist.min.css" />
    <link href="xhtml/vendor/bootstrap-select/dist/css/bootstrap-select.min.css" rel="stylesheet" />
    <link href="xhtml/css/style.css" rel="stylesheet" />
    <!-- Fonts -->
    <link href="https://fonts.googleapis.com/css2?family=Poppins:wght@100;200;300;400;500;600;700;800;900&amp;family=Roboto:wght@100;300;400;500;700;900&amp;display=swap" rel="stylesheet" />


    <!-- Custom Styles for Chatbox -->
    <style>
        /* Chatbox Styles */
        .chatbox {
            position: fixed;
            top: 20px; /* Positioned at the top-right corner */
            right: 20px;
            width: 300px;
            height: 400px; /* Fixed height */
            background-color: #fff;
            border: 1px solid #ddd;
            border-radius: 8px;
            box-shadow: 0 4px 8px rgba(0,0,0,0.1);
            z-index: 1050; /* Ensures the chatbox is above most elements */
            transition: transform 0.3s ease-in-out, opacity 0.3s ease-in-out;
            opacity: 1;
            transform: translateX(0); /* Visible state */
        }

        .chatbox.hidden {
            opacity: 0;
            transform: translateX(100%); /* Hidden state: moved completely to the right */
            pointer-events: none;
        }

        .chatbox-header {
            padding: 10px;
            background-color: #f5f5f5;
            border-bottom: 1px solid #ddd;
            border-top-left-radius: 8px;
            border-top-right-radius: 8px;
            cursor: pointer;
            display: flex;
            justify-content: space-between;
            align-items: center;
        }

        .chatbox-body {
            display: block; /* Always block, visibility controlled via CSS */
            padding: 10px;
            max-height: 340px; /* Adjusted height to accommodate header */
            overflow-y: auto;
            transition: max-height 0.3s ease-in-out;
        }

        .chatbox.hidden .chatbox-body {
            max-height: 0;
            padding: 0 10px;
        }

        /* Optional: Adjust chat messages styling if needed */
        .msg_container {
            margin-bottom: 10px;
        }
        .msg_container img {
            width: 40px;
            height: 40px;
            border-radius: 50%;
            margin-right: 10px;
        }
        .msg_cotainer {
            background-color: #f1f0f0;
            padding: 10px;
            border-radius: 8px;
            display: inline-block;
            max-width: 80%;
        }
        .msg_time {
            display: block;
            font-size: 0.8em;
            color: #888;
            margin-top: 5px;
        }
        .select2-dropdown.custom-dropdown {
        max-height: 200px !important; /* Set the maximum height as needed */
        overflow-y: auto !important;  /* Enable vertical scrolling */
       }

    /* Optional: Adjust the width to match the container */
        .select2-dropdown.custom-dropdown {
            width: auto !important;
            min-width: 100% !important;
        }
    </style>
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
        <!-- Add the ScriptManager here -->
        <asp:ScriptManager ID="ScriptManager1" runat="server" />

        <div id="Div1" class="show">
            <!-- Nav header start -->
            <div class="nav-header">
                <a href="EmployerDashboard.aspx" class="brand-logo">
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
            <!-- Nav header end -->

            <!-- Header start -->
            <div class="header">
                <div class="header-content">
                    <nav class="navbar navbar-expand">
                        <div class="collapse navbar-collapse justify-content-between">
                            <div class="header-left">
                                <div class="dashboard_bar">
                                    Employer Dashboard
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
                                
                                <!-- Messages Option -->
                                <li class="nav-item dropdown notification_dropdown">
                                    <a class="nav-link bell" href="javascript:void(0);" id="messagesDropdown">
                                        <!-- SVG Icon for Messages -->
                                        <svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" fill="none" viewBox="0 0 24 24">
                                            <path d="M21 15a2 2 0 01-2 2H7l-4 4V5a2 2 0 012-2h12a2 2 0 012 2z" stroke="#000" stroke-width="2" stroke-linecap="round" stroke-linejoin="round"/>
                                        </svg>
                                        <span class="badge light text-white bg-primary">3</span>
                                    </a>
                                </li>
                                <!-- User Profile Dropdown -->
                                <li class="nav-item dropdown header-profile">
                                    <a class="nav-link" href="javascript:void(0)" role="button" data-bs-toggle="dropdown">
                                        <div class="header-info">
                                            <span class="text-black"><strong><asp:Label ID="lblEmployerName" runat="server" Text="Company XYZ"></asp:Label></strong></span>
                                            <p class="fs-12 mb-0">Employer</p>
                                        </div>
                                        <img src="xhtml/images/profile/17.jpg" width="20" alt="Profile" />
                                    </a>
                                    <div class="dropdown-menu dropdown-menu-end">
                                        <asp:LinkButton ID="lnkViewProfile" runat="server" CssClass="dropdown-item ai-icon" OnClick="lnkViewProfile_Click">
                                            <!-- SVG Icon -->
                                            <svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" fill="none" viewBox="0 0 24 24">
                                                <!-- SVG paths go here -->
                                            </svg>
                                            <span class="ms-2">Profile </span>
                                        </asp:LinkButton>
                                        <asp:LinkButton ID="lnkInbox" runat="server" CssClass="dropdown-item ai-icon" OnClick="lnkInbox_Click">
                                            <!-- SVG Icon -->
                                            <svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" fill="none" viewBox="0 0 24 24">
                                                <!-- SVG paths go here -->
                                            </svg>
                                            <span class="ms-2">Inbox </span>
                                        </asp:LinkButton>
                                        <asp:LinkButton ID="lnkLogout" runat="server" CssClass="dropdown-item ai-icon" OnClick="lnkLogout_Click">
                                            <!-- SVG Icon -->
                                            <svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" fill="none" viewBox="0 0 24 24">
                                                <!-- SVG paths go here -->
                                            </svg>
                                            <span class="ms-2">Logout </span>
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
                            <a href="SearchJobSeekers.aspx" class="ai-icon">
                                <i class="flaticon-381-search"></i>
                                <span class="nav-text">Search JobSeekers</span>
                            </a>
                        </li>
                        <li>
                            <a href="PostJob.aspx" class="ai-icon">
                                <i class="flaticon-381-briefcase"></i>
                                <span class="nav-text">Post Job</span>
                            </a>
                        </li>
                        <li>
                            <a class="has-arrow ai-icon" href="javascript:void(0);" aria-expanded="false">
                                <i class="flaticon-381-settings-2"></i>
                                <span class="nav-text">Manage Jobs</span>
                            </a>
                            <ul aria-expanded="false">
                                <li><a href="ManageJobsAll.aspx?status=All">All</a></li>
                                <li><a href="ManageJobsActive.aspx?status=Active">Active</a></li>
                                <li><a href="ManageJobsInactive.aspx?status=Inactive">Inactive</a></li>
                            </ul>
                        </li>
                        <!-- Add other menu items as needed -->
                    </ul>
                </div>
            </div>
            <!-- Sidebar end -->
             <!-- Main content start -->
        <div class="content-body">
            <div class="container-fluid">
                <!-- Post Job Form -->
                <asp:UpdatePanel ID="UpdatePanelPostJob" runat="server">
                    <ContentTemplate>
                        <!-- Post Job Form -->
                        <div class="row">
                            <div class="col-xl-12">
                                <div class="card">
                                    <div class="card-header">
                                        <h4 class="card-title">Post a New Job</h4>
                                    </div>
                                    <div class="card-body">
                                        <div class="basic-form">
                                            <div class="row g-3">
                                                <!-- Job Title -->
                                                <div class="col-md-6">
                                                    <div class="mb-3">
                                                        <label for="txtJobTitle" class="form-label">Job Title</label>
                                                        <asp:TextBox ID="txtJobTitle" runat="server" CssClass="form-control" Placeholder="Enter Job Title"></asp:TextBox>
                                                    </div>
                                                </div>
                                                <!-- Department -->
                                                <div class="col-md-6">
                                                    <div class="mb-3">
                                                        <label for="ddlDepartment" class="form-label">Department</label>
                                                        <asp:DropDownList ID="ddlDepartment" runat="server" CssClass="form-select">
                                                            <asp:ListItem Value="">-- Select Department --</asp:ListItem>
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>
                                                <!-- Job Description -->
                                                <div class="col-md-12">
                                                    <div class="mb-3">
                                                        <label for="txtDescription" class="form-label">Job Description</label>
                                                        <asp:TextBox ID="txtDescription" runat="server" TextMode="MultiLine" Rows="5"
                                                                     CssClass="form-control" Placeholder="Enter Job Description"></asp:TextBox>
                                                    </div>
                                                </div>
                                                <!-- Location -->
                                                <div class="col-md-6">
                                                    <div class="mb-3">
                                                        <label for="txtLocation" class="form-label">Location</label>
                                                        <asp:TextBox ID="txtLocation" runat="server" CssClass="form-control" Placeholder="Enter Job Location"></asp:TextBox>
                                                    </div>
                                                </div>
                                                <!-- Salary -->
                                                <div class="col-md-6">
                                                    <div class="mb-3">
                                                        <label for="txtSalary" class="form-label">Salary ($)</label>
                                                        <asp:TextBox ID="txtSalary" runat="server" CssClass="form-control" Placeholder="Enter Salary"></asp:TextBox>
                                                    </div>
                                                </div>
                                                <!-- Application Deadline -->
                                                <div class="col-md-6">
                                                    <div class="mb-3">
                                                        <label for="txtDeadline" class="form-label">Application Deadline</label>
                                                        <asp:TextBox ID="txtDeadline" runat="server" CssClass="form-control" Placeholder="Select Deadline Date" TextMode="Date"></asp:TextBox>
                                                    </div>
                                                </div>
                                                <!-- Employment Type -->
                                                <div class="col-md-6">
                                                    <div class="mb-3">
                                                        <label for="ddlEmploymentType" class="form-label">Employment Type</label>
                                                        <asp:DropDownList ID="ddlEmploymentType" runat="server" CssClass="form-select">
                                                            <asp:ListItem Value="">-- Select Employment Type --</asp:ListItem>
                                                            <asp:ListItem Value="Full-Time">Full-Time</asp:ListItem>
                                                            <asp:ListItem Value="Part-Time">Part-Time</asp:ListItem>
                                                            <asp:ListItem Value="Contract">Contract</asp:ListItem>
                                                            <asp:ListItem Value="Freelance">Freelance</asp:ListItem>
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>
                                            </div>

                                            <!-- Degree Requirements Section -->
                                            <div class="row g-3">
                                                <div class="col-md-12">
                                                    <div class="mb-3">
                                                        <label class="form-label">Degree Requirements</label>
                                                        <asp:Repeater ID="rptDegrees" runat="server" OnItemDataBound="rptDegrees_ItemDataBound" OnItemCommand="rptDegrees_ItemCommand">
                                                            <ItemTemplate>
                                                                <div class="degree-entry row g-3 mb-2">
                                                                    <div class="col-md-6">
                                                                        <asp:DropDownList ID="ddlDegree" runat="server" CssClass="form-select degree-dropdown">
                                                                        </asp:DropDownList>
                                                                    </div>
                                                                    <div class="col-md-4">
                                                                        <asp:TextBox ID="txtMinPercentage" runat="server" CssClass="form-control min-percentage" Placeholder="Minimum Percentage"></asp:TextBox>
                                                                    </div>
                                                                    <div class="col-md-2 d-flex align-items-center">
                                                                        <asp:LinkButton ID="btnRemoveDegree" runat="server" Text="Remove" CssClass="btn btn-danger remove-degree" CommandName="RemoveDegree" CommandArgument='<%# Container.ItemIndex %>'></asp:LinkButton>
                                                                    </div>
                                                                </div>
                                                            </ItemTemplate>
                                                        </asp:Repeater>

                                                        <asp:Button ID="btnAddDegree" runat="server" Text="Add Degree Requirement" CssClass="btn btn-secondary" OnClick="btnAddDegree_Click" />
                                                    </div>
                                                </div>
                                            </div>

                                            <!-- Skill Requirements Section -->
                                            <div class="row g-3">
                                                <div class="col-md-12">
                                                    <div class="mb-3">
                                                        <label class="form-label">Skill Requirements</label>
                                                        <asp:Repeater ID="rptSkills" runat="server" OnItemDataBound="rptSkills_ItemDataBound" OnItemCommand="rptSkills_ItemCommand">
                                                            <ItemTemplate>
                                                                <div class="skill-entry row g-3 mb-2">
                                                                    <div class="col-md-6">
                                                                        <asp:DropDownList ID="ddlSkill" runat="server" CssClass="form-select skill-dropdown">
                                                                            <asp:ListItem Value="">-- Select Skill --</asp:ListItem>
                                                                        </asp:DropDownList>
                                                                    </div>
                                                                    <div class="col-md-4">
                                                                        <asp:DropDownList ID="ddlExpertise" runat="server" CssClass="form-select expertise-dropdown">
                                                                            <asp:ListItem Value="">-- Select Expertise --</asp:ListItem>
                                                                            <asp:ListItem Value="1">Novice</asp:ListItem>
                                                                            <asp:ListItem Value="2">Beginner</asp:ListItem>
                                                                            <asp:ListItem Value="3">Competent</asp:ListItem>
                                                                            <asp:ListItem Value="4">Proficient</asp:ListItem>
                                                                            <asp:ListItem Value="5">Expert</asp:ListItem>
                                                                        </asp:DropDownList>
                                                                    </div>
                                                                    <div class="col-md-2 d-flex align-items-center">
                                                                        <asp:LinkButton ID="btnRemoveSkill" runat="server" Text="Remove" CssClass="btn btn-danger remove-skill" CommandName="RemoveSkill" CommandArgument='<%# Container.ItemIndex %>'></asp:LinkButton>
                                                                    </div>
                                                                </div>
                                                            </ItemTemplate>
                                                        </asp:Repeater>
                                                        <asp:Button ID="btnAddSkill" runat="server" Text="Add Skill Requirement" CssClass="btn btn-secondary" OnClick="btnAddSkill_Click" />
                                                    </div>
                                                </div>
                                            </div>

                                            <!-- Post Job Button -->
                                            <div class="row">
                                                <div class="col-md-12 text-end">
                                                    <asp:Button ID="btnPostJob" runat="server" Text="Post Job" CssClass="btn btn-primary" OnClick="btnPostJob_Click" />
                                                </div>
                                            </div>
                                            <!-- Status Label -->
                                            <asp:Label ID="lblStatus" runat="server" CssClass="mt-2"></asp:Label>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <!-- Chat Box Start -->
                        <div class="chatbox hidden">
                            <!-- Chat Box Header -->
                            <div class="chatbox-header" id="Div2" runat="server" aria-label="Chat Header">
                                <span>Chat</span>
                                <!-- Minimize Icon -->
                                <span>&#x25BC;</span>
                            </div>
                            <!-- Chat Box Body -->
                            <div class="chatbox-body" id="Div3" runat="server">
                                <!-- Send a Message Section -->
                                <div class="card">
                                    <div class="card-header">
                                        <h5>Send a Message</h5>
                                    </div>
                                    <div class="card-body">
                                        <!-- Job Seeker Selection Dropdown -->
                                        <div class="mb-3">
                                            <label for="ddlJobSeekers" class="form-label">Select Job Seeker</label>
                                            <asp:DropDownList ID="DropDownList1" runat="server" CssClass="form-select">
                                                <asp:ListItem Value="">-- Select Job Seeker --</asp:ListItem>
                                            </asp:DropDownList>
                                        </div>

                                        <!-- Chat Message TextBox -->
                                        <div class="mb-3">
                                            <label for="txtChatMessage" class="form-label">Your Message</label>
                                            <asp:TextBox ID="TextBox1" runat="server" TextMode="MultiLine" Rows="4"
                                                         CssClass="form-control"></asp:TextBox>
                                        </div>
                                        <!-- Send Button -->
                                        <asp:Button ID="Button1" runat="server" Text="Send" CssClass="btn btn-primary"
                                                    OnClick="btnSendMessage_Click" />
                                        <!-- Message Label -->
                                        <asp:Label ID="Label1" runat="server" ForeColor="Red" CssClass="mt-2"></asp:Label>
                                    </div>
                                </div>
                                <!-- End of Send a Message Section -->

                                <!-- Chat Messages Section -->
                                <!-- Include if needed -->
                            </div>
                        </div>
                        <!-- Chat Box End -->

                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="btnPostJob" EventName="Click" />
                        <asp:AsyncPostBackTrigger ControlID="btnAddDegree" EventName="Click" />
                        <asp:AsyncPostBackTrigger ControlID="btnAddSkill" EventName="Click" />
                        <asp:AsyncPostBackTrigger ControlID="btnSendMessage" EventName="Click" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>
        </div>
        <!-- Main content end -->


          



            <!-- Footer start -->
            <!-- (Your existing footer code, if any) -->
            <!-- Footer end -->

            <!-- Chat Box Start -->
            <div class="chatbox hidden">
                <!-- Chat Box Header -->
                <div class="chatbox-header" id="ChatBoxHeader" runat="server" aria-label="Chat Header">
                    <span>Chat</span>
                    <!-- Minimize Icon -->
                    <span>&#x25BC;</span>
                </div>
                <!-- Chat Box Body -->
                <div class="chatbox-body" id="ChatBoxBody" runat="server">
                    <!-- Send a Message Section -->
                    <div class="card">
                        <div class="card-header">
                            <h5>Send a Message</h5>
                        </div>
                        <div class="card-body">
                            <!-- Job Seeker Selection Dropdown -->
                            <div class="mb-3">
                                <label for="ddlJobSeekers" class="form-label">Select Job Seeker</label>
                                <asp:DropDownList ID="ddlJobSeekers" runat="server" CssClass="form-select">
                                    <asp:ListItem Value="">-- Select Job Seeker --</asp:ListItem>
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
                                OnClick="btnSendMessage_Click" />
                            <!-- Message Label -->
                            <asp:Label ID="lblChatMessage" runat="server" ForeColor="Red" CssClass="mt-2"></asp:Label>
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
                                    <div class="d-flex justify-content-start mb-4">
                                        <div class="msg_cotainer">
                                            <%# Eval("Message") %>
                                            <span class="msg_time"><%# Eval("MessageTime", "{0:MM/dd/yyyy HH:mm}") %></span>
                                        </div>
                                    </div>
                                </ItemTemplate>
                            </asp:Repeater>
                        </div>
                    </div>
                    <!-- End of Chat Messages Section -->
                </div>
            </div>
            <!-- Chat Box End -->

        </div>
        <!-- Main wrapper end -->

        <!-- JavaScript for Chatbox Functionality -->
        <script>
            // Initialize chatbox behavior after the DOM is fully loaded
            document.addEventListener('DOMContentLoaded', function () {
                // Get references to chatbox elements using client-side ID
                var chatBox = document.querySelector('.chatbox');
                var chatHeader = document.getElementById('<%= ChatBoxHeader.ClientID %>');
                var chatBody = document.getElementById('<%= ChatBoxBody.ClientID %>');
                var messagesDropdown = document.getElementById('messagesDropdown'); // Correct client-side ID reference

                // Function to toggle chatbox visibility
                function toggleChatBox() {
                    chatBox.classList.toggle('hidden');
                }

                // Toggle chatbox visibility when clicking on the header
                chatHeader.addEventListener('click', function (event) {
                    toggleChatBox();
                    event.stopPropagation(); // Prevent event from bubbling up
                });

                // Toggle chatbox visibility when clicking on the Messages dropdown
                if (messagesDropdown) { // Added null check
                    messagesDropdown.addEventListener('click', function (event) {
                        toggleChatBox();
                        event.stopPropagation(); // Prevent event from bubbling up
                    });
                }

                // Hide chatbox when clicking outside of it
                document.addEventListener('click', function (event) {
                    if (!chatBox.contains(event.target) && event.target !== messagesDropdown) {
                        chatBox.classList.add('hidden');
                    }
                });

                // Prevent the chatbox from closing when clicking inside it
                chatBox.addEventListener('click', function (event) {
                    event.stopPropagation();
                });
            });
        </script>
          <!-- Initialize Select2 -->
        <script type="text/javascript">
            function initializeSelect2() {
                // Destroy existing Select2 instances to prevent duplication
                $('.degree-dropdown').select2('destroy');
                $('.skill-dropdown').select2('destroy');

                // Initialize Select2 for degree dropdowns
                $('.degree-dropdown').select2({
                    placeholder: 'Select Degree',
                    allowClear: true,
                    dropdownCssClass: 'custom-dropdown' // Add custom CSS class
                });

                // Initialize Select2 for skill dropdowns
                $('.skill-dropdown').select2({
                    placeholder: 'Select Skill',
                    allowClear: true,
                    dropdownCssClass: 'custom-dropdown' // Add custom CSS class
                });
            }

            // Initialize on document ready
            $(document).ready(function () {
                initializeSelect2();
            });

            // Re-initialize after each partial postback
            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(function () {
                initializeSelect2();
            });
          </script>

        
         
       
        <!-- Scripts -->
        <script src="xhtml/vendor/global/global.min.js"></script>
        <script src="xhtml/vendor/bootstrap-select/dist/js/bootstrap-select.min.js"></script>
        <script src="xhtml/vendor/chart-js/chart.bundle.min.js"></script>
        <script src="xhtml/js/custom.min.js"></script>
        <script src="xhtml/js/deznav-init.js"></script>
        <script src="xhtml/js/demo.js"></script>
    </form>
    <!-- Main wrapper end -->
</body>
</html>
