#pragma checksum "C:\Users\Q4C\Desktop\x\towing_services\Views\Home\Show_driver.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "ff8820f4668d9853634499a1a5ba64ea2bf7d51b"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Home_Show_driver), @"mvc.1.0.view", @"/Views/Home/Show_driver.cshtml")]
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
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"ff8820f4668d9853634499a1a5ba64ea2bf7d51b", @"/Views/Home/Show_driver.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"dd623dedb35a7c814e5f95f54ad8e3c2c508973a", @"/Views/_ViewImports.cshtml")]
    public class Views_Home_Show_driver : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<IEnumerable<Dal.Driver>>
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            WriteLiteral(@"
<style>
    body {
        background: #EADED2;
    }
    /* تنسيق القائمة المنسدلة */
    .nav-item .dropdown-menu {
        background-color: #f0f0f0; /* لون الخلفية */
        border: none;
        border-radius: 20px; /* زوايا مستديرة */
        box-shadow: 0px 8px 15px rgb(6 34 57 / 0.10); /* ظل ناعم */
        padding: 4px;
        /* إبعاد القائمة عن الحافة اليمنى */
        margin-left: -50px; /* إضافة مسافة من الحافة اليمنى */
    }


    /* عناصر القائمة المنسدلة */
    .dropdown-item {
        font-size: 18px;
        color: #333;
        padding: 10px 20px;
        font-weight: bold;
        transition: all 0.3s ease;
    }

        /* تأثير عند المرور على عنصر القائمة المنسدلة */
        .dropdown-item:hover {
            background-color: #0b2644; /* لون الخلفية عند المرور */
            color: #fff; /* لون النص عند المرور */
            transform: scale(1.05); /* تكبير بسيط */
        }

    /* تصميم السهم */
    .nav-link.dropdown-toggle::after {
        con");
            WriteLiteral(@"tent: ' ▼'; /* إضافة سهم */
        font-size: 12px;
        color: inherit; /* نفس لون النص */
        margin-left: 5px;
        transition: transform 0.3s ease;
    }

    /* دوران السهم عند الفتح */
    .nav-link.dropdown-toggle[aria-expanded=""true""]::after {
        transform: rotate(180deg);
    }

    /* تأثير الظهور التدريجي للقائمة */
    .nav-item .dropdown-menu {
        opacity: 0;
        transform: translateY(-10px);
        transition: opacity 0.3s ease, transform 0.3s ease;
    }

        /* عند عرض القائمة */
        .nav-item .dropdown-menu.show {
            opacity: 1;
            transform: translateY(0);
        }
    .table th, .table td, span, h2 {
        font-size: 20px;
        color: #A45848;
        font-weight:bold
    }

    .btn {
        font-size: 20px;
        background: #1c1445;
        color:white

    }
        .btn:hover {
            background: #808080;
        }

    .f {
        font-size: 20px;
    }
    .bg{
        backg");
            WriteLiteral(@"round:#3243ff;

    }
    .bg-n {
        background: #5b578f;
        padding:7px 17px
        
    }
</style>

<div style=""padding:20px""></div>
<h2 class=""my-4 text-center"">Manage Drivers</h2>
<div style=""padding:8px""></div>

<table class=""table  table-hover"">
    <thead class=""table-sm"">
        <tr>
            <th class=""f"">Name</th>
            <th class=""f"">VehicleType</th>
            <th class=""f"">Email</th>
            <th class=""f"">Phone</th>
            <th class=""f"">Status</th>
            <th class=""f"">Actions</th>

        </tr>
    </thead>
    <tbody>
");
#nullable restore
#line 108 "C:\Users\Q4C\Desktop\x\towing_services\Views\Home\Show_driver.cshtml"
         foreach (var driver in Model)
        {

#line default
#line hidden
#nullable disable
            WriteLiteral("        <tr");
            BeginWriteAttribute("class", " class=\"", 2744, "\"", 2785, 1);
#nullable restore
#line 110 "C:\Users\Q4C\Desktop\x\towing_services\Views\Home\Show_driver.cshtml"
WriteAttributeValue("", 2752, (driver.IsNewDriver) ? "" : "", 2752, 33, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            WriteLiteral(">\r\n            <!-- تمييز السائقين الجدد باللون الرمادي -->\r\n            <td>");
#nullable restore
#line 112 "C:\Users\Q4C\Desktop\x\towing_services\Views\Home\Show_driver.cshtml"
           Write(driver.UserName);

#line default
#line hidden
#nullable disable
            WriteLiteral("</td>\r\n            <td>");
#nullable restore
#line 113 "C:\Users\Q4C\Desktop\x\towing_services\Views\Home\Show_driver.cshtml"
           Write(driver.VehicleType);

#line default
#line hidden
#nullable disable
            WriteLiteral("</td>\r\n\r\n            <td>");
#nullable restore
#line 115 "C:\Users\Q4C\Desktop\x\towing_services\Views\Home\Show_driver.cshtml"
           Write(driver.Email);

#line default
#line hidden
#nullable disable
            WriteLiteral("</td>\r\n            <td>");
#nullable restore
#line 116 "C:\Users\Q4C\Desktop\x\towing_services\Views\Home\Show_driver.cshtml"
           Write(driver.PhoneNumber);

#line default
#line hidden
#nullable disable
            WriteLiteral("</td>\r\n\r\n            <td>\r\n");
#nullable restore
#line 119 "C:\Users\Q4C\Desktop\x\towing_services\Views\Home\Show_driver.cshtml"
                 if (driver.IsApproved)
                {

#line default
#line hidden
#nullable disable
            WriteLiteral("                    <span class=\"badge bg fs-6\">Approved</span>\r\n");
#nullable restore
#line 122 "C:\Users\Q4C\Desktop\x\towing_services\Views\Home\Show_driver.cshtml"
                }
                else
                {

#line default
#line hidden
#nullable disable
            WriteLiteral("                    <span class=\"badge bg-n fs-6\">Pending</span>\r\n");
#nullable restore
#line 126 "C:\Users\Q4C\Desktop\x\towing_services\Views\Home\Show_driver.cshtml"
                }

#line default
#line hidden
#nullable disable
            WriteLiteral("            </td>\r\n            <td>\r\n");
#nullable restore
#line 129 "C:\Users\Q4C\Desktop\x\towing_services\Views\Home\Show_driver.cshtml"
                 if (!driver.IsApproved)
                {

#line default
#line hidden
#nullable disable
            WriteLiteral("                    <a");
            BeginWriteAttribute("href", " href=\"", 3418, "\"", 3485, 1);
#nullable restore
#line 131 "C:\Users\Q4C\Desktop\x\towing_services\Views\Home\Show_driver.cshtml"
WriteAttributeValue("", 3425, Url.Action("ApproveDriver", "Home", new { id = driver.Id }), 3425, 60, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            WriteLiteral(" class=\"btn  btn-sm\">Approve</a>\r\n                    <a");
            BeginWriteAttribute("href", " href=\"", 3542, "\"", 3608, 1);
#nullable restore
#line 132 "C:\Users\Q4C\Desktop\x\towing_services\Views\Home\Show_driver.cshtml"
WriteAttributeValue("", 3549, Url.Action("RejectDriver", "Home", new { id = driver.Id }), 3549, 59, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            WriteLiteral(" class=\"btn  btn-sm\">Reject</a>\r\n");
#nullable restore
#line 133 "C:\Users\Q4C\Desktop\x\towing_services\Views\Home\Show_driver.cshtml"
                }
                else
                {

#line default
#line hidden
#nullable disable
            WriteLiteral("                    <span class=\"text-muted\">No actions</span>\r\n");
#nullable restore
#line 137 "C:\Users\Q4C\Desktop\x\towing_services\Views\Home\Show_driver.cshtml"
                }

#line default
#line hidden
#nullable disable
            WriteLiteral("            </td>\r\n        </tr>\r\n");
#nullable restore
#line 140 "C:\Users\Q4C\Desktop\x\towing_services\Views\Home\Show_driver.cshtml"
        }

#line default
#line hidden
#nullable disable
            WriteLiteral("    </tbody>\r\n</table>\r\n");
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
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<IEnumerable<Dal.Driver>> Html { get; private set; }
    }
}
#pragma warning restore 1591
