<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Dept_Assess_KH.aspx.cs"
    Inherits="GoldNet.JXKP.jxkh.Dept_Assess_KH" %>

<%@ Register Assembly="Goldnet.Ext.Web" Namespace="Goldnet.Ext.Web" TagPrefix="ext" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>科室考核</title>

    <script type="text/javascript">
        //查找处理
        var applyFilter = function() {
            Store1.filterBy(getRecordFilter());
        };
        
        //根据查找条件过滤数据
        var getRecordFilter = function() {
            var f = [];
            f.push({
                filter: function(record) {
                    return filterString(txt_SearchTxt.getValue(), 'DEPT_NAME', record);
                }
            });
//            f.push({
//                filter: function(record) {
//                return filterString(txt_SearchTxt.getValue(), 'STATION_NAME', record);
//                }
//            });
//            f.push({
//                filter: function(record) {
//                    return filterString(txt_SearchTxt.getValue(), 'USER_NAME', record);
//                }
//            });
           

            var len = f.length;
            return function(record) {
                if (f[0].filter(record) || f[1].filter(record) || f[2].filter(record) ) {
                    return true;
                }
                return false;
            }
        };
        
        //数据过滤
        var filterString = function(value, dataIndex, record) {
            var val = record.get(dataIndex);
            if (typeof val != "string") {
                return value.length == 0;
            }
            return val.toLowerCase().indexOf(value.toLowerCase()) > -1;
        };
        
        //临时保存处理
        function tempSaveCallback(btn, text) {
            if ((btn != "ok") && (btn != "yes")) {
                return;
            }
            if (text == "") {
                Ext.Msg.show({ title: '系统提示', msg: '请输入考核名称!', icon: 'ext-mb-info', buttons: { ok: true} });
                return;
            }
            GridPanel_List.el.mask('正在保存...', 'x-loading-mask');
            Goldnet.AjaxMethods.Btn_Save_Click(text, {
                success: function(result) {
                    if (result == "") {
                        Ext.Msg.show({ title: '系统提示', msg: '临时保存成功!', icon: 'ext-mb-info', buttons: { ok: true} });
                        Btn_Save.setDisabled(true);
                        Btn_Arch.setDisabled(false);
                        Btn_View.setDisabled(false);
                    } else {
                        Ext.Msg.show({ title: '系统提示', msg: result, icon: 'ext-mb-info', buttons: { ok: true} });
                    }
                    GridPanel_List.el.unmask();
                },
                failure: function(msg) {
                    Ext.Msg.show({ title: '系统错误', msg: '操作失败,未能保存数据 !' + msg, icon: 'ext-mb-warning', buttons: { ok: true} });
                    GridPanel_List.el.unmask();
                }
            });
        };
        
        //归档保存处理
        function archSaveCallback(btn, text) {
            if ((btn != "ok") && (btn != "yes")) {
                return;
            }
            if (text == "") {
                Ext.Msg.show({ title: '系统提示', msg: '请输入考核归档名称!', icon: 'ext-mb-info', buttons: { ok: true} });
                return;
            }
            GridPanel_List.el.mask('正在归档...', 'x-loading-mask');
            Goldnet.AjaxMethods.Btn_Arch_Click(text, {
                success: function(result) {
                    if (result == "") {
                        Ext.Msg.show({ title: '系统提示', msg: '考核归档成功!', icon: 'ext-mb-info', buttons: { ok: true} });
                        Btn_Save.setDisabled(true);
                        Btn_Arch.setDisabled(true);
                    } else {
                        Ext.Msg.show({ title: '系统提示', msg: result, icon: 'ext-mb-info', buttons: { ok: true} });
                    }
                    GridPanel_List.el.unmask();
                },
                failure: function(msg) {
                    Ext.Msg.show({ title: '系统错误', msg: '操作失败,未能数据归档 !' + msg, icon: 'ext-mb-warning', buttons: { ok: true} });
                    GridPanel_List.el.unmask();
                }
            });
        };
        
        //列表数字格式化处理
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
        
        //列表中点击明细查看处理
        function GetInfo(v_command,v_colIndex,dept_code)
        {
            Goldnet.AjaxMethod.request( 'GetInfo', {params: {command:v_command,colIndex:v_colIndex,deptcode:dept_code}});
        };
        
        //数据刷新
        var RefreshData = function() {
            Store1.reload();
        }   
    </script>

