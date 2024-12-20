using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace towing_services
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();

        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });

        ////////////////
        //*  [HttpGet]
//        public IActionResult VerifyOtp()
//        {
//            string email = HttpContext.Session.GetString("DriverEmail");

//            if (string.IsNullOrEmpty(email))
//            {
//                return RedirectToAction("Register");
//            }

//            // تمرير عدد محاولات إعادة الإرسال المتبقية
//            int otpResendAttempts = HttpContext.Session.GetInt32("OtpResendAttempts") ?? 0;
//            ViewBag.ResendAttemptsLeft = 4 - otpResendAttempts;

//            string lastResendTimeString = HttpContext.Session.GetString("LastResendTime");
//            DateTime? lastResendTime = string.IsNullOrEmpty(lastResendTimeString) ? null : DateTime.Parse(lastResendTimeString);

//            // حساب وقت الانتظار
//            if (otpResendAttempts >= 4 && lastResendTime != null)
//            {
//                TimeSpan timeSinceLastResend = DateTime.Now - lastResendTime.Value;
//                ViewBag.TimeUntilRetry = timeSinceLastResend.TotalSeconds < 60 ? 60 - (int)timeSinceLastResend.TotalSeconds : 0;

//                // إعادة تعيين المحاولات بعد دقيقة
//                if (ViewBag.TimeUntilRetry == 0)
//                {
//                    HttpContext.Session.SetInt32("OtpResendAttempts", 0);
//                }
//            }
//            else
//            {
//                ViewBag.TimeUntilRetry = 0;
//            }

//            return View();
//        }

//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public async Task<IActionResult> VerifyOtp(string enteredOtp)
//        {
//            string email = HttpContext.Session.GetString("DriverEmail");

//            if (string.IsNullOrEmpty(email))
//            {
//                return RedirectToAction("Register");
//            }

//            string expectedOtp = HttpContext.Session.GetString("OTPCode");
//            string otpGeneratedTimeString = HttpContext.Session.GetString("OtpGeneratedTime");
//            DateTime? otpGeneratedTime = string.IsNullOrEmpty(otpGeneratedTimeString) ? null : DateTime.Parse(otpGeneratedTimeString);

//            // التحقق من انتهاء صلاحية OTP
//            if (otpGeneratedTime == null || (DateTime.Now - otpGeneratedTime.Value).TotalSeconds > 30)
//            {
//                TempData["ErrorMessage"] = "The OTP has expired. Please request a new one.";
//                return RedirectToAction("VerifyOtp");
//            }

//            if (enteredOtp == expectedOtp)
//            {
//                // نجاح التحقق
//                var driver = new Driver
//                {
//                    UserName = HttpContext.Session.GetString("DriverName"),
//                    Email = HttpContext.Session.GetString("DriverEmail"),
//                    PhoneNumber = HttpContext.Session.GetString("DriverPhone"),
//                    VehicleType = HttpContext.Session.GetString("VehicleType"),
//                    ProfilePicture = await SaveFileToDisk(Convert.FromBase64String(HttpContext.Session.GetString("ProfilePicture")), "drivers"),
//                    LicensePicture = await SaveFileToDisk(Convert.FromBase64String(HttpContext.Session.GetString("LicensePicture")), "licenses"),
//                    IsAvailable = false,
//                    IsNewDriver = true
//                };

//                var passwordHasher = new PasswordHasher<Driver>();
//                driver.PasswordHash = passwordHasher.HashPassword(driver, "passwordFromModel");

//                db.Driver_table.Add(driver);
//                await db.SaveChangesAsync();

//                HttpContext.Session.Clear();
//                return RedirectToAction("PendingApproval");
//            }
//            else
//            {
//                TempData["ErrorMessage"] = "Invalid OTP. Please try again.";
//                return RedirectToAction("VerifyOtp");
//            }
//        }

//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public async Task<IActionResult> ResendOtp()
//        {
//            string email = HttpContext.Session.GetString("DriverEmail");

