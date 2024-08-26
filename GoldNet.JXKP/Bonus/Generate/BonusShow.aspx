<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="BonusShow.aspx.cs" Inherits="GoldNet.JXKP.BonusShow" %>

<%@ Register Assembly="Goldnet.Ext.Web" Namespace="Goldnet.Ext.Web" TagPrefix="ext" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <link rel="stylesheet" type="text/css" href="../Orthers/Cbouns.css" />
    <style type="text/css">
        body
        {
            font-size: 3px;
        }
        .pageSize
        {
            font-size: 13px;
        }
        .trheigh
        {
            height: 30px;
        }
    </style>

    <script type="text/javascript">
        function setVisible() {
            Btn_UpCheck;
        }
        var RefreshData = function() {
            SBONUS.reload();
        }
        function prepare(grid, command, record, row, col, value) {
            if (record.data["ITEM_NAME"] != "核算科室奖金" & record.data["ITEM_NAME"] != "平均奖科室奖金") {
                if (record.data["ITEM_NAME"] == "奖金审核") {
                    command.text = '审核';
                    command.value = "";
                 }
                else {
                    command.hidden = true;
                    command.hideMode = "visibility";
                   
                }
            }
            else {
                command.text = '查看' + record.data["ITEM_NAME"];
                command.iconCls = 'icon-zoom';
            }
           
        }
        var showprogress = function(v, p, record, rowIndex) {
            if (record.data["ITEM_NAME"] == '奖金审核') {
                var template = "<input id='radio1' type='radio' name='radioGroup' onclick=typeSelect(1) value='1' checked />同意&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<input id='radio2' onclick=typeSelect(0) type='radio' name='radioGroup' value='0'>不同意";
                return template;
            }
            else {
               return record.data["ITEM_VALUE"];
            }

        }
        function typeSelect(value) {
            SBONUS.data.items[8].data.ITEM_VALUE = value;
        }
    </script>

