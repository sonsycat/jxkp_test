<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="incomes_item_set.aspx.cs"
    Inherits="GoldNet.JXKP.cbhs.cbhsdict.incomes_item_set" %>

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

          <ext:FormPanel ID="FormPanel1" runat="server" Border="false" AutoHeight="true" AutoScroll="true"
        ButtonAlign="Right" StyleSpec="background-color:transparent" BodyStyle="background-color:transparent">
            <Body>
                <ext:Panel ID="Panel3" runat="server" Border="false" AutoHeight="true" AutoWidth="true"
                    StyleSpec="background-color:transparent" BodyStyle="background-color:transparent">
                    <TopBar>
                        <ext:Toolbar ID="Toolbar1" runat="server">
                            <Items>
                                <ext:ToolbarSeparator ID="ToolbarSeparator1" runat="server">
                                </ext:ToolbarSeparator>
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
                                <ext:TextField ID="FLAG" runat="server" Hidden="true" />
                            </Items>
                        </ext:Toolbar>
                    </TopBar>
                </ext:Panel>
                <ext:Panel ID="Panel4" runat="server" Border="false" AutoWidth="true" AutoHeight="true"
                    AutoScroll="true" StyleSpec="background-color:transparent" BodyStyle="background-color:transparent">
                    <Body>
                        <ext:FieldSet ID="fieldset1" runat="server" Title="基本属性" Collapsible="true" Collapsed="false"
                            StyleSpec="margin:0px" BodyStyle="background-color:Transparent;">
                            <Body>
                                <table width="100%">
                                    <tr>
                                        <td align="right" width="15%">
                                            <font face="宋体">项目名称：</font>
                                        </td>
                                        <td width="35%">
                                            <nobr>
                                        <ext:TextField ID="ITEM_NAME" runat="server" Width="120">
                                        </ext:TextField>
                                       </nobr>
                                        </td>
                                        <td align="right" width="15%">
                                            <font face="宋体">输入代码：</font>
                                        </td>
                                        <td width="35%">
                                            <nobr>
                                        <ext:TextField ID="INPUT_CODE" runat="server" Width="120">
                                        </ext:TextField>
                                       </nobr>
                                        </td>
                                    </tr>
                                    
                                </table>
                            </Body>
                        </ext:FieldSet>
                        <ext:FieldSet ID="fieldset2" runat="server" Title="分摊比例" Collapsible="true" Collapsed="false"
                            StyleSpec="margin:0px" BodyStyle="background-color:Transparent;">
                            <Body>
                                <table width="100%">
                                    
                                    <tr>
                                        <td align="right">
                                            <font face="宋体">住院开单：</font>
                                        </td>
                                        <td>
                                            <ext:NumberField ID="ORDER_DEPT_DISTRIBUT" runat="server" DataIndex="ORDER_DEPT_DISTRIBUT"
                                                AllowBlank="false" Width="120" EnableKeyEvents="true" MaxValue="1000" MinValue="0"
                                                StyleSpec="text-align:right" SelectOnFocus="true" DecimalPrecision="2" />
                                            <ext:Label ID="Label13" runat="server" Html="<span style='color:Red;'>*</span>" />
                                        </td>
                                        <td align="right">
                                            <font face="宋体">门诊开单：</font>
                                        </td>
                                        <td>
                                            <ext:NumberField ID="OUT_OPDEPT_PERCEN" runat="server" DataIndex="OUT_OPDEPT_PERCEN"
                                                AllowBlank="false" Width="120" EnableKeyEvents="true" MaxValue="1000" MinValue="0"
                                                StyleSpec="text-align:right" SelectOnFocus="true" DecimalPrecision="2" />
                                            <ext:Label ID="Label14" runat="server" Html="<span style='color:Red;'>*</span>" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="right">
                                            <font face="宋体">住院执行：</font>
                                        </td>
                                        <td>
                                            <ext:NumberField ID="PERFORM_DEPT_DISTRIBUT" runat="server" DataIndex="PERFORM_DEPT_DISTRIBUT"
                                                AllowBlank="false" Width="120" EnableKeyEvents="true" MaxValue="1000" MinValue="0"
                                                StyleSpec="text-align:right" SelectOnFocus="true" DecimalPrecision="2" />
                                            <ext:Label ID="Label18" runat="server" Html="<span style='color:Red;'>*</span>" />
                                        </td>
                                        <td align="right">
                                            <font face="宋体">门诊执行：</font>
                                        </td>
                                        <td>
                                            <ext:NumberField ID="OUT_EXDEPT_PERCEN" runat="server" DataIndex="OUT_EXDEPT_PERCEN"
                                                AllowBlank="false" Width="120" EnableKeyEvents="true" MaxValue="1000" MinValue="0"
                                                StyleSpec="text-align:right" SelectOnFocus="true" DecimalPrecision="2" />
                                            <ext:Label ID="Label19" runat="server" Html="<span style='color:Red;'>*</span>" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="right">
                                            <font face="宋体">住院护理：</font>
                                        </td>
                                        <td>
                                            <ext:NumberField ID="NURSING_PERCEN" runat="server" DataIndex="NURSING_PERCEN" AllowBlank="false"
                                                Width="120" EnableKeyEvents="true" MaxValue="1000" MinValue="0" StyleSpec="text-align:right"
                                                SelectOnFocus="true" DecimalPrecision="2" />
                                            <ext:Label ID="Label21" runat="server" Html="<span style='color:Red;'>*</span>" />
                                        </td>
                                        <td align="right">
                                            <font face="宋体">门诊护理：</font>
                                        </td>
                                        <td>
                                            <ext:NumberField ID="OUT_NURSING_PERCEN" runat="server" DataIndex="OUT_NURSING_PERCEN"
                                                AllowBlank="false" Width="120" EnableKeyEvents="true" MaxValue="1000" MinValue="0"
                                                StyleSpec="text-align:right" SelectOnFocus="true" DecimalPrecision="2" />
                                            <ext:Label ID="Label22" runat="server" Html="<span style='color:Red;'>*</span>" />
                                        </td>
                                    </tr>
                                   
                                </table>
                            </Body>
                        </ext:FieldSet>
                        <ext:FieldSet ID="fieldset3" runat="server" Title="其它属性" Collapsible="true" Collapsed="false"
                            StyleSpec="margin:0px" BodyStyle="background-color:Transparent;">
                            <Body>
                                <table width="100%">
                                    
                                  
                                    <tr>
                                        <td align="right">
                                            <font face="宋体">合作医疗：</font>
                                        </td>
                                        <td>
                                            <ext:NumberField ID="COOPERANT_PERCEN" runat="server" DataIndex="COOPERANT_PERCEN"
                                                AllowBlank="false" Width="120" EnableKeyEvents="true" MaxValue="100" MinValue="0"
                                                StyleSpec="text-align:right" SelectOnFocus="true" DecimalPrecision="2" />
                                            <ext:Label ID="Label23" runat="server" Html="<span style='color:Red;'>*</span>" />
                                        </td>
                                        <td align="right">
                                            <font face="宋体">分配科室：</font>
                                        </td>
                                        <td>
                                            <ext:ComboBox ID="cbbdept" runat="server" StoreID="SDept" DisplayField="DEPT_NAME"
                                                ValueField="DEPT_CODE" TypeAhead="false" LoadingText="Searching..." Width="120"
                                                PageSize="10" ItemSelector="div.search-item" MinChars="1" ListWidth="200">
                                                <Template ID="Template2" runat="server">
                                                    <tpl for=".">
                                                        <div class="search-item">
                                                             <h3>{DEPT_NAME}</h3>
                                                             </div>
                                                      </tpl>                                                                                                       
                                                </Template>
                                            </ext:ComboBox>
                                            <ext:Label ID="Label24" runat="server" Html="<span style='color:Red;'>*</span>" Hidden="true" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="right">
                                            <font face="宋体">收入类别：</font>
                                        </td>
                                        <td>
                                            <ext:ComboBox ID="classtype" runat="server" EmptyText="请选收入类别..." AllowBlank="false"
                                                Width="120" DisplayField="INCOM_TYPE_NAME" ValueField="INCOM_TYPE_CODE" StoreID="Store3">
                                            </ext:ComboBox>
                                            <ext:Label ID="Label25" runat="server" Html="<span style='color:Red;'>*</span>" />
                                        </td>
                                        <td align="right">
                                            <font face="宋体">核算类型：</font>
                                        </td>
                                        <td>
                                            <ext:ComboBox ID="CALCULATION_TYPE" runat="server" EmptyText="请选核算类型..." AllowBlank="false"
                                                Width="120" DisplayField="ACCOUNT_TYPE" ValueField="ID" StoreID="Store2">
                                            </ext:ComboBox>
                                            <ext:Label ID="Label26" runat="server" Html="<span style='color:Red;'>*</span>" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="right">
                                            <font face="宋体">固定折算：</font>
                                        </td>
                                        <td>
                                            <ext:NumberField ID="FIXED_PERCEN" runat="server" DataIndex="FIXED_PERCEN" AllowBlank="false"
                                                Width="120" EnableKeyEvents="true" MaxValue="100" MinValue="0" StyleSpec="text-align:right"
                                                SelectOnFocus="true" DecimalPrecision="2">
                                                <Listeners>
                                                    <Change Handler="if(#{FIXED_PERCEN}.value<100){#{COST_CODE}.enable()} else{#{COST_CODE}.disable();#{Label10}.hide()}" />
                                                </Listeners>
                                            </ext:NumberField><br />
                                            <ext:Label ID="Label27" runat="server" Html="<span style='color:Red;'>*成本=(100-固定折算)</span>" />
                                        </td>
                                        <td align="right">
                                            <font face="宋体">成本对照：</font>
                                        </td>
                                        <td>
                                            <ext:ComboBox ID="COST_CODE" runat="server" StoreID="Store1" DisplayField="ITEM_NAME"
                                                AllowBlank="true" ValueField="ITEM_CODE" TypeAhead="false" LoadingText="Searching..."
                                                Width="120" PageSize="10" ItemSelector="div.search-item" MinChars="1" Disabled="true">
                                                <Template ID="Template3" runat="server">
                                                          <tpl for=".">
                                                           <div class="search-item">
                                                             <h3>{ITEM_NAME}</h3>
                                                           </div>
                                                          </tpl>
                                                </Template>
                                            </ext:ComboBox>
                                            <ext:Label ID="Label10" runat="server" Html="<span style='color:Red;'>*</span>" Hidden="true" />
                                        </td>
                                    </tr>
                                </table>
                            </Body>
                        </ext:FieldSet>
                    </Body>
                </ext:Panel>
            </Body>
        </ext:FormPanel>
    </div>
    </form>
</body>
</html>
