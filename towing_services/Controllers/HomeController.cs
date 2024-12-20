using Dal;
using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using MimeKit;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using towing_services.Hubs;
using towing_services.Models;


namespace towing_services.Controllers
{
    public class HomeController : Controller
    {
        public readonly Towing_Collection db;
        private readonly UserManager<Driver> _userManager;
        private readonly SignInManager<Driver> _signInManager;

        private readonly UserManager<Admin> _adminUserManager;

        private readonly SignInManager<Admin> _adminSignInManager;
        private readonly IEmailSender _emailSender;
        private readonly IHubContext<NotificationHub> _hubContext;
        private readonly IMemoryCache _cache;  // تعريف التخزين المؤقت

        private const int MaxOtpAttempts = 4;
        private const int OtpExpiryMinutes = 10;
        private const int OtpCooldownMinutes = 10;

        public HomeController(Towing_Collection info, IEmailSender emailSender, UserManager<Driver> userManager, SignInManager<Driver> signInManager, IHubContext<NotificationHub> hubContext, UserManager<Admin> adminUserManager,
        SignInManager<Admin> adminSignInManager, IMemoryCache cache)
        {
            this.db = info;
            _emailSender = emailSender;
            _userManager = userManager;
            _signInManager = signInManager;
            _adminUserManager = adminUserManager;
            _adminSignInManager = adminSignInManager;
            _hubContext = hubContext;
            _cache = cache;


        }
        public IActionResult Noound()
        {
            return View();
        }
        public IActionResult Home()
        {
            var list = db.Service_table.Select(x => new Info_services
            {
                ServiceId = x.ServiceId,

                Name = x.Name,
                Description = x.Description,

                BasePrice = x.BasePrice,


            }).ToList();
            return View(list);
        }

        [HttpGet]
        public IActionResult Create_services()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create_services(Service a)
        {
            if (ModelState.IsValid)
            {


                db.Service_table.Add(a);
                db.SaveChanges();
                return RedirectToAction("Page_serv");


            }

            return View();
        }

        [HttpGet]
        public ActionResult Page_serv()
        {

            var list = db.Service_table.Select(x => new Info_services
            {
                ServiceId = x.ServiceId,

                Name = x.Name,
                Description = x.Description,

                BasePrice = x.BasePrice,


            }).ToList();
            return View(list);


        }



        private string GenerateTrackingNumber()
        {
            var random = new Random();
            return $"TRK-{random.Next(100000, 999999)}";
        }
        [HttpGet]
        public IActionResult Order_Req()
        {

            return View();
        }



        [HttpPost]
        public async Task<IActionResult> Order_Req(Customer customer, Order order)
        {
            if (ModelState.IsValid)
            {

                order.Status = "New Request";


                db.Customer_table.Add(customer);
                await db.SaveChangesAsync();

                HttpContext.Session.SetInt32("CustomerId", customer.CustomerId);

                if (order.Distance == 0 || order.TotalCost == 0 || order.DID == 0)
                {
                    order.DID = (decimal)(order.Distance / 2) * 10;

                    order.TotalCost = (decimal)(order.Distance * 10) + order.DID;

                }

                order.TrackingNumber = GenerateTrackingNumber();

                db.Order_table.Add(order);
                await db.SaveChangesAsync();

                var customerOrder = new Customer_order
                {
                    CustomerId = customer.CustomerId,
                    OrderId = order.OrderId
                };
                db.Customer_order_table.Add(customerOrder);
                await db.SaveChangesAsync();





                ViewBag.NewOrder = true;

                return RedirectToAction("TrackOrder", new { id = order.OrderId });
            }



            ViewBag.Order = order;
            return View();
        }
        private int? GetCurrentCustomerId()
        {
            return HttpContext.Session.GetInt32("CustomerId");
        }

        public IActionResult TrackOrder(int id)
        {
            var order = db.Order_table
          .Include(o => o.Customer_orders)
          .ThenInclude(co => co.Customers)
          .FirstOrDefault(o => o.OrderId == id);

            if (order == null)
            {
                return RedirectToAction("Order_Req");
            }
            var currentCustomerId = GetCurrentCustomerId();
            var orderCustomerId = order.Customer_orders.FirstOrDefault()?.CustomerId;

            if (orderCustomerId != currentCustomerId)
            {
                return RedirectToAction("Order_Req");
            }

            var customerName = order.Customer_orders.FirstOrDefault()?.Customers?.Name ?? "Unknown";

            var viewModel = new OrderTrackingViewModel
            {
                Order = order,
                CustomerName = customerName,


            };
            return View(viewModel);
        }



        [HttpGet]
        public IActionResult Show_services()
        {

            var list = db.Service_table.Select(x => new Info_services
            {
                ServiceId = x.ServiceId,

                Name = x.Name,
                Description = x.Description,

                BasePrice = x.BasePrice,


            }).ToList();
            return View(list);
        }

        [HttpGet]
        public IActionResult Contact()
        {

            return View();
        }

        [HttpPost]
        public IActionResult Contact(Customer customer)
        {
            if (ModelState.IsValid)
            {
                db.Customer_table.Add(customer);
                db.SaveChanges();

                ViewData["SuccessMessage"] = "Thank you for contacting us. We will get in touch with you as soon as possible.!";
                ModelState.Clear();
                return View();
            }

            return View();
        }




