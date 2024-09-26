<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="JobSeekerDashboard.aspx.cs" Inherits="OnlineJobPortal.JobSeekerDashboard" %>

<!DOCTYPE html>
<html lang="en" dir="ltr">
<head runat="server">
    <!-- Title -->
    <title>Qerza - Job Portal Job Seeker Dashboard | Bootstrap HTML Template</title>

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
    <link rel="icon" type="image/png" href="xhtml/images/favicon.png" />

    <!-- CSS Files -->
    <link href="xhtml/vendor/jqvmap/css/jqvmap.min.css" rel="stylesheet" />
    <link rel="stylesheet" href="xhtml/vendor/chartist/css/chartist.min.css" />
    <link href="xhtml/vendor/bootstrap-select/dist/css/bootstrap-select.min.css" rel="stylesheet" />
    <link href="xhtml/css/style.css" rel="stylesheet" />
    <!-- Fonts -->
    <link href="https://fonts.googleapis.com/css2?family=Poppins:wght@100;200;300;400;500;600;700;800;900&amp;family=Roboto:wght@100;300;400;500;700;900&amp;display=swap" rel="stylesheet" />
</head>
<body data-typography="poppins" data-theme-version="light" data-layout="vertical" data-nav-headerbg="color_1" data-headerbg="color_1" data-sidebar-style="mini" data-sidebarbg="color_1" data-sidebar-position="fixed" data-header-position="fixed" data-container="wide" direction="ltr" data-primary="color_1">
    <!-- Preloader -->
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
                    <img src="xhtml/images/logo.png" alt="Logo" />
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

            <!-- Chat box start -->
            <div class="chatbox">
                <!-- Existing Chatbox HTML remains unchanged -->
                <!-- ... (omitted for brevity) -->
            </div>
            <!-- Chat box End -->

            <!-- Header start -->
            <div class="header">
                <div class="header-content">
                    <nav class="navbar navbar-expand">
                        <div class="collapse navbar-collapse justify-content-between">
                            <div class="header-left">
                                <div class="dashboard_bar">
                                    Search Job
                                </div>
                            </div>
                            <ul class="navbar-nav header-right">
                                <li class="nav-item">
                                    <div class="input-group search-area d-xl-inline-flex d-none">
                                        <input type="text" class="form-control" placeholder="Search here" aria-label="Search" aria-describedby="header-search" />
                                        <span class="input-group-text" id="header-search">
                                            <a href="javascript:void(0);">
                                                <i class="flaticon-381-search-2"></i>
                                            </a>
                                        </span>
                                    </div>
                                </li>
                                <!-- Theme mode toggle -->
                                <li class="nav-item dropdown notification_dropdown">
                                    <a class="nav-link bell dz-theme-mode p-0" href="javascript:void(0);">
                                        <i id="icon-light" class="fas fa-sun"></i>
                                        <i id="icon-dark" class="fas fa-moon"></i>
                                    </a>
                                </li>
                                <!-- Notifications -->
                                <li class="nav-item dropdown notification_dropdown">
                                    <a class="nav-link bell bell-link" href="javascript:void(0)">
                                        <!-- Notification SVG Icon -->
                                        <!-- ... (icon code remains unchanged) -->
                                        <span class="badge light text-white bg-primary">3</span>
                                    </a>
                                </li>
                                <!-- User Profile Dropdown -->
                                <li class="nav-item dropdown header-profile">
                                    <a class="nav-link" href="javascript:void(0)" role="button" data-bs-toggle="dropdown">
                                        <div class="header-info">
                                            <span class="text-black"><strong><asp:Label ID="lblJobSeekerName" runat="server" Text="John Doe"></asp:Label></strong></span>
                                            <p class="fs-12 mb-0">Job Seeker</p>
                                        </div>
                                        <img src="xhtml/images/profile/pix.jpg" width="20" alt="Profile" />
                                    </a>
                                    <div class="dropdown-menu dropdown-menu-end">
                                        <asp:LinkButton ID="lnkViewProfile" runat="server" CssClass="dropdown-item ai-icon" OnClick="lnkViewProfile_Click">
                                            <!-- SVG Icon -->
                                            <!-- ... (icon code remains unchanged) -->
                                            <span class="ms-2">Profile </span>
                                        </asp:LinkButton>
                                        <asp:LinkButton ID="lnkInbox" runat="server" CssClass="dropdown-item ai-icon" OnClick="lnkInbox_Click">
                                            <!-- SVG Icon -->
                                            <!-- ... (icon code remains unchanged) -->
                                            <span class="ms-2">Inbox </span>
                                        </asp:LinkButton>
                                        <asp:LinkButton ID="lnkLogout" runat="server" CssClass="dropdown-item ai-icon" OnClick="lnkLogout_Click">
                                            <!-- SVG Icon -->
                                            <!-- ... (icon code remains unchanged) -->
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
                <div class="deznav-scroll mm-active">
                    <ul class="metismenu mm-show" id="menu">
                        <li class="mm-active">
                            <a class="has-arrow ai-icon" href="javascript:void(0);" aria-expanded="true">
                                <i class="flaticon-381-networking"></i>
                                <span class="nav-text">Dashboard</span>
                            </a>
                            <ul aria-expanded="false" class="mm-collapse mm-show">
                                <li><a href="DashboardLight.aspx">Search Job</a></li>
                                <li><a href="DashboardDark.aspx">Manage Job</a></li>
                                <li class="mm-active">
                                    <a href="JobSeekerDashboard.aspx" class="mm-active" aria-expanded="true">Apply Job</a>
                                </li>
                                <li><a href="JobListings.aspx">Profile Match</a></li>
                                <li><a href="MyApplications.aspx">My Applications</a></li>
                                <li><a href="Profile.aspx">Profile</a></li>
                                <li><a href="Settings.aspx">Settings</a></li>
                                <!-- Add more menu items as needed -->
                            </ul>
                        </li>
                        <!-- Add more menu items if necessary -->
                    </ul>
                </div>
            </div>
            <!-- Sidebar end -->

            <!-- Content Body start -->
            <div class="content-body">
                <div class="container-fluid">
                    <!-- Dashboard Widgets -->
                    <div class="row">
                        <!-- Total Applications -->
                        <div class="col-xl-3 col-xxl-3 col-sm-6">
                            <div class="card">
                                <div class="card-body">
                                    <h5 class="card-title">Total Applications</h5>
                                    <h2 class="text-primary">
                                        <asp:Label ID="lblTotalApplications" runat="server" Text="0"></asp:Label>
                                    </h2>
                                </div>
                            </div>
                        </div>
                        <!-- Active Jobs -->
                        <div class="col-xl-3 col-xxl-3 col-sm-6">
                            <div class="card">
                                <div class="card-body">
                                    <h5 class="card-title">Active Jobs</h5>
                                    <h2 class="text-success">
                                        <asp:Label ID="lblActiveJobs" runat="server" Text="0"></asp:Label>
                                    </h2>
                                </div>
                            </div>
                        </div>
                        <!-- Pending Applications -->
                        <div class="col-xl-3 col-xxl-3 col-sm-6">
                            <div class="card">
                                <div class="card-body">
                                    <h5 class="card-title">Pending Applications</h5>
                                    <h2 class="text-info">
                                        <asp:Label ID="lblPendingApplications" runat="server" Text="0"></asp:Label>
                                    </h2>
                                </div>
                            </div>
                        </div>
                        <!-- Messages -->
                        <div class="col-xl-3 col-xxl-3 col-sm-6">
                            <div class="card">
                                <div class="card-body">
                                    <h5 class="card-title">Messages</h5>
                                    <h2 class="text-warning">
                                        <asp:Label ID="lblMessages" runat="server" Text="0"></asp:Label>
                                    </h2>
                                </div>
                            </div>
                        </div>
                    </div>
                    <!-- End of Dashboard Widgets -->

                    <!-- Send a Message Section -->
                    <div class="row mt-4">
                        <div class="col-lg-6">
                            <div class="card">
                                <div class="card-header">
                                    <h5>Send a Message</h5>
                                </div>
                                <div class="card-body">
                                    <!-- Employer Selection Dropdown -->
                                    <div class="mb-3">
                                        <label for="ddlEmployers" class="form-label">Select Employer</label>
                                        <asp:DropDownList ID="ddlEmployers" runat="server" CssClass="form-select">
                                           
                                        </asp:DropDownList>
                                    </div>
                                    <!-- Chat Message TextBox -->
                                    <div class="mb-3">
                                        <label for="txtChatMessage" class="form-label">Your Message</label>
                                        <asp:TextBox ID="txtChatMessage" runat="server" TextMode="MultiLine" Rows="4" CssClass="form-control"></asp:TextBox>
                                    </div>
                                    <!-- Send Button -->
                                    <asp:Button ID="btnSendMessage" runat="server" Text="Send" CssClass="btn btn-primary" OnClick="btnSendMessage_Click" />
                                    <!-- Message Label -->
                                    <asp:Label ID="lblMessage" runat="server" ForeColor="Red" CssClass="mt-2"></asp:Label>
                                </div>
                            </div>
                        </div>
                    </div>
                    <!-- End of Send a Message Section -->

                    <!-- Chat Messages Section -->
                    <div class="row mt-4">
                        <div class="col-lg-12">
                            <div class="card">
                                <div class="card-header">
                                    <h5>Your Messages</h5>
                                </div>
                                <div class="card-body">
                                    <!-- Repeater for Chat Messages -->
                                    <asp:Repeater ID="rptChatMessages" runat="server">
                                        <ItemTemplate>
                                            <div class="d-flex justify-content-start mb-4">
                                                <div class="img_cont_msg">
                                                    <img src='<%# ResolveUrl(Eval("ProfilePicturePath", "~/xhtml/images/profile/{0}")) %>' class="rounded-circle user_img_msg" alt='<%# Eval("CompanyName") %>' />
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
                        </div>
                    </div>
                    <!-- End of Chat Messages Section -->
                </div>
            </div>
            <!-- Content Body end -->

            <!-- Footer start -->
            <div class="footer">
                <div class="copyright">
                    <p>&copy; Designed &amp; Developed by <a href="http://dexignzone.com/" target="_blank">DexignZone</a> 2023</p>
                </div>
            </div>
            <!-- Footer end -->
        </div>
        <!-- Main wrapper end -->
    </form>
    <!-- Scripts -->
    <!-- Required vendors -->
    <script src="xhtml/vendor/global/global.min.js"></script>
    <script src="xhtml/vendor/bootstrap-select/dist/js/bootstrap-select.min.js"></script>
    <script src="xhtml/vendor/chart-js/chart.bundle.min.js"></script>
    <script src="xhtml/js/custom.min.js"></script>
    <script src="xhtml/js/deznav-init.js"></script>
    <script src="xhtml/js/demo.js"></script>
</body>
</html>