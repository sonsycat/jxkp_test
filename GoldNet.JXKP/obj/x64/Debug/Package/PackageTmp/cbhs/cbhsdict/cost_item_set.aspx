<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="cost_item_set.aspx.cs"
    Inherits="GoldNet.JXKP.cbhs.cbhsdict.cost_item_set" %>

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
</head>
<body>

    <ext:ScriptManager ID="ScriptManager1" runat="server" />
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
    <form id="form1" runat="server">
    <div>
        <ext:FormPanel ID="FormPanel1" runat="server" Border="false" MonitorValid="true" ButtonAlign="Right"
            BodyStyle="background-color:transparent;margin:10px,0,0,10px">
            <Body>
                <ext:FormLayout ID="FormLayout1" runat="server" LabelWidth="80">
                    <ext:Anchor>
                        <ext:TextField ID="ITEM_CODE" runat="server" Hidden="true" />
                    </ext:Anchor>
                    <ext:Anchor>
                        <ext:TextField ID="ROW_ID" runat="server" Hidden="true" />
                    </ext:Anchor>
                    <ext:Anchor >
                        <ext:MultiField ID="MultiField1" runat="server" FieldLabel="项目类别">
                            <Fields>
                                <ext:ComboBox ID="ITEM_TYPE" runat="server" AllowBlank="false" Width="220" />
                                <ext:Label ID="Label1" runat="server" Html="<span style='color:Red;'>*</span>" />
                            </Fields>
                        </ext:MultiField>
                    </ext:Anchor>
                    <ext:Anchor>
                        <ext:MultiField ID="MultiField2" runat="server" FieldLabel="项目名称">
                            <Fields>
                                <ext:TextField ID="ITEM_NAME" runat="server" DataIndex="ITEM_NAME" AllowBlank="false"
                                    Width="220" EnableKeyEvents="true" />
                                <ext:Label ID="Label2" runat="server" Html="<span style='color:Red;'>*</span>" />
                            </Fields>
                        </ext:MultiField>
                    </ext:Anchor>
                    <ext:Anchor>
                        <ext:MultiField ID="MultiField3" runat="server" FieldLabel="输入码">
                            <Fields>
                                <ext:TextField ID="INPUT_CODE" runat="server" DataIndex="INPUT_CODE" AllowBlank="false"
                                    Width="220" EnableKeyEvents="true" />
                                <ext:Label ID="Label3" runat="server" Html="<span style='color:Red;'>*</span>" />
                            </Fields>
                        </ext:MultiField>
                    </ext:Anchor>
                    <ext:Anchor>
                        <ext:MultiField ID="MultiField4" runat="server" FieldLabel="成本属性">
                            <Fields>
                                <ext:ComboBox ID="COST_PROPERTY" runat="server" AllowBlank="false" Width="220" />
                                <ext:Label ID="Label4" runat="server" Html="<span style='color:Red;'>*</span>" />
                            </Fields>
                        </ext:MultiField>
                    </ext:Anchor>
                    <ext:Anchor>
                        <ext:MultiField ID="MultiField5" runat="server" FieldLabel="军地分摊方案">
                            <Fields>
                                <ext:ComboBox ID="ALLOT_FOR_JD" runat="server" AllowBlank="false" Width="220" />
                                <ext:Label ID="Label5" runat="server" Html="<span style='color:Red;'>*</span>" />
                            </Fields>
                        </ext:MultiField>
                    </ext:Anchor>
                    <ext:Anchor>
                        <ext:MultiField ID="MultiField6" runat="server" FieldLabel="级次分摊方案">
                            <Fields>
                                <ext:ComboBox ID="ALLOT_FOR_JC" runat="server" AllowBlank="false" Width="220" />
                                <ext:Label ID="Label6" runat="server" Html="<span style='color:Red;'>*</span>" />
                            </Fields>
                        </ext:MultiField>
                    </ext:Anchor>
                    <ext:Anchor>
                        <ext:MultiField ID="MultiField7" runat="server" FieldLabel="人员分摊方案">
                            <Fields>
                                <ext:ComboBox ID="ALLOT_FOR_RY" runat="server" AllowBlank="false" Width="220" />
                                <ext:Label ID="Label7" runat="server" Html="<span style='color:Red;'>*</span>" />
                            </Fields>
                        </ext:MultiField>
                    </ext:Anchor>
                      <ext:Anchor>
                        <ext:MultiField ID="MultiField10" runat="server" FieldLabel="核算类型">
                            <Fields>
                                <ext:ComboBox ID="ACCOUNT_TYPE" runat="server" AllowBlank="false" Width="220" FieldLabel="核算类型" 
                                 DisplayField="ACCOUNT_TYPE" ValueField="ID" StoreID="Store2" 
                                />
                                <ext:Label ID="Label10" runat="server" Html="<span style='color:Red;'>*</span>" />
                            </Fields>
                        </ext:MultiField>
                    </ext:Anchor>
                    <ext:Anchor>
                        <ext:MultiField ID="MultiField11" runat="server" FieldLabel="直接/间接成本">
                            <Fields>
                                <ext:ComboBox ID="COST_DIRECT" runat="server" AllowBlank="false" Width="220" FieldLabel="直接/间接成本" />
                                <ext:Label ID="Label11" runat="server" Html="<span style='color:Red;'>*</span>" />
                            </Fields>
                        </ext:MultiField>
                    </ext:Anchor>
                    <ext:Anchor>
                        <ext:MultiField ID="MultiField8" runat="server" FieldLabel="获取方式">
                            <Fields>
                                <ext:ComboBox ID="GETTYPE" runat="server" AllowBlank="false" Width="220" FieldLabel="获取方式" />
                                <ext:Label ID="Label8" runat="server" Html="<span style='color:Red;'>*</span>" />
                            </Fields>
                        </ext:MultiField>
                    </ext:Anchor>
                      <ext:Anchor>
                        <ext:MultiField ID="MultiField9" runat="server" FieldLabel="计入百分比">
                            <Fields>
                                <ext:NumberField ID="COMPUTE_PER" runat="server" AllowBlank="false" Width="220" FieldLabel="计入百分比" 
                                 Text="100" SelectOnFocus="true" DecimalPrecision="2" MaxValue="100" MinValue="0" StyleSpec="text-align:right"/>
                                <ext:Label ID="Label9" runat="server" Html="<span style='color:Red;'>*</span>" />
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
