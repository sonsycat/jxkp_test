<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CheckPersonsEdit.aspx.cs"
    Inherits="GoldNet.JXKP.CheckPersonsEdit" %>

<%@ Register Assembly="Goldnet.Ext.Web" Namespace="Goldnet.Ext.Web" TagPrefix="ext" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <link rel="stylesheet" type="text/css" href="../Orthers/Cbouns.css" />

    <script type="text/javascript">
        function backToList() {
            window.navigate("CheckPersonsList.aspx");
        }
        function selectDept(combox) {
            var id = combox.value;
            Store1.filterBy(getRecordFilter(id));

        }
        var getRecordFilter = function(id) {
            var f = [];
            f.push({
                filter: function(record) {
                return filterString(id, 'DEPT_TYPE', record);
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
        var filterString = function(value, dataIndex, record) {

            var val = record.get(dataIndex);
            if (typeof val != "string") {
                return value.length == 0;
            }
            return val.toLowerCase().indexOf(value.toLowerCase()) > -1;
        }
        var filterDate = function(value, dataIndex, record) {
            var val = record.get(dataIndex).clearTime(true).getTime();

            if (!Ext.isEmpty(value, false) && val != value.clearTime(true).getTime()) {
                return false;
            }
            return true;
        }
        var filterNumber = function(value, dataIndex, record) {
            var val = record.get(dataIndex);
            if (!Ext.isEmpty(value, false) && val != value) {
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
        function rowselect(grid, rowIndex, columnIndex) {
            var model = grid.getSelectionModel()
            if (columnIndex != 0 & columnIndex != 1 & columnIndex != 2) {
                model.deselectRow(rowIndex);
            }
        }
        var RefreshData = function() {
            Store1.reload();
        }   
         var rmbMoney = function(v) {
                   v = (Math.round((v - 0) * 100)) / 100;
                   v = (v == Math.floor(v)) ? v + ".00" : ((v * 10 == Math.floor(v * 10)) ? v + "0" : v);
                   v = String(v);
                   var ps = v.split('.');
                   var whole = ps[0];
                   var sub = ps[1] ? '.' + ps[1] : '.00';
                   var r = /(\d+)(\d{3})/;
                   while (r.test(whole)) {
                       whole = whole.replace(r, '$1' + ',' + '$2');
                   }
                   v = whole + sub;
                   if (v.charAt(0) == '-') {
                       return '-' + v.substr(1);
                   }
                   return v;
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
                    <ext:RecordField Name="PERSONS">
                    </ext:RecordField>
                    <ext:RecordField Name="DOCTOR_NUMBERS">
                    </ext:RecordField>
                    <ext:RecordField Name="NURSE_NUMBERS">
                    </ext:RecordField>
                    <ext:RecordField Name="OTHER_NUMBERS">
                    </ext:RecordField>
                    <ext:RecordField Name="DEPT_TYPE">
                    </ext:RecordField>
                </Fields>
            </ext:JsonReader>
        </Reader>
    </ext:Store>
    <ext:Store ID="Store2" runat="server">
        <Reader>
            <ext:JsonReader ReaderID="DEPTID">
                <Fields>
                    <ext:RecordField Name="DEPTNAME" />
                    <ext:RecordField Name="DEPTID" />
                </Fields>
            </ext:JsonReader>
        </Reader>
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
    <ext:Store ID="Store5" runat="server">
        <Reader>
            <ext:JsonReader ReaderID="TYPECODE">
                <Fields>
                    <ext:RecordField Name="TYPECODE" />
                    <ext:RecordField Name="TYPENAME" />
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
                            <ext:GridPanel ID="GridPanel2" runat="server" Border="false" StoreID="Store1" StripeRows="true"
                                TrackMouseOver="true" Height="480" Enabled="true" ClicksToEdit="1">
                                <TopBar>
                                    <ext:Toolbar ID="Toolbar_detptype" runat="server" Visible="true" AutoWidth="true">
                                        <Items>
                                            <ext:Label ID="lcaption" runat="server" Text="核算年月份:">
                                            </ext:Label>
                                            <ext:ComboBox ID="cbbYear" runat="server" ReadOnly="true" StoreID="Store3" Width="70"
                                                DisplayField="YEAR" ValueField="YEAR" ForceSelection="true" SelectOnFocus="true">
                                            </ext:ComboBox>
                                            <ext:Label ID="lYear" runat="server" Text="年">
                                            </ext:Label>
                                            <ext:ComboBox ID="cbbmonth" runat="server" ReadOnly="true" StoreID="Store4" Width="70"
                                                DisplayField="MONTH" ValueField="MONTH">
                                            </ext:ComboBox>
                                            <ext:Label ID="lmonth" runat="server" Text="月">
                                            </ext:Label>
                                            <ext:ComboBox runat="server" Title="核算类别" ID="cbbdept" StoreID="Store5" Width="100"
                                                DisplayField="TYPENAME" ValueField="TYPECODE" ReadOnly="true">
                                                <Listeners>
                                                    <Select Fn="selectDept" />
                                                </Listeners>
                                            </ext:ComboBox>
                                            <ext:ToolbarSeparator ID="ToolbarSeparator3" runat="server" />
                                            <ext:Button ID="bquery" Text="查询" Icon="FolderMagnify" runat="server">
                                                <AjaxEvents>
                                                    <Click OnEvent="btn_Qurey_Click">
                                                    </Click>
                                                </AjaxEvents>
                                            </ext:Button>
                                            <ext:Button ID="Btn_Save" Text="保存" Icon="Disk" runat="server">
                                                <AjaxEvents>
                                                    <Click OnEvent="btn_Save_Click">
                                                        <EventMask Msg="正在保存" ShowMask="true" />
                                                        <ExtraParams>
                                                            <ext:Parameter Name="Values" Value="Ext.encode(#{GridPanel2}.getRowsValues(false))"
                                                                Mode="Raw">
                                                            </ext:Parameter>
                                                        </ExtraParams>
                                                    </Click>
                                                </AjaxEvents>
                                            </ext:Button>
                                            <ext:Button ID="Btn_Persons" Text="科室人员设置" Icon="GroupGo" runat="server" Visible="true">
                                                <AjaxEvents>
                                                    <Click OnEvent="btn_Persons_Click">
                                                        <EventMask Msg="页面转向中..." ShowMask="true" />
                                                        <ExtraParams>
                                                            <ext:Parameter Name="Values" Value="Ext.encode(#{GridPanel2}.getRowsValues())" Mode="Raw">
                                                            </ext:Parameter>
                                                        </ExtraParams>
                                                    </Click>
                                                </AjaxEvents>
                                            </ext:Button>
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
                                        <ext:Column ColumnID="DEPTNAME" Header="<center>核算科室</center>" Width="130" DataIndex="DEPTNAME" MenuDisabled="true"
                                            Align="Left">
                                        </ext:Column>
                                        <ext:Column ColumnID="PERSONS" Header="人数合计" Width="100" DataIndex="PERSONS" MenuDisabled="true"
                                            Align="Center">
                                            <Renderer Fn="rmbMoney" />
                                        </ext:Column>
                                        <ext:Column ColumnID="DOCTOR_NUMBERS" Header="<div style='text-align:center;'>医生人数</div>"
                                            Width="100" DataIndex="DOCTOR_NUMBERS" MenuDisabled="true" Align="Right">
                                            <Editor>
                                                <ext:NumberField runat="server" ID="nfdoctornumbers" SelectOnFocus="true" DecimalPrecision="2">
                                                </ext:NumberField>
                                            </Editor>
                                            <Renderer Fn="rmbMoney" />
                                        </ext:Column>
                                        <ext:Column ColumnID="NURSE_NUMBERS" Header="<div style='text-align:center;'>护士人数</div>"
                                            Width="100" DataIndex="NURSE_NUMBERS" MenuDisabled="true" Align="Right">
                                            <Editor>
                                                <ext:NumberField runat="server" ID="nfnursenumbers" SelectOnFocus="true" DecimalPrecision="2">
                                                </ext:NumberField>
                                            </Editor>
                                            <Renderer Fn="rmbMoney" />
                                        </ext:Column>
                                        <ext:Column ColumnID="OTHER_NUMBERS" Header="<div style='text-align:center;'>其它人数</div>"
                                            Width="100" DataIndex="OTHER_NUMBERS" MenuDisabled="true" Align="Right">
                                            <Editor>
                                                <ext:NumberField runat="server" ID="nfothernumbers" SelectOnFocus="true" DecimalPrecision="2">
                                                </ext:NumberField>
                                            </Editor>
                                            <Renderer Fn="rmbMoney" />
                                        </ext:Column>
                                    </Columns>
                                </ColumnModel>
                                <SelectionModel>
                                    <ext:RowSelectionModel ID="rowselectionModel1" runat="server">
                                    </ext:RowSelectionModel>
                                </SelectionModel>
                                <LoadMask ShowMask="true" />
                            </ext:GridPanel>
                        </ext:LayoutColumn>
                    </Columns>
                </ext:ColumnLayout>
            </Body>
        </ext:ViewPort>
    </div>
    </form>
</body>
</html>
