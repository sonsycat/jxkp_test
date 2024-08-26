<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WorkloadSet_ADD.aspx.cs" Inherits="GoldNet.JXKP.Bonus.Input.WorkloadSet_ADD" %>

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
    <br />
    <ext:ScriptManager ID="ScriptManager1" runat="server" />
    <ext:Store ID="Store1" runat="server" AutoLoad="true">
        <Proxy>
        </Proxy>
        <Reader>
            <ext:JsonReader Root="costsitemlist" TotalProperty="totalitems">
                <Fields>
                    <ext:RecordField Name="ITEM_CODE" />
                    <ext:RecordField Name="ITEM_NAME" />
                </Fields>
            </ext:JsonReader>
        </Reader>
    </ext:Store>
    <ext:Store ID="Store2" runat="server" AutoLoad="true">
        <Reader>
            <ext:JsonReader>
                <Fields>
                    <ext:RecordField Name="ID" />
                    <ext:RecordField Name="ACCOUNT_TYPE" />
                </Fields>
            </ext:JsonReader>
        </Reader>
    </ext:Store>
    <ext:Store ID="Store3" runat="server" AutoLoad="true">
        <Reader>
            <ext:JsonReader>
                <Fields>
                    <ext:RecordField Name="INCOM_TYPE_CODE" />
                    <ext:RecordField Name="INCOM_TYPE_NAME" />
                </Fields>
            </ext:JsonReader>
        </Reader>
    </ext:Store>
    <ext:Store ID="SDept" runat="server" AutoLoad="true">
    </ext:Store>
    <form id="form1" runat="server">
    <div>
        <ext:FormPanel ID="FormPanel1" runat="server" Border="false" MonitorValid="true"
            ButtonAlign="Right" BodyStyle="background-color:transparent;margin:0px,0,0,10px">
            <Body>
                <ext:ColumnLayout ID="ColumnLayout1" runat="server">
                    <ext:LayoutColumn ColumnWidth=".5">
                        <ext:Panel ID="Panel1" runat="server" Border="false" Header="false" BodyStyle="background-color:transparent;">
                            <Body>
                                <ext:FormLayout ID="FormLayout1" runat="server" LabelWidth="80">
                                    <ext:Anchor>
                                        <ext:TextField ID="FLAG" runat="server" Hidden="true" />
                                    </ext:Anchor>
                                    <ext:Anchor>
                                        <ext:TextField ID="ITEM_NAME" runat="server" DataIndex="ITEM_NAME" AllowBlank="false"
                                            Width="120" EnableKeyEvents="true" FieldLabel="项目名称"  ReadOnly="false" />
                                    </ext:Anchor>
                                    <ext:Anchor>
                                        <ext:TextField ID="ITEM_CLASS" runat="server" DataIndex="ITEM_CLASS" AllowBlank="false"
                                            Width="120" EnableKeyEvents="true" FieldLabel="项目代码"  ReadOnly="false" />
                                    </ext:Anchor>
                                    <ext:Anchor>
                                        <ext:TextField ID="INPUT_CODE" runat="server" DataIndex="INPUT_CODE" AllowBlank="false"
                                            Width="120" EnableKeyEvents="true" FieldLabel="输入码" />
                                    </ext:Anchor>
                                    <ext:Anchor>
                                        <ext:MultiField ID="MultiField1" runat="server" FieldLabel="住院积分">
                                            <Fields>
                                                <ext:NumberField ID="INP_GRADE" runat="server" DataIndex="INP_GRADE"
                                                    AllowBlank="false" Width="120" EnableKeyEvents="true" 
                                                    StyleSpec="text-align:right" SelectOnFocus="true"  />
                                                <ext:Label ID="Label1" runat="server" Html="<span style='color:Red;'>*</span>" />
                                            </Fields>
                                        </ext:MultiField>
                                    </ext:Anchor>
                                    <ext:Anchor>
                                        <ext:MultiField ID="MultiField2" runat="server" FieldLabel="门诊积分">
                                            <Fields>
                                                <ext:NumberField ID="OUP_GRADE" runat="server" DataIndex="OUP_GRADE"
                                                    AllowBlank="false" Width="120" EnableKeyEvents="true" 
                                                    StyleSpec="text-align:right" SelectOnFocus="true"  />
                                                <ext:Label ID="Label2" runat="server" Html="<span style='color:Red;'>*</span>" />
                                            </Fields>
                                        </ext:MultiField>
                                    </ext:Anchor>                                   
                                </ext:FormLayout>
                            </Body>
                        </ext:Panel>
                    </ext:LayoutColumn>
                </ext:ColumnLayout>
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
