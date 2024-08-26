<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="xyhs_dept_item_account.aspx.cs" Inherits="GoldNet.JXKP.cbhs.xyhs.xyhs_dept_item_account" %>
<%@ Register Assembly="Goldnet.Ext.Web" Namespace="Goldnet.Ext.Web" TagPrefix="ext" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
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
        <ext:Store ID="Store1" runat="server">
            <Reader>
                <ext:JsonReader ReaderID="ITEM_CODE">
                    <Fields>
                        <ext:RecordField Name="ITEM_CODE" Type="String" Mapping="ITEM_CODE" />
                        <ext:RecordField Name="ITEM_NAME" Type="String" Mapping="ITEM_NAME" />
                        <ext:RecordField Name="INCOMES" Type="Float" Mapping="INCOMES" />
                        <ext:RecordField Name="COSTS" Type="Float" Mapping="COSTS" />
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
                                                    <Select OnEvent="Data_SelectOnChange">
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
                                                    <Select OnEvent="Data_SelectOnChange">
                                                        <EventMask Msg='载入中...' ShowMask="true" />
                                                    </Select>
                                                </AjaxEvents>
                                            </ext:ComboBox>
                                            <ext:ToolbarTextItem ID="ToolbarTextItem1" runat="server" Text="月 " />
                                            <ext:ToolbarSeparator ID="ToolbarSeparator3" runat="server" />
                                            <ext:Label ID="func" runat="server" Text="科室：" Width="40" Hidden="true">
                                            </ext:Label>
                                            <ext:ComboBox ID="Combo_DeptType" runat="server" AllowBlank="true" EmptyText="请选择科室"
                                                Width="100"  Hidden="true">
                                            </ext:ComboBox>
                                            <ext:ToolbarSeparator ID="ToolbarSeparator2" runat="server" />
                                            <ext:Button ID="Button_find" runat="server" Text="查询" Icon="DatabaseGo">
                                                <AjaxEvents>
                                                    <Click OnEvent="Button_find_click">
                                                        <ExtraParams>
                                                        </ExtraParams>
                                                        <EventMask Msg="载入中..." ShowMask="true" />
                                                    </Click>
                                                </AjaxEvents>
                                            </ext:Button>
                                            <ext:ToolbarSeparator ID="ToolbarSeparator1" runat="server" />
                                            <ext:Button ID="Button_create" runat="server" Text="项目核算" Icon="ArrowOut">
                                                <AjaxEvents>
                                                    <Click OnEvent="Button_create_click">
                                                        <Confirmation ConfirmRequest="true" Title="系统提示" Message="将进行项目核算,<br/>是否继续?" />
                                                        <ExtraParams>
                                                        </ExtraParams>
                                                        <EventMask Msg="载入中..." ShowMask="true" />
                                                    </Click>
                                                </AjaxEvents>
                                            </ext:Button>
                                        </Items>
                                    </ext:Toolbar>
                                </TopBar>
                                <ColumnModel ID="ColumnModel1" runat="server">
                                    <Columns>
                                        <ext:Column ColumnID="ITEM_CODE" Header="<div style='text-align:center;'>核算项目代码</div>" Width="120" Align="Left" Sortable="true"
                                            DataIndex="ITEM_CODE" MenuDisabled="true" />
                                        <ext:Column ColumnID="ITEM_NAME" Header="<div style='text-align:center;'>核算项目名称</div>" Width="120" Align="Left" Sortable="true"
                                            DataIndex="ITEM_NAME" MenuDisabled="true" />
                                        <ext:Column ColumnID="INCOMES" Header="<div style='text-align:center;'>项目收入</div>" Width="120" Align="Right" Sortable="true"
                                            DataIndex="INCOMES" MenuDisabled="true">
                                            <Renderer Fn="rmbMoney" />
                                        </ext:Column>
                                        <ext:Column ColumnID="COSTS" Header="<div style='text-align:center;'>项目成本</div>" Width="120" Align="Right" Sortable="true"
                                            DataIndex="COSTS" MenuDisabled="true">
                                            <Renderer Fn="rmbMoney" />
                                        </ext:Column>
                        
                                         <ext:CommandColumn Width="60" Align="Center" Header="<div style='text-align:center;'>明细</div>" >
                                            <Commands>
                                                <ext:GridCommand Icon="FolderTable" CommandName="DETAIL"  >
                                                    <ToolTip Text="查看明细数据" />
                                                </ext:GridCommand>
                                            </Commands>
                                        </ext:CommandColumn>
                                        <ext:CommandColumn Width="60" Align="Center" Header="<div style='text-align:center;'>构成比</div>"  Hidden="true">
                                            <Commands>
                                                <ext:GridCommand Icon="CogStart" CommandName="SCALE"  >
                                                    <ToolTip Text="查看构成比" />
                                                </ext:GridCommand>
                                            </Commands>
                                        </ext:CommandColumn>
                                    </Columns>
                                </ColumnModel>
                                <SelectionModel>
                                    <ext:RowSelectionModel ID="RowSelectionModel1" runat="server" SingleSelect="true">
                                    </ext:RowSelectionModel>
                                </SelectionModel>
                                <AjaxEvents>
                                    <Command   OnEvent ="Btn_Command_Click"   > 
                                        <ExtraParams>
                                            <ext:Parameter Name="item_code" Value="record.data.ITEM_CODE" Mode="Raw">
                                            </ext:Parameter>
                                            <ext:Parameter Name="item_name" Value="record.data.ITEM_NAME" Mode="Raw">
                                            </ext:Parameter>
                                            <ext:Parameter Name="command" Value="command" Mode="Raw">
                                            </ext:Parameter>
                                        </ExtraParams>
                                    </Command>
                                </AjaxEvents>
                                 <BottomBar>
                                    <ext:PagingToolbar ID="PagingToolBar2" runat="server" PageSize="25" StoreID="Store1"
                                        AutoWidth="true" DisplayInfo="false" AutoDataBind="true">
                                        
                                    </ext:PagingToolbar>
                                </BottomBar>
                            </ext:GridPanel>
                        </ext:LayoutColumn>
                    </Columns>
                </ext:ColumnLayout>
            </Body>
        </ext:ViewPort>
        <ext:Window ID="DetailWin" runat="server" Icon="Group" Title="详细信息" Width="530" Height="460"
            AutoShow="false" Modal="true" CenterOnLoad="true" AutoScroll="true" ShowOnLoad="false"
            Resizable="true" StyleSpec="background-color:Transparent;" BodyStyle="background-color:Transparent;">
        </ext:Window>
        <ext:Window ID="ScaleWin" runat="server" Icon="Group" Title="成本构成比" Width="550" Height="460"
            AutoShow="false" Modal="true" CenterOnLoad="true" AutoScroll="true" ShowOnLoad="false"
            Resizable="true" StyleSpec="background-color:Transparent;" BodyStyle="background-color:Transparent;">
        </ext:Window>
    </div>
    </form>
</body>
</html>

