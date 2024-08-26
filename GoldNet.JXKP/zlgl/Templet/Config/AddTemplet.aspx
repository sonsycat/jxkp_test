<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AddTemplet.aspx.cs" Inherits="GoldNet.JXKP.zlgl.Templet.Config.AddTemplet" %>

<%@ Register Assembly="Goldnet.Ext.Web" Namespace="Goldnet.Ext.Web" TagPrefix="ext" %>
<%@ Register TagPrefix="uc1" TagName="title" Src="title.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>无标题页</title>
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
                                        <Click OnEvent="save_Click" Before="if (CheckForm()== false){ Ext.Msg.show({title:'系统提示',msg:'请根据红线提示填写正确的信息！',icon: 'ext-mb-info',buttons: { ok: true }});return false;};">
                                        </Click>
                                    </AjaxEvents>
                                </ext:Button>
                                <ext:ToolbarSeparator ID="ToolbarSeparator1" runat="server">
                                </ext:ToolbarSeparator>
                                <ext:Button ID="CancelButton" runat="server" Text="返回" Icon="ArrowUndo">
                                    <Listeners>
                                        <Click Handler="parent.templetname.hide();" />
                                    </Listeners>
                                </ext:Button>
                            </Items>
                        </ext:Toolbar>
                    </TopBar>
                    <Body>
                        <table width="100%" border="0" cellpadding="0" cellspacing="0">
                            <tr>
                                <td width="50%" valign="top">
                                    <table width="100%" border="0" cellpadding="0" cellspacing="0">
                                        <tr>
                                            <td width="50%">
                                                名称用于唯一标识一个模板。
                                            </td>
                                            <td>
                                                名称：<br>
                                                &nbsp;
                                                <ext:TextField ID="textTempletName" runat="server" Width="180" AllowBlank="false">
                                                </ext:TextField>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td width="50%">
                                                标题显示于模板上方。
                                            </td>
                                            <td>
                                                标题：<br>
                                                &nbsp;
                                                <ext:TextField ID="textTempletTitle" runat="server" Width="180" AllowBlank="false">
                                                </ext:TextField>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td width="50%">
                                                模版在菜单的显示顺序。
                                            </td>
                                            <td onpaste="return false">
                                                显示顺序：<br>
                                                &nbsp;
                                                <ext:NumberField ID="showorder" runat="server" Width="180" AllowBlank="false">
                                                </ext:NumberField>
                                                <br>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td width="50%">
                                                是否用于生成质量数据。
                                            </td>
                                            <td onpaste="return false">
                                                是否生成汇总数据：
                                                 <ext:ComboBox ID="iscount" runat="server" ReadOnly="true" ForceSelection="true" SelectOnFocus="true" Width="60" SelectedIndex="0">
                                            <Items>
                                                    <ext:ListItem Text="是" Value="0"/>
                                                    <ext:ListItem Text="否" Value="1" />
                                                   
                                                </Items>
                                            </ext:ComboBox>
                                                <br>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td width="50%">
                                                说明将显示在模板列表的底部。
                                            </td>
                                            <td>
                                                说明：<br>
                                                &nbsp;
                                                <asp:TextBox ID="textTempletCommon" runat="server" TextMode="MultiLine" Rows="5"
                                                    MaxLength="200"></asp:TextBox><br>
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