        [HttpGet]
        public IActionResult AssignDriver(int orderId)
        {


            var user = _adminUserManager.GetUserAsync(User).Result;

            if (user == null || !user.IsAdmin)
            {
                return RedirectToAction("Home", "Home");
            }
            var order = db.Order_table.FirstOrDefault(o => o.OrderId == orderId);

            if (order == null)
            {
                ViewData["ErrorMessage"] = "Order not found.";
                return RedirectToAction("Index");
            }

            // عرض جميع السائقين المتوافقين مع نوع المركبة
            var availableDrivers = db.Driver_table
                                      .Where(d => d.VehicleType == order.VehicleType)
                                      .Select(d => new DriverViewModel
                                      {
                                          DriverId = d.Id,
                                          Name = d.UserName,
                                          Email = d.Email,
                                          phone = d.PhoneNumber,
                                          VehicleType = d.VehicleType,
                                          isAvailable = d.IsAvailable,
                                          ProfilePicture = d.ProfilePicture,
                                          LicensePicture = d.LicensePicture
                                      })
                                      .ToList();

            ViewBag.OrderId = orderId;

            return View(availableDrivers);
        }
        [HttpPost]
        public async Task<IActionResult> AssignDriver(int orderId, string driverId)
        {
            if (string.IsNullOrEmpty(driverId) || !int.TryParse(driverId, out int driverIdInt))
            {
                return BadRequest("Invalid driver ID.");
            }

            var order = db.Order_table.FirstOrDefault(o => o.OrderId == orderId);
            if (order == null)
            {
                return NotFound("Order not found.");
            }

            var driver = await _userManager.FindByIdAsync(driverId);
            if (driver == null)
            {
                return NotFound("Driver not found.");
            }

            order.Status = "In Progress";
            db.SaveChanges();

            var orderDriver = new Order_Driver
            {
                OrderId = orderId,
                DriverId = driverIdInt
            };

            db.Order_Driver_table.Add(orderDriver);
            db.SaveChanges();


            return RedirectToAction("Details", new { orderId = order.OrderId });
        }



        public IActionResult Details(int orderId)
        {


            var user = _adminUserManager.GetUserAsync(User).Result;

            if (user == null || !user.IsAdmin)
            {
                return RedirectToAction("Home", "Home");
            }

            var order = db.Order_table
                          .Include(o => o.Customer_orders)
                          .ThenInclude(co => co.Customers)
                          .Include(o => o.Order_Drivers)
                          .ThenInclude(od => od.Drivers)
                          .FirstOrDefault(o => o.OrderId == orderId);

            if (order == null)
            {
                return NotFound("Order not found.");
            }

            return View(order);
        }



        //.................................................Register...................................................
        private string GenerateOTP()
        {
            return new Random().Next(100000, 999999).ToString();
        }

        private async Task SendEmail(string email, string otpCode)
        {
            if (string.IsNullOrEmpty(email))
            {
                throw new ArgumentException("Email cannot be null or empty", nameof(email));
            }

            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("Admin", "sjerjeous@gmail.com"));
            message.To.Add(new MailboxAddress(email, email));
            message.Subject = "OTP Verification Code";
            message.Body = new TextPart("plain") { Text = $"Your OTP Code is: {otpCode}" };

            using var client = new SmtpClient();
            await client.ConnectAsync("smtp.gmail.com", 587, false);
            await client.AuthenticateAsync("sjerjeous@gmail.com", "whwf zglo bapr kwfw");
            await client.SendAsync(message);
            await client.DisconnectAsync(true);
        }

        private string GenerateTrackingDriver()
        {
            var random = new Random();
            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var trackingDriver = new StringBuilder();

            for (int i = 0; i < 7; i++)
            {
                trackingDriver.Append(chars[random.Next(chars.Length)]);
            }

            return trackingDriver.ToString();
        }

        [HttpGet]

        public IActionResult Register()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Register(RegisterDriverViewModel model, IFormFile profilePicture, IFormFile licensePicture)
        {
            if (!IsImageValid(profilePicture))
            {
                ModelState.AddModelError("ProfilePicture", "Only .jpg, .jpeg, and .png files are allowed for the profile picture.");
            }

            if (!IsImageValid(licensePicture))
            {
                ModelState.AddModelError("LicensePicture", "Only .jpg, .jpeg, and .png files are allowed for the license picture.");
            }

            if (!ModelState.IsValid)
                return View(model);

            var existingDriver = await db.Driver_table.FirstOrDefaultAsync(d => d.Email == model.Email || d.PhoneNumber == model.Phone);

            if (existingDriver != null)
            {
                ModelState.AddModelError("Email", "This email is already in use.");
                return View(model);
            }

            string trackingDriver = GenerateTrackingDriver();
            HttpContext.Session.SetString("TrackingDriver", trackingDriver);

            HttpContext.Session.SetString("DriverName", model.Name);
            HttpContext.Session.SetString("DriverEmail", model.Email);
            HttpContext.Session.SetString("DriverPhone", model.Phone);
            HttpContext.Session.SetString("VehicleType", model.VehicleType);
            HttpContext.Session.SetString("ProfilePicture", Convert.ToBase64String(await ConvertFileToByteArray(profilePicture)));
            HttpContext.Session.SetString("LicensePicture", Convert.ToBase64String(await ConvertFileToByteArray(licensePicture)));

            HttpContext.Session.SetString("Password", model.Password);

            //string otpCode = GenerateOTP();
            //DateTime otpExpiry = DateTime.Now.AddSeconds(30);
            //await SendEmail(model.Email, otpCode);
            //HttpContext.Session.SetString("OTPCode", otpCode);
            //HttpContext.Session.SetString("OtpExpiry", otpExpiry.ToString("o"));
            //HttpContext.Session.SetInt32("OtpAttempts", 0);

            return RedirectToAction("VerifyOtp");
        }




