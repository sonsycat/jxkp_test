<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="xyhs_incomes_dict_edit.aspx.cs" Inherits="GoldNet.JXKP.cbhs.xyhs.xyhs_incomes_dict_edit" %>
<%@ Register Assembly="Goldnet.Ext.Web" Namespace="Goldnet.Ext.Web" TagPrefix="ext" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
     <title></title>
      <style type="text/css">
        body{
         background-color: #DFE8F6;
         font-size:12px;
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
                        <ext:TextField ID="Text_ItemName" runat="server" DataIndex="rolename" MsgTarget="Side"
                            AllowBlank="false" FieldLabel="项目名称" Width="200" ReadOnly="true" />
                    </ext:Anchor>
                    <ext:Anchor>
                        <ext:ComboBox ID="Combo_ItemType" runat="server" AllowBlank="false" Width="200" EmptyText="请选择是否核算"
                            FieldLabel="是否核算">
                           <Items>
                           <ext:ListItem Text="是" Value="1"/>
                           <ext:ListItem Text="否"  Value="0"/>
                           </Items>
                        </ext:ComboBox>
                    </ext:Anchor>
                    <ext:Anchor>
                       <ext:NumberField ID="ratio" runat="server" DataIndex="ratio"
                                                    AllowBlank="false" Width="200" EnableKeyEvents="true" MaxValue="100" MinValue="0"
                                                    StyleSpec="text-align:right" SelectOnFocus="true" DecimalPrecision="2"  FieldLabel="核算系数"/>
                    </ext:Anchor>
                </ext:FormLayout>
            </Body>
        </ext:FormPanel>
    </div>
    </form>
</body>
</html>
