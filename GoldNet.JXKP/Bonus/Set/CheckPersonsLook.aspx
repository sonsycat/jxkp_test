<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CheckPersonsLook.aspx.cs"
    Inherits="GoldNet.JXKP.CheckPersonsLook" %>

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
        var RefreshData = function() {
            Store1.reload();
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
                                            <ext:Button ID="Btn_Persons" Text="科室人员设置" Icon="GroupGo" runat="server">
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
                                                <Listeners>
                                                    <Click Fn="backToList" />
                                                </Listeners>
                                            </ext:Button>
                                        </Items>
                                    </ext:Toolbar>
                                </TopBar>
                                <ColumnModel ID="ColumnModel22" runat="server" Enabled="false">
                                    <Columns>
                                        <ext:Column ColumnID="DEPTNAME" Header="<center>核算科室</center>" Width="130" DataIndex="DEPTNAME" MenuDisabled="true"
                                            Align="Left">
                                        </ext:Column>
                                        <ext:Column ColumnID="PERSONS" Header="<div style='text-align:center;'>科室人数</div>"
                                            Width="100" DataIndex="PERSONS" MenuDisabled="true" Align="Right">
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
