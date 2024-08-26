<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="flow_chart.aspx.cs" Inherits="GoldNet.JXKP.cbhs.datagather.flow_chart" %>

<%@ Register Assembly="Goldnet.Ext.Web" Namespace="Goldnet.Ext.Web" TagPrefix="ext" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <link rel="stylesheet" type="text/css" href="/resources/jquery/themes/default/easyui.css" />
    <link rel="stylesheet" type="text/css" href="/resources/jquery/themes/icon.css" />
    <script type="text/javascript" src="/resources/ExampleTab.js"></script>
    <script type="text/javascript" src="/resources/jquery/jquery-1.4.4.min.js"></script>
    <script type="text/javascript" src="/resources/jquery/jquery.easyui.min.js"></script>
    
    <script>
       var myDate = new Date();
       var year = myDate.getFullYear(); 
       var month = myDate.getMonth() + 1;
       if(month<10)
            month ='0'+month;
		$(function(){
		    createchart(year+month);
		});
	   function reloaddata()
       {
           year=years.getValue();
           month=months.getValue();
           var queryParams = $('#test').datagrid('options').queryParams; 
           queryParams.datetime = year+month; 
           queryParams.falgs = new Date().getTime(); 
           $('#test').treegrid('reload');
       }
       function createchart(year_month)
       {
       $('#test').treegrid({
				width:1000,
				height:500,
				nowrap: false,
				rownumbers: true,
				animate:true,
				collapsible:true,
				method: "GET",
				url:'../WebService/GetJson.ashx',
				queryParams: { datetime: year_month ,falgs:new Date().getTime()},
				idField:'name',
				treeField:'name',
				columns:[[
					{field:'name',title:'名称',width:180},
					{field:'step',title:'进度',width:80},
					{field:'stat',title:'状态',width:80,
					formatter:function(value){
					    if (value=='完成')
					       return '<span style="color:green">'+value+'</span>';
					    else 
					       return '<span style="color:red">'+value+'</span>';    
		                	
		                }},
					{field:'datavalue',title:'值',width:80},
					{field:'operate',title:'操作员',width:80},
					{field:'operatedate',title:'操作时间',width:100},
					{field:'memo',title:'备注',width:120},
					{field:'url',title:'',width:120,
					    formatter:function(value){
					        if(value==null||value=='')
					            return '';
					        else
		                	    return '<a href="" style="color:blue;" onclick="window.open(\''+value+'\' );">查看</a>';
		                }}
				]]
			});
       }
    </script>

</head>
<body>
    <form id="form1" runat="server">
    <div id="chart">
        <ext:ScriptManager ID="ScriptManager1" runat="server" AjaxMethodNamespace="CompanyX" />
        <ext:Toolbar ID="Toolbar1" runat="server">
            <Items>
                <ext:ComboBox ID="years" runat="server" Width="60" AllowBlank="true" EmptyText="请选择年..."
                    FieldLabel="年">
                </ext:ComboBox>
                <ext:ToolbarTextItem ID="dd1Name" runat="server" Text="年 " />
                <ext:ComboBox ID="months" runat="server" Width="60" AllowBlank="true" EmptyText="请选择月..."
                    FieldLabel="月">
                    <Items>
                        <ext:ListItem Text="01" Value="01" />
                        <ext:ListItem Text="02" Value="02" />
                        <ext:ListItem Text="03" Value="03" />
                        <ext:ListItem Text="04" Value="04" />
                        <ext:ListItem Text="05" Value="05" />
                        <ext:ListItem Text="06" Value="06" />
                        <ext:ListItem Text="07" Value="07" />
                        <ext:ListItem Text="08" Value="08" />
                        <ext:ListItem Text="09" Value="09" />
                        <ext:ListItem Text="10" Value="10" />
                        <ext:ListItem Text="11" Value="11" />
                        <ext:ListItem Text="12" Value="12" />
                    </Items>
                </ext:ComboBox>
                <ext:ToolbarTextItem ID="ToolbarTextItem1" runat="server" Text="月 " />
                <ext:ToolbarSeparator ID="ToolbarSeparator1" runat="server" />
                <ext:Button ID="Button_create" runat="server" Text="查看" Icon="DatabaseGo">
                    <Listeners>
                        <Click Handler="reloaddata();" />
                    </Listeners>
                </ext:Button>
            </Items>
        </ext:Toolbar>
        <div id="aaa">
            <table id="test"></table>
        </div>
    </div>
    </form>
</body>
</html>
