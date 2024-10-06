<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ManageEmployer.aspx.cs" Inherits="OnlineJobPortal.ManageEmployer" %>

<!DOCTYPE html>
<html lang="en">
<head runat="server">
    <title>Manage Employers</title>
    <meta charset="utf-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge" />
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
            <div class="nav-header">
                <a href="AdminDashboard.aspx" class="brand-logo">
                    <img src="xhtml/images/newme3.png" alt="Logo" class="nav_logo"/>
                </a>
            </div>

            <div class="header">
                <div class="header-content">
                    <nav class="navbar navbar-expand">
                        <div class="collapse navbar-collapse justify-content-between">
                            <div class="header-left">
                                <div class="dashboard_bar">Manage Employers</div>
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

            <div class="deznav">
                <div class="deznav-scroll">
                    <ul class="metismenu" id="menu">
                        <li>
                            <a href="ManageJobSeeker.aspx">
                                <span class="nav-text">Manage Job Seeker</span>
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

            <div class="content-body">
                <div class="container-fluid">
                    <h2 class="mt-5">Employer Management</h2>
                    <asp:Label ID="lblError" runat="server" CssClass="text-danger mt-2" Text=""></asp:Label>
                    <asp:GridView ID="GridViewEmployers" runat="server" AutoGenerateColumns="False" 
                        OnRowDeleting="GridViewEmployers_RowDeleting" 
                        CssClass="table table-bordered mt-4" DataKeyNames="EmployerID">
                        <Columns>
                            <asp:BoundField DataField="EmployerID" HeaderText="Employer ID" ReadOnly="True" />
                            <asp:BoundField DataField="Username" HeaderText="Username" />
                            <asp:BoundField DataField="CompanyName" HeaderText="Company Name" />
                            <asp:BoundField DataField="Email" HeaderText="Email" />
                            <asp:BoundField DataField="ContactPerson" HeaderText="Contact Person" />
                            <asp:BoundField DataField="ContactNumber" HeaderText="Contact Number" />
                            <asp:CommandField ShowDeleteButton="True" />
                        </Columns>
                    </asp:GridView>
                </div>
            </div>
        </div>
    </form>

    <script src="xhtml/vendor/global/global.min.js"></script>
    <script src="xhtml/vendor/bootstrap-select/dist/js/bootstrap-select.min.js"></script>
    <script src="xhtml/js/custom.min.js"></script>
    <script src="xhtml/js/deznav-init.js"></script>

</body>
</html>