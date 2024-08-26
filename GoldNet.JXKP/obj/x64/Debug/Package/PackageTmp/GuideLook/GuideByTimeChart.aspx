<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="GuideByTimeChart.aspx.cs" Inherits="GoldNet.JXKP.GuideLook.GuideByTimeChart" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>无标题页</title>
     <script type="text/javascript" src="/resources/hightChartsJs/jquery.min.js"></script>
    <script type="text/javascript" src="/resources/hightChartsJs/highcharts.js"></script>
    <script type="text/javascript" src="/resources/hightChartsJs/exporting.js"></script>
          <script type="text/javascript">                
        var chart;
        function refreshCharts(result) {
            showLoadMask();
            var chartsterm = escape(result);
            //var type = DeptCode;
              $.ajax({ 
                 type: "POST", 
                 
                 url: "GuideByTimeChart.aspx/GetDate", //注意调用方式，同样方式可以调用webservice 
                 //data:{}, //在这里可以设置需要传递的参数 
                 data:'{\'terms\':\''+chartsterm+'\'}', //在这里可以设置需要传递的参数 
                 
                 contentType: "application/json; charset=utf-8",
                 
                 dataType: "json", 
                 
                 beforeSend: function(xhr) {   
                                                xhr.setRequestHeader("Content-type",    
                                                                     "application/json; charset=utf-8");   
                                              },
                 success: function(data) { // 替换返回内容 
                        var chartInfo = data.d;
                        var chartsType = chartInfo.split('$')[0];
                        if(chartsType == 'pie') {
                            CreatePieCharts(chartInfo);
                        }
                        if(chartsType == 'bar') {
                            CreateBarCharts(chartInfo);
                        }
                 }, 
                 error: function(xhr,data,e) { 
                 } 
             });

            hideLoadMask();
        }
          
        //载入等待mask 显示
        function showLoadMask()
        {
            hideLoadMask();
            $('#loading').css("display","");
        }
        //载入等待mask 隐藏
        function hideLoadMask()
        {
          jQuery(function(){   
                jQuery('#loading-one').empty().append('页面载入完毕.').parent().fadeOut('slow');   
            });   
        }
        var EmptyChart = function() {
            $("#chart").html(""); 
        }
         var CreatePieCharts = function(data) { 
               var chartsInfo = data.split('$');
               if(chartsInfo[3] == '' || chartsInfo[3] == null) 
               {
                     $("#chart").html(""); 
                    window.parent.ChartsNodataMsg();
                    return;
               }
               
				chart = new Highcharts.Chart({
					chart: {
						renderTo: 'chart',
						//80, 80, 80, 80
						//50, 200, 60, 200
						margin: [80, 80, 80, 80]
					},
					title: {
						text: chartsInfo[1]
					},
					plotArea: {
						shadow: null,
						borderWidth: null,
						backgroundColor: null
					},
					tooltip: {
						formatter: function() {
							return '<b>'+ this.point.name +'</b>: '+ this.y ;
						}
					},
					plotOptions: {
						pie: {
							allowPointSelect: true,
							cursor: 'pointer',
                            dataLabels: {
								enabled: true,
								color: '#000000',
								connectorColor: '#000000',
								formatter: function() {
									return '<b>'+ this.point.name +'</b>: '+ this.y +'';
								}
							}
						}
					},
				    series: [eval("(" + chartsInfo[3] + ")")]
				});
        }
        
        
   var CreateBarCharts = function(data) {
       
        var chartsInfo = data.split('$');
        
	    if(chartsInfo[3] == '' || chartsInfo[3] == null) 
        {
            $("#chart").html(""); 
            window.parent.ChartsNodataMsg();    
            return;
        }
        chart = new Highcharts.Chart({
					chart: {
						renderTo: 'chart',
						defaultSeriesType: 'bar'
					},
					title: {
						text: chartsInfo[1]
					},
					xAxis: {
						categories: eval("(["+chartsInfo[2]+"])")
					},
				    yAxis: {
                         min: 0,
                         title: {
                            text: '指标值',
                            align: 'high'
                         }
                    },
					tooltip: {
						formatter: function() {
							return ''+
								    '指标值: '+ this.y;
						}
					},
				    plotOptions: {
						bar: {
							dataLabels: {
								enabled: true
							},
							cursor: 'pointer'
				
							}
					},
				    series: [eval("(" + chartsInfo[3] + ")")]
				});
        }
        
        </script>
</head>
<body>
    <div id="loading" style="display: none; fixed !important; position: absolute; top: 0;
        left: 0; height: 100%; width: 100%; z-index: 999; background: #000 url(../resources/images/load.gif) no-repeat center center;
        opacity: 0.6; filter: alpha(opacity=60); font-size: 14px; line-height: 20px;">
        <p id="loading-one" style="color: #fff; position: absolute; top: 50%; left: 50%;
            margin: 20px 0 0 -50px; padding: 3px 10px;">
            页面载入中..
        </p>
    </div>
    <div id="chart">
    </div>
</body>
</html>
