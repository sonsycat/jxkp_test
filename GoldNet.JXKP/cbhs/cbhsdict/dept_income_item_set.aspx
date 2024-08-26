<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="dept_income_item_set.aspx.cs" Inherits="GoldNet.JXKP.cbhs.cbhsdict.dept_income_item_set" %>
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
</head>
<body>
<br />
    <form id="form1" runat="server">
    <div>
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
    <ext:FormPanel ID="FormPanel1" runat="server" Border="false" MonitorValid="true" ButtonAlign="Right"
            BodyStyle="background-color:transparent;margin:0,0,0,10px">
            <Body>
                <ext:FormLayout ID="FormLayout1" runat="server" LabelWidth="80">
                    <ext:Anchor>
                         <ext:TextField ID="DEPT_CODE" runat="server" Hidden="true"/>
                    </ext:Anchor>
                    <ext:Anchor>
                         <ext:TextField ID="DEPT_NAME" runat="server" DataIndex="DEPT_NAME" AllowBlank="false"
                                    Width="220"  EnableKeyEvents="true" FieldLabel="科室" Disabled="true"/>
                    </ext:Anchor>
                    <ext:Anchor>
                         <ext:TextField ID="ITEM_NAME" runat="server" DataIndex="ITEM_NAME" AllowBlank="false"
                                    Width="220"  EnableKeyEvents="true" FieldLabel="项目名称" Disabled="true"/>
                    </ext:Anchor>
                    <ext:Anchor>
                         <ext:TextField ID="ITEM_CLASS" runat="server" DataIndex="ITEM_CLASS" AllowBlank="false"
                                    Width="220"  EnableKeyEvents="true" FieldLabel="项目代码"  Disabled="true"/>
                    </ext:Anchor>
                    <ext:Anchor>
                        <ext:MultiField ID="MultiField1" runat="server" FieldLabel="住院开单比">
                            <Fields>
                                <ext:NumberField ID="ORDER_DEPT_DISTRIBUT" runat="server" DataIndex="ORDER_DEPT_DISTRIBUT" AllowBlank="false"
                                    Width="220"  EnableKeyEvents="true"  MaxValue="100" MinValue="0"  StyleSpec="text-align:right" SelectOnFocus="true" DecimalPrecision="2"/>
                                <ext:Label ID="Label1" runat="server" Html="<span style='color:Red;'>*</span>" />
                            </Fields>
                        </ext:MultiField>
                    </ext:Anchor>
                    <ext:Anchor>
                        <ext:MultiField ID="MultiField2" runat="server" FieldLabel="住院执行比">
                            <Fields>
                                <ext:NumberField ID="PERFORM_DEPT_DISTRIBUT" runat="server" DataIndex="PERFORM_DEPT_DISTRIBUT" AllowBlank="false"
                                    Width="220"  EnableKeyEvents="true" MaxValue="100" MinValue="0"  StyleSpec="text-align:right" SelectOnFocus="true" DecimalPrecision="2"/>
                                <ext:Label ID="Label2" runat="server" Html="<span style='color:Red;'>*</span>" />
                            </Fields>
                        </ext:MultiField>
                    </ext:Anchor>
                    <ext:Anchor>
                        <ext:MultiField ID="MultiField3" runat="server" FieldLabel="住院护理比">
                            <Fields>
                                <ext:NumberField ID="NURSING_PERCEN" runat="server" DataIndex="NURSING_PERCEN" AllowBlank="false"
                                    Width="220"  EnableKeyEvents="true" MaxValue="100" MinValue="0"  StyleSpec="text-align:right" SelectOnFocus="true" DecimalPrecision="2"/>
                                <ext:Label ID="Label3" runat="server" Html="<span style='color:Red;'>*</span>" />
                            </Fields>
                        </ext:MultiField>
                    </ext:Anchor>
                    <ext:Anchor>
                        <ext:MultiField ID="MultiField4" runat="server" FieldLabel="门诊开单比">
                            <Fields>
                                <ext:NumberField ID="OUT_OPDEPT_PERCEN" runat="server" DataIndex="OUT_OPDEPT_PERCEN" AllowBlank="false"
                                    Width="220"  EnableKeyEvents="true" MaxValue="100" MinValue="0"  StyleSpec="text-align:right" SelectOnFocus="true" DecimalPrecision="2"/>
                                <ext:Label ID="Label4" runat="server" Html="<span style='color:Red;'>*</span>" />
                            </Fields>
                        </ext:MultiField>
                    </ext:Anchor>
                    <ext:Anchor>
                        <ext:MultiField ID="MultiField5" runat="server" FieldLabel="门诊执行比">
                            <Fields>
                                <ext:NumberField ID="OUT_EXDEPT_PERCEN" runat="server" DataIndex="OUT_EXDEPT_PERCEN" AllowBlank="false"
                                    Width="220"  EnableKeyEvents="true" MaxValue="100" MinValue="0"  StyleSpec="text-align:right" SelectOnFocus="true" DecimalPrecision="2"/>
                                <ext:Label ID="Label5" runat="server" Html="<span style='color:Red;'>*</span>" />
                            </Fields>
                        </ext:MultiField>
                    </ext:Anchor>
                    <ext:Anchor>
                        <ext:MultiField ID="MultiField6" runat="server" FieldLabel="门诊护理比">
                            <Fields>
                                <ext:NumberField ID="OUT_NURSING_PERCEN" runat="server" DataIndex="OUT_NURSING_PERCEN" AllowBlank="false"
                                    Width="220"  EnableKeyEvents="true" MaxValue="100" MinValue="0"  StyleSpec="text-align:right" SelectOnFocus="true" DecimalPrecision="2"/>
                                <ext:Label ID="Label6" runat="server" Html="<span style='color:Red;'>*</span>" />
                            </Fields>
                        </ext:MultiField>
                    </ext:Anchor>
                    <ext:Anchor>
                        <ext:MultiField ID="MultiField7" runat="server" FieldLabel="合作医疗">
                            <Fields>
                                <ext:NumberField ID="COOPERANT_PERCEN" runat="server" DataIndex="COOPERANT_PERCEN" AllowBlank="false" 
                                    Width="220"  EnableKeyEvents="true" MaxValue="100" MinValue="0"  StyleSpec="text-align:right" SelectOnFocus="true" DecimalPrecision="2"/>
                                <ext:Label ID="Label7" runat="server" Html="<span style='color:Red;'>*</span>" />
                            </Fields>
                        </ext:MultiField>
                    </ext:Anchor>
                     <ext:Anchor>
                        <ext:MultiField ID="MultiField11" runat="server" FieldLabel="利润率">
                            <Fields>
                                <ext:NumberField ID="PROFIT_RATE" runat="server" DataIndex="PROFIT_RATE" AllowBlank="false" 
                                    Width="220"  EnableKeyEvents="true" MaxValue="100" MinValue="0" StyleSpec="text-align:right" SelectOnFocus="true" DecimalPrecision="2"/>
                                <ext:Label ID="Label11" runat="server" Html="<span style='color:Red;'>*</span>" />
                            </Fields>
                        </ext:MultiField>
                    </ext:Anchor>
                    <ext:Anchor>
                        <ext:MultiField ID="MultiField9" runat="server" FieldLabel="折算比">
                            <Fields>
                                <ext:NumberField ID="FIXED_PERCEN" runat="server" DataIndex="FIXED_PERCEN" AllowBlank="false" 
                                    Width="220"  EnableKeyEvents="true" MaxValue="100" MinValue="0"  StyleSpec="text-align:right" SelectOnFocus="true" DecimalPrecision="2">
                                    <Listeners>
                                        <Change  Handler="if(#{FIXED_PERCEN}.value<100){#{COST_CODE}.enable()} else{#{COST_CODE}.disable();#{Label10}.hide()}"/>
                                    </Listeners>
                                    </ext:NumberField>
                                <ext:Label ID="Label9" runat="server" Html="<span style='color:Red;'>*</span>" />
                            </Fields>
                        </ext:MultiField>
                    </ext:Anchor>
                    <ext:Anchor>
                        <ext:MultiField ID="MultiField10" runat="server" FieldLabel="成本对照">
                            <Fields>
                                <ext:ComboBox ID="COST_CODE" runat="server" StoreID="Store1" DisplayField="ITEM_NAME"
                                    AllowBlank="false" ValueField="ITEM_CODE" TypeAhead="false" LoadingText="Searching..."
                                    Width="220" PageSize="10" ItemSelector="div.search-item" MinChars="1" Disabled="true">
                                    <Template ID="Template4" runat="server">
                              <tpl for=".">
                               <div class="search-item">
                                 <h3>{ITEM_NAME}</h3>
                               </div>
                              </tpl>
                                    </Template>
                                </ext:ComboBox>
                                <ext:Label ID="Label10" runat="server" Html="<span style='color:Red;'>*</span>"  Hidden="true"/>
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
