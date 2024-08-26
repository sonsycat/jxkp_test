<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="HightCharts.aspx.cs" Inherits="GoldNet.JXKP.GuideLook.HightCharts" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>无标题页</title>

    <script type="text/javascript" src="/resources/hightChartsJs/jquery.min.js"></script>

    <script type="text/javascript" src="/resources/hightChartsJs/highcharts.js"></script>

    <script type="text/javascript" src="/resources/hightChartsJs/exporting.js"></script>

    <!-- Additional files for the Highslide popup effect -->

    <script type="text/javascript" src="/resources/hightChartsJs/highslide-full.min.js"></script>

    <script type="text/javascript" src="/resources/hightChartsJs/highslide.config.js"
        charset="utf-8"></script>

    <link rel="stylesheet" type="text/css" href="/resources/hightChartsJs/highslide.css" />

    <script type="text/javascript">                
        var chart;
        var Organ;
        var year;
        var guideCode;
        var CurOrgan;
        
        //图表入口方法
        function refreshCharts(chartsterms) {
              showMessageBox();
              
              $("#chart").empty();
              $("#chart").remove();
              var ParentDiv = $("<div id='chart'></div>");
              ParentDiv.appendTo('body');
              
              var chartsInfo = chartsterms.split('*');
              Organ = chartsInfo[1];
              ChartsType = chartsInfo[0];
              year = chartsInfo[4];
              guideCode = chartsInfo[7];
              var chartsterm = escape(chartsterms);
              $.ajax({ 
                 type: "POST", 
                 url: "HightCharts.aspx/GetDate", //注意调用方式，同样方式可以调用webservice 
                 //data:{}, //在这里可以设置需要传递的参数 
                 data:'{\'terms\':\''+chartsterm+'\'}', //在这里可以设置需要传递的参数 
                 contentType: "application/json; charset=utf-8",
                 dataType: "json", 
                 
                 beforeSend: function(xhr) {   
                                                xhr.setRequestHeader("Content-type",    
                                                                     "application/json; charset=utf-8");   
                                           },
                 success: function(data) { 
                        var chartInfo = data.d;
                        var chartsType = chartInfo.split('$')[0];
                        if(chartsType == 'pie') {
                            CreatePieCharts(chartInfo);
                        }
                        if(chartsType == 'bar') {
                            CreateBarCharts(chartInfo);
                        }
                        if(chartsType == 'line') {
                            CreateLineCharts(chartInfo);
                        }
                 }, 
                 error: function(xhr,data,e) { 
                 } 
             });

              
             closeWindow();
        }
        
        //生成趋势图
        var CreateLineCharts = function(data) {
               var chartsInfo = data.split('$');
               var Categories = chartsInfo[2];
               var SeriesData = chartsInfo[3].split('*');
               if(chartsInfo[3] == '' || chartsInfo[3] == null) 
               {
                    $("#chart").html(""); 
                    window.parent.ChartsNodataMsg();      
                    return;
               }
               //chart = new Highcharts.Chart({
               var options  = {
			        chart: {
				        renderTo: 'chart',
				        defaultSeriesType: 'line',
				        ignoreHiddenSeries: false
			        },
                    title: {
				        text: chartsInfo[1]
			        },
                    xAxis: {
                      categories: []
                    },
                    tooltip: {
						formatter: function() {
							return '<b>'+ this.series.name +'</b><br/>'+
                                           '指标值:' + this.y;
						}
					},
					
                    legend: {
                        borderWidth: 0,
                        itemWidth: 78
                        //margin: 100
                    },
                    plotOptions: {
						line: {
							cursor: 'pointer',
							point: {
										events: {
											'click': function() {
		                                         if(Organ == '02') {
		                                                for (var i = 0; i < hs.expanders.length; i++) {
                                                            if (hs.expanders[i] != null) {
                                                                if (hs.expanders[i].maincontentId == "Div-"+ this.series.name + this.category) {
                                                                    return;
                                                                }
                                                            }
                                                        }
                                                        createDiv(this.series.name,this.category);
                                                        CurOrgan = '03';
                                                        var DivId = 'Div-' + this.series.name + this.category;
														hs.htmlExpand(null, {
													    headingText: '<select name=\'sldd\' onchange=\'PopUpChartChange("'+this.id+'",this,"'+CurOrgan+'","'+this.category+'","'+"my-content-" + this.series.name + this.category+'","'+guideCode+'");\'><option value=\'pie\'>饼型图</option><option value=\'bar\' selected=true>柱型图</option></select>',
										                preserveContent:true,
										                maincontentId: 'Div-' + this.series.name + this.category,
															width: 600,
															height:600
														});
														refreshPopUpCharts(this.id,'bar',Organ,this.category,"my-content-" + this.series.name + this.category,guideCode);
											    }
											    else if(Organ == '01') {
		                                                for (var i = 0; i < hs.expanders.length; i++) {
                                                            if (hs.expanders[i] != null) {
                                                                if (hs.expanders[i].maincontentId == "Div-"+ this.series.name + this.category) {
                                                                    return;
                                                                }
                                                            }
                                                        }
                                                        
                                                        createDiv(this.series.name,this.category);
                                                        CurOrgan = '02';
                                                        var DivId = 'Div-' + this.series.name + this.category;
														hs.htmlExpand(null, {
													    headingText: '<select name=\'sldd\' onchange=\'PopUpChartChange("'+this.id+'",this,"'+CurOrgan+'","'+year+'","'+"my-content-" + this.series.name + this.category+'","'+guideCode+'");\'><option value=\'line\' selected=true>趋势图</option><option value=\'pie\'>饼型图</option><option value=\'bar\'>柱型图</option></select>',
										                preserveContent:true,
										                maincontentId: 'Div-' + this.series.name + this.category,
															width: 600,
															height:600
														});
														refreshPopUpCharts(this.id,'line',Organ,this.category,"my-content-" + this.series.name + this.category,guideCode);  
											    }
											   else if(Organ == '03') {
                                                        for (var i = 0; i < hs.expanders.length; i++) {
                                                            if (hs.expanders[i] != null) {
                                                                if (hs.expanders[i].maincontentId == "Div-"+ this.series.name + this.category) {
                                                                    return;
                                                                }
                                                            }
                                                        }
                                                        createDiv(this.series.name,this.category);
                                                        CurOrgan = '04';
                                                        var DivId = 'Div-' + this.series.name + this.category;
														hs.htmlExpand(null, {
													    headingText: '<select name=\'sldd\' onchange=\'PopUpChartChange("'+this.id+'",this,"'+CurOrgan+'","'+this.category+'","'+"my-content-" + this.series.name + this.category+'","'+guideCode+'");\'><option value=\'pie\'>饼型图</option><option value=\'bar\' selected=true>柱型图</option></select>',
										                preserveContent:true,
										                maincontentId: 'Div-' + this.series.name + this.category,
															width: 600,
															height:600
														});
														refreshPopUpCharts(this.id,'bar',Organ,this.category,"my-content-" + this.series.name + this.category,guideCode);
											     } 
												}
											}
								   }
							}
						},
                    
                    series: []
                };
                
               var series = {data:[]};
               for(var j=0;j<Categories.split(',').length;j++) {
                 options.xAxis.categories.push(Categories.split(',')[j]);
               }
               
               for(var i=0;i<SeriesData.length;i++) {
                     options.series.push(eval("(" + SeriesData[i] + ")"));
               }
               chart = new Highcharts.Chart(options);
//               chart.xAxis[0].setCategories(eval("(["+Categories+"])"));
//               for(var i=0;i<SeriesData.length;i++) {
//                     chart.addSeries(eval("(" + SeriesData[i] + ")"));
//               }
               chart.yAxis[0].axisTitle.hide();
        }
         
        //生成饼图
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
					exporting: {
					    enabled: false
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
                               
							},
							//showInLegend: true,
							point: {
							events: {
											'click': function() {
											    if(Organ == '02') {
		                                                for (var i = 0; i < hs.expanders.length; i++) {
                                                            if (hs.expanders[i] != null) {
                                                                if (hs.expanders[i].maincontentId == "Div-"+ this.series.name + this.category) {
                                                                    return;
                                                                }
                                                            }
                                                        }
                                                        createDiv(this.series.name,this.category);
                                                        var DivId = 'Div-' + this.series.name + this.category;
														hs.htmlExpand(null, {
													    headingText: year+'年人员趋势图',
										                preserveContent:true,
										                maincontentId: DivId,
															width: 600,
															height:600
														});
														refreshPopUpCharts(this.id,'line',Organ,year,"my-content-" + this.series.name + this.category,guideCode);
											    }
									      }
							        }
							 }
							
						}
						 
					},
				    series: [eval("(" + chartsInfo[3] + ")")]
				});
        }
        
        //生成条图
        var CreateBarCharts = function(data) {
            //数据分解
            var chartsInfo = data.split('$');
            //图表显示单位个数
            var unitarr;
            //图表默认高度
            var charheight=600;
            
	        if(chartsInfo[3] == '' || chartsInfo[3] == null) 
            {
                $("#chart").html(""); 
                window.parent.ChartsNodataMsg();    
                return;
            }
            
            if(chartsInfo[2]=='' || chartsInfo[2] == null)
            {
                $("#chart").html(""); 
                window.parent.ChartsNodataMsg();    
                return;
            }
            else
            {
                unitarr=chartsInfo[2].split(',');
                charheight=unitarr.length;
                charheight=charheight*40+150;
            }
            
            chart = new Highcharts.Chart({
					chart: {
						renderTo: 'chart',
						defaultSeriesType: 'bar',
						height:charheight
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
								    '年度值: '+ this.y;
						}
					},
					exporting: {
					    enabled: false
					},
				    plotOptions: {
						bar: {
							dataLabels: {
								enabled: true
							},
							cursor: 'pointer',
							point: {
										events: {
											'click': function() {
											         if(Organ == '02') {
											               for (var i = 0; i < hs.expanders.length; i++) {
                                                                if (hs.expanders[i] != null) {
                                                                    if (hs.expanders[i].maincontentId == "Div-"+ this.series.name + this.category) {
                                                                        return;
                                                                    }
                                                                }
                                                            }
                                                            createDiv(this.series.name,this.category);
                                                            var DivId = 'Div-' + this.series.name + this.category;
														    hs.htmlExpand(null, {
													        headingText: year+'年'+this.category+'人员趋势图',
										                    preserveContent:true,
										                    maincontentId: 'Div-' + this.series.name + this.category,
															    width: 600,
															    height:600
														    });
														    refreshPopUpCharts(this.id,'line',Organ,year,"my-content-" + this.series.name + this.category,guideCode);
											            } 
												}
											}
								   }
							}
					},
					credits: {
						enabled: false
					},
				    series: eval("(["+chartsInfo[3]+"])")
				});
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
        
        //弹出明细页面
        var refreshPopUpCharts = function(unit_code,ChartsType,PageOrgan,year,divId,GuideCode) {
            var chartsterm = unit_code+'*'+PageOrgan+'*'+ChartsType+'*'+year+'*'+GuideCode;
            //获取数据方法
            var url = "HightCharts.aspx/GetPopUpDate";
            $.ajax({ 
                 type: "POST", 
                 url: url, //注意调用方式，同样方式可以调用webservice 
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
                           CreatePopUpPieCodeCharts(chartInfo,divId);
                        }
                        if(chartsType == 'bar') {
                            CreatePopUpBarCharts(chartInfo,divId);
                        }
                        if(chartsType == 'line') {
                            CreatePopUpLineCharts(chartInfo,divId);
                        }
                 }, 
                 error: function(xhr,data,e) {
                 } 
             });
        }
       
        //生成弹出趋势图
        var CreatePopUpLineCharts = function(data,divId) {
               var chartsInfo = data.split('$');
               var Categories = chartsInfo[2];
               var SeriesData = chartsInfo[3].split('*');
               var GuideCode = chartsInfo[4];
               
               if(chartsInfo[3] == '' || chartsInfo[3] == null) 
               {
                    window.parent.ChartsNodataMsg();      
                    return;
               }
               
               //chart = new Highcharts.Chart({
               var options  = {
			        chart: {
				        renderTo: divId,
				        defaultSeriesType: 'line',
				        ignoreHiddenSeries: false
			        },
                    title: {
				        text: chartsInfo[1]
			        },
                    xAxis: {
                      categories: []
                    },
                    tooltip: {
						formatter: function() {
							return '<b>'+ this.series.name +'</b><br/>'+
                                           '指标值:' + this.y;
						}
					},
                    legend: {
                        borderWidth: 0,
                        itemWidth: 78
                        //margin: 100
                    },
                    	plotOptions: {
						line: {
							cursor: 'pointer',
							point: {
										events: {
											'click': function() {
											                if(Organ == '01') {
                                                                for (var i = 0; i < hs.expanders.length; i++) {
                                                                    if (hs.expanders[i] != null) {
                                                                        if (hs.expanders[i].maincontentId == "Div-"+ this.series.name + this.category) {
                                                                            return;
                                                                        }
                                                                    }
                                                                }
                                                                createDiv(this.series.name,this.category);
													            hs.htmlExpand(null, {
												                    headingText: '<select name=\'sldd\'onchange=\'PopUpChartChange("'+this.id+'",this,"'+CurOrgan+'","'+this.category+'","'+"my-content-" + this.series.name + this.category+'","'+GuideCode+'");\'><option value=\'pie\'>饼型图</option><option value=\'bar\' selected=true>柱型图</option></select>',
									                                preserveContent:true,
									                                maincontentId: "Div-" + this.series.name + this.category,
														                width: 600,
														                height:600
														        });   
														        refreshPopUpCharts(this.id,'bar',Organ,this.category,"my-content-" + this.series.name + this.category,GuideCode);
														        
														        //人当年趋势图
											                } else if(Organ == '03') {
                                                                for (var i = 0; i < hs.expanders.length; i++) {
                                                                    if (hs.expanders[i] != null) {
                                                                        if (hs.expanders[i].maincontentId == "Div-"+ this.series.name + this.category) {
                                                                            return;
                                                                        }
                                                                    }
                                                                }
                                                                createDiv(this.series.name,this.category);
                                                                //当月的拄型图
														        hs.htmlExpand(null, {
													                headingText:'<select name=\'sldd\'onchange=\'PopUpChartChange("'+this.id+'",this,"'+CurOrgan+'","'+this.category+'","'+"my-content-" + this.series.name + this.category+'","'+GuideCode+'");\'><option value=\'pie\'>饼型图</option><option value=\'bar\' selected=true>柱型图</option></select>',
										                            preserveContent:true,
										                            maincontentId: "Div-" + this.series.name + this.category,
															            width: 600,
															            height:600
														        });   
														        refreshPopUpCharts(this.id,'line',Organ,this.category,"my-content-" + this.series.name + this.category,GuideCode);
											                } else if(Organ == '02') {
                                                                for (var i = 0; i < hs.expanders.length; i++) {
                                                                    if (hs.expanders[i] != null) {
                                                                        if (hs.expanders[i].maincontentId == "Div-"+ this.series.name + this.category) {
                                                                            return;
                                                                        }
                                                                    }
                                                                }
                                                                createDiv(this.series.name,this.category);
														        hs.htmlExpand(null, {
													                headingText: '<select name=\'sldd\'onchange=\'PopUpChartChange("'+this.id+'",this,"'+CurOrgan+'","'+this.category+'","'+"my-content-" + this.series.name + this.category+'","'+GuideCode+'");\'><option value=\'pie\'>饼型图</option><option value=\'bar\' selected=true>柱型图</option></select>',
										                            preserveContent:true,
										                            maincontentId: "Div-" + this.series.name + this.category,
															            width: 600,
															            height:600
														        });   
														        refreshPopUpCharts(this.id,'bar',Organ,this.category,"my-content-" + this.series.name + this.category,GuideCode);
											                }
 
												}
											}
								   }
							}
						},
                    
                    series: []
                };
               var series = {data:[]};
               for(var j=0;j<Categories.split(',').length;j++) {
                 options.xAxis.categories.push(Categories.split(',')[j]);
               }
               
               for(var i=0;i<SeriesData.length;i++) {
                     options.series.push(eval("(" + SeriesData[i] + ")"));
               }
               chart = new Highcharts.Chart(options);
//               chart.xAxis[0].setCategories(eval("(["+Categories+"])"));
//               for(var i=0;i<SeriesData.length;i++) {
//                     chart.addSeries(eval("(" + SeriesData[i] + ")"));
//               }
               chart.yAxis[0].axisTitle.hide();
        }
        
        //生成弹出条形图
        var CreatePopUpBarCharts = function(data,divId){
        var chartsInfo = data.split('$');
        var GuideCode = chartsInfo[4];
        if(chartsInfo[3] == '' || chartsInfo[3] == null) 
        {
            window.parent.ChartsNodataMsg();
            return;
        }
        chart = new Highcharts.Chart({
					chart: {
						renderTo: divId,
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
							cursor: 'pointer',
							point: {
										events: {
											'click': function() {
		                                                 if((Organ == '01'|| Organ == '02') && GuideCode.indexOf('3') != 0) {
                                                            //当年人的趋势图
                                                                for (var i = 0; i < hs.expanders.length; i++) {
                                                                    if (hs.expanders[i] != null) {
                                                                        if (hs.expanders[i].maincontentId == "Div-"+ this.series.name + this.category) {
                                                                            return;
                                                                        }
                                                                    }
                                                                }
                                                                createDiv(this.series.name,this.category);
												                hs.htmlExpand(null, {
											                        headingText: year+'年'+this.category+'人员趋势图',
								                                    preserveContent:true,
								                                    maincontentId: "Div-" + this.series.name + this.category,
													                    width: 600,
													                    height:600
												                });
												                refreshPopUpCharts(this.id,'line',Organ,year,"my-content-" + this.series.name + this.category,GuideCode);
		                                                 } 
												}
										}
								   }
							}
					},
				    series: [eval("(" + chartsInfo[3] + ")")]
				});
       }
        
        //生成弹出饼图图表
        var CreatePopUpPieCodeCharts = function(data,divId) { 
               var chartsInfo = data.split('$');
               var GuideCode = chartsInfo[4];
               if(chartsInfo[3] == '' || chartsInfo[3] == null) 
               {
                    window.parent.ChartsNodataMsg();
                    return;
               }
			   chart = new Highcharts.Chart({
					chart: {
						renderTo: divId,
						margin: [50, 200, 60, 200]
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
							},
							point: {
							events: {
											'click': function() {
											    if((Organ == '02'|| Organ == '01') && GuideCode.indexOf('3') != 0) {
		                                                for (var i = 0; i < hs.expanders.length; i++) {
                                                            if (hs.expanders[i] != null) {
                                                                if (hs.expanders[i].maincontentId == "Div-"+ this.series.name + this.category) {
                                                                    return;
                                                                }
                                                            }
                                                        }
                                                        createDiv(this.series.name,this.category);
                                                        var DivId = 'Div-' + this.series.name + this.category;
														hs.htmlExpand(null, {
													    headingText: year+'年人员趋势图',
										                preserveContent:true,
										                maincontentId: DivId,
															width: 600,
															height:600
														});
														refreshPopUpCharts(this.id,'line',Organ,year,"my-content-" + this.series.name + this.category,GuideCode);
											    }
									      }
							        }
							 }
							
						}
					},
				    series: [eval("(" + chartsInfo[3] + ")")]
				});
        }
        
        //构成图表区域
        var createDiv= function(x,y){
            $("#my-content-" + x + y).empty();
            $("#my-content-" + x + y).remove();
            $("#Div-" + x + y).empty();
            $("#Div-" + x + y).remove();
            var ParentDiv = $("<div id='Div-" + x + y + "'></div>");
            ParentDiv.appendTo('body');
            var ContentDiv = $("<div id='my-content-" + x + y + "' ></div>");
            ContentDiv.appendTo("#Div-" + x + y);
        }
        
        //构成弹出图表区域
        var PopUpChartChange = function(id,item,Organ,year,divName,guideCode) {
               refreshPopUpCharts(id,item.options[item.selectedIndex].value,Organ,year,divName,guideCode)     
        }
        
        //空字符处理
        var EmptyChart = function() {
            $("#chart").html(""); 
        }
        
    </script>

</head>
<body>
    <div id="loading" style="display: none; fixed !important; position: absolute; top: 0;
        left: 0; height: 150%; width: 100%; z-index: 999; background: #000 url(../resources/images/load.gif) no-repeat center center;
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
