<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DeptAdd.aspx.cs" Inherits="GoldNet.JXKP.WebPage.SysManager.DeptAdd" %>
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
    <form id="form1" runat="server">
    <div onpaste="return false">
        <ext:FormPanel ID="FormPanel1" runat="server" Border="false" AutoHeight="true" AutoWidth="true"
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
                                <ext:Button ID="save" runat="server" Text="保存" Icon="Disk">
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
                <ext:Panel ID="Panel2" runat="server" Border="false" AutoHeight="true" AutoWidth="true"
                    AutoScroll="true" StyleSpec="background-color:transparent" BodyStyle="background-color:transparent">
                    <Body>
                        <ext:FormLayout ID="FormLayout2" runat="server">
                            <ext:Anchor>
                                <ext:TextField ID="TextDeptcode" runat="server" DataIndex="dept_code" MsgTarget="Side"
                                    AllowBlank="false" FieldLabel="科室代码" Width="220"  />
                            </ext:Anchor>
                            <ext:Anchor>
                                <ext:TextField ID="TextDeptname" runat="server" DataIndex="dept_name" MsgTarget="Side"
                                    AllowBlank="false" FieldLabel="科室名称" Width="220"  />
                            </ext:Anchor>
                            <ext:Anchor>
                                <ext:ComboBox ID="Combo_DeptType" runat="server"  AllowBlank="false" Width="220" EmptyText="请选择科室类别"
                                    FieldLabel="科室类别" Visible="false">
                                    <AjaxEvents>
                                        <Select OnEvent="SelectedDepttype">
                                            <EventMask ShowMask="true" />
                                        </Select>
                                    </AjaxEvents>
                                </ext:ComboBox>
                            </ext:Anchor>
                            <ext:Anchor>
                                <ext:ComboBox ID="ComPdeptcode" runat="server" StoreID="Store2" DisplayField="DEPT_NAME"
                                    ValueField="DEPT_CODE" TypeAhead="false" LoadingText="Searching..." Width="220"
                                    PageSize="10" HideTrigger="false" ItemSelector="div.search-item" MinChars="1"
                                    FieldLabel="上级科室" ListWidth="300" Visible="false">
                                    <Template ID="Template1" runat="server">
                   <tpl for=".">
                      <div class="search-item">
                         <h3><span>{DEPT_NAME}</span>{DEPT_CODE}</h3>
                  
                      </div>
                   </tpl>
                                    </Template>
                                </ext:ComboBox>
                            </ext:Anchor>
                            <ext:Anchor>
                                <ext:ComboBox ID="ComAccountdeptcode" runat="server" StoreID="Store2" DisplayField="DEPT_NAME"
                                    ValueField="DEPT_CODE" TypeAhead="false" LoadingText="Searching..." Width="220"
                                    AllowBlank="false" PageSize="10" HideTrigger="false" ItemSelector="div.search-item"
                                    MinChars="1" FieldLabel="核算科室" ListWidth="300" Visible="false">
                                    <Template ID="Template2" runat="server">
                   <tpl for=".">
                      <div class="search-item">
                         <h3><span>{DEPT_CODE}</span>{DEPT_NAME}</h3>
                  
                      </div>
                   </tpl>
                                    </Template>
                                </ext:ComboBox>
                            </ext:Anchor>
                            <ext:Anchor>
                                <ext:ComboBox ID="ComDeptcodesecond" runat="server" StoreID="Store2" DisplayField="DEPT_NAME"
                                    ValueField="DEPT_CODE" TypeAhead="false" LoadingText="Searching..." Width="220"
                                    PageSize="10" HideTrigger="false" ItemSelector="div.search-item" MinChars="1"
                                    FieldLabel="二级科室" ListWidth="300" Visible="false">
                                    <Template ID="Template3" runat="server">
                   <tpl for=".">
                      <div class="search-item">
                         <h3><span>{DEPT_CODE}</span>{DEPT_NAME}</h3>
                  
                      </div>
                   </tpl>
                                    </Template>
                                </ext:ComboBox>
                            </ext:Anchor>
                            <ext:Anchor>
                                <ext:ComboBox ID="ComLcattr" runat="server"  Width="220" EmptyText="请选择临床属性"
                                    FieldLabel="临床属性" Visible="false">
                                </ext:ComboBox>
                            </ext:Anchor>
                            <ext:Anchor>
                                <ext:ComboBox ID="ComIsaccount" runat="server" AllowBlank="false" Width="220" EmptyText="选择是否核算"
                                    FieldLabel="是否核算" Visible="false">
                                    <Items>
                                        <ext:ListItem Text="是" Value="1" />
                                        <ext:ListItem Text="不是" Value="0" />
                                    </Items>
                                </ext:ComboBox>
                            </ext:Anchor>
                            <ext:Anchor>
                                <ext:NumberField ID="NumSortid" runat="server" DataIndex="SORT_NO" MsgTarget="Side"
                                    AllowBlank="true" FieldLabel="排列顺序" Width="220" Visible="false"/>
                            </ext:Anchor>
                            <ext:Anchor>
                                <ext:ComboBox ID="ComShowflag" runat="server" AllowBlank="false" Width="220" EmptyText="选择是否停用"
                                    FieldLabel="是否停用" SelectedIndex="0" Visible="false">
                                    <Items>
                                        <ext:ListItem Text="启用" Value="0" />
                                        <ext:ListItem Text="停用" Value="1" />
                                    </Items>
                                </ext:ComboBox>
                            </ext:Anchor>
                        </ext:FormLayout>
                    </Body>
                </ext:Panel>
            </Body>
        </ext:FormPanel>
    </div>
    </form>
</body>
</html>
