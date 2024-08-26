<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="main_kpi_ss.aspx.cs" Inherits="GoldNet.JXKP.mainpage.main_kpi_ss" %>

<%@ Register Assembly="Goldnet.Ext.Web" Namespace="Goldnet.Ext.Web" TagPrefix="ext" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <ext:ScriptManager ID="ScriptManager1" runat="server" />
    <ext:Store runat="server" ID="Store1">
        <Reader>
            <ext:JsonReader>
                <Fields>
                    <ext:RecordField Name="ZBL" />
                    <ext:RecordField Name="ZBDM" />
                    <ext:RecordField Name="ZBMC" />
                    <ext:RecordField Name="MBZ" />
                    <ext:RecordField Name="WCZ" />
                    <ext:RecordField Name="WCBFB" />
                </Fields>
            </ext:JsonReader>
        </Reader>
    </ext:Store>
    <div>
        <ext:Panel ID="Panel1" runat="server" Border="false" MonitorResize="true">
            <Body>
                <center>
                    <br>
                    <img src='/mainpage/TempImages/GaugePic2.png?temp=2<%= DateTime.Now.Ticks.ToString()%>''
                        alt="指标进度" width="300" height="186" />
                </center>
            </Body>
        </ext:Panel>
    </div>
    </form>
</body>
</html>
