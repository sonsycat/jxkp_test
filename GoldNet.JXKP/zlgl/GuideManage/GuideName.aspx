<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="GuideName.aspx.cs" Inherits="GoldNet.JXKP.zlgl.SysManage.GuideName" %>
<%@ Register Assembly="Goldnet.Ext.Web" Namespace="Goldnet.Ext.Web" TagPrefix="ext" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
     <script type="text/javascript">
        function backToList() {
            window.navigate("GuideName.aspx");
        }
         var RefreshData = function() {
            Store1.reload();
            Store2.reload();
        } 
          
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    <ext:ScriptManager ID="ScriptManager1" runat="server" AjaxMethodNamespace="Goldnet">
    </ext:ScriptManager>
    <ext:Store ID="Store1" runat="server" AutoLoad="true" OnRefreshData="Store_RefreshData">
        <Reader>
            <ext:JsonReader ReaderID="ID">
                <Fields>
                    <ext:RecordField Name="ID" />
                    <ext:RecordField Name="GUIDETYPE" />
                    <ext:RecordField Name="COMMGUIDENAME" />
                    <ext:RecordField Name="GUIDENUM" />
                    <ext:RecordField Name="MANADEPT" />
                    <ext:RecordField Name="TYPESIGN" />
                    <ext:RecordField Name="COMMGUIDETYPEID" />
                </Fields>
            </ext:JsonReader>
        </Reader>
    </ext:Store>
    <ext:Store ID="Store2" runat="server" AutoLoad="true" OnRefreshData="Store_RefreshData">
        <Reader>
            <ext:JsonReader ReaderID="ID">
                <Fields>
                    <ext:RecordField Name="ID" />
                    <ext:RecordField Name="GUIDETYPE" />
                    <ext:RecordField Name="SPECGUIDENAME" />
                    <ext:RecordField Name="GUIDENUM" />
                    <ext:RecordField Name="MANADEPT" />
                    <ext:RecordField Name="CHECKDEPT" />
                    <ext:RecordField Name="TYPESIGN" />
                    <ext:RecordField Name="SPECGUIDETYPEID" />
                </Fields>
            </ext:JsonReader>
        </Reader>
    </ext:Store>
    <ext:TabPanel ID="TabPanel1" runat="server" ActiveTabIndex="0" Border="false">
        <Tabs>
            <ext:Tab ID="Tab1" runat="server" Title="公共指标设置" AutoHeight="true" BodyStyle="padding: 1px;"
                Border="false">
                <Body>
                    <ext:ColumnLayout ID="ColumnLayout1" runat="server" Split="true" FitHeight="true">
                        <Columns>
                            <ext:LayoutColumn ColumnWidth="1">
                                <ext:GridPanel ID="GridPanel1" runat="server" StoreID="Store1" StripeRows="true"
                                    TrackMouseOver="true" AutoHeight="true" AutoWidth="true">
                                    <TopBar>
                                        <ext:Toolbar ID="Toolbar_ZLJK" runat="server" Visible="true" AutoWidth="true">
                                            <Items>
                                                <ext:ToolbarSpacer ID="ToolbarSpacer5" runat="server" Width="6" />
                                                <ext:ToolbarSeparator ID="ToolbarSeparator3" runat="server" />
                                                <ext:Button ID="Buttonadd" runat="server" Text="添加" Icon="Add">
                                                    <AjaxEvents>
                                                        <Click OnEvent="BtnAddComm_Click">
                                                            <EventMask Msg="载入中..." ShowMask="true" />
                                                        </Click>
                                                    </AjaxEvents>
                                                </ext:Button>
                                                <ext:Button ID="Buttondel" runat="server" Text="删除" Icon="Delete" >
                                                    <AjaxEvents>
                                                        <Click OnEvent="BtnDelComm_Click">
                                                            <EventMask Msg="载入中..." ShowMask="true" />
                                                        </Click>
                                                    </AjaxEvents>
                                                </ext:Button>
                                                <ext:Button ID="Buttonedit" runat="server" Text="修改" Icon="NoteEdit" >
                                                    <AjaxEvents>
                                                        <Click OnEvent="BtnEditComm_Click">
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
                                            <ext:Column Header="指标类别" Width="160" Align="Left" Sortable="true" MenuDisabled="true"
                                                ColumnID="GUIDETYPE" DataIndex="GUIDETYPE">
                                            </ext:Column>
                                            <ext:Column Header="指标名称" Width="240" Align="left" Sortable="true" MenuDisabled="true"
                                                ColumnID="COMMGUIDENAME" DataIndex="COMMGUIDENAME">
                                            </ext:Column>
                                            <ext:Column Header="指标分值" Width="300" Align="left" Sortable="true" MenuDisabled="true"
                                                ColumnID="GUIDENUM" DataIndex="GUIDENUM">
                                            </ext:Column>
                                            <ext:Column Header="主管部门" Width="300" Align="left" Sortable="true" MenuDisabled="true"
                                                ColumnID="MANADEPT" DataIndex="MANADEPT">
                                            </ext:Column>
                                        </Columns>
                                    </ColumnModel>
                                    <SelectionModel>
                                        <ext:RowSelectionModel ID="rowselection" SingleSelect="true">
                                            
                                        </ext:RowSelectionModel>
                                    </SelectionModel>
                                    <LoadMask ShowMask="true" />
                                </ext:GridPanel>
                            </ext:LayoutColumn>
                        </Columns>
                    </ext:ColumnLayout>
                </Body>
            </ext:Tab>
            <ext:Tab ID="Tab2" runat="server" Title="专业指标设置" AutoHeight="true" BodyStyle="padding:1px;"
                Border="false">
                <Body>
                    <ext:ColumnLayout ID="ColumnLayout2" runat="server" Split="true" FitHeight="true">
                        <Columns>
                            <ext:LayoutColumn ColumnWidth="1">
                                <ext:GridPanel ID="GridPanel2" runat="server" StoreID="Store2" StripeRows="true"
                                    TrackMouseOver="true" AutoHeight="true" AutoWidth="true">
                                    <TopBar>
                                        <ext:Toolbar ID="Toolbar1" runat="server" Visible="true" AutoWidth="true">
                                            <Items>
                                                <ext:ToolbarSpacer ID="ToolbarSpacer1" runat="server" Width="6" />
                                                <ext:ToolbarSeparator ID="ToolbarSeparator1" runat="server" />
                                                <ext:Button ID="BtnAddSpec" runat="server" Text="添加" Icon="Add">
                                                    <AjaxEvents>
                                                        <Click OnEvent="BtnAddSpec_Click">
                                                            <EventMask Msg="载入中..." ShowMask="true" />
                                                        </Click>
                                                    </AjaxEvents>
                                                </ext:Button>
                                                <ext:Button ID="BtnDelSpec" runat="server" Text="删除" Icon="Delete" >
                                                    <AjaxEvents>
                                                        <Click OnEvent="BtnDelSpec_Click">
                                                            <EventMask Msg="载入中..." ShowMask="true" />
                                                        </Click>
                                                    </AjaxEvents>
                                                </ext:Button>
                                                <ext:Button ID="BtnEditSpec" runat="server" Text="修改" Icon="NoteEdit" >
                                                    <AjaxEvents>
                                                        <Click OnEvent="BtnEditSpec_Click">
                                                            <EventMask Msg="载入中..." ShowMask="true" />
                                                        </Click>
                                                    </AjaxEvents>
                                                </ext:Button>
                                                <ext:Button ID="Button1" runat="server" Text="返回" Icon="ArrowUndo">
                                                    <AjaxEvents>
                                                        <Click OnEvent="btnCancle_Click">
                                                        </Click>
                                                    </AjaxEvents>
                                                </ext:Button>
                                                <ext:ToolbarSeparator ID="ToolbarSeparator2" runat="server" />
                                            </Items>
                                        </ext:Toolbar>
                                    </TopBar>
                                    <ColumnModel ID="ColumnModel2" runat="server">
                                        <Columns>
                                            <ext:Column Header="指标类别" Width="160" Align="Left" Sortable="true" MenuDisabled="true"
                                                ColumnID="GUIDETYPE" DataIndex="GUIDETYPE">
                                            </ext:Column>
                                            <ext:Column Header="指标名称" Width="240" Align="left" Sortable="true" MenuDisabled="true"
                                                ColumnID="SPECGUIDENAME" DataIndex="SPECGUIDENAME">
                                            </ext:Column>
                                            <ext:Column Header="指标分值" Width="300" Align="left" Sortable="true" MenuDisabled="true"
                                                ColumnID="GUIDENUM" DataIndex="GUIDENUM">
                                            </ext:Column>
                                            <ext:Column Header="主管部门" Width="300" Align="left" Sortable="true" MenuDisabled="true"
                                                ColumnID="MANADEPT" DataIndex="MANADEPT">
                                            </ext:Column>
                                        </Columns>
                                    </ColumnModel>
                                    <SelectionModel>
                                        <ext:RowSelectionModel ID="RowSelectionModel1" SingleSelect="true">
                                          
                                        </ext:RowSelectionModel>
                                    </SelectionModel>
                                    <LoadMask ShowMask="true" />
                                </ext:GridPanel>
                            </ext:LayoutColumn>
                        </Columns>
                    </ext:ColumnLayout>
                </Body>
            </ext:Tab>
        </Tabs>
    </ext:TabPanel>
    <ext:Window ID="guideinfo" runat="server" Icon="Group" Title="指标信息" Width="600"
        Height="550" AutoShow="false" Modal="true" CenterOnLoad="true" AutoScroll="true"
        ShowOnLoad="false" Resizable="false" StyleSpec="background-color:Transparent;"
        BodyStyle="background-color:Transparent;">
    </ext:Window>
    </div>
    </form>
</body>
</html>
