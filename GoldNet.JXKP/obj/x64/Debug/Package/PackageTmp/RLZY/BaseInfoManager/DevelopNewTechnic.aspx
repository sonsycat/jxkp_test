<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DevelopNewTechnic.aspx.cs"
    Inherits="GoldNet.JXKP.RLZY.BaseInfoManager.DevelopNewTechnic" %>

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

                txtName.setValue("");      //        新技术新业务名称                                
                cboPrincipal.setValue("");      //        负责人
                txtClubDept.setValue("");      //        主要协作科室                                
                cboNewTechnic.selectByIndex(0);      //        新技术新业务                                
                cboPrincipalSchoolAge.selectByIndex(0);      //        负责人学历                                
                cboPrincipalJob.selectByIndex(0);      //        负责人职称                                
                txrJoinPersons.setValue("");      //        参加人员                                    
                dtfDates.setValue(data);      //        开展时间                                
                txtCompCase.setValue("");      //        完成例数                                
                cboLevelCol.selectByIndex(0);      //        水平                                
                cboEffect.selectByIndex(0);      //        效果                                
                txtBrief.setValue("");      //        进展情况

                txrSug.setValue("");
                txrSetSug.setValue("");

                txtName.setDisabled(false);      //        新技术新业务名称                                
                cboPrincipal.setDisabled(false);      //        负责人
                txtClubDept.setDisabled(false);      //        主要协作科室                                
                cboNewTechnic.setDisabled(false);        //        新技术新业务                                
                cboPrincipalSchoolAge.setDisabled(false);        //        负责人学历                                
                cboPrincipalJob.setDisabled(false);        //        负责人职称                                
                txrJoinPersons.setDisabled(false);      //        参加人员                                    
                dtfDates.setDisabled(false);     //        开展时间                                
                txtCompCase.setDisabled(false);      //        完成例数                                
                cboLevelCol.setDisabled(false);        //        水平                                
                cboEffect.setDisabled(false);        //        效果   

                txrSug.setVisible(true);
                txrSug.setDisabled(true);
                txrSetSug.setVisible(true);
                txrSetSug.setDisabled(true);

                Btn_BatStart.setVisible(true);
                btnApprove.setVisible(false);
                btnNotApprove.setVisible(false);
                btnSetUp.setVisible(false);
                btnSetSave.setVisible(false);
                btnSaveSet.setVisible(false);

                arcEditWindow.show();
            } else if (optype == "2") {
                //初始化
                HiddenId.setValue(record.data.ID);
                txtName.setValue(record.data.NAME);      //        新技术新业务名称                                
                cboPrincipal.setValue(record.data.PRINCIPAL);      //        负责人
                txtClubDept.setValue(record.data.CLUB_DEPT);      //        主要协作科室                                
                cboNewTechnic.setValue(record.data.NEW_TECHNIC);      //        新技术新业务                                
                cboPrincipalSchoolAge.setValue(record.data.PRINCIPAL_SCHOOL_AGE);      //        负责人学历                                
                cboPrincipalJob.setValue(record.data.PRINCIPAL_JOB);      //        负责人职称                                
                txrJoinPersons.setValue(record.data.JOIN_PERSONS);      //        参加人员                                    
                dtfDates.setValue(record.data.DATES);      //        开展时间                                
                txtCompCase.setValue(record.data.COMP_CASE);      //        完成例数                                
                cboLevelCol.setValue(record.data.LEVELCOL);      //        水平                                
                cboEffect.setValue(record.data.EFFECT);      //        效果                                
                txtBrief.setValue(record.data.BRIEF);      //        进展情况

                txrSug.setValue(record.data.MARK_SUG);
                txrSetSug.setValue(record.data.SETUP_SUG);

                Btn_BatStart.setVisible(false);
                btnApprove.setVisible(false);
                btnSetUp.setVisible(false);

                //院级审批权限  liu.shh 2012.12.19
                if (PowerInfoHidden.value == 1) {
                    txrSug.setVisible(true);
                    txrSug.setDisabled(false);
                    txrSetSug.setVisible(true);
                    txrSetSug.setDisabled(false);

                    txtName.setDisabled(false);      //        新技术新业务名称                                
                    cboPrincipal.setDisabled(false);      //        负责人
                    txtClubDept.setDisabled(false);      //        主要协作科室                                
                    cboNewTechnic.setDisabled(false);        //        新技术新业务                                
                    cboPrincipalSchoolAge.setDisabled(false);        //        负责人学历                                
                    cboPrincipalJob.setDisabled(false);        //        负责人职称                                
                    txrJoinPersons.setDisabled(false);      //        参加人员                                    
                    dtfDates.setDisabled(false);     //        开展时间                                
                    txtCompCase.setDisabled(false);      //        完成例数                                
                    cboLevelCol.setDisabled(false);        //        水平                                
                    cboEffect.setDisabled(false);        //        效果                                
                    txtBrief.setDisabled(false);

                    Btn_BatStart.setVisible(false); //保存按钮不可见
                    btnSetUp.setVisible(false);   //纯提交，不可见
                    btnApprove.setVisible(true); //审批按钮可见
                    btnNotApprove.setVisible(true); //审批不通过按钮可见
                    btnSetSave.setVisible(false); //修改按钮不可见
                    btnSaveSet.setVisible(false); //保存时提交，不可见
                }
                //科主任权限  liu.shh  2012.12.19
                if (PowerInfoHidden.value == 2 && hiddenEdit.value == '1') {
                    txrSug.setVisible(true);
                    txrSug.setDisabled(true);
                    txrSetSug.setVisible(true);
                    txrSetSug.setDisabled(false);

                    if (!cbxOpration.checked) {
                        if (record.data.ADD_MARK == "审批通过") {  //审批已通过的不可再修改
                            txtName.setDisabled(true);      //        新技术新业务名称                                
                            cboPrincipal.setDisabled(true);      //        负责人
                            txtClubDept.setDisabled(true);      //        主要协作科室                                
                            cboNewTechnic.setDisabled(true);        //        新技术新业务                                
                            cboPrincipalSchoolAge.setDisabled(true);        //        负责人学历                                
                            cboPrincipalJob.setDisabled(true);        //        负责人职称                                
                            txrJoinPersons.setDisabled(true);      //        参加人员                                    
                            dtfDates.setDisabled(true);     //        开展时间                                
                            txtCompCase.setDisabled(true);      //        完成例数                                
                            cboLevelCol.setDisabled(true);        //        水平                                
                            cboEffect.setDisabled(true);        //        效果                                
                            txtBrief.setDisabled(true);

                            txrSug.setDisabled(true);
                            txrSetSug.setDisabled(true);

                            Btn_BatStart.setVisible(false); //保存按钮不可见
                            btnSetUp.setVisible(false);   //纯提交，不可见
                            btnApprove.setVisible(false); //审批按钮不可见
                            btnNotApprove.setVisible(false); //审批不通过按钮不可见
                            btnSetSave.setVisible(false); //修改按钮不可见
                            btnSaveSet.setVisible(false); //保存时提交，不可见
                        }
                        else {
                            txtName.setDisabled(false);      //        新技术新业务名称                                
                            cboPrincipal.setDisabled(false);      //        负责人
                            txtClubDept.setDisabled(false);      //        主要协作科室                                
                            cboNewTechnic.setDisabled(false);        //        新技术新业务                                
                            cboPrincipalSchoolAge.setDisabled(false);        //        负责人学历                                
                            cboPrincipalJob.setDisabled(false);        //        负责人职称                                
                            txrJoinPersons.setDisabled(false);      //        参加人员                                    
                            dtfDates.setDisabled(false);     //        开展时间                                
                            txtCompCase.setDisabled(false);      //        完成例数                                
                            cboLevelCol.setDisabled(false);        //        水平                                
                            cboEffect.setDisabled(false);        //        效果                                
                            txtBrief.setDisabled(false);

                            Btn_BatStart.setVisible(false); //保存按钮不可见
                            btnSetUp.setVisible(true);   //纯提交，可见
                            btnApprove.setVisible(false); //审批按钮不可见
                            btnNotApprove.setVisible(false); //审批不通过按钮不可见
                            btnSetSave.setVisible(true); //修改按钮可见
                            btnSaveSet.setVisible(false); //保存时提交，不可见
                        }
                    }
                    if (cbxOpration.checked) {
                        txtName.setDisabled(false);      //        新技术新业务名称                                
                        cboPrincipal.setDisabled(false);      //        负责人
                        txtClubDept.setDisabled(false);      //        主要协作科室                                
                        cboNewTechnic.setDisabled(false);        //        新技术新业务                                
                        cboPrincipalSchoolAge.setDisabled(false);        //        负责人学历                                
                        cboPrincipalJob.setDisabled(false);        //        负责人职称                                
                        txrJoinPersons.setDisabled(false);      //        参加人员                                    
                        dtfDates.setDisabled(false);     //        开展时间                                
                        txtCompCase.setDisabled(false);      //        完成例数                                
                        cboLevelCol.setDisabled(false);        //        水平                                
                        cboEffect.setDisabled(false);        //        效果                                
                        txtBrief.setDisabled(false);

                        Btn_BatStart.setVisible(false); //保存按钮不可见
                        btnSetUp.setVisible(true);   //纯提交，可见
                        btnApprove.setVisible(false); //审批按钮不可见
                        btnNotApprove.setVisible(false); //审批不通过按钮不可见
                        btnSetSave.setVisible(true); //修改按钮可见
                        btnSaveSet.setVisible(false); //保存时提交，不可见
                    }
                }
                //普通医生权限  liu.shh  2012.12.19
                if (PowerInfoHidden.value == 0) {
                    txrSug.setVisible(true);
                    txrSug.setDisabled(true);
                    txrSetSug.setVisible(true);
                    txrSetSug.setDisabled(true);

                    if (cbxOpration.checked) {
                        txtName.setDisabled(false);      //        新技术新业务名称                                
                        cboPrincipal.setDisabled(false);      //        负责人
                        txtClubDept.setDisabled(false);      //        主要协作科室                                
                        cboNewTechnic.setDisabled(false);        //        新技术新业务                                
                        cboPrincipalSchoolAge.setDisabled(false);        //        负责人学历                                
                        cboPrincipalJob.setDisabled(false);        //        负责人职称                                
                        txrJoinPersons.setDisabled(false);      //        参加人员                                    
                        dtfDates.setDisabled(false);     //        开展时间                                
                        txtCompCase.setDisabled(false);      //        完成例数                                
                        cboLevelCol.setDisabled(false);        //        水平                                
                        cboEffect.setDisabled(false);        //        效果                                
                        txtBrief.setDisabled(false);

                        Btn_BatStart.setVisible(false); //保存按钮不可见
                        btnSetUp.setVisible(false);   //纯提交，不可见
                        btnApprove.setVisible(false); //审批按钮不可见
                        btnNotApprove.setVisible(false); //审批不通过按钮不可见
                        btnSetSave.setVisible(true); //修改按钮可见
                        btnSaveSet.setVisible(false); //保存时提交，不可见
                    }
                    if (!cbxOpration.checked) {
                        if (record.data.ADD_MARK == "审批通过") {  //审批已通过的不可再修改
                            txtName.setDisabled(true);      //        新技术新业务名称                                
                            cboPrincipal.setDisabled(true);      //        负责人
                            txtClubDept.setDisabled(true);      //        主要协作科室                                
                            cboNewTechnic.setDisabled(true);        //        新技术新业务                                
                            cboPrincipalSchoolAge.setDisabled(true);        //        负责人学历                                
                            cboPrincipalJob.setDisabled(true);        //        负责人职称                                
                            txrJoinPersons.setDisabled(true);      //        参加人员                                    
                            dtfDates.setDisabled(true);     //        开展时间                                
                            txtCompCase.setDisabled(true);      //        完成例数                                
                            cboLevelCol.setDisabled(true);        //        水平                                
                            cboEffect.setDisabled(true);        //        效果                                
                            txtBrief.setDisabled(true);

                            Btn_BatStart.setVisible(false); //保存按钮不可见
                            btnSetUp.setVisible(false);   //纯提交，不可见
                            btnApprove.setVisible(false); //审批按钮不可见
                            btnNotApprove.setVisible(false); //审批不通过按钮不可见
                            btnSetSave.setVisible(false); //修改按钮不可见
                            btnSaveSet.setVisible(false); //保存时提交，不可见
                        }
                        else { //审批不通过的可以再修改
                            txtName.setDisabled(false);      //        新技术新业务名称                                
                            cboPrincipal.setDisabled(false);      //        负责人
                            txtClubDept.setDisabled(false);      //        主要协作科室                                
                            cboNewTechnic.setDisabled(false);        //        新技术新业务                                
                            cboPrincipalSchoolAge.setDisabled(false);        //        负责人学历                                
                            cboPrincipalJob.setDisabled(false);        //        负责人职称                                
                            txrJoinPersons.setDisabled(false);      //        参加人员                                    
                            dtfDates.setDisabled(false);     //        开展时间                                
                            txtCompCase.setDisabled(false);      //        完成例数                                
                            cboLevelCol.setDisabled(false);        //        水平                                
                            cboEffect.setDisabled(false);        //        效果                                
                            txtBrief.setDisabled(false);

                            Btn_BatStart.setVisible(false); //保存按钮不可见
                            btnSetUp.setVisible(false);   //纯提交，不可见
                            btnApprove.setVisible(false); //审批按钮不可见
                            btnNotApprove.setVisible(false); //审批不通过按钮不可见
                            btnSetSave.setVisible(true); //修改按钮可见
                            btnSaveSet.setVisible(false); //保存时提交，不可见
                        }
                    }
                }
                arcEditWindow.show();
            } else if (optype == "3") {
                if(!cbxOpration.checked && PowerInfoHidden.value != 1){
                    Ext.MessageBox.alert("提示","项目已审批，不可再删除！");
                    return;
                }
                var selections = CheckboxSelectionModel1.getSelections();
                Btn_BatStart.setText("");
                Ext.Msg.confirm("删除项目", "确定要删除该项目吗？", function(btn, text) { if((btn != "ok") && (btn != "yes")){return;} else {OperEchoCallback(selections,optype);}});
            } else if(optype == "4") {
                optype  = PowerInfoHidden.value==1?"6":"4";
                var selections = CheckboxSelectionModel1.getSelections();
                Btn_BatStart.setText("");
                var Name = PowerInfoHidden.value==1?"审批":"提交";
                Ext.Msg.confirm("批量"+Name+"项目", "确定要批量"+Name+"项目吗？", function(btn, text) { if((btn != "ok") && (btn != "yes")){return;} else {OperEchoCallback(selections,optype);}});
            }
        }
        
        function OperEchoCallback(selections,optype) {
            var id = '';
            for(var i =0;i<selections.length;i++) {
                if(i == selections.length -1) {
                    id = id+selections[i].data.ID;
                } else {
                    id = id+selections[i].data.ID +',';
                }
            }
            GridPanelToDataBase(id,optype);
        }
        
      
        
        function GridPanelToDataBase(id,optype) {
          Goldnet.AjaxMethod.request(
                  'FruitListAjaxOper',
                    {
                        params: {
                           Id:id,optype:optype
                        },
                        success: function(result) {
                            Store1.reload();
                            btn_EchoHandle.setDisabled(true);
                            btn_Delete.setDisabled(true);
                            arcEditWindow.hide();
                        },
                        failure: function(msg) {
                            GridPanel_Show.el.unmask();
                        }
                    });
        }
        
        var prepare = function(grid, toolbar, rowIndex, record) {
            var menuButton = toolbar.items.get(0);
            var menu1 = menuButton.menu.items.get(0);
            menu1.setDisabled(!hiddenMeunUp.getValue());
        }
        
        var gridCommand = function(command, record) {
              TreeOpration(2,record);
        }  
        
        
        
         var CheckForm = function() {
            if (txtCompCase.validate() == false) {
                return false;
            }
            return true;
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
    <ext:Hidden ID="PowerInfoHidden" runat="server">
    </ext:Hidden>
    <ext:Hidden ID="HiddenId" runat="server">
    </ext:Hidden>
    <ext:Hidden ID="hiddenMeunUp" runat="server">
    </ext:Hidden>
    <ext:Hidden ID="hiddenEdit" runat="server">
    </ext:Hidden>
    <ext:Hidden ID="ADD_MARK" runat="server">
    </ext:Hidden>
    <ext:Store ID="Store1" runat="server" OnRefreshData="Data_RefreshData">
        <Reader>
            <ext:JsonReader>
                <Fields>
                    <ext:RecordField Name="ID" />
                    <ext:RecordField Name="DEPT_CODE" />
                    <ext:RecordField Name="DEPT_NAME" />
                    <ext:RecordField Name="NEW_TECHNIC" />
                    <ext:RecordField Name="NAME" />
                    <ext:RecordField Name="PRINCIPAL" />
                    <ext:RecordField Name="PRINCIPAL_SCHOOL_AGE" />
                    <ext:RecordField Name="PRINCIPAL_JOB" />
                    <ext:RecordField Name="JOIN_PERSONS" />
                    <ext:RecordField Name="DATES" />
                    <ext:RecordField Name="BRIEF" />
                    <ext:RecordField Name="CLUB_DEPT" />
                    <ext:RecordField Name="COMP_CASE" />
                    <ext:RecordField Name="LEVELCOL" />
                    <ext:RecordField Name="EFFECT" />
                    <ext:RecordField Name="ADD_MARK" />
                    <ext:RecordField Name="STAFF_ID" />
                    <ext:RecordField Name="MARK_SUG"/>
                    <ext:RecordField Name="SETUP_SUG"/>
                    <ext:RecordField Name="ADD_MARK"/>
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
                                <ext:Label ID="Label2" runat="server" Text="年度:">
                                </ext:Label>
                                <ext:ComboBox ID="TimeOrgan" runat="server" Width="40">
                                    <Items>
                                        <ext:ListItem Text="&lt;=" Value="&lt;=" />
                                        <ext:ListItem Text="&gt;=" Value="&gt;=" />
                                        <ext:ListItem Text="=" Value="=" />
                                    </Items>
                                </ext:ComboBox>
                                <ext:ComboBox ID="cboTime" runat="server" Width="55">
                                </ext:ComboBox>
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
                                <ext:Button ID="btn_Add" runat="server" Text="添加新技术" Icon="Add">
                                    <Listeners>
                                        <Click Handler="if(#{DeptCodeCombo}.getSelectedItem().value == '') {Ext.Msg.show({ title: '信息提示', msg: '请选择科室', icon: 'ext-mb-info', buttons: { ok: true }  });} else {TreeOpration(1,'')}" />
                                    </Listeners>
                                </ext:Button>
                                <ext:Button Text="删除新技术" ID="btn_Delete" runat="server" Icon="Delete" Disabled="true">
                                    <Listeners>
                                        <Click Handler="TreeOpration(3)" />
                                    </Listeners>
                                </ext:Button>
                                <ext:Button Text="批量处理" ID="btn_EchoHandle" runat="server" Icon="Wrench" Disabled="true">
                                    <Listeners>
                                        <Click Handler="TreeOpration(4)" />
                                    </Listeners>
                                </ext:Button>
                                <ext:Checkbox ID="cbxOpration" runat="server" BoxLabel="显示">
                                    <Listeners>
                                        <Check Handler="#{Store1}.reload();#{btn_EchoHandle}.disable();#{btn_Delete}.disable();" />
                                    </Listeners>
                                </ext:Checkbox>
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
                                                        <ext:Column ColumnID="NEW_TECHNIC" Header="类别" Sortable="true" DataIndex="NEW_TECHNIC" />
                                                        <ext:Column ColumnID="NAME" Header="名称" Sortable="true" DataIndex="NAME" />
                                                        <ext:Column ColumnID="PRINCIPAL" Header="负责人" Sortable="true" DataIndex="PRINCIPAL" />
                                                        <ext:Column ColumnID="PRINCIPAL_SCHOOL_AGE" Header="负责人学历" Sortable="true" DataIndex="PRINCIPAL_SCHOOL_AGE" />
                                                        <ext:Column ColumnID="PRINCIPAL_JOB" Header="负责人职称" Sortable="true" DataIndex="PRINCIPAL_JOB" />
                                                        <ext:Column ColumnID="JOIN_PERSONS" Header="参加人员" Sortable="true" DataIndex="JOIN_PERSONS" />
                                                        <ext:Column ColumnID="DATES" Header="开展日期" Sortable="true" DataIndex="DATES" />
                                                        <ext:Column ColumnID="BRIEF" Header="进展情况" Sortable="true" DataIndex="BRIEF" />
                                                        <ext:Column ColumnID="ADD_MARK" Header="审批标识" Sortable="true" DataIndex="ADD_MARK" />
                                                        <ext:Column ColumnID="SETUP_SUG" Header="提交意见" Sortable="true" DataIndex="SETUP_SUG" />
                                                        <ext:Column ColumnID="MARK_SUG" Header="审批意见" Sortable="true" DataIndex="MARK_SUG" />
                                                        <ext:CommandColumn Width="38" Header="操作">
                                                            <Commands>
                                                                <ext:SplitCommand Icon="TableMultiple">
                                                                    <ToolTip Text="单项操作" />
                                                                    <Menu>
                                                                        <Items>
                                                                            <ext:MenuCommand CommandName="CmdBJGW" Icon="Wrench" Text="单项处理新技术">
                                                                            </ext:MenuCommand>
                                                                        </Items>
                                                                    </Menu>
                                                                </ext:SplitCommand>
                                                            </Commands>
                                                            <PrepareToolbar Fn="prepare" />
                                                        </ext:CommandColumn>
                                                    </Columns>
                                                </ColumnModel>
                                                <Listeners>
                                                    <Command Handler=" gridCommand(command,record);" />
                                                </Listeners>
                                                <SelectionModel>
                                                    <ext:CheckboxSelectionModel ID="CheckboxSelectionModel1" runat="server">
                                                        <Listeners>
                                                            <RowSelect Handler="#{btn_Delete}.enable();#{btn_EchoHandle}.enable();RowIndex = rowIndex" />
                                                            <RowDeselect Handler="if (!#{GridPanel_Show}.hasSelection()) {#{btn_Delete}.disable();#{btn_EchoHandle}.disable();}" />
                                                        </Listeners>
                                                    </ext:CheckboxSelectionModel>
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
    <ext:Window ID="arcEditWindow" runat="server" Icon="Group" Title="新业务新技术" Width="600"
        Height="380" AutoShow="false" Modal="true" CenterOnLoad="true" ShowOnLoad="false"
        Resizable="false" StyleSpec="background-color:Transparent;" BodyStyle="background-color:Transparent;">
        <Body>
            <ext:ColumnLayout ID="ColumnLayout2" runat="server">
                <ext:LayoutColumn ColumnWidth=".5">
                    <ext:Panel ID="Panel2" runat="server" Border="false" Header="false" BodyStyle="background-color:Transparent;margin:10px;">
                        <Body>
                            <ext:FormLayout ID="FormLayout1" runat="server" LabelAlign="Left">
                                <ext:Anchor Horizontal="92%">
                                    <ext:TextField ID="txtName" runat="server" FieldLabel="新技术新业务名称" />
                                </ext:Anchor>
                                <ext:Anchor Horizontal="92%">
                                    <ext:ComboBox ID="cboPrincipal" runat="server" StoreID="Store2" DisplayField="STAFF_NAME"
                                        Width="120" ValueField="STAFF_ID" TypeAhead="false" LoadingText="Searching..."
                                        PageSize="1000" ItemSelector="div.search-item" MinChars="1" FieldLabel="负责人"
                                        ListWidth="240">
                                        <Template ID="Template2" runat="server">
                                       <tpl for=".">
                                          <div class="search-item">
                                             <h3><span style="width:auto">{DEPT_NAME}</span>{STAFF_NAME}</h3>
                                          </div>
                                       </tpl>
                                        </Template>
                                    </ext:ComboBox>
                                </ext:Anchor>
                                <ext:Anchor Horizontal="92%">
                                    <ext:TextField ID="txtClubDept" runat="server" FieldLabel="主要协作科室" />
                                </ext:Anchor>
                                <ext:Anchor Horizontal="92%">
                                    <ext:ComboBox ID="cboNewTechnic" runat="server" FieldLabel="新技术新业务" />
                                </ext:Anchor>
                                <ext:Anchor Horizontal="92%">
                                    <ext:ComboBox ID="cboPrincipalSchoolAge" runat="server" FieldLabel="负责人学历" />
                                </ext:Anchor>
                                <ext:Anchor Horizontal="92%">
                                    <ext:ComboBox ID="cboPrincipalJob" runat="server" FieldLabel="负责人职称" />
                                </ext:Anchor>
                                <ext:Anchor Horizontal="92%">
                                    <ext:TextArea ID="txrJoinPersons" runat="server" FieldLabel="参加人员" Height="130" />
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
                                    <ext:DateField ID="dtfDates" runat="server" FieldLabel="开展时间" Format="yyyy-MM-dd" />
                                </ext:Anchor>
                                <ext:Anchor Horizontal="92%">
                                    <ext:NumberField ID="txtCompCase" runat="server" FieldLabel="完成例数" CausesValidation="true" AllowBlank="false"/>
                                </ext:Anchor>
                                <ext:Anchor Horizontal="92%">
                                    <ext:ComboBox ID="cboLevelCol" runat="server" FieldLabel="水平" />
                                </ext:Anchor>
                                <ext:Anchor Horizontal="92%">
                                    <ext:ComboBox ID="cboEffect" runat="server" FieldLabel="效果" />
                                </ext:Anchor>
                                <ext:Anchor Horizontal="92%">
                                    <ext:TextArea ID="txtBrief" runat="server" FieldLabel="进展情况" Height="50" />
                                </ext:Anchor>
                                <ext:Anchor Horizontal="92%">
                                    <ext:TextArea ID="txrSetSug" runat="server" FieldLabel="提交意见" Height="60" />
                                </ext:Anchor>
                                <ext:Anchor Horizontal="92%">
                                    <ext:TextArea ID="txrSug" runat="server" FieldLabel="审批意见" Height="70" />
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
                    <ext:ToolbarButton ID="btnSetUp" runat="server" Icon="Disk" Text="提交">
                        <AjaxEvents>
                            <Click OnEvent="SetUpInfo" Before="if (CheckForm()== false){ Ext.Msg.alert('系统提示','请根据红线提示填写正确的信息！');return false;};">
                                <ExtraParams>
                                    <ext:Parameter Name="deptCode" Value="#{Store1}.getAt(RowIndex).data.DEPT_CODE" Mode="Raw">
                                    </ext:Parameter>
                                    <ext:Parameter Name="deptName" Value="#{Store1}.getAt(RowIndex).data.DEPT_NAME" Mode="Raw">
                                    </ext:Parameter>
                                </ExtraParams>
                            </Click>
                        </AjaxEvents>
                    </ext:ToolbarButton>
                    <ext:ToolbarButton ID="btnApprove" runat="server" Icon="Disk" Text="审批">
                        <AjaxEvents>
                            <Click OnEvent="ApproveInfo" Before="if (CheckForm()== false){ Ext.Msg.alert('系统提示','请根据红线提示填写正确的信息！');return false;};">
                                <ExtraParams>
                                    <ext:Parameter Name="deptCode" Value="#{Store1}.getAt(RowIndex).data.DEPT_CODE" Mode="Raw">
                                    </ext:Parameter>
                                    <ext:Parameter Name="deptName" Value="#{Store1}.getAt(RowIndex).data.DEPT_NAME" Mode="Raw">
                                    </ext:Parameter>
                                </ExtraParams>
                            </Click>
                        </AjaxEvents>
                    </ext:ToolbarButton>
                    <ext:ToolbarButton ID="btnNotApprove" runat="server" Icon="Disk" Text="审批不通过">
                        <AjaxEvents>
                            <Click OnEvent="NotApproveInfo" Before="if (CheckForm()== false){ Ext.Msg.alert('系统提示','请根据红线提示填写正确的信息！');return false;};">
                                <ExtraParams>
                                    <ext:Parameter Name="deptCode" Value="#{Store1}.getAt(RowIndex).data.DEPT_CODE" Mode="Raw">
                                    </ext:Parameter>
                                    <ext:Parameter Name="deptName" Value="#{Store1}.getAt(RowIndex).data.DEPT_NAME" Mode="Raw">
                                    </ext:Parameter>
                                </ExtraParams>
                            </Click>
                        </AjaxEvents>
                    </ext:ToolbarButton>
                    <ext:ToolbarButton ID="btnSetSave" runat="server" Icon="Disk" Text="修改">
                        <AjaxEvents>
                            <Click OnEvent="SetSave" Before="if (CheckForm()== false){ Ext.Msg.alert('系统提示','请根据红线提示填写正确的信息！');return false;};">
                                <ExtraParams>
                                    <ext:Parameter Name="deptCode" Value="#{Store1}.getAt(RowIndex).data.DEPT_CODE" Mode="Raw">
                                    </ext:Parameter>
                                    <ext:Parameter Name="deptName" Value="#{Store1}.getAt(RowIndex).data.DEPT_NAME" Mode="Raw">
                                    </ext:Parameter>
                                </ExtraParams>
                            </Click>
                        </AjaxEvents>
                    </ext:ToolbarButton>
                    <%--添加界面直接提交按钮 --%>
                    <ext:ToolbarButton ID="btnSaveSet" runat="server" Icon="Disk" Text="提交">
                        <AjaxEvents>
                            <Click OnEvent="SaveSet" Before="if (CheckForm()== false){ Ext.Msg.alert('系统提示','请根据红线提示填写正确的信息！');return false;};">
                            </Click>
                        </AjaxEvents>
                    </ext:ToolbarButton>
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
