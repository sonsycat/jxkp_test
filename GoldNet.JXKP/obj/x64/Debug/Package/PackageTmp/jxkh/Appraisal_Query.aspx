<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Appraisal_Query.aspx.cs"
    Inherits="GoldNet.JXKP.jxkh.Appraisal_Query" %>

<%@ Register Assembly="Goldnet.Ext.Web" Namespace="Goldnet.Ext.Web" TagPrefix="ext" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>评价综合查询</title>

    <script type="text/javascript">
        function FormatRenderNew(v, p, record, rowIndex) {
            var cnt = Number(record.data.ARCHIVE_TAGS.toString());
            var template = '<span style="font-style:italic;font-weight:bolder;">{0}</span>';
            return cnt == 0 ? String.format(template, record.data.EVALUATE_NAME) : record.data.EVALUATE_NAME;
        }
    </script>

</head>
<body>
    <form id="form1" runat="server">
    <div>
        <ext:ScriptManager ID="ScriptManager1" runat="server">
            <Listeners>
                <DocumentReady Handler="CombEvalYear.setWidth(60); CombArchFlg.setWidth(60);CombEvalType.setWidth(150);" />
            </Listeners>
        </ext:ScriptManager>
        <ext:Store runat="server" ID="Store1" AutoLoad="true" OnRefreshData="Store_RefreshData">
            <Reader>
                <ext:JsonReader ReaderID="EVALUATE_CODE">
                    <Fields>
                        <ext:RecordField Name="EVALUATE_CODE" />
                        <ext:RecordField Name="EVALUATE_CLASS_NAME" />
                        <ext:RecordField Name="EVALUATE_NAME" />
                        <ext:RecordField Name="START_DATE" />
                        <ext:RecordField Name="END_DATE" />
                        <ext:RecordField Name="EVALUATE_TIME" />
                        <ext:RecordField Name="EVALUATE_APPRAISER" />
                        <ext:RecordField Name="EVALUATE_DESCRIPTION" />
                        <ext:RecordField Name="ORG_TYPE" />
                        <ext:RecordField Name="IS_EVLUATE_BONUS" />
                        <ext:RecordField Name="ARCHIVE_TAGS" />
                    </Fields>
                </ext:JsonReader>
            </Reader>
        </ext:Store>
        <ext:Store runat="server" ID="Store2">
            <Reader>
                <ext:JsonReader ReaderID="EVALUATE_CLASS_CODE">
                    <Fields>
                        <ext:RecordField Name="EVALUATE_CLASS_CODE" />
                        <ext:RecordField Name="EVALUATE_CLASS_NAME" />
                        <ext:RecordField Name="STATION_TYPE" />
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
                                StripeRows="true" TrackMouseOver="true" Height="480" AutoWidth="true" AutoExpandColumn="EVALUATE_DESCRIPTION">
                                <TopBar>
                                    <ext:Toolbar ID="Toolbar_1" runat="server" Visible="true" AutoWidth="true">
                                        <Items>
                                            <ext:Label ID="func" runat="server" Text="评价年度：" />
                                            <ext:ComboBox ID="CombEvalYear" runat="server" ReadOnly="true">
                                            </ext:ComboBox>
                                            <ext:Label ID="Label1" runat="server" Text="评价类别：" />
                                            <ext:ComboBox ID="CombEvalType" runat="server" ReadOnly="true" StoreID="Store2" DisplayField="EVALUATE_CLASS_NAME"
                                                ValueField="EVALUATE_CLASS_CODE" />
                                            <ext:ComboBox ID="CombArchFlg" runat="server" ReadOnly="true">
                                                <Items>
                                                    <ext:ListItem Text="已保存" Value="0" />
                                                    <ext:ListItem Text="已归档" Value="1" />
                                                </Items>
                                            </ext:ComboBox>
                                            <ext:ToolbarSeparator ID="ToolbarSeparator4" runat="server" />
                                            <ext:ToolbarButton ID="Btn_Search" runat="server" Text="查询" Icon="DatabaseGo">
                                                <AjaxEvents>
                                                    <Click OnEvent="Btn_Search_Click">
                                                        <EventMask ShowMask="true" Msg="请稍候..." />
                                                    </Click>
                                                </AjaxEvents>
                                            </ext:ToolbarButton>
                                            <ext:ToolbarSeparator ID="ToolbarSeparator1" runat="server" />
                                        </Items>
                                    </ext:Toolbar>
                                </TopBar>
                                <ColumnModel ID="ColumnModel1" runat="server">
                                    <Columns>
                                        <ext:Column Header="编码" Width="40" ColumnID="EVALUATE_CODE" Fixed="true" DataIndex="EVALUATE_CODE" />
                                        <ext:Column Header="评价分类" Width="120" ColumnID="EVALUATE_CLASS_NAME" DataIndex="EVALUATE_CLASS_NAME" />
                                        <ext:Column Header="评价名称" Width="150" ColumnID="EVALUATE_NAME" DataIndex="EVALUATE_NAME">
                                            <Renderer Fn="FormatRenderNew" />
                                        </ext:Column>
                                        <ext:Column Header="开始月份" Width="100" ColumnID="START_DATE" DataIndex="START_DATE" />
                                        <ext:Column Header="结束月份" Width="100" ColumnID="END_DATE" DataIndex="END_DATE" />
                                        <ext:Column Header="评价时间" Width="100" ColumnID="EVALUATE_TIME" DataIndex="EVALUATE_TIME" />
                                        <ext:Column Header="评价人" Width="100" ColumnID="EVALUATE_APPRAISER" DataIndex="EVALUATE_APPRAISER" />
                                        <ext:Column Header="参与绩效奖励" Width="100" Align="Center" ColumnID="IS_EVLUATE_BONUS"
                                            DataIndex="IS_EVLUATE_BONUS" />
                                        <ext:Column Header="简述" Width="150" ColumnID="EVALUATE_DESCRIPTION" DataIndex="EVALUATE_DESCRIPTION" />
                                        <ext:CommandColumn Width="40" Align="Left" Header="详情">
                                            <Commands>
                                                <ext:GridCommand Icon="FolderTable" CommandName="Appraisal_Detail">
                                                    <ToolTip Text="查看评价详情" />
                                                </ext:GridCommand>
                                            </Commands>
                                        </ext:CommandColumn>
                                    </Columns>
                                </ColumnModel>
                                <SelectionModel>
                                    <ext:RowSelectionModel ID="rowselection" SingleSelect="true">
                                    </ext:RowSelectionModel>
                                </SelectionModel>
                                <BottomBar>
                                    <ext:PagingToolbar ID="PagingToolBar1" runat="server" PageSize="20" StoreID="Store1"
                                        AutoWidth="true" DisplayInfo="true" AutoDataBind="true">
                                    </ext:PagingToolbar>
                                </BottomBar>
                                <LoadMask ShowMask="true" />
                                <AjaxEvents>
                                    <Command OnEvent="Appraisal_Detail_Show" Before=" if (command !='Appraisal_Detail') return false; ">
                                        <ExtraParams>
                                            <ext:Parameter Name="Values" Value="Ext.encode(#{GridPanel_List}.getRowsValues())"
                                                Mode="Raw">
                                            </ext:Parameter>
                                        </ExtraParams>
                                        <EventMask Msg="载入中..." ShowMask="true" />
                                    </Command>
                                    <RowDblClick OnEvent="Appraisal_Detail_Show">
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
        <ext:Window ID="DetailWin" runat="server" Icon="ApplicationViewIcons" Title="评价详情"
            Width="1024" Closable="true" CloseAction="Hide" Maximizable="true" Height="700"
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
