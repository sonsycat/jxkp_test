﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DeptBaseInfoSet.aspx.cs"
    Inherits="GoldNet.JXKP.RLZY.BaseInfoMaintain.DeptBaseInfoSet" %>

<%@ Register Assembly="Goldnet.Ext.Web" Namespace="Goldnet.Ext.Web" TagPrefix="ext" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>无标题页</title>
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

    <script type="text/javascript">
       var RowIndex;                   
        /*
            GRIDPANEL操作
            optype :1 添加;  2 重命名; 3 删除;
            TabPanel1.getActiveTab().id
        */
        function TreeOpration(optype) {
            if (optype == "1") {
            } else if (optype == "2") {
            } else if (optype == "3") {
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

            if (optype == "1") {          
            } else if (optype == "2") {
   
            } else if (optype == "3") {
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
//                            btn_Delete.setDisabled(true);
//                            arcEditWindow.hide();
                        },
                        failure: function(msg) {
                            GridPanel_Show.el.unmask();
                        }
                    });
        }
//        
//        var tabChangeBtn = function() {
//            if(TabPanel1.getActiveTab().id == 'Tab1') {
//                  btn_Modify.setDisabled(true);
//                  btn_Delete.setDisabled(true);
//                  btn_Add.setDisabled(true);
//            } else {
//                  btn_Add.setDisabled(false);
//            }
//        }
        
        
    </script>

