<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="income_item_other.aspx.cs" Inherits="GoldNet.JXKP.cbhs.cbhsdict.income_item_other" %>
<%@ Register Assembly="Goldnet.Ext.Web" Namespace="Goldnet.Ext.Web" TagPrefix="ext" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>无标题页</title>

    <script type="text/javascript">
        var RowIndex;
        /*
        GRIDPANEL操作
        optype :1 添加;  2 重命名; 3 删除;
        */
        function TreeOpration(optype) {
            if (optype == "1") {
                TextField3.setValue("");
                TextField1.setValue("");
                TextField2.setValue("");
                Btn_BatStart.setText("保存");
                arcEditWindow.show();
            } else if (optype == "2") {
                TextField3.setValue(this.Store1.getAt(RowIndex).get('CLASS_CODE'));
                TextField1.setValue(this.Store1.getAt(RowIndex).get('CLASS_NAME'));
                TextField2.setValue(this.Store1.getAt(RowIndex).get('INPUT_CODE'));
                Btn_BatStart.setText("修改");
                arcEditWindow.show();
            } else if (optype == "3") {
                Btn_BatStart.setText("");
                TextField3.setValue(this.Store1.getAt(RowIndex).get('CLASS_CODE'))
                Ext.Msg.confirm("删除项目", "确定要删除该项目：" + this.Store1.getAt(RowIndex).get('CLASS_NAME') + " 吗？", function (btn, text) { OpCallback(btn) });

            }
        }


        /*
        节点增删改操作回调函数
        */
        function OpCallback(btn) {
            var optype = '3';
            if (Btn_BatStart.text == "保存") {
                optype = "1";
            }
            if (Btn_BatStart.text == "修改") {
                optype = "2";
            }
            if ((btn != "ok") && (btn != "yes")) {
                return;
            }

            var code = TextField3.getValue();
            var name = "";
            var a1 = TextField1.getValue();
            var a2 = TextField2.getValue();

            if (optype == "1") {
                GridPanelToDataBase(code, name, optype, a1, a2);
            } else if (optype == "2") {
                GridPanelToDataBase(code, name, optype, a1, a2);
            } else if (optype == "3") {
                GridPanelToDataBase(this.Store1.getAt(RowIndex).get('CLASS_CODE'), "", optype, "", "");
            }
        }

        function GridPanelToDataBase(code, name, optype, a1, a2) {
            Goldnet.AjaxMethod.request(
                  'DictAjaxOper',
                    {
                        params: {
                            CLASS_CODE: code, DEPT_NAME: name, OperType: optype, CLASS_NAME: a1, INPUT_CODE: a2
                        },
                        success: function (result) {
                            Store1.reload();
                            btn_Modify.setDisabled(true);
                            btn_Delete.setDisabled(true);
                            arcEditWindow.hide();
                        },
                        failure: function (msg) {
                            GridPanel_Show.el.unmask();
                        }
                    });
        }
    </script>

