<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="income_create.aspx.cs"
    Inherits="GoldNet.JXKP.cbhs.datagather.income_create" %>

<%@ Register Assembly="Goldnet.Ext.Web" Namespace="Goldnet.Ext.Web" TagPrefix="ext" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>

    <script type="text/javascript" src="/resources/hightChartsJs/jquery.min.js"></script>

    <script type="text/javascript" src="/resources/hightChartsJs/highcharts.js"></script>

    <script type="text/javascript" src="/resources/hightChartsJs/exporting.js"></script>

    <script language="javascript" type="text/javascript">
        var chart;
        var CreatePieCharts = function(render_to,series_str) {
           chart = new Highcharts.Chart({
              chart: {
                 renderTo: render_to,
                 margin: [10, 10, 10, 40]
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
            document.getElementById('chart2').innerHTML="";
        }
        function refreshCharts1(series_str) {
             
             CreatePieCharts('chart1',series_str);   
        }
        function refreshCharts2(series_str) {
             
             CreatePieCharts('chart2',series_str);   
        }
        
        var rmbMoney = function(v) {
                   v = (Math.round((v - 0) * 100)) / 100;
                   v = (v == Math.floor(v)) ? v + ".00" : ((v * 10 == Math.floor(v * 10)) ? v + "0" : v);
                   v = String(v);
                   var ps = v.split('.');
                   var whole = ps[0];
                   var sub = ps[1] ? '.' + ps[1] : '.00';
                   var r = /(\d+)(\d{3})/;
                   while (r.test(whole)) {
                       whole = whole.replace(r, '$1' + ',' + '$2');
                   }
                   v = whole + sub;
                   if (v.charAt(0) == '-') {
                       return '-' + v.substr(1);
                   }
                   return v;
        }
        
        var CheckForm = function() {
        if (tfBName.validate() == false) {
                return false;
            }                  
            return true;
        }
        
        function setHidden() {
            Btn_BatStart.hide();
            BtnCancel.show();
            //BtnClose.disable();
            progressTip.show();
        }
        
        function setCancelHidd() {
            BtnCancel.hide();
            Btn_BatStart.show();
            //BtnClose.enable();
            progressTip.hide();
        }
           
    </script>
   

</head>
<body>
    <form id="form1" runat="server">
    <div>
        <ext:ScriptManager ID="ScriptManager1" runat="server" AjaxMethodNamespace="CompanyX" />
        <ext:Store ID="Store1" runat="server" WarningOnDirty="false">
            <Reader>
                <ext:JsonReader ReaderID="TASK_ID">
                    <Fields>
                        <ext:RecordField Name="TASK_ID" Type="String" Mapping="TASK_ID" />
                        <ext:RecordField Name="TASK_TIME" Type="String" Mapping="TASK_TIME" />
                        <ext:RecordField Name="TASK_NAME" Type="String" Mapping="TASK_NAME" />
                        <ext:RecordField Name="TASK_DEPICT" Type="String" Mapping="TASK_DEPICT" />
                        <ext:RecordField Name="TASK_STATE" Type="String" Mapping="TASK_STATE" />
                    </Fields>
                </ext:JsonReader>
            </Reader>
        </ext:Store>
        <ext:Store ID="Store2" runat="server" WarningOnDirty="false">
            <Reader>
                <ext:JsonReader ReaderID="ITEM_ID">
                    <Fields>
                        <ext:RecordField Name="ITEM_NAME" Type="String" Mapping="ITEM_NAME" />
                        <ext:RecordField Name="COSTS" Type="Float" Mapping="COSTS" />
                        <ext:RecordField Name="CHARGES" Type="Float" Mapping="CHARGES" />
                        <ext:RecordField Name="COUNT_INCOME" Type="Float" Mapping="COUNT_INCOME" />
                    </Fields>
                </ext:JsonReader>
            </Reader>
        </ext:Store>
        <ext:Store ID="Store3" runat="server" WarningOnDirty="false">
            <Reader>
                <ext:JsonReader ReaderID="ITEM_ID">
                    <Fields>
                        <ext:RecordField Name="ITEM_NAME" Type="String" Mapping="ITEM_NAME" />
                        <ext:RecordField Name="COSTS" Type="Float" Mapping="COSTS" />
                        <ext:RecordField Name="CHARGES" Type="Float" Mapping="CHARGES" />
                        <ext:RecordField Name="COUNT_INCOME" Type="Float" Mapping="COUNT_INCOME" />
                    </Fields>
                </ext:JsonReader>
            </Reader>
        </ext:Store>
        <ext:ViewPort ID="ViewPort1" runat="server">
            <Body>
                <ext:BorderLayout ID="BorderLayout1" runat="server">
                    <North>
                        <ext:Toolbar runat="server">
                            <Items>
                                <ext:ComboBox ID="years" runat="server" Width="60" AllowBlank="true" EmptyText="请选择年..."
                                    FieldLabel="年">
                                    <%-- <AjaxEvents>
                                        <Select OnEvent="IncomeItem_SelectOnChange">
                                            <EventMask Msg='载入中...' ShowMask="true" />
                                        </Select>
                                    </AjaxEvents>--%>
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
                                    <%--<AjaxEvents>
                                        <Select OnEvent="IncomeItem_SelectOnChange">
                                            <EventMask Msg='载入中...' ShowMask="true" />
                                        </Select>
                                    </AjaxEvents>--%>
                                </ext:ComboBox>
                                <ext:ToolbarTextItem ID="ToolbarTextItem1" runat="server" Text="月 " />
                                <ext:Button ID="Button2" runat="server" Text="查询" Icon="Zoom">
                                    <AjaxEvents>
                                        <Click OnEvent="IncomeItem_SelectOnChange" Timeout="200000000">
                                            <EventMask Msg="载入中..." ShowMask="true" />
                                        </Click>
                                    </AjaxEvents>
                                </ext:Button>
                                <ext:ToolbarSeparator ID="ToolbarSeparator1" runat="server" />
                                <ext:ComboBox ID="IncomeItem" runat="server" Width="80" AllowBlank="true">
                                    <AjaxEvents>
                                        <Select OnEvent="IncomeItem_SelectOnChange" Timeout="20000000">
                                            <EventMask Msg='载入中...' ShowMask="true" />
                                        </Select>
                                    </AjaxEvents>
                                </ext:ComboBox>
                                <ext:Button ID="Button_create" runat="server" Text="门诊收入生成" Icon="DatabaseGo" Hidden="true">
                                    <AjaxEvents>
                                        <Click OnEvent="Button_create_click" Timeout="120000">
                                            <Confirmation ConfirmRequest="true" Title="系统提示" Message="生成收入需要花费一些时间,<br/>是否继续?" />
                                            <ExtraParams>
                                            </ExtraParams>
                                            <EventMask Msg="载入中..." ShowMask="true" />
                                        </Click>
                                    </AjaxEvents>
                                </ext:Button>
                                <ext:Button ID="Button1" runat="server" Text="住院收入生成" Icon="DatabaseGo" Hidden="true">
                                    <AjaxEvents>
                                        <Click OnEvent="Button_create_click_inp" Timeout="120000">
                                            <Confirmation ConfirmRequest="true" Title="系统提示" Message="生成收入需要花费一些时间,<br/>是否继续?" />
                                            <ExtraParams>
                                            </ExtraParams>
                                            <EventMask Msg="载入中..." ShowMask="true" />
                                        </Click>
                                    </AjaxEvents>
                                </ext:Button>
                                <ext:ToolbarSeparator ID="ToolbarSeparator3" runat="server" />
                                <ext:Button ID="Buttond" runat="server" Text="科室数据核算" Icon="DatabaseGo" Hidden="true">
                                    <AjaxEvents>
                                        <Click OnEvent="Button_create_click_dept" Timeout="120000">
                                            <Confirmation ConfirmRequest="true" Title="系统提示" Message="生成收入需要花费一些时间,<br/>是否继续?" />
                                            <ExtraParams>
                                            </ExtraParams>
                                            <EventMask Msg="载入中..." ShowMask="true" />
                                        </Click>
                                    </AjaxEvents>
                                </ext:Button>
                                <ext:Button ID="buidequality" runat="server" Text="收入数据生成" Icon="Build">
                                    <AjaxEvents>
                                        <Click OnEvent="buide_Click">
                                        </Click>
                                    </AjaxEvents>
                                </ext:Button>
                                <ext:Button ID="Button3" runat="server" Text="数据提取" Icon="PlayGreen">
                                    <AjaxEvents>
                                        <Click OnEvent="getdata_Click">
                                        </Click>
                                    </AjaxEvents>
                                </ext:Button>
                            </Items>
                        </ext:Toolbar>
                    </North>
                    <Center>
                        <ext:TabPanel ID="TabPanel1" runat="server" ActiveTabIndex="0" Width="810" Height="330"
                            BodyBorder="false">
                            <Tabs>
                                <ext:Tab ID="Tab1" runat="server" Title="分解前数据" Height="300" AutoScroll="true">
                                    <Body>
                                        <table>
                                            <tr>
                                                <td>
                                                    <ext:Panel ID="Panel1" runat="server" Border="false" AutoScroll="true">
                                                        <Body>
                                                            <div id='chart1' style="width: 450px; height: 300px">
                                                            </div>
                                                        </Body>
                                                    </ext:Panel>
                                                </td>
                                                <td>
                                                    <ext:GridPanel ID="GridPanel2" Title="收入数据" runat="server" StoreID="Store2" StripeRows="true"
                                                        TrackMouseOver="true" Width="500" Height="300" AutoScroll="true">
                                                        <ColumnModel ID="ColumnModel2" runat="server">
                                                            <Columns>
                                                                <ext:Column ColumnID="ITEM_NAME" Header="<div style='text-align:center;'>名称</div>"
                                                                    Width="100" Align="left" Sortable="true" DataIndex="ITEM_NAME" MenuDisabled="true" />
                                                                <ext:Column ColumnID="COSTS" Header="<div style='text-align:center;'>总收入</div>" Width="120"
                                                                    Align="Right" Sortable="true" DataIndex="COSTS" MenuDisabled="true">
                                                                    <Renderer Fn="rmbMoney" />
                                                                </ext:Column>
                                                                <ext:Column ColumnID="CHARGES" Header="<div style='text-align:center;'>实际收入</div>"
                                                                    Width="120" Align="Right" Sortable="true" DataIndex="CHARGES" MenuDisabled="true">
                                                                    <Renderer Fn="rmbMoney" />
                                                                </ext:Column>
                                                                <ext:Column ColumnID="COUNT_INCOME" Header="<div style='text-align:center;'>计价收入</div>"
                                                                    Width="120" Align="Right" Sortable="true" DataIndex="COUNT_INCOME" MenuDisabled="true">
                                                                    <Renderer Fn="rmbMoney" />
                                                                </ext:Column>
                                                            </Columns>
                                                        </ColumnModel>
                                                        <SelectionModel>
                                                            <ext:RowSelectionModel ID="RowSelectionModel1" runat="server" SingleSelect="true">
                                                            </ext:RowSelectionModel>
                                                        </SelectionModel>
                                                    </ext:GridPanel>
                                                </td>
                                            </tr>
                                        </table>
                                    </Body>
                                </ext:Tab>
                                <ext:Tab ID="Tab2" runat="server" Title="分解后数据" Height="300" AutoScroll="true">
                                    <Body>
                                        <table>
                                            <tr>
                                                <td>
                                                    <ext:Panel ID="Panel2" runat="server" Border="false" AutoScroll="true">
                                                        <Body>
                                                            <div id='chart2' style="width: 450px; height: 300px">
                                                            </div>
                                                        </Body>
                                                    </ext:Panel>
                                                </td>
                                                <td>
                                                    <ext:GridPanel ID="GridPanel3" Title="收入数据" runat="server" StoreID="Store3" StripeRows="true"
                                                        TrackMouseOver="true" Width="500" Height="300" AutoScroll="true">
                                                        <ColumnModel ID="ColumnModel3" runat="server">
                                                            <Columns>
                                                                <ext:Column ColumnID="ITEM_NAME" Header="<div style='text-align:center;'>名称</div>"
                                                                    Width="100" Align="left" Sortable="true" DataIndex="ITEM_NAME" MenuDisabled="true" />
                                                                <ext:Column ColumnID="COSTS" Header="<div style='text-align:center;'>总收入</div>" Width="120"
                                                                    Align="Right" Sortable="true" DataIndex="COSTS" MenuDisabled="true">
                                                                    <Renderer Fn="rmbMoney" />
                                                                </ext:Column>
                                                                <ext:Column ColumnID="CHARGES" Header="<div style='text-align:center;'>实际收入</div>"
                                                                    Width="120" Align="Right" Sortable="true" DataIndex="CHARGES" MenuDisabled="true">
                                                                    <Renderer Fn="rmbMoney" />
                                                                </ext:Column>
                                                                <ext:Column ColumnID="COUNT_INCOME" Header="<div style='text-align:center;'>计价收入</div>"
                                                                    Width="120" Align="Right" Sortable="true" DataIndex="COUNT_INCOME" MenuDisabled="true">
                                                                    <Renderer Fn="rmbMoney" />
                                                                </ext:Column>
                                                            </Columns>
                                                        </ColumnModel>
                                                        <SelectionModel>
                                                            <ext:RowSelectionModel ID="RowSelectionModel2" runat="server" SingleSelect="true">
                                                            </ext:RowSelectionModel>
                                                        </SelectionModel>
                                                    </ext:GridPanel>
                                                </td>
                                            </tr>
                                        </table>
                                    </Body>
                                </ext:Tab>
                            </Tabs>
                        </ext:TabPanel>
                    </Center>
                    <South>
                        <ext:GridPanel ID="GridPanel1" Title="执行记录" runat="server" StoreID="Store1" StripeRows="true"
                            BodyBorder="false" AutoDataBind="true" TrackMouseOver="true" Width="810" Height="200">
                            <ColumnModel ID="ColumnModel1" runat="server">
                                <Columns>
                                    <ext:Column ColumnID="TASK_TIME" Header="<div style='text-align:center;'>时间</div>"
                                        Width="150" Align="left" Sortable="false" DataIndex="TASK_TIME" MenuDisabled="true" />
                                    <ext:Column ColumnID="TASK_NAME" Header="<div style='text-align:center;'>任务名称</div>"
                                        Width="150" Align="left" Sortable="false" DataIndex="TASK_NAME" MenuDisabled="true" />
                                    <ext:Column ColumnID="TASK_DEPICT" Header="<div style='text-align:center;'>任务描述</div>"
                                        Width="370" Align="left" Sortable="false" DataIndex="TASK_DEPICT" MenuDisabled="true" />
                                    <ext:Column ColumnID="TASK_STATE" Header="<div style='text-align:center;'>状态</div>"
                                        Width="80" Align="left" Sortable="false" DataIndex="TASK_STATE" MenuDisabled="true" />
                                </Columns>
                            </ColumnModel>
                            <SelectionModel>
                                <ext:RowSelectionModel ID="RowSelectionModel3" runat="server" SingleSelect="true">
                                </ext:RowSelectionModel>
                            </SelectionModel>
                        </ext:GridPanel>
                    </South>
                </ext:BorderLayout>
            </Body>
        </ext:ViewPort>
    </div>
    <ext:Window ID="BuideWin" runat="server" Icon="Add" Title="收入数据生成" Width="350" Height="200"
        AutoShow="false" Modal="true" CenterOnLoad="true" AutoScroll="false" ShowOnLoad="false"
        Closable="false" Resizable="false" StyleSpec="background-color:Transparent;"
        BodyStyle="background-color:Transparent;" CloseAction="Hide">
    </ext:Window>
    <ext:Window runat="server" ID="Win_BatchInit" AutoShow="false" ShowOnLoad="false"
        Modal="true" Resizable="false" Title="收入数据提取" CenterOnLoad="true" AutoScroll="false"
        Width="350" Height="200" CloseAction="Hide" AnimateTarget="Btn_BatInit" Icon="TagPink"
        BodyStyle="padding:2px;">
        <Body>
            <table align="center">
                <tr>
                    <td colspan="2">
                        <ext:Label runat="server" ID="progressTip" Text="注意：生成数据需要大约20分钟时间，在此时间内请不要关闭您的浏览器或者刷新页面。"
                            AutoWidth="true">
                        </ext:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        收入月份：
                    </td>
                    <td>
                        <table>
                            <tr>
                                <td>
                                    <ext:ComboBox ID="cbbBeginYear" runat="server" Width="70" ReadOnly="true">
                                    </ext:ComboBox>
                                </td>
                                <td>
                                    年
                                </td>
                                <td>
                                    <ext:ComboBox ID="cbbBeginMonth" runat="server" Width="50" ReadOnly="true">
                                    </ext:ComboBox>
                                </td>
                                <td>
                                    月
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td>
                        <p>
                            生成进度：
                        </p>
                    </td>
                    <td>
                        <ext:ProgressBar ID="Progress1" runat="server" Width="200">
                        </ext:ProgressBar>
                    </td>
                </tr>
            </table>
            <ext:TaskManager ID="TaskManager1" runat="server">
                <Tasks>
                    <ext:Task TaskID="longactionprogress" Interval="1000" AutoRun="false" OnStart="#{Btn_BatStart}.setDisabled(true); "
                        OnStop="#{Btn_BatStart}.setDisabled(false);">
                        <AjaxEvents>
                            <Update OnEvent="RefreshProgress" />
                        </AjaxEvents>
                    </ext:Task>
                </Tasks>
            </ext:TaskManager>
        </Body>
        <BottomBar>
            <ext:Toolbar ID="Toolbar1" runat="server">
                <Items>
                    <ext:ToolbarFill ID="ToolbarFill2" runat="server" />
                    <ext:ToolbarButton ID="Btn_BatStart" runat="server" Icon="PlayBlue" Text="生成">
                        <AjaxEvents>
                            <Click OnEvent="Btn_BatStart_Click" Timeout="1200000">
                            </Click>
                        </AjaxEvents>
                    </ext:ToolbarButton>
                    <ext:ToolbarSeparator ID="ToolbarSeparator2" runat="server" />
                    <ext:ToolbarButton ID="Btn_BatCancel" runat="server" Icon="Cancel" Text="取消">
                        <Listeners>
                            <Click Handler="Win_BatchInit.hide();"></Click>
                        </Listeners>
                    </ext:ToolbarButton>
                </Items>
            </ext:Toolbar>
        </BottomBar>
        <Listeners>
            <Show Handler="this.dirty = false;" />
            <BeforeHide Handler="
                    if ((this.dirty==false)&&(Btn_BatCancel.text=='取消') ){
                        Ext.Msg.confirm('系统提示', '注意:任务正在运行，确定取消任务并退出吗？', function (btn) { 
                            if(btn == 'yes') { 
                                this.dirty = true;
                                this.hide(); 
                            } 
                        }, this);
                        return false;    
                    }" />
            <Hide Handler="TaskManager1.stopAll();" />
        </Listeners>
        <AjaxEvents>
            <Hide OnEvent="CloseBatInit" />
        </AjaxEvents>
    </ext:Window>
    </form>
</body>
</html>
