<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ShortlistedJobs.aspx.cs" Inherits="OnlineJobPortal.ShortlistedJobs" %>

<!DOCTYPE html>
<html lang="en" dir="ltr">
<head runat="server">
    <!-- Title -->
    <title>Qerza - Shortlisted Jobs | Job Portal</title>

    <!-- Meta -->
    <meta charset="utf-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge" />
    <!-- CSS Files -->
    <link href="xhtml/vendor/jqvmap/css/jqvmap.min.css" rel="stylesheet" />
    <link rel="stylesheet" href="xhtml/vendor/chartist/css/chartist.min.css" />
    <link href="xhtml/vendor/bootstrap-select/dist/css/bootstrap-select.min.css" rel="stylesheet" />
    <link href="xhtml/css/style.css" rel="stylesheet" />
    <!-- Fonts -->
    <link href="https://fonts.googleapis.com/css2?family=Poppins:wght@100;900&amp;display=swap" rel="stylesheet" />
     <link href="https://fonts.googleapis.com/css2?family=Poppins:wght@100;200;300;400;500;600;700;800;900&family=Roboto:wght@100;300;400;500;700;900&display=swap" rel="stylesheet" />
</head>
<body data-typography="poppins" data-theme-version="light" data-layout="vertical" data-nav-headerbg="color_1"
      data-headerbg="color_1" data-sidebar-style="mini" data-sidebarbg="color_1" data-sidebar-position="fixed"
      data-header-position="fixed" data-container="wide">

    <form id="form1" runat="server">
        <div id="main-wrapper">

            <!-- Nav header start -->
            <div class="nav-header">
                <a href="JobSeekerDashboard.aspx" class="brand-logo">
                    <img src="xhtml/images/Logo_purp.png" alt="Logo" />
                </a>
                <div class="nav-control">
                    <div class="hamburger">
                        <span class="line"></span><span class="line"></span><span class="line"></span>
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
                                <div class="dashboard_bar">Shortlisted Jobs</div>
                            </div>
                            <ul class="navbar-nav header-right">
                                <li class="nav-item dropdown header-profile">
                                    <a class="nav-link" href="javascript:void(0)" role="button" data-bs-toggle="dropdown">
                                        <div class="header-info">
                                            <span class="text-black"><strong><asp:Label ID="lblJobSeekerName" runat="server" Text="John Doe"></asp:Label></strong></span>
                                            <p class="fs-12 mb-0">Job Seeker</p>
                                        </div>
                                        <img src="xhtml/images/profile/pro_1.png" width="20" alt="Profile" />
                                    </a>
                                    <div class="dropdown-menu dropdown-menu-end">
                                        <asp:LinkButton ID="lnkViewProfile" runat="server" CssClass="dropdown-item ai-icon" OnClick="lnkViewProfile_Click">
                                            <i class="fas fa-user"></i>
                                            <span class="ms-2">Profile</span>
                                        </asp:LinkButton>
                                        <asp:LinkButton ID="lnkInbox" runat="server" CssClass="dropdown-item ai-icon" OnClick="lnkInbox_Click">
                                            <i class="fas fa-envelope"></i>
                                            <span class="ms-2">Inbox</span>
                                        </asp:LinkButton>
                                        <asp:LinkButton ID="lnkLogout" runat="server" CssClass="dropdown-item ai-icon" OnClick="lnkLogout_Click">
                                            <i class="fas fa-sign-out-alt"></i>
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
                                <li><a href="AppliedJob.aspx">Applied</a></li>
                                <li><a href="ShortlistedJob.aspx">Shortlisted</a></li>
                                <li><a href="SelectedJob.aspx?status=Selected">Selected</a></li>
                                <li><a href="RejectedJobss.aspx?status=Rejected">Rejected</a></li>
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
            <!-- Sidebar end -->

            <!-- Content body start -->
            <div class="content-body">
                <div class="container-fluid">

                    <!-- Shortlisted Job Listings Grid -->
                    <div class="row">
                        <div class="col-xl-12">
                            <div class="card">
                                <div class="card-header">
                                    <h4 class="card-title">Shortlisted Jobs</h4>
                                </div>
                                <div class="card-body">
                                    <asp:GridView ID="gvShortlistedJobs" runat="server" CssClass="table table-responsive-lg" AutoGenerateColumns="False" AllowPaging="True" PageSize="10" OnPageIndexChanging="gvShortlistedJobs_PageIndexChanging">
                                        <Columns>
                                            <asp:BoundField DataField="JobTitle" HeaderText="Job Title" />
                                            <asp:BoundField DataField="CompanyName" HeaderText="Company" />
                                            <asp:BoundField DataField="Location" HeaderText="Location" />
                                            <asp:BoundField DataField="JobType" HeaderText="Job Type" />
                                            <asp:TemplateField HeaderText="Actions">
                                                <ItemTemplate>
                                                    <a href='JobDetails.aspx?JobID=<%# Eval("JobID") %>' class="btn btn-primary btn-sm">View Details</a>
                                                    <asp:Button ID="btnRemove" runat="server" Text="Remove" CommandArgument='<%# Eval("JobID") %>' OnClick="btnRemove_Click" CssClass="btn btn-danger btn-sm ms-2" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                    <asp:Label ID="lblStatus" runat="server" CssClass="mt-2 text-danger"></asp:Label>
                                </div>
                            </div>
                        </div>
                    </div>

                </div>
            </div>
            <!-- Content body end -->

        </div>
    </form>
    <!-- Main wrapper end -->

    <!-- Scripts -->
    <script src="xhtml/vendor/global/global.min.js"></script>
    <script src="xhtml/vendor/bootstrap-select/dist/js/bootstrap-select.min.js"></script>
    <script src="xhtml/vendor/chart-js/chart.bundle.min.js"></script>
    <script src="xhtml/js/custom.min.js"></script>
    <script src="xhtml/js/deznav-init.js"></script>
    <script src="xhtml/js/demo.js"></script>
</body>
</html>
