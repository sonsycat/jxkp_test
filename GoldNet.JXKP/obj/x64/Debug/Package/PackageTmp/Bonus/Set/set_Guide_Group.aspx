<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="set_Guide_Group.aspx.cs"
    Inherits="GoldNet.JXKP.Bonus.Set.set_Guide_Group" %>

<%@ Register Assembly="Goldnet.Ext.Web" Namespace="Goldnet.Ext.Web" TagPrefix="ext" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>

    <script type="text/javascript">
        function FormatRender(v, p, record, rowIndex) {
            var a = Number(record.data.GUIDE_ATTR.toString().substr(3, 2)) % 10;
            var colors = ["red", "blue", "purple", "lime", "green", "navy", "olive", "black", "yellow", "maroon"];
            var template = '<span style="color:{0};">{1}</span>';
            return String.format(template, colors[a], record.data.GUIDE_ATTR_NAME);
        }
        function FormatRenderNew(v, p, record, rowIndex) {
            var cnt = Number(record.data.CNT.toString());
            var template = '<span style="font-style:italic;font-weight:bolder;">{0}</span>';
            return cnt == 0 ? String.format(template, record.data.GUIDE_GATHER_NAME) : record.data.GUIDE_GATHER_NAME;
        }
    </script>

</head>
<body>
    <form id="form1" runat="server">
    <div>
        <ext:ScriptManager ID="ScriptManager1" runat="server" />
        <ext:Store runat="server" ID="Store1" AutoLoad="true" OnRefreshData="Store_RefreshData"
            WarningOnDirty="false">
            <Reader>
                <ext:JsonReader ReaderID="GUIDE_GATHER_CODE">
                    <Fields>
                        <ext:RecordField Name="GUIDE_GATHER_CODE" />
                        <ext:RecordField Name="GUIDE_GATHER_NAME" />
                        <ext:RecordField Name="GUIDE_ATTR" />
                        <ext:RecordField Name="GUIDE_ATTR_NAME" />
                        <ext:RecordField Name="ORGAN_CLASS" />
                        <ext:RecordField Name="ORGAN_CLASS_NAME" />
                        <ext:RecordField Name="EVALUATION_YEAR" />
                        <ext:RecordField Name="CNT" />
                    </Fields>
                </ext:JsonReader>
            </Reader>
        </ext:Store>
        <ext:Store ID="StoreCombo1" runat="server">
            <Reader>
                <ext:JsonReader ReaderID="GUIDE_GROUP_TYPE">
                    <Fields>
                        <ext:RecordField Name="GUIDE_GROUP_TYPE" />
                        <ext:RecordField Name="GUIDE_GROUP_TYPE_NAME" />
                    </Fields>
                </ext:JsonReader>
            </Reader>
        </ext:Store>
        <ext:Store ID="StoreCombo2" runat="server">
            <Reader>
                <ext:JsonReader ReaderID="ORGAN_CLASS_CODE">
                    <Fields>
                        <ext:RecordField Name="ORGAN_CLASS_CODE" />
                        <ext:RecordField Name="ORGAN_CLASS_NAME" />
                    </Fields>
                </ext:JsonReader>
            </Reader>
        </ext:Store>
        <ext:ViewPort ID="ViewPort1" runat="server">
            <Body>
                <ext:ColumnLayout ID="ColumnLayout1" runat="server" Split="true" FitHeight="true">
                    <Columns>
                        <ext:LayoutColumn ColumnWidth="1">
                            <ext:GridPanel ID="GridPanel_List" runat="server" Border="false" StoreID="Store1"
                                StripeRows="true" TrackMouseOver="true" Height="480" AutoWidth="true">
                                <TopBar>
                                    <ext:Toolbar ID="Toolbar1" runat="server" Visible="true" AutoWidth="true">
                                        <Items>
                                            <ext:ToolbarSeparator ID="ToolbarSeparator4" runat="server" />
                                            <ext:ToolbarButton ID="Btn_Add" runat="server" Text="增加" Icon="Add">
                                                <AjaxEvents>
                                                    <Click OnEvent="Btn_Add_Click">
                                                        <EventMask Msg="载入中..." ShowMask="true" />
                                                    </Click>
                                                </AjaxEvents>
                                            </ext:ToolbarButton>
                                            <ext:ToolbarSeparator ID="ToolbarSeparator5" runat="server">
                                            </ext:ToolbarSeparator>
                                            <ext:ToolbarButton ID="Btn_Edit" runat="server" Text="编辑" Icon="NoteEdit" Disabled="true">
                                                <AjaxEvents>
                                                    <Click OnEvent="Btn_Edit_Click">
                                                        <EventMask Msg="载入中..." ShowMask="true" />
                                                        <ExtraParams>
                                                            <ext:Parameter Name="Values" Value="Ext.encode(#{GridPanel_List}.getRowsValues())"
                                                                Mode="Raw">
                                                            </ext:Parameter>
                                                        </ExtraParams>
                                                    </Click>
                                                </AjaxEvents>
                                            </ext:ToolbarButton>
                                            <ext:ToolbarSeparator ID="ToolbarSeparator2" runat="server">
                                            </ext:ToolbarSeparator>
                                            <ext:ToolbarButton ID="Btn_Del" runat="server" Text="删除" Icon="Delete" Disabled="true">
                                                <AjaxEvents>
                                                    <Click OnEvent="Btn_Del_Click">
                                                        <Confirmation Title="系统提示" BeforeConfirm="config.confirmation.message = '删除指标集将会同时清除指标相关性以及岗位指标集的关联，<BR/>确定要删除指标集 '+GridPanel_List.getSelectionModel().getSelected().data.GUIDE_GATHER_NAME+' 吗？';"
                                                            ConfirmRequest="true" />
                                                        <ExtraParams>
                                                            <ext:Parameter Name="Values" Value="GridPanel_List.getSelectionModel().getSelected().data.GUIDE_GATHER_CODE"
                                                                Mode="Raw">
                                                            </ext:Parameter>
                                                        </ExtraParams>
                                                        <EventMask Msg="正在删除..." ShowMask="true" />
                                                    </Click>
                                                </AjaxEvents>
                                            </ext:ToolbarButton>
                                            <ext:ToolbarSeparator ID="ToolbarSeparator3" runat="server">
                                            </ext:ToolbarSeparator>
                                            <ext:ToolbarButton ID="Btn_Refresh" runat="server" Text="刷新" Icon="ArrowRefresh">
                                                <Listeners>
                                                    <Click Handler="#{Combo_GuideGroupType}.setValue('');#{Store1}.reload();" />
                                                </Listeners>
                                            </ext:ToolbarButton>
                                            <ext:ToolbarSeparator ID="ToolbarSeparator1" runat="server">
                                            </ext:ToolbarSeparator>
                                        </Items>
                                    </ext:Toolbar>
                                </TopBar>
                                <ColumnModel ID="ColumnModel1" runat="server">
                                    <Columns>
                                        <ext:Column Header="指标集代码" Width="80" ColumnID="GUIDE_GATHER_CODE" DataIndex="GUIDE_GATHER_CODE" />
                                        <ext:Column Header="指标集名称" Width="200" ColumnID="GUIDE_GATHER_NAME" DataIndex="GUIDE_GATHER_NAME">
                                            <%-- <Renderer Fn="FormatRenderNew" />--%>
                                        </ext:Column>
                                        <ext:CommandColumn Width="40" Align="Left" Header="指标">
                                            <Commands>
                                                <ext:GridCommand Icon="FolderTable" CommandName="GuideDefine">
                                                    <ToolTip Text="定义查看指标集包含的指标" />
                                                </ext:GridCommand>
                                            </Commands>
                                        </ext:CommandColumn>
                                    </Columns>
                                </ColumnModel>
                                <SelectionModel>
                                    <ext:RowSelectionModel ID="rowselection" SingleSelect="true">
                                        <Listeners>
                                            <SelectionChange Handler="var tmpflg=  #{GridPanel_List}.hasSelection()?false:true;   #{Btn_Edit}.setDisabled(tmpflg);  #{Btn_Del}.setDisabled(tmpflg);" />
                                        </Listeners>
                                    </ext:RowSelectionModel>
                                </SelectionModel>
                                <BottomBar>
                                    <ext:PagingToolbar ID="PagingToolBar1" runat="server" PageSize="20" StoreID="Store1"
                                        AutoWidth="true" DisplayInfo="true" AutoDataBind="true" />
                                </BottomBar>
                                <LoadMask ShowMask="true" />
                                <AjaxEvents>
                                    <Command OnEvent="Btn_Command_Click">
                                        <ExtraParams>
                                            <ext:Parameter Name="Values" Value="record.data.GUIDE_GATHER_CODE" Mode="Raw">
                                            </ext:Parameter>
                                            <ext:Parameter Name="Names" Value="record.data.GUIDE_GATHER_NAME" Mode="Raw">
                                            </ext:Parameter>
                                            <ext:Parameter Name="Organ" Value="record.data.ORGAN_CLASS" Mode="Raw">
                                            </ext:Parameter>
                                            <ext:Parameter Name="command" Value="command" Mode="Raw">
                                            </ext:Parameter>
                                        </ExtraParams>
                                    </Command>
                                    <RowDblClick OnEvent="Btn_Edit_Click">
                                        <EventMask Msg="载入中..." ShowMask="true" />
                                        <ExtraParams>
                                            <ext:Parameter Name="Values" Value="Ext.encode(#{GridPanel_List}.getRowsValues())"
                                                Mode="Raw">
                                            </ext:Parameter>
                                        </ExtraParams>
                                    </RowDblClick>
                                </AjaxEvents>
                            </ext:GridPanel>
                        </ext:LayoutColumn>
                    </Columns>
                </ext:ColumnLayout>
            </Body>
        </ext:ViewPort>
        <ext:Window ID="EditWin" runat="server" Icon="PageEdit" Title="指标集" Width="300" Height="200"
            AutoShow="false" Modal="true" AutoScroll="true" CenterOnLoad="true" ShowOnLoad="false"
            Resizable="false">
            <Body>
                <ext:FormPanel ID="FormPanel2" runat="server" AutoWidth="true" Header="false" Height="165"
                    LabelWidth="100" BodyStyle="padding:5px;background-color:Transparent; border-left:0px;border-top:0px;border-right:0px;"
                    ButtonAlign="Right">
                    <Body>
                        <ext:FormLayout ID="FormLayout2" runat="server">
                            <ext:Anchor Horizontal="95%">
                                <ext:Panel ID="Panel1" runat="server" BodyStyle="background-color:Transparent" Border="false">
                                    <Body>
                                        <table>
                                            <tr>
                                                <td style="width: 90px">
                                                    指标集名称:
                                                </td>
                                                <td colspan="2">
                                                    <ext:TextField ID="GuideGroupName" runat="server" FieldLabel="指标集名称" Width="184"
                                                        AllowBlank="false" CausesValidation="true" BlankText="请输入指标集名称" MaxLength="30" />
                                                    <ext:Hidden ID="SelectedID" runat="server">
                                                    </ext:Hidden>
                                                </td>
                                            </tr>
                                        </table>
                                    </Body>
                                </ext:Panel>
                            </ext:Anchor>
                        </ext:FormLayout>
                    </Body>
                    <Buttons>
                        <ext:Button runat="server" ID="Button1" Text="保存" Icon="Disk">
                            <AjaxEvents>
                                <Click OnEvent="Btn_Save_Click">
                                    <EventMask Msg="正在保存" ShowMask="false" />
                                </Click>
                            </AjaxEvents>
                        </ext:Button>
                        <ext:Button runat="server" ID="Button2" Text="取消" Icon="Cancel">
                            <Listeners>
                                <Click Handler="#{EditWin}.hide();" />
                            </Listeners>
                        </ext:Button>
                    </Buttons>
                </ext:FormPanel>
            </Body>
        </ext:Window>
        <ext:Window ID="DetailWin" runat="server" Icon="ApplicationViewIcons" Title="选择指标"
            Width="800" Closable="true" CloseAction="Hide" Maximizable="true" Height="700"
            AutoShow="false" Modal="true" AutoScroll="true" CenterOnLoad="true" ShowOnLoad="false"
            Resizable="true" StyleSpec="background-color:Transparent;" BodyStyle="background-color:Transparent;">
            <Listeners>
                <Hide Handler="this.clearContent();" />
                <BeforeShow Handler=" var height = Ext.getBody().getViewSize().height; var width = Ext.getBody().getViewSize().width; if (el.getSize().height > height) {  el.setHeight(height - 20) } ;if (el.getSize().width > width) {  el.setWidth(width - 20) }  " />
            </Listeners>
        </ext:Window>
    </div>
    </form>
</body>
</html>
