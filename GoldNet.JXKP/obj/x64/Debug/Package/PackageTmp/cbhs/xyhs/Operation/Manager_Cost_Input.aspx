<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Manager_Cost_Input.aspx.cs" Inherits="GoldNet.JXKP.cbhs.xyhs.Operation.Manager_Cost_Input" %>
<%@ Register Assembly="Goldnet.Ext.Web" Namespace="Goldnet.Ext.Web" TagPrefix="ext" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
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
        
        //项目代码赋值
        function selectOrderedDept(cc)
        {
            var record2 = Store1.getAt(RowIndex);
            record2.data['ITEM_CODE'] = cc;
            GridPanel1.getView().focusRow(RowIndex);
        };
         function selectOrderedProg(cc)
        {
            var record2 = Store1.getAt(RowIndex);
            record2.data['PROG_CODE'] = cc;
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
                <ext:JsonReader ReaderID="ID">
                    <Fields>
                        <ext:RecordField Name="ID" Type="String" Mapping="ID" />
                        <ext:RecordField Name="ITEM_CODE" Type="String" Mapping="ITEM_CODE" />
                        <ext:RecordField Name="ITEM_NAME" Type="String" Mapping="ITEM_NAME" />
                       
                        <ext:RecordField Name="COSTS" Type="Float" Mapping="COSTS" />
                        <ext:RecordField Name="PROG_CODE" Type="String" Mapping="PROG_CODE" />
                        <ext:RecordField Name="PROG_NAME" Type="String" Mapping="PROG_NAME" />
                          <ext:RecordField Name="MEMO" Type="String" Mapping="MEMO" />
                    </Fields>
                </ext:JsonReader>
            </Reader>
        </ext:Store>
        <ext:Store ID="Store2" runat="server" AutoLoad="true">
            <Proxy>
            </Proxy>
            <Reader>
                <ext:JsonReader Root="itemlist" TotalProperty="totalCount">
                    <Fields>
                        <ext:RecordField Name="ITEM_CODE" />
                        <ext:RecordField Name="ITEM_NAME" />
                    </Fields>
                </ext:JsonReader>
            </Reader>
        </ext:Store>
          <ext:Store ID="Store3" runat="server" AutoLoad="true">
            <Proxy>
            </Proxy>
            <Reader>
                <ext:JsonReader Root="itemlist" TotalProperty="totalCount">
                    <Fields>
                        <ext:RecordField Name="PROG_CODE" />
                        <ext:RecordField Name="PROG_NAME" />
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
                                                           <ext:ComboBox ID="years" runat="server" Width="60" AllowBlank="true" EmptyText="请选择年..."
                                                FieldLabel="年">
                                               <AjaxEvents>
                                                    <Select OnEvent="Data_SelectOnChange">
                                                        <EventMask Msg='载入中...' ShowMask="true" />
                                                    </Select>
                                                </AjaxEvents>
                                            </ext:ComboBox>
                                            <ext:ToolbarTextItem ID="dd1Name" runat="server" Text="年 " />
                                            <ext:ComboBox ID="months" runat="server" Width="60" AllowBlank="true" EmptyText="请选择月..."
                                                FieldLabel="月">
                                                <Items>
                                                    <ext:ListItem Text="01" Value="01" />
                                                    <ext:ListItem Text="02" Value="02" />
                                                    <ext:ListItem Text="03" Value="03" />
                                                    <ext:ListItem Text="04" Value="04" />
                                                    <ext:ListItem Text="05" Value="05" />
                                                    <ext:ListItem Text="06" Value="06" />
                                                    <ext:ListItem Text="07" Value="07" />
                                                    <ext:ListItem Text="08" Value="08" />
                                                    <ext:ListItem Text="09" Value="09" />
                                                    <ext:ListItem Text="10" Value="10" />
                                                    <ext:ListItem Text="11" Value="11" />
                                                    <ext:ListItem Text="12" Value="12" />
                                                </Items>
                                                <AjaxEvents>
                                                    <Select OnEvent="Data_SelectOnChange">
                                                        <EventMask Msg='载入中...' ShowMask="true" />
                                                    </Select>
                                                </AjaxEvents>
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
                                                           
                                                            <ext:ToolbarSeparator ID="ToolbarSeparator1" runat="server" />
                                                            <ext:Button ID="Button2" runat="server" Text="增加">
                                                                <Listeners>
                                                                    <Click Handler="#{GridPanel1}.insertRecord(0, {});#{GridPanel1}.getView().focusRow(0);#{GridPanel1}.startEditing(0, 0);" />
                                                                </Listeners>
                                                            </ext:Button>
                                                           
                                                        </Items>
                                                    </ext:Toolbar>
                                                </TopBar>
                                                <ColumnModel ID="ColumnModel1" runat="server">
                                                    <Columns>
                                                        <ext:Column ColumnID="ID" Hidden="true" />
                                                        <ext:Column ColumnID="ITEM_NAME" Header="<div style='text-align:center;'>项目</div>"
                                                            Width="130" Align="left" DataIndex="ITEM_NAME" MenuDisabled="true">
                                                            <Editor>
                                                                <ext:ComboBox ID="ComboBox4" runat="server" StoreID="Store2" DisplayField="ITEM_CODE"
                                                                    AllowBlank="true" ValueField="ITEM_NAME" TypeAhead="false" LoadingText="Searching..."
                                                                    Width="220" ListWidth="220" PageSize="10" ItemSelector="div.search-item" MinChars="1">
                                                                    <Template ID="Template5" runat="server">
                                                                  <tpl for=".">
                                                                   <div class="search-item">
                                                                     <h3><span>{ITEM_NAME}</span>{ITEM_CODE}</h3>
                                                                   </div>
                                                                  </tpl>
                                                                    </Template>
                                                                    <Listeners>
                                                                        <Select Handler="selectOrderedDept(this.getText());" />
                                                                    </Listeners>
                                                                </ext:ComboBox>
                                                            </Editor>
                                                        </ext:Column>
                                                        <ext:Column ColumnID="COSTS" Header="<div style='text-align:center;'>成本额</div>"
                                                            Width="130" Align="Right" Sortable="false" DataIndex="COSTS" MenuDisabled="true">
                                                            <Editor>
                                                                <ext:NumberField ID="NumberField1" runat="server" />
                                                            </Editor>
                                                            <Renderer Fn="rmbMoney" />
                                                        </ext:Column>
                                                       <ext:Column ColumnID="PROG_NAME" Header="<div style='text-align:center;'>分解方案</div>"
                                                            Width="130" Align="left" DataIndex="PROG_NAME" MenuDisabled="true">
                                                            <Editor>
                                                                <ext:ComboBox ID="ComboBox1" runat="server" StoreID="Store3" DisplayField="PROG_CODE"
                                                                    AllowBlank="true" ValueField="PROG_NAME" TypeAhead="false" LoadingText="Searching..."
                                                                    Width="220" ListWidth="220" PageSize="10" ItemSelector="div.search-item" MinChars="1">
                                                                    <Template ID="Template1" runat="server">
                                                                  <tpl for=".">
                                                                   <div class="search-item">
                                                                     <h3><span>{PROG_NAME}</span>{PROG_CODE}</h3>
                                                                   </div>
                                                                  </tpl>
                                                                    </Template>
                                                                    <Listeners>
                                                                        <Select Handler="selectOrderedProg(this.getText());" />
                                                                    </Listeners>
                                                                </ext:ComboBox>
                                                            </Editor>
                                                        </ext:Column>
                                                        <ext:Column ColumnID="MEMO" Header="<div style='text-align:center;'>备注</div>" Width="300"
                                                            Align="left" Sortable="false" DataIndex="MEMO" MenuDisabled="true">
                                                            <Editor>
                                                                <ext:TextField ID="NumberField2" runat="server" />
                                                            </Editor>
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
                
                </ext:BorderLayout>
            </Body>
        </ext:ViewPort>
       
    </div>
    </form>
</body>
</html>
