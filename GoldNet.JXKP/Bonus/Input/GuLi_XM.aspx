<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="GuLi_XM.aspx.cs" Inherits="GoldNet.JXKP.Bonus.Input.GuLi_XM" %>
<%@ Register Assembly="Goldnet.Ext.Web" Namespace="Goldnet.Ext.Web" TagPrefix="ext" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>无标题页</title>

    <script type="text/javascript">
        var RowIndex;
        /*
        GRIDPANEL操作
        optype :1 添加;  3 删除;
        */
        function TreeOpration(optype) {
            if (optype == "1") {
                txtName.setValue("");
                TextField1.setValue("");
                TextField2.setValue("");
                Btn_BatStart.setText("查询");
                arcEditWindow.show();
            }
            else if (optype == "3") {
                Btn_BatStart.setText("");
                TextField1.setValue(this.Store1.getAt(RowIndex).get('ITEM_CODE'));
                Ext.Msg.confirm("删除项目", "确定要删除该项目：" + this.Store1.getAt(RowIndex).get('ITEM_NAME') + " 吗？", function (btn, text) { OpCallback(btn) });
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
            if (Btn_BatStart.text == "查询") {
                optype = "4";
                Btn_BatStart.setText("保存");
            }
            if ((btn != "ok") && (btn != "yes")) {
                return;
            }
            var name = txtName.getValue();  //科室编码
            var a1 = TextField1.getValue();  //项目编码
            var a11 = TextField2.getValue();  //比例

            if (optype == "1") {
                GridPanelToDataBase(a1, name, a11, optype);
            } else if (optype == "3") {
                GridPanelToDataBase(this.Store1.getAt(RowIndex).get('ITEM_CODE'), name, "", optype);
            } else if (optype == "4") {
                GridPanelToDataBase(a1,name, "", optype);
            }
        }

        function GridPanelToDataBase(a1, name,a11, optype) {
            Goldnet.AjaxMethod.request(
                  'DictAjaxOper',
                    {
                        params: {
                            DictCode: a1, DictName: name,BL:a11, OperType: optype
                        },
                        success: function (result) {
                            Store1.reload();
                            btn_Delete.setDisabled(true);
                            //                            arcEditWindow.hide();
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
                    <ext:RecordField Name="DEPT_CODE" />
                    <ext:RecordField Name="DEPT_NAME" />
                    <ext:RecordField Name="ITEM_CODE" />
                    <ext:RecordField Name="ITEM_NAME" />
                    <ext:RecordField Name="BL" />
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
                                <ext:Button Text="删除" ID="btn_Delete" runat="server" Icon="Delete" Disabled="true">
                                    <Listeners>
                                        <Click Handler="TreeOpration(3)" />
                                    </Listeners>
                                </ext:Button>
                                 <ext:Button ID="btn_Excel" runat="server" OnClick="OutExcel" AutoPostBack="true"
                                                Text="导出Excel" Icon="PageWhiteExcel" CausesValidation="false">
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
                                                        <ext:Column Header="科室编码" Width="100" Align="Center" Sortable="false" MenuDisabled="true"
                                                            ColumnID="DEPT_CODE" DataIndex="DEPT_CODE" />
                                                        <ext:Column Header="科室名称" Width="150" Align="Center" Sortable="false" MenuDisabled="true"
                                                            ColumnID="DEPT_NAME" DataIndex="DEPT_NAME" /> 
                                                        <ext:Column Header="项目编码" Width="150" Align="Center" Sortable="false" MenuDisabled="true"
                                                            ColumnID="ITEM_CODE" DataIndex="ITEM_CODE" />
                                                         <ext:Column Header="项目名称" Width="150" Align="Center" Sortable="false" MenuDisabled="true"
                                                            ColumnID="ITEM_NAME" DataIndex="ITEM_NAME" />
                                                         <ext:Column Header="比例" Width="150" Align="Center" Sortable="false" MenuDisabled="true"
                                                            ColumnID="BL" DataIndex="BL" />                                                                                                                 
                                                    </Columns>
                                                </ColumnModel>
                                                <SelectionModel>
                                                    <ext:RowSelectionModel ID="rowselection" SingleSelect="true">
                                                        <Listeners>
                                                            <RowSelect Handler="#{btn_Delete}.enable();RowIndex = rowIndex;" />
                                                            <RowDeselect Handler="if (!#{GridPanel_Show}.hasSelection()) {#{btn_Delete}.disable();}RowIndex = -1;" />
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
    <ext:Window ID="arcEditWindow" runat="server" Icon="Group" Title="设置护士工作量项目" Width="250"
        Height="230" AutoShow="false" Modal="true" CenterOnLoad="true" ShowOnLoad="false"
        Resizable="false" StyleSpec="background-color:Transparent;" BodyStyle="background-color:Transparent;">
        <Body>
            <table style="margin: 10px,10px,10px,10px">  
                <tr>
                    <td>
                        项目编码:
                    </td>
                    <td align="left">
                        <ext:TextField runat="server" ID="TextField1" Width="120">
                        </ext:TextField>
                    </td>
                </tr>
                <tr>
                    <td>
                        科室编码:
                    </td>
                    <td align="left">
                        <ext:TextField runat="server" ID="txtName" Width="120">
                        </ext:TextField>
                    </td>
                </tr>
                 <tr>
                    <td>
                        比例:
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
