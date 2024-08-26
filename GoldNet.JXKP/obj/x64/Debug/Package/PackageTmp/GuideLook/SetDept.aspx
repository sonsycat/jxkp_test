<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SetDept.aspx.cs" Inherits="GoldNet.JXKP.GuideLook.SetDept" %>

<%@ Register Assembly="Goldnet.Ext.Web" Namespace="Goldnet.Ext.Web" TagPrefix="ext" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>无标题页</title>

    <script type="text/javascript">                    
         var SelectorLayout = function() {
             SelectorLeft.setHeight(Ext.lib.Dom.getViewHeight() - SelectorLeft.getPosition()[1]- 5);
             SelectorRight.setHeight(Ext.lib.Dom.getViewHeight() - SelectorRight.getPosition()[1]- 5);
         }
         var TwoSideSelector = {
             add: function(source, destination) {
                 source = source || SelectorLeft;
                 destination = destination || SelectorRight;
                 var selectionsArray = source.view.getSelectedIndexes();
                 var records = [];
                 if (selectionsArray.length > 0) {
                     for (var i = 0; i < selectionsArray.length; i++) {
                         var rec = source.view.store.getAt(selectionsArray[i]);
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
                 destination.store.add(source.store.getRange());
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
         
        var nodeState = function(node) {
            var box = node.getUI().checkbox;
            if (typeof box == 'undefined') return;
            if (box.checked) {
                return 1;
            } else if (box.indeterminate) {
                return 2;
            } else {
                return 3;
            }
        }
        var siblState = function(node) {
            var state = new Array();
            var firstNode = node.parentNode.firstChild;
            if (!firstNode) {
                return false;
            }
            do {
                state.push(nodeState(firstNode));
                firstNode = firstNode.nextSibling;
            } while (firstNode != null)
            return state;
        }
        var parentState = function(node) {
            var state = siblState(node).join();
            if (state.indexOf("3") == -1 && state.indexOf("2") == -1) {
                return 1;
            } else if (state.indexOf("1") == -1 && state.indexOf("2") == -1) {
                return -1;
            } else {
                return 0;
            }
        }
        var parentChecked = function(node) {
            var parentNode = node.parentNode;
            if (parentNode == null)  return false;
            var checkbox = parentNode.getUI().checkbox;
            if (typeof checkbox == 'undefined')  return false;
            var check = parentState(node);
            if (check == 1) {
                checkbox.indeterminate = false;
                checkbox.checked = true;
            } else if (check == -1) {
                checkbox.checked = false;
                checkbox.indeterminate = false;
            } else {
                checkbox.checked = false;
                checkbox.indeterminate = true;
            }
            parentChecked(parentNode);
        }

        //选择改变事件
        function ToCheckChange(node, checked) {
            if (checked) {
                node.expand();
                node.eachChild(function(child) {
                //toggleCheck将嵌套触发checkchange事件
                child.ui.toggleCheck(checked);
                ////child.ui.checkbox.checked = checked;
                //child.ui.node.attributes.checked = checked;
                });
                parentChecked(node);
            }
            else {
                node.collapse();
                node.eachChild(function(child) {
                //toggleCheck将嵌套触发checkchange事件
                child.ui.toggleCheck(checked);
                //child.ui.checkbox.checked = checked;
                //child.ui.node.attributes.checked = checked;
                });
                parentChecked(node);
            }
        }
        //选择改变事件
        function ToCheckSecondChange(node, checked) {
            if (checked) {
                node.expand();
                node.eachChild(function(child) {
                //toggleCheck将嵌套触发checkchange事件
                child.ui.toggleCheck(checked);
//                child.ui.checkbox.checked = checked;
//                child.ui.node.attributes.checked = checked;
                });
                parentChecked(node);
            }
            else {
                node.collapse();
                node.eachChild(function(child) {
                //toggleCheck将嵌套触发checkchange事件
                child.ui.toggleCheck(checked);
//                child.ui.checkbox.checked = checked;
//                child.ui.node.attributes.checked = checked;
                });
                parentChecked(node);
            }
        }
        
        
        
        
        function getCheckedNode() {
            var result = "";
            var checkeds = Ext.getCmp('TreeCtrlDept').getChecked();
            for (var i = 0; i < checkeds.length; i++) {
                result = result + checkeds[i].id + ";";
            }
            return result;
        }
        
        
        
        function refreshTree(tree) {
            tree.el.mask('正在加载...', 'x-loading-mask');
            Goldnet.AjaxMethods.RefreshMenu({
                success: function(result) {
                    var nodes = eval(result);
                    tree.root.ui.remove();
                    tree.initChildren(nodes);
                    tree.root.render();
                    tree.el.unmask();
                    SelectorLeft.store.removeAll();
                    SelectorRight.store.removeAll();
                },
                failure: function(msg) {
                    tree.el.unmask();
                    Ext.Msg.alert('Failure', '未能加载数据');
                }
            });
        }
        
        
        
    </script>

    <style type="text/css">
        .icon-expand-all
        {
            background-image: url(/resources/images/expand-all.gif) !important;
        }
        .icon-collapse-all
        {
            background-image: url(/resources/images/collapse-all.gif) !important;
        }
        table
        {
            font-size: 12px;
        }
        body
        {
            background-color: #DFE8F6;
        }
    </style>
</head>
<body>
    <form id="form2" runat="server">
    <ext:ScriptManager ID="ScriptManager1" runat="server">
        <Listeners>
            <DocumentReady Handler=" Ext.EventManager.onWindowResize(SelectorLayout);" />
        </Listeners>
    </ext:ScriptManager>
    <ext:Store runat="server" ID="Store1">
        <Reader>
            <ext:JsonReader>
                <Fields>
                    <ext:RecordField Name="VALUE" />
                    <ext:RecordField Name="TEXT" />
                </Fields>
            </ext:JsonReader>
        </Reader>
    </ext:Store>
    <ext:Store runat="server" ID="Store2">
        <Reader>
            <ext:JsonReader>
                <Fields>
                    <ext:RecordField Name="VALUE" />
                    <ext:RecordField Name="TEXT" />
                </Fields>
            </ext:JsonReader>
        </Reader>
    </ext:Store>
    <div>
        <ext:Panel ID="Panel1" runat="server" Border="false" Header="false" AutoShow="false"
            Width="760" Height="350" BodyStyle="background-color:transparent">
            <Body>
                <ext:Panel ID="SFM_DefineDetailsWindow" runat="server" Height="350" Width="760" Border="false"
                    BodyStyle="background-color:transparent">
                    <TopBar>
                        <ext:Toolbar runat="server" ID="ctl155">
                            <Items>
                                <ext:Label runat="server" Text="科室显示树形式：" ID="Label1">
                                </ext:Label>
                                <ext:ComboBox runat="server" ID="Combo_DeptType" Width="85">
                                    <Items>
                                        <ext:ListItem Text="按科室类别" Value="0" />
                                        <ext:ListItem Text="按二级科室" Value="1" />
                                    </Items>
                                    <Listeners>
                                        <Select Handler="refreshTree(#{TreeCtrlDept})" />
                                    </Listeners>
                                </ext:ComboBox>
                                <ext:ToolbarSeparator ID="ToolbarSeparator3" runat="server">
                                </ext:ToolbarSeparator>
                                <ext:Button ID="btn_Query" runat="server" Icon="DatabaseGo" Text="查询">
                                    <AjaxEvents>
                                        <Click OnEvent="TreeChangeChecked">
                                            <ExtraParams>
                                                <ext:Parameter Name="checkNodes" Value="getCheckedNode()" Mode="Raw" />
                                                <ext:Parameter Name="multi1" Value="Ext.encode(#{SelectorRight}.getValues())" Mode="Raw" />
                                            </ExtraParams>
                                            <EventMask ShowMask="true" Msg="请稍候..." />
                                        </Click>
                                    </AjaxEvents>
                                </ext:Button>
                            </Items>
                        </ext:Toolbar>
                    </TopBar>
                    <Body>
                        <ext:ColumnLayout ID="ColumnLayout1" runat="server" FitHeight="true">
                            <ext:LayoutColumn ColumnWidth="0.3">
                                <ext:TreePanel ID="TreeCtrlDept" runat="server" Width="160" Icon="BookOpen" AutoScroll="true"
                                    BodyStyle="background-color:transparent" BodyBorder="false">
                                    <TopBar>
                                        <ext:Toolbar ID="ToolBar1" runat="server">
                                            <Items>
                                                <ext:ToolbarTextItem ID="ToolbarTextItem1" runat="server" Text="请选择以下科室类别" />
                                                <ext:ToolbarFill ID="ToolbarFill2" runat="server" />
                                                <ext:ToolbarButton ID="ToolbarButton2" runat="server" IconCls="icon-expand-all">
                                                    <Listeners>
                                                        <Click Handler="#{TreeCtrlDept}.root.expand(true);" />
                                                    </Listeners>
                                                    <ToolTips>
                                                        <ext:ToolTip ID="ToolTip5" IDMode="Ignore" runat="server" Html="全部展开" />
                                                    </ToolTips>
                                                </ext:ToolbarButton>
                                                <ext:ToolbarButton ID="ToolbarButton3" runat="server" IconCls="icon-collapse-all">
                                                    <Listeners>
                                                        <Click Handler="#{TreeCtrlDept}.root.collapse(true);" />
                                                    </Listeners>
                                                    <ToolTips>
                                                        <ext:ToolTip ID="ToolTip6" IDMode="Ignore" runat="server" Html="全部收起" />
                                                    </ToolTips>
                                                </ext:ToolbarButton>
                                            </Items>
                                        </ext:Toolbar>
                                    </TopBar>
                                    <Listeners>
                                        <CheckChange Handler="if(#{Combo_DeptType}.getSelectedItem().value == '0') { ToCheckChange(node,checked);} else {ToCheckSecondChange(node,checked);}" />
                                    </Listeners>
                                </ext:TreePanel>
                            </ext:LayoutColumn>
                            <ext:LayoutColumn ColumnWidth="0.33">
                                <ext:Panel ID="Panel59" runat="server" Border="false" MonitorResize="true" BodyStyle="background-color:transparent">
                                    <Body>
                                        <ext:MultiSelect ID="SelectorLeft" runat="server" Legend="待选信息" DragGroup="grp1"
                                            DropGroup="grp2" StoreID="Store1" DisplayField="TEXT" ValueField="VALUE" EnableViewState="true"
                                            Stateful="true" AutoWidth="true" Height="250" KeepSelectionOnClick="WithCtrlKey">
                                        </ext:MultiSelect>
                                    </Body>
                                </ext:Panel>
                            </ext:LayoutColumn>
                            <ext:LayoutColumn>
                                <ext:Panel ID="Panel4" runat="server" Width="35" BodyStyle="background-color: transparent;"
                                    Border="false">
                                    <Body>
                                        <ext:AnchorLayout ID="AnchorLayout1" runat="server">
                                            <ext:Anchor Vertical="20%">
                                                <ext:Panel ID="Panel5" runat="server" Border="false" BodyStyle="background-color: transparent;" />
                                            </ext:Anchor>
                                            <ext:Anchor>
                                                <ext:Panel ID="Panel6" runat="server" Border="false" BodyStyle="padding:5px;background-color: transparent;">
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
                            <ext:LayoutColumn ColumnWidth="0.33">
                                <ext:Panel ID="Panel7" runat="server" Border="false" BodyStyle="background-color:transparent">
                                    <Body>
                                        <ext:MultiSelect ID="SelectorRight" runat="server" Legend="已选信息" DragGroup="grp2"
                                            DropGroup="grp1,grp2" StoreID="Store2" DisplayField="TEXT" ValueField="VALUE"
                                            AutoWidth="true" Height="250" KeepSelectionOnClick="WithCtrlKey">
                                        </ext:MultiSelect>
                                        <table>
                                            <tr>
                                                <td>
                                                    <ext:Checkbox ID="ckx_Sort" Checked="true" runat="server">
                                                    </ext:Checkbox>
                                                </td>
                                                <td>
                                                    <ext:Label ID="Label2" Text="不包含已选信息(反向选择)" ForID="ckx_Sort" runat="server">
                                                    </ext:Label>
                                                </td>
                                            </tr>
                                        </table>
                                    </Body>
                                </ext:Panel>
                            </ext:LayoutColumn>
                        </ext:ColumnLayout>
                    </Body>
                    <BottomBar>
                        <ext:Toolbar ID="ToolBar2" runat="server">
                            <Items>
                                <ext:ToolbarButton ID="btnPrev" runat="server" Text="上一步" Icon="PreviousGreen" MenuAlign="right">
                                    <AjaxEvents>
                                        <Click OnEvent="Prev_Click">
                                            <ExtraParams>
                                                <ext:Parameter Name="multi1" Value="Ext.encode(#{SelectorRight}.getValues(true))"
                                                    Mode="Raw" />
                                                <ext:Parameter Name="checkNodes" Value="getCheckedNode()" Mode="Raw" />
                                            </ExtraParams>
                                        </Click>
                                    </AjaxEvents>
                                </ext:ToolbarButton>
                                <ext:ToolbarFill ID="Toolba" runat="server" />
                                <ext:ToolbarButton ID="btnNext" runat="server" Text="保存" Icon="Disk" MenuAlign="right">
                                    <AjaxEvents>
                                        <Click OnEvent="Next_Click">
                                            <ExtraParams>
                                                <ext:Parameter Name="multi1" Value="Ext.encode(#{SelectorRight}.getValues())" Mode="Raw" />
                                                <ext:Parameter Name="checkNodes" Value="getCheckedNode()" Mode="Raw" />
                                            </ExtraParams>
                                        </Click>
                                    </AjaxEvents>
                                </ext:ToolbarButton>
                            </Items>
                        </ext:Toolbar>
                    </BottomBar>
                </ext:Panel>
            </Body>
        </ext:Panel>
    </div>
    </form>
</body>
</html>
