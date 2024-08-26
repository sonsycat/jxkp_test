<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="StationEvaluation.aspx.cs" Inherits="GoldNet.JXKP.jxkh.StationEvaluation" %>
<%@ Register Assembly="Goldnet.Ext.Web" Namespace="Goldnet.Ext.Web" TagPrefix="ext" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>岗位评测结果</title>
</head>
<body>
    <form id="form1" runat="server">
    <ext:ScriptManager ID="ScriptManager1" runat="server" />
    <ext:Store runat="server" ID="Store1" AutoLoad="true">
        <Reader>
            <ext:JsonReader>
                <Fields>
                </Fields>
            </ext:JsonReader>
        </Reader>
    </ext:Store>
    <div>
    <ext:ViewPort ID="ViewPort1" runat="server">
        <Body>
            <ext:BorderLayout ID="BorderLayout1" runat="server">
                <Center>
                    <ext:ExtGridPanel ID="GridPanel_Show" runat="server" StoreID="Store1" Border="true" Width="800" Height="400" AutoScroll="true" StyleSpec="margin:10px">
                        <ExtColumnModel ID="ColumnModel1" runat="server">
                            <Columns>
                            <ext:ExtColumn ColumnID="TEST"></ext:ExtColumn>
                            </Columns>
                            <HeadRows>
                                <ext:ExtRows>
                                    <Rows>
                                        <ext:ExtRow Header="" />
                                    </Rows>
                                </ext:ExtRows>
                            </HeadRows>
                        </ExtColumnModel>
                        <Plugins>
                            <ext:ExtGroupHeaderGrid ID="ExtGroupHeaderGrid2" runat="server">
                            </ext:ExtGroupHeaderGrid>
                        </Plugins>
                        <SelectionModel>
                            <ext:RowSelectionModel ID="rowselection" SingleSelect="true">
                            </ext:RowSelectionModel>
                        </SelectionModel>
                        <Listeners>
                            <BeforeRender Handler="Ext.EventManager.onWindowResize(function(){ if(Ext.getBody().getViewSize().width>850){this.setWidth( Ext.getBody().getViewSize().width -18);}; this.setHeight( Ext.getBody().getViewSize().height -18); }, this);" />
                            <Render Handler="if(Ext.getBody().getViewSize().width>850){this.setWidth( Ext.getBody().getViewSize().width -18);}; this.setHeight( Ext.getBody().getViewSize().height -18);" />
                        </Listeners>
                        <LoadMask ShowMask="true" Msg="查询中....." />
                    </ext:ExtGridPanel>
                </Center>
            </ext:BorderLayout>
        </Body>
    </ext:ViewPort>
    </div>
    </form>
</body>
</html>
