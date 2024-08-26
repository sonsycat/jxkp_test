<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="xyhs_dept_set.aspx.cs"
    Inherits="GoldNet.JXKP.cbhs.xyhs.xyhs_dept_set" %>

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
    <ext:Store ID="Store2" runat="server" AutoLoad="true">
        <Proxy>
        </Proxy>
        <Reader>
        </Reader>
    </ext:Store>
    <form id="form1" runat="server">
    <div>
        <ext:FormPanel ID="FormPanel1" runat="server" Border="false" MonitorValid="true"
            ButtonAlign="Right" BodyStyle="background-color:transparent;">
            <Body>
                <ext:FormLayout ID="FormLayout1" runat="server" LabelWidth="80" StyleSpec="margin:10px">
                    <ext:Anchor>
                        <ext:TextField ID="DEPT_CODE" runat="server" MsgTarget="Side" Width="220" FieldLabel="科室编码"  />
                    </ext:Anchor>
                    <ext:Anchor>
                        <ext:TextField ID="DEPT_NAME" runat="server" MsgTarget="Side" Width="220" FieldLabel="科室名称"  />
                    </ext:Anchor>
                    <ext:Anchor>
                        <ext:ComboBox ID="DEPT_TYPE" runat="server" EmptyText="请选科室类别..." AllowBlank="false"
                            FieldLabel="科室类别" Width="220">
                        </ext:ComboBox>
                    </ext:Anchor>
                    <ext:Anchor>
                        <ext:ComboBox ID="ACCOUNT_DEPT" runat="server" StoreID="Store2" DisplayField="DEPT_NAME" FieldLabel="中心科室"
                                    ValueField="DEPT_CODE" TypeAhead="false" LoadingText="Searching..." Width="220"
                                    AllowBlank="false" PageSize="10" ItemSelector="div.search-item" MinChars="1">
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
                        <ext:ComboBox ID="ATTR" runat="server" EmptyText="请选择..." AllowBlank="false" Width="220"
                            FieldLabel="是否核算">
                            <Items>
                                <ext:ListItem Text="是" Value="是" />
                                <ext:ListItem Text="不是" Value="不是" />
                            </Items>
                        </ext:ComboBox>
                    </ext:Anchor>
                    <ext:Anchor>
                        <ext:TextField ID="SORT_NO" runat="server" MsgTarget="Side" Width="220" AllowBlank="true" FieldLabel="排列序号" />
                    </ext:Anchor>
                    <ext:Anchor>
                        <ext:TextField ID="INPUT_CODE" runat="server" MsgTarget="Side" Width="220" FieldLabel="输入码" AllowBlank="false"/>
                    </ext:Anchor>
                    <ext:Anchor>
                        <ext:ComboBox ID="SHOW_FLAG" runat="server" EmptyText="请选择..." AllowBlank="false" 
                            FieldLabel="是否停用" Width="220">
                            <Items>
                                <ext:ListItem Text="启用" Value="0" />
                                <ext:ListItem Text="停用" Value="1" />
                            </Items>
                        </ext:ComboBox>
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
                <ext:Button ID="cancel" runat="server" Text="返回" Icon="Cancel">
                    <Listeners>
                        <Click Handler="parent.DeptSetWin.hide();" />
                    </Listeners>
                </ext:Button>
            </Buttons>
        </ext:FormPanel>
    </div>
    </form>
</body>
</html>
