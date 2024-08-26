<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="income_update_date_detail.aspx.cs" Inherits="GoldNet.JXKP.cbhs.cbhsdict.income_update_date_detail" %>

<%@ Register Assembly="Goldnet.Ext.Web" Namespace="Goldnet.Ext.Web" TagPrefix="ext" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
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
    <style type="text/css">
        .search-item
        {
            font: normal 11px tahoma, arial, helvetica, sans-serif;
            padding: 3px 10px 3px 10px;
            border: 1px solid #fff;
            border-bottom: 1px solid #eeeeee;
            white-space: normal;
            color: #555;
        }
        .search-item h3
        {
            display: block;
            font: inherit;
            font-weight: bold;
            color: #222;
        }
        .search-item h3 span
        {
            float: right;
            font-weight: normal;
            margin: 0 0 5px 5px;
            width: 100px;
            display: block;
            clear: none;
        }
        p
        {
            width: 650px;
        }
        .ext-ie .x-form-text
        {
            position: static !important;
        }
    </style>
</head>
<body>
    <ext:ScriptManager ID="ScriptManager1" runat="server" />
    <form id="form1" runat="server">
    <div>
        <ext:FormPanel ID="FormPanel1" runat="server" Border="false" MonitorValid="true"
            ButtonAlign="Right" BodyStyle="background-color:transparent;">
            <Body>
                <ext:FormLayout ID="FormLayout1" runat="server" LabelWidth="80" StyleSpec="margin:10px">                   
                    <ext:Anchor>
                        <ext:MultiField ID="MultiField8" runat="server" FieldLabel="开始时间">
                            <Fields>
                                <ext:DateField ID="S_DATE" runat="server" AllowBlank="false" Width="100" Format="yyyy-MM-dd"
                                    EnableKeyEvents="true" />
                                <ext:TextField ID="Txt_S_hh" runat="server" MsgTarget="Side" Width="30" />
                                <ext:Label ID="Label3" runat="server" Text=":" />
                                <ext:TextField ID="Txt_S_mm" runat="server" MsgTarget="Side" Width="30" />
                                <ext:Label ID="Label4" runat="server" Text=":" />
                                <ext:TextField ID="Txt_S_ss" runat="server" MsgTarget="Side" Width="30" />
                                <ext:Label ID="Label8" runat="server" Html="<span style='color:Red;'>*</span>" />
                            </Fields>
                        </ext:MultiField>
                    </ext:Anchor>
                    <ext:Anchor>
                        <ext:MultiField ID="MultiField1" runat="server" FieldLabel="结束时间">
                            <Fields>
                                <ext:DateField ID="D_DATE" runat="server" AllowBlank="false" Width="100" Format="yyyy-MM-dd"
                                    EnableKeyEvents="true" />
                                <ext:TextField ID="Txt_d_hh" runat="server" MsgTarget="Side" Width="30" />
                                <ext:Label ID="Label5" runat="server" Text=":" />
                                <ext:TextField ID="Txt_d_mm" runat="server" MsgTarget="Side" Width="30" />
                                <ext:Label ID="Label6" runat="server" Text=":" />
                                <ext:TextField ID="Txt_d_ss" runat="server" MsgTarget="Side" Width="30" />
                                <ext:Label ID="Label1" runat="server" Html="<span style='color:Red;'>*</span>" />
                            </Fields>
                        </ext:MultiField>
                    </ext:Anchor>
                    <ext:Anchor>
                        <ext:MultiField ID="MultiField2" runat="server" FieldLabel="目标日期">
                            <Fields>
                                <ext:DateField ID="TO_DATE_TIME" runat="server"  AllowBlank="false"
                                    Width="220" Format="yyyy-MM-dd" EnableKeyEvents="true" />
                                <ext:Label ID="Label2" runat="server" Html="<span style='color:Red;'>*</span>" />
                            </Fields>
                        </ext:MultiField>
                    </ext:Anchor>
                </ext:FormLayout>
            </Body>
            <Buttons>
                <ext:Button ID="save" runat="server" Text="保存" Icon="Disk">
                    <AjaxEvents>
                        <Click OnEvent="Buttonsave_Click" Before="if( #{FormPanel1}.getForm().isValid()) {return true;}else{Ext.Msg.alert('提示','填写信息不完整');  return false;}">
                            <EventMask Msg="保存中..." ShowMask="true" />
                        </Click>
                    </AjaxEvents>
                </ext:Button>
                <ext:Button ID="cancel" runat="server" Text="取消" Icon="Cancel">
                    <Listeners>
                        <Click Handler="parent.DetailWin.hide();" />
                    </Listeners>
                </ext:Button>
            </Buttons>
        </ext:FormPanel>
    
    </div>
    </form>
</body>
</html>
