<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SetReport.aspx.cs" Inherits="GoldNet.JXKP.GuideLook.SetReport" %>

<%@ Register Assembly="Goldnet.Ext.Web" Namespace="Goldnet.Ext.Web" TagPrefix="ext" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>无标题页</title>

    <script type="text/javascript">
        var RowIndex;                   
        function getcheckednode(node) {
            var Nodeid = "";
            if(node.hasChildNodes()) {
                Nodeid = 'p'+node.id;
            } else {
                if(node.parentNode.id == 'root') {
                    Nodeid = 'p'+node.id;
                } else {
                    Nodeid = node.id;
                }
            }
            if(Nodeid.indexOf('p') == 0) {
              btn_Add.setDisabled(true);
              btn_Modify.setDisabled(true);
              btn_Delete.setDisabled(true);
           } else {
              btn_Add.setDisabled(false);
              btn_Modify.setDisabled(true);
              btn_Delete.setDisabled(true);
           }
           Goldnet.AjaxMethod.request(
                  'TreeClick',
                    {
                    params: {
                        NodeCode:Nodeid
                    },
                    success: function(result) {
                        btn_up.setDisabled(true);
                        btn_down.setDisabled(true);
                    },
                    failure: function(msg) {
                        GridPanel1.el.unmask();
                    }
                }); 
        }
       function getBtnName(command) {
            document.form1.hideBtn.value = command;
        }
        
        function FormatType(value) {
            var template = '<span style="color:{0};">{1}</span>';
            if(value=='K') {
               return  String.format(template, 'green', "科");  
            }
            if(value == 'R') {
                return  String.format(template, 'green', "人");      
            }
            if(value == "Z") {
                return  String.format(template, 'green', "组");  
            }
        }
        
        function FormatReportType(v, p, record, rowIndex) {
            
            var a = Number(record.data.RPT_CLASSID)*100000000;
            
            a=a.toString(16).substr(0,6);
             
            var template = '<span style="color:#{0};">{1}</span>';
            
            return String.format(template, a, record.data.RPT_PNAME);  
        }
        var RefreshData = function() {
            Store1.reload();
        }
        
        /*
            GRIDPANEL操作
            optype :1 添加;  2 重命名; 3 删除; 4 上移; 5 下移
        */
        function TreeOpration(optype) {
            
            var selNode;
            if (TreeCtrl.getSelectionModel().getSelectedNode() == null) {
                return;
            } else {
                selNode = TreeCtrl.getSelectionModel().getSelectedNode();
            } 
                      
            if (optype == "1") {
                Ext.Msg.prompt("添加报表", "请输入报表名称：", function(btn, text) { nodeOpCallback(btn, text, optype, selNode) });
            } else if (optype == "2") {
                Ext.Msg.prompt("重命名报表", "请输入报表名称：", function(btn, text) { nodeOpCallback(btn, text, optype, selNode) }, true, false, this.Store1.getAt(RowIndex).get('RPT_NAME'));
            } else if (optype == "3") {
                Ext.Msg.confirm("删除报表", "确定要删除该报表：" + this.Store1.getAt(RowIndex).get('RPT_NAME') + " 吗？", function(btn, text) { nodeOpCallback(btn, text, optype, selNode) });
            } else if(optype == "4") {
                RowsMove(-1);
                GridPanelToDataBase(this.Store1.getAt(RowIndex).get('RPT_NAME'), "4", selNode, this.Store1.getAt(RowIndex).get('ID'), this.Store1.getAt(RowIndex + 1).get('ID'), this.Store1.getAt(RowIndex).get('RPT_RANKING'), this.Store1.getAt(RowIndex + 1).get('RPT_RANKING'));
        } else if (optype == "5") {
                RowsMove(1);
                GridPanelToDataBase(this.Store1.getAt(RowIndex).get('RPT_NAME'), "5", selNode, this.Store1.getAt(RowIndex).get('ID'), this.Store1.getAt(RowIndex - 1).get('ID'), this.Store1.getAt(RowIndex).get('RPT_RANKING'), this.Store1.getAt(RowIndex - 1).get('RPT_RANKING'));
            }
        }
        
        
        /*
            节点增删改操作回调函数
        */
        function nodeOpCallback (btn, text, optype, node) {
            if ((btn != "ok") && (btn != "yes")) {
                return;
            }
            if (optype == "1") {
                GridPanelToDataBase(text, optype, node,"","","","");
            } else if (optype == "2") {
                GridPanelToDataBase(text, optype, node,this.Store1.getAt(RowIndex).get('ID'),"","",this.Store1.getAt(RowIndex).get('RPT_RANKING'));
            } else if (optype == "3") {
               GridPanelToDataBase(text, optype, node,this.Store1.getAt(RowIndex).get('ID'),"","","");
            }
        }
        
        function GridPanelToDataBase(text, optype, node,reportid,TempId,TempRanking,SortNum) {
        
         if(optype == "4" || optype == "5") {
            GridPanel_Show.el.mask('正在保存...', 'x-loading-mask');
         }
          Goldnet.AjaxMethod.request(
                  'DataBaseByOperaType',
                    {
                        params: {
                            ReportName: text, Oper: optype, ReportTypeId: node.id, Reportid: reportid, TempRoportCode: TempId, TempReportRanking: TempRanking, ReportSortNum: SortNum
                        },
                        success: function(result) {
                            if (optype == "4" || optype == "5") {
                                
                                if(RowIndex != 0) {
                                    btn_up.setDisabled(false);
                                }
                                if(RowIndex != Store1.getCount() - 1) {
                                    btn_down.setDisabled(false);
                                }
                            }
                            else {
                                Store1.reload();
                            }
                            GridPanel_Show.el.unmask();
                        },
                        failure: function(msg) {
                            GridPanel_Show.el.unmask();
                        }
                    });
        }
        

        
        function RowsMove( rowoffset ) {
            // rowoffset -1 上移； 1 下移
            var selections = rowselection.getSelections();
            var index = Store1.indexOf(selections[0]);
            if ((rowoffset == -1) && (index == 0)) return;
            if ((rowoffset == 1) && (index == (Store1.getCount() - 1))) return;
            var selectionsoffset = this.Store1.getAt(index + rowoffset);
            var tmp = selections[0].data.RPT_RANKING;
            selections[0].data.RPT_RANKING = selectionsoffset.data.RPT_RANKING;
            selectionsoffset.data.RPT_RANKING = tmp;
            Store1.remove(selections[0]);            
            Store1.insert(index + rowoffset, selections[0]);
            GridPanel_Show.getView().refresh(); //更新视图的同时，行号被更新
            rowselection.selectRow(index+rowoffset);//依然选中刚才移动的那一行
        }
        

        
    </script>

