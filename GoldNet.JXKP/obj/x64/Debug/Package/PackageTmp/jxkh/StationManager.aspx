<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="StationManager.aspx.cs"
    Inherits="GoldNet.JXKP.jxkh.StationManager" %>

<%@ Register Assembly="Goldnet.Ext.Web" Namespace="Goldnet.Ext.Web" TagPrefix="ext" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>岗位管理</title>

    <script type="text/javascript">
        var gridCommand = function(command, record) {

        if (command == 'CmdJXPC') {
            Ext.Msg.confirm('系统提示', '注意：生成岗位测评数据需要大约2分钟时间，确实要进行吗？', function(btn) {
                    if (btn == 'yes') {
                        GridPanel1.el.mask('载入中...', 'x-loading-mask');
                        Goldnet.AjaxMethod.request(
                          'ShowDetailWindow',
                            {
                                params: {
                                    command: command,
                                    stationcode: record.data.STATION_CODE,
                                    guidegathercode: record.data.GUIDE_GATHER_CODE,
                                    stationname: record.data.STATION_NAME,
                                    deptcode: record.data.DEPT_CODE,
                                    deptname: record.data.DEPT_NAME
                                },
                                success: function(result) {
                                    GridPanel1.el.unmask();
                                },
                                failure: function(msg) {
                                    GridPanel1.el.unmask();
                                }
                            });
                    }
                }, this);
                return ;    
            }
            GridPanel1.el.mask('载入中...', 'x-loading-mask');
            Goldnet.AjaxMethod.request(
              'ShowDetailWindow',
                {
                    params: {
                        command: command,
                        stationcode: record.data.STATION_CODE,
                        guidegathercode: record.data.GUIDE_GATHER_CODE,
                        stationname: record.data.STATION_NAME,
                        deptcode: record.data.DEPT_CODE,
                        deptname: record.data.DEPT_NAME
                    },
                    success: function(result) {
                        GridPanel1.el.unmask();
                    },
                    failure: function(msg) {
                        GridPanel1.el.unmask();
                    }
                });
        }
        var refreshGrid = function(node) {
            if ((node.id.indexOf('CLASS') < 0) && (node.id != 'root')) {
                GridPanel1.setTitle(node.text);
                DeptCodeHidden.setValue(node.id);
                GridPanel1.el.mask('数据刷新中...', 'x-loading-mask');
                Goldnet.AjaxMethod.request(
                  'GridPanelRefresh',
                    {
                        params: {
                            DeptCode: node.id
                        },
                        success: function(result) {
                            GridPanel1.el.unmask();
                        },
                        failure: function(msg) {
                            GridPanel1.el.unmask();
                        }
                    });
            }
        }
        var RefreshData = function() {
            Store1.reload();
        }
        var RefreshData2 = function() {
            Store2.reload();
        }
        var prepare = function(grid, toolbar, rowIndex, record) {
            var menuButton = toolbar.items.get(0);
            var menu1 = menuButton.menu.items.get(1);
            var menu3 = menuButton.menu.items.get(3);
            if (record.data.GUIDE_GATHER_CODE == null || record.data.GUIDE_GATHER_CODE == "") {
                menu1.setDisabled(true);
                menu3.setDisabled(true);
            }
        } 
    </script>

    <style type="text/css">
        .icon-expand-all
        {
            background-image: url(/resources/images/expand-all.gif) !important;
        }
        .icon-collapse-all
        {
            background-image: url(/resources/images/collapse-all.gif) !important;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <ext:ScriptManager ID="ScriptManager1" runat="server" />
        <ext:Store runat="server" ID="Store1" AutoLoad="true" OnRefreshData="Store_RefreshData">
            <Reader>
                <ext:JsonReader ReaderID="STATION_CODE">
                    <Fields>
                        <ext:RecordField Name="STATION_CODE" />
                        <ext:RecordField Name="STATION_NAME" />
                        <ext:RecordField Name="STATION_CODE_REMARK" />
                        <ext:RecordField Name="DEPT_NAME" />
                        <ext:RecordField Name="INPUT_USER" />
                        <ext:RecordField Name="INPUT_TIME" />
                        <ext:RecordField Name="STATION_YEAR" />
                        <ext:RecordField Name="DEPT_CODE" />
                        <ext:RecordField Name="GUIDE_GATHER_CODE" />
                    </Fields>
                </ext:JsonReader>
            </Reader>
        </ext:Store>
        <ext:Store runat="server" ID="Store2" OnRefreshData="Store2_RefreshData">
            <Reader>
                <ext:JsonReader ReaderID="STATION_CODE">
                    <Fields>
                        <ext:RecordField Name="STATION_CODE" />
                        <ext:RecordField Name="STATION_NAME" />
                        <ext:RecordField Name="DEPT_CODE" />
                        <ext:RecordField Name="DEPT_NAME" />
                        <ext:RecordField Name="GUIDE_GATHER_CODE" />
                        <ext:RecordField Name="STATION_YEAR" />
                    </Fields>
                </ext:JsonReader>
            </Reader>
        </ext:Store>
        <ext:Window runat="server" ID="Win_BatchInit" AutoShow="false" ShowOnLoad="false"
            Modal="true" Resizable="false" Title="年度批量指标量化" CenterOnLoad="true" AutoScroll="false"
            Width="280" Height="180" CloseAction="Hide" AnimateTarget="Btn_BatInit" Icon="TagPink"
            BodyStyle="padding:2px;">
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
                            目标值参照年份:
                        </td>
                        <td align="left">
                            <ext:ComboBox runat="server" ID="Combo_TargetYear" FieldLabel="目标值参照年份" Width="90"
                                AllowBlank="false" Editable="false">
                            </ext:ComboBox>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <p>
                                <ext:Label runat="server" ID="progressTip" Text="进度" AutoWidth="true">
                                </ext:Label>
                            </p>
                            <ext:ProgressBar ID="Progress1" runat="server" Width="255">
                            </ext:ProgressBar>
                        </td>
                    </tr>
                </table>
                <ext:TaskManager ID="TaskManager1" runat="server">
                    <Tasks>
                        <ext:Task TaskID="longactionprogress" Interval="1000" AutoRun="false" OnStart="#{Btn_BatStart}.setDisabled(true); "
                            OnStop="#{Btn_BatStart}.setDisabled(false);">
                            <AjaxEvents>
                                <Update OnEvent="RefreshProgress" />
                            </AjaxEvents>
                        </ext:Task>
                    </Tasks>
                </ext:TaskManager>
            </Body>
            <BottomBar>
                <ext:Toolbar ID="Toolbar1" runat="server">
                    <Items>
                        <ext:ToolbarFill ID="ToolbarFill2" runat="server" />
                        <ext:ToolbarButton ID="Btn_BatStart" runat="server" Icon="PlayGreen" Text="开始生成">
                            <AjaxEvents>
                                <Click OnEvent="Btn_BatStart_Click" Timeout="1200000">
                                </Click>
                            </AjaxEvents>
                        </ext:ToolbarButton>
                        <ext:ToolbarSeparator ID="ToolbarSeparator3" runat="server" />
                        <ext:ToolbarButton ID="Btn_BatCancel" runat="server" Icon="Cancel" Text="退出">
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
                    if ((this.dirty==false)&&(Btn_BatCancel.text=='取消') ){
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
        </ext:Window>
        <ext:ViewPort ID="ViewPort1" runat="server">
            <Body>
                <ext:BorderLayout ID="BorderLayout1" runat="server">
                    <North>
                        <ext:Toolbar ID="Toolbar_detptype" runat="server" Visible="true" AutoWidth="true">
                            <Items>
                                <ext:ToolbarButton ID="Btn_CreateStation" runat="server" Text="建立岗位" Icon="Add">
                                    <AjaxEvents>
                                        <Click OnEvent="Btn_CreateStation_Click" Timeout="120000">
                                            <Confirmation BeforeConfirm="config.confirmation.message = '注意:您将要建立并更新'+Combo_StationYear.value+'年度的岗位。<br/>确定要建立岗位吗？';"
                                                Title="系统提示" ConfirmRequest="true" />
                                            <EventMask Msg="建立中.." ShowMask="true" />
                                        </Click>
                                    </AjaxEvents>
                                </ext:ToolbarButton>
                                <ext:ToolbarSeparator ID="ToolbarSeparator4" runat="server" />
                                <ext:ToolbarButton ID="Btn_PersonStation" runat="server" Text="人员进岗" Icon="UserHome">
                                    <AjaxEvents>
                                        <Click OnEvent="Btn_PersonStation_Click" Timeout="120000">
                                            <Confirmation BeforeConfirm="config.confirmation.message = '注意:您将要添加'+Combo_StationYear.value+'年的岗位下的人员。<br/>这样将会删除之前岗位下的所有人员再重新添加。<br/>确定要添加人员吗?';"
                                                Title="系统提示" ConfirmRequest="true" />
                                            <EventMask Msg="建立中.." ShowMask="true" />
                                        </Click>
                                    </AjaxEvents>
                                </ext:ToolbarButton>
                                <ext:ToolbarSeparator ID="ToolbarSeparator1" runat="server" />
                                <ext:ToolbarButton ID="Btn_BatInit" runat="server" Text="批量指标量化" Icon="TagPink">
                                    <AjaxEvents>
                                        <Click OnEvent="Btn_BatInit_Click">
                                        </Click>
                                    </AjaxEvents>
                                </ext:ToolbarButton>
                                <ext:ToolbarSeparator ID="ToolbarSeparator2" runat="server" />
                                <ext:ToolbarButton ID="Btn_DelStation" runat="server" Text="删除岗位" Icon="Delete" Disabled="true">
                                    <AjaxEvents>
                                        <Click OnEvent="Btn_DelStation_Click">
                                            <Confirmation Message="注意:确定要删除选中的岗位数据吗?" Title="系统提示" ConfirmRequest="true" />
                                            <EventMask Target="CustomTarget" CustomTarget="GridPanel1" Msg="刷新中..." ShowMask="true" />
                                        </Click>
                                    </AjaxEvents>
                                </ext:ToolbarButton>
                                <ext:ToolbarFill ID="ToolbarFill1" Enabled="true" runat="server">
                                </ext:ToolbarFill>
                                <ext:Label ID="func" runat="server" Text="岗位年度：" Width="40" />
                                <ext:ComboBox ID="Combo_StationYear" runat="server" ReadOnly="true" AllowBlank="false"
                                    FieldLabel="岗位年度">
                                    <AjaxEvents>
                                        <Select OnEvent="Combo_StationYear_Selected">
                                            <EventMask ShowMask="true" Msg="刷新中..." />
                                        </Select>
                                    </AjaxEvents>
                                </ext:ComboBox>
                            </Items>
                        </ext:Toolbar>
                    </North>
                    <West MinWidth="200" CollapseMode="Mini" Split="false" Collapsible="false">
                        <ext:Panel runat="server" Width="200" BodyBorder="false" Title="科室组织树状图" AutoScroll="true">
                            <Body>
                                <ext:TreePanel ID="TreeCtrl" runat="server" Width="195" Height="350" Icon="BookOpen"
                                    BodyBorder="false" AutoHeight="true" AutoScroll="false" RootVisible="true">
                                    <Root>
                                        <ext:TreeNode NodeID="root" Text="岗位体系">
                                        </ext:TreeNode>
                                    </Root>
                                    <Listeners>
                                        <Click Handler="refreshGrid(node);" />
                                    </Listeners>
                                </ext:TreePanel>
                                <ext:Hidden ID="DeptCodeHidden" runat="server" Text="">
                                </ext:Hidden>
                            </Body>
                            <BottomBar>
                                <ext:Toolbar ID="ToolBar2" runat="server">
                                    <Items>
                                        <ext:ToolbarButton ID="ToolbarButton2" runat="server" Text="展开树" IconCls="icon-expand-all">
                                            <Listeners>
                                                <Click Handler="#{TreeCtrl}.root.expand(true);" />
                                            </Listeners>
                                            <ToolTips>
                                                <ext:ToolTip ID="ToolTip5" IDMode="Ignore" runat="server" Html="全部展开" />
                                            </ToolTips>
                                        </ext:ToolbarButton>
                                        <ext:ToolbarSeparator runat="server">
                                        </ext:ToolbarSeparator>
                                        <ext:ToolbarButton ID="ToolbarButton3" runat="server" Text="折叠树" IconCls="icon-collapse-all">
                                            <Listeners>
                                                <Click Handler="#{TreeCtrl}.root.collapse(true);" />
                                            </Listeners>
                                            <ToolTips>
                                                <ext:ToolTip ID="ToolTip6" IDMode="Ignore" runat="server" Html="全部收起" />
                                            </ToolTips>
                                        </ext:ToolbarButton>
                                    </Items>
                                </ext:Toolbar>
                            </BottomBar>
                        </ext:Panel>
                    </West>
                    <Center>
                        <ext:Panel runat="server" BodyBorder="true" Border="false">
                            <Body>
                                <ext:ColumnLayout ID="ColumnLayout1" runat="server" FitHeight="true">
                                    <Columns>
                                        <ext:LayoutColumn ColumnWidth="1">
                                            <ext:GridPanel ID="GridPanel1" Header="true" Title="岗位信息" runat="server" Border="false"
                                                StoreID="Store1" StripeRows="true" Height="480" AutoWidth="true" AutoExpandColumn="STATION_CODE_REMARK">
                                                <ColumnModel ID="ColumnModel1" runat="server">
                                                    <Columns>
                                                        <ext:Column Header="岗位名称" Width="100" ColumnID="STATION_NAME" DataIndex="STATION_NAME" />
                                                        <ext:Column Header="岗位说明" Width="180" ColumnID="STATION_CODE_REMARK" DataIndex="STATION_CODE_REMARK" />
                                                        <ext:Column Header="所属部门" Width="100" ColumnID="DEPT_NAME" DataIndex="DEPT_NAME" />
                                                        <ext:Column Header="录入人员" Width="90" ColumnID="INPUT_USER" DataIndex="INPUT_USER" />
                                                        <ext:Column Header="录入时间" Width="90" ColumnID="INPUT_TIME" DataIndex="INPUT_TIME" />
                                                        <ext:CommandColumn Width="38" Header="操作">
                                                            <Commands>
                                                                <ext:SplitCommand Icon="TableMultiple">
                                                                    <ToolTip Text="更多岗位操作" />
                                                                    <Menu>
                                                                        <Items>
                                                                            <ext:MenuCommand CommandName="CmdBJGW" Icon="Wrench" Text="编辑岗位信息">
                                                                            </ext:MenuCommand>
                                                                            <ext:MenuCommand CommandName="CmdZBLH" Icon="ShapeAlignBottom" Text="岗位指标量化">
                                                                            </ext:MenuCommand>
                                                                            <ext:MenuCommand CommandName="CmdXSRY" Icon="GroupGear" Text="岗位下属人员">
                                                                            </ext:MenuCommand>
                                                                            <ext:MenuCommand CommandName="CmdJXPC" Icon="ChartCurve" Text="绩效评测">
                                                                            </ext:MenuCommand>
                                                                        </Items>
                                                                    </Menu>
                                                                </ext:SplitCommand>
                                                            </Commands>
                                                            <PrepareToolbar Fn="prepare" />
                                                        </ext:CommandColumn>
                                                    </Columns>
                                                </ColumnModel>
                                                <Listeners>
                                                    <Command Handler=" gridCommand(command,record);" />
                                                    <RowDblClick Handler=" var record = GridPanel1.getStore().getAt(rowIndex); gridCommand('CmdBJGW',record);" />
                                                </Listeners>
                                                <SelectionModel>
                                                    <ext:CheckboxSelectionModel ID="CheckboxSelectionModel1" runat="server">
                                                        <Listeners>
                                                            <RowSelect Handler="#{Btn_DelStation}.enable()" />
                                                            <RowDeselect Handler="if (!#{GridPanel1}.hasSelection()) {#{Btn_DelStation}.disable()}" />
                                                        </Listeners>
                                                    </ext:CheckboxSelectionModel>
                                                </SelectionModel>
                                                <BottomBar>
                                                    <ext:PagingToolbar ID="PagingToolBar1" runat="server" PageSize="20" StoreID="Store1"
                                                        AutoWidth="true" DisplayInfo="true" AutoDataBind="true">
                                                    </ext:PagingToolbar>
                                                </BottomBar>
                                                <LoadMask ShowMask="true" />
                                            </ext:GridPanel>
                                        </ext:LayoutColumn>
                                    </Columns>
                                </ext:ColumnLayout>
                            </Body>
                        </ext:Panel>
                    </Center>
                    <East MinWidth="200" MaxWidth="400" SplitTip="需要更新指标量化的岗位" Collapsible="true" Split="true">
                        <ext:Panel ID="Panel1" runat="server" Border="false" Width="240" Title="需要更新指标量化的岗位"
                            Collapsed="true">
                            <Tools>
                                <ext:Tool Type="Refresh" Qtip="刷新" Handler=" Store2.reload();" />
                            </Tools>
                            <Body>
                                <ext:GridPanel ID="GridPanel2" Header="false" runat="server" Border="false" StoreID="Store2"
                                    StripeRows="true" AutoHeight="true" AutoWidth="true" AutoScroll="true">
                                    <ColumnModel ID="ColumnModel2" runat="server">
                                        <Columns>
                                            <ext:Column Header="科室" Width="80" ColumnID="DEPT_NAME" DataIndex="DEPT_NAME" Sortable="false"
                                                MenuDisabled="true" />
                                            <ext:Column Header="岗位名称" Width="100" ColumnID="STATION_NAME" DataIndex="STATION_NAME"
                                                Sortable="false" MenuDisabled="true" />
                                            <ext:CommandColumn Width="38">
                                                <Commands>
                                                    <ext:GridCommand Icon="ArrowRefresh" CommandName="UpdateZBLH">
                                                        <ToolTip Text="更新该岗位指标量化" />
                                                    </ext:GridCommand>
                                                </Commands>
                                            </ext:CommandColumn>
                                        </Columns>
                                    </ColumnModel>
                                    <SelectionModel>
                                        <ext:RowSelectionModel ID="RowSelectionModel1" runat="server">
                                        </ext:RowSelectionModel>
                                    </SelectionModel>
                                    <Listeners>
                                        <Command Handler=" gridCommand('CmdZBLH',record);" />
                                    </Listeners>
                                    <LoadMask ShowMask="true" />
                                </ext:GridPanel>
                            </Body>
                            <BottomBar>
                                <ext:StatusBar runat="server" ID="TipStatus" Height="25">
                                </ext:StatusBar>
                            </BottomBar>
                        </ext:Panel>
                    </East>
                </ext:BorderLayout>
            </Body>
        </ext:ViewPort>
        <ext:Window ID="DetailWin" runat="server" Icon="ApplicationViewIcons" Title="详细信息"
            Width="1024" Closable="true" CloseAction="Hide" Maximizable="true" Height="800"
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
