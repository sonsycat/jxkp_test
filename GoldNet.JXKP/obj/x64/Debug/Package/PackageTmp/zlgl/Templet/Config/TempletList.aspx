<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TempletList.aspx.cs" Inherits="GoldNet.JXKP.zlgl.Templet.Config.TempletList" %>

<%@ Register Assembly="Goldnet.Ext.Web" Namespace="Goldnet.Ext.Web" TagPrefix="ext" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>

    <script type="text/javascript">
        function backToList() {
            window.navigate("TempletList.aspx");
        }
         var RefreshData = function() {
            Store1.reload();
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
                        <ext:RecordField Name="NAME" />
                        <ext:RecordField Name="TITLE" />
                        <ext:RecordField Name="CREATEDATE" />
                        <ext:RecordField Name="TABNAME" />
                        <ext:RecordField Name="SHOWORDER" />
                        <ext:RecordField Name="COMMON" />
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
                                            <ext:Button ID="Buttonadd" runat="server" Text="添加模版" Icon="Add">
                                                <AjaxEvents>
                                                    <Click OnEvent="Buttonadd_Click">
                                                        <EventMask Msg="载入中..." ShowMask="true" />
                                                    </Click>
                                                </AjaxEvents>
                                            </ext:Button>
                                            <ext:Button ID="Buttondel" runat="server" Text="删除模版" Icon="Delete"  >
                                                <AjaxEvents>
                                                    <Click OnEvent="Buttondel_Click">
                                                    
                                                        <EventMask Msg="载入中..." ShowMask="true" />
                                                    </Click>
                                                </AjaxEvents>
                                            </ext:Button>
                                            <ext:Button ID="Buttonedit" runat="server" Text="修改模版" Icon="NoteEdit" >
                                                <AjaxEvents>
                                                    <Click OnEvent="Buttonedit_Click">
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
                                        <ext:Column Header="模版名称" Width="160" Align="Left" Sortable="true" MenuDisabled="true"
                                            ColumnID="NAME" DataIndex="NAME">
                                        </ext:Column>
                                        <ext:Column Header="模版标题" Width="240" Align="left" Sortable="true" MenuDisabled="true"
                                            ColumnID="TITLE" DataIndex="TITLE">
                                        </ext:Column>
                                        <ext:Column Header="表名" Width="300" Align="left" Sortable="true" MenuDisabled="true"
                                            ColumnID="TABNAME" DataIndex="TABNAME">
                                        </ext:Column>
                                        <ext:Column Header="说明" Width="300" Align="left" Sortable="true" MenuDisabled="true"
                                            ColumnID="COMMON" DataIndex="COMMON">
                                        </ext:Column>
                                        <ext:Column Header="显示顺序" Width="100" Align="right" Sortable="true" MenuDisabled="true"
                                            ColumnID="SHOWORDER" DataIndex="SHOWORDER">
                                        </ext:Column>
                                    </Columns>
                                </ColumnModel>
                                <SelectionModel>
                                    <ext:RowSelectionModel ID="rowselection" SingleSelect="true">
                                       
                                    </ext:RowSelectionModel>
                                </SelectionModel>
                                <LoadMask ShowMask="true" />
                                <BottomBar>
                                 <ext:PagingToolbar ID="PagingToolBar1" runat="server" PageSize="25" StoreID="Store1"
                                                        AutoWidth="true" DisplayInfo="false" AutoDataBind="true"/>
                                </BottomBar>
                            </ext:GridPanel>
                        </ext:LayoutColumn>
                    </Columns>
                </ext:ColumnLayout>
            </Body>
        </ext:ViewPort>
    </div>
    <ext:Window ID="templetinfo" runat="server" Icon="Group" Title="模版信息" Width="600"
        Height="500" AutoShow="false" Modal="true" CenterOnLoad="true" AutoScroll="true"
        ShowOnLoad="false" Resizable="false" StyleSpec="background-color:Transparent;"
        BodyStyle="background-color:Transparent;">
    </ext:Window>
    <ext:Window ID="templetname" runat="server" Icon="Group" Title="模版信息" Width="500"
        Height="300" AutoShow="false" Modal="true" CenterOnLoad="true" AutoScroll="true"
        ShowOnLoad="false" Resizable="false" StyleSpec="background-color:Transparent;"
        BodyStyle="background-color:Transparent;">
    </ext:Window>
    </form>
</body>
</html>
