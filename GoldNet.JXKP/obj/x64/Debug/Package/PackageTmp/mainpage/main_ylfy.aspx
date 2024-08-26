<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="main_ylfy.aspx.cs" Inherits="GoldNet.JXKP.mainpage.main_ylfy" %>
<%@ Register Assembly="Goldnet.Ext.Web" Namespace="Goldnet.Ext.Web" TagPrefix="ext" %>
<%@ Register Assembly="dotnetCHARTING" Namespace="dotnetCHARTING" TagPrefix="dotnetCHARTING" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title></title>
</head>
<body>
   <form id="form1" runat="server">
   <ext:ScriptManager ID="ScriptManager1" runat="server">
    </ext:ScriptManager>
 
    <div>
        <ext:Panel ID="Panel1" runat="server" Border="false" MonitorResize="true">
            <Body>
                <center>
                    <dotnetCHARTING:Chart ID="Chartylfy" runat="server"> 
                    </dotnetCHARTING:Chart>
                </center>
            </Body>
        </ext:Panel>
    </div>
    </form>
</body>
</html>
