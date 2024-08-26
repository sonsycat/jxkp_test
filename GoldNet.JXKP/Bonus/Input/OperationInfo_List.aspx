<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="OperationInfo_List.aspx.cs"
    Inherits="GoldNet.JXKP.Bonus.Input.OperationInfo_List" %>

<%@ Register Assembly="Goldnet.Ext.Web" Namespace="Goldnet.Ext.Web" TagPrefix="ext" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
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
        function edit(data_id) {
            Goldnet.AjaxMethod.request('data_edit', { params: { rowsid: data_id} });
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
                    <ext:RecordField Name="ID" Mapping="ID" />
                    <ext:RecordField Name="ST_DATE" Mapping="ST_DATE" />
                    <ext:RecordField Name="DEPT_CODE" Mapping="DEPT_CODE" />
                    <ext:RecordField Name="DEPT_NAME" Mapping="DEPT_NAME" />
                    <ext:RecordField Name="LEVEL_J" Mapping="LEVEL_J" />
                    <ext:RecordField Name="OPERATOR" Mapping="OPERATOR" />
                    <ext:RecordField Name="OPERATOR_NAME" Mapping="OPERATOR_NAME" />
                    <ext:RecordField Name="FIRST_ASSISTANT" Mapping="FIRST_ASSISTANT" />
                    <ext:RecordField Name="FIRST_ASSISTANT_N" Mapping="FIRST_ASSISTANT_N" />
                    <ext:RecordField Name="SECOND_ASSISTANT" Mapping="SECOND_ASSISTANT" />
                    <ext:RecordField Name="SECOND_ASSISTANT_N" Mapping="SECOND_ASSISTANT_N" />
                    <ext:RecordField Name="ANESTHESIA_DOCTOR" Mapping="ANESTHESIA_DOCTOR" />
                    <ext:RecordField Name="ANESTHESIA_DOCTOR_N" Mapping="ANESTHESIA_DOCTOR_N" />
                    <ext:RecordField Name="EMERGENCY" Mapping="EMERGENCY" />
                    <ext:RecordField Name="HS1" Mapping="HS1" />
                    <ext:RecordField Name="HS_NAME1" Mapping="HS_NAME1" />
                    <ext:RecordField Name="HS2" Mapping="HS2" />
                    <ext:RecordField Name="HS_NAME2" Mapping="HS_NAME2" />
                    <ext:RecordField Name="HS3" Mapping="HS3" />
                    <ext:RecordField Name="HS_NAME3" Mapping="HS_NAME3" />
                    <ext:RecordField Name="HS4" Mapping="HS4" />
                    <ext:RecordField Name="HS_NAME4" Mapping="HS_NAME4" />
                    <ext:RecordField Name="HS5" Mapping="HS5" />
                    <ext:RecordField Name="HS_NAME5" Mapping="HS_NAME5" />
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
                                        <ext:ToolbarSeparator ID="ToolbarSeparator4" runat="server" />
                                        <ext:Button ID="Btn_Edit" Text="编辑" Icon="NoteEdit" runat="server" Disabled="true" Hidden="true">
                                                <AjaxEvents>
                                                    <Click OnEvent="Btn_Edit_Click">
                                                        <ExtraParams>
                                                            <ext:Parameter Name="Values" Value="Ext.encode(#{GridPanel1}.getRowsValues())" Mode="Raw" />
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
                                    <ext:Column ColumnID="ST_DATE" Header="<div style='text-align:center;'>日期</div>"
                                        Width="90" Align="left" Sortable="true" DataIndex="ST_DATE" MenuDisabled="true" />
                                    <ext:Column ColumnID="DEPT_CODE" Header="<div style='text-align:center;'>科室名称</div>"
                                        Width="90" Align="Right" Sortable="true" DataIndex="DEPT_CODE" MenuDisabled="true" >
                                    </ext:Column>
                                    <ext:Column ColumnID="DEPT_NAME" Header="<div style='text-align:center;'>科室代码</div>"
                                        Width="90" Align="Right" Sortable="true" DataIndex="DEPT_NAME" MenuDisabled="true" Hidden="true">
                                    </ext:Column>
                                    <ext:Column ColumnID="LEVEL_J" Header="<div style='text-align:center;'>手术级别</div>"
                                        Width="90" Align="left" Sortable="true" DataIndex="LEVEL_J" MenuDisabled="true" />
                                    <ext:Column ColumnID="OPERATOR" Header="<div style='text-align:center;'>手术医生</div>"
                                        Width="90" Align="left" Sortable="true" DataIndex="OPERATOR" MenuDisabled="true"  />
                                    <ext:Column ColumnID="OPERATOR_NAME" Header="<div style='text-align:center;'>手术医生名</div>"
                                        Width="90" Align="left" Sortable="true" DataIndex="OPERATOR_NAME" MenuDisabled="true" Hidden="true" />
                                    <ext:Column ColumnID="FIRST_ASSISTANT" Header="<div style='text-align:center;'>第一助手</div>"
                                        Width="80" Align="left" Sortable="true" DataIndex="FIRST_ASSISTANT" MenuDisabled="true"  />
                                    <ext:Column ColumnID="FIRST_ASSISTANT_N" Header="<div style='text-align:center;'>第一助手名</div>"
                                        Width="80" Align="left" Sortable="true" DataIndex="FIRST_ASSISTANT_N" MenuDisabled="true" Hidden="true" />
                                    <ext:Column ColumnID="SECOND_ASSISTANT" Header="<div style='text-align:center;'>第二助手</div>"
                                        Width="80" Align="left" Sortable="true" DataIndex="SECOND_ASSISTANT" MenuDisabled="true" />
                                    <ext:Column ColumnID="SECOND_ASSISTANT_N" Header="<div style='text-align:center;'>第二助手名</div>"
                                        Width="90" Align="left" Sortable="true" DataIndex="SECOND_ASSISTANT_N" MenuDisabled="true" Hidden="true" />
                                    <ext:Column ColumnID="ANESTHESIA_DOCTOR" Header="<div style='text-align:center;'>麻醉医师</div>"
                                        Width="120" Align="left" Sortable="true" DataIndex="ANESTHESIA_DOCTOR" MenuDisabled="true"  />
                                    <ext:Column ColumnID="ANESTHESIA_DOCTOR_N" Header="<div style='text-align:center;'>麻醉医师名</div>"
                                        Width="80" Align="left" Sortable="true" DataIndex="ANESTHESIA_DOCTOR_N" MenuDisabled="true" Hidden="true" />
                                    <ext:Column ColumnID="EMERGENCY" Header="<div style='text-align:center;'>急诊</div>"
                                        Width="80" Align="left" Sortable="true" DataIndex="EMERGENCY" MenuDisabled="true" />
                                    <ext:Column ColumnID="HS1" Header="<div style='text-align:center;'>护士1</div>" Width="80"
                                        Align="left" Sortable="true" DataIndex="HS1" MenuDisabled="true"  />
                                    <ext:Column ColumnID="HS_NAME1" Header="<div style='text-align:center;'>护士名</div>"
                                        Width="90" Align="left" Sortable="true" DataIndex="HS_NAME1" MenuDisabled="true" Hidden="true" />
                                    <ext:Column ColumnID="HS2" Header="<div style='text-align:center;'>护士2</div>" Width="120"
                                        Align="left" Sortable="true" DataIndex="HS2" MenuDisabled="true"  />
                                    <ext:Column ColumnID="HS_NAME2" Header="<div style='text-align:center;'>护士名2</div>"
                                        Width="120" Align="left" Sortable="true" DataIndex="HS_NAME2" MenuDisabled="true" Hidden="true" />
                                    <ext:Column ColumnID="HS3" Header="<div style='text-align:center;'>护士3</div>" Width="80"
                                        Align="left" Sortable="true" DataIndex="HS3" MenuDisabled="true"  />
                                    <ext:Column ColumnID="HS_NAME3" Header="<div style='text-align:center;'>护士名3</div>"
                                        Width="90" Align="left" Sortable="true" DataIndex="HS_NAME3" MenuDisabled="true" Hidden="true" />
                                    <ext:Column ColumnID="HS4" Header="<div style='text-align:center;'>护士4</div>" Width="120"
                                        Align="left" Sortable="true" DataIndex="HS4" MenuDisabled="true"  />
                                    <ext:Column ColumnID="HS_NAME4" Header="<div style='text-align:center;'>护士名4</div>"
                                        Width="120" Align="left" Sortable="true" DataIndex="HS_NAME4" MenuDisabled="true" Hidden="true" />
                                    <ext:Column ColumnID="HS5" Header="<div style='text-align:center;'>护士5</div>" Width="80"
                                        Align="left" Sortable="true" DataIndex="HS5" MenuDisabled="true"  />
                                    <ext:Column ColumnID="HS_NAME5" Header="<div style='text-align:center;'>护士名5</div>"
                                        Width="90" Align="left" Sortable="true" DataIndex="HS_NAME5" MenuDisabled="true" Hidden="true" />
                                </Columns>
                            </ColumnModel>
                            <SelectionModel>
                                <ext:CheckboxSelectionModel ID="RowSelectionModel1" runat="server">
                                    <Listeners>
                                        <SelectionChange Handler="if (!#{GridPanel1}.hasSelection()) {#{Button_del}.disable();#{Btn_Edit}.disable();} else {#{Btn_Edit}.enable();#{Button_del}.enable(); }" />
                                    </Listeners>
                                </ext:CheckboxSelectionModel>
                            </SelectionModel>
                            <LoadMask ShowMask="true" />
                            <BottomBar>
                                <ext:PagingToolbar ID="PagingToolBar1" runat="server" PageSize="20" StoreID="Store1" />
                            </BottomBar>
                            <Listeners>
                                <Command Handler="edit(record.data.ROW_ID);" />
                            </Listeners>
                        </ext:GridPanel>
                    </ext:LayoutColumn>
                </Columns>
            </ext:ColumnLayout>
        </Body>
    </ext:ViewPort>
    <ext:Window ID="DetailWin" runat="server" Icon="Group" Title="手术信息" Width="390" Height="450"
        AutoShow="false" Modal="true" CenterOnLoad="true" AutoScroll="true" ShowOnLoad="false"
        Resizable="true" StyleSpec="background-color:Transparent;" BodyStyle="background-color:Transparent;">
    </ext:Window>
    </form>
</body>
</html>
