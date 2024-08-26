<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PageList.aspx.cs" Inherits="GoldNet.JXKP.zlgl.Templet.Page.PageList" %>

<%@ Register Assembly="Goldnet.Ext.Web" Namespace="Goldnet.Ext.Web" TagPrefix="ext" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script type="text/javascript">
        function backToList() {
            window.navigate("PageList.aspx");
        }
         var RefreshData = function() {
            Store1.reload();
        }   
        
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <ext:ScriptManager ID="ScriptManager1" runat="server" AjaxMethodNamespace="Goldnet"/>
    <ext:Store runat="server" ID="Store1" AutoLoad="true" OnRefreshData="Store_RefreshData">
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
                                            <ext:Button ID="Buttonlist" runat="server" Text="刷新" Icon="ArrowRefresh">
                                                <AjaxEvents>
                                                    <Click OnEvent="GetQueryPortalet">
                                                        <EventMask Msg="载入中..." ShowMask="true" />
                                                    </Click>
                                                </AjaxEvents>
                                            </ext:Button>
                                            <ext:Button ID="Buttonserchset" runat="server" Text="查询设置" Icon="DatabaseKey">
                                                <AjaxEvents>
                                                    <Click OnEvent="SerchSet">
                                                        <EventMask Msg="载入中..." ShowMask="true" />
                                                    </Click>
                                                </AjaxEvents>
                                            </ext:Button>
                                            <ext:ToolbarSeparator ID="ToolbarSeparator1" runat="server" />
                                            <ext:Label ID="lable1" runat="server" Text="是否跟踪"></ext:Label>
                                            <ext:CheckBox ID="checks" runat="server" ></ext:CheckBox>
                                            <ext:ToolbarSeparator ID="ToolbarSeparator2" runat="server" />
                                            <ext:Label ID="Label2" runat="server" Text="是否解决"></ext:Label>
                        <ext:CheckBox ID="flags" runat="server"  ></ext:CheckBox>
                                            <ext:ToolbarSeparator ID="ToolbarSeparator4" runat="server" />
                                             <ext:Button ID="Btn_Export" runat="server" Text="导出Excel" Icon="PageWhiteExcel"  OnClick="OutExcel" AutoPostBack="true">
                        </ext:Button>
                                        </Items>
                                    </ext:Toolbar>
                                </TopBar>
                                
                                                <ColumnModel ID="ColumnModel1" runat="server">
                                                    <Columns>
                                                        <ext:Column Header="编号" Width="66" Align="Left" Sortable="true" MenuDisabled="true"
                                                            ColumnID="ID" DataIndex="ID" Hidden="true" >
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
     <ext:Window ID="ListDetail" runat="server" Icon="Group" Title="详细信息" Width="600"
            Height="500"  AutoShow="false" Modal="true" CenterOnLoad="true" AutoScroll="true" ShowOnLoad="false" Resizable="false" StyleSpec="background-color:Transparent;" BodyStyle="background-color:Transparent;">
     </ext:Window>
     <ext:Window ID="searchset" runat="server" Icon="Group" Title="查询设置" Width="600"
            Height="500"  AutoShow="false" Modal="true" CenterOnLoad="true" AutoScroll="true" ShowOnLoad="false" Resizable="false" StyleSpec="background-color:Transparent;" BodyStyle="background-color:Transparent;">
     </ext:Window>
    </form>
</body>
</html>
