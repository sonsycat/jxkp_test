<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Sys_Menu_Guide_Detail.aspx.cs" Inherits="GoldNet.JXKP.WebPage.SysManager.Sys_Menu_Guide_Detail" %>
<%@ Register Assembly="Goldnet.Ext.Web" Namespace="Goldnet.Ext.Web" TagPrefix="ext" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <ext:ScriptManager ID="ScriptManager1" runat="server" />
   <ext:Store ID="Store1" runat="server" AutoLoad="true" >
        <Reader>
            <ext:JsonReader ReaderID="GUIDE_CODE">
                <Fields>
                    <ext:RecordField Name="GUIDE_CODE"  />
                    <ext:RecordField Name="GUIDE_NAME" />
                    <ext:RecordField Name="GUIDE_VALUE"   />
                    
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
                                TrackMouseOver="true" Height="480" ClicksToEdit="1">
                               
                                <ColumnModel ID="ColumnModel2" runat="server">
                                    <Columns>
                                    <ext:Column Header="指标代码" Width="66" Align="Left" Sortable="true" MenuDisabled="true"
                                            ColumnID="GUIDE_CODE" DataIndex="GUIDE_CODE">
                                        </ext:Column>
                                        <ext:Column ColumnID="GUIDE_NAME" Header="指标名称" Width="130" Align="left" Sortable="true"
                                        DataIndex="GUIDE_NAME" MenuDisabled="true" />
                                         <ext:Column ColumnID="GUIDE_VALUE" Header="<div style='text-align:center;'>指标值</div>" Width="100" Align="Right" Sortable="true"
                                        DataIndex="GUIDE_VALUE" MenuDisabled="true" />  
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