<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="dept_costs_deal_scale.aspx.cs" Inherits="GoldNet.JXKP.cbhs.xyhs.Operation.dept_costs_deal_scale" %>
<%@ Register Assembly="Goldnet.Ext.Web" Namespace="Goldnet.Ext.Web" TagPrefix="ext" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
   <title></title>

    <script type="text/javascript" src="/resources/hightChartsJs/jquery.min.js"></script>

    <script type="text/javascript" src="/resources/hightChartsJs/highcharts.js"></script>

    <script type="text/javascript" src="/resources/hightChartsJs/exporting.js"></script>

    <!-- Additional files for the Highslide popup effect -->

    <script type="text/javascript" src="/resources/hightChartsJs/highslide-full.min.js"></script>

    <script type="text/javascript" src="/resources/hightChartsJs/highslide.config.js"
        charset="utf-8"></script>

    <link rel="stylesheet" type="text/css" href="/resources/hightChartsJs/highslide.css" />

    <script language="javascript" type="text/javascript">
        var chart;
        var CreatePieCharts = function(render_to,series_str) {
           chart = new Highcharts.Chart({
              chart: {
                 renderTo: render_to,
                 margin: [10, 10, 10, 10]
              },
              title: {
                 text: ''
              },
              plotArea: {
                 shadow: null,
                 borderWidth: null,
                 backgroundColor: null
              },
              tooltip: {
			      formatter: function() {
			         return '<b>'+ this.point.name +'</b>: '+ this.y 
			      }
			   },
              plotOptions: {
                 pie: {
                        allowPointSelect: true,
                        cursor: 'pointer',
                        formatter: function() {
			                   if (this.y > 5) return this.point.name;
			                },
                        dataLabels: {
                           enabled: true,
                            color:  '#000000',
                            connectorColor:  '#000000',
                           formatter: function() {
                              return '<b>'+ this.point.name +'</b>: '+ this.y 
                           }
                    }
                 }
              },
              legend: {
			      layout: 'vertical',
			      style: {
			         left: 'auto',
			         bottom: 'auto',
			         right: '5px',
			         top: '50px'
			      }
			   },
               series:  eval(series_str)
           });
        }
        
        function clare()
        {
            document.getElementById('chart1').innerHTML="";
        }
        function refreshCharts1(series_str) {
             
             CreatePieCharts('chart1',series_str);   
        }
    </script>

</head>
<body>
    <form id="form1" runat="server">
    <div>
        <ext:ScriptManager ID="ScriptManager1" runat="server" AjaxMethodNamespace="CompanyX" />
        <ext:Panel ID="Panel1" runat="server" Border="false">
            <TopBar>
                <ext:Toolbar ID="Toolbar1" runat="server">
                    <Items>
                        <ext:ToolbarTextItem ID="dd1Name" runat="server" />
                        <ext:ToolbarSeparator ID="ToolbarSeparator1" runat="server" />
                        <ext:Label ID="func" runat="server" Text="显示类别：" Width="40">
                        </ext:Label>
                        <ext:ComboBox ID="Combo_type" runat="server" AllowBlank="true" EmptyText="请选择类别"
                            Width="100">
                            <Items>
                                <ext:ListItem Text="按成本项目" Value="1" />
                                <ext:ListItem Text="按成本属性" Value="2" />
                            </Items>
                            <AjaxEvents>
                                <Select OnEvent="Type_Selected">
                                    <EventMask Msg='载入中...' ShowMask="true" />
                                </Select>
                            </AjaxEvents>
                        </ext:ComboBox>
                    </Items>
                </ext:Toolbar>
            </TopBar>
            <Body>
                <div id='chart1' style="width: 650px; height: 380px ; overflow:auto ;">
            </Body>
        </ext:Panel>
    </div>
    </form>
</body>
</html>