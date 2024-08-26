<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="xyhs_costs_item.aspx.cs" Inherits="GoldNet.JXKP.cbhs.xyhs.xyhsdict.xyhs_costs_item" %>
<%@ Register Assembly="Goldnet.Ext.Web" Namespace="Goldnet.Ext.Web" TagPrefix="ext" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title></title>
    <style type="text/css">
        body
        {
            background-color: #DFE8F6;
            font-size: 12px;
        }
    </style>
    <link rel="stylesheet" type="text/css" href="../../../Bonus/Orthers/Cbouns.css" />
    <script language="javascript" type="text/javascript">
        var RefreshData = function(msg) {
            Ext.Msg.alert('提示',msg);
            Store1.reload();
            TreeCtrl;
        }
        var RefreshTypeData = function() {
            TreeCtrl.el.mask('正在加载...', 'x-loading-mask');
            Goldnet.AjaxMethods.TreeBuild({
                success: function(result) {
                    var nodes = eval(result);
                    TreeCtrl.root.ui.remove();
                    TreeCtrl.initChildren(nodes);
                    TreeCtrl.root.render();
                    TreeCtrl.el.unmask();
                },
                failure: function(msg) {
                    TreeCtrl.el.unmask();
                    Ext.Msg.alert('Failure', '未能加载数据');
                }
            });  
        }
        
        function getDeptCheckedNode() {
            return  TreeCtrl.getSelectionModel().getSelectedNode().id;
                    }
        
        var refreshGrid = function(node) {
           
            GridPanel1.el.mask('数据刷新中...', 'x-loading-mask');
            Goldnet.AjaxMethod.request(
              'GridPanelRefresh',
                {
                    params: {
                        ITEM_TYPE: node.id == 'root' ? '' : node.id
                    },
                    success: function(result) {
                        GridPanel1.el.unmask();
                    },
                    failure: function(msg) {
                        GridPanel1.el.unmask();
                    }
                });
            
        }

    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <ext:ScriptManager ID="ScriptManager1" runat="server" />
        <ext:Store ID="Store1" runat="server"  GroupField="ITEM_TYPE" OnRefreshData="Store_RefreshData">
            <Reader>
                <ext:JsonReader ReaderID="ITEM_CODE">
                    <Fields>
                        <ext:RecordField Name="ITEM_TYPE" />
                        <ext:RecordField Name="ITEM_CODE" />
                        <ext:RecordField Name="ITEM_NAME" />
                        <ext:RecordField Name="FINANCE_ITEM" />
                        <ext:RecordField Name="FINANCE_ITEM_GL" />
                    </Fields>
                </ext:JsonReader>
            </Reader>
        </ext:Store>
         <div>
        <ext:ViewPort ID="ViewPort2" runat="server">
            <Body>
                <ext:BorderLayout ID="BorderLayout2" runat="server">
                    <North>
                        <ext:Toolbar ID="Toolbar1" runat="server" Visible="true" AutoWidth="true">
                            <Items>
                                <ext:Button ID="btn_editType" runat="server" Text="修改类别" Icon="TextPaddingTop">
                                    <AjaxEvents>
                                        <Click OnEvent="Button_editType_click">
                                            <EventMask Msg="载入中..." ShowMask="true" />
                                        </Click>
                                    </AjaxEvents>
                                </ext:Button>
                                <ext:Button ID="btn_Add" runat="server" Text="添加" Icon="Add"  >
                                    <AjaxEvents>
                                        <Click OnEvent="Button_add_click">
                                            <EventMask Msg="载入中..." ShowMask="true" />
                                            <ExtraParams>                                                
                                              <ext:Parameter Name="Values" Value="getDeptCheckedNode()"  Mode="Raw">
                                               </ext:Parameter>
                                            </ExtraParams>
                                        </Click>
                                    </AjaxEvents>
                                </ext:Button>
                                <ext:Button ID="btn_Modify" runat="server" Text="修改" Icon="NoteEdit" Disabled="true">
                                    <AjaxEvents>
                                        <Click OnEvent="Button_edit_click">
                                            <EventMask Msg="载入中..." ShowMask="true" />
                                        </Click>
                                    </AjaxEvents>
                                </ext:Button>
                                <ext:Button ID="btn_Delete" runat="server" Text="删除" Icon="Delete" Disabled="true">
                                    <AjaxEvents>
                                        <Click OnEvent="Button_del_click">
                                            <EventMask Msg="载入中..." ShowMask="true" />
                                        </Click>
                                    </AjaxEvents>
                                </ext:Button>
                                <ext:Button ID="allcostbset" runat="server" Text="全成本设置" Icon="NoteEdit" >
                                    <AjaxEvents>
                                        <Click OnEvent="Button_allcostbset_click">
                                            <EventMask Msg="载入中..." ShowMask="true" />
                                        </Click>
                                    </AjaxEvents>
                                </ext:Button>
                                <ext:Button ID="Button_refresh" runat="server" Text="刷新" Icon="ArrowRefresh">
                                    <AjaxEvents>
                                        <Click OnEvent="Button_refresh_click">
                                            <EventMask Msg="载入中..." ShowMask="true" />
                                        </Click>
                                    </AjaxEvents>
                                </ext:Button>
                            </Items>
                        </ext:Toolbar>
                    </North>
                    <West Collapsible="false" Split="false" CollapseMode="Mini">
                        <ext:Panel ID="Panel2" runat="server" Width="300" BodyBorder="false"
                            AutoScroll="true" Border="false">
                            <Body>
                                <ext:TreePanel runat="server" Width="300" ID="TreeCtrl" AutoHeight="true" AutoScroll="false"
                                    Border="false">
                                    <Listeners>
                                        <Click Handler="refreshGrid(node)" />
                                    </Listeners>
                                </ext:TreePanel>
                            </Body>
                        </ext:Panel>
                    </West>
                    <Center>
                         <ext:GridPanel ID="GridPanel1" runat="server" StoreID="Store1" StripeRows="true"
                                TrackMouseOver="true" AutoWidth="true" Height="480" Border="false">                                
                                <ColumnModel ID="ColumnModel1" runat="server">
                                    <Columns>
                                         <ext:Column ColumnID="ITEM_TYPE" Header="类别" Width="100"  Sortable="true"
                                            DataIndex="ITEM_TYPE" MenuDisabled="true" Align="Left" />
                                        <ext:Column ColumnID="ITEM_NAME" Header="<div style='text-align:center;'>项目名称</div>" Width="200"  Sortable="true"
                                            DataIndex="ITEM_NAME" MenuDisabled="true"  Align="Left"  />
                                        <ext:Column ColumnID="ITEM_CODE" Header="<div style='text-align:center;'>项目代码</div>" Width="100"  Sortable="true"
                                            DataIndex="ITEM_CODE" MenuDisabled="true"   Align="Left" />
                                        <ext:Column ColumnID="FINANCE_ITEM" Header="<div style='text-align:center;'>医疗业务代码</div>" Width="100"  Sortable="true"
                                            DataIndex="FINANCE_ITEM" MenuDisabled="true"   Align="Left" />
                                             <ext:Column ColumnID="FINANCE_ITEM_GL" Header="<div style='text-align:center;'>管理费用代码</div>" Width="100"  Sortable="true"
                                            DataIndex="FINANCE_ITEM_GL" MenuDisabled="true"   Align="Left" />
                                        <%--<ext:Column ColumnID="INPUT_CODE" Header="输入码" Width="80"  Sortable="true"
                                            DataIndex="INPUT_CODE" MenuDisabled="true"  Align="Center"  />
                                        <ext:Column ColumnID="COST_PROPERTY" Header="成本属性" Width="80"  Sortable="true"
                                            DataIndex="COST_PROPERTY" MenuDisabled="true"  Align="Center"  />
                                        <ext:Column ColumnID="ALLOT_FOR_JD" Header="军地分摊方案" Width="100" 
                                            Sortable="true" DataIndex="ALLOT_FOR_JD" MenuDisabled="true"  Align="Center"  />
                                        <ext:Column ColumnID="ALLOT_FOR_JC" Header="级次分摊方案" Width="100"  Sortable="true"
                                            DataIndex="ALLOT_FOR_JC" MenuDisabled="true"  Align="Center"  />
                                        <ext:Column ColumnID="ALLOT_FOR_RY" Header="人员分摊方案" Width="100" Sortable="true"
                                            DataIndex="ALLOT_FOR_RY" MenuDisabled="true"  Align="Center"  />
                                         <ext:Column ColumnID="GETTYPE" Header="获取方式" Width="70"  Sortable="true"
                                            DataIndex="GETTYPE" MenuDisabled="true"  Align="Center"  />
                                         <ext:Column ColumnID="ACCOUNT_TYPE" Header="核算类型" Width="70"  Sortable="true"
                                            DataIndex="ACCOUNT_TYPE" MenuDisabled="true"  Align="Center"  />
                                            <ext:Column ColumnID="COST_DIRECT" Header="直接/间接成本" Width="70"  Sortable="true"
                                            DataIndex="COST_DIRECT" MenuDisabled="true"  Align="Center"  />
                                        <ext:Column ColumnID="COMPUTE_PER" Header="计入百分比" Width="70"  Sortable="true"
                                            DataIndex="COMPUTE_PER" MenuDisabled="true"  Align="Center"  />
                                        <ext:Column ColumnID="ITEM_POWER" Header="所属用户" Width="100"  Sortable="true"
                                            DataIndex="ITEM_POWER" MenuDisabled="true"  Align="Center"  />
                                        <ext:CommandColumn Header="权限" Sortable="true" ColumnID="Columns5" Align="Center" Width="50">
                                            <Commands>
                                                <ext:GridCommand Icon="CogStart" CommandName="Show" >
                                                </ext:GridCommand>
                                            </Commands>
                                        </ext:CommandColumn>--%>
                                    </Columns>
                                </ColumnModel>
                                <AjaxEvents>
                                    <RowDblClick OnEvent="Button_edit_click" />
                                </AjaxEvents>
                                <SelectionModel>
                                    <ext:RowSelectionModel ID="RowSelectionModel1" runat="server" SingleSelect="true">
                                        <Listeners>
                                            <RowSelect Handler="#{btn_Modify}.enable();#{btn_Delete}.enable()" />
                                            <RowDeselect Handler="if (!#{GridPanel1}.hasSelection()) {#{btn_Modify}.disable();#{btn_Delete}.disable();}" />
                                        </Listeners>
                                    </ext:RowSelectionModel>
                                </SelectionModel>
                                 <View>
                                    <ext:GroupingView  
                                        ID="GroupingView1"
                                        HideGroupedColumn="true"
                                        runat="server" 
                                        GroupTextTpl='{text} ({[values.rs.length]})'
                                        EnableRowBody="false">
                                    </ext:GroupingView>
                                </View>
                                <AjaxEvents>
                                    <Command OnEvent="SetPower">
                                        <ExtraParams>
                                            <ext:Parameter Name="Values" Value="this.store.getAt(rowIndex).get('ITEM_CODE')"  Mode="Raw">
                                            </ext:Parameter>
                                        </ExtraParams>
                                    </Command>
                                </AjaxEvents>
                            </ext:GridPanel>
                    </Center>
                </ext:BorderLayout>
            </Body>
        </ext:ViewPort>
    </div>      
        <ext:Window ID="DetailWin" runat="server" Icon="Group" Title="成本项目" Width="400" Height="420"
            AutoShow="false" Modal="true" CenterOnLoad="true" AutoScroll="true" ShowOnLoad="false"
            Resizable="true" StyleSpec="background-color:Transparent;" BodyStyle="background-color:Transparent;">
        </ext:Window>
        <ext:Window ID="PowerWin" runat="server" Icon="Group" Title="权限设置" Width="320" Height="352"
            AutoShow="false" Modal="true" CenterOnLoad="true" AutoScroll="true" ShowOnLoad="false"
            Resizable="true" StyleSpec="background-color:Transparent;" BodyStyle="background-color:Transparent;">
        </ext:Window>
         <ext:Window ID="TypeWin" runat="server" Icon="Group" Title="类别设置" Width="300" Height="352"
            AutoShow="false" Modal="true" CenterOnLoad="true" AutoScroll="true" ShowOnLoad="false"
            Resizable="true" StyleSpec="background-color:Transparent;" BodyStyle="background-color:Transparent;">
        </ext:Window>
        <ext:Window ID="AllcostWin" runat="server" Icon="Group" Title="全成本设置" Width="400" Height="250"
            AutoShow="false" Modal="true" CenterOnLoad="true" AutoScroll="true" ShowOnLoad="false"
            Resizable="true" StyleSpec="background-color:Transparent;" BodyStyle="background-color:Transparent;">
        </ext:Window>
    </div>
    </form>
</body>
</html>
