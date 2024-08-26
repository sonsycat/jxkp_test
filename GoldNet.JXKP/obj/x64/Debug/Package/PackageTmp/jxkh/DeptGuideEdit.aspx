<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DeptGuideEdit.aspx.cs"
    Inherits="GoldNet.JXKP.jxkh.DeptGuideEdit" %>

<%@ Register Assembly="Goldnet.Ext.Web" Namespace="Goldnet.Ext.Web" TagPrefix="ext" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style type="text/css">
        .search-item
        {
            font: normal 11px tahoma, arial, helvetica, sans-serif;
            padding: 3px 10px 3px 10px;
            border: 1px solid #fff;
            border-bottom: 1px solid #eeeeee;
            white-space: normal;
            color: #555;
            width: 200px;
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
            width: 140px;
            display: block;
            clear: none;
        }
    </style>
    <style type="text/css">
        body
        {
            background-color: #DFE8F6;
            font-size: 12px;
        }
    </style>
</head>
<body>
    <ext:ScriptManager ID="ScriptManager1" runat="server">
    </ext:ScriptManager>
    <ext:Store ID="Store1" runat="server" AutoLoad="true">
        <Reader>
            <ext:JsonReader Root="Staffdepts">
                <Fields>
                    <ext:RecordField Name="GUIDE_CODE" />
                    <ext:RecordField Name="GUIDE_NAME" />
                    <ext:RecordField Name="INPUT_CODE" />
                </Fields>
            </ext:JsonReader>
        </Reader>
    </ext:Store>
    <form id="form1" runat="server">
    <div>
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
                <ext:FormLayout ID="FormLayout1" runat="server">
                    <ext:Anchor>
                        <ext:ComboBox ID="guide" runat="server" AllowBlank="true" Width="140" EmptyText="请选择指标"
                            FieldLabel="科室指标">
                        </ext:ComboBox>
                    </ext:Anchor>
                    <ext:Anchor>
                        <ext:ComboBox runat="server" ID="Comboguide" Width="140" ListWidth="240" StoreID="Store1"
                            DisplayField="GUIDE_NAME" ValueField="GUIDE_CODE" TypeAhead="false" LoadingText="Searching..."
                            PageSize="1000" ItemSelector="div.search-item" MinChars="1" FieldLabel="关联指标">
                            <Template ID="Template2" runat="server">
                                       <tpl for=".">
                                          <div class="search-item">
                                             <h3><span style="width:auto">{GUIDE_CODE}</span>{GUIDE_NAME}</h3>
                                          </div>
                                       </tpl>
                            </Template>
                        </ext:ComboBox>
                    </ext:Anchor>
                    <ext:Anchor>
                        <ext:NumberField ID="guidecause" runat="server" DataIndex="rolename" MsgTarget="Side"
                            AllowBlank="false" FieldLabel="达标值" Width="140" />
                    </ext:Anchor>
                </ext:FormLayout>
            </Body>
        </ext:FormPanel>
    </div>
    </form>
</body>
</html>
