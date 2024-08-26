<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="main_zljk.aspx.cs" Inherits="GoldNet.JXKP.mainpage.main_zljk" %>

<%@ Register Assembly="Goldnet.Ext.Web" Namespace="Goldnet.Ext.Web" TagPrefix="ext" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <ext:ScriptManager ID="ScriptManager1" runat="server">
    </ext:ScriptManager>
    <ext:Store ID="Store1" runat="server">
        <Reader>
            <ext:JsonReader>
                <Fields>
                    <ext:RecordField Name="TEMPLET_ID" />
                    <ext:RecordField Name="TABLE_ID" />
                    <ext:RecordField Name="GUIDE_CONTENT" />
                    <ext:RecordField Name="DUTY_DEPT_NAME" />
                    <ext:RecordField Name="DUTY_USER_NAME" />
                    <ext:RecordField Name="DATE_TIME" />
                </Fields>
            </ext:JsonReader>
        </Reader>
    </ext:Store>
    <div>
        <ext:ViewPort ID="ViewPort1" runat="server">
            <Body>
                <ext:ColumnLayout ID="ColumnLayout1" runat="server" Split="true" FitHeight="true">
                    <Columns>
                        <ext:LayoutColumn ColumnWidth="1">
                            <ext:GridPanel ID="GridPanel_Show" runat="server" StoreID="Store1" Border="false"
                                AutoWidth="true" Title="" MonitorResize="true" MonitorWindowResize="true" StripeRows="true"
                                TrackMouseOver="true" AutoExpandColumn="GUIDE_CONTENT">
                                <TopBar>
                                    <ext:Toolbar ID="Toolbar_ZLJK" runat="server" Visible="true">
                                        <Items>
                                            <ext:ComboBox runat="server" ID="pflg" Width="70" SelectedIndex="0">
                                                <Items>
                                                    <ext:ListItem Text="未解决" Value="0" />
                                                    <ext:ListItem Text="已解决" Value="1" />
                                                </Items>
                                            </ext:ComboBox>
                                            <ext:ToolbarSpacer ID="ToolbarSpacer2" runat="server" Width="10" />
                                            <ext:ComboBox runat="server" ID="Comb_StartYear" Width="60" ListWidth="60" SelectedIndex="0">
                                            </ext:ComboBox>
                                            <ext:ToolbarTextItem ID="ToolbarTextItem2" runat="server" Text="年" />
                                            <ext:ComboBox runat="server" ID="Comb_StartMonth" Width="40" ListWidth="40" SelectedIndex="0">
                                            </ext:ComboBox>
                                            <ext:ToolbarTextItem ID="ToolbarTextItem7" runat="server" Text="月   至   " />
                                            <ext:ToolbarSpacer ID="ToolbarSpacer1" runat="server" Width="6" />
                                            <ext:ComboBox runat="server" ID="Comb_EndYear" Width="60" ListWidth="60" SelectedIndex="0">
                                            </ext:ComboBox>
                                            <ext:ToolbarTextItem ID="ToolbarTextItem4" runat="server" Text="年" />
                                            <ext:ComboBox runat="server" ID="Comb_EndMonth" Width="40" ListWidth="40" SelectedIndex="0">
                                            </ext:ComboBox>
                                            <ext:ToolbarTextItem ID="ToolbarTextItem5" runat="server" Text="月" />
                                            <ext:ToolbarSeparator ID="ToolbarSeparator3" runat="server" />
                                            <ext:Button ID="Button1" runat="server" Text="查询" Icon="DatabaseGo">
                                                <AjaxEvents>
                                                    <Click OnEvent="GetQueryPortalet">
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
                                        <ext:RowNumbererColumn>
                                        </ext:RowNumbererColumn>
                                        <ext:Column Header="事项" Width="66" Align="Left" Sortable="false" MenuDisabled="true"
                                            ColumnID="GUIDE_CONTENT" DataIndex="GUIDE_CONTENT" />
                                        <ext:Column Header="科室" Width="90" Align="Center" Sortable="false" MenuDisabled="true"
                                            ColumnID="DUTY_DEPT_NAME" DataIndex="DUTY_DEPT_NAME" />
                                        <ext:Column Header="责任人" Width="60" Align="Center" Sortable="false" MenuDisabled="true"
                                            ColumnID="DUTY_USER_NAME" DataIndex="DUTY_USER_NAME" />
                                        <ext:Column Header="日期" Width="80" Align="Center" Sortable="false" MenuDisabled="true"
                                            ColumnID="DATE_TIME" DataIndex="DATE_TIME" />
                                        <ext:CommandColumn Width="30" Align="Center">
                                            <Commands>
                                                <ext:GridCommand Icon="Zoom" CommandName="DetailView">
                                                    <ToolTip Text="查看详情" />
                                                </ext:GridCommand>
                                            </Commands>
                                        </ext:CommandColumn>
                                    </Columns>
                                </ColumnModel>
                                <SelectionModel>
                                    <ext:RowSelectionModel ID="rowselection" SingleSelect="true" />
                                </SelectionModel>
                                <Listeners>
                                    <Command Handler="  parent.window.viewzljk(record.data.TABLE_ID, record.data.TEMPLET_ID);" />
                                </Listeners>
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
