<%@ Page Language="C#" AutoEventWireup="true" EnableEventValidation="false" ValidateRequest="false"
    CodeBehind="print.aspx.cs" Inherits="GoldNet.JXKP.print" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="../../lib/ligerUI/skins/aqua/css/ligerui-all.css" rel="stylesheet" type="text/css" />
    <script src="../../lib/jquery/jquery-1.3.2.min.js" type="text/javascript"></script>
    <script src="../../lib/ligerUI/js/ligerui.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        function GetQueryString(name) {
            var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)");
            var r = window.location.search.substr(1).match(reg);
            if (r != null) return unescape(r[2]); return null;
        }
        function gethtml(g) {
            parent.$(".l-grid-header-table", g).attr("border", "1");
            parent.$(".l-grid-body-table", g).attr("border", "1");
            
            $("#hf").val(
                        parent.$(".l-grid2", g).html()    //表身，具体数据  
                        );
            parent.$(".l-grid-header-table", g).attr("border", "0");
            parent.$(".l-grid-body-table", g).attr("border", "0");
        }

        function init() {
            if (GetQueryString("exporttype") == "xls") {
                document.getElementById("btnxls").click();
            }
            else {
                document.getElementById("btndoc").click();
            }
            setTimeout(function () {
                parent.dialog.close();
            }, 3000);
        }  
                                  
    </script>
</head>
<body onload="init()">
    <form id="form1" runat="server">
    导出中...
    <div style="visibility: hidden">
        <asp:Button ID="btnxls" runat="server" Text="导出Excel" OnClick="Button1_Click" OnClientClick="gethtml('#maingrid')" />
        <asp:Button ID="btndoc" runat="server" Text="导出Word" OnClick="Button2_Click" OnClientClick="gethtml('#maingrid')" />
    </div>
    <asp:HiddenField ID="hf" runat="server" />
    </form>
</body>
</html>
