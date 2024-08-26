<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Appraisal_Detail.aspx.cs"
    Inherits="GoldNet.JXKP.jxkh.Appraisal_Detail" %>

<%@ Register Assembly="Goldnet.Ext.Web" Namespace="Goldnet.Ext.Web" TagPrefix="ext" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>评价详情</title>

    <script type="text/javascript">

        //冒泡排序
        function SortArr(liste) {
            var temp; // 临时变量
            for (var i = liste.length - 1; i > 0; i--) {
                for (var j = 0; j <= i - 1; j++) {
                    if (liste[j] > liste[j + 1]) {
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
            for (var i = 0; i < Arrlist.length; i++) {
                if (i > 0) {
                    if (rankingArr[rankingArr.length - 1].split(',')[0] != Arrlist[i]) {
                        rankingArr[rankingArr.length] = Arrlist[i] + ',' + (i + 1);
                    }
                } else {
                    rankingArr[0] = Arrlist[0] + ',' + '1';
                }
            }
            return rankingArr;
        }

        //返回字段排名
        function ColunmArr(GridPanel_Show, ColIndex, SelectRowIndex) {
            var Arrlist = new Array();
            var SelectTextDisplay = '';
            var count = GridPanel_Show.getStore().getCount();
            for (var rowIndex = 0; rowIndex < GridPanel_Show.getStore().getCount(); rowIndex++) {
                var record = GridPanel_Show.getStore().getAt(rowIndex);
                var textDisplay = record.get(GridPanel_Show.colModel.getDataIndex(ColIndex));
                Arrlist[rowIndex] = Number(textDisplay);
                if (SelectRowIndex == rowIndex) {
                    SelectTextDisplay = textDisplay;
                }
            }
            Arrlist = SortArr(Arrlist);
            Arrlist = RankArr(Arrlist);
            var ColName = GridPanel_Show.getColumnModel().getColumnHeader(ColIndex);
            var ColRanking = RankingColor(ColName, CellRanking(SelectTextDisplay, Arrlist), GridPanel_Show.getStore().getCount());
            return ColRanking;
        }

        function RankingColor(ColName, Ranking, count) {
            var frontCount = Number(count + 2) * 0.3;
            var AfterCount = Number(count + 2) * 0.7;
            var rankingColor = '<font color=';
            if (Number(Ranking) <= Number(frontCount)) {
                rankingColor = rankingColor + 'red>';
            }
            else if ((Number(Ranking) > Number(frontCount)) && (Number(Ranking) < Number(AfterCount))) {
                rankingColor = rankingColor + 'green>';
            }
            else if (Number(Ranking) >= Number(AfterCount)) {
                rankingColor = rankingColor + 'blue>';
            }
            rankingColor = '<td>' + rankingColor + ColName + '</font></td><td>' + rankingColor + Ranking + '</font></td>';
            return rankingColor;
        }



        //计算名次
        function CellRanking(textDisplay, Arrlist) {
            var Ranking;
            for (var i = 0; i < Arrlist.length; i++) {
                if (textDisplay == Arrlist[i].split(',')[0]) {
                    Ranking = Arrlist[i].split(',')[1];
                    return Ranking;
                }
            }
        }


        var MouseGrid = function(e, t) {
            e.stopEvent();
            if (e.getTarget('.x-grid3-row') != null) {
                var rIndex = e.getTarget('.x-grid3-row').rowIndex;

                var ColRanking = '<table>';
                for (var columnIndex = 0; columnIndex < GridPanel_Show.colModel.getColumnCount(); columnIndex++) {
                    if (columnIndex > 2) {
                        ColRanking = ColRanking + '<tr>'
                        ColRanking = ColRanking + ColunmArr(GridPanel_Show, columnIndex, rIndex) + '</tr>';
                    }
                }
                ColRanking = ColRanking + '</table> ';
                new Ext.ToolTip({
                    target: e.target,
                    title: '',
                    autoHide: true,
                    html: ColRanking,
                    showDelay: 800,
                    autoHeight: true,
                    autowidth: true
                });
            }
        };  



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
       
       
       function cmSelectOnChange()
       {
       var a=IS_EVLUATE_BONUS.value;
       var b=Hide_Flag.value
       if(a!=b)
       {
            Btn_Arch.setDisabled(false);  
       }
       else
       {
            Btn_Arch.setDisabled(true);  
       }
       
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
        <AjaxEvents>
            <DocumentReady OnEvent="GridDataBind">
                <EventMask ShowMask="true" Msg="数据加载中..." />
            </DocumentReady>
        </AjaxEvents>
    </ext:ScriptManager>
    <ext:Store runat="server" ID="Store1">
        <Reader>
            <ext:JsonReader>
                <Fields>
                </Fields>
            </ext:JsonReader>
        </Reader>
    </ext:Store>
    <ext:ViewPort ID="ViewPort1" runat="server">
        <Body>
            <ext:BorderLayout ID="BorderLayout1" runat="server">
                <North>
                    <ext:Toolbar ID="Toolbar1" runat="server">
                        <Items>
                            <ext:Label runat="server" ID="Label1" Text="是否参与绩效奖励：">
                            </ext:Label>
                            <ext:ComboBox ID="IS_EVLUATE_BONUS" runat="server" Width="60">
                                <Items>
                                    <ext:ListItem Text="是" Value="是" />
                                    <ext:ListItem Text="否" Value="否" />
                                </Items>
                                <Listeners>
                                    <Select Handler="cmSelectOnChange();" />
                                </Listeners>
                            </ext:ComboBox>
                            <ext:Hidden ID="Hide_Flag" runat="server" Text="0">
                            </ext:Hidden>
                            <ext:ToolbarSeparator ID="ToolbarSeparator3" runat="server" />
                            <ext:Button ID="Btn_Arch" runat="server" Text="归档" Icon="PageLink">
                                <AjaxEvents>
                                    <Click OnEvent="Btn_Arch_Click">
                                        <EventMask Msg="载入中..." ShowMask="true" />
                                    </Click>
                                </AjaxEvents>
                            </ext:Button>
                            <ext:ToolbarSeparator ID="ToolbarSeparator4" runat="server" />
                            <ext:Button ID="Btn_Del" runat="server" Text="删除" Icon="Delete">
                                <AjaxEvents>
                                    <Click OnEvent="Btn_Del_Click">
                                        <Confirmation Title="系统提示" Message="确实要删除该评价吗?" ConfirmRequest="true" />
                                        <EventMask Msg="载入中..." ShowMask="true" />
                                    </Click>
                                </AjaxEvents>
                            </ext:Button>
                            <ext:ToolbarSeparator ID="ToolbarSeparator1" runat="server" />
                            <ext:Button ID="Btn_Export" runat="server" Text="导出Excel" Icon="PageWhiteExcel" OnClick="OutExcel"
                                AutoPostBack="true">
                            </ext:Button>
                            <ext:ToolbarSeparator ID="ToolbarSeparator2" runat="server" />
                            <ext:Label runat="server" ID="Lbl_AppMonth" Text="评价区间  (从 至 ) ">
                            </ext:Label>
                            <ext:ToolbarFill ID="ToolbarFill1" runat="server">
                            </ext:ToolbarFill>
                            <ext:ToolbarSeparator ID="ToolbarSeparator5" runat="server" />
                            <ext:Button ID="Btn_Close" runat="server" Text="关闭" Icon="ArrowUndo">
                                <Listeners>
                                    <Click Handler="parent.DetailWin.hide();" />
                                </Listeners>
                            </ext:Button>
                        </Items>
                    </ext:Toolbar>
                </North>
                <Center>
                    <ext:GridPanel ID="GridPanel_Show" runat="server" Border="false" StoreID="Store1"
                        Width="300" Height="300" StripeRows="true" AutoScroll="true">
                        <ColumnModel ID="ColumnModel1" runat="server">
                            <Columns>
                                <ext:RowNumbererColumn Width="32" Resizable="true" />
                            </Columns>
                        </ColumnModel>
                        <SelectionModel>
                            <ext:RowSelectionModel SingleSelect="true" />
                        </SelectionModel>
                        <LoadMask ShowMask="true" Msg="请稍候..." />
                        <Listeners>
                            <MouseOver Fn="MouseGrid" />
                        </Listeners>
                    </ext:GridPanel>
                </Center>
                <South Split="true" MinHeight="60">
                    <ext:TextArea runat="server" Height="60" ID="TxtMemo" FieldLabel="评价简述" HideWithLabel="false"
                        MaxLength="128" EmptyText="请输入评价简述">
                    </ext:TextArea>
                </South>
            </ext:BorderLayout>
        </Body>
    </ext:ViewPort>
    </form>
</body>
</html>
