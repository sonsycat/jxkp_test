<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="default_02.aspx.cs" Inherits="Goldnet.JXKP.home._default_02" %>

<%@ Register Assembly="Goldnet.Ext.Web" Namespace="Goldnet.Ext.Web" TagPrefix="ext" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
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
                                    
                                    <ext:ComboBox ID="Combo_Year" runat="server" ReadOnly="true" StoreID="SYear" Width="60"
                                        DisplayField="YEAR" ValueField="YEAR" ForceSelection="true" SelectOnFocus="true">
                                    </ext:ComboBox>
                                    
                                    <ext:ToolbarSpacer ID="ToolbarSpacer11" runat="server" Width="15" />
                                    
                                    <ext:ComboBox ID="cbbmonth" runat="server" ReadOnly="true" StoreID="SMonth" Width="50"
                                        DisplayField="MONTH" ValueField="MONTH">
                                    </ext:ComboBox>
                                    
                                    <ext:ToolbarSeparator ID="ToolbarSeparator1" runat="server" />
                                    <ext:Button runat="server" ID="BtnQuery" Text="查询" Icon="DatabaseGo" Flat="false">
                                        <AjaxEvents>
                                            <Click OnEvent="GetQueryPortalet">
                                            </Click>
                                        </AjaxEvents>
                                    </ext:Button>
                                </Items>
                            </ext:Toolbar>
                        </BottomBar>
                    </ext:Panel>
                </North>
                <Center>
                    <ext:Panel ID="PanelCenter" runat="server" Border="false" AutoScroll="false" Height="230">
                        <Body>
                            <ext:FitLayout ID="FitLayout3" runat="server">
                                <ext:Panel ID="Panel5" runat="server" Border="false" AutoScroll="false">
                                    <Body>
                                        <ext:ColumnLayout ID="ColumnLayoutRight" runat="server" Margin="0" Split="true">
                                            <ext:LayoutColumn ColumnWidth="0.25">
                                                <ext:Panel ID="Panel1" runat="server" Border="true" AutoScroll="false">
                                                    <Body>
                                                        <ext:AnchorLayout ID="AnchorLayoutLeft" runat="server">
                                                            <ext:Anchor>
                                                                <ext:Panel Height="230px" ID="Portlet_KPI_mz" runat="server" Border="false" AutoScroll="false" AutoDataBind="false">
                                                                    <AutoLoad Url="/mainpage/main_kpi_new.aspx" Mode="IFrame" NoCache="true" Scripts="false"
                                                                        ShowMask="true" MaskMsg="载入中...">
                                                                    </AutoLoad>
                                                                </ext:Panel>
                                                            </ext:Anchor>
                                                        </ext:AnchorLayout>
                                                    </Body>
                                                </ext:Panel>
                                            </ext:LayoutColumn>
                                            <ext:LayoutColumn ColumnWidth="0.25">
                                                <ext:Panel ID="Panel2" runat="server" Border="true" AutoScroll="false">
                                                    <Body>
                                                        <ext:AnchorLayout ID="AnchorLayout2" runat="server">
                                                            <ext:Anchor>
                                                                <ext:Panel Height="230px" ID="Portlet_KPI_zy" runat="server" Border="false" AutoScroll="false" AutoDataBind="false">
                                                                    <AutoLoad Url="/mainpage/main_kpi_zy.aspx" Mode="IFrame" NoCache="true" Scripts="false"
                                                                        ShowMask="true" MaskMsg="载入中...">
                                                                    </AutoLoad>
                                                                </ext:Panel>
                                                            </ext:Anchor>
                                                        </ext:AnchorLayout>
                                                    </Body>
                                                </ext:Panel>
                                            </ext:LayoutColumn>
                                            <ext:LayoutColumn ColumnWidth="0.25">
                                                <ext:Panel ID="Panel3" runat="server" Border="true" AutoScroll="false">
                                                    <Body>
                                                        <ext:AnchorLayout ID="AnchorLayout3" runat="server">
                                                            <ext:Anchor>
                                                                <ext:Panel Height="230px" ID="Portlet_KPI_ss" runat="server" Border="false" AutoScroll="false" AutoDataBind="false">
                                                                    <AutoLoad Url="/mainpage/main_kpi_ss.aspx" Mode="IFrame" NoCache="true" Scripts="false"
                                                                        ShowMask="true" MaskMsg="载入中...">
                                                                    </AutoLoad>
                                                                </ext:Panel>
                                                            </ext:Anchor>
                                                        </ext:AnchorLayout>
                                                    </Body>
                                                </ext:Panel>
                                            </ext:LayoutColumn>
                                            <ext:LayoutColumn ColumnWidth="0.25">
                                                <ext:Panel ID="Panel4" runat="server" Border="true" AutoScroll="false">
                                                    <Body>
                                                        <ext:AnchorLayout ID="AnchorLayout1" runat="server">
                                                            <ext:Anchor>
                                                                <ext:Panel Height="230px" ID="Portlet_KPI_yzb" runat="server" Border="false" AutoScroll="false" AutoDataBind="false">
                                                                    <AutoLoad Url="/mainpage/main_kpi_yzb.aspx" Mode="IFrame" NoCache="true" Scripts="false"
                                                                        ShowMask="true" MaskMsg="载入中...">
                                                                    </AutoLoad>
                                                                </ext:Panel>
                                                            </ext:Anchor>
                                                        </ext:AnchorLayout>
                                                    </Body>
                                                </ext:Panel>
                                            </ext:LayoutColumn>
                                        </ext:ColumnLayout>
                                    </Body>
                                </ext:Panel>
                            </ext:FitLayout>
                        </Body>
                    </ext:Panel>
                </Center>
                <South>
                    <ext:Panel ID="Panel9" runat="server" Border="false" AutoScroll="false" Height="280">
                        <Body>
                            <ext:FitLayout ID="FitLayout1" runat="server">
                                <ext:Panel ID="Panel6" runat="server" Border="false" AutoScroll="false">
                                    <Body>
                                        <ext:ColumnLayout ID="ColumnLayout1" runat="server" Margin="0" Split="true">
                                            <ext:LayoutColumn ColumnWidth="0.33">
                                                <ext:Panel ID="Panel7" runat="server" Border="false" AutoScroll="false">
                                                    <Body>
                                                        <ext:AnchorLayout ID="AnchorLayout4" runat="server">
                                                            <ext:Anchor>
                                                                <ext:Panel ID="Portlet_jjhs" Height="290px" runat="server" AutoScroll="false" AutoDataBind="false" Title="工作量指标">
                                                                    <AutoLoad Url="/mainpage/main_gzl.aspx" Mode="IFrame" NoCache="true" Scripts="false"
                                                                        MaskMsg="载入中..." ShowMask="true">
                                                                    </AutoLoad>
                                                                </ext:Panel>
                                                            </ext:Anchor>
                                                        </ext:AnchorLayout>
                                                    </Body>
                                                </ext:Panel>
                                            </ext:LayoutColumn>
                                            <ext:LayoutColumn ColumnWidth="0.33">
                                                <ext:Panel ID="Panel10" runat="server" Border="false" AutoScroll="false">
                                                    <Body>
                                                        <ext:AnchorLayout ID="AnchorLayout5" runat="server">
                                                            <ext:Anchor>
                                                                <ext:Panel ID="Portlet_gzl" Height="290px" runat="server" AutoScroll="false" AutoDataBind="false" Title="质量效率指标">
                                                                    <AutoLoad Url="/mainpage/main_zlxl.aspx" Mode="IFrame" NoCache="true" Scripts="false"
                                                                        MaskMsg="载入中..." ShowMask="true">
                                                                    </AutoLoad>
                                                                </ext:Panel>
                                                            </ext:Anchor>
                                                        </ext:AnchorLayout>
                                                    </Body>
                                                </ext:Panel>
                                            </ext:LayoutColumn>
                                            <ext:LayoutColumn ColumnWidth="0.34">
                                                <ext:Panel ID="Panel12" runat="server" Border="false" AutoScroll="false">
                                                    <Body>
                                                        <ext:AnchorLayout ID="AnchorLayout6" runat="server">
                                                            <ext:Anchor>
                                                                <ext:Panel ID="Portlet_zlxl" Height="290px" runat="server" AutoScroll="false" AutoDataBind="false" Title="经济指标">
                                                                    <AutoLoad Url="/mainpage/main_jjhs.aspx" Mode="IFrame" NoCache="true" Scripts="false"
                                                                        MaskMsg="载入中..." ShowMask="true">
                                                                    </AutoLoad>
                                                                </ext:Panel>
                                                            </ext:Anchor>
                                                        </ext:AnchorLayout>
                                                    </Body>
                                                </ext:Panel>
                                            </ext:LayoutColumn>
                                        </ext:ColumnLayout>
                                    </Body>
                                </ext:Panel>
                            </ext:FitLayout>
                        </Body>
                    </ext:Panel>
                </South>
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
