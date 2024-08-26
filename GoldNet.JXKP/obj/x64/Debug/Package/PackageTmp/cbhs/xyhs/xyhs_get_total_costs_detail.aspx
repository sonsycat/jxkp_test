<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="xyhs_get_total_costs_detail.aspx.cs"
    Inherits="GoldNet.JXKP.cbhs.xyhs.xyhs_get_total_costs_detail" %>

<%@ Register Assembly="Goldnet.Ext.Web" Namespace="Goldnet.Ext.Web" TagPrefix="ext" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
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
    <form id="form1" runat="server">
    <div>
        <ext:FormPanel ID="FormPanel1" runat="server" Border="false" MonitorValid="true"
            ButtonAlign="Right" BodyStyle="background-color:transparent;">
            <Body>
                <ext:FormLayout ID="FormLayout1" runat="server" LabelWidth="80" StyleSpec="margin:10px">
                
                    <ext:Anchor>
                        <ext:TextField runat="server" ID="DATA_ID" Hidden="true" />
                    </ext:Anchor>
                    <ext:Anchor>
                        <ext:ComboBox ID="COSTS_ITEM" runat="server" StoreID="Store1" DisplayField="ITEM_NAME" FieldLabel="成本项目"
                            AllowBlank="false" ValueField="ITEM_CODE" TypeAhead="false" LoadingText="Searching..."
                            Width="220" PageSize="10" ItemSelector="div.search-item" MinChars="1">
                            <Template ID="Template4" runat="server">
                              <tpl for=".">
                               <div class="search-item">
                                 <h3><span>{ITEM_CODE}</span>{ITEM_NAME}</h3>
                               </div>
                              </tpl>
                            </Template>
                        </ext:ComboBox>
                    </ext:Anchor>
                    <ext:Anchor>
                        <ext:NumberField ID="COSTS" runat="server" DataIndex="INCOMES" AllowBlank="false"  FieldLabel="成本总额"
                            Width="220" StyleSpec="text-align:right" DecimalPrecision="2" />
                    </ext:Anchor>
                    <ext:Anchor>
                        <ext:TextArea ID="REMARKS"  runat="server" FieldLabel="备注" DataIndex="REMARKS" MsgTarget="Side"
                            Width="220" Height="70" />
                    </ext:Anchor>
                    
                    <ext:Anchor>
                        <ext:Checkbox ID="Tag" runat="server" FieldLabel="同步到以后" Width="220" Hidden="true" Visible="false">
                        </ext:Checkbox>
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
                <ext:Button ID="cancel" runat="server" Text="返回" Icon="Cancel">
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
