<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="default_01.aspx.cs" Inherits="Goldnet.JXKP.home._default_01" %>

<%@ Register Assembly="Goldnet.Ext.Web" Namespace="Goldnet.Ext.Web" TagPrefix="ext" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>运行监控－－金网医院精细化绩效管理考评系统</title>
    <link rel="stylesheet" type="text/css" href="/resources/css/main.css" />
    <link rel="stylesheet" type="text/css" href="/resources/css/header.css" />
    <style type="text/css">
        .x-panel-tc
        {
            background: repeat-x scroll 0 0 #b0e0e6;
            overflow: hidden;
        }
        .x-panel-tl
        {
            background: repeat-x scroll 0 0 #b0e0e6;
            border-bottom: 1px solid #99BBE8;
            padding-left: 6px;
        }
        .x-panel-tr
        {
            background: no-repeat scroll right 0 #b0e0e6;
            padding-right: 6px;
        }
    </style>
</head>
<body>

    <script type="text/javascript">
        var showportal = function() {
            if (Portlet_Detail.hidden == true) {
                Portlet_Detail.show();
                Ext.get('Portlet_Detail').slideIn('l', { duration: 1 });
            }
            if (Portlet_Detail.collapsed == true) {
                Portlet_Detail.onExpand();
            }
        }
        
        var hideportal = function() {
            if (Portlet_Detail.hidden == false) {
                Ext.get('Portlet_Detail').slideOut('l', { duration: 1 });
                setTimeout(' Portlet_Detail.hide()', 1000);
            }
        }
        
        var viewzljk = function(tbid, tpid) {
            Portlet_ZRYL.onCollapse(true,true) ;
            showportal();
            Portlet_Detail.clearContent();
            Portlet_Detail.loadContent({
            url: "/mainpage/main_zljk_view.aspx?tbid="+tbid+"&tpid="+tpid,
                mode: "iframe",
                showMask: true,
                maskMsg: "载入中..."
            });
        }
        
        var viewxgxfx = function(guideCode) {
            Portlet_ZRYL.onCollapse(true,true) ;
            showportal();
            Portlet_Detail.clearContent();
            Portlet_Detail.loadContent({
            url: "/mainpage/guide_relation.aspx?id="+guideCode+"&oid='*'",
                mode: "iframe",
                showMask: true,
                maskMsg: "载入中..."
            });
        }
      
        var viewgwzbjk = function(command, zbdm,nextguideid, organ) {
            Portlet_ZRYL.onCollapse(true, true);
            showportal();
            Portlet_Detail.clearContent();
            var url = "/home/about.blank.html";
            if (command == "CmdZBMX") {
                url = "/mainpage/guide_detail.aspx";
            } else if (command == "CmdYQST") {
            url = "/mainpage/guide_trendline.aspx";
            } else if (command == "CmdXGXFX") {
                url = "/mainpage/guide_relation.aspx";
            }

            Portlet_Detail.loadContent({
            url: url + "?id="+ zbdm +"&nid=" + nextguideid + "&oid=" + organ,
                mode: "iframe",
                showMask: true,
                maskMsg: "载入中..."
            });

        }
        
        var showChart = function(areaid) {
            if(areaid == "01"){
                Portlet_MZL.hide();
            } else if(areaid == "02"){
                Portlet_YLFY.hide();
            } else if(areaid == "03"){
                Portlet_ZYL.hide();
            } else if(areaid == "04"){
                Portlet_SSL.hide();
            }
        }
