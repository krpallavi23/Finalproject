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

            <!-- Chat box start -->
            <div class="chatbox">
                <div class="chatbox-content">
                    <!-- Send a Message Section -->
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
                                <asp:TextBox ID="txtChatMessage" runat="server" TextMode="MultiLine" Rows="4"
                                             CssClass="form-control"></asp:TextBox>
                            </div>
                            <!-- Send Button -->
                            <asp:Button ID="btnSendMessage" runat="server" Text="Send" CssClass="btn btn-primary"
                                        OnClick="btnSendMessage_Click" />
                            <!-- Message Label -->
                            <asp:Label ID="lblMessage" runat="server" ForeColor="Red" CssClass="mt-2"></asp:Label>
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
                                                 class="rounded-circle user_img_msg" alt='<%# Eval("CompanyName") %>' />
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
                                <li><a href="JobApplication.aspx?status=All">All</a></li>
                                <li><a href="AppliedJob.aspx?status=Applied">Applied</a></li>
                                <li><a href="ShortlistedJob.aspx?status=Shortlisted">Shortlisted</a></li>
                                <li><a href="SelectedJob.aspx?status=Selected">Selected</a></li>
                                <li><a href="RejectedJobs.aspx?status=Rejected">Rejected</a></li>
                            </ul>
                        </li>
                        <li>
                            <a href="LikeJob.aspx" class="ai-icon">
                                <i class="flaticon-381-heart"></i>
                                <span class="nav-text">Like Job</span>
                            </a>
                        </li>
                        +
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
                </div>
            </div>
            <!-- Content Body end -->

            <!-- Footer start -->
            <!-- (Your existing footer code remains unchanged) -->
            <!-- Footer end -->
        </div>
        <!-- Main wrapper end -->
    </form>
    <script src="xhtml/vendor/global/global.min.js"></script>
    <script src="xhtml/vendor/bootstrap-select/dist/js/bootstrap-select.min.js"></script>
    <script src="xhtml/vendor/chart-js/chart.bundle.min.js"></script>
    <script src="xhtml/js/custom.min.js"></script>
    <script src="xhtml/js/deznav-init.js"></script>
    <script src="xhtml/js/demo.js"></script>
</body>
</html>
