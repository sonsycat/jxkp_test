<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SatffList.aspx.cs" Inherits="GoldNet.JXKP.RLZY.BaseInfoMaintain.SatffList" %>

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
                DisableContrl(false);
                txtDeptInput.setValue(DeptCodeCombo.getText());    // 科室
                txtStaffInput.setValue("");                        // 姓名
                dtfBirthday.setValue("");                          // 出生年月
                cboSex.setValue("");                               // 性别
                cboDuty.setValue("");                              // 行政职务
//               cboStationtype.setValue("");                      // 岗位类别
                cboIfarmy.setValue("");                            // 是否军人
                cboSanispecsort.setValue("");                      // 卫生专业分类
                txtMedicardmark.setValue("");                      // 医疗卡账号
                cboBraid.setValue("0");                            // 实虚编
                cboRank.setValue("");                              // 级别 
                dtfDutydate.setValue("");                          // 行政职务时间 
                cboPeople.setValue("");                            // 民族
                cboIsOnGuard.selectByIndex(0);                     // 在岗否
                cboStation.setValue("");                           // 岗位名称
                cboPerssort.setValue("");                          // 人员类别
                cboRootspecsort.setValue("");                      // 从事专业
                txtMediCard.setValue("");                          // 医疗卡号
                imgStaff.setImageUrl("/resources/UploadPicfile/user_default.png");           //
                cboSpeciality.setValue("");                        // 所学专业
                cboJobDuty.setValue("");                           // 技术职务
                cboCivilServiceClass.setValue("");                 // 文职级
                cboTiptopLearnStuffer.setValue("");                // 学历
                cboTitle.setValue("");                             // 职称
                cboTitleList.setValue("");                         // 职称序列
                cboTechnicTitle.setValue("");                      // 技术资格
                txtRetainTerm.setValue("");                        // 受聘期限
                cboTechnicClass.setValue("");                      // 技术级
                cboDegree.setValue("");                            // 学位
                cboCadreType.setValue("");                         // 干部类别
                cboDeptType.setValue("");                          // 所在科室类
                cboBackboneCircs.setValue("");                     // 专家骨干情况
                cboGovAllowance.setValue("");                      // 政府津贴
                cmbMaritalStatus.setValue("");                     // 婚姻状况
                dtfJobDate.setValue("");                           // 技术职务时间
                dtfCivilServiceClassDate.setValue("");             // 文职级时间
                dtfStudyOverdate.setValue("");                     // 毕业时间
                dtfWorkDate.setValue("");                          // 工作时间
                txtGraduateAcademy.setValue("");                   // 毕业院校
                dtfGradetitleDate.setValue("");                    // 取得学历时间
                txtCredithourPerYear.setValue("");                 // 年平均学分
                dtfTechnicClassDate.setValue("");                  // 技术级时间
                dtfTitleAssess.setValue("");                       // 资格评定时间
                dtfBeEnrolledInDate.setValue("");                  // 入伍时间
                txtBonusCoefficient.setValue("");                  // 奖金系数
                dtfInHospitalDate.setValue("");                    // 来院时间
                DateField1.setValue("");                           // 离职时间
                txtCertificateNo.setValue("");                     // 证件号码
                txtHomeplace.setValue("");                         // 出生地点
                txtMemo.setValue("");                              // 备 注
                empno.setValue("");                                // 人员工号
                
                ComboBox2.setValue("");                            // 是否发奖金
                ComboBox3.setValue("");                            // 是否考勤
                txtBonusNum.setValue("");                          // 奖金卡号
                Goldnet.AjaxMethod.request(
                      'StationTypeAjaxOper',
                        {
                                    params: {
                           deptCode:''
                        },
                            success: function(result) {
                                //cboStationtype.setValue(result);  // 岗位类别
                            },
                            failure: function(msg) {
                                GridPanel_Show.el.unmask();
                            }
                });
                
                Btn_BatStart.setVisible(true);
                btnApprove.setVisible(false);
                btnSetUp.setVisible(false);
                btnSaveUp.setVisible(false);
                btnSetSave.setVisible(false);
               
                if(PowerInfoHidden.value==2 && hiddenEdit.value == "1") {btnSaveUp.setVisible(true);}
                cboGord.setValue("");
                document.getElementById("txtBonusNum").readOnly=false;
                arcEditWindow.show();
            } else if (optype == "2") {
                ViewStore();
                var ShowFlg = hiddenEdit.value == '2'?true :false;
               
                DisableContrl(ShowFlg);
                //document.getElementById("txtBonusNum").readOnly=true;
               
                Btn_BatStart.setVisible(false);
                btnApprove.setVisible(false);
                btnSetUp.setVisible(false);
                btnSaveUp.setVisible(false);
                btnSetSave.setVisible(false);
                if(PowerInfoHidden.value==1) 
                {
                  btnApprove.setVisible(true)
                } else 
                {
                  btnSetUp.setVisible(true);
                  if(cbxOpration.checked) {
                    btnSetSave.setVisible(true);
                  }
                }
                arcEditWindow.show();
            } else if (optype == "3") {
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
        
        //
        function OperEchoCallback(selections,optype) {
            var id = '';
            for(var i =0;i<selections.length;i++) {
                if(i == selections.length -1) {
                    id = id+selections[i].data.STAFF_ID;
                } else {
                    id = id+selections[i].data.STAFF_ID +',';
                }
            }
            GridPanelToDataBase(id,optype);
        }
        
        //
        function GridPanelToDataBase(id,optype) {
          Goldnet.AjaxMethod.request(
                  'StaffInfoAjaxOper',
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
        
        //
        var applyFilter = function() {
            Store1.filterBy(getRecordFilter());
        };
        
        //
        var getRecordFilter = function() {
            var f = [];
            f.push({
                filter: function(record) {
                    return filterString(txt_SearchTxt.getValue(), 'NAME', record);
                }
            });
            var len = f.length;
            return function(record) {
                if (f[0].filter(record)) {
                    return true;
                }
                return false;
            }
        };
        
        //
        var filterString = function(value, dataIndex, record) {
            var val = record.get(dataIndex);
            if (typeof val != "string") {
                return value.length == 0;
            }
            return val.toLowerCase().indexOf(value.toLowerCase()) > -1;
        };
        
        //
        var CheckForm = function() {
            //人员类别
            if (cboPerssort.validate() == false) {
                return false;
            }
            //姓名
            if (txtStaffInput.validate() == false) {
                return false;
            }
            //出生年月
            if (dtfBirthday.validate() == false) {
                return false;
            }
            //行政职务
            if (cboDuty.validate() == false) {
                return false;
            }
            //从事专业
            if (cboRootspecsort.validate() == false) {
                return false;
            }
            //是否发放奖金
            if (ComboBox2.validate() == false) {
                return false;
            }
            //是否考勤
            if (ComboBox3.validate() == false) {
                return false;
            }
            //技术职务
            if (cboJobDuty.validate() == false) {
                return false;
            }
            //职称序列
            if (cboTitleList.validate() == false) {
                return false;
            }
            //是否军人
            if (cboIfarmy.validate() == false) {
                return false;
            }
            return true;
        }
          
        function checkType(){
            //得到上传文件的值
            var fileName=document.getElementById("photoimg").value;
            //返回String对象中子字符串最后出现的位置.
            var seat=fileName.lastIndexOf(".");
            //返回位于String对象中指定位置的子字符串并转换为小写.
            var extension=fileName.substring(seat).toLowerCase();
            //判断允许上传的文件格式
            if(extension!=".jpg"&&extension!=".jpeg"&&extension!=".gif"&&extension!=".png"&&extension!=".bmp"){
              Ext.Msg.show({ title: '信息提示', msg: '不支持'+extension+'文件的上传', icon: 'ext-mb-info', buttons: { ok: true }});
              return false;
            }else{
              return true;
            }
        }
          
        function ViewDetial() {
            ViewStore();
            DisableContrl(true);
            Btn_BatStart.setVisible(false);
            btnApprove.setVisible(false);
            btnSetUp.setVisible(false);
            btnSaveUp.setVisible(false);
            btnSetSave.setVisible(false);
            arcEditWindow.show();
          
        }
          
        function DisableContrl(ShowFlg) {
            cboInptName.setDisabled(ShowFlg);
            txtDeptInput.setDisabled(ShowFlg);            // 科室
            txtStaffInput.setDisabled(ShowFlg);           // 姓名
            dtfBirthday.setDisabled(ShowFlg);             // 出生年月
            cboSex.setDisabled(ShowFlg);                  // 性别
            cboDuty.setDisabled(ShowFlg);                 // 行政职务
            //cboStationtype.setDisabled(ShowFlg);        // 岗位类别
            cboIfarmy.setDisabled(ShowFlg);               // 是否军人
            cboSanispecsort.setDisabled(ShowFlg);         // 卫生专业分类
            txtMedicardmark.setDisabled(ShowFlg);         // 医疗卡账号
            cboBraid.setDisabled(ShowFlg);                // 实虚编
            cboRank.setDisabled(ShowFlg);                 // 级别 
            dtfDutydate.setDisabled(ShowFlg);             // 行政职务时间 
            cboPeople.setDisabled(ShowFlg);               // 民族
            cboIsOnGuard.setDisabled(ShowFlg);            // 在岗否
            cboStation.setDisabled(ShowFlg);              // 岗位名称
            cboPerssort.setDisabled(ShowFlg);             // 人员类别
            cboRootspecsort.setDisabled(ShowFlg);         // 从事专业
            txtMediCard.setDisabled(ShowFlg);             // 医疗卡号
            photoimg.setDisabled(ShowFlg);
            cboSpeciality.setDisabled(ShowFlg);           // 所学专业
            cboJobDuty.setDisabled(ShowFlg);              // 技术职务
            cboCivilServiceClass.setDisabled(ShowFlg);    // 文职级
            cboTiptopLearnStuffer.setDisabled(ShowFlg);   // 学历
            cboTitle.setDisabled(ShowFlg);                // 职称
            cboTitleList.setDisabled(ShowFlg);            // 职称序列
            cboTechnicTitle.setDisabled(ShowFlg);         // 技术资格
            txtRetainTerm.setDisabled(ShowFlg);           // 受聘期限
            cboTechnicClass.setDisabled(ShowFlg);         // 技术级
            cboDegree.setDisabled(ShowFlg);               // 学位
            cboCadreType.setDisabled(ShowFlg);            // 干部类别
            cboDeptType.setDisabled(ShowFlg);             // 所在科室类
            cboBackboneCircs.setDisabled(ShowFlg);        // 专家骨干情况
            cboGovAllowance.setDisabled(ShowFlg);         // 政府津贴
            cmbMaritalStatus.setDisabled(ShowFlg);        // 婚姻状况
            dtfJobDate.setDisabled(ShowFlg);              // 技术职务时间
            dtfCivilServiceClassDate.setDisabled(ShowFlg);// 文职级时间
            dtfStudyOverdate.setDisabled(ShowFlg);        // 毕业时间
            dtfWorkDate.setDisabled(ShowFlg);             // 工作时间
            DateField1.setDisabled(ShowFlg);              // 离职时间
            txtGraduateAcademy.setDisabled(ShowFlg);      // 毕业院校
            dtfGradetitleDate.setDisabled(ShowFlg);       // 取得学历时间
            txtCredithourPerYear.setDisabled(ShowFlg);    // 年平均学分
            dtfTechnicClassDate.setDisabled(ShowFlg);     // 技术级时间
            dtfTitleAssess.setDisabled(ShowFlg);          // 资格评定时间
            dtfBeEnrolledInDate.setDisabled(ShowFlg);     // 入伍时间
            txtBonusCoefficient.setDisabled(ShowFlg);     // 奖金系数
            dtfInHospitalDate.setDisabled(ShowFlg);       // 来院时间
            txtCertificateNo.setDisabled(ShowFlg);        // 证件号码
            txtHomeplace.setDisabled(ShowFlg);            // 出生地点
            txtMemo.setDisabled(ShowFlg);                 // 备注
            txtBonusNum.setDisabled(ShowFlg);               // 奖金卡号
            cboGord.setDisabled(ShowFlg);
            empno.setDisabled(ShowFlg);                   //人员工号
        }
          
        function ViewStore() {
                var record = Store1.getAt(RowIndex);
                if(record.data.USER_ID == '' || record.data.USER_ID == null) {
                    cboInptName.setValue(false);
                }else {
                    cboInptName.setValue(true);
                }
                HiddenId.setValue(record.data.STAFF_ID);
                txtDeptInput.setValue(record.data.DEPT_NAME);                 // 科室
                if(record.data.USER_ID == '' || record.data.USER_ID == null) {
                    txtStaffInput.setValue(record.data.NAME); 
                } else {
                    txtStaffInput.setValue(record.data.USER_ID); 
                }
                dtfBirthday.setValue(record.data.BIRTHDAY);                   // 出生年月
                cboSex.setValue(record.data.SEX);                             // 性别
                cboDuty.setValue(record.data.DUTY);                           // 行政职务
                cboIfarmy.setValue(record.data.IF_ARMY);                      // 是否军人
                cboSanispecsort.setValue(record.data.SANTSPECSORT);           // 卫生专业分类
                txtMedicardmark.setValue(record.data.MEDICARDMARK);           // 医疗卡账号
                if(record.data.ISBRAID == '' || record.data.ISBRAID == null) {
                    cboBraid.setValue('0');                                   // 实虚编
                }else {
                    cboBraid.setValue(record.data.ISBRAID);                   // 实虚编
                }
                cboRank.setValue(record.data.RANK);                           // 级别 
                dtfDutydate.setValue(record.data.DUTYDATE);                   // 行政职务时间 
                DateField1.setValue(record.data.TURNOVER_TIME);               // 离职时间
                cboPeople.setValue(record.data.NATIONALS);                    // 民族
                cboIsOnGuard.setValue(record.data.ISONGUARD);                 // 在岗否
                cboPerssort.setValue(record.data.STAFFSORT);                  // 人员类别
                cboRootspecsort.setValue(record.data.ROOTSPECSORT);           // 从事专业
                txtMediCard.setValue(record.data.MEDICARD);                   // 医疗卡号
                HiddenImage.setValue(record.data.IMG_ID);
                if(record.data.IMG_ID == '' || record.data.IMG_ID == null) {
                    imgStaff.setImageUrl("/resources/UploadPicfile/user_default.png");          // 图片
                } else {
                    imgStaff.setImageUrl("/resources/UploadPicfile/"+record.data.IMG_ID);       // 图片
                }
                cboSpeciality.setValue(record.data.STUDY_SPECSORT);                             // 所学专业
                cboJobDuty.setValue(record.data.JOB);                                           // 技术职务
                cboCivilServiceClass.setValue(record.data.CIVILSERVICECLASS);                   // 文职级
                cboTiptopLearnStuffer.setValue(record.data.TOPEDUCATE);                         // 学历
                cboTitle.setValue(record.data.TITLE);                                           // 职称
                cboTitleList.setValue(record.data.TITLE_LIST);                                  // 职称序列
                cboTechnicTitle.setValue(record.data.JOB_TITLE);                                // 技术资格
                txtRetainTerm.setValue(record.data.RETAINTERM);                                 // 受聘期限
                cboTechnicClass.setValue(record.data.TECHINCCLASS);                             // 技术级
                cboDegree.setValue(record.data.EDU1);                                           // 学位
                cboCadreType.setValue(record.data.CADRES_CATEGORIES);                           // 干部类别
                cboDeptType.setValue(record.data.DEPT_TYPE);                                    // 所在科室类
                cboBackboneCircs.setValue(record.data.EXPERT);                                  // 专家骨干情况
                cboGovAllowance.setValue(record.data.GOVERNMENT_ALLOWANCE);                     // 政府津贴
                cmbMaritalStatus.setValue(record.data.MARITAL_STATUS);                          // 婚姻状况
                dtfJobDate.setValue(record.data.JOBDATE);                                       // 技术职务时间
                dtfCivilServiceClassDate.setValue(record.data.CIVILSERVICECLASSDATE);           // 文职级时间
                dtfStudyOverdate.setValue(record.data.STUDY_OVER_DATE);                         // 毕业时间
                dtfWorkDate.setValue(record.data.WORKDATE);                                     // 工作时间
                txtGraduateAcademy.setValue(record.data.GRADUATE_ACADEMY);                      // 毕业院校
                dtfGradetitleDate.setValue(record.data.DATE_OF_GRADETITLE);                     // 取得学历时间
                txtCredithourPerYear.setValue(record.data.CREDITHOUR_PERYEAR);                  // 年平均学分
                dtfTechnicClassDate.setValue(record.data.TECHNICCLASSDATE);                     // 技术级时间
                dtfTitleAssess.setValue(record.data.TITLE_DATE);                                // 资格评定时间
                dtfBeEnrolledInDate.setValue(record.data.BEENROLLEDINDATE);                     // 入伍时间
                txtBonusCoefficient.setValue(record.data.BONUS_FACTOR);                         // 奖金系数
                dtfInHospitalDate.setValue(record.data.INHOSPITALDATE);                         // 来院时间
                txtCertificateNo.setValue(record.data.CERTIFICATE_NO);                          // 证件号码
                txtHomeplace.setValue(record.data.HOMEPLACE);                                   // 出生地点
                txtMemo.setValue(record.data.MEMO);                                             // 备注
                txtBonusNum.setValue(record.data.BANK_CODE);                                     // 奖金卡号
                Goldnet.AjaxMethod.request(
                      'StationTypeAjaxOper',
                        {
                        params: {
                           deptCode:record.data.DEPT_CODE
                        },
                            success: function(result) {
                                
                                cboStation.setValue(record.data.STATION_CODE);// 岗位名称
                            },
                            failure: function(msg) {
                                GridPanel_Show.el.unmask();
                            }
                });
                cboGord.setValue(record.data.GORD_CODE);
                empno.setValue(record.data.EMP_NO);         //人员工号
                
                ComboBox2.setValue(record.data.BONUS_FLAG); //是否发放奖金
                ComboBox3.setValue(record.data.CHECK_FLAG); //是否考勤
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
    <ext:Hidden ID="HiddenImage" runat="server">
    </ext:Hidden>
    <ext:Hidden ID="hiddenMeunUp" runat="server">
    </ext:Hidden>
    <ext:Hidden ID="hiddenEdit" runat="server">
    </ext:Hidden>
    <ext:Hidden ID="hiddenIsAmry" runat="server">
    </ext:Hidden>
    <ext:Store ID="Store1" runat="server" OnRefreshData="Data_RefreshData">
        <Reader>
            <ext:JsonReader ReaderID="STAFF_ID">
                <Fields>
                    <ext:RecordField Name="EMP_NO" />
                    <ext:RecordField Name="STAFF_ID" />
                    <ext:RecordField Name="DEPT_CODE" />
                    <ext:RecordField Name="DEPT_NAME" />
                    <ext:RecordField Name="NAME" />
                    <ext:RecordField Name="IF_ARMY" />
                    <ext:RecordField Name="ADD_MARK" />
                    <ext:RecordField Name="ISONGUARD" />
                    <ext:RecordField Name="BIRTHDAY" />
                    <ext:RecordField Name="SEX" />
                    <ext:RecordField Name="NATIONALS" />
                    <ext:RecordField Name="BONUS_FACTOR" />
                    <ext:RecordField Name="GOVERNMENT_ALLOWANCE" />
                    <ext:RecordField Name="CADRES_CATEGORIES" />
                    <ext:RecordField Name="STUDY_OVER_DATE" />
                    <ext:RecordField Name="DEPT_TYPE" />
                    <ext:RecordField Name="TOPEDUCATE" />
                    <ext:RecordField Name="STUDY_SPECSORT" />
                    <ext:RecordField Name="INHOSPITALDATE" />
                    <ext:RecordField Name="BASEPAY" />
                    <ext:RecordField Name="RETAINTERM" />
                    <ext:RecordField Name="JOB" />
                    <ext:RecordField Name="JOBDATE" />
                    <ext:RecordField Name="STAFFSORT" />
                    <ext:RecordField Name="BEENROLLEDINDATE" />
                    <ext:RecordField Name="WORKDATE" />
                    <ext:RecordField Name="DUTY" />
                    <ext:RecordField Name="DUTYDATE" />
                    <ext:RecordField Name="TECHINCCLASS" />
                    <ext:RecordField Name="TECHNICCLASSDATE" />
                    <ext:RecordField Name="CIVILSERVICECLASS" />
                    <ext:RecordField Name="CIVILSERVICECLASSDATE" />
                    <ext:RecordField Name="SANTSPECSORT" />
                    <ext:RecordField Name="ROOTSPECSORT" />
                    <ext:RecordField Name="MEDICARDMARK" />
                    <ext:RecordField Name="MEDICARD" />
                    <ext:RecordField Name="INPUT_USER" />
                    <ext:RecordField Name="INPUT_DATE" />
                    <ext:RecordField Name="USER_DATE" />
                    <ext:RecordField Name="GUARDTEAM" />
                    <ext:RecordField Name="GUARDGROUP" />
                    <ext:RecordField Name="GUARDDUTY" />
                    <ext:RecordField Name="GUARDTYPE" />
                    <ext:RecordField Name="GUARDCHAN" />
                    <ext:RecordField Name="GUARDTIME" />
                    <ext:RecordField Name="GUARDCAUS" />
                    <ext:RecordField Name="GUARDREMARK" />
                    <ext:RecordField Name="DEPTGROUP" />
                    <ext:RecordField Name="HOMEPLACE" />
                    <ext:RecordField Name="CERTIFICATE_NO" />
                    <ext:RecordField Name="MARITAL_STATUS" />
                    <ext:RecordField Name="TITLE_LIST" />
                    <ext:RecordField Name="EDU1" />
                    <ext:RecordField Name="GRADUATE_ACADEMY" />
                    <ext:RecordField Name="DATE_OF_GRADETITLE" />
                    <ext:RecordField Name="RANK" />
                    <ext:RecordField Name="TITLE" />
                    <ext:RecordField Name="GROUP_ID" />
                    <ext:RecordField Name="MEMO" />
                    <ext:RecordField Name="MARK_USER" />
                    <ext:RecordField Name="USER_ID" />
                    <ext:RecordField Name="JW_USER_NAME" />
                    <ext:RecordField Name="INPUT_CODE" />
                    <ext:RecordField Name="IMG_ID" />
                    <ext:RecordField Name="JOB_TITLE" />
                    <ext:RecordField Name="TITLE_DATE" />
                    <ext:RecordField Name="EXPERT" />
                    <ext:RecordField Name="CREDITHOUR_PERYEAR" />
                    <ext:RecordField Name="LEADTECN" />
                    <ext:RecordField Name="STATION_CODE" />
                    <ext:RecordField Name="STATION_NAME" />
                    <ext:RecordField Name="ISBRAID" />
                    <ext:RecordField Name="GORD_CODE" />
                    <ext:RecordField Name="TURNOVER_TIME" />
                    <ext:RecordField Name="CONTRACT_START" />
                    <ext:RecordField Name="CONTRACT_END" />
                    <ext:RecordField Name="BONUS_FLAG" />
                    <ext:RecordField Name="CHECK_FLAG" />
                    <ext:RecordField Name="BANK_CODE" />
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
            <ext:HttpProxy Method="POST" Url="/RLZY/WebService/NationInfos.ashx" />
        </Proxy>
        <Reader>
            <ext:JsonReader Root="NationInfos">
                <Fields>
                    <ext:RecordField Name="NATION_NAME" />
                    <ext:RecordField Name="NATION_CODE" />
                </Fields>
            </ext:JsonReader>
        </Reader>
    </ext:Store>
    <ext:Store ID="Store4" runat="server" AutoLoad="true">
        <Proxy>
            <ext:HttpProxy Method="POST" Url="/RLZY/WebService/UserInfos.ashx" />
        </Proxy>
        <Reader>
            <ext:JsonReader Root="UserInfos">
                <Fields>
                    <ext:RecordField Name="USER_ID" />
                    <ext:RecordField Name="ID" />
                    <ext:RecordField Name="USER_NAME" />
                    <ext:RecordField Name="DEPT_NAME" />
                </Fields>
            </ext:JsonReader>
        </Reader>
    </ext:Store>
    <ext:Store ID="StoreCombo" runat="server">
        <Reader>
            <ext:JsonReader ReaderID="ID">
                <Fields>
                    <ext:RecordField Name="ID" />
                    <ext:RecordField Name="STATION_NAME" />
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
                                    Width="90" ValueField="DEPT_CODE" TypeAhead="false" LoadingText="Searching..."
                                    PageSize="1000" ItemSelector="div.search-item" MinChars="1" FieldLabel="科室信息"
                                    ListWidth="240" ReadOnly="false">
                                    <Template ID="Template1" runat="server">
                                       <tpl for=".">
                                          <div class="search-item">
                                             <h3><span style="width:auto">{DEPT_CODE}</span>{DEPT_NAME}</h3>
                                          </div>
                                       </tpl>
                                    </Template>
                                </ext:ComboBox>
                                <ext:Label ID="Label1" runat="server" Text="人员类别：">
                                </ext:Label>
                                <ext:ComboBox ID="cboPersonType" runat="server" Editable="false" Width="80">
                                </ext:ComboBox>
                                <ext:ToolbarSeparator ID="ToolbarSeparator4" runat="server" />
                                <ext:Label ID="Label2" runat="server" Text="是否在岗：">
                                </ext:Label>
                                <ext:ComboBox ID="ComboBoxonguard" runat="server" Width="50">
                                </ext:ComboBox>
                                <ext:ToolbarSeparator ID="ToolbarSeparator1" runat="server" />
                                <ext:Label ID="Label4" runat="server" Text="性别：">
                                </ext:Label>
                                <ext:ComboBox ID="Combosex" runat="server" Width="60">
                                    <Items>
                                        <ext:ListItem Text="男" Value="男" />
                                        <ext:ListItem Text="女" Value="女" />
                                    </Items>
                                </ext:ComboBox>
                                <ext:Label ID="Label5" runat="server" Text="合同终止：">
                                </ext:Label>
                                <ext:DateField runat="server" ID="endtime" Vtype="daterange" AllowBlank="false" Format="yyyyMM"
                                    MaxLength="6" Width="70">
                                </ext:DateField>
                                <ext:Button ID="btnSearch" runat="server" Text="查询" Icon="DatabaseGo">
                                    <Listeners>
                                        <Click Handler="#{Store1}.reload();#{btn_EchoHandle}.disable();#{btn_Delete}.disable();" />
                                    </Listeners>
                                </ext:Button>
                                <ext:ToolbarSeparator>
                                </ext:ToolbarSeparator>
                                <ext:Button ID="btn_Add" runat="server" Text="添加" Icon="Add">
                                    <Listeners>
                                        <Click Handler="if(#{DeptCodeCombo}.getSelectedItem().value == '') {Ext.Msg.show({ title: '信息提示', msg: '请选择科室', icon: 'ext-mb-info', buttons: { ok: true }  });} else {TreeOpration(1,'')}" />
                                    </Listeners>
                                </ext:Button>
                                <ext:Button Text="删除" ID="btn_Delete" runat="server" Icon="Delete" Disabled="true">
                                    <Listeners>
                                        <Click Handler="TreeOpration(3)" />
                                    </Listeners>
                                </ext:Button>
                                <ext:Button Text="单项操作" ID="btnCommand" runat="server" Icon="ApplicationEdit" Disabled="true">
                                    <%-- <Listeners>
                                        <Click Handler="TreeOpration(2)" />
                                    </Listeners>--%>
                                    <AjaxEvents>
                                        <Click OnEvent="RowClick">
                                            <EventMask Msg="载入中..." ShowMask="true" />
                                            <ExtraParams>
                                                <ext:Parameter Name="Values" Value="Ext.encode(#{GridPanel_Show}.getRowsValues())"
                                                    Mode="Raw">
                                                </ext:Parameter>
                                            </ExtraParams>
                                        </Click>
                                    </AjaxEvents>
                                </ext:Button>
                                <ext:Button Text="技术档案" ID="jsda" runat="server" Icon="ApplicationEdit" Disabled="true"
                                    Hidden="true">
                                    <AjaxEvents>
                                        <Click OnEvent="DbRowClick">
                                            <EventMask Msg="载入中..." ShowMask="true" />
                                            <ExtraParams>
                                                <ext:Parameter Name="Values" Value="Ext.encode(#{GridPanel_Show}.getRowsValues())"
                                                    Mode="Raw">
                                                </ext:Parameter>
                                            </ExtraParams>
                                        </Click>
                                    </AjaxEvents>
                                </ext:Button>
                                <ext:Button Text="批量处理" ID="btn_EchoHandle" runat="server" Icon="Wrench" Disabled="true">
                                    <Listeners>
                                        <Click Handler="TreeOpration(4)" />
                                    </Listeners>
                                </ext:Button>
                                <ext:Button ID="Btn_Excel" Text="导出Excel" Icon="PageWhiteExcel" runat="server" OnClick="OutExcel"
                                    AutoPostBack="true">
                                </ext:Button>
                                <ext:Checkbox ID="cbxOpration" runat="server" BoxLabel="显示" Checked="true">
                                    <%-- <Listeners>
                                        <Check Handler="#{Store1}.reload();#{btn_EchoHandle}.disable();#{btnCommand}.disable();#{btn_Delete}.disable();" />
                                    </Listeners>--%>
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
                                                        <ext:Column Header="科室" Width="100" Sortable="true" DataIndex="DEPT_NAME">
                                                        </ext:Column>
                                                        <ext:Column Header="姓名" Width="100" Sortable="true" DataIndex="NAME">
                                                        </ext:Column>
                                                        <ext:Column Header="性别" Width="100" Sortable="true" DataIndex="SEX">
                                                        </ext:Column>
                                                        <ext:Column Header="职务" Width="100" Sortable="true" DataIndex="DUTY">
                                                        </ext:Column>
                                                        <ext:Column Header="在岗" Width="100" Sortable="true" DataIndex="ISONGUARD">
                                                        </ext:Column>
                                                        <ext:Column Header="岗位" Width="100" Sortable="true" DataIndex="STATION_NAME">
                                                        </ext:Column>
                                                        <ext:Column Header="资格评定时间" Width="100" Sortable="true" DataIndex="TITLE_DATE">
                                                        </ext:Column>
                                                        <ext:Column Header="来院时间" Width="100" Sortable="true" DataIndex="INHOSPITALDATE">
                                                        </ext:Column>
                                                        <ext:Column Header="人员类别" Width="100" Sortable="true" DataIndex="STAFFSORT">
                                                        </ext:Column>
                                                        <ext:Column Header="审核人" Width="100" Sortable="true" DataIndex="MARK_USER">
                                                        </ext:Column>
                                                        <ext:Column Header="离职时间" Width="100" Sortable="true" DataIndex="TURNOVER_TIME">
                                                        </ext:Column>
                                                        <ext:Column Header="合同开始" Width="100" Sortable="true" DataIndex="CONTRACT_START">
                                                        </ext:Column>
                                                        <ext:Column Header="合同终止" Width="100" Sortable="true" DataIndex="CONTRACT_END">
                                                        </ext:Column>
                                                    </Columns>
                                                </ColumnModel>
                                                <Listeners>
                                                    <DblClick Handler="ViewDetial()" />
                                                </Listeners>
                                                <SelectionModel>
                                                    <ext:CheckboxSelectionModel ID="CheckboxSelectionModel1" runat="server">
                                                        <Listeners>
                                                            <RowSelect Handler="#{btn_Delete}.enable();#{btn_EchoHandle}.enable();#{btnCommand}.enable();#{jsda}.enable(); RowIndex = rowIndex" />
                                                            <RowDeselect Handler="if (!#{GridPanel_Show}.hasSelection()) {#{btn_Delete}.disable();#{btn_EchoHandle}.disable();#{btnCommand}.disable();#{jsda}.disable();}" />
                                                        </Listeners>
                                                    </ext:CheckboxSelectionModel>
                                                </SelectionModel>
                                                <LoadMask ShowMask="true" />
                                                <BottomBar>
                                                    <ext:PagingToolbar ID="PagingToolBar2" runat="server" PageSize="20" StoreID="Store1"
                                                        AutoWidth="true" DisplayInfo="true" AutoDataBind="true">
                                                        <Items>
                                                            <ext:TextField ID="txt_SearchTxt" runat="server" EmptyText="查找信息">
                                                                <ToolTips>
                                                                    <ext:ToolTip ID="ToolTip1" runat="server" Html="根据姓名关键字查找">
                                                                    </ext:ToolTip>
                                                                </ToolTips>
                                                            </ext:TextField>
                                                            <ext:Button ID="btn_Search" Icon="Zoom" runat="server" Text="查询">
                                                                <Listeners>
                                                                    <Click Fn="applyFilter" />
                                                                </Listeners>
                                                            </ext:Button>
                                                            <ext:Label runat="server" Text="人数" ID="labConut">
                                                            </ext:Label>
                                                        </Items>
                                                    </ext:PagingToolbar>
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
    <ext:Window ID="arcEditWindow" runat="server" Icon="Group" Title="人员信息" Width="825"
        Height="500" AutoShow="false" Modal="true" CenterOnLoad="true" ShowOnLoad="false"
        Resizable="true" StyleSpec="background-color:Transparent;" BodyStyle="background-color:Transparent;"
        AutoScroll="true" Maximizable="true" MinHeight="10" MinWidth="10" MinButtonWidth="10"
        MonitorResize="true">
        <Body>
            <ext:FieldSet ID="FieldSet1" runat="server" Width="780" Title="基本信息" Collapsible="true"
                StyleSpec="margin:5px;">
                <Body>
                    <ext:ColumnLayout ID="ColumnLayout2" runat="server">
                        <ext:LayoutColumn ColumnWidth=".35">
                            <ext:Panel ID="Panel2" runat="server" Border="false" StyleSpec="background-color:Transparent;"
                                BodyStyle="background-color:Transparent;">
                                <Body>
                                    <ext:FormLayout ID="FormLayout1" runat="server" LabelAlign="Left">
                                        <ext:Anchor Horizontal="92%">
                                            <ext:TextField ID="txtDeptInput" runat="server" FieldLabel="科室" LabelStyle="color:blue;"
                                                ReadOnly="true" />
                                        </ext:Anchor>
                                        <ext:Anchor Horizontal="92%">
                                            <ext:Panel runat="server" ID="panel6666" StyleSpec="background-color:Transparent;"
                                                BodyStyle="background-color:Transparent;" Border="false">
                                                <Body>
                                                    <table>
                                                        <tr>
                                                            <td style="width: 104px">
                                                                <ext:Label Text="姓名:" runat="server">
                                                                </ext:Label>
                                                            </td>
                                                            <td>
                                                                <ext:ComboBox ID="txtStaffInput" runat="server" StoreID="Store4" DisplayField="USER_NAME"
                                                                    Width="120" ValueField="USER_ID" TypeAhead="false" LoadingText="Searching..."
                                                                    PageSize="1000" ItemSelector="div.search-item" MinChars="1" FieldLabel="姓名" ListWidth="240"
                                                                    CausesValidation="true" AllowBlank="false" MaxLength="20" ForceSelection="false">
                                                                    <Template ID="Template3" runat="server">
                                                                   <tpl for=".">
                                                                      <div class="search-item">
                                                                         <h3><span style="width:auto">{DEPT_NAME}</span>{USER_NAME}</h3>
                                                                      </div>
                                                                   </tpl>
                                                                    </Template>
                                                                </ext:ComboBox>
                                                            </td>
                                                            <td>
                                                                <ext:Checkbox ID="cboInptName" runat="server" Checked="false">
                                                                    <ToolTips>
                                                                        <ext:ToolTip Html="选择军卫" runat="server">
                                                                        </ext:ToolTip>
                                                                    </ToolTips>
                                                                </ext:Checkbox>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </Body>
                                            </ext:Panel>
                                        </ext:Anchor>
                                        <ext:Anchor Horizontal="92%">
                                            <ext:DateField ID="dtfBirthday" runat="server" FieldLabel="出生年月" LabelStyle="color:blue;"
                                                Format="yyyy-MM-dd" CausesValidation="true" AllowBlank="false">
                                            </ext:DateField>
                                        </ext:Anchor>
                                        <ext:Anchor Horizontal="92%">
                                            <ext:ComboBox ID="cboSex" runat="server" FieldLabel="性别" Editable="false">
                                                <Items>
                                                    <ext:ListItem Text="男" Value="男" />
                                                    <ext:ListItem Text="女" Value="女" />
                                                </Items>
                                            </ext:ComboBox>
                                        </ext:Anchor>
                                        <ext:Anchor Horizontal="92%">
                                            <ext:ComboBox ID="cboDuty" runat="server" FieldLabel="行政职务" LabelStyle="color:blue;"
                                                Editable="false" CausesValidation="true" AllowBlank="false">
                                            </ext:ComboBox>
                                        </ext:Anchor>
                                        <ext:Anchor Horizontal="92%">
                                            <ext:ComboBox ID="cboRootspecsort" runat="server" FieldLabel="从事专业" LabelStyle="color:blue;"
                                                CausesValidation="true" AllowBlank="false" Editable="false" />
                                        </ext:Anchor>
                                        <ext:Anchor Horizontal="92%">
                                            <ext:ComboBox ID="cboSanispecsort" runat="server" FieldLabel="卫生专业分类" Editable="false">
                                            </ext:ComboBox>
                                        </ext:Anchor>
                                        <ext:Anchor Horizontal="92%">
                                            <ext:TextField ID="empno" runat="server" FieldLabel="人员工号" Editable="false" />
                                        </ext:Anchor>
                                        <ext:Anchor Horizontal="92%">
                                            <ext:DateField ID="dtfContractStart" runat="server" FieldLabel="合同开始时间" Format="yyyy-MM-dd">
                                            </ext:DateField>
                                        </ext:Anchor>
                                        <ext:Anchor Horizontal="92%">
                                            <ext:ComboBox ID="ComboBox2" runat="server" FieldLabel="是否发放奖金" LabelStyle="color:blue;"
                                                Editable="false" CausesValidation="true" AllowBlank="false">
                                                <Items>
                                                    <ext:ListItem Text="否" Value="否" />
                                                    <ext:ListItem Text="是" Value="是" />
                                                </Items>
                                            </ext:ComboBox>
                                        </ext:Anchor>
                                    </ext:FormLayout>
                                </Body>
                            </ext:Panel>
                        </ext:LayoutColumn>
                        <ext:LayoutColumn ColumnWidth=".35">
                            <ext:Panel ID="Panel3" runat="server" Border="false" StyleSpec="background-color:Transparent;"
                                BodyStyle="background-color:Transparent;">
                                <Body>
                                    <ext:FormLayout ID="FormLayout3" runat="server" LabelAlign="Left">
                                        <ext:Anchor Horizontal="92%">
                                            <ext:ComboBox ID="cboBraid" runat="server" FieldLabel="实虚编" SelectedIndex="0" Editable="false"
                                                CausesValidation="true" AllowBlank="false">
                                                <Items>
                                                    <ext:ListItem Text="实编" Value="0" />
                                                    <ext:ListItem Text="虚编" Value="1" />
                                                </Items>
                                            </ext:ComboBox>
                                        </ext:Anchor>
                                        <ext:Anchor Horizontal="92%">
                                            <ext:ComboBox ID="cboRank" runat="server" FieldLabel="级别" Editable="false">
                                            </ext:ComboBox>
                                        </ext:Anchor>
                                        <ext:Anchor Horizontal="92%">
                                            <ext:DateField ID="dtfDutydate" runat="server" FieldLabel="行政职务时间" Format="yyyy-MM-dd">
                                            </ext:DateField>
                                        </ext:Anchor>
                                        <ext:Anchor Horizontal="92%">
                                            <ext:ComboBox ID="cboPeople" runat="server" StoreID="Store2" DisplayField="NATION_NAME"
                                                Width="90" ValueField="NATION_CODE" TypeAhead="false" LoadingText="Searching..."
                                                PageSize="1000" ItemSelector="div.search-item" MinChars="1" FieldLabel="民族" ListWidth="240">
                                                <Template ID="Template2" runat="server">
                                                   <tpl for=".">
                                                      <div class="search-item">
                                                         <h3><span style="width:auto">{NATION_CODE}</span>{NATION_NAME}</h3>
                                                      </div>
                                                   </tpl>
                                                </Template>
                                            </ext:ComboBox>
                                        </ext:Anchor>
                                        <ext:Anchor Horizontal="92%">
                                            <ext:ComboBox ID="cboIsOnGuard" runat="server" FieldLabel="在岗否" Editable="false">
                                            </ext:ComboBox>
                                        </ext:Anchor>
                                        <ext:Anchor Horizontal="92%">
                                            <ext:ComboBox ID="cboStation" runat="server" FieldLabel="岗位名称" StoreID="StoreCombo"
                                                DisplayField="STATION_NAME" ValueField="ID" Editable="false" ListWidth="240"
                                                Width="240" />
                                        </ext:Anchor>
                                        <ext:Anchor Horizontal="92%">
                                            <ext:ComboBox ID="cboPerssort" runat="server" FieldLabel="人员类别" LabelStyle="color:blue;"
                                                Editable="false" CausesValidation="true" AllowBlank="false" />
                                        </ext:Anchor>
                                        <ext:Anchor Horizontal="92%">
                                            <ext:ComboBox ID="cboGord" runat="server" StoreID="Store3" DisplayField="DEPT_NAME"
                                                Width="90" ValueField="DEPT_CODE" TypeAhead="false" LoadingText="Searching..."
                                                PageSize="1000" ItemSelector="div.search-item" MinChars="1" FieldLabel="核算组"
                                                ListWidth="240">
                                                <Template ID="Template4" runat="server">
                                       <tpl for=".">
                                          <div class="search-item">
                                             <h3><span style="width:auto">{DEPT_CODE}</span>{DEPT_NAME}</h3>
                                          </div>
                                       </tpl>
                                                </Template>
                                            </ext:ComboBox>
                                        </ext:Anchor>
                                        <ext:Anchor Horizontal="92%">
                                            <ext:DateField ID="dtfContractEnd" runat="server" FieldLabel="合同终止时间" Format="yyyy-MM-dd">
                                            </ext:DateField>
                                        </ext:Anchor>
                                        <ext:Anchor Horizontal="92%">
                                            <ext:ComboBox ID="ComboBox3" runat="server" FieldLabel="是否考勤" LabelStyle="color:blue;"
                                                Editable="false" CausesValidation="true" AllowBlank="false">
                                                <Items>
                                                    <ext:ListItem Text="否" Value="否" />
                                                    <ext:ListItem Text="是" Value="是" />
                                                </Items>
                                            </ext:ComboBox>
                                        </ext:Anchor>
                                    </ext:FormLayout>
                                </Body>
                            </ext:Panel>
                        </ext:LayoutColumn>
                        <ext:LayoutColumn ColumnWidth=".3">
                            <ext:Panel ID="Panel4" runat="server" Border="false" StyleSpec="background-color:Transparent;"
                                BodyStyle="background-color:Transparent;">
                                <Body>
                                    <ext:Image runat="server" Width="210" Height="200" ID="imgStaff" ImageUrl="/resources/UploadPicfile/user_default.png">
                                    </ext:Image>
                                    <div style="padding-left: -10px;">
                                        <ext:FileUploadField ID="photoimg" runat="server" ButtonOnly="true" ButtonText="上传图片"
                                            Icon="ImageAdd" Width="150">
                                            <AjaxEvents>
                                                <FileSelected OnEvent="UploadClick" Before="return checkType();">
                                                </FileSelected>
                                            </AjaxEvents>
                                        </ext:FileUploadField>
                                    </div>
                                </Body>
                            </ext:Panel>
                        </ext:LayoutColumn>
                    </ext:ColumnLayout>
                </Body>
            </ext:FieldSet>
            <ext:FieldSet ID="FieldSet2" runat="server" Width="780" Title="相关信息" StyleSpec="margin:5px;"
                Collapsible="true" Collapsed="false">
                <Body>
                    <ext:Panel ID="Panel345" runat="server" Border="false" StyleSpec="background-color:Transparent;"
                        BodyStyle="background-color:Transparent;">
                        <Body>
                            <ext:ColumnLayout ID="ColumnLayout3" runat="server">
                                <ext:LayoutColumn ColumnWidth=".25">
                                    <ext:Panel ID="Panel5" runat="server" Border="false" Header="false" StyleSpec="background-color:Transparent;"
                                        BodyStyle="background-color:Transparent;">
                                        <Body>
                                            <ext:FormLayout ID="FormLayout2" runat="server" LabelAlign="Left" LabelWidth="78">
                                                <ext:Anchor Horizontal="92%">
                                                    <ext:ComboBox ID="cboSpeciality" runat="server" FieldLabel="所学专业" Editable="false" />
                                                </ext:Anchor>
                                                <ext:Anchor Horizontal="92%">
                                                    <ext:ComboBox ID="cboJobDuty" runat="server" FieldLabel="技术职务" LabelStyle="color:blue;"
                                                        CausesValidation="true" AllowBlank="false" Editable="false" />
                                                </ext:Anchor>
                                                <ext:Anchor Horizontal="92%">
                                                    <ext:ComboBox ID="cboTiptopLearnStuffer" runat="server" FieldLabel="学历" Editable="false" />
                                                </ext:Anchor>
                                                <ext:Anchor Horizontal="92%">
                                                    <ext:ComboBox ID="cboTitle" runat="server" FieldLabel="职称" Editable="false" />
                                                </ext:Anchor>
                                                <ext:Anchor Horizontal="92%">
                                                    <ext:ComboBox ID="cboTitleList" runat="server" FieldLabel="职称序列" LabelStyle="color:blue;"
                                                        CausesValidation="true" AllowBlank="false" Editable="false" />
                                                </ext:Anchor>
                                            </ext:FormLayout>
                                        </Body>
                                    </ext:Panel>
                                </ext:LayoutColumn>
                                <ext:LayoutColumn ColumnWidth=".25">
                                    <ext:Panel ID="Panel6" runat="server" Border="false" StyleSpec="background-color:Transparent;"
                                        BodyStyle="background-color:Transparent;">
                                        <Body>
                                            <ext:FormLayout ID="FormLayout5" runat="server" LabelAlign="Left" LabelWidth="88">
                                                <ext:Anchor Horizontal="92%">
                                                    <ext:TextField ID="txtRetainTerm" runat="server" FieldLabel="受聘期限" />
                                                </ext:Anchor>
                                                <ext:Anchor Horizontal="92%">
                                                    <ext:ComboBox ID="cboDegree" runat="server" FieldLabel="学位" Editable="false" />
                                                </ext:Anchor>
                                                <ext:Anchor Horizontal="92%">
                                                    <ext:ComboBox ID="cboCadreType" runat="server" FieldLabel="干部类别" Editable="false" />
                                                </ext:Anchor>
                                                <ext:Anchor Horizontal="92%">
                                                    <ext:ComboBox ID="cboDeptType" runat="server" FieldLabel="所在科室类" Editable="false" />
                                                </ext:Anchor>
                                                <ext:Anchor Horizontal="92%">
                                                    <ext:ComboBox ID="cboBackboneCircs" runat="server" FieldLabel="专家骨干情况" Editable="false">
                                                        <Items>
                                                            <ext:ListItem Text="医院名医" Value="医院名医" />
                                                            <ext:ListItem Text="博士生导师" Value="博士生导师" />
                                                        </Items>
                                                    </ext:ComboBox>
                                                </ext:Anchor>
                                            </ext:FormLayout>
                                        </Body>
                                    </ext:Panel>
                                </ext:LayoutColumn>
                                <ext:LayoutColumn ColumnWidth=".25">
                                    <ext:Panel ID="Panel7" runat="server" Border="false" StyleSpec="background-color:Transparent;"
                                        BodyStyle="background-color:Transparent;">
                                        <Body>
                                            <ext:FormLayout ID="FormLayout4" runat="server" LabelAlign="Left" LabelWidth="78">
                                                <ext:Anchor Horizontal="92%">
                                                    <ext:ComboBox ID="cmbMaritalStatus" runat="server" FieldLabel="婚姻状况" Editable="false">
                                                        <Items>
                                                            <ext:ListItem Text="已婚" Value="已婚" />
                                                            <ext:ListItem Text="未婚" Value="未婚" />
                                                            <ext:ListItem Text="其他" Value="其他" />
                                                        </Items>
                                                    </ext:ComboBox>
                                                </ext:Anchor>
                                                <ext:Anchor Horizontal="92%">
                                                    <ext:DateField ID="dtfJobDate" runat="server" FieldLabel="技术职务时间" Format="yyyy-MM-dd">
                                                    </ext:DateField>
                                                </ext:Anchor>
                                                <ext:Anchor Horizontal="92%">
                                                    <ext:DateField ID="dtfStudyOverdate" runat="server" FieldLabel="毕业时间" Format="yyyy-MM-dd">
                                                    </ext:DateField>
                                                </ext:Anchor>
                                                <ext:Anchor Horizontal="92%">
                                                    <ext:DateField ID="dtfWorkDate" runat="server" FieldLabel="工作时间" Format="yyyy-MM-dd">
                                                    </ext:DateField>
                                                </ext:Anchor>
                                                <ext:Anchor Horizontal="92%">
                                                    <ext:TextField ID="txtGraduateAcademy" runat="server" FieldLabel="毕业院校" Format="yyyy-MM-dd" />
                                                </ext:Anchor>
                                            </ext:FormLayout>
                                        </Body>
                                    </ext:Panel>
                                </ext:LayoutColumn>
                                <ext:LayoutColumn ColumnWidth=".25">
                                    <ext:Panel ID="Panel8" runat="server" Border="false" StyleSpec="background-color:Transparent;"
                                        BodyStyle="background-color:Transparent;">
                                        <Body>
                                            <ext:FormLayout ID="FormLayout6" runat="server" LabelAlign="Left" LabelWidth="78">
                                                <ext:Anchor Horizontal="92%">
                                                    <ext:TextField ID="txtCredithourPerYear" runat="server" FieldLabel="年平均学分" Hidden="true" />
                                                </ext:Anchor>
                                                <ext:Anchor Horizontal="92%">
                                                    <ext:DateField ID="dtfTitleAssess" runat="server" FieldLabel="资格评定时间" Format="yyyy-MM-dd">
                                                    </ext:DateField>
                                                </ext:Anchor>
                                                <ext:Anchor Horizontal="92%">
                                                    <ext:NumberField ID="txtBonusCoefficient" runat="server" FieldLabel="奖金系数" Hidden="true" />
                                                </ext:Anchor>
                                                <ext:Anchor Horizontal="92%">
                                                    <ext:DateField ID="dtfInHospitalDate" runat="server" FieldLabel="来院时间" Format="yyyy-MM-dd">
                                                    </ext:DateField>
                                                </ext:Anchor>
                                                <ext:Anchor Horizontal="92%">
                                                    <ext:ComboBox ID="cboTechnicTitle" runat="server" FieldLabel="技术资格" Editable="false" />
                                                </ext:Anchor>
                                                <ext:Anchor Horizontal="92%">
                                                    <ext:ComboBox ID="cboGovAllowance" runat="server" FieldLabel="政府津贴" Editable="false">
                                                        <Items>
                                                            <ext:ListItem Text="不享受" Value="不享受" />
                                                            <ext:ListItem Text="享受" Value="享受" />
                                                        </Items>
                                                    </ext:ComboBox>
                                                </ext:Anchor>
                                                <ext:Anchor Horizontal="92%">
                                                    <ext:DateField ID="dtfGradetitleDate" runat="server" FieldLabel="取得学历时间" Format="yyyy-MM-dd">
                                                    </ext:DateField>
                                                </ext:Anchor>
                                            </ext:FormLayout>
                                        </Body>
                                    </ext:Panel>
                                </ext:LayoutColumn>
                            </ext:ColumnLayout>
                        </Body>
                    </ext:Panel>
                    <ext:Panel ID="Panel9" runat="server" Border="false" StyleSpec="background-color:Transparent;"
                        BodyStyle="background-color:Transparent;">
                        <Body>
                            <ext:ColumnLayout ID="ColumnLayout4" runat="server">
                                <ext:LayoutColumn ColumnWidth=".5">
                                    <ext:Panel ID="Panel10" runat="server" Border="false" Header="false" StyleSpec="background-color:Transparent;"
                                        BodyStyle="background-color:Transparent;">
                                        <Body>
                                            <ext:FormLayout ID="FormLayout7" runat="server" LabelWidth="78">
                                                <ext:Anchor>
                                                    <ext:TextField ID="txtCertificateNo" runat="server" Width="280" FieldLabel="证件号码"
                                                        MaxLength="30" />
                                                </ext:Anchor>
                                            </ext:FormLayout>
                                        </Body>
                                    </ext:Panel>
                                </ext:LayoutColumn>
                                <ext:LayoutColumn ColumnWidth=".5">
                                    <ext:Panel ID="Panel11" runat="server" Border="false" Header="false" StyleSpec="background-color:Transparent;"
                                        BodyStyle="background-color:Transparent;">
                                        <Body>
                                            <ext:FormLayout ID="FormLayout8" runat="server" LabelWidth="78">
                                                <ext:Anchor>
                                                    <ext:TextField ID="txtHomeplace" runat="server" Width="280" FieldLabel="地址" MaxLength="100" />
                                                </ext:Anchor>
                                            </ext:FormLayout>
                                        </Body>
                                    </ext:Panel>
                                </ext:LayoutColumn>
                            </ext:ColumnLayout>
                        </Body>
                    </ext:Panel>
                    <ext:Panel ID="Panel19" runat="server" Border="false" StyleSpec="background-color:Transparent;"
                        BodyStyle="background-color:Transparent;">
                        <Body>
                            <ext:ColumnLayout ID="ColumnLayout6" runat="server">
                                <ext:LayoutColumn ColumnWidth=".5">
                                    <ext:Panel ID="Panel20" runat="server" Border="false" Header="false" StyleSpec="background-color:Transparent;"
                                        BodyStyle="background-color:Transparent;">
                                        <Body>
                                            <ext:FormLayout ID="FormLayout14" runat="server" LabelWidth="78">
                                                <ext:Anchor>
                                                    <ext:TextField ID="txtBonusNum" runat="server" Width="280" FieldLabel="银行账号" MaxLength="30" />
                                                </ext:Anchor>
                                            </ext:FormLayout>
                                        </Body>
                                    </ext:Panel>
                                </ext:LayoutColumn>
                            </ext:ColumnLayout>
                        </Body>
                    </ext:Panel>
                    <ext:Panel ID="Panel12" runat="server" Border="false" StyleSpec="background-color:Transparent;"
                        BodyStyle="background-color:Transparent;">
                        <Body>
                            <ext:FormLayout ID="FormLayout9" runat="server" LabelWidth="78">
                                <ext:Anchor>
                                    <ext:TextArea ID="txtMemo" runat="server" Width="660" FieldLabel="备 注" MaxLength="300">
                                    </ext:TextArea>
                                </ext:Anchor>
                            </ext:FormLayout>
                        </Body>
                    </ext:Panel>
                </Body>
            </ext:FieldSet>
            <ext:FieldSet ID="FieldSet3" runat="server" Width="780" Title="军人相关" StyleSpec="margin:5px;"
                Collapsible="true" Collapsed="false">
                <Body>
                    <ext:Panel ID="Panel17" runat="server" Border="false" StyleSpec="background-color:Transparent;"
                        BodyStyle="background-color:Transparent;">
                        <Body>
                            <ext:ColumnLayout ID="ColumnLayout5" runat="server">
                                <ext:LayoutColumn ColumnWidth=".25">
                                    <ext:Panel ID="Panel13" runat="server" Border="false" StyleSpec="background-color:Transparent;"
                                        BodyStyle="background-color:Transparent;">
                                        <Body>
                                            <ext:FormLayout ID="FormLayout10" runat="server" LabelAlign="Left" LabelWidth="78">
                                                <ext:Anchor Horizontal="92%">
                                                    <ext:ComboBox ID="cboIfarmy" runat="server" FieldLabel="是否军人" LabelStyle="color:blue;"
                                                        CausesValidation="true" AllowBlank="false" Editable="false">
                                                        <Items>
                                                            <ext:ListItem Text="是" Value="1" />
                                                            <ext:ListItem Text="否" Value="0" />
                                                        </Items>
                                                    </ext:ComboBox>
                                                </ext:Anchor>
                                                <ext:Anchor Horizontal="92%">
                                                    <ext:DateField ID="dtfBeEnrolledInDate" runat="server" FieldLabel="入伍时间" Format="yyyy-MM-dd">
                                                    </ext:DateField>
                                                </ext:Anchor>
                                            </ext:FormLayout>
                                        </Body>
                                    </ext:Panel>
                                </ext:LayoutColumn>
                                <ext:LayoutColumn ColumnWidth=".25">
                                    <ext:Panel ID="Panel15" runat="server" Border="false" StyleSpec="background-color:Transparent;"
                                        BodyStyle="background-color:Transparent;">
                                        <Body>
                                            <ext:FormLayout ID="FormLayout12" runat="server" LabelAlign="Left" LabelWidth="78">
                                                <ext:Anchor Horizontal="92%">
                                                    <ext:ComboBox ID="cboCivilServiceClass" runat="server" FieldLabel="文职级" Editable="false" />
                                                </ext:Anchor>
                                                <ext:Anchor Horizontal="92%">
                                                    <ext:DateField ID="dtfCivilServiceClassDate" runat="server" FieldLabel="文职级时间" Format="yyyy-MM-dd">
                                                    </ext:DateField>
                                                </ext:Anchor>
                                            </ext:FormLayout>
                                        </Body>
                                    </ext:Panel>
                                </ext:LayoutColumn>
                                <ext:LayoutColumn ColumnWidth=".25">
                                    <ext:Panel ID="Panel16" runat="server" Border="false" StyleSpec="background-color:Transparent;"
                                        BodyStyle="background-color:Transparent;">
                                        <Body>
                                            <ext:FormLayout ID="FormLayout13" runat="server" LabelAlign="Left" LabelWidth="78">
                                                <ext:Anchor Horizontal="92%">
                                                    <ext:ComboBox ID="ComboBox1" runat="server" FieldLabel="文职级" Editable="false" />
                                                </ext:Anchor>
                                                <ext:Anchor Horizontal="92%">
                                                    <ext:DateField ID="DateField1" runat="server" FieldLabel="离职时间" Format="yyyy-MM-dd">
                                                    </ext:DateField>
                                                </ext:Anchor>
                                            </ext:FormLayout>
                                        </Body>
                                    </ext:Panel>
                                </ext:LayoutColumn>
                                <ext:LayoutColumn ColumnWidth=".25">
                                    <ext:Panel ID="Panel14" runat="server" Border="false" StyleSpec="background-color:Transparent;"
                                        BodyStyle="background-color:Transparent;">
                                        <Body>
                                            <ext:FormLayout ID="FormLayout11" runat="server" LabelAlign="Left" LabelWidth="78">
                                                <ext:Anchor Horizontal="92%">
                                                    <ext:ComboBox ID="cboTechnicClass" runat="server" FieldLabel="技术级" Editable="false" />
                                                </ext:Anchor>
                                                <ext:Anchor Horizontal="92%">
                                                    <ext:DateField ID="dtfTechnicClassDate" runat="server" FieldLabel="技术级时间" Format="yyyy-MM-dd">
                                                    </ext:DateField>
                                                </ext:Anchor>
                                            </ext:FormLayout>
                                        </Body>
                                    </ext:Panel>
                                </ext:LayoutColumn>
                            </ext:ColumnLayout>
                        </Body>
                    </ext:Panel>
                    <ext:Panel ID="Panel18" runat="server" Border="false" StyleSpec="background-color:Transparent;"
                        BodyStyle="background-color:Transparent;">
                        <Body>
                            <ext:ColumnLayout ID="ColumnLayout7" runat="server">
                                <ext:LayoutColumn ColumnWidth=".5">
                                    <ext:Panel ID="Panel21" runat="server" Border="false" Header="false" StyleSpec="background-color:Transparent;"
                                        BodyStyle="background-color:Transparent;">
                                        <Body>
                                            <ext:FormLayout ID="FormLayout16" runat="server" LabelWidth="78">
                                                <ext:Anchor>
                                                    <ext:TextField ID="txtMedicardmark" runat="server" Width="280" FieldLabel="医疗卡账号"
                                                        MaxLength="30" />
                                                </ext:Anchor>
                                            </ext:FormLayout>
                                        </Body>
                                    </ext:Panel>
                                </ext:LayoutColumn>
                                <ext:LayoutColumn ColumnWidth=".5">
                                    <ext:Panel ID="Panel22" runat="server" Border="false" Header="false" StyleSpec="background-color:Transparent;"
                                        BodyStyle="background-color:Transparent;">
                                        <Body>
                                            <ext:FormLayout ID="FormLayout17" runat="server" LabelWidth="78">
                                                <ext:Anchor>
                                                    <ext:TextField ID="txtMediCard" runat="server" Width="280" FieldLabel="医疗卡号" />
                                                </ext:Anchor>
                                            </ext:FormLayout>
                                        </Body>
                                    </ext:Panel>
                                </ext:LayoutColumn>
                            </ext:ColumnLayout>
                        </Body>
                    </ext:Panel>
                </Body>
            </ext:FieldSet>
        </Body>
        <BottomBar>
            <ext:Toolbar ID="Toolbar2" runat="server">
                <Items>
                    <ext:ToolbarFill ID="ToolbarFill2" runat="server" />
                    <ext:ToolbarButton ID="Btn_BatStart" runat="server" Icon="Disk" Text="保存">
                        <AjaxEvents>
                            <Click OnEvent="SaveInfo" Before="if (CheckForm()== false){ Ext.Msg.alert('系统提示','请根据红线提示填写正确的信息！');return false;};">
                                <ExtraParams>
                                    <ext:Parameter Name="index" Value="Ext.encode(#{txtStaffInput}.selectedIndex)" Mode="Raw">
                                    </ext:Parameter>
                                </ExtraParams>
                            </Click>
                        </AjaxEvents>
                    </ext:ToolbarButton>
                    <ext:ToolbarButton ID="btnSetSave" runat="server" Icon="Disk" Text="修改">
                        <AjaxEvents>
                            <Click OnEvent="SetSave" Before="if (CheckForm()== false){ Ext.Msg.alert('系统提示','请根据红线提示填写正确的信息！');return false;};">
                                <ExtraParams>
                                    <ext:Parameter Name="index" Value="Ext.encode(#{txtStaffInput}.selectedIndex)" Mode="Raw">
                                    </ext:Parameter>
                                    <ext:Parameter Name="deptCode" Value="#{Store1}.getAt(RowIndex).data.DEPT_CODE" Mode="Raw">
                                    </ext:Parameter>
                                    <ext:Parameter Name="UserId" Value="#{Store1}.getAt(RowIndex).data.USER_ID" Mode="Raw">
                                    </ext:Parameter>
                                    <ext:Parameter Name="deptName" Value="#{Store1}.getAt(RowIndex).data.DEPT_NAME" Mode="Raw">
                                    </ext:Parameter>
                                </ExtraParams>
                            </Click>
                        </AjaxEvents>
                    </ext:ToolbarButton>
                    <ext:ToolbarButton ID="btnSaveUp" runat="server" Icon="Disk" Text="提交">
                        <AjaxEvents>
                            <Click OnEvent="SaveUpInfo" Before="if (CheckForm()== false){ Ext.Msg.alert('系统提示','请根据红线提示填写正确的信息！');return false;};">
                                <ExtraParams>
                                    <ext:Parameter Name="index" Value="Ext.encode(#{txtStaffInput}.selectedIndex)" Mode="Raw">
                                    </ext:Parameter>
                                </ExtraParams>
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
    <ext:Window ID="jsda_Detail" runat="server" Icon="Group" Title="技术档案" Width="825"
        Height="500" AutoShow="false" Modal="true" CenterOnLoad="true" AutoScroll="true"
        ShowOnLoad="false" Resizable="true" StyleSpec="background-color:Transparent;"
        BodyStyle="background-color:Transparent;">
    </ext:Window>
    <ext:Window ID="staffinfodetail" runat="server" Icon="Group" Title="人员信息" Width="825"
        Height="500" AutoShow="false" Modal="true" CenterOnLoad="true" AutoScroll="true"
        ShowOnLoad="false" Resizable="true" StyleSpec="background-color:Transparent;"
        BodyStyle="background-color:Transparent;" Maximizable="true" MinHeight="10" MinWidth="10"
        MinButtonWidth="10" MonitorResize="true">
    </ext:Window>
    </form>
</body>
</html>