</head>
<body>
    <form id="form1" runat="server">
    <ext:ScriptManager ID="ScriptManager1" runat="server">
    </ext:ScriptManager>
    <ext:Store ID="Store1" runat="server" OnRefreshData="Data_RefreshData">
        <Reader>
            <ext:JsonReader>
                <Fields>
                    <ext:RecordField Name="SERIAL_NO" />
                    <ext:RecordField Name="CLASS_CODE" />
                    <ext:RecordField Name="CLASS_NAME" />
                    <ext:RecordField Name="INPUT_CODE" />
                </Fields>
            </ext:JsonReader>
        </Reader>
    </ext:Store>
    <div>
        <ext:ViewPort ID="ViewPort1" runat="server">
            <Body>
                <ext:BorderLayout ID="BorderLayout2" runat="server">
                    <North>
                        <ext:Toolbar runat="server" ID="ctl155" StyleSpec="border:0">
                            <Items>
                                <ext:Button ID="btn_Add" runat="server" Text="添加" Icon="Add">
                                    <Listeners>
                                        <Click Handler="TreeOpration(1)" />
                                    </Listeners>
                                </ext:Button>
                                <ext:Button Text="修改" ID="btn_Modify" runat="server" Icon="FolderEdit" Disabled="true">
                                    <Listeners>
                                        <Click Handler="TreeOpration(2)" />
                                    </Listeners>
                                </ext:Button>
                                <ext:Button Text="删除" ID="btn_Delete" runat="server" Icon="Delete" Disabled="true">
                                    <Listeners>
                                        <Click Handler="TreeOpration(3)" />
                                    </Listeners>
                                </ext:Button>
                            </Items>
                        </ext:Toolbar>
                    </North>
                    <Center>
                        <ext:Panel ID="Panel1" runat="server">
                            <Body>
                                <ext:ColumnLayout ID="ColumnLayout1" runat="server" Split="true" FitHeight="true">
                                    <Columns>
                                        <ext:LayoutColumn ColumnWidth="1">
                                            <ext:GridPanel ID="GridPanel_Show" runat="server" StoreID="Store1" Border="false"
                                                AutoWidth="true" Header="false" AutoScroll="true">
                                                <ColumnModel ID="ColumnModel1" runat="server">
                                                    <Columns>
                                                        <ext:Column Header="序号" Width="100" Align="Center" Sortable="false" MenuDisabled="true"
                                                            ColumnID="SERIAL_NO" DataIndex="SERIAL_NO" />
                                                        <ext:Column Header="项目编码" Width="150" Align="Center" Sortable="false" MenuDisabled="true"
                                                            ColumnID="CLASS_CODE" DataIndex="CLASS_CODE" />
                                                        <ext:Column Header="项目名称" Width="150" Align="Center" Sortable="false" MenuDisabled="true"
                                                            ColumnID="CLASS_NAME" DataIndex="CLASS_NAME" />
                                                         <ext:Column Header="缩写代码" Width="150" Align="Center" Sortable="false" MenuDisabled="true"
                                                            ColumnID="INPUT_CODE" DataIndex="INPUT_CODE" />
                                                    </Columns>
                                                </ColumnModel>
                                                <SelectionModel>
                                                    <ext:RowSelectionModel ID="rowselection" SingleSelect="true">
                                                        <Listeners>
                                                            <RowSelect Handler="#{btn_Delete}.enable();#{btn_Modify}.enable();RowIndex = rowIndex;" />
                                                            <RowDeselect Handler="if (!#{GridPanel_Show}.hasSelection()) {#{btn_Delete}.disable();#{btn_Modify}.disable();}RowIndex = -1;" />
                                                        </Listeners>
                                                    </ext:RowSelectionModel>
                                                </SelectionModel>
                                                <LoadMask ShowMask="true" />
                                            </ext:GridPanel>
                                        </ext:LayoutColumn>
                                    </Columns>
                                </ext:ColumnLayout>
                            </Body>
                        </ext:Panel>
                    </Center>
                </ext:BorderLayout>
            </Body>
        </ext:ViewPort>
    </div>
    <ext:Window ID="arcEditWindow" runat="server" Icon="Group" Title="设置核算收入项目" Width="250"
        Height="230" AutoShow="false" Modal="true" CenterOnLoad="true" ShowOnLoad="false"
        Resizable="false" StyleSpec="background-color:Transparent;" BodyStyle="background-color:Transparent;">
        <Body>
            <table style="margin: 10px,10px,10px,10px">
                <tr>
                    <td>
                        项目代码:
                    </td>
                    <td align="left">
                         <ext:TextField runat="server" ID="TextField3" Width="120">
                        </ext:TextField>
                    </td>
                </tr>                
                <tr>
                    <td>
                        项目名称:
                    </td>
                    <td align="left">
                        <ext:TextField runat="server" ID="TextField1" Width="120">
                        </ext:TextField>
                    </td>
                </tr>   
                <tr>
                    <td>
                        缩写代码:
                    </td>
                    <td align="left">
                        <ext:TextField runat="server" ID="TextField2" Width="120">
                        </ext:TextField>
                    </td>
                </tr>               
            </table>
        </Body>
        <BottomBar>
            <ext:Toolbar ID="Toolbar1" runat="server">
                <Items>
                    <ext:ToolbarFill ID="ToolbarFill2" runat="server" />
                    <ext:ToolbarButton ID="Btn_BatStart" runat="server" Icon="Disk" Text="保存">
                        <Listeners>
                            <Click Handler="OpCallback('ok');" />
                        </Listeners>
                    </ext:ToolbarButton>
                    <ext:ToolbarSeparator ID="ToolbarSeparator3" runat="server" />
                    <ext:ToolbarButton ID="Btn_BatCancel" runat="server" Icon="Cancel" Text="退出">
                        <Listeners>
                            <Click Handler="arcEditWindow.hide();" />
                        </Listeners>
                    </ext:ToolbarButton>
                </Items>
            </ext:Toolbar>
        </BottomBar>
    </ext:Window>
    </form>
</body>
</html>
