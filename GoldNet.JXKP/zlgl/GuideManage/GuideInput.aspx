<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="GuideInput.aspx.cs" Inherits="GoldNet.JXKP.zlgl.SysManage.GuideInput" %>

<%@ Register Assembly="Goldnet.Ext.Web" Namespace="Goldnet.Ext.Web" TagPrefix="ext" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
      <style type="text/css">
        body
        {
            background-color: #DFE8F6;
            font-size: 12px;
        }
    </style>
    
</head>
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
                            <ext:Button ID="btnSave" runat="server" Text="保存" Icon="Disk" >
                            <AjaxEvents>
                                                    <Click OnEvent="BtnSave_Click">
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
            <ext:Panel ID="Panel2" runat="server" Border="false" AutoHeight="true" AutoWidth="true"
                AutoScroll="true" StyleSpec="background-color:transparent" BodyStyle="background-color:transparent">
                <Body>
                    <table cellspacing="0" cellpadding="0" width="100%" border="0">
                        <tr>
                            <td align="right" width="20%">
                                <font face="宋体">指标类别：</font>
                            </td>
                            <td width="80%">
                                <asp:DropDownList ID="DDLGuideType" runat="server" CssClass="gs-text-style" AutoPostBack="True"
                                    OnSelectedIndexChanged="DDLGuideType_SelectedIndexChanged" Width="200">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td width="20%">
                                &nbsp;&nbsp;
                            </td>
                            <td width="80%">
                                &nbsp;&nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td align="right" width="20%">
                                <font face="宋体">指标名称：</font>
                            </td>
                            <td width="80%">
                                <asp:DropDownList ID="DDLGuideName" runat="server" CssClass="gs-text-style" AutoPostBack="True"
                                    OnSelectedIndexChanged="DDLGuideName_SelectedIndexChanged" Width="200">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td width="20%">
                                &nbsp;&nbsp;
                            </td>
                            <td width="80%">
                                &nbsp;&nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td align="right" width="20%">
                                <font face="宋体">主管部门：</font>
                            </td>
                            <td width="80%">
                                <asp:TextBox ID="TBManaDept" runat="server" CssClass="gs-text-style" Width="200px"
                                    ReadOnly="True"></asp:TextBox><input language="javascript" id="DEPT_INPUT" ondragenter="return false;"
                                        onblur="Dept_Selector_OnBlur();" onkeyup="Dept_Selector_OnKeyup(this, document.all['DEPT_ID']);"
                                        onfocus="this.select();" type="text" maxlength="10" name="DEPT_INPUT" runat="server"><input
                                            id="DEPT_ID" style="width: 65px" readonly type="text" maxlength="10" name="DEPT_ID"
                                            runat="server">
                            </td>
                        </tr>
                        <tr>
                            <td width="20%">
                                &nbsp;&nbsp;
                            </td>
                            <td width="80%">
                                &nbsp;&nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td align="right" width="20%">
                                <font face="宋体">考评分值：</font>
                            </td>
                            <td width="80%">
                                <asp:TextBox ID="TBGuideNum" runat="server" CssClass="gs-text-style" Width="200px"
                                    ReadOnly="True"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td width="20%">
                            </td>
                            <td width="80%">
                                <asp:TextBox ID="TBCheckDept" runat="server" CssClass="gs-text-style" Width="300px"
                                    ReadOnly="True" Visible="False"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td width="20%">
                                <font face="宋体"></font>
                            </td>
                            <td width="80%">
                                <asp:TextBox ID="GuideTypeID" runat="server" Visible="False"></asp:TextBox><asp:TextBox
                                    ID="TypeSign" runat="server" Visible="False"></asp:TextBox><asp:TextBox ID="GuideNameID"
                                        runat="server" Visible="False"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td width="20%">
                            </td>
                            <td width="80%">
                                <font face="宋体"></font>
                            </td>
                        </tr>
                        <tr>
                            <td align="right" width="20%">
                                <font face="宋体">
                                    <asp:Label ID="Label1" runat="server" Visible="False">考评类别：</asp:Label></font>
                            </td>
                            <td width="80%">
                                <asp:DropDownList ID="Dropdownlisttype" runat="server" CssClass="gs-text-style" 
                                    Visible="False" >
                                </asp:DropDownList>
                                
                            </td>
                        </tr>
                        <tr>
                            <td width="20%">
                            </td>
                            <td width="80%">
                                <font face="宋体"></font>
                            </td>
                        </tr>
                        <tr>
                            <td align="right" width="20%">
                                <font face="宋体">考评内容：</font>
                            </td>
                            <td width="80%">
                                <font face="宋体"></font>
                            </td>
                        </tr>
                        <tr>
                            <td width="20%">
                            </td>
                            <td width="80%">
                                <font face="宋体">
                                    
                                        <ext:TextArea ID="TBCheckCont" runat="server" Width="300" AllowBlank="false" ></ext:TextArea>
                                        
                            </td>
                        </tr>
                        <tr>
                            <td width="20%">
                                &nbsp;&nbsp;
                            </td>
                            <td width="80%">
                                &nbsp;&nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td align="right" width="20%">
                                <font face="宋体">考评标准：</font>
                            </td>
                            <td width="80%">
                            </td>
                        </tr>
                        <tr>
                            <td width="20%">
                            </td>
                            <td width="80%">
                                
                                    <ext:TextArea ID="TBCheckStan" runat="server" Width="300" AllowBlank="false"></ext:TextArea>
                            </td>
                        </tr>
                        <tr>
                            <td width="20%">
                                &nbsp;&nbsp;
                            </td>
                            <td width="80%">
                                &nbsp;&nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td align="right" width="20%">
                                <font face="宋体">考评办法：</font>
                            </td>
                            <td width="80%">
                            </td>
                        </tr>
                        <tr>
                            <td width="20%">
                            </td>
                            <td width="80%">
                               
                                    <ext:TextArea ID="TBCheckMeth" runat="server" Width="300" ></ext:TextArea>
                            </td>
                        </tr>
                        <tr>
                            <td width="20%" align="right">
                                <font face="宋体">是否是KPI：</font>
                            </td>
                            <td width="80%">
                            <asp:CheckBox ID="iskpi" runat="server" ></asp:CheckBox>
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
