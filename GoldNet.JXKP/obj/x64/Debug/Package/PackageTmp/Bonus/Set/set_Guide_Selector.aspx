<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="set_Guide_Selector.aspx.cs"
    Inherits="GoldNet.JXKP.Bonus.Set.set_Guide_Selector" %>

<%@ Register Assembly="Goldnet.Ext.Web" Namespace="Goldnet.Ext.Web" TagPrefix="ext" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style type="text/css">
    .icon-expand-all  { background-image: url(/resources/images/expand-all.gif) !important; }
    .icon-collapse-all  { background-image: url(/resources/images/collapse-all.gif) !important; } 
    </style>
    <script type="text/javascript">
        var SelectorLayout = function() {
            SelectorLeft.setHeight(Ext.lib.Dom.getViewHeight() - SelectorLeft.getPosition()[1] - 10);
            SelectorRight.setHeight(Ext.lib.Dom.getViewHeight() - SelectorRight.getPosition()[1] - 10);
        }
        var TwoSideSelector = {
            add: function(source, destination) {
                source = source || SelectorLeft;
                destination = destination || SelectorRight;
                var qz = '(' + QZNum.getValue() + ')';
                var selectionsArray = source.view.getSelectedIndexes();
                var records = [];
                if (selectionsArray.length > 0) {
                    for (var i = 0; i < selectionsArray.length; i++) {
                        var rec = source.view.store.getAt(selectionsArray[i]);
                        rec.data.GUIDE_NAME_QZ = rec.data.GUIDE_NAME + qz;
                        destination.store.add(rec);
                        records.push(rec);
                    }
                    for (var i = 0; i < selectionsArray.length; i++) {
                        source.store.remove(records[i]);
                    }
                }
            },
            addAll: function(source, destination) {
                source = source || SelectorLeft;
                destination = destination || SelectorRight;
                var qz = '(' + QZNum.getValue() + ')';
                var records = source.store.getRange();
                if (records.length > 0) {
                    for (var i = 0; i < records.length; i++) {
                        records[i].data.GUIDE_NAME_QZ = records[i].data.GUIDE_NAME + qz;
                    }
                }
                destination.store.add(records);
                source.store.removeAll();
            },
            remove: function() {
                var source = SelectorLeft;
                var destination = SelectorRight;
                this.add(destination, source);
            },
            removeAll: function() {
                var source = SelectorLeft;
                var destination = SelectorRight;
                this.addAll(destination, source);
            }
        };
        var dragToRight = function(ddView, n, dd, e, data) {
            if (dd.ddGroup == "grp1") {
                var qz = '(' + QZNum.getValue() + ')';
                if (data.records.length > 0) {
                    for (var i = 0; i < data.records.length; i++) {
                        data.records[i].data.GUIDE_NAME_QZ = data.records[i].data.GUIDE_NAME + qz;
                    }
                }
            }
        };
        var dblSelectLeft = function(vw, index, node, e) {
            var qz = '(' + QZNum.getValue() + ')';
            vw.store.data.items[index].data.GUIDE_NAME_QZ = vw.store.data.items[index].data.GUIDE_NAME + qz;
        };        


    </script>
