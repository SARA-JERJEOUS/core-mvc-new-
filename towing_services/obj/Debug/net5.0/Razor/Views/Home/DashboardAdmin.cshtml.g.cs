#pragma checksum "C:\Users\Q4C\Desktop\x\towing_services\Views\Home\DashboardAdmin.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "f296e409c3ea99ce208f5ca7379473f4af646567"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Home_DashboardAdmin), @"mvc.1.0.view", @"/Views/Home/DashboardAdmin.cshtml")]
namespace AspNetCore
{
    #line hidden
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;
#nullable restore
#line 1 "C:\Users\Q4C\Desktop\x\towing_services\Views\_ViewImports.cshtml"
using towing_services;

#line default
#line hidden
#nullable disable
#nullable restore
#line 4 "C:\Users\Q4C\Desktop\x\towing_services\Views\_ViewImports.cshtml"
using Microsoft.AspNetCore.Http;

#line default
#line hidden
#nullable disable
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"f296e409c3ea99ce208f5ca7379473f4af646567", @"/Views/Home/DashboardAdmin.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"dd623dedb35a7c814e5f95f54ad8e3c2c508973a", @"/Views/_ViewImports.cshtml")]
    public class Views_Home_DashboardAdmin : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<dynamic>
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            WriteLiteral(@"
<script src=""https://code.jquery.com/jquery-3.6.0.min.js""></script>
<script src=""https://cdn.jsdelivr.net/npm/select2@4.1.0-rc.0/dist/js/select2.min.js""></script>

<link href=""https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.0.0-beta3/css/all.min.css"" rel=""stylesheet"">
<style>
    body {
        margin: 0;
        height: 100%;
        font-family: Arial, sans-serif;
    }



    .sidebar {
        width: 100px; /* عرض الشريط الجانبي في البداية */
        background-color: #f0f0f0;
        font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;
        float: left;
        padding: 80px 10px;
        transition: width 0.3s ease;
        position: relative;
        height: 100vh; /* تأكيد أن الشريط الجانبي يغطي كامل ارتفاع الصفحة */
        overflow-y: auto; /* إضافة سكروول عمودي */
    }

        .sidebar.expanded {
            width: 400px; /* عرض الشريط الجانبي عند التوسيع */
        }

            .sidebar.expanded h2 {
                display: block;
           ");
            WriteLiteral(@" }

        .sidebar a {
            text-decoration: none;
            padding: 19px 27px;
            margin: 15px 0;
            display: flex;
            align-items: center;
            border-radius: 5px;
            cursor: pointer;
            font-size: 20px;
            white-space: nowrap;
            transition: background-color 0.2s ease, transform 0.3s ease; /* إضافة تأثير التكبير */
        }

            .sidebar a i {
                margin-right: 10px;
                font-size: 24px;
                transition: transform 0.3s ease, color 0.3s ease; /* تدوير الأيقونة */
            }

    i {
        color: #000000;
    }

    .sidebar a .sidebar-text {
        display: none;
    }

    .sidebar.expanded a .sidebar-text {
        display: inline-block;
    }

    /* إضافة التأثير عند التمرير */
    .sidebar a:hover {
        background-color: #ddd;
        transform: translateY(-10px); /* تحريك النص 10 بيكسل للأعلى */
    }

    .sidebar a i:hover {
 ");
            WriteLiteral(@"       color: #000000; /* تغيير لون الأيقونة عند التمرير */
        transform: rotate(360deg); /* تدوير الأيقونة */
    }

    .sidebar a.active {
        background-color: #ddd;
    }

    .sidebar a.logout {
        margin-top: auto;
        background-color: #ddd;
        text-align: center;
    }

    /* تنسيق زر التبديل بين الشريط الجانبي القابل للطي */
    .toggle-btn {
        position: absolute;
        top: 20px;
        left: 23px;
        padding: 20px;
        font-size: 24px; /* تكبير حجم الأيقونة */
        cursor: pointer;
        color: #fff;
        padding: 12px;
        border-radius: 5px;
        background-color: #34495e;
        transition: background-color 0.2s ease;
        z-index: 100;
        width: 50px; /* عرض الزر */
        height: 50px; /* ارتفاع الزر */
        display: flex;
        justify-content: center;
        align-items: center;
    }

        .toggle-btn:hover {
            background-color: #516f90; /* تغيير اللون عند التمرير */
 ");
            WriteLiteral("       }\r\n\r\n    span {\r\n        font-size: 23px;\r\n        font-weight: bold;\r\n    }\r\n\r\n    /* استعلامات الوسائط لتصميم متجاوب */\r\n    ");
            WriteLiteral(@"@media (max-width: 768px) {
        .sidebar {
            width: 80px;
            padding: 67px 2px;
            overflow-x: hidden;
        }

            .sidebar.expanded {
                width: 300px; /* الشريط الجانبي الموسع على الشاشات الصغيرة */
            }

        .toggle-btn {
            left: 13px;
        }

        .sidebar {
            display: block;
            font-size: 18px;
        }

            .sidebar a {
                font-size: 16px; /* تصغير حجم النص */
            }

                .sidebar a i {
                    font-size: 20px; /* تصغير الأيقونات */
                }

        span {
            font-size: 18px;
            font-weight: bold;
        }

        .sidebar a:hover {
            transform: none; /* تعطيل التأثيرات */
        }

        .sidebar a i:hover {
            transform: none; /* تعطيل تدوير الأيقونة */
        }
    }

    ");
            WriteLiteral(@"@media (max-width: 480px) {
        .sidebar {
            width: 80px;
            padding: 67px 2px;
            overflow-x: hidden;
        }

        span {
            font-size: 15px;
            font-weight: bold;
        }

        .sidebar.expanded {
            width: 230px; /* الشريط الجانبي الموسع على الهواتف */
        }

        .sidebar a {
            font-size: 14px; /* تصغير حجم النص أكثر */
        }

        .toggle-btn {
            left: 14px;
        }

        .sidebar a:hover {
            transform: none; /* تعطيل التأثيرات */
        }

        .sidebar a i:hover {
            transform: none; /* تعطيل تدوير الأيقونة */
        }
    }

    /* تحسين تنسيق السكروول */
    .sidebar::-webkit-scrollbar {
        width: 5px; /* عرض السكروول */
    }

    .sidebar::-webkit-scrollbar-thumb {
        background-color: #516f90; /* لون السكروول */
        border-radius: 5px;
        height: 1px !important; /* تحديد طول المقبض (الزر المتحرك) */
    }
");
            WriteLiteral("\r\n        .sidebar::-webkit-scrollbar-thumb:hover {\r\n            background-color: #1abc9c; /* تغيير اللون عند التمرير */\r\n        }\r\n\r\n    .container {\r\n        flex-direction: row; /* ترتيب العناصر بشكل أفقي */\r\n    }\r\n</style>\r\n\r\n\r\n\r\n<div");
            BeginWriteAttribute("class", " class=\"", 5414, "\"", 5422, 0);
            EndWriteAttribute();
            WriteLiteral(@">
    <div class=""sidebar"">
        <div class=""toggle-btn"" onclick=""toggleSidebar()"">&#9776;</div>
        <a data-section=""profile"" class=""active""><i class=""fas fa-user-circle""></i><span class=""sidebar-text"">Profile</span></a>
        <a data-section=""admin-requests""><i class=""fas fa-tasks""></i><span class=""sidebar-text"">Admin Requests</span></a>
        <a data-section=""drivers-management""><i class=""fas fa-car""></i><span class=""sidebar-text"">Drivers Management</span></a>
        <a data-section=""admins-management""><i class=""fas fa-users-cog""></i><span class=""sidebar-text"">Admins Management</span></a>
        <a data-section=""order-management"" id=""order-management-link"">
            <i class=""fas fa-users""></i><span class=""sidebar-text"">Order Management</span>
");
#nullable restore
#line 224 "C:\Users\Q4C\Desktop\x\towing_services\Views\Home\DashboardAdmin.cshtml"
             if (ViewBag.NewOrder != null && (bool)ViewBag.NewOrder)
            {

#line default
#line hidden
#nullable disable
            WriteLiteral("                <span style=\"color:red\" class=\"ms-2\">!</span>\r\n");
#nullable restore
#line 227 "C:\Users\Q4C\Desktop\x\towing_services\Views\Home\DashboardAdmin.cshtml"
            }

#line default
#line hidden
#nullable disable
            WriteLiteral(@"        </a>




        <a data-section=""reports""><i class=""fas fa-chart-line""></i><span class=""sidebar-text"">Reports</span></a>
        <a data-section=""add-admin""><i class=""fas fa-user-plus""></i><span class=""sidebar-text"">Add Admin</span></a>
        <a data-section=""add-driver""><i class=""fas fa-user-tie""></i><span class=""sidebar-text"">Add Driver</span></a>
        <a data-section=""logout"" class=""logout""><i class=""fas fa-sign-out-alt""></i><span class=""sidebar-text"">Logout</span></a>
    </div>
    <div id=""loadingIndicator"" style=""display:none;"">
        <p>Loading...</p> <!-- يمكنك تخصيص هذا بتغيير النص أو إضافة صورة Spinner -->
    </div>
    <div id=""container"" class=""main-content"">

        <!-- سيتم تحميل المحتوى هنا باستخدام AJAX -->
    </div>
</div>

<script>


    //function toggleSidebar() {
    //    document.querySelector('.sidebar').classList.toggle('expanded');
    //    const sidebar = document.querySelector('.sidebar');
    //    const mainContent = document.querySe");
            WriteLiteral(@"lector('.main-content');
    //    if (sidebar.classList.contains('expanded')) {
    //        mainContent.style.marginLeft = `${0}px`; // عرض الشريط الجانبي الموسع
    //    } else {
    //        mainContent.style.marginLeft = `${0}px`; // عرض الشريط الجانبي عند الطي
    //    }
    //}
    //$(document).ready(function () {
    //    // وظيفة لتفعيل القسم بناءً على البيانات
    //    function activateSection(section) {
    //        $.ajax({
    //            url: '/Home/GetPartialView',  // المسار الخاص بـ Controller
    //            type: 'GET',
    //            data: { section: section },  // إرسال القسم المختار
    //            success: function (data) {
    //                $('#container').html(data);  // تحميل المحتوى في الـ container
    //                $('.sidebar a').removeClass('active'); // إزالة الـ active عن كل الروابط
    //                $(`.sidebar a[data-section=""${section}""]`).addClass('active'); // إضافة active للرابط المطلوب
    //            },
    //          ");
            WriteLiteral(@"  error: function (xhr, status, error) {
    //                console.error(""Error loading partial view:"", error);
    //            }
    //        });
    //    }

    //    // التحقق من وجود الباراميتر في الرابط
    //    const urlParams = new URLSearchParams(window.location.search);
    //    const section = urlParams.get('section'); // جلب القيمة من باراميتر الـ section

    //    // إذا كان هناك باراميتر للقسم يتم تحميله مباشرة
    //    if (section) {
    //        activateSection(section);
    //    } else {
    //        activateSection(""default""); // تحميل القسم الافتراضي في حالة عدم وجود باراميتر
    //    }

    //    // تفعيل الأقسام عند الضغط اليدوي
    //    $('.sidebar a').on('click', function (e) {
    //        e.preventDefault();
    //        var section = $(this).data('section');
    //        activateSection(section);
    //        window.history.pushState({}, '', `?section=${section}`); // تحديث الرابط
    //    });
    //});



        function toggleSideba");
            WriteLiteral(@"r() {
            document.querySelector('.sidebar').classList.toggle('expanded');
        const sidebar = document.querySelector('.sidebar');
        const mainContent = document.querySelector('.main-content');
        if (sidebar.classList.contains('expanded')) {
            mainContent.style.marginLeft = `${0}px`; // عرض الشريط الجانبي الموسع
        } else {
            mainContent.style.marginLeft = `${0}px`; // عرض الشريط الجانبي عند الطي
        }
    }

        $(document).ready(function () {
            // وظيفة لتفعيل القسم بناءً على البيانات
            function activateSection(section) {
                $('#loadingIndicator').show(); // إظهار مؤشر التحميل

                $.ajax({
                    url: '/Home/GetPartialView',  // المسار الخاص بـ Controller
                    type: 'GET',
                    data: { section: section },  // إرسال القسم المختار
                    success: function (data) {
                        $('#container').html(data);  // تحميل المحتوى");
            WriteLiteral(@" في الـ container
                        $('.sidebar a').removeClass('active'); // إزالة الـ active عن كل الروابط
                        $(`.sidebar a[data-section=""${section}""]`).addClass('active'); // إضافة active للرابط المطلوب
                    },
                    error: function (xhr, status, error) {
                        console.error(""Error loading partial view:"", error);
                        console.log(xhr.responseText);  // طباعة الاستجابة من السيرفر

                    },
                    complete: function () {
                        $('#loadingIndicator').hide();  // إخفاء مؤشر التحميل عند الانتهاء
                    }
                });
            }

        // التحقق من وجود الباراميتر في الرابط
        const urlParams = new URLSearchParams(window.location.search);
        const section = urlParams.get('section'); // جلب القيمة من باراميتر الـ section

        // إذا كان هناك باراميتر للقسم يتم تحميله مباشرة
        if (section) {
            activateSec");
            WriteLiteral(@"tion(section);
        } else {
            activateSection(""default""); // تحميل القسم الافتراضي في حالة عدم وجود باراميتر
        }

        // تفعيل الأقسام عند الضغط اليدوي
        $('.sidebar a').on('click', function (e) {
            e.preventDefault();
        var section = $(this).data('section');
        activateSection(section);
        window.history.pushState({ }, '', `?section=${section}`); // تحديث الرابط
        });
    });




    ///*****2 not/

    //$('.sidebar a').on('click', function (e) {
    //    e.preventDefault(); // لمنع التحميل التقليدي للصفحة

    //    var section = $(this).data('section'); // الحصول على اسم القسم من البيانات

    //    $.ajax({
    //        url: '/Home/GetPartialView',  // المسار الخاص بـ Controller
    //        type: 'GET',
    //        data: { section: section },  // إرسال القسم المختار
    //        success: function (data) {
    //            $('#container').html(data);  // استبدال المحتوى في #container
    //        },
    /");
            WriteLiteral(@"/        error: function (xhr, status, error) {
    //            console.error(""Error loading partial view:"", error); // في حال حدوث خطأ
    //        }

    //    });
    //});





</script>



















































");
        }
        #pragma warning restore 1998
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.ViewFeatures.IModelExpressionProvider ModelExpressionProvider { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IUrlHelper Url { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IViewComponentHelper Component { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IJsonHelper Json { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<dynamic> Html { get; private set; }
    }
}
#pragma warning restore 1591
