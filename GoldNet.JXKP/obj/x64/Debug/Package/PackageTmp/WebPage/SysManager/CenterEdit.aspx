<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CenterEdit.aspx.cs" Inherits="GoldNet.JXKP.WebPage.SysManager.CenterEdit" %>
<%@ Register Assembly="Goldnet.Ext.Web" Namespace="Goldnet.Ext.Web" TagPrefix="ext" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
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
     <ext:Store ID="Store1" runat="server" AutoLoad="false">
           
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
                <ext:TextField ID="Text_CenterName" runat="server" DataIndex="centerame" MsgTarget="Side"
                    AllowBlank="false" FieldLabel="专科中心名称" Width="180" />
            </ext:Anchor>
            <ext:Anchor>
                <ext:ComboBox ID="Com_Director" runat="server" StoreID="Store1" DisplayField="USER_NAME"
                    ValueField="USER_ID" TypeAhead="false" LoadingText="Searching..." Width="180"
                    PageSize="10" HideTrigger="false" ItemSelector="div.search-item" MinChars="1" FieldLabel="负责人" ListWidth="220">
                    <Template ID="Template1" runat="server">
                   <tpl for=".">
                      <div class="search-item">
                         <h3><span>{DB_USER}</span>{USER_NAME}</h3>
                  
                      </div>
                   </tpl>
                    </Template>
                </ext:ComboBox>
                    
            </ext:Anchor>
             <ext:Anchor>
                <ext:ComboBox ID="Combo_Role" runat="server" AllowBlank="false" Width="180" EmptyText="请选择角色"
                    FieldLabel="对应角色">
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
