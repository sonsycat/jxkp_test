<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="main_set.aspx.cs" Inherits="GoldNet.JXKP.mainpage.main_set" %>

<%@ Register Assembly="Goldnet.Ext.Web" Namespace="Goldnet.Ext.Web" TagPrefix="ext" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>指标选择</title>
    <style type="text/css">
        .icon-expand-all
        {
            background-image: url(/resources/images/expand-all.gif) !important;
        }
        .icon-collapse-all
        {
            background-image: url(/resources/images/collapse-all.gif) !important;
        }
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
                //var qz = '(' + QZNum.getValue() + ')';
                var selectionsArray = source.view.getSelectedIndexes();
                var records = [];
                if (selectionsArray.length > 0) {
                    for (var i = 0; i < selectionsArray.length; i++) {
                        var rec = source.view.store.getAt(selectionsArray[i]);
                        rec.data.NAME = rec.data.NAME;
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
                //var qz = '(' + QZNum.getValue() + ')';
                var records = source.store.getRange();
                if (records.length > 0) {
                    for (var i = 0; i < records.length; i++) {
                        records[i].data.NAME = records[i].data.NAME;
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
                //var qz = '(' + QZNum.getValue() + ')';
                if (data.records.length > 0) {
                    for (var i = 0; i < data.records.length; i++) {
                        data.records[i].data.NAME = data.records[i].data.NAME;
                    }
                }
            }
        };
        var dblSelectLeft = function(vw, index, node, e) {
            //var qz = '(' + QZNum.getValue() + ')';
            vw.store.data.items[index].data.NAME = vw.store.data.items[index].data.NAME;
        };        
        var TreeNodeSelected = function() {
            if (TreeCtrl.getSelectionModel().getSelectedNode() != null) {
                var selNode = TreeCtrl.getSelectionModel().getSelectedNode();
                if (selNode.id == "root") return;
                var pNodeId = selNode.parentNode.id;
                if (pNodeId != "root") {
                    Goldnet.AjaxMethods.TreeSelectedGuide(selNode.id, Ext.encode(SelectorRight.getValues()), {
                        eventMask: {
                            msg: "请稍候...",
                            showMask: true,
                            minDelay: 500
                        }
                    });
                }
            }
        }
        var refreshTree = function(tree) {
            tree.el.mask('正在加载...', 'x-loading-mask');
            Goldnet.AjaxMethods.RefreshTree({
                success: function(result) {
                    var nodes = eval(result);
                    tree.root.ui.remove();
                    tree.initChildren(nodes);
                    tree.root.render();
                    tree.el.unmask();
                },
                failure: function(msg) {
                    tree.el.unmask();
                    Ext.Msg.show({ title: '系统错误', msg: '未能更新数据 ' + msg, icon: 'ext-mb-warning', buttons: { ok: true} });
                }
            });
        };
    </script>

</head>
<body>
    <form id="form1" runat="server">
    <ext:ScriptManager ID="ScriptManager1" runat="server">
        <Listeners>
            <DocumentReady Handler=" Ext.EventManager.onWindowResize(SelectorLayout); refreshTree(TreeCtrl);" />
        </Listeners>
    </ext:ScriptManager>
    <ext:Store runat="server" ID="Store1">
        <Reader>
            <ext:JsonReader>
                <Fields>
                    <ext:RecordField Name="NAME" />
                    <ext:RecordField Name="ID" />
                    <ext:RecordField Name="PID" />
                </Fields>
            </ext:JsonReader>
        </Reader>
    </ext:Store>
    <ext:Store runat="server" ID="Store2">
        <Reader>
            <ext:JsonReader>
                <Fields>
                    <ext:RecordField Name="NAME" />
                    <ext:RecordField Name="ID" />
                    <ext:RecordField Name="PID" />
                </Fields>
            </ext:JsonReader>
        </Reader>
    </ext:Store>
    <ext:Store ID="SYear" runat="server">
        <Reader>
            <ext:JsonReader ReaderID="YEAR">
                <Fields>
                    <ext:RecordField Name="YEAR" />
                </Fields>
            </ext:JsonReader>
        </Reader>
    </ext:Store>
    <ext:Store ID="Store3" runat="server">
        <Reader>
            <ext:JsonReader>
                <Fields>
                    <ext:RecordField Name="stationcode" />
                    <ext:RecordField Name="stationname" />
                </Fields>
            </ext:JsonReader>
        </Reader>
    </ext:Store>
    <div>
        <ext:Window runat="server" ID="Win_BatchInit" AutoShow="false" ShowOnLoad="false"
            Modal="true" Resizable="false" Title="生成岗位数据" CenterOnLoad="true" AutoScroll="false"
            Width="280" Height="140" CloseAction="Hide" AnimateTarget="Btn_BatInit" Icon="TagPink"
            BodyStyle="padding:2px;">
            <Body>
                <table>
                    <tr>
                        <td colspan="2" align="left">
                            <p>
                                注意：生成数据需要大约2分钟时间，在此时间内请不要关闭您的浏览器或者刷新页面。</p>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <p>
                                <ext:Label runat="server" ID="progressTip" Text="进度" AutoWidth="true">
                                </ext:Label>
                            </p>
                            <ext:ProgressBar ID="Progress1" runat="server" Width="255">
                            </ext:ProgressBar>
                        </td>
                    </tr>
                </table>
                <ext:TaskManager ID="TaskManager1" runat="server">
                    <Tasks>
                        <ext:Task TaskID="longactionprogress" Interval="1000" AutoRun="false" OnStart="#{Btn_BatStart}.setDisabled(true); "
                            OnStop="#{Btn_BatStart}.setDisabled(false);">
                            <AjaxEvents>
                                <Update OnEvent="RefreshProgress" />
                            </AjaxEvents>
                        </ext:Task>
                    </Tasks>
                </ext:TaskManager>
            </Body>
            <BottomBar>
                <ext:Toolbar ID="Toolbar2" runat="server">
                    <Items>
                        <ext:ToolbarFill ID="ToolbarFill2" runat="server" />
                        <ext:ToolbarButton ID="Btn_BatStart" runat="server" Icon="PlayGreen" Text="开始生成">
                            <AjaxEvents>
                                <Click OnEvent="Btn_BatStart_Click" Timeout="1200000">
                                </Click>
                            </AjaxEvents>
                        </ext:ToolbarButton>
                        <ext:ToolbarSeparator ID="ToolbarSeparator6" runat="server" />
                        <ext:ToolbarButton ID="Btn_BatCancel" runat="server" Icon="Cancel" Text="退出">
                            <Listeners>
                                <Click Handler="Win_BatchInit.hide();" />
                            </Listeners>
                        </ext:ToolbarButton>
                    </Items>
                </ext:Toolbar>
            </BottomBar>
            <Listeners>
                <Show Handler="this.dirty = false;" />
                <BeforeHide Handler="
                    if ((this.dirty==false)&&(Btn_BatCancel.text=='取消') ){
                        Ext.Msg.confirm('系统提示', '注意:任务正在运行，确定取消任务并退出吗？', function (btn) { 
                            if(btn == 'yes') { 
                                this.dirty = true;
                                this.hide(); 
                            } 
                        }, this);
                        return false;    
                    }" />
                <Hide Handler="TaskManager1.stopAll();" />
            </Listeners>
            <AjaxEvents>
                <Hide OnEvent="CloseBatInit" />
            </AjaxEvents>
        </ext:Window>
        
        <ext:ViewPort ID="ViewPort1" runat="server">
            <Body>
                <ext:ColumnLayout ID="ColumnLayout2" runat="server" Split="true" FitHeight="true">
                    <Columns>
                        <ext:LayoutColumn ColumnWidth="1">
                            <ext:Panel ID="Panel11" runat="server" Width="400" Height="300" BodyBorder="false">
                                <TopBar>
                                    <ext:Toolbar runat="server" ID="ctl155">
                                        <Items>
                                            <ext:Label ID="Label4" runat="server" Text="年度：" />
                                            <ext:ComboBox ID="cbbYear" runat="server" ReadOnly="true" StoreID="SYear" Width="60"
                                                DisplayField="YEAR" ValueField="YEAR">
                                                <AjaxEvents>
                                                    <Select OnEvent="Search_Select">
                                                        <EventMask ShowMask="true" />
                                                    </Select>
                                                </AjaxEvents>
                                            </ext:ComboBox>
                                            <ext:ToolbarSeparator ID="ToolbarSeparator2" runat="server" />
                                            <ext:Label ID="Label5" runat="server" Text="岗位：" />
                                            <ext:ComboBox ID="ComboBox1" runat="server" Width="180" Stateful="true" EmptyText="请选择岗位">
                                                <AjaxEvents>
                                                    <Select OnEvent="station_Select">
                                                        <EventMask ShowMask="true" />
                                                    </Select>
                                                </AjaxEvents>
                                            </ext:ComboBox>
                                            <ext:ToolbarSeparator ID="ToolbarSeparator3" runat="server" />
                                            <ext:Label runat="server" Text="区域：" ID="Label3">
                                            </ext:Label>
                                            <ext:ComboBox runat="server" ID="Combo_DeptType" Width="120">
                                                <AjaxEvents>
                                                    <Select OnEvent="depttype_Select">
                                                        <EventMask ShowMask="true" />
                                                    </Select>
                                                </AjaxEvents>
                                            </ext:ComboBox>
                                            <ext:ToolbarSeparator ID="ToolbarSeparator4" runat="server" />
                                            <ext:Label runat="server" Text="图表标题：" ID="Label1">
                                            </ext:Label>
                                            <ext:TextField ID="txtTagName" runat="server" FieldLabel="标题" Width="160">
                                            </ext:TextField>
                                            <ext:ToolbarSeparator ID="ToolbarSeparator1" runat="server" />
                                            <ext:Label runat="server" Text="显示：" ID="Label2" />
                                            <ext:RadioGroup ID="RadioGroup1" runat="server" ClearCls="x-form-radio-group" ItemCls="x-check-group-base"
                                                FieldLabel="是否考核" StyleSpec="background-color: transparent;">
                                                <Items>
                                                    <ext:Radio ID="Radio1" runat="server" BoxLabel="是" Checked="true" AutoWidth="true" />
                                                    <ext:Radio ID="Radio2" runat="server" BoxLabel="否" AutoWidth="true" />
                                                </Items>
                                            </ext:RadioGroup>
                                            <ext:ToolbarSeparator ID="ToolbarSeparator5" runat="server" />
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
                                            <ext:Button ID="creatstationguide" runat="server" Icon="Disk" Text="生成岗位数据" Visible="true">
                                                <AjaxEvents>
                                                    <Click OnEvent="CreateStationGuide" Timeout="9000000">
                                                        <ExtraParams>
                                                            <ext:Parameter Name="multi2" Value="Ext.encode(#{SelectorRight}.getValues(true))"
                                                                Mode="Raw" />
                                                        </ExtraParams>
                                                    </Click>
                                                </AjaxEvents>
                                            </ext:Button>
                                        </Items>
                                    </ext:Toolbar>
                                </TopBar>
                                <Body>
                                    <ext:ColumnLayout ID="ColumnLayout1" runat="server" FitHeight="true">
                                        <ext:LayoutColumn ColumnWidth="0.25">
                                            <ext:TreePanel ID="TreeCtrl" runat="server" Border="false" AutoScroll="true" Animate="false"
                                                UseArrows="true">
                                                <TopBar>
                                                    <ext:Toolbar ID="ToolBar1" runat="server">
                                                        <Items>
                                                            <ext:ToolbarTextItem ID="ToolbarTextItem1" runat="server" Text="指标分类列表" />
                                                            <ext:ToolbarFill ID="ToolbarFill1" runat="server" />
                                                            <ext:ToolbarButton ID="ToolbarButton2" runat="server" IconCls="icon-expand-all">
                                                                <Listeners>
                                                                    <Click Handler="#{TreeCtrl}.expandAll(true);" />
                                                                </Listeners>
                                                                <ToolTips>
                                                                    <ext:ToolTip ID="ToolTip5" IDMode="Ignore" runat="server" Html="全部展开" />
                                                                </ToolTips>
                                                            </ext:ToolbarButton>
                                                            <ext:ToolbarButton ID="ToolbarButton3" runat="server" IconCls="icon-collapse-all">
                                                                <Listeners>
                                                                    <Click Handler="#{TreeCtrl}.collapseAll(true);" />
                                                                </Listeners>
                                                                <ToolTips>
                                                                    <ext:ToolTip ID="ToolTip6" IDMode="Ignore" runat="server" Html="全部收起" />
                                                                </ToolTips>
                                                            </ext:ToolbarButton>
                                                        </Items>
                                                    </ext:Toolbar>
                                                </TopBar>
                                                <Root>
                                                    <ext:TreeNode NodeID="root" Text="指标体系">
                                                    </ext:TreeNode>
                                                </Root>
                                                <Listeners>
                                                    <BeforeClick Handler="node.select();" />
                                                    <Click Handler="TreeNodeSelected();" />
                                                </Listeners>
                                            </ext:TreePanel>
                                        </ext:LayoutColumn>
                                        <ext:LayoutColumn ColumnWidth="0.35">
                                            <ext:Panel ID="Panel5" runat="server" Border="false" MonitorResize="true">
                                                <Body>
                                                    <ext:MultiSelect ID="SelectorLeft" runat="server" Legend="待选指标" DragGroup="grp1"
                                                        DropGroup="grp2,grp1" StoreID="Store1" DisplayField="NAME" ValueField="ID" EnableViewState="true"
                                                        Stateful="true" AutoWidth="true" Height="250" KeepSelectionOnClick="WithCtrlKey"
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
                                        <ext:LayoutColumn ColumnWidth="0.35">
                                            <ext:Panel ID="Panel6" runat="server" Border="false">
                                                <Body>
                                                    <ext:MultiSelect ID="SelectorRight" runat="server" Legend="已选指标" DragGroup="grp2"
                                                        DropGroup="grp1,grp2" StoreID="Store2" DisplayField="NAME" ValueField="ID" AutoWidth="true"
                                                        Height="250" KeepSelectionOnClick="WithCtrlKey" StyleSpec="margin:5px; ">
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
