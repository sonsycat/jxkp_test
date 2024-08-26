<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="single_cost_input_new.aspx.cs"
    Inherits="GoldNet.JXKP.cbhs.datagather.single_cost_input_new" %>

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
        
        function openwin()
        {
            var cc="";
            var str = document.getElementById('stardate').value ; 
            var   reg=/^(\d{4})(\d{2})(\d{2})$/; 
            cc  =   str.replace(reg, '$1-$2-$3');
            var costitem = COST_ITEM.getSelectedItem().value;

            var url="single_cost_submit_new.aspx?pageid=010104_1&startime="+cc+"&costitem="+costitem;
            var newwindow=window.open(url,'newwin','resizable=no,scrollbars=yes,status=yes,toolbar=no,menubar=no,location=no');
            var wide = window.screen.availWidth;
            var high = window.screen.availHeight;
            newwindow.resizeTo(wide,high);
            newwindow.moveTo(0,0);
            
        };
        
    </script>

</head>
<body>
    <form id="form1" runat="server">
    <div>
        <ext:ScriptManager ID="ScriptManager1" runat="server" AjaxMethodNamespace="CompanyX" />
        <ext:Store ID="Store1" runat="server" OnRefreshData="Data_RefreshData" AutoLoad="true">
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
                                            <ext:GridPanel ID="GridPanel1" runat="server" StoreID="Store1" StripeRows="true"
                                                ClicksToEdit="1" TrackMouseOver="true" AutoWidth="true" Height="480" Border="false">
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
                                                                <%--                                                                <AjaxEvents>
                                                                    <Select OnEvent="Item_SelectOnChange">
                                                                        <EventMask Msg='载入中...' ShowMask="true" />
                                                                    </Select>
                                                                </AjaxEvents>--%>
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
                                                            <ext:Button ID="Button_del" runat="server" Text="删除" Icon="Delete" Disabled="true">
                                                                <AjaxEvents>
                                                                    <Click OnEvent="Button_del_click">
                                                                        <Confirmation ConfirmRequest="true" Title="系统提示" Message="将删除选中数据,<br/>是否继续?" />
                                                                        <ExtraParams>
                                                                        </ExtraParams>
                                                                        <EventMask Msg="载入中..." ShowMask="true" />
                                                                        <ExtraParams>
                                                                            <ext:Parameter Name="Values" Value="Ext.encode(#{GridPanel1}.getRowsValues())" Mode="Raw">
                                                                            </ext:Parameter>
                                                                        </ExtraParams>
                                                                    </Click>
                                                                </AjaxEvents>
                                                            </ext:Button>
                                                            <ext:ToolbarSeparator ID="ToolbarSeparator4" runat="server" />
                                                            <ext:Button ID="Button_save" runat="server" Text="保存" Icon="Disk">
                                                                <AjaxEvents>
                                                                    <Click OnEvent="Button_Save_click">
                                                                        <EventMask Msg="保存中..." ShowMask="true" />
                                                                        <ExtraParams>
                                                                            <ext:Parameter Name="Values" Value="Ext.encode(#{GridPanel1}.getRowsValues(false))"
                                                                                Mode="Raw">
                                                                            </ext:Parameter>
                                                                        </ExtraParams>
                                                                    </Click>
                                                                </AjaxEvents>
                                                            </ext:Button>
                                                            <ext:ToolbarSeparator ID="ToolbarSeparator7" runat="server" />
                                                            <ext:Button ID="btn_Excel" runat="server" OnClick="OutExcel" AutoPostBack="true"
                                                                Text="导出Excel" Icon="PageWhiteExcel" CausesValidation="false">
                                                            </ext:Button>
                                                            <ext:ToolbarSeparator ID="ToolbarSeparator6" runat="server" />
                                                            <ext:Button ID="costs" runat="server" Text="成本分解" Icon="ArrowOut">
                                                                <AjaxEvents>
                                                                    <Click OnEvent="costs_click" Timeout="99999999">
                                                                        <EventMask Msg="载入中..." ShowMask="true" />
                                                                    </Click>
                                                                </AjaxEvents>
                                                            </ext:Button>
                                                            <ext:ToolbarSeparator ID="ToolbarSeparator1" runat="server" />
                                                            <ext:Button ID="Button2" runat="server" Text="新增">
                                                                <Listeners>
                                                                    <Click Handler="#{GridPanel1}.insertRecord(0, {});#{GridPanel1}.getView().focusRow(0);#{GridPanel1}.startEditing(0, 0);" />
                                                                </Listeners>
                                                            </ext:Button>
                                                            <ext:Button ID="Btn_Add" Text="导入EXECL数据" Icon="Add" runat="server" Hidden="true">
                                                                <AjaxEvents>
                                                                    <Click OnEvent="Btn_Add_Click">
                                                                    </Click>
                                                                </AjaxEvents>
                                                            </ext:Button>
                                                        </Items>
                                                    </ext:Toolbar>
                                                </TopBar>
                                                <ColumnModel ID="ColumnModel1" runat="server">
                                                    <Columns>
                                                        <ext:Column ColumnID="ROW_ID" Hidden="true" />
                                                        <ext:Column ColumnID="DEPT_NAME" Header="<div style='text-align:center;'>科室</div>"
                                                            Width="130" Align="left" DataIndex="DEPT_NAME" MenuDisabled="true">
                                                            <Editor>
                                                                <ext:ComboBox ID="ComboBox4" runat="server" StoreID="Store2" DisplayField="DEPT_CODE"
                                                                    AllowBlank="true" ValueField="DEPT_NAME" TypeAhead="false" LoadingText="Searching..."
                                                                    Width="220" ListWidth="220" PageSize="10" ItemSelector="div.search-item" MinChars="1">
                                                                    <Template ID="Template5" runat="server">
                                                                  <tpl for=".">
                                                                   <div class="search-item">
                                                                     <h3><span>{DEPT_NAME}</span>{DEPT_CODE}</h3>
                                                                   </div>
                                                                  </tpl>
                                                                    </Template>
                                                                    <Listeners>
                                                                        <Select Handler="selectOrderedDept(this.getText());" />
                                                                    </Listeners>
                                                                </ext:ComboBox>
                                                            </Editor>
                                                        </ext:Column>
                                                        <ext:Column ColumnID="TOTAL_COSTS" Header="<div style='text-align:center;'>成本额</div>"
                                                            Width="130" Align="Right" Sortable="false" DataIndex="TOTAL_COSTS" MenuDisabled="true">
                                                            <Editor>
                                                                <ext:NumberField ID="NumberField1" runat="server" />
                                                            </Editor>
                                                            <Renderer Fn="rmbMoney" />
                                                        </ext:Column>
                                                        <ext:Column ColumnID="COSTS" Header="<div style='text-align:center;'>实际成本</div>"
                                                            Width="130" Align="Right" Sortable="false" DataIndex="COSTS" MenuDisabled="true">
                                                            <Renderer Fn="rmbMoney" />
                                                        </ext:Column>
                                                        <ext:Column ColumnID="COSTS_ARMYFREE" Header="<div style='text-align:center;'>减免成本</div>"
                                                            Width="130" Align="Right" Sortable="false" DataIndex="COSTS_ARMYFREE" MenuDisabled="true">
                                                            <Renderer Fn="rmbMoney" />
                                                        </ext:Column>
                                                        <ext:Column ColumnID="MEMO" Header="<div style='text-align:center;'>备注</div>" Width="150"
                                                            Align="left" Sortable="false" DataIndex="MEMO" MenuDisabled="true">
                                                            <Editor>
                                                                <ext:TextField ID="NumberField2" runat="server" />
                                                            </Editor>
                                                        </ext:Column>
                                                        <ext:Column ColumnID="GET_NAME" Header="<div style='text-align:center;'>类型</div>"
                                                            Width="80" Align="Center" Sortable="false" DataIndex="GET_NAME" MenuDisabled="true">
                                                        </ext:Column>
                                                    </Columns>
                                                </ColumnModel>
                                                <SelectionModel>
                                                    <ext:CheckboxSelectionModel ID="RowSelectionModel1" runat="server">
                                                        <Listeners>
                                                            <RowSelect Handler="RowIndex = rowIndex" />
                                                            <RowDeselect Handler="RowIndex = rowIndex" />
                                                            <SelectionChange Handler="#{GridPanel1}.hasSelection()? #{Button_del}.setDisabled(false): #{Button_del}.setDisabled(true);" />
                                                        </Listeners>
                                                    </ext:CheckboxSelectionModel>
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
                                                                    </ext:HeaderColumn>
                                                                    <ext:HeaderColumn>
                                                                        <Component>
                                                                            <ext:TextField runat="server" ID="CB_SUM" ReadOnly="true" StyleSpec="text-align:right">
                                                                            </ext:TextField>
                                                                        </Component>
                                                                    </ext:HeaderColumn>
                                                                    <ext:HeaderColumn>
                                                                        <Component>
                                                                            <ext:TextField runat="server" ID="SJ_SUM" ReadOnly="true" StyleSpec="text-align:right">
                                                                            </ext:TextField>
                                                                        </Component>
                                                                    </ext:HeaderColumn>
                                                                    <ext:HeaderColumn>
                                                                        <Component>
                                                                            <ext:TextField runat="server" ID="JM_SUM" ReadOnly="true" StyleSpec="text-align:right">
                                                                            </ext:TextField>
                                                                        </Component>
                                                                    </ext:HeaderColumn>
                                                                    <ext:HeaderColumn>
                                                                    </ext:HeaderColumn>
                                                                </Columns>
                                                            </ext:HeaderRow>
                                                        </HeaderRows>
                                                    </ext:GridView>
                                                </View>
                                                <Listeners>
                                                    <KeyDown Handler="if (e.getKey() == 40){ #{GridPanel1}.insertRecord(0, {});#{GridPanel1}.getView().focusRow(0);#{GridPanel1}.startEditing(0, 0);} ;" />
                                                </Listeners>
                                            </ext:GridPanel>
                                        </ext:LayoutColumn>
                                    </Columns>
                                </ext:ColumnLayout>
                            </Body>
                        </ext:Panel>
                    </Center>
                    <East MinWidth="200" MaxWidth="400" SplitTip="成本提交状态信息" Collapsible="true" Split="true">
                        <ext:Panel ID="Panel1" runat="server" Border="false" Width="350" Title="成本提交状态信息"
                            Collapsed="false" AutoScroll="true">
                            <TopBar>
                                <ext:Toolbar ID="Toolbar1" runat="server">
                                    <Items>
                                        <ext:Button ID="Button_ok" runat="server" Text="提交" Icon="Disk">
                                            <%--<AjaxEvents>
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
                                            </AjaxEvents>--%>
                                            <Listeners>
                                                <Click Handler="openwin();" />
                                            </Listeners>
                                        </ext:Button>
                                        <%--<ext:ToolbarSeparator ID="ToolbarSeparator8" runat="server" />--%>
                                        <ext:Button ID="Button1" runat="server" Text="审核" Icon="Disk" Hidden="true">
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
                                        <%--<ext:ToolbarSeparator ID="ToolbarSeparator9" runat="server" />--%>
                                        <ext:Button ID="Buttonfh" runat="server" Text="复核" Icon="Disk" Hidden="true">
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
                                        <%--<ext:ToolbarSeparator ID="ToolbarSeparator5" runat="server" />--%>
                                        <ext:Button ID="Button_no" runat="server" Text="取消" Icon="Disk" Hidden="true">
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
                            
                            <Body>
                                <ext:ColumnLayout ID="ColumnLayout2" runat="server" Split="true">
                                    <Columns>
                                        <ext:LayoutColumn ColumnWidth="1">
                                            <ext:GridPanel ID="GridPanel2" runat="server" Border="false" StoreID="Store3" StripeRows="true"
                                                AutoHeight="true" AutoWidth="true" TrackMouseOver="true" AutoScroll="true">
                                                <ColumnModel ID="ColumnModel2" runat="server">
                                                    <Columns>
                                                        <ext:Column ColumnID="ITEM_CODE" Hidden="true" />
                                                        <ext:Column Header="成本项目" Width="100" ColumnID="ITEM_NAME" DataIndex="ITEM_NAME"
                                                            Sortable="false" MenuDisabled="true" />
                                                        <ext:Column Header="状态" Width="60" ColumnID="FLAGS" DataIndex="FLAGS" Sortable="false"
                                                            MenuDisabled="true" />
                                                        <ext:Column Header="提交人" Width="60" ColumnID="SUBMIT_PERSONS" DataIndex="SUBMIT_PERSONS"
                                                            Sortable="false" MenuDisabled="true" />
                                                        <ext:Column Header="审核人" Width="60" ColumnID="CHECK_NAME" DataIndex="CHECK_NAME"
                                                            Sortable="false" MenuDisabled="true" />
                                                        <ext:Column Header="复核人" Width="60" ColumnID="COMP_NAME" DataIndex="COMP_NAME" Sortable="false"
                                                            MenuDisabled="true" />
                                                    </Columns>
                                                </ColumnModel>
                                                <SelectionModel>
                                                    <ext:CheckboxSelectionModel ID="RowSelectionModel2" runat="server">
                                                    </ext:CheckboxSelectionModel>
                                                </SelectionModel>
                                                <Listeners>
                                                </Listeners>
                                                <LoadMask ShowMask="true" />
                                            </ext:GridPanel>
                                        </ext:LayoutColumn>
                                    </Columns>
                                </ext:ColumnLayout>
                            </Body>
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
