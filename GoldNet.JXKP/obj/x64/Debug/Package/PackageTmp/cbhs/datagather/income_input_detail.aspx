<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="income_input_detail.aspx.cs"
    Inherits="GoldNet.JXKP.cbhs.datagather.income_input_add" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Assembly="Goldnet.Ext.Web" Namespace="Goldnet.Ext.Web" TagPrefix="ext" %>
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
            <ext:JsonReader Root="itemlist" TotalProperty="totalitems">
                <Fields>
                    <ext:RecordField Name="CLASS_CODE" />
                    <ext:RecordField Name="CLASS_NAME" />
                </Fields>
            </ext:JsonReader>
        </Reader>
    </ext:Store>
    <ext:Store ID="Store2" runat="server" AutoLoad="true">
        <Proxy>
        </Proxy>
        <Reader>
            <ext:JsonReader Root="deptlist" TotalProperty="totalCount">
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
                        <ext:TextField runat="server" ID="ROW_ID" Hidden="true" />
                    </ext:Anchor>
                    <ext:Anchor>
                        <ext:MultiField ID="MultiField1" runat="server" FieldLabel="收入项目">
                            <Fields>
                                <ext:ComboBox ID="RECK_ITEM" runat="server" StoreID="Store1" DisplayField="CLASS_NAME"
                                    AllowBlank="false" ValueField="CLASS_CODE" TypeAhead="false" LoadingText="Searching..."
                                    Width="220" PageSize="10" ItemSelector="div.search-item" MinChars="1">
                                    <Template ID="Template4" runat="server">
                              <tpl for=".">
                               <div class="search-item">
                                 <h3><span>{CLASS_NAME}</span>{CLASS_CODE}</h3>
                  
                               </div>
                              </tpl>
                                    </Template>
                                </ext:ComboBox>
                                <ext:Label ID="Label1" runat="server" Html="<span style='color:Red;'>*</span>" />
                            </Fields>
                        </ext:MultiField>
                    </ext:Anchor>
                    <ext:Anchor>
                        <ext:MultiField ID="MultiField2" runat="server" FieldLabel="应收金额">
                            <Fields>
                                <ext:NumberField ID="INCOMES" runat="server" DataIndex="INCOMES" AllowBlank="false"
                                    Width="220" StyleSpec="text-align:right" DecimalPrecision="2" />
                                <ext:Label ID="Label2" runat="server" Html="<span style='color:Red;'>*</span>" />
                            </Fields>
                        </ext:MultiField>
                    </ext:Anchor>
                    <ext:Anchor>
                        <ext:MultiField ID="MultiField3" runat="server" FieldLabel="实收金额">
                            <Fields>
                                <ext:NumberField ID="INCOMES_CHARGES" runat="server" DataIndex="INCOMES_CHARGES"
                                    AllowBlank="false" Width="220" StyleSpec="text-align:right"  DecimalPrecision="2"/>
                                <ext:Label ID="Label3" runat="server" Html="<span style='color:Red;'>*</span>" />
                            </Fields>
                        </ext:MultiField>
                    </ext:Anchor>
                    <ext:Anchor>
                        <ext:MultiField ID="MultiField4" runat="server" FieldLabel="开单科室">
                            <Fields>
                                <ext:ComboBox ID="ORDERED_BY" runat="server" StoreID="Store2" DisplayField="DEPT_NAME"
                                    ValueField="DEPT_CODE" TypeAhead="false" LoadingText="Searching..." Width="220"
                                    AllowBlank="false" PageSize="10" ItemSelector="div.search-item" MinChars="1">
                                    <Template ID="Template2" runat="server">
                              <tpl for=".">
                               <div class="search-item">
                                 <h3><span style="width:auto">{DEPT_CODE}</span>{DEPT_NAME}</h3>
                               </div>
                              </tpl>
                                    </Template>
                                </ext:ComboBox>
                                <ext:Label ID="Label4" runat="server" Html="<span style='color:Red;'>*</span>" />
                            </Fields>
                        </ext:MultiField>
                    </ext:Anchor>
                    <ext:Anchor>
                        <ext:MultiField ID="MultiField5" runat="server" FieldLabel="执行科室">
                            <Fields>
                                <ext:ComboBox ID="PERFORMED_BY" runat="server" StoreID="Store2" DisplayField="DEPT_NAME"
                                    ValueField="DEPT_CODE" TypeAhead="false" LoadingText="Searching..." Width="220"
                                    AllowBlank="false" PageSize="10" ItemSelector="div.search-item" MinChars="1">
                                    <Template ID="Template1" runat="server">
                              <tpl for=".">
                               <div class="search-item">
                                 <h3><span style="width:auto">{DEPT_CODE}</span>{DEPT_NAME}</h3>
                  
                               </div>
                              </tpl>
                                    </Template>
                                </ext:ComboBox>
                                <ext:Label ID="Label5" runat="server" Html="<span style='color:Red;'>*</span>" />
                            </Fields>
                        </ext:MultiField>
                    </ext:Anchor>
                    <ext:Anchor>
                        <ext:MultiField ID="MultiField10" runat="server" FieldLabel="护理单元">
                            <Fields>
                                <ext:ComboBox ID="WARD_CODE" runat="server" StoreID="Store2" DisplayField="DEPT_NAME"
                                    ValueField="DEPT_CODE" TypeAhead="false" LoadingText="Searching..." Width="220"
                                    ListWidth="250" PageSize="10" ItemSelector="div.search-item" MinChars="1">
                                    <Template ID="Template3" runat="server">
                                        <tpl for=".">
                                            <div class="search-item">
                                 <h3><span style="width:auto">{DEPT_CODE}</span>{DEPT_NAME}</h3>
                                            </div>
                                        </tpl>
                                    </Template>
                                </ext:ComboBox>
                            </Fields>
                        </ext:MultiField>
                    </ext:Anchor>
                    <ext:Anchor>
                        <ext:MultiField ID="MultiField9" runat="server" FieldLabel="开单医生">
                            <Fields>
                                <ext:TextField ID="ORDER_DOCTOR" runat="server" DataIndex="ORDER_DOCTOR" MsgTarget="Side"
                                    Width="220" MaxLength="10" />
                            </Fields>
                        </ext:MultiField>
                    </ext:Anchor>
                    <ext:Anchor>
                        <ext:MultiField ID="MultiField6" runat="server" FieldLabel="收入类别">
                            <Fields>
                                <ext:ComboBox ID="INCOM_TYPE" runat="server" EmptyText="请选收入类别..." AllowBlank="false"
                                    Width="220">
                                </ext:ComboBox>
                                <ext:Label ID="Label6" runat="server" Html="<span style='color:Red;'>*</span>" />
                            </Fields>
                        </ext:MultiField>
                    </ext:Anchor>
                    <ext:Anchor>
                        <ext:MultiField ID="MultiField7" runat="server" FieldLabel="结算标识">
                            <Fields>
                                <ext:ComboBox ID="ACCOUNT_TYPE" runat="server" EmptyText="请选结算标识..." AllowBlank="false"
                                    Width="220">
                                </ext:ComboBox>
                                <ext:Label ID="Label7" runat="server" Html="<span style='color:Red;'>*</span>" />
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
                        <ext:MultiField ID="MultiField11" runat="server" FieldLabel="备注">
                            <Fields>
                                <ext:TextField ID="REMARKS" runat="server" DataIndex="REMARKS" MsgTarget="Side" Width="220" />
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
        <ext:KeyNav ID="KeyNav1" runat="server" Target="ACCOUNTING_DATE">
            <Enter Handler="var str = document.getElementById('ACCOUNTING_DATE').value ; var   reg=/^(\d{4})(\d{2})(\d{2})$/; document.getElementById('ACCOUNTING_DATE').value   =   str.replace(reg, '$1-$2-$3');" />
        </ext:KeyNav>
    </div>
    </form>
</body>
</html>
