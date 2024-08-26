<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="default.aspx.cs" Inherits="Goldnet.JXKP.home._default" %>

<%@ Register Assembly="Goldnet.Ext.Web" Namespace="Goldnet.Ext.Web" TagPrefix="ext" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>医院精细化绩效管理考评系统</title>
    <link rel="stylesheet" type="text/css" href="/resources/css/main.css" />
    <link rel="stylesheet" type="text/css" href="/resources/css/header.css" />
    <script type="text/javascript" src="/resources/ExampleTab.js"></script>
    <script type="text/javascript">
        var loadAppTab = function(href, id,title,ico) {
            var tab = ExampleTabs.getComponent(id);

            if (tab) {
                ExampleTabs.setActiveTab(tab);
            } else {
                createExampleTab(id, href,title,ico);
            }
        }

        var selectionChaged = function(dv, nodes) {
            if (nodes.length > 0) {
                var url = nodes[0].getAttribute("ext:url"),
                    id = nodes[0].getAttribute("ext:id");

                loadAppTab(url, id);
            }
        }

        var viewClick = function(dv, e) {
            var group = e.getTarget("h2", 3, true);

            if (group) {
                group.up("div").toggleClass("collapsed");
            }
        }
    </script>

</head>
<body>
    <form id="form1" runat="server">
    <ext:ScriptManager ID="ScriptManager1" runat="server" WindowUnloadMsg="正在跳转">
        <CustomAjaxEvents>
            <ext:AjaxEvent Target="sys_logout" OnEvent="Logout">
                <EventMask ShowMask="true" MinDelay="500" Msg="注销中,请稍候..." />
            </ext:AjaxEvent>
        </CustomAjaxEvents>
    </ext:ScriptManager>
    <ext:ViewPort ID="ViewPort1" runat="server">
        <Body>
            <ext:BorderLayout ID="BorderLayout1" runat="server">
                <North Collapsible="true" Split="true" CollapseMode="Mini" UseSplitTips="true">
                    <ext:Panel ID="HeadPanel" IDMode="Ignore" runat="server" Header="false" Border="false"
                        Height="56px" AutoScroll="false">
                       
                    </ext:Panel>
                </North>
                <West Collapsible="true" Split="true" MinWidth="175">
                    <ext:Panel runat="server" Title="精细化医疗绩效" Width="175" TitleCollapse="True" AutoScroll="true"
                        ID="MenuList">
                        <Body>
                            <ext:Accordion runat="server" Animate="true" Fill="true" ID="according1">
                            </ext:Accordion>
                        </Body>
                    </ext:Panel>
                </West>
                <Center>
                    <ext:TabPanel ID="ExampleTabs" runat="server" ActiveTabIndex="0" EnableTabScroll="true">
                        <Tabs>
                            <ext:Tab ID="tabHome" runat="server" Title="系统首页" Icon="Vcard">
                                <Body>
                                </Body>
                            </ext:Tab>
                        </Tabs>
                         <Plugins>
                            <ext:TabCloseMenu ID="TabCloseMenu1"  CloseOtherTabsText="关闭其它标签页" CloseTabText="关闭标签页" runat="server" />
                        </Plugins>
                    </ext:TabPanel>
                </Center>
            </ext:BorderLayout>
        </Body>
    </ext:ViewPort>
    <ext:TaskManager ID="TaskManager1" runat="server">
        <Tasks>
            <ext:Task TaskID="servertime" Interval="60000">
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
