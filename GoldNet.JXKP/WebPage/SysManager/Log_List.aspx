<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Log_List.aspx.cs" Inherits="GoldNet.JXKP.WebPage.SysManager.Log_List" %>

<%@ Register Assembly="Goldnet.Ext.Web" Namespace="Goldnet.Ext.Web" TagPrefix="ext" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <ext:ScriptManager ID="ScriptManager1" runat="server" AjaxMethodNamespace="CompanyX" />
        <ext:Store ID="Store1" runat="server" WarningOnDirty="false">
            <Reader>
                <ext:JsonReader>
                    <Fields>
                        <ext:RecordField Name="BEGIN_DATE_TIME" Type="String" Mapping="BEGIN_DATE_TIME" />
                        <ext:RecordField Name="TASK_NAME" Type="String" Mapping="TASK_NAME" />
                    </Fields>
                </ext:JsonReader>
            </Reader>
        </ext:Store>
        <ext:Store ID="Store2" runat="server" WarningOnDirty="false">
            <Reader>
                <ext:JsonReader>
                    <Fields>
                        <ext:RecordField Name="BEGIN_DATE_TIME" Type="String" Mapping="BEGIN_DATE_TIME" />
                        <ext:RecordField Name="TASK_NAME" Type="String" Mapping="TASK_NAME" />
                        <ext:RecordField Name="TABLE_NAME" Type="String" Mapping="TABLE_NAME" />
                        <ext:RecordField Name="ERROR_MESSAGE" Type="String" Mapping="ERROR_MESSAGE" />
                    </Fields>
                </ext:JsonReader>
            </Reader>
        </ext:Store>
        <ext:ViewPort ID="ViewPort1" runat="server">
            <Body>
                <ext:BorderLayout ID="BorderLayout1" runat="server">
                    <North>
                        <ext:Toolbar ID="Toolbar1" runat="server">
                            <Items>
                                <ext:ComboBox ID="years" runat="server" Width="60" AllowBlank="true" EmptyText="请选择年..."
                                    FieldLabel="年">
                                </ext:ComboBox>
                                <ext:ToolbarTextItem ID="dd1Name" runat="server" Text="年 " />
                                <ext:ComboBox ID="months" runat="server" Width="60" AllowBlank="true" EmptyText="请选择月..."
                                    FieldLabel="月">
                                    <Items>
                                        <ext:ListItem Text="01" Value="01" />
                                        <ext:ListItem Text="02" Value="02" />
                                        <ext:ListItem Text="03" Value="03" />
                                        <ext:ListItem Text="04" Value="04" />
                                        <ext:ListItem Text="05" Value="05" />
                                        <ext:ListItem Text="06" Value="06" />
                                        <ext:ListItem Text="07" Value="07" />
                                        <ext:ListItem Text="08" Value="08" />
                                        <ext:ListItem Text="09" Value="09" />
                                        <ext:ListItem Text="10" Value="10" />
                                        <ext:ListItem Text="11" Value="11" />
                                        <ext:ListItem Text="12" Value="12" />
                                    </Items>
                                </ext:ComboBox>
                                <ext:ToolbarTextItem ID="ToolbarTextItem1" runat="server" Text="月 " />
                                <ext:ToolbarSeparator ID="ToolbarSeparator1" runat="server" />
                                <ext:Button ID="Buttond" runat="server" Text="查询" Icon="DatabaseGo">
                                    <AjaxEvents>
                                        <Click OnEvent="Button_click" Timeout="120000">
                                            <EventMask Msg="载入中..." ShowMask="true" />
                                        </Click>
                                    </AjaxEvents>
                                </ext:Button>
                            </Items>
                        </ext:Toolbar>
                    </North>
                    <Center>
                        <ext:GridPanel ID="GridPanel2" Title="WORK_LOG" runat="server" StoreID="Store1" StripeRows="true"
                            TrackMouseOver="true" Width="360" Height="100" AutoScroll="true" AutoExpandColumn="TASK_NAME">
                            <ColumnModel ID="ColumnModel2" runat="server">
                                <Columns>
                                    <ext:Column ColumnID="BEGIN_DATE_TIME" Header="<div style='text-align:center;'>时间</div>"
                                        Width="100" Align="left" Sortable="true" DataIndex="BEGIN_DATE_TIME" MenuDisabled="true" />
                                    <ext:Column ColumnID="TASK_NAME" Header="<div style='text-align:center;'>状态</div>"
                                        Width="300" Align="left" Sortable="true" DataIndex="TASK_NAME" MenuDisabled="true">
                                    </ext:Column>
                                </Columns>
                            </ColumnModel>
                            <SelectionModel>
                                <ext:RowSelectionModel ID="RowSelectionModel1" runat="server" SingleSelect="true">
                                </ext:RowSelectionModel>
                            </SelectionModel>
                            <BottomBar>
                                <ext:PagingToolbar ID="PagingToolBar2" runat="server" PageSize="100" StoreID="Store1"
                                    AutoWidth="true" DisplayInfo="false" AutoDataBind="true">
                                </ext:PagingToolbar>
                            </BottomBar>
                        </ext:GridPanel>
                    </Center>
                    <South>
                        <ext:GridPanel ID="GridPanel1" Title="EXEC_LOG" runat="server" StoreID="Store2" StripeRows="true"
                            BodyBorder="false" AutoDataBind="true" TrackMouseOver="true" Width="810" Height="300" AutoExpandColumn="ERROR_MESSAGE">
                            <ColumnModel ID="ColumnModel1" runat="server">
                                <Columns>
                                    <ext:Column ColumnID="BEGIN_DATE_TIME" Header="<div style='text-align:center;'>时间</div>"
                                        Width="100" Align="left" Sortable="false" DataIndex="BEGIN_DATE_TIME" MenuDisabled="true" />
                                    <ext:Column ColumnID="TASK_NAME" Header="<div style='text-align:center;'>任务名称</div>"
                                        Width="200" Align="left" Sortable="false" DataIndex="TASK_NAME" MenuDisabled="true" />
                                    <ext:Column ColumnID="TABLE_NAME" Header="<div style='text-align:center;'>表名</div>"
                                        Width="200" Align="left" Sortable="false" DataIndex="TABLE_NAME" MenuDisabled="true" />
                                    <ext:Column ColumnID="ERROR_MESSAGE" Header="<div style='text-align:center;'>错误信息</div>"
                                        Width="300" Align="left" Sortable="false" DataIndex="ERROR_MESSAGE" MenuDisabled="true" />
                                </Columns>
                            </ColumnModel>
                            <SelectionModel>
                                <ext:RowSelectionModel ID="RowSelectionModel3" runat="server" SingleSelect="true">
                                </ext:RowSelectionModel>
                            </SelectionModel>
                            <BottomBar>
                                <ext:PagingToolbar ID="PagingToolBar1" runat="server" PageSize="100" StoreID="Store2"
                                    AutoWidth="true" DisplayInfo="false" AutoDataBind="true">
                                </ext:PagingToolbar>
                            </BottomBar>
                        </ext:GridPanel>
                    </South>
                </ext:BorderLayout>
            </Body>
        </ext:ViewPort>
    </div>
    </form>
</body>
</html>
