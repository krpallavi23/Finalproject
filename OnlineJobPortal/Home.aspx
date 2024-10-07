<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Home.aspx.cs" Inherits="OnlineJobPortal.Home" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml" lang="en">
<head runat="server">
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1, shrink-to-fit=no"/>
    <meta name="description" content="Job Portal Application" />
    <meta name="keywords" content="Jobs, Employment, Career" />
    <meta name="author" content="YourName" />
    <title>JobBoard &mdash; Your Gateway to Employment</title>
    
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
</head>
<body id="top">
    <form id="form1" runat="server">
        <div id="overlayer"></div>
        <div class="loader">
            <div class="spinner-border text-primary" role="status">
                <span class="sr-only">Loading...</span>
            </div>
        </div>

        <div class="site-wrap">

            <!-- SITE MOBILE MENU -->
            <div class="site-mobile-menu site-navbar-target">
                <div class="site-mobile-menu-header">
                    <div class="site-mobile-menu-close mt-3">
                        <span class="icon-close2 js-menu-toggle"></span>
                    </div>
                </div>
                <div class="site-mobile-menu-body"></div>
            </div> <!-- .site-mobile-menu -->
            

            <!-- NAVBAR -->
            <header class="site-navbar mt-3">
                <div class="container-fluid">
                    <div class="row align-items-center">
                        <div class="site-logo col-6"><a href="Home.aspx">JobBoard</a></div>

                        <nav class="mx-auto site-navigation">
                            <ul class="site-menu js-clone-nav d-none d-xl-block ml-0 pl-0">
                                <li><a href="Home.aspx" class="nav-link active">Home</a></li>
                                <li><a href="About.aspx" class="nav-link active">About</a></li>
                                <li><a href="Contact.aspx" class="nav-link active">Contact</a></li>
                                <!-- Removed other Sign Up links -->
                            </ul>
                        </nav>
                        
                        <div class="right-cta-menu text-right d-flex align-items-center col-6">
                            <div class="ml-auto d-flex">
                                <!-- Log In Dropdown -->
                                <div class="dropdown mr-3">
                                    <button class="btn btn-primary border-width-2 dropdown-toggle" type="button" id="loginDropdown" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">
                                        <span class="mr-2 icon-lock_outline"></span>Log In
                                    </button>
                                    <div class="dropdown-menu" aria-labelledby="loginDropdown">
                                        <a class="dropdown-item" href="AdminLogin.aspx">Admin</a>
                                        <a class="dropdown-item" href="EmployerLogin.aspx">Employer</a>
                                        <a class="dropdown-item" href="JobSeekerLogin.aspx">Job Seeker</a>
                                    </div>
                                </div>

                                <!-- Sign Up Dropdown -->
                                <div class="dropdown">
                                    <button class="btn btn-warning border-width-2 dropdown-toggle" type="button" id="signupDropdown" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">
                                        <span class="mr-2 icon-user_plus"></span>Sign Up
                                    </button>
                                    <div class="dropdown-menu" aria-labelledby="signupDropdown">
                                        <a class="dropdown-item" href="EmployerSignUp.aspx">Employer</a>
                                        <a class="dropdown-item" href="JobSeekerSignUp.aspx">Job Seeker</a>
                                    </div>
                                </div>
                            </div>
                            <!-- Removed Post a Job button -->

                            <a href="#" class="site-menu-toggle js-menu-toggle d-inline-block d-xl-none mt-lg-2 ml-3">
                                <span class="icon-menu h3 m-0 p-0 mt-2"></span>
                            </a>
                        </div>

                    </div>
                </div>
            </header>

            <!-- HOME SECTION -->
            <section class="home-section section-hero overlay bg-image" style="background-image: url('images/hero_1.jpg');" id="home-section">
                <div class="container">
                    <div class="row align-items-center justify-content-center">
                        <div class="col-md-12">
                            <div class="mb-5 text-center">
                                <h1 class="text-white font-weight-bold">The Easiest Way To Get Your Dream Job</h1>
                                <p class="lead text-white">Find the perfect job that matches your skills and interests.</p>
                            </div>
                            
                            <!-- Search Jobs Form -->
                            <div class="search-jobs-form">
                                <div class="row mb-5">
                                    <div class="col-12 col-sm-6 col-md-6 col-lg-3 mb-4 mb-lg-0">
                                        <asp:TextBox ID="txtJobTitle" runat="server" CssClass="form-control form-control-lg" Placeholder="Job title, Company..."></asp:TextBox>
                                    </div>
                                    <div class="col-12 col-sm-6 col-md-6 col-lg-3 mb-4 mb-lg-0">
                                        <asp:DropDownList ID="ddlRegion" runat="server" CssClass="selectpicker btn-white btn-lg" DataStyle="btn-white btn-lg" DataWidth="100%" DataLiveSearch="True" AppendDataBoundItems="True">
                                            <asp:ListItem Text="Select Region" Value="" />
                                        </asp:DropDownList>
                                    </div>
                                    <div class="col-12 col-sm-6 col-md-6 col-lg-3 mb-4 mb-lg-0">
                                        <asp:DropDownList ID="ddlJobType" runat="server" CssClass="selectpicker btn-white btn-lg" DataStyle="btn-white btn-lg" DataWidth="100%" DataLiveSearch="True" AppendDataBoundItems="True">
                                            <asp:ListItem Text="Select Job Type" Value="" />
                                        </asp:DropDownList>
                                    </div>
                                    <div class="col-12 col-sm-6 col-md-6 col-lg-3 mb-4 mb-lg-0">
                                        <asp:Button ID="btnSearch" runat="server" Text="Search Job" CssClass="btn btn-primary btn-lg btn-block text-white btn-search" OnClick="btnSearch_Click" />
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-md-12 popular-keywords">
                                        <h3>Trending Keywords:</h3>
                                        <ul class="keywords list-unstyled m-0 p-0">
                                            <li><a href="#" class="">UI Designer</a></li>
                                            <li><a href="#" class="">Python</a></li>
                                            <li><a href="#" class="">Developer</a></li>
                                        </ul>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>

                <a href="#next" class="scroll-button smoothscroll">
                    <span class=" icon-keyboard_arrow_down"></span>
                </a>
            </section>
            
            <!-- JOBBOARD SITE STATS SECTION -->
            <section class="py-5 bg-image overlay-primary fixed overlay" id="next" style="background-image: url('images/hero_1.jpg');">
                <div class="container">
                    <div class="row mb-5 justify-content-center">
                        <div class="col-md-7 text-center">
                            <h2 class="section-title mb-2 text-white">JobBoard Site Stats</h2>
                            <p class="lead text-white">Join thousands of candidates and employers worldwide.</p>
                        </div>
                    </div>
                    <div class="row pb-0 block__19738 section-counter">

                        <div class="col-6 col-md-6 col-lg-3 mb-5 mb-lg-0">
                            <div class="d-flex align-items-center justify-content-center mb-2">
                                <strong class="number" data-number="1930">0</strong>
                            </div>
                            <span class="caption">Candidates</span>
                        </div>

                        <div class="col-6 col-md-6 col-lg-3 mb-5 mb-lg-0">
                            <div class="d-flex align-items-center justify-content-center mb-2">
                                <strong class="number" data-number="54">0</strong>
                            </div>
                            <span class="caption">Jobs Posted</span>
                        </div>

                        <div class="col-6 col-md-6 col-lg-3 mb-5 mb-lg-0">
                            <div class="d-flex align-items-center justify-content-center mb-2">
                                <strong class="number" data-number="120">0</strong>
                            </div>
                            <span class="caption">Jobs Filled</span>
                        </div>

                        <div class="col-6 col-md-6 col-lg-3 mb-5 mb-lg-0">
                            <div class="d-flex align-items-center justify-content-center mb-2">
                                <strong class="number" data-number="550">0</strong>
                            </div>
                            <span class="caption">Companies</span>
                        </div>

                    </div>
                </div>
            </section>

            <!-- JOB LISTINGS SECTION -->
            <section class="site-section">
                <div class="container">

                    <div class="row mb-5 justify-content-center">
                        <div class="col-md-7 text-center">
                            <h2 class="section-title mb-2">Available Jobs</h2>
                        </div>
                    </div>
                    
                    <ul class="job-listings mb-5">
                        <asp:Repeater ID="rptJobListings" runat="server">
                            <ItemTemplate>
                                <li class="job-listing d-block d-sm-flex pb-3 pb-sm-0 align-items-center">
                                    <a href="JobSingle.aspx?JobID=<%# Eval("JobID") %>" class="job-listing-link"></a>
                                    <!-- Company Logo Removed -->

                                    <div class="job-listing-about d-sm-flex custom-width w-100 justify-content-between mx-4">
                                        <div class="job-listing-position custom-width w-50 mb-3 mb-sm-0">
                                            <h2><%# Eval("Title") %></h2>
                                            <strong><%# Eval("CompanyName") %></strong>
                                        </div>
                                        <div class="job-listing-location mb-3 mb-sm-0 custom-width w-25">
                                            <span class="icon-room"></span> <%# Eval("Location") %>
                                        </div>
                                        <div class="job-listing-meta">
                                            <span class="badge badge-<%# Eval("JobTypeBadgeClass") %>"><%# Eval("JobType") %></span>
                                        </div>
                                    </div>
                                </li>
                            </ItemTemplate>
                        </asp:Repeater>
                    </ul>

                    <!-- Pagination (Static Example) -->
                    <div class="row pagination-wrap">
                        <div class="col-md-6 text-center text-md-left mb-4 mb-md-0">
                            <span>Showing 1-7 Of 43,167 Jobs</span>
                        </div>
                        <div class="col-md-6 text-center text-md-right">
                            <div class="custom-pagination ml-auto">
                                <a href="#" class="prev">Prev</a>
                                <div class="d-inline-block">
                                    <a href="#" class="active">1</a>
                                    <a href="#">2</a>
                                    <a href="#">3</a>
                                    <a href="#">4</a>
                                </div>
                                <a href="#" class="next">Next</a>
                            </div>
                        </div>
                    </div>

                </div>
            </section>

            <!-- CALL TO ACTION SECTION -->

            <!-- COMPANY WE'VE HELPED SECTION -->
            <section class="site-section py-4">
                <div class="container">
            
                    <div class="row align-items-center">
                        <div class="col-12 text-center mt-4 mb-5">
                            <div class="row justify-content-center">
                                <div class="col-md-7">
                                    <h2 class="section-title mb-2">Companies We've Helped</h2>
                                    <p class="lead">Trusted by top companies around the globe to find the best talent.</p>
                                </div>
                            </div>
                            
                        </div>
                        <div class="col-6 col-lg-3 col-md-6 text-center">
                            <img src="images/logo_mailchimp.svg" alt="Mailchimp" class="img-fluid logo-1" />
                        </div>
                        <div class="col-6 col-lg-3 col-md-6 text-center">
                            <img src="images/logo_paypal.svg" alt="PayPal" class="img-fluid logo-2" />
                        </div>
                        <div class="col-6 col-lg-3 col-md-6 text-center">
                            <img src="images/logo_stripe.svg" alt="Stripe" class="img-fluid logo-3" />
                        </div>
                        <div class="col-6 col-lg-3 col-md-6 text-center">
                            <img src="images/logo_visa.svg" alt="Visa" class="img-fluid logo-4" />
                        </div>

                        <div class="col-6 col-lg-3 col-md-6 text-center">
                            <img src="images/logo_apple.svg" alt="Apple" class="img-fluid logo-5" />
                        </div>
                        <div class="col-6 col-lg-3 col-md-6 text-center">
                            <img src="images/logo_tinder.svg" alt="Tinder" class="img-fluid logo-6" />
                        </div>
                        <div class="col-6 col-lg-3 col-md-6 text-center">
                            <img src="images/logo_sony.svg" alt="Sony" class="img-fluid logo-7" />
                        </div>
                        <div class="col-6 col-lg-3 col-md-6 text-center">
                            <img src="images/logo_airbnb.svg" alt="Airbnb" class="img-fluid logo-8" />
                        </div>
                    </div>
                </div>
            </section>


            <!-- TESTIMONIALS SECTION -->
            <section class="bg-light pt-5 testimony-full">
                <div class="owl-carousel single-carousel">

                    <div class="container">
                        <div class="row">
                            <div class="col-lg-6 align-self-center text-center text-lg-left">
                                <blockquote>
                                    <p>&ldquo;Soluta quasi cum delectus eum facilis recusandae nesciunt molestias accusantium libero dolores repellat id in dolorem laborum ad modi qui at quas dolorum voluptatem voluptatum repudiandae.&rdquo;</p>
                                    <p><cite>&mdash; Corey Woods, @Dribbble</cite></p>
                                </blockquote>
                            </div>
                            <div class="col-lg-6 align-self-end text-center text-lg-right">
                                <img src="images/person_transparent_2.png" alt="Person" class="img-fluid mb-0" />
                            </div>
                        </div>
                    </div>

                    <div class="container">
                        <div class="row">
                            <div class="col-lg-6 align-self-center text-center text-lg-left">
                                <blockquote>
                                    <p>&ldquo;Soluta quasi cum delectus eum facilis recusandae nesciunt molestias accusantium libero dolores repellat id in dolorem laborum ad modi qui at quas dolorum voluptatem voluptatum repudiandae.&rdquo;</p>
                                    <p><cite>&mdash; Chris Peters, @Google</cite></p>
                                </blockquote>
                            </div>
                            <div class="col-lg-6 align-self-end text-center text-lg-right">
                                <img src="images/person_transparent.png" alt="Person" class="img-fluid mb-0" />
                            </div>
                        </div>
                    </div>

                </div>
            </section>

            <!-- MOBILE APPS SECTION -->
            <section class="pt-5 bg-image overlay-primary fixed overlay" style="background-image: url('images/hero_1.jpg');">
                <div class="container">
                    <div class="row">
                        <div class="col-md-6 align-self-center text-center text-md-left mb-5 mb-md-0">
                            <h2 class="text-white">Get The Mobile Apps</h2>
                            <p class="mb-5 lead text-white">Manage your job search on the go with our mobile app.</p>
                            <p class="mb-0">
                                <a href="#" class="btn btn-dark btn-md px-4 border-width-2">
                                    <span class="icon-apple mr-3"></span>App Store
                                </a>
                                <a href="#" class="btn btn-dark btn-md px-4 border-width-2">
                                    <span class="icon-android mr-3"></span>Play Store
                                </a>
                            </p>
                        </div>
                        <div class="col-md-6 ml-auto align-self-end">
                            <img src="images/apps.png" alt="Mobile Apps" class="img-fluid" />
                        </div>
                    </div>
                </div>
            </section>
            
            <!-- FOOTER -->
            <footer class="site-footer">

                <a href="#top" class="smoothscroll scroll-top">
                    <span class="icon-keyboard_arrow_up"></span>
                </a>

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
                                <!-- Link back to Colorlib can't be removed. Template is licensed under CC BY 3.0. -->
                                Copyright &copy;<script>document.write(new Date().getFullYear());</script> All rights reserved | This template is made with <i class="icon-heart text-danger" aria-hidden="true"></i> by Pallavi & Ayushi </a>
                                <!-- Link back to Colorlib can't be removed. Template is licensed under CC BY 3.0. -->
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

    </form>
</body>
</html>
