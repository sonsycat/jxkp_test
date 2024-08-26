<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Eval_Result_Detail.aspx.cs"
    Inherits="GoldNet.JXKP.jxkh.Eval_Result_Detail" %>

<%@ Register Assembly="Goldnet.Ext.Web" Namespace="Goldnet.Ext.Web" TagPrefix="ext" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>评价结果一览</title>
    <style type="text/css">
        .x-grid3-col
        {
            border-left: 1px solid #eee;
        }
        .x-grid3-row td
        {
            padding-left: 0px;
            padding-right: 0px;
        }
    </style>

    <script type="text/javascript">
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

</head>
<body>
    <form id="form1" runat="server">
    <ext:ScriptManager ID="ScriptManager1" runat="server">
        <AjaxEvents>
            <DocumentReady OnEvent="GridDataBind">
                <EventMask ShowMask="true" Msg="数据加载中..." />
            </DocumentReady>
        </AjaxEvents>
        <%--<Listeners>
            <DocumentReady Handler="if (Hide_Flag.value!= '0') Comb_BonusClass.setVisible(false);" />
        </Listeners>--%>
    </ext:ScriptManager>
    <ext:Store runat="server" ID="Store1">
        <Reader>
            <ext:JsonReader>
                <Fields>
                </Fields>
            </ext:JsonReader>
        </Reader>
    </ext:Store>
    <ext:ViewPort ID="ViewPort1" runat="server">
        <Body>
            <ext:BorderLayout ID="BorderLayout1" runat="server">
                <North>
                    <ext:Toolbar ID="Toolbar1" runat="server">
                        <Items>
                            <ext:Label ID="Label1" runat="server" Text="评价名称：">
                            </ext:Label>
                            <ext:TextField ID="Txt_EvalName" runat="server" Width="100" AllowBlank="false" EmptyText="请输入评价名称"
                                BlankText="请输入评价名称！">
                            </ext:TextField>
                            <ext:ToolbarSeparator ID="ToolbarSeparator5" runat="server" />
                            <ext:Label ID="Label2" runat="server" Text="评价类别：">
                            </ext:Label>
                            <ext:ComboBox ID="Comb_EvalType" runat="server" Width="120" AllowBlank="false" EmptyText="请选择评价类别"
                                BlankText="请选择评价类别!">
                            </ext:ComboBox>
                            <%--<ext:Hidden ID="Hide_Flag"  runat="server" Text="0"></ext:Hidden>
                        <ext:ComboBox ID="Comb_BonusClass" runat="server" Width="120" AllowBlank="false" EmptyText="请选择奖金测评类别" BlankText="请选择奖金测评类别!">
                        </ext:ComboBox>--%>
                            <ext:ToolbarSeparator ID="ToolbarSeparator4" runat="server" />
                            <ext:Button ID="Btn_Save" runat="server" Text="保存" Icon="Disk">
                                <AjaxEvents>
                                    <Click OnEvent="Btn_Save_Click">
                                        <EventMask Msg="请稍候..." ShowMask="true" />
                                    </Click>
                                </AjaxEvents>
                            </ext:Button>
                            <ext:ToolbarSeparator ID="ToolbarSeparator1" runat="server" />
                            <ext:Button ID="Btn_Export" runat="server" Text="导出Excel" Icon="PageWhiteExcel" OnClick="OutExcel"
                                AutoPostBack="true">
                            </ext:Button>
                            <ext:ToolbarSeparator ID="ToolbarSeparator3" runat="server" />
                            <ext:ToolbarFill ID="ToolbarFill1" runat="server">
                            </ext:ToolbarFill>
                            <ext:ToolbarSeparator ID="ToolbarSeparator2" runat="server" />
                            <ext:Button ID="Btn_Close" runat="server" Text="关闭" Icon="ArrowUndo">
                                <Listeners>
                                    <Click Handler="parent.DetailWin.hide();" />
                                </Listeners>
                            </ext:Button>
                        </Items>
                    </ext:Toolbar>
                </North>
                <Center>
                    <ext:GridPanel ID="GridPanel_Show" runat="server" Border="false" StoreID="Store1"
                        Width="300" Height="300" StripeRows="true" AutoScroll="true">
                        <ColumnModel ID="ColumnModel1" runat="server">
                            <Columns>
                                <ext:RowNumbererColumn Width="32" Resizable="true" />
                            </Columns>
                        </ColumnModel>
                        <SelectionModel>
                            <ext:RowSelectionModel SingleSelect="true" />
                        </SelectionModel>
                        <LoadMask ShowMask="true" Msg="请稍候..." />
                    </ext:GridPanel>
                </Center>
                <South Split="true" MinHeight="60">
                    <ext:TextArea runat="server" Height="60" ID="TxtMemo" FieldLabel="评价简述" HideWithLabel="false"
                        MaxLength="128" EmptyText="请输入评价简述">
                    </ext:TextArea>
                </South>
            </ext:BorderLayout>
        </Body>
    </ext:ViewPort>
    </form>
</body>
</html>
