<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SelectedJob.aspx.cs" Inherits="OnlineJobPortal.SelectedJob" %>

<!DOCTYPE html>
<html lang="en" dir="ltr">
<head runat="server">
    <!-- Title -->
    <title>Qerza - Selected Jobs | Bootstrap HTML Template</title>

    <!-- Meta -->
    <meta charset="utf-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge" />
    <meta name="author" content="DexignZone" />
    <meta name="robots" content="" />
    <meta name="keywords" content="job portal, selected jobs, GridView, Bootstrap HTML template, job listings, user-friendly interface" />
    <meta name="description" content="Qerza - Selected Jobs page for job seekers. View all your selected job applications. User-friendly interface with powerful functionalities." />
    <meta property="og:title" content="Qerza - Selected Jobs | Bootstrap HTML Template" />
    <meta property="og:description" content="Qerza - Selected Jobs page for job seekers. View all your selected job applications. User-friendly interface with powerful functionalities." />
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
                                    Selected Jobs
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
                                        <svg width="28" height="28" viewBox="0 0 28 28" fill="none"
                                            xmlns="http://www.w3.org/2000/svg">
                                            <!-- SVG Path data -->
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
                                        <asp:LinkButton ID="lnkViewProfile" runat="server" CssClass="dropdown-item ai-icon" OnClick="lnkViewProfile_Click">
                                            <!-- SVG Icon -->
                                            <span class="ms-2">Profile </span>
                                        </asp:LinkButton>
                                        <asp:LinkButton ID="lnkLogout" runat="server" CssClass="dropdown-item ai-icon" OnClick="lnkLogout_Click">
                                            <!-- SVG Icon -->
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
                                <li><a href="AllJobApplications.aspx">All</a></li>
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
                        <!-- Remove the '+' if it's unintended -->
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

                    <!-- Selected Jobs Section -->
                    <div class="row mt-4">
                        <div class="col-12">
                            <div class="card">
                                <div class="card-header">
                                    <h5>Selected Jobs</h5>
                                </div>
                                <div class="card-body">
                                    <asp:GridView ID="gvSelectedJobs" runat="server" CssClass="table table-bordered table-striped"
                                        AutoGenerateColumns="False" AllowPaging="True" PageSize="10"
                                        OnPageIndexChanging="gvSelectedJobs_PageIndexChanging"
                                        OnRowCommand="gvSelectedJobs_RowCommand">
                                        <Columns>
                                            <asp:BoundField DataField="ApplicationID" HeaderText="Application ID" />
                                            <asp:BoundField DataField="JobTitle" HeaderText="Job Title" />
                                            <asp:BoundField DataField="CompanyName" HeaderText="Company" />
                                            <asp:BoundField DataField="ApplicationDate" HeaderText="Applied On" DataFormatString="{0:MM/dd/yyyy}" />
                                            <asp:BoundField DataField="Status" HeaderText="Status" />
                                            <asp:TemplateField HeaderText="Actions">
                                                <ItemTemplate>
                                                    <asp:Button ID="btnViewDetails" runat="server" Text="View"
                                                        CommandName="ViewDetails" CommandArgument='<%# Eval("ApplicationID") %>'
                                                        CssClass="btn btn-info btn-sm" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </div>
                            </div>
                        </div>
                    </div>
                    <!-- End of Selected Jobs Section -->
                </div>
            </div>
            <!-- Content Body end -->

            <!-- Footer start -->
            <div class="footer">
                <div class="copyright">
                    <p>Made by Ayushi & Pallavi.</p>
                </div>
            </div>
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
