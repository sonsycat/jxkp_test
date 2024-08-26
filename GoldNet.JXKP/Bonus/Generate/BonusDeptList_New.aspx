<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="BonusDeptList_New.aspx.cs"
    Inherits="GoldNet.JXKP.BonusDeptList_New" %>

<%@ Register Assembly="FixGride" Namespace="FixGride" TagPrefix="wy" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>表头固定测试</title>
    <link href="../../lib/ligerUI/skins/Aqua/css/ligerui-all.css" rel="stylesheet" type="text/css" />
    <link href="../../lib/ligerUI/skins/Gray/css/all.css" rel="stylesheet" type="text/css" />
    <%--    <script src="../../lib/jquery/jquery-1.5.2.min.js" type="text/javascript"></script>
    <script src="../../lib/ligerUI/js/ligerui.min.js" type="text/javascript"></script>
    <script src="../../lib/js/ligerui.expand.js" type="text/javascript"></script>
    <script src="../../lib/json2.js" type="text/javascript"></script>--%>
    <link href="../../lib/css/common.css" rel="stylesheet" type="text/css" />
    <%--    <script src="../../lib/js/common.js" type="text/javascript"></script>
    <script src="../../lib/js/LG.js" type="text/javascript"></script>
    <script src="../../lib/js/fieldType.js" type="text/javascript"></script>--%>
    <link rel="stylesheet" type="text/css" href="../Orthers/Cbouns.css" />
    <style type="text/css">
        .x-grid-record-gray
        {
            background-color: #CCDDFF;
            border-right: 1px solid #666666;
            border-bottom: 1px solid #666666;
        }
        .x-grid-record-col
        {
            border-right: 1px solid #666666;
            border-bottom: 1px solid #666666;
        }
        .x-grid-record-gray-red
        {
            background-color: #CCDDFF;
            border-right: 1px solid #666666;
            border-bottom: 1px solid #666666;
            color: #FF0000;
        }
        .x-grid-record-col-red
        {
            border-right: 1px solid #666666;
            border-bottom: 1px solid #666666;
            color: #FF0000;
        }
    </style>
</head>
<body style="height: 100%; text-align: center;">
<%--    <ext:ScriptManager ID="ScriptManager1" runat="server" />--%>
    <div id="layout" style="width: 99.2%; margin: 0 auto; margin-top: 4px;">
        <form id="mainform" runat="server">
        <%--<ext:Panel ID="Panel1" runat="server" Border="false">
            <TopBar>
                <ext:Toolbar ID="Toolbar1" runat="server" Visible="true" AutoWidth="true">
                    <Items>
                        <ext:Button ID="Btn_Excel" Text="导出Excel" Icon="TextColumns" runat="server" OnClick="OutExcel"
                            AutoPostBack="true">
                        </ext:Button>
                        <ext:Button ID="Btn_Back" Text="返回" Icon="ReverseGreen" runat="server">
                            <AjaxEvents>
                                <Click OnEvent="Back_Click">
                                </Click>
                            </AjaxEvents>
                        </ext:Button>
                        <ext:Label ID="Label3" runat="server" Text="科室类型：" Width="60" />
                        <ext:ComboBox ID="depttype" runat="server" Width="100" AllowBlank="true" EmptyText="请选择...">
                            <AjaxEvents>
                                <Select OnEvent="Item_SelectOnChange">
                                    <EventMask Msg='载入中...' ShowMask="true" />
                                </Select>
                            </AjaxEvents>
                        </ext:ComboBox>
                        <ext:ComboBox ID="cbbType" runat="server" ReadOnly="true" ForceSelection="true" SelectOnFocus="true"
                            SelectedIndex="0">
                            <Items>
                                <ext:ListItem Text="奖金科室" Value="1" />
                                <ext:ListItem Text="核算科室" Value="0" />
                            </Items>
                            <AjaxEvents>
                                <Select OnEvent="Item_SelectOnChange">
                                    <EventMask Msg='载入中...' ShowMask="true" />
                                </Select>
                            </AjaxEvents>
                        </ext:ComboBox>
                    </Items>
                </ext:Toolbar>
            </TopBar>
        </ext:Panel>--%>

        <wy:ShowDate ID="WelcomeLabel1" runat="server"></wy:ShowDate>
        </form>
    </div>
</body>
</html>
