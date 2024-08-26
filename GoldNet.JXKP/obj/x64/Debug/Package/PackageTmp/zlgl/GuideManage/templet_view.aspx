<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="templet_view.aspx.cs" Inherits="GoldNet.JXKP.zlgl.SysManage.templet_view" %>

<%@ Register Assembly="Goldnet.Ext.Web" Namespace="Goldnet.Ext.Web" TagPrefix="ext" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>

    <script type="text/javascript">
        function backToList() {
            window.navigate("templet_view.aspx");
        }
         var RefreshData = function() {
            Store1.reload();
        }   
    </script>

</head>
<body>
    <form id="form1" runat="server">
    <ext:ScriptManager ID="ScriptManager1" runat="server" />
    <ext:Store runat="server" ID="Store1">
        <Reader>
            <ext:JsonReader ReaderID="ID">
                <Fields>
                </Fields>
            </ext:JsonReader>
        </Reader>
    </ext:Store>
    <div>
        <ext:ViewPort ID="ViewPort1" runat="server">
            <Body>
                <ext:BorderLayout ID="BorderLayout2" runat="server">
                    <Center>
                        <ext:Panel runat="server" ID="panel3">
                            <Body>
                                <ext:ColumnLayout ID="ColumnLayout1" runat="server" Split="true" FitHeight="true">
                                    <Columns>
                                        <ext:LayoutColumn ColumnWidth="1">
                                            <ext:GridPanel ID="GridPanel" runat="server" Border="false" StoreID="Store1" StripeRows="true"
                                                Height="480" AutoWidth="true">
                                                <TopBar>
                                                    <ext:Toolbar ID="Toolbar_ZLJK" runat="server" Visible="true" AutoWidth="true">
                                                        <Items>
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
                                                <ColumnModel ID="ColumnModel1" runat="server">
                                                    <Columns>
                                                        <ext:Column Header="编号" Width="66" Align="Left" Sortable="true" MenuDisabled="true"
                                                            ColumnID="ID" DataIndex="ID" Hidden="true">
                                                        </ext:Column>
                                                        <ext:RowNumbererColumn Width="32" Resizable="true">
                                                        </ext:RowNumbererColumn>
                                                    </Columns>
                                                </ColumnModel>
                                                <SelectionModel>
                                                    <ext:RowSelectionModel SingleSelect="true">
                                                    </ext:RowSelectionModel>
                                                </SelectionModel>
                                                <AjaxEvents>
                                                    <DblClick OnEvent="DbRowClick" />
                                                </AjaxEvents>
                                                <LoadMask ShowMask="true" />
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
    <ext:Window ID="ListDetail" runat="server" Icon="Group" Title="详细信息" Width="550"
        Height="460" AutoShow="false" Modal="true" CenterOnLoad="true" AutoScroll="true"
        ShowOnLoad="false" Resizable="true" StyleSpec="background-color:Transparent;"
        BodyStyle="background-color:Transparent;">
    </ext:Window>
    </form>
</body>
</html>
