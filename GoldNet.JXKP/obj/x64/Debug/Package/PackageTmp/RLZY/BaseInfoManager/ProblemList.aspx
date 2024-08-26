<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ProblemList.aspx.cs" Inherits="GoldNet.JXKP.RLZY.BaseInfoManager.ProblemList" %>

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
                txtProblemCode.setValue("");         
                cboProblemSort.selectByIndex(0);     
                dtfStartDate.setValue("");           
                cboOutlayType.selectByIndex(0);          
                txtPrincipal.setValue("");          
                txtPrincipalSpeciality.setValue(""); 
                cboPassedUnit.selectByIndex(0);          
                txtUnit.setValue("");                
                txrConent.setValue("");               
                txtProblemName.setValue("");         
                txtYears.setValue("");               
                dtfEndDate.setValue("");
                txtOutlayNum.setValue("");
                cboPrincipalSchoolAge.selectByIndex(0);  
                cboPrincipalJob.selectByIndex(0);        
                cboLerver.selectByIndex(0);
                Btn_BatStart.setText("保存");

                txrSug.setValue("");
                txrSetSug.setValue("");           

                txtProblemCode.setDisabled(false);           
                cboProblemSort.setDisabled(false);       
                dtfStartDate.setDisabled(false);             
                cboOutlayType.setDisabled(false);            
                txtPrincipal.setDisabled(false);            
                txtPrincipalSpeciality.setDisabled(false);   
                cboPassedUnit.setDisabled(false);            
                txtUnit.setDisabled(false);                  
                txrConent.setDisabled(false);                 
                txtProblemName.setDisabled(false);           
                txtYears.setDisabled(false);                 
                dtfEndDate.setDisabled(false);               
                txtOutlayNum.setDisabled(false);             
                cboPrincipalSchoolAge.setDisabled(false);    
                cboPrincipalJob.setDisabled(false);
                cboLerver.setDisabled(false);

                txrSug.setVisible(true);
                txrSug.setDisabled(true);
                txrSetSug.setVisible(true);
                txrSetSug.setDisabled(true);

                Btn_BatStart.setVisible(true); //保存可见
                btnSetSave.setVisible(false); //修改不可见
                btnSaveSet.setVisible(false); //提交不可见
                btnNotApprove.setVisible(false); //审批不通过不可见

                arcEditWindow.show();
            } else if (optype == "2") {
                //初始化
                HiddenId.setValue(record.data.ID);
                txtProblemCode.setValue(record.data.PROBLEM_CODE);        
                cboProblemSort.setValue(record.data.PROBLEM_SORT);        
                dtfStartDate.setValue(record.data.START_DATE);          
                cboOutlayType.setValue(record.data.OUTLAY_TYPE);         
                txtPrincipal.setValue(record.data.PRINCIPAL);          
                txtPrincipalSpeciality.setValue(record.data.PRINCIPAL_SPECIALITY);
                cboPassedUnit.setValue(record.data.PASSED_UNIT);         
                txtUnit.setValue(record.data.UNIT);               
                txrConent.setValue(record.data.CONTENT);             
                txtProblemName.setValue(record.data.PROBLEM_NAME);        
                txtYears.setValue(record.data.YEARS);              
                dtfEndDate.setValue(record.data.END_DATE);            
                txtOutlayNum.setValue(record.data.OUTLAY_NUM);          
                cboPrincipalSchoolAge.setValue(record.data.PRINCIPAL_SCHOOL_AGE); 
                cboPrincipalJob.setValue(record.data.PRINCIPAL_JOB);
                cboLerver.setValue(record.data.LERVER);

                txrSug.setValue(record.data.MARK_SUG);
                txrSetSug.setValue(record.data.SETUP_SUG);

                var Name = PowerInfoHidden.value == 1 ? "审批" : "提交";
                Btn_BatStart.setText(Name);

                //院级审批权限  liu.shh 2012.12.19
                if (PowerInfoHidden.value == 1) {
                    txrSug.setVisible(true);
                    txrSug.setDisabled(false);
                    txrSetSug.setVisible(true);
                    txrSetSug.setDisabled(false);

                    txtProblemCode.setDisabled(false);
                    cboProblemSort.setDisabled(false);
                    dtfStartDate.setDisabled(false);
                    cboOutlayType.setDisabled(false);
                    txtPrincipal.setDisabled(false);
                    txtPrincipalSpeciality.setDisabled(false);
                    cboPassedUnit.setDisabled(false);
                    txtUnit.setDisabled(false);
                    txrConent.setDisabled(false);
                    txtProblemName.setDisabled(false);
                    txtYears.setDisabled(false);
                    dtfEndDate.setDisabled(false);
                    txtOutlayNum.setDisabled(false);
                    cboPrincipalSchoolAge.setDisabled(false);
                    cboPrincipalJob.setDisabled(false);
                    cboLerver.setDisabled(false);

                    if (Name == "审批") {
                        Btn_BatStart.setVisible(true); //审批可见
                    }
                    else {
                        Btn_BatStart.setVisible(false); //其它不可见
                    }
                    btnSetSave.setVisible(false); //修改不可见
                    btnSaveSet.setVisible(false); //提交不可见
                    btnNotApprove.setVisible(true); //审批不通过可见
                }

                //科主任权限  liu.shh  2012.12.19
                if (PowerInfoHidden.value == 2 && hiddenEdit.value == '1') {
                    txrSug.setVisible(true);
                    txrSug.setDisabled(true);
                    txrSetSug.setVisible(true);
                    txrSetSug.setDisabled(false);

                    if (!cbxOpration.checked) {
                        if (record.data.ADD_MARK == "审批通过") {  //审批已通过的不可再修改
                            txtProblemCode.setDisabled(true);
                            cboProblemSort.setDisabled(true);
                            dtfStartDate.setDisabled(true);
                            cboOutlayType.setDisabled(true);
                            txtPrincipal.setDisabled(true);
                            txtPrincipalSpeciality.setDisabled(true);
                            cboPassedUnit.setDisabled(true);
                            txtUnit.setDisabled(true);
                            txrConent.setDisabled(true);
                            txtProblemName.setDisabled(true);
                            txtYears.setDisabled(true);
                            dtfEndDate.setDisabled(true);
                            txtOutlayNum.setDisabled(true);
                            cboPrincipalSchoolAge.setDisabled(true);
                            cboPrincipalJob.setDisabled(true);
                            cboLerver.setDisabled(true);

                            txrSug.setDisabled(true);
                            txrSetSug.setDisabled(true);

                            if (Name == "提交") {
                                Btn_BatStart.setVisible(false);
                            }
                            btnSetSave.setVisible(false);
                            btnSaveSet.setVisible(false);
                            btnNotApprove.setVisible(false);
                        }
                        else {
                            txtProblemCode.setDisabled(false);
                            cboProblemSort.setDisabled(false);
                            dtfStartDate.setDisabled(false);
                            cboOutlayType.setDisabled(false);
                            txtPrincipal.setDisabled(false);
                            txtPrincipalSpeciality.setDisabled(false);
                            cboPassedUnit.setDisabled(false);
                            txtUnit.setDisabled(false);
                            txrConent.setDisabled(false);
                            txtProblemName.setDisabled(false);
                            txtYears.setDisabled(false);
                            dtfEndDate.setDisabled(false);
                            txtOutlayNum.setDisabled(false);
                            cboPrincipalSchoolAge.setDisabled(false);
                            cboPrincipalJob.setDisabled(false);
                            cboLerver.setDisabled(false);

                            if (Name == "提交") {
                                Btn_BatStart.setVisible(true);
                            }
                            btnSetSave.setVisible(true);
                            btnSaveSet.setVisible(false);
                            btnNotApprove.setVisible(false);
                        }
                    }
                    if (cbxOpration.checked) {
                        txtProblemCode.setDisabled(false);
                        cboProblemSort.setDisabled(false);
                        dtfStartDate.setDisabled(false);
                        cboOutlayType.setDisabled(false);
                        txtPrincipal.setDisabled(false);
                        txtPrincipalSpeciality.setDisabled(false);
                        cboPassedUnit.setDisabled(false);
                        txtUnit.setDisabled(false);
                        txrConent.setDisabled(false);
                        txtProblemName.setDisabled(false);
                        txtYears.setDisabled(false);
                        dtfEndDate.setDisabled(false);
                        txtOutlayNum.setDisabled(false);
                        cboPrincipalSchoolAge.setDisabled(false);
                        cboPrincipalJob.setDisabled(false);
                        cboLerver.setDisabled(false);

                        if (Name == "提交") {
                            Btn_BatStart.setVisible(true);
                        }
                        btnSetSave.setVisible(true);
                        btnSaveSet.setVisible(false);
                        btnNotApprove.setVisible(false);
                    }
                }

                //普通医生权限 liu.shh  2012.12.19              
                if (PowerInfoHidden.value == 0) {
                    txrSug.setVisible(true);
                    txrSug.setDisabled(true);
                    txrSetSug.setVisible(true);
                    txrSetSug.setDisabled(true);

                    if(cbxOpration.checked){
                        txtProblemCode.setDisabled(false);           
                        cboProblemSort.setDisabled(false);       
                        dtfStartDate.setDisabled(false);             
                        cboOutlayType.setDisabled(false);            
                        txtPrincipal.setDisabled(false);            
                        txtPrincipalSpeciality.setDisabled(false);   
                        cboPassedUnit.setDisabled(false);            
                        txtUnit.setDisabled(false);                  
                        txrConent.setDisabled(false);                 
                        txtProblemName.setDisabled(false);           
                        txtYears.setDisabled(false);                 
                        dtfEndDate.setDisabled(false);               
                        txtOutlayNum.setDisabled(false);             
                        cboPrincipalSchoolAge.setDisabled(false);    
                        cboPrincipalJob.setDisabled(false);          
                        cboLerver.setDisabled(false);

                        btnSetSave.setVisible(true);
                        btnSaveSet.setVisible(false);
                        btnNotApprove.setVisible(false);
                    }
                    if (!cbxOpration.checked) {
                        if (record.data.ADD_MARK == "审批通过") {  //审批已通过的不可再修改
                            txtProblemCode.setDisabled(true);
                            cboProblemSort.setDisabled(true);
                            dtfStartDate.setDisabled(true);
                            cboOutlayType.setDisabled(true);
                            txtPrincipal.setDisabled(true);
                            txtPrincipalSpeciality.setDisabled(true);
                            cboPassedUnit.setDisabled(true);
                            txtUnit.setDisabled(true);
                            txrConent.setDisabled(true);
                            txtProblemName.setDisabled(true);
                            txtYears.setDisabled(true);
                            dtfEndDate.setDisabled(true);
                            txtOutlayNum.setDisabled(true);
                            cboPrincipalSchoolAge.setDisabled(true);
                            cboPrincipalJob.setDisabled(true);
                            cboLerver.setDisabled(true);

                            btnSetSave.setVisible(false);
                            btnSaveSet.setVisible(false);
                            btnNotApprove.setVisible(false);
                        }
                        else { //审批不通过的可以再修改
                            txtProblemCode.setDisabled(false);
                            cboProblemSort.setDisabled(false);
                            dtfStartDate.setDisabled(false);
                            cboOutlayType.setDisabled(false);
                            txtPrincipal.setDisabled(false);
                            txtPrincipalSpeciality.setDisabled(false);
                            cboPassedUnit.setDisabled(false);
                            txtUnit.setDisabled(false);
                            txrConent.setDisabled(false);
                            txtProblemName.setDisabled(false);
                            txtYears.setDisabled(false);
                            dtfEndDate.setDisabled(false);
                            txtOutlayNum.setDisabled(false);
                            cboPrincipalSchoolAge.setDisabled(false);
                            cboPrincipalJob.setDisabled(false);
                            cboLerver.setDisabled(false);

                            btnSetSave.setVisible(true);
                            btnSaveSet.setVisible(false);
                            btnNotApprove.setVisible(false);
                        }
                    }
                    if(Name == "提交"){
                        Btn_BatStart.setVisible(false);
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
            GridPanelToDataBase(id,"","","","","","",
                                        "","","","","","","","",
                                        "","","",optype,"","","");
        }
        
        
        /*
            节点增删改操作回调函数
        */
        function OpCallback (btn) {
            if (Btn_BatStart.text == "保存") {
                optype = "1";
            }
            if(Btn_BatStart.text == "提交") {
                optype = "2";
            }
            if(Btn_BatStart.text == "审批") {
                optype = "5";
            }
            if((btn != "ok") && (btn != "yes")){
                return;
            }
            var myDate = new Date();                
            var deptCode = DeptCodeCombo.getSelectedItem().value;
            var ProblemCode = txtProblemCode.getValue();         
            var ProblemSort = cboProblemSort.getSelectedItem().text;
            var StartDateFormat = dtfStartDate.getValue() == ''?myDate:dtfStartDate.getValue();
            var StartDate = StartDateFormat.format('Y-m-d');         
            var OutlayType = cboOutlayType.getSelectedItem().text;         
            var Principal = txtPrincipal.getValue();          
            var PrincipalSpeciality = txtPrincipalSpeciality.getValue(); 
            var PassedUnit = cboPassedUnit.getSelectedItem().value;        
            var Unit = txtUnit.getValue();                
            var Conent = txrConent.getValue();               
            var ProblemName = txtProblemName.getValue();         
            var Years = txtYears.getValue();
            var EndDateFormat = dtfEndDate.getValue() == ''?myDate:dtfEndDate.getValue();
            var EndDate = EndDateFormat.format('Y-m-d'); 
            var OutlayNum = txtOutlayNum.getValue();           
            var PrincipalSchoolAge = cboPrincipalSchoolAge.getSelectedItem().text; 
            var PrincipalJob = cboPrincipalJob.getSelectedItem().text;
            var Lerver = cboLerver.getSelectedItem().value;
            var setup_sug = txrSetSug.getValue();
            var mark_sug = txrSug.getValue();        
            if (optype == "1") {
               GridPanelToDataBase("",deptCode,ProblemCode,ProblemSort,StartDate,OutlayType,Principal,
                                        PrincipalSpeciality,PassedUnit,Unit,Conent,ProblemName,Years,EndDate,OutlayNum,
                                        PrincipalSchoolAge, PrincipalJob, Lerver, optype, '', mark_sug, setup_sug);
            } else if (optype == "2") {
               GridPanelToDataBase(HiddenId.value,Store1.getAt(RowIndex).data.DEPT_CODE,ProblemCode,ProblemSort,StartDate,OutlayType,Principal,
                                        PrincipalSpeciality,PassedUnit,Unit,Conent,ProblemName,Years,EndDate,OutlayNum,
                                        PrincipalSchoolAge, PrincipalJob, Lerver, optype, Store1.getAt(RowIndex).data.DEPT_NAME, mark_sug, setup_sug);
            } else if(optype == "5") {
                GridPanelToDataBase(HiddenId.value,Store1.getAt(RowIndex).data.DEPT_CODE,ProblemCode,ProblemSort,StartDate,OutlayType,Principal,
                                        PrincipalSpeciality,PassedUnit,Unit,Conent,ProblemName,Years,EndDate,OutlayNum,
                                        PrincipalSchoolAge, PrincipalJob, Lerver, optype, Store1.getAt(RowIndex).data.DEPT_NAME, mark_sug, setup_sug);
            }
        }
        
        function GridPanelToDataBase(id,deptCode,ProblemCode,ProblemSort,StartDate,OutlayType,Principal,
                                            PrincipalSpeciality,PassedUnit,Unit,Conent,ProblemName,Years,EndDate,OutlayNum,
                                            PrincipalSchoolAge, PrincipalJob, Lerver, optype, deptName, mark_sug, setup_sug) {
          Goldnet.AjaxMethod.request(
                  'ProblemListAjaxOper',
                    {
                        params: {
                           Id:id,deptCode:deptCode,ProblemCode:ProblemCode,ProblemSort:ProblemSort,StartDate:StartDate,OutlayType:OutlayType,Principal:Principal,
                           PrincipalSpeciality: PrincipalSpeciality, PassedUnit: PassedUnit, Unit: Unit, Conent: Conent, ProblemName: ProblemName, Years: Years, EndDate: EndDate, OutlayNum: OutlayNum,
                                    PrincipalSchoolAge:PrincipalSchoolAge,PrincipalJob:PrincipalJob,Lerver:Lerver,optype:optype,deptName:deptName,mark_sug:mark_sug,setup_sug:setup_sug
                        },
                        success: function(result) {
                            Store1.reload();
                            btn_EchoHandle.setDisabled(true);
                            btn_Delete.setDisabled(true);
                            arcEditWindow.hide();
                        },
                        failure: function(msg) {
                            alert(msg)
                            GridPanel_Show.el.unmask();
                        }
                    });
        }
        
        
        function SetSaveMethod(type) {
            var myDate = new Date();                
            //var deptCode = DeptCodeCombo.getSelectedItem().value;
            var ProblemCode = txtProblemCode.getValue();         
            var ProblemSort = cboProblemSort.getSelectedItem().text;
            var StartDateFormat = dtfStartDate.getValue() == ''?myDate:dtfStartDate.getValue();
            var StartDate = StartDateFormat.format('Y-m-d');         
            var OutlayType = cboOutlayType.getSelectedItem().text;         
            var Principal = txtPrincipal.getValue();          
            var PrincipalSpeciality = txtPrincipalSpeciality.getValue(); 
            var PassedUnit = cboPassedUnit.getSelectedItem().value;        
            var Unit = txtUnit.getValue();                
            var Conent = txrConent.getValue();               
            var ProblemName = txtProblemName.getValue();         
            var Years = txtYears.getValue();
            var EndDateFormat = dtfEndDate.getValue() == ''?myDate:dtfEndDate.getValue();
            var EndDate = EndDateFormat.format('Y-m-d'); 
            var OutlayNum = txtOutlayNum.getValue();           
            var PrincipalSchoolAge = cboPrincipalSchoolAge.getSelectedItem().text; 
            var PrincipalJob = cboPrincipalJob.getSelectedItem().text;
            var Lerver = cboLerver.getSelectedItem().value;
            var setup_sug = txrSetSug.getValue();
            var mark_sug = txrSug.getValue();  
            var deptCode = '';
            var deptName = ''; 
            if(type == '8') {
                deptCode = DeptCodeCombo.getSelectedItem().value;
            } else {
                deptCode = Store1.getAt(RowIndex).data.DEPT_CODE;
                deptName = Store1.getAt(RowIndex).data.DEPT_NAME;
            }
            
            
            GridPanelToDataBase(HiddenId.value,deptCode,ProblemCode,ProblemSort,StartDate,OutlayType,Principal,
                                PrincipalSpeciality,PassedUnit,Unit,Conent,ProblemName,Years,EndDate,OutlayNum,
                                PrincipalSchoolAge, PrincipalJob, Lerver, type, deptName, mark_sug, setup_sug);
        }
        
        var prepare = function(grid, toolbar, rowIndex, record) {
            var menuButton = toolbar.items.get(0);
            var menu1 = menuButton.menu.items.get(0);
            menu1.setDisabled(!hiddenMeunUp.getValue());
            var menu2 = menuButton.menu.items.get(1);
            menu2.setDisabled(!hiddenAuthor.getValue());
        }
        
        var gridCommand = function(command, record) {
           if(command == 'CmdEditAuthor') {
              HiddenId.setValue(record.data.ID);
              ViewAuthor.show();
              Store.reload();
           } else if(command == 'CmdAddAuthor') {
              HiddenId.setValue(record.data.ID);
              //cboStaffInfo,cboRanking,txaRemarks
              cboRanking.selectByIndex(0);
              cboStaffInfo.setValue("");
              txaRemarks.setValue("");
              arcEditAuthor.show();
           } else if(command == 'CmdBJGW') {
              TreeOpration(2,record);
           }
        }
        
        var AuthorOpration = function(optype) {
            if(optype == '1') {
                var staffid = cboStaffInfo.getSelectedItem().value;
                var name = cboStaffInfo.getSelectedItem().text;
                var AuthorRanking = cboRanking.getSelectedItem().text;
                var Remarks = txaRemarks.getValue();
                var code = HiddenId.getValue();
                var mark_sug = txrSug.getValue();
                AuthorDataBaseOper(code,staffid,name,AuthorRanking,Remarks,optype);
            } else if(optype == '2') {
                var selections = CheckboxSelectionModel2.getSelections();
                var id = '';
                for(var i =0;i<selections.length;i++) {
                    if(i == selections.length -1) {
                        id = id+selections[i].data.ID;
                    } else {
                        id = id+selections[i].data.ID +',';
                    }
                }
                AuthorDataBaseOper(id,"","","","",optype);
            }
        }
        
        var AuthorDataBaseOper = function(OperCode,staffid,name,AuthorRanking,Remarks,optype,mark_sug) {
            Goldnet.AjaxMethod.request(
                  'AuthorDataBaseAjaxOper',
                    {
                        params: {
                           OperCode:OperCode,staffid:staffid,name:name,AuthorRanking:AuthorRanking,Remarks:Remarks,optype:optype
                        },
                        success: function(result) {
                            if(optype == '2') {
                                Store.reload();
                            } else {
                                Store1.reload();
                            }
                            btn_EchoHandle.setDisabled(true);
                            btn_Delete.setDisabled(true);
                            btnDelAuthor.setDisabled(true);
                            arcEditAuthor.hide();
                        },
                        failure: function(msg) {
                            GridPanel_Show.el.unmask();
                        }
                    });
        }
        
          var CheckForm = function() {
            if (txtYears.validate() == false) {
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
    <ext:Hidden ID="hiddenAuthor" runat="server">
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
                    <ext:RecordField Name="PROBLEM_NAME" />
                    <ext:RecordField Name="PROBLEM_SORT" />
                    <ext:RecordField Name="YEARS" />
                    <ext:RecordField Name="PRINCIPAL" />
                    <ext:RecordField Name="PRINCIPAL_SPECIALITY" />
                    <ext:RecordField Name="PRINCIPAL_SCHOOL_AGE" />
                    <ext:RecordField Name="PRINCIPAL_JOB" />
                    <ext:RecordField Name="UNIT" />
                    <ext:RecordField Name="MOSTLY_PERSONS" />
                    <ext:RecordField Name="CONTENT" />
                    <ext:RecordField Name="PROBLEM_CODE" />
                    <ext:RecordField Name="START_DATE" />
                    <ext:RecordField Name="END_DATE" />
                    <ext:RecordField Name="OUTLAY_TYPE" />
                    <ext:RecordField Name="OUTLAY_NUM" />
                    <ext:RecordField Name="RECORD_DATE" />
                    <ext:RecordField Name="ENTER_PERS" />
                    <ext:RecordField Name="ADD_MARK" />
                    <ext:RecordField Name="PASSED_UNIT" />
                    <ext:RecordField Name="LERVER" />
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
    <ext:Store ID="Store" runat="server" OnRefreshData="RefreshData">
        <Reader>
            <ext:JsonReader>
                <Fields>
                    <ext:RecordField Name="ID" />
                    <ext:RecordField Name="PROBLEM_CODE" />
                    <ext:RecordField Name="STAFF_ID" />
                    <ext:RecordField Name="STAFF_NAME" />
                    <ext:RecordField Name="RANK" />
                    <ext:RecordField Name="REMARK" />
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
                                <ext:Label ID="Label2" runat="server" Text="年度：">
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
                                <ext:ToolbarSeparator ID="ToolbarSeparator1" runat="server" />
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
                                <ext:Label ID="Label1" runat="server" Text="负责人姓名：" />
                                <ext:TextField ID="txtPrName" runat="server" Width="60" />
                                <ext:ToolbarSeparator ID="ToolbarSeparator4" runat="server" />
                                <ext:Button ID="btnSearch" runat="server" Text="查询" Icon="DatabaseGo">
                                    <Listeners>
                                        <Click Handler="#{Store1}.reload();#{btn_Delete}.disable();" />
                                    </Listeners>
                                </ext:Button>
                                <ext:ToolbarSeparator>
                                </ext:ToolbarSeparator>
                                <ext:Button ID="btn_Add" runat="server" Text="添加课题" Icon="Add">
                                    <Listeners>
                                        <Click Handler="if(#{DeptCodeCombo}.getSelectedItem().value == '') {Ext.Msg.show({ title: '信息提示', msg: '请选择科室', icon: 'ext-mb-info', buttons: { ok: true }  });} else {TreeOpration(1,'')}" />
                                    </Listeners>
                                </ext:Button>
                                <ext:Button Text="删除课题" ID="btn_Delete" runat="server" Icon="Delete" Disabled="true">
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
                                                        <ext:Column ColumnID="ID" Header="编号" Sortable="true" DataIndex="ID" />
                                                        <ext:Column ColumnID="YEARS" Header="年度" Sortable="true" DataIndex="YEARS" />
                                                        <ext:Column ColumnID="PROBLEM_NAME" Header="课题名称" Sortable="true" DataIndex="PROBLEM_NAME" />
                                                        <ext:Column ColumnID="PROBLEM_SORT" Header="课题类别" Sortable="true" DataIndex="PROBLEM_SORT" />
                                                        <ext:Column ColumnID="DEPT_NAME" Header="科室" Sortable="true" DataIndex="DEPT_NAME" />
                                                        <ext:Column ColumnID="START_DATE" Header="课题开始时间" Sortable="true" DataIndex="START_DATE" />
                                                        <ext:Column ColumnID="END_DATE" Header="课题终止时间" Sortable="true" DataIndex="END_DATE" />
                                                        <ext:Column ColumnID="PRINCIPAL" Header="负责人" Sortable="true" DataIndex="PRINCIPAL" />
                                                        <ext:Column ColumnID="PRINCIPAL_SCHOOL_AGE" Header="负责人学历" Sortable="true" DataIndex="PRINCIPAL_SCHOOL_AGE" />
                                                        <ext:Column ColumnID="PRINCIPAL_JOB" Header="负责人职称" Sortable="true" DataIndex="PRINCIPAL_JOB" />
                                                        <ext:Column ColumnID="PRINCIPAL_SPECIALITY" Header="负责人专业" Sortable="true" DataIndex="PRINCIPAL_SPECIALITY" />
                                                        <ext:Column ColumnID="ADD_MARK" Header="审批标识" Sortable="true" DataIndex="ADD_MARK" />
                                                        <ext:Column ColumnID="SETUP_SUG" Header="提交意见" Sortable="true" DataIndex="SETUP_SUG" />
                                                        <ext:Column ColumnID="MARK_SUG" Header="审批意见" Sortable="true" DataIndex="MARK_SUG" />
                                                        <ext:CommandColumn Width="38" Header="操作">
                                                            <Commands>
                                                                <ext:SplitCommand Icon="TableMultiple">
                                                                    <ToolTip Text="单项操作" />
                                                                    <Menu>
                                                                        <Items>
                                                                            <ext:MenuCommand CommandName="CmdBJGW" Icon="Wrench" Text="单项处理">
                                                                            </ext:MenuCommand>
                                                                            <ext:MenuCommand CommandName="CmdAddAuthor" Icon="TagBlue" Text="添加作者">
                                                                            </ext:MenuCommand>
                                                                            <ext:MenuCommand CommandName="CmdEditAuthor" Icon="TextListBullets" Text="查看作者">
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
    <ext:Window ID="arcEditAuthor" runat="server" Icon="Group" Title="作者编辑" Width="300"
        Height="230" AutoShow="false" Modal="true" CenterOnLoad="true" ShowOnLoad="false"
        Resizable="false" StyleSpec="background-color:Transparent;" BodyStyle="background-color:Transparent;">
        <Body>
            <ext:ColumnLayout ID="ColumnLayout3" runat="server">
                <ext:LayoutColumn ColumnWidth=".5">
                    <ext:Panel ID="Panel4" runat="server" Border="false" Header="false" BodyStyle="background-color:Transparent;margin:10px;">
                        <Body>
                            <ext:FormLayout ID="FormLayout3" runat="server" LabelAlign="Left">
                                <ext:Anchor Horizontal="92%">
                                    <ext:ComboBox ID="cboStaffInfo" runat="server" StoreID="Store2" DisplayField="STAFF_NAME"
                                        Width="120" ValueField="STAFF_ID" TypeAhead="false" LoadingText="Searching..."
                                        PageSize="1000" ItemSelector="div.search-item" MinChars="1" FieldLabel="负责人"
                                        ListWidth="240">
                                        <Template ID="Template2" runat="server">
                                       <tpl for=".">
                                          <div class="search-item">
                                             <h3><span style="width:auto">{STAFF_ID}</span>{STAFF_NAME}　　　　{DEPT_NAME}</h3>
                                          </div>
                                       </tpl>
                                        </Template>
                                    </ext:ComboBox>
                                </ext:Anchor>
                                <ext:Anchor Horizontal="92%">
                                    <ext:ComboBox ID="cboRanking" runat="server" FieldLabel="排名" />
                                </ext:Anchor>
                                <ext:Anchor Horizontal="92%">
                                    <ext:TextArea ID="txaRemarks" runat="server" FieldLabel="备注" Height="100" />
                                </ext:Anchor>
                            </ext:FormLayout>
                        </Body>
                    </ext:Panel>
                </ext:LayoutColumn>
            </ext:ColumnLayout>
        </Body>
        <BottomBar>
            <ext:Toolbar ID="Toolbar3" runat="server">
                <Items>
                    <ext:ToolbarFill ID="ToolbarFill1" runat="server" />
                    <ext:ToolbarButton ID="ToolbarButton1" runat="server" Icon="Disk" Text="保存">
                        <Listeners>
                            <Click Handler="if(#{cboStaffInfo}.getSelectedItem().value != '') {AuthorOpration(1)} else {Ext.Msg.show({ title: '信息提示', msg: '请选择人员', icon: 'ext-mb-info', buttons: { ok: true }  });}" />
                        </Listeners>
                    </ext:ToolbarButton>
                    <ext:ToolbarSeparator ID="ToolbarSeparator2" runat="server" />
                    <ext:ToolbarButton ID="ToolbarButton2" runat="server" Icon="Cancel" Text="退出">
                        <Listeners>
                            <Click Handler="arcEditAuthor.hide();" />
                        </Listeners>
                    </ext:ToolbarButton>
                </Items>
            </ext:Toolbar>
        </BottomBar>
    </ext:Window>
    <ext:Window ID="ViewAuthor" runat="server" Icon="Group" Title="查看作者" Width="600"
        Height="400" AutoShow="false" Modal="true" CenterOnLoad="true" ShowOnLoad="false"
        Resizable="false">
        <TopBar>
            <ext:Toolbar ID="Toolbar1" runat="server">
                <Items>
                    <ext:Button Text="删除作者" ID="btnDelAuthor" runat="server" Icon="Delete" Disabled="true">
                        <Listeners>
                            <Click Handler="AuthorOpration(2)" />
                        </Listeners>
                    </ext:Button>
                </Items>
            </ext:Toolbar>
        </TopBar>
        <Body>
            <ext:GridPanel ID="GridPanel1" runat="server" StoreID="Store" Border="false" AutoWidth="true"
                Header="false" AutoScroll="true" Height="335">
                <ColumnModel ID="ColumnModel2" runat="server">
                    <Columns>
                        <ext:Column ColumnID="PROBLEM_CODE" Header="课题编号" Sortable="true" DataIndex="PROBLEM_CODE" />
                        <ext:Column ColumnID="STAFF_NAME" Header="负责人" Sortable="true" DataIndex="STAFF_NAME" />
                        <ext:Column ColumnID="RANK" Header="排名" Sortable="true" DataIndex="RANK" />
                        <ext:Column ColumnID="REMARK" Header="备注" Sortable="true" DataIndex="REMARK" />
                    </Columns>
                </ColumnModel>
                <SelectionModel>
                    <ext:CheckboxSelectionModel ID="CheckboxSelectionModel2" runat="server">
                        <Listeners>
                            <RowSelect Handler="#{btnDelAuthor}.enable();" />
                            <RowDeselect Handler="if (!#{GridPanel1}.hasSelection()) {#{btnDelAuthor}.disable();}" />
                        </Listeners>
                    </ext:CheckboxSelectionModel>
                </SelectionModel>
                <LoadMask ShowMask="true" />
            </ext:GridPanel>
        </Body>
    </ext:Window>
    <ext:Window ID="arcEditWindow" runat="server" Icon="Group" Title="课题" Width="600"
        Height="410" AutoShow="false" Modal="true" CenterOnLoad="true" ShowOnLoad="false"
        Resizable="false" StyleSpec="background-color:Transparent;" BodyStyle="background-color:Transparent;">
        <Body>
            <ext:ColumnLayout ID="ColumnLayout2" runat="server">
                <ext:LayoutColumn ColumnWidth=".5">
                    <ext:Panel ID="Panel2" runat="server" Border="false" Header="false" BodyStyle="background-color:Transparent;margin:10px;">
                        <Body>
                            <ext:FormLayout ID="FormLayout1" runat="server" LabelAlign="Left">
                                <ext:Anchor Horizontal="92%">
                                    <ext:TextField ID="txtProblemCode" runat="server" FieldLabel="课题号" />
                                </ext:Anchor>
                                <ext:Anchor Horizontal="92%">
                                    <ext:ComboBox ID="cboProblemSort" runat="server" FieldLabel="课题类别" />
                                </ext:Anchor>
                                <ext:Anchor Horizontal="92%">
                                    <ext:DateField ID="dtfStartDate" runat="server" FieldLabel="开始时间" Format="yyyy-MM-dd" />
                                </ext:Anchor>
                                <ext:Anchor Horizontal="92%">
                                    <ext:ComboBox ID="cboOutlayType" runat="server" FieldLabel="经费投入类别" />
                                </ext:Anchor>
                                <ext:Anchor Horizontal="92%">
                                    <ext:TextField ID="txtPrincipal" runat="server" FieldLabel="负责人" />
                                </ext:Anchor>
                                <ext:Anchor Horizontal="92%">
                                    <ext:TextField ID="txtPrincipalSpeciality" runat="server" FieldLabel="负责人专业" />
                                </ext:Anchor>
                                <ext:Anchor Horizontal="92%">
                                    <ext:ComboBox ID="cboPassedUnit" runat="server" FieldLabel="批准单位" />
                                </ext:Anchor>
                                <ext:Anchor Horizontal="92%">
                                    <ext:TextField ID="txtUnit" runat="server" FieldLabel="协作单位" />
                                </ext:Anchor>
                                <ext:Anchor Horizontal="92%">
                                    <ext:TextArea ID="txrConent" runat="server" FieldLabel="研究主要内容及预期目标" Height="106" />
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
                                    <ext:TextField ID="txtProblemName" runat="server" FieldLabel="课题名称" />
                                </ext:Anchor>
                                <ext:Anchor Horizontal="92%">
                                    <ext:NumberField ID="txtYears" runat="server" FieldLabel="年度" MaxLength="4" CausesValidation="true" AllowBlank="false"/>
                                </ext:Anchor>
                                <ext:Anchor Horizontal="92%">
                                    <ext:DateField ID="dtfEndDate" runat="server" FieldLabel="终止时间" Format="yyyy-MM-dd" />
                                </ext:Anchor>
                                <ext:Anchor Horizontal="92%">
                                    <ext:NumberField ID="txtOutlayNum" runat="server" FieldLabel="经费投入金额" />
                                </ext:Anchor>
                                <ext:Anchor Horizontal="92%">
                                    <ext:ComboBox ID="cboPrincipalSchoolAge" runat="server" FieldLabel="负责人学历" />
                                </ext:Anchor>
                                <ext:Anchor Horizontal="92%">
                                    <ext:ComboBox ID="cboPrincipalJob" runat="server" FieldLabel="负责人职称" />
                                </ext:Anchor>
                                <ext:Anchor Horizontal="92%">
                                    <ext:ComboBox ID="cboLerver" runat="server" FieldLabel="等级" />
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
                        <Listeners>
                            <Click Handler="if (CheckForm()== false){ Ext.Msg.alert('系统提示','请根据红线提示填写正确的信息！')} else {OpCallback('ok');}" />
                        </Listeners>
                    </ext:ToolbarButton>
                    <%--加上审批不通过按钮 --%>
                    <ext:ToolbarButton ID="btnNotApprove" runat="server" Icon="Disk" Text="审批不通过">
                        <Listeners>
                            <Click Handler="SetSaveMethod('-1');" />
                        </Listeners>
                    </ext:ToolbarButton>
                    <%--添加界面直接提交按钮 --%>
                    <ext:ToolbarButton ID="btnSaveSet" runat="server" Icon="Disk" Text="提交">
                        <Listeners>
                            <Click Handler="if (CheckForm()== false){ Ext.Msg.alert('系统提示','请根据红线提示填写正确的信息！')} else {SetSaveMethod('8');}" />
                        </Listeners>
                    </ext:ToolbarButton>
                    <ext:ToolbarButton ID="btnSetSave" runat="server" Icon="Disk" Text="修改">
                        <Listeners>
                            <Click Handler="if (CheckForm()== false){ Ext.Msg.alert('系统提示','请根据红线提示填写正确的信息！')} else {SetSaveMethod('7');}" />
                        </Listeners>
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
