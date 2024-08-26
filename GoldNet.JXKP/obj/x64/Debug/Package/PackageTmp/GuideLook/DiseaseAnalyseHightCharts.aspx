<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DiseaseAnalyseHightCharts.aspx.cs"
    Inherits="GoldNet.JXKP.GuideLook.DiseaseAnalyseHightCharts" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>病种分析图表</title>
    <script type="text/javascript" src="/resources/hightChartsJs/jquery.min.js"></script>
    <script type="text/javascript" src="/resources/hightChartsJs/highcharts.js"></script>
    <script type="text/javascript" src="/resources/hightChartsJs/exporting.js"></script>
    <script type="text/javascript">                
        var chart;
        function refreshCharts(name,type) {
            showLoadMask();
            var ChartsName = escape(name);
            $.ajax({ 
                 type: "POST", 
                 url: "DiseaseAnalyseHightCharts.aspx/GetData", //注意调用方式，同样方式可以调用webservice 
                 data:'{\'name\':\''+ChartsName+'\',\'type\':\''+type+'\'}', //在这里可以设置需要传递的参数 
                 contentType: "application/json; charset=utf-8",
                 dataType: "json", 
                 beforeSend: function(xhr) {  xhr.setRequestHeader("Content-type",    
                                                                   "application/json; charset=utf-8");   
                                              },
                 success: function(data) { // 替换返回内容 
                        $("#chart").empty();
                        var ParentDiv = $("div id='chart'></div>");
                        ParentDiv.appendTo('chartTable');
                        CreateCharts(data.d);       
                 }, 
                 error: function(xhr,data,e) { 
                    alert(data); 
                 } 
             });
            hideLoadMask();
        }
        
        var CreateCharts = function(ChartsInfo) 
        {
                var lines = ChartsInfo.split('$'); 
                
                var title = lines[0];
                
                var Categories = lines[1];
                
                var SeriesData = lines[2].split('*');
                
                var CaleInfo = lines[3].split(',');
                chart = new Highcharts.Chart({
					    chart: {
						    renderTo: 'chart',
						    defaultSeriesType: 'line',
						    ignoreHiddenSeries: false
					    },
                        title: {
						    text: title
					    },
                        tooltip: {
						    formatter: function() {
							    return ''+
								        '金额: '+ this.y;
						      }
					    },
                        xAxis: {
                          categories: []
                        },
                        series: []
                    });
                 
                 
               chart.xAxis[0].setCategories(eval("(["+Categories+"])"));
                
               for(var i=0;i<SeriesData.length;i++) {
                     chart.addSeries(eval("(" + SeriesData[i] + ")"));
                }
                
                chart.yAxis[0].axisTitle.hide();
                
                
                var costs = document.getElementById("costs");
                
                costs.innerHTML = CaleInfo[0];
                
                var benefit = document.getElementById("benefit");
                
                benefit.innerHTML = CaleInfo[1];
                
                var perhos = document.getElementById("perhos");
                
                perhos.innerHTML = CaleInfo[2];
                
                var inhos = document.getElementById("inhos");
                
                inhos.innerHTML = CaleInfo[4];
                
                var numbers = document.getElementById("numbers");
                
                numbers.innerHTML = CaleInfo[3];
                
                var bedcosts = document.getElementById("bedcosts");
                
                bedcosts.innerHTML = CaleInfo[5];
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
        
       
             
    </script>

    <style type="text/css">
        /* 标准表格属性 */TABLE, TR
        {
            font-family: "Tahoma" , "宋体" , "Verdana";
            font-size: 12px;
        }
        .gs-pagetopframe
        {
            border-bottom-width: 1px;
            border-bottom-style: solid;
            border-bottom-color: #D6DBEF;
        }
        -- ></style>
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

    <table width="100%">
        <tr>
            <td style="width:100%" align="center">
                <img height="12" src="/resources/images/NCDOTS.GIF" width="12">
                平均收入：<asp:Label ID="costs" runat="server" Text="0"></asp:Label>&nbsp; &nbsp;
                <img height="12" src="/resources/images/NCDOTS.GIF" width="12">
                平均效益：<asp:Label ID="benefit" runat="server" Text="0"></asp:Label> &nbsp; &nbsp;
                <img height="12" src="/resources/images/NCDOTS.GIF" width="12">
                术前平均住院日：<asp:Label ID="perhos" runat="server" Text="0"></asp:Label>&nbsp; &nbsp;
                <img height="12" src="/resources/images/NCDOTS.GIF" width="12">
                平均住院日：<asp:Label ID="inhos" runat="server" Text="0"></asp:Label>&nbsp; &nbsp;
                <img height="12" src="/resources/images/NCDOTS.GIF" width="12">
                例数：<asp:Label ID="numbers" runat="server" Text="0"></asp:Label>
            </td>
        </tr>
        <tr>
            <td align="center">
                <img height="14" src="/resources/images/warn16.gif" width="14">
                床均效益：<asp:Label ID="bedcosts" runat="server" Text="0" ForeColor="red"></asp:Label>
            </td>
        </tr>
    </table>
    <br/>
    <table width="100%">
        <tr>
            <td width="100%" valign="top" id="chartTable">
                <div id="chart">
                </div>
            </td>
        </tr>
    </table>
</body>
</html>
