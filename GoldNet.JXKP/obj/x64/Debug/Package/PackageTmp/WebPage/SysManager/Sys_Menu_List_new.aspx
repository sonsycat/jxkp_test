<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Sys_Menu_List_new.aspx.cs" Inherits="GoldNet.JXKP.WebPage.SysManager.Sys_Menu_List_new" %>
<%@ Register Assembly="Goldnet.Ext.Web" Namespace="Goldnet.Ext.Web" TagPrefix="ext" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
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
    <link rel="stylesheet" type="text/css" href="../../Bonus/Orthers/Cbouns.css" />

    <script type="text/javascript">
         var RefreshData = function(msg) {
         Ext.Msg.show({ title: '提示', msg: msg, icon: 'ext-mb-info', buttons: { ok: true} });
            Store1.reload();
        }
        function refreshDeptTree(tree) {
            tree.el.mask('正在加载...', 'x-loading-mask');
            Goldnet.AjaxMethods.RefreshDeptTree(Ext.encode(GridPanel1.getRowsValues()),{
                success: function(result) {
                    var nodes = eval(result);
                    tree.root.ui.remove();
                    tree.initChildren(nodes);
                    tree.root.render();
                    tree.el.unmask();
                },
                failure: function(msg) {
                    tree.el.unmask();
                    Ext.Msg.alert('Failure', '未能加载数据');
                }

            });
        }
       
        var nodeState = function(node) {
            var box = node.getUI().checkbox;
            if (typeof box == 'undefined') return;
            if (box.checked) {
                return 1;
            } else if (box.indeterminate) {
                return 2;
            } else {
                return 3;
            }
        }
        var siblState = function(node) {
            var state = new Array();
            var firstNode = node.parentNode.firstChild;
            if (!firstNode) {
                return false;
            }
            do {
                state.push(nodeState(firstNode));
                firstNode = firstNode.nextSibling;
            } while (firstNode != null)
            return state;
        }
        var parentState = function(node) {
            var state = siblState(node).join();
            if (state.indexOf("3") == -1 && state.indexOf("2") == -1) {
                return 1;
            } else if (state.indexOf("1") == -1 && state.indexOf("2") == -1) {
                return -1;
            } else {
                return 0;
            }
        }
        var parentChecked = function(node) {
            var parentNode = node.parentNode;
            if (parentNode == null) return false;
            var checkbox = parentNode.getUI().checkbox;
            if (typeof checkbox == 'undefined') return false;
            var check = parentState(node);
            if (check == 1) {
                checkbox.indeterminate = false;
                checkbox.checked = true;
            } else if (check == -1) {
                checkbox.checked = false;
                checkbox.indeterminate = false;
            } else {
                checkbox.checked = false;
                checkbox.indeterminate = true;
            }
            parentChecked(parentNode);
        }

        //选择改变事件
        function ToCheckChange(node, checked) {
            if (checked) {
                node.expand();
                node.eachChild(function(child) {
                    //toggleCheck将嵌套触发checkchange事件
                    child.ui.toggleCheck(checked);
//                    child.ui.checkbox.checked = checked;
//                    child.ui.node.attributes.checked = checked;
                });
                parentChecked(node);
            }
            else {
                node.collapse();
                node.eachChild(function(child) {
                    //toggleCheck将嵌套触发checkchange事件
                    child.ui.toggleCheck(checked);
//                    child.ui.checkbox.checked = checked;
//                    child.ui.node.attributes.checked = checked;
                });
                parentChecked(node);
            }
        }
        function getStaffCheckedNode() {
            var result = "";
            var checkeds = Ext.getCmp('DeptTreePanel').getChecked();
            for (var i = 0; i < checkeds.length; i++) {
                if (checkeds[i].id != 'root' && checkeds[i].id.indexOf('CLASS') <0)
                    result = result + checkeds[i].id + ",";                
            }           
            Goldnet.AjaxMethods.DefaultDept(result,Ext.encode(GridPanel1.getRowsValues()));
        }
        function saveProperty() {
            var c = PropertyGrid1.getStore().data.items.length;
            var str='';
            for (var i = 0; i < c; i++) {
                str = str + PropertyGrid1.getStore().data.items[i].data.name + ':' + PropertyGrid1.getStore().data.items[i].data.value + ',';
            }
             Goldnet.AjaxMethods.Btn_Save(str);
        }
    </script>

