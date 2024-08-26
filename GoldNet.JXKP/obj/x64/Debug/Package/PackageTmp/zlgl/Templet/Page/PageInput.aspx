<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PageInput.aspx.cs" Inherits="GoldNet.JXKP.zlgl.Templet.Page.PageInput" %>
<%@ Register Assembly="Goldnet.Ext.Web" Namespace="Goldnet.Ext.Web" TagPrefix="ext" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link rel="stylesheet" type="text/css" href="/resources/css/main.css" />
    <style type="text/css">
        body
        {
            background-color: #DFE8F6;
            font-size: 12px;
        }
        td strong
        {
            color: Red;
        }
    </style>
    <style type="text/css">
        .search-item
        {
            font: normal 11px tahoma, arial, helvetica, sans-serif;
            padding: 3px 10px 3px 10px;
            border: 1px solid #fff;
            border-bottom: 1px solid #eeeeee;
            white-space: normal;
            color: #555;
        }
        .search-item h3
        {
            display: block;
            font: inherit;
            font-weight: bold;
            color: #222;
        }
        .search-item h3 span
        {
            float: right;
            font-weight: normal;
            margin: 0 0 5px 5px;
            width: 100px;
            display: block;
            clear: none;
        }
        p
        {
            width: 650px;
        }
        .ext-ie .x-form-text
        {
            position: static !important;
        }
    </style>
</head>
<body>
    <ext:ScriptManager ID="ScriptManager1" runat="server">
    </ext:ScriptManager>
    <form id="form1" runat="server" method="post" style="background-color: Transparent">
    <div>
        <ext:FormPanel ID="FormPanel1" runat="server" Border="false" AutoScroll="true" ButtonAlign="Right"
            StyleSpec="background-color:transparent" BodyStyle="background-color:transparent">
            <Body>
                <ext:Panel ID="Panel1" runat="server" Border="false" AutoHeight="true" AutoWidth="true"
                    StyleSpec="background-color:transparent" BodyStyle="background-color:transparent">
                    <TopBar>
                        <ext:Toolbar ID="Toolbar1" runat="server">
                            <Items>
                                <ext:Button ID="BtnSave" runat="server" Text="确定" Icon="Disk">
                                    <AjaxEvents>
                                        <Click OnEvent="BtnSave_Click">
                                        </Click>
                                    </AjaxEvents>
                                </ext:Button>
                                <ext:ToolbarSeparator ID="ToolbarSeparator1" runat="server">
                                </ext:ToolbarSeparator>
                                <ext:Button ID="CancelButton" runat="server" Text="返回" Icon="ArrowUndo">
                                    <Listeners>
                                        <Click Handler="parent.ListDetail.hide();" />
                                    </Listeners>
                                </ext:Button>
                            </Items>
                        </ext:Toolbar>
                    </TopBar>
                </ext:Panel>
                <ext:Panel ID="Panel2" runat="server" Border="false" Height="400" AutoWidth="true"
                    AutoScroll="true" StyleSpec="background-color:transparent" BodyStyle="background-color:transparent">
                    <Body>
                        <table id="tabInput" style="width: 100%" border="0" runat="server" valign="top">
                            <tr>
                                <td>
                                    <asp:Label ID="labTitle" runat="server"></asp:Label>
                                </td>
                            </tr>
                        </table>
                       
                        <asp:DropDownList ID="DropDownList1" runat="server" Visible="false">
                        </asp:DropDownList>
                        <asp:CheckBox ID="quality" runat="server" Text="是否跟踪监控"></asp:CheckBox>
                    </Body>
                </ext:Panel>
            </Body>
        </ext:FormPanel>
    </div>
    </form>
</body>
</html>
