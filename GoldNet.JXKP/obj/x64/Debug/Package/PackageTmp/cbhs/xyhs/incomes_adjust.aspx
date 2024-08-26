<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="incomes_adjust.aspx.cs" Inherits="GoldNet.JXKP.cbhs.xyhs.incomes_adjust" %>
<%@ Register Assembly="Goldnet.Ext.Web" Namespace="Goldnet.Ext.Web" TagPrefix="ext" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title></title>
    <style type="text/css">
        body
        {
            background-color: #DFE8F6;
            font-size: 12px;
        }
    </style>

    <script language="javascript" type="text/javascript">
        var RefreshData = function(msg,year,month) {
            years.setValue(year);
            months.setValue(month);
            Ext.Msg.alert('提示',msg);
            Store1.reload();
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
       function edit(data_id)
       {
           Goldnet.AjaxMethod.request( 'data_edit', {params: {rowsid:data_id}});
       }
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <ext:ScriptManager ID="ScriptManager1" runat="server" AjaxMethodNamespace="CompanyX" />
    <ext:Store ID="Store1" runat="server" OnRefreshData="Store_RefreshData">
        <Reader>
            <ext:JsonReader ReaderID="ID">
                <Fields>
                    <ext:RecordField Name="ID" Type="String" Mapping="ID" />
                    <ext:RecordField Name="ST_DATE" Type="String" Mapping="ST_DATE" />
                    <ext:RecordField Name="DEPT_CODE" Type="String" Mapping="DEPT_CODE" />
                    <ext:RecordField Name="DEPT_NAME" Type="String" Mapping="DEPT_NAME" />
                    <ext:RecordField Name="INCOMES_ADJUST" Type="String" Mapping="INCOMES_ADJUST" />
                    <ext:RecordField Name="INCOMES_DIFFERENCE" Type="String" Mapping="INCOMES_DIFFERENCE" />
                    
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
                                            FieldLabel="年" Hidden="true">
                                            <AjaxEvents>
                                                <Select OnEvent="Date_SelectOnChange">
                                                    <EventMask Msg='载入中...' ShowMask="true" />
                                                </Select>
                                            </AjaxEvents>
                                        </ext:ComboBox>
                                        <ext:ComboBox ID="months" runat="server" Width="60" AllowBlank="true" EmptyText="请选择月..."
                                            FieldLabel="月" Hidden="true">
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
                                        
                                        
                                        <ext:ToolbarSeparator ID="ToolbarSeparator1" runat="server" Hidden="true" />
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
                                                    <ExtraParams>
                                                        <ext:Parameter Name="Values" Value="Ext.encode(#{GridPanel1}.getRowsValues())" Mode="Raw">
                                                        </ext:Parameter>
                                                    </ExtraParams>
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
                                    </Items>
                                </ext:Toolbar>
                            </TopBar>
                            <ColumnModel ID="ColumnModel1" runat="server">
                                <Columns>
                                    <ext:Column ColumnID="ID" Hidden="true" />
                                    <ext:Column ColumnID="ST_DATE" Header="<div style='text-align:center;'>日期</div>" Width="90" Align="left" Sortable="true"
                                        DataIndex="ST_DATE" MenuDisabled="true" />
                                    <ext:Column ColumnID="DEPT_CODE" Header="<div style='text-align:center;'>科室编码</div>" Width="90" Align="Right" Sortable="true"
                                        DataIndex="DEPT_CODE" MenuDisabled="true">
                                        <Renderer Fn="rmbMoney" />
                                    </ext:Column>
                                    <ext:Column ColumnID="DEPT_NAME" Header="<div style='text-align:center;'>科室名称</div>" Width="90" Align="left" Sortable="true"
                                        DataIndex="DEPT_NAME" MenuDisabled="true" />
                                    <ext:Column ColumnID="INCOMES_ADJUST" Header="<div style='text-align:center;'>调整金额</div>" Width="90" Align="Right" Sortable="true"
                                        DataIndex="INCOMES_ADJUST" MenuDisabled="true">
                                        <Renderer Fn="rmbMoney" />
                                    </ext:Column>
                                    <ext:Column ColumnID="INCOMES_DIFFERENCE" Header="<div style='text-align:center;'>差值</div>" Width="90" Align="left" Sortable="true"
                                        DataIndex="INCOMES_DIFFERENCE" MenuDisabled="true" />
                                    <ext:CommandColumn Width="60" Header="<div style='text-align:center;'>操作</div>">
                                        <Commands>
                                            <ext:GridCommand Icon="NoteEdit" Text="<div style='text-align:center;'>编辑</div>" CommandName="Edit">
                                                <ToolTip Text="编辑" />
                                            </ext:GridCommand>
                                        </Commands>
                                    </ext:CommandColumn>
                                </Columns>
                            </ColumnModel>
                            <SelectionModel>
                                <ext:CheckboxSelectionModel ID="RowSelectionModel1" runat="server">
                                    <Listeners>
                                        <SelectionChange Handler="#{GridPanel1}.hasSelection()? #{Button_del}.setDisabled(false): #{Button_del}.setDisabled(true);" />
                                    </Listeners>
                                </ext:CheckboxSelectionModel>
                            </SelectionModel>
                            <LoadMask ShowMask="true" />
                            <BottomBar>
                                <ext:PagingToolbar ID="PagingToolBar1" runat="server" PageSize="20" StoreID="Store1" />
                            </BottomBar>
                            <Listeners>
                                <Command Handler="edit(record.data.ID);" />
                            </Listeners>
                        </ext:GridPanel>
                    </ext:LayoutColumn>
                </Columns>
            </ext:ColumnLayout>
        </Body>
    </ext:ViewPort>
    <ext:Window ID="DetailWin" runat="server" Icon="Group" Title="附加收入" Width="370" Height="410"
        AutoShow="false" Modal="true" CenterOnLoad="true" AutoScroll="true" ShowOnLoad="false"
        Resizable="true" StyleSpec="background-color:Transparent;" BodyStyle="background-color:Transparent;">
    </ext:Window>
    </form>
</body>
</html>
