<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Sys_Menu_Dict.aspx.cs"
    Inherits="GoldNet.JXKP.WebPage.SysManager.Sys_Menu_Dict" %>

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
    <ext:ScriptManager ID="ScriptManager1" runat="server">
    </ext:ScriptManager>
    <ext:Store ID="Store1" runat="server" AutoLoad="true">
        <Reader>
            <ext:JsonReader ReaderID="APP_ID">
                <Fields>
                    <ext:RecordField Name="APP_ID" />
                    <ext:RecordField Name="APP_NAME" />
                </Fields>
            </ext:JsonReader>
        </Reader>
    </ext:Store>
    <ext:Store ID="Store2" runat="server" AutoLoad="true">
        <Reader>
            <ext:JsonReader>
                <Fields>
                    <ext:RecordField Name="GROUPTEXT" />
                    <ext:RecordField Name="GROUPTEXT" />
                </Fields>
            </ext:JsonReader>
        </Reader>
    </ext:Store>
    <ext:Store ID="Store3" runat="server" AutoLoad="true">
        <Reader>
            <ext:JsonReader>
                <Fields>
                    <ext:RecordField Name="TYPE_ID" />
                    <ext:RecordField Name="TYPE_NAME" />
                </Fields>
            </ext:JsonReader>
        </Reader>
    </ext:Store>
    <ext:Store ID="Store4" runat="server" AutoLoad="true">
        <Reader>
            <ext:JsonReader>
                <Fields>
                    <ext:RecordField Name="ATTR_ID" />
                    <ext:RecordField Name="ATTR_NAME" />
                </Fields>
            </ext:JsonReader>
        </Reader>
    </ext:Store>
</head>
<body>
    <form id="form1" runat="server" style="background-color: Transparent">
    <div>
        <ext:FormPanel ID="FormPanel1" runat="server" Border="false" AutoScroll="false" ButtonAlign="Right"
            StyleSpec="background-color:Transparent" BodyStyle="background-color:Transparent;margin:10px,0,0,10px">
            <Body>
                <ext:FormLayout ID="FormLayout1" runat="server">
                    <ext:Anchor Horizontal="95%">
                        <ext:ComboBox ID="Menutypes" runat="server" StoreID="Store3" DisplayField="TYPE_NAME"
                            ValueField="TYPE_ID" Width="100" FieldLabel="菜单类型" CausesValidation="true" AllowBlank="false"
                            ReadOnly="true">
                        </ext:ComboBox>
                    </ext:Anchor>
                    <ext:Anchor Horizontal="95%">
                        <ext:ComboBox ID="apptype" runat="server" StoreID="Store1" DisplayField="APP_NAME"
                            ValueField="APP_ID" Width="100" FieldLabel="菜单模块" CausesValidation="true" AllowBlank="false"
                            ReadOnly="true">
                            <AjaxEvents>
                                <Select OnEvent="Selectedtype">
                                    <EventMask ShowMask="true" />
                                </Select>
                            </AjaxEvents>
                        </ext:ComboBox>
                    </ext:Anchor>
                    <ext:Anchor Horizontal="95%">
                        <ext:ComboBox ID="menutype" runat="server" StoreID="Store2" DisplayField="GROUPTEXT"
                            ValueField="GROUPTEXT" Width="100" FieldLabel="模块分类" CausesValidation="true"
                            AllowBlank="false" ReadOnly="true">
                        </ext:ComboBox>
                    </ext:Anchor>
                    <ext:Anchor Horizontal="95%">
                        <ext:ComboBox ID="Menuattr" runat="server" StoreID="Store4" DisplayField="ATTR_NAME"
                            ValueField="ATTR_ID" Width="100" FieldLabel="菜单属性" CausesValidation="true" AllowBlank="false"
                            ReadOnly="true">
                        </ext:ComboBox>
                    </ext:Anchor>
                    <ext:Anchor Horizontal="95%">
                        <ext:TextField runat="server" ID="MENU_NAME" CausesValidation="true" FieldLabel="菜单名称"
                            AllowBlank="false" />
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
                        <Click Handler="parent.Menudict.hide();" />
                    </Listeners>
                </ext:Button>
            </Buttons>
        </ext:FormPanel>
    </div>
    </form>
</body>
</html>
