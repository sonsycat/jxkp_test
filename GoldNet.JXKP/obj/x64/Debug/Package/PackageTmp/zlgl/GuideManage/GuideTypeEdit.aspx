<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="GuideTypeEdit.aspx.cs" Inherits="GoldNet.JXKP.zlgl.SysManage.GuideTypeEdit" %>
<%@ Register Assembly="Goldnet.Ext.Web" Namespace="Goldnet.Ext.Web" TagPrefix="ext" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>标题</title>
     <link rel="stylesheet" type="text/css" href="/resources/css/main.css" />
    <style type="text/css">
        body
        {
            background-color: #DFE8F6;
            font-size: 12px;
        }
        td strong
        {
            color: Red;
        }
    </style>
   
</head>
<body>
    <ext:ScriptManager ID="ScriptManager1" runat="server">
    </ext:ScriptManager>
    <form id="form1" runat="server">
    <div>
    <ext:FormPanel ID="FormPanel1" runat="server" Border="false" AutoScroll="true" ButtonAlign="Right"
        StyleSpec="background-color:transparent" BodyStyle="background-color:transparent">
        <Body>
   
            <ext:Panel ID="Panel1" runat="server" Border="false" AutoHeight="true" AutoWidth="true"
                StyleSpec="background-color:transparent" BodyStyle="background-color:transparent">
                <TopBar>
                    <ext:Toolbar ID="Toolbar1" runat="server">
                        <Items>
                            <ext:Button ID="BtnSave" runat="server" Text="确定" Icon="Disk" >
                                <AjaxEvents>
                                    <Click OnEvent="BtnSave_Click">
                                    </Click>
                                </AjaxEvents>
                            </ext:Button>
                            <ext:ToolbarSeparator ID="ToolbarSeparator1" runat="server">
                            </ext:ToolbarSeparator>
                            <ext:Button ID="Button1" runat="server" Text="返回" Icon="ArrowUndo">
                                <Listeners>
                                    <Click Handler="parent.guidetypeedit.hide();" />
                                </Listeners>
                            </ext:Button>
                        </Items>
                    </ext:Toolbar>
                </TopBar>
                <Body>
                    <ext:FormLayout ID="FormLayout1" runat="server">
                        <ext:Anchor>
                            <ext:TextField ID="Text_guidetype" runat="server" DataIndex="gudiename" MsgTarget="Side"
                                 AllowBlank="false" FieldLabel="指标类别" Width="200" />
                        </ext:Anchor>
                        <ext:Anchor>
                            <ext:TextField ID="Text_guidenumber" runat="server" DataIndex="guidenumber" MsgTarget="Side"
                                 AllowBlank="false" FieldLabel="指标分值" Width="200" Text="0" />
                        </ext:Anchor>
                    </ext:FormLayout>
                </Body>
            </ext:Panel>
        </Body>
    </ext:FormPanel>
    </div>
    </form>
</body>
</html>
