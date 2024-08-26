<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ward_all.aspx.cs" Inherits="GoldNet.JXKP.ward_all" %>

<%@ Register Assembly="Goldnet.Ext.Web" Namespace="Goldnet.Ext.Web" TagPrefix="ext" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>

    <script type="text/javascript">
        var rmbMoney = function (v) {
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

        var namecolor = function (value) {
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
    <ext:Store ID="Store1" runat="server">
        <Reader>
            <ext:JsonReader ReaderID="DEPT_CODE">
                <Fields>
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
                                            <ext:Label ID="lcaption" runat="server" Text="从：">
                                            </ext:Label>
                                            <ext:ComboBox ID="cbbYear" runat="server" ReadOnly="true" StoreID="SYear" Width="60"
                                                DisplayField="YEAR" ValueField="YEAR" ForceSelection="true" SelectOnFocus="true">
                                            </ext:ComboBox>
                                            <ext:Label ID="lYear" runat="server" Text="年">
                                            </ext:Label>
                                            <ext:ComboBox ID="cbbmonth" runat="server" ReadOnly="true" StoreID="SMonth" Width="50"
                                                DisplayField="MONTH" ValueField="MONTH">
                                            </ext:ComboBox>
                                            <ext:Label ID="lmonth" runat="server" Text="月 到：">
                                            </ext:Label>
                                            <ext:ComboBox ID="ccbYearTo" runat="server" ReadOnly="true" StoreID="SYear" Width="60"
                                                DisplayField="YEAR" ValueField="YEAR" ForceSelection="true" SelectOnFocus="true">
                                            </ext:ComboBox>
                                            <ext:Label ID="Label1" runat="server" Text="年">
                                            </ext:Label>
                                            <ext:ComboBox ID="ccbMonthTo" runat="server" ReadOnly="true" StoreID="SMonth" Width="50"
                                                DisplayField="MONTH" ValueField="MONTH">
                                            </ext:ComboBox>
                                            <ext:Label ID="Label2" runat="server" Text="月">
                                            </ext:Label>
                                            <ext:ToolbarSpacer ID="ToolbarSpacer4" runat="server" Width="20">
                                            </ext:ToolbarSpacer>
                                            <ext:ComboBox ID="comestype" runat="server" ReadOnly="true" ForceSelection="true"
                                                SelectOnFocus="true" Width="100" SelectedIndex="0">
                                                <Items>
                                                    <ext:ListItem Text="全院收入" Value="0" />
                                                    <ext:ListItem Text="门诊收入" Value="1" />
                                                    <ext:ListItem Text="住院收入" Value="2" />
                                                </Items>
                                            </ext:ComboBox>
                                            <ext:ToolbarSpacer ID="ToolbarSpacer1" runat="server" Width="20">
                                            </ext:ToolbarSpacer>
                                            <ext:ComboBox ID="deptType" runat="server" AllowBlank="true" ForceSelection="true"
                                                SelectOnFocus="true" EmptyText="选择科室类别" Width="100">
                                            </ext:ComboBox>
                                            <ext:ComboBox ID="incomestype" runat="server" ReadOnly="true" ForceSelection="true"
                                                SelectOnFocus="true" Width="100" SelectedIndex="0">
                                                <Items>
                                                    <ext:ListItem Text="开单" Value="0" />
                                                    <ext:ListItem Text="执行" Value="1" />
                                                    <ext:ListItem Text="护理" Value="2" />
                                                </Items>
                                            </ext:ComboBox>
                                            <ext:ComboBox ID="costtype" runat="server" ReadOnly="true" ForceSelection="true"
                                                SelectOnFocus="true" Width="100">
                                                <Items>
                                                    <ext:ListItem Text="实收" Value="1" />
                                                    <ext:ListItem Text="计价" Value="0" />
                                                    <ext:ListItem Text="药品成本" Value="2" />
                                                </Items>
                                            </ext:ComboBox>
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
                                    </Columns>
                                </ColumnModel>
                                <SelectionModel>
                                    <ext:RowSelectionModel ID="rowselection" SingleSelect="true">
                                    </ext:RowSelectionModel>
                                </SelectionModel>
                                <%--<AjaxEvents>
                                    <DblClick OnEvent="DbRowClick" />
                                </AjaxEvents>--%>
                                <LoadMask ShowMask="true" />
                            </ext:GridPanel>
                        </ext:LayoutColumn>
                    </Columns>
                </ext:ColumnLayout>
            </Body>
        </ext:ViewPort>
    </div>
    <ext:Window ID="Income_Detail" runat="server" Icon="Group" Title="收入项目" Width="900"
        Height="500" AutoShow="false" Modal="true" CenterOnLoad="true" AutoScroll="true"
        ShowOnLoad="false" Resizable="true" StyleSpec="background-color:Transparent;"
        BodyStyle="background-color:Transparent;">
    </ext:Window>
    </form>
</body>
</html>
