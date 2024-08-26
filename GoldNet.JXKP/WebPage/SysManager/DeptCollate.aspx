<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DeptCollate.aspx.cs" Inherits="GoldNet.JXKP.WebPage.SysManager.DeptCollate" %>

<%@ Register Assembly="Goldnet.Ext.Web" Namespace="Goldnet.Ext.Web" TagPrefix="ext" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link rel="stylesheet" type="text/css" href="../../Bonus/Orthers/Cbouns.css" />

    <script type="text/javascript">
        var RefreshData = function() {
            Store1.reload();
        }
    </script>

</head>
<body>
    <form id="form1" runat="server">
    <div>
        <ext:ScriptManager ID="ScriptManager1" runat="server" />
        <ext:Store runat="server" ID="Store1" AutoLoad="true" OnRefreshData="Store_RefreshData">
            <Reader>
                <ext:JsonReader ReaderID="DEPT_CODE">
                    <Fields>
                        <ext:RecordField Name="ID" Type="String" Mapping="ID" />
                        <ext:RecordField Name="DEPT_CODE" Type="String" Mapping="DEPT_CODE" />
                        <ext:RecordField Name="DEPT_NAME" Type="String" Mapping="DEPT_NAME" />
                        <ext:RecordField Name="P_DEPT_CODE" Type="String" Mapping="P_DEPT_CODE" />
                        <ext:RecordField Name="DEPT_TYPE" Type="String" Mapping="DEPT_TYPE" />
                        <ext:RecordField Name="DEPT_LCATTR" Type="String" Mapping="DEPT_LCATTR" />
                        <ext:RecordField Name="DEPT_DATE" Type="String" Mapping="DEPT_DATE" />
                        <ext:RecordField Name="SORT_NO" Type="String" Mapping="SORT_NO" />
                        <ext:RecordField Name="SHOW_FLAG" Type="String" Mapping="SHOW_FLAG" />
                        <ext:RecordField Name="ATTR" Type="String" Mapping="ATTR" />
                        <ext:RecordField Name="ACCOUNT_DEPT_CODE" Type="String" Mapping="ACCOUNT_DEPT_CODE" />
                        <ext:RecordField Name="ACCOUNT_DEPT_NAME" Type="String" Mapping="ACCOUNT_DEPT_NAME" />
                        <ext:RecordField Name="DEPT_NAME_SECOND" Type="String" Mapping="DEPT_NAME_SECOND" />
                        <ext:RecordField Name="DEPT_CODE_SECOND" Type="String" Mapping="DEPT_CODE_SECOND" />
                        <ext:RecordField Name="INPUT_CODE" Type="String" Mapping="INPUT_CODE" />
                        <ext:RecordField Name="P_DEPT_NAME" Type="String" Mapping="P_DEPT_NAME" />
                    </Fields>
                </ext:JsonReader>
            </Reader>
        </ext:Store>
        <ext:Store ID="Store3" runat="server">
            <Reader>
                <ext:JsonReader ReaderID="YEAR">
                    <Fields>
                        <ext:RecordField Name="YEAR" />
                    </Fields>
                </ext:JsonReader>
            </Reader>
        </ext:Store>
        <ext:Store ID="Store4" runat="server">
            <Reader>
                <ext:JsonReader ReaderID="MONTH">
                    <Fields>
                        <ext:RecordField Name="MONTH" />
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
                                TrackMouseOver="true" Height="480" AutoWidth="true" AutoExpandColumn="INPUT_CODE">
                                <TopBar>
                                    <ext:Toolbar ID="Toolbar_detptype" runat="server" Visible="true" AutoWidth="true">
                                        <Items>
                                            <ext:Label runat="server" ID="label2" Text="快照年月">
                                            </ext:Label>
                                            <ext:ComboBox ID="cbbYear" runat="server" ReadOnly="true" StoreID="Store3" Width="70"
                                                DisplayField="YEAR" ValueField="YEAR">
                                            </ext:ComboBox>
                                            <ext:Label ID="lYear" runat="server" Text="年">
                                            </ext:Label>
                                            <ext:ComboBox ID="cbbmonth" runat="server" ReadOnly="true" StoreID="Store4" Width="70"
                                                DisplayField="MONTH" ValueField="MONTH">
                                            </ext:ComboBox>
                                            <ext:Label ID="lmonth" runat="server" Text="月">
                                            </ext:Label>
                                            <ext:ToolbarSeparator ID="ToolbarSeparator1" runat="server" />
                                            <ext:Button ID="Btn_Query" Text="查询" Icon="Zoom" runat="server">
                                                <AjaxEvents>
                                                    <Click OnEvent="Btn_Query_Click">
                                                    </Click>
                                                </AjaxEvents>
                                            </ext:Button>
                                            <ext:Button ID="Btn_Save" Text="保存" Icon="Disk" runat="server">
                                                <AjaxEvents>
                                                    <Click OnEvent="Btn_Save_Click">
                                                        <EventMask Msg="正在保存" ShowMask="true" />
                                                        <ExtraParams>
                                                            <ext:Parameter Name="Values" Value="Ext.encode(#{GridPanel1}.getRowsValues(false))"
                                                                Mode="Raw" />
                                                        </ExtraParams>
                                                    </Click>
                                                </AjaxEvents>
                                            </ext:Button>
                                            <ext:Button ID="Btn_Collate" Text="重新快照科室" Icon="Cd" runat="server">
                                                <AjaxEvents>
                                                    <Click OnEvent="Btn_Collate_Click">
                                                    </Click>
                                                </AjaxEvents>
                                            </ext:Button>
                                            <ext:ToolbarSeparator ID="ToolbarSeparator2" runat="server" />
                                            <ext:Button ID="Btn_Add" Text="增加" Icon="Add" runat="server">
                                                <AjaxEvents>
                                                    <Click OnEvent="Btn_Add_Click">
                                                    </Click>
                                                </AjaxEvents>
                                            </ext:Button>
                                            <ext:Button ID="Btn_Edit" Text="编辑" Icon="NoteEdit" runat="server" Disabled="true">
                                                <AjaxEvents>
                                                    <Click OnEvent="Btn_Edit_Click">
                                                        <ExtraParams>
                                                            <ext:Parameter Name="Values" Value="Ext.encode(#{GridPanel1}.getRowsValues())" Mode="Raw">
                                                            </ext:Parameter>
                                                        </ExtraParams>
                                                    </Click>
                                                </AjaxEvents>
                                            </ext:Button>
                                            <ext:Button ID="Btn_Del" Text="删除" Icon="Delete" runat="server" Disabled="true">
                                                <AjaxEvents>
                                                    <Click OnEvent="Btn_Del_Click">
                                                        <Confirmation BeforeConfirm="config.confirmation.message = '你确定要删除吗？';" Title="系统提示"
                                                            ConfirmRequest="true" />
                                                        <EventMask Msg="正在删除" ShowMask="true" />
                                                        <ExtraParams>
                                                            <ext:Parameter Name="Values" Value="Ext.encode(#{GridPanel1}.getRowsValues())" Mode="Raw">
                                                            </ext:Parameter>
                                                        </ExtraParams>
                                                    </Click>
                                                </AjaxEvents>
                                            </ext:Button>
                                        </Items>
                                    </ext:Toolbar>
                                </TopBar>
                                <ColumnModel ID="ColumnModel1" runat="server">
                                    <Columns>
                                        <ext:Column Header="科室编码" Width="66" Align="Left" Sortable="true" MenuDisabled="true"
                                            ColumnID="DEPT_CODE" DataIndex="DEPT_CODE">
                                        </ext:Column>
                                        <ext:Column Header="科室名称" Width="120" Align="left" Sortable="true" MenuDisabled="true"
                                            ColumnID="DEPT_NAME" DataIndex="DEPT_NAME">
                                        </ext:Column>
                                        <ext:Column Header="上级科室" Width="120" Align="left" Sortable="true" MenuDisabled="true"
                                            ColumnID="P_DEPT_NAME" DataIndex="P_DEPT_NAME">
                                        </ext:Column>
                                        <ext:Column Header="核算科室名称" Width="120" Align="left" Sortable="true" MenuDisabled="true"
                                            ColumnID="ACCOUNT_DEPT_NAME" DataIndex="ACCOUNT_DEPT_NAME">
                                        </ext:Column>
                                        <ext:Column Header="二级科室名称" Width="120" Align="left" Sortable="true" MenuDisabled="true"
                                            ColumnID="DEPT_NAME_SECOND" DataIndex="DEPT_NAME_SECOND">
                                        </ext:Column>
                                        <ext:Column Header="临床属性" Width="120" Align="left" Sortable="true" MenuDisabled="true"
                                            ColumnID="DEPT_LCATTR" DataIndex="DEPT_LCATTR" Hidden="true">
                                        </ext:Column>
                                        <ext:Column Header="是否核算科室" Width="120" Align="left" Sortable="true" MenuDisabled="true"
                                            ColumnID="ATTR" DataIndex="ATTR">
                                        </ext:Column>
                                        <ext:Column Header="排列顺序" Width="120" Align="left" Sortable="true" MenuDisabled="true"
                                            ColumnID="SORT_NO" DataIndex="SORT_NO">
                                        </ext:Column>
                                        <ext:Column Header="输入码" Width="120" Align="left" Sortable="true" MenuDisabled="true"
                                            ColumnID="INPUT_CODE" DataIndex="INPUT_CODE">
                                        </ext:Column>
                                    </Columns>
                                </ColumnModel>
                                <SelectionModel>
                                    <ext:RowSelectionModel ID="rowselection" SingleSelect="true">
                                        <Listeners>
                                            <RowSelect Handler="#{Btn_Edit}.enable();#{Btn_Del}.enable();" />
                                            <RowDeselect Handler="if (!#{GridPanel1}.hasSelection()) {#{Btn_Del}.disable();#{Btn_Edit}.disable();}" />
                                        </Listeners>
                                    </ext:RowSelectionModel>
                                </SelectionModel>
                                <LoadMask ShowMask="true" />
                            </ext:GridPanel>
                        </ext:LayoutColumn>
                    </Columns>
                </ext:ColumnLayout>
            </Body>
        </ext:ViewPort>
        <ext:Window ID="DetailWin" runat="server" Icon="Group" Title="科室快照设置" Width="400"
            Height="400" AutoShow="false" Modal="true" CenterOnLoad="true" AutoScroll="true"
            ShowOnLoad="false" Resizable="false" StyleSpec="background-color:Transparent;"
            BodyStyle="background-color:Transparent;">
        </ext:Window>
    </div>
    </form>
</body>
</html>
