<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="KSHSXX_YLZC.aspx.cs" Inherits="GoldNet.JXKP.cbhs.Report.KSHSXX_YLZC" %>

<%@ Register Assembly="Goldnet.Ext.Web" Namespace="Goldnet.Ext.Web" TagPrefix="ext" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>

    <script type="text/javascript">
     
        //
        var RefreshData = function() {
            Store1.reload();
        }
        
        //数字格式化处理
        var rmbMoney = function(v) {
            v = (Math.round((v - 0) * 100)) / 100;
            v = (v == Math.floor(v)) ? v + ".00" : ((v * 10 == Math.floor(v * 10)) ? v + "0" : v);
            v = String(v);
            var ps = v.split('.');
            var whole = ps[0];
            var sub = ps[1] ? '.' + ps[1] : '.00';
            var r = /(\d+)(\d{3})/;
            while (r.test(whole)) {
                whole = whole.replace(r, '$1' + ',' + '$2');
            }
            v = whole + sub;
            if (v.charAt(0) == '-') {
                return '-' + v.substr(1);
            }
            return v;
        }
    </script>

    <style type="text/css">
        .icon-expand-all
        {
            background-image: url(/resources/images/expand-all.gif) !important;
        }
        .icon-collapse-all
        {
            background-image: url(/resources/images/collapse-all.gif) !important;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <ext:ScriptManager ID="ScriptManager1" runat="server" />
        <ext:Store runat="server" ID="Store1" AutoLoad="true" OnRefreshData="Store_RefreshData">
            <Reader>
                <ext:JsonReader ReaderID="ITEM_CLASS">
                    <Fields>
                        <ext:RecordField Name="ITEM_CLASS" />
                        <ext:RecordField Name="ITEM_TYPE" />
                        <ext:RecordField Name="ZCHJ" />
                        <ext:RecordField Name="CC" />
                        <ext:RecordField Name="DD" />
                    </Fields>
                </ext:JsonReader>
            </Reader>
        </ext:Store>
        <ext:ViewPort ID="ViewPort1" runat="server">
            <Body>
                <ext:ColumnLayout ID="ColumnLayout2" runat="server" FitHeight="true">
                    <Columns>
                        <ext:LayoutColumn ColumnWidth="1">
                            <ext:GridPanel ID="GridPanel1" runat="server" BodyBorder="false" AutoScroll="true"
                                Border="false" StoreID="Store1" StripeRows="true" TrackMouseOver="true" Height="480"
                                AutoWidth="true">
                                <TopBar>
                                    <ext:Toolbar ID="Toolbar1" runat="server">
                                        <Items>
                                            <ext:Button ID="CancelButton" runat="server" Text="返回" Icon="ArrowUndo">
                                                <Listeners>
                                                    <Click Handler="parent.DetailWin.hide();" />
                                                </Listeners>
                                            </ext:Button>
                                            <ext:ToolbarSeparator ID="ToolbarSeparator3" runat="server">
                                            </ext:ToolbarSeparator>
                                            <ext:Button ID="Btn_Excel" runat="server" Disabled="true" Icon="PageWhiteExcel" Text="EXCEL导出"
                                                OnClick="OutExcel" AutoPostBack="true">
                                            </ext:Button>
                                        </Items>
                                    </ext:Toolbar>
                                </TopBar>
                                <ColumnModel ID="ColumnModel1" runat="server">
                                    <Columns>
                                        <ext:Column Header="项目名称" Width="130" ColumnID="ITEM_CLASS" DataIndex="ITEM_CLASS">
                                        </ext:Column>
                                        <ext:Column Header="支出合计" Width="90" ColumnID="ZCHJ" DataIndex="ZCHJ">
                                            <Renderer Fn="rmbMoney" />
                                        </ext:Column>
                                        <ext:Column Header="实收支出" Width="90" ColumnID="CC" DataIndex="CC">
                                            <Renderer Fn="rmbMoney" />
                                        </ext:Column>
                                        <ext:Column Header="计价支出" Width="90" ColumnID="DD" DataIndex="DD">
                                            <Renderer Fn="rmbMoney" />
                                        </ext:Column>
                                    </Columns>
                                </ColumnModel>
                                <Listeners>
                                    <Command Handler=" gridCommand(command,record);" />
                                </Listeners>
                                <SelectionModel>
                                    <ext:RowSelectionModel ID="rowselection" SingleSelect="true" />
                                </SelectionModel>
                                <LoadMask ShowMask="true" Msg="载入中..." />
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