</head>
<body>
    <ext:ScriptManager ID="ScriptManager1" runat="server" />
    <ext:Store ID="SBONUS" AutoLoad="true" runat="server" OnRefreshData="Store_RefreshData">
        <Reader>
            <ext:JsonReader>
                <Fields>
                    <ext:RecordField Name="ITEM_NAME">
                    </ext:RecordField>
                    <ext:RecordField Name="ITEM_VALUE">
                    </ext:RecordField>
                </Fields>
            </ext:JsonReader>
        </Reader>
    </ext:Store>
    <form id="form1" runat="server">
    <div>
        <ext:ViewPort ID="ViewPort1" runat="server">
            <Body>
                <table cellspacing="0" cellpadding="0" border="0" width="100%" id="Table4" style="background-color: Transparent;
                    background-color: Transparent; font-size: 20px">
                    <tr>
                        <td>
                            <ext:GridPanel ID="GridPanel2" runat="server" Border="false" StoreID="SBONUS" StripeRows="true"
                                TrackMouseOver="true" Height="300">
                                <TopBar>
                                    <ext:Toolbar ID="Toolbar1" runat="server" Visible="true" AutoWidth="true">
                                        <Items>
                                            <ext:Button ID="Btn_ReBuild" Text="重新生成" Icon="Add" runat="server">
                                                <AjaxEvents>
                                                    <Click OnEvent="Btn_Add_Click">
                                                    </Click>
                                                </AjaxEvents>
                                            </ext:Button>
                                            <ext:Button ID="Btn_Del" Text="删除奖金" Icon="Delete" runat="server">
                                                <AjaxEvents>
                                                    <Click OnEvent="Btn_Del_Click">
                                                        <Confirmation BeforeConfirm="config.confirmation.message = '你确定要删除吗？';" Title="系统提示"
                                                            ConfirmRequest="true" />
                                                    </Click>
                                                </AjaxEvents>
                                            </ext:Button>
                                            <ext:Button ID="Btn_Public" Text="公开奖金" Icon="NoteEdit" runat="server">
                                                <AjaxEvents>
                                                    <Click OnEvent="Btn_Open">
                                                        <Confirmation BeforeConfirm="config.confirmation.message = '你确定要公开奖金吗？';" Title="系统提示"
                                                            ConfirmRequest="true" />
                                                    </Click>
                                                </AjaxEvents>
                                            </ext:Button>
                                            <ext:Button ID="Btn_UpCheck" Text="提交审核" Icon="ArrowUp" runat="server">
                                                <AjaxEvents>
                                                    <Click OnEvent="Btn_Check">
                                                        <Confirmation BeforeConfirm="config.confirmation.message = '你确定要提交审核吗？';" Title="系统提示"
                                                            ConfirmRequest="true" />
                                                    </Click>
                                                </AjaxEvents>
                                            </ext:Button>
                                            <ext:Button ID="Btn_Look" Text="查看本科奖金" Icon="Zoom" runat="server">
                                                <AjaxEvents>
                                                    <Click OnEvent="Btn_Look_SelfDept">
                                                    </Click>
                                                </AjaxEvents>
                                            </ext:Button>
                                            <ext:Button ID="Btn_Back" Text="返回" Icon="ReverseGreen" runat="server">
                                                <AjaxEvents>
                                                    <Click OnEvent="Btn_Back_Clcik">
                                                    </Click>
                                                </AjaxEvents>
                                            </ext:Button>
                                            <ext:Button ID="Btn_Exl" Text="导出全院人员" Icon="TextColumns" runat="server" OnClick="OutExcel"
                                                AutoPostBack="true">
                                            </ext:Button>
                                        </Items>
                                    </ext:Toolbar>
                                </TopBar>
                                <ColumnModel ID="ColumnModel2" runat="server">
                                    <Columns>
                                        <ext:Column ColumnID="ITEM_NAME" Header="项目" Width="200" DataIndex="ITEM_NAME" MenuDisabled="true">
                                        </ext:Column>
                                        <ext:Column ColumnID="ITEM_VALUE" Header="内容" Width="200" DataIndex="ITEM_VALUE"
                                            MenuDisabled="true">
                                            <Renderer Fn="showprogress" />
                                        </ext:Column>
                                        <ext:ImageCommandColumn Header="" Width="200">
                                            <Commands>
                                                <ext:ImageCommand Icon="UserEdit" CommandName="Zoom" Text="查看" />
                                            </Commands>
                                            <PrepareCommand Fn="prepare" />
                                        </ext:ImageCommandColumn>
                                        <%-- <ext:CommandColumn Width="200" Header="" >
                                            <Commands>
                                                <ext:GridCommand Text="查看" CommandName="kankan" Icon="UserEdit">
                                                </ext:GridCommand>
                                            </Commands>  
                                         </ext:CommandColumn>   --%>
                                    </Columns>
                                </ColumnModel>
                                <AjaxEvents>
                                    <Command OnEvent="Btn_Link_Click">
                                        <ExtraParams>
                                            <ext:Parameter Name="Values" Value="Ext.encode(#{GridPanel2}.getRowsValues())" Mode="Raw" />
                                        </ExtraParams>
                                    </Command>
                                </AjaxEvents>
                                <SelectionModel>
                                    <ext:RowSelectionModel ID="rowselection" SingleSelect="true">
                                    </ext:RowSelectionModel>
                                </SelectionModel>
                                <LoadMask ShowMask="true" />
                            </ext:GridPanel>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <table>
                                <tr>
                                    <td>
                                        <ext:Button ID="buttonok" runat="server" Text="同意" Visible="false">
                                            <AjaxEvents>
                                                <Click OnEvent="Btn_subok_Click">
                                                    <Confirmation BeforeConfirm="config.confirmation.message = '你确定同意发放奖金吗？';" Title="系统提示"
                                                        ConfirmRequest="true" />
                                                </Click>
                                            </AjaxEvents>
                                        </ext:Button>
                                    </td>
                                    <td>
                                        <ext:Button ID="buttonon" runat="server" Text="不同意" Visible="false">
                                            <AjaxEvents>
                                                <Click OnEvent="Btn_subno_Click">
                                                    <Confirmation BeforeConfirm="config.confirmation.message = '你确定不同意发放奖金吗？';" Title="系统提示"
                                                        ConfirmRequest="true" />
                                                </Click>
                                            </AjaxEvents>
                                        </ext:Button>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </Body>
        </ext:ViewPort>
        <ext:Window ID="DetailWin" runat="server" Icon="Group" Title="重新测算奖金" Width="360"
            Height="200" AutoShow="false" Modal="true" CenterOnLoad="true" AutoScroll="false"
            ShowOnLoad="false" Closable="false" Resizable="false" StyleSpec="background-color:Transparent;"
            BodyStyle="background-color:Transparent;" CloseAction="Close">
        </ext:Window>
    </div>
    </form>
</body>
</html>
