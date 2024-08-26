<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SimpleEncourageList.aspx.cs"
    Inherits="GoldNet.JXKP.SimpleEncourageList" %>

<%@ Register Assembly="Goldnet.Ext.Web" Namespace="Goldnet.Ext.Web" TagPrefix="ext" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link rel="stylesheet" type="text/css" href="../Orthers/Cbouns.css" />

    <script type="text/javascript">
        var RefreshData = function() {
            Store1.reload();
        }
    </script>

</head>
<body>
    <ext:ScriptManager ID="ScriptManager1" runat="server" />
    <ext:Store ID="Store1" AutoLoad="true" runat="server" OnRefreshData="Store_RefreshData">
        <Reader>
            <ext:JsonReader ReaderID="ID">
                <Fields>
                    <ext:RecordField Name="ID">
                    </ext:RecordField>
                    <ext:RecordField Name="ITEMNAME">
                    </ext:RecordField>
                    <ext:RecordField Name="CHECKSTAN">
                    </ext:RecordField>
                    <ext:RecordField Name="REMARK">
                    </ext:RecordField>
                </Fields>
            </ext:JsonReader>
        </Reader>
    </ext:Store>
    <form id="form1" runat="server">
    <div>
        <ext:ViewPort ID="ViewPort1" runat="server">
            <Body>
                <ext:ColumnLayout ID="ColumnLayout1" runat="server" Split="true" FitHeight="true">
                    <Columns>
                        <ext:LayoutColumn ColumnWidth="1">
                            <ext:GridPanel ID="GridPanel2" runat="server" Border="false" StoreID="Store1" StripeRows="true"
                                TrackMouseOver="true" Height="480">
                                <TopBar>
                                    <ext:Toolbar ID="Toolbar_simpleencourage" runat="server" Visible="true" AutoWidth="true">
                                        <Items>
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
                                                            <ext:Parameter Name="Values" Value="Ext.encode(#{GridPanel2}.getRowsValues())" Mode="Raw">
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
                                                        <ExtraParams>
                                                            <ext:Parameter Name="Values" Value="Ext.encode(#{GridPanel2}.getRowsValues())" Mode="Raw">
                                                            </ext:Parameter>
                                                        </ExtraParams>
                                                    </Click>
                                                </AjaxEvents>
                                            </ext:Button>
                                            <%-- <ext:Button runat="server" Text="刷新" Icon="ArrowRefresh">
                                                <Listeners>
                                                    <Click Fn="RefreshData" />
                                                </Listeners>
                                            </ext:Button>--%>
                                        </Items>
                                    </ext:Toolbar>
                                </TopBar>
                                <ColumnModel ID="ColumnModel2" runat="server">
                                    <Columns>
                                        <ext:Column ColumnID="itemname" Header="序号" Width="50" DataIndex="ID" MenuDisabled="true">
                                        </ext:Column>
                                        <ext:Column ColumnID="itemname" Header="奖惩名称" Width="100" DataIndex="ITEMNAME" MenuDisabled="true">
                                        </ext:Column>
                                        <ext:Column ColumnID="checkstan" Header="奖惩标准" Width="400" DataIndex="CHECKSTAN"
                                            MenuDisabled="true">
                                        </ext:Column>
                                        <ext:Column ColumnID="remark" Header="备注信息" Width="400" DataIndex="REMARK" MenuDisabled="true">
                                        </ext:Column>
                                    </Columns>
                                </ColumnModel>
                                <SelectionModel>
                                    <ext:RowSelectionModel ID="rowselection" SingleSelect="true">
                                        <Listeners>
                                            <RowSelect Handler="#{Btn_Edit}.enable();#{Btn_Del}.enable()" />
                                            <RowDeselect Handler="if (!#{GridPanel2}.hasSelection()) {#{Btn_Del}.disable();#{Btn_Edit}.disable()}" />
                                        </Listeners>
                                    </ext:RowSelectionModel>
                                </SelectionModel>
                                <LoadMask ShowMask="true" />
                                <AjaxEvents>
                                    <DblClick OnEvent="Btn_Edit_Click">
                                        <ExtraParams>
                                            <ext:Parameter Name="Values" Value="Ext.encode(#{GridPanel2}.getRowsValues())" Mode="Raw">
                                            </ext:Parameter>
                                        </ExtraParams>
                                    </DblClick>
                                </AjaxEvents>
                            </ext:GridPanel>
                        </ext:LayoutColumn>
                    </Columns>
                </ext:ColumnLayout>
            </Body>
        </ext:ViewPort>
        <ext:Window ID="DetailWin" runat="server" Icon="Group" Title="单项奖惩类别设置" Width="400"
            Height="400" AutoShow="false" Modal="true" CenterOnLoad="true" AutoScroll="true"
            ShowOnLoad="false" Resizable="false" StyleSpec="background-color:Transparent;"
            BodyStyle="background-color:Transparent;">
        </ext:Window>
    </div>
    </form>
</body>
</html>
