<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Information.aspx.cs" Inherits="GoldNet.JXKP.RLZY.BaseInfoMaintain.Information" %>

<%@ Register Assembly="Goldnet.Ext.Web" Namespace="Goldnet.Ext.Web" TagPrefix="ext" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>无标题页</title>

    <script type="text/javascript">
       var RowIndex;                   
        /*
            GRIDPANEL操作
            optype :1 添加;  2 重命名; 3 删除;
        */
        function TreeOpration(optype) {
            if (optype == "1") {
                var myDate = new Date();
                var year = myDate.getFullYear();   //获取完整的年份(4位,1970-????)
                var month = myDate.getMonth() + 1;       //获取当前月份(0-11,0代表1月)
                var MMmonth  = month;
                if(month < 10) {
                    MMmonth = '0' + month;
                }
                var data = year +''+ MMmonth;
                txtStatMonth.setValue(data);
                txtNetNum.setValue("");
                txtServerNum.setValue("");
                txtHISTechPers.setValue("");
                txtPlanetLongCase.setValue("");
                txtAppSubsysNum.setValue("");
                txtNetCompNum.setValue("");
                txtInvestTotal.setValue("");
                txtPlanetMediCase.setValue("");
                txtPlanetLongPers.setValue("");
                dtfOpenDate.setValue("");
                Btn_BatStart.setText("保存");
                arcEditWindow.show();
            } else if (optype == "2") {
                
                txtStatMonth.setValue(this.Store1.getAt(RowIndex).get('STAT_MONTH'));
                txtNetNum.setValue(this.Store1.getAt(RowIndex).get('NET_NUM'));
                txtServerNum.setValue(this.Store1.getAt(RowIndex).get('SERVER_NUM'));
                txtHISTechPers.setValue(this.Store1.getAt(RowIndex).get('HIS_TECH_PERS'));
                txtPlanetLongCase.setValue(this.Store1.getAt(RowIndex).get('PLANET_LONG_CASE'));
                txtAppSubsysNum.setValue(this.Store1.getAt(RowIndex).get('APP_SUBSYS_NUM'));
                txtNetCompNum.setValue(this.Store1.getAt(RowIndex).get('NET_COMP_NUM'));
                txtInvestTotal.setValue(this.Store1.getAt(RowIndex).get('INVEST_TOTAL'));
                txtPlanetMediCase.setValue(this.Store1.getAt(RowIndex).get('PLANET_MEDI_CASE'));
                txtPlanetLongPers.setValue(this.Store1.getAt(RowIndex).get('PLANET_LONG_PERS'));
                dtfOpenDate.setValue(this.Store1.getAt(RowIndex).get('OPEN_DATE'));
                
                Btn_BatStart.setText("修改");
                arcEditWindow.show();
            } else if (optype == "3") {
                Btn_BatStart.setText("");
                Ext.Msg.confirm("删除项目", "确定要删除该项目吗？", function(btn, text) { OpCallback(btn) });
            } 
        }
        
        
        /*
            节点增删改操作回调函数
        */
        function OpCallback (btn) {
            var optype = '3';
            if (Btn_BatStart.text == "保存") {
                optype = "1";
            }
            if(Btn_BatStart.text == "修改") {
                optype = "2";
            }
            if((btn != "ok") && (btn != "yes")){
                return;
            }
            var myDate = new Date();
            var statmonth = txtStatMonth.getValue();
            var opendateFormat = dtfOpenDate.getValue() == ''?myDate:dtfOpenDate.getValue();
            var opendate = opendateFormat.format('Y-m-d');
            var appsubsysnum = txtAppSubsysNum.getValue().toString() == ''?'0':txtAppSubsysNum.getValue().toString();
            var netnum = txtNetNum.getValue().toString() == ''?'0':txtNetNum.getValue().toString();
            var netcompnum =txtNetCompNum.getValue().toString() == ''?'0':txtNetCompNum.getValue().toString();
            var servernum = txtServerNum.getValue().toString() == ''?'0':txtServerNum.getValue().toString();
            var investtotal = txtInvestTotal.getValue().toString() == ''?'0':txtInvestTotal.getValue().toString();
            var HIStechpers = txtHISTechPers.getValue().toString() == ''?'0':txtHISTechPers.getValue().toString();
            var planetmedicase = txtPlanetMediCase.getValue().toString() == ''?'0':txtPlanetMediCase.getValue().toString();
            var planetlongcase = txtPlanetLongCase.getValue().toString() == ''?'0':txtPlanetLongCase.getValue().toString();
            var planetlongppers = txtPlanetLongPers.getValue().toString() == ''?'0':txtPlanetLongPers.getValue().toString();
            
            if (optype == "1") {
               GridPanelToDataBase("",statmonth,opendate,appsubsysnum,netnum,netcompnum,servernum,investtotal,
                           HIStechpers,planetmedicase,planetlongcase,planetlongppers,optype);
            } else if (optype == "2") {
               GridPanelToDataBase(this.Store1.getAt(RowIndex).get('ID'), statmonth,opendate,appsubsysnum,netnum,netcompnum,servernum,investtotal,
                           HIStechpers,planetmedicase,planetlongcase,planetlongppers,optype);
            } else if (optype == "3") {
               GridPanelToDataBase(this.Store1.getAt(RowIndex).get('ID'),statmonth,opendate,appsubsysnum,netnum,netcompnum,servernum,investtotal,
                           HIStechpers,planetmedicase,planetlongcase,planetlongppers,optype);
            }
        }
        
        function GridPanelToDataBase(id,statmonth,opendate,appsubsysnum,netnum,netcompnum,servernum,investtotal,
                           HIStechpers,planetmedicase,planetlongcase,planetlongppers,optype) {
          Goldnet.AjaxMethod.request(
                  'InformatinAjaxOper',
                    {
                        params: {
                           Id:id,statMonth:statmonth,openDate:opendate,appSubsysNum:appsubsysnum,netNum:netnum,netCompNum:netcompnum,serverNum:servernum,investTotal:investtotal,
                           HIStechPers:HIStechpers,planetMediCase:planetmedicase,planetLongCase:planetlongcase,planetLongPpers:planetlongppers,OperType:optype
                        },
                        success: function(result) {
                            Store1.reload();
                            btn_Modify.setDisabled(true);
                            btn_Delete.setDisabled(true);
                            arcEditWindow.hide();
                        },
                        failure: function(msg) {
                            GridPanel_Show.el.unmask();
                        }
                    });
        }
    </script>

