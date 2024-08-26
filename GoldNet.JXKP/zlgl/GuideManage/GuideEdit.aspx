<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="GuideEdit.aspx.cs" Inherits="GoldNet.JXKP.zlgl.SysManage.GuideEdit" %>

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
      <script type="text/javascript">
        var CheckForm = function() {
            if (TBCheckCont.validate() == false) {
                return false;
            }
            if (TBCheckStan.validate() == false) {
                return false;
            }
           
            return true;
        }
    </script>
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
                            <ext:Button ID="btnSave" runat="server" Text="保存" Icon="Disk"
                                >
                                 <AjaxEvents>
                                                    <Click OnEvent="BtnSave_Click" >
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
                    <table width="100%">
                        <tr>
                            <td width="20%" align="right">
                                <p>
                                    <font face="宋体">指标</font><font face="宋体">名称：</font></p>
                            </td>
                            <td width="80%">
                                <asp:TextBox ID="TBGuideName" runat="server" ReadOnly="True" Width="300px"></asp:TextBox>
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
                            <td width="20%" align="right">
                                <font face="宋体">考评内容：</font>
                            </td>
                            <td width="80%">
                            </td>
                        </tr>
                        <tr>
                            <td width="20%">
                                &nbsp;&nbsp;
                            </td>
                            <td width="80%">
                            <ext:TextArea ID="TBCheckCont" runat="server" Width="300" Height="55"  AllowBlank="false"></ext:TextArea>
                               
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
                            <td width="20%" align="right">
                                <font face="宋体">考评标准：</font>
                            </td>
                            <td width="80%">
                            </td>
                        </tr>
                        <tr>
                            <td width="20%">
                                &nbsp;&nbsp;
                            </td>
                            <td width="80%">
                                <ext:TextArea ID="TBCheckStan" runat="server" Width="300" Height="55"  AllowBlank="false"></ext:TextArea>
                               
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
                            <td width="20%" align="right">
                                <font face="宋体">考评办法：</font>
                            </td>
                            <td width="80%">
                            </td>
                        </tr>
                        <tr>
                            <td width="20%">
                                &nbsp;&nbsp;
                            </td>
                            <td width="80%">
                               <ext:TextArea ID="TBCheckMeth" runat="server" Width="300" Height="55"  ></ext:TextArea>
                                
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
