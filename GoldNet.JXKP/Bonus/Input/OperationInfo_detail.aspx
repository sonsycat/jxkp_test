<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="OperationInfo_detail.aspx.cs"
    Inherits="GoldNet.JXKP.Bonus.Input.OperationInfo_detail" %>

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
        var rmbMoney = function (v) {
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
    <ext:Store ID="Store3" runat="server" AutoLoad="false">
        <Reader>
            <ext:JsonReader Root="hisusers" TotalProperty="totalCount">
                <Fields>
                    <ext:RecordField Name="USER_ID" />
                    <ext:RecordField Name="DB_USER" />
                    <ext:RecordField Name="USER_NAME" />
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
                        <ext:MultiField ID="MultiField12" runat="server" FieldLabel="日期">
                            <Fields>
                                <ext:DateField ID="ST_DATE" runat="server" DataIndex="ST_DATE" AllowBlank="false"
                                    Width="220" Format="yyyy-MM-dd" EnableKeyEvents="true" />
                                <ext:Label ID="Label9" runat="server" Html="<span style='color:Red;'>*</span>" />
                            </Fields>
                        </ext:MultiField>
                    </ext:Anchor>
                    <ext:Anchor>
                        <ext:MultiField ID="MultiField1" runat="server" FieldLabel="科室">
                            <Fields>
                                <ext:ComboBox ID="cb_dept" runat="server" StoreID="Store2" DisplayField="DEPT_NAME"
                                    ValueField="DEPT_CODE" TypeAhead="false" LoadingText="Searching..." Width="220"
                                    AllowBlank="false" PageSize="10" ItemSelector="div.search-item" MinChars="1">
                                    <Template ID="Template4" runat="server">
                              <tpl for=".">
                               <div class="search-item">
                                 <h3><span style="width:auto">{DEPT_CODE}</span>{DEPT_NAME}</h3>
                               </div>
                              </tpl>
                                    </Template>
                                </ext:ComboBox>
                                <ext:Label ID="Label1" runat="server" Html="<span style='color:Red;'>*</span>" />
                            </Fields>
                        </ext:MultiField>
                    </ext:Anchor>
                    <ext:Anchor>
                        <ext:MultiField ID="MultiField2" runat="server" FieldLabel="手术级别">
                            <Fields>
                                <ext:ComboBox ID="LEVEL_J" runat="server" Width="220" AllowBlank="true" EmptyText="请选择级别...">
                                    <Items>
                                        <ext:ListItem Text="一级" Value="1" />
                                        <ext:ListItem Text="二级" Value="2" />
                                        <ext:ListItem Text="三级" Value="3" />
                                        <ext:ListItem Text="四级" Value="4" />
                                    </Items>
                                </ext:ComboBox>
                            </Fields>
                        </ext:MultiField>
                    </ext:Anchor>
                    <ext:Anchor>
                        <ext:MultiField ID="MultiField6" runat="server" FieldLabel="急诊">
                            <Fields>
                                <ext:ComboBox ID="EMERGENCY" runat="server" EmptyText="请选择..." AllowBlank="false"
                                    Width="220">
                                    <Items>
                                        <ext:ListItem Text="不是" Value="0" />
                                        <ext:ListItem Text="是" Value="1" />
                                    </Items>
                                </ext:ComboBox>
                            </Fields>
                        </ext:MultiField>
                    </ext:Anchor>
                    <ext:Anchor>
                        <ext:MultiField ID="MultiField3" runat="server" FieldLabel="手术医生">
                            <Fields>
                                <ext:ComboBox ID="OPERATOR" runat="server" StoreID="Store3" DisplayField="USER_NAME"
                                    ValueField="USER_ID" TypeAhead="false" LoadingText="Searching..." Width="220"
                                    PageSize="10" HideTrigger="false" ItemSelector="div.search-item" MinChars="1">
                                    <Template ID="Template5" runat="server">
                   <tpl for=".">
                      <div class="search-item">
                         <h3><span>{DB_USER}</span>{USER_NAME}</h3>
                  
                      </div>
                   </tpl>
                                    </Template>
                                </ext:ComboBox>
                            </Fields>
                        </ext:MultiField>
                    </ext:Anchor>
                    <ext:Anchor>
                        <ext:MultiField ID="MultiField4" runat="server" FieldLabel="第一助手">
                            <Fields>
                                <ext:ComboBox ID="FIRST_ASSISTANT" runat="server" StoreID="Store3" DisplayField="USER_NAME"
                                    ValueField="DB_USER" TypeAhead="false" LoadingText="Searching..." Width="220"
                                    PageSize="10" HideTrigger="false" ItemSelector="div.search-item" MinChars="1">
                                    <Template ID="Template2" runat="server">
                   <tpl for=".">
                      <div class="search-item">
                         <h3><span>{DB_USER}</span>{USER_NAME}</h3>
                  
                      </div>
                   </tpl>
                                    </Template>
                                </ext:ComboBox>
                            </Fields>
                        </ext:MultiField>
                    </ext:Anchor>
                    <ext:Anchor>
                        <ext:MultiField ID="MultiField5" runat="server" FieldLabel="第二助手">
                            <Fields>
                                <ext:ComboBox ID="SECOND_ASSISTANT" runat="server" StoreID="Store3" DisplayField="USER_NAME"
                                    ValueField="DB_USER" TypeAhead="false" LoadingText="Searching..." Width="220"
                                    PageSize="10" HideTrigger="false" ItemSelector="div.search-item" MinChars="1">
                                    <Template ID="Template1" runat="server">
                   <tpl for=".">
                      <div class="search-item">
                         <h3><span>{DB_USER}</span>{USER_NAME}</h3>
                  
                      </div>
                   </tpl>
                                    </Template>
                                </ext:ComboBox>
                            </Fields>
                        </ext:MultiField>
                    </ext:Anchor>
                    <ext:Anchor>
                        <ext:MultiField ID="MultiField9" runat="server" FieldLabel="麻醉医师">
                            <Fields>
                                <ext:ComboBox ID="ANESTHESIA_DOCTOR" runat="server" StoreID="Store3" DisplayField="USER_NAME"
                                    ValueField="DB_USER" TypeAhead="false" LoadingText="Searching..." Width="220"
                                    PageSize="10" HideTrigger="false" ItemSelector="div.search-item" MinChars="1">
                                    <Template ID="Template3" runat="server">
                   <tpl for=".">
                      <div class="search-item">
                         <h3><span>{DB_USER}</span>{USER_NAME}</h3>
                  
                      </div>
                   </tpl>
                                    </Template>
                                </ext:ComboBox>
                            </Fields>
                        </ext:MultiField>
                    </ext:Anchor>
                    <ext:Anchor>
                        <ext:MultiField ID="MultiField7" runat="server" FieldLabel="护士1">
                            <Fields>
                                <ext:ComboBox ID="HS1" runat="server" StoreID="Store3" DisplayField="USER_NAME" ValueField="DB_USER"
                                    TypeAhead="false" LoadingText="Searching..." Width="220" PageSize="10" HideTrigger="false"
                                    ItemSelector="div.search-item" MinChars="1">
                                    <Template ID="Template6" runat="server">
                   <tpl for=".">
                      <div class="search-item">
                         <h3><span>{DB_USER}</span>{USER_NAME}</h3>
                  
                      </div>
                   </tpl>
                                    </Template>
                                </ext:ComboBox>
                            </Fields>
                        </ext:MultiField>
                    </ext:Anchor>
                    <ext:Anchor>
                        <ext:MultiField ID="MultiField8" runat="server" FieldLabel="护士2">
                            <Fields>
                                <ext:ComboBox ID="HS2" runat="server" StoreID="Store3" DisplayField="USER_NAME" ValueField="DB_USER"
                                    TypeAhead="false" LoadingText="Searching..." Width="220" PageSize="10" HideTrigger="false"
                                    ItemSelector="div.search-item" MinChars="1">
                                    <Template ID="Template7" runat="server">
                   <tpl for=".">
                      <div class="search-item">
                         <h3><span>{DB_USER}</span>{USER_NAME}</h3>
                  
                      </div>
                   </tpl>
                                    </Template>
                                </ext:ComboBox>
                            </Fields>
                        </ext:MultiField>
                    </ext:Anchor>
                    <ext:Anchor>
                        <ext:MultiField ID="MultiField11" runat="server" FieldLabel="护士3">
                            <Fields>
                                <ext:ComboBox ID="HS3" runat="server" StoreID="Store3" DisplayField="USER_NAME" ValueField="DB_USER"
                                    TypeAhead="false" LoadingText="Searching..." Width="220" PageSize="10" HideTrigger="false"
                                    ItemSelector="div.search-item" MinChars="1">
                                    <Template ID="Template8" runat="server">
                   <tpl for=".">
                      <div class="search-item">
                         <h3><span>{DB_USER}</span>{USER_NAME}</h3>
                  
                      </div>
                   </tpl>
                                    </Template>
                                </ext:ComboBox>
                            </Fields>
                        </ext:MultiField>
                    </ext:Anchor>
                    <ext:Anchor>
                        <ext:MultiField ID="MultiField13" runat="server" FieldLabel="护士4">
                            <Fields>
                                <ext:ComboBox ID="HS4" runat="server" StoreID="Store3" DisplayField="USER_NAME" ValueField="DB_USER"
                                    TypeAhead="false" LoadingText="Searching..." Width="220" PageSize="10" HideTrigger="false"
                                    ItemSelector="div.search-item" MinChars="1">
                                    <Template ID="Template9" runat="server">
                   <tpl for=".">
                      <div class="search-item">
                         <h3><span>{DB_USER}</span>{USER_NAME}</h3>
                  
                      </div>
                   </tpl>
                                    </Template>
                                </ext:ComboBox>
                            </Fields>
                        </ext:MultiField>
                    </ext:Anchor>
                    <ext:Anchor>
                        <ext:MultiField ID="MultiField14" runat="server" FieldLabel="护士5">
                            <Fields>
                                <ext:ComboBox ID="HS5" runat="server" StoreID="Store3" DisplayField="USER_NAME" ValueField="DB_USER"
                                    TypeAhead="false" LoadingText="Searching..." Width="220" PageSize="10" HideTrigger="false"
                                    ItemSelector="div.search-item" MinChars="1">
                                    <Template ID="Template10" runat="server">
                   <tpl for=".">
                      <div class="search-item">
                         <h3><span>{DB_USER}</span>{USER_NAME}</h3>
                  
                      </div>
                   </tpl>
                                    </Template>
                                </ext:ComboBox>
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
