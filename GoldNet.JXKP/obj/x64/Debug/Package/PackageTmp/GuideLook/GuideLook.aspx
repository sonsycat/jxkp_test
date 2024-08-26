<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="GuideLook.aspx.cs" Inherits="GoldNet.JXKP.GuideLook.GuideLook" %>

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
        .x-form-group .x-form-group-header-text
        {
            background-color: #dfe8f6;
        }
        .x-label-text
        {
            font-weight: bold;
            font-size: 11px;
        }
        table
        {
            font-size: 12px;
            background-color: #dfe8f6;
        }
        body
        {
            background-color: #DFE8F6;
            font-size: 12px;
        }
    </style>

    <script type="text/javascript">
           
            //指标结构树加载,调用AJAX方法。
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
            };
                    
            //组织结构加载，初始化科室过滤         
            function refreshDeptTree(tree) {
                tree.el.mask('正在加载...', 'x-loading-mask');
                Goldnet.AjaxMethods.RefreshDeptTree({
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
            }; 
             
            //岗位加载      
            function refreshStaffTree(tree) {
                tree.el.mask('正在加载...', 'x-loading-mask');
                Goldnet.AjaxMethods.RefreshStaffTree({
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
            };
            
            //指标结构树的选择处理         
            var refreshGrid = function(node) {
                if ((node.id.length == 8) && (node.id!= 'root')) {
                    var guidecode = node.id;
                    radNa.setDisabled(true);
                    radTj.setDisabled(true);

                    Goldnet.AjaxMethod.request(
                      'GridPanelRefresh',
                        {
                        params: {
                            GuideCode:guidecode
                        },
                        success: function(result) {
                            //调用图表方法
                            document.getElementById('GridPanel_IFrame').contentWindow.refreshCharts(result);
                        },
                        failure: function(msg) {
                            GridPanel1.el.unmask();
                        }
                    }); 
                }
            };
        
            //选择图表类型
            var ChartsTypeSelect = function() {
                var OrganTypeType = document.getElementById('cbx_OrgType_Value');
                if(!DeptInfobtn.disabled || !empInfobtn.disabled){
                    if(OrganTypeType.value == '02') {
                        refreshDeptChartsConfig();
                    } 
                    if(OrganTypeType.value == '03') {
                        refreshChartsConfig();
                    }
                }
            };
        
            //人员过滤后查询图表
            function refreshChartsConfig() {
                var StationCode = "";
                if(QueryStation.getSelectedItem().text !="") {
                    StationCode = QueryStation.getSelectedItem().value;
                }
                Goldnet.AjaxMethod.request(
                  'ChartsRefresh',
                    {
                    params: {
                        StationCode:StationCode
                    },
                    success: function(result) {
                        document.getElementById('GridPanel_IFrame').contentWindow.refreshCharts(result);
                    },
                    failure: function(msg) {
                        GridPanel1.el.unmask();
                    }
                });    
            };
            
            //
            function getDeptCheckedNode() {
                var result = "";
                var checkeds = Ext.getCmp('DeptTreePanel').getChecked();
                for (var i = 0; i < checkeds.length; i++) {
                    result = result + checkeds[i].id + ";";
                }
                return result;
            };
            
            //科室过滤后查询图表
            function refreshDeptChartsConfig() {
                var temp = getDeptCheckedNode().split(';');
                var deptCode = "";
                for(var i=0;i<temp.length;i++) {
                    if(temp[i].indexOf('CLASS') == -1 && temp[i] != '') {
                        deptCode = deptCode +"'" +temp[i]+"',";
                    }
                }
               Goldnet.AjaxMethod.request(
                  'DeptChartsRefresh',
                    {
                    params: {
                        DeptCode:deptCode
                    },
                    success: function(result) {
                        document.getElementById('GridPanel_IFrame').contentWindow.refreshCharts(result);
                    },
                    failure: function(msg) {
                        GridPanel1.el.unmask();
                    }
                });    
            };
            
            //
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
            };            
            
            //
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
            };
            
            //
            var parentState = function(node) {
                var state = siblState(node).join();
                if (state.indexOf("3") == -1 && state.indexOf("2") == -1) {
                    return 1;
                } else if (state.indexOf("1") == -1 && state.indexOf("2") == -1) {
                    return -1;
                } else {
                    return 0;
                }
            };
            
            //
            var parentChecked = function(node) {
                var parentNode = node.parentNode;
                if (parentNode == null)  return false;
                var checkbox = parentNode.getUI().checkbox;
                if (typeof checkbox == 'undefined')  return false;
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
            };

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
    //                child.ui.checkbox.checked = checked;
    //                child.ui.node.attributes.checked = checked;
                    });
                    parentChecked(node);
                }
            }
            
            //
            function getStaffCheckedNode() {
                var result = "";
                var checkeds = Ext.getCmp('TreePanel1').getChecked();
                for (var i = 0; i < checkeds.length; i++) {
                    result = result + checkeds[i].id + ";";
                }
                return result;
            }
            
            //
            function SelectedOrg(value) {
                if(value == "01") {
                   empInfobtn.show();
                   DeptInfobtn.show();
                   empInfobtn.setDisabled(true);
                   DeptInfobtn.setDisabled(true);
                   cbx_ChartsType.setValue("line");
                   cbx_ChartsType.setDisabled(true);
                }
                if(value == "02") {
                   DeptInfobtn.show();
                   empInfobtn.setDisabled(true);
                   DeptInfobtn.setDisabled(true);
                   cbx_ChartsType.setDisabled(false);
                }
                if(value == "03") {
                   empInfobtn.show();
                   DeptInfobtn.setDisabled(true);
                   empInfobtn.setDisabled(true);
                   cbx_ChartsType.setDisabled(false);
                }
                radNa.setDisabled(false);
                radTj.setDisabled(false);
                cbx_Years.setDisabled(false);
                document.getElementById('GridPanel_IFrame').contentWindow.EmptyChart();
                refreshTree(TreeCtrl);
            }
            
            //
            function SelectedDeptType(OrganValue,DeptValue) {
                if(OrganValue == "01") {
                    empInfobtn.show();
                    DeptInfobtn.show();
                    empInfobtn.setDisabled(true);
                    DeptInfobtn.setDisabled(true);
                    cbx_ChartsType.setValue("line");
                    cbx_ChartsType.setDisabled(true);
                }
                if(OrganValue == "02") {
                    DeptInfobtn.show();
                    empInfobtn.setDisabled(true);
                    DeptInfobtn.setDisabled(true);
                    cbx_ChartsType.setDisabled(false);
                }
                if(OrganValue == "03") {
                    empInfobtn.show();
                    DeptInfobtn.setDisabled(true);
                    empInfobtn.setDisabled(true);
                    cbx_ChartsType.setDisabled(false);
                }
                radNa.setDisabled(false);
                radTj.setDisabled(false);
                cbx_Years.setDisabled(false);
                document.getElementById('GridPanel_IFrame').contentWindow.EmptyChart();
                refreshTree(TreeCtrl);
           }
           
           //
           function ChartsNodataMsg() {
                Ext.Msg.show({ title: '信息提示', msg: '无数据显示,请重新选择', icon: 'ext-mb-info', buttons: { ok: true }  });
           }
           
           //
           function PageOnload() {
                document.write('<div id="loading" style="position: fixed !important; position: absolute; top: 0; left: 0; height: 100%; width: 100%; z-index: 999; background: #000 url(../resources/images/load.gif) no-repeat center center; opacity: 0.6; filter: alpha(opacity=60); font-size: 14px; line-height: 20px;">');
                document.write('   <p id="loading-one" style="font-size:12px;color: #fff; position: absolute; top: 50%; left: 50%; margin: 20px 0 0 -50px; padding: 3px 10px;">   页面载入中.. </p>');
                document.write('</div>');
            }
            
            PageOnload();
    </script>

