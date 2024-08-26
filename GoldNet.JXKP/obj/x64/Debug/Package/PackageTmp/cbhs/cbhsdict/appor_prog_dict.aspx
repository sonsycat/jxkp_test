<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="appor_prog_dict.aspx.cs"
    Inherits="GoldNet.JXKP.appor_prog_dict" %>

<%@ Register Assembly="Goldnet.Ext.Web" Namespace="Goldnet.Ext.Web" TagPrefix="ext" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
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
    <ext:Store ID="Store1" runat="server" AutoLoad="true" GroupField="APPORTION_NAME"
        OnRefreshData="Store_RefreshData">
        <Reader>
            <ext:JsonReader ReaderID="ID">
                <Fields>
                    <ext:RecordField Name="ID" />
                    <ext:RecordField Name="PROG_CODE" />
                    <ext:RecordField Name="PROG_NAME" />
                    <ext:RecordField Name="INPUT_CODE" />
                    <ext:RecordField Name="PROG_EXPRESS" />
                    <ext:RecordField Name="APPORTION_NAME" />
                    <ext:RecordField Name="FLAGS" />
                    <ext:RecordField Name="APPLY_DEPT" />
                    <ext:RecordField Name="PROG_MEMO" />
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
                                                    <Confirmation BeforeConfirm="config.confirmation.message = '你确定要删除吗？';" Title="系统提示"
                                                        ConfirmRequest="true" />
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
                                    <ext:Column ColumnID="PROG_CODE" Header="<div style='text-align:center;'>方案代码</div>"
                                        Width="100" Align="left" Sortable="true" DataIndex="PROG_CODE" MenuDisabled="true" />
                                    <ext:Column ColumnID="PROG_NAME" Header="<div style='text-align:center;'>方案名称</div>"
                                        Width="100" Align="left" Sortable="true" DataIndex="PROG_NAME" MenuDisabled="true" />
                                    <ext:Column ColumnID="APPORTION_NAME" Header="方案类别" Width="100" Align="left" Sortable="true"
                                        DataIndex="APPORTION_NAME" MenuDisabled="true" />
                                    <ext:Column ColumnID="INPUT_CODE" Header="<div style='text-align:center;'>输入码</div>"
                                        Width="100" Align="left" Sortable="true" DataIndex="INPUT_CODE" MenuDisabled="true" />
                                    <ext:Column ColumnID="PROG_EXPRESS" Header="<div style='text-align:center;'>方案比例</div>"
                                        Width="340" DataIndex="PROG_EXPRESS" Align="Left" RightCommandAlign="True">
                                        <Commands>
                                            <ext:ImageCommand CommandName="PEXPRESS" Icon="NoteEdit" Text="选择">
                                            </ext:ImageCommand>
                                        </Commands>
                                    </ext:Column>
                                    <ext:CommandColumn Width="80" Align="Center" Header="<div style='text-align:center;'>选择科室</div>">
                                        <Commands>
                                            <ext:GridCommand Icon="Outline" CommandName="DefaultDept" ToolTip-Text="请选择适用科室">
                                            </ext:GridCommand>
                                        </Commands>
                                    </ext:CommandColumn>
                                    <ext:CommandColumn Width="80" Align="Center" Header="<div style='text-align:center;'>选择项目</div>">
                                        <Commands>
                                            <ext:GridCommand Icon="Outline" CommandName="APPLYCOST" ToolTip-Text="请选择适用成本项目">
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
                                <Command Handler="if(command=='PEXPRESS'){ Goldnet.AjaxMethods.SQLExpressShow(record.data.ID,record.data.FLAGS)}else if(command=='APPLYCOST'){Goldnet.AjaxMethods.SetCost(record.data.PROG_CODE)} else{ Goldnet.AjaxMethods.SetDept(record.data.PROG_CODE)}" />
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
    <ext:Window ID="CostWin" runat="server" Icon="Group" Title="选择成本项目" Width="520" Height="400"
        AutoShow="false" Modal="true" CenterOnLoad="true" AutoScroll="true" ShowOnLoad="false"
        Resizable="true" StyleSpec="background-color:Transparent;" BodyStyle="background-color:Transparent;">
    </ext:Window>
    <ext:Window ID="DeptWin" runat="server" Icon="Group" Title="选择科室" Width="550" Height="400"
        AutoShow="false" Modal="true" CenterOnLoad="true" AutoScroll="true" ShowOnLoad="false"
        Resizable="true" StyleSpec="background-color:Transparent;" BodyStyle="background-color:Transparent;">
    </ext:Window>
    <ext:Window ID="DetailWin" runat="server" Icon="Group" Title="成本分摊方案设置" Width="330"
        Height="230" AutoShow="false" Modal="true" CenterOnLoad="true" AutoScroll="true"
        ShowOnLoad="false" Resizable="false" StyleSpec="background-color:Transparent;"
        BodyStyle="background-color:Transparent;">
    </ext:Window>
    <ext:Window ID="DictWin" runat="server" Icon="Group" Title="成本比例设置" Width="300" Height="300"
        Border="false" AutoShow="false" Modal="true" CenterOnLoad="true" ShowOnLoad="false"
        Resizable="false" StyleSpec="background-color:Transparent;" BodyStyle="background-color:Transparent;">
        <Body>
            <ext:PropertyGrid ID="PropertyGrid1" runat="server" AutoWidth="true" AutoScroll="true"
                Height="270">
                <Source>
                </Source>
                <Buttons>
                    <ext:Button runat="server" ID="btn_Save" Text="保存" Icon="Disk">
                        <Listeners>
                            <Click Fn="saveProperty" />
                        </Listeners>
                    </ext:Button>
                    <ext:Button runat="server" ID="btn_Canel" Text="取消" Icon="Cancel">
                        <Listeners>
                            <Click Handler="DictWin.hide();" />
                        </Listeners>
                    </ext:Button>
                </Buttons>
            </ext:PropertyGrid>
        </Body>
    </ext:Window>
    <ext:Window ID="DefaultDeptWin" runat="server" Icon="Group" Title="适用科室设置" Width="260"
        Height="383" AutoShow="false" Modal="true" CenterOnLoad="true" AutoScroll="true"
        ShowOnLoad="false" Resizable="false" StyleSpec="background-color:Transparent;"
        BodyStyle="background-color:Transparent;">
        <Body>
            <ext:Panel runat="server" Border="false">
                <Body>
                    <ext:TreePanel runat="server" ID="DeptTreePanel" BodyBorder="false" AutoWidth="true"
                        Height="350" Icon="BookOpen" AutoScroll="true" RootVisible="false" UseArrows="true">
                        <TopBar>
                            <ext:Toolbar ID="Toolbar2" runat="server">
                                <Items>
                                    <ext:ToolbarButton ID="ToolbarButton55" runat="server" IconCls="icon-expand-all">
                                        <Listeners>
                                            <Click Handler="#{DeptTreePanel}.root.expand(true);" />
                                        </Listeners>
                                        <ToolTips>
                                            <ext:ToolTip ID="ToolTip77" IDMode="Ignore" runat="server" Html="全部展开" />
                                        </ToolTips>
                                    </ext:ToolbarButton>
                                    <ext:ToolbarButton ID="ToolbarButton66" runat="server" IconCls="icon-collapse-all">
                                        <Listeners>
                                            <Click Handler="#{DeptTreePanel}.root.collapse(true);" />
                                        </Listeners>
                                        <ToolTips>
                                            <ext:ToolTip ID="ToolTip8" IDMode="Ignore" runat="server" Html="全部收起" />
                                        </ToolTips>
                                    </ext:ToolbarButton>
                                </Items>
                            </ext:Toolbar>
                        </TopBar>
                        <Root>
                            <ext:TreeNode NodeID="root" Text="科室部门">
                            </ext:TreeNode>
                        </Root>
                        <Listeners>
                            <CheckChange Handler="ToCheckChange(node,checked);" />
                        </Listeners>
                        <BottomBar>
                            <ext:Toolbar runat="server">
                                <Items>
                                    <ext:ToolbarFill ID="ToolbarFill11" runat="server">
                                    </ext:ToolbarFill>
                                    <ext:Button runat="server" ID="btn_OK" Text="确定" Icon="Disk">
                                        <Listeners>
                                            <Click Fn="getStaffCheckedNode" />
                                        </Listeners>
                                    </ext:Button>
                                    <ext:Button runat="server" ID="btn_Cancel" Text="取消" Icon="Cancel">
                                        <Listeners>
                                            <Click Handler="DefaultDeptWin.hide();" />
                                        </Listeners>
                                    </ext:Button>
                                </Items>
                            </ext:Toolbar>
                        </BottomBar>
                    </ext:TreePanel>
                </Body>
            </ext:Panel>
        </Body>
    </ext:Window>
    </form>
</body>
</html>
