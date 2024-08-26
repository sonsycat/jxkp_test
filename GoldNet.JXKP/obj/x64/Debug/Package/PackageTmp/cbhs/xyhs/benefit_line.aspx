<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="benefit_line.aspx.cs" Inherits="GoldNet.JXKP.cbhs.xyhs.benefit_line" %>
<%@ Register Assembly="Goldnet.Ext.Web" Namespace="Goldnet.Ext.Web" TagPrefix="ext" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title></title>

    <script type="text/javascript" src="/resources/hightChartsJs/jquery.min.js"></script>
    <script type="text/javascript" src="/resources/hightChartsJs/highcharts.js"></script>
    <script type="text/javascript" src="/resources/hightChartsJs/exporting.js"></script>
    
    <script type="text/javascript">
        var chart;
        var CreateLineCharts = function(phz,render_to,line1,line2,jd) {
           chart = new Highcharts.Chart({
              chart: {
						renderTo: render_to
					},
					xAxis: {
					},
					yAxis: {
					},
					title: {
						text: phz
					},
					series: [{
						type: 'line',
						name: '收入',
						data: eval(line1),
						marker: {
							enabled: false
						},
						states: {
							hover: {
								lineWidth: 0
							}
						},
						enableMouseTracking: false
					},{
						type: 'line',
						name: '成本',
						data: eval(line2),
						marker: {
							enabled: false
						},
						states: {
							hover: {
								lineWidth: 0
							}
						},
						enableMouseTracking: false
					}, {
						type: 'scatter',
						name: '数值',
						data: eval(jd),
						marker: {
							radius: 4
						}
					}]

           });
        }
        function clare()
        {
            document.getElementById('chart1').innerHTML="";
        }
        function refreshCharts1(dept_name,charges,costs,gd_costs,x,y) {
            if(x<0||x==0)
            {
                line1='[['+x+','+y+'],[100,'+charges+']]';
                line2='[['+x+','+y+'],[100,'+costs+']]';
                jd='[[0,0],[100,'+charges+'],[0,'+gd_costs+'],[100,'+costs+'],['+x+','+y+']]';
            }
            if(x>100)
            {
                line1='[[0,0],['+x+','+y+']]';
                line2='[[0,'+gd_costs+'],['+x+','+y+']]';
                jd='[[0,0],[100,'+charges+'],[0,'+gd_costs+'],[100,'+costs+'],['+x+','+y+']]'
            }
            if(x>0&&x<100)
            {
                line1='[[0,0],[100,'+charges+']]';
                line2='[[0,'+gd_costs+'],[100,'+costs+']]';
                jd='[[0,0],[100,'+charges+'],[0,'+gd_costs+'],[100,'+costs+'],['+x+','+y+']]';
            }
            CreateLineCharts(dept_name+'-收支平衡线-平衡值：'+y,'chart1',line1,line2,jd);   
        }

    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <ext:ScriptManager ID="ScriptManager1" runat="server" AjaxMethodNamespace="CompanyX" />
        <div id='chart1' style="width: 650px; height: 400px ; overflow:auto ;"/>
    </div>
    </form>
</body>
</html>
