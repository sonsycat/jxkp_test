<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PlanGuideStats.aspx.cs"
    Inherits="GoldNet.JXKP.GuideLook.Statement.PlanGuideStats" %>

<%@ Register Assembly="Goldnet.Ext.Web" Namespace="Goldnet.Ext.Web" TagPrefix="ext" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link rel="stylesheet" type="text/css" href="../../Bonus/Orthers/Cbouns.css" />

    <style type="text/css">

     </style>
    <script language="javascript" type="text/javascript">
        var rmbMoney = function(v) {
               if(v==null||v=="")
               {
               return "";
               }
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
       
       function rowselect(grid, rowIndex, columnIndex) {
            var model = grid.getSelectionModel()
            if (columnIndex != 0 & columnIndex != 1 ) {                
                    model.deselectRow(rowIndex);
                }

            };
    </script>

</head>
<body>
    <form id="form1" runat="server">
    <div>
        <ext:ScriptManager ID="ScriptManager1" runat="server" />
        <ext:Store ID="Store1" runat="server" OnRefreshData="Data_RefreshData"
            WarningOnDirty="false">
            <Reader>
                <ext:JsonReader ReaderID="DEPT_CODE">
                    <Fields>
                        <ext:RecordField Name="DEPT_CODE" Type="String" Mapping="DEPT_CODE" />
                        <ext:RecordField Name="DEPT_NAME" Type="String" Mapping="DEPT_NAME" />
                        <ext:RecordField Name="CAUSE_VALUE" Type="Float" Mapping="CAUSE_VALUE" />
                        <ext:RecordField Name="FACT_VALUE" Type="Float" Mapping="FACT_VALUE" />
                        <ext:RecordField Name="COMPARE" Type="Float" Mapping="COMPARE" />
                        <ext:RecordField Name="SUB_VALUE" Type="Float" Mapping="SUB_VALUE" />                       
                    </Fields>
                </ext:JsonReader>
            </Reader>
        </ext:Store>
        <ext:ViewPort ID="ViewPort1" runat="server">
            <Body>
                <ext:ColumnLayout ID="ColumnLayout1" runat="server" Split="true" FitHeight="true">
                    <Columns>
                        <ext:LayoutColumn ColumnWidth="1">
                            <ext:GridPanel ID="GridPanel1" runat="server" StoreID="Store1" StripeRows="true"
                                ClicksToEdit="1" TrackMouseOver="true" AutoWidth="true" Height="480" >
                                <TopBar>
                                    <ext:Toolbar ID="Toolbar_fjsr" runat="server" Visible="true" AutoWidth="true">
                                        <Items>
                                            <ext:ComboBox ID="years" runat="server" Width="60" AllowBlank="true" EmptyText="请选择年..."
                                                FieldLabel="年">
                                            </ext:ComboBox>
                                            <ext:ToolbarTextItem ID="dd1Name" runat="server" Text="年 " />
                                            <ext:ComboBox ID="months" runat="server" Width="60" AllowBlank="true" EmptyText="请选择月..."
                                                FieldLabel="月 到">
                                                <Items>
                                                    <ext:ListItem Text="1" Value="01" />
                                                    <ext:ListItem Text="2" Value="02" />
                                                    <ext:ListItem Text="3" Value="03" />
                                                    <ext:ListItem Text="4" Value="04" />
                                                    <ext:ListItem Text="5" Value="05" />
                                                    <ext:ListItem Text="6" Value="06" />
                                                    <ext:ListItem Text="7" Value="07" />
                                                    <ext:ListItem Text="8" Value="08" />
                                                    <ext:ListItem Text="9" Value="09" />
                                                    <ext:ListItem Text="10" Value="10" />
                                                    <ext:ListItem Text="11" Value="11" />
                                                    <ext:ListItem Text="12" Value="12" />
                                                </Items>
                                            </ext:ComboBox>
                                            <ext:ToolbarTextItem ID="ToolbarTextItem1" runat="server" Text="月 到" />
                                            <ext:ComboBox ID="years1" runat="server" Width="60" AllowBlank="true" EmptyText="请选择年..."
                                                FieldLabel="年">
                                            </ext:ComboBox>
                                            <ext:ToolbarTextItem ID="ToolbarTextItem2" runat="server" Text="年 " />
                                            <ext:ComboBox ID="months1" runat="server" Width="60" AllowBlank="true" EmptyText="请选择月..."
                                                FieldLabel="月">
                                                <Items>
                                                    <ext:ListItem Text="1" Value="01" />
                                                    <ext:ListItem Text="2" Value="02" />
                                                    <ext:ListItem Text="3" Value="03" />
                                                    <ext:ListItem Text="4" Value="04" />
                                                    <ext:ListItem Text="5" Value="05" />
                                                    <ext:ListItem Text="6" Value="06" />
                                                    <ext:ListItem Text="7" Value="07" />
                                                    <ext:ListItem Text="8" Value="08" />
                                                    <ext:ListItem Text="9" Value="09" />
                                                    <ext:ListItem Text="10" Value="10" />
                                                    <ext:ListItem Text="11" Value="11" />
                                                    <ext:ListItem Text="12" Value="12" />
                                                </Items>
                                            </ext:ComboBox>
                                            <ext:ToolbarTextItem ID="ToolbarTextItem3" runat="server" Text="月 " />
                                            <ext:ToolbarSeparator ID="ToolbarSeparator1" runat="server" />
                                            <ext:Label ID="Label3" runat="server" Text="指标：" Width="60" />
                                            <ext:ComboBox ID="GUIDE_ITEM" runat="server"  Width="150" AllowBlank="true" EmptyText="请选择...">
                                                <AjaxEvents>
                                                    <Select OnEvent="Item_SelectOnChange">
                                                        <EventMask Msg='载入中...' ShowMask="true" />
                                                    </Select>
                                                </AjaxEvents>
                                            </ext:ComboBox>
                                          
                                            <ext:Button ID="Button_look" runat="server" Text="查询" Icon="DatabaseGo">                                                  
                                                <Listeners>
                                                    <Click Handler="#{Store1}.reload();" />
                                                </Listeners>                                               
                                            </ext:Button>
                                                                                 
                                            
                                            <ext:Button ID="btnExcel" runat="server"  CausesValidation="false"  Text=" 导出" Icon="PageWhiteExcel"
                                                OnClick="OutExcel" AutoPostBack="true">
                                            </ext:Button>                                         
                                        </Items>
                                    </ext:Toolbar>
                                </TopBar>
                                <ColumnModel ID="ColumnModel1" runat="server" >
                                    <Columns>
                                        <ext:Column ColumnID="DEPT_CODE" Header="科室代码"
                                            Width="1" Align="left" Sortable="false" Hidden="true" DataIndex="DEPT_CODE"
                                            MenuDisabled="true" />
                                        <ext:Column ColumnID="DEPT_NAME" Header="科室"
                                            Width="130" Align="left" Sortable="false" DataIndex="DEPT_NAME" MenuDisabled="true" />
                                        <ext:Column ColumnID="CAUSE_VALUE" Header="指标值"
                                            Width="130" Align="Right" Sortable="false" DataIndex="CAUSE_VALUE" MenuDisabled="true">
                                            <%--<Editor>
                                                <ext:NumberField ID="NumberField1" runat="server" />
                                            </Editor>--%>
                                        </ext:Column>
                                        <ext:Column ColumnID="FACT_VALUE" Header="完成值" Width="130"
                                            Align="Right" Sortable="false" DataIndex="FACT_VALUE" MenuDisabled="true">
                                        </ext:Column>
                                        <ext:Column ColumnID="COMPARE" Header="超标幅度"
                                            Width="130" Align="Right" Sortable="false" DataIndex="COMPARE" MenuDisabled="true">
                                        </ext:Column>
                                        <ext:Column ColumnID="SUB_VALUE" Header="增长量"
                                            Width="130" Align="Right" Sortable="false" DataIndex="SUB_VALUE" MenuDisabled="true">
                                        </ext:Column>
                                    </Columns>
                                </ColumnModel>
                                <SelectionModel>
                                    <ext:RowSelectionModel SingleSelect="true">
                                    </ext:RowSelectionModel>
                                </SelectionModel>
                                <BottomBar>
                                <ext:PagingToolbar ID="PagingToolBar1" runat="server" PageSize="60" StoreID="Store1"
                                        AutoWidth="true" DisplayInfo="true" AutoDataBind="true">
                                    </ext:PagingToolbar>
                                </BottomBar>
                                <Listeners>
                                    <CellClick Fn="rowselect" />                                    
                                </Listeners>
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
