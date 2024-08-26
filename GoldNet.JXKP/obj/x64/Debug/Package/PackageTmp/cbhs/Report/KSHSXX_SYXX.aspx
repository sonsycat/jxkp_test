<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="KSHSXX_SYXX.aspx.cs" Inherits="GoldNet.JXKP.cbhs.Report.KSHSXX_SYXX" %>

<%@ Register Assembly="Goldnet.Ext.Web" Namespace="Goldnet.Ext.Web" TagPrefix="ext" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>

    <script type="text/javascript">
    
        //下拉菜单命令执行
        var gridCommand = function(command, record) {

            GridPanel1.el.mask('载入中...', 'x-loading-mask');
            Goldnet.AjaxMethod.request(
              'ShowDetailWindow',
                {
                    params: {
                        command: command,
                        deptcode: record.data.ACCOUNT_DEPT_CODE,
                        deptname: record.data.ACCOUNT_DEPT_NAME
                    },
                    success: function(result) {
                        GridPanel1.el.unmask();
                    },
                    failure: function(msg) {
                        GridPanel1.el.unmask();
                    }
                });
        }
       
        //
        var RefreshData = function() {
            Store1.reload();
        }
        
        //
        var RefreshData2 = function() {
//            Store2.reload();
        }
        
        //
        var prepare = function(grid, toolbar, rowIndex, record) {
//            var menuButton = toolbar.items.get(0);
//            var menu1 = menuButton.menu.items.get(1);
//            var menu3 = menuButton.menu.items.get(3);
//            if (record.data.GUIDE_GATHER_CODE == null || record.data.GUIDE_GATHER_CODE == "") {
//                menu1.setDisabled(true);
//                menu3.setDisabled(true);
//            }
        }
        
        //数字格式化处理
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
                <ext:JsonReader ReaderID="ACCOUNT_DEPT_CODE">
                    <Fields>
                        <ext:RecordField Name="ACCOUNT_DEPT_CODE" />
                        <ext:RecordField Name="ACCOUNT_DEPT_NAME" />
                        <ext:RecordField Name="AA" />
                        <ext:RecordField Name="BB" />
                        <ext:RecordField Name="SRZE" />
                        <ext:RecordField Name="SSZSB" />
                        <ext:RecordField Name="JJZSB" />
                        <ext:RecordField Name="CC" />
                        <ext:RecordField Name="DD" />
                        <ext:RecordField Name="ZCZE" />
                        <ext:RecordField Name="ZCZSB" />
                        <ext:RecordField Name="JSY" />
                        <ext:RecordField Name="MSY" />
                        <ext:RecordField Name="MSYB" />
                    </Fields>
                </ext:JsonReader>
            </Reader>
        </ext:Store>
        <ext:ViewPort ID="ViewPort1" runat="server">
            <Body>
                <ext:ColumnLayout ID="ColumnLayout2" runat="server" FitHeight="true">
                    <Columns>
                        <ext:LayoutColumn ColumnWidth="1">
                            <ext:GridPanel ID="GridPanel1" runat="server" BodyBorder="false" AutoScroll="true"
                                Border="false" StoreID="Store1" StripeRows="true" TrackMouseOver="true" Height="480"
                                AutoWidth="true">
                                <TopBar>
                                    <ext:Toolbar ID="Toolbar1" runat="server">
                                        <Items>
                                            <ext:ComboBox runat="server" ID="Comb_StartYear" Width="60" ListWidth="60" SelectedIndex="0">
                                            </ext:ComboBox>
                                            <ext:Label ID="lYear" runat="server" Text="年">
                                            </ext:Label>
                                            <ext:ComboBox runat="server" ID="Comb_StartMonth" Width="40" ListWidth="50" SelectedIndex="0">
                                            </ext:ComboBox>
                                            <ext:Label ID="Label1" runat="server" Text="月 到：">
                                            </ext:Label>
                                            <ext:ComboBox ID="ccbYearTo" runat="server" Width="60" ListWidth="60" SelectedIndex="0">
                                            </ext:ComboBox>
                                            <ext:Label ID="Label2" runat="server" Text="年">
                                            </ext:Label>
                                            <ext:ComboBox ID="ccbMonthTo" runat="server" Width="40" ListWidth="50" SelectedIndex="0">
                                            </ext:ComboBox>
                                            <ext:Label ID="Label3" runat="server" Text="月">
                                            </ext:Label>
                                            <ext:ToolbarSeparator ID="ToolbarSeparator6" runat="server">
                                            </ext:ToolbarSeparator>
                                            <ext:Button ID="Btn_View" runat="server" Icon="Zoom" Text="查询">
                                                <AjaxEvents>
                                                    <Click OnEvent="Btn_View_Click" Timeout="9000000">
                                                        <EventMask ShowMask="true" Msg="请稍候..." />
                                                    </Click>
                                                </AjaxEvents>
                                            </ext:Button>
                                            <ext:ToolbarSeparator ID="ToolbarSeparator3" runat="server">
                                            </ext:ToolbarSeparator>
                                            <ext:Button ID="Btn_Excel" runat="server" Disabled="true" Icon="PageWhiteExcel" Text="EXCEL导出"
                                                OnClick="OutExcel" AutoPostBack="true">
                                            </ext:Button>
                                        </Items>
                                    </ext:Toolbar>
                                </TopBar>
                                <ColumnModel ID="ColumnModel1" runat="server">
                                    <Columns>
                                        <ext:Column Header="科室" Width="130" ColumnID="ACCOUNT_DEPT_NAME" DataIndex="ACCOUNT_DEPT_NAME">
                                        </ext:Column>
                                        <ext:Column Header="实际收入" Width="90" ColumnID="AA" DataIndex="AA">
                                            <Renderer Fn="rmbMoney" />
                                        </ext:Column>
                                        <ext:Column Header="计价收入" Width="90" ColumnID="BB" DataIndex="BB">
                                            <Renderer Fn="rmbMoney" />
                                        </ext:Column>
                                        <ext:Column Header="收入总额" Width="90" ColumnID="SRZE" DataIndex="SRZE">
                                            <Renderer Fn="rmbMoney" />
                                        </ext:Column>
                                        <ext:Column Header="实收总收比" Width="90" ColumnID="SSZSB" DataIndex="SSZSB">
                                            <Renderer Fn="rmbMoney" />
                                        </ext:Column>
                                        <ext:Column Header="计价总收比" Width="90" ColumnID="JJZSB" DataIndex="JJZSB">
                                            <Renderer Fn="rmbMoney" />
                                        </ext:Column>
                                        <ext:Column Header="实际支出" Width="90" ColumnID="CC" DataIndex="CC">
                                            <Renderer Fn="rmbMoney" />
                                        </ext:Column>
                                        <ext:Column Header="计价支出" Width="90" ColumnID="DD" DataIndex="DD">
                                            <Renderer Fn="rmbMoney" />
                                        </ext:Column>
                                        <ext:Column Header="支出总额" Width="90" ColumnID="ZCZE" DataIndex="ZCZE">
                                            <Renderer Fn="rmbMoney" />
                                        </ext:Column>
                                        <ext:Column Header="支出总收比" Width="90" ColumnID="ZCZSB" DataIndex="ZCZSB">
                                            <Renderer Fn="rmbMoney" />
                                        </ext:Column>
                                        <ext:Column Header="净收益" Width="90" ColumnID="JSY" DataIndex="JSY">
                                            <Renderer Fn="rmbMoney" />
                                        </ext:Column>
                                        <ext:Column Header="毛收益" Width="90" ColumnID="MSY" DataIndex="MSY">
                                            <Renderer Fn="rmbMoney" />
                                        </ext:Column>
                                        <ext:Column Header="净收益/毛收益" Width="90" ColumnID="MSYB" DataIndex="MSYB">
                                            <Renderer Fn="rmbMoney" />
                                        </ext:Column>
                                        <ext:CommandColumn Width="38" Header="操作">
                                            <Commands>
                                                <ext:SplitCommand Icon="TableMultiple">
                                                    <ToolTip Text="选择查看" />
                                                    <Menu>
                                                        <Items>
                                                            <ext:MenuCommand CommandName="YLZC" Icon="Wrench" Text="一类支出">
                                                            </ext:MenuCommand>
                                                            <ext:MenuCommand CommandName="XXZC" Icon="ShapeAlignBottom" Text="详细支出">
                                                            </ext:MenuCommand>
                                                            <ext:MenuCommand CommandName="BBXMSR" Icon="GroupGear" Text="报表项目收入">
                                                            </ext:MenuCommand>
                                                            <ext:MenuCommand CommandName="HSXMSR" Icon="ChartCurve" Text="核算项目收入">
                                                            </ext:MenuCommand>
                                                            <ext:MenuCommand CommandName="DYKSQK" Icon="ChartCurve" Text="对应科室情况">
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
                                    <%--<RowDblClick Handler=" var record = GridPanel1.getStore().getAt(rowIndex); gridCommand('CmdBJGW',record);" />--%>
                                </Listeners>
                                <SelectionModel>
                                    <ext:RowSelectionModel ID="rowselection" SingleSelect="true" />
                                </SelectionModel>
                                <LoadMask ShowMask="true" Msg="载入中..." />
                            </ext:GridPanel>
                        </ext:LayoutColumn>
                    </Columns>
                </ext:ColumnLayout>
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
