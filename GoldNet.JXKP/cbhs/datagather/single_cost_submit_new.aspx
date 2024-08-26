<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="single_cost_submit_new.aspx.cs"
    Inherits="GoldNet.JXKP.cbhs.datagather.single_cost_submit_new" %>

<%@ Register Assembly="Goldnet.Ext.Web" Namespace="Goldnet.Ext.Web" TagPrefix="ext" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
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
    <link rel="stylesheet" type="text/css" href="../../Bonus/Orthers/Cbouns.css" />

    <script language="javascript" type="text/javascript">
        
        var RowIndex;
        
        //列表刷新
        var RefreshData = function() {
            Store1.reload();
        };
        
        //列表单元格格式化（金额单元）
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
        
        //列表单元格
        function FormatRender(v, p, record, rowIndex) {
            var colors = ["red", "black","blue"];
            if(record.data.FLAG=="1")
            {
                var template = '<span style="color:{0};font-weight:bold;">{1}</span>';
                return String.format(template, colors[0], record.data.DEPT_NAME);
            }
            else
            {
                var templateb = '<span style="color:{0};">{1}</span>';
                return String.format(templateb, colors[1], record.data.DEPT_NAME);
            }
        }
        
        //科室代码赋值
        function selectOrderedDept(cc)
        {
            var record2 = Store1.getAt(RowIndex);
            record2.data['DEPT_CODE'] = cc;
            GridPanel1.getView().focusRow(RowIndex);
        };
        
    </script>

