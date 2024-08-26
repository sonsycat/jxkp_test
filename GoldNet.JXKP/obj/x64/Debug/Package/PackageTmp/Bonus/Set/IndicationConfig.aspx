<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="IndicationConfig.aspx.cs"
    Inherits="GoldNet.JXKP.IndicationConfig" %>

<%@ Register Assembly="Goldnet.Ext.Web" Namespace="Goldnet.Ext.Web" TagPrefix="ext" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link rel="stylesheet" type="text/css" href="../Orthers/Cbouns.css" />
</head>
<body>
    <form id="form1" runat="server">
    <ext:ScriptManager ID="ScriptManager1" runat="server" />
    <ext:Store ID="Store1" AutoLoad="true" runat="server">
        <Reader>
            <ext:JsonReader ReaderID="ID">
                <Fields>
                    <ext:RecordField Name="ID">
                    </ext:RecordField>
                    <ext:RecordField Name="NAME">
                    </ext:RecordField>
                    <ext:RecordField Name="REMARK">
                    </ext:RecordField>
                    <ext:RecordField Name="PROPERTYVALUE">
                    </ext:RecordField>
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
                                ClicksToEdit="1" TrackMouseOver="true" Height="480">
                                <TopBar>
                                    <ext:Toolbar ID="Toolbar_detptype" runat="server" Visible="true" AutoWidth="true">
                                        <Items>
                                            <ext:Button ID="bsave" Text="保存" Icon="Disk" runat="server">
                                                <AjaxEvents>
                                                    <Click OnEvent="Save">
                                                        <EventMask Msg="正在保存" ShowMask="true" />
                                                        <ExtraParams>
                                                            <ext:Parameter Name="Values" Value="Ext.encode(#{GridPanel2}.getStore().getChangedData())"
                                                                Mode="Raw" />
                                                        </ExtraParams>
                                                    </Click>
                                                </AjaxEvents>
                                            </ext:Button>
                                        </Items>
                                    </ext:Toolbar>
                                </TopBar>
                                <ColumnModel ID="ColumnModel2" runat="server">
                                    <Columns>
                                        <ext:Column ColumnID="DeptName" Header="指标名称" Width="180" DataIndex="NAME" MenuDisabled="true"
                                            Align="Center">
                                        </ext:Column>
                                        <ext:Column ColumnID="Per" Header="<div style='text-align:center;'>值<div>" Width="120"
                                            DataIndex="PROPERTYVALUE" MenuDisabled="true" Align="Right">
                                            <Editor>
                                                <ext:NumberField runat="server" ID="nfValue" SelectOnFocus="true" DecimalPrecision="2">
                                                </ext:NumberField>
                                            </Editor>
                                        </ext:Column>
                                        <ext:Column ColumnID="Director" Header="备注" Width="600" DataIndex="REMARK" MenuDisabled="true"
                                            Align="Center">
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
