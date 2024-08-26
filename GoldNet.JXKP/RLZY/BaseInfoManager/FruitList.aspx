<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FruitList.aspx.cs" Inherits="GoldNet.JXKP.RLZY.BaseInfoManager.FruitList" %>

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
                txtFruitCode.setValue("");
                txtFruitName.setValue("");
                txtMostlyUnit.setValue("");
                txtMostlyPersons.setValue("");
                dtfBearThePalmDate.setValue("");
                cboAuthUnit.selectByIndex(0); 
                cboExtendAppBound.selectByIndex(0); 
                cboFruitKind.selectByIndex(0); 
                txtThematic.setValue("");
                txtSummary.setValue("");
                cboMostlyPersonsSchoolAge.selectByIndex(0); 
                cboMostlyPersonsJob.selectByIndex(0); 
                cboBearThePalmGrade.selectByIndex(0); 
                cboIsextendApp.selectByIndex(0); 
                txtExtendIncome.setValue("");
                cboTaskSource.selectByIndex(0); 
                txtNewReadNumber.setValue("");
                txtPatentIncome.setValue("");
                txtPatent.setValue("");

                txrSug.setValue("");
                txrSetSug.setValue("");

                cboPatentSort.selectByIndex(0);                
                
                txtFruitCode.setDisabled(false);  
                txtFruitName.setDisabled(false);  
                txtMostlyUnit.setDisabled(false);  
                txtMostlyPersons.setDisabled(false);  
                dtfBearThePalmDate.setDisabled(false);  
                cboAuthUnit.setDisabled(false);   
                cboExtendAppBound.setDisabled(false);   
                cboFruitKind.setDisabled(false);   
                txtThematic.setDisabled(false);  
                txtSummary.setDisabled(false);  
                cboMostlyPersonsSchoolAge.setDisabled(false);   
                cboMostlyPersonsJob.setDisabled(false);   
                cboBearThePalmGrade.setDisabled(false);   
                cboIsextendApp.setDisabled(false);   
                txtExtendIncome.setDisabled(false);  
                cboTaskSource.setDisabled(false);   
                txtNewReadNumber.setDisabled(false);  
                txtPatentIncome.setDisabled(false);  
                txtPatent.setDisabled(false);
                cboPatentSort.setDisabled(false);

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
                txtFruitCode.setValue(record.data.FRUIT_CODE);
                txtFruitName.setValue(record.data.FRUIT_NAME);
                txtMostlyUnit.setValue(record.data.MOSTLY_UNIT);
                txtMostlyPersons.setValue(record.data.MOSTLY_PERSONS);
                dtfBearThePalmDate.setValue(record.data.BEAR_THE_PALM_DATE);
                cboAuthUnit.setValue(record.data.AUTH_UNIT);
                cboExtendAppBound.setValue(record.data.EXTEND_APP_BOUND);
                cboFruitKind.setValue(record.data.FRUIT_KIND);
                txtThematic.setValue(record.data.THEMATIC);
                txtSummary.setValue(record.data.SUMMARY);
                cboMostlyPersonsSchoolAge.setValue(record.data.MOSTLY_PERSONS_SCHOOL_AGE);
                cboMostlyPersonsJob.setValue(record.data.MOSTLY_PERSONS_JOB);
                cboBearThePalmGrade.setValue(record.data.BEAR_THE_PALM_GRADE);
                cboIsextendApp.setValue(record.data.ISEXTEND_APP);
                txtExtendIncome.setValue(record.data.EXTEND_INCOME);
                cboTaskSource.setValue(record.data.TASK_SOURCE);
                txtNewReadNumber.setValue(record.data.NEW_READ_NUMBER);
                txtPatentIncome.setValue(record.data.PATENT_INCOME);
                txtPatent.setValue(record.data.PATENT);
                cboPatentSort.setValue(record.data.PATENT_SORT);

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

                    txtFruitCode.setDisabled(false);
                    txtFruitName.setDisabled(false);
                    txtMostlyUnit.setDisabled(false);
                    txtMostlyPersons.setDisabled(false);
                    dtfBearThePalmDate.setDisabled(false);
                    cboAuthUnit.setDisabled(false);
                    cboExtendAppBound.setDisabled(false);
                    cboFruitKind.setDisabled(false);
                    txtThematic.setDisabled(false);
                    txtSummary.setDisabled(false);
                    cboMostlyPersonsSchoolAge.setDisabled(false);
                    cboMostlyPersonsJob.setDisabled(false);
                    cboBearThePalmGrade.setDisabled(false);
                    cboIsextendApp.setDisabled(false);
                    txtExtendIncome.setDisabled(false);
                    cboTaskSource.setDisabled(false);
                    txtNewReadNumber.setDisabled(false);
                    txtPatentIncome.setDisabled(false);
                    txtPatent.setDisabled(false);
                    cboPatentSort.setDisabled(false);

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
                            txtFruitCode.setDisabled(true);
                            txtFruitName.setDisabled(true);
                            txtMostlyUnit.setDisabled(true);
                            txtMostlyPersons.setDisabled(true);
                            dtfBearThePalmDate.setDisabled(true);
                            cboAuthUnit.setDisabled(true);
                            cboExtendAppBound.setDisabled(true);
                            cboFruitKind.setDisabled(true);
                            txtThematic.setDisabled(true);
                            txtSummary.setDisabled(true);
                            cboMostlyPersonsSchoolAge.setDisabled(true);
                            cboMostlyPersonsJob.setDisabled(true);
                            cboBearThePalmGrade.setDisabled(true);
                            cboIsextendApp.setDisabled(true);
                            txtExtendIncome.setDisabled(true);
                            cboTaskSource.setDisabled(true);
                            txtNewReadNumber.setDisabled(true);
                            txtPatentIncome.setDisabled(true);
                            txtPatent.setDisabled(true);
                            cboPatentSort.setDisabled(true);

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
                            txtFruitCode.setDisabled(false);
                            txtFruitName.setDisabled(false);
                            txtMostlyUnit.setDisabled(false);
                            txtMostlyPersons.setDisabled(false);
                            dtfBearThePalmDate.setDisabled(false);
                            cboAuthUnit.setDisabled(false);
                            cboExtendAppBound.setDisabled(false);
                            cboFruitKind.setDisabled(false);
                            txtThematic.setDisabled(false);
                            txtSummary.setDisabled(false);
                            cboMostlyPersonsSchoolAge.setDisabled(false);
                            cboMostlyPersonsJob.setDisabled(false);
                            cboBearThePalmGrade.setDisabled(false);
                            cboIsextendApp.setDisabled(false);
                            txtExtendIncome.setDisabled(false);
                            cboTaskSource.setDisabled(false);
                            txtNewReadNumber.setDisabled(false);
                            txtPatentIncome.setDisabled(false);
                            txtPatent.setDisabled(false);
                            cboPatentSort.setDisabled(false);

                            Btn_BatStart.setVisible(false); //保存按钮不可见
                            btnSetUp.setVisible(true);   //纯提交，可见
                            btnApprove.setVisible(false); //审批按钮不可见
                            btnNotApprove.setVisible(false); //审批不通过按钮不可见
                            btnSetSave.setVisible(true); //修改按钮可见
                            btnSaveSet.setVisible(false); //保存时提交，不可见
                        }
                    }
                    if (cbxOpration.checked) {
                        txtFruitCode.setDisabled(false);
                        txtFruitName.setDisabled(false);
                        txtMostlyUnit.setDisabled(false);
                        txtMostlyPersons.setDisabled(false);
                        dtfBearThePalmDate.setDisabled(false);
                        cboAuthUnit.setDisabled(false);
                        cboExtendAppBound.setDisabled(false);
                        cboFruitKind.setDisabled(false);
                        txtThematic.setDisabled(false);
                        txtSummary.setDisabled(false);
                        cboMostlyPersonsSchoolAge.setDisabled(false);
                        cboMostlyPersonsJob.setDisabled(false);
                        cboBearThePalmGrade.setDisabled(false);
                        cboIsextendApp.setDisabled(false);
                        txtExtendIncome.setDisabled(false);
                        cboTaskSource.setDisabled(false);
                        txtNewReadNumber.setDisabled(false);
                        txtPatentIncome.setDisabled(false);
                        txtPatent.setDisabled(false);
                        cboPatentSort.setDisabled(false);

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
                        txtFruitCode.setDisabled(false);
                        txtFruitName.setDisabled(false);
                        txtMostlyUnit.setDisabled(false);
                        txtMostlyPersons.setDisabled(false);
                        dtfBearThePalmDate.setDisabled(false);
                        cboAuthUnit.setDisabled(false);
                        cboExtendAppBound.setDisabled(false);
                        cboFruitKind.setDisabled(false);
                        txtThematic.setDisabled(false);
                        txtSummary.setDisabled(false);
                        cboMostlyPersonsSchoolAge.setDisabled(false);
                        cboMostlyPersonsJob.setDisabled(false);
                        cboBearThePalmGrade.setDisabled(false);
                        cboIsextendApp.setDisabled(false);
                        txtExtendIncome.setDisabled(false);
                        cboTaskSource.setDisabled(false);
                        txtNewReadNumber.setDisabled(false);
                        txtPatentIncome.setDisabled(false);
                        txtPatent.setDisabled(false);
                        cboPatentSort.setDisabled(false);

                        Btn_BatStart.setVisible(false); //保存按钮不可见
                        btnSetUp.setVisible(false);   //纯提交，不可见
                        btnApprove.setVisible(false); //审批按钮不可见
                        btnNotApprove.setVisible(false); //审批不通过按钮不可见
                        btnSetSave.setVisible(true); //修改按钮可见
                        btnSaveSet.setVisible(false); //保存时提交，不可见
                    }
                    if (!cbxOpration.checked) {
                        if (record.data.ADD_MARK == "审批通过") {  //审批已通过的不可再修改
                            txtFruitCode.setDisabled(true);
                            txtFruitName.setDisabled(true);
                            txtMostlyUnit.setDisabled(true);
                            txtMostlyPersons.setDisabled(true);
                            dtfBearThePalmDate.setDisabled(true);
                            cboAuthUnit.setDisabled(true);
                            cboExtendAppBound.setDisabled(true);
                            cboFruitKind.setDisabled(true);
                            txtThematic.setDisabled(true);
                            txtSummary.setDisabled(true);
                            cboMostlyPersonsSchoolAge.setDisabled(true);
                            cboMostlyPersonsJob.setDisabled(true);
                            cboBearThePalmGrade.setDisabled(true);
                            cboIsextendApp.setDisabled(true);
                            txtExtendIncome.setDisabled(true);
                            cboTaskSource.setDisabled(true);
                            txtNewReadNumber.setDisabled(true);
                            txtPatentIncome.setDisabled(true);
                            txtPatent.setDisabled(true);
                            cboPatentSort.setDisabled(true);

                            Btn_BatStart.setVisible(false); //保存按钮不可见
                            btnSetUp.setVisible(false);   //纯提交，不可见
                            btnApprove.setVisible(false); //审批按钮不可见
                            btnNotApprove.setVisible(false); //审批不通过按钮不可见
                            btnSetSave.setVisible(false); //修改按钮不可见
                            btnSaveSet.setVisible(false); //保存时提交，不可见
                        }
                        else { //审批不通过的可以再修改
                            txtFruitCode.setDisabled(false);
                            txtFruitName.setDisabled(false);
                            txtMostlyUnit.setDisabled(false);
                            txtMostlyPersons.setDisabled(false);
                            dtfBearThePalmDate.setDisabled(false);
                            cboAuthUnit.setDisabled(false);
                            cboExtendAppBound.setDisabled(false);
                            cboFruitKind.setDisabled(false);
                            txtThematic.setDisabled(false);
                            txtSummary.setDisabled(false);
                            cboMostlyPersonsSchoolAge.setDisabled(false);
                            cboMostlyPersonsJob.setDisabled(false);
                            cboBearThePalmGrade.setDisabled(false);
                            cboIsextendApp.setDisabled(false);
                            txtExtendIncome.setDisabled(false);
                            cboTaskSource.setDisabled(false);
                            txtNewReadNumber.setDisabled(false);
                            txtPatentIncome.setDisabled(false);
                            txtPatent.setDisabled(false);
                            cboPatentSort.setDisabled(false);

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
        
        var AuthorDataBaseOper = function(OperCode,staffid,name,AuthorRanking,Remarks,optype) {
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
        
         var applyFilter = function() {
            Store1.filterBy(getRecordFilter());
        };
        var getRecordFilter = function() {
            var f = [];
            f.push({
                filter: function(record) {
                    return filterString(txt_SearchTxt.getValue(), 'MOSTLY_UNIT', record);
                }
            });
            f.push({
                filter: function(record) {
                    return filterString(txt_SearchTxt.getValue(), 'MOSTLY_PERSONS', record);
                }
            });
            f.push({
                filter: function(record) {
                    return filterString(txt_SearchTxt.getValue(), 'MOSTLY_JON_PERSONS', record);
                }
            });
            f.push({
                filter: function(record) {
                    return filterString(txt_SearchTxt.getValue(), 'THEMATIC', record);
                }
            });

            var len = f.length;
            return function(record) {
                if (f[0].filter(record) || f[1].filter(record) || f[2].filter(record) || f[3].filter(record)) {
                    return true;
                }
                return false;
            }
        };
        var filterString = function(value, dataIndex, record) {
            var val = record.get(dataIndex);
            if (typeof val != "string") {
                return value.length == 0;
            }
            return val.toLowerCase().indexOf(value.toLowerCase()) > -1;
        };
        
        
        var CheckForm = function() {
            if (dtfBearThePalmDate.validate() == false) {
                return false;
            }
            if (txtExtendIncome.validate() == false) {
                return false;
            }
            if (txtPatentIncome.validate() == false) {
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
                    <ext:RecordField Name="FRUIT_NAME" />
                    <ext:RecordField Name="MOSTLY_UNIT" />
                    <ext:RecordField Name="MOSTLY_PERSONS" />
                    <ext:RecordField Name="MOSTLY_PERSONS_SCHOOL_AGE" />
                    <ext:RecordField Name="MOSTLY_PERSONS_JOB" />
                    <ext:RecordField Name="MOSTLY_JON_PERSONS" />
                    <ext:RecordField Name="BEAR_THE_PALM_DATE" />
                    <ext:RecordField Name="BEAR_THE_PALM_GRADE" />
                    <ext:RecordField Name="FRUIT_KIND" />
                    <ext:RecordField Name="TASK_SOURCE" />
                    <ext:RecordField Name="PATENT" />
                    <ext:RecordField Name="NEW_READ_NUMBER" />
                    <ext:RecordField Name="THEMATIC" />
                    <ext:RecordField Name="SUMMARY" />
                    <ext:RecordField Name="AUTH_UNIT" />
                    <ext:RecordField Name="ISEXTEND_APP" />
                    <ext:RecordField Name="EXTEND_APP_BOUND" />
                    <ext:RecordField Name="EXTEND_INCOME" />
                    <ext:RecordField Name="PATENT_SORT" />
                    <ext:RecordField Name="PATENT_INCOME" />
                    <ext:RecordField Name="FRUIT_CODE" />
                    <ext:RecordField Name="MARK_SUG"/>
                    <ext:RecordField Name="SETUP_SUG"/>
                    <ext:RecordField Name="ADD_MARK" />
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
                    <ext:RecordField Name="FRUIT_CODE" />
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
                                <ext:Label ID="Label7" runat="server" Text="获奖类别等级:">
                                </ext:Label>
                                <ext:ComboBox ID="cboThePalmGrade" runat="server" Width="160">
                                </ext:ComboBox>
                                <ext:ToolbarSeparator ID="ToolbarSeparator4" runat="server" />
                                <ext:Button ID="btnSearch" runat="server" Text="查询" Icon="DatabaseGo">
                                    <Listeners>
                                        <Click Handler="#{Store1}.reload();#{btn_Delete}.disable();" />
                                    </Listeners>
                                </ext:Button>
                                <ext:ToolbarSeparator>
                                </ext:ToolbarSeparator>
                                <ext:Button ID="btn_Add" runat="server" Text="添加成果" Icon="Add">
                                    <Listeners>
                                        <Click Handler="if(#{DeptCodeCombo}.getSelectedItem().value == '') {Ext.Msg.show({ title: '信息提示', msg: '请选择科室', icon: 'ext-mb-info', buttons: { ok: true }  });} else {TreeOpration(1,'')}" />
                                    </Listeners>
                                </ext:Button>
                                <ext:Button Text="删除成果" ID="btn_Delete" runat="server" Icon="Delete" Disabled="true">
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
                                                        <ext:Column ColumnID="FRUIT_NAME" Header="成果名称" Sortable="true" DataIndex="FRUIT_NAME" />
                                                        <ext:Column ColumnID="MOSTLY_PERSONS" Header="第一作者" Sortable="true" DataIndex="MOSTLY_PERSONS" />
                                                        <ext:Column ColumnID="BEAR_THE_PALM_DATE" Header="获奖时间" Sortable="true" DataIndex="BEAR_THE_PALM_DATE" />
                                                        <ext:Column ColumnID="BEAR_THE_PALM_GRADE" Header="获奖类别及等级" Sortable="true" DataIndex="BEAR_THE_PALM_GRADE" />
                                                        <ext:Column ColumnID="FRUIT_KIND" Header="成果性质" Sortable="true" DataIndex="FRUIT_KIND" />
                                                        <ext:Column ColumnID="TASK_SOURCE" Header="任务来源" Sortable="true" DataIndex="TASK_SOURCE" />
                                                        <ext:Column ColumnID="ADD_MARK" Header="审批标识" Sortable="true" DataIndex="ADD_MARK" />
                                                        <ext:Column ColumnID="SETUP_SUG" Header="提交意见" Sortable="true" DataIndex="SETUP_SUG" />
                                                        <ext:Column ColumnID="MARK_SUG" Header="审批意见" Sortable="true" DataIndex="MARK_SUG" />
                                                        <ext:CommandColumn Width="38" Header="操作">
                                                            <Commands>
                                                                <ext:SplitCommand Icon="TableMultiple">
                                                                    <ToolTip Text="单项操作" />
                                                                    <Menu>
                                                                        <Items>
                                                                            <ext:MenuCommand CommandName="CmdBJGW" Icon="Wrench" Text="单项处理成果">
                                                                            </ext:MenuCommand>
                                                                            <ext:MenuCommand CommandName="CmdAddAuthor" Icon="TagBlue" Text="添加参加者">
                                                                            </ext:MenuCommand>
                                                                            <ext:MenuCommand CommandName="CmdEditAuthor" Icon="TextListBullets" Text="查看参加者">
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
                                                <BottomBar>
                                                    <ext:Toolbar ID="Toolbar4" runat="server" Height="26">
                                                        <Items>
                                                            <ext:TextField ID="txt_SearchTxt" runat="server" EmptyText="查找信息">
                                                                <ToolTips>
                                                                    <ext:ToolTip ID="ToolTip1" runat="server" Html="根据完成单位,完成人,参加者,主题词关键字查找">
                                                                    </ext:ToolTip>
                                                                </ToolTips>
                                                            </ext:TextField>
                                                            <ext:Button ID="btn_Search" Icon="Zoom" runat="server" Text="查询">
                                                                <Listeners>
                                                                    <Click Fn="applyFilter" />
                                                                </Listeners>
                                                            </ext:Button>
                                                        </Items>
                                                    </ext:Toolbar>
                                                </BottomBar>
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
    <ext:Window ID="arcEditAuthor" runat="server" Icon="Group" Title="参加者编辑" Width="300"
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
                                    <ext:TextArea ID="txaRemarks" runat="server" FieldLabel="技术贡献点" Height="100" />
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
    <ext:Window ID="ViewAuthor" runat="server" Icon="Group" Title="查看参加者" Width="600"
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
                Header="false" AutoScroll="true" BodyStyle="background-color:Transparent;" Height="335">
                <ColumnModel ID="ColumnModel2" runat="server">
                    <Columns>
                        <ext:Column ColumnID="PROBLEM_CODE" Header="成果编号" Sortable="true" DataIndex="FRUIT_CODE" />
                        <ext:Column ColumnID="STAFF_NAME" Header="负责人" Sortable="true" DataIndex="STAFF_NAME" />
                        <ext:Column ColumnID="RANK" Header="排名" Sortable="true" DataIndex="RANK" />
                        <ext:Column ColumnID="REMARK" Header="技术贡献点" Sortable="true" DataIndex="REMARK" />
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
    <ext:Window ID="arcEditWindow" runat="server" Icon="Group" Title="成果" Width="600"
        Height="470" AutoShow="false" Modal="true" CenterOnLoad="true" ShowOnLoad="false"
        Resizable="false" StyleSpec="background-color:Transparent;" BodyStyle="background-color:Transparent;">
        <Body>
            <ext:ColumnLayout ID="ColumnLayout2" runat="server">
                <ext:LayoutColumn ColumnWidth=".5">
                    <ext:Panel ID="Panel2" runat="server" Border="false" Header="false" BodyStyle="background-color:Transparent;margin:10px;">
                        <Body>
                            <ext:FormLayout ID="FormLayout1" runat="server" LabelAlign="Left">
                                <ext:Anchor Horizontal="92%">
                                    <ext:TextField ID="txtFruitCode" runat="server" FieldLabel="成果编号" />
                                </ext:Anchor>
                                <ext:Anchor Horizontal="92%">
                                    <ext:TextField ID="txtFruitName" runat="server" FieldLabel="成果名称" />
                                </ext:Anchor>
                                <ext:Anchor Horizontal="92%">
                                    <ext:TextField ID="txtMostlyUnit" runat="server" FieldLabel="主要完成单位" />
                                </ext:Anchor>
                                <ext:Anchor Horizontal="92%">
                                    <ext:TextField ID="txtMostlyPersons" runat="server" FieldLabel="主要完成人" />
                                </ext:Anchor>
                                <ext:Anchor Horizontal="92%">
                                    <ext:DateField ID="dtfBearThePalmDate" runat="server" FieldLabel="获奖时间" CausesValidation="true" AllowBlank="false"/>
                                </ext:Anchor>
                                <ext:Anchor Horizontal="92%">
                                    <ext:ComboBox ID="cboAuthUnit" runat="server" FieldLabel="批准单位" />
                                </ext:Anchor>
                                <ext:Anchor Horizontal="92%">
                                    <ext:ComboBox ID="cboExtendAppBound" runat="server" FieldLabel="推广应用范围" />
                                </ext:Anchor>
                                <ext:Anchor Horizontal="92%">
                                    <ext:ComboBox ID="cboFruitKind" runat="server" FieldLabel="成果性质" />
                                </ext:Anchor>
                                <ext:Anchor Horizontal="92%">
                                    <ext:TextField ID="txtThematic" runat="server" FieldLabel="主题词" />
                                </ext:Anchor>
                                <ext:Anchor Horizontal="92%">
                                    <ext:TextArea ID="txtSummary" runat="server" FieldLabel="成果摘要" Height="130"/>
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
                                    <ext:ComboBox ID="cboMostlyPersonsSchoolAge" runat="server" FieldLabel="主要完成人学历" />
                                </ext:Anchor>
                                <ext:Anchor Horizontal="92%">
                                    <ext:ComboBox ID="cboMostlyPersonsJob" runat="server" FieldLabel="主要完成人职称" />
                                </ext:Anchor>
                                <ext:Anchor Horizontal="92%">
                                    <ext:ComboBox ID="cboBearThePalmGrade" runat="server" FieldLabel="获奖类别及等级" />
                                </ext:Anchor>
                                <ext:Anchor Horizontal="92%">
                                    <ext:ComboBox ID="cboIsextendApp" runat="server" FieldLabel="是否推广应用" SelectedIndex="0">
                                        <Items>
                                            <ext:ListItem Value="是" Text="是" />
                                            <ext:ListItem Value="否" Text="否" />
                                        </Items>
                                    </ext:ComboBox>
                                </ext:Anchor>
                                <ext:Anchor Horizontal="92%">
                                    <ext:NumberField ID="txtExtendIncome" runat="server" FieldLabel="推广收益" CausesValidation="true" AllowBlank="false"/>
                                </ext:Anchor>
                                <ext:Anchor Horizontal="92%">
                                    <ext:ComboBox ID="cboTaskSource" runat="server" FieldLabel="任务来源" />
                                </ext:Anchor>
                                <ext:Anchor Horizontal="92%">
                                    <ext:TextField ID="txtNewReadNumber" runat="server" FieldLabel="新药批文号" />
                                </ext:Anchor>
                                <ext:Anchor Horizontal="92%">
                                    <ext:NumberField ID="txtPatentIncome" runat="server" FieldLabel="专利收益" CausesValidation="true" AllowBlank="false"/>
                                </ext:Anchor>
                                <ext:Anchor Horizontal="92%">
                                    <ext:TextField ID="txtPatent" runat="server" FieldLabel="专利号" />
                                </ext:Anchor>
                                <ext:Anchor Horizontal="92%">
                                    <ext:ComboBox ID="cboPatentSort" runat="server" FieldLabel="专利类别" />
                                </ext:Anchor>
                                <ext:Anchor Horizontal="92%">
                                    <ext:TextArea ID="txrSetSug" runat="server" FieldLabel="提交意见" Height="50" />
                                </ext:Anchor>
                                <ext:Anchor Horizontal="92%">
                                    <ext:TextArea ID="txrSug" runat="server" FieldLabel="审批意见" Height="50" />
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
                            <Click OnEvent="SaveInfo" Before="if (CheckForm()== false){ Ext.Msg.alert('系统提示','请根据红线提示填写正确的信息！');return false;};">
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