</head>
<body>
    <form id="form1" runat="server">
    <ext:Hidden ID="hideBtn" runat="server">
    </ext:Hidden>
    <ext:ScriptManager ID="ScriptManager1" runat="server" />
    <ext:Store ID="Store1" runat="server" OnRefreshData="Data_RefreshData" WarningOnDirty="false">
        <Reader>
            <ext:JsonReader>
                <Fields>
                    <ext:RecordField Name="ID" />
                    <ext:RecordField Name="RPT_NAME" />
                    <ext:RecordField Name="RPT_TYPE" />
                    <ext:RecordField Name="RPT_CLASSID" />
                    <ext:RecordField Name="RPT_PNAME" />
                    <ext:RecordField Name="RPT_RANKING" />
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
                                <ext:Button ID="btn_Add" runat="server" Text="添加报表" Icon="Add">
                                    <Listeners>
                                        <Click Handler="TreeOpration(1)" />
                                    </Listeners>
                                </ext:Button>
                                <ext:Button Text="修改报表" ID="btn_Modify" runat="server" Icon="FolderEdit">
                                    <Listeners>
                                        <Click Handler="TreeOpration(2)" />
                                    </Listeners>
                                </ext:Button>
                                <ext:Button Text="删除报表" ID="btn_Delete" runat="server" Icon="Delete">
                                    <Listeners>
                                        <Click Handler="TreeOpration(3)" />
                                    </Listeners>
                                </ext:Button>
                                <ext:Button Text="向上移动" ID="btn_up" runat="server" Disabled="true" Icon="ArrowUp">
                                    <Listeners>
                                        <Click Handler="TreeOpration(4);this.setDisabled(true);" />
                                    </Listeners>
                                </ext:Button>
                                <ext:Button Text="向下移动" ID="btn_down" runat="server" Disabled="true" Icon="ArrowDown">
                                    <Listeners>
                                        <Click Handler="TreeOpration(5);this.setDisabled(true);" />
                                    </Listeners>
                                </ext:Button>
                            </Items>
                        </ext:Toolbar>
                    </North>
                    <West Collapsible="false" Split="false" CollapseMode="Mini">
                        <ext:Panel ID="Panel2" runat="server" Width="175" BodyBorder="false" Title="报表类别"
                            AutoScroll="true" Border="false">
                            <Body>
                                <ext:TreePanel runat="server" Width="175" ID="TreeCtrl" AutoHeight="true" AutoScroll="false"
                                    Border="false">
                                    <Listeners>
                                        <Click Handler="getcheckednode(node)" />
                                    </Listeners>
                                </ext:TreePanel>
                            </Body>
                        </ext:Panel>
                    </West>
                    <Center>
                        <ext:GridPanel ID="GridPanel_Show" runat="server" Border="false" AutoWidth="true"
                            Height="591" Title="报表信息" MonitorResize="true" MonitorWindowResize="true" StripeRows="true"
                            TrackMouseOver="true" StoreID="Store1" AutoExpandColumn="Columns2">
                            <ColumnModel ID="ColumnModel1" runat="server">
                                <Columns>
                                    <ext:Column Header="<center>报表类别</center>" Sortable="true" ColumnID="Columns4" DataIndex="RPT_PNAME"
                                        Align="center" Width="116">
                                        <Renderer Fn="FormatReportType" />
                                    </ext:Column>
                                    <ext:Column Header="<center>名称</center>" Sortable="true" ColumnID="Columns2" DataIndex="RPT_NAME"
                                        Align="center" Width="116">
                                    </ext:Column>
                                    <ext:Column Header="<center>科室类别</center>" Sortable="true" ColumnID="Columns3" DataIndex="RPT_TYPE"
                                        Align="center" Width="116">
                                        <Renderer Fn="FormatType" />
                                    </ext:Column>
                                    <ext:CommandColumn Header="<center>定义报表</center>" Sortable="true" ColumnID="Columns4"
                                        Align="center" Width="116">
                                        <Commands>
                                            <ext:GridCommand Icon="NoteEdit" CommandName="Edit" Text="定义" />
                                        </Commands>
                                    </ext:CommandColumn>
                                    <ext:CommandColumn Header="<center>预览</center>" Sortable="true" ColumnID="Columns5"
                                        Align="center" Width="116">
                                        <Commands>
                                            <ext:GridCommand Icon="Zoom" CommandName="Show" Text="预览">
                                            </ext:GridCommand>
                                        </Commands>
                                    </ext:CommandColumn>
                                </Columns>
                            </ColumnModel>
                            <SelectionModel>
                                <ext:RowSelectionModel ID="rowselection" SingleSelect="true">
                                    <Listeners>
                                        <RowSelect Handler="#{btn_Delete}.enable();#{btn_Modify}.enable();RowIndex = rowIndex;if(TreeCtrl.getSelectionModel().getSelectedNode().parentNode.id != 'root'){#{btn_up}.enable();#{btn_down}.enable();}if(#{Store1}.getCount() - 1  == rowIndex && TreeCtrl.getSelectionModel().getSelectedNode().parentNode.id != 'root'){#{btn_down}.disable();}if(rowIndex  == 0 && TreeCtrl.getSelectionModel().getSelectedNode().parentNode.id != 'root'){#{btn_up}.disable();}" />
                                        <RowDeselect Handler="if (!#{GridPanel_Show}.hasSelection()) {#{btn_Delete}.disable();#{btn_Modify}.disable();RowIndex = -1;#{btn_up}.disable();#{btn_down}.disable();}" />
                                    </Listeners>
                                </ext:RowSelectionModel>
                            </SelectionModel>
                            <LoadMask ShowMask="true" />
                            <Listeners>
                                <Command Handler="getBtnName(command)" />
                            </Listeners>
                            <AjaxEvents>
                                <Command OnEvent="ShowDetail">
                                    <ExtraParams>
                                        <ext:Parameter Name="Values" Value="Ext.encode(#{GridPanel_Show}.getRowsValues())"
                                            Mode="Raw">
                                        </ext:Parameter>
                                    </ExtraParams>
                                </Command>
                            </AjaxEvents>
                        </ext:GridPanel>
                    </Center>
                </ext:BorderLayout>
            </Body>
        </ext:ViewPort>
    </div>
    <ext:Window ID="arcEditWindow" runat="server" Icon="Group" Title="设置报表" Width="775"
        Height="385" AutoShow="false" Modal="true" CenterOnLoad="true" AutoScroll="true"
        ShowOnLoad="false" Resizable="false" StyleSpec="background-color:Transparent;"
        BodyStyle="background-color:Transparent;">
        <AjaxEvents>
            <Hide OnEvent="WindowsCloseSession">
            </Hide>
        </AjaxEvents>
    </ext:Window>
    <ext:Window ID="ViewWindow" runat="server" Icon="Group" Title="报表预览" Width="775"
        Height="385" AutoShow="false" Modal="true" CenterOnLoad="true" AutoScroll="true"
        ShowOnLoad="false" Resizable="false" StyleSpec="background-color:Transparent;"
        BodyStyle="background-color:Transparent;" Maximizable="true">
    </ext:Window>
    </form>
</body>
</html>
