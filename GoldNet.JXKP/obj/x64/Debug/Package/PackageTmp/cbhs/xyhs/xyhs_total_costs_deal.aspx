<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="xyhs_total_costs_deal.aspx.cs" Inherits="GoldNet.JXKP.cbhs.xyhs.xyhs_total_costs_deal" %>
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
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    <ext:ScriptManager ID="ScriptManager1" runat="server" AjaxMethodNamespace="CompanyX" />
        <ext:Store ID="Store1" runat="server" OnSubmitData="SubmitData" OnRefreshData="Data_RefreshData"
            WarningOnDirty="false">
            <Reader>
                <ext:JsonReader ReaderID="DEPT_CODE">
                    <Fields>
                        <ext:RecordField Name="DEPT_CODE" Type="String" Mapping="DEPT_CODE" />
                        <ext:RecordField Name="DEPT_NAME" Type="String" Mapping="DEPT_NAME" />
                        <ext:RecordField Name="TOTAL_COSTS" Type="String" Mapping="TOTAL_COSTS" />
                        <ext:RecordField Name="COSTS" Type="Float" Mapping="COSTS" />
                        <ext:RecordField Name="COSTS_ARMYFREE" Type="Float" Mapping="COSTS_ARMYFREE" />
                        <ext:RecordField Name="MEMO" Type="String" Mapping="MEMO" />
                    </Fields>
                </ext:JsonReader>
            </Reader>
        </ext:Store>
        <ext:Store ID="Store2" runat="server">
            <Reader>
                <ext:JsonReader ReaderID="ITEM_CODE">
                    <Fields>
                        <ext:RecordField Name="ITEM_CODE" Type="String" Mapping="ITEM_CODE" />
                        <ext:RecordField Name="ITEM_NAME" Type="String" Mapping="ITEM_NAME" />
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
                                            <ext:Label ID="Label3" runat="server" Text="成本项目：" Width="60" />
                                            <ext:ComboBox ID="COST_ITEM" StoreID="Store2" DisplayField="ITEM_NAME" ValueField="ITEM_CODE" runat="server" Width="100" AllowBlank="true" EmptyText="请选择...">
                                                <AjaxEvents>
                                                    <Select OnEvent="Item_SelectOnChange">
                                                        <EventMask Msg='载入中...' ShowMask="true" />
                                                    </Select>
                                                </AjaxEvents>
                                            </ext:ComboBox>
                                            <ext:ToolbarSeparator ID="ToolbarSeparator2" runat="server" />
                                            <ext:Button ID="Button_look" runat="server" Text="查询" Icon="DatabaseGo">
                                                <AjaxEvents>
                                                    <Click OnEvent="Button_look_click">
                                                        <EventMask Msg="载入中..." ShowMask="true" />
                                                    </Click>
                                                </AjaxEvents>
                                            </ext:Button>
                                            <ext:ToolbarSeparator ID="ToolbarSeparator3" runat="server" />
                                            <ext:Button ID="Button_del" runat="server" Text="删除" Icon="Delete" Disabled="true">
                                                <AjaxEvents>
                                                    <Click OnEvent="Button_del_click">
                                                        <Confirmation ConfirmRequest="true" Title="系统提示" Message="将删除选中数据,<br/>是否继续?" />
                                                        <ExtraParams>
                                                        </ExtraParams>
                                                        <EventMask Msg="载入中..." ShowMask="true" />
                                                        <ExtraParams>
                                                            <ext:Parameter Name="Values" Value="Ext.encode(#{GridPanel1}.getRowsValues())" Mode="Raw">
                                                            </ext:Parameter>
                                                        </ExtraParams>
                                                    </Click>
                                                </AjaxEvents>
                                            </ext:Button>
                                            <ext:ToolbarSeparator ID="ToolbarSeparator4" runat="server" />
                                            <ext:Button ID="Button_save" runat="server" Text="保存" Icon="Disk">
                                                <Listeners>
                                                    <Click Handler="#{GridPanel1}.submitData();" />
                                                </Listeners>
                                            </ext:Button>
                                            <ext:ToolbarSeparator ID="ToolbarSeparator7" runat="server" />

                                            <ext:ToolbarSpacer ID="ToolbarSpacer1" Width="30" runat="server"></ext:ToolbarSpacer>
                                            
                                            <ext:Label ID="Label5" runat="server" Text="成本总额：" Width="60" />
                                            <ext:NumberField ID="AllCosts" runat="server" Width="80"/>
                                            <ext:ToolbarSeparator ID="ToolbarSeparator6" runat="server" />
                                            <ext:Button ID="Button_break" runat="server" Text="分摊" Icon="ArrowOut">
                                                <AjaxEvents>
                                                    <Click OnEvent="Button_break_click">
                                                        <Confirmation ConfirmRequest="true" Title="系统提示" Message="分摊将覆盖当前数据,<br/>是否继续?" />
                                                        <ExtraParams>
                                                        </ExtraParams>
                                                        <EventMask Msg="载入中..." ShowMask="true" />
                                                    </Click>
                                                </AjaxEvents>
                                            </ext:Button>
                                            <ext:ToolbarSeparator ID="ToolbarSeparator5" runat="server" />

                                        </Items>
                                    </ext:Toolbar>
                                </TopBar>
                                <ColumnModel ID="ColumnModel1" runat="server">
                                <Columns>
                                     <ext:Column ColumnID="DEPT_CODE" Header="<div style='text-align:center;'>科室代码</div>" Width="130" Align="left" Sortable="false"
                                        Hidden="true" DataIndex="DEPT_CODE" MenuDisabled="true" />
                                    <ext:Column ColumnID="DEPT_NAME" Header="<div style='text-align:center;'>科室</div>" Width="130" Align="left" Sortable="false"
                                        DataIndex="DEPT_NAME" MenuDisabled="true" />
                                    <ext:Column ColumnID="TOTAL_COSTS" Header="<div style='text-align:center;'>成本额</div>" Width="130" Align="Right" Sortable="false"
                                        DataIndex="TOTAL_COSTS" MenuDisabled="true">
                                        <Editor>
                                            <ext:NumberField ID="NumberField1" runat="server" />
                                        </Editor>
                                        <Renderer Fn="rmbMoney" />
                                    </ext:Column>
                                    <ext:Column ColumnID="COSTS" Header="<div style='text-align:center;'>实际成本</div>" Width="130" Align="Right" Sortable="false"
                                        DataIndex="COSTS" MenuDisabled="true" >
                                        <Renderer Fn="rmbMoney" />
                                    </ext:Column>
                                    <ext:Column ColumnID="COSTS_ARMYFREE" Header="<div style='text-align:center;'>减免成本</div>" Width="130" Align="Right" Sortable="false"
                                        DataIndex="COSTS_ARMYFREE" MenuDisabled="true" >
                                        <Renderer Fn="rmbMoney" />
                                    </ext:Column>
                                    <ext:Column ColumnID="MEMO" Header="<div style='text-align:center;'>备注</div>" Width="150" Align="left" Sortable="false"
                                        DataIndex="MEMO" MenuDisabled="true">
                                        <Editor>
                                            <ext:NumberField ID="NumberField2" runat="server" />
                                        </Editor>
                                    </ext:Column>
                                    </Columns>
                                </ColumnModel>
                                <SelectionModel>
                                    <ext:CheckboxSelectionModel ID="RowSelectionModel1" runat="server">
                                    </ext:CheckboxSelectionModel>
                                </SelectionModel>
                            </ext:GridPanel>
                        </ext:LayoutColumn>
                    </Columns>
                </ext:ColumnLayout>
            </Body>
        </ext:ViewPort>
    </div>
    </form>
    </div>
    </form>
</body>
</html>