//            if (string.IsNullOrEmpty(email))
//            {
//                return RedirectToAction("Register");
//            }

//            int otpResendAttempts = HttpContext.Session.GetInt32("OtpResendAttempts") ?? 0;
//            string lastResendTimeString = HttpContext.Session.GetString("LastResendTime");
//            DateTime? lastResendTime = string.IsNullOrEmpty(lastResendTimeString) ? null : DateTime.Parse(lastResendTimeString);

//            if (otpResendAttempts >= 4 && lastResendTime != null && (DateTime.Now - lastResendTime.Value).TotalSeconds < 60)
//            {
//                return Json(new { success = false, message = "Please wait a minute before trying again." });
//            }

//            if (otpResendAttempts >= 4)
//            {
//                HttpContext.Session.SetInt32("OtpResendAttempts", 0); // إعادة تعيين المحاولات بعد دقيقة
//            }

//            string otpCode = GenerateOTP();
//            await SendEmail(email, otpCode);

//            HttpContext.Session.SetString("OTPCode", otpCode);
//            HttpContext.Session.SetString("OtpGeneratedTime", DateTime.Now.ToString("o"));
//            HttpContext.Session.SetString("LastResendTime", DateTime.Now.ToString("o"));
//            HttpContext.Session.SetInt32("OtpResendAttempts", otpResendAttempts + 1);

//            return Json(new { success = true, remainingTime = 30 });
//        }

