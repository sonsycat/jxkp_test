<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DeptCostAccount.aspx.cs"
    Inherits="GoldNet.JXKP.DeptCostAccount" %>

<%@ Register Assembly="Goldnet.Ext.Web" Namespace="Goldnet.Ext.Web" TagPrefix="ext" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
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
        .titile-h1
        {
            font-size: 12px;
            margin: 5px 0 0 20px;
        }
        .benefit-item
        {
            font-size: 12px;
            width: 30px;
            margin: 5px 10px 0 0;
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
        var totalMoney = function(a, b, c) {
//            lChange.text = rmbMoney(a);
//            lIncome.Text = rmbMoney(b);
//            lBenefit.Text = rmbMoney(c);
        };
    </script>

</head>
<body>
    <ext:ScriptManager ID="ScriptManager1" runat="server">
        <Listeners>
            <DocumentReady Handler="cbbType.setWidth(80);" />
        </Listeners>
    </ext:ScriptManager>
    <form id="form1" runat="server">
    <ext:Store ID="SReportIncome" AutoLoad="true" runat="server">
        <Reader>
            <ext:JsonReader>
                <Fields>
                    <ext:RecordField Name="INCOM_TYPE_NAME">
                    </ext:RecordField>
                    <ext:RecordField Name="ITEM_NAME">
                    </ext:RecordField>
                    <ext:RecordField Name="INCOMES_CHARGES">
                    </ext:RecordField>
                    <ext:RecordField Name="CHARGES">
                    </ext:RecordField>
                    <ext:RecordField Name="INCOMES">
                    </ext:RecordField>
                    <ext:RecordField Name="RATE">
                    </ext:RecordField>
                    <ext:RecordField Name="ARMCOSTS">
                    </ext:RecordField>
                </Fields>
            </ext:JsonReader>
        </Reader>
    </ext:Store>
    <ext:Store ID="SReportCost" AutoLoad="true" runat="server">
        <Reader>
            <ext:JsonReader ReaderID="ITEM_CODE">
                <Fields>
                    <ext:RecordField Name="COST_TYPE_NAME">
                    </ext:RecordField>
                    <ext:RecordField Name="ITEM_NAME">
                    </ext:RecordField>
                    <ext:RecordField Name="ITEM_CODE">
                    </ext:RecordField>
                    <ext:RecordField Name="COSTS">
                    </ext:RecordField>
                    <ext:RecordField Name="COSTS_ARMYFREE">
                    </ext:RecordField>
                    <ext:RecordField Name="TOTALCOST">
                    </ext:RecordField>
                    <ext:RecordField Name="RATE">
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
                                            <ext:ToolbarSpacer ID="ToolbarSpacer1" runat="server" Width="5">
                                            </ext:ToolbarSpacer>
                                            <ext:Label ID="Label3" runat="server" Text="科室：" />
                                            <ext:ComboBox ID="cbbdept" runat="server" StoreID="SDept" DisplayField="DEPT_NAME"
                                                ValueField="DEPT_CODE" TypeAhead="false" LoadingText="Searching..." Width="150"
                                                PageSize="10" ItemSelector="div.search-item" MinChars="1" ListWidth="200">
                                                <Template ID="Template1" runat="server">
                                                    <tpl for=".">
                                                        <div class="search-item">
                                                             <h3>{DEPT_NAME}</h3>
                                                             </div>
                                                      </tpl>                                                                                                       
                                                </Template>
                                            </ext:ComboBox>
                                            <ext:ComboBox ID="cbbType" runat="server" ReadOnly="true" ForceSelection="true" SelectOnFocus="true"
                                                SelectedIndex="1">
                                                <Items>
                                                    <ext:ListItem Text="收付实现" Value="1" />
                                                    <ext:ListItem Text="责权发生" Value="0" />
                                                </Items>
                                            </ext:ComboBox>
                                            <ext:ToolbarSpacer ID="ToolbarSpacer2" runat="server" Width="20">
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
                                    <table width="800">
                                        <tr>
                                            <td>
                                                <center>
                                                    <h2>
                                                        核算单位成本核算信息汇总表</h2>
                                                </center>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <table width="600" runat="server" style="margin: 10px" visible="false">
                                                    <tr>
                                                        <td class="benefit-item" width="100">
                                                            医疗收益：
                                                            <ext:Label runat="server" ID="lChange" Text="0">
                                                                <Listeners>
                                                                    <Render Fn="rmbMoney" />
                                                                </Listeners>
                                                            </ext:Label>
                                                        </td>
                                                        <td class="benefit-item" width="200">
                                                            军免虚收结余：
                                                            <ext:Label runat="server" ID="lIncome" Text="0">
                                                            </ext:Label>
                                                        </td>
                                                        <td class="benefit-item" width="100">
                                                            利润合计：
                                                            <ext:Label runat="server" ID="lBenefit" Text="0">
                                                            </ext:Label>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <div class="titile-h1">
                                                    收入情况：</div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <ext:GridPanel ID="gpIncome" runat="server" Border="true" StoreID="SReportIncome"
                                                    StripeRows="true" TrackMouseOver="true" Height="480" StyleSpec="margin:10px">
                                                    <ColumnModel ID="ColumnModel2" runat="server">
                                                        <Columns>
                                                            <%-- <ext:Column ColumnID="INCOM_TYPE_NAME" Header="<div style='text-align:center;'>类别</div>" Width="100" DataIndex="INCOM_TYPE_NAME"
                                                                MenuDisabled="true" Align="Left">
                                                            </ext:Column>
                                                           <ext:Column ColumnID="ITEM_NAME" Header="<div style='text-align:center;'>项目名称</div>" Width="100" DataIndex="ITEM_NAME"
                                                                MenuDisabled="true" Align="Left">
                                                            </ext:Column>
                                                            <ext:Column ColumnID="INCOMES_CHARGES" Header="<div style='text-align:center;'>实际收入</div>" Width="100" DataIndex="INCOMES_CHARGES"
                                                                MenuDisabled="true" Align="Right">
                                                                 <Renderer Fn="rmbMoney" />
                                                            </ext:Column>
                                                            <ext:Column ColumnID="CHARGES" Header="<div style='text-align:center;'>减免收入</div>" Width="100" DataIndex="CHARGES"
                                                                MenuDisabled="true" Align="Right">
                                                                 <Renderer Fn="rmbMoney" />
                                                            </ext:Column>
                                                            <ext:Column ColumnID="INCOMES" Header="<div style='text-align:center;'>收入合计</div>" Width="100" DataIndex="INCOMES"
                                                                MenuDisabled="true" Align="Right">
                                                                 <Renderer Fn="rmbMoney" />
                                                            </ext:Column>
                                                            <ext:Column ColumnID="RATE" Header="<div style='text-align:center;'>占收入比</div>" Width="100" DataIndex="RATE"
                                                                MenuDisabled="true" Align="Right">
                                                                 <Renderer Fn="rmbMoney" />
                                                            </ext:Column>--%>
                                                        </Columns>
                                                    </ColumnModel>
                                                    <SelectionModel>
                                                        <ext:RowSelectionModel ID="rowselection" SingleSelect="true">
                                                        </ext:RowSelectionModel>
                                                    </SelectionModel>
                                                    <Listeners>
                                                        <BeforeRender Handler="Ext.EventManager.onWindowResize(function(){ if(Ext.getBody().getViewSize().width>850){this.setWidth( Ext.getBody().getViewSize().width -30);}this.setHeight( Ext.getBody().getViewSize().height/2 ); }, this)" />
                                                        <Render Handler="if(Ext.getBody().getViewSize().width>850){this.setWidth( Ext.getBody().getViewSize().width -30);}this.setHeight( Ext.getBody().getViewSize().height/2 );" />
                                                    </Listeners>
                                                    <LoadMask ShowMask="true" />
                                                </ext:GridPanel>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <div class="titile-h1">
                                                    支出情况：</div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <ext:GridPanel ID="gpCost" runat="server" Border="true" StoreID="SReportCost" StripeRows="true"
                                                    TrackMouseOver="true" Height="480" StyleSpec="margin:10px">
                                                    <ColumnModel ID="ColumnModel1" runat="server">
                                                        <Columns>
                                                            <%-- <ext:Column ColumnID="COST_TYPE_NAME" Header="<div style='text-align:center;'>类别</div>" Width="100" DataIndex="COST_TYPE_NAME"
                                                                MenuDisabled="true" Align="Left">
                                                            </ext:Column>
                                                           <ext:Column ColumnID="ITEM_NAME" Header="<div style='text-align:center;'>项目名称</div>" Width="100" DataIndex="ITEM_NAME"
                                                                MenuDisabled="true" Align="Left">
                                                            </ext:Column>
                                                            <ext:Column ColumnID="COSTS" Header="<div style='text-align:center;'>对外成本</div>" Width="100" DataIndex="COSTS"
                                                                MenuDisabled="true" Align="Right">
                                                                <Renderer Fn="rmbMoney" />
                                                            </ext:Column>
                                                            <ext:Column ColumnID="COSTS_ARMYFREE" Header="<div style='text-align:center;'>减免成本</div>" Width="100" DataIndex="COSTS_ARMYFREE"
                                                                MenuDisabled="true" Align="Right">
                                                                <Renderer Fn="rmbMoney" />
                                                            </ext:Column>
                                                            <ext:Column ColumnID="TOTALCOST" Header="<div style='text-align:center;'>支出合计</div>" Width="100" DataIndex="TOTALCOST"
                                                                MenuDisabled="true" Align="Right">
                                                                <Renderer Fn="rmbMoney" />
                                                            </ext:Column>
                                                            <ext:Column ColumnID="RATE" Header="<div style='text-align:center;'>占支出比</div>" Width="100" DataIndex="RATE"
                                                                MenuDisabled="true" Align="Right">
                                                                 <Renderer Fn="rmbMoney" />
                                                            </ext:Column>--%>
                                                        </Columns>
                                                    </ColumnModel>
                                                    <SelectionModel>
                                                        <ext:RowSelectionModel ID="RowSelectionModel1" SingleSelect="true">
                                                        </ext:RowSelectionModel>
                                                    </SelectionModel>
                                                    <AjaxEvents>
                                                        <DblClick OnEvent="DbRowClick" />
                                                    </AjaxEvents>
                                                    <Listeners>
                                                        <BeforeRender Handler="Ext.EventManager.onWindowResize(function(){ if(Ext.getBody().getViewSize().width>850){this.setWidth( Ext.getBody().getViewSize().width -30);}this.setHeight( Ext.getBody().getViewSize().height/2 ); }, this)" />
                                                        <Render Handler="if(Ext.getBody().getViewSize().width>850){this.setWidth( Ext.getBody().getViewSize().width -30);}this.setHeight( Ext.getBody().getViewSize().height/2 );" />
                                                    </Listeners>
                                                    <LoadMask ShowMask="true" />
                                                </ext:GridPanel>
                                            </td>
                                        </tr>
                                    </table>
                                </Body>
                            </ext:Panel>
                        </ext:LayoutColumn>
                    </Columns>
                </ext:ColumnLayout>
            </Body>
        </ext:ViewPort>
    </div>
    <ext:Window ID="Cost_Detail" runat="server" Icon="Group" Title="核算项目" Width="850"
        Height="500" AutoShow="false" Modal="true" CenterOnLoad="true" AutoScroll="true"
        ShowOnLoad="false" Resizable="true" StyleSpec="background-color:Transparent;"
        BodyStyle="background-color:Transparent;">
    </ext:Window>
    </form>
</body>
</html>
