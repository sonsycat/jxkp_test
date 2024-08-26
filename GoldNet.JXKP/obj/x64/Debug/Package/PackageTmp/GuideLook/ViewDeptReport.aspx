<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ViewDeptReport.aspx.cs"
    Inherits="GoldNet.JXKP.GuideLook.ViewDeptReport" %>

<%@ Register Assembly="Goldnet.Ext.Web" Namespace="Goldnet.Ext.Web" TagPrefix="ext" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>无标题页</title>

    <script type="text/javascript">                           
       var cellSelect = function (grid, rowIndex, colIndex,MenuItem1,MenuItem2,MenuItem3,MenuItem4) {
            var record = grid.store.getAt(rowIndex);
            var name = grid.getColumnModel().getDataIndex(colIndex);
            var value = record.get(name);
            var deptCode = record.get('DEPT_CODE');
            var deptName = record.get('DEPT_NAME');
            var GuideCode = name.substring(1);
            var isECorrelation = false;
            var isEGuideExpressions = false;
            
            Goldnet.AjaxMethod.request(
                  'MenuItemJuge',
                    {
                    params: {
                        GuideCode:GuideCode,DeptId:deptCode
                    },
                    success: function(result) {
                        var juge = result;
                        var jugeCode = juge.split('*');
                        if(jugeCode[0] == "0") {
                            isECorrelation = true;
                        }
                        if(jugeCode[1] == "0") {
                            isEGuideExpressions = true;
                        }   
                        
                        
                    if(colIndex == 2) {
                        MenuItem1.enable();
                        MenuItem2.disable();
                        MenuItem3.disable();
                        MenuItem4.enable();
                    } else {
                        MenuItem1.disable();
                        MenuItem2.enable();
                        MenuItem4.disable();
                        MenuItem3.enable();
                    }
                    
                    MenuItem3.setDisabled(!isEGuideExpressions);
                    MenuItem2.setDisabled(!isECorrelation);
                    
                    var path = 'DEPT_CODE='+deptCode+'&GuideCode='+GuideCode+'&Name='+deptName;
                    document.form1.HiddenPath.value = path;
                                            
                    },
                    failure: function(msg) {
                        GridPanel1.el.unmask();
                    }
                }); 
            

        }
        
     var TreeNodeButton = function(node) {
         if(node.hasChildNodes() || node.parentNode.id == 'root') {
            Button1.setDisabled(true);
         } else {
            Button1.setDisabled(false);
         }
     } 



//冒泡排序
function SortIslowTohighArr(liste) {
    var temp;// 临时变量
    for(var i = liste.length - 1; i > 0; i--) {
        for(var j = 0; j <= i - 1; j++) 
        { 
            if(liste[j] > liste[j + 1]) { 
                temp = liste[j];
                liste[j] = liste[j + 1];
                liste[j + 1] = temp;
            }
        } 
    }
    return liste;
}


//冒泡排序
function SortIshightoLowArr(liste) {
    var temp;// 临时变量
    for(var i = liste.length - 1; i > 0; i--) {
        for(var j = 0; j <= i - 1; j++) 
        { 
            if(liste[j] < liste[j + 1]) { 
                temp = liste[j];
                liste[j] = liste[j + 1];
                liste[j + 1] = temp;
            }
        } 
    }
    return liste;
}

//计算数值排序数组
function RankArr(Arrlist) {
    var rankingArr = new Array();
    for(var i=0;i<Arrlist.length;i++) {
        if(i>0) 
        {
            if(rankingArr[rankingArr.length-1].split(',')[0] != Arrlist[i]) {
                rankingArr[rankingArr.length] = Arrlist[i]+','+(i+1);
            }
        } else {
            rankingArr[0] = Arrlist[0]+','+'1';
        }
    }
    return rankingArr;
} 

