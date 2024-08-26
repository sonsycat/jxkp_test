<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Sys_Menu_Item.aspx.cs" Inherits="GoldNet.JXKP.WebPage.SysManager.Sys_Menu_Item" %>
<%@ Register Assembly="Goldnet.Ext.Web" Namespace="Goldnet.Ext.Web" TagPrefix="ext" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
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
    <link rel="stylesheet" type="text/css" href="../../Bonus/Orthers/Cbouns.css" />

    <script language="javascript" type="text/javascript">
        var RefreshData = function(msg) {
            Ext.Msg.show({ title: '提示', msg: msg, icon: 'ext-mb-info', buttons: { ok: true} });
            Store1.reload();
        }
    </script>

</head>
<body>
    <form id="form1" runat="server">
    <div>
        <ext:ScriptManager ID="ScriptManager1" runat="server" />
        <ext:Store ID="Store1" runat="server" OnRefreshData="Store_RefreshData">
            <Reader>
                <ext:JsonReader ReaderID="ITEM_ID">
                    <Fields>
                        <ext:RecordField Name="ITEM_ID" Type="String" Mapping="ITEM_ID" />
                        <ext:RecordField Name="ITEM_NAME" Type="String" Mapping="ITEM_NAME" />
                        <ext:RecordField Name="ITEM_VALUE" Type="String" Mapping="ITEM_VALUE" />

                    </Fields>
                </ext:JsonReader>
            </Reader>
        </ext:Store>
       
        <ext:ViewPort ID="ViewPort1" runat="server">
            <Body>
                <ext:ColumnLayout ID="ColumnLayout1" runat="server" Split="true" FitHeight="true">
                    <Columns>
                        <ext:LayoutColumn ColumnWidth="1">
                            <ext:GridPanel ID="GridPanel1" runat="server" StoreID="Store1" StripeRows="true"
                                TrackMouseOver="true" AutoWidth="true" Height="480" Border="false" AutoScroll="true"
                                ClicksToEdit="1">
                                <TopBar>
                                    <ext:Toolbar ID="Toolbar1" runat="server" Visible="true" AutoWidth="true">
                                        <Items>
                                           
                                            <ext:Button ID="Button1" runat="server" Text="保存" Icon="Disk">
                                                <AjaxEvents>
                                                    <Click OnEvent="Button_Save_click">
                                                        <EventMask Msg="保存中..." ShowMask="true" />
                                                        <ExtraParams>
                                                            <ext:Parameter Name="Values" Value="Ext.encode(#{GridPanel1}.getRowsValues(false))"
                                                                Mode="Raw">
                                                            </ext:Parameter>
                                                        </ExtraParams>
                                                    </Click>
                                                </AjaxEvents>
                                            </ext:Button>
                                           
                                        </Items>
                                    </ext:Toolbar>
                                </TopBar>
                                <ColumnModel ID="ColumnModel1" runat="server">
                                    <Columns>
                                    <ext:Column ColumnID="ITEM_ID" Header="<div style='text-align:center;'>编号</div>"
                                            Width="100" Align="Center" Sortable="false" DataIndex="ITEM_ID" MenuDisabled="true"/>
                                        <ext:Column ColumnID="ITEM_NAME" Header="<div style='text-align:center;'>选项名称</div>"
                                            Width="100" Align="Center" Sortable="true" DataIndex="ITEM_NAME" MenuDisabled="true">
                                            <Editor>
                                                <ext:TextField ID="itemname" runat="server" SelectOnFocus="true"></ext:TextField>
                                            </Editor>
                                        </ext:Column>
                                        
                                        <ext:Column ColumnID="ITEM_VALUE" Header="选项值" Width="60" Align="right"
                                            Sortable="false" DataIndex="ITEM_VALUE" MenuDisabled="true">
                                            <Editor>
                                                <ext:NumberField ID="nfpercent" runat="server" SelectOnFocus="true" DecimalPrecision="2"
                                                    MaxValue="100" MinValue="0">
                                                </ext:NumberField>
                                            </Editor>
                                        </ext:Column>
                                       
                                      
                                    </Columns>
                                </ColumnModel>
                             
                                <SelectionModel>
                                    <ext:RowSelectionModel ID="RowSelectionModel1" runat="server" SingleSelect="true">
                                      
                                    </ext:RowSelectionModel>
                                </SelectionModel>
                               
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