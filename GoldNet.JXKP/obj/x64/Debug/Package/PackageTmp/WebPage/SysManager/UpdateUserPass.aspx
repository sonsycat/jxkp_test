<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="UpdateUserPass.aspx.cs"
    Inherits="GoldNet.JXKP.UpdateUserPass" %>

<%@ Register Assembly="Goldnet.Ext.Web" Namespace="Goldnet.Ext.Web" TagPrefix="ext" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>无标题页</title>
    <link rel="stylesheet" type="text/css" href="/resources/css/main.css" />
    <style type="text/css">
        body
        {
            background-color: white;
            font-size: 12px;
        }
        td strong
        {
            color: Red;
        }
    </style>

    <script type="text/javascript">
        var CheckForm = function() {
            if (showorder.validate() == false) {
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
        <ext:FormPanel ID="FormPanel1" runat="server" Border="false" AutoScroll="true" ButtonAlign="Right"
            StyleSpec="background-color:transparent" BodyStyle="background-color:transparent">
            <Body>
                <ext:Panel ID="Panel1" runat="server" Border="false" AutoHeight="true" AutoWidth="true"
                    StyleSpec="background-color:transparent" BodyStyle="background-color:transparent">
                    <%--  <TopBar>
                        <ext:Toolbar ID="Toolbar1" runat="server">
                            <Items>
                                <ext:Button ID="BtnSave" runat="server" Text="保存" Icon="Disk">
                                    <AjaxEvents>
                                        <Click OnEvent="save_Click" Before="if (CheckForm()== false){ Ext.Msg.show({title:'系统提示',msg:'请根据红线提示填写正确的信息！',icon: 'ext-mb-info',buttons: { ok: true }});return false;};">
                                        </Click>
                                    </AjaxEvents>
                                </ext:Button>
                                <ext:ToolbarSeparator ID="ToolbarSeparator1" runat="server">
                                </ext:ToolbarSeparator>
                            </Items>
                        </ext:Toolbar>
                    </TopBar>--%>
                    <Body>
                        <%--    <table width="100%" border="0" cellpadding="0" cellspacing="0">--%>
                        <%--<tr>
                                <td width="50%" valign="top">--%>
                        <br />
                        <table width="100%" border="0" cellpadding="0" cellspacing="10" style="text-align: left;">
                            <tr>
                                <td align="right">
                                    原密码：
                                </td>
                                <td align="left">
                                    <ext:TextField ID="txtYpass" runat="server" Width="180" InputType="Password" AllowBlank="false">
                                    </ext:TextField>
                                </td>
                            </tr>
                            <tr>
                                <td align="right">
                                    新密码：
                                </td>
                                <td align="left">
                                    <ext:TextField ID="txtNewpass" runat="server" InputType="Password" Width="180" AllowBlank="false">
                                    </ext:TextField>
                                </td>
                            </tr>
                            <tr>
                                <td align="right">
                                    重新输入新密码：
                                </td>
                                <td align="left">
                                    <ext:TextField ID="txtNewPassT" InputType="Password" runat="server" Width="180" AllowBlank="false">
                                    </ext:TextField>
                                    <br />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                </td>
                                <td align="left">
                                    <asp:Button ID="Button1" runat="server" Text="修改密码" OnClick="Button1_Click" />
                                </td>
                            </tr>
                        </table>
                        <%--  </td>
                            </tr>
                        </table>--%>
                    </Body>
                </ext:Panel>
            </Body>
        </ext:FormPanel>
    </div>
    </form>
</body>
</html>
