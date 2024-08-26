<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Sys_Menu_Edit.aspx.cs" Inherits="GoldNet.JXKP.WebPage.SysManager.Sys_Menu_Edit" %>
<%@ Register Assembly="Goldnet.Ext.Web" Namespace="Goldnet.Ext.Web" TagPrefix="ext" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title></title>
       <style type="text/css">
        body{
         background-color: #DFE8F6;
         font-size:12px;
        }
    </style>
     <ext:ScriptManager ID="ScriptManager1" runat="server">
 </ext:ScriptManager>
  <ext:Store ID="Store1" runat="server" AutoLoad="true">
        <Reader>
            <ext:JsonReader ReaderID="TYPE_ID">
                <Fields>
                    <ext:RecordField Name="TYPE_ID"  />
                    <ext:RecordField Name="TYPE_NAME" />
                </Fields>
            </ext:JsonReader>
        </Reader>
    </ext:Store>
      <ext:Store ID="Store2" runat="server" AutoLoad="true">
        <Reader>
            <ext:JsonReader ReaderID="ID">
                <Fields>
                    <ext:RecordField Name="ID"  />
                    <ext:RecordField Name="TYPES" />
                </Fields>
            </ext:JsonReader>
        </Reader>
    </ext:Store>
    <ext:Store ID="Store3" runat="server" AutoLoad="true">
        <Reader>
            <ext:JsonReader ReaderID="MENU_ATTR_ID">
                <Fields>
                    <ext:RecordField Name="MENU_ATTR_ID"  />
                    <ext:RecordField Name="MENU_ATTR_NAME" />
                </Fields>
            </ext:JsonReader>
        </Reader>
    </ext:Store>
    <ext:Store ID="Store4" runat="server" AutoLoad="true">
        <Reader>
            <ext:JsonReader ReaderID="GUIDE_CODE">
                <Fields>
                    <ext:RecordField Name="GUIDE_CODE"  />
                    <ext:RecordField Name="GUIDE_NAME" />
                </Fields>
            </ext:JsonReader>
        </Reader>
    </ext:Store>
</head>
<body>
 <form id="form1" runat="server"  style="background-color:Transparent" >   
    <div>
        <ext:FormPanel ID="FormPanel1" runat="server" Border="false" AutoScroll="false" ButtonAlign="Right"
            StyleSpec="background-color:Transparent" BodyStyle="background-color:Transparent;margin:10px,0,0,10px">
            <Body>               
                <ext:FormLayout ID="FormLayout1" runat="server">
                    <ext:Anchor Horizontal="95%">
                       <ext:TextField runat="server" ID="MENU_GUIDE_NAME"  CausesValidation="true"  FieldLabel="字段名称"   AllowBlank="false"/>
                    </ext:Anchor>
                    <ext:Anchor Horizontal="95%">
                       <ext:ComboBox ID="apptype" runat="server" StoreID="Store1" DisplayField="TYPE_NAME"
                            ValueField="TYPE_ID" Width="100" FieldLabel="字段类型"  CausesValidation="true" AllowBlank="false" ReadOnly="true">
                        </ext:ComboBox>
                    </ext:Anchor>
                    <ext:Anchor Horizontal="95%">
                       <ext:ComboBox ID="menuattr" runat="server" StoreID="Store3" DisplayField="MENU_ATTR_NAME"
                            ValueField="MENU_ATTR_ID" Width="100" FieldLabel="字段属性"  CausesValidation="true" AllowBlank="false" ReadOnly="true">
                        </ext:ComboBox>
                    </ext:Anchor>
                    <ext:Anchor Horizontal="95%">
                       <ext:ComboBox ID="accounttype" runat="server" StoreID="Store2" DisplayField="TYPES"
                            ValueField="ID" Width="100" FieldLabel="是否求和"  CausesValidation="true" AllowBlank="false" ReadOnly="true">
                        </ext:ComboBox>
                    </ext:Anchor>
                    <ext:Anchor Horizontal="95%">
                         <ext:NumberField runat="server" ID="SHOW_WIDTH" CausesValidation="true" FieldLabel="显示宽度" AllowBlank="false"></ext:NumberField>
                    </ext:Anchor>
                    <ext:Anchor Horizontal="95%">
                       <ext:TextField runat="server" ID="guidecode"  CausesValidation="true"  FieldLabel="关联指标"   AllowBlank="false"/>
                    </ext:Anchor>
                </ext:FormLayout>
            </Body>
            <Buttons>
                <ext:Button ID="BtnSave" runat="server" Text="保存" Icon="Disk">
                    <AjaxEvents>
                        <Click OnEvent="SaveProg_onClick">
                        </Click>
                    </AjaxEvents>
                </ext:Button>
                <ext:Button ID="CancelButton" runat="server" Text="取消" Icon="Cancel">
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
