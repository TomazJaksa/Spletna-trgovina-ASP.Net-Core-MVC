#pragma checksum "C:\Users\CRIMSON\source\repos\Voleska\Voleska\Views\Shared\Components\PrenosPodatkovLajkKomentarVC\Default.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "a14d099435dec61dee404a026f703d17bf96cca5"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Shared_Components_PrenosPodatkovLajkKomentarVC_Default), @"mvc.1.0.view", @"/Views/Shared/Components/PrenosPodatkovLajkKomentarVC/Default.cshtml")]
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
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"a14d099435dec61dee404a026f703d17bf96cca5", @"/Views/Shared/Components/PrenosPodatkovLajkKomentarVC/Default.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"7c5f7b007a5a54447059229f1ea38e9da81d1708", @"/Views/_ViewImports.cshtml")]
    public class Views_Shared_Components_PrenosPodatkovLajkKomentarVC_Default : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<Voleska.ViewModel.LajkAjaxViewModel>
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            WriteLiteral("\r\n<input disabled type=\"hidden\"");
            BeginWriteAttribute("id", " id=\"", 75, "\"", 112, 2);
            WriteAttributeValue("", 80, "prenosPodatkovKomentar-", 80, 23, true);
#nullable restore
#line 3 "C:\Users\CRIMSON\source\repos\Voleska\Voleska\Views\Shared\Components\PrenosPodatkovLajkKomentarVC\Default.cshtml"
WriteAttributeValue("", 103, Model.ID, 103, 9, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            BeginWriteAttribute("asp-route-likeAjax", " asp-route-likeAjax=\"", 113, "\"", 145, 1);
#nullable restore
#line 3 "C:\Users\CRIMSON\source\repos\Voleska\Voleska\Views\Shared\Components\PrenosPodatkovLajkKomentarVC\Default.cshtml"
WriteAttributeValue("", 134, Model.Like, 134, 11, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            BeginWriteAttribute("asp-route-dislikeAjax", " asp-route-dislikeAjax=\"", 146, "\"", 184, 1);
#nullable restore
#line 3 "C:\Users\CRIMSON\source\repos\Voleska\Voleska\Views\Shared\Components\PrenosPodatkovLajkKomentarVC\Default.cshtml"
WriteAttributeValue("", 170, Model.Dislike, 170, 14, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            BeginWriteAttribute("asp-route-steviloDislikeAjax", " asp-route-steviloDislikeAjax=\"", 185, "\"", 237, 1);
#nullable restore
#line 3 "C:\Users\CRIMSON\source\repos\Voleska\Voleska\Views\Shared\Components\PrenosPodatkovLajkKomentarVC\Default.cshtml"
WriteAttributeValue("", 216, Model.SteviloDislike, 216, 21, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            BeginWriteAttribute("asp-route-steviloLikeAjax", " asp-route-steviloLikeAjax=\"", 238, "\"", 284, 1);
#nullable restore
#line 3 "C:\Users\CRIMSON\source\repos\Voleska\Voleska\Views\Shared\Components\PrenosPodatkovLajkKomentarVC\Default.cshtml"
WriteAttributeValue("", 266, Model.SteviloLike, 266, 18, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            BeginWriteAttribute("asp-route-lajkAjax", " asp-route-lajkAjax=\"", 285, "\"", 315, 1);
#nullable restore
#line 3 "C:\Users\CRIMSON\source\repos\Voleska\Voleska\Views\Shared\Components\PrenosPodatkovLajkKomentarVC\Default.cshtml"
WriteAttributeValue("", 306, Model.ID, 306, 9, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            BeginWriteAttribute("asp-route-komentarAjax", " asp-route-komentarAjax=\"", 316, "\"", 358, 1);
#nullable restore
#line 3 "C:\Users\CRIMSON\source\repos\Voleska\Voleska\Views\Shared\Components\PrenosPodatkovLajkKomentarVC\Default.cshtml"
WriteAttributeValue("", 341, Model.KomentarID, 341, 17, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            WriteLiteral(">\r\n");
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
