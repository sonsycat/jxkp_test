<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Assess_ResultInfo.aspx.cs"
    Inherits="GoldNet.JXKP.jxkh.Assess_ResultInfo" %>

<%@ Register Assembly="Goldnet.Ext.Web" Namespace="Goldnet.Ext.Web" TagPrefix="ext" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <ext:ScriptManager ID="ScriptManager1" runat="server" />
        <ext:Store runat="server" ID="Store1" GroupField="BSC_CLASS_NAME">
            <Reader>
                <ext:JsonReader>
                    <Fields>
                        <ext:RecordField Name="BSC_CLASS_NAME" />
                        <ext:RecordField Name="GUIDE_NAME" />
                        <ext:RecordField Name="GUIDE_VALUE" />
                        <ext:RecordField Name="GUIDE_CAUSE" />
                        <ext:RecordField Name="GUIDE_FACT" />
                        <ext:RecordField Name="GUIDE_I_VALUE" />
                        <ext:RecordField Name="GUIDE_D_VALUE" />
                        <ext:RecordField Name="GUIDE_F_VALUE" />
                    </Fields>
                </ext:JsonReader>
            </Reader>
        </ext:Store>
        <ext:ViewPort ID="ViewPort1" runat="server">
            <Body>
                <ext:GridPanel ID="GridPanel1" runat="server" BodyBorder="false" AutoScroll="true"
                    Border="false" StoreID="Store1" StripeRows="true" TrackMouseOver="true" Height="470"
                    AutoWidth="true">
                    <ColumnModel ID="ColumnModel1" runat="server">
                        <Columns>
                            <ext:RowNumbererColumn Width="30" />
                            <ext:Column Header="类别" ColumnID="BSC_CLASS_NAME" DataIndex="BSC_CLASS_NAME" Width="90"
                                MenuDisabled="true" />
                            <ext:Column Header="指标名" Width="140" ColumnID="GUIDE_NAME" DataIndex="GUIDE_NAME"
                                MenuDisabled="true" />
                            <ext:Column Header="指标分值" Width="80" ColumnID="GUIDE_VALUE" DataIndex="GUIDE_VALUE"
                                MenuDisabled="true" />
                            <ext:Column Header="目标值" Width="80" ColumnID="GUIDE_CAUSE" DataIndex="GUIDE_CAUSE"
                                MenuDisabled="true" />
                            <ext:Column Header="实际完成值" Width="80" ColumnID="GUIDE_FACT" DataIndex="GUIDE_FACT"
                                MenuDisabled="true" />
                            <ext:Column Header="加分" Width="80" ColumnID="GUIDE_I_VALUE" DataIndex="GUIDE_I_VALUE"
                                MenuDisabled="true" />
                            <ext:Column Header="扣分" Width="80" ColumnID="GUIDE_D_VALUE" DataIndex="GUIDE_D_VALUE"
                                MenuDisabled="true" />
                            <ext:Column Header="最后得分" Width="80" ColumnID="GUIDE_F_VALUE" DataIndex="GUIDE_F_VALUE"
                                MenuDisabled="true" />
                        </Columns>
                    </ColumnModel>
                    <SelectionModel>
                        <ext:RowSelectionModel ID="RowSelectionModel1" runat="server" SingleSelect="true">
                        </ext:RowSelectionModel>
                    </SelectionModel>
                    <View>
                        <ext:GroupingView ID="GroupingView1" HideGroupedColumn="true" runat="server" GroupTextTpl='{text} ({[values.rs.length]})'
                            EnableRowBody="false">
                        </ext:GroupingView>
                    </View>
                    <BottomBar>
                        <ext:Toolbar ID="Toolbar2" runat="server">
                            <Items>
                                <ext:ToolbarFill ID="ToolbarFill2" runat="server" />
                                <ext:ToolbarButton ID="Btn_BatCancel" runat="server" Icon="Cancel" Text="退出">
                                    <Listeners>
                                        <Click Handler="parent.ResultInfoWin.hide();" />
                                    </Listeners>
                                </ext:ToolbarButton>
                            </Items>
                        </ext:Toolbar>
                    </BottomBar>
                </ext:GridPanel>
            </Body>
        </ext:ViewPort>
    </div>
    </form>
</body>
</html>
