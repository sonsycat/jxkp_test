<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Income_Difference_Data.aspx.cs"
    Inherits="GoldNet.JXKP.Income_Difference_Data" %>

<%@ Register Assembly="Goldnet.Ext.Web" Namespace="Goldnet.Ext.Web" TagPrefix="ext" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <ext:ScriptManager ID="ScriptManager1" runat="server" />
    <form id="form1" runat="server">
    <div>
        <ext:ViewPort ID="ViewPort111" runat="server">
            <Body>
                <ext:ColumnLayout ID="ColumnLayout11" runat="server" Split="true" FitHeight="true">
                    <Columns>
                        <ext:LayoutColumn ColumnWidth="1">
                            <ext:FormPanel ID="FormPanel1" runat="server" Border="false" AutoScroll="true" ButtonAlign="Right">
                                <Body>
                                    <ext:FormLayout ID="FormLayout1" runat="server" LabelWidth="80">
                                        <ext:Anchor Horizontal="98.5%">
                                            <ext:Toolbar runat="server">
                                                <Items>
                                                    <ext:Button ID="Btn_Pre" Text="上一步" Icon="ArrowLeft" runat="server">
                                                        <AjaxEvents>
                                                            <Click OnEvent="Btn_Pre_Click">
                                                            </Click>
                                                        </AjaxEvents>
                                                    </ext:Button>
                                                    <ext:Button ID="Btn_Update" Text="更新" Icon="Disk" runat="server">
                                                        <AjaxEvents>
                                                            <Click OnEvent="Btn_Update_Click">
                                                            </Click>
                                                        </AjaxEvents>
                                                    </ext:Button>
                                                </Items>
                                            </ext:Toolbar>
                                        </ext:Anchor>
                                    </ext:FormLayout>
                                </Body>
                            </ext:FormPanel>
                        </ext:LayoutColumn>
                    </Columns>
                </ext:ColumnLayout>
            </Body>
        </ext:ViewPort>
    </div>
    </form>
</body>
</html>
