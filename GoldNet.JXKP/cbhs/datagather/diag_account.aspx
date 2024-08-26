<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="diag_account.aspx.cs" Inherits="GoldNet.JXKP.cbhs.datagather.diag_account" %>

<%@ Register Assembly="Goldnet.Ext.Web" Namespace="Goldnet.Ext.Web" TagPrefix="ext" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
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
                <ext:JsonReader ReaderID="DIAGNOSIS_NAME">
                    <Fields>
                        <ext:RecordField Name="DIAGNOSIS_NAME" Type="String" Mapping="DIAGNOSIS_NAME" />
                        <ext:RecordField Name="INCOMES_DF" Type="Float" Mapping="INCOMES_DF" />
                        <ext:RecordField Name="INCOMES_JD" Type="Float" Mapping="INCOMES_JD" />
                        <ext:RecordField Name="INCOMES" Type="Float" Mapping="INCOMES" />
                        <ext:RecordField Name="COSTS_ZJ" Type="Float" Mapping="COSTS_ZJ" />
                        <ext:RecordField Name="COSTS_JJ" Type="Float" Mapping="COSTS_JJ" />
                        <ext:RecordField Name="COSTS" Type="Float" Mapping="COSTS" />
                        <ext:RecordField Name="BENEFIT" Type="Float" Mapping="BENEFIT" />
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
                                    <ext:Toolbar ID="Toolbar1" runat="server">
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
                                            <ext:Button ID="Button_create" runat="server" Text="按病种核算" Icon="DatabaseGo">
                                                <AjaxEvents>
                                                    <Click OnEvent="Button_create_click" Timeout="900000">
                                                        <Confirmation ConfirmRequest="true" Title="系统提示" Message="将按病种核算,<br/>是否继续?" />
                                                        <ExtraParams>
                                                        </ExtraParams>
                                                        <EventMask Msg="载入中..." ShowMask="true" />
                                                    </Click>
                                                </AjaxEvents>
                                            </ext:Button>
                                            <ext:ToolbarSeparator ID="ToolbarSeparator4" runat="server" />
                                            <ext:ToolbarSpacer ID="ToolbarSpacer1" Width="30" runat="server"/>
                                            <ext:Button ID="btn_Excel" runat="server"  OnClick="Button_OutExcel_click" AutoPostBack="true"  Text="导出Excel" Icon="PageWhiteExcel"/>
                                        </Items>
                                    </ext:Toolbar>
                                </TopBar>
                                <ColumnModel ID="ColumnModel1" runat="server">
                                    <Columns>
                                        <ext:Column ColumnID="DIAGNOSIS_NAME" Header="<div style='text-align:center;'>病种名称</div>" Width="90" Align="left" Sortable="true"
                                            DataIndex="DIAGNOSIS_NAME" MenuDisabled="true" />
                                        <ext:Column ColumnID="INCOMES_DF" Header="<div style='text-align:center;'>实际收入</div>" Width="90" Align="Right" Sortable="true"
                                            DataIndex="INCOMES_DF" MenuDisabled="true" >
                                            <Renderer Fn="rmbMoney" />
                                        </ext:Column>
                                        <ext:Column ColumnID="INCOMES_JD" Header="<div style='text-align:center;'>减免收入</div>" Width="90" Align="Right" Sortable="true"
                                            DataIndex="INCOMES_JD" MenuDisabled="true" >
                                            <Renderer Fn="rmbMoney" />
                                        </ext:Column>
                                        <ext:Column ColumnID="INCOMES" Header="<div style='text-align:center;'>总收入</div>" Width="90" Align="Right" Sortable="true"
                                            DataIndex="INCOMES" MenuDisabled="true" >
                                            <Renderer Fn="rmbMoney" />
                                        </ext:Column>
                                        <ext:Column ColumnID="COSTS_ZJ" Header="<div style='text-align:center;'>直接成本</div>" Width="90" Align="Right" Sortable="true"
                                            DataIndex="COSTS_ZJ" MenuDisabled="true" >
                                            <Renderer Fn="rmbMoney" />
                                        </ext:Column>
                                        <ext:Column ColumnID="COSTS_JJ" Header="<div style='text-align:center;'>间接成本</div>" Width="90" Align="Right" Sortable="true"
                                            DataIndex="COSTS_JJ" MenuDisabled="true" >
                                            <Renderer Fn="rmbMoney" />
                                        </ext:Column>
                                        <ext:Column ColumnID="COSTS" Header="<div style='text-align:center;'>总成本</div>" Width="90" Align="Right" Sortable="true"
                                            DataIndex="COSTS" MenuDisabled="true" >
                                            <Renderer Fn="rmbMoney" />
                                        </ext:Column>
                                        <ext:Column ColumnID="BENEFIT" Header="<div style='text-align:center;'>效益</div>" Width="90" Align="Right" Sortable="true"
                                            DataIndex="BENEFIT" MenuDisabled="true" >
                                            <Renderer Fn="rmbMoney" />
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
