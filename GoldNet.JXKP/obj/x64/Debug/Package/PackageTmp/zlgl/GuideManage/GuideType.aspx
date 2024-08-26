<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="GuideType.aspx.cs" Inherits="GoldNet.JXKP.zlgl.SysManage.GuideType" %>

<%@ Register Assembly="Goldnet.Ext.Web" Namespace="Goldnet.Ext.Web" TagPrefix="ext" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>

    <script type="text/javascript">
        function backToList() {
            window.navigate("GuideType.aspx");
        }
         var RefreshData = function() {
            Store1.reload();
        }
        function edit(id,straction) {
            window.navigate("GuideTypeEdit.aspx?id="+id+"&straction="+straction);
        }
    </script>

</head>
<body>
    <form id="form1" runat="server">
    <div>
        <ext:ScriptManager ID="ScriptManager1" runat="server" AjaxMethodNamespace="Goldnet" />
        <ext:Store ID="Store1" runat="server" AutoLoad="true" OnRefreshData="Store_RefreshData">
            <Reader>
                <ext:JsonReader ReaderID="ID">
                    <Fields>
                        <ext:RecordField Name="ID" />
                        <ext:RecordField Name="GUIDETYPE" />
                        <ext:RecordField Name="TYPESIGN" />
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
                                    <ext:Toolbar ID="Toolbar_guidetype" runat="server" Visible="true" AutoWidth="true">
                                        <Items>
                                            <ext:ToolbarSpacer ID="ToolbarSpacer5" runat="server" Width="6" />
                                            <ext:ToolbarSeparator ID="ToolbarSeparator3" runat="server" />
                                            <ext:Button ID="Buttonadd" runat="server" Text="添加" Icon="Add">
                                                <AjaxEvents>
                                                    <Click OnEvent="Buttonadd_Click">
                                                        <EventMask Msg="载入中..." ShowMask="true" />
                                                    </Click>
                                                </AjaxEvents>
                                            </ext:Button>
                                            <ext:Button ID="Buttondel" runat="server" Text="删除" Icon="Delete">
                                                <AjaxEvents>
                                                    <Click OnEvent="Buttondel_Click">
                                                        <EventMask Msg="载入中..." ShowMask="true" />
                                                    </Click>
                                                </AjaxEvents>
                                            </ext:Button>
                                            <ext:Button ID="Buttonedit" runat="server" Text="修改" Icon="NoteEdit">
                                                <AjaxEvents>
                                                    <Click OnEvent="Buttonedit_Click">
                                                        <EventMask Msg="载入中..." ShowMask="true" />
                                                    </Click>
                                                </AjaxEvents>
                                            </ext:Button>
                                            <ext:Button ID="btnCancle" runat="server" Text="返回" Icon="ArrowUndo">
                                                <AjaxEvents>
                                                    <Click OnEvent="btnCancle_Click">
                                                    </Click>
                                                </AjaxEvents>
                                            </ext:Button>
                                            <ext:ToolbarSeparator ID="ToolbarSeparator4" runat="server" />
                                        </Items>
                                    </ext:Toolbar>
                                </TopBar>
                                <ColumnModel ID="ColumnModel1" runat="server">
                                    <Columns>
                                        <ext:Column Header="序号" Width="66" Align="Left" Sortable="true" MenuDisabled="true"
                                            ColumnID="ID" DataIndex="ID">
                                        </ext:Column>
                                        <ext:Column Header="指标类别" Width="120" Align="left" Sortable="true" MenuDisabled="true"
                                            ColumnID="GUIDETYPE" DataIndex="GUIDETYPE">
                                        </ext:Column>
                                        <ext:CommandColumn Width="30" Align="Center">
                                            <Commands>
                                                <ext:GridCommand Icon="Zoom" CommandName="DetailView" Hidden="true">
                                                    <ToolTip Text="修改" />
                                                </ext:GridCommand>
                                            </Commands>
                                        </ext:CommandColumn>
                                    </Columns>
                                </ColumnModel>
                                <SelectionModel>
                                    <ext:RowSelectionModel ID="rowselection" SingleSelect="true">
                                    </ext:RowSelectionModel>
                                </SelectionModel>
                                <Listeners>
                                    <Command Handler="edit(record.data.ID, 'edit');" />
                                </Listeners>
                                <LoadMask ShowMask="true" />
                            </ext:GridPanel>
                        </ext:LayoutColumn>
                    </Columns>
                </ext:ColumnLayout>
            </Body>
        </ext:ViewPort>
    </div>
    <ext:Window ID="guidetypeedit" runat="server" Icon="Group" Title="编辑指标类别" Width="350"
        Height="200" AutoShow="false" Modal="true" CenterOnLoad="true" AutoScroll="true"
        ShowOnLoad="false" Resizable="true" StyleSpec="background-color:Transparent;"
        BodyStyle="background-color:Transparent;">
    </ext:Window>
    </form>
</body>
</html>