</head>
<body>
    <form id="form1" runat="server">
    <ext:ScriptManager ID="ScriptManager1" runat="server">
        <Listeners>
            <DocumentReady Handler=" Ext.EventManager.onWindowResize(SelectorLayout); ;" />
        </Listeners>
    </ext:ScriptManager>
    <ext:Store runat="server" ID="Store1">
        <Reader>
            <ext:JsonReader>
                <Fields>
                    <ext:RecordField Name="GUIDE_NAME" />
                    <ext:RecordField Name="GUIDE_CODE" />
                    <ext:RecordField Name="GUIDE_NAME_QZ" />
                </Fields>
            </ext:JsonReader>
        </Reader>
    </ext:Store>
    <ext:Store runat="server" ID="Store2">
        <Reader>
            <ext:JsonReader>
                <Fields>
                    <ext:RecordField Name="GUIDE_NAME" />
                    <ext:RecordField Name="GUIDE_CODE" />
                    <ext:RecordField Name="GUIDE_NAME_QZ" />
                </Fields>
            </ext:JsonReader>
        </Reader>
    </ext:Store>
    <div>
        <ext:ViewPort ID="ViewPort1" runat="server">
            <Body>
                <ext:ColumnLayout ID="ColumnLayout2" runat="server" Split="true" FitHeight="true">
                    <Columns>
                        <ext:LayoutColumn ColumnWidth="1">
                            <ext:Panel ID="Panel11" runat="server" Width="400" Height="300" BodyBorder="false">
                                <TopBar>
                                    <ext:Toolbar runat="server" ID="ctl155">
                                        <Items>
                                            <ext:Button ID="save" runat="server" Icon="Disk" Text="保存">
                                                <AjaxEvents>
                                                    <Click OnEvent="SaveGuide">
                                                        <EventMask Msg="请稍候..." ShowMask="true" />
                                                        <ExtraParams>
                                                            <ext:Parameter Name="multi2" Value="Ext.encode(#{SelectorRight}.getValues(true))"
                                                                Mode="Raw" />
                                                        </ExtraParams>
                                                    </Click>
                                                </AjaxEvents>
                                            </ext:Button>
                                            <ext:ToolbarSeparator ID="ToolbarSeparator6" runat="server">
                                            </ext:ToolbarSeparator>
                                        </Items>
                                    </ext:Toolbar>
                                </TopBar>
                                <Body>
                                    <ext:ColumnLayout ID="ColumnLayout1" runat="server" FitHeight="true">
                                       
                                        <ext:LayoutColumn ColumnWidth="0.5">
                                            <ext:Panel ID="Panel5" runat="server" Border="false" MonitorResize="true">
                                                <Body>
                                                    <ext:MultiSelect ID="SelectorLeft" runat="server" Legend="待选指标" DragGroup="grp1"
                                                        DropGroup="grp2,grp1" StoreID="Store1" DisplayField="GUIDE_NAME" ValueField="GUIDE_CODE"
                                                        EnableViewState="true" Stateful="true" AutoWidth="true" Height="250" KeepSelectionOnClick="WithCtrlKey"
                                                        StyleSpec="margin:5px; ">
                                                        <Listeners>
                                                            <Render Handler=" this.setHeight(Ext.lib.Dom.getViewHeight() - this.getPosition()[1] );" />
                                                            <DblClick Fn="dblSelectLeft" />
                                                        </Listeners>
                                                    </ext:MultiSelect>
                                                </Body>
                                            </ext:Panel>
                                        </ext:LayoutColumn>
                                        <ext:LayoutColumn>
                                            <ext:Panel ID="Panel2" runat="server" Width="35" BodyStyle="background-color: transparent;"
                                                Border="false">
                                                <Body>
                                                    <ext:AnchorLayout ID="AnchorLayout1" runat="server">
                                                        <ext:Anchor Vertical="20%">
                                                            <ext:Panel ID="Panel1" runat="server" Border="false" BodyStyle="background-color: transparent;" />
                                                        </ext:Anchor>
                                                        <ext:Anchor>
                                                            <ext:Panel ID="Panel4" runat="server" Border="false" BodyStyle="padding:5px;background-color: transparent;">
                                                                <Body>
                                                                    <ext:Label ID="Label3" Text="权重" runat="server" Width="25" StyleSpec="font-size:12px;color:red">
                                                                    </ext:Label>
                                                                    <ext:NumberField ID="QZNum" runat="server" Text="1" DecimalPrecision="2" Width="25"
                                                                        MaxLength="4" StyleSpec="margin-bottom:10px;">
                                                                    </ext:NumberField>
                                                                    <ext:Button ID="Button1" runat="server" Icon="ResultsetNext" StyleSpec="margin-bottom:2px;">
                                                                        <Listeners>
                                                                            <Click Handler="TwoSideSelector.add();" />
                                                                        </Listeners>
                                                                        <ToolTips>
                                                                            <ext:ToolTip ID="ToolTip1" runat="server" Title="添加" Html="添加左侧选中行" />
                                                                        </ToolTips>
                                                                    </ext:Button>
                                                                    <ext:Button ID="Button2" runat="server" Icon="ResultsetLast" StyleSpec="margin-bottom:2px;">
                                                                        <Listeners>
                                                                            <Click Handler="TwoSideSelector.addAll();" />
                                                                        </Listeners>
                                                                        <ToolTips>
                                                                            <ext:ToolTip ID="ToolTip2" runat="server" Title="添加全部" Html="添加左侧全部" />
                                                                        </ToolTips>
                                                                    </ext:Button>
                                                                    <ext:Button ID="Button3" runat="server" Icon="ResultsetPrevious" StyleSpec="margin-bottom:2px;">
                                                                        <Listeners>
                                                                            <Click Handler="TwoSideSelector.remove();" />
                                                                        </Listeners>
                                                                        <ToolTips>
                                                                            <ext:ToolTip ID="ToolTip3" runat="server" Title="移除" Html="移除右侧选中行" />
                                                                        </ToolTips>
                                                                    </ext:Button>
                                                                    <ext:Button ID="Button4" runat="server" Icon="ResultsetFirst" StyleSpec="margin-bottom:2px;">
                                                                        <Listeners>
                                                                            <Click Handler="TwoSideSelector.removeAll();" />
                                                                        </Listeners>
                                                                        <ToolTips>
                                                                            <ext:ToolTip ID="ToolTip4" runat="server" Title="移除全部" Html="移除右侧全部" />
                                                                        </ToolTips>
                                                                    </ext:Button>
                                                                </Body>
                                                            </ext:Panel>
                                                        </ext:Anchor>
                                                    </ext:AnchorLayout>
                                                </Body>
                                            </ext:Panel>
                                        </ext:LayoutColumn>
                                        <ext:LayoutColumn ColumnWidth="0.5">
                                            <ext:Panel ID="Panel6" runat="server" Border="false">
                                                <Body>
                                                    <ext:MultiSelect ID="SelectorRight" runat="server" Legend="已选指标" DragGroup="grp2"
                                                        DropGroup="grp1,grp2" StoreID="Store2" DisplayField="GUIDE_NAME_QZ" ValueField="GUIDE_CODE"
                                                        AutoWidth="true" Height="250" KeepSelectionOnClick="WithCtrlKey" StyleSpec="margin:5px; ">
                                                        <Listeners>
                                                            <Render Handler=" this.setHeight(Ext.lib.Dom.getViewHeight()  - this.getPosition()[1] );" />
                                                            <BeforeDrop Fn="dragToRight" />
                                                        </Listeners>
                                                    </ext:MultiSelect>
                                                </Body>
                                            </ext:Panel>
                                        </ext:LayoutColumn>
                                    </ext:ColumnLayout>
                                </Body>
                            </ext:Panel>
                        </ext:LayoutColumn>
                    </Columns>
                </ext:ColumnLayout>
            </Body>
        </ext:ViewPort>
    </div>
    </form>
</body>
</html>
