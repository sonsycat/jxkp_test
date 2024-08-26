<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="dept_info.aspx.cs" Inherits="GoldNet.JXKP.cbhs.cbhsdict.dept_info" %>

<%@ Register Assembly="Goldnet.Ext.Web" Namespace="Goldnet.Ext.Web" TagPrefix="ext" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <link rel="stylesheet" type="text/css" href="../../Bonus/Orthers/Cbouns.css" />
</head>
<body>
    <form id="form1" runat="server">
    <ext:ScriptManager ID="ScriptManager1" runat="server" AjaxMethodNamespace="CompanyX" />
    <ext:Store ID="Store1" runat="server" OnSubmitData="SubmitData">
        <Reader>
            <ext:JsonReader ReaderID="DEPT_CODE">
                <Fields>
                    <ext:RecordField Name="DEPT_CODE" Type="String" Mapping="DEPT_CODE" />
                    <ext:RecordField Name="DEPT_NAME" Type="String" Mapping="DEPT_NAME" />
                    <ext:RecordField Name="DEPT_PERSON_COUNT" Type="String" Mapping="DEPT_PERSON_COUNT" />
                    <ext:RecordField Name="DEPT_AREA" Type="String" Mapping="DEPT_AREA" />
                    <ext:RecordField Name="DEPT_EQUIPMENT_COUNT" Type="String" Mapping="DEPT_EQUIPMENT_COUNT" />
                    <ext:RecordField Name="DEPT_COOPERATION_INCOMS" Type="String" Mapping="DEPT_COOPERATION_INCOMS" />
                    <ext:RecordField Name="DEPT_COOPERATION_BENEFITS" Type="String" Mapping="DEPT_COOPERATION_BENEFITS" />
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
                            <ext:GridPanel ID="GridPanel1" runat="server" StoreID="Store1" StripeRows="true"
                                ClicksToEdit="1" TrackMouseOver="true" AutoWidth="true" Height="480" Border="false">
                                <TopBar>
                                    <ext:Toolbar ID="Toolbar_fjsr" runat="server" Visible="true" AutoWidth="true">
                                        <Items>
                                            <ext:ComboBox ID="cbbYear" runat="server" ReadOnly="true" StoreID="Store3" Width="70"
                                                DisplayField="YEAR" ValueField="YEAR" ForceSelection="true" SelectOnFocus="true">
                                            </ext:ComboBox>
                                            <ext:Label ID="lYear" runat="server" Text="年">
                                            </ext:Label>
                                            <ext:ComboBox ID="cbbmonth" runat="server" ReadOnly="true" StoreID="Store4" Width="70"
                                                DisplayField="MONTH" ValueField="MONTH">
                                            </ext:ComboBox>
                                            <ext:Label ID="lmonth" runat="server" Text="月">
                                            </ext:Label>
                                            <ext:Button ID="Button_look" runat="server" Text="查询" Icon="DatabaseGo">
                                                <AjaxEvents>
                                                    <Click OnEvent="Button_look_click" Timeout="99999999">
                                                        <EventMask Msg="载入中..." ShowMask="true" />
                                                    </Click>
                                                </AjaxEvents>
                                            </ext:Button>
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
                                        <ext:Column ColumnID="DEPT_NAME" Header="<div style='text-align:center;'>科室名称</div>"
                                            Width="130" Align="Left" Sortable="true" DataIndex="DEPT_NAME" MenuDisabled="true" />
                                        <ext:Column ColumnID="DEPT_CODE" Header="<div style='text-align:center;'>科室代码</div>"
                                            Width="90" Align="Left" Sortable="true" DataIndex="DEPT_CODE" MenuDisabled="true" />
                                        <%--<ext:Column ColumnID="DEPT_PERSON_COUNT" Header="<div style='text-align:center;'>面积(全)</div>"
                                            Width="80" Align="Right" Sortable="true" DataIndex="DEPT_PERSON_COUNT" MenuDisabled="true">
                                            <Editor>
                                                <ext:NumberField runat="server" ID="nfPersonCount" MinValue="0" DecimalPrecision="2">
                                                </ext:NumberField>
                                            </Editor>
                                        </ext:Column>--%>
                                        <ext:Column ColumnID="DEPT_AREA" Header="<div style='text-align:center;'>面积(东，南，西，门诊)</div>"
                                            Width="130" Align="Right" Sortable="true" DataIndex="DEPT_AREA" MenuDisabled="true">
                                            <Editor>
                                                <ext:NumberField runat="server" ID="nfDeptArea" MinValue="0" DecimalPrecision="2">
                                                </ext:NumberField>
                                            </Editor>
                                        </ext:Column>
                                        <ext:Column ColumnID="DEPT_EQUIPMENT_COUNT" Header="<div style='text-align:center;'>设备总值</div>"
                                            Width="80" Align="Right" Sortable="true" DataIndex="DEPT_EQUIPMENT_COUNT" MenuDisabled="true">
                                            <Editor>
                                                <ext:NumberField runat="server" ID="nfEquicount" MinValue="0" DecimalPrecision="2">
                                                </ext:NumberField>
                                            </Editor>
                                        </ext:Column>
                                        <ext:Column ColumnID="DEPT_COOPERATION_INCOMS" Header="<div style='text-align:center;'>合作分配比例（收入）</div>"
                                            Width="200" Align="Right" Sortable="true" DataIndex="DEPT_COOPERATION_INCOMS"
                                            MenuDisabled="true">
                                            <Editor>
                                                <ext:NumberField runat="server" ID="nfcoopeartion" MinValue="0" DecimalPrecision="2">
                                                </ext:NumberField>
                                            </Editor>
                                        </ext:Column>
                                        <ext:Column ColumnID="DEPT_COOPERATION_BENEFITS" Header="<div style='text-align:center;'>合作分配比例（效益)</div>"
                                            Width="200" Align="Right" Sortable="true" DataIndex="DEPT_COOPERATION_BENEFITS"
                                            MenuDisabled="true">
                                            <Editor>
                                                <ext:NumberField runat="server" ID="nfcoopeation" MinValue="0" DecimalPrecision="2">
                                                </ext:NumberField>
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
