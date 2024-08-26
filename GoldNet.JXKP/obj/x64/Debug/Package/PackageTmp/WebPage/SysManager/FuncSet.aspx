<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FuncSet.aspx.cs" Inherits="GoldNet.JXKP.WebPage.SysManager.FuncSet" %>
<%@ Register Assembly="Goldnet.Ext.Web" Namespace="Goldnet.Ext.Web" TagPrefix="ext" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
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
                <ext:ComboBox ID="Combo_Functype" runat="server" AllowBlank="false" Width="130" EmptyText="请选择项目"
                    FieldLabel="项目">
                     <AjaxEvents>
                                            <Select OnEvent="SelectedFuncType">
                                             <EventMask ShowMask="true"  />
                                            </Select>
                                        </AjaxEvents>
                </ext:ComboBox>
            </ext:Anchor>
            <ext:Anchor>
                <ext:ComboBox ID="Combo_Powertype" runat="server" AllowBlank="false" Width="130" EmptyText="请选择"
                    FieldLabel="是否单独授权">
                   
                </ext:ComboBox>
            </ext:Anchor>
            <ext:Anchor>
                <ext:ComboBox ID="Combo_Role" runat="server" AllowBlank="false" Width="130" EmptyText="请选择角色"
                    FieldLabel="单独授权角色">
                </ext:ComboBox>
            </ext:Anchor>
            
        </ext:FormLayout>
            </body>
            </ext:FormPanel>
    </div>
    </form>
</body>
</html>
