<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AdminDashboard.aspx.cs" Inherits="OnlineJobPortal.AdminDashboard" %>

<!DOCTYPE html>
<html lang="en" dir="ltr">
<head runat="server">
    <title>Qerza - Job Portal Admin Dashboard | Bootstrap HTML Template</title>
    <meta charset="utf-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge" />
    <meta name="author" content="DexignZone" />
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <link rel="icon" type="image/png" href="xhtml/images/favicon.png" />
    <link href="xhtml/vendor/bootstrap-select/dist/css/bootstrap-select.min.css" rel="stylesheet" />
    <link href="xhtml/css/style.css" rel="stylesheet" />
    <link href="https://fonts.googleapis.com/css2?family=Poppins:wght@100;300;400;500;600;700&display=swap" rel="stylesheet" />

</head>
<body data-typography="poppins" data-theme-version="light" data-layout="vertical" data-nav-headerbg="color_1"
      data-headerbg="color_1" data-sidebarbg="color_1" data-sidebar-position="fixed"
      data-header-position="fixed" data-container="wide" direction="ltr" data-primary="color_1">
   
    <form id="form1" runat="server">
        <div id="Div1" class="show">
            <!-- Nav header start -->
            <div class="nav-header">
                <a href="AdminDashboard.aspx" class="brand-logo">
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

            <!-- Header start -->
            <div class="header">
                <div class="header-content">
                    <nav class="navbar navbar-expand">
                        <div class="collapse navbar-collapse justify-content-between">
                            <div class="header-left">
                                <div class="dashboard_bar">Admin Dashboard</div>
                            </div>
                            <ul class="navbar-nav header-right">
                                 <li class="nav-item dropdown header-profile">
                                    <a class="nav-link" href="javascript:void(0)" role="button" data-bs-toggle="dropdown">
                                        <div class="header-info">
                                            <span class="text-black"><strong><asp:Label ID="lblAdminName" runat="server" Text="Admin Name"></asp:Label></strong></span>
                                            <p class="fs-12 mb-0">Administrator</p>
                                        </div>
                                        <img src="xhtml/images/profile/pro_1.png" width="20" alt="Profile" />
                                    </a>
                                    <div class="dropdown-menu dropdown-menu-end">
                                        <asp:LinkButton ID="lnkViewProfile" runat="server" CssClass="dropdown-item ai-icon" OnClick="lnkViewProfile_Click">
                                            <span class="ms-2">Profile</span>
                                        </asp:LinkButton>
                                       <asp:LinkButton ID="LinkButton1" runat="server" CssClass="dropdown-item ai-icon" OnClick="lnkInbox_Click">
    <span class="ms-2">Change Password</span>
</asp:LinkButton>
                                        <asp:LinkButton ID="lnkLogout" runat="server" CssClass="dropdown-item ai-icon" OnClick="lnkLogout_Click">
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
                            <a href="ManageJobseeker.aspx">
                                
                                <span class="nav-text">Manage Jobseeker</span>
                            </a>
                        </li>
                        <li>
                            <a href="ManageEmployer.aspx">
                                <span class="nav-text">Manage Employer</span>
                            </a>
                        </li>
                    </ul>
                </div>
            </div>
            <!-- Sidebar end -->


            <!-- Content Body start -->
            <div class="content-body">
                <div class="container-fluid">
                    <!-- Dashboard Widgets -->
                    <div class="row">
                        <!-- Total Users -->
                        <div class="col-xl-3 col-xxl-3 col-sm-6">
                            <div class="card">
                                <div class="card-body">
                                    <h5 class="card-title">Total Users</h5>
                                    <h2 class="text-primary">
                                        <asp:Label ID="lblTotalUsers" runat="server" Text="0"></asp:Label>
                                    </h2>
                                </div>
                            </div>
                        </div>
                        <!-- Total Job Postings -->
                        <div class="col-xl-3 col-xxl-3 col-sm-6">
                            <div class="card">
                                <div class="card-body">
                                    <h5 class="card-title">Total Job Postings</h5>
                                    <h2 class="text-success">
                                        <asp:Label ID="lblTotalJobPostings" runat="server" Text="0"></asp:Label>
                                    </h2>
                                </div>
                            </div>
                        </div>
                        <!-- Total Applications -->
                        <div class="col-xl-3 col-xxl-3 col-sm-6">
                            <div class="card">
                                <div class="card-body">
                                    <h5 class="card-title">Total Applications</h5>
                                    <h2 class="text-info">
                                        <asp:Label ID="lblTotalApplications" runat="server" Text="0"></asp:Label>
                                    </h2>
                                </div>
                            </div>
                        </div>
                        <!-- Pending Applications -->
                        <div class="col-xl-3 col-xxl-3 col-sm-6">
                            <div class="card">
                                <div class="card-body">
                                    <h5 class="card-title">Pending Applications</h5>
                                    <h2 class="text-warning">
                                        <asp:Label ID="lblPendingApplications" runat="server" Text="0"></asp:Label>
                                    </h2>
                                </div>
                            </div>
                        </div>
                    </div>
                    <!-- End of Dashboard Widgets -->
                </div>
            </div>
            <!-- Content Body end -->
        </div>
        <!-- Main wrapper end -->
    </form>

    <script src="xhtml/vendor/global/global.min.js"></script>
    <script src="xhtml/vendor/bootstrap-select/dist/js/bootstrap-select.min.js"></script>
    <script src="xhtml/js/custom.min.js"></script>
    <script src="xhtml/js/deznav-init.js"></script>
</body>
</html>