<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DeptGuideSet.aspx.cs" Inherits="GoldNet.JXKP.jxkh.DeptGuideSet" %>
<%@ Register Assembly="Goldnet.Ext.Web" Namespace="Goldnet.Ext.Web" TagPrefix="ext" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
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
                <ext:JsonReader>
                    <Fields>
                        <ext:RecordField Name="DEPT_CODE" />
                        <ext:RecordField Name="DEPT_NAME" />
                        <ext:RecordField Name="GUIDE_CODE" />
                        <ext:RecordField Name="GUIDE_NAME" />
                        <ext:RecordField Name="VS_GUIDE_CODE" />
                        <ext:RecordField Name="VS_GUIDE_NAME" />
                        <ext:RecordField Name="GUIDE_CAUSE" />
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
                                     
                                            <ext:Button ID="Buttonadd" runat="server" Text="添加" Icon="DatabaseAdd">
                                                <AjaxEvents>
                                                    <Click OnEvent="Buttonadd_Click">
                                                        <EventMask Msg="载入中..." ShowMask="true" />
                                                         <ExtraParams>
                                                            <ext:Parameter Name="Values" Value="Ext.encode(#{GridPanel1}.getRowsValues())" Mode="Raw">
                                                            </ext:Parameter>
                                                        </ExtraParams>
                                                    </Click>
                                                </AjaxEvents>
                                            </ext:Button>
                                            <ext:Button ID="Buttondel" runat="server" Text="删除" Icon="DatabaseDelete">
                                                <AjaxEvents>
                                                    <Click OnEvent="Buttondel_Click">
                                                    <Confirmation ConfirmRequest="true" Title="系统提示" Message="将删除选中数据,<br/>是否继续?" />
                                                        <EventMask Msg="载入中..." ShowMask="true" />
                                                         <ExtraParams>
                                                            <ext:Parameter Name="Values" Value="Ext.encode(#{GridPanel1}.getRowsValues())" Mode="Raw">
                                                            </ext:Parameter>
                                                        </ExtraParams>
                                                    </Click>
                                                </AjaxEvents>
                                            </ext:Button>
                                            <ext:Button ID="Buttonedit" runat="server" Text="修改" Icon="DatabaseEdit">
                                                <AjaxEvents>
                                                    <Click OnEvent="Buttonedit_Click">
                                                        <EventMask Msg="载入中..." ShowMask="true" />
                                                         <ExtraParams>
                                                            <ext:Parameter Name="Values" Value="Ext.encode(#{GridPanel1}.getRowsValues())" Mode="Raw">
                                                            </ext:Parameter>
                                                        </ExtraParams>
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
                                         
                                            <ext:ToolbarSeparator ID="ToolbarSeparator4" runat="server" />
                                        </Items>
                                    </ext:Toolbar>
                                </TopBar>
                                <ColumnModel ID="ColumnModel1" runat="server">
                                    <Columns>
                                        <ext:Column Header="科室代码" Width="66" Align="Left" Sortable="true" MenuDisabled="true"
                                            ColumnID="DEPT_CODE" DataIndex="DEPT_CODE">
                                        </ext:Column>
                                        <ext:Column Header="科室名称" Width="120" Align="left" Sortable="true" MenuDisabled="true"
                                            ColumnID="DEPT_NAME" DataIndex="DEPT_NAME">
                                        </ext:Column>
                                        <ext:Column Header="指标代码" Width="66" Align="left" Sortable="true" MenuDisabled="true"
                                            ColumnID="GUIDE_CODE" DataIndex="GUIDE_CODE" Hidden="true">
                                        </ext:Column>
                                        <ext:Column Header="指标名称" Width="100" Align="Left" Sortable="true" MenuDisabled="true"
                                            ColumnID="GUIDE_NAME" DataIndex="GUIDE_NAME">
                                        </ext:Column>
                                        <ext:Column Header="关联指标代码" Width="100" Align="left" Sortable="true" MenuDisabled="true"
                                            ColumnID="VS_GUIDE_CODE" DataIndex="VS_GUIDE_CODE" Hidden="true">
                                        </ext:Column>
                                        <ext:Column Header="关联指标名称" Width="100" Align="Left" Sortable="true" MenuDisabled="true"
                                            ColumnID="VS_GUIDE_NAME" DataIndex="VS_GUIDE_NAME">
                                        </ext:Column>
                                        <ext:Column Header="达标值" Width="60" Align="Right" Sortable="true" MenuDisabled="true"
                                            ColumnID="GUIDE_CAUSE" DataIndex="GUIDE_CAUSE">
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
    <ext:Window ID="RoleEdit" runat="server" Icon="Group" Title="编辑" Width="400" Height="200"
        AutoShow="false" Modal="true" CenterOnLoad="true" AutoScroll="true" ShowOnLoad="false"
        Resizable="false" StyleSpec="background-color:Transparent;" BodyStyle="background-color:Transparent;">
    </ext:Window>
   
    </form>
</body>
</html>
