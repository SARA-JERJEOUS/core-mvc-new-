using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore.SqlServer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dal;
using towing_services.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using towing_services.Hubs;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using towing_services.service;
using System.Security.Claims;
using Microsoft.AspNetCore.ResponseCompression;

namespace towing_services
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
          

            services.AddDistributedMemoryCache();
            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(30); // تعيين مدة انتهاء الجلسة
                options.Cookie.IsEssential = true; // لتخزين الجلسة بشكل أساسي
            });

            // إعداد CORS للسماح بالوصول من جميع النطاقات
            services.AddCors(options =>
            {
                options.AddPolicy("AllowAllOrigins", builder =>
                {
                    builder.AllowAnyOrigin()
                           .AllowAnyMethod()
                           .AllowAnyHeader();
                });
            });

            services.AddAuthentication(options =>
            {
                options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            })
            .AddCookie(options =>
            {

                options.ExpireTimeSpan = TimeSpan.FromDays(7); // مدة انتهاء الكوكيز
                options.SlidingExpiration = true; // تمديد صلاحية الكوكيز إذا كان هناك نشاط
                options.Cookie.HttpOnly = true; // منع الوصول للكوكيز عبر JavaScript
                options.Cookie.SecurePolicy = CookieSecurePolicy.Always; // استخدام الكوكيز فقط عبر HTTPS
                options.Cookie.SameSite = SameSiteMode.Strict; // منع إرسال الكوكيز مع طلبات الطرف الثالث
            });
     
            // إضافة SignalR
            services.AddSignalR()
                    .AddHubOptions<NotificationHub>(options =>
                    {
                        options.ClientTimeoutInterval = TimeSpan.FromSeconds(60); // تحديد المهلة الزمنية للعميل
                    });

            // إعدادات الحماية ضد CSRF
            services.AddAntiforgery(options =>
            {
                options.Cookie.Name = "X-CSRF-TOKEN";
                options.Cookie.HttpOnly = true;
                options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
            });

            //services.AddResponseCompression(options =>
            //{
            //    options.EnableForHttps = /*true*/false; // تمكين الضغط لطلبات HTTPS
            //});
             // إذا كانت بيئة التطوير، يتم تعطيل ضغط الاستجابة
    //if (env.IsDevelopment())
    //{
    //    services.AddResponseCompression(options =>
    //    {
    //        options.EnableForHttps = false; // تعطيل الضغط في بيئة التطوير
    //    });
    //}
            services.AddResponseCompression(options =>
            {
                // تمكين الضغط فقط لطلبات HTTPS
                options.EnableForHttps = true;

                // تحديد أنواع المحتوى التي سيتم ضغطها
                options.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(new[]
                {
        "application/json",     // JSON
        "text/html",            // HTML
        "application/xml",      // XML (إن كنت تستخدمه)
        "text/css",             // CSS
        "application/javascript" // JavaScript
    });

                // تحديد استخدام GZIP
                options.Providers.Add<GzipCompressionProvider>();
            });



            // تسجيل Identity للمستخدمين والإداريين
            services.AddIdentityCore<Admin>(options =>
            {
                options.Password.RequireDigit = true;
            })
            .AddRoles<IdentityRole<int>>()
            .AddSignInManager<SignInManager<Admin>>() // تسجيل SignInManager لـ Admin
            .AddEntityFrameworkStores<Towing_Collection>()
            .AddDefaultTokenProviders();

            services.AddIdentity<Driver, IdentityRole<int>>()
                .AddEntityFrameworkStores<Towing_Collection>()
                .AddDefaultTokenProviders();

            // تسجيل RoleManager و UserManager
            services.AddScoped<RoleManager<IdentityRole<int>>>();
            services.AddScoped<UserManager<Admin>>();
            services.AddScoped<SignInManager<Admin>>();
            services.AddScoped<UserManager<Driver>>();
            services.AddScoped<SignInManager<Driver>>();

           



            services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = "/Home/Login"; // صفحة الدخول التي ستكون مشتركة
                options.AccessDeniedPath = "/Home/Noound"; // صفحة الوصول الممنوع
            });

            // تخصيص إعدادات DataProtectorTokenProvider
            services.Configure<DataProtectionTokenProviderOptions>(options =>
            {
                options.TokenLifespan = TimeSpan.FromMinutes(15); // تحديد مدة صلاحية التوكن بدقيقة واحدة

            });

            // إعداد قاعدة البيانات
            services.AddDbContext<Towing_Collection>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("Towing"));
            });

            // إعداد البريد الإلكتروني
            var smtpConfig = Configuration.GetSection("Smtp").Get<SmtpSettings>();
            services.AddSingleton(smtpConfig);
            services.AddTransient<IEmailSender, EmailSender>();
          


            // إضافة Razor Pages و Controllers
            services.AddControllersWithViews()
                    .AddRazorRuntimeCompilation()
                    .AddMvcOptions(options => options.EnableEndpointRouting = false)
                    .AddDataAnnotationsLocalization();

            services.AddRazorPages();
            services.AddMemoryCache();  

        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            // تفعيل الجلسات
            app.UseSession();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Noound");
                app.UseHsts();
            }


            // تفعيل CORS
            app.UseCors("AllowAllOrigins");

            // تفعيل HTTPS
            app.UseHttpsRedirection();

            // تفعيل الملفات الثابتة
            app.UseStaticFiles();

            // تفعيل التوجيه
            app.UseRouting();

            // تفعيل المصادقة والتفويض
            app.UseAuthentication();
            app.UseAuthorization();
            using (var scope = app.ApplicationServices.CreateScope())
            {
                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<Admin>>();

                // إنشاء المستخدم الإداري فقط
                CreateAdminUser(userManager).Wait();
            }


            app.UseResponseCompression();
            // تفعيل SignalR
            app.UseWebSockets();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHub<NotificationHub>("/notificationHub");
                Console.WriteLine("NotificationHub mapped to '/notificationHub'");

                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Home}/{id?}");

            });

        }



        private async Task CreateAdminUser(UserManager<Admin> userManager)
        {
            var admin = await userManager.FindByEmailAsync("jerjeous@gmail.com");
            if (admin == null)
            {
                admin = new Admin
                {
                    UserName = "jerjeous@gmail.com",
                    Email = "jerjeous@gmail.com"
                };

                var result = await userManager.CreateAsync(admin, "YourSecure@1");

                if (result.Succeeded)
                {
                    Console.WriteLine("Admin user created successfully.");

                    // يمكنك إضافة هنا أي خصائص أخرى للمستخدم الإداري، مثل الحقل IsAdmin
                    admin.IsAdmin = true;  // تعيين الحقل IsAdmin إلى true
                    await userManager.UpdateAsync(admin);

                    Console.WriteLine("Admin user is marked as 'IsAdmin'.");
                }
                else
                {
                    Console.WriteLine($"Error creating admin user: {string.Join(", ", result.Errors.Select(e => e.Description))}");
                }
            }
            else
            {
                Console.WriteLine("Admin user already exists.");
            }
        }

    }

}




