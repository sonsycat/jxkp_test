<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DeptGuideInformation.aspx.cs"
    Inherits="GoldNet.JXKP.jxkh.DeptGuideInformation" %>

<%@ Register Assembly="Goldnet.Ext.Web" Namespace="Goldnet.Ext.Web" TagPrefix="ext" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style type="text/css">
        .x-grid3-body .x-grid3-td-GUIDE_CAUSE
        {
            background-color: #CCFF99;
        }
        .x-grid3-body .x-grid3-td-GUIDE_VALUE
        {
            background-color: #CCFFCC;
        }
        .x-grid3-body .x-grid3-td-THRESHOLD_VALUE
        {
            background-color: #CCFF99;
        }
        .tipslabel
        {
            margin-left: 5px;
            color: red;
        }
        .grouptoolbar
        {
            border-top: 0px;
        }
        body, html
        {
            overflow: hidden;
            margin: 0;
        }
    </style>

    <script type="text/javascript">
        var ConvertGuideUnit = function(v) {
            if (v == "T") {
                return true;
            } else {
                return false;
            }
        }
        var ConvertMinusFlag = function(v) {
            if (v == "1") {
                return true;
            } else {
                return false;
            }
        }
    </script>

</head>
<body>
    <form id="form1" runat="server">
    <ext:ScriptManager runat="server" ID="ScriptManager1" AjaxMethodNamespace="Goldnet">
    </ext:ScriptManager>
    <ext:ViewPort ID="ViewPort1" runat="server" AutoWidth="true">
        <Body>
            <ext:ColumnLayout ID="ColumnLayout1" runat="server" Split="true" FitHeight="true">
                <Columns>
                    <ext:LayoutColumn ColumnWidth="1">
                        <ext:Panel runat="server" ID="Panel1" AutoScroll="true" Border="false">
                            <TopBar>
                                <ext:Toolbar ID="Toolbar2" runat="server">
                                    <Items>
                                        <ext:Button ID="BtnSave" runat="server" Text="保存" Icon="Disk">
                                            <AjaxEvents>
                                                <Click OnEvent="BtnSave_Click">
                                                    <EventMask ShowMask="true" Msg="请稍候..." />
                                                    <ExtraParams>
                                                        <ext:Parameter Name="Value1" Value="typeof(BSC01_GridPanel1) == 'undefined' ?'':Ext.encode(BSC01_GridPanel1.getRowsValues(false)) "
                                                            Mode="Raw">
                                                        </ext:Parameter>
                                                        <ext:Parameter Name="Value2" Value="typeof(BSC02_GridPanel1) == 'undefined' ?'':Ext.encode(BSC02_GridPanel1.getRowsValues(false)) "
                                                            Mode="Raw">
                                                        </ext:Parameter>
                                                        <ext:Parameter Name="Value3" Value="typeof(BSC03_GridPanel1) == 'undefined' ?'':Ext.encode(BSC03_GridPanel1.getRowsValues(false)) "
                                                            Mode="Raw">
                                                        </ext:Parameter>
                                                        <ext:Parameter Name="Value4" Value="typeof(BSC04_GridPanel1) == 'undefined' ?'':Ext.encode(BSC04_GridPanel1.getRowsValues(false)) "
                                                            Mode="Raw">
                                                        </ext:Parameter>
                                                    </ExtraParams>
                                                </Click>
                                            </AjaxEvents>
                                        </ext:Button>
                                        <ext:ToolbarSeparator ID="ToolbarSeparator1" runat="server">
                                        </ext:ToolbarSeparator>
                                        <ext:Button ID="Buttonset" runat="server" Text="设置" Icon="DatabaseKey" Hidden="false">
                                            <AjaxEvents>
                                                <Click OnEvent="Buttonset_Click">
                                                    <EventMask Msg="载入中..." ShowMask="true" />
                                                </Click>
                                            </AjaxEvents>
                                        </ext:Button>
                                        <ext:Button ID="CancelButton" runat="server" Text="返回" Icon="ArrowUndo">
                                            <Listeners>
                                                <Click Handler="parent.DeptGuideSet.hide();" />
                                            </Listeners>
                                        </ext:Button>
                                    </Items>
                                </ext:Toolbar>
                            </TopBar>
                            <Body>
                            </Body>
                        </ext:Panel>
                    </ext:LayoutColumn>
                </Columns>
            </ext:ColumnLayout>
        </Body>
    </ext:ViewPort>
    <ext:Window ID="MonthsSetWin" runat="server" Icon="Group" Title="关联指标设置" Width="700"
        Height="400" AutoShow="false" Modal="true" CenterOnLoad="true" AutoScroll="true"
        ShowOnLoad="false" Resizable="true" StyleSpec="background-color:Transparent;"
        BodyStyle="background-color:Transparent;">
    </ext:Window>
    </form>
</body>
</html>
