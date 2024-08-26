<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="BonusGuidePrimary.aspx.cs"
    Inherits="GoldNet.JXKP.BonusGuidePrimary" %>

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
            <ext:JsonReader>
                <Fields>
                    <ext:RecordField Name="GUIDE_CODE">
                    </ext:RecordField>
                    <ext:RecordField Name="GUIDE_NAME">
                    </ext:RecordField>
                    <ext:RecordField Name="BONUS_TYPE">
                    </ext:RecordField>
                    <ext:RecordField Name="BONUS_NAME">
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
                                                            <ext:Parameter Name="Values" Value="Ext.encode(#{GridPanel2}.getRowsValues(false))"
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
                                        <ext:Column ColumnID="GUIDECODE" Header="指标代码" Width="180" DataIndex="GUIDE_CODE"
                                            MenuDisabled="true" Align="Center">
                                            <Editor>
                                                <ext:NumberField runat="server" ID="nfguidecode" SelectOnFocus="true">
                                                </ext:NumberField>
                                            </Editor>
                                        </ext:Column>
                                        <ext:Column ColumnID="GUIDENAME" Header="指标名称" Width="180" DataIndex="GUIDE_NAME"
                                            MenuDisabled="true" Align="Center">
                                        </ext:Column>
                                        <ext:Column ColumnID="BONUSNAME" Header="备注" Width="180" DataIndex="BONUS_NAME" MenuDisabled="true"
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