</head>
<body>
    <form id="form1" runat="server">
    <ext:ScriptManager ID="ScriptManager1" runat="server" />
    <ext:Store ID="Store1" runat="server" AutoLoad="true" OnRefreshData="Store_RefreshData">
        <Reader>
            <ext:JsonReader ReaderID="APP_MENU_ID">
                <Fields>
                    <ext:RecordField Name="APP_MENU_ID" />
                    <ext:RecordField Name="APP_MENU_NAME" />
                    <ext:RecordField Name="APP_NAME" />
                    <ext:RecordField Name="GROUPTEXT" />
                    <ext:RecordField Name="FUNCTION_ID" />
                    <ext:RecordField Name="MENUID" />
                    <ext:RecordField Name="MODID" />
                    <ext:RecordField Name="TYPE_NAME" />
                    <ext:RecordField Name="ATTR_NAME" />
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
                                        <ext:Button ID="Btn_Add" Text="增加" Icon="Add" runat="server">
                                            <AjaxEvents>
                                                <Click OnEvent="Btn_Add_Click">
                                                </Click>
                                            </AjaxEvents>
                                        </ext:Button>
                                        <ext:Button ID="Btn_Edit" Text="编辑" Icon="NoteEdit" runat="server" Disabled="true">
                                            <AjaxEvents>
                                                <Click OnEvent="Btn_Edit_Click">
                                                    <ExtraParams>
                                                        <ext:Parameter Name="Values" Value="Ext.encode(#{GridPanel1}.getRowsValues())" Mode="Raw">
                                                        </ext:Parameter>
                                                    </ExtraParams>
                                                </Click>
                                            </AjaxEvents>
                                        </ext:Button>
                                        <ext:Button ID="Btn_Del" Text="删除" Icon="Delete" runat="server" Disabled="true">
                                            <AjaxEvents>
                                                <Click OnEvent="Btn_Del_Click">
                                                    <Confirmation BeforeConfirm="config.confirmation.message = '删除菜单会删除关于菜单的所有数据，你确定要删除吗？';"
                                                        Title="系统提示" ConfirmRequest="true" />
                                                    <ExtraParams>
                                                        <ext:Parameter Name="Values" Value="Ext.encode(#{GridPanel1}.getRowsValues())" Mode="Raw">
                                                        </ext:Parameter>
                                                    </ExtraParams>
                                                </Click>
                                            </AjaxEvents>
                                        </ext:Button>
                                        <ext:Button ID="Button1" Text="菜单属性" Icon="NoteEdit" runat="server">
                                            <AjaxEvents>
                                                <Click OnEvent="Btn_Attr_Click">
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
                                    <ext:Column Header="序号" Width="66" Align="Left" Sortable="true" MenuDisabled="true"
                                        ColumnID="APP_MENU_ID" DataIndex="APP_MENU_ID">
                                    </ext:Column>
                                    <ext:Column ColumnID="APP_MENU_NAME" Header="菜单名称" Width="100" Align="left" Sortable="true"
                                        DataIndex="APP_MENU_NAME" MenuDisabled="true" />
                                    <ext:Column ColumnID="TYPE_NAME" Header="<div style='text-align:center;'>菜单类型</div>"
                                        Width="100" Align="Center" Sortable="true" DataIndex="TYPE_NAME" MenuDisabled="true" />
                                    <ext:Column ColumnID="APP_NAME" Header="<div style='text-align:center;'>模块名称</div>"
                                        Width="100" Align="Center" Sortable="true" DataIndex="APP_NAME" MenuDisabled="true" />
                                    <ext:Column ColumnID="GROUPTEXT" Header="<div style='text-align:center;'>模块分类</div>"
                                        Width="100" Align="Center" Sortable="true" DataIndex="GROUPTEXT" MenuDisabled="true" />
                                    <ext:Column ColumnID="ATTR_NAME" Header="<div style='text-align:center;'>菜单属性</div>"
                                        Width="100" Align="Center" Sortable="true" DataIndex="ATTR_NAME" MenuDisabled="true" />
                                    <ext:Column Header="functionid" Width="66" Align="Left" Sortable="true" MenuDisabled="true"
                                        ColumnID="FUNCTION_ID" DataIndex="FUNCTION_ID" Hidden="true">
                                    </ext:Column>
                                    <ext:Column Header="menuid" Width="66" Align="Left" Sortable="true" MenuDisabled="true"
                                        ColumnID="MENUID" DataIndex="MENUID" Hidden="true">
                                    </ext:Column>
                                    <ext:Column Header="menuid" Width="66" Align="Left" Sortable="true" MenuDisabled="true"
                                        ColumnID="MODID" DataIndex="MODID" Hidden="true">
                                    </ext:Column>
                                    <ext:CommandColumn Width="80" Align="Center" Header="<div style='text-align:center;'>编辑科室</div>">
                                        <Commands>
                                            <ext:GridCommand Icon="Outline" CommandName="DefaultDept" ToolTip-Text="请选择适用科室">
                                            </ext:GridCommand>
                                        </Commands>
                                    </ext:CommandColumn>
                                    <ext:CommandColumn Width="80" Align="Center" Header="<div style='text-align:center;'>编辑字段</div>">
                                        <Commands>
                                            <ext:GridCommand Icon="Outline" CommandName="APPLYCOST" ToolTip-Text="请添加适用的项目">
                                            </ext:GridCommand>
                                        </Commands>
                                    </ext:CommandColumn>
                                    <ext:CommandColumn Width="80" Align="Center" Header="<div style='text-align:center;'>查询指标</div>">
                                        <Commands>
                                            <ext:GridCommand Icon="Outline" CommandName="APPLYGUIDE" ToolTip-Text="请添加适用的指标">
                                            </ext:GridCommand>
                                        </Commands>
                                    </ext:CommandColumn>
                                    <ext:CommandColumn Width="80" Align="Center" Header="<div style='text-align:center;'>指标关联</div>">
                                        <Commands>
                                            <ext:GridCommand Icon="Outline" CommandName="APPLYGUIDELink" ToolTip-Text="请添加适用的指标">
                                            </ext:GridCommand>
                                        </Commands>
                                    </ext:CommandColumn>
                                </Columns>
                            </ColumnModel>
                            <SelectionModel>
                                <ext:RowSelectionModel ID="RowSelectionModel1" runat="server" SingleSelect="true">
                                    <Listeners>
                                        <RowSelect Handler="#{Btn_Edit}.enable();#{Btn_Del}.enable()" />
                                        <RowDeselect Handler="if (!#{GridPanel1}.hasSelection()) {#{Btn_Del}.disable();#{Btn_Edit}.disable()}" />
                                    </Listeners>
                                </ext:RowSelectionModel>
                            </SelectionModel>
                            <Listeners>
                                <Command Handler="if(command=='APPLYGUIDE'){Goldnet.AjaxMethods.SetGuide(record.data.APP_MENU_ID)} else if(command=='APPLYGUIDELink'){Goldnet.AjaxMethods.SetGuidedetail(record.data.APP_MENU_ID)} else if(command=='APPLYCOST'){Goldnet.AjaxMethods.SetCost(record.data.APP_MENU_ID)} else{ Goldnet.AjaxMethods.SetDept(record.data.APP_MENU_ID)}" />
                            </Listeners>
                            <View>
                                <ext:GroupingView ID="GroupingView1" HideGroupedColumn="true" runat="server" GroupTextTpl='{text} ({[values.rs.length]})'
                                    EnableRowBody="false">
                                </ext:GroupingView>
                            </View>
                        </ext:GridPanel>
                    </ext:LayoutColumn>
                </Columns>
            </ext:ColumnLayout>
        </Body>
    </ext:ViewPort>
    <ext:Window ID="CostWin" runat="server" Icon="Group" Title="编辑" Width="600" Height="400"
        AutoShow="false" Modal="true" CenterOnLoad="true" AutoScroll="true" ShowOnLoad="false"
        Resizable="true" StyleSpec="background-color:Transparent;" BodyStyle="background-color:Transparent;">
    </ext:Window>
    <ext:Window ID="DeptWin" runat="server" Icon="Group" Title="选择科室" Width="550" Height="400"
        AutoShow="false" Modal="true" CenterOnLoad="true" AutoScroll="true" ShowOnLoad="false"
        Resizable="true" StyleSpec="background-color:Transparent;" BodyStyle="background-color:Transparent;">
    </ext:Window>
    <ext:Window ID="MenuGuide" runat="server" Icon="Group" Title="选择指标" Width="800" Height="390"
        AutoShow="false" Modal="true" CenterOnLoad="true" AutoScroll="true" ShowOnLoad="false"
        Resizable="true" StyleSpec="background-color:Transparent;" BodyStyle="background-color:Transparent;">
    </ext:Window>
    <ext:Window ID="Menudict" runat="server" Icon="Group" Title="菜单编辑" Width="400" Height="300"
        AutoShow="false" Modal="true" CenterOnLoad="true" AutoScroll="true" ShowOnLoad="false"
        Resizable="true" StyleSpec="background-color:Transparent;" BodyStyle="background-color:Transparent;">
    </ext:Window>
    <ext:Window ID="Menuattr" runat="server" Icon="Group" Title="菜单属性" Width="300" Height="300"
        AutoShow="false" Modal="true" CenterOnLoad="true" AutoScroll="true" ShowOnLoad="false"
        Resizable="true" StyleSpec="background-color:Transparent;" BodyStyle="background-color:Transparent;">
    </ext:Window>
    </form>
</body>
</html>
