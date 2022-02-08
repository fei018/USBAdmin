#pragma checksum "C:\CodeRepos\USBAdmin\Server\USBAdminWebMVC\Views\Computer\Index.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "1bb432cdbf467c5d34b68493df07d05054635945"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Computer_Index), @"mvc.1.0.view", @"/Views/Computer/Index.cshtml")]
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
#line 1 "C:\CodeRepos\USBAdmin\Server\USBAdminWebMVC\Views\_ViewImports.cshtml"
using USBAdminWebMVC;

#line default
#line hidden
#nullable disable
#nullable restore
#line 2 "C:\CodeRepos\USBAdmin\Server\USBAdminWebMVC\Views\_ViewImports.cshtml"
using USBModel;

#line default
#line hidden
#nullable disable
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"1bb432cdbf467c5d34b68493df07d05054635945", @"/Views/Computer/Index.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"4594b8874090f6e413bd2d1f83140d4f6dde779b", @"/Views/_ViewImports.cshtml")]
    public class Views_Computer_Index : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<dynamic>
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
#nullable restore
#line 1 "C:\CodeRepos\USBAdmin\Server\USBAdminWebMVC\Views\Computer\Index.cshtml"
   
    var computerListUrl = USBAdminHelp.WebHttpUrlPrefix + "/computer/computerList";
    var usbHistoryUrl = USBAdminHelp.WebHttpUrlPrefix + "/computer/usbhistory";
    var printJobUrl = USBAdminHelp.WebHttpUrlPrefix + "/Computer/printjob";

    var deleteUrl = USBAdminHelp.WebHttpUrlPrefix + "/computer/delete";

#line default
#line hidden
#nullable disable
            WriteLiteral(@"
<div>
    <fieldset class=""layui-elem-field layui-field-title"" style=""margin-top: 20px;"">
        <legend>Computer</legend>
    </fieldset>

    <table id=""index"" lay-filter=""index"" class=""layui-hide"">
    </table>
</div>

<script type=""text/html"" id=""action"">
    <a class=""layui-btn layui-btn-primary layui-border-blue layui-btn-xs"" href=""javascript:;""");
            BeginWriteAttribute("layuimini-content-href", " layuimini-content-href=\"", 692, "\"", 743, 2);
#nullable restore
#line 19 "C:\CodeRepos\USBAdmin\Server\USBAdminWebMVC\Views\Computer\Index.cshtml"
WriteAttributeValue("", 717, usbHistoryUrl, 717, 14, false);

#line default
#line hidden
#nullable disable
            WriteAttributeValue("", 731, "?Id={{d.id}}", 731, 12, true);
            EndWriteAttribute();
            WriteLiteral(" data-title=\"USBHistory\">USBHistory</a>\r\n    <a class=\"layui-btn layui-btn-primary layui-border-blue layui-btn-xs\" href=\"javascript:;\"");
            BeginWriteAttribute("layuimini-content-href", " layuimini-content-href=\"", 878, "\"", 927, 2);
#nullable restore
#line 20 "C:\CodeRepos\USBAdmin\Server\USBAdminWebMVC\Views\Computer\Index.cshtml"
WriteAttributeValue("", 903, printJobUrl, 903, 12, false);

#line default
#line hidden
#nullable disable
            WriteAttributeValue("", 915, "?Id={{d.id}}", 915, 12, true);
            EndWriteAttribute();
            WriteLiteral(" data-title=\"PrintJob\">PrintJob</a>\r\n    <button type=\"button\" class=\"layui-btn layui-btn-sm layui-btn-danger\" data-id=\"{{d.id}}\" onclick=\"delComputer(this)\">Delete</button>\r\n</script>\r\n\r\n");
            DefineSection("Script", async() => {
                WriteLiteral(@"
    <script>
        layui.use(['form', 'table', 'miniTab'], function () {
            let form = layui.form,
                table = layui.table,
                miniTab = layui.miniTab;

            table.render({
                elem: '#index'
                , url: '");
#nullable restore
#line 33 "C:\CodeRepos\USBAdmin\Server\USBAdminWebMVC\Views\Computer\Index.cshtml"
                   Write(computerListUrl);

#line default
#line hidden
#nullable disable
                WriteLiteral(@"'
                , page: true
                , cols: [[
                    { field: 'hostName', title: 'HostName', width: 150, sort: true },
                    { field: 'lastSeenString', title: 'LastSeen', width: 200, sort: true },
                    { field: 'agentVersion', title: 'AgentVer', width: 100, sort: true },
                    { field: 'domain', title: 'domain', width: 150, sort: true },
                    { field: 'biosSerial', title: 'BiosSerial', width: 150, sort: true },
                    { field: 'ipAddress', title: 'IP', width: 150, sort: true },
                    { field: 'macAddress', title: 'MAC', width: 180 },                   
                    { fixed: 'right', title: 'Action', toolbar: '#action', width: 300 }
                ]]
            });

            miniTab.listen();
            
        });

        function delComputer(btn) {
            layer.confirm('Delete ?', { icon: 3, title: 'Confirm' }, function (index) {
                let id = $(btn)");
                WriteLiteral(".attr(\'data-id\');\r\n\r\n                $.post(\'");
#nullable restore
#line 55 "C:\CodeRepos\USBAdmin\Server\USBAdminWebMVC\Views\Computer\Index.cshtml"
                   Write(deleteUrl);

#line default
#line hidden
#nullable disable
                WriteLiteral(@"', { Id: id }, function (json) {
                    layer.alert(json.msg, { title: 'Message' });
                    window.location.reload();
                }, 'json');

                layer.close(index);
            });
        }
    </script>
");
            }
            );
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
