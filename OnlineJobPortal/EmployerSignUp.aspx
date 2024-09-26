
    <%@ Page Language="C#" AutoEventWireup="true" CodeBehind="JobSeekerSignUp.aspx.cs" Inherits="OnlineJobPortal.JobSeekerSignUp" %>

    <!DOCTYPE html>
    <html lang="en">
    <head>
        <title>JobBoard &mdash; Sign Up as Job Seeker</title>
        <meta charset="utf-8">
        <meta name="viewport" content="width=device-width, initial-scale=1, shrink-to-fit=no">
    
        <link rel="stylesheet" href="css/custom-bs.css">
        <link rel="stylesheet" href="css/jquery.fancybox.min.css">
        <link rel="stylesheet" href="css/bootstrap-select.min.css">
        <link rel="stylesheet" href="fonts/icomoon/style.css">
        <link rel="stylesheet" href="fonts/line-icons/style.css">
        <link rel="stylesheet" href="css/owl.carousel.min.css">
        <link rel="stylesheet" href="css/animate.min.css">
        <link rel="stylesheet" href="css/quill.snow.css">
        <link rel="stylesheet" href="css/style.css">    
    </head>
    <body id="top">

    <div id="overlayer"></div>
    <div class="loader">
        <div class="spinner-border text-primary" role="status">
            <span class="sr-only">Loading...</span>
        </div>
    </div>

    <div class="site-wrap">
        <header class="site-navbar mt-3">
            <!-- Your navbar code here -->
        </header>

        <section class="section-hero overlay inner-page bg-image" style="background-image: url('images/hero_1.jpg');" id="home-section">
            <div class="container">
                <div class="row">
                    <div class="col-md-7">
                        <h1 class="text-white font-weight-bold">Sign Up as Job Seeker</h1>
                        <div class="custom-breadcrumbs">
                            <a href="#">Home</a> <span class="mx-2 slash">/</span>
                            <span class="text-white"><strong>Sign Up</strong></span>
                        </div>
                    </div>
                </div>
            </div>
        </section>

        <section class="site-section">
            <div class="container">
                <div class="row">
                    <div class="col-lg-6 mb-5">
                        <h2 class="mb-4">Sign Up to JobBoard</h2>
                        <form runat="server" class="p-4 border rounded">

                            <div class="row form-group">
                                <div class="col-md-12 mb-3 mb-md-0">
                                    <label class="text-black" for="FullName">Full Name</label>
                                    <asp:TextBox ID="FullNameInput" runat="server" CssClass="form-control" placeholder="Full Name" required></asp:TextBox>
                                </div>
                            </div>

                            <div class="row form-group">
                                <div class="col-md-12 mb-3 mb-md-0">
                                    <label class="text-black" for="Email">Email</label>
                                    <asp:TextBox ID="EmailInput" runat="server" CssClass="form-control" placeholder="Email address" required></asp:TextBox>
                                </div>
                            </div>

                            <div class="row form-group">
                                <div class="col-md-12 mb-3 mb-md-0">
                                    <label class="text-black" for="Password">Password</label>
                                    <asp:TextBox ID="PasswordInput" runat="server" CssClass="form-control" TextMode="Password" placeholder="Password" required></asp:TextBox>
                                </div>
                            </div>

                            <div class="row form-group mb-4">
                                <div class="col-md-12 mb-3 mb-md-0">
                                    <label class="text-black" for="RetypePassword">Re-Type Password</label>
                                    <asp:TextBox ID="RetypePasswordInput" runat="server" CssClass="form-control" TextMode="Password" placeholder="Re-type Password" required></asp:TextBox>
                                </div>
                            </div>

                            <div class="row form-group">
                                <div class="col-md-12 mb-3 mb-md-0">
                                    <label class="text-black" for="PhoneNumber">Phone Number</label>
                                    <asp:TextBox ID="PhoneNumberInput" runat="server" CssClass="form-control" placeholder="Phone Number" required></asp:TextBox>
                                </div>
                            </div>

                            <div class="row form-group">
                                <div class="col-md-12 mb-3 mb-md-0">
                                    <label class="text-black" for="Resume">Upload Resume</label>
                                    <asp:FileUpload ID="ResumeUpload" runat="server" CssClass="form-control" />
                                </div>
                            </div>

                            <div class="row form-group">
                                <div class="col-md-12">
                                    <asp:Button ID="BtnSignUp" runat="server" Text="Sign Up" CssClass="btn px-4 btn-primary text-white" OnClick="BtnSignUp_Click" />
                                </div>
                            </div>

                        </form>
                    </div>
                </div>
            </div>
        </section>

        <footer class="site-footer">
            <!-- Your footer code here -->
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
    <script src="js/quill.min.js"></script>
    <script src="js/bootstrap-select.min.js"></script>
    <script src="js/custom.js"></script>

    </body>
    </html>
