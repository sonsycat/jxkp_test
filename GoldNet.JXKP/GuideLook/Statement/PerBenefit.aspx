<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PerBenefit.aspx.cs" Inherits="GoldNet.JXKP.GuideLook.Statement.PerBenefit" %>

<%@ Register Assembly="Goldnet.Ext.Web" Namespace="Goldnet.Ext.Web" TagPrefix="ext" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>无标题页</title>
</head>
<body>
    <form id="form1" runat="server">
    <ext:ScriptManager ID="ScriptManager1" runat="server" />
    <ext:Store runat="server" ID="Store1">
        <Reader>
            <ext:JsonReader>
                <Fields>
                    <ext:RecordField Name="ITEM_NAME" />
                    <ext:RecordField Name="PRICE" />
                    <ext:RecordField Name="UNITS" />
                    <ext:RecordField Name="AMOUNT" />
                    <ext:RecordField Name="JE" />
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
                                    <ext:ExtColumn ColumnID="ITEM_NAME" Header="名称" Sortable="true" DataIndex="ITEM_NAME" />
                                    <ext:ExtColumn ColumnID="AMOUNT" Header="数量" Sortable="true" DataIndex="AMOUNT" />
                                    <ext:ExtColumn ColumnID="PRICE" Header="单价" Sortable="true" DataIndex="PRICE">
                                    </ext:ExtColumn>
                                    <ext:ExtColumn ColumnID="UNITS" Header="单位" Sortable="true" DataIndex="UNITS" />
                                    <ext:ExtColumn ColumnID="JE" Header="金额" Sortable="true" DataIndex="JE" />
                                </Columns>
                            </ColumnModel>
                            <SelectionModel>
                                <ext:RowSelectionModel SingleSelect="true">
                                </ext:RowSelectionModel>
                            </SelectionModel>
                            <BottomBar>
                                <ext:PagingToolbar ID="PagingToolBar1" runat="server" PageSize="20" StoreID="Store1"
                                    AutoWidth="true" DisplayInfo="true" AutoDataBind="true">
                                </ext:PagingToolbar>
                            </BottomBar>
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
