<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="OutRestNonus_new.aspx.cs"
    Inherits="GoldNet.JXKP.Bonus.Input.OutRestNonus_new" %>

<%@ Register Assembly="Goldnet.Ext.Web" Namespace="Goldnet.Ext.Web" TagPrefix="ext" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <link rel="stylesheet" type="text/css" href="../../Bonus/Orthers/Cbouns.css" />
    <style type="text/css">
        h2
        {
            font-size: 24px;
            letter-spacing: 1px;
            margin: 10px 0 20px;
            padding: 0;
        }
        .search-item
        {
            font: normal 11px tahoma, arial, helvetica, sans-serif;
            padding: 3px 10px 3px 10px;
            border: 1px solid #fff;
            border-bottom: 1px solid #eeeeee;
            white-space: normal;
            color: #555;
            width: 200px;
        }
        .search-item h3
        {
            display: block;
            font: inherit;
            font-weight: bold;
            color: #222;
        }
        .search-item h3 span
        {
            float: right;
            font-weight: normal;
            margin: 0 0 5px 5px;
            width: 140px;
            display: block;
            clear: none;
        }
    </style>

    <script type="text/javascript">
        //列表单元格
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
        //设置列表名称的字体颜色
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
    <ext:ScriptManager ID="ScriptManager1" runat="server" AjaxMethodNamespace="CompanyX" />
    <form id="form1" runat="server">
    <ext:Store ID="SReport" runat="server" OnRefreshData="Data_RefreshData" AutoLoad="true">
        <Reader>
            <ext:JsonReader>
                <Fields>
                    <ext:RecordField Name="DATE_TIME">
                    </ext:RecordField>
                    <ext:RecordField Name="DEPT_CODE">
                    </ext:RecordField>
                    <ext:RecordField Name="DEPT_NAME">
                    </ext:RecordField>
                    <ext:RecordField Name="A1_1">
                    </ext:RecordField>
                    <ext:RecordField Name="A1_2">
                    </ext:RecordField>
                    <ext:RecordField Name="A1_3">
                    </ext:RecordField>
                    <ext:RecordField Name="A1_4">
                    </ext:RecordField>
                    <ext:RecordField Name="A2_1">
                    </ext:RecordField>
                    <ext:RecordField Name="A2_2">
                    </ext:RecordField>
                    <ext:RecordField Name="A2_3">
                    </ext:RecordField>
                    <ext:RecordField Name="A2_4">
                    </ext:RecordField>
                    <ext:RecordField Name="A3_1">
                    </ext:RecordField>
                    <ext:RecordField Name="A3_2">
                    </ext:RecordField>
                    <ext:RecordField Name="A3_3">
                    </ext:RecordField>
                    <ext:RecordField Name="A3_4">
                    </ext:RecordField>
                    <ext:RecordField Name="A4_1">
                    </ext:RecordField>
                    <ext:RecordField Name="A4_2">
                    </ext:RecordField>
                    <ext:RecordField Name="A4_3">
                    </ext:RecordField>
                    <ext:RecordField Name="A4_4">
                    </ext:RecordField>
                    <ext:RecordField Name="A5_1">
                    </ext:RecordField>
                    <ext:RecordField Name="A5_2">
                    </ext:RecordField>
                    <ext:RecordField Name="A5_3">
                    </ext:RecordField>
                    <ext:RecordField Name="A5_4">
                    </ext:RecordField>
                    <ext:RecordField Name="A6_1">
                    </ext:RecordField>
                    <ext:RecordField Name="A6_2">
                    </ext:RecordField>
                    <ext:RecordField Name="A6_3">
                    </ext:RecordField>
                    <ext:RecordField Name="A6_4">
                    </ext:RecordField>
                    <ext:RecordField Name="A7_1">
                    </ext:RecordField>
                    <ext:RecordField Name="A7_2">
                    </ext:RecordField>
                    <ext:RecordField Name="A7_3">
                    </ext:RecordField>
                    <ext:RecordField Name="A7_4">
                    </ext:RecordField>
                    <ext:RecordField Name="A8_1">
                    </ext:RecordField>
                    <ext:RecordField Name="A8_2">
                    </ext:RecordField>
                    <ext:RecordField Name="A8_3">
                    </ext:RecordField>
                    <ext:RecordField Name="A8_4">
                    </ext:RecordField>
                    <ext:RecordField Name="A9_1">
                    </ext:RecordField>
                    <ext:RecordField Name="A9_2">
                    </ext:RecordField>
                    <ext:RecordField Name="A9_3">
                    </ext:RecordField>
                    <ext:RecordField Name="A9_4">
                    </ext:RecordField>
                    <ext:RecordField Name="A10_1">
                    </ext:RecordField>
                    <ext:RecordField Name="A10_2">
                    </ext:RecordField>
                    <ext:RecordField Name="A10_3">
                    </ext:RecordField>
                    <ext:RecordField Name="A10_4">
                    </ext:RecordField>
                    <ext:RecordField Name="A11_1">
                    </ext:RecordField>
                    <ext:RecordField Name="A11_2">
                    </ext:RecordField>
                    <ext:RecordField Name="A11_3">
                    </ext:RecordField>
                    <ext:RecordField Name="A11_4">
                    </ext:RecordField>
                    <ext:RecordField Name="A12_1">
                    </ext:RecordField>
                    <ext:RecordField Name="A12_2">
                    </ext:RecordField>
                    <ext:RecordField Name="A12_3">
                    </ext:RecordField>
                    <ext:RecordField Name="A12_4">
                    </ext:RecordField>
                    <ext:RecordField Name="A13_1">
                    </ext:RecordField>
                    <ext:RecordField Name="A13_2">
                    </ext:RecordField>
                    <ext:RecordField Name="A13_3">
                    </ext:RecordField>
                    <ext:RecordField Name="A13_4">
                    </ext:RecordField>
                    <ext:RecordField Name="A14_1">
                    </ext:RecordField>
                    <ext:RecordField Name="A14_2">
                    </ext:RecordField>
                    <ext:RecordField Name="A14_3">
                    </ext:RecordField>
                    <ext:RecordField Name="A14_4">
                    </ext:RecordField>
                    <ext:RecordField Name="OUT_TOTAL">
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
    <ext:Store ID="SDept" runat="server" AutoLoad="true">
    </ext:Store>
    <div>
        <ext:ViewPort ID="ViewPort1" runat="server">
            <Body>
                <ext:ColumnLayout ID="ColumnLayout1" runat="server" Split="true" FitHeight="true">
                    <Columns>
                        <ext:LayoutColumn ColumnWidth="1">
                            <ext:Panel runat="server" ID="p11" AutoScroll="true" Border="false">
                                <TopBar>
                                    <ext:Toolbar ID="Toolbar_detptype" runat="server" StyleSpec="border:0">
                                        <Items>
                                            <ext:Label ID="lcaption" runat="server" Text="年月：">
                                            </ext:Label>
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
                                            <ext:Button ID="btn_Query" runat="server" Text="查询" Icon="Zoom" AutoPostBack="true"
                                                OnClick="Btn_Query_Click">
                                            </ext:Button>
                                            <ext:ToolbarSeparator ID="ToolbarSeparator4" runat="server" />
                                            <ext:Button ID="Button_save" runat="server" Text="保存" Icon="Disk">
                                                <AjaxEvents>
                                                    <Click OnEvent="Button_Save_click">
                                                        <EventMask Msg="保存中..." ShowMask="true" />
                                                        <ExtraParams>
                                                            <ext:Parameter Name="Values" Value="Ext.encode(#{GridPanel_Show}.getRowsValues(false))"
                                                                Mode="Raw">
                                                            </ext:Parameter>
                                                        </ExtraParams>
                                                    </Click>
                                                </AjaxEvents>
                                            </ext:Button>
                                            <ext:ToolbarSpacer ID="ToolbarSpacer3" runat="server" Width="20">
                                            </ext:ToolbarSpacer>
                                            <ext:Button ID="btn_Excel" runat="server" OnClick="OutExcel" AutoPostBack="true"
                                                Text="导出Excel" Icon="PageWhiteExcel">
                                            </ext:Button>
                                        </Items>
                                    </ext:Toolbar>
                                </TopBar>
                                <Body>
                                    <ext:ExtGridPanel ID="GridPanel_Show" runat="server" StoreID="SReport" Border="true"
                                        Width="800" Height="400" AutoScroll="true" StyleSpec="margin:10px" ClicksToEdit="1"
                                        StripeRows="true">
                                        <ExtColumnModel ID="ColumnModel1" runat="server">
                                            <Columns>
                                            </Columns>
                                            <HeadRows>
                                            </HeadRows>
                                        </ExtColumnModel>
                                        <Plugins>
                                            <ext:ExtGroupHeaderGrid ID="ExtGroupHeaderGrid2" runat="server">
                                            </ext:ExtGroupHeaderGrid>
                                        </Plugins>
                                        <SelectionModel>
                                            <ext:RowSelectionModel ID="rowselection" SingleSelect="true">
                                            </ext:RowSelectionModel>
                                        </SelectionModel>
                                        <LoadMask ShowMask="true" />
                                        <Listeners>
                                            <BeforeRender Handler="Ext.EventManager.onWindowResize(function(){ if(Ext.getBody().getViewSize().width>850){this.setWidth( Ext.getBody().getViewSize().width -18);}; this.setHeight( Ext.getBody().getViewSize().height -60); }, this);" />
                                            <Render Handler="if(Ext.getBody().getViewSize().width>850){this.setWidth( Ext.getBody().getViewSize().width -18);}; this.setHeight( Ext.getBody().getViewSize().height -60);" />
                                        </Listeners>
                                        <LoadMask ShowMask="true" Msg="查询中....." />
                                    </ext:ExtGridPanel>
                                </Body>
                            </ext:Panel>
                        </ext:LayoutColumn>
                    </Columns>
                </ext:ColumnLayout>
            </Body>
        </ext:ViewPort>
    </div>
    </form>
</body>
</html>
