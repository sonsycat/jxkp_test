<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="BonusPersonList.aspx.cs"
    Inherits="GoldNet.JXKP.BonusPersonList" %>

<%@ Register Assembly="Goldnet.Ext.Web" Namespace="Goldnet.Ext.Web" TagPrefix="ext" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <link rel="stylesheet" type="text/css" href="../Orthers/Cbouns.css" />

    <script type="text/javascript">
        function backToList() {
            window.navigate("Dept_List.aspx");
        }
         var RefreshData = function() {
            Store1.reload();
        }   
    </script>

    <script type="text/javascript">
             var rmbMoney = function(v) {
                 v = (Math.round((v - 0) * 100)) / 100;
                 v = (v == Math.floor(v)) ? v + ".00" : ((v * 10 == Math.floor(v * 10)) ? v + "0" : v);
                 v = String(v);
                 var ps = v.split('.');
                 var whole = ps[0];
                 var sub = ps[1] ? '.' + ps[1] : '.00';
                 var r = /(\d+)(\d{3})/;
                 while (r.test(whole)) {
                     whole = whole.replace(r, '$1' + ',' + '$2');
                 }
                 v = whole + sub;
                 if (v.charAt(0) == '-') {
                     return '-' + v.substr(1);
                 }
                 return v;
             }; 
             
    </script>