//返回字段排名
function ColunmArr(GridPanel_Show,ColIndex,SelectRowIndex,isHighGude) {
        var Arrlist = new Array();
        var SelectTextDisplay = '';
        var count = GridPanel_Show.getStore().getCount();
        for(var rowIndex=0;rowIndex<GridPanel_Show.getStore().getCount();rowIndex++)   {
              var record=GridPanel_Show.getStore().getAt(rowIndex); 
              var textDisplay=record.get(GridPanel_Show.colModel.getDataIndex(ColIndex));   
              Arrlist[rowIndex] = Number(textDisplay);
              if(SelectRowIndex == rowIndex) {
                SelectTextDisplay = textDisplay;
              }
        }
        if(isHighGude == '1') {
            Arrlist = SortIshightoLowArr(Arrlist);
        } else {
            Arrlist = SortIslowTohighArr(Arrlist);
        }
        
        Arrlist =RankArr(Arrlist);
        var ColName = GridPanel_Show.getColumnModel().getColumnHeader(ColIndex);
        var ColRanking = RankingColor(ColName.substring(0,ColName.indexOf('合')),CellRanking(SelectTextDisplay,Arrlist),GridPanel_Show.getStore().getCount(),isHighGude);
     return ColRanking;
}

function RankingColor(ColName,Ranking,count,isHighGude) {
    var frontCount =  Number(count) * 0.3;
    var AfterCount = Number(count) * 0.7;
    var rankingColor = '<font color=';
    if(isHighGude == '1') {
        if(Number(Ranking) <= Number(frontCount)) {
            rankingColor = rankingColor + 'blue>';
        }else if((Number(Ranking) > Number(frontCount)) && (Number(Ranking) < Number(AfterCount))) {  
            rankingColor = rankingColor + 'green>';
        }else if(Number(Ranking) >= Number(AfterCount)) {
            
            rankingColor = rankingColor + 'red>';
        }
        rankingColor =  '<td>'+rankingColor+ColName+'</font></td><td>'+rankingColor+Ranking+'</font></td>';
    
    } else {
        if(Number(Ranking) <= Number(frontCount)) {
            rankingColor = rankingColor + 'red>';
        }else if((Number(Ranking) > Number(frontCount)) && (Number(Ranking) < Number(AfterCount))) {  
            rankingColor = rankingColor + 'green>';
        }else if(Number(Ranking) >= Number(AfterCount)) {
            rankingColor = rankingColor + 'blue>';
        }
        rankingColor =  '<td>'+rankingColor+ColName+'</font></td><td>'+rankingColor+Ranking+'</font></td>';
    }

   return rankingColor;
}



//计算名次
function CellRanking(textDisplay,Arrlist) {
    var Ranking;
    for(var i=0;i<Arrlist.length;i++) {
        if(textDisplay == Arrlist[i].split(',')[0]) {
            Ranking = Arrlist[i].split(',')[1];
            return Ranking;
        }
    }
}


