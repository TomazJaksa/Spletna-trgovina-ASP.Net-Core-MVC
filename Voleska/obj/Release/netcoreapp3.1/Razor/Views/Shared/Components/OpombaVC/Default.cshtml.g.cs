#pragma checksum "C:\Users\CRIMSON\source\repos\Voleska\Voleska\Views\Shared\Components\OpombaVC\Default.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "c6142de7d4fb32186466071f53944036353f2b92"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Shared_Components_OpombaVC_Default), @"mvc.1.0.view", @"/Views/Shared/Components/OpombaVC/Default.cshtml")]
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
#line 1 "C:\Users\CRIMSON\source\repos\Voleska\Voleska\Views\_ViewImports.cshtml"
using Voleska;

#line default
#line hidden
#nullable disable
#nullable restore
#line 2 "C:\Users\CRIMSON\source\repos\Voleska\Voleska\Views\_ViewImports.cshtml"
using Voleska.Models;

#line default
#line hidden
#nullable disable
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"c6142de7d4fb32186466071f53944036353f2b92", @"/Views/Shared/Components/OpombaVC/Default.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"7c5f7b007a5a54447059229f1ea38e9da81d1708", @"/Views/_ViewImports.cshtml")]
    public class Views_Shared_Components_OpombaVC_Default : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<Voleska.ViewModel.OpombaAjaxViewModel>
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            WriteLiteral("\r\n<div class=\"col-12 mt-5 mt-md-3 px-0\"");
            BeginWriteAttribute("id", " id=\"", 85, "\"", 115, 2);
            WriteAttributeValue("", 90, "vsebina-", 90, 8, true);
#nullable restore
#line 3 "C:\Users\CRIMSON\source\repos\Voleska\Voleska\Views\Shared\Components\OpombaVC\Default.cshtml"
WriteAttributeValue("", 98, Model.NarociloID, 98, 17, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            WriteLiteral(">\r\n    <div class=\"card bg-warning mb-3\">\r\n        <div class=\"card-header text-white black d-flex justify-content-between align-items-center\">\r\n            <span>Opombe za izdelek &nbsp; \'");
#nullable restore
#line 6 "C:\Users\CRIMSON\source\repos\Voleska\Voleska\Views\Shared\Components\OpombaVC\Default.cshtml"
                                       Write(Model.ImeIzdelka);

#line default
#line hidden
#nullable disable
            WriteLiteral("\'</span>\r\n");
#nullable restore
#line 7 "C:\Users\CRIMSON\source\repos\Voleska\Voleska\Views\Shared\Components\OpombaVC\Default.cshtml"
             if (ViewBag.Blagajna != true)
            {

#line default
#line hidden
#nullable disable
            WriteLiteral("                <div>\r\n                    <span data-toggle=\"modal\" data-target=\"#exampleModalCenter\">\r\n                        <span class=\"no-display\"");
            BeginWriteAttribute("id", " id=\"", 544, "\"", 578, 2);
            WriteAttributeValue("", 549, "podajOpombo-", 549, 12, true);
#nullable restore
#line 11 "C:\Users\CRIMSON\source\repos\Voleska\Voleska\Views\Shared\Components\OpombaVC\Default.cshtml"
WriteAttributeValue("", 561, Model.NarociloID, 561, 17, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            WriteLiteral(">");
#nullable restore
#line 11 "C:\Users\CRIMSON\source\repos\Voleska\Voleska\Views\Shared\Components\OpombaVC\Default.cshtml"
                                                                               Write(Model.Opomba);

#line default
#line hidden
#nullable disable
            WriteLiteral("</span>\r\n                        <button type=\"button\" class=\"btn-sm btn-outline-white\"");
            BeginWriteAttribute("onclick", " onclick=\"", 680, "\"", 812, 9);
            WriteAttributeValue("", 690, "prenesiPodatke(", 690, 15, true);
#nullable restore
#line 12 "C:\Users\CRIMSON\source\repos\Voleska\Voleska\Views\Shared\Components\OpombaVC\Default.cshtml"
WriteAttributeValue("", 705, Model.NarociloID, 705, 17, false);

#line default
#line hidden
#nullable disable
            WriteAttributeValue("", 722, ",", 722, 1, true);
            WriteAttributeValue(" ", 723, "\'", 724, 2, true);
#nullable restore
#line 12 "C:\Users\CRIMSON\source\repos\Voleska\Voleska\Views\Shared\Components\OpombaVC\Default.cshtml"
WriteAttributeValue("", 725, Model.ImeIzdelka, 725, 17, false);

#line default
#line hidden
#nullable disable
            WriteAttributeValue("", 742, "\',", 742, 2, true);
            WriteAttributeValue(" ", 744, "document.getElementById(\'podajOpombo-", 745, 38, true);
#nullable restore
#line 12 "C:\Users\CRIMSON\source\repos\Voleska\Voleska\Views\Shared\Components\OpombaVC\Default.cshtml"
WriteAttributeValue("", 782, Model.NarociloID, 782, 17, false);

#line default
#line hidden
#nullable disable
            WriteAttributeValue("", 799, "\').innerHTML)", 799, 13, true);
            EndWriteAttribute();
            WriteLiteral(">\r\n                            <i class=\"fas fa-pencil-alt prefix text-warning\"></i>\r\n                        </button>\r\n                    </span>\r\n\r\n                    <button type=\"button\" class=\"btn-sm btn-outline-white\"");
            BeginWriteAttribute("onclick", " onclick=\"", 1039, "\"", 1082, 3);
            WriteAttributeValue("", 1049, "odstraniOpombo(", 1049, 15, true);
#nullable restore
#line 17 "C:\Users\CRIMSON\source\repos\Voleska\Voleska\Views\Shared\Components\OpombaVC\Default.cshtml"
WriteAttributeValue("", 1064, Model.NarociloID, 1064, 17, false);

#line default
#line hidden
#nullable disable
            WriteAttributeValue("", 1081, ")", 1081, 1, true);
            EndWriteAttribute();
            WriteLiteral(">\r\n                        <i class=\"fa fa-trash text-warning\"></i>\r\n                    </button>\r\n                </div>\r\n");
#nullable restore
#line 21 "C:\Users\CRIMSON\source\repos\Voleska\Voleska\Views\Shared\Components\OpombaVC\Default.cshtml"
            }

#line default
#line hidden
#nullable disable
            WriteLiteral("        </div>\r\n            \r\n        <div class=\"card-body\">\r\n\r\n            <p class=\"text-black\"");
            BeginWriteAttribute("id", " id=\"", 1320, "\"", 1355, 2);
            WriteAttributeValue("", 1325, "dodanaOpomba-", 1325, 13, true);
#nullable restore
#line 26 "C:\Users\CRIMSON\source\repos\Voleska\Voleska\Views\Shared\Components\OpombaVC\Default.cshtml"
WriteAttributeValue("", 1338, Model.NarociloID, 1338, 17, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            WriteLiteral(">");
#nullable restore
#line 26 "C:\Users\CRIMSON\source\repos\Voleska\Voleska\Views\Shared\Components\OpombaVC\Default.cshtml"
                                                                 Write(Model.Opomba);

#line default
#line hidden
#nullable disable
            WriteLiteral("</p>\r\n        </div>\r\n    </div>\r\n</div>\r\n\r\n");
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
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<Voleska.ViewModel.OpombaAjaxViewModel> Html { get; private set; }
    }
}
#pragma warning restore 1591