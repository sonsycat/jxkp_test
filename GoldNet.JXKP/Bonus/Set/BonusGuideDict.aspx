<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="BonusGuideDict.aspx.cs"
    Inherits="GoldNet.JXKP.BonusGuideDict" %>

<%@ Register Assembly="Goldnet.Ext.Web" Namespace="Goldnet.Ext.Web" TagPrefix="ext" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>指标列表</title>
    <link rel="stylesheet" type="text/css" href="../Orthers/Cbouns.css" />

    <script type="text/javascript">
        var applyFilter = function () {
            Store1.filterBy(getRecordFilter());
        };
        var getRecordFilter = function () {
            var f = [];
            f.push({
                filter: function (record) {
                    return filterString(txt_SearchTxt.getValue(), 'GUIDE_NAME', record);
                }
            });
            f.push({
                filter: function (record) {
                    return filterString(txt_SearchTxt.getValue(), 'DEPT_CLASS_NAME', record);
                }
            });
            f.push({
                filter: function (record) {
                    return filterString(txt_SearchTxt.getValue(), 'BSC_CLASS_NAME2', record);
                }
            });
            f.push({
                filter: function (record) {
                    return filterString(txt_SearchTxt.getValue(), 'GUIDE_CODE', record);
                }
            });
            f.push({
                filter: function (record) {
                    return filterString(txt_SearchTxt.getValue(), 'ISPAGE', record);
                }
            });

            var len = f.length;
            return function (record) {
                if (f[0].filter(record) || f[1].filter(record) || f[2].filter(record) || f[3].filter(record) || f[4].filter(record)) {
                    return true;
                }
                return false;
            }
        };
        var filterString = function (value, dataIndex, record) {
            var val = record.get(dataIndex);
            if (typeof val != "string") {
                return value.length == 0;
            }
            return val.toLowerCase().indexOf(value.toLowerCase()) > -1;
        };
        var filterNumber = function (value, dataIndex, record) {
            var val = record.get(dataIndex);
            if (!Ext.isEmpty(value, false) && val != value) {
                return false;
            }
            return true;
        };
        function FormatRenderNew(v, p, record, rowIndex) {
            var newFlg = false;
            if (record.data.ISPAGE == '启用' && record.data.ISEXPRESS == '否' && record.data.GUIDE_SQL == '无')
                newFlg = true;
            var template = '<span style="font-style:italic;font-weight:bolder;">{0}</span>';
            return newFlg ? String.format(template, record.data.GUIDE_NAME) : record.data.GUIDE_NAME;
        }
    </script>

