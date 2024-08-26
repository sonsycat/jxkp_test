<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="GuideByDept.aspx.cs" Inherits="GoldNet.JXKP.GuideLook.GuideByDept" %>

<%@ Register Assembly="Goldnet.Ext.Web" Namespace="Goldnet.Ext.Web" TagPrefix="ext" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>无标题页</title>

    <script type="text/javascript">
        var showResult = function (node) {
            //GridPanel_Show.title = "图表----"+node.text
        };
        var addItems = function(code, text) {
            if (code == '') {
                btnSreach.setDisabled(true);
            } else {
                selectGuideCode.value = code;
                selectGuideText.value = text;
                btnSreach.setDisabled(false);
            }
        };

        var ChartRefresh = function() {
            var year = Combo_StationYear.getValue();
            document.getElementById('GridPanel_Show_IFrame').contentWindow.refreshCharts(selectGuideCode.value,TreeCtrl.getSelectionModel().getSelectedNode().id,year);
        }
        
       function ChartsNodataMsg() {
            Ext.Msg.show({ title: '信息提示', msg: '无数据显示,请重新选择', icon: 'ext-mb-info', buttons: { ok: true }  });
       }
        
        
       function PageOnload() {
            document.write('<div id="loading" style="position: fixed !important; position: absolute; top: 0; left: 0; height: 100%; width: 100%; z-index: 999; background: #000 url(../resources/images/load.gif) no-repeat center center; opacity: 0.6; filter: alpha(opacity=60); font-size: 14px; line-height: 20px;">');
            document.write('   <p id="loading-one" style="font-size:12px;color: #fff; position: absolute; top: 50%; left: 50%; margin: 20px 0 0 -50px; padding: 3px 10px;">   页面载入中.. </p>');
            document.write('</div>');
        }
        PageOnload();
    </script>

</head>
<body>
    <form id="form1" runat="server">
    <ext:ScriptManager ID="ScriptManager1" runat="server">
        <Listeners>
            <DocumentReady Handler=" Ext.get('loading').setOpacity(0.0,{ duration:1.0,easing:'easeNone'});Ext.get('loading').hide();GridPanel_Show.setHeight(Ext.getBody().getViewSize().height);" />
        </Listeners>
    </ext:ScriptManager>
    <ext:Hidden ID="selectGuideCode" runat="server"></ext:Hidden>
    <ext:Hidden ID="selectGuideText" runat="server"></ext:Hidden>
    <ext:Store runat="server" ID="Store1">
        <Reader>
            <ext:JsonReader>
                <Fields>
                </Fields>
            </ext:JsonReader>
        </Reader>
    </ext:Store>
    <div>
        <ext:ViewPort ID="ViewPort1" runat="server">
            <Body>
                <ext:BorderLayout ID="BorderLayout2" runat="server">
                    <North>
                        <ext:Toolbar runat="server" ID="ctl155" StyleSpec="border:0">
                            <Items>
                                <ext:Button ID="Btn_SelectGuide" runat="server" Icon="ShapeSquareAdd" Text="选择指标">
                                    <AjaxEvents>
                                        <Click OnEvent="Btn_SelectGuide_Click">
                                            <EventMask ShowMask="true" Msg="请稍候..." />
                                        </Click>
                                    </AjaxEvents>
                                </ext:Button>
                                <ext:ToolbarSeparator ID="ToolbarSeparator3" runat="server">
                                </ext:ToolbarSeparator>
                                <ext:Button ID="btnSreach" runat="server" Text=" 查询 " Icon="DatabaseGo" Disabled="true">
                                    <Listeners>
                                        <Click  Handler="ChartRefresh()"/>
                                    </Listeners>
                                </ext:Button>
                                <ext:ToolbarFill ID="ToolbarFill1" Enabled="true" runat="server">
                                </ext:ToolbarFill>
                                <ext:Label ID="func" runat="server" Text="绩效年度：" Width="40" />
                                <ext:ComboBox ID="Combo_StationYear" runat="server" ReadOnly="true" AllowBlank="false"
                                    FieldLabel="绩效年度">
                                </ext:ComboBox>
                            </Items>
                        </ext:Toolbar>
                    </North>
                    <Center>
                        <ext:Panel ID="GridPanel_Show" runat="server" Border="false" Height="800" Title="图表"
                            StripeRows="true" AutoScroll="true">
                             <AutoLoad Url="GuideByDeptChart.aspx" Mode="IFrame"></AutoLoad>
                        </ext:Panel>
                    </Center>
                    <West CollapseMode="Mini" Split="false" Collapsible="false">
                        <ext:Panel ID="Panel1" runat="server" Width="200" BodyBorder="false" Title="请选择科室"
                            AutoScroll="true" Border="false">
                            <Body>
                                <ext:TreePanel runat="server" Width="200" ID="TreeCtrl" AutoHeight="true" AutoScroll="false"
                                    Border="false">
                                    <Listeners>
                                        <Click Handler="showResult(node)" />
                                    </Listeners>
                                </ext:TreePanel>
                            </Body>
                        </ext:Panel>
                    </West>
                </ext:BorderLayout>
            </Body>
        </ext:ViewPort>
        <ext:Window ID="DetailWin" runat="server" Icon="ApplicationViewIcons" Title="选择"
            Width="760" Closable="true" CloseAction="Hide" Maximizable="true" Height="430"
            AutoShow="false" Modal="true" AutoScroll="true" CenterOnLoad="true" ShowOnLoad="false"
            Resizable="true" StyleSpec="background-color:Transparent;" BodyStyle="background-color:Transparent;">
            <Listeners>
                <Hide Handler="this.clearContent();" />
                <BeforeShow Handler=" var height = Ext.getBody().getViewSize().height; var width = Ext.getBody().getViewSize().width; if (el.getSize().height > height) {  el.setHeight(height - 20) } ;if (el.getSize().width > width) {  el.setWidth(width - 20) }  " />
            </Listeners>
        </ext:Window>
    </div>
    </form>
</body>
</html>
