<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DeptCostDetail.aspx.cs"
    Inherits="GoldNet.JXKP.cbhs.datagather.DeptCostDetail" %>

<%@ Register Assembly="Goldnet.Ext.Web" Namespace="Goldnet.Ext.Web" TagPrefix="ext" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>

    <script language="javascript" type="text/javascript">
        var RefreshData = function(msg) {
            Store2.reload();
        }
    </script>

</head>
<body>
    <form id="form1" runat="server">
    <ext:ScriptManager ID="ScriptManager1" runat="server" />
    <ext:Store ID="Store2" AutoLoad="true" runat="server" OnRefreshData="Store_RefreshData">
        <Reader>
            <ext:JsonReader>
                <Fields>
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
                            <ext:GridPanel ID="GridPanel2" runat="server" Border="false" StoreID="Store2" StripeRows="true"
                                TrackMouseOver="true" Height="480" ClicksToEdit="1">
                                <TopBar>
                                    <ext:Toolbar ID="Toolbar_detptype" runat="server" Visible="true" AutoWidth="true">
                                        <Items>
                                            <ext:Label ID="Label1" runat="server" Text="分摊方案：" Width="60" />
                                            <ext:ComboBox ID="PROG_ITEM" runat="server" Width="100" AllowBlank="true" EmptyText="请选择...">
                                            </ext:ComboBox>
                                            <%--<ext:Button ID="Button1" runat="server" Text="自动分解" Icon="ArrowUndo">
                                                <AjaxEvents>
                                                    <Click OnEvent="btnDecompose_Click" Timeout="200000">
                                                        <ExtraParams>
                                                            <ext:Parameter Name="Values" Value="Ext.encode(#{GridPanel2}.getRowsValues())" Mode="Raw" />
                                                        </ExtraParams>
                                                        <EventMask Msg="成本分解中..." ShowMask="true" />
                                                    </Click>
                                                </AjaxEvents>
                                            </ext:Button>--%>
                                            <%--<ext:ToolbarSeparator ID="ToolbarSeparator1" runat="server" />
                                            <ext:Button ID="Button3" runat="server" Text="手动分解" Icon="ArrowUndo">
                                                <AjaxEvents>
                                                    <Click OnEvent="btnDecomposesd_Click" Timeout="200000">
                                                        <ExtraParams>
                                                            <ext:Parameter Name="Values" Value="Ext.encode(#{GridPanel2}.getRowsValues())" Mode="Raw" />
                                                        </ExtraParams>
                                                        <EventMask Msg="成本分解中..." ShowMask="true" />
                                                    </Click>
                                                </AjaxEvents>
                                            </ext:Button>--%>
                                            <ext:ToolbarSeparator ID="ToolbarSeparator3" runat="server" />
                                            <ext:Button ID="Button2" runat="server" Text="刷新" Icon="ArrowUndo">
                                                <AjaxEvents>
                                                    <Click OnEvent="btnBrush_Click">
                                                        <EventMask Msg="载入中..." ShowMask="true" />
                                                    </Click>
                                                </AjaxEvents>
                                            </ext:Button>
                                            <ext:ToolbarSeparator ID="ToolbarSeparator2" runat="server" />
                                            <ext:Button ID="Button_refresh" runat="server" Text="返回" Icon="ArrowUndo">
                                                <AjaxEvents>
                                                    <Click OnEvent="btnCancle_Click">
                                                        <EventMask Msg="载入中..." ShowMask="true" />
                                                    </Click>
                                                </AjaxEvents>
                                            </ext:Button>
                                            <ext:ToolbarSeparator ID="ToolbarSeparator4" runat="server" />
                                        </Items>
                                    </ext:Toolbar>
                                </TopBar>
                                <ColumnModel ID="ColumnModel2" runat="server">
                                    <Columns>
                                    </Columns>
                                </ColumnModel>
                                <SelectionModel>
                                    <ext:RowSelectionModel ID="rowselection" SingleSelect="true">
                                    </ext:RowSelectionModel>
                                </SelectionModel>
                                <%-- <AjaxEvents>
                                         <DblClick OnEvent="DbRowClick">
                                         <ExtraParams>
                                            <ext:Parameter Name="Values" Value="Ext.encode(#{GridPanel2}.getRowsValues())" Mode="Raw" />
                                        </ExtraParams>
                                        </DblClick>
                                </AjaxEvents>--%>
                                <LoadMask ShowMask="true" />
                            </ext:GridPanel>
                        </ext:LayoutColumn>
                    </Columns>
                </ext:ColumnLayout>
            </Body>
        </ext:ViewPort>
    </div>
    <ext:Window ID="CostDetail" runat="server" Icon="Group" Title="成本详细" Width="700"
        Height="450" AutoShow="false" Modal="true" CenterOnLoad="true" AutoScroll="true"
        ShowOnLoad="false" Resizable="true" StyleSpec="background-color:Transparent;"
        BodyStyle="background-color:Transparent;">
    </ext:Window>
    <ext:Window ID="Decompose" runat="server" Icon="Group" Title="成本分解" Width="700" Height="450"
        AutoShow="false" Modal="true" CenterOnLoad="true" AutoScroll="true" ShowOnLoad="false"
        Resizable="true" StyleSpec="background-color:Transparent;" BodyStyle="background-color:Transparent;">
    </ext:Window>
    <ext:Window ID="deptset" runat="server" Icon="Group" Title="手动分解设置" Width="550" Height="400"
        AutoShow="false" Modal="true" CenterOnLoad="true" AutoScroll="true" ShowOnLoad="false"
        Resizable="true" StyleSpec="background-color:Transparent;" BodyStyle="background-color:Transparent;">
    </ext:Window>
    </form>
</body>
</html>
