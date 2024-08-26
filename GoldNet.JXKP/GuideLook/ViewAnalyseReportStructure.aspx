<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ViewAnalyseReportStructure.aspx.cs"
    Inherits="GoldNet.JXKP.GuideLook.ViewAnalyseReportStructure" %>

<%@ Register Assembly="Goldnet.Ext.Web" Namespace="Goldnet.Ext.Web" TagPrefix="ext" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>无标题页</title>
</head>
<body>
    <form id="form1" runat="server">
    <ext:ScriptManager ID="ScriptManager1" runat="server" />
    <ext:Store runat="server" ID="Store1">
        <Reader>
            <ext:JsonReader>
                <Fields>
                    <ext:RecordField Name="GUIDE_CODE" />
                    <ext:RecordField Name="GUIDE_NAME" />
                    <ext:RecordField Name="UNIT_CODE" />
                    <ext:RecordField Name="UNIT_NAME" />
                    <ext:RecordField Name="GUIDE_VALUE" />
                    <ext:RecordField Name="GUIDE_VALUE_YEAR" />
                    <ext:RecordField Name="GUIDE_CAUSE" />
                    <ext:RecordField Name="GUIDE_FACT" />
                    <ext:RecordField Name="GUIDE_RATIO_TONG" />
                    <ext:RecordField Name="GUIDE_VALUE_TONG" />
                    <ext:RecordField Name="GUIDE_RATIO_HUAN" />
                    <ext:RecordField Name="GUIDE_VALUE_HUAN" />
                    <ext:RecordField Name="GUIDE_WCB_MONTH" />
                    <ext:RecordField Name="GUIDE_WCB_YEAR" />
                    <ext:RecordField Name="ORGAN" />
                </Fields>
            </ext:JsonReader>
        </Reader>
    </ext:Store>
    <div>
        <ext:ViewPort ID="ViewPort1" runat="server">
            <Body>
                <ext:BorderLayout ID="BorderLayout2" runat="server">
                    <Center>
                        <ext:GridPanel ID="GridPanel_Show" runat="server" Border="false" StoreID="Store1"
                            StripeRows="true" Height="480" Width="600" AutoScroll="true">
                            <ColumnModel ID="ColumnModel1" runat="server">
                                <Columns>
                                    <ext:RowNumbererColumn Width="32" Resizable="true">
                                    </ext:RowNumbererColumn>
                                 <ext:Column Header="指标名称" Width="90" Align="Left" Sortable="false" MenuDisabled="true"
                                        ColumnID="GUIDE_NAME" DataIndex="GUIDE_NAME" />
                                    <ext:Column Header="指标值" Width="90" Align="Center" Sortable="false" MenuDisabled="true"
                                        ColumnID="GUIDE_VALUE" DataIndex="GUIDE_VALUE" />
                                    <ext:Column Header="年度目标值" Width="90" Align="Center" Sortable="false" MenuDisabled="true"
                                        ColumnID="GUIDE_VALUE_YEAR" DataIndex="GUIDE_VALUE_YEAR" />
                                    <ext:Column Header="同期" Width="90" Align="Left" Sortable="false" MenuDisabled="true"
                                        ColumnID="GUIDE_VALUE_TONG" DataIndex="GUIDE_VALUE_TONG" />
                                    <ext:Column Header="同比" Width="90" Align="Left" Sortable="false" MenuDisabled="true"
                                        ColumnID="GUIDE_RATIO_TONG" DataIndex="GUIDE_RATIO_TONG" />
                                    <ext:Column Header="上月" Width="90" Align="Left" Sortable="false" MenuDisabled="true"
                                        ColumnID="GUIDE_VALUE_HUAN" DataIndex="GUIDE_VALUE_HUAN" />
                                    <ext:Column Header="环比" Width="90" Align="Left" Sortable="false" MenuDisabled="true"
                                        ColumnID="GUIDE_RATIO_HUAN" DataIndex="GUIDE_RATIO_HUAN" />
                                    <ext:Column Header="月份完成比" Width="90" Align="Left" Sortable="false" MenuDisabled="true"
                                        ColumnID="GUIDE_VALUE_HUAN" DataIndex="GUIDE_VALUE_HUAN" />
                                    <ext:Column Header="年度完成比" Width="90" Align="Left" Sortable="false" MenuDisabled="true"
                                        ColumnID="GUIDE_RATIO_HUAN" DataIndex="GUIDE_RATIO_HUAN" />
                                    <ext:Column Header="对象" Width="90" Align="Center" Sortable="false" MenuDisabled="true"
                                        ColumnID="UNIT_NAME" DataIndex="UNIT_NAME" />
                                </Columns>
                            </ColumnModel>
                            <SelectionModel>
                                <ext:RowSelectionModel SingleSelect="true">
                                </ext:RowSelectionModel>
                            </SelectionModel>
                            <LoadMask ShowMask="true" />
                        </ext:GridPanel>
                    </Center>
                </ext:BorderLayout>
            </Body>
        </ext:ViewPort>
    </div>
    </form>
</body>
</html>
