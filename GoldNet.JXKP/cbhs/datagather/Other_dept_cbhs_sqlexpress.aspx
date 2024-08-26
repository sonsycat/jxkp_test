<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Other_dept_cbhs_sqlexpress.aspx.cs" Inherits="GoldNet.JXKP.Other_dept_cbhs_sqlexpress" %>
<%@ Register Assembly="Goldnet.Ext.Web" Namespace="Goldnet.Ext.Web" TagPrefix="ext" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
     <title></title>
    <link rel="stylesheet" type="text/css" href="../../Bonus/Orthers/Cbouns.css" />
    <script type="text/javascript">
        var RefreshData = function(msg) {
            Ext.Msg.show({ title: '提示', msg: msg, icon: 'ext-mb-info', buttons: { ok: true} });
            Store1.reload();
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
     <ext:ScriptManager ID="ScriptManager1" runat="server"/>
    <ext:Store ID="Store1" runat="server" AutoLoad="true"  OnRefreshData="Store_RefreshData">
        <Reader>
            <ext:JsonReader ReaderID="ID">
                <Fields>
                    <ext:RecordField Name="ID"  />
                    <ext:RecordField Name="SQL_NAME"  />
                    <ext:RecordField Name="SQLEXPRESS" />
                    <ext:RecordField Name="FLAGSS"  />
                    <ext:RecordField Name="MEMO"  />
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
                                                    <Click OnEvent="Btn_Add_Click"></Click>
                                                </AjaxEvents>
                                        </ext:Button>
                                        <ext:Button ID="Btn_Edit" Text="编辑" Icon="NoteEdit" runat="server" Disabled="true">
                                             <AjaxEvents>
                                                    <Click OnEvent="Btn_Edit_Click">
                                                        <ExtraParams>
                                                            <ext:Parameter Name="Values" Value="Ext.encode(#{GridPanel1}.getRowsValues())"
                                                                Mode="Raw">
                                                            </ext:Parameter>
                                                        </ExtraParams>
                                                    </Click>
                                                </AjaxEvents>
                                        </ext:Button>
                                        <ext:Button ID="Btn_Del" Text="删除" Icon="Delete" runat="server" Disabled="true">
                                            <AjaxEvents>                                                    
                                                    <Click OnEvent="Btn_Del_Click">
                                                    <Confirmation BeforeConfirm="config.confirmation.message = '你确定要删除吗？';"  Title="系统提示"   ConfirmRequest="true" />
                                                        <ExtraParams>
                                                             <ext:Parameter Name="Values" Value="Ext.encode(#{GridPanel1}.getRowsValues())"
                                                                Mode="Raw">
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
                                    <ext:Column ColumnID="ID" Header="序号" Width="100" Align="left" Sortable="true"
                                        DataIndex="ID" MenuDisabled="true" />  
                                    <ext:Column ColumnID="SQL_NAME" Header="语句名称" Width="100" Align="left" Sortable="true"
                                        DataIndex="SQL_NAME" MenuDisabled="true" />  
                                    <ext:Column ColumnID="SQLEXPRESS" Header="语句" Width="100" Align="left" Sortable="true"
                                        DataIndex="SQLEXPRESS" MenuDisabled="true" />
                                    <ext:Column ColumnID="FLAGSS" Header="是否启用" Width="100" Align="left" Sortable="true"
                                        DataIndex="FLAGSS" MenuDisabled="true" />
                                        <ext:Column ColumnID="MEMO" Header="备注" Width="100" Align="left" Sortable="true"
                                        DataIndex="MEMO" MenuDisabled="true" />
                                </Columns>
                            </ColumnModel>
                            <SelectionModel>
                                <ext:RowSelectionModel ID="RowSelectionModel1" runat="server" SingleSelect="true">  
                                     <Listeners>
                                            <RowSelect Handler="#{Btn_Edit}.enable();#{Btn_Del}.enable()" />
                                            <RowDeselect Handler="if (!#{GridPanel1}.hasSelection()) {#{Btn_Del}.disable();#{Btn_Edit}.disable()}" />
                                        </Listeners>                                
                                </ext:RowSelectionModel>
                            </SelectionModel>
                        </ext:GridPanel>
                    </ext:LayoutColumn>
                </Columns>
            </ext:ColumnLayout>
        </Body>
    </ext:ViewPort>
     <ext:Window ID="DetailWin" runat="server" Icon="Group" Title="语句设置" Width="400"
            Height="500"  AutoShow="false" Modal="true" CenterOnLoad="true" AutoScroll="true" ShowOnLoad="false" Resizable="false" 
            StyleSpec="background-color:Transparent;" BodyStyle="background-color:Transparent;">
    </ext:Window>
    </form>
</body>
</html>