var MouseGrid = function(e, t) {    
            e.stopEvent();   
            if(e.getTarget('.x-grid3-row') != null) {
                    var rIndex = e.getTarget('.x-grid3-row').rowIndex;
                    var ishighGuide = isHighGuide.value;
                    var ColRanking = '<table>';
                    for(var columnIndex=0;columnIndex<GridPanel_Show.colModel.getColumnCount();columnIndex++) {
                        if(columnIndex > 2) {
                            var i = 0;
                            var ishigh = ishighGuide.split(',')[i];
                            ColRanking = ColRanking + '<tr>'
                            ColRanking = ColRanking + ColunmArr(GridPanel_Show,columnIndex,rIndex,ishigh)+'</tr>';
                            i++;
                        }
                   }
                     ColRanking = ColRanking +'</table> ';
                     new Ext.ToolTip({   
                            target : e.target,   
                            title : '',
                            autoHide : true,   
                            html : ColRanking,   
                            showDelay : 800,   
                            autoHeight : true,   
                            autowidth : true  
                        });  
           }         
        };   
   var rmbMoney = function(v) {
                  if(v != '0') {
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
                  }
                  return v;
           }

    </script>

    <style type="text/css">
        .x-grid3-col
        {
            border-left: 1px solid #eee;
        }
        .x-grid3-row td
        {
            padding-left: 0px;
            padding-right: 0px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <ext:ScriptManager ID="ScriptManager1" runat="server">
        <Listeners>
            <%--            <DocumentReady Handler="#{Comb_StartDate}.setVisible(false);
                                    #{Comb_EndDate}.setVisible(false);#{dd1Name}.hide();#{dd2Name}.hide();
                                    #{HideStartDate}.setValue('2');#{HiddenEndDate}.setValue('2');" />--%>
        </Listeners>
    </ext:ScriptManager>
    <ext:Hidden ID="HiddenPath" runat="server">
    </ext:Hidden>
    <ext:Hidden ID="HideStartDate" runat="server" Text="1">
    </ext:Hidden>
    <ext:Hidden ID="HiddenEndDate" runat="server" Text="1">
    </ext:Hidden>
    <ext:Hidden ID="isHighGuide" runat="server">
    </ext:Hidden>
    <ext:Store runat="server" ID="Store1">
        <Reader>
            <ext:JsonReader>
                <Fields>
                </Fields>
            </ext:JsonReader>
        </Reader>
    </ext:Store>
    <ext:Menu ID="ContextMenu" runat="server">
        <Items>
            <ext:MenuItem ID="MenuItem1" runat="server" Icon="NoteEdit" Text="查看科室人员信息" Disabled="true">
                <AjaxEvents>
                    <Click OnEvent="StuffByDeptView">
                        <ExtraParams>
                            <ext:Parameter Name="NodeId" Value="Ext.encode(#{TreeCtrl}.getSelectionModel().getSelectedNode().id)"
                                Mode="Raw">
                            </ext:Parameter>
                        </ExtraParams>
                    </Click>
                </AjaxEvents>
            </ext:MenuItem>
            <ext:MenuItem ID="MenuItem2" runat="server" Icon="NoteEdit" Text="查看相关性信息" Disabled="true">
                <AjaxEvents>
                    <Click OnEvent="DeptGuideRelation">
                        <ExtraParams>
                            <ext:Parameter Name="NodeId" Value="Ext.encode(#{TreeCtrl}.getSelectionModel().getSelectedNode().id)"
                                Mode="Raw">
                            </ext:Parameter>
                        </ExtraParams>
                    </Click>
                </AjaxEvents>
            </ext:MenuItem>
            <ext:MenuItem ID="MenuItem3" runat="server" Icon="NoteEdit" Text="查看明细数据" Disabled="true">
                <AjaxEvents>
                    <Click OnEvent="DeptGuideDetail">
                        <ExtraParams>
                            <ext:Parameter Name="NodeId" Value="Ext.encode(#{TreeCtrl}.getSelectionModel().getSelectedNode().id)"
                                Mode="Raw">
                            </ext:Parameter>
                        </ExtraParams>
                    </Click>
                </AjaxEvents>
            </ext:MenuItem>
            <ext:MenuItem ID="MenuItem4" runat="server" Icon="NoteEdit" Text="按月分析指标值" Disabled="true">
                <AjaxEvents>
                    <Click OnEvent="DeptMonthGuide">
                        <ExtraParams>
                            <ext:Parameter Name="NodeId" Value="Ext.encode(#{TreeCtrl}.getSelectionModel().getSelectedNode().id)"
                                Mode="Raw">
                            </ext:Parameter>
                        </ExtraParams>
                    </Click>
                </AjaxEvents>
            </ext:MenuItem>
        </Items>
    </ext:Menu>
    <div>
        <ext:ViewPort ID="ViewPort1" runat="server">
            <Body>
                <ext:BorderLayout ID="BorderLayout2" runat="server">
                    <North>
                        <ext:Toolbar runat="server" ID="ctl155" StyleSpec="border:0">
                            <Items>
                                <ext:ToolbarSpacer ID="ToolbarSpacer2" runat="server" Width="10" />
                                <ext:Label ID="Label7" runat="server" Text="统计月份">
                                </ext:Label>
                                <ext:ToolbarSpacer ID="ToolbarSpacer1" runat="server" Width="10" />
                                <ext:ComboBox runat="server" ID="Comb_StartYear" Width="60" ListWidth="60" SelectedIndex="0">
                                </ext:ComboBox>
                                <ext:ToolbarTextItem ID="ToolbarTextItem2" runat="server" Text="年" />
                                <ext:ComboBox runat="server" ID="Comb_StartMonth" Width="40" ListWidth="40" SelectedIndex="0">
                                </ext:ComboBox>
                                <ext:ToolbarTextItem ID="ToolbarTextItem1" runat="server" Text="月" />
                                <ext:ComboBox runat="server" ID="Comb_StartDate" Width="40" ListWidth="40" SelectedIndex="0">
                                </ext:ComboBox>
                                <ext:ToolbarTextItem ID="dd1Name" runat="server" Text="日" />
                                <ext:ToolbarTextItem ID="ToolbarTextItem3" runat="server" Text="  　至" />
                                <ext:ToolbarSpacer ID="ToolbarSpacer5" runat="server" Width="6" />
                                <ext:ComboBox runat="server" ID="Comb_EndYear" Width="60" ListWidth="60" SelectedIndex="0">
                                </ext:ComboBox>
                                <ext:ToolbarTextItem ID="ToolbarTextItem4" runat="server" Text="年" />
                                <ext:ComboBox runat="server" ID="Comb_EndMonth" Width="40" ListWidth="40" SelectedIndex="0">
                                </ext:ComboBox>
                                <ext:ToolbarTextItem ID="ToolbarTextItem5" runat="server" Text="月" />
                                <ext:ComboBox runat="server" ID="Comb_EndDate" Width="40" ListWidth="40" SelectedIndex="0">
                                </ext:ComboBox>
                                <ext:ToolbarTextItem ID="dd2Name" runat="server" Text="日 " Visible="true" />
                                <ext:ToolbarSeparator ID="ToolbarSeparator3" runat="server" />
                                <ext:Button ID="Button1" runat="server" Text=" 查询 " Icon="DatabaseGo" Disabled="true">
                                    <AjaxEvents>
                                        <Click OnEvent="GetQueryPortalet" Timeout="99999999">
                                            <EventMask Msg="载入中..." ShowMask="true" />
                                            <ExtraParams>
                                                <ext:Parameter Name="NodeId" Value="Ext.encode(#{TreeCtrl}.getSelectionModel().getSelectedNode().id)"
                                                    Mode="Raw">
                                                </ext:Parameter>
                                                <ext:Parameter Name="NodeName" Value="Ext.encode(#{TreeCtrl}.getSelectionModel().getSelectedNode().text)"
                                                    Mode="Raw">
                                                </ext:Parameter>
                                            </ExtraParams>
                                            <EventMask ShowMask="true" Msg="请稍候..." />
                                        </Click>
                                    </AjaxEvents>
                                </ext:Button>
                                <ext:ToolbarFill>
                                </ext:ToolbarFill>
                                <ext:Button ID="btnExcel" runat="server" Text=" Excel上报 " Icon="PageWhiteExcel" OnClick="OutExcel"
                                    AutoPostBack="true">
                                </ext:Button>
                            </Items>
                        </ext:Toolbar>
                    </North>
                    <Center>
                        <ext:GridPanel ID="GridPanel_Show" runat="server" Border="false" StoreID="Store1"
                            Height="800" Title="报表信息" StripeRows="true" AutoScroll="true" Width="600">
                            <ColumnModel ID="ColumnModel1" runat="server">
                                <Columns>
                                    <ext:RowNumbererColumn Width="32" Resizable="true">
                                    </ext:RowNumbererColumn>
                                </Columns>
                            </ColumnModel>
                            <SelectionModel>
                                <ext:RowSelectionModel SingleSelect="true">
                                </ext:RowSelectionModel>
                            </SelectionModel>
                            <LoadMask ShowMask="true" />
                            <Listeners>
                                <RowContextMenu Handler="this.getSelectionModel().selectRow(rowIndex);e.preventDefault(); #{ContextMenu}.dataRecord = this.store.getAt(rowIndex);#{ContextMenu}.showAt(e.getXY());" />
                                <CellContextMenu Handler="cellSelect(this,rowIndex,cellIndex,#{MenuItem1},#{MenuItem2},#{MenuItem3},#{MenuItem4})" />
                                <MouseOver Fn="MouseGrid" />
                            </Listeners>
                        </ext:GridPanel>
                    </Center>
                    <West CollapseMode="Mini" Split="false" Collapsible="false">
                        <ext:Panel ID="Panel1" runat="server" Width="200" BodyBorder="false" Title="请选择报表"
                            AutoScroll="true" Border="false">
                            <Body>
                                <ext:TreePanel runat="server" Width="200" ID="TreeCtrl" AutoHeight="true" AutoScroll="false"
                                    Border="false">
                                    <Listeners>
                                        <Click Handler="TreeNodeButton(node)" />
                                    </Listeners>
                                </ext:TreePanel>
                            </Body>
                        </ext:Panel>
                    </West>
                </ext:BorderLayout>
            </Body>
        </ext:ViewPort>
    </div>
    <ext:Window ID="arcEditWindow" runat="server" Icon="Group" Width="580" Height="384"
        AutoShow="false" Modal="true" CenterOnLoad="true" AutoScroll="true" ShowOnLoad="false"
        Resizable="true" StyleSpec="background-color:Transparent;" BodyStyle="background-color:Transparent;"
        Maximizable="true">
    </ext:Window>
    </form>
</body>
</html>
