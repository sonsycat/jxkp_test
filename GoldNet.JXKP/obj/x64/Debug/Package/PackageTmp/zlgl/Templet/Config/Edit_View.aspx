<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Edit_View.aspx.cs" Inherits="GoldNet.JXKP.zlgl.Templet.Config.Edit_View" %>

<%@ Register TagPrefix="uc1" TagName="title" Src="title.ascx" %>
<%@ Register Assembly="Goldnet.Ext.Web" Namespace="Goldnet.Ext.Web" TagPrefix="ext" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<uc1:title ID="Title1" runat="server"></uc1:title>
<body>
    <ext:ScriptManager ID="ScriptManager1" runat="server">
    </ext:ScriptManager>
    		<SCRIPT language="JavaScript">
<!--
function ShowHideGroup(group, img)
{
	if ((group == null))
	{
		return;
	}
	if (group.style.display != "none")
	{
		group.style.display = "none";
		img.src = "../images/plus.gif";
	}
	else
	{
        group.style.display = "inline";
		img.src = "../images/minus.gif";
	}
}
//-->
		</SCRIPT>
		
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
                                                    <Click OnEvent="btnSave_Click" >
                                                    
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
                    <table class="gs-maintab" cellspacing="0" cellpadding="0" width="90%" border="0">
                        <tr>
                            <td>
                                <table class="gs-input-tab" cellspacing="0" cellpadding="0" width="100%" border="0">
                                    <tr>
                                        <td class="gs-input-desc">
                                            使用此页面为模板添加新字段。
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="gs-input-separator">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td valign="top">
                                            <table class="gs-input-tab" cellspacing="0" cellpadding="0" width="100%" border="0">
                                                <tr>
                                                    <td class="gs-input-title" valign="top" width="40%">
                                                        名称
                                                        <table class="gs-input-desc" cellspacing="0" cellpadding="0" width="100%" border="0">
                                                            <tr>
                                                                <td>
                                                                    请为此视图键入名称，请使用具有描述性的名称， 如“本月销量列表”以使操作者明确视图内容和形式。
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                    <td  width="60%">
                                                        <table cellspacing="0" cellpadding="0" width="100%" border="0">
                                                            <tr>
                                                                <td class="gs-input-section">
                                                                    视图名称:<br>
                                                                    &nbsp;
                                                                     <ext:TextField ID="textViewName" runat="server" Width="180" AllowBlank="false"></ext:TextField>
                                                                    
                                                                        <br>
                                                                    视图名称将显示页面中，供用户选择需要的形式。
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
                                        <td valign="top">
                                            <table class="gs-input-tab" cellspacing="0" cellpadding="0" width="100%" border="0">
                                                <tbody>
                                                    <tr>
                                                        <td class="gs-input-title" valign="top" width="40%">
                                                            <a style="cursor: hand" onclick="javascript:ShowHideGroup(document.all.groupField, document.all.imgField);">
                                                                <img id="imgField" alt="" src="../images/minus.gif" border="0">字段</a>
                                                        </td>
                                                        <td>
                                                            &nbsp;
                                                        </td>
                                                    </tr>
                                                    <tbody id="groupField">
                                                        <tr>
                                                            <td valign="top" width="40%">
                                                                <table class="gs-input-desc" cellspacing="0" cellpadding="0" width="100%" border="0">
                                                                    <tr>
                                                                        <td>
                                                                            请为此字段指定详细的选项。
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                            <td width="60%">
                                                                <table  id="tabFieldList" cellspacing="0" cellpadding="0" width="100%"
                                                                    border="0" runat="server">
                                                                    <tr>
                                                                        <td id="td1" align="center" width="17%" runat="server">
                                                                            显示
                                                                        </td>
                                                                        
                                                                        <td width="45%" align="center">
                                                                            字段名称
                                                                        </td>
                                                                        <td align="center" width="38%">
                                                                            位置
                                                                        </td>
                                                                        <td id="td2" align="center" width="0%" runat="server">
                                                                            &nbsp;
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                    </tbody>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="gs-input-separator">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td valign="top">
                                            <table class="gs-input-tab" cellspacing="0" cellpadding="0" width="100%" border="0">
                                                <tbody>
                                                    <tr>
                                                        <td class="gs-input-title" valign="top" width="40%">
                                                            <a style="cursor: hand" onclick="javascript:ShowHideGroup(document.all.sortField, document.all.imgSort);">
                                                                <img id="imgSort" alt="" src="../images/plus.gif" border="0">排序</a>
                                                        </td>
                                                        <td >
                                                            &nbsp;
                                                        </td>
                                                    </tr>
                                                    <tbody id="sortField" style="display:none">
                                                        <tr>
                                                            <td valign="top" width="40%">
                                                                <table class="gs-input-desc" cellspacing="0" cellpadding="0" width="100%" border="0">
                                                                    <tr>
                                                                        <td>
                                                                            请指定排序字段并选择一个排序方向。
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                            <td width="60%">
                                                                <table cellspacing="0" cellpadding="0" width="100%" border="0">
                                                                    <tr>
                                                                        <td width="90%">
                                                                            <label for="listFristSortField">
                                                                                主要排序依据:</label><br>
                                                                            &nbsp;&nbsp;&nbsp;&nbsp;<asp:ListBox ID="listFristSortField" runat="server" CssClass="gs-input-text"
                                                                                Rows="1"></asp:ListBox>
                                                                            <br>
                                                                        </td>
                                                                        <td width="5%">
                                                                            &nbsp;
                                                                        </td>
                                                                        <td width="5%">
                                                                            &nbsp;
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            <table cellspacing="0" cellpadding="0" width="100%" border="0">
                                                                                <tr>
                                                                                    <td width="5%">
                                                                                        &nbsp;
                                                                                    </td>
                                                                                    <td width="5%">
                                                                                        <asp:RadioButton ID="rbtnFristSortAsc" runat="server" Checked="True" GroupName="fristSort">
                                                                                        </asp:RadioButton>
                                                                                    </td>
                                                                                    <td class="gs-input-section" width="5%">
                                                                                        <img height="32" src="../images/sortasc.gif" width="30">
                                                                                    </td>
                                                                                    <td width="2%">
                                                                                    </td>
                                                                                    <td width="83%">
                                                                                        <label for="rbtnFristSortAsc">
                                                                                            按升序顺序显示项目<br>
                                                                                            (A, B, C, 或 1, 2, 3)
                                                                                        </label>
                                                                                    </td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td>
                                                                                        &nbsp;
                                                                                    </td>
                                                                                    <td>
                                                                                        <asp:RadioButton ID="rbtnFristSortDesc" runat="server" GroupName="fristSort"></asp:RadioButton>
                                                                                    </td>
                                                                                    <td class="gs-input-section">
                                                                                        <img height="32" src="../images/sortdesc.gif" width="30">
                                                                                    </td>
                                                                                    <td>
                                                                                    </td>
                                                                                    <td>
                                                                                        <label for="rbtnFristSortDesc">
                                                                                            按降序顺序显示项目<br>
                                                                                            (C, B, A, 或 3, 2, 1)</label>
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </td>
                                                                        <td>
                                                                            &nbsp;
                                                                        </td>
                                                                        <td>
                                                                            &nbsp;
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            <label for="listSecondSortField">
                                                                                次要排序依据:</label><br>
                                                                            &nbsp;&nbsp;&nbsp;&nbsp;<asp:ListBox ID="listSecondSortField" runat="server" Rows="1"
                                                                                CssClass="gs-input-text"></asp:ListBox>
                                                                            <br>
                                                                        </td>
                                                                        <td>
                                                                            &nbsp;
                                                                        </td>
                                                                        <td>
                                                                            &nbsp;
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            <table cellspacing="0" cellpadding="0" width="100%" border="0">
                                                                                <tr>
                                                                                    <td width="5%">
                                                                                        &nbsp;
                                                                                    </td>
                                                                                    <td width="5%">
                                                                                        <asp:RadioButton ID="rbtnSecondSortAsc" runat="server" Checked="True" GroupName="secondSort">
                                                                                        </asp:RadioButton>
                                                                                    </td>
                                                                                    <td class="gs-input-section" width="5%">
                                                                                        <img height="32" src="../images/sortasc.gif" width="30">
                                                                                    </td>
                                                                                    <td width="2%">
                                                                                    </td>
                                                                                    <td width="83%">
                                                                                        <label for="rbtnSecondSortAsc">
                                                                                            按升序顺序显示项目<br>
                                                                                            (A, B, C, 或 1, 2, 3)
                                                                                        </label>
                                                                                    </td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td>
                                                                                        &nbsp;
                                                                                    </td>
                                                                                    <td>
                                                                                        <asp:RadioButton ID="rbtnSecondSortDesc" runat="server" GroupName="secondSort"></asp:RadioButton>
                                                                                    </td>
                                                                                    <td class="gs-input-section">
                                                                                        <img height="32" src="../images/sortdesc.gif" width="30">
                                                                                    </td>
                                                                                    <td>
                                                                                    </td>
                                                                                    <td>
                                                                                        <label for="rbtnSecondSortDesc">
                                                                                            按降序顺序显示项目<br>
                                                                                            (C, B, A, 或 3, 2, 1)</label>
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </td>
                                                                        <td>
                                                                            &nbsp;
                                                                        </td>
                                                                        <td>
                                                                            &nbsp;
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                    </tbody>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="gs-input-separator">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td valign="top">
                                            <table class="gs-input-tab" cellspacing="0" cellpadding="0" width="100%" border="0">
                                                <tbody>
                                                    <tr>
                                                        <td class="gs-input-title" valign="top" width="40%">
                                                            <a style="cursor: hand" onclick="javascript:ShowHideGroup(document.all.groupFilter, document.all.imgFilter);">
                                                                <img id="imgFilter" alt="" src="../images/plus.gif" border="0">筛选条件</a>
                                                        </td>
                                                        <td >
                                                            &nbsp;
                                                        </td>
                                                    </tr>
                                                    <tbody id="groupFilter" style="display:none">
                                                        <tr>
                                                            <td valign="top" width="40%">
                                                                <table cellspacing="0" cellpadding="0" width="100%" border="0">
                                                                    <tr>
                                                                        <td>
                                                                            请为要筛选的字段指定筛选条件和值。<br>
                                                                            <font color="#3333cc">提示：<br>
                                                                                "等于" "2008-8" 来查询2008年8月份的信息。<br>
                                                                                可以填写 "上一月"或 "上两月" 来表示查询时间。
                                                                                </font></FONT>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                            <td width="60%">
                                                                <table  id="tabFilter" cellspacing="0" cellpadding="0" width="100%"
                                                                    border="0" runat="server">
                                                                    <tr>
                                                                        
                                                                        <td width="28%" align="left">
                                                                            字段名称
                                                                        </td>
                                                                        <td width="15%" align="left">
                                                                            条件
                                                                        </td>
                                                                        <td width="55%" align="center">
                                                                            值
                                                                        </td>
                                                                        <td id="tdFilter" width="2%" runat="server">
                                                                            &nbsp;
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                    </tbody>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="gs-input-separator">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td valign="top">
                                            <table class="gs-input-tab" cellspacing="0" cellpadding="0" width="100%" border="0">
                                                <tbody>
                                                    <tr>
                                                        <td class="gs-input-title" valign="top" width="40%">
                                                            <a style="cursor: hand" onclick="javascript:ShowHideGroup(document.all.groupCollect, document.all.imgCollects);">
                                                                <img id="imgCollects" alt="" src="../images/plus.gif" border="0">汇总</a>
                                                        </td>
                                                        <td >
                                                            &nbsp;
                                                        </td>
                                                    </tr>
                                                    <tbody id="groupCollect" style="display:none">
                                                        <tr>
                                                            <td valign="top" width="40%">
                                                                <table  cellspacing="0" cellpadding="0" width="100%" border="0">
                                                                    <tr>
                                                                        <td>
                                                                            请为要汇总的字段指定汇总方法。
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                            <td width="60%">
                                                                <table id="tabCollect" cellspacing="0" cellpadding="0" width="100%"
                                                                    border="0" runat="server">
                                                                    <tr>
                                                                       
                                                                        <td width="70%" align="left">
                                                                            字段名称
                                                                        </td>
                                                                        <td align="left" width="28%">
                                                                            汇总
                                                                        </td>
                                                                         <td id="tdCollectField" align="center" width="2%" runat="server">
                                                                            &nbsp;
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                    </tbody>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="gs-input-separator">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td valign="top">
                                            <table class="gs-input-tab" cellspacing="0" cellpadding="0" width="100%" border="0">
                                                <tbody>
                                                    <tr>
                                                        <td class="gs-input-title" valign="top" width="40%">
                                                            <a style="cursor: hand" onclick="javascript:ShowHideGroup(document.all.groupPage, document.all.imgPage);">
                                                                <img id="imgPage" alt="" src="../images/plus.gif" border="0">页面限制</a>
                                                        </td>
                                                        <td >
                                                            &nbsp;
                                                        </td>
                                                    </tr>
                                                    <tbody id="groupPage" style="display:none">
                                                        <tr>
                                                            <td valign="top" width="40%">
                                                                <table  cellspacing="0" cellpadding="0" width="100%" border="0">
                                                                    <tr>
                                                                        <td>
                                                                            请指定每页显示的记录数，0为显示所有记录，不分页。
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                            <td  width="60%">
                                                                <table cellspacing="0" cellpadding="0" width="100%" border="0">
                                                                    <tr>
                                                                        <td class="gs-input-section" onpaste="return   false">
                                                                            每页显示的记录数:<br>
                                                                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                                            <ext:NumberField ID="textPageCount" runat="server" Width="60" AllowBlank="false" ></ext:NumberField>
                                                                            
                                                                            
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                    </tbody>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="gs-input-separator2">
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td align="right">
                                
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