</head>
<body>
    <ext:ScriptManager ID="ScriptManager1" runat="server" AjaxMethodNamespace="Goldnet" />
    <ext:Store ID="Store1" AutoLoad="true" runat="server" OnRefreshData="Store_RefreshData">
        <Reader>
            <ext:JsonReader ReaderID="ID">
                <Fields>
                </Fields>
            </ext:JsonReader>
        </Reader>
    </ext:Store>
    <ext:Store ID="Store3" runat="server" WarningOnDirty="false">
        <Reader>
            <ext:JsonReader ReaderID="DEPT_CODE">
                <Fields>
                    <ext:RecordField Name="DEPT_CODE" Type="String" Mapping="DEPT_CODE" />
                    <ext:RecordField Name="DEPT_NAME" Type="String" Mapping="DEPT_NAME" />
                    <ext:RecordField Name="FLAGS" Type="String" Mapping="FLAGS" />
                    <ext:RecordField Name="SUBMIT_PERSONS" Type="String" Mapping="SUBMIT_PERSONS" />
                </Fields>
            </ext:JsonReader>
        </Reader>
    </ext:Store>
    <form id="form1" runat="server">
    <div>
        <ext:ViewPort ID="ViewPort1" runat="server" AutoWidth="true">
            <Body>
                <ext:BorderLayout ID="BorderLayout1" runat="server">
                    <Center>
                        <ext:Panel ID="Panel2" runat="server" BodyBorder="true" Border="false">
                            <Body>
                                <ext:ColumnLayout ID="ColumnLayout1" runat="server" Split="true" FitHeight="true">
                                    <Columns>
                                        <ext:LayoutColumn ColumnWidth="1">
                                            <ext:GridPanel ID="GridPanel1" runat="server" Border="false" StoreID="Store1" StripeRows="true"
                                                Width="2000" TrackMouseOver="true" Height="480" ClicksToEdit="1">
                                                <TopBar>
                                                    <ext:Toolbar ID="Toolbar1" runat="server" Visible="true" AutoWidth="true">
                                                        <Items>
                                                            <ext:Button ID="Btn_Exl" Text="导出Excel" Icon="TextColumns" runat="server" OnClick="OutExcel"
                                                                AutoPostBack="true">
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
                                                            <ext:Button ID="Btn_Del" Text="删除" Icon="Delete" runat="server">
                                                                <AjaxEvents>
                                                                    <Click OnEvent="Btn_Del_Click">
                                                                        <Confirmation BeforeConfirm="config.confirmation.message = '你确定要删除吗？';" Title="系统提示"
                                                                            ConfirmRequest="true" />
                                                                        <ExtraParams>
                                                                            <ext:Parameter Name="Values" Value="Ext.encode(#{GridPanel1}.getRowsValues(false))"
                                                                                Mode="Raw" />
                                                                            <ext:Parameter Name="Selecct" Value="Ext.encode(#{GridPanel1}.getRowsValues())" Mode="Raw">
                                                                            </ext:Parameter>
                                                                        </ExtraParams>
                                                                    </Click>
                                                                </AjaxEvents>
                                                            </ext:Button>
                                                            <ext:Button ID="Btn_DelAll" Text="全部删除" Icon="Decline" runat="server">
                                                                <AjaxEvents>
                                                                    <Click OnEvent="Btn_DelAll_Click">
                                                                        <Confirmation BeforeConfirm="config.confirmation.message = '你确定要删除吗？';" Title="系统提示"
                                                                            ConfirmRequest="true" />
                                                                    </Click>
                                                                </AjaxEvents>
                                                            </ext:Button>
                                                            <ext:Button ID="AddPersons" Text="添加人员" Icon="ApplicationAdd" runat="server" Hidden="false">
                                                                <AjaxEvents>
                                                                    <Click OnEvent="AddPersons_Click">
                                                                    </Click>
                                                                </AjaxEvents>
                                                            </ext:Button>
                                                            <ext:Button ID="Commit" Text="提交" Icon="Accept" runat="server">
                                                                <AjaxEvents>
                                                                    <Click OnEvent="Btn_Commit_Click">
                                                                        <Confirmation BeforeConfirm="config.confirmation.message = '你确定要提交吗？';" Title="系统提示"
                                                                            ConfirmRequest="true" />
                                                                    </Click>
                                                                </AjaxEvents>
                                                            </ext:Button>
                                                            <ext:Button ID="Btn_Back" Text="返回" Icon="ReverseGreen" runat="server">
                                                                <AjaxEvents>
                                                                    <Click OnEvent="Back_Click">
                                                                    </Click>
                                                                </AjaxEvents>
                                                            </ext:Button>
                                                            <%--<ext:ComboBox ID="comindex" runat="server" AllowBlank="true" EmptyText="请选择奖金" Width="200"
                                                                FieldLabel="奖金选择">
                                                            </ext:ComboBox>--%>
                                                            <ext:Button ID="Button1" Text="增加人员" Icon="ApplicationAdd" runat="server" Hidden="true" >
                                                                <AjaxEvents>
                                                                    <Click OnEvent="SelectedFunc">
                                                                        <Confirmation BeforeConfirm="config.confirmation.message = '重新提取会先删除本科室人员，你确定要从新提取以前奖金人员吗？';"
                                                                            Title="系统提示" ConfirmRequest="true" />
                                                                    </Click>
                                                                </AjaxEvents>
                                                            </ext:Button>
                                                            <ext:ToolbarSpacer ID="ToolbarSpacer5" runat="server" Width="6" />
                                                            <ext:ToolbarSeparator ID="ToolbarSeparator3" runat="server" />
                                                            <ext:Label ID="memu" runat="server">
                                                            </ext:Label>
                                                        </Items>
                                                    </ext:Toolbar>
                                                </TopBar>
                                                <ColumnModel ID="extColumnModel2" runat="server">
                                                    <Columns>
                                                    </Columns>
                                                </ColumnModel>
                                                <SelectionModel>
                                                    <ext:RowSelectionModel SingleSelect="true">
                                                    </ext:RowSelectionModel>
                                                </SelectionModel>
                                                <LoadMask ShowMask="true" />
                                            </ext:GridPanel>
                                        </ext:LayoutColumn>
                                    </Columns>
                                </ext:ColumnLayout>
                            </Body>
                        </ext:Panel>
                    </Center>
                    <East MinWidth="150" MaxWidth="300" SplitTip="人员奖金提交状态信息" Collapsible="true" Split="true">
                        <ext:Panel ID="Panel1" runat="server" Border="false" Width="250" Title="人员奖金提交状态信息"
                            Collapsed="true" AutoScroll="true" TitleCollapse="True">
                            <TopBar>
                                <ext:Toolbar ID="Toolbar2" runat="server">
                                    <Items>
                                        <ext:Button ID="Button_ok" runat="server" Text="提交" Icon="Disk">
                                            <AjaxEvents>
                                                <Click OnEvent="Button_ok_click">
                                                    <Confirmation ConfirmRequest="true" Title="系统提示" Message="将提交选中的科室,<br/>是否继续?" />
                                                    <ExtraParams>
                                                    </ExtraParams>
                                                    <EventMask Msg="载入中..." ShowMask="true" />
                                                    <ExtraParams>
                                                        <ext:Parameter Name="Values" Value="Ext.encode(#{GridPanel2}.getRowsValues())" Mode="Raw">
                                                        </ext:Parameter>
                                                    </ExtraParams>
                                                </Click>
                                            </AjaxEvents>
                                        </ext:Button>
                                        <ext:ToolbarSeparator ID="ToolbarSeparator5" runat="server" />
                                        <ext:Button ID="Button_no" runat="server" Text="取消" Icon="Disk">
                                            <AjaxEvents>
                                                <Click OnEvent="Button_no_click">
                                                    <Confirmation ConfirmRequest="true" Title="系统提示" Message="将取消选中的科室,<br/>是否继续?" />
                                                    <ExtraParams>
                                                    </ExtraParams>
                                                    <EventMask Msg="载入中..." ShowMask="true" />
                                                    <ExtraParams>
                                                        <ext:Parameter Name="Values" Value="Ext.encode(#{GridPanel2}.getRowsValues())" Mode="Raw">
                                                        </ext:Parameter>
                                                    </ExtraParams>
                                                </Click>
                                            </AjaxEvents>
                                        </ext:Button>
                                    </Items>
                                </ext:Toolbar>
                            </TopBar>
                            <Body>
                                <ext:ColumnLayout ID="ColumnLayout2" runat="server" Split="true">
                                    <Columns>
                                        <ext:LayoutColumn ColumnWidth="1">
                                            <ext:GridPanel ID="GridPanel2" runat="server" Border="false" StoreID="Store3" StripeRows="true"
                                                AutoHeight="true" AutoWidth="true" TrackMouseOver="true" AutoScroll="true">
                                                <ColumnModel ID="ColumnModel2" runat="server">
                                                    <Columns>
                                                        <ext:Column ColumnID="DEPT_CODE" Hidden="true" />
                                                        <ext:Column Header="科室" Width="100" ColumnID="DEPT_NAME" DataIndex="DEPT_NAME" Sortable="false"
                                                            MenuDisabled="true" />
                                                        <ext:Column Header="状态" Width="60" ColumnID="FLAGS" DataIndex="FLAGS" Sortable="false"
                                                            MenuDisabled="true" />
                                                        <ext:Column Header="提交人" Width="60" ColumnID="SUBMIT_PERSONS" DataIndex="SUBMIT_PERSONS"
                                                            Sortable="false" MenuDisabled="true" />
                                                    </Columns>
                                                </ColumnModel>
                                                <SelectionModel>
                                                    <ext:CheckboxSelectionModel ID="RowSelectionModel2" runat="server">
                                                    </ext:CheckboxSelectionModel>
                                                </SelectionModel>
                                                <Listeners>
                                                </Listeners>
                                                <LoadMask ShowMask="true" />
                                            </ext:GridPanel>
                                        </ext:LayoutColumn>
                                    </Columns>
                                </ext:ColumnLayout>
                            </Body>
                        </ext:Panel>
                    </East>
                </ext:BorderLayout>
            </Body>
        </ext:ViewPort>
    </div>
    <ext:Window ID="addpersonsWin" runat="server" Icon="Group" Title="添加人员" Width="580"
        Height="400" AutoShow="false" Modal="true" CenterOnLoad="true" AutoScroll="true"
        ShowOnLoad="false" Resizable="true" StyleSpec="background-color:Transparent;"
        BodyStyle="background-color:Transparent;">
    </ext:Window>
    </form>
</body>
</html>
