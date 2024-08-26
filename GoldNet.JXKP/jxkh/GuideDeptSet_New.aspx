<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="GuideDeptSet_New.aspx.cs"
    Inherits="GoldNet.JXKP.jxkh.GuideDeptSet_New" %>

<%@ Register Assembly="Goldnet.Ext.Web" Namespace="Goldnet.Ext.Web" TagPrefix="ext" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>指标选择</title>
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

    <script type="text/javascript">
        var SelectorLayout = function() {
            //Panel11.setHeight(Ext.lib.Dom.getViewHeight() - Panel11.getPosition()[1]- 10);
            //SelectorRight.setHeight(Ext.lib.Dom.getViewHeight() - SelectorRight.getPosition()[1]- 5);
        }
       
        //指标点击选择后处理
        var TreeNodeSelected = function(node) {
//            if (TreeCtrl.getSelectionModel().getSelectedNode() != null) {
//                //
//                var selNode = TreeCtrl.getSelectionModel().getSelectedNode();
//                if (node.id == "root") return;
//                var pNodeId = selNode.parentNode.id;
                if (node.id != "root" && (node.id.length == 8)) {
                    //指标节点选择后调用节点执行处理函数
                    Goldnet.AjaxMethods.TreeSelectedGuide(node.id, {
                        eventMask: {
                            msg: "请稍候...",
                            showMask: true,
                            minDelay: 500
                        }
                    });
                }
//            }
        }
        
        //指标树刷新处理
        var refreshTree = function(tree) {
            tree.el.mask('正在加载...', 'x-loading-mask');
            Goldnet.AjaxMethods.RefreshTree({
                success: function(result) {
                    var nodes = eval(result);
                    tree.root.ui.remove();
                    tree.initChildren(nodes);
                    tree.root.render();
                    tree.el.unmask();
                },
                failure: function(msg) {
                    tree.el.unmask();
                    Ext.Msg.show({ title: '系统错误', msg: '未能更新数据 ' + msg, icon: 'ext-mb-warning', buttons: { ok: true} });
                }
            });
        };
        
        var ConvertGuideUnit = function(v) {
            if (v == "T") {
                return true;
            } else {
                return false;
            }
        };
        
        var ConvertMinusFlag = function(v) {
            if (v == "1") {
                return true;
            } else {
                return false;
            }
        }
    </script>

