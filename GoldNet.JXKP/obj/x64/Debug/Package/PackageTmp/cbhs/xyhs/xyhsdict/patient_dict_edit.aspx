<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="patient_dict_edit.aspx.cs" Inherits="GoldNet.JXKP.cbhs.xyhs.xyhsdict.patient_dict_edit" %>
<%@ Register Assembly="Goldnet.Ext.Web" Namespace="Goldnet.Ext.Web" TagPrefix="ext" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
     <title></title>
    <link rel="stylesheet" type="text/css" href="/resources/css/main.css" />
    <style type="text/css">
        body{
         background-color: #DFE8F6;
         font-size:12px;
        }
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
<ext:ScriptManager ID="ScriptManager1" runat="server">
    </ext:ScriptManager>
     <ext:Store ID="Store2" runat="server" AutoLoad="true">
        <Reader>
            <ext:JsonReader ReaderID="ID">
                <Fields>
                    <ext:RecordField Name="DIAGNOSIS_CODE" />
                    <ext:RecordField Name="DIAGNOSIS_NAME" />
                </Fields>
            </ext:JsonReader>
        </Reader>
    </ext:Store>
        <ext:ViewPort ID="ViewPort1" runat="server">
    <Body>
    <form id="form1" runat="server" style="background-color:Transparent">
    <div>
    <ext:FormPanel ID="FormPanel1" runat="server" Border="false" Height="250" Width="360"  AutoScroll="true"  ButtonAlign="Right" StyleSpec="background-color:transparent" BodyStyle="background-color:transparent" >
     <Body>
       <ext:Panel ID="Panel1" runat="server" Border="false" AutoHeight="true" AutoWidth="true"
                    StyleSpec="background-color:transparent" BodyStyle="background-color:transparent">
                    <TopBar>
                        <ext:Toolbar ID="Toolbar1" runat="server">
                            <Items>
                                <ext:ToolbarSeparator ID="ToolbarSeparator1" runat="server">
                                </ext:ToolbarSeparator>
                                <ext:Button ID="Button1" runat="server" Text="保存" Icon="Disk">
                                    <AjaxEvents>
                                        <Click OnEvent="Buttonsave_Click">
                                        </Click>
                                    </AjaxEvents>
                                </ext:Button>
                                <ext:Button ID="btnCancle" runat="server" Text="返回" Icon="ArrowUndo">
                                    <AjaxEvents>
                                        <Click OnEvent="btnCancle_Click">
                                        </Click>
                                    </AjaxEvents>
                                </ext:Button>
                            </Items>
                        </ext:Toolbar>
                    </TopBar>
                </ext:Panel>
        <ext:FormLayout ID="FormLayout1" runat="server">
         <ext:Anchor>
           <ext:ComboBox ID="ComShowflag" runat="server" AllowBlank="false" Width="180" 
                                            FieldLabel="门诊住院" SelectedIndex="0">
                                            <Items>
                                                <ext:ListItem Text="住院" Value="0" />
                                                <ext:ListItem Text="门诊" Value="1" />
                                            </Items>
                                           
                                        </ext:ComboBox>
                                         </ext:Anchor>
            <ext:Anchor>
                <ext:TextField ID="patient_id" runat="server" DataIndex="centerame" MsgTarget="Side"
                    AllowBlank="false" FieldLabel="病人id" Width="180" />
            </ext:Anchor>
             <ext:Anchor>
                <ext:NumberField ID="TextField1" runat="server" DataIndex="centerame" MsgTarget="Side"
                   FieldLabel="住院标识" Width="180" />
            </ext:Anchor>
             <ext:Anchor>
                        <ext:DateField runat="server" ID="dfInputDate" CausesValidation="true" FieldLabel="起始时间" 
                            AllowBlank="false" Format="yyyy-MM-dd" MaxLength="10" EnableKeyEvents="true" Width="180">
                           
                        </ext:DateField>
                    </ext:Anchor>
                    <ext:Anchor Horizontal="95%">
                        <ext:ComboBox ID="ccbtype" runat="server" StoreID="Store2" DisplayField="DIAGNOSIS_NAME"
                            ValueField="DIAGNOSIS_CODE" Width="180" FieldLabel="病种名称" CausesValidation="true"
                            AllowBlank="false">
                        </ext:ComboBox>
                    </ext:Anchor>
        </ext:FormLayout>
    </Body>
   
    </ext:FormPanel>
    </div>
    </form>
    </body>
    </ext:ViewPort>
</body>
</html>

