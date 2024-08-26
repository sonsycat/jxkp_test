<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Assess_KH.aspx.cs" Inherits="GoldNet.JXKP.jxkh.Assess_KH" %>

<%@ Register Assembly="Goldnet.Ext.Web" Namespace="Goldnet.Ext.Web" TagPrefix="ext" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>绩效考核</title>

    <script type="text/javascript">
        var applyFilter = function() {
            Store1.filterBy(getRecordFilter());
        };
        var getRecordFilter = function() {
            var f = [];
            f.push({
                filter: function(record) {
                    return filterString(txt_SearchTxt.getValue(), 'DEPT_NAME', record);
                }
            });
            f.push({
                filter: function(record) {
                return filterString(txt_SearchTxt.getValue(), 'STATION_NAME', record);
                }
            });
            f.push({
                filter: function(record) {
                    return filterString(txt_SearchTxt.getValue(), 'USER_NAME', record);
                }
            });
           

            var len = f.length;
            return function(record) {
                if (f[0].filter(record) || f[1].filter(record) || f[2].filter(record) ) {
                    return true;
                }
                return false;
            }
        };
        var filterString = function(value, dataIndex, record) {
            var val = record.get(dataIndex);
            if (typeof val != "string") {
                return value.length == 0;
            }
            return val.toLowerCase().indexOf(value.toLowerCase()) > -1;
        };

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
        }
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
        }

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
        function GetInfo(v_command,v_colIndex,persion_id)
        {
            Goldnet.AjaxMethod.request( 'GetInfo', {params: {command:v_command,colIndex:v_colIndex,staff_id:persion_id}});
        }
    </script>

</head>
<body>
    <form id="form1" runat="server">
    <ext:ScriptManager ID="ScriptManager1" runat="server">
    </ext:ScriptManager>
    <ext:Store runat="server" ID="Store1">
        <Reader>
            <ext:JsonReader>
                <Fields>
                    <ext:RecordField Name="DEPT_CODE" />
                    <ext:RecordField Name="DEPT_NAME" />
                    <ext:RecordField Name="STATION_CODE" />
                    <ext:RecordField Name="STATION_NAME" />
                    <ext:RecordField Name="PERSON_ID" />
                    <ext:RecordField Name="USER_NAME" />
                    <ext:RecordField Name="DAYS_ON_DUTY" />
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
                                <ext:Toolbar runat="server">
                                    <Items>
                                        <ext:ToolbarTextItem ID="ToolbarTextItem1" runat="server" Text="考核月份 从：" />
                                        <ext:ComboBox runat="server" ID="Comb_StartYear" Width="60" ListWidth="60" SelectedIndex="0">
                                        </ext:ComboBox>
                                        <ext:ToolbarTextItem ID="ToolbarTextItem2" runat="server" Text="年" />
                                        <ext:ComboBox runat="server" ID="Comb_StartMonth" Width="40" ListWidth="40" SelectedIndex="0">
                                        </ext:ComboBox>
                                        <ext:ToolbarTextItem ID="ToolbarTextItem3" runat="server" Text="月　至：" />
                                        <ext:ComboBox runat="server" ID="Comb_EndYear" Width="60" ListWidth="60" SelectedIndex="0">
                                        </ext:ComboBox>
                                        <ext:ToolbarTextItem ID="ToolbarTextItem4" runat="server" Text="年" />
                                        <ext:ComboBox runat="server" ID="Comb_EndMonth" Width="40" ListWidth="40">
                                        </ext:ComboBox>
                                        <ext:ToolbarTextItem ID="ToolbarTextItem5" runat="server" Text="月" />
                                        <ext:ToolbarSeparator ID="ToolbarSeparator4" runat="server">
                                        </ext:ToolbarSeparator>
                                        <ext:Checkbox ID="Cbox_ff" runat="server">
                                        </ext:Checkbox>
                                        <ext:ToolbarTextItem runat="server" Text="总是允许超分">
                                        </ext:ToolbarTextItem>
                                        <ext:ToolbarSeparator ID="ToolbarSeparator8" runat="server">
                                        </ext:ToolbarSeparator>
                                        <ext:Button ID="Btn_Create" runat="server" Icon="DatabaseGo" Text="生成">
                                            <AjaxEvents>
                                                <Click OnEvent="Btn_Create_Click" Timeout="900000">
                                                    <Confirmation ConfirmRequest="true" Title="系统提示" Message="生成岗位绩效考核数据大约耗时2分钟,<br/>是否继续?" />
                                                    <ExtraParams>
                                                    </ExtraParams>
                                                    <EventMask ShowMask="true" Msg="请稍候,正在生成岗位绩效考核数据..." />
                                                </Click>
                                            </AjaxEvents>
                                        </ext:Button>
                                        <ext:ToolbarSeparator ID="ToolbarSeparator6" runat="server">
                                        </ext:ToolbarSeparator>
                                        <ext:Button ID="Btn_View" runat="server" Icon="Zoom" Text="查询">
                                            <AjaxEvents>
                                                <Click OnEvent="Btn_View_Click" Timeout="90000">
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
                                    <ext:Column Header="所属岗位" Width="120" ColumnID="STATION_NAME" DataIndex="STATION_NAME"
                                        MenuDisabled="true" />
                                    <ext:Column ColumnID="PERSON_ID" DataIndex="PERSON_ID" Hidden="true" />
                                    <ext:Column Header="姓名" Width="90" ColumnID="USER_NAME" DataIndex="USER_NAME" MenuDisabled="true">
                                        <Commands>
                                            <ext:ImageCommand CommandName="NameInfo" Icon="UserHome" Style="margin-left: 5px !important;" />
                                        </Commands>
                                    </ext:Column>
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
                                    <ext:Column Header="在岗天数" Width="60" ColumnID="DAYS_ON_DUTY" DataIndex="DAYS_ON_DUTY"
                                        MenuDisabled="true" />
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
                                <Command Handler="GetInfo(command,this.getColumnModel().getDataIndex(colIndex),record.data.PERSON_ID);" />
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
        Height="500" AutoShow="false" Modal="true" CenterOnLoad="true" ShowOnLoad="false"
        Resizable="false" StyleSpec="background-color:Transparent;" BodyStyle="background-color:Transparent;"
        AutoScroll="true">
    </ext:Window>
    </form>
</body>
</html>