</head>
<body>
    <form id="form1" runat="server">
    <ext:ScriptManager ID="ScriptManager1" runat="server">
        <Listeners>
            <DocumentReady Handler=" Ext.EventManager.onWindowResize(SelectorLayout); refreshTree(TreeCtrl);" />
        </Listeners>
    </ext:ScriptManager>
    <ext:Store runat="server" ID="Store1">
        <Reader>
            <ext:JsonReader>
                <Fields>
                    <ext:RecordField Name="ID" />
                    <ext:RecordField Name="GUIDENAME" />
                    <ext:RecordField Name="PID" />
                    <ext:RecordField Name="GUIDETYPE" />
                    <ext:RecordField Name="SHOWWIDTH" />
                    <ext:RecordField Name="SHOWSTYLE" />
                </Fields>
            </ext:JsonReader>
        </Reader>
    </ext:Store>
    <ext:Store runat="server" ID="Store2">
        <Reader>
            <ext:JsonReader>
                <Fields>
                    <ext:RecordField Name="ID" />
                    <ext:RecordField Name="GUIDENAME" />
                    <ext:RecordField Name="GUIDETYPE" />
                    <ext:RecordField Name="SHOWWIDTH" />
                    <ext:RecordField Name="SHOWSTYLE" />
                </Fields>
            </ext:JsonReader>
        </Reader>
    </ext:Store>
    <ext:Store runat="server" ID="Store3">
        <Reader>
            <ext:JsonReader ReaderID="DEPT_CODE">
                <Fields>
                    <ext:RecordField Name="DEPT_CODE" />
                    <ext:RecordField Name="DEPT_NAME" />
                    <ext:RecordField Name="BSCPOINT" />
                    <ext:RecordField Name="GUIDE_VALUE" />
                    <ext:RecordField Name="GUIDE_CAUSE" />
                    <ext:RecordField Name="GUIDE_UNIT">
                        <Convert Fn="ConvertGuideUnit" />
                    </ext:RecordField>
                    <ext:RecordField Name="INCREASE" />
                    <ext:RecordField Name="INCREASE_ARITHMETIC" />
                    <ext:RecordField Name="DECREASE" />
                    <ext:RecordField Name="DECREASE_ARITHMETIC" />
                    <ext:RecordField Name="MINUSFLAG">
                        <Convert Fn="ConvertMinusFlag" />
                    </ext:RecordField>
                    <ext:RecordField Name="PLUSFLAG">
                        <Convert Fn="ConvertMinusFlag" />
                    </ext:RecordField>
                    <ext:RecordField Name="FIXNUM">
                        <Convert Fn="ConvertMinusFlag" />
                    </ext:RecordField>
                    <ext:RecordField Name="PLUS_INCREASE" />
                    <ext:RecordField Name="PLUS_ARITHMETIC" />
                    <ext:RecordField Name="MINUS_INCREASE" />
                    <ext:RecordField Name="MINUS_ARITHMETIC" />
                    <ext:RecordField Name="THRESHOLD_VALUE" />
                    <ext:RecordField Name="PLUS_LIMIT" />
                    <ext:RecordField Name="MINUS_LIMIT" />
                    <ext:RecordField Name="GUIDE_CAUSE1" />
                    <ext:RecordField Name="GUIDE_CAUSE2" />
                    <ext:RecordField Name="GUIDE_CAUSE3" />
                    <ext:RecordField Name="GUIDE_CAUSE4" />
                    <ext:RecordField Name="GUIDE_CAUSE5" />
                    <ext:RecordField Name="GUIDE_CAUSE6" />
                    <ext:RecordField Name="GUIDE_CAUSE7" />
                    <ext:RecordField Name="GUIDE_CAUSE8" />
                    <ext:RecordField Name="GUIDE_CAUSE9" />
                    <ext:RecordField Name="GUIDE_CAUSE10" />
                    <ext:RecordField Name="GUIDE_CAUSE11" />
                    <ext:RecordField Name="GUIDE_CAUSE12" />
                </Fields>
            </ext:JsonReader>
        </Reader>
    </ext:Store>
    <ext:Store ID="Store4" runat="server">
        <Reader>
            <ext:JsonReader ReaderID="YEAR">
                <Fields>
                    <ext:RecordField Name="YEAR" />
                </Fields>
            </ext:JsonReader>
        </Reader>
    </ext:Store>
    <div>
        <ext:ViewPort ID="ViewPort1" runat="server">
            <Body>
                <ext:BorderLayout ID="BorderLayout1" runat="server">
                    <North>
                        <ext:Toolbar ID="Toolbar2" runat="server" AutoShow="true" AutoRender="true">
                            <Items>
                                <ext:Label ID="Label1" runat="server" Text="年度：" />
                                <ext:ComboBox ID="cbbYear" runat="server" ReadOnly="true" StoreID="Store4" Width="70"
                                    DisplayField="YEAR" ValueField="YEAR" ForceSelection="true" SelectOnFocus="true">
                                    <AjaxEvents>
                                        <Select OnEvent="Button_look_click">
                                        </Select>
                                    </AjaxEvents>
                                </ext:ComboBox>
                                <ext:Button ID="save" runat="server" Icon="Disk" Text="保存">
                                    <AjaxEvents>
                                        <Click OnEvent="SaveGuide">
                                            <EventMask Msg="请稍候..." ShowMask="true" />
                                            <ExtraParams>
                                                <ext:Parameter Name="Value1" Value="Ext.encode(#{GridPanel1}.getRowsValues(false))"
                                                    Mode="Raw" />
                                                <ext:Parameter Name="Value2" Value="Ext.encode(#{TreeCtrl}.getSelectionModel().getSelectedNode().id)"
                                                    Mode="Raw" />
                                            </ExtraParams>
                                        </Click>
                                    </AjaxEvents>
                                </ext:Button>
                            </Items>
                        </ext:Toolbar>
                    </North>
                    <West Collapsible="true" MaxWidth="200">
                        <%--指标树区域--%>
                        <ext:TreePanel runat="server" ID="TreeCtrl" AutoScroll="true" BodyBorder="false"
                            Width="200">
                            <Root>
                                <ext:TreeNode NodeID="root" Text="指标体系">
                                </ext:TreeNode>
                            </Root>
                            <BottomBar>
                                <ext:Toolbar ID="ToolBar1" runat="server">
                                    <Items>
                                        <ext:ToolbarTextItem ID="ToolbarTextItem1" runat="server" Text="指标分类列表" />
                                        <ext:ToolbarFill ID="ToolbarFill1" runat="server" />
                                        <ext:ToolbarButton ID="ToolbarButton2" runat="server" IconCls="icon-expand-all">
                                            <Listeners>
                                                <Click Handler="#{TreeCtrl}.expandAll(true);" />
                                            </Listeners>
                                            <ToolTips>
                                                <ext:ToolTip ID="ToolTip5" IDMode="Ignore" runat="server" Html="全部展开" />
                                            </ToolTips>
                                        </ext:ToolbarButton>
                                        <ext:ToolbarButton ID="ToolbarButton3" runat="server" IconCls="icon-collapse-all">
                                            <Listeners>
                                                <Click Handler="#{TreeCtrl}.collapseAll(true);" />
                                            </Listeners>
                                            <ToolTips>
                                                <ext:ToolTip ID="ToolTip6" IDMode="Ignore" runat="server" Html="全部收起" />
                                            </ToolTips>
                                        </ext:ToolbarButton>
                                    </Items>
                                </ext:Toolbar>
                            </BottomBar>
                            <Listeners>
                                <BeforeClick Handler="node.select();" />
                                <Click Handler="TreeNodeSelected(node);" />
                            </Listeners>
                        </ext:TreePanel>
                    </West>
                    <Center>
                        <%--指标设置列表--%>
                        <ext:GridPanel ID="GridPanel1" runat="server" StoreID="Store3" StripeRows="true"
                            ClicksToEdit="1" TrackMouseOver="true" AutoWidth="true" Height="480" Border="true">
                            <ColumnModel runat="server" ID="ColumnModel1">
                                <Columns>
                                    <ext:Column ColumnID="DEPT_NAME" Width="120" Header="科室" DataIndex="DEPT_NAME" />
                                    <ext:Column ColumnID="INCREASE" Width="60" Header="降低量" Sortable="true" DataIndex="INCREASE"
                                        Align="Right">
                                        <Editor>
                                            <ext:NumberField runat="server" ID="INCREASE_TXT" SelectOnFocus="true" AllowNegative="false"
                                                AllowDecimals="true" DecimalPrecision="2">
                                            </ext:NumberField>
                                        </Editor>
                                    </ext:Column>
                                    <ext:Column Width="60" ColumnID="INCREASE_ARITHMETIC" Header="扣分" Sortable="true"
                                        DataIndex="INCREASE_ARITHMETIC" Align="Right">
                                        <Editor>
                                            <ext:NumberField ID="INCREASE_ARITHMETIC_TXT" runat="server" SelectOnFocus="true"
                                                AllowNegative="false" AllowDecimals="true" DecimalPrecision="2">
                                            </ext:NumberField>
                                        </Editor>
                                    </ext:Column>
                                    <ext:Column Width="60" ColumnID="DECREASE" Header="增加量" Sortable="true" DataIndex="DECREASE"
                                        Align="Right">
                                        <Editor>
                                            <ext:NumberField ID="DECREASE_TXT" runat="server" SelectOnFocus="true" AllowNegative="false"
                                                AllowDecimals="true" DecimalPrecision="2">
                                            </ext:NumberField>
                                        </Editor>
                                    </ext:Column>
                                    <ext:Column Width="60" ColumnID="DECREASE_ARITHMETIC" Header="加分" Sortable="false"
                                        DataIndex="DECREASE_ARITHMETIC" Align="Right">
                                        <Editor>
                                            <ext:NumberField ID="DECREASE_ARITHMETIC_TXT" runat="server" SelectOnFocus="true"
                                                AllowNegative="false" AllowDecimals="true" DecimalPrecision="2">
                                            </ext:NumberField>
                                        </Editor>
                                    </ext:Column>
                                    <ext:CheckColumn Width="50" ColumnID="GUIDE_UNIT" Header="单位(%)" Sortable="true"
                                        Editable="true" DataIndex="GUIDE_UNIT">
                                    </ext:CheckColumn>
                                    <%--                                    <ext:Column Width="60" ColumnID="GUIDE_CAUSE" Header="目标值" Sortable="true" DataIndex="GUIDE_CAUSE"
                                        Align="Right">
                                        <Editor>
                                            <ext:NumberField ID="GUIDE_CAUSE_TXT" runat="server" SelectOnFocus="true" AllowNegative="false"
                                                AllowDecimals="true" DecimalPrecision="2">
                                            </ext:NumberField>
                                        </Editor>
                                    </ext:Column>--%>
                                    <ext:Column Width="60" ColumnID="GUIDE_VALUE" Header="指标分值" Sortable="true" DataIndex="GUIDE_VALUE"
                                        Align="Right">
                                        <Editor>
                                            <ext:NumberField ID="GUIDE_VALUE_TXT" runat="server" SelectOnFocus="true" AllowNegative="false"
                                                AllowDecimals="true" DecimalPrecision="2">
                                            </ext:NumberField>
                                        </Editor>
                                    </ext:Column>
                                    <ext:CheckColumn Width="60" ColumnID="PLUSFLAG" Header="允许超分" Sortable="true" Editable="true"
                                        DataIndex="PLUSFLAG">
                                    </ext:CheckColumn>
                                    <ext:CheckColumn Width="60" ColumnID="MINUSFLAG" Header="允许负分" Sortable="true" Editable="true"
                                        DataIndex="MINUSFLAG">
                                    </ext:CheckColumn>
                                    <ext:CheckColumn Width="60" ColumnID="FIXNUM" Header="固定分值" Sortable="true" Editable="true"
                                        DataIndex="FIXNUM">
                                    </ext:CheckColumn>
                                    <ext:Column Width="60" ColumnID="THRESHOLD_VALUE" Header="阀值" Sortable="true" DataIndex="THRESHOLD_VALUE"
                                        Align="Right">
                                        <Editor>
                                            <ext:NumberField ID="NumberField5" runat="server" SelectOnFocus="true" AllowNegative="false"
                                                AllowDecimals="true" DecimalPrecision="2" AutoDataBind="true">
                                            </ext:NumberField>
                                        </Editor>
                                    </ext:Column>
                                    <ext:Column Width="60" ColumnID="PLUS_LIMIT" Header="超分限制" Sortable="false" DataIndex="PLUS_LIMIT"
                                        Align="Right">
                                        <Editor>
                                            <ext:NumberField ID="NumberField6" runat="server" SelectOnFocus="true" AllowNegative="false"
                                                AllowDecimals="true" DecimalPrecision="2">
                                            </ext:NumberField>
                                        </Editor>
                                    </ext:Column>
                                    <ext:Column Width="60" ColumnID="MINUS_LIMIT" Header="减分限制" Sortable="false" DataIndex="MINUS_LIMIT"
                                        Align="Right">
                                        <Editor>
                                            <ext:NumberField ID="NumberField7" runat="server" SelectOnFocus="true" AllowNegative="false"
                                                AllowDecimals="true" DecimalPrecision="2">
                                            </ext:NumberField>
                                        </Editor>
                                    </ext:Column>
                                    <ext:Column Width="80" ColumnID="GUIDE_CAUSE1" Header="1月目标值" Sortable="true" DataIndex="GUIDE_CAUSE1"
                                        Align="Right">
                                        <Editor>
                                            <ext:NumberField ID="GUIDE_CAUSE_TXT1" runat="server" SelectOnFocus="true" AllowNegative="false"
                                                AllowDecimals="true" DecimalPrecision="2">
                                            </ext:NumberField>
                                        </Editor>
                                    </ext:Column>
                                    <ext:Column Width="80" ColumnID="GUIDE_CAUSE2" Header="2月目标值" Sortable="true" DataIndex="GUIDE_CAUSE2"
                                        Align="Right">
                                        <Editor>
                                            <ext:NumberField ID="GUIDE_CAUSE_TXT2" runat="server" SelectOnFocus="true" AllowNegative="false"
                                                AllowDecimals="true" DecimalPrecision="2">
                                            </ext:NumberField>
                                        </Editor>
                                    </ext:Column>
                                    <ext:Column Width="80" ColumnID="GUIDE_CAUSE3" Header="3月目标值" Sortable="true" DataIndex="GUIDE_CAUSE3"
                                        Align="Right">
                                        <Editor>
                                            <ext:NumberField ID="GUIDE_CAUSE_TXT3" runat="server" SelectOnFocus="true" AllowNegative="false"
                                                AllowDecimals="true" DecimalPrecision="2">
                                            </ext:NumberField>
                                        </Editor>
                                    </ext:Column>
                                    <ext:Column Width="80" ColumnID="GUIDE_CAUSE4" Header="4月目标值" Sortable="true" DataIndex="GUIDE_CAUSE4"
                                        Align="Right">
                                        <Editor>
                                            <ext:NumberField ID="GUIDE_CAUSE_TXT4" runat="server" SelectOnFocus="true" AllowNegative="false"
                                                AllowDecimals="true" DecimalPrecision="2">
                                            </ext:NumberField>
                                        </Editor>
                                    </ext:Column>
                                    <ext:Column Width="80" ColumnID="GUIDE_CAUSE5" Header="5月目标值" Sortable="true" DataIndex="GUIDE_CAUSE5"
                                        Align="Right">
                                        <Editor>
                                            <ext:NumberField ID="GUIDE_CAUSE_TXT5" runat="server" SelectOnFocus="true" AllowNegative="false"
                                                AllowDecimals="true" DecimalPrecision="2">
                                            </ext:NumberField>
                                        </Editor>
                                    </ext:Column>
                                    <ext:Column Width="80" ColumnID="GUIDE_CAUSE6" Header="6月目标值" Sortable="true" DataIndex="GUIDE_CAUSE6"
                                        Align="Right">
                                        <Editor>
                                            <ext:NumberField ID="GUIDE_CAUSE_TXT6" runat="server" SelectOnFocus="true" AllowNegative="false"
                                                AllowDecimals="true" DecimalPrecision="2">
                                            </ext:NumberField>
                                        </Editor>
                                    </ext:Column>
                                    <ext:Column Width="80" ColumnID="GUIDE_CAUSE7" Header="7月目标值" Sortable="true" DataIndex="GUIDE_CAUSE7"
                                        Align="Right">
                                        <Editor>
                                            <ext:NumberField ID="GUIDE_CAUSE_TXT7" runat="server" SelectOnFocus="true" AllowNegative="false"
                                                AllowDecimals="true" DecimalPrecision="2">
                                            </ext:NumberField>
                                        </Editor>
                                    </ext:Column>
                                    <ext:Column Width="80" ColumnID="GUIDE_CAUSE8" Header="8月目标值" Sortable="true" DataIndex="GUIDE_CAUSE8"
                                        Align="Right">
                                        <Editor>
                                            <ext:NumberField ID="GUIDE_CAUSE_TXT8" runat="server" SelectOnFocus="true" AllowNegative="false"
                                                AllowDecimals="true" DecimalPrecision="2">
                                            </ext:NumberField>
                                        </Editor>
                                    </ext:Column>
                                    <ext:Column Width="80" ColumnID="GUIDE_CAUSE9" Header="9月目标值" Sortable="true" DataIndex="GUIDE_CAUSE9"
                                        Align="Right">
                                        <Editor>
                                            <ext:NumberField ID="GUIDE_CAUSE_TXT9" runat="server" SelectOnFocus="true" AllowNegative="false"
                                                AllowDecimals="true" DecimalPrecision="2">
                                            </ext:NumberField>
                                        </Editor>
                                    </ext:Column>
                                    <ext:Column Width="80" ColumnID="GUIDE_CAUSE10" Header="10月目标值" Sortable="true" DataIndex="GUIDE_CAUSE10"
                                        Align="Right">
                                        <Editor>
                                            <ext:NumberField ID="GUIDE_CAUSE_TXT10" runat="server" SelectOnFocus="true" AllowNegative="false"
                                                AllowDecimals="true" DecimalPrecision="2">
                                            </ext:NumberField>
                                        </Editor>
                                    </ext:Column>
                                    <ext:Column Width="80" ColumnID="GUIDE_CAUSE11" Header="11月目标值" Sortable="true" DataIndex="GUIDE_CAUSE11"
                                        Align="Right">
                                        <Editor>
                                            <ext:NumberField ID="GUIDE_CAUSE_TXT11" runat="server" SelectOnFocus="true" AllowNegative="false"
                                                AllowDecimals="true" DecimalPrecision="2">
                                            </ext:NumberField>
                                        </Editor>
                                    </ext:Column>
                                    <ext:Column Width="80" ColumnID="GUIDE_CAUSE12" Header="12月目标值" Sortable="true" DataIndex="GUIDE_CAUSE12"
                                        Align="Right">
                                        <Editor>
                                            <ext:NumberField ID="GUIDE_CAUSE_TXT12" runat="server" SelectOnFocus="true" AllowNegative="false"
                                                AllowDecimals="true" DecimalPrecision="2">
                                            </ext:NumberField>
                                        </Editor>
                                    </ext:Column>
                                </Columns>
                            </ColumnModel>
                            <SelectionModel>
                                <ext:RowSelectionModel ID="RowSelectionModel1" runat="server" SingleSelect="true">
                                </ext:RowSelectionModel>
                            </SelectionModel>
                        </ext:GridPanel>
                    </Center>
                </ext:BorderLayout>
            </Body>
        </ext:ViewPort>
    </div>
    </form>
</body>
</html>
