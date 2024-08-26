<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="StaffWorkday_DG_report.aspx.cs"
    Inherits="GoldNet.JXKP.RLZY.BaseInfoMaintain.StaffWorkday_DG_report" %>

<%@ Register Assembly="Goldnet.Ext.Web" Namespace="Goldnet.Ext.Web" TagPrefix="ext" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>

    <script type="text/javascript">
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

        };
        
        var template = '<span style="color:{0};">{1}</span>';

        var namecolor = function(value) {
            if (value.indexOf("　") > -1) {
                return String.format(template, 'black', value);
            }
            else {
                return String.format(template, (value == '合计') ? 'red' : 'blue', value);
            }
        };

    </script>

</head>
<body>
    <form id="form1" runat="server">
    <ext:ScriptManager ID="ScriptManager1" runat="server" />
    <ext:Store ID="Store1" AutoLoad="true" runat="server">
        <Reader>
            <ext:JsonReader>
                <Fields>
                    <ext:RecordField Name="DEPT_CODE">
                    </ext:RecordField>
                    <ext:RecordField Name="DEPT_NAME">
                    </ext:RecordField>
                    <ext:RecordField Name="STAFF_ID">
                    </ext:RecordField>
                    <ext:RecordField Name="NAME">
                    </ext:RecordField>
                    <ext:RecordField Name="DZW">
                    </ext:RecordField>
                    <ext:RecordField Name="DGTS">
                    </ext:RecordField>
                    <ext:RecordField Name="ZWBZ">
                    </ext:RecordField>
                    <ext:RecordField Name="DZJT">
                    </ext:RecordField>
                </Fields>
            </ext:JsonReader>
        </Reader>
    </ext:Store>
    <ext:Store ID="SYear" runat="server">
        <Reader>
            <ext:JsonReader ReaderID="YEAR">
                <Fields>
                    <ext:RecordField Name="YEAR" />
                </Fields>
            </ext:JsonReader>
        </Reader>
    </ext:Store>
    <ext:Store ID="SMonth" runat="server">
        <Reader>
            <ext:JsonReader ReaderID="MONTH">
                <Fields>
                    <ext:RecordField Name="MONTH" />
                </Fields>
            </ext:JsonReader>
        </Reader>
    </ext:Store>
    <div>
        <ext:ViewPort ID="ViewPort1" runat="server">
            <Body>
                <ext:ColumnLayout ID="ColumnLayout1" runat="server" Split="true" FitHeight="true">
                    <Columns>
                        <ext:LayoutColumn ColumnWidth="1">
                            <ext:GridPanel ID="GridPanel2" runat="server" Border="false" StoreID="Store1" StripeRows="true"
                                TrackMouseOver="true" Height="480" ClicksToEdit="1">
                                <TopBar>
                                    <ext:Toolbar ID="Toolbar_detptype" runat="server" StyleSpec="border:0">
                                        <Items>
                                            <ext:ComboBox ID="cbbYear" runat="server" ReadOnly="true" StoreID="SYear" Width="60"
                                                DisplayField="YEAR" ValueField="YEAR" ForceSelection="true" SelectOnFocus="true">
                                            </ext:ComboBox>
                                            <ext:Label ID="lYear" runat="server" Text="年">
                                            </ext:Label>
                                            <ext:ComboBox ID="cbbmonth" runat="server" ReadOnly="true" StoreID="SMonth" Width="50"
                                                DisplayField="MONTH" ValueField="MONTH">
                                            </ext:ComboBox>
                                            <ext:Label ID="lmonth" runat="server" Text="月">
                                            </ext:Label>
                                            <ext:ToolbarSpacer ID="ToolbarSpacer2" runat="server" Width="20">
                                            </ext:ToolbarSpacer>
                                            <ext:Button ID="btn_Query" runat="server" Text="查询" Icon="Zoom">
                                                <AjaxEvents>
                                                    <Click OnEvent="Btn_Query_Click" Timeout="99999999">
                                                        <EventMask Msg="查询中......" ShowMask="true" />
                                                    </Click>
                                                </AjaxEvents>
                                            </ext:Button>
                                            <ext:ToolbarSpacer ID="ToolbarSpacer3" runat="server" Width="20">
                                            </ext:ToolbarSpacer>
                                            <ext:Button ID="btn_Excel" runat="server" OnClick="OutExcel" AutoPostBack="true"
                                                Text="导出Excel" Icon="PageWhiteExcel" CausesValidation="false">
                                            </ext:Button>
                                        </Items>
                                    </ext:Toolbar>
                                </TopBar>
                                <ColumnModel ID="ColumnModel2" runat="server">
                                    <Columns>
                                        <ext:ExtColumn ColumnID="DEPT_NAME" Header="<div style='text-align:center;'>科室</div>"
                                            Width="120" DataIndex="DEPT_NAME" MenuDisabled="true">
                                        </ext:ExtColumn>
                                        <ext:ExtColumn ColumnID="NAME" Header="<div style='text-align:center;'>姓名</div>"
                                            Width="120" DataIndex="NAME" MenuDisabled="true">
                                        </ext:ExtColumn>
                                        <ext:ExtColumn ColumnID="DZW" Header="<div style='text-align:center;'>代职务</div>"
                                            Width="120" DataIndex="DZW" MenuDisabled="true">
                                        </ext:ExtColumn>
                                        <ext:ExtColumn ColumnID="DGTS" Header="<div style='text-align:center;'>代岗天数</div>"
                                            Width="120" DataIndex="DGTS" MenuDisabled="true" Align="Right">
                                            <Renderer Fn="rmbMoney" />
                                        </ext:ExtColumn>
                                        <ext:ExtColumn ColumnID="ZWBZ" Header="<div style='text-align:center;'>职务标准</div>"
                                            Width="120" DataIndex="ZWBZ" MenuDisabled="true" Align="Right">
                                            <Renderer Fn="rmbMoney" />
                                        </ext:ExtColumn>
                                        <ext:ExtColumn ColumnID="DZJT" Header="<div style='text-align:center;'>代职津贴</div>"
                                            Width="120" DataIndex="DZJT" MenuDisabled="true" Align="Right">
                                            <Renderer Fn="rmbMoney" />
                                        </ext:ExtColumn>
                                    </Columns>
                                </ColumnModel>
                                <SelectionModel>
                                    <ext:RowSelectionModel ID="rowselection" SingleSelect="true">
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
