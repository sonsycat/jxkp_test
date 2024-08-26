<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AverageBonusDaysDetail.aspx.cs"
    Inherits="GoldNet.JXKP.AverageBonusDaysDetail" %>

<%@ Register Assembly="Goldnet.Ext.Web" Namespace="Goldnet.Ext.Web" TagPrefix="ext" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <link rel="stylesheet" type="text/css" href="../Orthers/Cbouns.css" />

    <script type="text/javascript">
        function backToList() {
            window.navigate("AverageBonusDaysList.aspx");
        }
        function selectDept(combox) {
            var id = combox.value;
            Store1.filterBy(getRecordFilter(id));
            Store5.filterBy(getRecordFilter(id));
            RenderTotalData(Store1);
        }
        var getRecordFilter = function (id) {
            var f = [];
            f.push({
                filter: function (record) {
                    return filterString(id, 'DEPTID', record);
                }
            });
            var len = f.length;
            return function (record) {
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
        var filterString = function (value, dataIndex, record) {

            var val = record.get(dataIndex);
            if (typeof val != "string") {
                return value.length == 0;
            }
            //           return val.toLowerCase().indexOf(value.toLowerCase())>-1;
            return val.toLowerCase() == value.toLowerCase();
        }
        var filterDate = function (value, dataIndex, record) {
            var val = record.get(dataIndex).clearTime(true).getTime();

            if (!Ext.isEmpty(value, false) && val != value.clearTime(true).getTime()) {
                return false;
            }
            return true;
        }
        var filterNumber = function (value, dataIndex, record) {
            var val = record.get(dataIndex);
            if (!Ext.isEmpty(value, false) && val != value) {
                return false;
            }
            return true;
        }

        var SelectorLayout = function () {
            SelectorLeft.setHeight(Ext.lib.Dom.getViewHeight() - SelectorLeft.getPosition()[1] - 5);
            SelectorRight.setHeight(Ext.lib.Dom.getViewHeight() - SelectorRight.getPosition()[1] - 5);
        }
        var CountrySelector = {
            add: function (source, destination) {
                source = source || GridPanel1;
                destination = destination || GridPanel2;
                if (source.hasSelection()) {
                    destination.store.add(source.selModel.getSelections());
                    source.deleteSelected();
                }
                RenderTotalData(destination.store);
            },
            addAll: function (source, destination) {
                source = source || GridPanel1;
                destination = destination || GridPanel2;
                destination.store.add(source.store.getRange());
                source.store.removeAll();
                RenderTotalData(destination.store);
            },
            addByName: function (name) {
                if (!Ext.isEmpty(name)) {
                    var result = Store1.query("Name", name);
                    if (!Ext.isEmpty(result.items)) {
                        GridPanel2.store.add(result.items[0]);
                        GridPanel1.store.remove(result.items[0]);
                    }
                }
            },
            addByNames: function (name) {
                for (var i = 0; i < name.length; i++) {
                    this.addByName(name[i]);
                }
            },
            remove: function (source, destination) {
                this.add(destination, source);
            },
            removeAll: function (source, destination) {
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
            var bonusvalue = 0;
            var daysvalue = 0;
            var modulusvalue = 0;
            var oblimodulusvalue = 0;
            for (var i = 0; i < rcount; i++) {
                var record = Store1.getAt(i);
                bonusvalue = bonusvalue + record.get('ISBONUS');
                daysvalue = daysvalue + Number(record.get('DAYS'));
                modulusvalue = modulusvalue + Number(record.get('BONUSMODULUS'));
                oblimodulusvalue = oblimodulusvalue + Number(record.get('OBLIGATION'));
            }
            TextField3.setValue('共有' + bonusvalue + '人发放奖金');
            TextField4.setValue('合计：' + daysvalue);
            TextField5.setValue('合计：' + modulusvalue.toFixed(2));
            //TextField6.setValue('合计：' + oblimodulusvalue);

        }
        function totalData(cell) {
            var rcount = Store1.getCount();
            var total = 0;
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
                    columnvalue = record.get('OBLIGATION');
                }
                total = total + Number(columnvalue);
            }
            if (cell.column == 3) {
                TextField3.setValue('共有' + total + '人发放奖金');
            }
            else if (cell.column == 4) {
                TextField4.setValue('合计：' + total);
            }
            else if (cell.column == 5) {
                TextField5.setValue('合计：' + total.toFixed(2));
            }
            //            else if (cell.column == 6)
            //            {
            //                TextField6.setValue( '责任奖系数合计：' + total);
            //            }

        }
        var RefreshData = function () {
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
                    <ext:RecordField Name="OBLIGATION">
                        <%--责任奖系数--%>
                    </ext:RecordField>
                    <ext:RecordField Name="EMP_NO">
                    </ext:RecordField>
                    <ext:RecordField Name="QUA_VAL">
                    </ext:RecordField>
                    <ext:RecordField Name="STAFF_ID">
                    </ext:RecordField>
                </Fields>
            </ext:JsonReader>
        </Reader>
        <Listeners>
            <Load Fn="RenderTotalData" />
        </Listeners>
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
    <ext:Store ID="Store5" runat="server" OnRefreshData="Store_RefreshData">
        <Reader>
            <ext:JsonReader ReaderID="STAFFID">
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
                    <ext:RecordField Name="OBLIGATION">
                        <%--责任奖系数--%>
                    </ext:RecordField>
                    <ext:RecordField Name="EMP_NO">
                    </ext:RecordField>
                    <ext:RecordField Name="QUA_VAL">
                    </ext:RecordField>
                    <ext:RecordField Name="STAFF_ID">
                    </ext:RecordField>
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
                                                            </ext:ComboBox>
                                                            <ext:Label ID="lYear" runat="server" Text="年">
                                                            </ext:Label>
                                                            <ext:ComboBox ID="cbbmonth" runat="server" ReadOnly="true" StoreID="Store4" Width="70"
                                                                DisplayField="MONTH" ValueField="MONTH">
                                                            </ext:ComboBox>
                                                            <ext:Label ID="lmonth" runat="server" Text="月">
                                                            </ext:Label>
                                                            <ext:ToolbarSeparator ID="ToolbarSeparator1" runat="server" />
                                                            <ext:ComboBox runat="server" Title="科室" ID="cbbdept" StoreID="Store2" Width="100"
                                                                DisplayField="DEPTNAME" ValueField="DEPTID" ReadOnly="true">
                                                            </ext:ComboBox>
                                                            <ext:TextField ID="staffname" runat="server" EmptyText="姓名" Visible="false">
                                                            </ext:TextField>
                                                            <ext:ToolbarSeparator ID="ToolbarSeparator3" runat="server" />
                                                            <ext:Button ID="Btn_Qurey" Text="查询" Icon="FolderMagnify" runat="server">
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
                                                            <%-- <ext:Button ID="Btn_Ref" runat="server" Text="刷新" Icon="ArrowRefresh">
                                                                <AjaxEvents>
                                                                    <Click OnEvent="btn_Refresh_Click"></Click>
                                                                </AjaxEvents>
                                                            </ext:Button>--%>
                                                            <ext:ToolbarSeparator ID="ToolbarSeparator4" runat="server" />
                                                            <ext:Button ID="Btn_Add" Text="增加人员" Icon="Add" runat="server">
                                                                <AjaxEvents>
                                                                    <Click OnEvent="btn_Add_Click">
                                                                    </Click>
                                                                </AjaxEvents>
                                                            </ext:Button>
                                                            <ext:Button ID="Btn_Del" Text="删除人员" Icon="Delete" runat="server" Disabled="true">
                                                                <AjaxEvents>
                                                                    <Click OnEvent="btn_Delete_Click">
                                                                        <Confirmation BeforeConfirm="config.confirmation.message = '你确定要删除吗？';" Title="系统提示"
                                                                            ConfirmRequest="true" />
                                                                        <ExtraParams>
                                                                            <ext:Parameter Name="Values" Value="Ext.encode(#{GridPanel2}.getRowsValues())" Mode="Raw">
                                                                            </ext:Parameter>
                                                                        </ExtraParams>
                                                                    </Click>
                                                                </AjaxEvents>
                                                            </ext:Button>
                                                            <ext:ToolbarSeparator ID="ToolbarSeparator2" runat="server" />
                                                            <ext:Button ID="Button3" Text="同步系数" Icon="Add" runat="server">
                                                                <AjaxEvents>
                                                                    <Click OnEvent="btn_Attendance_Click">
                                                                        <Confirmation BeforeConfirm="config.confirmation.message = '你确定要同步上一个月的人员系数吗？';" Title="系统提示"
                                                                            ConfirmRequest="true" />
                                                                    </Click>
                                                                </AjaxEvents>
                                                            </ext:Button>
                                                            <ext:ToolbarSeparator ID="ToolbarSeparator5" runat="server" />
                                                            <ext:Button ID="Btn_Back" runat="server" Text="返回到列表" Icon="ReverseGreen">
                                                                <Listeners>
                                                                    <Click Fn="backToList" />
                                                                </Listeners>
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
                                                        <ext:CheckColumn ColumnID="ISBONUS" Header="是否发放奖金" Width="150" DataIndex="ISBONUS"
                                                            Align="Center" MenuDisabled="true" Editable="true" >
                                                        </ext:CheckColumn>
                                                        <ext:Column ColumnID="DAYS" Header="<div style='text-align:center;'>工作日数</div>" Width="120"
                                                            DataIndex="DAYS" MenuDisabled="true" Align="Right">
                                                            <Editor>
                                                                <ext:NumberField runat="server" ID="nfdays" SelectOnFocus="true" DecimalPrecision="2"
                                                                    StyleSpec="text-align:right">
                                                                </ext:NumberField>
                                                            </Editor>
                                                        </ext:Column>
                                                        <ext:Column ColumnID="BONUSMODULUS" Header="<div style='text-align:center;'>岗位系数</div>"
                                                            Width="120" DataIndex="BONUSMODULUS" Align="Right" MenuDisabled="true">
                                                            <Editor>
                                                                <ext:NumberField runat="server" ID="nfbonusmodules" SelectOnFocus="true" DecimalPrecision="4">
                                                                </ext:NumberField>
                                                            </Editor>
                                                        </ext:Column>
                                                        <ext:Column ColumnID="OBLIGATION" Header="<div style='text-align:center;'>科室系数</div>"
                                                            Width="120" DataIndex="OBLIGATION" Align="Right" MenuDisabled="true">
                                                            <Editor>
                                                                <ext:NumberField runat="server" ID="nfobligation" SelectOnFocus="true" DecimalPrecision="4">
                                                                </ext:NumberField>
                                                            </Editor>
                                                        </ext:Column>
                                                        <ext:Column ColumnID="QUA_VAL" Header="<div style='text-align:center;'>是否质量</div>"
                                                            Width="120" DataIndex="QUA_VAL" Align="Right" MenuDisabled="true">
                                                            <Editor>
                                                                <ext:NumberField runat="server" ID="NumberField1" SelectOnFocus="true" DecimalPrecision="4">
                                                                </ext:NumberField>
                                                            </Editor>
                                                        </ext:Column>
                                                    </Columns>
                                                </ColumnModel>
                                                <SelectionModel>
                                                    <ext:CheckboxSelectionModel ID="CheckboxSelectionModel1" runat="server">
                                                        <Listeners>
                                                            <RowSelect Handler="#{Btn_Del}.enable();" />
                                                            <RowDeselect Handler="if (!#{GridPanel2}.hasSelection()) {#{Btn_Del}.disable();}" />
                                                        </Listeners>
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
                                                                </Columns>
                                                            </ext:HeaderRow>
                                                        </HeaderRows>
                                                    </ext:GridView>
                                                </View>
                                            </ext:GridPanel>
                                        </Center>
                                        <East Collapsible="True" Split="True" MinWidth="40" MaxWidth="248">
                                            <ext:Panel runat="server" ID="PanelEast" Width="248" Title="平均奖科室人员" Border="false">
                                                <Body>
                                                    <ext:ColumnLayout ID="ColumnLayout2" runat="server" Split="true" FitHeight="true">
                                                        <Columns>
                                                            <ext:LayoutColumn ColumnWidth="0.16">
                                                                <ext:Panel ID="Panel4" runat="server" Border="false">
                                                                    <Body>
                                                                        <ext:AnchorLayout ID="AnchorLayout1" runat="server">
                                                                            <ext:Anchor Vertical="40%">
                                                                                <ext:Panel ID="Panel1" runat="server" Border="false" BodyStyle="background-color: transparent;" />
                                                                            </ext:Anchor>
                                                                            <ext:Anchor>
                                                                                <ext:Panel ID="Panel3" runat="server" Border="false" BodyStyle="padding:5px;background-color: transparent;">
                                                                                    <Body>
                                                                                        <ext:Button ID="Button1" runat="server" Icon="ResultsetPrevious" StyleSpec="margin-bottom:2px;">
                                                                                            <Listeners>
                                                                                                <Click Handler="CountrySelector.add();" />
                                                                                            </Listeners>
                                                                                            <ToolTips>
                                                                                                <ext:ToolTip ID="ToolTip1" runat="server" Title="添加" Html="添加选中行" />
                                                                                            </ToolTips>
                                                                                        </ext:Button>
                                                                                        <ext:Button ID="Button2" runat="server" Icon="ResultsetFirst" StyleSpec="margin-bottom:2px;">
                                                                                            <Listeners>
                                                                                                <Click Handler="CountrySelector.addAll();" />
                                                                                            </Listeners>
                                                                                            <ToolTips>
                                                                                                <ext:ToolTip ID="ToolTip2" runat="server" Title="全部添加" Html="全部添加" />
                                                                                            </ToolTips>
                                                                                        </ext:Button>
                                                                                    </Body>
                                                                                </ext:Panel>
                                                                            </ext:Anchor>
                                                                        </ext:AnchorLayout>
                                                                    </Body>
                                                                </ext:Panel>
                                                            </ext:LayoutColumn>
                                                            <ext:LayoutColumn ColumnWidth="0.84">
                                                                <ext:GridPanel runat="server" ID="GridPanel1" StoreID="Store5">
                                                                    <ColumnModel ID="ColumnModel2" runat="server">
                                                                        <Columns>
                                                                            <ext:Column ColumnID="STAFFNAME" Header="姓名" Width="80" DataIndex="STAFFNAME" MenuDisabled="true" />
                                                                        </Columns>
                                                                        <Columns>
                                                                            <ext:Column ColumnID="DEPTNAME" Header="科室名称" Width="100" DataIndex="DEPTNAME" MenuDisabled="true" />
                                                                        </Columns>
                                                                    </ColumnModel>
                                                                    <SelectionModel>
                                                                        <ext:CheckboxSelectionModel ID="edit" runat="server">
                                                                        </ext:CheckboxSelectionModel>
                                                                    </SelectionModel>
                                                                </ext:GridPanel>
                                                            </ext:LayoutColumn>
                                                        </Columns>
                                                    </ext:ColumnLayout>
                                                </Body>
                                            </ext:Panel>
                                        </East>
                                    </ext:BorderLayout>
                                </Body>
                            </ext:Panel>
                        </ext:LayoutColumn>
                    </Columns>
                </ext:ColumnLayout>
            </Body>
        </ext:ViewPort>
        <ext:Window ID="DetailWin" runat="server" Icon="Group" Title="增加人员" Width="350" Height="280"
            AutoShow="false" Modal="true" CenterOnLoad="true" AutoScroll="true" ShowOnLoad="false"
            Resizable="false" StyleSpec="background-color:Transparent;" BodyStyle="background-color:Transparent;">
        </ext:Window>
    </div>
    </form>
</body>
</html>
