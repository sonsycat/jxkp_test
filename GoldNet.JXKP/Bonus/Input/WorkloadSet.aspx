<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WorkloadSet.aspx.cs" Inherits="GoldNet.JXKP.Bonus.Input.WorkloadSet" %>

<%@ Register Assembly="Goldnet.Ext.Web" Namespace="Goldnet.Ext.Web" TagPrefix="ext" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <style type="text/css">
        body
        {
            background-color: #DFE8F6;
            font-size: 12px;
        }
    </style>
    <link rel="stylesheet" type="text/css" href="../../Bonus/Orthers/Cbouns.css" />
    <script language="javascript" type="text/javascript">
        var RefreshData = function (msg) {
            Ext.Msg.show({ title: '提示', msg: msg, icon: 'ext-mb-info', buttons: { ok: true} });
            Store1.reload();
        }
        function getcheckednode(node) {
            btn_Add.enable();
            var Nodeid = "";
            if (node.id == 'root') {
                Nodeid = "";
            } else {
                Nodeid = node.id;
            }
            Store1.filterBy(getRecordFilter(Nodeid));
        }
        function dbonclick(item_class) {
            document.location.href = "dept_income_item.aspx?item_class=" + item_class;
        }
        var rmbMoney = function (v) {
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
        
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <ext:ScriptManager ID="ScriptManager1" runat="server" />
    <ext:Store ID="Store1" runat="server" AutoLoad="true" GroupField="CLASS_CODE" OnRefreshData="Store_RefreshData">
        <Reader>
            <ext:JsonReader ReaderID="ITEM_CLASS">
                <Fields>
                    <ext:RecordField Name="CLASS_CODE" />
                    <ext:RecordField Name="ITEM_CLASS" />
                    <ext:RecordField Name="ITEM_NAME" />
                    <ext:RecordField Name="INPUT_CODE" />
                    <ext:RecordField Name="INP_GRADE" />
                    <ext:RecordField Name="OUP_GRADE" />
                    <ext:RecordField Name="TYPE_CODE" />
                </Fields>
            </ext:JsonReader>
        </Reader>
    </ext:Store>
    <ext:ViewPort ID="ViewPort1" runat="server">
        <Body>
            <ext:ColumnLayout ID="ColumnLayout1" runat="server" Split="true" FitHeight="true">
                <Columns>
                    <ext:LayoutColumn ColumnWidth="1">
                        <ext:GridPanel ID="GridPanel1" runat="server" StoreID="Store1" StripeRows="true"
                            TrackMouseOver="true" AutoWidth="true" Height="480" Border="false">
                            <TopBar>
                                <ext:Toolbar ID="Toolbar1" runat="server" Visible="true" AutoWidth="true">
                                    <Items>
                                        <ext:Button ID="btn_Add" runat="server" Text="添加" Icon="Add">
                                            <AjaxEvents>
                                                <Click OnEvent="Button_add_click">
                                                    <EventMask Msg="载入中..." ShowMask="true" />
                                                    <%--<ExtraParams>                                                
                                                    <ext:Parameter Name="Values" Value="getDeptCheckedNode()"  Mode="Raw">
                                                    </ext:Parameter>
                                                    </ExtraParams>--%>
                                                </Click>
                                            </AjaxEvents>
                                        </ext:Button>
                                        <ext:Button ID="Button_set" runat="server" Text="设置" Icon="DatabaseGo" Disabled="true">
                                            <AjaxEvents>
                                                <Click OnEvent="Button_set_click">
                                                    <EventMask Msg="载入中..." ShowMask="true" />
                                                </Click>
                                            </AjaxEvents>
                                        </ext:Button>
                                        <ext:Button ID="Button_del" runat="server" Text="删除" Icon="DatabaseGo">
                                            <AjaxEvents>
                                                <Click OnEvent="Button_del_click">
                                                    <EventMask Msg="载入中..." ShowMask="true" />
                                                </Click>
                                            </AjaxEvents>
                                        </ext:Button>
                                        <ext:Button ID="Button_dept" runat="server" Text="科室明细" Icon="Cog" Visible="false">
                                            <AjaxEvents>
                                                <Click OnEvent="Button_dept_click">
                                                    <EventMask Msg="载入中..." ShowMask="true" />
                                                </Click>
                                            </AjaxEvents>
                                        </ext:Button>
                                        <ext:Button ID="Button_refresh" runat="server" Text="刷新" Icon="ArrowRefresh">
                                            <AjaxEvents>
                                                <Click OnEvent="Button_refresh_click">
                                                    <EventMask Msg="载入中..." ShowMask="true" />
                                                </Click>
                                            </AjaxEvents>
                                        </ext:Button>
                                        <ext:ComboBox ID="cbbType" runat="server" ReadOnly="true" ForceSelection="true" SelectOnFocus="true"
                                            Width="120" >
                                            <AjaxEvents>
                                                <Select OnEvent="Button_refresh_click">
                                                    <EventMask Msg="载入中..." ShowMask="true" />
                                                </Select>
                                            </AjaxEvents>
                                            <Items>
                                                <ext:ListItem Text="执行医生" Value="1" />
                                                <ext:ListItem Text="护士" Value="2" />
                                                <ext:ListItem Text="开单医生" Value="3" />
                                            
                                            </Items>
                                        </ext:ComboBox>
                                    </Items>
                                </ext:Toolbar>
                            </TopBar>
                            <ColumnModel ID="ColumnModel1" runat="server">
                                <Columns>
                                    <ext:Column ColumnID="CLASS_CODE" Header="收入类别" Width="100" Align="left" Sortable="true"
                                        DataIndex="CLASS_CODE" MenuDisabled="true" />
                                    <ext:Column ColumnID="ITEM_NAME" Header="项目名称" Width="100" Align="left" Sortable="true"
                                        DataIndex="ITEM_NAME" MenuDisabled="true" />
                                    <ext:Column ColumnID="ITEM_CLASS" Header="项目代码" Width="100" Align="left" Sortable="true"
                                        DataIndex="ITEM_CLASS" MenuDisabled="true" />
                                    <ext:Column ColumnID="INPUT_CODE" Header="输入码" Width="100" Align="left" Sortable="true"
                                        DataIndex="INPUT_CODE" MenuDisabled="true" />
                                    <ext:Column ColumnID="INP_GRADE" Header="积分" Width="100" Align="right" Sortable="true"
                                        DataIndex="INP_GRADE" MenuDisabled="true">
                                        <Renderer Fn="rmbMoney" />
                                    </ext:Column>
                                    <ext:Column ColumnID="OUP_GRADE" Header="门诊积分" Width="100" Align="right" Sortable="true"
                                        DataIndex="OUP_GRADE" MenuDisabled="true" Hidden="true">
                                        <Renderer Fn="rmbMoney" />
                                    </ext:Column>
                                    <ext:CommandColumn Width="200" Align="Center" Header="<div style='text-align:center;'>收费项目</div>">
                                        <Commands>
                                            <ext:GridCommand Icon="Outline" CommandName="OtherDept" ToolTip-Text="收费项目">
                                            </ext:GridCommand>
                                        </Commands>
                                    </ext:CommandColumn>
                                </Columns>
                            </ColumnModel>
                            <AjaxEvents>
                                <RowDblClick OnEvent="Button_set_click" />
                            </AjaxEvents>
                            <SelectionModel>
                                <ext:RowSelectionModel ID="RowSelectionModel1" runat="server" SingleSelect="true">
                                    <Listeners>
                                        <RowSelect Handler="#{Button_set}.enable()" />
                                        <RowDeselect Handler="if (!#{GridPanel1}.hasSelection()) {#{Button_set}.disable()}" />
                                    </Listeners>
                                </ext:RowSelectionModel>
                            </SelectionModel>
                            <Listeners>
                                <Command Handler="if(command=='OtherDept'){ Goldnet.AjaxMethods.SetDept(record.data.ITEM_CLASS)}" />
                            </Listeners>
                            <View>
                                <ext:GroupingView ID="GroupingView1" HideGroupedColumn="true" runat="server" GroupTextTpl='{text} ({[values.rs.length]})'
                                    EnableRowBody="false">
                                </ext:GroupingView>
                            </View>
                            <BottomBar>
                                <ext:PagingToolbar ID="PagingToolBar2" runat="server" PageSize="20" StoreID="Store1"
                                    AutoWidth="true" DisplayInfo="false" AutoDataBind="true">
                                </ext:PagingToolbar>
                            </BottomBar>
                        </ext:GridPanel>
                    </ext:LayoutColumn>
                </Columns>
            </ext:ColumnLayout>
        </Body>
    </ext:ViewPort>
    <ext:Window ID="DetailWin" runat="server" Icon="Group" Title="收入项目比例设置" Width="300"
        Height="400" AutoShow="false" Modal="true" CenterOnLoad="true" AutoScroll="false"
        ShowOnLoad="false" Resizable="false" StyleSpec="background-color:Transparent;"
        BodyStyle="background-color:Transparent;">
    </ext:Window>
    <ext:Window ID="DeptWin" runat="server" Icon="Group" Title="设置第三方科室" Width="1000"
        Height="400" AutoShow="false" Modal="true" CenterOnLoad="true" AutoScroll="true"
        ShowOnLoad="false" Resizable="true" StyleSpec="background-color:Transparent;"
        BodyStyle="background-color:Transparent;">
    </ext:Window>
    <ext:Window ID="TypeWin" runat="server" Icon="Group" Title="添加新类别" Width="400" Height="300"
        AutoShow="false" Modal="true" CenterOnLoad="true" AutoScroll="true" ShowOnLoad="false"
        Resizable="true" StyleSpec="background-color:Transparent;" BodyStyle="background-color:Transparent;">
    </ext:Window>
    </form>
</body>
</html>
