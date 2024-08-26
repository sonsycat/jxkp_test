<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Hospital_costs_list.aspx.cs" Inherits="GoldNet.JXKP.cbhs.datagather.Hospital_costs_list" %>
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
        var RefreshData = function() {
            
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
                    <ext:RecordField Name="DEPT_CODE" Type="String" Mapping="DEPT_CODE" />
                    <ext:RecordField Name="DEPT_NAME" Type="String" Mapping="DEPT_NAME" />
                    <ext:RecordField Name="COST_NAME" Type="String" Mapping="COST_NAME" />
                    <ext:RecordField Name="COSTS" Type="String" Mapping="COSTS" />
                    <ext:RecordField Name="TO_COST_CODE" Type="String" Mapping="TO_COST_CODE" />
                    <ext:RecordField Name="TO_COST_NAME" Type="String" Mapping="TO_COST_NAME" />
                    <ext:RecordField Name="HOS_PROG_CODE" Type="String" Mapping="HOS_PROG_CODE" />
                    <ext:RecordField Name="HOS_PROG_NAME" Type="String" Mapping="HOS_PROG_NAME" />
                    <ext:RecordField Name="DEPT_PROG_CODE" Type="String" Mapping="DEPT_PROG_CODE" />
                    <ext:RecordField Name="DEPT_PROG_NAME" Type="String" Mapping="DEPT_PROG_NAME" />
                    <ext:RecordField Name="OPERATOR" Type="String" Mapping="OPERATOR" />
                    <ext:RecordField Name="MEMO" Type="String" Mapping="MEMO" />
                    
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
                                        <ext:ToolbarSeparator ID="ToolbarSeparator4" runat="server" />
                                        <ext:Button ID="Button1" runat="server" Text="成本分解" Icon="DatabaseGo">
                                            <AjaxEvents>
                                                <Click OnEvent="Button_decompose_click" Timeout="300000">
                                                <Confirmation ConfirmRequest="true" Title="系统提示" Message="分解将覆盖原来的成本数据,<br/>是否继续?" />
                                                    <EventMask Msg="成本分解中..." ShowMask="true" />
                                                </Click>
                                            </AjaxEvents>
                                        </ext:Button>
                                        <ext:ToolbarSeparator ID="ToolbarSeparator5" runat="server" />
                                        <ext:Button ID="Button2" runat="server" Text="分解查看" Icon="DatabaseGo">
                                            <AjaxEvents>
                                                <Click OnEvent="Button_decompose_look">
                                                    <EventMask Msg="载入中..." ShowMask="true" />
                                                </Click>
                                            </AjaxEvents>
                                        </ext:Button>
                                    </Items>
                                </ext:Toolbar>
                            </TopBar>
                            <ColumnModel ID="ColumnModel1" runat="server">
                                <Columns>
                                    <ext:Column ColumnID="ID" Hidden="true" />
                                    <ext:Column ColumnID="DEPT_NAME" Header="<div style='text-align:center;'>科室</div>" Width="90" Align="left" Sortable="true"
                                        DataIndex="DEPT_NAME" MenuDisabled="true" />
                                    <ext:Column ColumnID="COST_NAME" Header="<div style='text-align:center;'>成本名称</div>" Width="90" Align="left" Sortable="true"
                                        DataIndex="COST_NAME" MenuDisabled="true">
                                        
                                    </ext:Column>
                                    <ext:Column ColumnID="COSTS" Header="<div style='text-align:center;'>成本金额</div>" Width="90" Align="Right" Sortable="true"
                                        DataIndex="COSTS" MenuDisabled="true">
                                        <Renderer Fn="rmbMoney" />
                                    </ext:Column>
                                    <ext:Column ColumnID="TO_COST_NAME" Header="<div style='text-align:center;'>成本项目</div>" Width="90" Align="left" Sortable="true"
                                        DataIndex="TO_COST_NAME" MenuDisabled="true" />
                                    <ext:Column ColumnID="HOS_PROG_NAME" Header="<div style='text-align:center;'>院分解方案</div>" Width="90" Align="left" Sortable="true"
                                        DataIndex="HOS_PROG_NAME" MenuDisabled="true"  Hidden="true"/>
                                    <ext:Column ColumnID="DEPT_PROG_NAME" Header="<div style='text-align:center;'>科分解方案</div>" Width="90" Align="left" Sortable="true"
                                        DataIndex="DEPT_PROG_NAME" MenuDisabled="true" />
                                    <ext:Column ColumnID="OPERATOR" Header="<div style='text-align:center;'>操作人</div>" Width="90" Align="left" Sortable="true"
                                        DataIndex="OPERATOR" MenuDisabled="true" />
                                    <ext:Column ColumnID="MEMO" Header="<div style='text-align:center;'>备注</div>" Width="80" Align="left" Sortable="true"
                                        DataIndex="MEMO" MenuDisabled="true" />
                                    
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
    <ext:Window ID="DetailWin" runat="server" Icon="Group" Title="分摊成本编辑" Width="450" Height="350"
        AutoShow="false" Modal="true" CenterOnLoad="true" AutoScroll="true" ShowOnLoad="false"
        Resizable="true" StyleSpec="background-color:Transparent;" BodyStyle="background-color:Transparent;">
    </ext:Window>
     <ext:Window ID="Hospital_Detail" runat="server" Icon="Group" Title="分摊成本详细" Width="600" Height="400"
        AutoShow="false" Modal="true" CenterOnLoad="true" AutoScroll="true" ShowOnLoad="false"
        Resizable="true" StyleSpec="background-color:Transparent;" BodyStyle="background-color:Transparent;">
    </ext:Window>
    </form>
</body>
</html>
