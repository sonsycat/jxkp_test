<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="cost_item_power.aspx.cs"
    Inherits="GoldNet.JXKP.cbhs.cbhsdict.cost_item_power" %>

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
     <link rel="stylesheet" type="text/css" href="../../Bonus/Orthers/Cbouns.css" />
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
            position: static;
        }
    </style>
</head>
<body>
    <ext:ScriptManager ID="ScriptManager1" runat="server" />
    <ext:Store ID="Store1" runat="server">
        <Reader>
            <ext:JsonReader ReaderID="USER_ID">
                <Fields>
                    <ext:RecordField Name="ITEM_CODE" />
                    <ext:RecordField Name="USER_ID" />
                    <ext:RecordField Name="USER_NAME" />
                </Fields>
            </ext:JsonReader>
        </Reader>
    </ext:Store>
    <ext:Store ID="Store2" runat="server">
    </ext:Store>
    <form id="form1" runat="server">
    <div>
        <ext:FormPanel ID="FormPanel1" runat="server" Border="false" MonitorValid="true" AutoScroll="false"
            ButtonAlign="Right" BodyStyle="background-color:transparent;">
            <Body>
                <table border="0">
                    <tr>
                        <td>
                            <ext:ComboBox ID="USER" runat="server" StoreID="Store2" DisplayField="STAFF_NAME"
                                AllowBlank="false" ValueField="STAFF_ID" TypeAhead="false" LoadingText="Searching..."
                                Width="280" PageSize="10" ItemSelector="div.search-item" MinChars="1">
                                <Template ID="Template4" runat="server">
                                            <tpl for=".">
                                                <div class="search-item">
                                                    <h3>{STAFF_ID}&nbsp&nbsp&nbsp{STAFF_NAME}</h3>
                                                </div>
                                            </tpl>
                                </Template>
                            </ext:ComboBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <ext:GridPanel ID="GridPanel1" runat="server" StoreID="Store1" StripeRows="true"
                                Border="false" TrackMouseOver="true" Height="272">
                                <ColumnModel ID="ColumnModel1" runat="server">
                                    <Columns>
                                        <ext:Column ColumnID="USER_ID" Header="人员ID" Width="110" Align="Center" Sortable="true"
                                            DataIndex="USER_ID" MenuDisabled="true" />
                                        <ext:Column ColumnID="USER_NAME" Header="姓名" Width="110" Align="Center" Sortable="true"
                                            DataIndex="USER_NAME" MenuDisabled="true" />
                                    </Columns>
                                </ColumnModel>
                                <SelectionModel>
                                    <ext:RowSelectionModel ID="RowSelectionModel1" runat="server" SingleSelect="true">
                                        <Listeners>
                                            <RowSelect Handler="#{Button_del}.enable()" />
                                            <RowDeselect Handler="if (!#{GridPanel1}.hasSelection()) {#{Button_del}.disable()}" />
                                        </Listeners>
                                    </ext:RowSelectionModel>
                                </SelectionModel>
                                <BottomBar>
                                    <ext:Toolbar ID="Toolbar1" runat="server" Visible="true" AutoWidth="true">
                                        <Items>
                                            <ext:ToolbarFill runat="server" ID="toolbarfill1"></ext:ToolbarFill>
                                            <ext:Button ID="save" runat="server" Text="添加" Icon="Add">
                                                <AjaxEvents>
                                                    <Click OnEvent="Button_add_click" Before="if( #{FormPanel1}.getForm().isValid()) {return true;}else{Ext.Msg.alert('提示','人员不能为空');  return false;}">
                                                        <EventMask Msg="保存中..." ShowMask="true" />
                                                    </Click>
                                                </AjaxEvents>
                                            </ext:Button>
                                             <ext:Button ID="Button_del" runat="server" Text="移除" Icon="Delete" Disabled="true">
                                                <AjaxEvents>
                                                    <Click OnEvent="Button_del_click">
                                                        <EventMask Msg="载入中..." ShowMask="true" />
                                                    </Click>
                                                </AjaxEvents>
                                            </ext:Button>
                                            <ext:Button ID="cancel" runat="server" Text="关闭" Icon="Accept">
                                                <AjaxEvents>
                                                    <Click OnEvent="Button_accept_click">
                                                        <EventMask Msg="跳转中..." ShowMask="true" />
                                                    </Click>
                                                </AjaxEvents>
                                            </ext:Button>
                                        </Items>
                                    </ext:Toolbar>
                                </BottomBar>
                            </ext:GridPanel>
                        </td>
                    </tr>
                </table>
            </Body>          
        </ext:FormPanel>
    </div>
    </form>
</body>
</html>
