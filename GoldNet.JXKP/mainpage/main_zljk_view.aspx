<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="main_zljk_view.aspx.cs"
    Inherits="GoldNet.JXKP.mainpage.main_zljk_view" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>质量监控详细</title>
    <style type="text/css">
        .gs-mainbody
        {
            padding: 0px;
        }
        .gs-maintab
        {
            padding: 0px;
        }
        .gs-input-tab
        {
            padding: 0px;
        }
        .gs-input-title
        {
            font-size: 12pt;
            color: #003399;
            font-weight: bold;
            padding: 2px;
            height: 18px;
        }
        .gs-input-separator
        {
            background-color: #2155B5;
            height: 1px;
        }
        .gs-input-separator2
        {
            background-color: #2155B5;
            height: 2px;
        }
        .gs-input-desc
        {
            padding: 5px 2px;
            color: #444444;
        }
        .gs-input-section 
        {
	        padding-top: 4px;
	        padding-bottom: 6px;
	        FONT-FAMILY: "Tahoma", "宋体";
        }
        TABLE, TR
        {
	        FONT-FAMILY: "Tahoma", "宋体", "Verdana";
            FONT-SIZE: 12px;
        }
    </style>
</head>
<body class="gs-mainbody">
    <form id="Form1" method="post" runat="server">
    <table class="gs-maintab" cellspacing="0" cellpadding="0" width="100%" border="0">
        <tr>
            <td id="ContentTD" valign="top">
                <table id="tabInput" runat="server" border="0" style="width: 100%">
                    <tr>
                        <td class="gs-input-title" colspan="2">
                            <asp:Label ID="labTitle" runat="server"></asp:Label>
                        </td>
                    </tr>
                     <tr>
                        <td class="gs-input-separator2" colspan="2">
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td class="gs-input-separator2">
            </td>
        </tr>
        <tr>
            <td style="height: 4px">
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="labCommon" runat="server" CssClass="gs-input-desc"></asp:Label>
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
