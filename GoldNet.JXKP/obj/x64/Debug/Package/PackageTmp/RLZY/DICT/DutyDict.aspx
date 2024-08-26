<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DutyDict.aspx.cs" Inherits="GoldNet.JXKP.RLZY.DICT.DutyDict" %>

<%@ Register Assembly="Goldnet.Ext.Web" Namespace="Goldnet.Ext.Web" TagPrefix="ext" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>无标题页</title>

    <script type="text/javascript">
        var RowIndex;                   
        /*
            GRIDPANEL操作
            optype :1 添加;  2 重命名; 3 删除;
        */
        function TreeOpration(optype) {
            if (optype == "1") {
                txtCode.setValue("");
                txtName.setValue("");
                TextField1.setValue("");
                TextField2.setValue("");
                Btn_BatStart.setText("保存");
                arcEditWindow.show();
            } else if (optype == "2") {
                txtCode.setValue(this.Store1.getAt(RowIndex).get('ID'));
                txtName.setValue(this.Store1.getAt(RowIndex).get('DUTY'));
                TextField1.setValue(this.Store1.getAt(RowIndex).get('SUBSIDY'));
                TextField2.setValue(this.Store1.getAt(RowIndex).get('SORT_NO'));
                Btn_BatStart.setText("修改");
                arcEditWindow.show();
            } else if (optype == "3") {
                Btn_BatStart.setText("");
                Ext.Msg.confirm("删除项目", "确定要删除该项目：" + this.Store1.getAt(RowIndex).get('DUTY') + " 吗？", function(btn, text) { OpCallback(btn) });
            } 
        }
        
        
        /*
            节点增删改操作回调函数
        */
        function OpCallback (btn) {
            var optype = '3';
            if (Btn_BatStart.text == "保存") {
                optype = "1";
            }
            if(Btn_BatStart.text == "修改") {
                optype = "2";
            }
            if((btn != "ok") && (btn != "yes")){
                return;
            }
            var code = txtCode.getValue();
            var name = txtName.getValue();
            var a1= TextField1.getValue();
            var a2= TextField2.getValue();
            
            if (optype == "1") {
               GridPanelToDataBase("",code,name,optype,a1,a2);
            } else if (optype == "2") {
               GridPanelToDataBase(this.Store1.getAt(RowIndex).get('ID'), code,name,optype,a1,a2);
            } else if (optype == "3") {
               GridPanelToDataBase(this.Store1.getAt(RowIndex).get('ID'),"","",optype,a1,a2);
            }
        }
        
        function GridPanelToDataBase(id,code,name,optype,a1,a2) {
          Goldnet.AjaxMethod.request(
                  'DictAjaxOper',
                    {
                        params: {
                            DictCode:id,DictName:name,OperType:optype,Subsidy:a1,Sortno:a2
                        },
                        success: function(result) {
                            Store1.reload();
                            btn_Modify.setDisabled(true);
                            btn_Delete.setDisabled(true);
                            arcEditWindow.hide();
                        },
                        failure: function(msg) {
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
                    <ext:RecordField Name="ID" />
                    <ext:RecordField Name="DUTY" />
                    <ext:RecordField Name="SUBSIDY" />
                    <ext:RecordField Name="SORT_NO" />
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
                                                        <ext:Column Header="ID" Width="100" Align="Center" Sortable="false" MenuDisabled="true"
                                                            ColumnID="ID" DataIndex="ID" />
                                                        <ext:Column Header="职务" Width="150" Align="Center" Sortable="false" MenuDisabled="true"
                                                            ColumnID="DUTY" DataIndex="DUTY" />
                                                        <ext:Column Header="津贴" Width="150" Align="Center" Sortable="false" MenuDisabled="true"
                                                            ColumnID="SUBSIDY" DataIndex="SUBSIDY" />
                                                        <ext:Column Header="排序" Width="150" Align="Center" Sortable="false" MenuDisabled="true"
                                                            ColumnID="SORT_NO" DataIndex="SORT_NO" />
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
    <ext:Window ID="arcEditWindow" runat="server" Icon="Group" Title="设置职务字典" Width="250"
        Height="230" AutoShow="false" Modal="true" CenterOnLoad="true" ShowOnLoad="false"
        Resizable="false" StyleSpec="background-color:Transparent;" BodyStyle="background-color:Transparent;">
        <Body>
            <table style="margin: 10px,10px,10px,10px">
                <tr>
                    <td>
                        ID:
                    </td>
                    <td align="left">
                        <ext:TextField runat="server" ID="txtCode" Width="120" ReadOnly="true">
                        </ext:TextField>
                    </td>
                </tr>
                <tr>
                    <td>
                        职务名称:
                    </td>
                    <td align="left">
                        <ext:TextField runat="server" ID="txtName" Width="120">
                        </ext:TextField>
                    </td>
                </tr>
                <tr>
                    <td>
                        津贴:
                    </td>
                    <td align="left">
                        <ext:TextField runat="server" ID="TextField1" Width="120">
                        </ext:TextField>
                    </td>
                </tr>
                <tr>
                    <td>
                        排序:
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
