<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Income_Difference.aspx.cs"
    Inherits="GoldNet.JXKP.Income_Difference" %>

<%@ Register Assembly="Goldnet.Ext.Web" Namespace="Goldnet.Ext.Web" TagPrefix="ext" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style type="text/css">
        .label
        {
            width: 300px;
            height: 15px;
            text-align: center;
            padding: 5px 0;
            border: 1px dotted #99bbe8;
            background: #dfe8f6;
            color: #15428b;
            cursor: default;
            margin: 20px,0,0,20px;
            margin-left: 0px;
            font: bold 11px tahoma,arial,sans-serif;
        }
    </style>

    <script type="text/javascript">
        function checkSelect(check) {
            var barStatus = '';
            var rcount = Store1.getTotalCount();
            for (i = 0; i < FormPanel1.items.items.length; i++) {
                if (FormPanel1.items.items[i].checked) {
                    for (var j = 0; j < rcount; j++) {
                        var record = Store1.getAt(j);
                        var columnvalue = record.get('DIFFTYPE');
                        if (columnvalue == FormPanel1.items.items[i].fieldLabel) {
                            var columnid = record.get('DIFFTYPETABLENAME');
                            barStatus += columnid;
                        }
                    }
                }
            }
        }
    </script>

</head>
<body>
    <ext:ScriptManager ID="ScriptManager1" runat="server" />
    <form id="form1" runat="server">
    <div>
        <ext:Store ID="Store1" AutoLoad="true" runat="server">
            <Reader>
                <ext:JsonReader>
                    <Fields>
                        <ext:RecordField Name="ID">
                        </ext:RecordField>
                        <ext:RecordField Name="DIFFTYPE">
                        </ext:RecordField>
                        <ext:RecordField Name="DIFFTYPETABLENAME">
                        </ext:RecordField>
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
                                    <ext:Toolbar ID="Toolbar_fjsr" runat="server" Visible="true" AutoWidth="true">
                                        <Items>
                                            <ext:ComboBox ID="years" runat="server" Width="60" AllowBlank="true" EmptyText="请选择年..."
                                                FieldLabel="年">
                                            </ext:ComboBox>
                                            <ext:ToolbarTextItem ID="dd1Name" runat="server" Text="年 " />
                                            <ext:ComboBox ID="months" runat="server" Width="60" AllowBlank="true" EmptyText="请选择月..."
                                                FieldLabel="月">
                                                <Items>
                                                    <ext:ListItem Text="01" Value="01" />
                                                    <ext:ListItem Text="02" Value="02" />
                                                    <ext:ListItem Text="03" Value="03" />
                                                    <ext:ListItem Text="04" Value="04" />
                                                    <ext:ListItem Text="05" Value="05" />
                                                    <ext:ListItem Text="06" Value="06" />
                                                    <ext:ListItem Text="07" Value="07" />
                                                    <ext:ListItem Text="08" Value="08" />
                                                    <ext:ListItem Text="09" Value="09" />
                                                    <ext:ListItem Text="10" Value="10" />
                                                    <ext:ListItem Text="11" Value="11" />
                                                    <ext:ListItem Text="12" Value="12" />
                                                </Items>
                                            </ext:ComboBox>
                                            <ext:ToolbarTextItem ID="ToolbarTextItem1" runat="server" Text="月 " />
                                            <ext:ToolbarSeparator ID="ToolbarSeparator1" runat="server" />
                                            <ext:Button ID="Button1" runat="server" Text="对比" Icon="ArrowRight">
                                                <AjaxEvents>
                                                    <Click OnEvent="Btn_Next_Click" Timeout="120000" >
                                                        <Confirmation ConfirmRequest="true" Message="差异对比数据量较大,这将耗时2分钟左右,是否继续?" Title="系统提示" />
                                                        <EventMask Msg='正在对比数据差异...' ShowMask="true" />
                                                       <ExtraParams>
                                                            <ext:Parameter Name="Values" Value="Ext.encode(#{GridPanel1}.getRowsValues())" Mode="Raw">
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
                                        <ext:Column ColumnID="ID" Hidden="true" />
                                        <ext:Column ColumnID="DIFFTYPE" Header="<div style='text-align:center;'>类别</div>" Width="90" Align="left" Sortable="true"
                                            DataIndex="DIFFTYPE" MenuDisabled="true" />
                                        <ext:Column ColumnID="DIFFTYPETABLENAME" Header="<div style='text-align:center;'>表名</div>" Width="150" Align="left" Sortable="true"
                                            DataIndex="DIFFTYPETABLENAME" MenuDisabled="true" />
                                    </Columns>
                                </ColumnModel>
                                <SelectionModel>
                                    <ext:CheckboxSelectionModel ID="RowSelectionModel1" runat="server">
                                    </ext:CheckboxSelectionModel>
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
