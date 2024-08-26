<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="xyhs_diagnosis_icd.aspx.cs"
    Inherits="GoldNet.JXKP.cbhs.xyhs.xyhsdict.xyhs_diagnosis_icd" %>

<%@ Register Assembly="Goldnet.Ext.Web" Namespace="Goldnet.Ext.Web" TagPrefix="ext" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link rel="stylesheet" type="text/css" href="../../../Bonus/Orthers/Cbouns.css" />

    <script type="text/javascript">
        var RefreshData = function(msg) {
//            Ext.Msg.show({ title: '提示', msg: msg, icon: 'ext-mb-info', buttons: { ok: true} });
            Store1.reload();
        }
    </script>

</head>
<body>
    <form id="form1" runat="server">
    <ext:ScriptManager ID="ScriptManager1" runat="server" />
    <ext:Store ID="Store1" runat="server" AutoLoad="true" OnRefreshData="Store_RefreshData">
        <Reader>
            <ext:JsonReader ReaderID="ID">
                <Fields>
                    <ext:RecordField Name="DIAGNOSIS_CODE" />
                    <ext:RecordField Name="DIAGNOSIS_NAME" />
                    <ext:RecordField Name="ICD_CODE" />
                    <ext:RecordField Name="ICD_NAME" />
                    <ext:RecordField Name="ID" />
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
                            TrackMouseOver="true" AutoWidth="true" Height="480" Border="false">
                            <TopBar>
                                <ext:Toolbar ID="Toolbar1" runat="server" Visible="true" AutoWidth="true">
                                    <Items>
                                        <ext:Button ID="Btn_Add" Text="增加" Icon="Add" runat="server">
                                            <AjaxEvents>
                                                <Click OnEvent="Btn_Add_Click">
                                                </Click>
                                            </AjaxEvents>
                                        </ext:Button>
                                        <ext:Button ID="Button1" Text="修改" Icon="NoteEdit" runat="server">
                                            <AjaxEvents>
                                                <Click OnEvent="Button_edit_click">
                                                </Click>
                                            </AjaxEvents>
                                        </ext:Button>
                                        <ext:Button ID="Btn_Del" Text="删除" Icon="Delete" runat="server">
                                            <AjaxEvents>
                                                <Click OnEvent="Btn_Del_Click">
                                                    <Confirmation BeforeConfirm="config.confirmation.message = '你确定要删除吗？';" Title="系统提示"
                                                        ConfirmRequest="true" />
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
                                    <ext:Column ColumnID="DIAGNOSIS_CODE" Header="病种编码" Width="200" Align="left" Sortable="true"
                                        DataIndex="DIAGNOSIS_CODE" MenuDisabled="true" />
                                    <ext:Column ColumnID="DIAGNOSIS_NAME" Header="病种名称" Width="200" Align="left" Sortable="true"
                                        DataIndex="DIAGNOSIS_NAME" MenuDisabled="true" />
                                    <ext:Column ColumnID="ICD_CODE" Header="ICD编码" Width="200" Align="left" Sortable="true"
                                        DataIndex="ICD_CODE" MenuDisabled="true" />
                                    <ext:Column ColumnID="ICD_NAME" Header="ICD名称" Width="200" Align="left" Sortable="true"
                                        DataIndex="ICD_NAME" MenuDisabled="true" />
                                    <ext:Column ColumnID="ID" Header="ID" Width="200" Align="left" Sortable="true" DataIndex="ID"
                                        Hidden="true" MenuDisabled="true" />
                                </Columns>
                            </ColumnModel>
                            <SelectionModel>
                                <ext:RowSelectionModel ID="RowSelectionModel1" runat="server" SingleSelect="true">
                                </ext:RowSelectionModel>
                            </SelectionModel>
                            <BottomBar>
                                <ext:PagingToolbar ID="PagingToolBar2" runat="server" PageSize="25" StoreID="Store1"
                                    AutoWidth="true" DisplayInfo="false" AutoDataBind="true">
                                </ext:PagingToolbar>
                            </BottomBar>
                            <AjaxEvents>
                                <DblClick OnEvent="Button_edit_click" />
                            </AjaxEvents>
                        </ext:GridPanel>
                    </ext:LayoutColumn>
                </Columns>
            </ext:ColumnLayout>
        </Body>
    </ext:ViewPort>
    <ext:Window ID="DetailWin" runat="server" Icon="Group" Title="病种设置" Width="400" Height="400"
        AutoShow="false" Modal="true" CenterOnLoad="true" AutoScroll="true" ShowOnLoad="false"
        StyleSpec="background-color:Transparent;" BodyStyle="background-color:Transparent;">
    </ext:Window>
    </form>
</body>
</html>
