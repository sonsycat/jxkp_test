<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="account_data.aspx.cs" Inherits="GoldNet.JXKP.cbhs.datagather.account_data" %>

<%@ Register Assembly="Goldnet.Ext.Web" Namespace="Goldnet.Ext.Web" TagPrefix="ext" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>

    <script language="javascript" type="text/javascript">
        var rmbMoney = function(v) {
               if(v==null||v=="")
               {
               return "";
               }
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
       var pctChange = function(value) {
            return  value + '%';
        }
    </script>

</head>
<body>
    <form id="form1" runat="server">
    <div>
        <ext:ScriptManager ID="ScriptManager1" runat="server" AjaxMethodNamespace="CompanyX" />
        <ext:Store ID="Store1" runat="server">
            <Reader>
                <ext:JsonReader ReaderID="DEPT_CODE">
                    <Fields>
                        <ext:RecordField Name="DEPT_CODE" Type="String" Mapping="DEPT_CODE" />
                        <ext:RecordField Name="DEPT_NAME" Type="String" Mapping="DEPT_NAME" />
                        <ext:RecordField Name="INCOMES_CHARGES" Type="Float" Mapping="INCOMES_CHARGES" />
                        <ext:RecordField Name="INCOME_COUNT" Type="Float" Mapping="INCOME_COUNT" />
                        <ext:RecordField Name="INCOMES" Type="Float" Mapping="INCOMES" />
                        <ext:RecordField Name="COST_FAC" Type="Float" Mapping="COST_FAC" />
                        <ext:RecordField Name="COST_ARM" Type="Float" Mapping="COST_ARM" />
                        <ext:RecordField Name="COSTS" Type="Float" Mapping="COSTS" />
                        <ext:RecordField Name="NET_INCOME" Type="Float" Mapping="NET_INCOME" />
                        <ext:RecordField Name="GROSS_INCOME" Type="Float" Mapping="GROSS_INCOME" />
                        <ext:RecordField Name="DEPT_LRL" Type="Float" Mapping="DEPT_LRL" />
                        <ext:RecordField Name="DEPT_CBL" Type="Float" Mapping="DEPT_CBL" />
                        <ext:RecordField Name="DEPT_GDCBL" Type="Float" Mapping="DEPT_GDCBL" />
                        <ext:RecordField Name="DEPT_BDCBL" Type="Float" Mapping="DEPT_BDCBL" />
                        <ext:RecordField Name="DEPT_YPSRL" Type="Float" Mapping="DEPT_YPSRL" />
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
                                ClicksToEdit="1" TrackMouseOver="true" AutoWidth="true" Height="480" Border="false">
                                <TopBar>
                                    <ext:Toolbar runat="server">
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
                                            <ext:Button ID="Button_create" runat="server" Text="核算数据生成" Icon="DatabaseGo">
                                                <AjaxEvents>
                                                    <Click OnEvent="Button_create_click"  Timeout="900000">
                                                        <Confirmation ConfirmRequest="true" Title="系统提示" Message="核算数据生成需要一分钟左右,<br/>是否继续?" />
                                                        <ExtraParams>
                                                        </ExtraParams>
                                                        <EventMask Msg="载入中..." ShowMask="true" />
                                                    </Click>
                                                </AjaxEvents>
                                            </ext:Button>
                                            <ext:ToolbarSeparator ID="ToolbarSeparator4" runat="server" />
                                            <ext:Button ID="Button_del" runat="server" Text="删除" Icon="DatabaseGo">
                                                <AjaxEvents>
                                                    <Click OnEvent="Button_del_click">
                                                    <Confirmation ConfirmRequest="true" Title="系统提示" Message="将删除当月核算数据,<br/>是否继续?" />
                                                        <ExtraParams>
                                                        </ExtraParams>
                                                        <EventMask Msg="载入中..." ShowMask="true" />
                                                    </Click>
                                                </AjaxEvents>
                                            </ext:Button>
                                            <ext:ToolbarSeparator ID="ToolbarSeparator2" runat="server" />
                                            <ext:ToolbarSpacer ID="ToolbarSpacer1" Width="30" runat="server"></ext:ToolbarSpacer>
                                            <ext:Button ID="btn_Excel" runat="server"  OnClick="Button_OutExcel_click" AutoPostBack="true"  Text="导出Excel" Icon="PageWhiteExcel">
                                            </ext:Button>
                                            <ext:ToolbarFill ID="ToolbarFill1" runat="server"/>                                                                              
                                            <ext:ComboBox ID="AccountSign" runat="server" Width="80" AllowBlank="true">
                                                <AjaxEvents>
                                                    <Select OnEvent="AccountSign_SelectOnChange">
                                                        <EventMask Msg='载入中...' ShowMask="true" />
                                                    </Select>
                                                </AjaxEvents>
                                            </ext:ComboBox>
                                        </Items>
                                    </ext:Toolbar>
                                </TopBar>
                                <ColumnModel ID="ColumnModel1" runat="server">
                                    <Columns>
                                        <ext:Column ColumnID="DEPT_CODE" Hidden="true" />
                                        <ext:Column ColumnID="DEPT_NAME" Header="<div style='text-align:center;'>科室</div>" Width="90" Align="left" Sortable="true"
                                            DataIndex="DEPT_NAME" MenuDisabled="true" />
                                        <ext:Column ColumnID="INCOMES_CHARGES" Header="<div style='text-align:center;'>实际收入</div>" Width="90" Align="Right" Sortable="true"
                                            DataIndex="INCOMES_CHARGES" MenuDisabled="true">
                                            <Renderer Fn="rmbMoney" />
                                        </ext:Column>
                                        <ext:Column ColumnID="INCOME_COUNT" Header="<div style='text-align:center;'>减免收入</div>" Width="90" Align="Right" Sortable="true"
                                            DataIndex="INCOME_COUNT" MenuDisabled="true">
                                            <Renderer Fn="rmbMoney" />
                                        </ext:Column>
                                        <ext:Column ColumnID="INCOMES" Header="<div style='text-align:center;'>总收入</div>" Width="90" Align="Right" Sortable="true"
                                            DataIndex="INCOMES" MenuDisabled="true">
                                            <Renderer Fn="rmbMoney" />
                                        </ext:Column>
                                        <ext:Column ColumnID="COST_FAC" Header="<div style='text-align:center;'>实际成本</div>" Width="90" Align="Right" Sortable="true"
                                            DataIndex="COST_FAC" MenuDisabled="true">
                                            <Renderer Fn="rmbMoney" />
                                        </ext:Column>
                                        <ext:Column ColumnID="COST_ARM" Header="<div style='text-align:center;'>减免成本</div>" Width="90" Align="Right" Sortable="true"
                                            DataIndex="COST_ARM" MenuDisabled="true">
                                            <Renderer Fn="rmbMoney" />
                                        </ext:Column>
                                        <ext:Column ColumnID="COSTS" Header="<div style='text-align:center;'>总成本</div>" Width="90" Align="Right" Sortable="true"
                                            DataIndex="COSTS" MenuDisabled="true">
                                            <Renderer Fn="rmbMoney" />
                                        </ext:Column>
                                        <ext:Column ColumnID="GROSS_INCOME" Header="<div style='text-align:center;'>地方收益</div>" Width="90" Align="Right" Sortable="true"
                                            DataIndex="GROSS_INCOME" MenuDisabled="true">
                                            <Renderer Fn="rmbMoney" />
                                        </ext:Column>
                                        <ext:Column ColumnID="NET_INCOME" Header="<div style='text-align:center;'>收益</div>" Width="90" Align="Right" Sortable="true"
                                            DataIndex="NET_INCOME" MenuDisabled="true">
                                            <Renderer Fn="rmbMoney" />
                                        </ext:Column>
                                        <ext:Column ColumnID="DEPT_LRL" Header="<div style='text-align:center;'>收益率</div>" Width="60" Align="Right" Sortable="true"
                                            DataIndex="DEPT_LRL" MenuDisabled="true" >
                                            <Renderer Fn="pctChange" />
                                        </ext:Column>
                                        <ext:Column ColumnID="DEPT_CBL" Header="<div style='text-align:center;'>成本率</div>" Width="60" Align="Right" Sortable="true"
                                            DataIndex="DEPT_CBL" MenuDisabled="true"  >
                                            <Renderer Fn="pctChange" />
                                        </ext:Column>
                                        <ext:Column ColumnID="DEPT_GDCBL" Header="<div style='text-align:center;'>固定成本率</div>" Width="70" Align="Right" Sortable="true"
                                            DataIndex="DEPT_GDCBL" MenuDisabled="true"  >
                                            <Renderer Fn="pctChange" />
                                        </ext:Column>
                                        <ext:Column ColumnID="DEPT_BDCBL" Header="<div style='text-align:center;'>非固定成本率</div>" Width="100" Align="Right" Sortable="true"
                                            DataIndex="DEPT_BDCBL" MenuDisabled="true"  >
                                            <Renderer Fn="pctChange" />
                                        </ext:Column>
                                        <ext:Column ColumnID="DEPT_YPSRL" Header="<div style='text-align:center;'>药品收入比</div>" Width="70" Align="Right" Sortable="true"
                                            DataIndex="DEPT_YPSRL" MenuDisabled="true"  >
                                            <Renderer Fn="pctChange" />
                                        </ext:Column>
                                    </Columns>
                                </ColumnModel>
                                <SelectionModel>
                                    <ext:RowSelectionModel ID="RowSelectionModel1" runat="server" SingleSelect="true">
                                    </ext:RowSelectionModel>
                                </SelectionModel>
                            </ext:GridPanel>
                        </ext:LayoutColumn>
                    </Columns>
                </ext:ColumnLayout>
            </Body>
        </ext:ViewPort>
    </div>
    </form>
</body>
</html>
