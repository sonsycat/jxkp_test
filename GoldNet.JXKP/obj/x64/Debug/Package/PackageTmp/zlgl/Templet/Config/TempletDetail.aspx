<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TempletDetail.aspx.cs"
    Inherits="GoldNet.JXKP.zlgl.Templet.Config.TempletDetail" %>

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
</head>
<body>
    <ext:ScriptManager ID="ScriptManager1" runat="server">
    </ext:ScriptManager>
    <form id="form1" runat="server">
    <div>
        <ext:FormPanel ID="FormPanel1" runat="server" Border="false" AutoScroll="true" ButtonAlign="Right"
            StyleSpec="background-color:transparent" BodyStyle="background-color:transparent">
            <Body>
                <ext:Panel ID="Panel1" runat="server" Border="false" AutoHeight="true" AutoWidth="true"
                    StyleSpec="background-color:transparent" BodyStyle="background-color:transparent">
                    <TopBar>
                        <ext:Toolbar ID="Toolbar1" runat="server">
                            <Items>
                                <ext:ToolbarSeparator ID="ToolbarSeparator1" runat="server">
                                </ext:ToolbarSeparator>
                                <ext:Button ID="CancelButton" runat="server" Text="返回" Icon="ArrowUndo">
                                    <AjaxEvents>
                                        <Click OnEvent="CancelButton_Click">
                                        </Click>
                                    </AjaxEvents>
                                </ext:Button>
                            </Items>
                        </ext:Toolbar>
                    </TopBar>
                    <Body>
                        <ext:FieldSet ID="fieldset1" runat="server" Title="常规属性" Collapsible="true" Collapsed="false"
                            StyleSpec="margin:2px"  BodyStyle="background-color:Transparent;">
                            <Body>
                                <table width="95%">
                                    <tbody>
                                        <tr>
                                            <td width="35">
                                            </td>
                                            <td colspan="2">
                                                常规属性包含模板的基本信息，如名称、标题和说明等。此模板的常规属性:
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                &nbsp;
                                            </td>
                                            <td width="30%" valign="top" class="gs-input-desc">
                                                名称:
                                            </td>
                                            <td width="65%">
                                                <asp:Label ID="labName" runat="server"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                &nbsp;
                                            </td>
                                            <td valign="top" class="gs-input-desc">
                                                标题:
                                            </td>
                                            <td>
                                                <asp:Label ID="labTitle" runat="server"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                &nbsp;
                                            </td>
                                            <td valign="top" class="gs-input-desc">
                                                说明:
                                            </td>
                                            <td>
                                                <asp:Label ID="labCommon" runat="server"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                            </td>
                                            <td class="gs-navitem" colspan="2">
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                &nbsp;
                                            </td>
                                            <td colspan="2" class="gs-navitem">
                                                <asp:LinkButton ID="lnkbtnEdit" runat="server">更改常规属性</asp:LinkButton>
                                            </td>
                                        </tr>
                                        
                                    </tbody>
                                </table>
                            </Body>
                        </ext:FieldSet>
                        <ext:FieldSet ID="fieldset3" runat="server" Title="字段" Collapsible="true" Collapsed="false"
                            StyleSpec="margin:2px"  BodyStyle="background-color:Transparent;">
                            <Body>
                                <table width="100%" border="0" cellpadding="0" cellspacing="0">
                                    <tbody>
                                        <tr>
                                            <td width="35" class="gs-input-title">
                                            </td>
                                            <td width="95%">
                                                可以为此模板添加指定类型的字段。当前此模板中的字段:
                                                <asp:Label ID="labNoFieldInTemplet" runat="server" ForeColor="Red" Visible="False"><br>* 当前模板中没有字段,此模板无法使用,请为此模板添加至少一个字段。</asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                &nbsp;
                                            </td>
                                            <td valign="top">
                                                <table width="100%" border="0" cellspacing="0" cellpadding="0" id="tabFieldList"
                                                    runat="server" style="padding-right: 3px; padding-left: 3px; padding-bottom: 3px;
                                                    padding-top: 3px">
                                                    <tr>
                                                        <td width="30%" class="gs-input-desc" align="left">
                                                            字段(单击可编辑)
                                                        </td>
                                                        <td width="30%" class="gs-input-desc" align="center">
                                                            数据类型
                                                        </td>
                                                        <td width="40%" class="gs-input-desc" align="center">
                                                            是否在默认视图中
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                &nbsp;
                                            </td>
                                            <td valign="top" class="gs-input-desc">
                                                &nbsp;
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                &nbsp;
                                            </td>
                                            <td valign="top" class="gs-navitem">
                                                <asp:LinkButton ID="lnkbtnAddNewField" runat="server" CausesValidation="False">添加字段</asp:LinkButton>
                                            </td>
                                        </tr>
                                       
                                    </tbody>
                                </table>
                            </Body>
                        </ext:FieldSet>
                        <ext:FieldSet ID="fieldset2" runat="server" Title="视图" Collapsible="true" Collapsed="false"
                            StyleSpec="margin:2px" BodyStyle="background-color:Transparent;">
                            <Body>
                                <table width="100%" border="0" cellpadding="0" cellspacing="0">
                                    <tbody>
                                        <tr>
                                            <td width="35" class="gs-input-title">
                                            </td>
                                            <td colspan="3">
                                                模板视图允许您定制查看数据的样式，包含条件、排序、汇总等。此模板当前配置的视图:
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                &nbsp;
                                            </td>
                                            <td width="35%" valign="top" class="gs-input-desc">
                                                视图名称(单击可编辑)
                                            </td>
                                            <td width="40%" valign="top" class="gs-input-desc">
                                                <font face="宋体"></font>
                                            </td>
                                            <td width="20%" valign="top" class="gs-input-desc">
                                                &nbsp;
                                            </td>
                                        </tr>
                                        <tr>
                                            <td height="18">
                                                &nbsp;
                                            </td>
                                            <td valign="top" class="gs-input-desc" height="18">
                                                <asp:HyperLink ID="linkEditView" runat="server">HyperLink</asp:HyperLink>
                                            </td>
                                            <td valign="top" class="gs-input-desc" height="18">
                                                <font face="宋体"></font>
                                            </td>
                                            <td valign="top" class="gs-input-desc" height="18">
                                                &nbsp;
                                            </td>
                                        </tr>
                                        
                                        
                                    </tbody>
                                </table>
                            </Body>
                        </ext:FieldSet>
                    </Body>
                </ext:Panel>
            </Body>
        </ext:FormPanel>
    </div>
    </form>
</body>
</html>
