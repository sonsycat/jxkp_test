<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="xyhs_item_type_sfyl.aspx.cs"
    Inherits="GoldNet.JXKP.cbhs.xyhs.xyhsdict.xyhs_item_type_sfyl" %>

<%@ Register Assembly="Goldnet.Ext.Web" Namespace="Goldnet.Ext.Web" TagPrefix="ext" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
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
    <ext:Store ID="Store1" runat="server" >
        <Reader>
            <ext:JsonReader ReaderID="ITEM_CODE">
                <Fields>
                    <ext:RecordField Name="ITEM_CODE" Type="String" Mapping="ITEM_CODE" />
                    <ext:RecordField Name="ITEM_NAME" Type="String" Mapping="ITEM_NAME" />
                    <ext:RecordField Name="ROOT" Type="String" Mapping="ROOT" />
                    <ext:RecordField Name="DEPNAME" Type="String" Mapping="DEPNAME" />
                    <ext:RecordField Name="SFYL" Type="String" Mapping="SFYL" />
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
                            <ext:GridPanel ID="GridPanel1" runat="server" StoreID="Store1" StripeRows="true"
                                ClicksToEdit="1" TrackMouseOver="true" AutoWidth="true" Height="480" Border="false">
                                <TopBar>
                                    <ext:Toolbar ID="Toolbar_fjsr" runat="server" Visible="true" AutoWidth="true">
                                        <Items>
                                            <ext:Button ID="Button_save" runat="server" Text="保存" Icon="Disk">
                                                <AjaxEvents>
                                                <Click OnEvent="Button_save_Click">
                                                    <ExtraParams>
                                                        <ext:Parameter Name="Values" Value="Ext.encode(#{GridPanel1}.getRowsValues(false))"
                                                            Mode="Raw">
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
                                        <ext:Column ColumnID="ITEM_CODE" Header="<div style='text-align:center;'>编码</div>"
                                            Width="90" Align="Left" Sortable="true" DataIndex="ITEM_CODE" MenuDisabled="true">
                                        </ext:Column>
                                        <ext:Column ColumnID="ITEM_NAME" Header="<div style='text-align:center;'>名称</div>"
                                            Width="90" Align="Left" Sortable="true" DataIndex="DEPT_CODE" MenuDisabled="true"
                                            Hidden="true" />
                                        <ext:Column ColumnID="ROOT" Header="<div style='text-align:center;'>跟节点</div>" Width="130"
                                            Align="Left" Sortable="true" DataIndex="ROOT" MenuDisabled="true">
                                        </ext:Column>
                                        <ext:Column ColumnID="DEPNAME" Header="<div style='text-align:center;'>目录</div>"
                                            Width="200" Align="Left" Sortable="true" DataIndex="DEPNAME" MenuDisabled="true">
                                        </ext:Column>
                                        <ext:Column ColumnID="SFYL" Header="类别(0:医疗成本 1:全成本)" Width="150" Align="left" Sortable="false"
                                            DataIndex="SFYL" MenuDisabled="true">
                                            <Editor>
                                                <ext:NumberField runat="server" ID="nfsfyl" MinValue="0" DecimalPrecision="0"></ext:NumberField>
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
