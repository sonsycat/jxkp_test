<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AverageBonusDaysLook.aspx.cs"
    Inherits="GoldNet.JXKP.AverageBonusDaysLook" %>

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
            RenderTotalData(Store1)

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
                oblimodulusvalue =oblimodulusvalue+Number(record.get('OBLIGATION'));
            }
            TextField3.setValue('共有' + bonusvalue + '人发放奖金');
            TextField4.setValue('共有' + daysvalue + '天工作日');
            TextField5.setValue('奖金系数合计：' + modulusvalue);
            TextField6.setValue('责任奖系数合计：' + oblimodulusvalue);

        }
    </script>

</head>
<body>
    <ext:ScriptManager ID="ScriptManager1" runat="server" />
    <form id="form1" runat="server">
    <ext:Store ID="Store1" AutoLoad="true" runat="server">
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
    <ext:Store ID="Store5" runat="server">
        <Reader>
            <ext:JsonReader ReaderID="STAFFID">
                <Fields>
                    <ext:RecordField Name="YEARS">
                    </ext:RecordField>
                    <ext:RecordField Name="MONTHS">
                    </ext:RecordField>
                    <ext:RecordField Name="DEPTID" />
                    <ext:RecordField Name="DEPTNAME" />
                    <ext:RecordField Name="STAFFID" />
                    <ext:RecordField Name="STAFFNAME" />
                    <ext:RecordField Name="ISBONUS" />
                    <ext:RecordField Name="DAYS" />
                    <ext:RecordField Name="BONUSMODULUS" />
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
                                TrackMouseOver="true" Height="480" Enabled="false">
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
                                            <ext:ToolbarSeparator ID="ToolbarSeparator1" runat="server" />
                                            <ext:ComboBox runat="server" Title="科室" ID="cbbdept" StoreID="Store2" Width="100"
                                                DisplayField="DEPTNAME" ValueField="DEPTID" ReadOnly="true">
                                                <Listeners>
                                                    <Select Fn="selectDept" />
                                                </Listeners>
                                            </ext:ComboBox>
                                            <%--<ext:ToolbarSeparator ID="ToolbarSeparator3" runat="server" />
                                                    <ext:Button ID="Btn_Qurey" Text="查询" Icon="DatabaseGo" runat="server">
                                                        <AjaxEvents>
                                                            <Click OnEvent="btn_Qurey_Click">
                                                            </Click>
                                                        </AjaxEvents>
                                                    </ext:Button>--%>
                                            <%-- <ext:Button ID="Btn_Ref" runat="server" Text="刷新" Icon="ArrowRefresh">
                                                        <AjaxEvents>
                                                            <Click OnEvent="btn_Refresh_Click">
                                                            </Click>
                                                        </AjaxEvents>
                                                    </ext:Button>--%>
                                            <ext:ToolbarSeparator ID="ToolbarSeparator2" runat="server" />
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
                                        <ext:Column ColumnID="STAFFNAME" Header="科室人员" Width="150" DataIndex="STAFFNAME"
                                            Align="Center" MenuDisabled="true">
                                        </ext:Column>
                                        <ext:CheckColumn ColumnID="ISBONUS" Header="是否发放奖金" Width="150" DataIndex="ISBONUS"
                                            Align="Center" MenuDisabled="true" Editable="false">
                                        </ext:CheckColumn>
                                        <ext:Column ColumnID="DAYS" Header="<div style='text-align:center;'>工作日数</div>" Width="150"
                                            DataIndex="DAYS" MenuDisabled="true" Align="Right">
                                        </ext:Column>
                                        <ext:Column ColumnID="BONUSMODULUS" Header="<div style='text-align:center;'>岗位系数</div>"
                                            Width="150" DataIndex="BONUSMODULUS" Align="Right" MenuDisabled="true">
                                        </ext:Column>
                                        <ext:Column ColumnID="OBLIGATION" Header="<div style='text-align:center;'>责任奖系数</div>"
                                            Width="150" DataIndex="OBLIGATION" Align="Right" MenuDisabled="true">
                                        </ext:Column>
                                    </Columns>
                                </ColumnModel>
                                <SelectionModel>
                                    <ext:RowSelectionModel ID="RowSelectionModel" SingleSelect="true" />
                                </SelectionModel>
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
                        </ext:LayoutColumn>
                    </Columns>
                </ext:ColumnLayout>
            </Body>
        </ext:ViewPort>
    </div>
    </form>
</body>
</html>
