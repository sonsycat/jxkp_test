<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Dept_Assess_Result.aspx.cs"
    Inherits="GoldNet.JXKP.jxkh.Dept_Assess_Result" %>

<%@ Register Assembly="Goldnet.Ext.Web" Namespace="Goldnet.Ext.Web" TagPrefix="ext" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>科室绩效考核查询</title>

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
                if (f[0].filter(record) || f[1].filter(record) || f[2].filter(record)) {
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
        function GetInfo(v_command,v_colIndex,dept_code)
        {
            Goldnet.AjaxMethod.request( 'GetInfo', {params: {command:v_command,colIndex:v_colIndex,deptcode:dept_code}});
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
                                        <ext:ToolbarTextItem ID="ToolbarTextItem1" runat="server" Text="考核年度：" />
                                        <ext:ComboBox runat="server" ID="Comb_StartYear" Width="60" ListWidth="60" SelectedIndex="0">
                                            <AjaxEvents>
                                                <Select OnEvent="Comb_Year_Selected">
                                                    <EventMask Msg="请稍候..." ShowMask="true" />
                                                </Select>
                                            </AjaxEvents>
                                            <Listeners>
                                                <Change Handler="Store1.removeAll();" />
                                            </Listeners>
                                        </ext:ComboBox>
                                        <ext:ToolbarTextItem ID="ToolbarTextItem2" runat="server" Text="年" />
                                        <ext:ToolbarSeparator ID="ToolbarSeparator1" runat="server">
                                        </ext:ToolbarSeparator>
                                        <ext:ToolbarTextItem ID="ToolbarTextItem3" runat="server" Text="考核名称：" />
                                        <ext:ComboBox runat="server" ID="Comb_AssessName" Width="140" ListWidth="140">
                                            <Listeners>
                                                <Change Handler="Store1.removeAll();" />
                                            </Listeners>
                                        </ext:ComboBox>
                                        <ext:ToolbarSeparator ID="ToolbarSeparator4" runat="server">
                                        </ext:ToolbarSeparator>
                                        <ext:ToolbarTextItem ID="ToolbarTextItem4" runat="server" Text="科室类别：" />
                                        <ext:ComboBox runat="server" ID="ComboBoxdepttype" Width="140" ListWidth="140">
                                        </ext:ComboBox>
                                        <ext:ToolbarSeparator ID="ToolbarSeparator6" runat="server">
                                        </ext:ToolbarSeparator>
                                        <ext:Button ID="Btn_View" runat="server" Icon="Zoom" Text="查看">
                                            <AjaxEvents>
                                                <Click OnEvent="Btn_View_Clicked">
                                                    <EventMask ShowMask="true" Msg="请稍候..." />
                                                </Click>
                                            </AjaxEvents>
                                        </ext:Button>
                                        <ext:ToolbarSeparator ID="ToolbarSeparator2" runat="server">
                                        </ext:ToolbarSeparator>
                                        <ext:Button ID="Btn_Del" runat="server" Icon="Delete" Text="删除" Disabled="true">
                                            <AjaxEvents>
                                                <Click OnEvent="Btn_Del_Clicked">
                                                    <Confirmation ConfirmRequest="true" Title="系统提示" Message="确实要删除该绩效考核结果吗?" />
                                                    <EventMask ShowMask="true" Msg="请稍候..." />
                                                </Click>
                                            </AjaxEvents>
                                        </ext:Button>
                                        <ext:ToolbarSeparator ID="ToolbarSeparator3" runat="server">
                                        </ext:ToolbarSeparator>
                                        <ext:Button ID="Btn_Excel" runat="server" Icon="PageWhiteExcel" Text="EXCEL导出" OnClick="OutExcel"
                                            AutoPostBack="true">
                                        </ext:Button>
                                        <ext:ToolbarSeparator ID="ToolbarSeparator5" runat="server">
                                        </ext:ToolbarSeparator>
                                    </Items>
                                </ext:Toolbar>
                            </TopBar>
                            <ColumnModel ID="ColumnModel1" runat="server">
                                <Columns>
                                    <ext:RowNumbererColumn />
                                    <ext:Column Header="考核部门" Width="90" ColumnID="DEPT_NAME" DataIndex="DEPT_NAME" MenuDisabled="true" />
                                    <ext:Column ColumnID="DEPT_CODE" DataIndex="DEPT_CODE" Hidden="true" />
                                    <ext:Column Header="内部管理" Width="110" ColumnID="GUIDE_F_VALUE_01" DataIndex="GUIDE_F_VALUE_01"
                                        MenuDisabled="true" Align="Right">
                                        <Renderer Fn="rmbMoney" />
                                        <Commands>
                                            <ext:ImageCommand CommandName="ResultInfo" Text="..." ToolTip-Text="明细" />
                                        </Commands>
                                    </ext:Column>
                                    <ext:Column Header="卫生经济" Width="110" ColumnID="GUIDE_F_VALUE_02" DataIndex="GUIDE_F_VALUE_02"
                                        MenuDisabled="true" Align="Right">
                                        <Renderer Fn="rmbMoney" />
                                        <Commands>
                                            <ext:ImageCommand CommandName="ResultInfo" Text="..." ToolTip-Text="明细" />
                                        </Commands>
                                    </ext:Column>
                                    <ext:Column Header="服务满意度" Width="110" ColumnID="GUIDE_F_VALUE_03" DataIndex="GUIDE_F_VALUE_03"
                                        MenuDisabled="true" Align="Right">
                                        <Renderer Fn="rmbMoney" />
                                        <Commands>
                                            <ext:ImageCommand CommandName="ResultInfo" Text="..." ToolTip-Text="明细" />
                                        </Commands>
                                    </ext:Column>
                                    <ext:Column Header="学习与成长" Width="110" ColumnID="GUIDE_F_VALUE_04" DataIndex="GUIDE_F_VALUE_04"
                                        MenuDisabled="true" Align="Right">
                                        <Renderer Fn="rmbMoney" />
                                        <Commands>
                                            <ext:ImageCommand CommandName="ResultInfo" Text="..." ToolTip-Text="明细" />
                                        </Commands>
                                    </ext:Column>
                                    <ext:Column Header="总分" Width="120" ColumnID="ALL_VALUE" DataIndex="ALL_VALUE" MenuDisabled="true"
                                        Align="Right">
                                        <Renderer Fn="rmbMoney" />
                                        <Commands>
                                            <ext:ImageCommand CommandName="ResultInfo" Text="..." ToolTip-Text="明细" />
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
                                        <ext:Button ID="btn_Search" Icon="Zoom" runat="server" Text="查询">
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
    <%--  <ext:Window ID="DetailWin" runat="server" Icon="ApplicationViewIcons" Title="选择"
        Width="760" Closable="true" CloseAction="Hide" Maximizable="true" Height="430"
        AutoShow="false" Modal="true" AutoScroll="true" CenterOnLoad="true" ShowOnLoad="false"
        Resizable="true" StyleSpec="background-color:Transparent;" BodyStyle="background-color:Transparent;">
        <Listeners>
            <Hide Handler="this.clearContent();" />
            <BeforeShow Handler=" var height = Ext.getBody().getViewSize().height; var width = Ext.getBody().getViewSize().width; if (el.getSize().height > height) {  el.setHeight(height - 20) } ;if (el.getSize().width > width) {  el.setWidth(width - 20) }  " />
        </Listeners>
    </ext:Window>--%>
    <ext:Window ID="StaffInfoWin" runat="server" Icon="Group" Title="人员信息" Width="825"
        Height="500" AutoShow="false" Modal="true" CenterOnLoad="true" ShowOnLoad="false"
        Resizable="false" StyleSpec="background-color:Transparent;" BodyStyle="background-color:Transparent;"
        AutoScroll="true">
    </ext:Window>
    <ext:Window ID="ResultInfoWin" runat="server" Icon="Group" Title="详细信息" Width="700"
        Height="440" AutoShow="false" Modal="true" CenterOnLoad="true" ShowOnLoad="false"
        Resizable="false" StyleSpec="background-color:Transparent;" BodyStyle="background-color:Transparent;"
        AutoScroll="true">
    </ext:Window>
    </form>
</body>
</html>
