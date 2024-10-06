<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ViewDetails.aspx.cs" Inherits="OnlineJobPortal.ViewDetails" %>

<!DOCTYPE html>
<html lang="en" dir="ltr">
<head runat="server">
    <title>Qerza - Job Details</title>
    <meta charset="utf-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge" />

    <!-- CSS Files -->
    <link href="xhtml/vendor/jqvmap/css/jqvmap.min.css" rel="stylesheet" />
    <link rel="stylesheet" href="xhtml/vendor/chartist/css/chartist.min.css" />
    <link href="xhtml/vendor/bootstrap-select/dist/css/bootstrap-select.min.css" rel="stylesheet" />
    <link href="xhtml/css/style.css" rel="stylesheet" />
    <link href="https://fonts.googleapis.com/css2?family=Poppins:wght@100;900&display=swap" rel="stylesheet" />
</head>
<body data-typography="poppins" data-theme-version="light" data-layout="vertical" data-nav-headerbg="color_1"
      data-headerbg="color_1" data-sidebar-style="mini" data-sidebarbg="color_1" data-sidebar-position="fixed"
      data-header-position="fixed" data-container="wide" direction="ltr" data-primary="color_1">

    <form id="form1" runat="server">
        <div id="main-wrapper">

            <div class="nav-header">
                <a href="JobSeekerDashboard.aspx" class="brand-logo">
                    <img src="xhtml/images/newme3.png" alt="Logo" />
                </a>
                <div class="nav-control">
                    <div class="hamburger">
                        <span class="line"></span><span class="line"></span><span class="line"></span>
                    </div>
                </div>
            </div>

            <div class="header">
                <div class="header-content">
                    <nav class="navbar navbar-expand">
                        <div class="collapse navbar-collapse justify-content-between">
                            <div class="header-left">
                                <div class="dashboard_bar">Job Details</div>
                            </div>
                            <ul class="navbar-nav header-right">
                                <li class="nav-item dropdown header-profile">
                                    <a class="nav-link" href="javascript:void(0)" role="button" data-bs-toggle="dropdown">
                                        <div class="header-info">
                                            <span class="text-black"><strong>
                                                <asp:Label ID="lblJobSeekerName" runat="server" Text="John Doe"></asp:Label></strong></span>
                                            <p class="fs-12 mb-0">Job Seeker</p>
                                        </div>
                                        <img src="xhtml/images/profile/17.jpg" width="20" alt="Profile" />
                                    </a>
                                    <div class="dropdown-menu dropdown-menu-end">
                                        <asp:LinkButton ID="lnkViewProfile" runat="server" CssClass="dropdown-item" OnClick="lnkViewProfile_Click">
                                            <i class="fas fa-user"></i><span class="ms-2">Profile</span>
                                        </asp:LinkButton>
                                        <asp:LinkButton ID="lnkInbox" runat="server" CssClass="dropdown-item" OnClick="lnkInbox_Click">
                                            <i class="fas fa-envelope"></i><span class="ms-2">Inbox</span>
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
                                <li><a href="Rejected.aspx?status=Rejected">Rejected</a></li>
                            </ul>
                        </li>
                        <li>
                            <a href="LikeJob.aspx" class="ai-icon">
                                <i class="flaticon-381-heart"></i>
                                <span class="nav-text">Liked Jobs</span>
                            </a>
                        </li>
                    </ul>
                </div>
            </div>

            <div class="content-body">
                <div class="container-fluid">
                    <div class="row">
                        <div class="col-xl-12">
                            <div class="card">
                                <div class="card-header">
                                    <h4 class="card-title">Job Details</h4>
                                </div>
                                <div class="card-body">
                                    <h5>Title: <asp:Label ID="lblTitle" runat="server"></asp:Label></h5>
                                    <h6>Company: <asp:Label ID="lblCompany" runat="server"></asp:Label></h6>
                                    <p><strong>Location:</strong> <asp:Label ID="lblLocation" runat="server"></asp:Label></p>
                                    <p><strong>Salary:</strong> <asp:Label ID="lblSalary" runat="server"></asp:Label></p>
                                    <p><strong>Job Type:</strong> <asp:Label ID="lblJobType" runat="server"></asp:Label></p>
                                    <p><strong>Application Deadline:</strong> <asp:Label ID="lblDeadline" runat="server"></asp:Label></p>
                                    <p><strong>Description:</strong><br />
                                        <asp:Label ID="lblDescription" runat="server" CssClass="job-description"></asp:Label>
                                    </p>

                                    <asp:Button ID="btnApply" runat="server" Text="Apply Now" CssClass="btn btn-success" OnClick="btnApply_Click" />
                                    <asp:Label ID="lblMessage" runat="server" CssClass="text-success" Visible="false"></asp:Label>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>

        </div>
    </form>

    <!-- Scripts -->
    <script src="xhtml/vendor/global/global.min.js"></script>
    <script src="xhtml/vendor/bootstrap-select/dist/js/bootstrap-select.min.js"></script>
    <script src="xhtml/vendor/chart-js/chart.bundle.min.js"></script>
    <script src="xhtml/js/custom.min.js"></script>
    <script src="xhtml/js/deznav-init.js"></script>
    <script src="xhtml/js/demo.js"></script>
</body>
</html>