</head>
<body>
    <form id="form1" runat="server" enableviewstate="false">
    <ext:ScriptManager ID="ScriptManager1" runat="server">
        <Listeners>
            <DocumentReady Handler=" Ext.get('loading').setOpacity(0.0,{ duration:1.0,easing:'easeNone'});Ext.get('loading').hide();refreshTree(#{TreeCtrl}); cbx_OrgType.setWidth(45);cbx_DeptType.setWidth(90);refreshDeptTree(DeptTreePanel);refreshStaffTree(TreePanel1);GridPanel.setHeight(Ext.getBody().getViewSize().height);" />
        </Listeners>
    </ext:ScriptManager>
    <ext:Hidden ID="hideCode" runat="server" EnableViewState="false">
    </ext:Hidden>
    <ext:Store ID="StoreCombo" runat="server">
        <Reader>
            <ext:JsonReader>
                <Fields>
                    <ext:RecordField Name="VALUE" />
                    <ext:RecordField Name="TEXT" />
                </Fields>
            </ext:JsonReader>
        </Reader>
    </ext:Store>
    <ext:Store ID="SYear" runat="server">
        <Reader>
            <ext:JsonReader ReaderID="YEAR">
                <Fields>
                    <ext:RecordField Name="YEAR" />
                </Fields>
            </ext:JsonReader>
        </Reader>
    </ext:Store>
    <ext:Store ID="SMonth" runat="server">
        <Reader>
            <ext:JsonReader ReaderID="MONTH">
                <Fields>
                    <ext:RecordField Name="MONTH" />
                </Fields>
            </ext:JsonReader>
        </Reader>
    </ext:Store>
    <div>
        <ext:ViewPort ID="ViewPort1" runat="server">
            <Body>
                <ext:BorderLayout ID="BorderLayout2" runat="server">
                    <West Collapsible="true" MaxWidth="200">
                        <%--指标树区域--%>
                        <ext:TreePanel runat="server" ID="TreeCtrl" AutoScroll="true" BodyBorder="false"
                            Width="200">
                            <TopBar>
                                <ext:Toolbar ID="Toolbar3" runat="server" Height="26">
                                    <Items>
                                        <ext:Label runat="server" Text="请选择：" ID="Label1">
                                        </ext:Label>
                                        <ext:ComboBox runat="server" ID="cbx_OrgType" Width="60" Editable="false">
                                            <Listeners>
                                                <Select Handler="#{Window1}.hide();SelectedOrg(#{cbx_OrgType}.getSelectedItem().value);" />
                                            </Listeners>
                                        </ext:ComboBox>
                                        <ext:ComboBox runat="server" ID="cbx_DeptType" Width="100">
                                            <Listeners>
                                                <Select Handler="#{Window1}.hide();SelectedDeptType(#{cbx_OrgType}.getSelectedItem().value,#{cbx_DeptType}.getSelectedItem().value);" />
                                            </Listeners>
                                        </ext:ComboBox>
                                    </Items>
                                </ext:Toolbar>
                            </TopBar>
                            <Root>
                                <ext:TreeNode NodeID="root" Text="指标体系">
                                </ext:TreeNode>
                            </Root>
                            <BottomBar>
                                <ext:Toolbar ID="Toolbar4" runat="server">
                                    <Items>
                                        <ext:ToolbarButton ID="ToolbarButton2" runat="server" IconCls="icon-expand-all">
                                            <Listeners>
                                                <Click Handler="#{TreeCtrl}.root.expand(true);" />
                                            </Listeners>
                                            <ToolTips>
                                                <ext:ToolTip ID="ToolTip5" IDMode="Ignore" runat="server" Html="全部展开" />
                                            </ToolTips>
                                        </ext:ToolbarButton>
                                        <ext:ToolbarButton ID="ToolbarButton3" runat="server" IconCls="icon-collapse-all">
                                            <Listeners>
                                                <Click Handler="#{TreeCtrl}.root.collapse(true);" />
                                            </Listeners>
                                            <ToolTips>
                                                <ext:ToolTip ID="ToolTip6" IDMode="Ignore" runat="server" Html="全部收起" />
                                            </ToolTips>
                                        </ext:ToolbarButton>
                                    </Items>
                                </ext:Toolbar>
                            </BottomBar>
                            <Listeners>
                                <Click Handler="refreshGrid(node);" />
                            </Listeners>
                        </ext:TreePanel>
                    </West>
                    <Center>
                        <ext:Panel runat="server" ID="panel3" BodyBorder="false">
                            <TopBar>
                                <ext:Toolbar runat="server" ID="ctl155" StyleSpec="border:0">
                                    <Items>
                                        <ext:ToolbarButton ID="DeptInfobtn" runat="server" Text="科室信息过滤" Icon="NoteEdit">
                                            <Menu>
                                                <ext:Menu ID="orderMenu" runat="server">
                                                    <Items>
                                                        <ext:ElementMenuItem Target="#{DeptTreePanel}" Shift="false" />
                                                    </Items>
                                                </ext:Menu>
                                            </Menu>
                                        </ext:ToolbarButton>
                                        <ext:ToolbarSplitButton ID="empInfobtn" runat="server" Text="人员信息过滤" Icon="NoteEdit">
                                            <Listeners>
                                                <ArrowClick Handler="#{Window1}.show()" />
                                                <Click Handler="#{Window1}.show()" />
                                            </Listeners>
                                        </ext:ToolbarSplitButton>
                                    </Items>
                                </ext:Toolbar>
                            </TopBar>
                            <Body>
                                <ext:Panel ID="GridPanel" runat="server" Border="false" AutoScroll="true">
                                    <AutoLoad Url="HightCharts.aspx" Mode="IFrame">
                                    </AutoLoad>
                                </ext:Panel>
                            </Body>
                            <BottomBar>
                                <ext:Toolbar ID="Toolbar2" runat="server" Height="26">
                                    <Items>
                                        <ext:Radio runat="server" ID="radNa" GroupName="yearSelect" BoxLabel="自然年度" Hidden="true">
                                        </ext:Radio>
                                        <ext:Radio runat="server" ID="radTj" GroupName="yearSelect" BoxLabel="统计年度" Checked="true"
                                            Hidden="true">
                                        </ext:Radio>
                                        <ext:Label ID="Label3" runat="server" Text="查询时间：" />
                                        <ext:ComboBox runat="server" ID="cbx_Years" Width="60" ReadOnly="true" StoreID="SYear" DisplayField="YEAR" ValueField="YEAR">
                                        </ext:ComboBox>
                                        <ext:ToolbarTextItem ID="ToolbarTextItem2" runat="server" Text="年" />
                                        <ext:ComboBox runat="server" ID="cbx_Months" Width="40" ReadOnly="true" StoreID="SMonth" DisplayField="MONTH" ValueField="MONTH">
                                        </ext:ComboBox>
                                        <ext:ToolbarTextItem ID="ToolbarTextItem7" runat="server" Text="月   至   " />
                                        <ext:ComboBox runat="server" ID="cbx_Yearsto" Width="60" ReadOnly="true"  StoreID="SYear" DisplayField="YEAR" ValueField="YEAR">
                                        </ext:ComboBox>
                                        <ext:ToolbarTextItem ID="ToolbarTextItem4" runat="server" Text="年" />
                                        <ext:ComboBox runat="server" ID="cbx_Monthsto" Width="40" ReadOnly="true" StoreID="SMonth" DisplayField="MONTH" ValueField="MONTH">
                                        </ext:ComboBox>
                                        <ext:ToolbarTextItem ID="ToolbarTextItem5" runat="server" Text="月" />
                                        <ext:ToolbarSeparator ID="ToolbarSeparator4" runat="server" />
                                        <ext:Label ID="Label7" runat="server" Text="图形状态：" />
                                        <ext:ComboBox runat="server" ID="cbx_ChartsType" Width="80" SelectedIndex="0">
                                            <Items>
                                                <ext:ListItem Text="趋势图" Value="line" />
                                                <ext:ListItem Text="条形图" Value="bar" />
                                                <ext:ListItem Text="饼型图" Value="pie" />
                                            </Items>
                                            <Listeners>
                                                <Select Handler="ChartsTypeSelect();" />
                                            </Listeners>
                                        </ext:ComboBox>
                                        <ext:ToolbarSeparator ID="ToolbarSeparator1" runat="server" />
                                        <ext:Button Text="查询" ID="btn_Modify" runat="server" Icon="FolderMagnify">
                                            <Listeners>
                                                <Click Handler="ChartsTypeSelect()" />
                                            </Listeners>
                                        </ext:Button>
                                    </Items>
                                </ext:Toolbar>
                            </BottomBar>
                        </ext:Panel>
                    </Center>
                </ext:BorderLayout>
            </Body>
        </ext:ViewPort>
    </div>
    <div class="x-hide-display">
        <ext:Panel runat="server">
            <Body>
                <ext:TreePanel runat="server" ID="DeptTreePanel" BodyBorder="false" Width="250" Height="350"
                    Icon="BookOpen" AutoScroll="true" RootVisible="false" UseArrows="true">
                    <TopBar>
                        <ext:Toolbar ID="Toolbar1" runat="server">
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
                                <ext:ToolbarFill ID="ToolbarFill11" runat="server">
                                </ext:ToolbarFill>
                                <ext:TextField ID="txtDeptCode" runat="server" EmptyText="请输入部门名称">
                                </ext:TextField>
                                <ext:ToolbarButton ID="btnDeptQuery" runat="server" Icon="ServerEdit">
                                    <ToolTips>
                                        <ext:ToolTip ID="ToolTip9" IDMode="Ignore" runat="server" Html="过滤部门" />
                                    </ToolTips>
                                    <Listeners>
                                        <Click Handler="if(#{txtDeptCode}.getValue()=='') {Ext.Msg.show({ title: '信息提示', msg: '请输入部门名称', icon: 'ext-mb-info', buttons: { ok: true }  });} else {refreshDeptTree(#{DeptTreePanel});}" />
                                    </Listeners>
                                </ext:ToolbarButton>
                                <ext:ToolbarButton ID="ToolbarButton8" runat="server" Icon="ArrowJoin">
                                    <ToolTips>
                                        <ext:ToolTip ID="ToolTip10" IDMode="Ignore" runat="server" Html="重置" />
                                    </ToolTips>
                                    <Listeners>
                                        <Click Handler="#{txtDeptCode}.setValue('');refreshDeptTree(#{DeptTreePanel});" />
                                    </Listeners>
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
                        <ext:Toolbar ID="Toolbar5" runat="server">
                            <Items>
                                <ext:ToolbarFill ID="ToolbarFill1" runat="server" />
                                <ext:ToolbarButton ID="ToolbarButton6" runat="server" Icon="Magnifier" Text="查询图表">
                                    <Listeners>
                                        <Click Handler="refreshDeptChartsConfig()" />
                                    </Listeners>
                                </ext:ToolbarButton>
                            </Items>
                        </ext:Toolbar>
                    </BottomBar>
                </ext:TreePanel>
            </Body>
        </ext:Panel>
        <ext:Window ID="Window1" runat="server" Title="人员信息过滤" Width="300" Height="400" BodyBorder="false"
            ShowOnLoad="false" Icon="UserEdit" Resizable="false">
            <Body>
                <ext:Accordion ID="AccordionLayout1" runat="server">
                    <ext:TreePanel ID="TreePanel1" runat="server" Title="按岗位过滤" RootVisible="false" AutoScroll="true"
                        Icon="TagBlue" UseArrows="true">
                        <TopBar>
                            <ext:Toolbar runat="server">
                                <Items>
                                    <ext:ToolbarButton ID="ToolbarButton1" runat="server" IconCls="icon-expand-all">
                                        <Listeners>
                                            <Click Handler="#{TreePanel1}.root.expand(true);" />
                                        </Listeners>
                                        <ToolTips>
                                            <ext:ToolTip ID="ToolTip1" IDMode="Ignore" runat="server" Html="全部展开" />
                                        </ToolTips>
                                    </ext:ToolbarButton>
                                    <ext:ToolbarButton ID="ToolbarButton4" runat="server" IconCls="icon-collapse-all">
                                        <Listeners>
                                            <Click Handler="#{TreePanel1}.root.collapse(true);" />
                                        </Listeners>
                                        <ToolTips>
                                            <ext:ToolTip ID="ToolTip2" IDMode="Ignore" runat="server" Html="全部收起" />
                                        </ToolTips>
                                    </ext:ToolbarButton>
                                    <ext:ToolbarFill runat="server">
                                    </ext:ToolbarFill>
                                    <ext:ComboBox ID="QueryStation" runat="server" StoreID="StoreCombo" DisplayField="TEXT"
                                        ValueField="VALUE" Editable="false">
                                    </ext:ComboBox>
                                    <ext:ToolbarButton ID="btnQueryStation" runat="server" Icon="ServerEdit">
                                        <ToolTips>
                                            <ext:ToolTip ID="ToolTip3" IDMode="Ignore" runat="server" Html="查询科室岗位" />
                                        </ToolTips>
                                        <%-- <Listeners>
                                            <Click Handler="if(#{QueryStation}.getValue()=='') {Ext.Msg.show({ title: '信息提示', msg: '请输入岗位名称', icon: 'ext-mb-info', buttons: { ok: true }  });} else {refreshStaffTree(#{TreePanel1});}" />
                                        </Listeners>--%>
                                        <AjaxEvents>
                                            <Click OnEvent="QueryStationNode">
                                                <ExtraParams>
                                                    <ext:Parameter Name="nodeDept" Value="getStaffCheckedNode()" Mode="Raw">
                                                    </ext:Parameter>
                                                </ExtraParams>
                                            </Click>
                                        </AjaxEvents>
                                    </ext:ToolbarButton>
                                    <ext:ToolbarButton ID="btnRefresh" runat="server" Icon="ArrowJoin">
                                        <ToolTips>
                                            <ext:ToolTip ID="ToolTip4" IDMode="Ignore" runat="server" Html="重置" />
                                        </ToolTips>
                                        <Listeners>
                                            <Click Handler="#{QueryStation}.setValue('');refreshStaffTree(#{TreePanel1});" />
                                        </Listeners>
                                    </ext:ToolbarButton>
                                </Items>
                            </ext:Toolbar>
                        </TopBar>
                        <Root>
                            <ext:TreeNode NodeID="root" Text="人员岗位">
                            </ext:TreeNode>
                        </Root>
                        <Listeners>
                            <CheckChange Handler="ToCheckChange(node,checked);" />
                        </Listeners>
                    </ext:TreePanel>
                    <ext:Panel ID="panelwin" runat="server" BodyStyle="background-color:transparent"
                        Title="按人员过滤" Icon="TagGreen">
                        <Body>
                            <table width="100%" cellpadding="0" cellspacing="0">
                                <tr>
                                    <td style="width: 45%">
                                        人员类别
                                    </td>
                                    <td>
                                        <ext:ComboBox ID="cbx_Ptype" runat="server" Width="42" FieldLabel="人员类别">
                                        </ext:ComboBox>
                                    </td>
                                    <td>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        技术级
                                    </td>
                                    <td>
                                        <ext:ComboBox ID="cbx_PTechType" runat="server" Width="42" FieldLabel="技术级">
                                        </ext:ComboBox>
                                    </td>
                                    <td>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        技职类别
                                    </td>
                                    <td>
                                        <ext:ComboBox ID="cbx_PTech" runat="server" Width="42" FieldLabel="技职类别">
                                        </ext:ComboBox>
                                    </td>
                                    <td>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        技术职务时间:
                                    </td>
                                    <td style="width: 60px;">
                                        <ext:ComboBox ID="cbx_TimeOrgan" Width="42" runat="server">
                                            <Items>
                                                <ext:ListItem Text="全部" Value="" />
                                                <ext:ListItem Text="晚于" Value=">="/>
                                                <ext:ListItem Text="早于" Value="<=" />
                                                <ext:ListItem Text="为" Value="=" />
                                            </Items>
                                        </ext:ComboBox>
                                    </td>
                                    <td style="border-spacing: 0">
                                        <ext:DateField ID="timer" runat="server" Width="80" Format="yyyy-MM-dd">
                                        </ext:DateField>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        学位
                                    </td>
                                    <td>
                                        <ext:ComboBox ID="cbx_PCollage" runat="server" Width="42" FieldLabel="学位">
                                        </ext:ComboBox>
                                    </td>
                                    <td>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        技术系列
                                    </td>
                                    <td>
                                        <ext:ComboBox ID="cbx_PLevel" runat="server" Width="42" FieldLabel="技术系列">
                                        </ext:ComboBox>
                                    </td>
                                    <td>
                                    </td>
                                </tr>
                            </table>
                        </Body>
                    </ext:Panel>
                </ext:Accordion>
            </Body>
            <BottomBar>
                <ext:Toolbar runat="server">
                    <Items>
                        <ext:ToolbarFill ID="Toolba" runat="server" />
                        <ext:ToolbarButton ID="ToolbarButton5" runat="server" Icon="Magnifier" Text="查询图表">
                            <Listeners>
                                <Click Handler="refreshChartsConfig()" />
                            </Listeners>
                        </ext:ToolbarButton>
                    </Items>
                </ext:Toolbar>
            </BottomBar>
        </ext:Window>
    </div>
    </form>
</body>
</html>
