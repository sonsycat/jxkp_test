<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Cost_13.aspx.cs" Inherits="GoldNet.JXKP.cbhs.xyhs.Report.Cost_13" %>

<%@ Register Assembly="Goldnet.Ext.Web" Namespace="Goldnet.Ext.Web" TagPrefix="ext" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
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
    <ext:ScriptManager ID="ScriptManager1" runat="server">
    </ext:ScriptManager>
    <form id="form1" runat="server">
    <ext:Store ID="SReport" AutoLoad="true" runat="server">
        <Reader>
            <ext:JsonReader>
                <Fields>
                    <ext:RecordField Name="DEPT_CODE">
                    </ext:RecordField>
                    <ext:RecordField Name="ACCOUNT_DEPT_NAME">
                    </ext:RecordField>
                    <ext:RecordField Name="BZKSMC">
                    </ext:RecordField>
                    <ext:RecordField Name="HC">
                    </ext:RecordField>
                    <ext:RecordField Name="MZRC">
                    </ext:RecordField>
                    <ext:RecordField Name="ZCRS">
                    </ext:RecordField>
                    <ext:RecordField Name="CY_RS">
                    </ext:RecordField>
                    <ext:RecordField Name="ZC_SF">
                    </ext:RecordField>
                    <ext:RecordField Name="ZC_CB">
                    </ext:RecordField>
                    <ext:RecordField Name="ZC_YPSF">
                    </ext:RecordField>
                    <ext:RecordField Name="ZC_JY">
                    </ext:RecordField>
                    <ext:RecordField Name="CR_SF">
                    </ext:RecordField>
                    <ext:RecordField Name="CR_CB">
                    </ext:RecordField>
                    <ext:RecordField Name="CR_YPSF">
                    </ext:RecordField>
                    <ext:RecordField Name="CR_JY">
                    </ext:RecordField>
                    <ext:RecordField Name="CY_SF">
                    </ext:RecordField>
                    <ext:RecordField Name="CY_CB">
                    </ext:RecordField>
                    <ext:RecordField Name="CY_YPSF">
                    </ext:RecordField>
                    <ext:RecordField Name="CY_JY">
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
                                            <ext:ToolbarSpacer ID="ToolbarSpacer1" runat="server" Width="50">
                                            </ext:ToolbarSpacer>
                                            <ext:Button ID="btn_Query" runat="server" Text="查询" Icon="Zoom">
                                                <AjaxEvents>
                                                    <Click OnEvent="Btn_Query_Click" Timeout="99999999">
                                                        <EventMask Msg="查询中..." ShowMask="true" />
                                                        <EventMask ShowMask="true" Msg="请稍候..." />
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
                                    <center>
                                        <h2>
                                            表13 医院临床服务类科室工作量及次均费用表</h2>
                                    </center>
                                    <ext:ExtGridPanel ID="GridPanel_Show" runat="server" StoreID="SReport" Border="true"
                                        Width="800" Height="400" AutoScroll="true" StyleSpec="margin:10px">
                                        <ExtColumnModel ID="ColumnModel1" runat="server">
                                            <Columns>
                                                <ext:ExtColumn ColumnID="ACCOUNT_DEPT_NAME" Header="<div style='text-align:center;'>医院科室名称</div>"
                                                    Width="120" DataIndex="ACCOUNT_DEPT_NAME" MenuDisabled="true">
                                                </ext:ExtColumn>
                                                <ext:ExtColumn ColumnID="BZKSMC" Header="<div style='text-align:center;'>标准科室名称</div>"
                                                    Width="120" DataIndex="BZKSMC" MenuDisabled="true" Align="Right">
                                                </ext:ExtColumn>
                                                <ext:ExtColumn ColumnID="HC" Header="<div style='text-align:center;'>行次</div>" Width="120"
                                                    DataIndex="HC" MenuDisabled="true" Align="Right">
                                                </ext:ExtColumn>
                                                <ext:ExtColumn ColumnID="MZRC" Header="<div style='text-align:center;'>门（急）诊人次数</div>"
                                                    Width="120" DataIndex="MZRC" MenuDisabled="true" Align="Right">
                                                    <Renderer Fn="rmbMoney" />
                                                </ext:ExtColumn>
                                                <ext:ExtColumn ColumnID="ZCRS" Header="<div style='text-align:center;'>实际占用床日数</div>"
                                                    Width="120" DataIndex="ZCRS" MenuDisabled="true" Align="Right">
                                                    <Renderer Fn="rmbMoney" />
                                                </ext:ExtColumn>
                                                <ext:ExtColumn ColumnID="CY_RS" Header="<div style='text-align:center;'>出院人数</div>"
                                                    Width="120" DataIndex="CY_RS" MenuDisabled="true" Align="Right">
                                                    <Renderer Fn="rmbMoney" />
                                                </ext:ExtColumn>
                                                <ext:ExtColumn ColumnID="ZC_SF" Header="<div style='text-align:center;'>平均收费</div>"
                                                    Width="120" DataIndex="ZC_SF" MenuDisabled="true" Align="Right">
                                                    <Renderer Fn="rmbMoney" />
                                                </ext:ExtColumn>
                                                <ext:ExtColumn ColumnID="ZC_CB" Header="<div style='text-align:center;'>平均医疗成本</div>"
                                                    Width="120" DataIndex="ZC_CB" MenuDisabled="true" Align="Right">
                                                    <Renderer Fn="rmbMoney" />
                                                </ext:ExtColumn>
                                                <ext:ExtColumn ColumnID="ZC_YPSF" Header="<div style='text-align:center;'>其中：药品费</div>"
                                                    Width="120" DataIndex="ZC_YPSF" MenuDisabled="true" Align="Right">
                                                    <Renderer Fn="rmbMoney" />
                                                </ext:ExtColumn>
                                                <ext:ExtColumn ColumnID="ZC_JY" Header="<div style='text-align:center;'>均次结余</div>"
                                                    Width="120" DataIndex="ZC_JY" MenuDisabled="true" Align="Right">
                                                    <Renderer Fn="rmbMoney" />
                                                </ext:ExtColumn>
                                                <ext:ExtColumn ColumnID="CR_SF" Header="<div style='text-align:center;'>平均收费</div>"
                                                    Width="120" DataIndex="CR_SF" MenuDisabled="true" Align="Right">
                                                    <Renderer Fn="rmbMoney" />
                                                </ext:ExtColumn>
                                                <ext:ExtColumn ColumnID="CR_CB" Header="<div style='text-align:center;'>平均医疗成本</div>"
                                                    Width="120" DataIndex="CR_CB" MenuDisabled="true" Align="Right">
                                                    <Renderer Fn="rmbMoney" />
                                                </ext:ExtColumn>
                                                <ext:ExtColumn ColumnID="CR_YPSF" Header="<div style='text-align:center;'>其中：药品费</div>"
                                                    Width="120" DataIndex="CR_YPSF" MenuDisabled="true" Align="Right">
                                                    <Renderer Fn="rmbMoney" />
                                                </ext:ExtColumn>
                                                <ext:ExtColumn ColumnID="CR_JY" Header="<div style='text-align:center;'>均次结余</div>"
                                                    Width="120" DataIndex="CR_JY" MenuDisabled="true" Align="Right">
                                                    <Renderer Fn="rmbMoney" />
                                                </ext:ExtColumn>
                                                <ext:ExtColumn ColumnID="CY_SF" Header="<div style='text-align:center;'>平均收费</div>"
                                                    Width="120" DataIndex="CY_SF" MenuDisabled="true" Align="Right">
                                                    <Renderer Fn="rmbMoney" />
                                                </ext:ExtColumn>
                                                <ext:ExtColumn ColumnID="CY_CB" Header="<div style='text-align:center;'>平均医疗成本</div>"
                                                    Width="120" DataIndex="CY_CB" MenuDisabled="true" Align="Right">
                                                    <Renderer Fn="rmbMoney" />
                                                </ext:ExtColumn>
                                                <ext:ExtColumn ColumnID="CY_YPSF" Header="<div style='text-align:center;'>其中：药品费</div>"
                                                    Width="120" DataIndex="CY_YPSF" MenuDisabled="true" Align="Right">
                                                    <Renderer Fn="rmbMoney" />
                                                </ext:ExtColumn>
                                                <ext:ExtColumn ColumnID="CY_JY" Header="<div style='text-align:center;'>均次结余</div>"
                                                    Width="120" DataIndex="CY_JY" MenuDisabled="true" Align="Right">
                                                    <Renderer Fn="rmbMoney" />
                                                </ext:ExtColumn>
                                            </Columns>
                                            <HeadRows>
                                                <ext:ExtRows>
                                                    <Rows>
                                                        <ext:ExtRow Header="" ColSpan="1" Align="Center" />
                                                        <ext:ExtRow Header="" ColSpan="1" Align="Center" />
                                                        <ext:ExtRow Header="" ColSpan="1" Align="Center" />
                                                        <ext:ExtRow Header="" ColSpan="3" Align="Center" />
                                                        <ext:ExtRow Header="次均费用" ColSpan="12" Align="Center" />
                                                    </Rows>
                                                </ext:ExtRows>
                                                <ext:ExtRows>
                                                    <Rows>
                                                        <ext:ExtRow Header="" ColSpan="1" Align="Center" />
                                                        <ext:ExtRow Header="" ColSpan="1" Align="Center" />
                                                        <ext:ExtRow Header="" ColSpan="1" Align="Center" />
                                                        <ext:ExtRow Header="工作量" ColSpan="3" Align="Center" />
                                                        <ext:ExtRow Header="每门（急）诊人次" ColSpan="4" Align="Center" />
                                                        <ext:ExtRow Header="每床日" ColSpan="4" Align="Center" />
                                                        <ext:ExtRow Header="每出院人次" ColSpan="4" Align="Center" />
                                                    </Rows>
                                                </ext:ExtRows>
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
                                        <Listeners>
                                            <BeforeRender Handler="Ext.EventManager.onWindowResize(function(){ if(Ext.getBody().getViewSize().width>850){this.setWidth( Ext.getBody().getViewSize().width -18);}; this.setHeight( Ext.getBody().getViewSize().height -100); }, this);" />
                                            <Render Handler="if(Ext.getBody().getViewSize().width>850){this.setWidth( Ext.getBody().getViewSize().width -18);}; this.setHeight( Ext.getBody().getViewSize().height -100);" />
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
