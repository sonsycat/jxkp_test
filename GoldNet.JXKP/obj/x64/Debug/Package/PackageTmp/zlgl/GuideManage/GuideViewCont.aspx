<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="GuideViewCont.aspx.cs"
    Inherits="GoldNet.JXKP.zlgl.SysManage.GuideViewCont" %>

<%@ Register Assembly="Goldnet.Ext.Web" Namespace="Goldnet.Ext.Web" TagPrefix="ext" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>

    <script type="text/javascript">
        function backToList() {
            window.navigate("GuideViewCont.aspx");
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
                        <ext:RecordField Name="DEPTCODE" />
                        <ext:RecordField Name="CHECKCONT" />
                        <ext:RecordField Name="CHECKSTAN" />
                        <ext:RecordField Name="CHECKMETH" />
                    </Fields>
                </ext:JsonReader>
            </Reader>
        </ext:Store>
        <ext:Panel ID="Panel1" runat="server" Border="false" AutoHeight="true" AutoWidth="true"
            StyleSpec="background-color:transparent" BodyStyle="background-color:transparent">
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
                        <ext:Button ID="Contentype" runat="server" Text="考评内容分类" Icon="NoteEdit" Visible="false">
                            <AjaxEvents>
                                <Click OnEvent="Buttoneidttype_Click">
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
        </ext:Panel>
        <ext:Panel runat="server"><Body><table width="100%"><tr align="center"><td align="center" width="100%"><ext:Label runat="server" ID="guidename"></ext:Label></td></tr></table></Body></ext:Panel>
        <ext:ViewPort ID="ViewPort1" runat="server" AutoHeight="true">
            <Body>
                <ext:ColumnLayout ID="ColumnLayout1" runat="server" Split="true" >
                    <Columns>
                        <ext:LayoutColumn ColumnWidth="1">
                            <ext:GridPanel ID="GridPanel1" runat="server" StoreID="Store1" StripeRows="true"
                                TrackMouseOver="true" Height="450" AutoWidth="true" AutoScroll="true">
                                <ColumnModel ID="ColumnModel1" runat="server">
                                    <Columns>
                                        <ext:Column Header="序号" Width="66" Align="Left" Sortable="true" MenuDisabled="true"
                                            ColumnID="ID" DataIndex="ID" Hidden="true">
                                        </ext:Column>
                                        <ext:Column Header="考核科室" Width="120" Align="left" Sortable="true" MenuDisabled="true"
                                            ColumnID="DEPTCODE" DataIndex="DEPTCODE" Hidden="true">
                                        </ext:Column>
                                        <ext:Column Header="考核内容" Width="200" Align="left" Sortable="true" MenuDisabled="true"
                                            ColumnID="CHECKCONT" DataIndex="CHECKCONT">
                                        </ext:Column>
                                        <ext:Column Header="考评标准" Width="500" Align="left" Sortable="true" MenuDisabled="true"
                                            ColumnID="CHECKSTAN" DataIndex="CHECKSTAN">
                                        </ext:Column>
                                        <ext:Column Header="考评办法" Width="200" Align="left" Sortable="true" MenuDisabled="true"
                                            ColumnID="CHECKMETH" DataIndex="CHECKMETH">
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
        </ext:ViewPort>
    </div>
    <ext:Window ID="guidecontedit" runat="server" Icon="Group" Title="编辑指标内容" Width="500"
        Height="520" AutoShow="false" Modal="true" CenterOnLoad="true" AutoScroll="true"
        ShowOnLoad="false" Resizable="false" StyleSpec="background-color:Transparent;"
        BodyStyle="background-color:Transparent;">
    </ext:Window>
    <ext:Window ID="guideconttype" runat="server" Icon="Group" Title="专业指标分类" Width="400"
        Height="300" AutoShow="false" Modal="true" CenterOnLoad="true" AutoScroll="true"
        ShowOnLoad="false" Resizable="false" StyleSpec="background-color:Transparent;"
        BodyStyle="background-color:Transparent;">
    </ext:Window>
    </form>
</body>
</html>