</head>
<body>
    <form id="form1" runat="server">
    <ext:ScriptManager ID="ScriptManager1" runat="server">
    </ext:ScriptManager>
    <ext:Store ID="Store1" runat="server" OnRefreshData="Data_RefreshData">
        <Reader>
            <ext:JsonReader>
                <Fields>
                    <ext:RecordField Name="ID" />
                    <ext:RecordField Name="YEARS" />
                    <ext:RecordField Name="STAT_MONTH" />
                    <ext:RecordField Name="OPEN_DATE" />
                    <ext:RecordField Name="APP_SUBSYS_NUM" />
                    <ext:RecordField Name="NET_NUM" />
                    <ext:RecordField Name="NET_COMP_NUM" />
                    <ext:RecordField Name="SERVER_NUM" />
                    <ext:RecordField Name="INVEST_TOTAL" />
                    <ext:RecordField Name="HIS_TECH_PERS" />
                    <ext:RecordField Name="PLANET_MEDI_CASE" />
                    <ext:RecordField Name="PLANET_LONG_CASE" />
                    <ext:RecordField Name="PLANET_LONG_PERS" />
                </Fields>
            </ext:JsonReader>
        </Reader>
    </ext:Store>
    <div>
        <ext:ViewPort ID="ViewPort1" runat="server">
            <Body>
                <ext:BorderLayout ID="BorderLayout2" runat="server">
                    <North>
                        <ext:Toolbar runat="server" ID="ctl155" StyleSpec="border:0">
                            <Items>
                                <ext:Label ID="Label3" runat="server" Text="年度：">
                                </ext:Label>
                                <ext:ComboBox ID="TimeOrgan" runat="server" Width="40">
                                    <Items>
                                        <ext:ListItem Text="&lt;=" Value="&lt;=" />
                                        <ext:ListItem Text="&gt;=" Value="&gt;=" />
                                        <ext:ListItem Text="=" Value="=" />
                                    </Items>
                                </ext:ComboBox>
                                <ext:ComboBox ID="cboTime" runat="server" Width="60">
                                </ext:ComboBox>
                                <ext:Button ID="btnSearch" runat="server" Text="查询" Icon="DatabaseGo">
                                    <Listeners>
                                        <Click  Handler="#{Store1}.reload();#{btn_Modify}.disable();#{btn_Delete}.disable();"/>
                                    </Listeners>
                                </ext:Button>
                                <ext:ToolbarSeparator>
                                </ext:ToolbarSeparator>
                                <ext:Button ID="btn_Add" runat="server" Text="添加项目" Icon="Add">
                                    <Listeners>
                                        <Click Handler="TreeOpration(1)" />
                                    </Listeners>
                                </ext:Button>
                                <ext:Button Text="修改项目" ID="btn_Modify" runat="server" Icon="FolderEdit" Disabled="true">
                                    <Listeners>
                                        <Click Handler="TreeOpration(2)" />
                                    </Listeners>
                                </ext:Button>
                                <ext:Button Text="删除项目" ID="btn_Delete" runat="server" Icon="Delete" Disabled="true">
                                    <Listeners>
                                        <Click Handler="TreeOpration(3)" />
                                    </Listeners>
                                </ext:Button>
                            </Items>
                        </ext:Toolbar>
                    </North>
                    <Center>
                        <ext:Panel ID="Panel1" runat="server">
                            <Body>
                                <ext:ColumnLayout ID="ColumnLayout1" runat="server" Split="true" FitHeight="true">
                                    <Columns>
                                        <ext:LayoutColumn ColumnWidth="1">
                                            <ext:GridPanel ID="GridPanel_Show" runat="server" StoreID="Store1" Border="false"
                                                AutoWidth="true" Header="false" AutoScroll="true">
                                                <ColumnModel ID="ColumnModel1" runat="server">
                                                    <Columns>
                                                        <ext:Column ColumnID="STAT_MONTH" Header="统计年月" Sortable="true" DataIndex="STAT_MONTH" />
                                                        <ext:Column ColumnID="APP_SUBSYS_NUM" Header="应用子系统数" Sortable="true" DataIndex="APP_SUBSYS_NUM" />
                                                        <ext:Column ColumnID="NET_NUM" Header="布网点数" Sortable="true" DataIndex="NET_NUM"
                                                            Width="75" />
                                                        <ext:Column ColumnID="INVEST_TOTAL" Header="投资总额(万元)" Sortable="true" DataIndex="INVEST_TOTAL" />
                                                        <ext:Column ColumnID="HIS_TECH_PERS" Header="HIS技术人员数" Sortable="true" DataIndex="HIS_TECH_PERS" />
                                                    </Columns>
                                                </ColumnModel>
                                                <SelectionModel>
                                                    <ext:RowSelectionModel ID="rowselection" SingleSelect="true">
                                                        <Listeners>
                                                            <RowSelect Handler="#{btn_Delete}.enable();#{btn_Modify}.enable();RowIndex = rowIndex;" />
                                                            <RowDeselect Handler="if (!#{GridPanel_Show}.hasSelection()) {#{btn_Delete}.disable();#{btn_Modify}.disable();}RowIndex = -1;" />
                                                        </Listeners>
                                                    </ext:RowSelectionModel>
                                                </SelectionModel>
                                                <LoadMask ShowMask="true" />
                                            </ext:GridPanel>
                                        </ext:LayoutColumn>
                                    </Columns>
                                </ext:ColumnLayout>
                            </Body>
                        </ext:Panel>
                    </Center>
                </ext:BorderLayout>
            </Body>
        </ext:ViewPort>
    </div>
    <ext:Window ID="arcEditWindow" runat="server" Icon="Group" Title="设置信息化建设" Width="600"
        Height="250" AutoShow="false" Modal="true" CenterOnLoad="true" ShowOnLoad="false"
        Resizable="false" StyleSpec="background-color:Transparent;" BodyStyle="background-color:Transparent;">
        <Body>
            <ext:ColumnLayout ID="ColumnLayout2" runat="server">
                <ext:LayoutColumn ColumnWidth=".5">
                    <ext:Panel ID="Panel2" runat="server" Border="false" Header="false" BodyStyle="background-color:Transparent;margin:10px;">
                        <Body>
                            <ext:FormLayout ID="FormLayout1" runat="server" LabelAlign="Left">
                                <ext:Anchor Horizontal="92%">
                                    <ext:TextField ID="txtStatMonth" runat="server" FieldLabel="统计年月" ReadOnly="true" />
                                </ext:Anchor>
                                <ext:Anchor Horizontal="92%">
                                    <ext:DateField ID="dtfOpenDate" runat="server" FieldLabel="His正式启动时间"  Format="yyyy-MM-dd"/>
                                </ext:Anchor>
                                <ext:Anchor Horizontal="92%">
                                    <ext:NumberField ID="txtNetNum" runat="server" FieldLabel="布网点数" />
                                </ext:Anchor>
                                <ext:Anchor Horizontal="92%">
                                    <ext:NumberField ID="txtServerNum" runat="server" FieldLabel="服务器台数" />
                                </ext:Anchor>
                                <ext:Anchor Horizontal="92%">
                                    <ext:NumberField ID="txtHISTechPers" runat="server" FieldLabel="His技术人员数" />
                                </ext:Anchor>
                                <ext:Anchor Horizontal="92%">
                                    <ext:NumberField ID="txtPlanetLongCase" runat="server" FieldLabel="卫星网远程教学例数" />
                                </ext:Anchor>
                            </ext:FormLayout>
                        </Body>
                    </ext:Panel>
                </ext:LayoutColumn>
                <ext:LayoutColumn ColumnWidth=".5">
                    <ext:Panel ID="Panel3" runat="server" Border="false" BodyStyle="background-color:Transparent;margin:10px;">
                        <Body>
                            <ext:FormLayout ID="FormLayout2" runat="server" LabelAlign="Left">
                                <ext:Anchor Horizontal="92%">
                                    <ext:NumberField ID="txtID" runat="server" FieldLabel="序号" Visible="false" />
                                </ext:Anchor>
                                <ext:Anchor Horizontal="92%">
                                    <ext:NumberField ID="txtAppSubsysNum" runat="server" FieldLabel="应用子系统数" />
                                </ext:Anchor>
                                <ext:Anchor Horizontal="92%">
                                    <ext:NumberField ID="txtNetCompNum" runat="server" FieldLabel="上网微机数" />
                                </ext:Anchor>
                                <ext:Anchor Horizontal="92%">
                                    <ext:NumberField ID="txtInvestTotal" runat="server" FieldLabel="投资总额(万元)" />
                                </ext:Anchor>
                                <ext:Anchor Horizontal="92%">
                                    <ext:NumberField ID="txtPlanetMediCase" runat="server" FieldLabel="卫星网医疗会诊例数" />
                                </ext:Anchor>
                                <ext:Anchor Horizontal="92%">
                                    <ext:NumberField ID="txtPlanetLongPers" runat="server" FieldLabel="卫星网远程教学参加人次" />
                                </ext:Anchor>
                            </ext:FormLayout>
                        </Body>
                    </ext:Panel>
                </ext:LayoutColumn>
            </ext:ColumnLayout>
        </Body>
        <BottomBar>
            <ext:Toolbar ID="Toolbar1" runat="server">
                <Items>
                    <ext:ToolbarFill ID="ToolbarFill2" runat="server" />
                    <ext:ToolbarButton ID="Btn_BatStart" runat="server" Icon="Disk" Text="保存">
                        <Listeners>
                            <Click Handler="OpCallback('ok');" />
                        </Listeners>
                    </ext:ToolbarButton>
                    <ext:ToolbarSeparator ID="ToolbarSeparator3" runat="server" />
                    <ext:ToolbarButton ID="Btn_BatCancel" runat="server" Icon="Cancel" Text="退出">
                        <Listeners>
                            <Click Handler="arcEditWindow.hide();" />
                        </Listeners>
                    </ext:ToolbarButton>
                </Items>
            </ext:Toolbar>
        </BottomBar>
    </ext:Window>
    </form>
</body>
</html>
