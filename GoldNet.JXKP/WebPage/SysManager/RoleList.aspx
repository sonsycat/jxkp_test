<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="RoleList.aspx.cs" Inherits="GoldNet.JXKP.WebPage.SysManager.RoleList" %>

<%@ Register Assembly="Goldnet.Ext.Web" Namespace="Goldnet.Ext.Web" TagPrefix="ext" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>

     <script type="text/javascript">
        function backToList() {
            window.navigate("RoleList.aspx");
        }
         var RefreshData = function() {
            Store1.reload();
        }   
    </script>

</head>
<body>
    <form id="form1" runat="server">
    <div>
    <ext:ScriptManager ID="ScriptManager2" runat="server" AjaxMethodNamespace="Goldnet" />
        
        <ext:Store ID="Store1" runat="server" AutoLoad="true" OnRefreshData="Store_RefreshData">
            <Reader>
                <ext:JsonReader ReaderID="ROLE_ID">
                    <Fields>
                        <ext:RecordField Name="ROLE_ID" />
                        <ext:RecordField Name="ROLE_NAME" />
                        <ext:RecordField Name="REMARK" />
                        <ext:RecordField Name="ROLE_TYPE" />
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
                                TrackMouseOver="true" Height="480" AutoWidth="true">
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
                                            <ext:Button ID="Buttonadd" runat="server" Text="添加" Icon="DatabaseAdd">
                                                <AjaxEvents>
                                                    <Click OnEvent="Buttonadd_Click">
                                                        <EventMask Msg="载入中..." ShowMask="true" />
                                                    </Click>
                                                </AjaxEvents>
                                            </ext:Button>
                                            <ext:Button ID="Buttondel" runat="server" Text="删除" Icon="DatabaseDelete">
                                                <AjaxEvents>
                                                    <Click OnEvent="Buttondel_Click">
                                                        <EventMask Msg="载入中..." ShowMask="true" />
                                                    </Click>
                                                </AjaxEvents>
                                            </ext:Button>
                                            <ext:Button ID="Buttonedit" runat="server" Text="修改" Icon="DatabaseEdit">
                                                <AjaxEvents>
                                                    <Click OnEvent="Buttonedit_Click">
                                                        <EventMask Msg="载入中..." ShowMask="true" />
                                                    </Click>
                                                </AjaxEvents>
                                            </ext:Button>
                                            <ext:Button ID="Buttonlist" runat="server" Text="刷新" Icon="ArrowRefresh">
                                                <AjaxEvents>
                                                    <Click OnEvent="GetQueryPortalet">
                                                        <EventMask Msg="载入中..." ShowMask="true" />
                                                    </Click>
                                                </AjaxEvents>
                                            </ext:Button>
                                             <ext:Button ID="Button1" runat="server" Text="导出" Icon="ArrowRefresh" OnClick="GetQueryPortalet1" AutoPostBack="true" Visible="true">
                                             
                                            </ext:Button>
                                            <ext:ToolbarSeparator ID="ToolbarSeparator4" runat="server" />
                                        </Items>
                                    </ext:Toolbar>
                                </TopBar>
                                <ColumnModel ID="ColumnModel1" runat="server">
                                    <Columns>
                                        <ext:Column Header="序号" Width="66" Align="Left" Sortable="true" MenuDisabled="true"
                                            ColumnID="ROLE_ID" DataIndex="ROLE_ID">
                                        </ext:Column>
                                        <ext:Column Header="角色名称" Width="200" Align="left" Sortable="true" MenuDisabled="true"
                                            ColumnID="ROLE_NAME" DataIndex="ROLE_NAME">
                                        </ext:Column>
                                        <ext:Column Header="角色说明" Width="230" Align="left" Sortable="true" MenuDisabled="true"
                                            ColumnID="REMARK" DataIndex="REMARK">
                                        </ext:Column>
                                        <ext:Column Header="角色类别" Width="66" Align="Left" Sortable="true" MenuDisabled="true"
                                            ColumnID="ROLE_TYPE" DataIndex="ROLE_TYPE">
                                        </ext:Column>
                                    </Columns>
                                </ColumnModel>
                                <SelectionModel>
                                    <ext:RowSelectionModel ID="rowselection" SingleSelect="true">
                                        <AjaxEvents>
                                            <RowSelect OnEvent="RowSelect" Buffer="250">
                                                <EventMask ShowMask="true" Target="CustomTarget" CustomTarget="#{Store1}" />
                                                <ExtraParams>
                                                    <ext:Parameter Name="ROLE_ID" Value="this.getSelected().id" Mode="Raw" />
                                                </ExtraParams>
                                            </RowSelect>
                                        </AjaxEvents>
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
    <ext:Window ID="RoleEdit" runat="server" Icon="Group" Title="编辑角色" Width="400" Height="200"
        AutoShow="false" Modal="true" CenterOnLoad="true" AutoScroll="true" ShowOnLoad="false"
        Resizable="false" StyleSpec="background-color:Transparent;" BodyStyle="background-color:Transparent;">
    </ext:Window>
    <ext:Window ID="Rle_Set" runat="server" Icon="Group" Title="角色设置" Width="700" Height="520"
        AutoShow="false" Modal="true" CenterOnLoad="true" AutoScroll="true" ShowOnLoad="false"
        Resizable="false" StyleSpec="background-color:Transparent;" BodyStyle="background-color:Transparent;">
    </ext:Window>
    </form>
</body>
</html>
