#pragma checksum "C:\Users\CRIMSON\source\repos\Voleska\Voleska\Views\Shared\Components\PrenosPodatkovLajkVC\Default.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "d3fdcbe84f72589ed7282dd6ce770337a3f7c4b1"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Shared_Components_PrenosPodatkovLajkVC_Default), @"mvc.1.0.view", @"/Views/Shared/Components/PrenosPodatkovLajkVC/Default.cshtml")]
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
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"d3fdcbe84f72589ed7282dd6ce770337a3f7c4b1", @"/Views/Shared/Components/PrenosPodatkovLajkVC/Default.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"7c5f7b007a5a54447059229f1ea38e9da81d1708", @"/Views/_ViewImports.cshtml")]
    public class Views_Shared_Components_PrenosPodatkovLajkVC_Default : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<Voleska.ViewModel.LajkAjaxViewModel>
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            WriteLiteral("\r\n<input disabled type=\"hidden\"");
            BeginWriteAttribute("id", " id=\"", 75, "\"", 104, 2);
            WriteAttributeValue("", 80, "prenosPodatkov-", 80, 15, true);
#nullable restore
#line 3 "C:\Users\CRIMSON\source\repos\Voleska\Voleska\Views\Shared\Components\PrenosPodatkovLajkVC\Default.cshtml"
WriteAttributeValue("", 95, Model.ID, 95, 9, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            BeginWriteAttribute("asp-route-likeAjax", " asp-route-likeAjax=\"", 105, "\"", 137, 1);
#nullable restore
#line 3 "C:\Users\CRIMSON\source\repos\Voleska\Voleska\Views\Shared\Components\PrenosPodatkovLajkVC\Default.cshtml"
WriteAttributeValue("", 126, Model.Like, 126, 11, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            BeginWriteAttribute("asp-route-dislikeAjax", " asp-route-dislikeAjax=\"", 138, "\"", 176, 1);
#nullable restore
#line 3 "C:\Users\CRIMSON\source\repos\Voleska\Voleska\Views\Shared\Components\PrenosPodatkovLajkVC\Default.cshtml"
WriteAttributeValue("", 162, Model.Dislike, 162, 14, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            BeginWriteAttribute("asp-route-steviloDislikeAjax", " asp-route-steviloDislikeAjax=\"", 177, "\"", 229, 1);
#nullable restore
#line 3 "C:\Users\CRIMSON\source\repos\Voleska\Voleska\Views\Shared\Components\PrenosPodatkovLajkVC\Default.cshtml"
WriteAttributeValue("", 208, Model.SteviloDislike, 208, 21, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            BeginWriteAttribute("asp-route-steviloLikeAjax", " asp-route-steviloLikeAjax=\"", 230, "\"", 276, 1);
#nullable restore
#line 3 "C:\Users\CRIMSON\source\repos\Voleska\Voleska\Views\Shared\Components\PrenosPodatkovLajkVC\Default.cshtml"
WriteAttributeValue("", 258, Model.SteviloLike, 258, 18, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            BeginWriteAttribute("asp-route-lajkAjax", " asp-route-lajkAjax=\"", 277, "\"", 307, 1);
#nullable restore
#line 3 "C:\Users\CRIMSON\source\repos\Voleska\Voleska\Views\Shared\Components\PrenosPodatkovLajkVC\Default.cshtml"
WriteAttributeValue("", 298, Model.ID, 298, 9, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            WriteLiteral(" >\r\n");
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
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<Voleska.ViewModel.LajkAjaxViewModel> Html { get; private set; }
    }
}
#pragma warning restore 1591
