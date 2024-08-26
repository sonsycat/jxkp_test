<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Guide_Dict.aspx.cs" Inherits="GoldNet.JXKP.jxkh.Guide_Dict" %>

<%@ Register Assembly="Goldnet.Ext.Web" Namespace="Goldnet.Ext.Web" TagPrefix="ext" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>指标列表</title>

    <script type="text/javascript">
        var applyFilter = function() { 
            Store1.filterBy(getRecordFilter());
        };
        var getRecordFilter=function(){
            var f=[];
            f.push({
                filter:function(record){
                return filterString(txt_SearchTxt.getValue(), 'GUIDE_NAME', record);
                }
            });
            f.push({
                filter: function(record) {
                return filterString(txt_SearchTxt.getValue(), 'DEPT_CLASS_NAME', record);
                }
            });
            f.push({
                filter: function(record) {
                return filterString(txt_SearchTxt.getValue(), 'BSC_CLASS_NAME2', record);
                }
            });
            f.push({
                filter: function(record) {
                    return filterString(txt_SearchTxt.getValue(), 'GUIDE_CODE', record);
                }
            });
            f.push({
                filter: function(record) {
                return filterString(txt_SearchTxt.getValue(), 'ISPAGE', record);
                }
            });

            var len=f.length;
            return function(record){
            if (f[0].filter(record) || f[1].filter(record) || f[2].filter(record) || f[3].filter(record) || f[4].filter(record)) {
                    return true; 
                }
                return false;
            }
        };
         var filterString=function(value,dataIndex,record){
            var val=record.get(dataIndex);
            if(typeof val!="string"){
                return value.length==0;
            }    
            return val.toLowerCase().indexOf(value.toLowerCase())>-1;
        };
        var filterNumber=function(value,dataIndex,record){
            var val=record.get(dataIndex);            
             if(!Ext.isEmpty(value,false) && val!=value){
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
        function FormatRenderNewSQL(v, p, record, rowIndex) {
            var newFlg = false;
            if (record.data.ISPAGE == '启用' && record.data.ISEXPRESS == '否' && record.data.GUIDE_SQL == '无')
                newFlg = true;
            var template = '<span style="font-style:italic;font-weight:bolder;">{0}</span>';
            return newFlg ? String.format(template, record.data.GUIDE_SQL) : record.data.GUIDE_SQL;
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
                        <ext:RecordField Name="THRESHOLD_RATIO" />
                        <ext:RecordField Name="FIXNUM" />
                        <ext:RecordField Name="EXPLAIN" />
                        <ext:RecordField Name="SORT_NO" />
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
                                            <ext:ToolbarButton ID="Btn_Sql" runat="server" Text="指标算法" Icon="ChartCurve" >
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
                                            <ext:ToolbarSeparator ID="ToolbarSeparator6" runat="server">
                                            </ext:ToolbarSeparator>
                                            <ext:ToolbarButton ID="Btn_Create_Open" runat="server" Text="全部指标生成" Icon="Bricks">
                                                <AjaxEvents>
                                                    <Click OnEvent="CreateStationGuide" Timeout="9000000">
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
                                        <ext:Column Header="指标代码" Width="66" ColumnID="GUIDE_CODE" DataIndex="GUIDE_CODE" />
                                        <ext:Column Header="BSC一级分类" Width="98" ColumnID="BSC_CLASS_NAME1" DataIndex="BSC_CLASS_NAME1" />
                                        <ext:Column Header="BSC二级分类" Width="98" ColumnID="BSC_CLASS_NAME2" DataIndex="BSC_CLASS_NAME2" />
                                        <ext:Column Header="指标名称" Width="120" ColumnID="GUIDE_NAME" DataIndex="GUIDE_NAME">
                                            <Renderer Fn="FormatRenderNew" />
                                        </ext:Column>
                                        <ext:Column Header="指标表达式" Width="130" ColumnID="GUIDE_EXPRESS" DataIndex="GUIDE_EXPRESS" />
                                        <ext:Column Header="是否<br/>表达式" Width="50" ColumnID="ISEXPRESS" DataIndex="ISEXPRESS" />
                                        <ext:Column Header="是否<br/>启用" Width="50" ColumnID="ISPAGE" DataIndex="ISPAGE" />
                                        <ext:Column Header="是否高<br/>优指标" Width="50" ColumnID="ISHIGHGUIDE" DataIndex="ISHIGHGUIDE" />
                                        <ext:Column Header="综合评<br/>价选择" Width="50" ColumnID="ISSEL" DataIndex="ISSEL" />
                                        <ext:Column Header="是否<br/>绝对值" Width="50" ColumnID="ISABS" DataIndex="ISABS" />
                                        <ext:Column Header="指标<br/>算法" Width="50" ColumnID="GUIDE_SQL" DataIndex="GUIDE_SQL">
                                            <Renderer Fn="FormatRenderNewSQL" />
                                        </ext:Column>
                                        <ext:Column Header="部门" Width="80" ColumnID="DEPT_CLASS_NAME" DataIndex="DEPT_CLASS_NAME" />
                                        <ext:Column Header="组织" Width="36" ColumnID="ORGAN_CLASS_NAME" DataIndex="ORGAN_CLASS_NAME" />
                                        <ext:Column Header="阀值比" Width="36" ColumnID="THRESHOLD_RATIO" DataIndex="THRESHOLD_RATIO" />
                                        <ext:Column Header="是否固定分值" Width="36" ColumnID="FIXNUM" DataIndex="FIXNUM" Hidden="true" />
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
                                            <SelectionChange Handler="var tmpflg= #{GridPanel_List}.hasSelection()?false:true; #{Btn_Edit}.setDisabled(tmpflg); #{Btn_Del}.setDisabled(tmpflg); tmpflg= true; if( #{GridPanel_List}.hasSelection()){tmpflg = GridPanel_List.getRowsValues(true)[0].ISEXPRESS=='否'? false:true } #{Btn_Sql}.setDisabled(false);" />
                                        </Listeners>
                                    </ext:RowSelectionModel>
                                </SelectionModel>
                                <BottomBar>
                                    <ext:PagingToolbar ID="PagingToolBar2" runat="server" PageSize="20" StoreID="Store1"
                                        AutoWidth="true" DisplayInfo="true" AutoDataBind="true">
                                        <Items>
                                            <ext:TextField ID="txt_SearchTxt" runat="server" EmptyText="查找指标">
                                                <ToolTips>
                                                    <ext:ToolTip runat="server" Html="根据指标代码、名称、分类、科室、启用等关键字查找">
                                                    </ext:ToolTip>
                                                </ToolTips>
                                            </ext:TextField>
                                            <ext:Button ID="btn_Search" Icon="Zoom" runat="server" Text="查询">
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
            Width="1024" Closable="true" CloseAction="Hide" Height="600" AutoShow="false"
            Modal="true" AutoScroll="false" CenterOnLoad="true" ShowOnLoad="false" Resizable="false"
            StyleSpec="background-color:Transparent;" BodyStyle="background-color:Transparent;">
            <Listeners>
                <Hide Handler="this.clearContent();" />
                <BeforeShow Handler=" var height = Ext.getBody().getViewSize().height; var width = Ext.getBody().getViewSize().width; if (el.getSize().height > height) {  el.setHeight(height - 20) } ;if (el.getSize().width > width) {  el.setWidth(width - 20) }  " />
            </Listeners>
        </ext:Window>
        <ext:Window ID="Win_BatchInit" runat="server" Icon="Group" Title="指标生成" Width="345"
            Height="180" AutoShow="false" Modal="true" CenterOnLoad="true" ShowOnLoad="false"
            Resizable="false" StyleSpec="background-color:Transparent;" BodyStyle="background-color:Transparent;"
            AutoScroll="true">
            <Body>
                <ext:FormPanel ID="FormPanel1" runat="server" Border="false" MonitorValid="true"
                    ButtonAlign="Right" BodyStyle="background-color:transparent;">
                    <Body>
                        <table>
                            <tr>
                                <td colspan="2" align="left">
                                    <p>
                                        注意：生成数据需要大约2分钟时间，在此时间内请不要关闭您的浏览器或者刷新页面。</p>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    起始日期:
                                </td>
                                <td align="left">
                                    <ext:MultiField ID="MultiField1" runat="server" FieldLabel="起始日期">
                                        <Fields>
                                            <ext:DateField ID="start_date" runat="server" DataIndex="ACCOUNTING_DATE" AllowBlank="false"
                                                Width="180" Format="yyyy-MM-dd" EnableKeyEvents="true" />
                                        </Fields>
                                    </ext:MultiField>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    结束日期:
                                </td>
                                <td align="left">
                                    <ext:MultiField ID="MultiField2" runat="server" FieldLabel="结束日期">
                                        <Fields>
                                            <ext:DateField ID="end_date" runat="server" AllowBlank="false" Width="180" Format="yyyy-MM-dd"
                                                EnableKeyEvents="true" />
                                        </Fields>
                                    </ext:MultiField>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <p>
                                        <ext:Label runat="server" ID="progressTip" Text="进度" AutoWidth="true">
                                        </ext:Label>
                                    </p>
                                    <ext:ProgressBar ID="Progress1" runat="server" Width="260">
                                    </ext:ProgressBar>
                                </td>
                            </tr>
                        </table>
                        <ext:TaskManager ID="TaskManager1" runat="server">
                            <Tasks>
                                <ext:Task TaskID="longactionprogress" Interval="1000" AutoRun="false" OnStart="#{Btn_Create}.setDisabled(true); "
                                    OnStop="#{Btn_BatStart}.setDisabled(false);">
                                    <AjaxEvents>
                                        <Update OnEvent="RefreshProgress" />
                                    </AjaxEvents>
                                </ext:Task>
                            </Tasks>
                        </ext:TaskManager>
                    </Body>
                    <BottomBar>
                        <ext:Toolbar ID="Toolbar2" runat="server">
                            <Items>
                                <ext:ToolbarFill ID="ToolbarFill2" runat="server" />
                                <ext:ToolbarButton ID="Btn_Create" runat="server" Icon="Bricks" Text="开始生成">
                                    <AjaxEvents>
                                        <Click OnEvent="Btn_Create_Click" Timeout="1200000">
                                            <Confirmation ConfirmRequest="true" Title="系统提示" Message="全部指标生成数据,大约耗时2分钟,<br/>是否继续?" />
                                        </Click>
                                    </AjaxEvents>
                                </ext:ToolbarButton>
                                <ext:ToolbarSeparator ID="ToolbarSeparator7" runat="server" />
                                <ext:ToolbarButton ID="cancel" runat="server" Icon="Cancel" Text="退出">
                                    <Listeners>
                                        <Click Handler="Win_BatchInit.hide();" />
                                    </Listeners>
                                </ext:ToolbarButton>
                            </Items>
                        </ext:Toolbar>
                    </BottomBar>
                    <Listeners>
                        <Show Handler="this.dirty = false;" />
                        <BeforeHide Handler="
                    if ((this.dirty==false)&&(cancel.text=='退出') ){
                        Ext.Msg.confirm('系统提示', '注意:任务正在运行，确定取消任务并退出吗？', function (btn) { 
                            if(btn == 'yes') { 
                                this.dirty = true;
                                this.hide(); 
                            } 
                        }, this);
                        return false;    
                    }" />
                        <Hide Handler="TaskManager1.stopAll();" />
                    </Listeners>
                    <AjaxEvents>
                        <Hide OnEvent="CloseBatInit" />
                    </AjaxEvents>
                </ext:FormPanel>
            </Body>
        </ext:Window>
    </div>
    </form>
</body>
</html>
