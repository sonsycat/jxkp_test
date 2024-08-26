<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Graph.aspx.cs" Inherits="GoldNet.JXKP.RLZY.BaseInfoMaintain.Graph" %>

<%@ Register Assembly="Goldnet.Ext.Web" Namespace="Goldnet.Ext.Web" TagPrefix="ext" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <style type="text/css">
        .search-item
        {
            font: normal 11px tahoma, arial, helvetica, sans-serif;
            padding: 3px 10px 3px 10px;
            border: 1px solid #fff;
            border-bottom: 1px solid #eeeeee;
            white-space: normal;
            color: #555;
            width: 200px;
        }
        .search-item h3
        {
            display: block;
            font: inherit;
            font-weight: bold;
            color: #222;
        }
        .search-item h3 span
        {
            float: right;
            font-weight: normal;
            margin: 0 0 5px 5px;
            width: 140px;
            display: block;
            clear: none;
        }
    </style>

    <script type="text/javascript">
       function ChartsNodataMsg() {
            Ext.Msg.show({ title: '信息提示', msg: '无数据显示,请重新选择', icon: 'ext-mb-info', buttons: { ok: true }  });
       }
       function PageOnload() {
            document.write('<div id="loading" style="position: fixed !important; position: absolute; top: 0; left: 0; height: 100%; width: 100%; z-index: 999; background: #000 url(../resources/images/load.gif) no-repeat center center; opacity: 0.6; filter: alpha(opacity=60); font-size: 14px; line-height: 20px;">');
            document.write('   <p id="loading-one" style="font-size:12px;color: #fff; position: absolute; top: 50%; left: 50%; margin: 20px 0 0 -50px; padding: 3px 10px;">   页面载入中.. </p>');
            document.write('</div>');
        }
        PageOnload();
       
        var chartsOper = function(oper) {
            var deptCode = DeptCodeCombo.getSelectedItem().value;
            var result = oper+'*'+deptCode;
            document.getElementById('GridPanel_IFrame').contentWindow.refreshCharts(result);
        }
    </script>

</head>
<body>
    <form id="form1" runat="server" enableviewstate="false">
    <ext:ScriptManager ID="ScriptManager1" runat="server">
        <Listeners>
            <DocumentReady Handler=" Ext.get('loading').setOpacity(0.0,{ duration:1.0,easing:'easeNone'});Ext.get('loading').hide();" />
        </Listeners>
    </ext:ScriptManager>
    <ext:Store ID="Store3" runat="server" AutoLoad="true">
        <Reader>
            <ext:JsonReader Root="Staffdepts">
                <Fields>
                    <ext:RecordField Name="DEPT_NAME" />
                    <ext:RecordField Name="DEPT_CODE" />
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
                                <ext:Label ID="Label3" runat="server" Text="科室：">
                                </ext:Label>
                                <ext:ComboBox ID="DeptCodeCombo" runat="server" StoreID="Store3" DisplayField="DEPT_NAME"
                                    Width="120" ValueField="DEPT_CODE" TypeAhead="false" LoadingText="Searching..."
                                    PageSize="1000" ItemSelector="div.search-item" MinChars="1" FieldLabel="科室信息"
                                    ListWidth="240">
                                    <Template ID="Template1" runat="server">
                                       <tpl for=".">
                                          <div class="search-item">
                                             <h3><span style="width:auto">{DEPT_CODE}</span>{DEPT_NAME}</h3>
                                          </div>
                                       </tpl>
                                    </Template>
                                </ext:ComboBox>
                                <ext:Button ID="btnMedi" runat="server" Text="全院医师分布图" Icon="ChartBar">
                                    <Listeners>
                                        <Click Handler="chartsOper(1);" />
                                    </Listeners>
                                </ext:Button>
                                <ext:Button ID="btnEdu" runat="server" Text="学历层次分布图" Icon="ChartPie">
                                    <Listeners>
                                        <Click Handler="chartsOper(2);" />
                                    </Listeners>
                                </ext:Button>
                                <ext:Button ID="btnYears" runat="server" Text="年龄结构分布图" Icon="ChartPie">
                                    <Listeners>
                                        <Click Handler="chartsOper(3);" />
                                    </Listeners>
                                </ext:Button>
                                <ext:Button ID="btnJob" runat="server" Text="职称结构分布图" Icon="ChartPie">
                                    <Listeners>
                                        <Click Handler="chartsOper(4);" />
                                    </Listeners>
                                </ext:Button>
                            </Items>
                        </ext:Toolbar>
                    </North>
                    <Center>
                        <ext:Panel runat="server" ID="panel3" BodyBorder="false">
                            <Body>
                                <ext:Panel ID="GridPanel" runat="server" Border="false" Height="600" AutoScroll="true">
                                    <AutoLoad Url="GraphHightChart.aspx" Mode="IFrame">
                                    </AutoLoad>
                                </ext:Panel>
                            </Body>
                        </ext:Panel>
                    </Center>
                </ext:BorderLayout>
            </Body>
        </ext:ViewPort>
    </div>
    </form>
</body>
</html>
