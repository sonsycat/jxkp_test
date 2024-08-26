<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="JobListDict.aspx.cs" Inherits="GoldNet.JXKP.RLZY.DICT.JobListDict" %>

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
                Ext.Msg.prompt("添加项目", "请输入项目名称：", function(btn, text) { nodeOpCallback(btn, text, optype) });
            } else if (optype == "2") {
                Ext.Msg.prompt("重命名项目", "请输入项目名称：", function(btn, text) { nodeOpCallback(btn, text, optype) }, true, false, this.Store1.getAt(RowIndex).get('NAME'));
            } else if (optype == "3") {
                Ext.Msg.confirm("删除项目", "确定要删除该项目：" + this.Store1.getAt(RowIndex).get('NAME') + " 吗？", function(btn, text) { nodeOpCallback(btn, text, optype) });
            } 
        }
        
        
        /*
            节点增删改操作回调函数
        */
        function nodeOpCallback (btn, text, optype) {
            if ((btn != "ok") && (btn != "yes")) {
                return;
            }
            if (optype == "1") {
               GridPanelToDataBase("",text,optype);
            } else if (optype == "2") {
               GridPanelToDataBase(this.Store1.getAt(RowIndex).get('ID'), text,optype);
            } else if (optype == "3") {
               GridPanelToDataBase(this.Store1.getAt(RowIndex).get('ID'),"",optype);
            }
        }
        
        function GridPanelToDataBase(id,text,optype) {
          Goldnet.AjaxMethod.request(
                  'DictAjaxOper',
                    {
                        params: {
                            DictCode:id,DictName:text,OperType:optype
                        },
                        success: function(result) {
                            Store1.reload();
                            btn_Modify.setDisabled(true);
                            btn_Delete.setDisabled(true);
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
                    <ext:RecordField Name="NAME" />
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
                                                        <ext:Column Header="序号" Width="90" Align="Left" Sortable="false" MenuDisabled="true"
                                                            ColumnID="ID" DataIndex="ID" />
                                                        <ext:Column Header="职称" Width="150" Align="Center" Sortable="false" MenuDisabled="true"
                                                            ColumnID="NAME" DataIndex="NAME" />
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
    </form>
</body>
</html>
