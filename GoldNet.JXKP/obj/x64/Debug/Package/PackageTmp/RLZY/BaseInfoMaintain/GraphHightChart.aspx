<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="GraphHightChart.aspx.cs"
    Inherits="GoldNet.JXKP.RLZY.BaseInfoMaintain.GraphHightChart" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>无标题页</title>

    <script type="text/javascript" src="/resources/hightChartsJs/jquery.min.js"></script>

    <script type="text/javascript" src="/resources/hightChartsJs/highcharts.js"></script>

    <script type="text/javascript" src="/resources/hightChartsJs/exporting.js"></script>
    
    <script type="text/javascript">                
        var chart;        
        function refreshCharts(chartsterms) {
              showMessageBox();
              $("#chart").empty();
              $("#chart").remove();
              var ParentDiv = $("<div id='chart'></div>");
              ParentDiv.appendTo('body');

              $.ajax({ 
                 type: "POST", 
                 
                 url: "GraphHightChart.aspx/GetDate", //注意调用方式，同样方式可以调用webservice 
                 //data:{}, //在这里可以设置需要传递的参数 
                 data:'{\'terms\':\''+chartsterms+'\'}', //在这里可以设置需要传递的参数 
                 
                 contentType: "application/json; charset=utf-8",
                 
                 dataType: "json", 
                 
                 beforeSend: function(xhr) {   
                                                xhr.setRequestHeader("Content-type",    
                                                                     "application/json; charset=utf-8");   
                                              },
                 success: function(data) { // 替换返回内容 
                        var chartInfo = data.d;
                        var chartsType = chartInfo.split('*')[0];
                        if(chartsType == '1') {
                            CreateBarCharts(chartInfo);
                        }
                        if(chartsType == '2') {
                            CreatePieCharts(chartInfo);
                        }
                 }, 
                 error: function(xhr,data,e) { 
                 } 
             });
             closeWindow();
        }
        
        
          var CreatePieCharts = function(data) { 
               var chartsInfo = data.split('*');
               if(chartsInfo[2] == '' || chartsInfo[2] == null) 
               {
                     $("#chart").html(""); 
                    window.parent.ChartsNodataMsg();
                    return;
               }
               
				chart = new Highcharts.Chart({
					chart: {
						renderTo: 'chart',
                        plotBackgroundColor: null,
                        plotBorderWidth: null,
                        plotShadow: false

					},
					title: {
						text: chartsInfo[3]
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
									return '<b>'+ this.point.name +'</b>: '+ this.y +'人';
								}
						    }
						}
				    },
				    series: [eval("(" + chartsInfo[2] + ")")]
				});
        }
        
     var CreateBarCharts = function(data) {
        var chartsInfo = data.split('*');
	    if(chartsInfo[2] == '' || chartsInfo[2] == null) 
        {
            $("#chart").html(""); 
            window.parent.ChartsNodataMsg();    
            return;
        }
        chart = new Highcharts.Chart({
					chart: {
						renderTo: 'chart',
						defaultSeriesType: 'column',
                        margin: [ 50, 50, 100, 80]
					},
					title: {
						text: '全院医师分布图'
					},
					tooltip: {
						formatter: function() {
							return ''+
								    '人数: '+ this.y;
						}
					},
					xAxis: {
						categories: eval("(["+chartsInfo[1]+"])"),
					    labels: {
                        rotation: -45,
                        align: 'right',
                        style: {
                            font: 'normal 13px Verdana, sans-serif'
                        }
                     }

					},
				    yAxis: {
                         min: 0,
                         title: {
                            text: '人数'
                         }
                      },

				    series: [eval("(" + chartsInfo[2] + ")")]
				});
        }
        
        
        
        var EmptyChart = function() {
            $("#chart").html(""); 
        }
        //弹出方法
        function showMessageBox()
        {
            closeWindow();
            $('#loading').css("display","");
        }
          
        //关闭窗口
        function closeWindow()
        {
          jQuery(function(){   
                jQuery('#loading-one').empty().append('图表载入完毕.').parent().fadeOut('slow');   
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
    <div id='my-content'>
    </div>
</body>
</html>
