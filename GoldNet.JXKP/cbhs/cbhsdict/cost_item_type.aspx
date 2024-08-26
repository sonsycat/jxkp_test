<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="cost_item_type.aspx.cs" Inherits="GoldNet.JXKP.cost_item_type" %>
<%@ Register Assembly="Goldnet.Ext.Web" Namespace="Goldnet.Ext.Web" TagPrefix="ext" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>报表类别设置</title>
    <script type="text/javascript">
        /*
            树节点拖拽事件
            拖拽结束之后，选中源节点，调用TreeNodeSelected()方法
            更新右键菜单及顶部工具条按扭[能/禁止]状态
            根据树节点顺序，更新数据库中节点的排序
        */
        function TreeDragDropHandler(t, node, dd, e) {
            node.select();
            TreeNodeSelected();
            updateTreeNodes();
        }

        /*
            树节点拖拽放置前事件
            在完成拖拽动作之前，判断该次拖拽是否有效动作
            拖拽排序只允许兄弟节点之间改变位置顺序，不能从改变父亲节点
        */
        function TreeBeforeNodeDrop(e) {
            var type = e.point;
            if (type == "append") {
                return false;
            }
            var curTree = e.tree;
            var dropNode = e.dropNode;
            var targetNode = e.target; 
            if (dropNode.parentNode.id != targetNode.parentNode.id) {
                return false;
            }
        }

        /*
            右键点击树显示菜单事件
            只在树节点附近300像素有效范围内显示菜单
            显示菜单之前，选中该节点，更新菜单及按扭状态
        */        
        function TreeContextMenu(node, e) {
            var position = e.getPoint();
            if (position[0] <= 300) {
                node.select();
                TreeNodeSelected();
                treeMenu.node=node;
                treeMenu.showAt(position);
            }
        }

        /*
            节点选中方法，在改变选中节点时调用本方法
            支持鼠标点击，键盘上下左右键导航
            根据选中的节点，更新菜单及按钮的状态
        */
        function TreeNodeSelected() {
            if (TreeCtrl.getSelectionModel().getSelectedNode() != null) {
                var selNode = TreeCtrl.getSelectionModel().getSelectedNode();
                UpdateMenuButton(selNode);
            }
        }

        /*
            右键菜单及按钮状态更新方法
            传入参数为选中的树节点
        */
        function UpdateMenuButton(node) {
            //根节点
            var selNodeId = node.id;
            if (selNodeId == "0") {
                btn_Add.setDisabled(false);
                btn_Del.setDisabled(true);
                btn_Edit.setDisabled(true);
                return;
            }
            btn_Edit.setDisabled(false);
            btn_Del.setDisabled(false);
            //二级节点
            var pNodeId = node.parentNode.id;
            if (pNodeId == "0") {
                btn_Add.setDisabled(false);
                if (node.hasChildNodes()) {
                    btn_Del.setDisabled(true); 
                }

            } else {
                btn_Add.setDisabled(true);
            }
        }

        /*
            树节点操作
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
                Ext.Msg.prompt("添加子类别", "请输入子类别名称：", function(btn, text) { nodeOpCallback(btn, text, optype, selNode) });
            } else if (optype == "2") {
                Ext.Msg.prompt("重命名子类别", "请输入子类别名称：", function(btn, text) { nodeOpCallback(btn, text, optype, selNode) }, true, false, selNode.text);
            } else if (optype == "3") {
                Ext.Msg.confirm("删除类别", "确定要删除该类别：" + selNode.text + " 吗？", function(btn, text) { nodeOpCallback(btn, text, optype, selNode) });
            } else if ((optype == "4")|| (optype == "5"))  {
                var pNode = selNode.parentNode;
                var selNodeTarget = (optype == "4" ? selNode.previousSibling : selNode.nextSibling);
                if (optype == "4") {
                    pNode.insertBefore(selNode, selNodeTarget);
                } else {
                    pNode.insertBefore(selNodeTarget, selNode);
                }
                selNode.select();
                TreeNodeSelected();
                updateTreeNodes();
            }
        }

        /*
            根据传入的树节点克隆新的树节点（包含该节点的子节点）
        */
        function cloneNodes(node) {
            var atts = node.attributes;
            atts.id = Ext.id();
            var clonedNode = new Ext.tree.TreeNode(Ext.apply({}, atts));
            clonedNode.text = node.text;
            clonedNode.id = node.id;
            for (var i = 0; i < node.childNodes.length; i++) {
                clonedNode.appendChild(cloneNodes(node.childNodes[i]));
            }
            return clonedNode;
        }
        /*
            节点增删改操作回调函数
        */
        function nodeOpCallback (btn, text, optype, node) {
            if ((btn != "ok") && (btn != "yes")) {
                return;
            }
            if (optype == "1") {
                if (node.childNodes.length > 26) {
                    Ext.Msg.show({ title: '系统错误', msg: '此节点已饱和', icon: 'ext-mb-warning', buttons: { ok: true} });
                    return;
                }
                //创建新节点，在该节点的最后位置追加
                var atts = node.attributes;
                atts.id = Ext.id();
                var clonedMaxNode = new Ext.tree.TreeNode(Ext.apply({}, atts));
                clonedMaxNode.text = "clonetemp";
                clonedMaxNode.id = "0";
                clonedMaxNode = getMaxTreeNode(clonedMaxNode, TreeCtrl.root);
                var clonedNode = new Ext.tree.TreeNode(Ext.apply({}, atts));
                clonedNode.text = text;
                var cloneid = getTypeCode(node);
                clonedNode.id = cloneid.toString();
                node.appendChild(clonedNode);
                updateTreeNodes();
                clonedNode.select();
                TreeNodeSelected();
                
            } else if (optype == "2") {
                //当名称发生改变时，更新该节点
                if (node.text != text) {
                    node.setText(text);
                    updateTreeNodes();
                }
            } else if (optype == "3") {
            //检查是否类别下已经存在报表，如果不存在，则可以删除
                Goldnet.AjaxMethods.DelTreeNode(node.id, {
                    success: function(result) {
                        if (result == "") {
                            var pNode = node.parentNode;
                            pNode.select();
                            TreeNodeSelected();
                            node.remove();
                        } else {
                            Ext.Msg.show({ title: '系统提示', msg:  result,icon: 'ext-mb-info',  buttons: { ok: true }  });
                        }
                    },
                    failure: function(msg) {
                        Ext.Msg.show({ title: '系统错误', msg: '未能更新数据 ' + msg, icon: 'ext-mb-warning', buttons: { ok: true } });
                    }
                });
            }
            
        }
        var getTypeCode = function(parentNode) {
            var myword = new Array();
            myword[0] = "A";
            myword[1] = "B";
            myword[2] = "C";
            myword[3] = "D";
            myword[4] = "E";
            myword[5] = "F";
            myword[6] = "G";
            myword[7] = "H";
            myword[8] = "I";
            myword[9] = "J";
            myword[10] = "K";
            myword[11] = "L";
            myword[12] = "M";
            myword[13] = "N";
            myword[14] = "O";
            myword[15] = "P";
            myword[16] = "Q";
            myword[17] = "R";
            myword[18] = "S";
            myword[19] = "T";
            myword[20] = "U";
            myword[21] = "V";
            myword[22] = "W";
            myword[23] = "X";
            myword[24] = "Y";
            myword[25] = "Z";
            
            var childcount=parentNode.childNodes.length;
            if (parentNode.parentNode==null) {
                if (childcount > 0) {
                    var id = parentNode.childNodes[0].id;
                    var index = myword.indexOf(id)
                    if (index == 0) {
                        for (i = 0; i < childcount; i++) {
                            var cid = parentNode.childNodes[i].id;
                            var wid = myword[i];
                            if (cid != wid) {
                                return wid;
                            }
                        }
                        return myword[childcount];
                    }
                    else {
                        return myword[0];
                    }
                }
                else {
                    return myword[childcount];
                }
            }
            else {
              return parentNode.id+ myword[childcount];
            }
        }
        /*
            遍历树取nodeid最大值的节点
        */
        var getMaxTreeNode = function(nodemax, node) {
            var maxNode = nodemax;
            if (parseInt(node.id) > parseInt(maxNode.id)) {
                maxNode = node;
            }
            for (var i = 0; i < node.childNodes.length; i++) {
                if (parseInt(node.childNodes[i].id) > parseInt(maxNode.id)) {
                    maxNode = node.childNodes[i];
                }
                if (node.childNodes[i].childNodes.length >0 ) {
                    maxNode = getMaxTreeNode(maxNode,node.childNodes[i]);
                }
            }
            return maxNode;
        }

        /*
            遍历树节点JSON
        */
        var getJsonTree = function(node) {
            var str = '';
            for (var i = 0; i < node.childNodes.length; i++) {
                str = str + '{ "Id": "' + node.childNodes[i].id + '"';
                str = str + ', "Text": "' + node.childNodes[i].text + '"';
                str = str + ', "Pid": "' + node.id + '"}';
                if (node.childNodes[i].childNodes.length > 0) {
                    str = str + ',  ' + getJsonTree(node.childNodes[i]);
                }
                if (i < (node.childNodes.length - 1)) {
                    str = str + ',';
                }
            }
            return str;
        }   

        /*
            异步更新树
        */
        function updateTreeNodes() {
            var treeNodesJSON = '['+getJsonTree(TreeCtrl.root)+']';
            Goldnet.AjaxMethods.UpdateTree(treeNodesJSON,{
                failure: function(msg) {
                    Ext.Msg.show({ title: '系统错误', msg: '未能更新数据 '+msg, icon: 'ext-mb-warning', buttons: { ok: true }  });
                }
            });
        }        
        
    </script>
    <style type="text/css">
        .icon-expand-all  { background-image: url(/resources/images/expand-all.gif) !important; }
        .icon-collapse-all  { background-image: url(/resources/images/collapse-all.gif) !important;         
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <ext:ScriptManager ID="ScriptManager1" runat="server" >       
    </ext:ScriptManager>
    <div>
        <ext:ViewPort ID="ViewPort1" runat="server">
            <Body>
                <ext:BorderLayout ID="BorderLayout2" runat="server">
                    <Center>
                        <ext:TreePanel runat="server" Header="false" Width="400" ID="TreeCtrl" UseArrows="true"
                            Border="false"  AutoScroll="true" >
                            <TopBar>
                                <ext:Toolbar runat="server" ID="ctl155">
                                    <Items>
                                        <ext:Button ID="btn_Add" runat="server" Text="添加子类别"  Disabled="false" Icon="Add">
                                            <Listeners>
                                                <Click Handler="TreeOpration(1);" /> 
                                            </Listeners>
                                        </ext:Button>
                                        <ext:Button Text="修改类别名称" ID="btn_Edit" runat="server" Disabled="true"  Icon="FolderEdit">
                                            <Listeners>
                                                <Click Handler="TreeOpration(2);" /> 
                                            </Listeners>
                                        </ext:Button>
                                        <ext:Button Text="删除类别" ID="btn_Del" runat="server" Disabled="true"  Icon="Delete">
                                            <Listeners>
                                                <Click Handler="TreeOpration(3);" /> 
                                            </Listeners>
                                        </ext:Button>
                                    </Items>
                                </ext:Toolbar>
                            </TopBar>
                            <Root>
                            </Root>
                             <Listeners>
                                <BeforeClick Handler="node.select();" />
                                <Click Fn="TreeNodeSelected" />
                            </Listeners>
                            <BottomBar>
                                <ext:Toolbar ID="ToolBar2" runat="server">
                                 <Items>
                                    <ext:ToolbarButton ID="ToolbarButton2" runat="server"  Text="展开树" IconCls="icon-expand-all">
                                        <Listeners>
                                            <Click Handler="#{TreeCtrl}.expandAll(true);" />
                                        </Listeners>
                                        <ToolTips>
                                            <ext:ToolTip ID="ToolTip5" IDMode="Ignore" runat="server" Html="全部展开" />
                                        </ToolTips>
                                    </ext:ToolbarButton>
                                    <ext:ToolbarSeparator ID="ToolbarSeparator1" runat="server"></ext:ToolbarSeparator>
                                    <ext:ToolbarButton ID="ToolbarButton3" runat="server" Text="折叠树"  IconCls="icon-collapse-all">
                                        <Listeners>
                                            <Click Handler="#{TreeCtrl}.collapseAll(true);#{TreeCtrl}.root.select(); TreeNodeSelected();" />
                                        </Listeners>
                                        <ToolTips>
                                            <ext:ToolTip ID="ToolTip6" IDMode="Ignore" runat="server" Html="全部收起" />
                                        </ToolTips>
                                    </ext:ToolbarButton>
                                    <ext:ToolbarFill ID="ToolbarFill2" runat="server"></ext:ToolbarFill>
                                    <ext:ToolbarButton ID="tbn_close" runat="server" Text="关闭" Icon="Cancel">
                                        <Listeners>
                                            <Click Handler="parent.TypeWin.hide();parent.RefreshTypeData();" />
                                        </Listeners>
                                    </ext:ToolbarButton>
                                </Items>
                             </ext:Toolbar>                            
                            </BottomBar>
                        </ext:TreePanel>
                    </Center>
                </ext:BorderLayout>
            </Body>
        </ext:ViewPort>
    </div>    
    </form>
</body>
</html>