        [HttpGet]
        public IActionResult VerifyOtp()
        {
            string email = HttpContext.Session.GetString("DriverEmail");
            if (string.IsNullOrEmpty(email)) return RedirectToAction("Register");

            int otpAttempts = HttpContext.Session.GetInt32("OtpAttempts") ?? 0;
            string otpExpiryString = HttpContext.Session.GetString("OtpExpiry");
            DateTime? otpExpiry = string.IsNullOrEmpty(otpExpiryString) ? null : DateTime.Parse(otpExpiryString);

            if (otpExpiry == null || DateTime.Now > otpExpiry)
            {
                string otpCode = GenerateOTP();
                otpExpiry = DateTime.Now.AddSeconds(30);
                HttpContext.Session.SetString("OTPCode", otpCode);
                HttpContext.Session.SetString("OtpExpiry", otpExpiry.Value.ToString("o"));
                HttpContext.Session.SetInt32("OtpAttempts", 0);

                _ = SendEmail(email, otpCode);
            }

            ViewBag.RemainingTime = otpExpiry != null ? Math.Max((otpExpiry.Value - DateTime.Now).TotalSeconds, 0) : 30;
            ViewBag.RemainingAttempts = Math.Max(4 - otpAttempts, 0);

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> VerifyOtp(string enteredOtp)
        {
            string email = HttpContext.Session.GetString("DriverEmail");
            if (string.IsNullOrEmpty(email)) return RedirectToAction("Register");

            string expectedOtp = HttpContext.Session.GetString("OTPCode");
            string otpExpiryString = HttpContext.Session.GetString("OtpExpiry");
            DateTime? otpExpiry = string.IsNullOrEmpty(otpExpiryString) ? null : DateTime.Parse(otpExpiryString);

            if (otpExpiry == null || DateTime.Now > otpExpiry)
            {
                TempData["ErrorMessage"] = "The OTP has expired. Please request a new one.";
                return RedirectToAction("VerifyOtp");
            }

            if (enteredOtp == expectedOtp)
            {
                string password = HttpContext.Session.GetString("Password");

                byte[] profilePictureBytes = Convert.FromBase64String(HttpContext.Session.GetString("ProfilePicture"));
                byte[] licensePictureBytes = Convert.FromBase64String(HttpContext.Session.GetString("LicensePicture"));

                //string profilePicturePath = await SaveFileToDisk(profilePictureBytes, "drivers");
                //string licensePicturePath = await SaveFileToDisk(licensePictureBytes, "licenses");
                string profilePictureExtension = ".jpg";
                string licensePictureExtension = ".png";

                string profilePicturePath = await SaveFileToDisk(profilePictureBytes, "drivers", profilePictureExtension);

                string licensePicturePath = await SaveFileToDisk(licensePictureBytes, "licenses", licensePictureExtension);

                var driver = new Driver
                {
                    UserName = HttpContext.Session.GetString("DriverName"),
                    Email = HttpContext.Session.GetString("DriverEmail"),
                    PhoneNumber = HttpContext.Session.GetString("DriverPhone"),
                    VehicleType = HttpContext.Session.GetString("VehicleType"),
                    ProfilePicture = profilePicturePath,
                    LicensePicture = licensePicturePath,
                    IsAvailable = false,
                    IsNewDriver = true,
                    TrackingDriver = HttpContext.Session.GetString("TrackingDriver")
                };

                var passwordHasher = new PasswordHasher<Driver>();
                driver.PasswordHash = passwordHasher.HashPassword(driver, password);

                var result = await _userManager.CreateAsync(driver, password);

                if (result.Succeeded)
                {
                    var cookieOptions = new CookieOptions
                    {
                        HttpOnly = true,
                        Secure = true,
                        Expires = DateTime.Now.AddMinutes(10)
                    };
                    Response.Cookies.Append("DriverAuth", $"{driver.Id}|verified", cookieOptions);

                    HttpContext.Session.Clear();

                    return RedirectToAction("PendingApproval", new { id = driver.Id });
                }

                TempData["ErrorMessage"] = "Error occurred while creating the driver.";
                return RedirectToAction("Register");
            }

            int otpAttempts = HttpContext.Session.GetInt32("OtpAttempts") ?? 0;
            otpAttempts++;
            HttpContext.Session.SetInt32("OtpAttempts", otpAttempts);

            if (otpAttempts >= 4)
            {
                HttpContext.Session.SetString("LastOtpAttempt", DateTime.Now.ToString("o"));
            }

            TempData["ErrorMessage"] = "Invalid OTP. Please try again.";
            return RedirectToAction("VerifyOtp");
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResendOtp()
        {
            string email = HttpContext.Session.GetString("DriverEmail");
            if (string.IsNullOrEmpty(email)) return RedirectToAction("Register");

            int otpAttempts = HttpContext.Session.GetInt32("OtpAttempts") ?? 0;

            string lastAttemptString = HttpContext.Session.GetString("LastOtpAttempt");

            DateTime? lastAttemptTime = string.IsNullOrEmpty(lastAttemptString) ? null : DateTime.Parse(lastAttemptString);

            if (otpAttempts >= 4)
            {
                if (lastAttemptTime != null && (DateTime.Now - lastAttemptTime.Value).TotalMinutes < 1)
                {
                    TempData["ErrorMessage"] = "You have exceeded the maximum attempts. Please wait for a minute before trying again.";
                    return RedirectToAction("VerifyOtp");
                }

                if (lastAttemptTime != null && (DateTime.Now - lastAttemptTime.Value).TotalMinutes >= 1)
                {
                    otpAttempts = 0;
                    HttpContext.Session.SetInt32("OtpAttempts", otpAttempts);
                }
            }

            string otpCode = GenerateOTP();
            await SendEmail(email, otpCode);

            HttpContext.Session.SetString("OTPCode", otpCode);
            HttpContext.Session.SetString("OtpExpiry", DateTime.Now.AddSeconds(30).ToString("o"));
            HttpContext.Session.SetInt32("OtpAttempts", otpAttempts + 1);

            HttpContext.Session.SetString("LastOtpAttempt", DateTime.Now.ToString("o"));

            TempData["SuccessMessage"] = "A new OTP has been sent to your email.";
            return RedirectToAction("VerifyOtp");
        }

        private async Task<string> SaveFileToDisk(byte[] fileData, string folderName, string fileExtension)
        {
            var allowedExtensions = new List<string> { ".jpg", ".jpeg", ".png" };
            if (!allowedExtensions.Contains(fileExtension.ToLower()))
            {
                throw new InvalidOperationException("This file type is not allowed.");
            }

            string directoryPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", folderName);

            if (!Directory.Exists(directoryPath)) Directory.CreateDirectory(directoryPath);

            string filePath = Path.Combine(directoryPath, $"{Guid.NewGuid()}{fileExtension}");

            await System.IO.File.WriteAllBytesAsync(filePath, fileData);

            return Path.Combine("uploads", folderName, Path.GetFileName(filePath));
        }


        private async Task<byte[]> ConvertFileToByteArray(IFormFile file)
        {
            using (var memoryStream = new MemoryStream())
            {
                await file.CopyToAsync(memoryStream);
                return memoryStream.ToArray();
            }
        }

        private bool IsImageValid(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return false;

            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png" };
            return allowedExtensions.Contains(Path.GetExtension(file.FileName).ToLower());
        }

        public IActionResult PendingApproval(int id)
        {
            // قراءة الكوكي
            var driverAuth = Request.Cookies["DriverAuth"];
            if (string.IsNullOrEmpty(driverAuth))
            {
                return RedirectToAction("Register");
            }

            // التحقق من الكوكي
            var cookieParts = driverAuth.Split('|');
            if (cookieParts.Length != 2 || cookieParts[1] != "verified")
            {
                return RedirectToAction("Register");
            }

            int driverId = int.Parse(cookieParts[0]);
            if (driverId != id)
            {
                return RedirectToAction("Register");
            }

            var driver = db.Driver_table.FirstOrDefault(d => d.Id == id);
            if (driver == null)
            {
                return NotFound();
            }
            Response.Cookies.Delete("DriverAuth");

            return View(driver);
        }

        ////////////////////////////////dash admin driver////////////////////////////////


        //*********************login driver and logout /////////////////////////////////


        //[HttpGet]
        //public IActionResult Login()
        //{
        //    var model = new LoginDriverViewModel();

        //    if (User.Identity.IsAuthenticated)
        //    {
        //        model.RememberMe = true;  // تعيين RememberMe إذا كان المستخدم مسجل دخول
        //    }

        //    return View(model);
        //}
        //[HttpPost]
        //public async Task<IActionResult> Login(LoginDriverViewModel model, string returnUrl = null)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        // البحث عن السائق باستخدام البريد الإلكتروني
        //        var driver = await _userManager.FindByEmailAsync(model.Email);

        //        if (driver == null)
        //        {
        //            // إذا لم يتم العثور على السائق
        //            ModelState.AddModelError("", "This email is not registered in our system.");
        //        }
        //        else if (!await _userManager.CheckPasswordAsync(driver, model.Password))
        //        {
        //            // إذا كانت كلمة المرور غير صحيحة
        //            ModelState.AddModelError("", "The email or password is incorrect.");
        //        }
        //        else
        //        {
        //            // تسجيل الدخول باستخدام SignInManager
        //            await _signInManager.SignInAsync(driver, model.RememberMe);

        //            // حفظ بيانات السائق في الجلسة إذا كنت بحاجة
        //            HttpContext.Session.SetString("DriverId", driver.Id.ToString());
        //            HttpContext.Session.SetString("DriverName", driver.UserName); // احفظ UserName (البريد الإلكتروني)

        //            return RedirectToAction("Dashboard", "Home");
        //        }
        //    }

        //    return View(model);
        //}
        /*****************TEST****************/
        [HttpGet]
        public IActionResult Login()
        {
            var model = new LoginDriverViewModel();

            if (User.Identity.IsAuthenticated)
            {
                model.RememberMe = true;
            }

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginDriverViewModel model, string returnUrl = null)
        {
            if (ModelState.IsValid)
            {
                var driver = await _userManager.FindByEmailAsync(model.Email);
                if (driver != null)
                {
                    if (await _userManager.CheckPasswordAsync(driver, model.Password))
                    {
                        if (!driver.IsApproved)
                        {
                            ModelState.AddModelError("", "Your account has not been approved by the admin yet.");
                            return View(model);
                        }

                        if (driver.IsDriver)
                        {
                            await _signInManager.SignInAsync(driver, model.RememberMe);
                            return RedirectToAction("Dashboard", "Home");
                        }
                    }
                }
                else
                {
                    var admin = await _adminUserManager.FindByEmailAsync(model.Email);
                    if (admin != null && await _adminUserManager.CheckPasswordAsync(admin, model.Password))
                    {
                        if (admin.IsAdmin)
                        {
                            await _adminSignInManager.SignInAsync(admin, model.RememberMe);
                            return RedirectToAction("DashboardAdmin", "Home");
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("", "Invalid email or password.");
                    }
                }
            }

            return View(model);
        }




        /*******************************/
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login", "Home");
        }
        public IActionResult ForgotPassword()
        {
            return View(new ForgotPasswordViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var driver = await _userManager.FindByEmailAsync(model.Email);

                if (driver == null)
                {
                    ModelState.AddModelError("", "This email is not registered in our system.");
                    return View(model);
                }

                var token = await _userManager.GeneratePasswordResetTokenAsync(driver);
                var resetLink = Url.Action("ResetPassword", "Home", new { token, email = model.Email }, Request.Scheme);

                await _emailSender.SendEmailAsync(model.Email, "Password Reset",
                    $"Please reset your password by clicking <a href='{resetLink}'>here</a>.");

                return RedirectToAction("PasswordResetSent");
            }

            return View(model);
        }


        public async Task<IActionResult> ResetPassword(string email, string token)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(token))
            {
                TempData["ErrorMessage"] = "Oops! It seems like the link has expired or is no longer valid. Please request a new password reset link.";
                return RedirectToAction("Errors", "Home");
            }

            var driver = await _userManager.FindByEmailAsync(email);
            if (driver == null)
            {
                TempData["ErrorMessage"] = "User not found. Please check your email address and try again.";
                return RedirectToAction("Errors", "Home");
            }

            var isTokenValid = await _userManager.VerifyUserTokenAsync(driver, _userManager.Options.Tokens.PasswordResetTokenProvider, "ResetPassword", token);

            if (!isTokenValid)
            {
                TempData["ErrorMessage"] = "Oops! It seems like the link has expired or is no longer valid. Please request a new password reset link.";
                return RedirectToAction("Errors", "Home");
            }

            return View(new ResetPasswordViewModel { Email = email, Token = token });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var driver = await _userManager.FindByEmailAsync(model.Email);

                if (driver != null)
                {
                    var isTokenValid = await _userManager.VerifyUserTokenAsync(driver, _userManager.Options.Tokens.PasswordResetTokenProvider, "ResetPassword", model.Token);

                    if (!isTokenValid)
                    {
                        ModelState.AddModelError("", "The password reset token has expired or is invalid. Please request a new token.");
                        return RedirectToAction("Errors", "Home");
                    }

                    var result = await _userManager.ResetPasswordAsync(driver, model.Token, model.NewPassword);

                    if (result.Succeeded)
                    {
                        var emailSubject = "Password Reset Successful";
                        var emailBody = "Your password has been successfully changed. If you didn't request this change, please contact us immediately.";
                        await _emailSender.SendEmailAsync(driver.Email, emailSubject, emailBody);

                        return RedirectToAction("Login");
                    }

                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Email not found.");
                }
            }