//        
//        function layout() {
//            var BorderLayout = Ext.get('PanelCenter');
//            var xy = BorderLayout.getXY();
//           BorderLayout.setHeight(Ext.lib.Dom.getViewHeight() - xy[1]);
//        }
//        
//        Ext.onReady(layout, this);
//        Ext.EventManager.onWindowResize(layout);
    </script>

    <form id="form1" runat="server">
    <ext:ScriptManager ID="ScriptManager1" runat="server">
    </ext:ScriptManager>
    <ext:ViewPort ID="ViewPort1" runat="server">
        <Body>
            <ext:BorderLayout ID="BorderLayout1" runat="server">
                <North Collapsible="false">
                    <ext:Panel ID="PanelTop" IDMode="Ignore" runat="server" Header="false" Border="false"
                        Height="86px" AutoScroll="false">
                        <%-- <AutoLoad Url="/home/header.aspx?7" ></AutoLoad>--%>
                        <BottomBar>
                            <ext:Toolbar ID="Toolbar1" runat="server" AutoShow="true" AutoRender="true">
                                <Items>
                                    <ext:Label ID="Label1" runat="server" Text="年度：" />
                                    <ext:ComboBox ID="Combo_Year" runat="server" Width="54" MaxLength="4" MinLength="4" />
                                    <ext:ToolbarSpacer ID="ToolbarSpacer11" runat="server" Width="15" />
                                    <ext:Label ID="Label2" runat="server" Text="科室分类：" />
                                    <ext:ComboBox ID="Combo_DeptType" runat="server" Width="88" EmptyText="请选择">
                                        <AjaxEvents>
                                            <Select OnEvent="SelectedDeptType">
                                                <EventMask ShowMask="true" />
                                            </Select>
                                        </AjaxEvents>
                                    </ext:ComboBox>
                                    <ext:ToolbarSpacer ID="ToolbarSpacer2" runat="server" Width="15" />
                                    <ext:Label ID="Label3" runat="server" Text="科室：" />
                                    <ext:ComboBox ID="Combo_Dept" runat="server" Width="100" EmptyText="请选择科室" Stateful="true">
                                        <AjaxEvents>
                                            <Select OnEvent="SelectedDept">
                                                <EventMask ShowMask="true" />
                                            </Select>
                                        </AjaxEvents>
                                    </ext:ComboBox>
                                    <ext:ToolbarSpacer ID="ToolbarSpacer3" runat="server" Width="15" />
                                    <ext:Label ID="Label4" runat="server" Text="岗位：" />
                                    <ext:ComboBox ID="Combo_Station" runat="server" Width="130" EmptyText="请选择岗位">
                                        <AjaxEvents>
                                            <Select OnEvent="SelectedStation">
                                                <EventMask ShowMask="true" />
                                            </Select>
                                        </AjaxEvents>
                                    </ext:ComboBox>
                                    <ext:ToolbarSpacer ID="ToolbarSpacer4" runat="server" Width="15" />
                                    <ext:Label ID="Label5" runat="server" Text="人员：" />
                                    <ext:ComboBox ID="Combo_Person" runat="server" Width="90" Enabled="false" EmptyText="请选择人员" />
                                    <ext:ToolbarSpacer ID="ToolbarSpacer12" runat="server" Width="10" />
                                    <ext:ToolbarSeparator ID="ToolbarSeparator1" runat="server" />
                                    <ext:Button runat="server" ID="BtnQuery" Text="查询" Icon="DatabaseGo" Flat="false">
                                        <AjaxEvents>
                                            <Click OnEvent="GetQueryPortalet">
                                            </Click>
                                        </AjaxEvents>
                                    </ext:Button>
                                    <ext:ToolbarSpacer ID="ToolbarSpacer1" runat="server" Width="15" />
                                    <ext:Label ID="Label6" runat="server" Text="" />
                                </Items>
                            </ext:Toolbar>
                        </BottomBar>
                    </ext:Panel>
                </North>
                <Center>
                    <ext:Panel ID="PanelCenter" runat="server" Border="false" AutoScroll="false">
                        <Body>
                            <ext:FitLayout ID="FitLayout3" runat="server">
                                <ext:Portal ID="Portal" runat="server" AutoScroll="false" StyleSpec="overflow-x:hidden;overflow-y:scroll;">
                                    <Listeners>
                                        <Drop Handler="e.panel.el.frame();" />
                                    </Listeners>
                                    <Body>
                                        <ext:ColumnLayout ID="ColumnLayoutRight" runat="server" Margin="0" Split="false">
                                            <ext:LayoutColumn ColumnWidth="0.30">
                                                <ext:PortalColumn ID="PortalColumnLeft" runat="server" StyleSpec="padding:2px 2px 0 2px"
                                                    AutoScroll="true">
                                                    <Body>
                                                        <ext:AnchorLayout ID="AnchorLayoutLeft" runat="server">
                                                            <ext:Anchor>
                                                                <ext:Portlet Height="340px" ID="Portlet_KPI" Title="关键指标监控" Icon="ChartPie" runat="server"
                                                                    AutoScroll="false" AutoDataBind="false">
                                                                    <AutoLoad Url="/mainpage/main_kpi.aspx" Mode="IFrame" NoCache="true" Scripts="false"
                                                                        ShowMask="true" MaskMsg="载入中...">
                                                                    </AutoLoad>
                                                                    <Tools>
                                                                        <ext:Tool Type="Refresh" Qtip="刷新" Handler="Portlet_KPI.reload();" />
                                                                        <ext:Tool Type="Close" Handler="panel.ownerCt.remove(panel, true);" />
                                                                    </Tools>
                                                                </ext:Portlet>
                                                            </ext:Anchor>
                                                            <%--<ext:Anchor>
                                                                <ext:Portlet Height="280px" Border="false" ID="Portlet_MZL" Title="" runat="server">
                                                                    <AutoLoad Url="/mainpage/main_mzl.aspx" Mode="IFrame" NoCache="true" Scripts="false"
                                                                        ShowMask="true" MaskMsg="载入中...">
                                                                    </AutoLoad>
                                                                </ext:Portlet>
                                                            </ext:Anchor>
                                                            <ext:Anchor>
                                                                <ext:Portlet Height="280px" Border="false" ID="Portlet_ZYL" Title="" runat="server"
                                                                    Hidden="true">
                                                                    <AutoLoad Url="/mainpage/main_zyl.aspx" Mode="IFrame" NoCache="true" Scripts="false"
                                                                        ShowMask="true" MaskMsg="载入中...">
                                                                    </AutoLoad>
                                                                </ext:Portlet>
                                                            </ext:Anchor>--%>
                                                        </ext:AnchorLayout>
                                                    </Body>
                                                </ext:PortalColumn>
                                            </ext:LayoutColumn>
                                            <ext:LayoutColumn ColumnWidth=".33">
                                                <ext:PortalColumn ID="PortalColumnCenter" runat="server" StyleSpec="padding:2px 0 0 2px">
                                                    <Body>
                                                        <ext:AnchorLayout ID="AnchorLayoutCenter" runat="server">
                                                            <ext:Anchor>
                                                                <ext:Portlet Height="340px" ID="Portlet_GWZBJK" Title="岗位指标监控" Icon="Magnifier" runat="server"
                                                                    AutoScroll="true">
                                                                    <Tools>
                                                                        <%--<ext:Tool Type="Gear" Handler="Portlet_GWZBJK.loadContent({ url: '/mainpage/guide_station.aspx', mode: 'iframe', showMask: true, maskMsg: '载入中...' });" Qtip="设置监控指标" />--%>
                                                                        <ext:Tool Type="Plus" Handler="if(Portlet_GWZBJK.getHeight()<700) Portlet_GWZBJK.setHeight(Portlet_GWZBJK.getHeight()+100);" />
                                                                        <ext:Tool Type="Minus" Handler="if(Portlet_GWZBJK.getHeight()>400) Portlet_GWZBJK.setHeight(Portlet_GWZBJK.getHeight()-100);" />
                                                                        <ext:Tool Type="Refresh" Qtip="刷新" Handler="Portlet_GWZBJK.reload();" />
                                                                    </Tools>
                                                                    <AutoLoad Url="/mainpage/main_gwzbjk.aspx" Mode="IFrame" NoCache="true" Scripts="false"
                                                                        MaskMsg="载入中..." ShowMask="true">
                                                                    </AutoLoad>
                                                                </ext:Portlet>
                                                            </ext:Anchor>
                                                            <%--<ext:Anchor>
                                                                <ext:Portlet Height="280px" Border="false" ID="Portlet_YLFY" Title="" runat="server">
                                                                    <AutoLoad Url="/mainpage/main_ylfy.aspx" Mode="IFrame" NoCache="true" Scripts="false"
                                                                        MaskMsg="载入中..." ShowMask="true">
                                                                    </AutoLoad>
                                                                </ext:Portlet>
                                                            </ext:Anchor>
                                                            <ext:Anchor>
                                                                <ext:Portlet Height="280px" Border="false" ID="Portlet_SSL" Title="" runat="server"
                                                                    Hidden="true">
                                                                    <AutoLoad Url="/mainpage/main_ssl.aspx" Mode="IFrame" NoCache="true" Scripts="false"
                                                                        MaskMsg="载入中..." ShowMask="true">
                                                                    </AutoLoad>
                                                                </ext:Portlet>
                                                            </ext:Anchor>--%>
                                                        </ext:AnchorLayout>
                                                    </Body>
                                                </ext:PortalColumn>
                                            </ext:LayoutColumn>
                                            <ext:LayoutColumn ColumnWidth=".36">
                                                <ext:PortalColumn ID="PortalColumnRight" runat="server" StyleSpec="padding:2px 18px 0 2px">
                                                    <Body>
                                                        <ext:AnchorLayout ID="AnchorLayoutRight" runat="server">
                                                            <ext:Anchor>
                                                                <ext:Portlet Height="340px" ID="Portlet_Detail" Title="详情查看" Icon="Zoom" runat="server">
                                                                    <Tools>
                                                                        <ext:Tool Type="Plus" Handler="if(Portlet_Detail.getHeight()<700) Portlet_Detail.setHeight(Portlet_Detail.getHeight()+100);" />
                                                                        <ext:Tool Type="Minus" Handler="if(Portlet_Detail.getHeight()>400) Portlet_Detail.setHeight(Portlet_Detail.getHeight()-100);" />
                                                                        <ext:Tool Type="Close" Handler="hideportal();" />
                                                                    </Tools>
                                                                </ext:Portlet>
                                                            </ext:Anchor>
                                                            <ext:Anchor>
                                                                <ext:Portlet Height="340px" ID="Portlet_ZRYL" Title="昨日医疗概览" Icon="MonitorGo" runat="server">
                                                                    <AutoLoad Url="/mainpage/main_zrylgl.aspx" Mode="IFrame" NoCache="true" Scripts="false"
                                                                        MaskMsg="载入中..." ShowMask="true">
                                                                    </AutoLoad>
                                                                    <Tools>
                                                                        <ext:Tool Type="Refresh" Qtip="刷新" Handler="Portlet_ZRYL.reload();" />
                                                                    </Tools>
                                                                </ext:Portlet>
                                                            </ext:Anchor>
                                                          <%--  <ext:Anchor>
                                                                <ext:Portlet Height="280px" ID="Portlet_ZLJK" Title="质量监控" Icon="DatabaseTable" runat="server">
                                                                    <AutoLoad Url="/mainpage/main_zljk.aspx" Mode="IFrame" NoCache="true" Scripts="false"
                                                                        MaskMsg="载入中..." ShowMask="true">
                                                                    </AutoLoad>
                                                                    <Tools>
                                                                        <ext:Tool Type="Refresh" Qtip="刷新" Handler="Portlet_ZLJK.reload();" />
                                                                    </Tools>
                                                                </ext:Portlet>
                                                            </ext:Anchor>--%>
                                                        </ext:AnchorLayout>
                                                    </Body>
                                                </ext:PortalColumn>
                                            </ext:LayoutColumn>
                                        </ext:ColumnLayout>
                                    </Body>
                                </ext:Portal>
                            </ext:FitLayout>
                        </Body>
                    </ext:Panel>
                </Center>
            </ext:BorderLayout>
        </Body>
    </ext:ViewPort>
    <ext:TaskManager ID="TaskManager1" runat="server">
        <Tasks>
            <ext:Task TaskID="servertime" Interval="600000">
                <AjaxEvents>
                    <Update OnEvent="RefreshTime">
                    </Update>
                </AjaxEvents>
            </ext:Task>
        </Tasks>
    </ext:TaskManager>
    </form>
</body>
</html>
