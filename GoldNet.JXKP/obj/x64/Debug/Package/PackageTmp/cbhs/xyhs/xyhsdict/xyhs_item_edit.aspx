<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="xyhs_item_edit.aspx.cs"
    Inherits="GoldNet.JXKP.cbhs.xyhs.xyhsdict.xyhs_item_edit" %>

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
        <ext:FormPanel ID="FormPanel1" runat="server" Border="false" AutoHeight="true" AutoWidth="true"
            AutoScroll="true" ButtonAlign="Right" StyleSpec="background-color:transparent"
            BodyStyle="background-color:transparent">
            <Body>
                <ext:Panel ID="Panel1" runat="server" Border="false" AutoHeight="true" AutoWidth="true"
                    StyleSpec="background-color:transparent" BodyStyle="background-color:transparent">
                    <TopBar>
                        <ext:Toolbar ID="Toolbar1" runat="server">
                            <Items>
                                <ext:ToolbarSeparator ID="ToolbarSeparator1" runat="server">
                                </ext:ToolbarSeparator>
                                <ext:Button ID="save" runat="server" Text="保存" Icon="Disk">
                                    <AjaxEvents>
                                        <Click OnEvent="Buttonsave_Click">
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
                <ext:FormLayout ID="FormLayout1" runat="server">
                    <ext:Anchor>
                        <ext:ComboBox ID="Combo_ItemType" runat="server" AllowBlank="false" Width="200" EmptyText="请选择类别"
                            FieldLabel="成本类别">
                        </ext:ComboBox>
                    </ext:Anchor>
                    <ext:Anchor>
                        <ext:TextField ID="Text_ItemCode" runat="server" DataIndex="rolename" MsgTarget="Side"
                            AllowBlank="false" FieldLabel="成本代码" Width="200" />
                    </ext:Anchor>
                    <ext:Anchor>
                        <ext:TextField ID="Text_ItemName" runat="server" DataIndex="rolename" MsgTarget="Side"
                            AllowBlank="false" FieldLabel="成本名称" Width="200" />
                    </ext:Anchor>
                    <ext:Anchor>
                        <ext:TextField ID="Text_FINANCE_ITEM" runat="server" DataIndex="rolename" MsgTarget="Side"
                            AllowBlank="false" FieldLabel="医疗业务代码" Width="200" />
                    </ext:Anchor>
                    <ext:Anchor>
                        <ext:TextField ID="Text_FINANCE_ITEM_GL" runat="server" DataIndex="rolename" MsgTarget="Side"
                            AllowBlank="false" FieldLabel="管理费用代码" Width="200" />
                    </ext:Anchor>
                </ext:FormLayout>
            </Body>
        </ext:FormPanel>
    </div>
    </form>
</body>
</html>
