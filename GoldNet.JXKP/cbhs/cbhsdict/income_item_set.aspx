<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="income_item_set.aspx.cs"
    Inherits="GoldNet.JXKP.cbhs.cbhsdict.income_item_set" %>

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
                                            Width="120" EnableKeyEvents="true" FieldLabel="项目名称"  ReadOnly="true" />
                                    </ext:Anchor>
                                    <ext:Anchor>
                                        <ext:TextField ID="ITEM_CLASS" runat="server" DataIndex="ITEM_CLASS" AllowBlank="false"
                                            Width="120" EnableKeyEvents="true" FieldLabel="项目代码"  ReadOnly="true" />
                                    </ext:Anchor>
                                    <ext:Anchor>
                                        <ext:TextField ID="INPUT_CODE" runat="server" DataIndex="INPUT_CODE" AllowBlank="false"
                                            Width="120" EnableKeyEvents="true" FieldLabel="输入码" />
                                    </ext:Anchor>
                                    <ext:Anchor>
                                        <ext:MultiField ID="MultiField1" runat="server" FieldLabel="住院开单比">
                                            <Fields>
                                                <ext:NumberField ID="ORDER_DEPT_DISTRIBUT" runat="server" DataIndex="ORDER_DEPT_DISTRIBUT"
                                                    AllowBlank="false" Width="120" EnableKeyEvents="true" MaxValue="1000" MinValue="0"
                                                    StyleSpec="text-align:right" SelectOnFocus="true" DecimalPrecision="2" />
                                                <ext:Label ID="Label1" runat="server" Html="<span style='color:Red;'>*</span>" />
                                            </Fields>
                                        </ext:MultiField>
                                    </ext:Anchor>
                                    <ext:Anchor>
                                        <ext:MultiField ID="MultiField2" runat="server" FieldLabel="住院执行比">
                                            <Fields>
                                                <ext:NumberField ID="PERFORM_DEPT_DISTRIBUT" runat="server" DataIndex="PERFORM_DEPT_DISTRIBUT"
                                                    AllowBlank="false" Width="120" EnableKeyEvents="true" MaxValue="1000" MinValue="0"
                                                    StyleSpec="text-align:right" SelectOnFocus="true" DecimalPrecision="2" />
                                                <ext:Label ID="Label2" runat="server" Html="<span style='color:Red;'>*</span>" />
                                            </Fields>
                                        </ext:MultiField>
                                    </ext:Anchor>
                                    <ext:Anchor>
                                        <ext:MultiField ID="MultiField3" runat="server" FieldLabel="住院护理比">
                                            <Fields>
                                                <ext:NumberField ID="NURSING_PERCEN" runat="server" DataIndex="NURSING_PERCEN" AllowBlank="false"
                                                    Width="120" EnableKeyEvents="true" MaxValue="1000" MinValue="0" StyleSpec="text-align:right"
                                                    SelectOnFocus="true" DecimalPrecision="2" />
                                                <ext:Label ID="Label3" runat="server" Html="<span style='color:Red;'>*</span>" />
                                            </Fields>
                                        </ext:MultiField>
                                    </ext:Anchor>
                                    <ext:Anchor>
                                        <ext:MultiField ID="MultiField4" runat="server" FieldLabel="门诊开单比">
                                            <Fields>
                                                <ext:NumberField ID="OUT_OPDEPT_PERCEN" runat="server" DataIndex="OUT_OPDEPT_PERCEN"
                                                    AllowBlank="false" Width="120" EnableKeyEvents="true" MaxValue="1000" MinValue="0"
                                                    StyleSpec="text-align:right" SelectOnFocus="true" DecimalPrecision="2" />
                                                <ext:Label ID="Label4" runat="server" Html="<span style='color:Red;'>*</span>" />
                                            </Fields>
                                        </ext:MultiField>
                                    </ext:Anchor>
                                    <ext:Anchor>
                                        <ext:MultiField ID="MultiField5" runat="server" FieldLabel="门诊执行比">
                                            <Fields>
                                                <ext:NumberField ID="OUT_EXDEPT_PERCEN" runat="server" DataIndex="OUT_EXDEPT_PERCEN"
                                                    AllowBlank="false" Width="120" EnableKeyEvents="true" MaxValue="1000" MinValue="0"
                                                    StyleSpec="text-align:right" SelectOnFocus="true" DecimalPrecision="2" />
                                                <ext:Label ID="Label5" runat="server" Html="<span style='color:Red;'>*</span>" />
                                            </Fields>
                                        </ext:MultiField>
                                    </ext:Anchor>
                                    <ext:Anchor>
                                        <ext:MultiField ID="MultiField6" runat="server" FieldLabel="门诊护理比">
                                            <Fields>
                                                <ext:NumberField ID="OUT_NURSING_PERCEN" runat="server" DataIndex="OUT_NURSING_PERCEN"
                                                    AllowBlank="false" Width="120" EnableKeyEvents="true" MaxValue="1000" MinValue="0"
                                                    StyleSpec="text-align:right" SelectOnFocus="true" DecimalPrecision="2" />
                                                <ext:Label ID="Label6" runat="server" Html="<span style='color:Red;'>*</span>" />
                                            </Fields>
                                        </ext:MultiField>
                                    </ext:Anchor>
                                    <ext:Anchor>
                                        <ext:MultiField ID="MultiField7" runat="server" FieldLabel="合作医疗">
                                            <Fields>
                                                <ext:NumberField ID="COOPERANT_PERCEN" runat="server" DataIndex="COOPERANT_PERCEN"
                                                    AllowBlank="false" Width="120" EnableKeyEvents="true" MaxValue="1000" MinValue="0"
                                                    StyleSpec="text-align:right" SelectOnFocus="true" DecimalPrecision="2" />
                                                <ext:Label ID="Label7" runat="server" Html="<span style='color:Red;'>*</span>" />
                                            </Fields>
                                        </ext:MultiField>
                                    </ext:Anchor>
                                    
                                </ext:FormLayout>
                            </Body>
                        </ext:Panel>
                    </ext:LayoutColumn>
                    <ext:LayoutColumn ColumnWidth=".5">
                        <ext:Panel ID="Panel2" runat="server" Height="280" Border="false" BodyStyle="background-color:transparent;>
                            <Body>
                                <ext:FormLayout ID="FormLayout2" runat="server" LabelWidth="80">
                                    <ext:Anchor>
                                        <ext:MultiField ID="MultiField12" runat="server" FieldLabel="分配科室">
                                            <Fields>
                                                <ext:ComboBox ID="cbbdept" runat="server" StoreID="SDept" DisplayField="DEPT_NAME"
                                                    ValueField="DEPT_CODE" TypeAhead="false" LoadingText="Searching..." Width="120"
                                                    PageSize="10" ItemSelector="div.search-item" MinChars="1" ListWidth="200">
                                                    <Template ID="Template1" runat="server">
                                                    <tpl for=".">
                                                        <div class="search-item">
                                                             <h3>{DEPT_NAME}</h3>
                                                             </div>
                                                      </tpl>                                                                                                       
                                                    </Template>
                                                </ext:ComboBox>
                                                <ext:Label ID="Label12" runat="server" Html="<span style='color:Red;'>*</span>" Hidden="true" />
                                            </Fields>
                                        </ext:MultiField>
                                    </ext:Anchor>
                                    
                                  
                                    <ext:Anchor>
                                        <ext:MultiField ID="MultiField15" runat="server" FieldLabel="直接成本比例">
                                            <Fields>
                                                <ext:NumberField ID="ZJCBBL" runat="server" Width="120" EnableKeyEvents="true" MaxValue="100"
                                                    MinValue="0" StyleSpec="text-align:right" SelectOnFocus="true" DecimalPrecision="2" />
                                                <ext:Label ID="Label15" runat="server" Html="<span style='color:Red;'>*</span>" />
                                            </Fields>
                                        </ext:MultiField>
                                    </ext:Anchor>
                                    <ext:Anchor>
                                        <ext:MultiField ID="MultiField16" runat="server" FieldLabel="间接成本比例">
                                            <Fields>
                                                <ext:NumberField ID="JJCBBL" runat="server" Width="120" EnableKeyEvents="true" MaxValue="100"
                                                    MinValue="0" StyleSpec="text-align:right" SelectOnFocus="true" DecimalPrecision="2" />
                                                <ext:Label ID="Label16" runat="server" Html="<span style='color:Red;'>*</span>" />
                                            </Fields>
                                        </ext:MultiField>
                                    </ext:Anchor>
                                    <ext:Anchor>
                                        <ext:MultiField ID="MultiField17" runat="server" FieldLabel="单次成本">
                                            <Fields>
                                                <ext:NumberField ID="DCCB" runat="server" Width="120" EnableKeyEvents="true" MaxValue="100"
                                                    MinValue="0" StyleSpec="text-align:right" SelectOnFocus="true" DecimalPrecision="2" />
                                                <ext:Label ID="Label17" runat="server" Html="<span style='color:Red;'>*</span>" />
                                            </Fields>
                                        </ext:MultiField>
                                    </ext:Anchor>
                                    <ext:Anchor>
                                       <ext:MultiField ID="MultiField20" runat="server" FieldLabel="收入类别">
                                            <Fields>
                                                <ext:ComboBox ID="classtype" runat="server" EmptyText="请选收入类别..." AllowBlank="false"
                                                    Width="120" DisplayField="INCOM_TYPE_NAME" ValueField="INCOM_TYPE_CODE" StoreID="Store3">
                                                </ext:ComboBox>
                                                <ext:Label ID="Label20" runat="server" Html="<span style='color:Red;'>*</span>" />
                                            </Fields>
                                        </ext:MultiField>
                                    </ext:Anchor>
                                    <ext:Anchor>
                                        <ext:MultiField ID="MultiField11" runat="server" FieldLabel="利润率">
                                            <Fields>
                                                <ext:NumberField ID="PROFIT_RATE" runat="server" DataIndex="PROFIT_RATE" AllowBlank="false"
                                                    Width="120" EnableKeyEvents="true" MaxValue="100" MinValue="0" StyleSpec="text-align:right"
                                                    SelectOnFocus="true" DecimalPrecision="2" />
                                                <ext:Label ID="Label11" runat="server" Html="<span style='color:Red;'>*</span>" />
                                            </Fields>
                                        </ext:MultiField>
                                    </ext:Anchor>
                                    <ext:Anchor>
                                        <ext:MultiField ID="MultiField8" runat="server" FieldLabel="核算类型">
                                            <Fields>
                                                <ext:ComboBox ID="CALCULATION_TYPE" runat="server" EmptyText="请选核算类型..." AllowBlank="false"
                                                    Width="120" DisplayField="ACCOUNT_TYPE" ValueField="ID" StoreID="Store2">
                                                </ext:ComboBox>
                                                <ext:Label ID="Label8" runat="server" Html="<span style='color:Red;'>*</span>" />
                                            </Fields>
                                        </ext:MultiField>
                                    </ext:Anchor>
                                    <ext:Anchor>
                                        <ext:MultiField ID="MultiField9" runat="server" FieldLabel="固定折算比">
                                            <Fields>
                                                <ext:NumberField ID="FIXED_PERCEN" runat="server" DataIndex="FIXED_PERCEN" AllowBlank="false"
                                                    Width="120" EnableKeyEvents="true" MaxValue="100" MinValue="0" StyleSpec="text-align:right"
                                                    SelectOnFocus="true" DecimalPrecision="2">
                                                    <Listeners>
                                                        <Change Handler="if(#{FIXED_PERCEN}.value<100){#{COST_CODE}.enable()} else{#{COST_CODE}.disable();#{Label10}.hide()}" />
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
                                                    AllowBlank="true" ValueField="ITEM_CODE" TypeAhead="false" LoadingText="Searching..."
                                                    Width="120" PageSize="10" ItemSelector="div.search-item" MinChars="1" Disabled="true">
                                                    <Template ID="Template4" runat="server">
                                                          <tpl for=".">
                                                           <div class="search-item">
                                                             <h3>{ITEM_NAME}</h3>
                                                           </div>
                                                          </tpl>
                                                    </Template>
                                                </ext:ComboBox>
                                                <ext:Label ID="Label10" runat="server" Html="<span style='color:Red;'>*</span>" Hidden="true" />
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
