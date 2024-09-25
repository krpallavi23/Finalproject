<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EmployerDashboard.aspx.cs" Inherits="OnlineJobPortal.EmployerDashboard" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml" lang="en">
<head runat="server">
    <!-- Self-Closing Meta Tags for XHTML5 Compliance -->
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1, shrink-to-fit=no" />
    <meta name="description" content="Employer Dashboard - Manage Your Jobs and Candidates" />
    <meta name="keywords" content="Employer Dashboard, Job Management, Candidate Search" />
    <meta name="author" content="YourName" />
    <title>EmployerDashboard &mdash; Manage Your Jobs and Candidates</title>
    
    <!-- Favicon -->
    <link rel="shortcut icon" href="images/ftco-32x32.png" />
    
    <!-- CSS Files -->
    <link rel="stylesheet" href="css/custom-bs.css" />
    <link rel="stylesheet" href="css/jquery.fancybox.min.css" />
    <link rel="stylesheet" href="css/bootstrap-select.min.css" />
    <link rel="stylesheet" href="fonts/icomoon/style.css" />
    <link rel="stylesheet" href="fonts/line-icons/style.css" />
    <link rel="stylesheet" href="css/owl.carousel.min.css" />
    <link rel="stylesheet" href="css/animate.min.css" />

    <!-- MAIN CSS -->
    <link rel="stylesheet" href="css/style.css" />
    
    <!-- Additional CSS for Dashboard -->
    <style>
        /* Sidebar Styles */
        .sidebar {
            height: 100vh;
            background-color: #2c3e50;
            padding-top: 20px;
            position: fixed;
            width: 200px;
            transition: all 0.3s;
        }
        .sidebar a {
            color: #ecf0f1;
            padding: 15px 20px;
            display: block;
            text-decoration: none;
            font-size: 16px;
        }
        .sidebar a:hover, .sidebar a.active {
            background-color: #34495e;
            color: #ffffff;
        }
        /* Main Content Styles */
        .main-content {
            padding: 20px;
            margin-left: 200px; /* Same as sidebar width */
            transition: all 0.3s;
        }
        /* Top Navigation Styles */
        .top-nav {
            background-color: #ffffff;
            padding: 10px 20px;
            box-shadow: 0 2px 4px rgba(0,0,0,0.1);
            position: fixed;
            width: calc(100% - 200px);
            margin-left: 200px; /* Same as sidebar width */
            z-index: 1000;
            display: flex;
            align-items: center;
            justify-content: space-between;
        }
        .top-nav .search-bar {
            width: 300px;
            display: inline-block;
        }
        .top-nav .profile-section {
            position: relative;
        }
        .profile-section img {
            width: 40px;
            height: 40px;
            border-radius: 50%;
            cursor: pointer;
        }
        .dropdown-menu {
            min-width: 150px;
        }
        /* Dashboard Widgets */
        .widget {
            background-color: #ffffff;
            padding: 20px;
            margin-bottom: 20px;
            border-radius: 8px;
            box-shadow: 0 1px 3px rgba(0,0,0,0.1);
        }
        .widget h3 {
            margin-bottom: 15px;
            color: #2c3e50;
        }
        /* Responsive Layout */
        @media (max-width: 992px) {
            .sidebar {
                width: 100%;
                height: auto;
                position: relative;
            }
            .main-content {
                margin-left: 0;
                margin-top: 60px; /* Height of top-nav */
            }
            .top-nav {
                width: 100%;
                margin-left: 0;
                justify-content: space-between;
            }
            .top-nav .search-bar {
                width: 100%;
                margin: 10px 0;
            }
        }
    </style>
