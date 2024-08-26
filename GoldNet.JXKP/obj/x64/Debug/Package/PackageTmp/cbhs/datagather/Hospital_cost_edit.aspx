<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Hospital_cost_edit.aspx.cs" Inherits="GoldNet.JXKP.cbhs.datagather.Hospital_cost_edit" %>
<%@ Register Assembly="Goldnet.Ext.Web" Namespace="Goldnet.Ext.Web" TagPrefix="ext" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
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

    <script language="javascript" type="text/javascript">
         var rmbMoney = function(v) {
               v = (Math.round((v - 0) * 100)) / 100;
               v = (v == Math.floor(v)) ? v + ".00" : ((v * 10 == Math.floor(v * 10)) ? v + "0" : v);
               v = String(v);
               var ps = v.split('.');
               var whole = ps[0];
               var sub = ps[1] ? '.' + ps[1] : '.00';
               var r = /(\d+)(\d{3})/;
               while (r.test(whole)) {
                   whole = whole.replace(r, '$1' + ',' + '$2');
               }
               v = whole + sub;
               if (v.charAt(0) == '-') {
                   return '-' + v.substr(1);
               }
               return v;
          }
    </script>

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
        <Proxy>
        </Proxy>
        <Reader>
            <%--<ext:JsonReader Root="deptlist" TotalProperty="totalCount">--%>
            <ext:JsonReader Root="Staffdepts">
                <Fields>
                    <ext:RecordField Name="DEPT_CODE" />
                    <ext:RecordField Name="DEPT_NAME" />
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
                        <ext:TextField runat="server" ID="ID" Hidden="true" />
                    </ext:Anchor>
                    <ext:Anchor>
                        <ext:MultiField ID="MultiField4" runat="server" FieldLabel="科室">
                            <Fields>
                                <ext:ComboBox ID="deptcode" runat="server" StoreID="Store2" DisplayField="DEPT_NAME"
                                    ValueField="DEPT_CODE" TypeAhead="false" LoadingText="Searching..." Width="220"
                                    AllowBlank="true"  PageSize="1500" ItemSelector="div.search-item" MinChars="1" ListWidth="260">
                                    <Template ID="Template2" runat="server">
                              <tpl for=".">
                               <div class="search-item">
                                 <h3><span>{DEPT_CODE}</span>{DEPT_NAME}</h3>
                               </div>
                              </tpl>
                                    </Template>
                                </ext:ComboBox>
                                <ext:Label ID="Label4" runat="server" Html="<span style='color:Red;'>*</span>" />
                            </Fields>
                        </ext:MultiField>
                    </ext:Anchor>
                     <ext:Anchor>
                        <ext:MultiField ID="costname" runat="server" FieldLabel="成本名称">
                            <Fields>
                                <ext:TextField ID="COST_NAME" runat="server" DataIndex="COST_NAME" MsgTarget="Side"
                                    Width="220" MaxLength="10" />
                            </Fields>
                        </ext:MultiField>
                    </ext:Anchor>
                    <ext:Anchor>
                        <ext:MultiField ID="MultiField1" runat="server" FieldLabel="成本项目">
                            <Fields>
                                <ext:ComboBox ID="RECK_ITEM" runat="server" StoreID="Store1" DisplayField="ITEM_NAME"
                                    AllowBlank="false" ValueField="ITEM_CODE" TypeAhead="false" LoadingText="Searching..."
                                    Width="220"  ItemSelector="div.search-item" MinChars="1" PageSize="300">
                                    <Template ID="Template4" runat="server">
                              <tpl for=".">
                               <div class="search-item">
                                 <h3><span>{ITEM_NAME}</span>{ITEM_CODE}</h3>
                  
                               </div>
                              </tpl>
                                    </Template>
                                </ext:ComboBox>
                                <ext:Label ID="Label1" runat="server" Html="<span style='color:Red;'>*</span>" />
                            </Fields>
                        </ext:MultiField>
                    </ext:Anchor>
                   
                    <ext:Anchor>
                        <ext:MultiField ID="MultiField8" runat="server" FieldLabel="发生日期">
                            <Fields>
                                <ext:DateField ID="ACCOUNTING_DATE" runat="server" DataIndex="ACCOUNTING_DATE" AllowBlank="false"
                                    Width="220" Format="yyyy-MM-dd" EnableKeyEvents="true" />
                                <ext:Label ID="Label8" runat="server" Html="<span style='color:Red;'>*</span>" />
                            </Fields>
                        </ext:MultiField>
                    </ext:Anchor>
                    <ext:Anchor>
                        <ext:MultiField ID="MultiField2" runat="server" FieldLabel="成本金额">
                            <Fields>
                                <ext:NumberField ID="COSTS" runat="server" DataIndex="COSTS" AllowBlank="false"
                                    Width="220" StyleSpec="text-align:right" DecimalPrecision="2" />
                                <ext:Label ID="Label2" runat="server" Html="<span style='color:Red;'>*</span>" />
                            </Fields>
                        </ext:MultiField>
                    </ext:Anchor>
                    <ext:Anchor>
                        <ext:MultiField ID="MultiField6" runat="server" FieldLabel="院分摊方案" Hidden="true">
                            <Fields>
                                <ext:ComboBox ID="hosprogcode" runat="server" EmptyText="请选分摊方案..." AllowBlank="true"
                                    Width="220" Hidden="true">
                                </ext:ComboBox>
                                <ext:Label ID="Label5" runat="server" Html="<span style='color:Red;'>可以为空</span>" Hidden="true" />
                            </Fields>
                        </ext:MultiField>
                    </ext:Anchor>
                    <ext:Anchor>
                        <ext:MultiField ID="MultiField3" runat="server" FieldLabel="科分摊方案">
                            <Fields>
                                <ext:ComboBox ID="deptprogcode" runat="server" EmptyText="请选分摊方案..." AllowBlank="false"
                                    Width="220">
                                </ext:ComboBox>
                                <ext:Label ID="Label3" runat="server" Html="<span style='color:Red;'>*</span>" />
                            </Fields>
                        </ext:MultiField>
                    </ext:Anchor>
                    <ext:Anchor>
                        <ext:MultiField ID="MultiField11" runat="server" FieldLabel="备注">
                            <Fields>
                                <ext:TextArea ID="memo" runat="server" DataIndex="MEMO" MsgTarget="Side" Width="220" MaxLength=300 />
                                
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
                     <AjaxEvents>
                                        <Click OnEvent="btnCancle_Click">
                                        </Click>
                                    </AjaxEvents>
                </ext:Button>
            </Buttons>
        </ext:FormPanel>
        <ext:KeyNav ID="KeyNav1" runat="server" Target="ACCOUNTING_DATE">
            <Enter Handler="var str = document.getElementById('ACCOUNTING_DATE').value ; var   reg=/^(\d{4})(\d{2})(\d{2})$/; document.getElementById('ACCOUNTING_DATE').value   =   str.replace(reg, '$1-$2-$3');" />
        </ext:KeyNav>
    </div>
    </form>
</body>
</html>
