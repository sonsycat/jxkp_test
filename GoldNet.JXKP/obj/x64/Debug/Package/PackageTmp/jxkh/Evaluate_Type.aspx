<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Evaluate_Type.aspx.cs" Inherits="GoldNet.JXKP.jxkh.Evaluate_Type" %>
<%@ Register Assembly="Goldnet.Ext.Web" Namespace="Goldnet.Ext.Web" TagPrefix="ext" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>评价归档类别</title>
    <style type="text/css">
    .x-grid3-cell-inner{
        border-right: 1px solid #eceff6;
    }
    </style>
    <script type="text/javascript">

        var typeTargetRenderer = function(value) {
            var r = StoreCombo.getById(value);
            if (Ext.isEmpty(r)) {
                return "";
            }
            return r.data.STATION_TYPE_NAME;
        };
        function addRecord(grid) {
            var rowindex = grid.addRecord({ EVALUATE_CLASS_CODE: getMaxid(grid) });
            grid.getView().focusRow(rowindex);
            grid.getSelectionModel().selectRow(rowindex);
            grid.startEditing(rowindex, 1);
            updateButtonState(1);
        }
        function getMaxid(grid) {
            var rs = grid.getStore();
            var maxid = 0;
            for (var i = 0; i < rs.data.length; i++) {
                if (rs.data.items[i].data.EVALUATE_CLASS_CODE > maxid) maxid = rs.data.items[i].data.EVALUATE_CLASS_CODE;
            }
            return parseInt(maxid) + 1;
        }
        function editRecord(grid) {
            var rowindex = 0;
            if (grid.hasSelection()) {
                rowindex = grid.getSelectionModel().last;
            }
            grid.getView().focusRow(rowindex);
            grid.getSelectionModel().selectRow(rowindex);
            grid.startEditing(rowindex, 1);
            updateButtonState(1);

        }
        function cancelRecord(grid) {
            if (grid.isDirty()) {
                Ext.Msg.confirm("系统提示", "数据已改变，是否要取消？",
                      function(button, text) {
                          if (button == 'yes') {
                              grid.reload();
                              updateButtonState(0);
                              return true;
                          } else {
                              editRecord(grid);
                              return false;
                          }
                      });                
            }else {
                grid.reload();
                updateButtonState(0);
            }
        }

        function updateButtonState(op) {
            if (op == 0) {
                Btn_Add.setDisabled(false);
                var tmpflg = GridPanel_List.hasSelection()?false:true;
                Btn_Edit.setDisabled(tmpflg);
                Btn_Del.setDisabled(tmpflg);
                Btn_Refresh.setDisabled(false);
                Btn_Save.setDisabled(true);
                Btn_Cancel.setDisabled(true);            
            }
            if (op == 1) {
                Btn_Add.setDisabled(true);
                Btn_Edit.setDisabled(true);
                Btn_Del.setDisabled(true);
                Btn_Refresh.setDisabled(true);
                Btn_Save.setDisabled(false);
                Btn_Cancel.setDisabled(false);
            }
        }
    </script>    
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <ext:ScriptManager ID="ScriptManager1" runat="server"  />
        <ext:Store runat="server" ID="Store1"  AutoLoad="true"
             WarningOnDirty = "false"
             OnRefreshData="Store_RefreshData">
            <Reader>
                <ext:JsonReader>
                    <Fields>
                        <ext:RecordField Name="EVALUATE_CLASS_CODE" />
                        <ext:RecordField Name="EVALUATE_CLASS_NAME" />
                        <ext:RecordField Name="STATION_TYPE_CODE" />
                        <ext:RecordField Name="STATION_TYPE_NAME" />
                    </Fields>
                </ext:JsonReader>
            </Reader>
        </ext:Store>
        <ext:Store ID="StoreCombo" runat="server">
            <Reader>
                <ext:JsonReader ReaderID="STATION_TYPE_CODE">
                    <Fields>
                        <ext:RecordField Name="STATION_TYPE_CODE" />
                        <ext:RecordField Name="STATION_TYPE_NAME" />
                    </Fields>
                </ext:JsonReader>
            </Reader>
        </ext:Store>
        <ext:ViewPort ID="ViewPort1" runat="server">
            <Body>
                <ext:ColumnLayout ID="ColumnLayout1" runat="server" Split="true" FitHeight="true">
                    <Columns>
                        <ext:LayoutColumn ColumnWidth="1">
                            <ext:GridPanel ID="GridPanel_List" runat="server" Border="false" 
                            StoreID="Store1" StripeRows="true" TrackMouseOver="true" Height="480"  
                            AutoWidth="true" >
                                <TopBar>
                                    <ext:Toolbar ID="Toolbar1" runat="server" >
                                        <Items>
                                            <ext:ToolbarButton ID="Btn_Add" runat="server" Text="增加" Icon="Add" >
                                                <Listeners>
                                                    <Click Handler="addRecord(#{GridPanel_List});" />
                                                </Listeners>
                                            </ext:ToolbarButton>
                                            <ext:ToolbarSeparator ID="ToolbarSeparator5" runat="server"></ext:ToolbarSeparator>
                                            <ext:ToolbarButton ID="Btn_Edit" runat="server" Text="编辑" Icon="NoteEdit"  Disabled="true" >
                                                <Listeners>
                                                    <Click Handler="editRecord(#{GridPanel_List});" />
                                                </Listeners>
                                            </ext:ToolbarButton>
                                            <ext:ToolbarSeparator ID="ToolbarSeparator2" runat="server"></ext:ToolbarSeparator>
                                            <ext:ToolbarButton ID="Btn_Del" runat="server" Text="删除" Icon="Delete"  Disabled="true" >
                                                <AjaxEvents>
                                                    <Click OnEvent="Btn_Del_Click" >
                                                        <Confirmation Title="系统提示" BeforeConfirm="config.confirmation.message = '确定要删除评价类别 '+GridPanel_List.getSelectionModel().getSelected().data.EVALUATE_CLASS_NAME+' 吗？';"  ConfirmRequest="true" />                                                        
                                                        <ExtraParams>
                                                            <ext:Parameter Name="Values" Value="GridPanel_List.getSelectionModel().getSelected().data.EVALUATE_CLASS_CODE" Mode="Raw">
                                                            </ext:Parameter>
                                                        </ExtraParams>
                                                    </Click>
                                                </AjaxEvents>
                                            </ext:ToolbarButton>
                                            <ext:ToolbarSeparator ID="ToolbarSeparator1" runat="server"></ext:ToolbarSeparator>
                                            <ext:ToolbarButton ID="Btn_Save" runat="server" Text="保存" Icon="Disk"  Disabled="true" >
                                                <AjaxEvents>
                                                    <Click OnEvent="Btn_Save_Click">
                                                        <EventMask Msg="正在保存" ShowMask="true" />
                                                        <ExtraParams>
                                                            <ext:Parameter Name="Values" Value="Ext.encode(#{GridPanel_List}.getRowsValues(false))" Mode="Raw">
                                                            </ext:Parameter>
                                                        </ExtraParams>
                                                    </Click>
                                                </AjaxEvents>
                                            </ext:ToolbarButton>
                                            <ext:ToolbarButton ID="Btn_Cancel" runat="server" Text="取消" Icon="ArrowUndo" Disabled="true">
                                                <Listeners>
                                                    <Click Handler="cancelRecord(#{GridPanel_List});" />
                                                </Listeners>                                            
                                            </ext:ToolbarButton>
                                            <ext:ToolbarSeparator ID="ToolbarSeparator3" runat="server"></ext:ToolbarSeparator>
                                            <ext:ToolbarButton ID="Btn_Refresh" runat="server" Text="刷新" Icon="ArrowRefresh">
                                                <Listeners>
                                                    <Click Handler="#{Store1}.reload();" />
                                                </Listeners>
                                            </ext:ToolbarButton>
                                            <ext:ToolbarSeparator ID="ToolbarSeparator4" runat="server"></ext:ToolbarSeparator>
                                        </Items>
                                    </ext:Toolbar>
                                </TopBar>
                                <ColumnModel ID="ColumnModel1" runat="server">
                                    <Columns>
                                        <ext:Column Header="编号" Width="50"   ColumnID="EVALUATE_CLASS_CODE"  DataIndex="EVALUATE_CLASS_CODE"  MenuDisabled="true"/>
                                        <ext:Column Header="评价分类名称" Width="200"  ColumnID="EVALUATE_CLASS_NAME"  DataIndex="EVALUATE_CLASS_NAME"  MenuDisabled="true"  >
                                        <Editor>
                                            <ext:TextField ID="TextField1" runat="server"  MaxLength="32"/>
                                        </Editor>
                                        </ext:Column>
                                        <ext:Column Header="评价对象"     Width="100"   ColumnID="STATION_TYPE_CODE"    DataIndex="STATION_TYPE_CODE"    MenuDisabled="true">
                                        <Renderer Fn="typeTargetRenderer" />
                                        <Editor>                        
                                            <ext:ComboBox ID="cbDepartment" runat="server"  TriggerAction="All"  DataIndex="STATION_TYPE_CODE"  Editable="false"
                                                StoreID="StoreCombo" DisplayField="STATION_TYPE_NAME" ValueField="STATION_TYPE_CODE" >
                                            </ext:ComboBox>
                                        </Editor>
                                        </ext:Column>
                                    </Columns>
                                </ColumnModel>
                                <SelectionModel>
                                    <ext:RowSelectionModel ID="RowSelectionModel1" SingleSelect="true">
                                        <Listeners>
                                            <SelectionChange Handler="if (#{Btn_Save}.disabled) {var tmpflg=  #{GridPanel_List}.hasSelection()?false:true;   #{Btn_Edit}.setDisabled(tmpflg);  #{Btn_Del}.setDisabled(tmpflg);}" />
                                        </Listeners>
                                    </ext:RowSelectionModel>
                                </SelectionModel>
                                <Listeners>
                                    <CellDblClick   Handler="if (columnIndex != 0) updateButtonState(1);" />
                                    <ValidateEdit Handler="if(e.value==''){ #{GridPanel_List}.startEditing(e.row,e.column); }" />
                                </Listeners>
                                <LoadMask ShowMask="true" />
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