//*/
        /// 
        /// 
        /// 
        /// 
        /// 
        /// //////////////////////////////
        ///
        /// 
        /// 
        ///   // صفحة GET لعرض النموذج
        //[HttpGet]
        //public IActionResult VerifyOtp()
        //{
        //    string email = HttpContext.Session.GetString("DriverEmail");

        //    // التحقق إذا كان البريد الإلكتروني مفقودًا أو فارغًا
        //    if (string.IsNullOrEmpty(email))
        //    {
        //        return RedirectToAction("Register"); // التوجيه إلى صفحة التسجيل إذا لم يكن البريد الإلكتروني موجودًا
        //    }
        //    // تحديد حالة الزر بناءً على عدد المحاولات
        //    int otpAttempts = HttpContext.Session.GetInt32("OtpAttempts") ?? 0;
        //    ViewBag.ShowResendButton = otpAttempts > 0 && otpAttempts < 4;

        //    return View();
        //}

        ////// صفحة POST لعرض النموذج بعد إرسال OTP
        //[HttpPost]
        //[ValidateAntiForgeryToken]

        //public async Task<IActionResult> VerifyOtp(string enteredOtp)
        //{

        //    string email = HttpContext.Session.GetString("DriverEmail");

        //    // التحقق إذا كان البريد الإلكتروني مفقودًا أو فارغًا
        //    if (string.IsNullOrEmpty(email))
        //    {
        //        return RedirectToAction("Register"); // التوجيه إلى صفحة التسجيل إذا لم يكن البريد الإلكتروني موجودًا
        //    }


        //    string expectedOtp = HttpContext.Session.GetString("OTPCode");
        //    int otpAttempts = HttpContext.Session.GetInt32("OtpAttempts") ?? 0;
        //    string lastAttemptString = HttpContext.Session.GetString("LastOtpAttempt");
        //    DateTime? lastAttemptTime = string.IsNullOrEmpty(lastAttemptString) ? null : DateTime.Parse(lastAttemptString);

        //    // التحقق من تجاوز المحاولات
        //    if (otpAttempts >= 4)
        //    {
        //        if (lastAttemptTime != null && (DateTime.Now - lastAttemptTime.Value).TotalMinutes < 1)
        //        {
        //            TempData["ErrorMessage"] = "Please wait a minute before trying again.";
        //            ViewBag.ShowResendButton = false;
        //            return View();
        //        }
        //        else
        //        {
        //            otpAttempts = 0; // إعادة تعيين المحاولات بعد الانتظار
        //            HttpContext.Session.SetInt32("OtpAttempts", otpAttempts);
        //        }
        //    }



        //    if (enteredOtp == expectedOtp)
        //    {
        //        // منطق التسجيل عند نجاح OTP
        //        byte[] profilePictureBytes = Convert.FromBase64String(HttpContext.Session.GetString("ProfilePicture"));
        //        byte[] licensePictureBytes = Convert.FromBase64String(HttpContext.Session.GetString("LicensePicture"));

        //        string profilePicturePath = await SaveFileToDisk(profilePictureBytes, "drivers");
        //        string licensePicturePath = await SaveFileToDisk(licensePictureBytes, "licenses");

        //        var driver = new Driver
        //        {
        //            UserName = HttpContext.Session.GetString("DriverName"),
        //            Email = HttpContext.Session.GetString("DriverEmail"),
        //            PhoneNumber = HttpContext.Session.GetString("DriverPhone"),
        //            VehicleType = HttpContext.Session.GetString("VehicleType"),
        //            ProfilePicture = profilePicturePath,
        //            LicensePicture = licensePicturePath,
        //            IsAvailable = false,
        //            IsNewDriver = true
        //        };
        //        // تخزين كلمة السر بشكل آمن
        //        var passwordHasher = new PasswordHasher<Driver>();
        //        driver.PasswordHash = passwordHasher.HashPassword(driver, "passwordFromModel");  // استخدم كلمة السر من النموذج

        //        db.Driver_table.Add(driver);
        //        await db.SaveChangesAsync();

        //        HttpContext.Session.Clear(); // تنظيف الجلسة بعد النجاح

        //        return RedirectToAction("PendingApproval");
        //    }
        //    else
        //    {
        //        otpAttempts++;
        //        HttpContext.Session.SetInt32("OtpAttempts", otpAttempts);
        //        HttpContext.Session.SetString("LastOtpAttempt", DateTime.Now.ToString("o"));

        //        ViewBag.ShowResendButton = otpAttempts < 4;
        //        TempData["ErrorMessage"] = "Invalid OTP. Please try again.";
        //        return View();
        //    }
        //}

        ////// صفحة POST لإرسال OTP جديد
        //[HttpPost]
        //[ValidateAntiForgeryToken]

        //public async Task<IActionResult> ResendOtp()
        //{
        //    string email = HttpContext.Session.GetString("DriverEmail");

        //    // التحقق إذا كان البريد الإلكتروني مفقودًا أو فارغًا
        //    if (string.IsNullOrEmpty(email))
        //    {
        //        return RedirectToAction("Register"); // التوجيه إلى صفحة التسجيل إذا لم يكن البريد الإلكتروني موجودًا
        //    }
        //    string lastAttemptString = HttpContext.Session.GetString("LastOtpAttempt");
        //    DateTime? lastAttemptTime = string.IsNullOrEmpty(lastAttemptString) ? null : DateTime.Parse(lastAttemptString);

        //    int otpAttempts = HttpContext.Session.GetInt32("OtpAttempts") ?? 0;

        //    if (otpAttempts >= 4 && lastAttemptTime != null && (DateTime.Now - lastAttemptTime.Value).TotalMinutes < 1)
        //    {
        //        TempData["ErrorMessage"] = "Please wait a minute before requesting a new OTP.";
        //        ViewBag.ShowResendButton = false;
        //        return View("VerifyOtp");
        //    }

        //    // توليد وإرسال OTP جديد
        //    string otpCode = GenerateOTP();
        //    await SendEmail(email, otpCode);

        //    HttpContext.Session.SetString("OTPCode", otpCode);
        //    HttpContext.Session.SetString("LastOtpAttempt", DateTime.Now.ToString("o"));

        //    ViewBag.ShowResendButton = false; // إخفاء الزر بعد الإرسال
        //    TempData["SuccessMessage"] = "A new OTP has been sent to your email.";
        //    return View("VerifyOtp");
        //}
        ///////////////////////////////////////
        /// 
        /// 
        /// 
