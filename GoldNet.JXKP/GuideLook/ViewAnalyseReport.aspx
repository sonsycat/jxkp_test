<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ViewAnalyseReport.aspx.cs"
    Inherits="GoldNet.JXKP.GuideLook.ViewAnalyseReport" %>

<%@ Register Assembly="Goldnet.Ext.Web" Namespace="Goldnet.Ext.Web" TagPrefix="ext" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>无标题页</title>

    <script type="text/javascript">                    
      var TreeNodeButton = function(node) {
         if(node.hasChildNodes() || node.parentNode.id == 'root') {
            Button1.setDisabled(true);
         } else {
            Button1.setDisabled(false);
         }
     } 
    </script>

</head>
<body>
    <form id="form1" runat="server">
    <ext:ScriptManager ID="ScriptManager1" runat="server" />
    <ext:Store runat="server" ID="Store1">
        <Reader>
            <ext:JsonReader>
                <Fields>
                    <ext:RecordField Name="GUIDE_CODE" />
                    <ext:RecordField Name="GUIDE_NAME" />
                    <ext:RecordField Name="UNIT_CODE" />
                    <ext:RecordField Name="UNIT_NAME" />
                    <ext:RecordField Name="GUIDE_VALUE" />
                    <ext:RecordField Name="GUIDE_VALUE_YEAR" />
                    <ext:RecordField Name="GUIDE_CAUSE" />
                    <ext:RecordField Name="GUIDE_FACT" />
                    <ext:RecordField Name="GUIDE_RATIO_TONG" />
                    <ext:RecordField Name="GUIDE_VALUE_TONG" />
                    <ext:RecordField Name="GUIDE_RATIO_HUAN" />
                    <ext:RecordField Name="GUIDE_VALUE_HUAN" />
                    <ext:RecordField Name="GUIDE_WCB_MONTH" />
                    <ext:RecordField Name="GUIDE_WCB_YEAR" />
                    <ext:RecordField Name="ORGAN" />
                </Fields>
            </ext:JsonReader>
        </Reader>
    </ext:Store>
    <div>
        <ext:ViewPort ID="ViewPort1" runat="server">
            <Body>
                <ext:BorderLayout ID="BorderLayout2" runat="server">
                    <North>
                        <ext:Toolbar runat="server" ID="ctl155" StyleSpec="border:0">
                            <Items>
                                <ext:ToolbarSpacer ID="ToolbarSpacer2" runat="server" Width="10" />
                                <ext:ComboBox runat="server" ID="Comb_StartYear" Width="60" ListWidth="60" SelectedIndex="0">
                                </ext:ComboBox>
                                <ext:ToolbarTextItem ID="ToolbarTextItem2" runat="server" Text="年" />
                                <ext:ComboBox runat="server" ID="Comb_StartMonth" Width="40" ListWidth="40" SelectedIndex="0">
                                </ext:ComboBox>
                                <ext:ToolbarTextItem ID="ToolbarTextItem3" runat="server" Text="月" />
                                <ext:ToolbarSeparator ID="ToolbarSeparator3" runat="server" />
                                <ext:Button ID="Button1" runat="server" Text=" 查询 " Icon="DatabaseGo" Disabled="true">
                                    <AjaxEvents>
                                        <Click OnEvent="GetQueryPortalet">
                                            <EventMask Msg="载入中..." ShowMask="true" />
                                            <ExtraParams>
                                                <ext:Parameter Name="NodeId" Value="Ext.encode(#{TreeCtrl}.getSelectionModel().getSelectedNode().id)"
                                                    Mode="Raw">
                                                </ext:Parameter>
                                                <ext:Parameter Name="NodeName" Value="Ext.encode(#{TreeCtrl}.getSelectionModel().getSelectedNode().text)"
                                                    Mode="Raw">
                                                </ext:Parameter>
                                            </ExtraParams>
                                            <EventMask ShowMask="true" Msg="请稍候..." />
                                        </Click>
                                    </AjaxEvents>
                                </ext:Button>
                                <ext:ToolbarSeparator ID="ToolbarSeparator4" runat="server" />
                            </Items>
                        </ext:Toolbar>
                    </North>
                    <Center>
                        <ext:GridPanel ID="GridPanel_Show" runat="server" Border="false" StoreID="Store1"
                            Height="800" Title="分析报表信息" StripeRows="true" MonitorResize="true" MonitorWindowResize="true"
                            AutoExpandColumn="GUIDE_NAME">
                            <ColumnModel ID="ColumnModel1" runat="server">
                                <Columns>
                                    <ext:RowNumbererColumn Width="32" Resizable="true">
                                    </ext:RowNumbererColumn>
                                    <ext:Column Header="指标名称" Width="150" Align="Left" Sortable="false" MenuDisabled="true"
                                        ColumnID="GUIDE_NAME" DataIndex="GUIDE_NAME" />
                                    <ext:Column Header="指标值" Width="90" Align="Center" Sortable="false" MenuDisabled="true"
                                        ColumnID="GUIDE_VALUE" DataIndex="GUIDE_VALUE" />
                                    <ext:Column Header="年度目标值" Width="90" Align="Center" Sortable="false" MenuDisabled="true"
                                        ColumnID="GUIDE_VALUE_YEAR" DataIndex="GUIDE_VALUE_YEAR" />
                                    <ext:Column Header="<center>同期</center>" Width="50" Align="Left" Sortable="false"
                                        MenuDisabled="true" ColumnID="GUIDE_VALUE_TONG" DataIndex="GUIDE_VALUE_TONG" />
                                    <ext:Column Header="<center>同比</center>" Width="50" Align="Left" Sortable="false"
                                        MenuDisabled="true" ColumnID="GUIDE_RATIO_TONG" DataIndex="GUIDE_RATIO_TONG" />
                                    <ext:Column Header="<center>上月</center>" Width="50" Align="Left" Sortable="false"
                                        MenuDisabled="true" ColumnID="GUIDE_VALUE_HUAN" DataIndex="GUIDE_VALUE_HUAN" />
                                    <ext:Column Header="<center>环比</center>" Width="50" Align="Left" Sortable="false"
                                        MenuDisabled="true" ColumnID="GUIDE_RATIO_HUAN" DataIndex="GUIDE_RATIO_HUAN" />
                                    <ext:Column Header="<center>月份完成比</center>" Width="90" Align="Left" Sortable="false"
                                        MenuDisabled="true" ColumnID="GUIDE_VALUE_HUAN" DataIndex="GUIDE_VALUE_HUAN" />
                                    <ext:Column Header="<center>年度完成比</center>" Width="90" Align="Left" Sortable="false"
                                        MenuDisabled="true" ColumnID="GUIDE_RATIO_HUAN" DataIndex="GUIDE_RATIO_HUAN" />
                                    <ext:Column Header="<center>对象</center>" Width="50" Align="Center" Sortable="false"
                                        MenuDisabled="true" ColumnID="UNIT_NAME" DataIndex="UNIT_NAME" />
                                </Columns>
                            </ColumnModel>
                            <SelectionModel>
                                <ext:RowSelectionModel SingleSelect="true">
                                </ext:RowSelectionModel>
                            </SelectionModel>
                            <LoadMask ShowMask="true" />
                        </ext:GridPanel>
                    </Center>
                    <West CollapseMode="Mini" Split="false" Collapsible="false">
                        <ext:Panel ID="Panel1" runat="server" Width="200" BodyBorder="false" Title="请选择报表"
                            AutoScroll="true" Border="false">
                            <Body>
                                <ext:TreePanel runat="server" Width="200" ID="TreeCtrl" AutoHeight="true" AutoScroll="false"
                                    Border="false">
                                    <Listeners>
                                        <Click Handler="TreeNodeButton(node)" />
                                    </Listeners>
                                </ext:TreePanel>
                            </Body>
                        </ext:Panel>
                    </West>
                </ext:BorderLayout>
            </Body>
        </ext:ViewPort>
    </div>
    </form>
</body>
</html>
