<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CheckPersonsList.aspx.cs"
    Inherits="GoldNet.JXKP.CheckPersonsList" %>

<%@ Register Assembly="Goldnet.Ext.Web" Namespace="Goldnet.Ext.Web" TagPrefix="ext" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link rel="stylesheet" type="text/css" href="../Orthers/Cbouns.css" />
</head>
<body>
    <ext:ScriptManager ID="ScriptManager1" runat="server" />
    <form id="form1" runat="server">
    <ext:Store ID="Store1" AutoLoad="true" runat="server">
        <Reader>
            <ext:JsonReader ReaderID="YEARMONTH">
                <Fields>
                    <ext:RecordField Name="YEARMONTH">
                    </ext:RecordField>
                    <ext:RecordField Name="YEARS">
                    </ext:RecordField>
                    <ext:RecordField Name="MONTHS">
                    </ext:RecordField>
                    <ext:RecordField Name="INPUTDATE">
                    </ext:RecordField>
                    <ext:RecordField Name="INPUTER">
                    </ext:RecordField>
                    <ext:RecordField Name="TOTALPERSONS">
                    </ext:RecordField>
                </Fields>
            </ext:JsonReader>
        </Reader>
    </ext:Store>
    <div>
        <ext:ViewPort ID="ViewPort1" runat="server">
            <Body>
                <ext:ColumnLayout ID="ColumnLayout1" runat="server" Split="true" FitHeight="true">
                    <Columns>
                        <ext:LayoutColumn ColumnWidth="1">
                            <ext:GridPanel ID="GridPanel2" runat="server" Border="false" StoreID="Store1" StripeRows="true"
                                TrackMouseOver="true" Height="480" ClicksToEdit="1">
                                <TopBar>
                                    <ext:Toolbar ID="Toolbar_detptype" runat="server" Visible="true" AutoWidth="true">
                                        <Items>
                                            <ext:Button ID="Btn_Add" Text="增加" Icon="Add" runat="server">
                                                <AjaxEvents>
                                                    <Click OnEvent="btn_Add_Click">
                                                    </Click>
                                                </AjaxEvents>
                                            </ext:Button>
                                            <ext:Button ID="Btn_Edit" Text="编辑" Icon="NoteEdit" runat="server" Disabled="true">
                                                <AjaxEvents>
                                                    <Click OnEvent="btn_Edit_Click">
                                                        <ExtraParams>
                                                            <ext:Parameter Name="Values" Value="Ext.encode(#{GridPanel2}.getRowsValues())" Mode="Raw" />
                                                        </ExtraParams>
                                                    </Click>
                                                </AjaxEvents>
                                            </ext:Button>
                                            <ext:Button ID="Btn_Del" Text="删除" Icon="Delete" runat="server" Disabled="true">
                                                <AjaxEvents>
                                                    <Click OnEvent="btn_Del_Click">
                                                        <Confirmation BeforeConfirm="config.confirmation.message = '你确定要删除吗？';" Title="系统提示"
                                                            ConfirmRequest="true" />
                                                        <ExtraParams>
                                                            <ext:Parameter Name="Values" Value="Ext.encode(#{GridPanel2}.getRowsValues())" Mode="Raw" />
                                                        </ExtraParams>
                                                    </Click>
                                                </AjaxEvents>
                                            </ext:Button>
                                            <%-- <ext:Button ID="Btn_Ref" runat="server" Text="刷新" Icon="ArrowRefresh">
                                            <AjaxEvents>
                                                 <Click OnEvent="btn_Ref_Click">
                                                 </Click>
                                             </AjaxEvents>
                                            </ext:Button>--%>
                                            <ext:Button ID="Btn_look" runat="server" Text="查看" Icon="Zoom" Disabled="true">
                                                <AjaxEvents>
                                                    <Click OnEvent="btn_Look_Click">
                                                        <ExtraParams>
                                                            <ext:Parameter Name="Values" Value="Ext.encode(#{GridPanel2}.getRowsValues())" Mode="Raw" />
                                                        </ExtraParams>
                                                    </Click>
                                                </AjaxEvents>
                                            </ext:Button>
                                        </Items>
                                    </ext:Toolbar>
                                </TopBar>
                                <ColumnModel ID="ColumnModel2" runat="server">
                                    <Columns>
                                        <ext:Column ColumnID="YEARMONTH" Header="设置年月" Width="200" DataIndex="YEARMONTH"
                                            MenuDisabled="true" Align="Center">
                                        </ext:Column>
                                        <ext:Column ColumnID="TOTALPERSONS" Header="总人数" Width="200" DataIndex="TOTALPERSONS"
                                            MenuDisabled="true" Align="Center">
                                        </ext:Column>
                                        <ext:Column ColumnID="INPUTDATE" Header="输入时间" Width="200" DataIndex="INPUTDATE"
                                            MenuDisabled="true" Align="Center">
                                        </ext:Column>
                                        <ext:Column ColumnID="INPUTER" Header="录入人" Width="200" DataIndex="INPUTER" MenuDisabled="true"
                                            Align="Center">
                                        </ext:Column>
                                    </Columns>
                                </ColumnModel>
                                <SelectionModel>
                                    <ext:RowSelectionModel ID="rowselection" SingleSelect="true">
                                        <Listeners>
                                            <RowSelect Handler="#{Btn_Edit}.enable();#{Btn_Del}.enable();#{Btn_look}.enable()" />
                                            <RowDeselect Handler="if (!#{GridPanel2}.hasSelection()) {#{Btn_Del}.disable();#{Btn_Edit}.disable();#{Btn_look}.disable()}" />
                                        </Listeners>
                                    </ext:RowSelectionModel>
                                </SelectionModel>
                                <AjaxEvents>
                                    <DblClick OnEvent="btn_Look_Click">
                                        <ExtraParams>
                                            <ext:Parameter Name="Values" Value="Ext.encode(#{GridPanel2}.getRowsValues())" Mode="Raw" />
                                        </ExtraParams>
                                    </DblClick>
                                </AjaxEvents>
                                <LoadMask ShowMask="true" />
                            </ext:GridPanel>
                        </ext:LayoutColumn>
                    </Columns>
                </ext:ColumnLayout>
            </Body>
        </ext:ViewPort>
    </div>
    </form>
</body>
</html>