</head>
<body>
    <form id="form1" runat="server">
    <ext:ScriptManager ID="ScriptManager1" runat="server" />
    <%-- <ext:Store ID="Store1" runat="server">
        <Reader>
            <ext:JsonReader>
                <Fields>
                    <ext:RecordField Name="DEPT_NAME" />
                    <ext:RecordField Name="ATTRIBUE" />
                    <ext:RecordField Name="WEAVE_BED" />
                    <ext:RecordField Name="DEPLOY_BED" />
                    <ext:RecordField Name="DIRECTOR" />
                    <ext:RecordField Name="SUBDIRECOTR" />
                    <ext:RecordField Name="CHARGE_NURSE" />
                    <ext:RecordField Name="SPEC_SORT_NAME" />
                    <ext:RecordField Name="SORT_NAME" />
                    <ext:RecordField Name="CENTER_NAME" />
                    <ext:RecordField Name="IS_PIVOT_DEPT" />
                </Fields>
            </ext:JsonReader>
        </Reader>
    </ext:Store>--%>
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
    <div>
        <ext:ViewPort ID="ViewPort1" runat="server">
            <Body>
                <ext:BorderLayout ID="BorderLayout2" runat="server">
                    <North>
                        <ext:Toolbar runat="server" ID="ctl155" StyleSpec="border:0">
                            <Items>
                                <ext:Label ID="Label3" runat="server" Text="科室：">
                                </ext:Label>
                                <ext:ComboBox ID="DeptCodeCombo" runat="server" StoreID="Store3" DisplayField="DEPT_NAME"
                                    Width="120" ValueField="DEPT_CODE" TypeAhead="false" LoadingText="Searching..."
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
                                <ext:Button ID="btnSearch" runat="server" Text="查询" Icon="DatabaseGo">
                                    <AjaxEvents>
                                        <Click OnEvent="QueryDept">
                                            <EventMask ShowMask="true" Msg="请稍候..." />
                                        </Click>
                                    </AjaxEvents>
                                </ext:Button>
                                <ext:Button Text="修改科室信息" ID="btnModifyDeptInfo" runat="server" Icon="FolderEdit">
                                    <AjaxEvents>
                                        <Click OnEvent="InsertInfo">
                                        </Click>
                                    </AjaxEvents>
                                </ext:Button>
                               <%-- <ext:Button ID="btn_Add" runat="server" Text="添加项目" Icon="Add" Disabled="true">
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
                                </ext:Button>--%>
                            </Items>
                        </ext:Toolbar>
                    </North>
                    <Center>
                        <%--                 <ext:TabPanel ID="TabPanel1" runat="server" ActiveTabIndex="0" Width="450">
                            <Tabs>
                                <ext:Tab ID="Tab1" runat="server" Title="基础信息维护" AutoHeight="true" BodyStyle="padding: 6px;">
                                    <Body>--%>
                        <ext:Panel ID="Panel3" runat="server">
                            <Body>
                                <ext:ColumnLayout ID="ColumnLayout1" runat="server">
                                    <ext:LayoutColumn ColumnWidth=".2">
                                        <ext:Panel ID="Panel1" runat="server" Border="false" Header="false" BodyStyle="margin:10px;">
                                            <Body>
                                                <ext:FormLayout ID="FormLayout1" runat="server" LabelAlign="Left">
                                                    <ext:Anchor Horizontal="92%">
                                                        <ext:TextField ID="txtDeptDirector" runat="server" FieldLabel="科主任(负责人)" AllowBlank="false" />
                                                    </ext:Anchor>
                                                    <ext:Anchor Horizontal="92%">
                                                        <ext:NumberField ID="txtWeaveBed" runat="server" FieldLabel="编制床位" />
                                                    </ext:Anchor>
                                                    <ext:Anchor Horizontal="92%">
                                                        <ext:TextField ID="txtChargeNurse" runat="server" FieldLabel="护士长" />
                                                    </ext:Anchor>
                                                    <ext:Anchor Horizontal="92%">
                                                        <ext:NumberField ID="txtShouleNum" runat="server" FieldLabel="干部床" />
                                                    </ext:Anchor>
                                                    <ext:Anchor Horizontal="92%">
                                                        <ext:NumberField ID="txtShouldNum" runat="server" FieldLabel="实有人数" />
                                                    </ext:Anchor>
                                                    <ext:Anchor Horizontal="92%">
                                                        <ext:NumberField ID="txtApproManager" runat="server" FieldLabel="编制管理人数" />
                                                    </ext:Anchor>
                                                    <ext:Anchor Horizontal="92%">
                                                        <ext:ComboBox ID="cboTraining" runat="server" FieldLabel="所属专科中心" />
                                                    </ext:Anchor>
                                                    <ext:Anchor Horizontal="92%">
                                                        <ext:ComboBox ID="cboDeptAttrib" runat="server" FieldLabel="临床科室属性" />
                                                    </ext:Anchor>
                                                    <ext:Anchor Horizontal="92%">
                                                        <ext:TextField ID="txtSpesManager" runat="server" FieldLabel="专业负责人" />
                                                    </ext:Anchor>
                                                    <ext:Anchor Horizontal="92%">
                                                        <ext:ComboBox ID="cboTernalOrSergery" runat="server" FieldLabel="内外科标识" />
                                                    </ext:Anchor>
                                                    <ext:Anchor Horizontal="92%">
                                                        <ext:NumberField ID="txtApproDrug" runat="server" FieldLabel="编制药剂专业人数" />
                                                    </ext:Anchor>
                                                    <ext:Anchor Horizontal="92%">
                                                        <ext:NumberField ID="txtApproProject" runat="server" FieldLabel="编制工程专业人数" />
                                                    </ext:Anchor>
                                                </ext:FormLayout>
                                            </Body>
                                        </ext:Panel>
                                    </ext:LayoutColumn>
                                    <ext:LayoutColumn ColumnWidth=".2">
                                        <ext:Panel ID="Panel2" runat="server" Border="false" BodyStyle="margin:10px;">
                                            <Body>
                                                <ext:FormLayout ID="FormLayout2" runat="server" LabelAlign="Left">
                                                    <ext:Anchor Horizontal="92%">
                                                        <ext:TextField ID="txtViceDirector" runat="server" FieldLabel="副主任" />
                                                    </ext:Anchor>
                                                    <ext:Anchor Horizontal="92%">
                                                        <ext:NumberField ID="txtDeployBed" runat="server" FieldLabel="展开床位" />
                                                    </ext:Anchor>
                                                    <ext:Anchor Horizontal="92%">
                                                        <ext:ComboBox ID="cboDeptSpeciality" runat="server" FieldLabel="科室专业" />
                                                    </ext:Anchor>
                                                    <ext:Anchor Horizontal="92%">
                                                        <ext:ComboBox ID="cboOutpOrInp" runat="server" FieldLabel="门诊住院标识" />
                                                    </ext:Anchor>
                                                    <ext:Anchor Horizontal="92%">
                                                        <ext:NumberField ID="txtApproDoctor" runat="server" FieldLabel="编制医疗专业人数" />
                                                    </ext:Anchor>
                                                    <ext:Anchor Horizontal="92%">
                                                        <ext:NumberField ID="txtApproTech" runat="server" FieldLabel="编制医技专业人数" />
                                                    </ext:Anchor>
                                                    <ext:Anchor Horizontal="92%">
                                                        <ext:ComboBox ID="cboTrainingType" runat="server" FieldLabel="所属专科中心类别" />
                                                    </ext:Anchor>
                                                    <ext:Anchor Horizontal="92%">
                                                        <ext:ComboBox ID="cboPivotDept" runat="server" FieldLabel="重点科室">
                                                            <Items>
                                                                <ext:ListItem Text="是" Value="1" />
                                                                <ext:ListItem Text="否" Value="0" />
                                                            </Items>
                                                        </ext:ComboBox>
                                                    </ext:Anchor>
                                                    <ext:Anchor Horizontal="92%">
                                                        <ext:ComboBox ID="cboDeptattr" runat="server" FieldLabel="科室属性" />
                                                    </ext:Anchor>
                                                    <ext:Anchor Horizontal="92%">
                                                        <ext:NumberField ID="txtApproNum" runat="server" FieldLabel="编制人数" />
                                                    </ext:Anchor>
                                                    <ext:Anchor Horizontal="92%">
                                                        <ext:NumberField ID="txtApproNurse" runat="server" FieldLabel="编制护理专业人数" />
                                                    </ext:Anchor>
                                                    <ext:Anchor Horizontal="92%">
                                                        <ext:NumberField ID="txtApproOther" runat="server" FieldLabel="编制其它专业人数" />
                                                    </ext:Anchor>
                                                </ext:FormLayout>
                                            </Body>
                                        </ext:Panel>
                                    </ext:LayoutColumn>
                                      <ext:LayoutColumn ColumnWidth=".6">
                                        <ext:Panel ID="Panel4" runat="server" Border="false">
                                            <Body>
                                                <ext:FormLayout ID="FormLayout3" runat="server" LabelAlign="Left">
                                                </ext:FormLayout>
                                            </Body>
                                        </ext:Panel>
                                    </ext:LayoutColumn>
                                </ext:ColumnLayout>
                                
                                <%--    </Body>
                                </ext:Tab>
                                <ext:Tab ID="Tab2" runat="server" Title="科室核算组维护" AutoHeight="true" BodyStyle="padding: 6px;">
                                    <Body>--%>
                                <%--<ext:Panel ID="Panel3" runat="server">
                                            <Body>
                                                <ext:ColumnLayout ID="ColumnLayout2" runat="server" Split="true" FitHeight="true">
                                                    <Columns>
                                                        <ext:LayoutColumn ColumnWidth="1">
                                                            <ext:GridPanel ID="GridPanel_Show" runat="server" StoreID="Store1" Border="false"
                                                                AutoWidth="true" Header="false" AutoScroll="true">
                                                                <ColumnModel ID="ColumnModel1" runat="server">
                                                                    <Columns>
                                                                        <ext:Column ColumnID="STAT_MONTH" Header="核算组代码" Sortable="true" DataIndex="STAT_MONTH" />
                                                                        <ext:Column ColumnID="APP_SUBSYS_NUM" Header="核算组名称" Sortable="true" DataIndex="APP_SUBSYS_NUM" />
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
                                        </ext:Panel>--%>
                                <%--                             </Body>
                                </ext:Tab>
                            </Tabs>
                            <Listeners>
                                <TabChange Handler="tabChangeBtn();" />
                            </Listeners>
                        </ext:TabPanel>--%>
                            </Body>
                        </ext:Panel>
                    </Center>
                </ext:BorderLayout>
            </Body>
        </ext:ViewPort>
    </div>
    </form>
</body>
</html>
