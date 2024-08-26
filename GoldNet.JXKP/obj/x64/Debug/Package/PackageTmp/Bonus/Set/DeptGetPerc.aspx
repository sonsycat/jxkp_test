<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DeptGetPerc.aspx.cs" Inherits="GoldNet.JXKP.DeptGetPerc" %>

<%@ Register Assembly="Goldnet.Ext.Web" Namespace="Goldnet.Ext.Web" TagPrefix="ext" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link rel="stylesheet" type="text/css" href="../Orthers/Cbouns.css" />

    <script type="text/javascript">
        function rset() {
            var rcount = Store1.getTotalCount();
            for (var i = 0; i < rcount; i++) {
                var record = Store1.getAt(i);
                record.data["PERCENT"] = 0;
                record.data["DIRECTOR"] = 0;
                record.data["FUNDPERCENT"] = 0;
                record.data["REMITPERCENT"] = 0;
                record.data["SECDEPTFOUND"] = 0;
                record.data["JC_BZ"] = 0;
                record.data["JC_BZ_XY"] = 0;
                GridPanel2.getView().refreshRow(record);
            }
        }    
    </script>

</head>
<body>
    <form id="form1" runat="server">
    <ext:ScriptManager ID="ScriptManager1" runat="server" />
    <ext:Store ID="Store1" AutoLoad="true" runat="server">
        <Reader>
            <ext:JsonReader ReaderID="DEPTCODE">
                <Fields>
                    <ext:RecordField Name="DEPTCODE">
                    </ext:RecordField>
                    <ext:RecordField Name="DEPTNAME">
                    </ext:RecordField>
                    <ext:RecordField Name="PERCENT">
                    </ext:RecordField>
                    <ext:RecordField Name="DIRECTOR">
                    </ext:RecordField>
                    <ext:RecordField Name="FUNDPERCENT">
                    </ext:RecordField>
                    <ext:RecordField Name="REMITPERCENT">
                    </ext:RecordField>
                    <ext:RecordField Name="SECDEPTFOUND">
                    </ext:RecordField>
                    <ext:RecordField Name="JC_BZ">
                    </ext:RecordField>
                    <ext:RecordField Name="JC_BZ_XY">
                    </ext:RecordField>
                    <ext:RecordField Name="GZBZ">
                    </ext:RecordField>
                    <ext:RecordField Name="JMCB">
                    </ext:RecordField>
                </Fields>
            </ext:JsonReader>
        </Reader>
    </ext:Store>
    <ext:Store ID="Store3" runat="server">
        <Reader>
            <ext:JsonReader ReaderID="YEAR">
                <Fields>
                    <ext:RecordField Name="YEAR" />
                </Fields>
            </ext:JsonReader>
        </Reader>
    </ext:Store>
    <ext:Store ID="Store4" runat="server">
        <Reader>
            <ext:JsonReader ReaderID="MONTH">
                <Fields>
                    <ext:RecordField Name="MONTH" />
                </Fields>
            </ext:JsonReader>
        </Reader>
    </ext:Store>
    <div>
        <ext:ViewPort ID="ViewPort1" runat="server">
            <Body>
                <ext:ColumnLayout ID="ColumnLayout1" runat="server" Split="true" FitHeight="true">
                    <Columns>
                        <ext:LayoutColumn ColumnWidth="1">
                            <ext:GridPanel ID="GridPanel2" runat="server" Border="false" StoreID="Store1" StripeRows="true"
                                TrackMouseOver="true" Height="480" ClicksToEdit="1">
                                <TopBar>
                                    <ext:Toolbar ID="Toolbar_detptype" runat="server" Visible="true" AutoWidth="true">
                                        <Items>
                                            <ext:Label ID="lcaption" runat="server" Text="核算年月份:">
                                            </ext:Label>
                                            <ext:ComboBox ID="cbbYear" runat="server" ReadOnly="true" StoreID="Store3" Width="70"
                                                DisplayField="YEAR" ValueField="YEAR">
                                            </ext:ComboBox>
                                            <ext:Label ID="lYear" runat="server" Text="年">
                                            </ext:Label>
                                            <ext:ComboBox ID="cbbmonth" runat="server" ReadOnly="true" StoreID="Store4" Width="70"
                                                DisplayField="MONTH" ValueField="MONTH">
                                            </ext:ComboBox>
                                            <ext:Label ID="lmonth" runat="server" Text="月">
                                            </ext:Label>
                                            <ext:ToolbarSeparator ID="ToolbarSeparator4" runat="server" />
                                            <ext:Button ID="bquery" Text="查询" Icon="FolderMagnify" runat="server">
                                                <AjaxEvents>
                                                    <Click OnEvent="Query">
                                                    </Click>
                                                </AjaxEvents>
                                            </ext:Button>
                                            <ext:Button ID="bsave" Text="保存" Icon="Disk" runat="server">
                                                <AjaxEvents>
                                                    <Click OnEvent="Save">
                                                        <EventMask Msg="正在保存" ShowMask="true" />
                                                        <ExtraParams>
                                                            <ext:Parameter Name="Values" Value="Ext.encode(#{GridPanel2}.getRowsValues(false))"
                                                                Mode="Raw" />
                                                        </ExtraParams>
                                                    </Click>
                                                </AjaxEvents>
                                            </ext:Button>
                                            <ext:Button ID="bReset" Text="重置" runat="server" Icon="Plugin">
                                                <Listeners>
                                                    <Click Fn="rset" />
                                                </Listeners>
                                            </ext:Button>
                                        </Items>
                                    </ext:Toolbar>
                                </TopBar>
                                <ColumnModel ID="ColumnModel2" runat="server">
                                    <Columns>
                                        <ext:Column ColumnID="DeptName" Header="<center>科室名称</center>" Width="150" DataIndex="DEPTNAME" MenuDisabled="true"
                                            Align="Left">
                                        </ext:Column>
                                         <ext:Column ColumnID="GZBZ" Header="<center>工资补助</center>" Width="110"
                                            DataIndex="GZBZ" MenuDisabled="true" Align="Right">
                                            <Editor>
                                                <ext:NumberField ID="NumberField3" runat="server" SelectOnFocus="true" DecimalPrecision="2">
                                                </ext:NumberField>
                                            </Editor>
                                        </ext:Column>
                                         <ext:Column ColumnID="JMCB" Header="<center>减免成本</center>" Width="110"
                                            DataIndex="JMCB" MenuDisabled="true" Align="Right">
                                            <Editor>
                                                <ext:NumberField ID="NumberField4" runat="server" SelectOnFocus="true" DecimalPrecision="2">
                                                </ext:NumberField>
                                            </Editor>
                                        </ext:Column>
                                        <ext:Column ColumnID="Per" Header="<center>提成比</center>" Width="110"
                                            DataIndex="PERCENT" MenuDisabled="true" Align="Right">
                                            <Editor>
                                                <ext:NumberField ID="nfpercent" runat="server" SelectOnFocus="true" DecimalPrecision="2">
                                                </ext:NumberField>
                                            </Editor>
                                        </ext:Column>
                                        <ext:Column ColumnID="Director" Header="<center>床日标准</center>"
                                            Width="110" DataIndex="DIRECTOR" Align="Right" MenuDisabled="true">
                                            <Editor>
                                                <ext:NumberField ID="nfdirector" runat="server" SelectOnFocus="true" DecimalPrecision="2">
                                                </ext:NumberField>
                                            </Editor>
                                        </ext:Column>
                                        <ext:Column ColumnID="DeptFound" Header="<center>床日效益标准</center>"
                                            Width="110" DataIndex="FUNDPERCENT" Align="Right" MenuDisabled="true">
                                            <Editor>
                                                <ext:NumberField ID="nfdeptfound" runat="server" SelectOnFocus="true" DecimalPrecision="2">
                                                </ext:NumberField>
                                            </Editor>
                                        </ext:Column>
                                        <ext:Column ColumnID="DeptSecFound" Header="<center>门诊标准</center>"
                                            Width="110" DataIndex="SECDEPTFOUND" Align="Right" MenuDisabled="true">
                                            <Editor>
                                                <ext:NumberField ID="nfSecFound" runat="server" SelectOnFocus="true" DecimalPrecision="2">
                                                </ext:NumberField>
                                            </Editor>
                                        </ext:Column>
                                        <ext:Column ColumnID="Remit" Header="<center>门诊效益标准</center>"
                                            Width="110" DataIndex="REMITPERCENT" Align="Right" MenuDisabled="true">
                                            <Editor>
                                                <ext:NumberField ID="nfRemit" runat="server" SelectOnFocus="true" DecimalPrecision="2">
                                                </ext:NumberField>
                                            </Editor>
                                        </ext:Column>
                                        <ext:Column ColumnID="JC_BZ" Header="<center>检查标准</center>"
                                            Width="110" DataIndex="JC_BZ" Align="Right" MenuDisabled="true">
                                            <Editor>
                                                <ext:NumberField ID="NumberField1" runat="server" SelectOnFocus="true" DecimalPrecision="2">
                                                </ext:NumberField>
                                            </Editor>
                                        </ext:Column>
                                        <ext:Column ColumnID="JC_BZ_XY" Header="<center>检查效益标准</center>"
                                            Width="110" DataIndex="JC_BZ_XY" Align="Right" MenuDisabled="true">
                                            <Editor>
                                                <ext:NumberField ID="NumberField2" runat="server" SelectOnFocus="true" DecimalPrecision="2">
                                                </ext:NumberField>
                                            </Editor>
                                        </ext:Column>
                                    </Columns>
                                </ColumnModel>
                                <SelectionModel>
                                    <ext:RowSelectionModel ID="rowselection" SingleSelect="true">
                                    </ext:RowSelectionModel>
                                </SelectionModel>
                                <LoadMask ShowMask="true" />
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
