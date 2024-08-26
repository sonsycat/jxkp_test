<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DeptCostDetail.aspx.cs" Inherits="GoldNet.JXKP.DeptCostDetail" %>
<%@ Register Assembly="Goldnet.Ext.Web" Namespace="Goldnet.Ext.Web" TagPrefix="ext" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
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
    </script>
</head>
<body>
    <ext:ScriptManager ID="ScriptManager1" runat="server">
      <Listeners>
            <DocumentReady Handler="cbbType.setWidth(80);" />
        </Listeners>
    </ext:ScriptManager>
    <form id="form1" runat="server">
    <ext:Store ID="SReport" AutoLoad="true" runat="server">
        <Reader>
            <ext:JsonReader>
                <Fields>
                    <ext:RecordField Name="DEPT_CODE">
                    </ext:RecordField>
                     <ext:RecordField Name="DEPT_NAME">
                    </ext:RecordField>
                     <ext:RecordField Name="ITEM_NAME">
                    </ext:RecordField>
                     <ext:RecordField Name="COST_DATE">
                    </ext:RecordField>
                     <ext:RecordField Name="COSTS">
                    </ext:RecordField>
                     <ext:RecordField Name="COSTS_ARMYFREE">
                    </ext:RecordField>
                     <ext:RecordField Name="TOTALCOST">
                    </ext:RecordField>
                     <ext:RecordField Name="OPERATORNAME">
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
    <ext:Store ID="SCostitem" runat="server" AutoLoad="true">
    </ext:Store>
    <div>
        <ext:ViewPort ID="ViewPort1" runat="server">
            <Body>
                <ext:ColumnLayout ID="ColumnLayout1" runat="server" Split="true" FitHeight="true">
                    <Columns>
                        <ext:LayoutColumn ColumnWidth="1">
                            <ext:Panel runat="server" ID="p11" AutoScroll="true" Border="false">
                                <TopBar>
                                    <ext:Toolbar ID="Toolbar_detptype" runat="server" StyleSpec="border:0" >
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
                                            <ext:Label ID="Label4" runat="server" Text="成本项目：" />
                                            <ext:ComboBox ID="COST_CODE" runat="server" StoreID="SCostitem" DisplayField="ITEM_NAME"
                                                ValueField="ITEM_CODE" TypeAhead="false" LoadingText="Searching..." Width="150"
                                                ListWidth="220" PageSize="10" ItemSelector="div.search-item" MinChars="1">
                                                <Template ID="Template4" runat="server">
                                                  <tpl for=".">
                                                   <div class="search-item">
                                                     <h3>{ITEM_NAME}</h3>
                                                   </div>
                                                  </tpl>
                                                </Template>
                                            </ext:ComboBox>
                                            <ext:ComboBox ID="cbbType" runat="server" ReadOnly="true" ForceSelection="true" SelectOnFocus="true" SelectedIndex="1">
                                                <Items>
                                                   <ext:ListItem Text="收付实现" Value="1" />
                                                    <ext:ListItem Text="责权发生" Value="0" />
                                                </Items>
                                            </ext:ComboBox>
                                            <ext:ToolbarSpacer ID="ToolbarSpacer2" runat="server" Width="5">
                                            </ext:ToolbarSpacer>
                                           <ext:Button ID="btn_Query" runat="server" Text="查询" Icon="Zoom">
                                                <AjaxEvents>
                                                    <Click OnEvent="Btn_Query_Click" Timeout="99999999"></Click>
                                                </AjaxEvents>
                                            </ext:Button>
                                            <ext:ToolbarSpacer ID="ToolbarSpacer3" runat="server" Width="20">
                                            </ext:ToolbarSpacer>
                                            <ext:Button ID="btn_Excel" runat="server"  OnClick="OutExcel" AutoPostBack="true"   Text="导出Excel" Icon="PageWhiteExcel">
                                            </ext:Button>
                                        </Items>
                                    </ext:Toolbar>
                                </TopBar>
                                <Body>
                                    <center>
                                        <h2>各单位月份成本报表</h2>
                                    </center>
                                    <ext:GridPanel ID="GridPanel_Show" runat="server" StoreID="SReport" Border="true"
                                        Width="800" Height="400" AutoScroll="true" StyleSpec="margin:10px">
                                        <ColumnModel ID="ColumnModel1" runat="server">
                                            <Columns>
                                                <ext:Column ColumnID="DEPT_NAME" Header="<div style='text-align:center;'>科室</div>" Width="140" DataIndex="DEPT_NAME"
                                                    MenuDisabled="true" Align="Left">
                                                </ext:Column>
                                                <ext:Column ColumnID="ITEM_NAME" Header="<div style='text-align:center;'>项目</div>" Width="140" DataIndex="ITEM_NAME"
                                                    MenuDisabled="true" Align="Left">
                                                </ext:Column>
                                                <ext:Column ColumnID="COST_DATE" Header="<div style='text-align:center;'>发生日期</div>" Width="140" DataIndex="COST_DATE" MenuDisabled="true"
                                                    Align="Left">
                                                </ext:Column>
                                                <ext:Column ColumnID="COSTS" Header="<div style='text-align:center;'>实际成本</div>" Width="140" DataIndex="COSTS" MenuDisabled="true"
                                                    Align="Right">
                                                        <Renderer Fn="rmbMoney" />
                                                </ext:Column>
                                                <ext:Column ColumnID="COSTS_ARMYFREE" Header="<div style='text-align:center;'>计价成本</div>" Width="140" DataIndex="COSTS_ARMYFREE" MenuDisabled="true"
                                                    Align="Right">                                                    
                                                        <Renderer Fn="rmbMoney" />
                                                </ext:Column>
                                                <ext:Column ColumnID="TOTALCOST" Header="<div style='text-align:center;'>总金额</div>" Width="140" DataIndex="TOTALCOST" MenuDisabled="true"
                                                    Align="Right">                                                        
                                                        <Renderer Fn="rmbMoney" />
                                                </ext:Column>
                                                <ext:Column ColumnID="OPERATORNAME" Header="<div style='text-align:center;'>录入员</div>" Width="140" DataIndex="OPERATORNAME" MenuDisabled="true"
                                                    Align="Left">
                                                </ext:Column>
                                            </Columns>
                                        </ColumnModel>
                                        <SelectionModel>
                                            <ext:RowSelectionModel ID="rowselection" SingleSelect="true">
                                            </ext:RowSelectionModel>
                                        </SelectionModel>
                                        <Listeners>
                                            <BeforeRender Handler="Ext.EventManager.onWindowResize(function(){ if(Ext.getBody().getViewSize().width>850){this.setWidth( Ext.getBody().getViewSize().width -18);}this.setHeight( Ext.getBody().getViewSize().height -100); }, this)" />
                                            <Render Handler="if(Ext.getBody().getViewSize().width>850){this.setWidth( Ext.getBody().getViewSize().width -18);}this.setHeight( Ext.getBody().getViewSize().height -100);" />
                                        </Listeners>
                                        <LoadMask ShowMask="true" />
                                    </ext:GridPanel>
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
