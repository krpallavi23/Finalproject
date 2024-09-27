<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AppliedJob.aspx.cs" Inherits="OnlineJobPortal.AppliedJob" %>

<!DOCTYPE html>
<html lang="en" dir="ltr">
<head runat="server">
    <title>Your Applied Jobs | Online Job Portal</title>
    <meta charset="utf-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge" />
    <link href="xhtml/vendor/bootstrap-select/dist/css/bootstrap-select.min.css" rel="stylesheet" />
    <link href="xhtml/css/style.css" rel="stylesheet" />
    <link href="https://fonts.googleapis.com/css2?family=Poppins:wght@100;900&amp;display=swap" rel="stylesheet" />
</head>
<body data-typography="poppins" data-theme-version="light" data-layout="vertical">

    <form id="form1" runat="server">
        <div id="main-wrapper">
            <div class="nav-header">
                <a href="JobSeekerDashboard.aspx" class="brand-logo">
                    <img src="xhtml/images/logo.png" alt="Logo" />
                </a>
            </div>

            <div class="header">
                <div class="header-content">
                    <nav class="navbar navbar-expand">
                        <div class="collapse navbar-collapse justify-content-between">
                            <div class="header-left">
                                <div class="dashboard_bar">Applied Jobs</div>
                            </div>
                            <ul class="navbar-nav header-right">
                                <li class="nav-item dropdown header-profile">
                                    <a class="nav-link" href="javascript:void(0)" role="button" data-bs-toggle="dropdown">
                                        <div class="header-info">
                                            <span class="text-black"><strong><asp:Label ID="lblJobSeekerName" runat="server" Text="John Doe"></asp:Label></strong></span>
                                            <p class="fs-12 mb-0">Job Seeker</p>
                                        </div>
                                        <img src="xhtml/images/profile/17.jpg" width="20" alt="Profile" />
                                    </a>
                                    <div class="dropdown-menu dropdown-menu-end">
                                        <asp:LinkButton ID="lnkViewProfile" runat="server" CssClass="dropdown-item" OnClick="lnkViewProfile_Click">Profile</asp:LinkButton>
                                        <asp:LinkButton ID="lnkLogout" runat="server" CssClass="dropdown-item" OnClick="lnkLogout_Click">Logout</asp:LinkButton>
                                    </div>
                                </li>
                            </ul>
                        </div>
                    </nav>
                </div>
            </div>

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
                                <li><a href="ShortlistedJob.aspx?status=Shortlisted">Shortlisted</a></li>
                                <li><a href="SelectedJob.aspx?status=Selected">Selected</a></li>
                                <li><a href="RejectedJobs.aspx?status=Rejected">Rejected</a></li>
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

            <div class="content-body">
                <div class="container-fluid">
                    <div class="row">
                        <div class="col-xl-12">
                            <div class="card">
                                <div class="card-header">
                                    <h4 class="card-title">Your Applied Jobs</h4>
                                </div>
                                <div class="card-body">
                                    <asp:GridView ID="gvAppliedJobs" runat="server" CssClass="table table-responsive-lg" AutoGenerateColumns="False" AllowPaging="True" PageSize="10" OnPageIndexChanging="gvAppliedJobs_PageIndexChanging">
                                        <Columns>
                                            <asp:BoundField DataField="JobTitle" HeaderText="Job Title" />
                                            <asp:BoundField DataField="CompanyName" HeaderText="Company" />
                                            <asp:BoundField DataField="ApplicationDate" HeaderText="Applied On" DataFormatString="{0:MM/dd/yyyy}" />
                                            <asp:TemplateField HeaderText="Status">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblStatus" runat="server" Text='<%# Eval("Status") %>' CssClass='<%# Eval("Status") == "Accepted" ? "text-success" : "text-danger" %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Actions">
                                                <ItemTemplate>
                                                    <a href='JobDetails.aspx?JobID=<%# Eval("JobID") %>' class="btn btn-primary btn-sm">View Details</a>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                    <asp:Label ID="lblStatusMessage" runat="server" CssClass="mt-2 text-danger"></asp:Label>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </form>

    <script src="xhtml/vendor/global/global.min.js"></script>
    <script src="xhtml/vendor/bootstrap-select/dist/js/bootstrap-select.min.js"></script>
    <script src="xhtml/js/custom.min.js"></script>
</body>
</html>