///////////////////////////////
        //////**  //// صفحة التحقق من OTP
        //[HttpGet]
        //public IActionResult VerifyOtp()
        //{
        //    return View();
        //}

        //[HttpPost]
        //public async Task<IActionResult> VerifyOtp(string enteredOtp)
        //{
        //    string expectedOtp = HttpContext.Session.GetString("OTPCode");
        //    int otpAttempts = HttpContext.Session.GetInt32("OtpAttempts") ?? 0;
        //    string lastAttemptString = HttpContext.Session.GetString("LastOtpAttempt");
        //    DateTime? lastAttemptTime = null;

        //    if (!string.IsNullOrEmpty(lastAttemptString))
        //    {
        //        lastAttemptTime = DateTime.Parse(lastAttemptString);
        //    }

        //    if (otpAttempts >= 4)
        //    {
        //        if (lastAttemptTime != null && (DateTime.Now - lastAttemptTime.Value).TotalMinutes < 10)
        //        {
        //            TempData["ErrorMessage"] = "Please wait 10 minutes before requesting a new OTP.";
        //            return View();
        //        }
        //    }

        //    if (enteredOtp == expectedOtp)
        //    {
        //        byte[] profilePictureBytes = Convert.FromBase64String(HttpContext.Session.GetString("ProfilePicture"));
        //        byte[] licensePictureBytes = Convert.FromBase64String(HttpContext.Session.GetString("LicensePicture"));

        //        string profilePicturePath = await SaveFileToDisk(profilePictureBytes, "drivers");
        //        string licensePicturePath = await SaveFileToDisk(licensePictureBytes, "licenses");

        //        var driver = new Driver
        //        {
        //            Name = HttpContext.Session.GetString("DriverName"),
        //            Email = HttpContext.Session.GetString("DriverEmail"),
        //            PhoneNumber = HttpContext.Session.GetString("DriverPhone"),
        //            VehicleType = HttpContext.Session.GetString("VehicleType"),
        //            ProfilePicture = profilePicturePath,
        //            LicensePicture = licensePicturePath,
        //            IsAvailable = false,
        //            IsNewDriver = true
        //        };

        //        db.Driver_table.Add(driver);
        //        await db.SaveChangesAsync();

        //        TempData["SuccessMessage"] = "Driver registered successfully and awaiting approval.";
        //        return RedirectToAction("PendingApproval");
        //    }
        //    else
        //    {
        //        otpAttempts++;
        //        HttpContext.Session.SetInt32("OtpAttempts", otpAttempts);
        //        HttpContext.Session.SetString("LastOtpAttempt", DateTime.Now.ToString("o"));

        //        TempData["ErrorMessage"] = "Invalid OTP. Please try again.";
        //        return View();
        //    }
        //}

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> ResendOtp()
        //{
        //    string email = HttpContext.Session.GetString("DriverEmail");
        //    string lastAttemptString = HttpContext.Session.GetString("LastOtpAttempt");
        //    DateTime? lastAttemptTime = null;

        //    if (!string.IsNullOrEmpty(lastAttemptString))
        //    {
        //        lastAttemptTime = DateTime.Parse(lastAttemptString);
        //    }

        //    if (lastAttemptTime != null && (DateTime.Now - lastAttemptTime.Value).TotalMinutes < 10)
        //    {
        //        TempData["ErrorMessage"] = "Please wait 10 minutes before requesting a new OTP.";
        //        return RedirectToAction("VerifyOtp");
        //    }

        //    string otpCode = GenerateOTP();
        //    await SendEmail(email, otpCode);

        //    HttpContext.Session.SetString("OTPCode", otpCode);
        //    HttpContext.Session.SetString("LastOtpAttempt", DateTime.Now.ToString("o"));

        //    TempData["SuccessMessage"] = "A new OTP has been sent to your email.";
        //    return RedirectToAction("VerifyOtp");
        //}

        ////////// صفحة GET لعرض النموذج
        ////////[HttpGet]
        ////////public IActionResult VerifyOtp()
        ////////{
        ////////    // إخفاء الزر إذا كانت المحاولات أقل من 4 أو أكثر من 4 فيجب إظهار الرسالة
        ////////    bool showResendButton = HttpContext.Session.GetInt32("OtpAttempts") > 0 && HttpContext.Session.GetInt32("OtpAttempts") < 4;
        ////////    ViewBag.ShowResendButton = showResendButton; // تعيين حالة عرض الزر في الـ View

        ////////    return View();
        ////////}

        ////////// صفحة POST لعرض النموذج بعد إرسال OTP
        ////////[HttpPost]
        ////////public async Task<IActionResult> VerifyOtp(string enteredOtp)
        ////////{
        ////////    string expectedOtp = HttpContext.Session.GetString("OTPCode");
        ////////    int otpAttempts = HttpContext.Session.GetInt32("OtpAttempts") ?? 0;
        ////////    string lastAttemptString = HttpContext.Session.GetString("LastOtpAttempt");
        ////////    DateTime? lastAttemptTime = null;

        ////////    if (!string.IsNullOrEmpty(lastAttemptString))
        ////////    {
        ////////        lastAttemptTime = DateTime.Parse(lastAttemptString);
        ////////    }

        ////////    // تحقق من تجاوز المحاولات
        ////////    if (otpAttempts >= 4)
        ////////    {
        ////////        if (lastAttemptTime != null && (DateTime.Now - lastAttemptTime.Value).TotalMinutes < 1)
        ////////        {
        ////////            TempData["ErrorMessage"] = "Please wait 10 minutes before requesting a new OTP.";
        ////////            ViewBag.ShowResendButton = false; // إخفاء الزر بعد المحاولات الفاشلة
        ////////            return View(); // إرجاع الصفحة مع تحديث الزر
        ////////        }
        ////////        else
        ////////        {
        ////////            // إعادة تعيين المحاولات بعد الانتظار
        ////////            otpAttempts = 0;
        ////////            HttpContext.Session.SetInt32("OtpAttempts", otpAttempts);
        ////////        }
        ////////    }

        ////////    if (enteredOtp == expectedOtp)
        ////////    {
        ////////        // منطق التسجيل عند نجاح OTP
        ////////        byte[] profilePictureBytes = Convert.FromBase64String(HttpContext.Session.GetString("ProfilePicture"));
        ////////        byte[] licensePictureBytes = Convert.FromBase64String(HttpContext.Session.GetString("LicensePicture"));

        ////////        string profilePicturePath = await SaveFileToDisk(profilePictureBytes, "drivers");
        ////////        string licensePicturePath = await SaveFileToDisk(licensePictureBytes, "licenses");

        ////////        var driver = new Driver
        ////////        {
        ////////            Name = HttpContext.Session.GetString("DriverName"),
        ////////            Email = HttpContext.Session.GetString("DriverEmail"),
        ////////            PhoneNumber = HttpContext.Session.GetString("DriverPhone"),
        ////////            VehicleType = HttpContext.Session.GetString("VehicleType"),
        ////////            ProfilePicture = profilePicturePath,
        ////////            LicensePicture = licensePicturePath,
        ////////            IsAvailable = false,
        ////////            IsNewDriver = true
        ////////        };

        ////////        db.Driver_table.Add(driver);
        ////////        await db.SaveChangesAsync();

        ////////        // تنظيف الجلسة بعد النجاح
        ////////        HttpContext.Session.Clear();

        ////////        TempData["SuccessMessage"] = "Driver registered successfully and awaiting approval.";
        ////////        return RedirectToAction("PendingApproval");
        ////////    }
        ////////    else
        ////////    {
        ////////        otpAttempts++;
        ////////        HttpContext.Session.SetInt32("OtpAttempts", otpAttempts);
        ////////        HttpContext.Session.SetString("LastOtpAttempt", DateTime.Now.ToString("o"));

        ////////        // عرض الزر إذا كانت المحاولات أقل من 4
        ////////        ViewBag.ShowResendButton = otpAttempts < 4;

        ////////        TempData["ErrorMessage"] = "Invalid OTP. Please try again.";
        ////////        return View(); // إرجاع الصفحة مع عرض الزر
        ////////    }
        ////////}

        ////////// صفحة POST لإرسال OTP جديد
        ////////[HttpPost]
        ////////[ValidateAntiForgeryToken]
        ////////public async Task<IActionResult> ResendOtp()
        ////////{
        ////////    string email = HttpContext.Session.GetString("DriverEmail");
        ////////    string lastAttemptString = HttpContext.Session.GetString("LastOtpAttempt");
        ////////    DateTime? lastAttemptTime = null;

        ////////    if (!string.IsNullOrEmpty(lastAttemptString))
        ////////    {
        ////////        lastAttemptTime = DateTime.Parse(lastAttemptString);
        ////////    }

        ////////    // التحقق إذا كانت المحاولات أكثر من 4 وكان هناك وقت انتظار
        ////////    int otpAttempts = HttpContext.Session.GetInt32("OtpAttempts") ?? 0;
        ////////    if (otpAttempts >= 4 && lastAttemptTime != null && (DateTime.Now - lastAttemptTime.Value).TotalMinutes < 1)
        ////////    {
        ////////        TempData["ErrorMessage"] = "Please wait 10 minutes before requesting a new OTP.";
        ////////        ViewBag.ShowResendButton = false; // إخفاء الزر عند هذه الحالة
        ////////        return View("VerifyOtp"); // إرجاع الصفحة مع تحديث الزر
        ////////    }

        ////////    // توليد OTP جديد وإرساله
        ////////    string otpCode = GenerateOTP();
        ////////    await SendEmail(email, otpCode);

        ////////    HttpContext.Session.SetString("OTPCode", otpCode);
        ////////    HttpContext.Session.SetString("LastOtpAttempt", DateTime.Now.ToString("o"));

        ////////    // إخفاء الزر بعد الإرسال
        ////////    ViewBag.ShowResendButton = false;

        ////////    TempData["SuccessMessage"] = "A new OTP has been sent to your email.";
        ////////    return View("VerifyOtp"); // إرجاع الصفحة مع تحديث الزر
        ////////}



        ///////////////////////****************////////////

        //// صفحة GET لعرض النموذج
        //[HttpGet]
        //public IActionResult VerifyOtp()
        //{
        //    // إخفاء الزر إذا كانت المحاولات أقل من 4 أو أكثر من 4 فيجب إظهار الرسالة
        //    bool showResendButton = HttpContext.Session.GetInt32("OtpAttempts") > 0 && HttpContext.Session.GetInt32("OtpAttempts") < 4;
        //    ViewBag.ShowResendButton = showResendButton; // تعيين حالة عرض الزر في الـ View

        //    return View();
        //}

        //// صفحة POST لعرض النموذج بعد إرسال OTP
        //[HttpPost]
        //public async Task<IActionResult> VerifyOtp(string enteredOtp)
        //{
        //    string expectedOtp = HttpContext.Session.GetString("OTPCode");
        //    int otpAttempts = HttpContext.Session.GetInt32("OtpAttempts") ?? 0;
        //    string lastAttemptString = HttpContext.Session.GetString("LastOtpAttempt");
        //    DateTime? lastAttemptTime = null;

        //    if (!string.IsNullOrEmpty(lastAttemptString))
        //    {
        //        lastAttemptTime = DateTime.Parse(lastAttemptString);
        //    }

        //    // تحقق من تجاوز المحاولات
        //    if (otpAttempts >= 4)
        //    {
        //        if (lastAttemptTime != null && (DateTime.Now - lastAttemptTime.Value).TotalMinutes < 1)
        //        {
        //            TempData["ErrorMessage"] = "Please wait 10 minutes before requesting a new OTP.";
        //            ViewBag.ShowResendButton = false; // إخفاء الزر بعد المحاولات الفاشلة
        //            return View(); // إرجاع الصفحة مع تحديث الزر
        //        }
        //        else
        //        {
        //            // إعادة تعيين المحاولات بعد الانتظار
        //            otpAttempts = 0;
        //            HttpContext.Session.SetInt32("OtpAttempts", otpAttempts);
        //        }
        //    }

        //    if (enteredOtp == expectedOtp)
        //    {
        //        // منطق التسجيل عند نجاح OTP
        //        byte[] profilePictureBytes = Convert.FromBase64String(HttpContext.Session.GetString("ProfilePicture"));
        //        byte[] licensePictureBytes = Convert.FromBase64String(HttpContext.Session.GetString("LicensePicture"));

        //        string profilePicturePath = await SaveFileToDisk(profilePictureBytes, "drivers");
        //        string licensePicturePath = await SaveFileToDisk(licensePictureBytes, "licenses");

        //        var driver = new Driver
        //        {
        //            Name = HttpContext.Session.GetString("DriverName"),
        //            Email = HttpContext.Session.GetString("DriverEmail"),
        //            PhoneNumber = HttpContext.Session.GetString("DriverPhone"),
        //            VehicleType = HttpContext.Session.GetString("VehicleType"),
        //            ProfilePicture = profilePicturePath,
        //            LicensePicture = licensePicturePath,
        //            IsAvailable = false,
        //            IsNewDriver = true
        //        };

        //        db.Driver_table.Add(driver);
        //        await db.SaveChangesAsync();

        //        // تنظيف الجلسة بعد النجاح
        //        HttpContext.Session.Clear();

        //        return RedirectToAction("PendingApproval");
        //    }
        //    else
        //    {
        //        otpAttempts++;
        //        HttpContext.Session.SetInt32("OtpAttempts", otpAttempts);
        //        HttpContext.Session.SetString("LastOtpAttempt", DateTime.Now.ToString("o"));

        //        // عرض الزر إذا كانت المحاولات أقل من 4
        //        ViewBag.ShowResendButton = otpAttempts < 4;

        //        return View(); // إرجاع الصفحة مع عرض الزر
        //    }
        //}

        //// صفحة POST لإرسال OTP جديد
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> ResendOtp()
        //{
        //    string email = HttpContext.Session.GetString("DriverEmail");
        //    string lastAttemptString = HttpContext.Session.GetString("LastOtpAttempt");
        //    DateTime? lastAttemptTime = null;

        //    if (!string.IsNullOrEmpty(lastAttemptString))
        //    {
        //        lastAttemptTime = DateTime.Parse(lastAttemptString);
        //    }

        //    // التحقق إذا كانت المحاولات أكثر من 4 وكان هناك وقت انتظار
        //    int otpAttempts = HttpContext.Session.GetInt32("OtpAttempts") ?? 0;
        //    if (otpAttempts >= 4 && lastAttemptTime != null && (DateTime.Now - lastAttemptTime.Value).TotalMinutes < 1)
        //    {
        //        TempData["ErrorMessage"] = "Please wait 10 minutes before requesting a new OTP.";
        //        ViewBag.ShowResendButton = false; // إخفاء الزر عند هذه الحالة
        //        return View("VerifyOtp"); // إرجاع الصفحة مع تحديث الزر
        //    }

        //    // توليد OTP جديد وإرساله
        //    string otpCode = GenerateOTP();
        //    await SendEmail(email, otpCode);

        //    HttpContext.Session.SetString("OTPCode", otpCode);
        //    HttpContext.Session.SetString("LastOtpAttempt", DateTime.Now.ToString("o"));

        //    // إخفاء الزر بعد الإرسال
        //    ViewBag.ShowResendButton = false;

        //    TempData["SuccessMessage"] = "A new OTP has been sent to your email.";
        //    return View("VerifyOtp"); // إرجاع الصفحة مع تحديث الزر
        //}*///
    }
}
