<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="guide_relation.aspx.cs"
    Inherits="GoldNet.JXKP.mainpage.guide_relation" %>

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
                    <ext:RecordField Name="GUIDE_CODE" />
                    <ext:RecordField Name="GUIDE_NAME" />
                    <ext:RecordField Name="GUIDE_VALUE" />
                </Fields>
            </ext:JsonReader>
        </Reader>
    </ext:Store>
    <ext:Hidden runat="server" ID="GuideCodeHidden"></ext:Hidden>
    <div>
        <ext:ViewPort ID="ViewPort1" runat="server">
            <Body>
                <ext:ColumnLayout ID="ColumnLayout1" runat="server" Split="true" FitHeight="true">
                    <Columns>
                        <ext:LayoutColumn ColumnWidth="1">
                            <ext:GridPanel ID="GridPanel_Show" runat="server" StoreID="Store1" Border="false"
                                AutoWidth="true" AutoHeight="true" Header="false" Title="相关性分析" MonitorResize="true"
                                MonitorWindowResize="true" StripeRows="true" TrackMouseOver="true" AutoExpandColumn="GUIDE_NAME" AutoScroll="true">
                                <TopBar>
                                    <ext:Toolbar ID="Toolbar_ZLJK" runat="server" Visible="true">
                                        <Items>
                                            <ext:ToolbarFill runat="server" ID="ToolbarFill1">
                                            </ext:ToolbarFill>
                                            <ext:Button ID="btnQeury" runat="server" Text="查询" Icon="DatabaseGo" Disabled="true">
                                                <AjaxEvents>
                                                    <Click OnEvent="GetQueryPortalet">
                                                        <ExtraParams>
                                                            <ext:Parameter Name="Values" Value="Ext.encode(#{GridPanel_Show}.getRowsValues())" Mode="Raw"></ext:Parameter>
                                                        </ExtraParams>
                                                        <EventMask Msg="载入中..." ShowMask="true" />
                                                    </Click>
                                                </AjaxEvents>
                                            </ext:Button>
                                            <ext:ToolbarSeparator ID="ToolbarSeparator4" runat="server" />
                                            <ext:Button ID="BtnExcel" runat="server" Text="返回" Icon="PageExcel">
                                                <AjaxEvents>
                                                    <Click OnEvent="GetBackPortalet"></Click>
                                                </AjaxEvents>
                                            </ext:Button>
                                        </Items>
                                    </ext:Toolbar>
                                </TopBar>
                                <ColumnModel ID="ColumnModel1" runat="server">
                                    <Columns>
                                        <ext:RowNumbererColumn>
                                        </ext:RowNumbererColumn>
                                        <ext:Column Header="关联指标编码" Width="90" Align="Left" Sortable="false" MenuDisabled="true"
                                            ColumnID="GUIDE_CODE" DataIndex="GUIDE_CODE" />
                                        <ext:Column Header="指标名称" Width="90" Align="Center" Sortable="false" MenuDisabled="true"
                                            ColumnID="GUIDE_NAME" DataIndex="GUIDE_NAME" />
                                        <ext:Column Header="指标值" Width="90" Align="Center" Sortable="false" MenuDisabled="true"
                                            ColumnID="GUIDE_VALUE" DataIndex="GUIDE_VALUE" />
                                    </Columns>
                                </ColumnModel>
                                <SelectionModel>
                                    <ext:RowSelectionModel ID="rowselection" SingleSelect="true">
                                        <Listeners>
                                            <RowSelect  Handler="#{btnQeury}.enable();"/>
                                            <RowDeselect  Handler="#{btnQeury}.disable();"/>
                                        </Listeners>
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
