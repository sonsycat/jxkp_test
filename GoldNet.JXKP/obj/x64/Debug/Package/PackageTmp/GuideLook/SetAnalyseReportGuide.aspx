<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SetAnalyseReportGuide.aspx.cs" Inherits="GoldNet.JXKP.GuideLook.SetAnalyseReportGuide" %>
<%@ Register Assembly="Goldnet.Ext.Web" Namespace="Goldnet.Ext.Web" TagPrefix="ext" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>指标选择</title>
    <style type="text/css">
    .icon-expand-all  { background-image: url(/resources/images/expand-all.gif) !important; }
    .icon-collapse-all  { background-image: url(/resources/images/collapse-all.gif) !important; } 
    </style>
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
       <DocumentReady Handler= " Ext.EventManager.onWindowResize(SelectorLayout); refreshTree(TreeCtrl);"  />
       </Listeners>
   </ext:ScriptManager>
    <ext:Store runat="server" ID="Store1">
        <Reader>
            <ext:JsonReader>
                <Fields>
                    <ext:RecordField Name="TEXT" />
                    <ext:RecordField Name="VALUE" />
                </Fields>
            </ext:JsonReader>
        </Reader>
    </ext:Store>
    <ext:Store runat="server" ID="Store2">
        <Reader>
            <ext:JsonReader>
                <Fields>
                    <ext:RecordField Name="TEXT" />
                    <ext:RecordField Name="VALUE" />
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
        

<ext:Panel ID="Panel11" runat="server"  Width="400" Height="300"  BodyBorder="false" >
    <TopBar >
        <ext:Toolbar runat="server" ID="ctl155" >
            <Items>
                <ext:ToolbarTextItem ID="ToolbarTextItem2" runat="server" Text="组织："/>
                <ext:ComboBox runat="server" ID="Combo_Org"  Width="40" >
                    <Listeners>
                        <Select Handler="refreshTree(TreeCtrl);" />
                    </Listeners>
                </ext:ComboBox>
                <ext:Label runat="server" Text="科室：" ID="Label2" ></ext:Label>
                <ext:ComboBox runat="server" ID="Combo_DeptType" Width="88" >
                    <Listeners>
                        <Select Handler="refreshTree(TreeCtrl);" />
                    </Listeners>
                </ext:ComboBox>
                <ext:ToolbarSeparator ID="ToolbarSeparator4" runat="server"></ext:ToolbarSeparator>
                <ext:Label runat="server" Text="指标名称(代码)：" ID="Label1" ></ext:Label>
                <ext:TextField ID="txtTagName" runat="server" FieldLabel="指标" Width="100">
                </ext:TextField>
                <ext:ToolbarSeparator ID="ToolbarSeparator1" runat="server"></ext:ToolbarSeparator>
                <ext:Button ID="Button5" runat="server" Icon="DatabaseGo" Text="查询"> 
                    <AjaxEvents>
                        <Click OnEvent="SearchGuide" >
                            <EventMask Msg="请稍候..." ShowMask="true" />
                            <ExtraParams>
                                <ext:Parameter Name="multi1" Value="Ext.encode(#{SelectorRight}.getValues())" Mode="Raw" />
                                <ext:Parameter Name="multi2" Value="Ext.encode(#{SelectorRight}.getValues(true))" Mode="Raw" />
                            </ExtraParams>
                        </Click>
                    </AjaxEvents>
                </ext:Button>
                <ext:ToolbarSeparator ID="ToolbarSeparator2" runat="server"></ext:ToolbarSeparator>
                <ext:ToolbarFill ID="ToolbarFill2" runat="server"></ext:ToolbarFill>
                <ext:ToolbarSeparator ID="ToolbarSeparator5" runat="server"></ext:ToolbarSeparator>
                <ext:Button ID="save" runat="server" Icon="Disk" Text="保存">
                    <AjaxEvents>
                        <Click OnEvent="SaveGuide" >
                            <EventMask Msg="请稍候..." ShowMask="true" />
                            <ExtraParams>
                                <ext:Parameter Name="multi2" Value="Ext.encode(#{SelectorRight}.getValues(true))" Mode="Raw" />
                            </ExtraParams>
                        </Click>
                    </AjaxEvents>
                    <Listeners>
                        <Click  Handler="parent.window.arcEditWindow.hide()"/>
                    </Listeners>
                </ext:Button>
                <ext:ToolbarSeparator ID="ToolbarSeparator6" runat="server"></ext:ToolbarSeparator>
                
            </Items>
        </ext:Toolbar>
    </TopBar>
    <Body>
        <ext:ColumnLayout ID="ColumnLayout1" runat="server" FitHeight="true" >
        <ext:LayoutColumn ColumnWidth="0.25">
          <ext:TreePanel ID="TreeCtrl"  runat="server"   Border="false" AutoScroll="true" Animate="false"  UseArrows="true" >
            <TopBar>
                <ext:Toolbar ID="ToolBar1" runat="server">
                    <Items>
                     <ext:ToolbarTextItem ID="ToolbarTextItem1" runat="server" Text="指标分类列表" />
                            <ext:ToolbarFill ID="ToolbarFill1" runat="server" />
                                
                                <ext:ToolbarButton ID="ToolbarButton2" runat="server"  IconCls="icon-expand-all">
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
                <ext:TreeNode NodeID="root" Text="指标体系" ></ext:TreeNode>
            </Root>
            <Listeners>
                <BeforeClick Handler="node.select();" />
                <Click Handler = "TreeNodeSelected();" />
            </Listeners>
        </ext:TreePanel>
        </ext:LayoutColumn>
            <ext:LayoutColumn ColumnWidth="0.35">
                 <ext:Panel ID="Panel5" runat="server" Border="false"  MonitorResize="true" >
                        <Body>  
                            <ext:MultiSelect ID="SelectorLeft" runat="server"  Legend="待选指标" DragGroup="grp1" DropGroup="grp2,grp1" 
                                StoreID="Store1"
                                DisplayField="TEXT"
                                ValueField="VALUE"
                                EnableViewState ="true"
                                 Stateful ="true"
                                AutoWidth="true" Height="250" KeepSelectionOnClick="WithCtrlKey" StyleSpec="margin:5px; ">
                                <Listeners>  
                                    <Render Handler=" this.setHeight(Ext.lib.Dom.getViewHeight() - this.getPosition()[1] );" />
                                </Listeners>
                            </ext:MultiSelect>      
                            </Body>
                     </ext:Panel>
            </ext:LayoutColumn>
            
            <ext:LayoutColumn>
            
                <ext:Panel ID="Panel2" runat="server" Width="35" BodyStyle="background-color: transparent;"  Border="false">
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
               <ext:Panel ID="Panel6" runat="server" Border="false" >
                    <Body>
                        <ext:MultiSelect ID="SelectorRight" runat="server" Legend="已选指标" DragGroup="grp2" DropGroup="grp1,grp2" 
                                StoreID="Store2"
                                DisplayField="TEXT"
                                ValueField="VALUE"
                                AutoWidth="true" Height="250" KeepSelectionOnClick="WithCtrlKey"  StyleSpec="margin:5px; ">
                            <Listeners>
                                <Render Handler=" this.setHeight(Ext.lib.Dom.getViewHeight()  - this.getPosition()[1] );" />
                                
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
    </body> 
    </ext:ViewPort>
    </div>
    </form>

</body>
</html>
