<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FunctionSearch.aspx.cs"
    Inherits="GoldNet.JXKP.WebPage.SysManager.FunctionSearch" %>

<%@ Register Assembly="Goldnet.Ext.Web" Namespace="Goldnet.Ext.Web" TagPrefix="ext" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>无标题页</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <ext:ScriptManager ID="ScriptManager1" runat="server" AjaxMethodNamespace="CompanyX" />
        <ext:Store ID="Store1" runat="server">
            <Reader>
                <ext:JsonReader ReaderID="ROLE_NAME">
                    <Fields>
                        <ext:RecordField Name="USER_NAME" />
                        <ext:RecordField Name="ROLE_NAME" />
                    </Fields>
                </ext:JsonReader>
            </Reader>
        </ext:Store>
        <ext:ViewPort ID="ViewPort1" runat="server">
            <Body>
                <ext:ColumnLayout ID="ColumnLayout1" runat="server" Split="true" FitHeight="true">
                    <Columns>
                        <ext:LayoutColumn ColumnWidth="1">
                            <ext:GridPanel ID="GridPanel1" runat="server" StoreID="Store1" StripeRows="true"
                                TrackMouseOver="true" Height="480">
                                <TopBar>
                                    <ext:Toolbar ID="Toolbar_ZLJK" runat="server" Visible="true" AutoWidth="true">
                                        <Items>
                                            <ext:Label ID="func" runat="server" Text="选择功能：" Width="40">
                                            </ext:Label>
                                            <ext:ComboBox ID="Combo_Func" runat="server" AllowBlank="true" EmptyText="请选择功能"
                                                Width="300" FieldLabel="功能选择">
                                                <AjaxEvents>
                                                    <Select OnEvent="SelectedFunc">
                                                        <EventMask ShowMask="true" />
                                                    </Select>
                                                </AjaxEvents>
                                            </ext:ComboBox>
                                            <ext:Button ID="Button1" runat="server" Text="查询" Icon="Zoom">
                                                <AjaxEvents>
                                                    <Click OnEvent="SelectFunc">
                                                        <EventMask Msg="载入中..." ShowMask="true" />
                                                    </Click>
                                                </AjaxEvents>
                                            </ext:Button>
                                            <ext:ToolbarSeparator ID="ToolbarSeparator4" runat="server" />
                                        </Items>
                                    </ext:Toolbar>
                                </TopBar>
                                <ColumnModel ID="ColumnModel1" runat="server">
                                    <Columns>
                                        <ext:Column Header="姓名" Width="200" Align="left" Sortable="true" MenuDisabled="true"
                                            ColumnID="USER_NAME" DataIndex="USER_NAME">
                                        </ext:Column>
                                        <ext:Column Header="角色名称" Width="120" Align="left" Sortable="true" MenuDisabled="true"
                                            ColumnID="ROLE_NAME" DataIndex="ROLE_NAME">
                                        </ext:Column>
                                    </Columns>
                                </ColumnModel>
                                <SelectionModel>
                                    <ext:RowSelectionModel ID="rowselection" SingleSelect="true">
                                    </ext:RowSelectionModel>
                                </SelectionModel>
                                <LoadMask ShowMask="true" />
                            </ext:GridPanel>
                        </ext:LayoutColumn>
                    </Columns>
                </ext:ColumnLayout>
            </Body>
        </ext:ViewPort>
    </div>
    </form>
</body>
</html>
