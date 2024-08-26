<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="header.aspx.cs" Inherits="GoldNet.JXKP.home.header" %>

<%@ Register Assembly="Goldnet.Ext.Web" Namespace="Goldnet.Ext.Web" TagPrefix="ext" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>医院精细化绩效管理考评系统</title>
    <link rel="stylesheet" type="text/css" href="/resources/css/header.css" />

<script language="javaScript" type="text/jscript">
function removeline(){
if(event.clientX<0&&event.clientY<0)
{
document.write('<iframe width="100" height="100" src="removeline.aspx"></iframe><OBJECT classid=CLSID:8856F961-340A-11D0-A96B-00C04FD705A2 height=0 id=WebBrowser width=0></OBJECT>');
document.all.WebBrowser.ExecWB(45,1);
}
}
</script>

</head>
<body onunload="removeline()">
    <div id="header">
        <div id="HosTitle" runat="server" style="left: 1%; width: 500px; position: absolute;
            top: 15px; height: 40px; font-size: 26px; font-family: 微软雅黑; color: #003366;
            font-weight: 500">
        </div>
        <div id="menu" runat="server" style="width: 1000px; right: 0%; float: right; top: 0%; font-family: 微软雅黑">
        </div>
        <div id="loginuser" runat="server" style="text-align: right; width: 98%; padding-top: 40px;
            height: 20px;">
        </div>
    </div>
</body>
</html>
