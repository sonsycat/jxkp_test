<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="BonusSubmitEdit.aspx.cs"
    Inherits="GoldNet.JXKP.BonusSubmitEdit" %>

<%@ Register Assembly="Goldnet.Ext.Web" Namespace="Goldnet.Ext.Web" TagPrefix="ext" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link rel="stylesheet" type="text/css" href="../Orthers/Cbouns.css" />
    <script type="text/javascript">
        var RefreshData = function () {
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
                    <ext:RecordField Name="ST_DATE">
                    </ext:RecordField>
                    <ext:RecordField Name="INDEX_ID">
                    </ext:RecordField>
                    <ext:RecordField Name="DEPT_CODE">
                    </ext:RecordField>
                    <ext:RecordField Name="DEPT_NAME">
                    </ext:RecordField>
                    <ext:RecordField Name="FLAGS">
                    </ext:RecordField>
                    <ext:RecordField Name="ISCOMMIT">
                    </ext:RecordField>
                </Fields>
            </ext:JsonReader>
        </Reader>
    </ext:Store>
    <ext:Store ID="SYear" runat="server">
        <Reader>
            <ext:JsonReader ReaderID="YEAR">
                <Fields>
                    <ext:RecordField Name="YEAR" />
                </Fields>
            </ext:JsonReader>
        </Reader>
    </ext:Store>
    <ext:Store ID="SMonths" runat="server">
        <Reader>
            <ext:JsonReader ReaderID="MONTH">
                <Fields>
                    <ext:RecordField Name="MONTH" />
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
                                            <ext:ToolbarSpacer ID="ToolbarSpacer5" runat="server" Width="10" />
                                            <ext:ComboBox ID="cbbYear" runat="server" ReadOnly="true" StoreID="SYear" Width="80"
                                                DisplayField="YEAR" ValueField="YEAR">
                                            </ext:ComboBox>
                                            <ext:Label ID="lYear" runat="server" Text="年">
                                            </ext:Label>
                                            <ext:ComboBox ID="cbbmonth" runat="server" ReadOnly="true" StoreID="SMonths" Width="60"
                                                DisplayField="MONTH" ValueField="MONTH">
                                            </ext:ComboBox>
                                            <ext:Label ID="lMonth" runat="server" Text="月">
                                            </ext:Label>
                                            <ext:ToolbarSpacer ID="ToolbarSpacer1" runat="server" Width="20" />
                                            <ext:Button ID="btn_Query" runat="server" Text="查询" Icon="Zoom">
                                                <AjaxEvents>
                                                    <Click OnEvent="Btn_Query_Click" Timeout="99999999">
                                                        <EventMask Msg="查询中......" ShowMask="true" />
                                                    </Click>
                                                </AjaxEvents>
                                            </ext:Button>
                                            <ext:ToolbarSpacer ID="ToolbarSpacer3" runat="server" Width="20" />
                                            <ext:Button ID="Btn_Del" Text="返回提交" Icon="Delete" runat="server">
                                                <AjaxEvents>
                                                    <Click OnEvent="Btn_Del_Click">
                                                        <Confirmation BeforeConfirm="config.confirmation.message = '你确定要返回提交吗？';" Title="系统提示"
                                                            ConfirmRequest="true" />
                                                        <ExtraParams>
                                                            <ext:Parameter Name="Values" Value="Ext.encode(#{GridPanel2}.getRowsValues())" Mode="Raw">
                                                            </ext:Parameter>
                                                        </ExtraParams>
                                                    </Click>
                                                </AjaxEvents>
                                            </ext:Button>
                                            <ext:ToolbarSpacer ID="ToolbarSpacer2" runat="server" Width="20" />
                                             <ext:Button ID="btn_Excel" runat="server" OnClick="OutExcel" AutoPostBack="true"
                                                Text="导出Excel" Icon="PageWhiteExcel" CausesValidation="false">
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
                                        <ext:Column ColumnID="INDEX_ID" Header="奖金ID" Width="50" DataIndex="INDEX_ID" MenuDisabled="true">
                                        </ext:Column>
                                        <ext:Column ColumnID="DEPT_CODE" Header="科室编码" Width="100" DataIndex="DEPT_CODE" MenuDisabled="true">
                                        </ext:Column>
                                        <ext:Column ColumnID="DEPT_NAME" Header="科室名称" Width="50" DataIndex="DEPT_NAME" MenuDisabled="true">
                                        </ext:Column>
                                        <ext:Column ColumnID="FLAGS" Header="<div style='text-align:center;'>奖金类型</div>"
                                            Width="100" Align="Center" Sortable="true" DataIndex="FLAGS" MenuDisabled="true">
                                            <Renderer Handler="return value == 0 ? '平均' : '核算';" />
                                            <%--<Editor>
                                                <ext:ComboBox ID="ComboBox1" runat="server" FieldLabel="奖金类型" Editable="false"
                                                    LabelStyle="color:blue;" CausesValidation="true" AllowBlank="false">
                                                    <Items>
                                                        <ext:ListItem Text="平均" Value="0" />
                                                        <ext:ListItem Text="核算" Value="1" />
                                                    </Items>
                                                </ext:ComboBox>
                                            </Editor>--%>
                                        </ext:Column>
                                        <ext:Column ColumnID="ISCOMMIT" Header="<div style='text-align:center;'>是否提交</div>"
                                            Width="100" Align="Center" Sortable="true" DataIndex="ISCOMMIT" MenuDisabled="true">
                                            <Renderer Handler="return value == 0 ? '否' : '是';" />
                                            <%--<Editor>
                                                <ext:ComboBox ID="ComboBox2" runat="server" FieldLabel="是否提交" Editable="false"
                                                    LabelStyle="color:blue;" CausesValidation="true" AllowBlank="false">
                                                    <Items>
                                                        <ext:ListItem Text="否" Value="0" />
                                                        <ext:ListItem Text="是" Value="1" />
                                                    </Items>
                                                </ext:ComboBox>
                                            </Editor>--%>
                                        </ext:Column>
                                    </Columns>
                                </ColumnModel>
                                <SelectionModel>
                                    <ext:RowSelectionModel ID="rowselection" SingleSelect="true">
                                        <Listeners>
                                            <RowSelect Handler="#{Btn_Del}.enable()" />
                                            <RowDeselect Handler="if (!#{GridPanel2}.hasSelection()) {#{Btn_Del}.disable();}" />
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
    </div>
    </form>
</body>
</html>
