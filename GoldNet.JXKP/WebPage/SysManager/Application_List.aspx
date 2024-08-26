<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Application_List.aspx.cs"
    Inherits="GoldNet.JXKP.WebPage.SysManager.Application_List" %>
<%@ Register Assembly="Goldnet.Ext.Web" Namespace="Goldnet.Ext.Web" TagPrefix="ext" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script type="text/javascript">
       
         var RefreshData = function() {
            Store1.reload();
        }   
    </script>

</head>
<body>
    <form id="form1" runat="server">
    <div>
        <ext:ScriptManager ID="ScriptManager1" runat="server" AjaxMethodNamespace="CompanyX" />
        <ext:Store ID="Store1" runat="server"  AutoLoad="true" OnRefreshData="Store_RefreshData">
            <Reader>
                <ext:JsonReader ReaderID="APP_ID">
                    <Fields>
                        <ext:RecordField Name="APP_ID" />
                        <ext:RecordField Name="APP_NAME" />
                        <ext:RecordField Name="POWER_TYPE" />
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
                                TrackMouseOver="true" Height="480" AutoWidth="true" Border="false" AutoExpandColumn="APP_NAME" AutoExpandMax="300">
                                <TopBar>
                                    <ext:Toolbar ID="Toolbar_ZLJK" runat="server" Visible="true" AutoWidth="true">
                                        <Items>
                                            <ext:ToolbarSpacer ID="ToolbarSpacer5" runat="server" Width="6" />
                                            <ext:ToolbarSeparator ID="ToolbarSeparator3" runat="server" />
                                            <ext:Button ID="Buttonset" runat="server" Text="设置" Icon="DatabaseKey">
                                                <AjaxEvents>
                                                    <Click OnEvent="Buttonset_Click">
                                                        <EventMask Msg="载入中..." ShowMask="true" />
                                                    </Click>
                                                </AjaxEvents>
                                            </ext:Button>
                                            <ext:Button ID="Buttonlist" runat="server" Text="刷新" Icon="ArrowRefresh">
                                                <AjaxEvents>
                                                    <Click OnEvent="GetQueryFunc">
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
                                            ColumnID="APP_ID" DataIndex="APP_ID">
                                        </ext:Column>
                                        <ext:Column Header="项目名称" Width="120" Align="left" Sortable="true" MenuDisabled="true"
                                            ColumnID="APP_NAME" DataIndex="APP_NAME">
                                        </ext:Column>
                                        <ext:Column Header="是否单独授权" Width="100" Align="left" Sortable="true" MenuDisabled="true"
                                            ColumnID="POWER_TYPE" DataIndex="POWER_TYPE">
                                        </ext:Column>
                                    </Columns>
                                </ColumnModel>
                                <SelectionModel>
                                    <ext:RowSelectionModel ID="rowselection" SingleSelect="true">
                                        <AjaxEvents>
                                            <RowSelect OnEvent="RowSelect" Buffer="250">
                                                <EventMask ShowMask="true" Target="CustomTarget" CustomTarget="#{Store1}" />
                                                <ExtraParams>
                                                    <ext:Parameter Name="APP_ID" Value="this.getSelected().id" Mode="Raw" />
                                                </ExtraParams>
                                            </RowSelect>
                                        </AjaxEvents>
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
        </ext:ViewPort>
    </div>
    <ext:Window ID="Func_Set" runat="server" Icon="Group" Title="科室设置" Width="300"
            Height="200"  AutoShow="false" Modal="true" CenterOnLoad="true" AutoScroll="true" ShowOnLoad="false" Resizable="true" StyleSpec="background-color:Transparent;" BodyStyle="background-color:Transparent;">
           
    </ext:Window>
    </form>
</body>
</html>
