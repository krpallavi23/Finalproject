<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ChangeEmployerPassword.aspx.cs" Inherits="OnlineJobPortal.ChangeEmployerPassword" %>
<!DOCTYPE html>
<html lang="en" dir="ltr">
<head runat="server">
    <!-- Title -->
    <title>Qerza - Change Password | Employer Dashboard</title>

    <!-- Meta Tags -->
    <meta charset="utf-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge" />
    <meta name="author" content="DexignZone" />
    <meta name="robots" content="" />
    <meta name="keywords" content="job portal, employer dashboard, Bootstrap HTML template, job postings, manage jobs, post jobs, user-friendly interface, powerful functionalities" />
    <meta name="description" content="Qerza is a versatile job portal dashboard for employers powered by Bootstrap HTML. Manage job postings, search job seekers, and enhance your recruitment process. User-friendly interface with powerful functionalities. Job portal, employer dashboard, Bootstrap HTML template." />
    <meta property="og:title" content="Qerza - Employer Dashboard | Bootstrap HTML Template" />
    <meta property="og:description" content="Qerza is a versatile job portal dashboard for employers powered by Bootstrap HTML. Manage job postings, search job seekers, and enhance your recruitment process. User-friendly interface with powerful functionalities. Job portal, employer dashboard, Bootstrap HTML template." />
    <meta property="og:image" content="xhtml/social-image.png" />
    <meta name="format-detection" content="telephone=no" />

    <!-- Mobile Specific -->
    <meta name="viewport" content="width=device-width, initial-scale=1" />

    <!-- Favicon -->
    <link rel="icon" type="image/png" href="xhtml/images/logo.png" />

    <!-- CSS Files -->
    <link href="xhtml/vendor/jqvmap/css/jqvmap.min.css" rel="stylesheet" />
    <link rel="stylesheet" href="xhtml/vendor/chartist/css/chartist.min.css" />
    <link href="xhtml/vendor/bootstrap-select/dist/css/bootstrap-select.min.css" rel="stylesheet" />
    <link href="xhtml/css/style.css" rel="stylesheet" />

    <!-- Google Fonts -->
    <link href="https://fonts.googleapis.com/css2?family=Poppins:wght@100;200;300;400;500;600;700;800;900&amp;family=Roboto:wght@100;300;400;500;700;900&amp;display=swap" rel="stylesheet" />

    <!-- Custom Styles -->
    <style>
        /* Chatbox Styles */
        .chatbox {
            position: fixed;
            bottom: 20px;
            right: 20px;
            width: 300px;
            background-color: #fff;
            border: 1px solid #ddd;
            border-radius: 8px;
            box-shadow: 0 4px 8px rgba(0,0,0,0.1);
            z-index: 1050; /* Ensures the chatbox is above most elements */
            transition: transform 0.3s ease-in-out, opacity 0.3s ease-in-out;
            opacity: 1;
            transform: translateX(0);
        }

        .chatbox.hidden {
            opacity: 0;
            transform: translateX(100%);
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
            max-height: 400px;
            overflow-y: auto;
            transition: max-height 0.3s ease-in-out;
        }

        .chatbox.hidden .chatbox-body {
            max-height: 0;
            padding: 0 10px;
        }

        .chatbox-footer {
            padding: 10px;
            border-top: 1px solid #ddd;
        }
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
    <form id="form2" runat="server">
        <!-- ScriptManager for UpdatePanel -->
        <asp:ScriptManager ID="ScriptManager1" runat="server" />

        <div id="Div1" class="show">
            <!-- Nav header start -->
            <div class="nav-header">
                <a href="EmployerDashboard.aspx" class="brand-logo">
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
                                            <span class="text-black"><strong><asp:Label ID="lblEmployerName" runat="server" Text="Company Name"></asp:Label></strong></span>
                                            <p class="fs-12 mb-0">Employer</p>
                                        </div>
                                        <img src="xhtml/images/profile/pro_1.png" width="20" alt="Profile" />
                                    </a>
                                    <div class="dropdown-menu dropdown-menu-end">
                                        <!-- View Profile -->
                                        <asp:LinkButton ID="lnkViewProfile" runat="server" CssClass="dropdown-item ai-icon" OnClick="lnkViewProfile_Click">
                                            <!-- SVG Icon -->
                                            <svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" fill="none" viewBox="0 0 24 24">
                                                <path d="M12 12c2.21 0 4-1.79 4-4s-1.79-4-4-4-4 1.79-4 4 1.79 4 4 4zm0 2c-2.67 0-8 1.34-8 4v2h16v-2c0-2.66-5.33-4-8-4z" fill="#000"/>
                                            </svg>
                                            <span class="ms-2">Profile</span>
                                        </asp:LinkButton>
                                        <!-- Inbox -->
                                        <asp:LinkButton ID="lnkInbox" runat="server" CssClass="dropdown-item ai-icon" OnClick="lnkInbox_Click">
                                            <!-- SVG Icon -->
                                            <svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" fill="none" viewBox="0 0 24 24">
                                                <path d="M21 8V7l-3 2-2-1-3 2-2-1-3 2v1l3-2 2 1 3-2 2 1 3-2zM3 8V7l3 2 2-1 3 2 2-1 3 2v1l-3-2-2 1-3-2-2 1-3-2z" fill="#000"/>
                                            </svg>
                                            <span class="ms-2">Change Password</span>
                                        </asp:LinkButton>
                                        <!-- Logout -->
                                        <asp:LinkButton ID="lnkLogout" runat="server" CssClass="dropdown-item ai-icon" OnClick="lnkLogout_Click">
                                            <!-- SVG Icon -->
                                            <svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" fill="none" viewBox="0 0 24 24">
                                                <path d="M16 17v-2a4 4 0 00-4-4H5a2 2 0 00-2 2v3a2 2 0 002 2h7a4 4 0 004-4zM14 7l-5 5 5 5" stroke="#000" stroke-width="2" stroke-linecap="round" stroke-linejoin="round"/>
                                            </svg>
                                            <span class="ms-2">Logout</span>
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

            <!-- Content Body start -->
            <div class="content-body">
                <div class="container-fluid">
                    <div class="card">
                        <div class="card-header">
                            <h5>Change Your Password</h5>
                        </div>
                        <div class="card-body">
                            <div class="mb-3">
                                <label for="txtOldPassword" class="form-label">Old Password</label>
                                <asp:TextBox ID="txtOldPassword" runat="server" TextMode="Password" CssClass="form-control"></asp:TextBox>
                            </div>
                            <div class="mb-3">
                                <label for="txtNewPassword" class="form-label">New Password</label>
                                <asp:TextBox ID="txtNewPassword" runat="server" TextMode="Password" CssClass="form-control"></asp:TextBox>
                            </div>
                            <div class="mb-3">
                                <label for="txtConfirmPassword" class="form-label">Confirm New Password</label>
                                <asp:TextBox ID="txtConfirmPassword" runat="server" TextMode="Password" CssClass="form-control"></asp:TextBox>
                            </div>
                            <asp:Button ID="btnChangePassword" runat="server" Text="Change Password" CssClass="btn btn-primary" OnClick="btnChangePassword_Click" />
                            <asp:Label ID="lblMessage" runat="server" ForeColor="Red" CssClass="mt-2"></asp:Label>
                        </div>
                    </div>
                </div>
            </div>
            <!-- Content Body end -->

            

            <!-- Chat Box Start -->
            <asp:UpdatePanel ID="UpdatePanelChat" runat="server">
                <ContentTemplate>
                    <div class="chatbox hidden">
                        <!-- Chat Box Header -->
                        <div class="chatbox-header" id="ChatBoxHeader" runat="server">
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
                                    <!-- Chat Message Label -->
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
                                                <div class="img_cont_msg">
                                                    <img src='<%# ResolveUrl(Eval("ProfilePicturePath", "~/xhtml/images/profile/{0}")) %>'
                                                        class="rounded-circle user_img_msg" alt='<%# Eval("FirstName") %>' />
                                                </div>
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
                </ContentTemplate>
            </asp:UpdatePanel>
            <!-- Chat Box End -->

        </div>
        <!-- Main wrapper end -->

        <script>
            // Initialize chatbox behavior after the DOM is fully loaded
            document.addEventListener('DOMContentLoaded', function () {
                // Get references to chatbox elements using ClientID
                var chatBox = document.querySelector('.chatbox');
                var chatHeader = document.getElementById('<%= ChatBoxHeader.ClientID %>');
                var messagesDropdown = document.getElementById('messagesDropdown');

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
                messagesDropdown.addEventListener('click', function (event) {
                    toggleChatBox();
                    event.stopPropagation(); // Prevent event from bubbling up
                });

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

        <!-- Scripts -->
        <script src="xhtml/vendor/global/global.min.js"></script>
        <script src="xhtml/vendor/bootstrap-select/dist/js/bootstrap-select.min.js"></script>
        <script src="xhtml/vendor/chart-js/chart.bundle.min.js"></script>
        <script src="xhtml/js/custom.min.js"></script>
        <script src="xhtml/js/deznav-init.js"></script>
        <script src="xhtml/js/demo.js"></script>
    </form>
</body>
</html>
