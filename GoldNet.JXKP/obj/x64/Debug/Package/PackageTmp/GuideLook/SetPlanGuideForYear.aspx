<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SetPlanGuideForYear.aspx.cs"
    Inherits="GoldNet.JXKP.GuideLook.SetPlanGuideForYear" %>

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
            Panel11.setHeight(Ext.lib.Dom.getViewHeight() - Panel11.getPosition()[1]- 10);
            //SelectorRight.setHeight(Ext.lib.Dom.getViewHeight() - SelectorRight.getPosition()[1]- 5);
        }
        
        var TwoSideSelector = {
            add: function(source, destination) {
                    source = source || GridPanel1;
                    destination = destination || GridPanel2;
                    if (source.hasSelection()) {
                        destination.store.add(source.selModel.getSelections());
                        source.deleteSelected();
                    }
                },
                addAll: function(source, destination) {
                    source = source || GridPanel1;
                    destination = destination || GridPanel2;
                    destination.store.add(source.store.getRange());
                    source.store.removeAll();
                },
                addByName: function(name) {
                    if (!Ext.isEmpty(name)) {
                        var result = Store1.query("Name", name);
                        if (!Ext.isEmpty(result.items)) {
                            GridPanel2.store.add(result.items[0]);
                            GridPanel1.store.remove(result.items[0]);
                        }
                    }
                },
                addByNames: function(name) {
                    for (var i = 0; i < name.length; i++) {
                        this.addByName(name[i]);
                    }
                },
                remove: function(source, destination) {
                    this.add(destination, source);
                },
                removeAll: function(source, destination) {
                    this.addAll(destination, source);
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
                    Goldnet.AjaxMethods.TreeSelectedGuide(selNode.id, Ext.encode(GridPanel2.getRowsValues(false)), {
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
                    <ext:RecordField Name="ID" />
                    <ext:RecordField Name="GUIDENAME" />
                    <ext:RecordField Name="PID" />
                    <ext:RecordField Name="GUIDETYPE" />
                    <ext:RecordField Name="SHOWWIDTH" />
                    <ext:RecordField Name="SHOWSTYLE" />
                </Fields>
            </ext:JsonReader>
        </Reader>
    </ext:Store>
    <ext:Store runat="server" ID="Store2">
        <Reader>
            <ext:JsonReader>
                <Fields>
                    <ext:RecordField Name="ID" />
                    <ext:RecordField Name="GUIDENAME" />
                    <ext:RecordField Name="GUIDETYPE" />
                    <ext:RecordField Name="SHOWWIDTH" />
                    <ext:RecordField Name="SHOWSTYLE" />
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
                                                            <ext:Parameter Name="multi2" Value="Ext.encode(#{GridPanel2}.getRowsValues(false))"
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
                                            <ext:GridPanel runat="server" ID="GridPanel1" EnableDragDrop="false" AutoExpandColumn="GUIDENAME"
                                                StoreID="Store1" StripeRows="true">
                                                <ColumnModel ID="ColumnModel1" runat="server">
                                                    <Columns>
                                                        <ext:Column ColumnID="ID" Header="代码" DataIndex="ID" Hidden="true" />
                                                    </Columns>
                                                    <Columns>
                                                        <ext:Column ColumnID="GUIDENAME" Header="指标名称" DataIndex="GUIDENAME" Sortable="true" />
                                                    </Columns>
                                                </ColumnModel>
                                                <SelectionModel>
                                                    <ext:CheckboxSelectionModel ID="CheckboxSelectionModel1" runat="server" />
                                                </SelectionModel>
                                                <Plugins>
                                                    <ext:GridFilters ID="GridFilters1" runat="server" Local="true">
                                                        <Filters>
                                                            <ext:StringFilter DataIndex="GUIDENAME" />
                                                        </Filters>
                                                    </ext:GridFilters>
                                                </Plugins>
                                                <Listeners>
                                                    <DblClick Handler="TwoSideSelector.add();" />
                                                </Listeners>
                                            </ext:GridPanel>
                                        </ext:LayoutColumn>
                                        <ext:LayoutColumn>
                                            <ext:Panel ID="Panel3" runat="server" Width="35" BodyStyle="background-color: transparent;"
                                                Border="false">
                                                <Body>
                                                    <ext:AnchorLayout ID="AnchorLayout1" runat="server">
                                                        <ext:Anchor Vertical="40%">
                                                            <ext:Panel ID="Panel4" runat="server" Border="false" BodyStyle="background-color: transparent;" />
                                                        </ext:Anchor>
                                                        <ext:Anchor>
                                                            <ext:Panel ID="Panel5" runat="server" Border="false" BodyStyle="padding:5px;background-color: transparent;">
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
                                                                            <Click Handler="TwoSideSelector.remove(GridPanel1, GridPanel2);" />
                                                                        </Listeners>
                                                                        <ToolTips>
                                                                            <ext:ToolTip ID="ToolTip3" runat="server" Title="移除" Html="移除右侧选中行" />
                                                                        </ToolTips>
                                                                    </ext:Button>
                                                                    <ext:Button ID="Button4" runat="server" Icon="ResultsetFirst" StyleSpec="margin-bottom:2px;">
                                                                        <Listeners>
                                                                            <Click Handler="TwoSideSelector.removeAll(GridPanel1, GridPanel2);" />
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
                                            <ext:GridPanel runat="server" ID="GridPanel2" EnableDragDrop="false" StoreID="Store2" StripeRows="true" AutoExpandColumn="GUIDENAME">
                                                <ColumnModel ID="ColumnModel2" runat="server">
                                                    <Columns>
                                                        <ext:Column ColumnID="ID" Header="代码" DataIndex="ID" Hidden="true" />
                                                        <ext:Column ColumnID="GUIDENAME" Header="指标名称" DataIndex="GUIDENAME" Sortable="true"
                                                            Width="150" />
                                                        <ext:Column ColumnID="SHOWWIDTH" Header="<center>宽度</center>" Width="100" DataIndex="SHOWWIDTH"
                                                            MenuDisabled="true" Align="Right">
                                                            <Editor>
                                                                <ext:NumberField runat="server" ID="NumberField1" SelectOnFocus="true" DecimalPrecision="2">
                                                                </ext:NumberField>
                                                            </Editor>
                                                        </ext:Column>
                                                    </Columns>
                                                </ColumnModel>
                                                <SelectionModel>
                                                    <ext:CheckboxSelectionModel ID="edit" runat="server" />
                                                </SelectionModel>
                                                <Listeners>
                                                   <%-- <DblClick Handler="TwoSideSelector.remove(GridPanel1, GridPanel2);" />--%>
                                                </Listeners>
                                            </ext:GridPanel>
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
