<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="xyhs_dept_info.aspx.cs" Inherits="GoldNet.JXKP.cbhs.xyhs.xyhs_dept_info" %>
<%@ Register Assembly="Goldnet.Ext.Web" Namespace="Goldnet.Ext.Web" TagPrefix="ext" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title></title>
    <script type="text/javascript">

        var typeTargetRenderer = function(value) {
            var r = Store2.getById(value);
            if (Ext.isEmpty(r)) {
                return "";
            }
            return r.data.PROG_NAME;
        };
         function FormatRender(v, p, record, rowIndex) {
            var colors = ["red", "black"];
            if(record.data.DEPT_PERSON_COUNT==""||record.data.DEPT_AREA==""||record.data.DEPT_EQUIPMENT_COUNT==""||record.data.DEC_SCHEME=="")
            {
            var template = '<span style="color:{0};">{1}</span>';
            return String.format(template, colors[1], record.data.DEPT_NAME);
            }
            else
            {
            var templateb = '<span style="color:{0};">{1}</span>';
            return String.format(templateb, colors[1], record.data.DEPT_NAME);
            }
        }
    </script>
</head>
<body>
<ext:ScriptManager ID="ScriptManager1" runat="server" AjaxMethodNamespace="CompanyX" />
    <ext:Store ID="Store1" runat="server" OnSubmitData="SubmitData">
        <Reader>
            <ext:JsonReader ReaderID="DEPT_CODE" >
                <Fields>
                    <ext:RecordField Name="DEPT_CODE" Type="String" Mapping="DEPT_CODE" />
                    <ext:RecordField Name="DEPT_NAME" Type="String" Mapping="DEPT_NAME" />
                    <ext:RecordField Name="DEPT_TYPE" Type="String" Mapping="DEPT_TYPE" />
                    <ext:RecordField Name="INPUT_CODE" Type="String" Mapping="INPUT_CODE" />
                    <ext:RecordField Name="SORT_NO" Type="String" Mapping="SORT_NO" />
                    <ext:RecordField Name="DEPT_PERSON_COUNT" Type="String" Mapping="DEPT_PERSON_COUNT" />
                    <ext:RecordField Name="DEPT_AREA" Type="String" Mapping="DEPT_AREA" />
                    <ext:RecordField Name="DEPT_EQUIPMENT_COUNT" Type="String" Mapping="DEPT_EQUIPMENT_COUNT" />
                    <ext:RecordField Name="DEC_SCHEME" Type="String" Mapping="DEC_SCHEME" />
                </Fields>
            </ext:JsonReader>
        </Reader>
    </ext:Store>
    <ext:Store ID="Store2" runat="server">
        <Reader>
            <ext:JsonReader ReaderID="PROG_CODE" >
                <Fields>
                    <ext:RecordField Name="PROG_CODE" Type="String" Mapping="PROG_CODE" />
                    <ext:RecordField Name="PROG_NAME" Type="String" Mapping="PROG_NAME" />
                </Fields>
            </ext:JsonReader>
        </Reader>
    </ext:Store>
    <form id="form1" runat="server">
    <div>
    <ext:ViewPort ID="ViewPort1" runat="server">
            <Body>
                <ext:ColumnLayout ID="ColumnLayout1" runat="server" Split="true" FitHeight="true">
                    <Columns>
                        <ext:LayoutColumn ColumnWidth="1">
                            <ext:GridPanel ID="GridPanel1" runat="server" StoreID="Store1" StripeRows="true" ClicksToEdit="1"
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
                                            <ext:Button ID="Button_save" runat="server" Text="保存" Icon="Disk">
                                                <Listeners>
                                                    <Click Handler="#{GridPanel1}.submitData();" />
                                                </Listeners>
                                            </ext:Button>
                                        </Items>
                                    </ext:Toolbar>
                                </TopBar>
                                <ColumnModel ID="ColumnModel1" runat="server">
                                    <Columns>
                                        <ext:Column ColumnID="DEPT_NAME" Header="<div style='text-align:center;'>科室名称</div>" Width="90" Align="Left" Sortable="true"
                                            DataIndex="DEPT_NAME" MenuDisabled="true" >
                                            <Renderer Fn="FormatRender" />
                                        </ext:Column>
                                        <ext:Column ColumnID="DEPT_CODE" Header="<div style='text-align:center;'>科室代码</div>" Width="90" Align="Left" Sortable="true"
                                            DataIndex="DEPT_CODE" MenuDisabled="true" />
                                         <ext:Column ColumnID="DEPT_TYPE" Hidden="true" DataIndex="DEPT_TYPE"></ext:Column> 
                                         <ext:Column ColumnID="INOUT_CODE" Hidden="true" DataIndex="INOUT_CODE"></ext:Column> 
                                         <ext:Column ColumnID="SORT_NO" Hidden="true" DataIndex="SORT_NO"></ext:Column> 
                                        <ext:Column ColumnID="DEPT_PERSON_COUNT" Header="<div style='text-align:center;'>人数</div>" Width="80"  Align="Right" Sortable="true" 
                                            DataIndex="DEPT_PERSON_COUNT" MenuDisabled="true">
                                            <Editor>
                                                <ext:NumberField runat="server" ID="nfPersonCount" MinValue="0" DecimalPrecision="2"></ext:NumberField>
                                            </Editor>
                                        </ext:Column>
                                        <ext:Column ColumnID="DEPT_AREA" Header="<div style='text-align:center;'>面积</div>" Width="80"  Align="Right" Sortable="true" 
                                            DataIndex="DEPT_AREA" MenuDisabled="true">
                                            <Editor>
                                                <ext:NumberField runat="server" ID="nfDeptArea" MinValue="0" DecimalPrecision="2"></ext:NumberField>
                                            </Editor>
                                        </ext:Column>
                                        <ext:Column ColumnID="DEPT_EQUIPMENT_COUNT" Header="<div style='text-align:center;'>设备总值</div>" Width="80"  Align="Right" Sortable="true" 
                                            DataIndex="DEPT_EQUIPMENT_COUNT" MenuDisabled="true">
                                            <Editor>
                                                <ext:NumberField runat="server" ID="nfEquicount" MinValue="0" DecimalPrecision="2"></ext:NumberField>
                                            </Editor>
                                        </ext:Column>
                                        <ext:Column ColumnID="DEC_SCHEME" Header="<div style='text-align:center;'>公摊方案</div>" Width="200"  Align="Left" Sortable="true" 
                                            DataIndex="DEC_SCHEME" MenuDisabled="true" Hidden="true" >
                                            <Renderer Fn="typeTargetRenderer" />
                                            <Editor>
                                                <ext:ComboBox ID="DEPT_PROG" runat="server"  TriggerAction="All"  DataIndex="PROG_CODE"  Editable="false"
                                                    StoreID="Store2" DisplayField="PROG_NAME" ValueField="PROG_CODE" >
                                            </ext:ComboBox>
                                            </Editor>
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
