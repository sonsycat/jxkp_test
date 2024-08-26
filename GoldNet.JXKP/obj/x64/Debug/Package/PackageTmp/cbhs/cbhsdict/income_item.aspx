<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="income_item.aspx.cs" Inherits="GoldNet.JXKP.income_item" %>

<%@ Register Assembly="Goldnet.Ext.Web" Namespace="Goldnet.Ext.Web" TagPrefix="ext" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style type="text/css">
        body
        {
            background-color: #DFE8F6;
            font-size: 12px;
        }
    </style>
    <link rel="stylesheet" type="text/css" href="../../Bonus/Orthers/Cbouns.css" />

    <script language="javascript" type="text/javascript">
        var RefreshData = function(msg) {
            Ext.Msg.show({ title: '提示', msg: msg, icon: 'ext-mb-info', buttons: { ok: true} });
            Store1.reload();
        }
            
        function dbonclick(item_class)
		{
		   document.location.href="dept_income_item.aspx?item_class="+item_class;  
		}
    </script>

</head>
<body>
    <form id="form1" runat="server">
    <ext:ScriptManager ID="ScriptManager1" runat="server" />
    <ext:Store ID="Store1" runat="server" AutoLoad="true" GroupField="CLASS_CODE" OnRefreshData="Store_RefreshData">
        <Reader>
            <ext:JsonReader ReaderID="ITEM_CLASS">
                <Fields>
                    <ext:RecordField Name="CLASS_CODE" />
                    <ext:RecordField Name="ITEM_CLASS" />
                    <ext:RecordField Name="ITEM_NAME" />
                    <ext:RecordField Name="INPUT_CODE" />
                    <ext:RecordField Name="ORDER_DEPT_DISTRIBUT" />
                    <ext:RecordField Name="PERFORM_DEPT_DISTRIBUT" />
                    <ext:RecordField Name="NURSING_PERCEN" />
                    <ext:RecordField Name="OUT_OPDEPT_PERCEN" />
                    <ext:RecordField Name="OUT_EXDEPT_PERCEN" />
                    <ext:RecordField Name="OUT_NURSING_PERCEN" />
                    <ext:RecordField Name="COOPERANT_PERCEN" />
                    <ext:RecordField Name="CALCULATION_TYPE" />
                    <ext:RecordField Name="FIXED_PERCEN" />
                    <ext:RecordField Name="COST_CODE" />
                    <ext:RecordField Name="PROFIT_RATE" />
                    <ext:RecordField Name="PERFRO_DEPT" />
                    <ext:RecordField Name="ZJCBBL" />
                    <ext:RecordField Name="JJCBBL" />
                    <ext:RecordField Name="DCCB" />
                    <ext:RecordField Name="CLASS_NAME" />
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
                            TrackMouseOver="true" AutoWidth="true" Height="480" Border="false">
                            <TopBar>
                                <ext:Toolbar ID="Toolbar1" runat="server" Visible="true" AutoWidth="true">
                                    <Items>
                                        <ext:Button ID="Button_set" runat="server" Text="设置" Icon="DatabaseGo" Disabled="true">
                                            <AjaxEvents>
                                                <Click OnEvent="Button_set_click">
                                                    <EventMask Msg="载入中..." ShowMask="true" />
                                                </Click>
                                            </AjaxEvents>
                                        </ext:Button>
                                        <ext:Button ID="Button_dept" runat="server" Text="科室明细" Icon="Cog" Visible="false">
                                            <AjaxEvents>
                                                <Click OnEvent="Button_dept_click">
                                                    <EventMask Msg="载入中..." ShowMask="true" />
                                                </Click>
                                            </AjaxEvents>
                                        </ext:Button>
                                        <ext:Button ID="Button_refresh" runat="server" Text="刷新" Icon="ArrowRefresh">
                                            <AjaxEvents>
                                                <Click OnEvent="Button_refresh_click">
                                                    <EventMask Msg="载入中..." ShowMask="true" />
                                                </Click>
                                            </AjaxEvents>
                                        </ext:Button>
                                    </Items>
                                </ext:Toolbar>
                            </TopBar>
                            <ColumnModel ID="ColumnModel1" runat="server">
                                <Columns>
                                    <ext:Column ColumnID="CLASS_CODE" Header="收入类别" Width="100" Align="left" Sortable="true"
                                        DataIndex="CLASS_CODE" MenuDisabled="true" />
                                    <ext:Column ColumnID="ITEM_NAME" Header="项目名称" Width="100" Align="left" Sortable="true"
                                        DataIndex="ITEM_NAME" MenuDisabled="true" />
                                    <ext:Column ColumnID="ITEM_CLASS" Header="项目代码" Width="80" Align="left" Sortable="true"
                                        DataIndex="ITEM_CLASS" MenuDisabled="true" />
                                    <ext:Column ColumnID="INPUT_CODE" Header="输入码" Width="80" Align="left" Sortable="true"
                                        DataIndex="INPUT_CODE" MenuDisabled="true" />
                                    <ext:Column ColumnID="ORDER_DEPT_DISTRIBUT" Header="住院开单" Width="80" Align="right"
                                        Sortable="true" DataIndex="ORDER_DEPT_DISTRIBUT" MenuDisabled="true" />
                                    <ext:Column ColumnID="PERFORM_DEPT_DISTRIBUT" Header="住院执行" Width="80" Align="right"
                                        Sortable="true" DataIndex="PERFORM_DEPT_DISTRIBUT" MenuDisabled="true" />
                                    <ext:Column ColumnID="NURSING_PERCEN" Header="住院护理" Width="80" Align="right" Sortable="true"
                                        DataIndex="NURSING_PERCEN" MenuDisabled="true" />
                                    <ext:Column ColumnID="OUT_OPDEPT_PERCEN" Header="门诊开单" Width="80" Align="right" Sortable="true"
                                        DataIndex="OUT_OPDEPT_PERCEN" MenuDisabled="true" />
                                    <ext:Column ColumnID="OUT_EXDEPT_PERCEN" Header="门诊执行" Width="80" Align="right" Sortable="true"
                                        DataIndex="OUT_EXDEPT_PERCEN" MenuDisabled="true" />
                                    <ext:Column ColumnID="OUT_NURSING_PERCEN" Header="门诊护理" Width="80" Align="right"
                                        Sortable="true" DataIndex="OUT_NURSING_PERCEN" MenuDisabled="true" />
                                    <ext:Column ColumnID="COOPERANT_PERCEN" Header="合作医疗" Width="80" Align="right" Sortable="true"
                                        DataIndex="COOPERANT_PERCEN" MenuDisabled="true" />
                                    <ext:Column ColumnID="PROFIT_RATE" Header="利润率" Width="60" Align="right" Sortable="true"
                                        DataIndex="PROFIT_RATE" MenuDisabled="true" Hidden="true" />
                                    <ext:Column ColumnID="CALCULATION_TYPE" Header="核算类型" Width="80" Align="left" Sortable="true"
                                        DataIndex="CALCULATION_TYPE" MenuDisabled="true" />
                                    <ext:Column ColumnID="FIXED_PERCEN" Header="折算比" Width="60" Align="right" Sortable="true"
                                        DataIndex="FIXED_PERCEN" MenuDisabled="true" />
                                    <ext:Column ColumnID="COST_CODE" Header="成本对照" Width="100" Align="left" Sortable="true"
                                        DataIndex="COST_CODE" MenuDisabled="true" />
                                    <ext:Column ColumnID="PROFIT_RATE" Header="利润率" Width="100" Align="left" Sortable="true"
                                        DataIndex="PROFIT_RATE" MenuDisabled="true" Hidden="true" />
                                    <ext:Column ColumnID="PERFRO_DEPT" Header="分配科室" Width="100" Align="left" Sortable="true"
                                        DataIndex="PERFRO_DEPT" MenuDisabled="true" />
                                    <ext:Column ColumnID="ZJCBBL" Header="直接成本比例" Width="100" Align="left" Sortable="true"
                                        DataIndex="ZJCBBL" MenuDisabled="true" Hidden="true" />
                                    <ext:Column ColumnID="JJCBBL" Header="间接成本比例" Width="100" Align="left" Sortable="true"
                                        DataIndex="JJCBBL" MenuDisabled="true" Hidden="true" />
                                    <ext:Column ColumnID="DCCB" Header="单次成本" Width="100" Align="left" Sortable="true"
                                        DataIndex="DCCB" MenuDisabled="true" Hidden="true" />
                                    <ext:Column ColumnID="CLASS_NAME" Header="收入类别" Width="100" Sortable="true" DataIndex="CLASS_NAME"
                                        MenuDisabled="true" />
                                    <ext:CommandColumn Width="80" Align="Center" Header="<div style='text-align:center;'>第三方科室</div>">
                                        <Commands>
                                            <ext:GridCommand Icon="Outline" CommandName="OtherDept" ToolTip-Text="设置第三方科室">
                                            </ext:GridCommand>
                                        </Commands>
                                    </ext:CommandColumn>
                                </Columns>
                            </ColumnModel>
                            <AjaxEvents>
                                <RowDblClick OnEvent="Button_set_click" />
                            </AjaxEvents>
                            <SelectionModel>
                                <ext:RowSelectionModel ID="RowSelectionModel1" runat="server" SingleSelect="true">
                                    <Listeners>
                                        <RowSelect Handler="#{Button_set}.enable()" />
                                        <RowDeselect Handler="if (!#{GridPanel1}.hasSelection()) {#{Button_set}.disable()}" />
                                    </Listeners>
                                </ext:RowSelectionModel>
                            </SelectionModel>
                            <Listeners>
                                <Command Handler="if(command=='OtherDept'){ Goldnet.AjaxMethods.SetDept(record.data.ITEM_CLASS)}" />
                            </Listeners>
                            <View>
                                <ext:GroupingView ID="GroupingView1" HideGroupedColumn="true" runat="server" GroupTextTpl='{text} ({[values.rs.length]})'
                                    EnableRowBody="false">
                                </ext:GroupingView>
                            </View>
                            <BottomBar>
                                <ext:PagingToolbar ID="PagingToolBar2" runat="server" PageSize="20" StoreID="Store1"
                                    AutoWidth="true" DisplayInfo="false" AutoDataBind="true">
                                </ext:PagingToolbar>
                            </BottomBar>
                        </ext:GridPanel>
                    </ext:LayoutColumn>
                </Columns>
            </ext:ColumnLayout>
        </Body>
    </ext:ViewPort>
    <ext:Window ID="DetailWin" runat="server" Icon="Group" Title="收入项目比例设置" Width="590"
        Height="400" AutoShow="false" Modal="true" CenterOnLoad="true" AutoScroll="false"
        ShowOnLoad="false" Resizable="false" StyleSpec="background-color:Transparent;"
        BodyStyle="background-color:Transparent;">
    </ext:Window>
    <ext:Window ID="DeptWin" runat="server" Icon="Group" Title="设置第三方科室" Width="550"
        Height="400" AutoShow="false" Modal="true" CenterOnLoad="true" AutoScroll="true"
        ShowOnLoad="false" Resizable="true" StyleSpec="background-color:Transparent;"
        BodyStyle="background-color:Transparent;">
    </ext:Window>
    </form>
</body>
</html>
