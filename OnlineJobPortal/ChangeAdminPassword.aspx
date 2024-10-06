<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ChangeAdminPassword.aspx.cs" Inherits="OnlineJobPortal.ChangeAdminPassword" %>

<!DOCTYPE html>
<html lang="en" dir="ltr">
<head runat="server">
    <title>Qerza - Change Password | Admin Dashboard</title>
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <link rel="icon" type="image/png" href="xhtml/images/favicon.png" />
    <link href="xhtml/css/style.css" rel="stylesheet" />
    <link href="https://fonts.googleapis.com/css2?family=Poppins:wght@400;600&display=swap" rel="stylesheet" />
</head>
<body data-typography="poppins" data-theme-version="light" data-layout="vertical">
    <form id="form1" runat="server">
        <div id="Div1" class="show">
            <div class="nav-header">
                <a href="AdminDashboard.aspx" class="brand-logo">
                    <img src="xhtml/images/newme3.png" alt="Logo" />
                </a>
            </div>

            <div class="header">
                <div class="header-content">
                    <nav class="navbar navbar-expand">
                        <div class="collapse navbar-collapse justify-content-between">
                            <div class="header-left">
                                <div class="dashboard_bar">
                                    Change Password
                                </div>
                            </div>
                            <ul class="navbar-nav header-right">
                                <li class="nav-item dropdown header-profile">
                                    <a class="nav-link" href="javascript:void(0)" role="button" data-bs-toggle="dropdown">
                                        <div class="header-info">
                                            <span class="text-black"><strong>
                                                <asp:Label ID="lblAdminName" runat="server" Text="Admin Name"></asp:Label></strong></span>
                                            <p class="fs-12 mb-0">Administrator</p>
                                        </div>
                                        <img src="xhtml/images/profile/pro_1.png" width="20" alt="Profile" />
                                    </a>
                                    <div class="dropdown-menu dropdown-menu-end">
                                        <asp:LinkButton ID="lnkViewProfile" runat="server" CssClass="dropdown-item" OnClick="lnkViewProfile_Click">
                                            <i class="fas fa-user"></i><span class="ms-2">Profile</span>
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
        </div>
    </form>
    <script src="xhtml/vendor/global/global.min.js"></script>
    <script src="xhtml/js/custom.min.js"></script>
</body>
</html>
