using System;
using System.Data;
using GoldNet.Comm;
using Goldnet.Ext.Web;
using Goldnet.Dal;
using GoldNet.Model;
using GoldNet.Comm.DAL.Oracle;

namespace GoldNet.JXKP.RLZY.BaseInfoMaintain
{
    public partial class StaffInfodetail : PageBase
    {
        BaseInfoManagerDal dal = new BaseInfoManagerDal();
        BaseInfoMaintainDal tdal = new BaseInfoMaintainDal();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Ext.IsAjaxRequest)
            {
                string staffid = "";
                if (Request["staffid"] != null)
                {
                    staffid = Request["staffid"].ToString();
                }
                string deptcode = this.DeptFilter("", Request["pageid"].ToString());
                HttpProxy proxy = new HttpProxy();
                proxy.Method = HttpMethod.POST;
                proxy.Url = "/RLZY/WebService/DeptInfo.ashx?deptfilter=" + deptcode;
                this.Store3.Proxy.Add(proxy);
                HttpProxy proxystaff = new HttpProxy();
                proxystaff.Method = HttpMethod.POST;
                proxystaff.Url = "/RLZY/WebService/UserInfos.ashx?staff_id=" + staffid;
                this.Store4.Proxy.Add(proxystaff);
                SetCombox();
                if (Request["staffid"] != null)
                {
                    setdata(Request["staffid"].ToString());
                }
            }
            if (this.IsPass(Request["pageid"].ToString()))
            {
                Btn_BatStart.Text = "审核";
                this.ToolbarButton1.Hidden = true;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="staffid"></param>
        private void setdata(string staffid)
        {
            DataTable td = tdal.GetStaffList(staffid);
            if (td.Rows.Count > 0)
            {
                if (td.Rows[0]["USER_ID"].ToString() == "")
                {
                    cboInptName.Checked = false;
                }
                else
                {
                    cboInptName.Checked = true;
                }
                this.DeptCodeCombo.SelectedItem.Value = td.Rows[0]["DEPT_CODE"].ToString();                  // 科室
                //txtStaffInput.SelectedItem.Value = td.Rows[0]["user_id"].ToString();
                txtStaffInput.SelectedIndex = 0;
                dtfBirthday.Value = td.Rows[0]["BIRTHDAY"];                                                  // 出生年月
                cboSex.SelectedItem.Value = td.Rows[0]["sex"].ToString();                                    // 性别
                cboDuty.SelectedItem.Value = td.Rows[0]["duty"].ToString();                                  // 行政职务
                cboIfarmy.SelectedItem.Value = td.Rows[0]["if_army"].ToString();                             // 是否军人
                cboSanispecsort.SelectedItem.Value = td.Rows[0]["SANTSPECSORT"].ToString();                  // 卫生专业分类
                txtMedicardmark.Value = td.Rows[0]["MEDICARDMARK"].ToString();                               // 医疗卡账号
                cboBraid.SelectedItem.Value = td.Rows[0]["ISBRAID"].ToString();                              // 实虚编
                cboRank.SelectedItem.Text = td.Rows[0]["rank"].ToString();                                   // 级别 
                dtfDutydate.Value = td.Rows[0]["DUTYDATE"].ToString();                                       // 行政职务时间 
                cboPeople.SelectedItem.Value = td.Rows[0]["NATIONALS"].ToString();                           // 民族
                cboIsOnGuard.SelectedItem.Value = td.Rows[0]["ISONGUARD"].ToString();                        // 在岗否
                cboPerssort.SelectedItem.Value = td.Rows[0]["STAFFSORT"].ToString();                         // 人员类别
                cboRootspecsort.SelectedItem.Value = td.Rows[0]["ROOTSPECSORT"].ToString();                  // 从事专业
                txtMediCard.Value = td.Rows[0]["MEDICARD"].ToString();                                       // 医疗卡号
                //HiddenImage.setValue(record.data.IMG_ID);
                if (td.Rows[0]["IMG_ID"].ToString() == "")
                {
                    imgStaff.ImageUrl = "/resources/UploadPicfile/user_default.png";                         //图片
                }
                else
                {
                    imgStaff.ImageUrl = "/resources/UploadPicfile/" + td.Rows[0]["IMG_ID"].ToString();       // 图片
                }
                cboSpeciality.SelectedItem.Value = td.Rows[0]["STUDY_SPECSORT"].ToString();                  // 所学专业
                cboJobDuty.SelectedItem.Value = td.Rows[0]["JOB"].ToString();                                // 技术职务
                cboCivilServiceClass.SelectedItem.Value = td.Rows[0]["CIVILSERVICECLASS"].ToString();        // 文职级
                cboTiptopLearnStuffer.SelectedItem.Value = td.Rows[0]["TOPEDUCATE"].ToString();              // 学历
                cboTitle.SelectedItem.Text = td.Rows[0]["TITLE"].ToString();                                 // 职称
                cboTitleList.SelectedItem.Value = td.Rows[0]["TITLE_LIST"].ToString();                       // 职称序列
                cboTechnicTitle.SelectedItem.Value = td.Rows[0]["JOB_TITLE"].ToString();                     // 技术资格
                txtRetainTerm.Value = td.Rows[0]["RETAINTERM"].ToString();                                   // 受聘期限
                cboTechnicClass.SelectedItem.Value = td.Rows[0]["TECHINCCLASS"].ToString();                  // 技术级
                cboDegree.SelectedItem.Value = td.Rows[0]["EDU1"].ToString();                                // 学位
                cboCadreType.SelectedItem.Value = td.Rows[0]["CADRES_CATEGORIES"].ToString();                // 干部类别
                cboDeptType.SelectedItem.Value = td.Rows[0]["DEPT_TYPE"].ToString();                         // 所在科室类
                cboBackboneCircs.SelectedItem.Value = td.Rows[0]["EXPERT"].ToString();                       // 专家骨干情况
                cboGovAllowance.SelectedItem.Value = td.Rows[0]["GOVERNMENT_ALLOWANCE"].ToString();          // 政府津贴
                cmbMaritalStatus.SelectedItem.Value = td.Rows[0]["MARITAL_STATUS"].ToString();               // 婚姻状况
                dtfJobDate.Value = td.Rows[0]["JOBDATE"].ToString();                                         // 技术职务时间
                dtfCivilServiceClassDate.Value = td.Rows[0]["CIVILSERVICECLASSDATE"].ToString();             // 文职级时间
                dtfStudyOverdate.Value = td.Rows[0]["STUDY_OVER_DATE"].ToString();                           // 毕业时间
                dtfWorkDate.Value = td.Rows[0]["WORKDATE"].ToString();                                       // 工作时间
                txtGraduateAcademy.Value = td.Rows[0]["GRADUATE_ACADEMY"].ToString();                        // 毕业院校
                dtfGradetitleDate.Value = td.Rows[0]["DATE_OF_GRADETITLE"].ToString();                       // 取得学历时间
                txtCredithourPerYear.Value = td.Rows[0]["CREDITHOUR_PERYEAR"].ToString();                    // 年平均学分
                dtfTechnicClassDate.Value = td.Rows[0]["TECHNICCLASSDATE"].ToString();                       // 技术级时间
                dtfTitleAssess.Value = td.Rows[0]["TITLE_DATE"].ToString();                                  // 资格评定时间
                dtfBeEnrolledInDate.Value = td.Rows[0]["BEENROLLEDINDATE"].ToString();                       // 入伍时间
                txtBonusCoefficient.Value = td.Rows[0]["BONUS_FACTOR"].ToString() == "" ? 0 : Double.Parse(td.Rows[0]["BONUS_FACTOR"].ToString());           // 奖金系数
                dtfInHospitalDate.Value = td.Rows[0]["INHOSPITALDATE"].ToString();                           // 来院时间
                txtCertificateNo.Value = td.Rows[0]["CERTIFICATE_NO"].ToString();                            // 证件号码
                txtHomeplace.Value = td.Rows[0]["HOMEPLACE"].ToString();                                     // 出生地点
                txtMemo.Value = td.Rows[0]["MEMO"].ToString();                                               // 备 注
                cboStation.SelectedItem.Value = td.Rows[0]["STATION_CODE"].ToString();                       // 岗位名称
                cboGord.SelectedItem.Value = td.Rows[0]["GORD_CODE"].ToString();                             // 核算组
                empno.Value = td.Rows[0]["EMP_NO"].ToString();                                               // 人员工号
                this.DateField1.Value = td.Rows[0]["TURNOVER_TIME"].ToString();                              // 离职时间

                ComboBox2.SelectedItem.Value = td.Rows[0]["BONUS_FLAG"].ToString();
                ComboBox3.SelectedItem.Value = td.Rows[0]["CHECK_FLAG"].ToString();

                if (td.Rows[0]["CONTRACT_START"] != null)
                {
                    this.dtfContractStart.Value = td.Rows[0]["CONTRACT_START"].ToString();
                }
                if (td.Rows[0]["CONTRACT_END"] != null)
                {
                    this.dtfContractEnd.Value = td.Rows[0]["CONTRACT_END"].ToString();
                }
                if (td.Rows[0]["BANK_CODE"] != null)
                {
                    this.txtBonusNum.Text = td.Rows[0]["BANK_CODE"].ToString();
                }

                //加载岗位字典
                DictMainTainDal dictdal = new DictMainTainDal();
                DataTable l_dt1 = dictdal.getStationNameInfo(td.Rows[0]["DEPT_CODE"].ToString()).Tables[0];
                if (l_dt1.Rows.Count > 0)
                {
                    this.StoreCombo.DataSource = l_dt1;
                    this.StoreCombo.DataBind();
                }
            }
        }

        /// <summary>
        /// 初始化Combox
        /// </summary>
        private void SetCombox()
        {
            DictMainTainDal dictdal = new DictMainTainDal();
            DataTable cboDt = null;

            //等级字典信息
            DataTable l_dt = PersTypeFilter();
            //if (l_dt.Rows.Count > 0)
            //{
            //    cboPersonType.Items.Add(new Goldnet.Ext.Web.ListItem("全部", "全部"));
            //}
            //SetCboData("", "", l_dt, cboPersonType);
            //if (l_dt.Rows.Count > 0)
            //{
            //    cboPersonType.SelectedIndex = 0;
            //}

            SetCboData("", "", l_dt, cboPerssort);
            //cboPerssort.SelectedIndex = 0;

            //加载职称字典,技术资格
            cboDt = dictdal.getDictInfo("JOB_DICT", "ID", "JOB", true, "IS_DEL").Tables[0];
            SetCboData("JOB", "JOB", cboDt, cboJobDuty);
            //cboJobDuty.SelectedIndex = 0;
            SetCboData("JOB", "JOB", cboDt, cboTechnicTitle);
            //cboTechnicTitle.SelectedIndex = 0;

            //加载级别
            cboDt = dictdal.getDictInfo("RANK", "ID", "RANKS", false, "").Tables[0];
            SetCboData("RANKS", "RANKS", cboDt, cboRank);
            // cboRank.SelectedIndex = 0;

            //加载职务字典
            cboDt = dictdal.getDictInfo("DUTY_DICT", "ID", "DUTY", true, "IS_DEL").Tables[0];
            SetCboData("DUTY", "DUTY", cboDt, cboDuty);
            //cboDuty.SelectedIndex = 0;

            //在岗状态
            cboDt = dictdal.getDictInfo("ONGUARD", "ID", "STATS", false, "").Tables[0];
            SetCboData("STATS", "STATS", cboDt, cboIsOnGuard);
            //cboIsOnGuard.SelectedIndex = 0;

            //所学专业
            cboDt = dictdal.getDictInfo("STUDYSPEECSORT", "ID", "SPEECSORT", false, "").Tables[0];
            SetCboData("SPEECSORT", "SPEECSORT", cboDt, cboSpeciality);
            //cboSpeciality.SelectedIndex = 0;

            //干部
            cboDt = dictdal.getDictInfo("CADRES_CATEGORIES", "ID", "CADRES_TYPE", false, "").Tables[0];
            SetCboData("CADRES_TYPE", "CADRES_TYPE", cboDt, cboCadreType);
            //cboCadreType.SelectedIndex = 0;

            //科室类
            cboDt = dictdal.getDictInfo("DEPT_TYPE", "ID", "DEPTTYPE", false, "").Tables[0];
            SetCboData("DEPTTYPE", "DEPTTYPE", cboDt, cboDeptType);
            //cboDeptType.SelectedIndex = 0;

            //加载技术级字典
            cboDt = dictdal.getDictInfo("TECHNICCLASS_DICT", "ID", "TECHNICCLASS", true, "IS_DEL").Tables[0];
            SetCboData("TECHNICCLASS", "TECHNICCLASS", cboDt, cboTechnicClass);
            // cboTechnicClass.SelectedIndex = 0;

            //加载文职级字典
            cboDt = dictdal.getDictInfo("CIVILSERVICECLASS_DICT", "ID", "CIVILSERVICECLASS", true, "IS_DEL").Tables[0];
            SetCboData("CIVILSERVICECLASS", "CIVILSERVICECLASS", cboDt, cboCivilServiceClass);
            //cboCivilServiceClass.SelectedIndex = 0;

            //加载最高学历字典
            cboDt = dictdal.getDictInfo("LEARNSUFFER_DICT", "ID", "LEARNSUFFER", true, "IS_DEL").Tables[0];
            SetCboData("LEARNSUFFER", "LEARNSUFFER", cboDt, cboTiptopLearnStuffer);
            //cboTiptopLearnStuffer.SelectedIndex = 0;

            //加载最高学位字典
            cboDt = dictdal.getDictInfo("DEGREE_DICT", "ID", "DEGREE", true, "IS_DEL").Tables[0];
            SetCboData("DEGREE", "DEGREE", cboDt, cboDegree);
            //cboDegree.SelectedIndex = 0;

            //人员职称序列字典
            cboDt = dictdal.getDictInfo("TITLE_LIST_DICT", "SERIAL_NO", "TITLE_LIST_NAME", false, "").Tables[0];
            SetCboData("TITLE_LIST_NAME", "TITLE_LIST_NAME", cboDt, cboTitleList);
            //cboTitleList.SelectedIndex = 0;

            //职称
            cboDt = dictdal.getDictInfo("TITLE_DICT", "SERIAL_NO", "TITLE_NAME", false, "").Tables[0];
            SetCboData("TITLE_NAME", "TITLE_NAME", cboDt, cboTitle);
            // cboTitle.SelectedIndex = 0;

            //加载卫生专业分类字典
            cboDt = dictdal.getDictInfo("SANI_SPEC_SORT_DICT", "SERIAL_NO", "SORT_NAME", false, "").Tables[0];
            SetCboData("SORT_NAME", "SORT_NAME", cboDt, cboSanispecsort);
            //cboSanispecsort.SelectedIndex = 0;

            //加载从事专业分类字典
            cboDt = dictdal.getDictInfo("PERS_SPEC_SORT_DICT", "SERIAL_NO", "SPEC_SORT_NAME", false, "").Tables[0];
            SetCboData("SPEC_SORT_NAME", "SPEC_SORT_NAME", cboDt, cboRootspecsort);
            //cboRootspecsort.SelectedIndex = 0;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="text"></param>
        /// <param name="dtSource"></param>
        /// <param name="cbo"></param>
        private void SetCboData(string ID, string text, DataTable dtSource, ComboBox cbo)
        {
            if (dtSource.Rows.Count < 1)
            {
                return;
            }
            for (int idx = 0; idx < dtSource.Rows.Count; idx++)
            {
                cbo.Items.Add(new Goldnet.Ext.Web.ListItem(dtSource.Rows[idx]["NAME"].ToString(), dtSource.Rows[idx]["ID"].ToString()));
            }
        }

        /// <summary>
        /// 保存按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void SaveInfo(object sender, AjaxEventArgs e)
        {
            string mark = "3";
            if (this.IsPass(Request["pageid"].ToString()))
                mark = "1";

            ToolbarButton tb = (ToolbarButton)sender;
            if (tb.Text == "提交")
                mark = "0";
            int values = cboInptName.Checked == true ? this.txtStaffInput.SelectedItem.Value.Length : 0;
            string usernames = this.txtStaffInput.SelectedItem.Text;
            string CreaterName = ((User)Session["CURRENTSTAFF"]).UserName;
            string markname = mark == "1" ? CreaterName : "";
            string Creater = ((User)Session["CURRENTSTAFF"]).UserId == null ? "NotUserId" : ((User)Session["CURRENTSTAFF"]).UserId;

            string[] filename = this.photoimg.PostedFile.FileName.ToString().Split('\\');
            string[] file_name = this.imgStaff.ImageUrl.Split('/');
            string imagid = file_name[file_name.Length - 1];
            if (filename.Length != 0 && !filename[0].Equals(""))
            {
                string fpath = @"/resources/UploadPicTemp/" + Creater + "temp" + filename[filename.Length - 1].Substring(filename[filename.Length - 1].LastIndexOf("."));
                //保存人员相应图片
                if (System.IO.File.Exists(Server.MapPath(fpath)))
                {
                    string staffid = OracleOledbBase.GetMaxID("STAFF_ID", DataUser.RLZY + ".NEW_STAFF_INFO").ToString();
                    if (Request["staffid"] != null)
                    {
                        staffid = Request["staffid"].ToString();
                    }
                    string imageName = staffid + filename[filename.Length - 1].Substring(filename[filename.Length - 1].LastIndexOf("."));
                    if (System.IO.File.Exists(Server.MapPath(@"/resources/UploadPicfile/" + imageName)))
                    {
                        System.IO.File.Delete(Server.MapPath(@"/resources/UploadPicfile/" + imageName));
                    }
                    System.IO.File.Move(Server.MapPath(fpath), Server.MapPath(@"/resources/UploadPicfile/" + imageName));
                    if (System.IO.File.Exists(Server.MapPath(fpath)))
                    {
                        System.IO.File.Delete(Server.MapPath(fpath));
                    }
                    imagid = imageName;
                }
            }

            string DB_USER = "";

            if (Request["staffid"] == null)
            {
                if (tdal.isExStaffInfoByName(this.DeptCodeCombo.SelectedItem.Value, this.txtStaffInput.SelectedItem.Text))
                {
                    this.ShowMessage("系统提示", "本科室人员姓名不能重复！");
                    return;
                }
            }
            else
            {
                if (tdal.isExStaffInfoByName(this.DeptCodeCombo.SelectedItem.Value, this.txtStaffInput.SelectedItem.Text, Request["staffid"].ToString()))
                {
                    this.ShowMessage("系统提示", "本科室人员姓名不能重复！");
                    return;
                }
            }

            string isAmry = GetConfig.GetConfigString("isAmry");
            if (isAmry == "2")
            {
                this.cboIfarmy.SelectedItem.Value = "0";

            }

            DB_USER = tdal.getHisDataDbUserByUserid(this.txtStaffInput.SelectedItem.Value);

            string contractstart = "";
            string contractend = "";

            if (dtfContractStart.SelectedDate.ToString("yyyy") != "0001")
            {
                contractstart = dtfContractStart.SelectedDate.ToString("yyyy-MM-dd");
            }

            if (dtfContractEnd.SelectedDate.ToString("yyyy") != "0001")
            {
                contractend = dtfContractEnd.SelectedDate.ToString("yyyy-MM-dd");
            }

            StaffInfo info = new StaffInfo(this.empno.Text.Trim(), this.cboPeople.SelectedItem.Text, this.txtBonusCoefficient.Text, this.cboGovAllowance.SelectedItem.Text,
                                            this.cboCadreType.SelectedItem.Text, TimeConvert(this.dtfStudyOverdate),
                                            this.cboDeptType.SelectedItem.Text, this.cboTiptopLearnStuffer.SelectedItem.Text, this.cboSpeciality.SelectedItem.Text,
                                            this.DeptCodeCombo.SelectedItem.Value, this.DeptCodeCombo.SelectedItem.Text, usernames, this.cboIsOnGuard.SelectedItem.Text,
                                            this.dtfBirthday.SelectedDate.ToString("yyyy-MM-dd"), this.cboSex.SelectedItem.Text, TimeConvert(this.dtfInHospitalDate),
                                            "0", this.txtRetainTerm.Text, this.cboIfarmy.SelectedItem.Value, this.cboJobDuty.SelectedItem.Text, TimeConvert(this.dtfJobDate),
                                            this.cboPerssort.SelectedItem.Text, TimeConvert(this.dtfBeEnrolledInDate), TimeConvert(this.dtfWorkDate),
                                            this.cboDuty.SelectedItem.Text, TimeConvert(this.dtfDutydate), this.cboTechnicClass.SelectedItem.Text,
                                            TimeConvert(this.dtfTechnicClassDate), this.cboCivilServiceClass.SelectedItem.Text, TimeConvert(this.dtfCivilServiceClassDate),
                                            this.cboSanispecsort.SelectedItem.Text, this.cboRootspecsort.SelectedItem.Text, this.txtMedicardmark.Text, this.txtMediCard.Text, CreaterName,
                                            DateTime.Now.ToString("yyyyMMdd"), Convert.ToDateTime(System.DateTime.Now.Year.ToString() + "-" + System.DateTime.Now.Month + "-01").ToString("yyyyMMdd"), this.txtHomeplace.Text,
                                            this.txtCertificateNo.Text, this.cmbMaritalStatus.SelectedItem.Text, this.cboTitleList.SelectedItem.Text,
                                            this.cboDegree.SelectedItem.Text, this.txtGraduateAcademy.Text, TimeConvert(this.dtfGradetitleDate),
                                            this.cboRank.SelectedItem.Text, this.cboTitle.SelectedItem.Text, "",
                                            this.txtMemo.Text, markname, usernames, "", pinyin.GetChineseSpell(usernames), this.cboTechnicTitle.SelectedItem.Text,
                                            TimeConvert(this.dtfTitleAssess), this.cboBackboneCircs.SelectedItem.Text, imagid, this.txtCredithourPerYear.Text,
                                            "", this.cboStation.SelectedItem.Value, cboGord.SelectedItem.Value, contractstart, contractend, ComboBox2.SelectedItem.Text, ComboBox3.SelectedItem.Text, this.txtBonusNum.Text);

            if (Request["staffid"] == null)
                tdal.InsertStaffInfo(info, mark, "", "", this.cboBraid.SelectedItem.Value, usernames, this.txtStaffInput.SelectedItem.Value.ToString(), TimeConvert(this.DateField1));
            else
                tdal.UpdateStaffInfo(Request["staffid"].ToString(), info, mark, "", "", this.cboBraid.SelectedItem.Value, usernames, this.txtStaffInput.SelectedItem.Value.ToString(), pinyin.GetChineseSpell(usernames), TimeConvert(this.DateField1));
            Goldnet.Ext.Web.ScriptManager scManager = Goldnet.Ext.Web.ScriptManager.GetInstance(this.Page);
            scManager.AddScript("parent.Store1.reload();");
            scManager.AddScript("parent.staffinfodetail.hide();");
        }

        /// <summary>
        /// 上传照片
        /// </summary>
        protected void UploadClick(object sender, AjaxEventArgs e)
        {
            System.Web.HttpFileCollection _files = System.Web.HttpContext.Current.Request.Files;
            try
            {
                string Creater = ((User)Session["CURRENTSTAFF"]).UserId == null ? "NotUserId" : ((User)Session["CURRENTSTAFF"]).UserId;
                string[] filename = this.photoimg.PostedFile.FileName.ToString().Split('\\');
                string fpath = @"/resources/UploadPicTemp/" + Creater + "temp" + filename[filename.Length - 1].Substring(filename[filename.Length - 1].LastIndexOf("."));
                if (System.IO.File.Exists(Server.MapPath(fpath)))
                {
                    System.IO.File.Delete(Server.MapPath(fpath));
                }
                photoimg.PostedFile.SaveAs(Server.MapPath(fpath));          //执行上传操作

                this.imgStaff.ImageUrl = fpath + "?temp=" + DateTime.Now.ToString();

                Goldnet.Ext.Web.Ext.Msg.Alert("照片上传成功！", photoimg.PostedFile.FileName).Show();
            }
            catch
            {
                Goldnet.Ext.Web.Ext.Msg.Alert("提示", "该照片过大或不存在！").Show();
            }
        }

        /// <summary>
        /// 时间控件返回值设定
        /// </summary>
        /// <param name="timeConctrl">时间控件</param>
        /// <returns></returns>
        private string TimeConvert(DateField timeConctrl)
        {
            if (timeConctrl.SelectedValue == null)
            {
                return "";
            }
            else
            {
                return timeConctrl.SelectedDate.ToString("yyyy-MM-dd");
            }
        }

    }
}
