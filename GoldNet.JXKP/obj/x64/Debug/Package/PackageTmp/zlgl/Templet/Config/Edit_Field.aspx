<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Edit_Field.aspx.cs" Inherits="GoldNet.JXKP.zlgl.Templet.Config.Edit_Field" %>

<%@ Register TagPrefix="uc1" TagName="title" Src="title.ascx" %>
<%@ Register Assembly="Goldnet.Ext.Web" Namespace="Goldnet.Ext.Web" TagPrefix="ext" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<uc1:title ID="Title1" runat="server"></uc1:title>
<body>
    <ext:ScriptManager ID="ScriptManager1" runat="server">
    </ext:ScriptManager>
    <form id="form1" runat="server">
    <div>
    <ext:FormPanel ID="FormPanel1" runat="server" Border="false" AutoHeight="true" AutoScroll="true"
        ButtonAlign="Right" StyleSpec="background-color:transparent" BodyStyle="background-color:transparent">
        <Body>
            <ext:Panel ID="Panel1" runat="server" Border="false" AutoHeight="true" AutoWidth="true"
                StyleSpec="background-color:transparent" BodyStyle="background-color:transparent">
                <TopBar>
                    <ext:Toolbar ID="Toolbar1" runat="server">
                        <Items>
                            <ext:ToolbarSeparator ID="ToolbarSeparator1" runat="server">
                            </ext:ToolbarSeparator>
                            <ext:Button ID="btnSave" runat="server" Text="保存" Icon="Disk"
                               >
                                  <AjaxEvents>
                                                    <Click OnEvent="btnSave_Click">
                                                    </Click>
                                                </AjaxEvents>
                            </ext:Button>
                            <ext:Button ID="btnCancle" runat="server" Text="返回" Icon="ArrowUndo">
                                <AjaxEvents>
                                    <Click OnEvent="btnCancle_Click">
                                    </Click>
                                </AjaxEvents>
                            </ext:Button>
                        </Items>
                    </ext:Toolbar>
                </TopBar>
                </ext:Panel>
                 <ext:Panel ID="Panel2" runat="server" Border="false" Height="400" AutoWidth="true" AutoScroll="true"
                StyleSpec="background-color:transparent" BodyStyle="background-color:transparent">
                <Body>
                    <table class="gs-maintab" cellspacing="0" cellpadding="0" border="0" width="90%">
                        <tr>
                            <td>
                                <table class="gs-input-tab" cellspacing="0" cellpadding="0" border="0" width="100%">
                                    <tr>
                                        <td class="gs-input-desc">
                                            使用此页面修改已有字段。
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="gs-input-separator">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td valign="top">
                                            <table width="100%" border="0" cellpadding="0" cellspacing="0" class="gs-input-tab">
                                                <tr>
                                                    <td class="gs-input-title" width="40%" valign="top">
                                                        名称和类型
                                                        <table width="100%" border="0" cellpadding="0" cellspacing="0" class="gs-input-desc">
                                                            <tr>
                                                                <td>
                                                                    可以在此修改字段名称，但不支持更改字段的类型。
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                    <td width="60%">
                                                        <table width="100%" border="0" cellpadding="0" cellspacing="0">
                                                            <tr>
                                                                <td class="gs-input-section">
                                                                    字段名:<br>
                                                                    &nbsp;
                                                                         <ext:TextField ID="textFieldName" runat="server" Width="180" AllowBlank="false"></ext:TextField>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td class="gs-input-section">
                                                                    字段类型:<br>
                                                                    <table id="tabFieldTypes" width="150" border="0" cellspacing="0" cellpadding="0">
                                                                        <tr>
                                                                            <td width="5%" align="right">
                                                                            </td>
                                                                            <td>
                                                                                <table id="radiobtnlistFieldTypes" border="0" style="width: 100px">
                                                                                    <tr>
                                                                                        <td>
                                                                                            <asp:Label ID="labFieldType" runat="server"></asp:Label>
                                                                                        </td>
                                                                                    </tr>
                                                                                </table>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td class="gs-input-section">
                                                                    是否添加到默认视图:<br>
                                                                    &nbsp;
                                                                    <asp:CheckBox ID="chkboxAddToDefaultView" runat="server" Text="添加到默认视图中"></asp:CheckBox>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="gs-input-separator">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td id="ContentTD" valign="top">
                                            <table width="100%" border="0" cellpadding="0" cellspacing="0" class="gs-input-tab">
                                                <tr>
                                                    <td class="gs-input-title" width="40%" valign="top">
                                                        字段的可选属性
                                                        <table width="100%" border="0" cellpadding="0" cellspacing="0" class="gs-input-desc">
                                                            <tr>
                                                                <td>
                                                                    请为此字段指定详细的选项。
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                    <td id="tdSpecialProperty" width="60%" runat="server">
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="gs-input-separator">
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        
                    </table>
                </Body>
            </ext:Panel>
        </Body>
    </ext:FormPanel>
    </div>
    </form>
</body>
</html>