            return View(model);
        }



        public IActionResult Errors()
        {
            var errorMessage = TempData["ErrorMessage"] as string;
            return View("Errors", model: errorMessage);
        }




        public IActionResult PasswordResetSent()
        {
            return View();
        }




        [HttpGet]

        public async Task<IActionResult> Dashboard()
        {

            var driverId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            var driver = await db.Driver_table
                .Include(d => d.Order_Drivers)
                    .ThenInclude(od => od.Orders)
                    .ThenInclude(o => o.Customer_orders)
                    .ThenInclude(co => co.Customers)
                .Where(d => d.Id == driverId)
                .FirstOrDefaultAsync();

            if (driver == null)
            {
                return NotFound("Driver not found.");
            }

            if (!driver.IsDriver)
            {
                return Unauthorized("You are not an approved driver.");
            }
            ViewBag.IsDriver = true;
            ViewBag.IsAdmin = false;
            return View(driver);
        }




        //[HttpPost]
        //public IActionResult SaveETA(int orderId, string eta)
        //{
        //    var order = db.Order_table.FirstOrDefault(o => o.OrderId == orderId);
        //    if (order == null)
        //    {
        //        return NotFound("Order not found.");
        //    }

        //    if (string.IsNullOrEmpty(eta))
        //    {
        //        return BadRequest("ETA cannot be empty.");
        //    }

        //    order.ETA = eta; 
        //    db.SaveChanges(); 

        //    return RedirectToAction("DashboardAdmin"); 
        //}
        [HttpGet]
        public IActionResult IndexPartialView(string status, string trackingNumber)
        {


            //var referer = Request.Headers["Referer"].ToString();
            //if (!referer.Contains("/DashboardAdmin"))
            //{
            //    return RedirectToAction("DashboardAdmin", "Home");
            //}
            var user = _adminUserManager.GetUserAsync(User).Result;

            if (user == null || !user.IsAdmin)
            {
                return RedirectToAction("Home", "Home");
            }
            var ordersQuery = db.Order_table.Include(o => o.Customer_orders)
                                             .ThenInclude(co => co.Customers)
                                             .AsQueryable();

            ViewBag.NewRequestCount = ordersQuery.Count(o => o.Status == "New Request");

            if (string.IsNullOrEmpty(status) || status == "All")
            {

            }
            else
            {
                ordersQuery = ordersQuery.Where(o => o.Status == status);
            }
            if (!string.IsNullOrEmpty(trackingNumber) && trackingNumber != "TRK-")
            {
                ordersQuery = ordersQuery.Where(o => o.TrackingNumber == trackingNumber);
            }


            var orders = ordersQuery.ToList();

            return PartialView("IndexPartialView", orders);
        }

        [HttpPost]
        public IActionResult SaveETA(int orderId, string eta)
        {
            var order = db.Order_table.FirstOrDefault(o => o.OrderId == orderId);
            if (order == null)
            {
                return Json(new { success = false, message = "Order not found." });
            }

            if (string.IsNullOrEmpty(eta))
            {
                return Json(new { success = false, message = "ETA cannot be empty." });
            }

            order.ETA = eta;
            db.SaveChanges();

            return Json(new { success = true, orderId = orderId, eta = eta });
        }




        [HttpGet]

        public async Task<IActionResult> DashboardAdmin(string section = "default")
        {
            if (!string.IsNullOrEmpty(section))
            {
                ViewBag.Section = section;
            }
            var adminUser = await _adminUserManager.GetUserAsync(User);

            if (adminUser == null)
            {
                return RedirectToAction("Home", "Home");
            }

            ViewBag.AdminName = adminUser.UserName;

            ViewBag.IsDriver = false;
            ViewBag.IsAdmin = true;


            return View();
        }


        [HttpGet]
        public IActionResult GetPartialView(string section)
        {

            switch (section)
            {
                case "drivers-management":
                    return RedirectToAction("GetDriver");
                case "order-management":
                    var ordersQuery = db.Order_table.Include(o => o.Customer_orders)
                                                     .ThenInclude(co => co.Customers)
                                                     .AsQueryable();

                    ViewBag.NewRequestCount = ordersQuery.Count(o => o.Status == "New Request");
                    var orders = ordersQuery.ToList();

                    return PartialView("IndexPartialView", orders);
                default:
                    return PartialView("_Profile");
            }


        }
        /****/

        public IActionResult GetDriver()
        {
            var user = _adminUserManager.GetUserAsync(User).Result;

            if (user == null || !user.IsAdmin)
            {
                return RedirectToAction("Home", "Home");
            }

            try
            {
                // جلب البيانات من الكاش أو تحديثها
                var cachedDrivers = _cache.Get<List<DriverViewModel>>("drivers");

                if (cachedDrivers == null)
                {
                    Console.WriteLine("Cache miss - Fetching data from database.");
                    cachedDrivers = db.Driver_table
                                      .Select(d => new DriverViewModel
                                      {
                                          DriverId = d.Id,
                                          Name = d.UserName,
                                          Email = d.Email,
                                          phone = d.PhoneNumber,
                                          VehicleType = d.VehicleType,
                                          TrackingDriver = d.TrackingDriver
                                      }).ToList();

                    _cache.Set("drivers", cachedDrivers, TimeSpan.FromMinutes(5));
                    Console.WriteLine("Data cached successfully for 5 minutes.");
                }
                else
                {
                    Console.WriteLine("Cache hit - Data retrieved from cache.");
                }

                return PartialView("GetDriver", cachedDrivers);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"An error occurred: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }





        public async Task<IActionResult> show_driver()
        {
            var drivers = await _userManager.Users
                                  .OrderByDescending(d => d.IsNewDriver)
                                  .ToListAsync();


            return View(drivers);
        }

        public async Task<IActionResult> ApproveDriver(string id)
        {
            var driver = await _userManager.FindByIdAsync(id);
            if (driver == null)
            {
                return NotFound();
            }

            if (!driver.IsDriver)
            {
                driver.IsDriver = true;
            }

            driver.IsApproved = true;

            var result = await _userManager.UpdateAsync(driver);

            if (!result.Succeeded)
            {
                ModelState.AddModelError(string.Empty, "An error occurred while updating the driver.");
                return View(driver);
            }

            await _emailSender.SendEmailAsync(driver.Email, "Your account has been approved",
                "Congratulations! Your application to become a driver has been approved.");

            return RedirectToAction(nameof(show_driver));
        }



        public async Task<IActionResult> RejectDriver(string id)
        {
            var driver = await _userManager.FindByIdAsync(id);
            if (driver == null)
            {
                return NotFound();
            }

            driver.IsApproved = false;

            var result = await _userManager.UpdateAsync(driver);
            if (!result.Succeeded)
            {
            }

            await _emailSender.SendEmailAsync(driver.Email, "Your account has been rejected",
                "Sorry, your application to become a driver in the system has been rejected. Thank you for your understanding.");

            return RedirectToAction(nameof(show_driver));
        }



        [HttpGet]
        public IActionResult AddDriver()
        {
            return RedirectToAction("Register");  // إعادة التوجيه إلى صفحة التسجيل
        }

































        /***/
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var driver = db.Driver_table.Find(id);

            if (driver == null)
            {
                return NotFound();
            }

            // حذف السائق من قاعدة البيانات
            db.Driver_table.Remove(driver);
            await db.SaveChangesAsync();

            // تحديث الكاش
            var updatedDrivers = db.Driver_table
                                   .Select(d => new DriverViewModel
                                   {
                                       DriverId = d.Id,
                                       Name = d.UserName,
                                       Email = d.Email,
                                       phone = d.PhoneNumber,
                                       VehicleType = d.VehicleType,
                                       TrackingDriver = d.TrackingDriver
                                   }).ToList();

            _cache.Set("drivers", updatedDrivers, TimeSpan.FromMinutes(5));

            // إعادة البيانات بعد الحذف
            return PartialView("GetDriver", updatedDrivers);
        }































        /***********************/



        public async Task<IActionResult> GetCustomerDetails(int orderId)
        {
            var order = await db.Order_table
                .Include(o => o.Customer_orders)
                .ThenInclude(co => co.Customers)
                .FirstOrDefaultAsync(o => o.OrderId == orderId);

            if (order == null || order.Customer_orders.Count == 0)
                return NotFound("Customer details not found.");//404

            var customer = order.Customer_orders.FirstOrDefault()?.Customers;
            return PartialView("GetCustomerDetails", customer);
        }






        [HttpPost]
        public IActionResult UpdateAvailability(bool isAvailable)
        {
            var driverId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var driver = db.Driver_table.FirstOrDefault(d => d.Id == int.Parse(driverId));

            if (driver != null)
            {
                driver.IsAvailable = isAvailable;
                db.SaveChanges();
            }

            return RedirectToAction("Dashboard", "Home");
        }





        /*******************/
        [HttpPost]
        public async Task<IActionResult> MarkOrderOnTheWay(int orderId)
        {
            var orderDriver = await db.Order_Driver_table
                .Include(od => od.Orders)
                .Include(od => od.Drivers)
                .FirstOrDefaultAsync(od => od.OrderId == orderId);

            if (orderDriver == null)
                return NotFound("Order not found.");//404

            orderDriver.Orders.Status = "On the Way";

            orderDriver.Drivers.Status = "Busy";

            db.Update(orderDriver);
            await db.SaveChangesAsync();

            return RedirectToAction("Dashboard");
        }


        [HttpPost]
        public async Task<IActionResult> MarkOrderCompleted(int orderId)
        {
            var orderDriver = await db.Order_Driver_table
                .Include(od => od.Orders)
                .Include(od => od.Drivers)
                .FirstOrDefaultAsync(od => od.OrderId == orderId);

            if (orderDriver == null)
                return NotFound("Order not found.");//404

            orderDriver.Orders.Status = "Completed";

            orderDriver.Drivers.Status = "Available";

            db.Update(orderDriver);
            await db.SaveChangesAsync();

            return RedirectToAction("Dashboard");
        }



        /******************************************/

        //[HttpGet]
        //public async Task<IActionResult> UpdateProfile()
        //{
        //    var driverId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        //    if (string.IsNullOrEmpty(driverId))
        //    {
        //        return Unauthorized();
        //    }

        //    var driver = await db.Driver_table.FindAsync(int.Parse(driverId));
        //    if (driver == null)
        //    {
        //        return NotFound("Driver not found."); // 404
        //    }

        //    // تحويل كائن Driver إلى DriverViewModel
        //    var model = new DriverViewModel
        //    {
        //        Name = driver.UserName,
        //        Email = driver.Email,
        //        phone = driver.PhoneNumber,
        //        VehicleType = driver.VehicleType,
        //        ProfilePicture = driver.ProfilePicture,
        //        LicensePicture = driver.LicensePicture
        //    };

        //    return View(model);
        //}
     

        [HttpGet]
        public async Task<IActionResult> UpdateProfile()
        {
            var driver = await _userManager.GetUserAsync(User); // الحصول على السائق الحالي
            if (driver == null) return NotFound();

            var model = new DriverViewModel
            {
                Name = driver.UserName,
                Email = driver.Email,
                phone = driver.PhoneNumber,
                VehicleType = driver.VehicleType,
                // إذا كانت هناك صورة موجودة، إرجاع الـ Base64 لعرضها في النموذج
                ProfilePicture = driver.ProfilePicture,
                LicensePicture = driver.LicensePicture
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateProfile(DriverViewModel model)
        {
            var request = HttpContext.Request;

            // تأكد من وجود ملفات مرفوعة
            if (request.Form.Files.Count > 0)
            {
                foreach (var file in request.Form.Files)
                {
                    if (file.Length > 0)
                    {
                        // التحقق من اسم الملف ونوعه
                        string fileExtension = Path.GetExtension(file.FileName).ToLower();
                        if (fileExtension != ".jpg" && fileExtension != ".jpeg" && fileExtension != ".png")
                        {
                            Console.WriteLine("Error: Invalid file type.");
                            TempData["ErrorMessage"] = "Invalid file type. Please upload a .jpg, .jpeg, or .png file.";
                            return View(model);  // يجب إظهار رسالة خطأ للمستخدم
                        }

                        // تحديد مجلد التحميل
                        string directoryPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
                        if (!Directory.Exists(directoryPath))
                        {
                            Directory.CreateDirectory(directoryPath);
                        }

                        // تحديد المجلد الفرعي بناءً على الحقل (ProfilePicture أو LicensePicture)
                        string subFolder = string.Empty;

                        if (file.Name == nameof(model.ProfilePicture))  // إذا كانت الصورة هي ProfilePicture
                        {
                            subFolder = "drivers";
                        }
                        else if (file.Name == nameof(model.LicensePicture))  // إذا كانت الصورة هي LicensePicture
                        {
                            subFolder = "licenses";
                        }

                        // تحديد المسار الفرعي بناءً على المجلد
                        string subFolderPath = Path.Combine(directoryPath, subFolder);
                        if (!Directory.Exists(subFolderPath))
                        {
                            Directory.CreateDirectory(subFolderPath);
                        }

                        // إنشاء مسار الملف
                        string filePath = Path.Combine(subFolderPath, $"{Guid.NewGuid()}{fileExtension}");

                        // حفظ الملف على الخادم
                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await file.CopyToAsync(stream);
                        }

                        // عرض رسالة في الكونسول للتحقق
                        Console.WriteLine($"File uploaded: {file.FileName} -> {filePath}");

                        // إذا كانت الصورة هي Profile Picture أو License Picture
                        if (file.Name == nameof(model.ProfilePicture))
                        {
                            model.ProfilePicture = Path.Combine("uploads", "drivers", Path.GetFileName(filePath));  // حفظ مسار الملف في الـ model
                        }
                        else if (file.Name == nameof(model.LicensePicture))
                        {
                            model.LicensePicture = Path.Combine("uploads", "licenses", Path.GetFileName(filePath));  // حفظ مسار الملف في الـ model
                        }
                    }
                }
            }
            else
            {
                // إذا لم يتم رفع أي ملف
                Console.WriteLine("No files uploaded.");
            }

            // تحديث البيانات النصية
            var driver = await _userManager.GetUserAsync(User);
            if (driver == null) return RedirectToAction("Register");

            driver.UserName = model.Name;
            driver.Email = model.Email;
            driver.PhoneNumber = model.phone;
            driver.VehicleType = model.VehicleType;

            // تحديث الصور في قاعدة البيانات
            driver.ProfilePicture = model.ProfilePicture;
            driver.LicensePicture = model.LicensePicture;

            var result = await _userManager.UpdateAsync(driver);

            if (result.Succeeded)
            {
                TempData["SuccessMessage"] = "Your profile has been updated successfully.";
                Console.WriteLine("Profile updated successfully.");
                return RedirectToAction("Dashboard");
            }

            TempData["ErrorMessage"] = "An error occurred while updating your profile.";
            return View(model);
        }


        //[HttpPost]
        //public async Task<IActionResult> UpdateProfile(DriverViewModel model)
        //{
        //    var request = HttpContext.Request;

        //    // تأكد من وجود ملفات مرفوعة
        //    if (request.Form.Files.Count > 0)
        //    {
        //        foreach (var file in request.Form.Files)
        //        {
        //            if (file.Length > 0)
        //            {
        //                // التحقق من اسم الملف ونوعه
        //                string fileExtension = Path.GetExtension(file.FileName).ToLower();
        //                if (fileExtension != ".jpg" && fileExtension != ".jpeg" && fileExtension != ".png")
        //                {
        //                    Console.WriteLine("Error: Invalid file type.");
        //                    TempData["ErrorMessage"] = "Invalid file type. Please upload a .jpg, .jpeg, or .png file.";
        //                    return View(model);  // يجب إظهار رسالة خطأ للمستخدم
        //                }

        //                // تحديد مجلد التحميل
        //                string directoryPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
        //                if (!Directory.Exists(directoryPath))
        //                {
        //                    Directory.CreateDirectory(directoryPath);
        //                }

        //                // تحديد المجلد الفرعي (إما 'drivers' أو 'licenses')
        //                string subFolder = file.Name == "profilePicture" ? "drivers" : "licenses";

        //                string subFolderPath = Path.Combine(directoryPath, subFolder);
        //                if (!Directory.Exists(subFolderPath))
        //                {
        //                    Directory.CreateDirectory(subFolderPath);
        //                }

        //                // إنشاء مسار الملف
        //                string filePath = Path.Combine(subFolderPath, $"{Guid.NewGuid()}{fileExtension}");

        //                // حفظ الملف على الخادم
        //                using (var stream = new FileStream(filePath, FileMode.Create))
        //                {
        //                    await file.CopyToAsync(stream);
        //                }

        //                // عرض رسالة في الكونسول للتحقق
        //                Console.WriteLine($"File uploaded: {file.FileName} -> {filePath}");

        //                // إذا كانت الصورة هي Profile Picture أو License Picture
        //                if (file.Name == "profilePicture")
        //                {
        //                    model.ProfilePicture = Path.Combine("uploads", "drivers", Path.GetFileName(filePath));  // حفظ مسار الملف في الـ model
        //                }
        //                else if (file.Name == "licensePicture")
        //                {
        //                    model.LicensePicture = Path.Combine("uploads", "licenses", Path.GetFileName(filePath));  // حفظ مسار الملف في الـ model
        //                }
        //            }
        //        }
        //    }
        //    else
        //    {
        //        // إذا لم يتم رفع أي ملف
        //        Console.WriteLine("No files uploaded.");
        //    }

        //    // تحديث البيانات النصية
        //    var driver = await _userManager.GetUserAsync(User);
        //    if (driver == null) return RedirectToAction("Register");

        //    driver.UserName = model.Name;
        //    driver.Email = model.Email;
        //    driver.PhoneNumber = model.phone;
        //    driver.VehicleType = model.VehicleType;

        //    // تحديث الصور في قاعدة البيانات
        //    driver.ProfilePicture = model.ProfilePicture;
        //    driver.LicensePicture = model.LicensePicture;

        //    var result = await _userManager.UpdateAsync(driver);

        //    if (result.Succeeded)
        //    {
        //        TempData["SuccessMessage"] = "Your profile has been updated successfully.";
        //        Console.WriteLine("Profile updated successfully.");
        //        return RedirectToAction("Dashboard");
        //    }

        //    TempData["ErrorMessage"] = "An error occurred while updating your profile.";
        //    return View(model);
        //}



        //    var driverId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        //    if (string.IsNullOrEmpty(driverId))
        //    {
        //        return Unauthorized();
        //    }

        //    var driver = await db.Driver_table.FindAsync(int.Parse(driverId));
        //    if (driver == null)
        //    {
        //        return NotFound("Driver not found."); // 404
        //    }

        //    return View(driver);
        //}

        ////[HttpPost]
        ////public async Task<IActionResult> UpdateProfile(Driver driver)
        ////{
        ////    if (!ModelState.IsValid)
        ////    {
        ////        return View(driver);
        ////    }

        ////    var driverId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        ////    if (string.IsNullOrEmpty(driverId))
        ////    {
        ////        return Unauthorized();
        ////    }

        ////    var existingDriver = await db.Driver_table.FirstOrDefaultAsync(d => d.Id == int.Parse(driverId));

        ////    if (existingDriver == null)
        ////    {
        ////        return NotFound("Driver not found.");//404
        ////    }

        ////    existingDriver.UserName = driver.UserName;
        ////    existingDriver.Email = driver.Email;
        ////    existingDriver.PhoneNumber = driver.PhoneNumber;
        ////    existingDriver.VehicleType = driver.VehicleType;
        ////    existingDriver.ProfilePicture = driver.ProfilePicture;
        ////    existingDriver.LicensePicture = driver.LicensePicture;

        ////    existingDriver.CurrentLocation = driver.CurrentLocation;
        ////    existingDriver.Latitude = driver.Latitude;
        ////    existingDriver.Longitude = driver.Longitude;

        ////    try
        ////    {
        ////        db.Update(existingDriver);
        ////        await db.SaveChangesAsync();
        ////        TempData["SuccessMessage"] = "Profile updated successfully!";
        ////        return RedirectToAction("Dashboard");
        ////    }
        ////    catch (Exception ex)
        ////    {
        ////        ModelState.AddModelError(string.Empty, "Error saving data: " + ex.Message);//@Html.ValidationSummary in html
        ////        return View(driver);
        ////    }



        //[HttpPost]
        //public async Task<IActionResult> UpdateProfile(Driver driver, string profilePicture, string licensePicture)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return View(driver);
        //    }

        //    var driverId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        //    if (string.IsNullOrEmpty(driverId))
        //    {
        //        return Unauthorized();
        //    }

        //    var existingDriver = await db.Driver_table.FirstOrDefaultAsync(d => d.Id == int.Parse(driverId));

        //    if (existingDriver == null)
        //    {
        //        return NotFound("Driver not found.");//404
        //    }

        //    // تحديث البيانات الأخرى
        //    existingDriver.UserName = driver.UserName;
        //    existingDriver.Email = driver.Email;
        //    existingDriver.PhoneNumber = driver.PhoneNumber;
        //    existingDriver.VehicleType = driver.VehicleType;

        //    if (!string.IsNullOrEmpty(profilePicture))
        //    {
        //        existingDriver.ProfilePicture = profilePicture; // حفظ المسار الذي تم تمريره
        //    }

        //    // إذا كانت صورة رخصة القيادة موجودة في المسار، نقوم بتحديث مسار الصورة
        //    if (!string.IsNullOrEmpty(licensePicture))
        //    {
        //        existingDriver.LicensePicture = licensePicture; // حفظ المسار الذي تم تمريره
        //    }

        //    // باقي البيانات
        //    existingDriver.CurrentLocation = driver.CurrentLocation;
        //    existingDriver.Latitude = driver.Latitude;
        //    existingDriver.Longitude = driver.Longitude;

        //    try
        //    {
        //        db.Update(existingDriver);
        //        await db.SaveChangesAsync();
        //        TempData["SuccessMessage"] = "Profile updated successfully!";
        //        return RedirectToAction("Dashboard");
        //    }
        //    catch (Exception ex)
        //    {
        //        ModelState.AddModelError(string.Empty, "Error saving data: " + ex.Message);
        //        return View(driver);
        //    }
        //}


    }
}




