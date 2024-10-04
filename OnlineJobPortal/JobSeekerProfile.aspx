<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AdminProfile.aspx.cs" Inherits="OnlineJobPortal.AdminProfile" %>

<!DOCTYPE html>
<html lang="en" dir="ltr">
<head runat="server">
    <title>Admin Profile Management</title>
    <meta charset="utf-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge" />
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <link rel="icon" type="image/png" href="xhtml/images/favicon.png" />

    <!-- Bootstrap CSS -->
    <link href="xhtml/vendor/bootstrap/css/bootstrap.min.css" rel="stylesheet" />

    <!-- Custom CSS -->
    <link href="xhtml/css/style.css" rel="stylesheet" />
</head>
<body data-typography="poppins" data-theme-version="light" data-layout="vertical" data-nav-headerbg="color_1"
      data-headerbg="color_1" data-sidebarbg="color_1" data-sidebar-position="fixed"
      data-header-position="fixed" data-container="wide" direction="ltr" data-primary="color_1">
    
    <form id="form1" runat="server">
        <div id="Div1" class="show">
            <!-- Nav header start -->
            <div class="nav-header">
                <a href="AdminDashboard.aspx" class="brand-logo">
                    <img src="xhtml/images/logo.png" alt="Logo" />
                </a>
            </div>

            <!-- Header start -->
            <div class="header">
                <div class="header-content">
                    <nav class="navbar navbar-expand">
                        <div class="collapse navbar-collapse justify-content-between">
                            <div class="header-left">
                                <div class="dashboard_bar">Admin Profile Management</div>
                            </div>
                            <ul class="navbar-nav header-right">
                                <li class="nav-item dropdown header-profile">
                                    <a class="nav-link dropdown-toggle" href="javascript:void(0)" role="button" data-bs-toggle="dropdown" aria-expanded="false">
                                        <div class="header-info">
                                            <span class="text-black"><strong><asp:Label ID="lblAdminName" runat="server" Text="Admin Name"></asp:Label></strong></span>
                                            <p class="fs-12 mb-0">Administrator</p>
                                        </div>
                                        <img src="xhtml/images/profile/pix.jpg" width="20" alt="Profile" />
                                    </a>
                                    <ul class="dropdown-menu dropdown-menu-end">
                                        <li>
                                            <asp:LinkButton ID="lnkViewDashboard" runat="server" CssClass="dropdown-item" OnClick="lnkViewDashboard_Click">Dashboard</asp:LinkButton>
                                        </li>
                                        <li>
                                            <asp:LinkButton ID="lnkLogout" runat="server" CssClass="dropdown-item" OnClick="lnkLogout_Click">Logout</asp:LinkButton>
                                        </li>
                                    </ul>
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
                    <h2 class="mt-5">Admin Profile Management</h2>
                    <asp:GridView ID="GridViewAdmins" runat="server" AutoGenerateColumns="False" 
                        OnRowEditing="GridViewAdmins_RowEditing" 
                        OnRowCancelingEdit="GridViewAdmins_RowCancelingEdit" 
                        OnRowUpdating="GridViewAdmins_RowUpdating" 
                        CssClass="table table-bordered mt-4" DataKeyNames="UserID">
                        <Columns>
                            <asp:BoundField DataField="UserID" HeaderText="User ID" ReadOnly="True" />
                            <asp:TemplateField HeaderText="Username">
                                <ItemTemplate>
                                    <%# Eval("Username") %>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox ID="TextBoxUsername" runat="server" Text='<%# Bind("Username") %>' />
                                </EditItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Email">
                                <ItemTemplate>
                                    <%# Eval("Email") %>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox ID="TextBoxEmail" runat="server" Text='<%# Bind("Email") %>' />
                                </EditItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="User Type">
                                <ItemTemplate>
                                    <%# Eval("UserType") %>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:DropDownList ID="UserTypeDropDown" runat="server" SelectedValue='<%# Bind("UserType") %>'>
                                        <asp:ListItem Text="Admin" Value="Admin"></asp:ListItem>
                                        <asp:ListItem Text="JobSeeker" Value="JobSeeker"></asp:ListItem>
                                        <asp:ListItem Text="Employer" Value="Employer"></asp:ListItem>
                                    </asp:DropDownList>
                                </EditItemTemplate>
                            </asp:TemplateField>
                            <asp:CommandField ShowEditButton="True" />
                        </Columns>
                    </asp:GridView>
                </div>
            </div>
            <!-- Content Body end -->
        </div>
    </form>

    <!-- Bootstrap JS (Includes Popper.js) -->
    <script src="xhtml/vendor/bootstrap/js/bootstrap.bundle.min.js"></script>

    <!-- Optional: Include jQuery if using Bootstrap 4 -->
    <script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>

    <!-- Custom JS -->
    <script src="xhtml/js/custom.min.js"></script>
</body>
</html>
