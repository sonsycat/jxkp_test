<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="xyhs_cost_item_set.aspx.cs" Inherits="GoldNet.JXKP.cbhs.xyhs.xyhs_cost_item_set" %>
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
    <link rel="stylesheet" type="text/css" href="../../Bonus/Orthers/Cbouns.css" />
    <script language="javascript" type="text/javascript">
        var RefreshData = function(msg) {
           
            Store1.reload();
//            TreeCtrl;
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
        function getcheckednode(node) {
            btn_Add.enable();
            var Nodeid = "";    
                if (node.id == 'root') {
                    Nodeid = "";
                } else {
                    Nodeid = node.id;
                }
               Store1.filterBy(getRecordFilter(Nodeid));
        }
        var getRecordFilter = function(nodeid) {
            var f = [];
            f.push({
                filter: function(record) {
                return filterString(nodeid, 'ITEM_TYPE', record);
                }
            });           
           var len = f.length;
              return function(record) {             
                  if (f[0].filter(record)) {
                      return true;
                  }
                  else {
                      return false;
                  }
              }
               
        };         
        var filterString = function(value, dataIndex, record) {
            var val = record.get(dataIndex);
            if (typeof val != "string") {
                return value.length == 0;
            }
            return val.toLowerCase().indexOf(value.toLowerCase()) > -1;
        };
        function getDeptCheckedNode() {
            return  TreeCtrl.getSelectionModel().getSelectedNode().id;
                    }

    </script>

</head>
<body>
    <form id="form1" runat="server">
    <div>
        <ext:ScriptManager ID="ScriptManager1" runat="server" />
        <ext:Store ID="Store1" runat="server"  GroupField="ITEM_TYPE_NAME" OnRefreshData="Store_RefreshData">
            <Reader>
                <ext:JsonReader ReaderID="ITEM_CODE">
                    <Fields>
                        <ext:RecordField Name="ITEM_TYPE_CODE" />
                        <ext:RecordField Name="ITEM_TYPE_NAME" />
                        <ext:RecordField Name="ITEM_CODE" />
                        <ext:RecordField Name="ITEM_NAME" />
                       
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
                                <ext:Button ID="Button1" runat="server" Text="添加类别" Icon="NoteEdit">
                                    <AjaxEvents>
                                        <Click OnEvent="Button_add_click">
                                            <EventMask Msg="载入中..." ShowMask="true" />
                                        </Click>
                                    </AjaxEvents>
                                </ext:Button>
                                <ext:Button ID="btn_Modify" runat="server" Text="修改类别" Icon="NoteEdit">
                                    <AjaxEvents>
                                        <Click OnEvent="Button_edit_click">
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
                                 <ext:Button ID="allcostbset" runat="server" Text="全成本设置" Icon="NoteEdit" >
                                    <AjaxEvents>
                                        <Click OnEvent="Button_allcostbset_click">
                                            <EventMask Msg="载入中..." ShowMask="true" />
                                        </Click>
                                    </AjaxEvents>
                                </ext:Button>
                            </Items>
                        </ext:Toolbar>
                    </North>
                    
                    <Center>
                         <ext:GridPanel ID="GridPanel1" runat="server" StoreID="Store1" StripeRows="true"
                                TrackMouseOver="true" AutoWidth="true" Height="480" Border="false">                                
                                <ColumnModel ID="ColumnModel1" runat="server">
                                    <Columns>
                                         <ext:Column ColumnID="ITEM_TYPE_NAME" Header="类别" Width="100"  Sortable="true"
                                            DataIndex="ITEM_TYPE_NAME" MenuDisabled="true" Align="Left" />
                                        <ext:Column ColumnID="ITEM_NAME" Header="<div style='text-align:center;'>项目名称</div>" Width="100"  Sortable="true"
                                            DataIndex="ITEM_NAME" MenuDisabled="true"  Align="Left"  />
                                        <ext:Column ColumnID="ITEM_CODE" Header="<div style='text-align:center;'>项目代码</div>" Width="70"  Sortable="true"
                                            DataIndex="ITEM_CODE" MenuDisabled="true"   Align="Left" />
                                        <ext:Column ColumnID="ITEM_TYPE_NAME" Header="项目类别" Width="80"  Sortable="true"
                                            DataIndex="ITEM_TYPE_NAME" MenuDisabled="true"  Align="Center"  />
                                        
                                    </Columns>
                                </ColumnModel>
                                <AjaxEvents>
                                    <RowDblClick OnEvent="Button_edit_click" />
                                </AjaxEvents>
                                <SelectionModel>
                                    <ext:RowSelectionModel ID="RowSelectionModel1" runat="server" SingleSelect="true">
                                       
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
                                
                            </ext:GridPanel>
                    </Center>
                </ext:BorderLayout>
            </Body>
        </ext:ViewPort>
    </div>      
  
         <ext:Window ID="TypeWin" runat="server" Icon="Group" Title="类别设置" Width="400" Height="200"
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
