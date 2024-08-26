<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="income_update_date.aspx.cs" Inherits="GoldNet.JXKP.cbhs.cbhsdict.income_update_date" %>

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
    <script language="javascript" type="text/javascript">
        var RefreshData = function (msg, year, month) {
            years.setValue(year);
            months.setValue(month);
            Ext.Msg.alert('提示', msg);
            Store1.reload();
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
        }
      
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <ext:ScriptManager ID="ScriptManager1" runat="server" AjaxMethodNamespace="CompanyX" />
    <ext:Store ID="Store1" runat="server" OnRefreshData="Store_RefreshData">
        <Reader>
            <ext:JsonReader>
                <Fields>
                    <ext:RecordField Name="ID" Type="String" Mapping="ID" />
                    <ext:RecordField Name="INCOME_NAME" Type="String" Mapping="INCOME_NAME" />
                    <ext:RecordField Name="S_DATE" Type="String" Mapping="S_DATE" />
                    <ext:RecordField Name="D_DATE" Type="String" Mapping="D_DATE" />
                    <ext:RecordField Name="DATE_TIME" Type="String" Mapping="DATE_TIME" />
                    <ext:RecordField Name="TO_DATE_TIME" Type="String" Mapping="TO_DATE_TIME" />
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
                                <ext:Toolbar ID="Toolbar_fjsr" runat="server" Visible="true" AutoWidth="true">
                                    <Items>
                                        <ext:ComboBox ID="years" runat="server" Width="60" AllowBlank="true" EmptyText="请选择年..."
                                            FieldLabel="年">
                                            <AjaxEvents>
                                                <Select OnEvent="Date_SelectOnChange">
                                                    <EventMask Msg='载入中...' ShowMask="true" />
                                                </Select>
                                            </AjaxEvents>
                                        </ext:ComboBox>
                                        <ext:ToolbarTextItem ID="dd1Name" runat="server" Text="年 " />
                                        <ext:ComboBox ID="months" runat="server" Width="60" AllowBlank="true" EmptyText="请选择月..."
                                            FieldLabel="月">
                                            <Items>
                                                <ext:ListItem Text="01" Value="01" />
                                                <ext:ListItem Text="02" Value="02" />
                                                <ext:ListItem Text="03" Value="03" />
                                                <ext:ListItem Text="04" Value="04" />
                                                <ext:ListItem Text="05" Value="05" />
                                                <ext:ListItem Text="06" Value="06" />
                                                <ext:ListItem Text="07" Value="07" />
                                                <ext:ListItem Text="08" Value="08" />
                                                <ext:ListItem Text="09" Value="09" />
                                                <ext:ListItem Text="10" Value="10" />
                                                <ext:ListItem Text="11" Value="11" />
                                                <ext:ListItem Text="12" Value="12" />
                                            </Items>
                                            <AjaxEvents>
                                                <Select OnEvent="Date_SelectOnChange">
                                                    <EventMask Msg='载入中...' ShowMask="true" />
                                                </Select>
                                            </AjaxEvents>
                                        </ext:ComboBox>
                                        <ext:ToolbarTextItem ID="ToolbarTextItem1" runat="server" Text="月 " />
                                        <ext:ToolbarSeparator ID="ToolbarSeparator1" runat="server" />
                                        <ext:Button ID="Button_look" runat="server" Text="查询" Icon="DatabaseGo">
                                            <AjaxEvents>
                                                <Click OnEvent="Button_look_click">
                                                    <EventMask Msg="载入中..." ShowMask="true" />
                                                </Click>
                                            </AjaxEvents>
                                        </ext:Button>
                                        <ext:ToolbarSeparator ID="ToolbarSeparator2" runat="server" />
                                        <ext:Button ID="Button_add" runat="server" Text="添加" Icon="Add">
                                            <AjaxEvents>
                                                <Click OnEvent="Button_add_click">
                                                    <EventMask Msg="载入中..." ShowMask="true" />
                                                    
                                                </Click>
                                            </AjaxEvents>
                                        </ext:Button>
                                    </Items>
                                </ext:Toolbar>
                            </TopBar>
                            <ColumnModel ID="ColumnModel1" runat="server">
                                <Columns>
                                    <ext:Column ColumnID="ID" Header="<div style='text-align:center;'>编号</div>" Width="90"
                                        Align="left" Sortable="true" DataIndex="ID" MenuDisabled="true" />
                                    <ext:Column ColumnID="INCOME_NAME" Header="<div style='text-align:center;'>调整表</div>"
                                        Width="190" Align="Right" Sortable="true" DataIndex="INCOME_NAME" MenuDisabled="true">
                                    </ext:Column>
                                    <ext:Column ColumnID="S_DATE" Header="<div style='text-align:center;'>开始时间</div>"
                                        Width="190" Align="Right" Sortable="true" DataIndex="S_DATE" MenuDisabled="true">
                                    </ext:Column>
                                    <ext:Column ColumnID="D_DATE" Header="<div style='text-align:center;'>结束时间</div>"
                                        Width="190" Align="left" Sortable="true" DataIndex="D_DATE" MenuDisabled="true" />
                                    <ext:Column ColumnID="TO_DATE_TIME" Header="<div style='text-align:center;'>目标日期</div>"
                                        Width="190" Align="left" Sortable="true" DataIndex="TO_DATE_TIME" MenuDisabled="true" />
                                    <ext:Column ColumnID="DATE_TIME" Header="<div style='text-align:center;'>操作时间</div>"
                                        Width="190" Align="left" Sortable="true" DataIndex="DATE_TIME" MenuDisabled="true" />
                                </Columns>
                            </ColumnModel>
                            <LoadMask ShowMask="true" />
                            <BottomBar>
                                <ext:PagingToolbar ID="PagingToolBar1" runat="server" PageSize="20" StoreID="Store1" />
                            </BottomBar>
                        </ext:GridPanel>
                    </ext:LayoutColumn>
                </Columns>
            </ext:ColumnLayout>
        </Body>
    </ext:ViewPort>
    <ext:Window ID="DetailWin" runat="server" Icon="Group" Title="添加" Width="410" Height="410"
        AutoShow="false" Modal="true" CenterOnLoad="true" AutoScroll="true" ShowOnLoad="false"
        Resizable="true" StyleSpec="background-color:Transparent;" BodyStyle="background-color:Transparent;">
    </ext:Window>
    </form>
</body>
</html>