</head>
<body>
    <form id="form1" runat="server">
    <ext:ScriptManager ID="ScriptManager1" runat="server">
    </ext:ScriptManager>
    <ext:Store runat="server" ID="Store1" OnRefreshData="Store_RefreshData">
        <Reader>
            <ext:JsonReader>
                <Fields>
                    <ext:RecordField Name="DEPT_CODE" />
                    <ext:RecordField Name="DEPT_NAME" />
                    <ext:RecordField Name="GUIDE_F_VALUE_01" />
                    <ext:RecordField Name="GUIDE_F_VALUE_02" />
                    <ext:RecordField Name="GUIDE_F_VALUE_03" />
                    <ext:RecordField Name="GUIDE_F_VALUE_04" />
                    <ext:RecordField Name="ALL_VALUE" />
                    <ext:RecordField Name="DESTVALUE" />
                </Fields>
            </ext:JsonReader>
        </Reader>
    </ext:Store>
    <ext:ViewPort ID="ViewPort1" runat="server">
        <Body>
            <ext:ColumnLayout ID="ColumnLayout2" runat="server" FitHeight="true">
                <Columns>
                    <ext:LayoutColumn ColumnWidth="1">
                        <ext:GridPanel ID="GridPanel_List" runat="server" BodyBorder="false" AutoScroll="true"
                            Border="false" StoreID="Store1" StripeRows="true" TrackMouseOver="true" Height="480"
                            AutoWidth="true">
                            <TopBar>
                                <ext:Toolbar ID="Toolbar1" runat="server">
                                    <Items>
                                        <ext:ToolbarTextItem ID="ToolbarTextItem11" runat="server" Text="考核时间：" />
                                        <ext:ComboBox runat="server" ID="Comb_StartYear" Width="60" ListWidth="60" SelectedIndex="0">
                                        </ext:ComboBox>
                                        <ext:ToolbarTextItem ID="ToolbarTextItem2" runat="server" Text="年" />
                                        <ext:ComboBox runat="server" ID="Comb_StartMonth" Width="40" ListWidth="40" SelectedIndex="0">
                                        </ext:ComboBox>
                                        <ext:ToolbarTextItem ID="ToolbarTextItem14" runat="server" Text="月" />
                                        <ext:ToolbarSeparator ID="ToolbarSeparator4" runat="server">
                                        </ext:ToolbarSeparator>
                                        <ext:Checkbox ID="Cbox_ff" runat="server">
                                        </ext:Checkbox>
                                        <ext:ToolbarTextItem ID="ToolbarTextItem1" runat="server" Text="总是允许超分">
                                        </ext:ToolbarTextItem>
                                        <ext:ToolbarSeparator ID="ToolbarSeparator8" runat="server">
                                        </ext:ToolbarSeparator>
                                        <ext:Button ID="Btn_Create" runat="server" Icon="DatabaseGo" Text="生成">
                                            <%-- <AjaxEvents>
                                                <Click OnEvent="Btn_Create_Click" Timeout="90000000">
                                                    <Confirmation ConfirmRequest="true" Title="系统提示" Message="生成岗位绩效考核数据大约耗时2分钟,<br/>是否继续?" />
                                                    <ExtraParams>
                                                    </ExtraParams>
                                                    <EventMask ShowMask="true" Msg="请稍候,正在生成岗位绩效考核数据..." />
                                                </Click>
                                            </AjaxEvents>--%>
                                            <AjaxEvents>
                                                <Click OnEvent="Btn_BatInit_Click">
                                                </Click>
                                            </AjaxEvents>
                                        </ext:Button>
                                        <ext:ToolbarSeparator ID="ToolbarSeparator6" runat="server">
                                        </ext:ToolbarSeparator>
                                        <ext:Button ID="Btn_View" runat="server" Icon="Zoom" Text="查询">
                                            <AjaxEvents>
                                                <Click OnEvent="Btn_View_Click" Timeout="9000000">
                                                    <EventMask ShowMask="true" Msg="请稍候..." />
                                                </Click>
                                            </AjaxEvents>
                                        </ext:Button>
                                        <ext:ToolbarSeparator ID="ToolbarSeparator7" runat="server">
                                        </ext:ToolbarSeparator>
                                        <ext:ToolbarFill ID="ToolbarFill1" runat="server">
                                        </ext:ToolbarFill>
                                        <ext:ToolbarSeparator ID="ToolbarSeparator2" runat="server">
                                        </ext:ToolbarSeparator>
                                        <ext:Button ID="Btn_Save" runat="server" Disabled="true" Icon="PageLightning" Text="临时保存">
                                            <Listeners>
                                                <Click Handler=" Ext.Msg.prompt('临时保存','请输入绩效考核名称：', function(btn, text) { tempSaveCallback(btn, text) }, true, false, '');" />
                                            </Listeners>
                                        </ext:Button>
                                        <ext:ToolbarSeparator ID="ToolbarSeparator1" runat="server">
                                        </ext:ToolbarSeparator>
                                        <ext:Button ID="Btn_Arch" runat="server" Disabled="true" Icon="PageLink" Text="归档">
                                            <Listeners>
                                                <Click Handler=" Ext.Msg.prompt('绩效归档','请输入考核归档名称：', function(btn, text) { archSaveCallback(btn, text) }, true, false, '');" />
                                            </Listeners>
                                        </ext:Button>
                                        <ext:ToolbarSeparator ID="ToolbarSeparator3" runat="server">
                                        </ext:ToolbarSeparator>
                                        <ext:Button ID="Btn_Excel" runat="server" Disabled="true" Icon="PageWhiteExcel" Text="EXCEL导出"
                                            OnClick="OutExcel" AutoPostBack="true">
                                        </ext:Button>
                                        <ext:ToolbarSeparator ID="ToolbarSeparator5" runat="server">
                                        </ext:ToolbarSeparator>
                                    </Items>
                                </ext:Toolbar>
                            </TopBar>
                            <ColumnModel ID="ColumnModel1" runat="server">
                                <Columns>
                                    <ext:RowNumbererColumn Width="30" />
                                    <ext:Column Header="考核部门" Width="90" ColumnID="DEPT_NAME" DataIndex="DEPT_NAME" MenuDisabled="true" />
                                    <ext:Column ColumnID="DEPT_CODE" DataIndex="DEPT_CODE" Hidden="true" />
                                    <ext:Column Header="内部管理" Width="110" ColumnID="GUIDE_F_VALUE_01" DataIndex="GUIDE_F_VALUE_01"
                                        MenuDisabled="true" Align="Right">
                                        <Renderer Fn="rmbMoney" />
                                        <Commands>
                                            <ext:ImageCommand CommandName="ResultInfo" Icon="Zoom" Style="margin-left: 5px !important;" />
                                        </Commands>
                                    </ext:Column>
                                    <ext:Column Header="卫生经济" Width="110" ColumnID="GUIDE_F_VALUE_02" DataIndex="GUIDE_F_VALUE_02"
                                        MenuDisabled="true" Align="Right">
                                        <Renderer Fn="rmbMoney" />
                                        <Commands>
                                            <ext:ImageCommand CommandName="ResultInfo" Icon="Zoom" Style="margin-left: 5px !important;" />
                                        </Commands>
                                    </ext:Column>
                                    <ext:Column Header="服务满意度" Width="110" ColumnID="GUIDE_F_VALUE_03" DataIndex="GUIDE_F_VALUE_03"
                                        MenuDisabled="true" Align="Right">
                                        <Renderer Fn="rmbMoney" />
                                        <Commands>
                                            <ext:ImageCommand CommandName="ResultInfo" Icon="Zoom" Style="margin-left: 5px !important;" />
                                        </Commands>
                                    </ext:Column>
                                    <ext:Column Header="学习与成长" Width="110" ColumnID="GUIDE_F_VALUE_04" DataIndex="GUIDE_F_VALUE_04"
                                        MenuDisabled="true" Align="Right">
                                        <Renderer Fn="rmbMoney" />
                                        <Commands>
                                            <ext:ImageCommand CommandName="ResultInfo" Icon="Zoom" Style="margin-left: 5px !important;" />
                                        </Commands>
                                    </ext:Column>
                                    <ext:Column Header="总分" Width="120" ColumnID="ALL_VALUE" DataIndex="ALL_VALUE" MenuDisabled="true"
                                        Align="Right">
                                        <Renderer Fn="rmbMoney" />
                                        <Commands>
                                            <ext:ImageCommand CommandName="ResultInfo" Icon="Zoom" Style="margin-left: 5px !important;" />
                                        </Commands>
                                    </ext:Column>
                                </Columns>
                            </ColumnModel>
                            <SelectionModel>
                                <ext:RowSelectionModel ID="rowselection" SingleSelect="true">
                                </ext:RowSelectionModel>
                            </SelectionModel>
                            <LoadMask ShowMask="true" Msg="载入中..." />
                            <BottomBar>
                                <ext:PagingToolbar ID="PagingToolBar2" runat="server" PageSize="30" StoreID="Store1"
                                    AutoWidth="true" DisplayInfo="true" AutoDataBind="true">
                                    <Items>
                                        <ext:TextField ID="txt_SearchTxt" runat="server" EmptyText="查找信息">
                                            <ToolTips>
                                                <ext:ToolTip ID="ToolTip1" runat="server" Html="根据科室或岗位名称关键字查找">
                                                </ext:ToolTip>
                                            </ToolTips>
                                        </ext:TextField>
                                        <ext:Button ID="btn_Search" Icon="Zoom" runat="server" Text="查找">
                                            <Listeners>
                                                <Click Fn="applyFilter" />
                                            </Listeners>
                                        </ext:Button>
                                    </Items>
                                </ext:PagingToolbar>
                            </BottomBar>
                            <Listeners>
                                <Command Handler="GetInfo(command,this.getColumnModel().getDataIndex(colIndex),record.data.DEPT_CODE);" />
                            </Listeners>
                        </ext:GridPanel>
                    </ext:LayoutColumn>
                </Columns>
            </ext:ColumnLayout>
        </Body>
    </ext:ViewPort>
    <%-- <ext:Window ID="DetailWin" runat="server" Icon="ApplicationViewIcons" Title="选择" Width="760"  Closable="true" CloseAction="Hide" Maximizable="true"
            Height="430"  AutoShow="false" Modal="true"  AutoScroll="true" CenterOnLoad="true"  ShowOnLoad="false"  Resizable="true" StyleSpec="background-color:Transparent;" BodyStyle="background-color:Transparent;">
        <Listeners>
            <Hide Handler="this.clearContent();" />
            <BeforeShow  Handler=" var height = Ext.getBody().getViewSize().height; var width = Ext.getBody().getViewSize().width; if (el.getSize().height > height) {  el.setHeight(height - 20) } ;if (el.getSize().width > width) {  el.setWidth(width - 20) }  " />
        </Listeners>
    </ext:Window> --%>
    <ext:Window ID="StaffInfoWin" runat="server" Icon="Group" Title="人员信息" Width="825"
        Height="500" AutoShow="false" Modal="true" CenterOnLoad="true" ShowOnLoad="false"
        Resizable="false" StyleSpec="background-color:Transparent;" BodyStyle="background-color:Transparent;"
        AutoScroll="true">
    </ext:Window>
    <ext:Window ID="ResultInfoWin" runat="server" Icon="Group" Title="详细信息" Width="825"
        Height="450" AutoShow="false" Modal="true" CenterOnLoad="true" ShowOnLoad="false"
        Resizable="false" StyleSpec="background-color:Transparent;" BodyStyle="background-color:Transparent;"
        AutoScroll="true">
    </ext:Window>
    <ext:Window runat="server" ID="Win_BatchInit" AutoShow="false" ShowOnLoad="false"
        Modal="true" Resizable="false" Title="科室考评" CenterOnLoad="true" AutoScroll="false"
        Width="280" Height="160" CloseAction="Hide" Closable="false" AnimateTarget="Btn_BatInit"
        Icon="TagPink" BodyStyle="padding:2px;">
        <Body>
            <table>
                <tr>
                    <td colspan="2" align="left">
                        <p>
                            注意：生成数据需要大约2分钟时间，在此时间内请不要关闭您的浏览器或者刷新页面。</p>
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
            <ext:Toolbar ID="Toolbar2" runat="server">
                <Items>
                    <ext:ToolbarFill ID="ToolbarFill2" runat="server" />
                    <ext:ToolbarButton ID="Btn_BatStart" runat="server" Icon="PlayGreen" Text="开始生成">
                        <AjaxEvents>
                            <Click OnEvent="Btn_BatStart_Click" Timeout="1200000">
                            </Click>
                        </AjaxEvents>
                    </ext:ToolbarButton>
                    <ext:ToolbarSeparator ID="ToolbarSeparator9" runat="server" />
                    <ext:ToolbarButton ID="Btn_BatCancel" runat="server" Icon="Cancel" Text="关闭">
                        <AjaxEvents>
                            <Click OnEvent="Buttonclose">
                            </Click>
                        </AjaxEvents>
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
    </form>
</body>
</html>
