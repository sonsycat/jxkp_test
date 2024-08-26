<%@ Page Language="C#" CodeBehind="main_kpi.aspx.cs" Inherits="GoldNet.JXKP.mainpage.main_kpi" %>

<%@ Register Assembly="Goldnet.Ext.Web" Namespace="Goldnet.Ext.Web" TagPrefix="ext" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <ext:ScriptManager ID="ScriptManager1" runat="server" />
    <ext:Store runat="server" ID="Store1">
        <Reader>
            <ext:JsonReader>
                <Fields>
                    <ext:RecordField Name="ZBL" />
                    <ext:RecordField Name="ZBDM" />
                    <ext:RecordField Name="ZBMC" />
                    <ext:RecordField Name="MBZ" />
                    <ext:RecordField Name="WCZ" />
                    <ext:RecordField Name="WCBFB" />
                </Fields>
            </ext:JsonReader>
        </Reader>
    </ext:Store>
    <div>
        <ext:Panel ID="Panel1" runat="server" Border="false" MonitorResize="true">
            <Body>
                <br /><br/>
                <img src='/mainpage/TempImages/GaugePic.png?temp=<%= DateTime.Now.Ticks.ToString()%>'
                    alt="指标进度" width="321" height="181" />
                <br />
                <ext:FitLayout ID="FitLayout1" runat="server">
                    <ext:GridPanel ID="GridPanel_Show" runat="server" StoreID="Store1" Border="false"
                        AutoWidth="true" AutoHeight="true" Title="" MonitorResize="true" MonitorWindowResize="true"
                        StripeRows="true" AutoExpandColumn="zbmc">
                        <ColumnModel ID="ColumnModel1" runat="server">
                            <Columns>
                                <ext:Column Header="指标名称" Width="180" Align="Center" MenuDisabled="true" Sortable="false"
                                    ColumnID="zbmc" DataIndex="ZBMC" />
                                <ext:Column Header="目标值" Width="69" Align="Center" MenuDisabled="true" Sortable="false"
                                    ColumnID="mbz" DataIndex="MBZ" />
                                <ext:Column Header="完成值" Width="69" Align="Center" MenuDisabled="true" Sortable="false"
                                    ColumnID="wcz" DataIndex="WCZ" />
                                <ext:Column Header="完成比" Width="58" Align="Center" MenuDisabled="true" Sortable="false"
                                    ColumnID="wcbfb" DataIndex="WCBFB">
                                    <Renderer Handler="return String.format('{0}%',parseFloat(record.data['WCBFB']*100).toFixed(2) );" />
                                </ext:Column>
                            </Columns>
                        </ColumnModel>
                        <SelectionModel>
                            <ext:RowSelectionModel ID="RowSelectionModel1" runat="server" />
                        </SelectionModel>
                        <LoadMask ShowMask="true" />
                    </ext:GridPanel>
                </ext:FitLayout>
            </Body>
        </ext:Panel>
    </div>
    </form>
</body>
</html>
