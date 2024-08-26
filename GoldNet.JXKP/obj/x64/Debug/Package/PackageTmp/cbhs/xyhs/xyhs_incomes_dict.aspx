<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="xyhs_incomes_dict.aspx.cs" Inherits="GoldNet.JXKP.cbhs.xyhs.xyhs_incomes_dict" %>
<%@ Register Assembly="Goldnet.Ext.Web" Namespace="Goldnet.Ext.Web" TagPrefix="ext" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
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
    <ext:ScriptManager ID="ScriptManager1" runat="server"/>
    <ext:Store ID="Store1" runat="server" AutoLoad="true"  OnRefreshData="Store_RefreshData"  >
        <Reader>
            <ext:JsonReader ReaderID="ITEM_CODE">
                <Fields>
                    <%--<ext:RecordField Name="CLASS_CODE"  />
                    <ext:RecordField Name="CLASS_NAME"   />--%>
                    <ext:RecordField Name="ITEM_CODE"  />
                    <ext:RecordField Name="ITEM_NAME"   />
                    <ext:RecordField Name="FLAGS"  />
                    <ext:RecordField Name="RATIO" />
                   
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
                               
                                    <ext:Column ColumnID="ITEM_NAME" Header="项目名称" Width="100" Align="left" Sortable="true"
                                        DataIndex="ITEM_NAME" MenuDisabled="true" />
                                    <ext:Column ColumnID="ITEM_CODE" Header="项目代码" Width="80" Align="left" Sortable="true"
                                        DataIndex="ITEM_CODE" MenuDisabled="true" />
                                    <ext:Column ColumnID="FLAGS" Header="是否核算" Width="80" Align="left" Sortable="true"
                                        DataIndex="FLAGS" MenuDisabled="true" />
                                    <ext:Column ColumnID="RATIO" Header="核算系数" Width="80" Align="right"
                                        Sortable="true" DataIndex="RATIO" MenuDisabled="true" />
                        
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
                            
                            <View>
                                    <ext:GroupingView  
                                        ID="GroupingView1"
                                        HideGroupedColumn="true"
                                        runat="server" 
                                        GroupTextTpl='{text} ({[values.rs.length]})'
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
    <ext:Window ID="DetailWin" runat="server" Icon="Group" Title="全成本收入项目设置" Width="400"
        Height="200" AutoShow="false" Modal="true" CenterOnLoad="true" AutoScroll="false"
        ShowOnLoad="false" Resizable="false" StyleSpec="background-color:Transparent;"
        BodyStyle="background-color:Transparent;">
    </ext:Window>
     
    </form>
</body>
</html>
