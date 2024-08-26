<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Disease_Analyse.aspx.cs"
    Inherits="GoldNet.JXKP.GuideLook.Disease_Analyse" %>

<%@ Register Assembly="Goldnet.Ext.Web" Namespace="Goldnet.Ext.Web" TagPrefix="ext" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>

    <script type="text/javascript"> 
        function getTreeNodeNameUrl(item,type) {
            var node = TreeCtrl.getSelectionModel().getSelectedNode();
            if(!node.hasChildNodes()) {
                document.getElementById('GridPanel_IFrame').contentWindow.refreshCharts(item,type);
            }
        }
        function refreshTree(tree) {
            tree.el.mask('正在加载...', 'x-loading-mask');
            Goldnet.AjaxMethods.RefreshMenu({
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
        
    </script>

</head>
<body>
    <form id="form1" runat="server">
    <ext:ScriptManager ID="ScriptManager1" runat="server">
        <Listeners>
            <DocumentReady Handler="refreshTree(#{TreeCtrl});" />
        </Listeners>
    </ext:ScriptManager>
    <div>
        <ext:ViewPort ID="ViewPort1" runat="server">
            <Body>
                <ext:BorderLayout ID="BorderLayout2" runat="server">
                    <North>
                        <ext:Toolbar runat="server" ID="ctl155" StyleSpec="border:0">
                            <Items>
                                <ext:Label runat="server" Text="病种类别：">
                                </ext:Label>
                                <ext:ComboBox ID="diseasetype" runat="server" ReadOnly="true">
                                    <Items>
                                        <ext:ListItem Text="手术病种" Value="DEPT_OPERATION_DICT" />
                                        <ext:ListItem Text="非手术病种" Value="DEPT_DISEASE_DICT" />
                                    </Items>
                                    <Listeners>
                                        <Select Handler="refreshTree(#{TreeCtrl})" />
                                    </Listeners>
                                </ext:ComboBox>
                                <ext:ToolbarSeparator ID="ToolbarSeparator4" runat="server" />
                            </Items>
                        </ext:Toolbar>
                    </North>
                    <West Collapsible="false" Split="false" CollapseMode="Mini">
                        <ext:Panel ID="Panel2" runat="server" Width="200" BodyBorder="false" Title="选择病例"
                            AutoScroll="true" Border="false">
                            <Body>
                                <ext:TreePanel runat="server" Width="200" ID="TreeCtrl" AutoHeight="true" AutoScroll="false"
                                    Border="false">
                                    <Root>
                                        <ext:TreeNode NodeID="root" Text="病种">
                                        </ext:TreeNode>
                                    </Root>
                                    <Listeners>
                                        <BeforeClick Handler="node.select();" />
                                        <Click Handler="getTreeNodeNameUrl(#{TreeCtrl}.getSelectionModel().getSelectedNode().text,#{diseasetype}.getSelectedItem().value)" />
                                    </Listeners>
                                </ext:TreePanel>
                            </Body>
                        </ext:Panel>
                    </West>
                    <Center>
                        <ext:Panel ID="GridPanel" runat="server" Border="false" StripeRows="true" Title="病种分析图">
                            <AutoLoad Url="DiseaseAnalyseHightCharts.aspx" Mode="IFrame">
                            </AutoLoad>
                        </ext:Panel>
                    </Center>
                </ext:BorderLayout>
            </Body>
        </ext:ViewPort>
    </div>
    </form>
</body>
</html>
