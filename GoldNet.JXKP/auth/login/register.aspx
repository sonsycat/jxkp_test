<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="register.aspx.cs" Inherits="GoldNet.JXKP.auth.login.register" %>

<%@ Register Assembly="Goldnet.Ext.Web" Namespace="Goldnet.Ext.Web" TagPrefix="ext" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <style type="text/css">
        body
        {
            background-color: #DFE8F6;
            font-size: 12px;
        }
    </style>
    <script language="javascript" type="text/javascript">
        function edit(data_id) {
            Goldnet.AjaxMethod.request('data_edit',
            {
                params:
                    {
                        id: data_id
                    }
            });
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <ext:ScriptManager ID="ScriptManager1" runat="server" AjaxMethodNamespace="CompanyX" />
    <ext:Store ID="Store1" runat="server" OnRefreshData="Store_RefreshData">
        <Reader>
            <ext:JsonReader ReaderID="USER_ID">
                <Fields>
                    <ext:RecordField Name="USER_ID" />
                    <ext:RecordField Name="DB_USER" />
                    <ext:RecordField Name="USER_NAME" />
                    <ext:RecordField Name="USER_DEPT" />
                    <ext:RecordField Name="LOGIN_DATE" />
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
                                <ext:Toolbar ID="Toolbar_fjsr" runat="server" Visible="true" AutoWidth="true">
                                    <Items>
                                        <ext:Button ID="Button_add" runat="server" Text="添加" Icon="Add">
                                            <AjaxEvents>
                                                <Click OnEvent="Button_add_click">
                                                    <EventMask Msg="载入中..." ShowMask="true" />
                                                    <ExtraParams>
                                                        <ext:Parameter Name="Values" Value="Ext.encode(#{GridPanel1}.getRowsValues())" Mode="Raw">
                                                        </ext:Parameter>
                                                    </ExtraParams>
                                                </Click>
                                            </AjaxEvents>
                                        </ext:Button>
                                        <ext:ToolbarSeparator ID="ToolbarSeparator3" runat="server" />
                                        <ext:Button ID="Button_del" runat="server" Text="删除" Icon="Delete" Disabled="true">
                                            <AjaxEvents>
                                                <Click OnEvent="Button_del_click">
                                                    <Confirmation ConfirmRequest="true" Title="系统提示" Message="将删除选中数据,<br/>是否继续?" />
                                                    <ExtraParams>
                                                    </ExtraParams>
                                                    <EventMask Msg="载入中..." ShowMask="true" />
                                                    <ExtraParams>
                                                        <ext:Parameter Name="Values" Value="Ext.encode(#{GridPanel1}.getRowsValues())" Mode="Raw">
                                                        </ext:Parameter>
                                                    </ExtraParams>
                                                </Click>
                                            </AjaxEvents>
                                        </ext:Button>
                                        <ext:TextField runat="server" Width="260" ID="tfBName" AllowBlank="false" CausesValidation="true">
                                        </ext:TextField>
                                        <ext:Button ID="Button1" runat="server" Text="查询" Icon="Zoom">
                                            <AjaxEvents>
                                                <Click OnEvent="Btn_Search_Click">
                                                    <EventMask Msg="载入中..." ShowMask="true" />
                                                </Click>
                                            </AjaxEvents>
                                        </ext:Button>

                                    </Items>
                                </ext:Toolbar>
                            </TopBar>
                            <ColumnModel ID="ColumnModel1" runat="server">
                                <Columns>
                                    <ext:Column ColumnID="USER_ID" Header="<div style='text-align:center;'>USER_ID</div>"
                                        Width="90" Sortable="true" DataIndex="USER_ID" MenuDisabled="true" />
                                    <ext:Column ColumnID="DB_USER" Header="<div style='text-align:center;'>登录名</div>"
                                        Width="90" Sortable="true" DataIndex="DB_USER" MenuDisabled="true">
                                    </ext:Column>
                                    <ext:Column ColumnID="USER_NAME" Header="<div style='text-align:center;'>人员名称</div>"
                                        Width="90" Sortable="true" DataIndex="USER_NAME" MenuDisabled="true">
                                    </ext:Column>
                                    <ext:Column ColumnID="USER_DEPT" Header="<div style='text-align:center;'>科室编码</div>"
                                        Width="90" Sortable="true" DataIndex="USER_DEPT" MenuDisabled="true" />
                                    <ext:Column ColumnID="LOGIN_DATE" Header="<div style='text-align:center;'>创建时间</div>"
                                        Width="90" Sortable="true" DataIndex="LOGIN_DATE" MenuDisabled="true" />
                                    <ext:CommandColumn Width="60" Header="<div style='text-align:center;'>操作</div>">
                                        <Commands>
                                            <ext:GridCommand Icon="NoteEdit" Text="<div style='text-align:center;'>编辑</div>"
                                                CommandName="Edit">
                                                <ToolTip Text="编辑" />
                                            </ext:GridCommand>
                                        </Commands>
                                    </ext:CommandColumn>
                                </Columns>
                            </ColumnModel>
                            <SelectionModel>
                                <ext:CheckboxSelectionModel ID="RowSelectionModel1" runat="server">
                                    <Listeners>
                                        <SelectionChange Handler="#{GridPanel1}.hasSelection()? #{Button_del}.setDisabled(false): #{Button_del}.setDisabled(true);" />
                                    </Listeners>
                                </ext:CheckboxSelectionModel>
                            </SelectionModel>
                            <LoadMask ShowMask="true" />
                            <BottomBar>
                                <ext:PagingToolbar ID="PagingToolBar1" runat="server" PageSize="20" StoreID="Store1" />
                            </BottomBar>
                            <Listeners>
                                <Command Handler="edit(record.data.USER_ID);" />
                            </Listeners>
                        </ext:GridPanel>
                    </ext:LayoutColumn>
                </Columns>
            </ext:ColumnLayout>
        </Body>
    </ext:ViewPort>
    <ext:Window ID="DetailWin" runat="server" Icon="Group" Title="人员详细信息" Width="370"
        Height="240" AutoShow="false" Modal="true" CenterOnLoad="true" AutoScroll="true"
        ShowOnLoad="false" Resizable="true" StyleSpec="background-color:Transparent;"
        BodyStyle="background-color:Transparent;">
    </ext:Window>
    </form>
</body>
</html>
