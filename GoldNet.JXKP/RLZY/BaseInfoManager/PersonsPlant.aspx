<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PersonsPlant.aspx.cs" Inherits="GoldNet.JXKP.RLZY.BaseInfoManager.PersonsPlant" %>

<%@ Register Assembly="Goldnet.Ext.Web" Namespace="Goldnet.Ext.Web" TagPrefix="ext" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>无标题页</title>

    <script type="text/javascript">        
        /*
            GRIDPANEL操作
            optype :1 添加;  2 重命名; 3 删除;
        */
        var RowIndex;
        function TreeOpration(optype,record) {
            if (optype == "1") {
                var myDate = new Date();
                var year = myDate.getFullYear();   //获取完整的年份(4位,1970-????)
                var month = myDate.getMonth() + 1;       //获取当前月份(0-11,0代表1月)
                var day = myDate.getDate();
                var MMmonth  = month;
                var DDday = day;
                if(month < 10) {
                    MMmonth = '0' + month;
                }
                 if(day < 10) {
                    DDday = '0' + day;
                }
                var data = year +'-'+ MMmonth +'-'+ DDday;

                cboSort.selectByIndex(0);   // 类别
                cboStaffInfo.setValue("");   // 人员
                txtUnit.setValue("");   // 单位或地址
                dtfStartDate.setValue(data);   // 起始日期
                dtfEndDate.setValue(data);   // 结束日期
                txtTerm.setValue("");   // 期限
                txtDeptSpecial.setValue("");   // 培养专业
                txrContent.setValue("");   // 结业情况
                txtLightspotName.setValue("");   // 优势或亮点名称
                txtFinishingSituation.setValue("");   // 年度完成情况
                txrCheckResult.setValue("");   // 考核结果
                Btn_BatStart.setVisible(true);
                btnEdit.setVisible(false);
                arcEditWindow.show();
            } else if (optype == "2") {
                HiddenId.setValue(this.Store1.getAt(RowIndex).get('ID'));
                cboSort.setValue(this.Store1.getAt(RowIndex).get('SORT'));   // 类别
                cboStaffInfo.setValue(this.Store1.getAt(RowIndex).get('STAFF_ID'));   // 人员
                txtUnit.setValue(this.Store1.getAt(RowIndex).get('UNIT'));   // 单位或地址
                dtfStartDate.setValue(this.Store1.getAt(RowIndex).get('START_DATE'));   // 起始日期
                dtfEndDate.setValue(this.Store1.getAt(RowIndex).get('END_DATE'));   // 结束日期
                txtTerm.setValue(this.Store1.getAt(RowIndex).get('TERM'));   // 期限
                txtDeptSpecial.setValue(this.Store1.getAt(RowIndex).get('DEPT_SPECIAL'));   // 培养专业
                txrContent.setValue(this.Store1.getAt(RowIndex).get('CONTENT'));   // 结业情况
                txtLightspotName.setValue(this.Store1.getAt(RowIndex).get('LIGHTSPOT_NAME'));   // 优势或亮点名称
                txtFinishingSituation.setValue(this.Store1.getAt(RowIndex).get('FINISHING_SITUATION'));   // 年度完成情况
                txrCheckResult.setValue(this.Store1.getAt(RowIndex).get('CHECK_RESULT'));   // 考核结果
                btnEdit.setVisible(true);
                Btn_BatStart.setVisible(false);
                arcEditWindow.show();
            } else if (optype == "3") {
                Ext.Msg.confirm("删除项目", "确定要删除该项目吗？", function(btn, text) { if((btn != "ok") && (btn != "yes")){return;} else {GridPanelToDataBase(this.Store1.getAt(RowIndex).get('ID'));}});
            }
        }
        
        function GridPanelToDataBase(id) {
          Goldnet.AjaxMethod.request(
                  'PersonsPlantAjaxOper',
                    {
                        params: {
                           Id:id
                        },
                        success: function(result) {
                            Store1.reload();
                            btn_Delete.setDisabled(true);
                            btn_Modify.setDisabled(true);
                            arcEditWindow.hide();
                        },
                        failure: function(msg) {
                            GridPanel_Show.el.unmask();
                        }
                    });
        }
        
    </script>

    <style type="text/css">
        .search-item
        {
            font: normal 11px tahoma, arial, helvetica, sans-serif;
            padding: 3px 10px 3px 10px;
            border: 1px solid #fff;
            border-bottom: 1px solid #eeeeee;
            white-space: normal;
            color: #555;
            width: 200px;
        }
        .search-item h3
        {
            display: block;
            font: inherit;
            font-weight: bold;
            color: #222;
        }
        .search-item h3 span
        {
            float: right;
            font-weight: normal;
            margin: 0 0 5px 5px;
            width: 140px;
            display: block;
            clear: none;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <ext:ScriptManager ID="ScriptManager1" runat="server">
    </ext:ScriptManager>
    <ext:Hidden ID="HiddenId" runat="server">
    </ext:Hidden>
    <ext:Store ID="Store1" runat="server" OnRefreshData="Data_RefreshData">
        <Reader>
            <ext:JsonReader>
                <Fields>
                    <ext:RecordField Name="ID" />
                    <ext:RecordField Name="DEPT_CODE" />
                    <ext:RecordField Name="SORT" />
                    <ext:RecordField Name="PERSONS" />
                    <ext:RecordField Name="UNIT" />
                    <ext:RecordField Name="START_DATE" />
                    <ext:RecordField Name="END_DATE" />
                    <ext:RecordField Name="TERM" />
                    <ext:RecordField Name="CONTENT" />
                    <ext:RecordField Name="DEPT_SPECIAL" />
                    <ext:RecordField Name="STAFF_ID" />
                    <ext:RecordField Name="LIGHTSPOT_NAME" />
                    <ext:RecordField Name="FINISHING_SITUATION" />
                    <ext:RecordField Name="CHECK_RESULT" />
                    <ext:RecordField Name="DEPT_NAME" />
                </Fields>
            </ext:JsonReader>
        </Reader>
    </ext:Store>
    <ext:Store ID="Store3" runat="server" AutoLoad="true">
        <Reader>
            <ext:JsonReader Root="Staffdepts">
                <Fields>
                    <ext:RecordField Name="DEPT_NAME" />
                    <ext:RecordField Name="DEPT_CODE" />
                </Fields>
            </ext:JsonReader>
        </Reader>
    </ext:Store>
    <ext:Store ID="Store2" runat="server" AutoLoad="true">
        <Proxy>
            <ext:HttpProxy Method="POST" Url="/RLZY/WebService/StaffInfos.ashx" />
        </Proxy>
        <Reader>
            <ext:JsonReader Root="StaffInfos">
                <Fields>
                    <ext:RecordField Name="DEPT_NAME" />
                    <ext:RecordField Name="STAFF_ID" />
                    <ext:RecordField Name="STAFF_NAME" />
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
                                <ext:ToolbarSpacer ID="ToolbarSpacer2" runat="server" Width="10" />
                                <ext:Label ID="Label1" runat="server" Text="统计月份">
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
                                <ext:ToolbarTextItem ID="dd1Name" runat="server" Text="日 " />
                                <ext:ToolbarTextItem ID="ToolbarTextItem7" runat="server" Text="   至   " />
                                <ext:ToolbarSpacer ID="ToolbarSpacer5" runat="server" Width="6" />
                                <ext:ComboBox runat="server" ID="Comb_EndYear" Width="60" ListWidth="60" SelectedIndex="0">
                                </ext:ComboBox>
                                <ext:ToolbarTextItem ID="ToolbarTextItem4" runat="server" Text="年" />
                                <ext:ComboBox runat="server" ID="Comb_EndMonth" Width="40" ListWidth="40" SelectedIndex="0">
                                </ext:ComboBox>
                                <ext:ToolbarTextItem ID="ToolbarTextItem5" runat="server" Text="月" />
                                <ext:ComboBox runat="server" ID="Comb_EndDate" Width="40" ListWidth="40" SelectedIndex="0">
                                </ext:ComboBox>
                                <ext:ToolbarTextItem ID="dd2Name" runat="server" Text="日 " />
                                <ext:ToolbarSeparator ID="ToolbarSeparator1" runat="server" />
                                <ext:Label ID="Label3" runat="server" Text="科室:">
                                </ext:Label>
                                <ext:ComboBox ID="DeptCodeCombo" runat="server" StoreID="Store3" DisplayField="DEPT_NAME"
                                    Width="70" ValueField="DEPT_CODE" TypeAhead="false" LoadingText="Searching..."
                                    PageSize="1000" ItemSelector="div.search-item" MinChars="1" FieldLabel="科室信息"
                                    ListWidth="240">
                                    <Template ID="Template1" runat="server">
                                       <tpl for=".">
                                          <div class="search-item">
                                             <h3><span style="width:auto">{DEPT_CODE}</span>{DEPT_NAME}</h3>
                                          </div>
                                       </tpl>
                                    </Template>
                                </ext:ComboBox>
                                <ext:ToolbarSeparator ID="ToolbarSeparator4" runat="server" />
                                <ext:Button ID="btnSearch" runat="server" Text="查询" Icon="DatabaseGo">
                                    <Listeners>
                                        <Click Handler="#{Store1}.reload();#{btn_Delete}.disable();" />
                                    </Listeners>
                                </ext:Button>
                                <ext:ToolbarSeparator>
                                </ext:ToolbarSeparator>
                                <ext:Button ID="btn_Add" runat="server" Text="添加培养计划" Icon="Add">
                                    <Listeners>
                                        <Click Handler="if(#{DeptCodeCombo}.getSelectedItem().value == '') {Ext.Msg.show({ title: '信息提示', msg: '请选择科室', icon: 'ext-mb-info', buttons: { ok: true }  });} else {TreeOpration(1)}" />
                                    </Listeners>
                                </ext:Button>
                                <ext:Button Text="修改培养计划" ID="btn_Modify" runat="server" Icon="FolderEdit" Disabled="true">
                                    <Listeners>
                                        <Click Handler="TreeOpration(2)" />
                                    </Listeners>
                                </ext:Button>
                                <ext:Button Text="删除培养计划" ID="btn_Delete" runat="server" Icon="Delete" Disabled="true">
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
                                                        <ext:Column ColumnID="DEPT_NAME" Header="科室" Sortable="true" DataIndex="DEPT_NAME" />
                                                        <ext:Column ColumnID="SORT" Header="类别" Sortable="true" DataIndex="SORT" />
                                                        <ext:Column ColumnID="PERSONS" Header="人员" Sortable="true" DataIndex="PERSONS" />
                                                        <ext:Column ColumnID="UNIT" Header="单位或地址" Sortable="true" DataIndex="UNIT" />
                                                        <ext:Column ColumnID="START_DATE" Header="起始时间" Sortable="true" DataIndex="START_DATE" />
                                                        <ext:Column ColumnID="END_DATE" Header="结束时间" Sortable="true" DataIndex="END_DATE" />
                                                        <ext:Column ColumnID="TERM" Header="期限" Sortable="true" DataIndex="TERM" />
                                                        <ext:Column ColumnID="CONTENT" Header="内容" Sortable="true" DataIndex="CONTENT" />
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
    <ext:Window ID="arcEditWindow" runat="server" Icon="Group" Title="人才培养记录" Width="600"
        Height="300" AutoShow="false" Modal="true" CenterOnLoad="true" ShowOnLoad="false"
        Resizable="false" StyleSpec="background-color:Transparent;" BodyStyle="background-color:Transparent;">
        <Body>
            <ext:ColumnLayout ID="ColumnLayout2" runat="server">
                <ext:LayoutColumn ColumnWidth=".4">
                    <ext:Panel ID="Panel2" runat="server" Border="false" Header="false" BodyStyle="background-color:Transparent;margin:10px;">
                        <Body>
                            <ext:FormLayout ID="FormLayout1" runat="server" LabelAlign="Left">
                                <ext:Anchor Horizontal="95%">
                                    <ext:ComboBox ID="cboSort" runat="server" FieldLabel="类别" />
                                </ext:Anchor>
                                <ext:Anchor Horizontal="95%">
                                    <ext:ComboBox ID="cboStaffInfo" runat="server" StoreID="Store2" DisplayField="STAFF_NAME"
                                        Width="120" ValueField="STAFF_ID" TypeAhead="false" LoadingText="Searching..."
                                        PageSize="1000" ItemSelector="div.search-item" MinChars="1" FieldLabel="人员" ListWidth="240">
                                        <Template ID="Template2" runat="server">
                                       <tpl for=".">
                                          <div class="search-item">
                                             <h3><span style="width:auto">{STAFF_ID}</span>{STAFF_NAME}　　　　{DEPT_NAME}</h3>
                                          </div>
                                       </tpl>
                                        </Template>
                                    </ext:ComboBox>
                                </ext:Anchor>
                                <ext:Anchor Horizontal="95%">
                                    <ext:TextField ID="txtUnit" runat="server" FieldLabel="单位或地址" />
                                </ext:Anchor>
                                <ext:Anchor Horizontal="95%">
                                    <ext:DateField ID="dtfStartDate" runat="server" FieldLabel="起始日期" Format="yyyy-MM-dd" />
                                </ext:Anchor>
                                <ext:Anchor Horizontal="95%">
                                    <ext:DateField ID="dtfEndDate" runat="server" FieldLabel="结束日期" Format="yyyy-MM-dd" />
                                </ext:Anchor>
                                <ext:Anchor Horizontal="95%">
                                    <ext:TextField ID="txtTerm" runat="server" FieldLabel="期限" />
                                </ext:Anchor>
                                <ext:Anchor Horizontal="95%">
                                    <ext:TextField ID="txtDeptSpecial" runat="server" FieldLabel="培养专业" />
                                </ext:Anchor>
                            </ext:FormLayout>
                        </Body>
                    </ext:Panel>
                </ext:LayoutColumn>
                <ext:LayoutColumn ColumnWidth=".6">
                    <ext:Panel ID="Panel3" runat="server" Border="false" BodyStyle="background-color:Transparent;margin:10px;">
                        <Body>
                            <ext:FormLayout ID="FormLayout2" runat="server" LabelAlign="Left">
                                <ext:Anchor Horizontal="92%">
                                    <ext:TextArea ID="txrContent" runat="server" FieldLabel="结业情况" Height="80" MaxLength="350" />
                                </ext:Anchor>
                                <ext:Anchor Horizontal="92%">
                                    <ext:TextField ID="txtLightspotName" runat="server" FieldLabel="优势或亮点名称" />
                                </ext:Anchor>
                                <ext:Anchor Horizontal="92%">
                                    <ext:TextArea ID="txtFinishingSituation" runat="server" FieldLabel="年度完成情况" Height="50" />
                                </ext:Anchor>
                                <ext:Anchor Horizontal="92%">
                                    <ext:TextArea ID="txrCheckResult" runat="server" FieldLabel="考核结果" Height="50" />
                                </ext:Anchor>
                            </ext:FormLayout>
                        </Body>
                    </ext:Panel>
                </ext:LayoutColumn>
            </ext:ColumnLayout>
        </Body>
        <BottomBar>
            <ext:Toolbar ID="Toolbar2" runat="server">
                <Items>
                    <ext:ToolbarFill ID="ToolbarFill2" runat="server" />
                    <ext:ToolbarButton ID="Btn_BatStart" runat="server" Icon="Disk" Text="保存">
                        <AjaxEvents>
                            <Click OnEvent="SaveInfo">
                            </Click>
                        </AjaxEvents>
                    </ext:ToolbarButton>
                    <ext:ToolbarButton ID="btnEdit" runat="server" Icon="Disk" Text="修改">
                        <AjaxEvents>
                            <Click OnEvent="EditInfo">
                                <ExtraParams>
                                    <ext:Parameter Name="deptCode" Value="#{Store1}.getAt(RowIndex).data.DEPT_CODE" Mode="Raw">
                                    </ext:Parameter>
                                    <ext:Parameter Name="deptName" Value="#{Store1}.getAt(RowIndex).data.DEPT_NAME" Mode="Raw">
                                    </ext:Parameter>
                                </ExtraParams>
                            </Click>
                        </AjaxEvents>
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
