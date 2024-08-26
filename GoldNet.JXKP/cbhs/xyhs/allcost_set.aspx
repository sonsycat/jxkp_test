<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="allcost_set.aspx.cs" Inherits="GoldNet.JXKP.allcost_set" %>

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
    <link rel="stylesheet" type="text/css" href="../../Bonus/Orthers/Cbouns.css" />

    <script type="text/javascript">
        var RefreshData = function(msg) {
            Ext.Msg.show({ title: '提示', msg: msg, icon: 'ext-mb-info', buttons: { ok: true} });
            Store1.reload();
        }
    </script>

</head>
<body>
    <form id="form1" runat="server">
    <ext:ScriptManager ID="ScriptManager1" runat="server" />
    <ext:Store ID="Store1" runat="server" AutoLoad="true">
        <Reader>
            <ext:JsonReader ReaderID="PROG_ITEM">
                <Fields>
                    <ext:RecordField Name="ITEM_CODE" />
                    <ext:RecordField Name="ITEM_NAME" />
                    <ext:RecordField Name="ID" />
                    <ext:RecordField Name="XYHS_DEPT_TYPE" />
                    <ext:RecordField Name="PROG_CODE" />
                    <ext:RecordField Name="PROG_NAME" />
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
                            TrackMouseOver="true" AutoWidth="true" Height="480" Border="false">
                            <TopBar>
                                <ext:Toolbar ID="Toolbar1" runat="server" Visible="true" AutoWidth="true">
                                    <Items>
                                        <ext:Button ID="Btn_save" Text="保存" Icon="Add" runat="server">
                                            <AjaxEvents>
                                                <Click OnEvent="Btn_Add_Click">
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
                                    <ext:Column ColumnID="ID" Header="ID" Width="100" Align="left" Sortable="true" DataIndex="ID"
                                        Hidden="true" />
                                    <ext:Column ColumnID="XYHS_DEPT_TYPE" Header="四级分摊类别" Width="100" Align="left" Sortable="false"
                                        DataIndex="XYHS_DEPT_TYPE" MenuDisabled="true" />
                                    <ext:Column ColumnID="PROG_NAME" Header="分摊方案" Width="150" Align="left" Sortable="false"
                                        DataIndex="PROG_NAME" MenuDisabled="true">
                                        <Editor>
                                            <ext:ComboBox ID="proglist" runat="server">
                                            </ext:ComboBox>
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
    </form>
</body>
</html>