</head>
<body id="top">
    <form id="form1" runat="server">
        <!-- Loader and Overlayer -->
        <div id="overlayer"></div>
        <div class="loader">
            <div class="spinner-border text-primary" role="status">
                <span class="sr-only">Loading...</span>
            </div>
        </div>

        <div class="site-wrap">

            <!-- TOP NAVIGATION BAR -->
            <header class="top-nav">
                <!-- Site Logo -->
                <asp:Label ID="lblSiteLogo" runat="server" Text="EmployerDashboard" CssClass="site-logo" Style="font-size:24px; font-weight:bold; color:#2c3e50;"></asp:Label>
                
                <!-- Search Bar -->
                <asp:TextBox ID="txtSearch" runat="server" CssClass="form-control search-bar" Placeholder="Search..."></asp:TextBox>
                
                <!-- Profile Section -->
                <div class="profile-section">
                    <asp:ImageButton ID="imgProfile" runat="server" ImageUrl="images/profile.png" OnClick="imgProfile_Click" />
                    <asp:Panel ID="pnlProfileMenu" runat="server" CssClass="dropdown-menu" Style="display:none; position:absolute; right:0; background-color:#fff; box-shadow: 0 2px 5px rgba(0,0,0,0.15); border-radius:5px; z-index:1000;">
                        <asp:LinkButton ID="lnkEditProfile" runat="server" CssClass="dropdown-item" Text="Edit Profile" OnClick="lnkEditProfile_Click" />
                        <asp:LinkButton ID="lnkSettings" runat="server" CssClass="dropdown-item" Text="Settings" OnClick="lnkSettings_Click" />
                        <asp:LinkButton ID="lnkLogout" runat="server" CssClass="dropdown-item" Text="Logout" OnClick="lnkLogout_Click" />
                    </asp:Panel>
                </div>
            </header>

            <div class="d-flex">
                <!-- LEFT SIDEBAR MENU -->
                <nav class="sidebar d-none d-lg-block">
                    <asp:HyperLink ID="lnkDashboard" runat="server" NavigateUrl="EmployerDashboard.aspx" CssClass="active"><span class="icon-home mr-2"></span> Dashboard</asp:HyperLink>
                    <asp:HyperLink ID="lnkPostJob" runat="server" NavigateUrl="PostJob.aspx"><span class="icon-plus-circle mr-2"></span> Post Job</asp:HyperLink>
                    <asp:HyperLink ID="lnkManageJob" runat="server" NavigateUrl="ManageJob.aspx"><span class="icon-edit mr-2"></span> Manage Job</asp:HyperLink>
                    <asp:HyperLink ID="lnkSearchCandidates" runat="server" NavigateUrl="SearchCandidates.aspx"><span class="icon-search mr-2"></span> Search Candidates</asp:HyperLink>
                    <asp:HyperLink ID="lnkProfileMatch" runat="server" NavigateUrl="ProfileMatch.aspx"><span class="icon-clipboard mr-2"></span> Profile Match</asp:HyperLink>
                </nav>

                <!-- MAIN CONTENT -->
                <div class="main-content w-100">
                    <div class="container-fluid">
                        <!-- Dashboard Header -->
                        <div class="row mb-4">
                            <div class="col-12">
                                <h2>Welcome, <asp:Label ID="lblEmployerName" runat="server" Text=""></asp:Label></h2>
                            </div>
                        </div>

                        <!-- Dashboard Widgets -->
                        <div class="row">
                            <!-- Jobs Posted Widget -->
                            <div class="col-md-3">
                                <div class="widget">
                                    <h3>Total Jobs Posted</h3>
                                    <h4><asp:Label ID="lblTotalJobs" runat="server" Text="0"></asp:Label></h4>
                                </div>
                            </div>
                            <!-- Candidates Shortlisted Widget -->
                            <div class="col-md-3">
                                <div class="widget">
                                    <h3>Candidates Shortlisted</h3>
                                    <h4><asp:Label ID="lblCandidatesShortlisted" runat="server" Text="0"></asp:Label></h4>
                                </div>
                            </div>
                            <!-- Pending Reviews Widget -->
                            <div class="col-md-3">
                                <div class="widget">
                                    <h3>Pending Reviews</h3>
                                    <h4><asp:Label ID="lblPendingReviews" runat="server" Text="0"></asp:Label></h4>
                                </div>
                            </div>
                            <!-- Interviews Scheduled Widget -->
                            <div class="col-md-3">
                                <div class="widget">
                                    <h3>Interviews Scheduled</h3>
                                    <h4><asp:Label ID="lblInterviewsScheduled" runat="server" Text="0"></asp:Label></h4>
                                </div>
                            </div>
                        </div>

                        <!-- Data Tables Section -->
                        <div class="row">
                            <!-- Jobs Posted Over Time Table -->
                            <div class="col-md-6">
                                <div class="widget">
                                    <h3>Jobs Posted Over Time</h3>
                                    <asp:GridView ID="gvJobsPosted" runat="server" CssClass="table table-striped" AllowSorting="True" OnSorting="gvJobsPosted_Sorting">
                                        <Columns>
                                            <asp:BoundField DataField="Month" HeaderText="Month" SortExpression="Month" />
                                            <asp:BoundField DataField="JobsPosted" HeaderText="Jobs Posted" SortExpression="JobsPosted" />
                                            <asp:BoundField DataField="ChangePercentage" HeaderText="Change (%)" DataFormatString="{0:N2}%" SortExpression="ChangePercentage" />
                                        </Columns>
                                    </asp:GridView>
                                </div>
                            </div>
                            <!-- Candidates Matched Table -->
                            <div class="col-md-6">
                                <div class="widget">
                                    <h3>Candidates Matched</h3>
                                    <asp:GridView ID="gvCandidatesMatched" runat="server" CssClass="table table-striped" AllowSorting="True" OnSorting="gvCandidatesMatched_Sorting">
                                        <Columns>
                                            <asp:BoundField DataField="Department" HeaderText="Department" SortExpression="Department" />
                                            <asp:BoundField DataField="CandidatesMatched" HeaderText="Candidates Matched" SortExpression="CandidatesMatched" />
                                            <asp:BoundField DataField="ChangePercentage" HeaderText="Change (%)" DataFormatString="{0:N2}%" SortExpression="ChangePercentage" />
                                        </Columns>
                                    </asp:GridView>
                                </div>
                            </div>
                        </div>

                        <!-- Task Progress Bars -->
                        <div class="row">
                            <div class="col-md-6">
                                <div class="widget">
                                    <h3>Job Postings Progress</h3>
                                    <div class="mb-3">
                                        <asp:Label ID="lblTotalJobsProgress" runat="server" Text="Total Jobs Posted"></asp:Label>
                                        <div class="progress">
                                            <div class="progress-bar bg-primary" role="progressbar" style="width: 0%;" aria-valuenow="0" aria-valuemin="0" aria-valuemax="100" id="divTotalJobsProgress" runat="server">
                                            </div>
                                        </div>
                                        <span><asp:Label ID="lblTotalJobsPercent" runat="server" Text="0%"></asp:Label></span>
                                    </div>
                                    
                                    <div class="mb-3">
                                        <asp:Label ID="lblCandidatesShortlistedProgress" runat="server" Text="Candidates Shortlisted"></asp:Label>
                                        <div class="progress">
                                            <div class="progress-bar bg-success" role="progressbar" style="width: 0%;" aria-valuenow="0" aria-valuemin="0" aria-valuemax="100" id="divCandidatesShortlistedProgress" runat="server">
                                            </div>
                                        </div>
                                        <span><asp:Label ID="lblCandidatesShortlistedPercent" runat="server" Text="0%"></asp:Label></span>
                                    </div>
                                    
                                    <div class="mb-3">
                                        <asp:Label ID="lblPendingReviewsProgress" runat="server" Text="Pending Reviews"></asp:Label>
                                        <div class="progress">
                                            <div class="progress-bar bg-warning" role="progressbar" style="width: 0%;" aria-valuenow="0" aria-valuemin="0" aria-valuemax="100" id="divPendingReviewsProgress" runat="server">
                                            </div>
                                        </div>
                                        <span><asp:Label ID="lblPendingReviewsPercent" runat="server" Text="0%"></asp:Label></span>
                                    </div>
                                    
                                    <div class="mb-3">
                                        <asp:Label ID="lblInterviewsScheduledProgress" runat="server" Text="Interviews Scheduled"></asp:Label>
                                        <div class="progress">
                                            <div class="progress-bar bg-danger" role="progressbar" style="width: 0%;" aria-valuenow="0" aria-valuemin="0" aria-valuemax="100" id="divInterviewsScheduledProgress" runat="server">
                                            </div>
                                        </div>
                                        <span><asp:Label ID="lblInterviewsScheduledPercent" runat="server" Text="0%"></asp:Label></span>
                                    </div>
                                </div>
                            </div>
                            <!-- Server Stats Widgets -->
                            <div class="col-md-6">
                                <div class="widget">
                                    <h3>Server Stats</h3>
                                    <div class="mb-3">
                                        <strong>Server Load:</strong>
                                        <div class="progress">
                                            <div class="progress-bar bg-info" role="progressbar" style="width: 0%;" aria-valuenow="0" aria-valuemin="0" aria-valuemax="100" id="divServerLoadProgress" runat="server">
                                            </div>
                                        </div>
                                        <span><asp:Label ID="lblServerLoad" runat="server" Text="0%"></asp:Label></span>
                                    </div>
                                    <div class="mb-3">
                                        <strong>Disk Space:</strong>
                                        <div class="progress">
                                            <div class="progress-bar bg-warning" role="progressbar" style="width: 0%;" aria-valuenow="0" aria-valuemin="0" aria-valuemax="100" id="divDiskSpaceProgress" runat="server">
                                            </div>
                                        </div>
                                        <span><asp:Label ID="lblDiskSpace" runat="server" Text="0%"></asp:Label></span>
                                    </div>
                                    <div class="mb-3">
                                        <strong>Memory Usage:</strong>
                                        <div class="progress">
                                            <div class="progress-bar bg-success" role="progressbar" style="width: 0%;" aria-valuenow="0" aria-valuemin="0" aria-valuemax="100" id="divMemoryUsageProgress" runat="server">
                                            </div>
                                        </div>
                                        <span><asp:Label ID="lblMemoryUsage" runat="server" Text="0%"></asp:Label></span>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <!-- Notifications Area -->
                        <div class="row">
                            <div class="col-md-12">
                                <div class="widget">
                                    <h3>Notifications</h3>
                                    <asp:Repeater ID="rptNotifications" runat="server">
                                        <ItemTemplate>
                                            <div class="notification-item mb-2">
                                                <p><%# Eval("NotificationText") %></p>
                                                <small><%# Eval("Date") %></small>
                                                <hr />
                                            </div>
                                        </ItemTemplate>
                                    </asp:Repeater>
                                </div>
                            </div>
                        </div>

                        <!-- Smart Chat Area -->
                        <div class="row">
                            <div class="col-md-12">
                                <div class="widget">
                                    <h3>Smart Chat</h3>
                                    <asp:TextBox ID="txtChat" runat="server" CssClass="form-control" Placeholder="Type a message..."></asp:TextBox>
                                    <asp:Button ID="btnSend" runat="server" Text="Send" CssClass="btn btn-primary mt-2" OnClick="btnSend_Click" />
                                    <asp:Repeater ID="rptChatMessages" runat="server">
                                        <ItemTemplate>
                                            <div class="chat-message mb-2">
                                                <p><%# Eval("Message") %></p>
                                                <small><%# Eval("Time") %></small>
                                                <hr />
                                            </div>
                                        </ItemTemplate>
                                    </asp:Repeater>
                                </div>
                            </div>
                        </div>

                        <!-- Map or Visualization Section -->
                        <div class="row">
                            <div class="col-md-12">
                                <div class="widget">
                                    <h3>Candidate Locations</h3>
                                    <!-- Placeholder for map or heatmap -->
                                    <div id="map" style="width: 100%; height: 400px; background-color:#eaeaea;" runat="server">
                                        <!-- Integrate a map library like Google Maps or Leaflet here -->
                                        <p class="text-center mt-5">Map Visualization Placeholder</p>
                                    </div>
                                </div>
                            </div>
                        </div>

                    </div>
                </div>
            </div>

            <!-- FOOTER -->
            <footer class="site-footer">
                <div class="container">
                    <div class="row mb-5">
                        <div class="col-6 col-md-3 mb-4 mb-md-0">
                            <h3>Search Trending</h3>
                            <ul class="list-unstyled">
                                <li><a href="#">Web Design</a></li>
                                <li><a href="#">Graphic Design</a></li>
                                <li><a href="#">Web Developers</a></li>
                                <li><a href="#">Python</a></li>
                                <li><a href="#">HTML5</a></li>
                                <li><a href="#">CSS3</a></li>
                            </ul>
                        </div>
                        <div class="col-6 col-md-3 mb-4 mb-md-0">
                            <h3>Company</h3>
                            <ul class="list-unstyled">
                                <li><a href="About.aspx">About Us</a></li>
                                <li><a href="#">Career</a></li>
                                <li><a href="#">Blog</a></li>
                                <li><a href="#">Resources</a></li>
                            </ul>
                        </div>
                        <div class="col-6 col-md-3 mb-4 mb-md-0">
                            <h3>Support</h3>
                            <ul class="list-unstyled">
                                <li><a href="#">Support</a></li>
                                <li><a href="#">Privacy</a></li>
                                <li><a href="#">Terms of Service</a></li>
                            </ul>
                        </div>
                        <div class="col-6 col-md-3 mb-4 mb-md-0">
                            <h3>Contact Us</h3>
                            <div class="footer-social">
                                <a href="#"><span class="icon-facebook"></span></a>
                                <a href="#"><span class="icon-twitter"></span></a>
                                <a href="#"><span class="icon-instagram"></span></a>
                                <a href="#"><span class="icon-linkedin"></span></a>
                            </div>
                        </div>
                    </div>

                    <div class="row text-center">
                        <div class="col-12">
                            <p class="copyright"><small>
                                &copy;<script>document.write(new Date().getFullYear());</script> All rights reserved | This template is made with <i class="icon-heart text-danger" aria-hidden="true"></i> by <a href="https://colorlib.com" target="_blank">Colorlib</a>
                            </small></p>
                        </div>
                    </div>
                </div>
            </footer>
          
        </div>

        <!-- SCRIPTS -->
        <script src="js/jquery.min.js"></script>
        <script src="js/bootstrap.bundle.min.js"></script>
        <script src="js/isotope.pkgd.min.js"></script>
        <script src="js/stickyfill.min.js"></script>
        <script src="js/jquery.fancybox.min.js"></script>
        <script src="js/jquery.easing.1.3.js"></script>
        
        <script src="js/jquery.waypoints.min.js"></script>
        <script src="js/jquery.animateNumber.min.js"></script>
        <script src="js/owl.carousel.min.js"></script>
        
        <script src="js/bootstrap-select.min.js"></script>
        
        <script src="js/custom.js"></script>
        
        <!-- Additional Scripts for Dashboard Functionality -->
        <script>
            // Toggle Profile Menu
            $(document).ready(function () {
                $('#<%= imgProfile.ClientID %>').click(function (e) {
                    e.stopPropagation(); // Prevent the click from bubbling up
                    $('#<%= pnlProfileMenu.ClientID %>').toggle();
                });
                // Hide profile menu when clicking outside
                $(document).click(function (event) {
                    var $target = $(event.target);
                    if (!$target.closest('#<%= imgProfile.ClientID %>').length &&
                        !$target.closest('#<%= pnlProfileMenu.ClientID %>').length) {
                        $('#<%= pnlProfileMenu.ClientID %>').hide();
                    }
                });
            });
        </script>
    </form>
</body>
</html>
