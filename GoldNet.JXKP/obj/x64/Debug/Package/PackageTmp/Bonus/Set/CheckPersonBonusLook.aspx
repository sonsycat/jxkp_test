<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CheckPersonBonusLook.aspx.cs"
    Inherits="GoldNet.JXKP.CheckPersonBonusLook" %>

<%@ Register Assembly="Goldnet.Ext.Web" Namespace="Goldnet.Ext.Web" TagPrefix="ext" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link rel="stylesheet" type="text/css" href="../Orthers/Cbouns.css" />

    <script type="text/javascript">
        function selectDept(combox) {
            var id = combox.value;            
            Store1.filterBy(getRecordFilter(id));
            Store5.filterBy(getRecordFilter(id));
            RenderTotalData(Store1);
        }
        var getRecordFilter = function(id) {
            var f = [];
            f.push({
                filter: function(record) {
                    return filterString(id, 'DEPTID', record);
                }
            });
            var len = f.length;
            return function(record) {
                if (id == '00000') {
                    return true;
                }
                if (f[0].filter(record)) {
                    return true;
                }
                else {
                    return false;
                }
            }

        }
        var filterString=function(value,dataIndex,record){

            var val=record.get(dataIndex);
            if(typeof val!="string"){
                return value.length==0;
            }    
            return val.toLowerCase().indexOf(value.toLowerCase())>-1;
        }
        var filterDate=function(value,dataIndex,record){
            var val=record.get(dataIndex).clearTime(true).getTime();
            
            if(!Ext.isEmpty(value,false) && val!=value.clearTime(true).getTime()){
                return false;
            }
            return true;
        }
        var filterNumber=function(value,dataIndex,record){
            var val=record.get(dataIndex);            
             if(!Ext.isEmpty(value,false) && val!=value)
             {
                return false;
             }
             return true;
        }
         
        var SelectorLayout = function() {
            SelectorLeft.setHeight(Ext.lib.Dom.getViewHeight() - SelectorLeft.getPosition()[1] - 5);
            SelectorRight.setHeight(Ext.lib.Dom.getViewHeight() - SelectorRight.getPosition()[1] - 5);
        }
        var CountrySelector = {
            add: function(source, destination) {
                source = source || GridPanel1;
                destination = destination || GridPanel2;
                if (source.hasSelection()) {
                    destination.store.add(source.selModel.getSelections());
                    source.deleteSelected();
                }
                RenderTotalData(destination.store);
            },
            addAll: function(source, destination) {
                source = source || GridPanel1;
                destination = destination || GridPanel2;
                destination.store.add(source.store.getRange());
                source.store.removeAll();
                RenderTotalData(destination.store);
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
        function rowselect(grid, rowIndex, columnIndex) {
            var model = grid.getSelectionModel()
            if (columnIndex != 0 & columnIndex != 1 & columnIndex != 2) {                
                    model.deselectRow(rowIndex);
                }

            };
       function RenderTotalData(store) {
                var rcount = store.getCount();
                var bonusvalue=0;
                var daysvalue = 0;
                var modulusvalue = 0;
                var modulusvalue2 = 0;
                var modulusvalue3 = 0;
                for (var i = 0; i < rcount; i++) {
                    var record = Store1.getAt(i);
                    bonusvalue =bonusvalue+record.get('ISBONUS');
                    daysvalue =daysvalue+Number(record.get('DAYS'));
                    modulusvalue =modulusvalue+Number(record.get('BONUSMODULUS'));
                    modulusvalue2 =modulusvalue2+Number(record.get('PERSONSMODULUS'));
                    modulusvalue3 =modulusvalue3+Number(record.get('SUBSIDYMODULUS'));
                }
                TextField3.setValue('共有' + bonusvalue + '人发放奖金');
                TextField4.setValue('共有' + daysvalue + '天工作日');
                TextField5.setValue('合计：' + modulusvalue);
                TextField6.setValue('合计：' + modulusvalue2);
                TextField7.setValue('合计：' + modulusvalue3);
            
            }
       function totalData(cell) {
           var rcount = Store1.getCount();
           var total=0;
           var columnvalue;
           for (var i = 0; i < rcount; i++) {           
               var record = Store1.getAt(i);
               if (cell.column == 3) {
                    columnvalue = record.get('ISBONUS');  
               }
               else if (cell.column == 4) {
                    columnvalue = record.get('DAYS');
               }
               else if (cell.column == 5) {
                    columnvalue = record.get('BONUSMODULUS');
                }
               else if (cell.column == 6) {
                    columnvalue = record.get('PERSONSMODULUS');
                }
               else if (cell.column == 7) {
                    columnvalue = record.get('SUBSIDYMODULUS');
                }
                total = total + Number(columnvalue);
            }
            if (cell.column == 3) {
                TextField3.setValue('共有' + total + '人发放奖金');
            }
            else if (cell.column == 4) {
                TextField4.setValue('共有' + total + '天工作日') ;
             }
            else if (cell.column == 5)
            {
                TextField5.setValue( '合计：' + total);
            }
            else if (cell.column == 6)
            {
                TextField6.setValue( '合计：' + total);
            }
            else if (cell.column == 7)
            {
                TextField7.setValue( '合计：' + total);
            }
          
       }
        var RefreshData = function() {
            Store1.reload();
            Store5.reload();
        }   
    </script>

</head>
<body>
    <ext:ScriptManager ID="ScriptManager1" runat="server" />
    <form id="form1" runat="server">
    <ext:Store ID="Store1" AutoLoad="true" runat="server" OnRefreshData="Store_RefreshData">
        <Reader>
            <ext:JsonReader>
                <Fields>
                    <ext:RecordField Name="YEARS">
                    </ext:RecordField>
                    <ext:RecordField Name="MONTHS">
                    </ext:RecordField>
                    <ext:RecordField Name="DEPTID">
                    </ext:RecordField>
                    <ext:RecordField Name="DEPTNAME">
                    </ext:RecordField>
                    <ext:RecordField Name="STAFFNAME">
                    </ext:RecordField>
                    <ext:RecordField Name="ISBONUS">
                    </ext:RecordField>
                    <ext:RecordField Name="DAYS">
                    </ext:RecordField>
                    <ext:RecordField Name="BONUSMODULUS">
                    </ext:RecordField>
                    <ext:RecordField Name="PERSONSMODULUS" />
                    <ext:RecordField Name="SUBSIDYMODULUS" />
                </Fields>
            </ext:JsonReader>
        </Reader>
        <Listeners>
            <Load Fn="RenderTotalData" />
        </Listeners>
    </ext:Store>
    <ext:Store ID="Store3" runat="server">
        <Reader>
            <ext:JsonReader ReaderID="YEAR">
                <Fields>
                    <ext:RecordField Name="YEAR" />
                </Fields>
            </ext:JsonReader>
        </Reader>
    </ext:Store>
    <ext:Store ID="Store4" runat="server">
        <Reader>
            <ext:JsonReader ReaderID="MONTH">
                <Fields>
                    <ext:RecordField Name="MONTH" />
                </Fields>
            </ext:JsonReader>
        </Reader>
    </ext:Store>
    <div>
        <ext:ViewPort ID="ViewPort111" runat="server">
            <Body>
                <ext:ColumnLayout ID="ColumnLayout11" runat="server" Split="true" FitHeight="true">
                    <Columns>
                        <ext:LayoutColumn ColumnWidth="1">
                            <ext:Panel runat="server" ID="panel11" Border="false">
                                <Body>
                                    <ext:BorderLayout ID="BorderLayout1" runat="server">
                                        <Center>
                                            <ext:GridPanel ID="GridPanel2" runat="server" Border="false" StoreID="Store1" StripeRows="true"
                                                TrackMouseOver="true" Height="480" Enabled="true" ClicksToEdit="1">
                                                <TopBar>
                                                    <ext:Toolbar ID="Toolbar_detptype" runat="server" Visible="true" AutoWidth="true">
                                                        <Items>
                                                            <ext:Label ID="lcaption" runat="server" Text="核算年月份:">
                                                            </ext:Label>
                                                            <ext:ComboBox ID="cbbYear" runat="server" ReadOnly="true" StoreID="Store3" Width="70"
                                                                DisplayField="YEAR" ValueField="YEAR" ForceSelection="true" SelectOnFocus="true">
                                                                <AjaxEvents>
                                                                    <Select OnEvent="btn_Qurey_Click">
                                                                    </Select>
                                                                </AjaxEvents>
                                                            </ext:ComboBox>
                                                            <ext:Label ID="lYear" runat="server" Text="年">
                                                            </ext:Label>
                                                            <ext:ComboBox ID="cbbmonth" runat="server" ReadOnly="true" StoreID="Store4" Width="70"
                                                                DisplayField="MONTH" ValueField="MONTH">
                                                                <AjaxEvents>
                                                                    <Select OnEvent="btn_Qurey_Click">
                                                                    </Select>
                                                                </AjaxEvents>
                                                            </ext:ComboBox>
                                                            <ext:Label ID="lmonth" runat="server" Text="月">
                                                            </ext:Label>
                                                            <ext:ToolbarSeparator ID="ToolbarSeparator2" runat="server" />
                                                            <ext:Button ID="Btn_Back" runat="server" Text="返回到列表" Icon="ReverseGreen">
                                                                <AjaxEvents>
                                                                    <Click OnEvent="btn_Back_Click">
                                                                    </Click>
                                                                </AjaxEvents>
                                                            </ext:Button>
                                                        </Items>
                                                    </ext:Toolbar>
                                                </TopBar>
                                                <ColumnModel ID="ColumnModel22" runat="server" Enabled="false">
                                                    <Columns>
                                                        <ext:Column ColumnID="DEPTNAME" Header="平均奖科室" Width="100" DataIndex="DEPTNAME" MenuDisabled="true"
                                                            Align="Center">
                                                        </ext:Column>
                                                        <ext:Column ColumnID="STAFFNAME" Header="科室人员" Width="100" DataIndex="STAFFNAME"
                                                            Align="Center" MenuDisabled="true">
                                                        </ext:Column>
                                                        <ext:CheckColumn ColumnID="ISBONUS" Header="是否发放奖金" Width="100" DataIndex="ISBONUS"
                                                            Align="Center" MenuDisabled="true" Editable="false">
                                                        </ext:CheckColumn>
                                                        <ext:Column ColumnID="DAYS" Header="<div style='text-align:center;'>工作日数</div>" Width="100"
                                                            DataIndex="DAYS" MenuDisabled="true" Align="Right">
                                                        </ext:Column>
                                                        <ext:Column ColumnID="BONUSMODULUS" Header="<div style='text-align:center;'>岗位系数</div>"
                                                            Width="100" DataIndex="BONUSMODULUS" Align="Right" MenuDisabled="true">
                                                        </ext:Column>
                                                        <ext:Column ColumnID="PERSONSMODULUS" Header="<div style='text-align:center;'>人员系数</div>"
                                                            Width="100" DataIndex="PERSONSMODULUS" Align="Right" MenuDisabled="true">
                                                        </ext:Column>
                                                        <ext:Column ColumnID="SUBSIDYMODULUS" Header="<div style='text-align:center;'>补贴系数</div>"
                                                            Width="100" DataIndex="SUBSIDYMODULUS" Align="Right" MenuDisabled="true">
                                                        </ext:Column>
                                                    </Columns>
                                                </ColumnModel>
                                                <SelectionModel>
                                                    <ext:CheckboxSelectionModel ID="CheckboxSelectionModel1" runat="server">
                                                    </ext:CheckboxSelectionModel>
                                                </SelectionModel>
                                                <Listeners>
                                                    <CellClick Fn="rowselect" />
                                                    <AfterEdit Fn="totalData" />
                                                </Listeners>
                                                <LoadMask ShowMask="true" />
                                                <View>
                                                    <ext:GridView ID="GridView1" runat="server">
                                                        <HeaderRows>
                                                            <ext:HeaderRow>
                                                                <Columns>
                                                                    <ext:HeaderColumn>
                                                                    </ext:HeaderColumn>
                                                                    <ext:HeaderColumn>
                                                                    </ext:HeaderColumn>
                                                                    <ext:HeaderColumn>
                                                                    </ext:HeaderColumn>
                                                                    <ext:HeaderColumn>
                                                                        <Component>
                                                                            <ext:TextField runat="server" ID="TextField3" ReadOnly="true">
                                                                            </ext:TextField>
                                                                        </Component>
                                                                    </ext:HeaderColumn>
                                                                    <ext:HeaderColumn>
                                                                        <Component>
                                                                            <ext:TextField runat="server" ID="TextField4" ReadOnly="true">
                                                                            </ext:TextField>
                                                                        </Component>
                                                                    </ext:HeaderColumn>
                                                                    <ext:HeaderColumn>
                                                                        <Component>
                                                                            <ext:TextField runat="server" ID="TextField5" ReadOnly="true">
                                                                            </ext:TextField>
                                                                        </Component>
                                                                    </ext:HeaderColumn>
                                                                    <ext:HeaderColumn>
                                                                        <Component>
                                                                            <ext:TextField runat="server" ID="TextField6" ReadOnly="true">
                                                                            </ext:TextField>
                                                                        </Component>
                                                                    </ext:HeaderColumn>
                                                                    <ext:HeaderColumn>
                                                                        <Component>
                                                                            <ext:TextField runat="server" ID="TextField7" ReadOnly="true">
                                                                            </ext:TextField>
                                                                        </Component>
                                                                    </ext:HeaderColumn>
                                                                </Columns>
                                                            </ext:HeaderRow>
                                                        </HeaderRows>
                                                    </ext:GridView>
                                                </View>
                                            </ext:GridPanel>
                                        </Center>
                                    </ext:BorderLayout>
                                </Body>
                            </ext:Panel>
                        </ext:LayoutColumn>
                    </Columns>
                </ext:ColumnLayout>
            </Body>
        </ext:ViewPort>
        </ext:Window>
    </div>
    </form>
</body>
</html>
