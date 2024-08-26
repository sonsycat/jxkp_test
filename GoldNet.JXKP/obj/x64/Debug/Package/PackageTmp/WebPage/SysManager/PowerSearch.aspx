<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PowerSearch.aspx.cs" Inherits="GoldNet.JXKP.WebPage.SysManager.PowerSearch" %>

<%@ Register Assembly="Goldnet.Ext.Web" Namespace="Goldnet.Ext.Web" TagPrefix="ext" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>无标题页</title>
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
        <ext:ScriptManager ID="ScriptManager1" runat="server" AjaxMethodNamespace="CompanyX" />
        <ext:Store ID="Store1" runat="server">
            <Reader>
                <ext:JsonReader ReaderID="FUNCTION_NAME">
                    <Fields>
                        <ext:RecordField Name="ROLE_NAME" />
                        <ext:RecordField Name="FUNCTION_NAME" />
                    </Fields>
                </ext:JsonReader>
            </Reader>
        </ext:Store>
        <ext:Store ID="Store2" runat="server" AutoLoad="false">
           
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
        <ext:Toolbar ID="Toolbar_ZLJK" runat="server" Visible="true" AutoWidth="true">
            <Items>
                <ext:Label ID="func" runat="server" Text="选择人员：" Width="40">
                </ext:Label>
                <ext:ComboBox ID="ComboBox1" runat="server" StoreID="Store2" DisplayField="USER_NAME"
                    ValueField="USER_ID" TypeAhead="false" LoadingText="Searching..." Width="200"
                    PageSize="10" HideTrigger="false" ItemSelector="div.search-item" MinChars="1">
                    <Template ID="Template1" runat="server">
                   <tpl for=".">
                      <div class="search-item">
                         <h3><span>{DB_USER}</span>{USER_NAME}</h3>
                  
                      </div>
                   </tpl>
                    </Template>
                </ext:ComboBox>
                <ext:Button ID="Button1" runat="server" Text="查询" Icon="Zoom">
                    <AjaxEvents>
                        <Click OnEvent="SelectPower">
                            <EventMask Msg="载入中..." ShowMask="true" />
                        </Click>
                    </AjaxEvents>
                </ext:Button>
                <ext:ToolbarSeparator ID="ToolbarSeparator4" runat="server" />
            </Items>
        </ext:Toolbar>
             <ext:ViewPort ID="ViewPort1" runat="server">
            <Body>
                <ext:ColumnLayout ID="ColumnLayout1" runat="server" Split="true" FitHeight="true">
                    <Columns>
                        <ext:LayoutColumn ColumnWidth="1">
        <ext:GridPanel ID="GridPanel1" runat="server" StoreID="Store1" StripeRows="true"
            TrackMouseOver="true" Height="480">
            <ColumnModel ID="ColumnModel1" runat="server">
                <Columns>
                    <ext:Column Header="角色名称" Width="120" Align="left" Sortable="true" MenuDisabled="true"
                        ColumnID="ROLE_NAME" DataIndex="ROLE_NAME">
                    </ext:Column>
                    <ext:Column Header="功能" Width="200" Align="left" Sortable="true" MenuDisabled="true"
                        ColumnID="FUNCTION_NAME" DataIndex="FUNCTION_NAME">
                    </ext:Column>
                </Columns>
            </ColumnModel>
            <SelectionModel>
                <ext:RowSelectionModel ID="rowselection" SingleSelect="true">
                </ext:RowSelectionModel>
            </SelectionModel>
            <LoadMask ShowMask="true" />
        </ext:GridPanel>
        </ext:LayoutColumn>
                    </Columns>
                </ext:ColumnLayout>
            </Body>
        </ext:ViewPort>
    </div>
    </form>
</body>
</html>
