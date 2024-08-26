<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SimpleEncouragePM.aspx.cs" Inherits="GoldNet.JXKP.WebPage.SpecManager.SimpleEncouragePM" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Assembly="Goldnet.Ext.Web" Namespace="Goldnet.Ext.Web" TagPrefix="ext" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <style type="text/css">
        body
        {
            background-color: #DFE8F6;
            font-size: 12px;
        }
    </style>

    <script language="javascript" type="text/javascript">
        var RefreshData = function(msg) {
            
            Store1.reload();
        }
       
    </script>

</head>
<body>
    <form id="form1" runat="server">
    <div>
        <ext:ScriptManager ID="ScriptManager1" runat="server" />
        <ext:Store ID="Store1" runat="server"  OnRefreshData="Store_RefreshData"
            AutoLoad="true">
            <Reader>
                <ext:JsonReader ReaderID="ITEM_CODE">
                    <Fields>
                        <ext:RecordField Name="ID" />
                        <ext:RecordField Name="ITEMNAME" />
                    </Fields>
                </ext:JsonReader>
            </Reader>
        </ext:Store>
        <div>
            <ext:ViewPort ID="ViewPort2" runat="server">
                <Body>
                    <ext:BorderLayout ID="BorderLayout2" runat="server">                       
                        <Center>
                            <ext:GridPanel ID="GridPanel1" runat="server" StoreID="Store1" StripeRows="true"
                                TrackMouseOver="true" AutoWidth="true" Height="480" Border="false">
                                <ColumnModel ID="ColumnModel1" runat="server">
                                    <Columns>
                                        <ext:Column ColumnID="ITEMNAME" Header="奖惩名称" Width="200" Sortable="true" DataIndex="ITEMNAME"
                                            MenuDisabled="true" Align="Left" />
                                        <ext:CommandColumn Header="权限" Sortable="true" ColumnID="Columns5" Align="Center"
                                            Width="50">
                                            <Commands>
                                                <ext:GridCommand Icon="CogStart" CommandName="Show">
                                                </ext:GridCommand>
                                            </Commands>
                                        </ext:CommandColumn>
                                    </Columns>
                                </ColumnModel>
                                <SelectionModel>
                                    <ext:RowSelectionModel ID="RowSelectionModel1" runat="server" SingleSelect="true">
                                    </ext:RowSelectionModel>
                                </SelectionModel>
                                <AjaxEvents>
                                    <Command OnEvent="SetPower">
                                        <ExtraParams>
                                            <ext:Parameter Name="Values" Value="this.store.getAt(rowIndex).get('ID')"
                                                Mode="Raw">
                                            </ext:Parameter>
                                        </ExtraParams>
                                    </Command>
                                </AjaxEvents>
                            </ext:GridPanel>
                        </Center>
                    </ext:BorderLayout>
                </Body>
            </ext:ViewPort>
        </div>
        <ext:Window ID="PowerWin" runat="server" Icon="Group" Title="权限设置" Width="500" Height="450"
            AutoShow="false" Modal="true" CenterOnLoad="true" AutoScroll="true" ShowOnLoad="false"
            Resizable="true" StyleSpec="background-color:Transparent;" BodyStyle="background-color:Transparent;">
        </ext:Window>
    </div>
    </form>
</body>
</html>
