#pragma checksum "C:\Users\CRIMSON\source\repos\Voleska\Voleska\Views\Shared\Components\VnosKomentarjaVC\Default.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "f144eeb6497e4e7a4e610d7467730beea7c49bc6"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Shared_Components_VnosKomentarjaVC_Default), @"mvc.1.0.view", @"/Views/Shared/Components/VnosKomentarjaVC/Default.cshtml")]
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
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"f144eeb6497e4e7a4e610d7467730beea7c49bc6", @"/Views/Shared/Components/VnosKomentarjaVC/Default.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"7c5f7b007a5a54447059229f1ea38e9da81d1708", @"/Views/_ViewImports.cshtml")]
    public class Views_Shared_Components_VnosKomentarjaVC_Default : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<dynamic>
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            WriteLiteral(@"
<div class=""form-outline md-form amber-textarea active-amber-textarea animated fadeIn"">
    <i class=""fas fa-pencil-alt prefix""></i>
    <textarea id=""vsebina"" name=""vsebina"" class=""md-textarea form-control p-3"" rows=""3""></textarea>
    <label for=""vsebina"">Dodaj ");
#nullable restore
#line 5 "C:\Users\CRIMSON\source\repos\Voleska\Voleska\Views\Shared\Components\VnosKomentarjaVC\Default.cshtml"
                          Write(ViewBag.TipKomentarja);

#line default
#line hidden
#nullable disable
            WriteLiteral("...</label>\r\n</div>\r\n\r\n<div class=\"alert alert-danger no-display my-3 \" role=\"alert\" id=\"napaka\">\r\n</div>\r\n\r\n");
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