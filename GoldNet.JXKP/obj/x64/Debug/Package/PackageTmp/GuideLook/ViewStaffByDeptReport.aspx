<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ViewStaffByDeptReport.aspx.cs"
    Inherits="GoldNet.JXKP.GuideLook.ViewStaffByDeptReport" %>

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
                </Fields>
            </ext:JsonReader>
        </Reader>
    </ext:Store>
    <div>
        <ext:ViewPort ID="ViewPort1" runat="server">
            <Body>
            <ext:BorderLayout ID="BorderLayout2" runat="server">
                <Center>
            
                <ext:GridPanel ID="GridPanel1" runat="server" Border="false" StoreID="Store1"
                    StripeRows="true" Height="480" Width="600" AutoScroll="true">
                    <ColumnModel ID="ColumnModel1" runat="server">
                        <Columns>
                            <ext:RowNumbererColumn Width="32" Resizable="true">
                            </ext:RowNumbererColumn>
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
