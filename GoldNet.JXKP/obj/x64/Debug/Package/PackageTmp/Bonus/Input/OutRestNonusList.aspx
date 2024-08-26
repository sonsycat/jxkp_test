<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="OutRestNonusList.aspx.cs"
    Inherits="GoldNet.JXKP.Bonus.Input.OutRestNonusList" %>

<%@ Register Assembly="Goldnet.Ext.Web" Namespace="Goldnet.Ext.Web" TagPrefix="ext" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link rel="stylesheet" type="text/css" href="../Orthers/Cbouns.css" />
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
          var RowIndex;
          
          //刷新
          var RefreshData = function() {
              Store1.reload();
          }
         
         //数据格式化
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
         
         //类型赋值
         function selectOrderedItem(cc)
         {
            var record2 = Store1.getAt(RowIndex);
            Store1.data['HOLIDAY_FLAG'] = cc;
            GridPanel2.getView().focusRow(RowIndex);
         };
         
         //日期格式化
         function FormatRender(v, p, record, rowIndex) {
            var colors = ["red", "black","blue"];
            if(record.data.HOLIDAY_FLAG=="双休日" || record.data.HOLIDAY_FLAG=="假日")
            {
                var template = '<span style="color:{0};font-weight:bold;">{1}</span>';
                return String.format(template, colors[0], record.data.RIQI);
            }
            else
            {
                var templateb = '<span style="color:{0};">{1}</span>';
                return String.format(templateb, colors[1], record.data.RIQI);
            }
        }
        
        //星期格式化
        function FormatRender2(v, p, record, rowIndex) {
            var colors = ["red", "black","blue"];
            if(record.data.HOLIDAY_FLAG=="双休日" || record.data.HOLIDAY_FLAG=="假日")
            {
                var template = '<span style="color:{0};font-weight:bold;">{1}</span>';
                return String.format(template, colors[0], record.data.WEEKS);
            }
            else
            {
                var templateb = '<span style="color:{0};">{1}</span>';
                return String.format(templateb, colors[1], record.data.WEEKS);
            }
        }
    </script>

    <script type="text/javascript" src="../Orthers/Cbouns.js"></script>

</head>
<body>
    <ext:ScriptManager ID="ScriptManager1" runat="server" />
    <form id="form1" runat="server">
    <ext:Store ID="Store1" AutoLoad="true" runat="server" OnRefreshData="Store_RefreshData">
        <Reader>
            <ext:JsonReader>
                <Fields>
                    <ext:RecordField Name="RIQI">
                    </ext:RecordField>
                    <ext:RecordField Name="WEEKS_ID">
                    </ext:RecordField>
                    <ext:RecordField Name="WEEKS">
                    </ext:RecordField>
                    <ext:RecordField Name="HOLIDAY_FLAG">
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
    <ext:Store ID="Store4" runat="server" AutoLoad="true">
        <Proxy>
        </Proxy>
        <Reader>
            <ext:JsonReader>
                <Fields>
                    <ext:RecordField Name="HOLIDAY_FLAG" />
                    <ext:RecordField Name="HOLIDAY_NAME" />
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
                                TrackMouseOver="true" Height="480" ClicksToEdit="1" >
                                <TopBar>
                                    <ext:Toolbar ID="Toolbar_detptype" runat="server" Visible="true" AutoWidth="true">
                                        <Items>
                                            <ext:Label ID="Label1" runat="server" Text="年度：" />
                                            <ext:ComboBox ID="Combo_Year" runat="server" ReadOnly="true" StoreID="SYear" Width="60"
                                                DisplayField="YEAR" ValueField="YEAR" ForceSelection="true" SelectOnFocus="true">
                                            </ext:ComboBox>
                                            <ext:ToolbarSpacer ID="ToolbarSpacer11" runat="server" Width="15" />
                                            <ext:ComboBox ID="cbbmonth" runat="server" ReadOnly="true" StoreID="SMonth" Width="50"
                                                DisplayField="MONTH" ValueField="MONTH">
                                            </ext:ComboBox>
                                            <ext:Button ID="Btn_Add" Text="查询" Icon="Add" runat="server">
                                                <AjaxEvents>
                                                    <Click OnEvent="GetQueryPortalet">
                                                    </Click>
                                                </AjaxEvents>
                                            </ext:Button>
                                            <ext:Button ID="Btn_Del" Text="保存" Icon="Delete" runat="server">
                                                <AjaxEvents>
                                                    <Click OnEvent="Btn_Del_Click">
                                                        <Confirmation BeforeConfirm="config.confirmation.message = '你确定要保存吗？';" Title="系统提示"
                                                            ConfirmRequest="true" />
                                                        <ExtraParams>
                                                            <ext:Parameter Name="Values" Value="Ext.encode(#{GridPanel2}.getRowsValues(false))" Mode="Raw" />
                                                        </ExtraParams>
                                                    </Click>
                                                </AjaxEvents>
                                            </ext:Button>
                                        </Items>
                                    </ext:Toolbar>
                                </TopBar>
                                <ColumnModel ID="ColumnModel2" runat="server">
                                    <Columns>
                                        <ext:Column ColumnID="RIQI" Header="日期" Width="150" DataIndex="RIQI" MenuDisabled="true"
                                            Align="Center">
                                            <Renderer Fn="FormatRender" />
                                        </ext:Column>
                                        <ext:Column ColumnID="WEEKS" Header="<div style='text-align:center;'>星期</div>" Width="150"
                                            DataIndex="WEEKS" MenuDisabled="true" Align="Left">
                                            <Renderer Fn="FormatRender2" />
                                        </ext:Column>
                                        <ext:Column ColumnID="HOLIDAY_FLAG" Header="类型" Width="150" DataIndex="HOLIDAY_FLAG"
                                            MenuDisabled="true" Align="Center">
                                            <Editor>
                                                <ext:ComboBox ID="ComboBox2" runat="server" StoreID="Store4" DisplayField="HOLIDAY_FLAG"
                                                    ValueField="HOLIDAY_FLAG" TypeAhead="false" LoadingText="Searching..."
                                                    Width="200" ListWidth="200" PageSize="15" ItemSelector="div.search-item" MinChars="1">
                                                    <Template ID="Template3" runat="server">
                                                                  <tpl for=".">
                                                                   <div class="search-item">
                                                                     <h3>{HOLIDAY_FLAG}</h3>
                                                                   </div>
                                                                  </tpl>
                                                    </Template>
                                                    <Listeners>
                                                        <Select Handler="selectOrderedItem(this.getText());" />
                                                    </Listeners>
                                                </ext:ComboBox>
                                            </Editor>
                                        </ext:Column>
                                    </Columns>
                                </ColumnModel>
                                <SelectionModel>
                                    <ext:RowSelectionModel ID="rowselection" SingleSelect="true">
                                        <Listeners>
                                            <RowSelect Handler="RowIndex = rowIndex" />
                                        </Listeners>
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
