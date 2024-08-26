<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="BonusBalanceSearch.aspx.cs"
    Inherits="GoldNet.JXKP.Bonus.Set.BonusBalanceSearch" %>

<%@ Register Assembly="Goldnet.Ext.Web" Namespace="Goldnet.Ext.Web" TagPrefix="ext" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <link rel="stylesheet" type="text/css" href="../../Bonus/Orthers/Cbouns.css" />

    <script language="javascript" type="text/javascript">
        var RefreshData = function(msg) {
            Ext.Msg.show({ title: '提示', msg: msg, icon: 'ext-mb-info', buttons: { ok: true} });
            Store1.reload();
        }
            
    </script>

</head>
<body>
    <ext:ScriptManager ID="ScriptManager1" runat="server" />
    <form id="form1" runat="server">
    <ext:Store ID="Store1" AutoLoad="true" runat="server">
        <Reader>
            <ext:JsonReader ReaderID="DEPTCODE">
                <Fields>
                    <ext:RecordField Name="DEPTCODE">
                    </ext:RecordField>
                    <ext:RecordField Name="DEPTNAME">
                    </ext:RecordField>
                    <ext:RecordField Name="BYFH">
                    </ext:RecordField>
                    <ext:RecordField Name="BYFD">
                    </ext:RecordField>
                    <ext:RecordField Name="SYLC">
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
                            <ext:GridPanel ID="GridPanel1" runat="server" StoreID="Store1" StripeRows="true"
                                ClicksToEdit="1" TrackMouseOver="true" AutoWidth="true" Height="480" Border="false">
                                <TopBar>
                                    <ext:Toolbar ID="Toolbar_fjsr" runat="server" Visible="true" AutoWidth="true">
                                        <Items>
                                            <ext:ComboBox ID="cbbYear" runat="server" ReadOnly="true" StoreID="Store3" Width="70"
                                                DisplayField="YEAR" ValueField="YEAR" ForceSelection="true" SelectOnFocus="true">
                                                <AjaxEvents>
                                                    <Select OnEvent="Button_look_click">
                                                    </Select>
                                                </AjaxEvents>
                                            </ext:ComboBox>
                                            <ext:Label ID="lYear" runat="server" Text="年">
                                            </ext:Label>
                                            <ext:ComboBox ID="cbbmonth" runat="server" ReadOnly="true" StoreID="Store4" Width="70"
                                                DisplayField="MONTH" ValueField="MONTH">
                                            </ext:ComboBox>
                                            <ext:Label ID="lmonth" runat="server" Text="月">
                                            </ext:Label>
                                            <ext:Button ID="Button_look" runat="server" Text="查询" Icon="FolderMagnify">
                                                <AjaxEvents>
                                                    <Click OnEvent="Button_look_click">
                                                        <EventMask Msg="载入中..." ShowMask="true" />
                                                    </Click>
                                                </AjaxEvents>
                                            </ext:Button>
                                        </Items>
                                    </ext:Toolbar>
                                </TopBar>
                                <ColumnModel ID="ColumnModel1" runat="server">
                                    <Columns>
                                        <ext:Column ColumnID="DEPTNAME" Header="<div style='text-align:center;'>科室名称</div>"
                                            Width="200" Align="Left" Sortable="true" DataIndex="DEPTNAME" MenuDisabled="true" />
                                        <ext:Column ColumnID="BYFH" Header="<div style='text-align:center;'>本月返还</div>" Width="100"
                                            Align="Right" Sortable="true" DataIndex="BYFH" MenuDisabled="true" />
                                        <ext:Column ColumnID="BYFD" Header="<div style='text-align:center;'>本月封顶</div>"
                                            Width="100" Align="Right" Sortable="true" DataIndex="BYFD" MenuDisabled="true">
                                        </ext:Column>
                                        <ext:Column ColumnID="SYLC" Header="<div style='text-align:center;'>上月留存</div>"
                                            Width="100" Align="Right" Sortable="true" DataIndex="SYLC" MenuDisabled="true">
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