</head>
<body>
    <form id="form1" runat="server">
    <div>
        <ext:ScriptManager ID="ScriptManager1" runat="server" />
        <ext:Store runat="server" ID="Store1" AutoLoad="true" OnRefreshData="Store_RefreshData">
            <Reader>
                <ext:JsonReader ReaderID="GUIDE_CODE">
                    <Fields>
                        <ext:RecordField Name="GUIDE_CODE" />
                        <ext:RecordField Name="BSC_CLASS_NAME1" />
                        <ext:RecordField Name="BSC_CLASS_NAME2" />
                        <ext:RecordField Name="GUIDE_NAME" />
                        <ext:RecordField Name="GUIDE_EXPRESS" />
                        <ext:RecordField Name="ISEXPRESS" />
                        <ext:RecordField Name="ISPAGE" />
                        <ext:RecordField Name="ISHIGHGUIDE" />
                        <ext:RecordField Name="ISSEL" />
                        <ext:RecordField Name="ISABS" />
                        <ext:RecordField Name="GUIDE_SQL" />
                        <ext:RecordField Name="DEPT_CLASS_NAME" />
                        <ext:RecordField Name="ORGAN_CLASS_NAME" />
                        <ext:RecordField Name="BSC" />
                        <ext:RecordField Name="DEPT" />
                        <ext:RecordField Name="ORGAN" />
                        <ext:RecordField Name="SERIAL_NO" />
                        <ext:RecordField Name="GUIDE_GATHER_CODE" />
                        <ext:RecordField Name="ISSAME" />
                        <ext:RecordField Name="SHOWNUM" />
                        <ext:RecordField Name="EXPLAIN" />
                        <ext:RecordField Name="INDEXTYPE" />
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
                                StripeRows="true" TrackMouseOver="true" Height="480" AutoWidth="true" AutoExpandColumn="GUIDE_NAME">
                                <TopBar>
                                    <ext:Toolbar ID="Toolbar1" runat="server">
                                        <Items>
                                            <ext:ToolbarButton ID="Btn_Add" runat="server" Text="增加" Icon="Add">
                                                <AjaxEvents>
                                                    <Click OnEvent="Btn_Add_Click">
                                                        <EventMask ShowMask="true" Msg="请稍候..." />
                                                    </Click>
                                                </AjaxEvents>
                                            </ext:ToolbarButton>
                                            <ext:ToolbarSeparator ID="ToolbarSeparator5" runat="server">
                                            </ext:ToolbarSeparator>
                                            <ext:ToolbarButton ID="Btn_Edit" runat="server" Text="编辑" Icon="NoteEdit" Disabled="true">
                                                <AjaxEvents>
                                                    <Click OnEvent="Btn_Edit_Click">
                                                        <ExtraParams>
                                                            <ext:Parameter Name="Values" Value="Ext.encode(#{GridPanel_List}.getRowsValues())"
                                                                Mode="Raw">
                                                            </ext:Parameter>
                                                        </ExtraParams>
                                                        <EventMask ShowMask="true" Msg="请稍候..." />
                                                    </Click>
                                                </AjaxEvents>
                                            </ext:ToolbarButton>
                                            <ext:ToolbarSeparator ID="ToolbarSeparator2" runat="server">
                                            </ext:ToolbarSeparator>
                                            <ext:ToolbarButton ID="Btn_Del" runat="server" Text="删除" Icon="Delete" Disabled="true">
                                                <AjaxEvents>
                                                    <Click OnEvent="Btn_Del_Click">
                                                        <Confirmation Title="系统提示" BeforeConfirm="config.confirmation.message = '删除指标将会删除该指标所有相关的信息，<br/>确定要删除指标 '+GridPanel_List.getSelectionModel().getSelected().data.GUIDE_NAME+'('+GridPanel_List.getSelectionModel().getSelected().data.GUIDE_CODE+') 吗？';"
                                                            ConfirmRequest="true" />
                                                        <ExtraParams>
                                                            <ext:Parameter Name="Values" Value="GridPanel_List.getSelectionModel().getSelected().data.GUIDE_CODE"
                                                                Mode="Raw">
                                                            </ext:Parameter>
                                                        </ExtraParams>
                                                        <EventMask ShowMask="true" Msg="请稍候..." />
                                                    </Click>
                                                </AjaxEvents>
                                            </ext:ToolbarButton>
                                            <ext:ToolbarSeparator ID="ToolbarSeparator1" runat="server">
                                            </ext:ToolbarSeparator>
                                            <ext:ToolbarButton ID="Btn_Sql" runat="server" Text="指标算法" Icon="ChartCurve" Disabled="true">
                                                <AjaxEvents>
                                                    <Click OnEvent="Btn_Sql_Click">
                                                        <ExtraParams>
                                                            <ext:Parameter Name="Values" Value="Ext.encode(#{GridPanel_List}.getRowsValues())"
                                                                Mode="Raw">
                                                            </ext:Parameter>
                                                        </ExtraParams>
                                                        <EventMask ShowMask="true" Msg="请稍候..." />
                                                    </Click>
                                                </AjaxEvents>
                                            </ext:ToolbarButton>
                                            <ext:ToolbarSeparator ID="ToolbarSeparator3" runat="server">
                                            </ext:ToolbarSeparator>
                                            <ext:ToolbarButton ID="Btn_Refresh" runat="server" Text="刷新" Icon="ArrowRefresh">
                                                <Listeners>
                                                    <Click Handler="#{Store1}.reload();" />
                                                </Listeners>
                                            </ext:ToolbarButton>
                                            <ext:ToolbarSeparator ID="ToolbarSeparator4" runat="server">
                                            </ext:ToolbarSeparator>
                                        </Items>
                                    </ext:Toolbar>
                                </TopBar>
                                <ColumnModel ID="ColumnModel1" runat="server">
                                    <Columns>
                                        <ext:Column Header="指标代码" Width="70" ColumnID="GUIDE_CODE" DataIndex="GUIDE_CODE" />
                                        <ext:Column Header="BSC一级分类" Width="98" ColumnID="BSC_CLASS_NAME1" DataIndex="BSC_CLASS_NAME1" />
                                        <ext:Column Header="BSC二级分类" Width="98" ColumnID="BSC_CLASS_NAME2" DataIndex="BSC_CLASS_NAME2" />
                                        <ext:Column Header="指标名称" Width="100" ColumnID="GUIDE_NAME" DataIndex="GUIDE_NAME">
                                            <Renderer Fn="FormatRenderNew" />
                                        </ext:Column>
                                        <ext:Column Header="指标表达式" Width="130" ColumnID="GUIDE_EXPRESS" DataIndex="GUIDE_EXPRESS" />
                                        <ext:Column Header="是否<br/>表达式" Width="50" ColumnID="ISEXPRESS" DataIndex="ISEXPRESS" />
                                        <ext:Column Header="是否<br/>启用" Width="50" ColumnID="ISPAGE" DataIndex="ISPAGE" />
                                        <ext:Column Header="指标<br/>算法" Width="50" ColumnID="GUIDE_SQL" DataIndex="GUIDE_SQL" />
                                        <ext:Column Header="部门" Width="80" ColumnID="DEPT_CLASS_NAME" DataIndex="DEPT_CLASS_NAME" />
                                        <ext:Column Header="组织" Width="36" ColumnID="ORGAN_CLASS_NAME" DataIndex="ORGAN_CLASS_NAME" />
                                        <ext:Column Header="指标类型" Width="70" ColumnID="INDEXTYPE" DataIndex="INDEXTYPE" />
                                        <ext:Column Header="显示顺序" Width="36" ColumnID="SHOWNUM" DataIndex="SHOWNUM" />
                                        <ext:Column Header="说明" Width="160" ColumnID="EXPLAIN" DataIndex="EXPLAIN" />
                                    </Columns>
                                </ColumnModel>
                                <AjaxEvents>
                                    <RowDblClick OnEvent="Btn_Edit_Click">
                                        <EventMask Msg="载入中..." ShowMask="true" />
                                        <ExtraParams>
                                            <ext:Parameter Name="Values" Value="Ext.encode(#{GridPanel_List}.getRowsValues())"
                                                Mode="Raw">
                                            </ext:Parameter>
                                        </ExtraParams>
                                    </RowDblClick>
                                </AjaxEvents>
                                <SelectionModel>
                                    <ext:RowSelectionModel ID="rowselection" SingleSelect="true">
                                        <Listeners>
                                            <SelectionChange Handler="var tmpflg= #{GridPanel_List}.hasSelection()?false:true; #{Btn_Edit}.setDisabled(tmpflg); #{Btn_Del}.setDisabled(tmpflg); tmpflg= true; if( #{GridPanel_List}.hasSelection()){tmpflg = GridPanel_List.getRowsValues(true)[0].ISEXPRESS=='否'? false:true } #{Btn_Sql}.setDisabled(tmpflg);" />
                                        </Listeners>
                                    </ext:RowSelectionModel>
                                </SelectionModel>
                                <BottomBar>
                                    <ext:PagingToolbar ID="PagingToolBar2" runat="server" PageSize="20" StoreID="Store1"
                                        AutoWidth="true" DisplayInfo="true" AutoDataBind="true">
                                        <Items>
                                            <ext:TextField ID="txt_SearchTxt" runat="server" EmptyText="查找指标">
                                                <ToolTips>
                                                    <ext:ToolTip ID="ToolTip1" runat="server" Html="根据指标代码、名称、分类、科室、启用等关键字查找">
                                                    </ext:ToolTip>
                                                </ToolTips>
                                            </ext:TextField>
                                            <ext:Button ID="btn_Search" Icon="FolderMagnify" runat="server" Text="查询">
                                                <Listeners>
                                                    <Click Fn="applyFilter" />
                                                </Listeners>
                                            </ext:Button>
                                        </Items>
                                    </ext:PagingToolbar>
                                </BottomBar>
                                <LoadMask ShowMask="true" Msg="载入中..." />
                            </ext:GridPanel>
                        </ext:LayoutColumn>
                    </Columns>
                </ext:ColumnLayout>
            </Body>
        </ext:ViewPort>
        <ext:Window ID="DetailWin" runat="server" Icon="ApplicationViewIcons" Title="详细信息"
            Width="1024" Closable="true" CloseAction="Hide" Height="460" AutoShow="false"
            Modal="true" AutoScroll="true" CenterOnLoad="true" ShowOnLoad="false" Resizable="false"
            StyleSpec="background-color:Transparent;" BodyStyle="background-color:Transparent;">
            <Listeners>
                <Hide Handler="this.clearContent();" />
                <BeforeShow Handler=" var height = Ext.getBody().getViewSize().height; var width = Ext.getBody().getViewSize().width; if (el.getSize().height > height) {  el.setHeight(height - 20) } ;if (el.getSize().width > width) {  el.setWidth(width - 20) }  " />
            </Listeners>
        </ext:Window>
    </div>
    </form>
</body>
</html>
