<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="StationsMonthsSet.aspx.cs"
    Inherits="GoldNet.JXKP.jxkh.StationsMonthsSet" %>

<%@ Register Assembly="Goldnet.Ext.Web" Namespace="Goldnet.Ext.Web" TagPrefix="ext" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link rel="stylesheet" type="text/css" href="../../Bonus/Orthers/Cbouns.css" />

    <script language="javascript" type="text/javascript">
        var RefreshData = function() {
            Store1.reload();
        }
    </script>

</head>
<body>
    <form id="form1" runat="server">
    <ext:ScriptManager ID="ScriptManager1" runat="server" AjaxMethodNamespace="CompanyX" />
    <ext:Store ID="Store1" runat="server" OnSubmitData="SubmitData" AutoDataBind="true"
        OnRefreshData="Store_RefreshData" GroupField="BSC_NAME">
        <SortInfo Field="BSC_NAME" Direction="ASC" />
        <SortInfo Field="GUIDE_NAME" Direction="ASC" />
        <Reader>
            <ext:JsonReader>
                <Fields>
                    <ext:RecordField Name="BSC_TYPE" Type="String" Mapping="BSC_TYPE" />
                    <ext:RecordField Name="BSC_NAME" Type="String" Mapping="BSC_NAME" />
                    <ext:RecordField Name="STATION_CODE" Type="String" Mapping="STATION_CODE" />
                    <ext:RecordField Name="GUIDE_CODE" Type="String" Mapping="GUIDE_CODE" />
                    <ext:RecordField Name="GUIDE_NAME" Type="String" Mapping="GUIDE_NAME" />
                    <ext:RecordField Name="STATION_YEAR" Type="String" Mapping="STATION_YEAR" />
                    <ext:RecordField Name="MONTHS1" Type="String" Mapping="MONTHS1" />
                    <ext:RecordField Name="MONTHS2" Type="String" Mapping="MONTHS2" />
                    <ext:RecordField Name="MONTHS3" Type="String" Mapping="MONTHS3" />
                    <ext:RecordField Name="MONTHS4" Type="String" Mapping="MONTHS4" />
                    <ext:RecordField Name="MONTHS5" Type="String" Mapping="MONTHS5" />
                    <ext:RecordField Name="MONTHS6" Type="String" Mapping="MONTHS6" />
                    <ext:RecordField Name="MONTHS7" Type="String" Mapping="MONTHS7" />
                    <ext:RecordField Name="MONTHS8" Type="String" Mapping="MONTHS8" />
                    <ext:RecordField Name="MONTHS9" Type="String" Mapping="MONTHS9" />
                    <ext:RecordField Name="MONTHS10" Type="String" Mapping="MONTHS10" />
                    <ext:RecordField Name="MONTHS11" Type="String" Mapping="MONTHS11" />
                    <ext:RecordField Name="MONTHS12" Type="String" Mapping="MONTHS12" />
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
                                ClicksToEdit="1" TrackMouseOver="true" Height="480" Border="false" AutoScroll="true">
                                <TopBar>
                                    <ext:Toolbar ID="Toolbar_fjsr" runat="server" Visible="true" AutoWidth="true">
                                        <Items>
                                            <ext:Button ID="Button_save" runat="server" Text="保存" Icon="Disk">
                                                <Listeners>
                                                    <Click Handler="#{GridPanel1}.submitData();" />
                                                </Listeners>
                                            </ext:Button>
                                            <ext:Button ID="Button2" runat="server" Text="刷新" Icon="DatabaseGo">
                                                <AjaxEvents>
                                                    <Click OnEvent="Button_refresh_click">
                                                        <EventMask Msg="载入中..." ShowMask="true" />
                                                    </Click>
                                                </AjaxEvents>
                                            </ext:Button>
                                        </Items>
                                    </ext:Toolbar>
                                </TopBar>
                                <ColumnModel ID="ColumnModel1" runat="server">
                                    <Columns>
                                        <ext:Column ColumnID="BSC_NAME" Header="指标分类" Sortable="false" DataIndex="BSC_NAME"
                                            Width="100" />
                                        <ext:Column ColumnID="GUIDE_NAME" Header="<div style='text-align:center;'>指标名称</div>"
                                            Width="150" Align="Left" Sortable="true" DataIndex="GUIDE_NAME" MenuDisabled="true" />
                                        <ext:Column ColumnID="MONTHS1" Header="<div style='text-align:center;'>1月</div>"
                                            Width="60" Align="Right" Sortable="true" DataIndex="MONTHS1" MenuDisabled="true">
                                            <Editor>
                                                <ext:NumberField runat="server" ID="monthsyi" MinValue="0" DecimalPrecision="2">
                                                </ext:NumberField>
                                            </Editor>
                                        </ext:Column>
                                        <ext:Column ColumnID="MONTHS2" Header="<div style='text-align:center;'>2月</div>"
                                            Width="60" Align="Right" Sortable="true" DataIndex="MONTHS2" MenuDisabled="true">
                                            <Editor>
                                                <ext:NumberField runat="server" ID="monthser" MinValue="0" DecimalPrecision="2">
                                                </ext:NumberField>
                                            </Editor>
                                        </ext:Column>
                                        <ext:Column ColumnID="MONTHS3" Header="<div style='text-align:center;'>3月</div>"
                                            Width="60" Align="Right" Sortable="true" DataIndex="MONTHS3" MenuDisabled="true">
                                            <Editor>
                                                <ext:NumberField runat="server" ID="monthssan" MinValue="0" DecimalPrecision="2">
                                                </ext:NumberField>
                                            </Editor>
                                        </ext:Column>
                                        <ext:Column ColumnID="MONTHS4" Header="<div style='text-align:center;'>4月</div>"
                                            Width="60" Align="Right" Sortable="true" DataIndex="MONTHS4" MenuDisabled="true">
                                            <Editor>
                                                <ext:NumberField runat="server" ID="NumberField1" MinValue="0" DecimalPrecision="2">
                                                </ext:NumberField>
                                            </Editor>
                                        </ext:Column>
                                        <ext:Column ColumnID="MONTHS5" Header="<div style='text-align:center;'>5月</div>"
                                            Width="60" Align="Right" Sortable="true" DataIndex="MONTHS5" MenuDisabled="true">
                                            <Editor>
                                                <ext:NumberField runat="server" ID="monthswu" MinValue="0" DecimalPrecision="2">
                                                </ext:NumberField>
                                            </Editor>
                                        </ext:Column>
                                        <ext:Column ColumnID="MONTHS6" Header="<div style='text-align:center;'>6月</div>"
                                            Width="60" Align="Right" Sortable="true" DataIndex="MONTHS6" MenuDisabled="true">
                                            <Editor>
                                                <ext:NumberField runat="server" ID="monthsliu" MinValue="0" DecimalPrecision="2">
                                                </ext:NumberField>
                                            </Editor>
                                        </ext:Column>
                                        <ext:Column ColumnID="MONTHS7" Header="<div style='text-align:center;'>7月</div>"
                                            Width="60" Align="Right" Sortable="true" DataIndex="MONTHS7" MenuDisabled="true">
                                            <Editor>
                                                <ext:NumberField runat="server" ID="monthsqi" MinValue="0" DecimalPrecision="2">
                                                </ext:NumberField>
                                            </Editor>
                                        </ext:Column>
                                        <ext:Column ColumnID="MONTHS8" Header="<div style='text-align:center;'>8月</div>"
                                            Width="60" Align="Right" Sortable="true" DataIndex="MONTHS8" MenuDisabled="true">
                                            <Editor>
                                                <ext:NumberField runat="server" ID="monthsba" MinValue="0" DecimalPrecision="2">
                                                </ext:NumberField>
                                            </Editor>
                                        </ext:Column>
                                        <ext:Column ColumnID="MONTHS9" Header="<div style='text-align:center;'>9月</div>"
                                            Width="60" Align="Right" Sortable="true" DataIndex="MONTHS9" MenuDisabled="true">
                                            <Editor>
                                                <ext:NumberField runat="server" ID="monthsjiu" MinValue="0" DecimalPrecision="2">
                                                </ext:NumberField>
                                            </Editor>
                                        </ext:Column>
                                        <ext:Column ColumnID="MONTHS10" Header="<div style='text-align:center;'>10月</div>"
                                            Width="60" Align="Right" Sortable="true" DataIndex="MONTHS10" MenuDisabled="true">
                                            <Editor>
                                                <ext:NumberField runat="server" ID="monthsshi" MinValue="0" DecimalPrecision="2">
                                                </ext:NumberField>
                                            </Editor>
                                        </ext:Column>
                                        <ext:Column ColumnID="MONTHS11" Header="<div style='text-align:center;'>11月</div>"
                                            Width="60" Align="Right" Sortable="true" DataIndex="MONTHS11" MenuDisabled="true">
                                            <Editor>
                                                <ext:NumberField runat="server" ID="monthssy" MinValue="0" DecimalPrecision="2">
                                                </ext:NumberField>
                                            </Editor>
                                        </ext:Column>
                                        <ext:Column ColumnID="MONTHS12" Header="<div style='text-align:center;'>12月</div>"
                                            Width="60" Align="Right" Sortable="true" DataIndex="MONTHS12" MenuDisabled="true">
                                            <Editor>
                                                <ext:NumberField runat="server" ID="monthsse" MinValue="0" DecimalPrecision="2">
                                                </ext:NumberField>
                                            </Editor>
                                        </ext:Column>
                                    </Columns>
                                </ColumnModel>
                                <View>
                                    <ext:GroupingView runat="server" ID="GroupingView1" ForceFit="false" ShowGroupName="false"
                                        EnableNoGroups="false" HideGroupedColumn="true" GroupTextTpl="" EnableRowBody="true">
                                    </ext:GroupingView>
                                </View>
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
