<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="incomes_adjust_detail.aspx.cs"
    Inherits="GoldNet.JXKP.cbhs.xyhs.incomes_adjust_detail" %>

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
    <ext:ScriptManager ID="ScriptManager1" runat="server" />
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
                        <ext:MultiField ID="MultiField8" runat="server" FieldLabel="发生日期">
                            <Fields>
                                <ext:DateField ID="ST_DATE" runat="server" DataIndex="ST_DATE" AllowBlank="false"
                                    Width="220" Format="yyyy-MM-dd" EnableKeyEvents="true" />
                                <ext:Label ID="Label8" runat="server" Html="<span style='color:Red;'>*</span>" />
                            </Fields>
                        </ext:MultiField>
                    </ext:Anchor>
                    <ext:Anchor>
                        <ext:MultiField ID="MultiField2" runat="server" FieldLabel="调整金额">
                            <Fields>
                                <ext:NumberField ID="INCOMES_ADJUST" runat="server" DataIndex="INCOMES_ADJUST" AllowBlank="false"
                                    Width="220" StyleSpec="text-align:right" DecimalPrecision="2" />
                                <ext:Label ID="Label2" runat="server" Html="<span style='color:Red;'>*</span>" />
                            </Fields>
                        </ext:MultiField>
                    </ext:Anchor>
                    <ext:Anchor>
                        <ext:MultiField ID="MultiField3" runat="server" FieldLabel="差值">
                            <Fields>
                                <ext:NumberField ID="INCOMES_DIFFERENCE" runat="server" DataIndex="INCOMES_DIFFERENCE"
                                     Width="220" StyleSpec="text-align:right" DecimalPrecision="2" />
                            </Fields>
                        </ext:MultiField>
                    </ext:Anchor>
                    <ext:Anchor>
                        <ext:MultiField ID="MultiField4" runat="server" FieldLabel="开单科室">
                            <Fields>
                                <ext:ComboBox ID="DEPT" runat="server" StoreID="Store2" DisplayField="DEPT_NAME"
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
        <ext:KeyNav ID="KeyNav1" runat="server" Target="ST_DATE">
            <Enter Handler="var str = document.getElementById('ST_DATE').value ; var   reg=/^(\d{4})(\d{2})(\d{2})$/; document.getElementById('ST_DATE').value   =   str.replace(reg, '$1-$2-$3');" />
        </ext:KeyNav>
    </div>
    </form>
</body>
</html>
