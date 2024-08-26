<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Persons_change_edit.aspx.cs" Inherits="GoldNet.JXKP.WebPage.SysManager.Persons_change_edit" %>
<%@ Register Assembly="Goldnet.Ext.Web" Namespace="Goldnet.Ext.Web" TagPrefix="ext" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title></title>
 <link rel="stylesheet" type="text/css" href="/resources/css/main.css" />
    <style type="text/css">
        body
        {
            background-color: #DFE8F6;
            font-size: 12px;
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
   
    <ext:Store ID="Store2" runat="server" AutoLoad="false">
        <Proxy>
        </Proxy>
        <Reader>
        </Reader>
    </ext:Store>
    <ext:ViewPort ID="ViewPort1" runat="server">
        <Body>
            <form id="form1" runat="server" style="background-color: Transparent">
            <div>
                <ext:FormPanel ID="FormPanel1" runat="server" Border="false" Height="250" Width="360"
                    AutoScroll="true" ButtonAlign="Right" StyleSpec="background-color:transparent"
                    BodyStyle="background-color:transparent">
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
                        <ext:Label ID="deptcode" runat="server" Hidden="true"></ext:Label>
                        <ext:FormLayout ID="FormLayout1" runat="server">
                            <ext:Anchor>
        
                                <ext:ComboBox ID="Combo_Person" runat="server"   Width="180" EmptyText="请选择科室类别"
                                    FieldLabel="人员姓名" ReadOnly="true">
                                </ext:ComboBox>
                            </ext:Anchor>
                            <ext:Anchor>
                                <ext:ComboBox ID="ComAccountdeptcode" runat="server" StoreID="Store2" DisplayField="DEPT_NAME"
                                    ValueField="DEPT_CODE" TypeAhead="false" LoadingText="Searching..." Width="180"
                                    PageSize="10" HideTrigger="false" ItemSelector="div.search-item" MinChars="1"
                                    ListWidth="300" EmptyText="选择科室" FieldLabel="科室名称">
                                    <Template ID="Template2" runat="server">
                                                   <tpl for=".">
                                                      <div class="search-item">
                                                         <h3><span>
                                                          {DEPT_CODE}</span>{DEPT_NAME}</h3>
                                                      </div>
                                                   </tpl>
                                    </Template>
                                </ext:ComboBox>
                            </ext:Anchor>
                            
                           <ext:Anchor>
                           <ext:DateField ID="st_date" runat="server" FieldLabel="开始时间"   Width="180" EnableKeyEvents="true" />
                          
                           
                           </ext:Anchor>
                           <ext:Anchor>
                           <ext:DateField ID="END_Date" runat="server" FieldLabel="结束时间"   Width="180" EnableKeyEvents="true" />
                          
                           </ext:Anchor>
                           
                        </ext:FormLayout>
                        <ext:KeyNav ID="KeyNav1" runat="server" Target="ST_Date" >
                                            	<Enter Handler="var str = document.getElementById('st_date').value ; var   reg=/^(\d{4})(\d{2})(\d{2})$/; document.getElementById('st_date').value   =   str.replace(reg, '$1-$2-$3');" />
                                            </ext:KeyNav>
                         <ext:KeyNav ID="KeyNav2" runat="server" Target="END_Date" >
                                            	<Enter Handler="var str = document.getElementById('END_Date').value ; var   reg=/^(\d{4})(\d{2})(\d{2})$/; document.getElementById('END_Date').value   =   str.replace(reg, '$1-$2-$3');" />
                                            </ext:KeyNav>
                    </Body>
                </ext:FormPanel>
            </div>
            </form>
        </Body>
    </ext:ViewPort>
</body>
</html>
