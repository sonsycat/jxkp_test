<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CostPowerManager.aspx.cs"
    Inherits="GoldNet.JXKP.WebPage.SpecManager.CostPowerManager" %>

<%@ Register Assembly="Goldnet.Ext.Web" Namespace="Goldnet.Ext.Web" TagPrefix="ext" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style type="text/css">
        body
        {
            background-color: #DFE8F6;
            font-size: 12px;
        }
    </style>

    <script language="javascript" type="text/javascript">
        var RefreshData = function(msg) {
            
            Store1.reload();
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
    </script>

</head>
<body>
    <form id="form1" runat="server">
    <div>
        <ext:ScriptManager ID="ScriptManager1" runat="server" />
        <ext:Store ID="Store1" runat="server" GroupField="ITEM_TYPE" OnRefreshData="Store_RefreshData"
            AutoLoad="true">
            <Reader>
                <ext:JsonReader ReaderID="ITEM_CODE">
                    <Fields>
                        <ext:RecordField Name="ITEM_TYPE" />
                        <ext:RecordField Name="ITEM_CODE" />
                        <ext:RecordField Name="ITEM_NAME" />
                        <ext:RecordField Name="INPUT_CODE" />
                        <ext:RecordField Name="GETTYPE" />
                        <ext:RecordField Name="ACCOUNT_TYPE" />
                    </Fields>
                </ext:JsonReader>
            </Reader>
        </ext:Store>
        <div>
            <ext:ViewPort ID="ViewPort2" runat="server">
                <Body>
                    <ext:BorderLayout ID="BorderLayout2" runat="server">
                        <West Collapsible="false" Split="false" CollapseMode="Mini">
                            <ext:Panel ID="Panel2" runat="server" Width="175" BodyBorder="false" AutoScroll="true"
                                Border="false">
                                <Body>
                                    <ext:TreePanel runat="server" Width="175" ID="TreeCtrl" AutoHeight="true" AutoScroll="false"
                                        Border="false">
                                        <Listeners>
                                            <Click Handler="getcheckednode(node)" />
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
                                        <ext:Column ColumnID="ITEM_TYPE" Header="类别" Width="100" Sortable="true" DataIndex="ITEM_TYPE"
                                            MenuDisabled="true" Align="Center" />
                                        <ext:Column ColumnID="ITEM_NAME" Header="项目名称" Width="100" Sortable="true" DataIndex="ITEM_NAME"
                                            MenuDisabled="true" Align="Center" />
                                        <ext:Column ColumnID="ITEM_CODE" Header="项目代码" Width="70" Sortable="true" DataIndex="ITEM_CODE"
                                            MenuDisabled="true" Align="Center" />
                                        <ext:Column ColumnID="INPUT_CODE" Header="输入码" Width="80" Sortable="true" DataIndex="INPUT_CODE"
                                            MenuDisabled="true" Align="Center" />
                                        <ext:CommandColumn Header="权限" Sortable="true" ColumnID="Columns5" Align="Center"
                                            Width="50">
                                            <Commands>
                                                <ext:GridCommand Icon="CogStart" CommandName="Show">
                                                </ext:GridCommand>
                                            </Commands>
                                        </ext:CommandColumn>
                                    </Columns>
                                </ColumnModel>
                                <SelectionModel>
                                    <ext:RowSelectionModel ID="RowSelectionModel1" runat="server" SingleSelect="true">
                                    </ext:RowSelectionModel>
                                </SelectionModel>
                                <View>
                                    <ext:GroupingView ID="GroupingView1" HideGroupedColumn="true" runat="server" GroupTextTpl='{text} ({[values.rs.length]})'
                                        EnableRowBody="false">
                                    </ext:GroupingView>
                                </View>
                                <AjaxEvents>
                                    <Command OnEvent="SetPower">
                                        <ExtraParams>
                                            <ext:Parameter Name="Values" Value="this.store.getAt(rowIndex).get('ITEM_CODE')"
                                                Mode="Raw">
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
        <ext:Window ID="PowerWin" runat="server" Icon="Group" Title="权限设置" Width="500" Height="450"
            AutoShow="false" Modal="true" CenterOnLoad="true" AutoScroll="true" ShowOnLoad="false"
            Resizable="true" StyleSpec="background-color:Transparent;" BodyStyle="background-color:Transparent;">
        </ext:Window>
    </div>
    </form>
</body>
</html>