</head>
<body>
    <form id="form1" runat="server">
    <div>
        <ext:ScriptManager ID="ScriptManager1" runat="server" AjaxMethodNamespace="CompanyX" />
        <ext:Store ID="Store1" runat="server" >
            <Reader>
                <ext:JsonReader ReaderID="ROW_ID">
                    <Fields>
                        <ext:RecordField Name="ROW_ID" Type="String" Mapping="ROW_ID" />
                        <ext:RecordField Name="DEPT_CODE" Type="String" Mapping="DEPT_CODE" />
                        <ext:RecordField Name="DEPT_NAME" Type="String" Mapping="DEPT_NAME" />
                        <ext:RecordField Name="ITEM_CODE" Type="String" Mapping="ITEM_CODE" />
                        <ext:RecordField Name="ACCOUNTING_DATE" Type="String" Mapping="ACCOUNTING_DATE" />
                        <ext:RecordField Name="TOTAL_COSTS" Type="Float" Mapping="TOTAL_COSTS" />
                        <ext:RecordField Name="COSTS" Type="Float" Mapping="COSTS" />
                        <ext:RecordField Name="COSTS_ARMYFREE" Type="Float" Mapping="COSTS_ARMYFREE" />
                        <ext:RecordField Name="OPERATOR" Type="String" Mapping="OPERATOR" />
                        <ext:RecordField Name="OPERATOR_DATE" Type="String" Mapping="OPERATOR_DATE" />
                        <ext:RecordField Name="GET_TYPE" Type="String" Mapping="GET_TYPE" />
                        <ext:RecordField Name="COST_FLAG" Type="String" Mapping="COST_FLAG" />
                        <ext:RecordField Name="BALANCE_TAG" Type="String" Mapping="BALANCE_TAG" />
                        <ext:RecordField Name="DEPT_TYPE_FLAG" Type="String" Mapping="DEPT_TYPE_FLAG" />
                        <ext:RecordField Name="MEMO" Type="String" Mapping="MEMO" />
                        <ext:RecordField Name="FLAG" Type="String" />
                        <ext:RecordField Name="GET_TYPE" Type="String" />
                        <ext:RecordField Name="GET_NAME" Type="String" />
                    </Fields>
                </ext:JsonReader>
            </Reader>
        </ext:Store>
        <ext:Store ID="Store2" runat="server" AutoLoad="true">
            <Proxy>
            </Proxy>
            <Reader>
                <ext:JsonReader Root="deptlist" TotalProperty="totalCount">
                    <Fields>
                        <ext:RecordField Name="DEPT_CODE" />
                        <ext:RecordField Name="DEPT_NAME" />
                    </Fields>
                </ext:JsonReader>
            </Reader>
        </ext:Store>
        <ext:Store ID="Store3" runat="server" WarningOnDirty="false">
            <Reader>
                <ext:JsonReader ReaderID="ITEM_CODE">
                    <Fields>
                        <ext:RecordField Name="ITEM_CODE" Type="String" Mapping="ITEM_CODE" />
                        <ext:RecordField Name="ITEM_NAME" Type="String" Mapping="ITEM_NAME" />
                        <ext:RecordField Name="FLAGS" Type="String" Mapping="FLAGS" />
                        <ext:RecordField Name="SUBMIT_PERSONS" Type="String" Mapping="SUBMIT_PERSONS" />
                        <ext:RecordField Name="CHECK_FLAGS" Type="String" Mapping="CHECK_FLAGS" />
                        <ext:RecordField Name="CHECK_NAME" Type="String" Mapping="CHECK_NAME" />
                        <ext:RecordField Name="COMP_FLAGS" Type="String" Mapping="COMP_FLAGS" />
                        <ext:RecordField Name="COMP_NAME" Type="String" Mapping="COMP_NAME" />
                    </Fields>
                </ext:JsonReader>
            </Reader>
        </ext:Store>
        <ext:ViewPort ID="ViewPort1" runat="server">
            <Body>
                <ext:BorderLayout ID="BorderLayout1" runat="server">
                    <Center>
                        <ext:Panel ID="Panel2" runat="server" BodyBorder="true" Border="false">
                            <Body>
                                <ext:ColumnLayout ID="ColumnLayout1" runat="server" Split="true" FitHeight="true">
                                    <Columns>
                                        <ext:LayoutColumn ColumnWidth="1">
                                            <ext:GridPanel ID="GridPanel2" runat="server" Border="false" StoreID="Store3" StripeRows="true"
                                                AutoWidth="true" TrackMouseOver="true" Height="480" AutoScroll="true">
                                                <TopBar>
                                                    <ext:Toolbar ID="Toolbar_fjsr" runat="server" Visible="true" AutoWidth="true">
                                                        <Items>
                                                            <ext:Label ID="Label1" runat="server" Text="时间：" />
                                                            <ext:DateField ID="stardate" runat="server" FieldLabel="时间：" Width="80" EnableKeyEvents="true"
                                                                ReadOnly="true" />
                                                            <ext:KeyNav ID="stardate1" runat="server" Target="stardate">
                                                                <Enter Handler="var str = document.getElementById('stardate').value ; var   reg=/^(\d{4})(\d{2})(\d{2})$/; document.getElementById('stardate').value   =   str.replace(reg, '$1-$2-$3');" />
                                                            </ext:KeyNav>
                                                            <ext:Label ID="Label3" runat="server" Text="成本项目：" />
                                                            <ext:ComboBox ID="COST_ITEM" runat="server" Width="100" AllowBlank="true" EmptyText="请选择...">
                                                            </ext:ComboBox>
                                                            <ext:ComboBox ID="cbbType" runat="server" ReadOnly="true" ForceSelection="true" SelectOnFocus="true"
                                                                Width="70" SelectedIndex="0">
                                                                <Items>
                                                                    <ext:ListItem Text="分解前" Value="0" />
                                                                    <ext:ListItem Text="分解后" Value="1" />
                                                                </Items>
                                                            </ext:ComboBox>
                                                            <ext:ToolbarSeparator ID="ToolbarSeparator2" runat="server" />
                                                            <ext:Button ID="Button_look" runat="server" Text="查询" Icon="DatabaseGo">
                                                                <AjaxEvents>
                                                                    <Click OnEvent="Button_look_click" Timeout="99999999">
                                                                        <EventMask Msg="载入中..." ShowMask="true" />
                                                                    </Click>
                                                                </AjaxEvents>
                                                            </ext:Button>
                                                            <ext:ToolbarSeparator ID="ToolbarSeparator3" runat="server" />
                                                            <ext:Button ID="Button_ok" runat="server" Text="提交" Icon="Disk">
                                                                <AjaxEvents>
                                                                    <Click OnEvent="Button_ok_click">
                                                                        <Confirmation ConfirmRequest="true" Title="系统提示" Message="将提交选中的成本,<br/>请确认成本项目是否已经保存?" />
                                                                        <ExtraParams>
                                                                        </ExtraParams>
                                                                        <EventMask Msg="载入中..." ShowMask="true" />
                                                                        <ExtraParams>
                                                                            <ext:Parameter Name="Values" Value="Ext.encode(#{GridPanel2}.getRowsValues())" Mode="Raw">
                                                                            </ext:Parameter>
                                                                        </ExtraParams>
                                                                    </Click>
                                                                </AjaxEvents>
                                                            </ext:Button>
                                                            <ext:ToolbarSeparator ID="ToolbarSeparator7" runat="server" />
                                                            <ext:Button ID="Button1" runat="server" Text="审核" Icon="Disk">
                                                                <AjaxEvents>
                                                                    <Click OnEvent="Button_sh_click">
                                                                        <Confirmation ConfirmRequest="true" Title="系统提示" Message="将审核选中的成本,<br/>是否继续?" />
                                                                        <ExtraParams>
                                                                        </ExtraParams>
                                                                        <EventMask Msg="载入中..." ShowMask="true" />
                                                                        <ExtraParams>
                                                                            <ext:Parameter Name="Values" Value="Ext.encode(#{GridPanel2}.getRowsValues())" Mode="Raw">
                                                                            </ext:Parameter>
                                                                        </ExtraParams>
                                                                    </Click>
                                                                </AjaxEvents>
                                                            </ext:Button>
                                                            <ext:ToolbarSeparator ID="ToolbarSeparator1" runat="server" />
                                                            <ext:Button ID="Buttonfh" runat="server" Text="复核" Icon="Disk">
                                                                <AjaxEvents>
                                                                    <Click OnEvent="Button_fh_click">
                                                                        <Confirmation ConfirmRequest="true" Title="系统提示" Message="将复合选中的成本,<br/>是否继续?" />
                                                                        <ExtraParams>
                                                                        </ExtraParams>
                                                                        <EventMask Msg="载入中..." ShowMask="true" />
                                                                        <ExtraParams>
                                                                            <ext:Parameter Name="Values" Value="Ext.encode(#{GridPanel2}.getRowsValues())" Mode="Raw">
                                                                            </ext:Parameter>
                                                                        </ExtraParams>
                                                                    </Click>
                                                                </AjaxEvents>
                                                            </ext:Button>
                                                            <ext:ToolbarSeparator ID="ToolbarSeparator4" runat="server" />
                                                            <ext:Button ID="Button_no" runat="server" Text="取消" Icon="Disk">
                                                                <AjaxEvents>
                                                                    <Click OnEvent="Button_no_click">
                                                                        <Confirmation ConfirmRequest="true" Title="系统提示" Message="将取消选中的成本,<br/>是否继续?" />
                                                                        <ExtraParams>
                                                                        </ExtraParams>
                                                                        <EventMask Msg="载入中..." ShowMask="true" />
                                                                        <ExtraParams>
                                                                            <ext:Parameter Name="Values" Value="Ext.encode(#{GridPanel2}.getRowsValues())" Mode="Raw">
                                                                            </ext:Parameter>
                                                                        </ExtraParams>
                                                                    </Click>
                                                                </AjaxEvents>
                                                            </ext:Button>
                                                        </Items>
                                                    </ext:Toolbar>
                                                </TopBar>
                                                <ColumnModel ID="ColumnModel2" runat="server">
                                                    <Columns>
                                                        <ext:Column ColumnID="ITEM_CODE" Hidden="true" />
                                                        <ext:Column Header="成本项目" Width="150" ColumnID="ITEM_NAME" DataIndex="ITEM_NAME"
                                                            Sortable="false" MenuDisabled="true" />
                                                        <ext:Column Header="状态" Width="80" ColumnID="FLAGS" DataIndex="FLAGS" Sortable="false"
                                                            MenuDisabled="true" />
                                                        <ext:Column Header="提交人" Width="80" ColumnID="SUBMIT_PERSONS" DataIndex="SUBMIT_PERSONS"
                                                            Sortable="false" MenuDisabled="true" />
                                                        <ext:Column Header="审核人" Width="80" ColumnID="CHECK_NAME" DataIndex="CHECK_NAME"
                                                            Sortable="false" MenuDisabled="true" />
                                                        <ext:Column Header="复核人" Width="80" ColumnID="COMP_NAME" DataIndex="COMP_NAME" Sortable="false"
                                                            MenuDisabled="true" />
                                                    </Columns>
                                                </ColumnModel>
                                                <SelectionModel>
                                                    <ext:CheckboxSelectionModel ID="RowSelectionModel2" runat="server">
                                                    </ext:CheckboxSelectionModel>
                                                </SelectionModel>
                                                <LoadMask ShowMask="true" />
                                            </ext:GridPanel>
                                        </ext:LayoutColumn>
                                    </Columns>
                                </ext:ColumnLayout>
                            </Body>
                        </ext:Panel>
                    </Center>
                    <East MinWidth="200" MaxWidth="400" SplitTip="成本提交状态信息" Collapsible="true" Split="true">
                        <ext:Panel ID="Panel1" runat="server" Border="false" Width="0" Title="成本提交状态信息" Collapsed="false"
                            AutoScroll="true">
                        </ext:Panel>
                    </East>
                </ext:BorderLayout>
            </Body>
        </ext:ViewPort>
        <ext:Window ID="DetailWin" runat="server" Icon="Add" Title="导入execl数据" Width="360"
            Height="200" AutoShow="false" Modal="true" CenterOnLoad="true" AutoScroll="false"
            ShowOnLoad="false" Closable="false" Resizable="false" StyleSpec="background-color:Transparent;"
            BodyStyle="background-color:Transparent;" CloseAction="Hide">
        </ext:Window>
    </div>
    </form>
</body>
</html>
