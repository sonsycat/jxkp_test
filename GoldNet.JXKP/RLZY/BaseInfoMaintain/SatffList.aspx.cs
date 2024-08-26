using System;
using System.Collections.Generic;
using System.Data;
using Goldnet.Dal;
using Goldnet.Ext.Web;
using GoldNet.Comm;
using GoldNet.Comm.DAL.Oracle;
using GoldNet.Comm.ExportData;
using GoldNet.Model;
using System.IO;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;

namespace GoldNet.JXKP.RLZY.BaseInfoMaintain
{
    public partial class SatffList : PageBase
    {
        BaseInfoManagerDal dal = new BaseInfoManagerDal();
        BaseInfoMaintainDal tdal = new BaseInfoMaintainDal();
        //1:具有审批权限的人登入,2:普通用户进入
        private static int PowerPageInfo = -1;
        private static string add_mark = null;

        /// <summary>
        /// 初始化处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            //检查是否已经登录，否则停止
            if (Session["CURRENTSTAFF"] == null)
            {
                Response.End();
            }

            if (!Ext.IsAjaxRequest)
            {
                bool isPass = this.IsPass();
                bool isEdit = this.IsEdit();
                PowerPageInfo = isPass ? 1 : 2;
                this.hiddenEdit.Text = isEdit ? "1" : "2";
                if (PowerPageInfo == 1)
                {
                    if (!isEdit)
                    {
                        ScriptManager1.AddScript("#{btn_Delete}.hide();#{btn_Add}.hide();#{btnCommand}.hide();#{btn_EchoHandle}.hide();");
                        this.hiddenEdit.Text = "2";
                    }
                }
                else
                {
                    if (!isEdit)
                    {
                        ScriptManager1.AddScript("#{btn_Delete}.hide();#{btnCommand}.hide();#{btn_Add}.hide();#{btn_EchoHandle}.hide();#{cbxOpration}.hide();");
                    }
                    else
                    {
                        //ScriptManager1.AddScript("#{btn_Delete}.hide();#{btnCommand}.hide();#{btn_Add}.hide();#{btn_EchoHandle}.hide();");
                    }
                }
                this.PowerInfoHidden.Value = PowerPageInfo.ToString();

                string deptcode = this.DeptFilter("");

                HttpProxy proxy = new HttpProxy();
                proxy.Method = HttpMethod.POST;
                proxy.Url = "/RLZY/WebService/DeptAccount.ashx?deptfilter=" + deptcode;
                this.Store3.Proxy.Add(proxy);

                string isAmry = GetConfig.GetConfigString("isAmry");

                if (isAmry == "2")
                {
                    hiddenIsAmry.Text = "2";
                    ScriptManager1.AddScript("#{cboIfarmy}.hide();");
                }
                else
                {
                    FieldSet3.Show();
                }

                InitContrl();
                SetCombox();
                add_mark = PowerPageInfo == 2 ? "1" : "0";

                string sort = "";
                if (GetConfig.GetConfigString("Hospital").ToUpper() != "CA")
                {
                    DataTable l_dt = PersTypeFilter();
                    for (int i = 0; i < l_dt.Rows.Count; i++)
                    {
                        sort = sort + "'" + l_dt.Rows[i]["NAME"] + "',";
                    }
                    if (l_dt.Rows.Count == 0)
                    {
                        sort = "'-1'";
                    }
                }
                string contracttime = "";
                if (endtime.SelectedDate.ToString("yyyy") != "0001")
                {
                    contracttime = endtime.SelectedDate.ToString("yyyyMM");
                }

                string deptcodes = ((User)Session["CURRENTSTAFF"]).AccountDeptCode;

                this.DeptCodeCombo.Value = deptcodes;

                DataTable dt = tdal.ViewStaffList(deptcodes, "1", this.DeptFilter(""), sort.TrimEnd(new char[] { ',' }), this.ComboBoxonguard.SelectedItem.Text, Combosex.SelectedItem.Text, contracttime).Tables[0];
                this.Store1.DataSource = dt;
                this.Store1.DataBind();
                this.labConut.Text = "人数为" + dt.Rows.Count + "人";
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
            if (l_dt.Rows.Count > 0)
            {
                cboPersonType.Items.Add(new Goldnet.Ext.Web.ListItem("全部", "全部"));
            }
            SetCboData("", "", l_dt, cboPersonType);
            if (l_dt.Rows.Count > 0)
            {
                cboPersonType.SelectedIndex = 0;
            }

            SetCboData("", "", l_dt, cboPerssort);
            cboPerssort.SelectedIndex = 0;

            //加载职称字典,技术资格
            cboDt = dictdal.getDictInfo("JOB_DICT", "ID", "JOB", true, "IS_DEL").Tables[0];
            SetCboData("ID", "JOB", cboDt, cboJobDuty);
            cboJobDuty.SelectedIndex = 0;
            SetCboData("ID", "JOB", cboDt, cboTechnicTitle);
            cboTechnicTitle.SelectedIndex = 0;

            //加载级别
            cboDt = dictdal.getDictInfo("RANK", "ID", "RANKS", false, "").Tables[0];
            SetCboData("ID", "RANKS", cboDt, cboRank);
            cboRank.SelectedIndex = 0;

            //加载职务字典
            cboDt = dictdal.getDictInfo("DUTY_DICT", "ID", "DUTY", true, "IS_DEL").Tables[0];
            SetCboData("ID", "DUTY", cboDt, cboDuty);
            cboDuty.SelectedIndex = 0;

            //在岗状态
            cboDt = dictdal.getDictInfo("ONGUARD", "ID", "STATS", false, "").Tables[0];
            SetCboData("ID", "STATS", cboDt, cboIsOnGuard);
            cboIsOnGuard.SelectedIndex = 0;

            //所学专业
            cboDt = dictdal.getDictInfo("STUDYSPEECSORT", "ID", "SPEECSORT", false, "").Tables[0];
            SetCboData("ID", "SPEECSORT", cboDt, cboSpeciality);
            cboSpeciality.SelectedIndex = 0;

            //干部
            cboDt = dictdal.getDictInfo("CADRES_CATEGORIES", "ID", "CADRES_TYPE", false, "").Tables[0];
            SetCboData("ID", "CADRES_TYPE", cboDt, cboCadreType);
            cboCadreType.SelectedIndex = 0;

            //科室类
            cboDt = dictdal.getDictInfo("DEPT_TYPE", "ID", "DEPTTYPE", false, "").Tables[0];
            SetCboData("ID", "DEPTTYPE", cboDt, cboDeptType);
            cboDeptType.SelectedIndex = 0;

            //加载技术级字典
            cboDt = dictdal.getDictInfo("TECHNICCLASS_DICT", "ID", "TECHNICCLASS", true, "IS_DEL").Tables[0];
            SetCboData("ID", "TECHNICCLASS", cboDt, cboTechnicClass);
            cboTechnicClass.SelectedIndex = 0;

            //加载文职级字典
            cboDt = dictdal.getDictInfo("CIVILSERVICECLASS_DICT", "ID", "CIVILSERVICECLASS", true, "IS_DEL").Tables[0];
            SetCboData("ID", "CIVILSERVICECLASS", cboDt, cboCivilServiceClass);
            cboCivilServiceClass.SelectedIndex = 0;

            //加载最高学历字典
            cboDt = dictdal.getDictInfo("LEARNSUFFER_DICT", "ID", "LEARNSUFFER", true, "IS_DEL").Tables[0];
            SetCboData("ID", "LEARNSUFFER", cboDt, cboTiptopLearnStuffer);
            cboTiptopLearnStuffer.SelectedIndex = 0;

            //加载最高学位字典
            cboDt = dictdal.getDictInfo("DEGREE_DICT", "ID", "DEGREE", true, "IS_DEL").Tables[0];
            SetCboData("ID", "DEGREE", cboDt, cboDegree);
            cboDegree.SelectedIndex = 0;
            //是否在岗
            cboDt = dictdal.getDictInfo("ONGUARD", "ID", "STATS", false, "").Tables[0];
            SetCboData("ID", "ONGUARD", cboDt, ComboBoxonguard);
            //ComboBoxonguard.SelectedIndex = 0;

            //人员职称序列字典
            cboDt = dictdal.getDictInfo("TITLE_LIST_DICT", "SERIAL_NO", "TITLE_LIST_NAME", false, "").Tables[0];
            SetCboData("SERIAL_NO", "TITLE_LIST_NAME", cboDt, cboTitleList);
            cboTitleList.SelectedIndex = 0;

            //职称
            cboDt = dictdal.getDictInfo("TITLE_DICT", "SERIAL_NO", "TITLE_NAME", false, "").Tables[0];
            SetCboData("SERIAL_NO", "TITLE_NAME", cboDt, cboTitle);
            cboTitle.SelectedIndex = 0;

            //加载卫生专业分类字典
            cboDt = dictdal.getDictInfo("SANI_SPEC_SORT_DICT", "SERIAL_NO", "SORT_NAME", false, "").Tables[0];
            SetCboData("SERIAL_NO", "SORT_NAME", cboDt, cboSanispecsort);
            cboSanispecsort.SelectedIndex = 0;

            //加载从事专业分类字典
            cboDt = dictdal.getDictInfo("PERS_SPEC_SORT_DICT", "SERIAL_NO", "SPEC_SORT_NAME", false, "").Tables[0];
            SetCboData("SERIAL_NO", "SPEC_SORT_NAME", cboDt, cboRootspecsort);
            cboRootspecsort.SelectedIndex = 0;
        }

        /// <summary>
        /// 初始化COMBOX控件
        /// </summary>
        /// <param name="ID">数据库ID名称</param>
        /// <param name="text">数据库NAME名称</param>
        /// <param name="dtSource">数据源</param>
        /// <param name="cbo">COMBOX控件</param>
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
        /// 
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="optype"></param>
        [AjaxMethod]
        public void StaffInfoAjaxOper(string Id, string optype)
        {
            switch (optype)
            {
                case "3":
                    dal.EchoUpDataStaffListHandle("NEW_STAFF_INFO", "ADD_MARK", "STAFF_ID", Id, "2",
                        "", Convert.ToDateTime(System.DateTime.Now.Year.ToString() + "-" + System.DateTime.Now.Month + "-01").ToString("yyyyMMdd"));
                    break;
                case "4":
                    dal.EchoUpDataStaffListHandle("NEW_STAFF_INFO", "ADD_MARK", "STAFF_ID", Id, "0",
                        "", Convert.ToDateTime(System.DateTime.Now.Year.ToString() + "-" + System.DateTime.Now.Month + "-01").ToString("yyyyMMdd"));
                    break;
                case "6":
                    dal.EchoUpDataStaffListHandle("NEW_STAFF_INFO", "ADD_MARK", "STAFF_ID", Id, "1",
                        ((User)Session["CURRENTSTAFF"]).UserName, Convert.ToDateTime(System.DateTime.Now.Year.ToString() + "-" + System.DateTime.Now.Month + "-01").ToString("yyyyMMdd"));
                    break;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="deptCode"></param>
        /// <returns></returns>
        [AjaxMethod]
        public string StationTypeAjaxOper(string deptCode)
        {
            DictMainTainDal dictdal = new DictMainTainDal();
            if (deptCode == "")
            {
                deptCode = this.DeptCodeCombo.SelectedItem.Value;
            }
            DataTable l_dt = dictdal.getStationNameInfo(deptCode).Tables[0];
            if (l_dt.Rows.Count > 0)
            {
                this.StoreCombo.DataSource = l_dt;
                this.StoreCombo.DataBind();
            }
            return "";
        }

        /// <summary>
        /// 数据啊查新，数据更新
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void Data_RefreshData(object sender, StoreRefreshDataEventArgs e)
        {
            InitAddMark();
            string sort = "";
            if (GetConfig.GetConfigString("Hospital").ToUpper() != "CA")
            {
                if (this.cboPersonType.SelectedItem.Text == "" || this.cboPersonType.SelectedItem.Text == "全部")
                {
                    DataTable l_dt = PersTypeFilter();
                    for (int i = 0; i < l_dt.Rows.Count; i++)
                    {
                        sort = sort + "'" + l_dt.Rows[i]["NAME"] + "',";
                    }
                    if (l_dt.Rows.Count == 0)
                    {
                        sort = "'-1'";
                    }
                }
                else
                {
                    sort = "'" + this.cboPersonType.SelectedItem.Text + "'";
                }
            }
            string contracttime = "";
            if (endtime.SelectedDate.ToString("yyyy") != "0001")
            {
                contracttime = endtime.SelectedDate.ToString("yyyyMM");
            }
            DataTable dt = tdal.ViewStaffList(this.DeptCodeCombo.SelectedItem.Value, add_mark, this.DeptFilter(""), sort.TrimEnd(new char[] { ',' }), ComboBoxonguard.SelectedItem.Text, Combosex.SelectedItem.Text, contracttime).Tables[0];

            //this.Store1.DataSource = tdal.ViewStaffList(this.DeptCodeCombo.SelectedItem.Value, add_mark, this.DeptFilter(""), sort.TrimEnd(new char[]{','}));
            this.Store1.DataSource = dt;
            this.Store1.DataBind();
            this.labConut.Text = "人数为" + dt.Rows.Count + "人";
            this.Store3.DataBind();
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
        /// 初始化控件
        /// </summary>
        private void InitContrl()
        {
            switch (PowerPageInfo)
            {
                case 1:
                    this.cbxOpration.BoxLabel = "显示已审批";
                    this.btn_EchoHandle.Text = "批量审批";
                    break;
                case 2:
                    this.cbxOpration.BoxLabel = "显示未提交";
                    this.btn_EchoHandle.Text = "批量提交";
                    break;
            }
        }

        /// <summary>
        /// 判断标价值
        /// </summary>
        private void InitAddMark()
        {
            switch (PowerPageInfo)
            {
                case 1:
                    add_mark = (this.cbxOpration.Checked) ? "1" : "0";
                    break;
                case 2:
                    add_mark = (this.cbxOpration.Checked) ? "3" : "1";
                    break;
            }
        }

        /// <summary>
        /// 保存按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void SaveInfo(object sender, AjaxEventArgs e)
        {
            int values = cboInptName.Checked == true ? this.txtStaffInput.SelectedItem.Value.Length : 0;
            string usernames = this.txtStaffInput.SelectedItem.Text;
            string str111 = this.txtStaffInput.SelectedItem.Value;
            string mark = PowerPageInfo == 2 ? "3" : "1";
            string Creater = ((User)Session["CURRENTSTAFF"]).UserId == null ? "NotUserId" : ((User)Session["CURRENTSTAFF"]).UserId;
            string CreaterName = ((User)Session["CURRENTSTAFF"]).UserName;
            string[] filename = this.photoimg.PostedFile.FileName.ToString().Split('\\');
            string imagid = "";
            if (filename.Length != 0 && !filename[0].Equals(""))
            {
                string fpath = @"/resources/UploadPicTemp/" + Creater + "temp" + filename[filename.Length - 1].Substring(filename[filename.Length - 1].LastIndexOf("."));
                //保存人员相应图片
                if (System.IO.File.Exists(Server.MapPath(fpath)))
                {
                    string staffid = OracleOledbBase.GetMaxID("STAFF_ID", DataUser.RLZY + ".NEW_STAFF_INFO").ToString();
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

            string index = e.ExtraParams["index"].ToString();
            // 是否是军卫账户
            if (this.cboInptName.Checked)
            {
                if (index == "-1")
                {
                    Goldnet.Ext.Web.Ext.Msg.Show(new Goldnet.Ext.Web.MessageBox.Config
                    {
                        Title = "信息提示",
                        Message = "请选择军卫帐户",
                        Buttons = MessageBox.Button.OK,
                        Icon = (MessageBox.Icon)Enum.Parse(typeof(MessageBox.Icon), "INFO")
                    });
                    return;
                }
            }

            string DB_USER = "";
            bool theFrist = true;
            if (this.cboInptName.Checked)
            {
                if (tdal.isExUserInfoByUserId(this.txtStaffInput.SelectedItem.Value))
                {
                    Goldnet.Ext.Web.Ext.Msg.Show(new Goldnet.Ext.Web.MessageBox.Config
                    {
                        Title = "信息提示",
                        Message = "输入用户已存在信息",
                        Buttons = MessageBox.Button.OK,
                        Icon = (MessageBox.Icon)Enum.Parse(typeof(MessageBox.Icon), "INFO")
                    });
                    return;
                }
                else
                {
                    if (tdal.isExStaffInfoByName(this.DeptCodeCombo.SelectedItem.Value, this.txtStaffInput.SelectedItem.Text))
                    {
                        Goldnet.Ext.Web.Ext.Msg.Show(new Goldnet.Ext.Web.MessageBox.Config
                        {
                            Title = "信息提示",
                            Message = "本科人员姓名不能重复",
                            Buttons = MessageBox.Button.OK,
                            Icon = (MessageBox.Icon)Enum.Parse(typeof(MessageBox.Icon), "INFO")
                        });
                        return;
                    }
                }
            }
            else
            {
                if (tdal.isExStaffInfoByName(this.DeptCodeCombo.SelectedItem.Value, this.txtStaffInput.SelectedItem.Text))
                {
                    Goldnet.Ext.Web.Ext.Msg.Show(new Goldnet.Ext.Web.MessageBox.Config
                    {
                        Title = "信息提示",
                        Message = "本科人员姓名不能重复",
                        Buttons = MessageBox.Button.OK,
                        Icon = (MessageBox.Icon)Enum.Parse(typeof(MessageBox.Icon), "INFO")
                    });
                    return;
                }
                theFrist = false;
            }

            //是否是军队人员
            string isAmry = GetConfig.GetConfigString("isAmry");
            if (isAmry == "2")
            {
                this.cboIfarmy.SelectedItem.Value = "0";
            }
            isAmry = this.cboIfarmy.SelectedItem.Value == "" ? "0" : this.cboIfarmy.SelectedItem.Value;
            if (this.cboInptName.Checked)
            {
                DB_USER = tdal.getHisDataDbUserByUserid(this.txtStaffInput.SelectedItem.Value);
            }

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
                                            "0", this.txtRetainTerm.Text, isAmry, this.cboJobDuty.SelectedItem.Text, TimeConvert(this.dtfJobDate),
                                            this.cboPerssort.SelectedItem.Text, TimeConvert(this.dtfBeEnrolledInDate), TimeConvert(this.dtfWorkDate),
                                            this.cboDuty.SelectedItem.Text, TimeConvert(this.dtfDutydate), this.cboTechnicClass.SelectedItem.Text,
                                            TimeConvert(this.dtfTechnicClassDate), this.cboCivilServiceClass.SelectedItem.Text, TimeConvert(this.dtfCivilServiceClassDate),
                                            this.cboSanispecsort.SelectedItem.Text, this.cboRootspecsort.SelectedItem.Text, this.txtMedicardmark.Text, this.txtMediCard.Text, CreaterName,
                                            DateTime.Now.ToString("yyyyMMdd"), Convert.ToDateTime(System.DateTime.Now.Year.ToString() + "-" + System.DateTime.Now.Month + "-01").ToString("yyyyMMdd"), this.txtHomeplace.Text,
                                            this.txtCertificateNo.Text, this.cmbMaritalStatus.SelectedItem.Text, this.cboTitleList.SelectedItem.Text,
                                            this.cboDegree.SelectedItem.Text, this.txtGraduateAcademy.Text, TimeConvert(this.dtfGradetitleDate),
                                            this.cboRank.SelectedItem.Text, this.cboTitle.SelectedItem.Text, "",
                                            this.txtMemo.Text, PowerPageInfo == 2 ? "" : CreaterName, usernames, "", theFrist ? DB_USER : pinyin.GetChineseSpell(usernames), this.cboTechnicTitle.SelectedItem.Text,
                                            TimeConvert(this.dtfTitleAssess), this.cboBackboneCircs.SelectedItem.Text, imagid, this.txtCredithourPerYear.Text,
                                            "", this.cboIsOnGuard.SelectedItem.Text == "是" ? this.cboStation.SelectedItem.Value : "", cboGord.SelectedItem.Value, contractstart, contractend, ComboBox2.SelectedItem.Text, ComboBox3.SelectedItem.Text, this.txtBonusNum.Text);

            tdal.InsertStaffInfo(info, mark, "", "", this.cboBraid.SelectedItem.Value, theFrist ? usernames : "", theFrist ? this.txtStaffInput.SelectedItem.Value.ToString() : "", TimeConvert(this.DateField1));
            Goldnet.Ext.Web.ScriptManager scManager = Goldnet.Ext.Web.ScriptManager.GetInstance(this.Page);
            scManager.AddScript("Store1.reload();");
            scManager.AddScript("btn_EchoHandle.setDisabled(true);");
            scManager.AddScript("btn_Delete.setDisabled(true);");
            scManager.AddScript("btnCommand.setDisabled(true);");
            scManager.AddScript("arcEditWindow.hide();");
            this.Store3.DataBind();
        }

        /// <summary>
        /// 提交按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void SetUpInfo(object sender, AjaxEventArgs e)
        {
            int values = cboInptName.Checked == true ? this.txtStaffInput.SelectedItem.Value.Length : 0;
            string usernames = this.txtStaffInput.SelectedItem.Text;
            string Creater = ((User)Session["CURRENTSTAFF"]).UserId == null ? "NotUserId" : ((User)Session["CURRENTSTAFF"]).UserId;
            string[] filename = this.photoimg.PostedFile.FileName.ToString().Split('\\');
            string imagid = HiddenImage.Text;
            if (filename.Length != 0 && !filename[0].Equals(""))
            {
                string fpath = @"/resources/UploadPicTemp/" + Creater + "temp" + filename[filename.Length - 1].Substring(filename[filename.Length - 1].LastIndexOf("."));
                //保存人员相应图片
                //if (System.IO.File.Exists(Server.MapPath(fpath)))
                //{
                //    if (System.IO.File.Exists(Server.MapPath(@"/resources/UploadPicfile/" + HiddenImage.Text)))
                //    {
                //        System.IO.File.Delete(Server.MapPath(@"/resources/UploadPicfile/" + HiddenImage.Text));
                //    }
                //    if (imagid == "")
                //    {
                //        System.IO.File.Move(Server.MapPath(fpath), Server.MapPath(@"/resources/UploadPicfile/" + this.HiddenId.Text));
                //    }
                //    else
                //    {
                //        System.IO.File.Move(Server.MapPath(fpath), Server.MapPath(@"/resources/UploadPicfile/" + HiddenImage.Text));
                //    }
                //    if (System.IO.File.Exists(Server.MapPath(fpath)))
                //    {
                //        System.IO.File.Delete(Server.MapPath(fpath));
                //    }
                //    imagid = HiddenId.Text;
                //}
                //保存人员相应图片
                if (System.IO.File.Exists(Server.MapPath(fpath)))
                {
                    string staffid = OracleOledbBase.GetMaxID("STAFF_ID", DataUser.RLZY + ".NEW_STAFF_INFO").ToString();
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

            if (this.cboInptName.Checked)
            {
                if (!tdal.isExUserByUserId(this.txtStaffInput.SelectedItem.Value))
                {
                    Goldnet.Ext.Web.Ext.Msg.Show(new Goldnet.Ext.Web.MessageBox.Config
                    {
                        Title = "信息提示",
                        Message = "请选择军卫帐户",
                        Buttons = MessageBox.Button.OK,
                        Icon = (MessageBox.Icon)Enum.Parse(typeof(MessageBox.Icon), "INFO")
                    });
                    return;
                }
            }
            bool theFrist = true;

            string deptCode = e.ExtraParams["deptCode"].ToString();
            string deptName = e.ExtraParams["deptName"].ToString();

            if (this.IsEdit())
            {
                if (this.cboInptName.Checked)
                {
                    if (tdal.isExUserInfoByUserId(this.txtStaffInput.SelectedItem.Value, HiddenId.Text))
                    {
                        Goldnet.Ext.Web.Ext.Msg.Show(new Goldnet.Ext.Web.MessageBox.Config
                        {
                            Title = "信息提示",
                            Message = "输入用户已存在信息",
                            Buttons = MessageBox.Button.OK,
                            Icon = (MessageBox.Icon)Enum.Parse(typeof(MessageBox.Icon), "INFO")
                        });
                        return;
                    }
                    else
                    {
                        if (tdal.isExStaffInfoByName(deptCode, this.txtStaffInput.SelectedItem.Text, HiddenId.Text))
                        {
                            Goldnet.Ext.Web.Ext.Msg.Show(new Goldnet.Ext.Web.MessageBox.Config
                            {
                                Title = "信息提示",
                                Message = "本科人员姓名不能重复",
                                Buttons = MessageBox.Button.OK,
                                Icon = (MessageBox.Icon)Enum.Parse(typeof(MessageBox.Icon), "INFO")
                            });
                            return;
                        }
                    }
                }
                else
                {
                    if (tdal.isExStaffInfoByName(deptCode, this.txtStaffInput.SelectedItem.Text, HiddenId.Text))
                    {
                        Goldnet.Ext.Web.Ext.Msg.Show(new Goldnet.Ext.Web.MessageBox.Config
                        {
                            Title = "信息提示",
                            Message = "本科人员姓名不能重复",
                            Buttons = MessageBox.Button.OK,
                            Icon = (MessageBox.Icon)Enum.Parse(typeof(MessageBox.Icon), "INFO")
                        });
                        return;
                    }
                    theFrist = false;
                }
            }

            string isAmry = GetConfig.GetConfigString("isAmry");
            if (isAmry == "2")
            {
                this.cboIfarmy.SelectedItem.Value = "0";

            }
            string DB_USER = "";
            if (this.cboInptName.Checked)
            {
                DB_USER = tdal.getHisDataDbUserByUserid(this.txtStaffInput.SelectedItem.Value);
            }

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
                                          deptCode, deptName, usernames, this.cboIsOnGuard.SelectedItem.Text,
                                          this.dtfBirthday.SelectedDate.ToString("yyyy-MM-dd"), this.cboSex.SelectedItem.Text, TimeConvert(this.dtfInHospitalDate),
                                          "0", this.txtRetainTerm.Text, this.cboIfarmy.SelectedItem.Value, this.cboJobDuty.SelectedItem.Text, TimeConvert(this.dtfJobDate),
                                          this.cboPerssort.SelectedItem.Text, TimeConvert(this.dtfBeEnrolledInDate), TimeConvert(this.dtfWorkDate),
                                          this.cboDuty.SelectedItem.Text, TimeConvert(this.dtfDutydate), this.cboTechnicClass.SelectedItem.Text,
                                          TimeConvert(this.dtfTechnicClassDate), this.cboCivilServiceClass.SelectedItem.Text, TimeConvert(this.dtfCivilServiceClassDate),
                                          this.cboSanispecsort.SelectedItem.Text, this.cboRootspecsort.SelectedItem.Text, this.txtMedicardmark.Text, this.txtMediCard.Text, Creater,
                                          DateTime.Now.ToString("yyyyMMdd"), Convert.ToDateTime(System.DateTime.Now.Year.ToString() + "-" + System.DateTime.Now.Month + "-01").ToString("yyyyMMdd"), this.txtHomeplace.Text,
                                          this.txtCertificateNo.Text, this.cmbMaritalStatus.SelectedItem.Text, this.cboTitleList.SelectedItem.Text,
                                          this.cboDegree.SelectedItem.Text, this.txtGraduateAcademy.Text, TimeConvert(this.dtfGradetitleDate),
                                          this.cboRank.SelectedItem.Text, this.cboTitle.SelectedItem.Text, "",
                                          this.txtMemo.Text, "", this.txtStaffInput.SelectedItem.Text, "", pinyin.GetChineseSpell(usernames), this.cboTechnicTitle.SelectedItem.Text,
                                          TimeConvert(this.dtfTitleAssess), this.cboBackboneCircs.SelectedItem.Text, imagid, this.txtCredithourPerYear.Text,
                                          "", this.cboIsOnGuard.SelectedItem.Text == "是" ? this.cboStation.SelectedItem.Value : "", cboGord.SelectedItem.Value, contractstart, contractend, ComboBox2.SelectedItem.Text, ComboBox3.SelectedItem.Text, this.txtBonusNum.Text);

            tdal.UpdateStaffInfo(HiddenId.Text, info, "0", "",
                "", this.cboBraid.SelectedItem.Value,
                theFrist ? usernames : "",
                theFrist ? this.txtStaffInput.SelectedItem.Value.ToString() : "",
                theFrist ? DB_USER : pinyin.GetChineseSpell(usernames), TimeConvert(this.DateField1));

            Goldnet.Ext.Web.ScriptManager scManager = Goldnet.Ext.Web.ScriptManager.GetInstance(this.Page);
            scManager.AddScript("Store1.reload();");
            scManager.AddScript("btn_EchoHandle.setDisabled(true);");
            scManager.AddScript("btn_Delete.setDisabled(true);");
            scManager.AddScript("btnCommand.setDisabled(true);");
            scManager.AddScript("arcEditWindow.hide();");
        }

        /// <summary>
        /// 审批按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ApproveInfo(object sender, AjaxEventArgs e)
        {
            int values = cboInptName.Checked == true ? this.txtStaffInput.SelectedItem.Value.Length : 0;
            string usernames = this.txtStaffInput.SelectedItem.Text;
            string CreaterId = ((User)Session["CURRENTSTAFF"]).UserId == null ? "NotUserId" : ((User)Session["CURRENTSTAFF"]).UserId;
            string CreaterName = ((User)Session["CURRENTSTAFF"]).UserName;
            string[] filename = this.photoimg.PostedFile.FileName.ToString().Split('\\');
            string imagid = HiddenImage.Text;
            if (filename.Length != 0 && !filename[0].Equals(""))
            {
                string fpath = @"/resources/UploadPicTemp/" + CreaterId + "temp" + filename[filename.Length - 1].Substring(filename[filename.Length - 1].LastIndexOf("."));
                //保存人员相应图片
                //if (System.IO.File.Exists(Server.MapPath(fpath)))
                //{
                //    if (System.IO.File.Exists(Server.MapPath(@"/resources/UploadPicfile/" + HiddenImage.Text)))
                //    {
                //        System.IO.File.Delete(Server.MapPath(@"/resources/UploadPicfile/" + HiddenImage.Text));
                //    }
                //    if (imagid == "")
                //    {
                //        System.IO.File.Move(Server.MapPath(fpath), Server.MapPath(@"/resources/UploadPicfile/" + this.HiddenId.Text));
                //    }
                //    else
                //    {
                //        System.IO.File.Move(Server.MapPath(fpath), Server.MapPath(@"/resources/UploadPicfile/" + HiddenImage.Text));
                //    }
                //    if (System.IO.File.Exists(Server.MapPath(fpath)))
                //    {
                //        System.IO.File.Delete(Server.MapPath(fpath));
                //    }
                //    imagid = HiddenId.Text;
                //保存人员相应图片
                if (System.IO.File.Exists(Server.MapPath(fpath)))
                {
                    string staffid = this.txtStaffInput.SelectedItem.Value.ToString();//OracleOledbBase.GetMaxID("STAFF_ID", DataUser.RLZY + ".NEW_STAFF_INFO").ToString();
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

            //
            //
            if (this.cboInptName.Checked)
            {
                if (!tdal.isExUserByUserId(this.txtStaffInput.SelectedItem.Value))
                {
                    Goldnet.Ext.Web.Ext.Msg.Show(new Goldnet.Ext.Web.MessageBox.Config
                    {
                        Title = "信息提示",
                        Message = "请选择军卫帐户",
                        Buttons = MessageBox.Button.OK,
                        Icon = (MessageBox.Icon)Enum.Parse(typeof(MessageBox.Icon), "INFO")
                    });
                    return;
                }
            }

            string deptCode = e.ExtraParams["deptCode"].ToString();
            string deptName = e.ExtraParams["deptName"].ToString();
            bool theFrist = true;
            if (this.IsEdit())
            {
                if (this.cboInptName.Checked)
                {
                    if (tdal.isExUserInfoByUserId(this.txtStaffInput.SelectedItem.Value, HiddenId.Text))
                    {
                        Goldnet.Ext.Web.Ext.Msg.Show(new Goldnet.Ext.Web.MessageBox.Config
                        {
                            Title = "信息提示",
                            Message = "输入用户已存在信息",
                            Buttons = MessageBox.Button.OK,
                            Icon = (MessageBox.Icon)Enum.Parse(typeof(MessageBox.Icon), "INFO")
                        });
                        return;
                    }
                    else
                    {
                        if (tdal.isExStaffInfoByName(deptCode, this.txtStaffInput.SelectedItem.Text, HiddenId.Text))
                        {
                            Goldnet.Ext.Web.Ext.Msg.Show(new Goldnet.Ext.Web.MessageBox.Config
                            {
                                Title = "信息提示",
                                Message = "本科人员姓名不能重复",
                                Buttons = MessageBox.Button.OK,
                                Icon = (MessageBox.Icon)Enum.Parse(typeof(MessageBox.Icon), "INFO")
                            });
                            return;
                        }
                    }
                }
                else
                {
                    if (tdal.isExStaffInfoByName(deptCode, this.txtStaffInput.SelectedItem.Text, HiddenId.Text))
                    {
                        Goldnet.Ext.Web.Ext.Msg.Show(new Goldnet.Ext.Web.MessageBox.Config
                        {
                            Title = "信息提示",
                            Message = "本科人员姓名不能重复",
                            Buttons = MessageBox.Button.OK,
                            Icon = (MessageBox.Icon)Enum.Parse(typeof(MessageBox.Icon), "INFO")
                        });
                        return;
                    }
                    theFrist = false;
                }
            }

            string isAmry = GetConfig.GetConfigString("isAmry");
            if (isAmry == "2")
            {
                this.cboIfarmy.SelectedItem.Value = "0";

            }
            string DB_USER = "";
            if (this.cboInptName.Checked)
            {
                DB_USER = tdal.getHisDataDbUserByUserid(this.txtStaffInput.SelectedItem.Value);
            }

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
                                          deptCode, deptName, usernames, this.cboIsOnGuard.SelectedItem.Text,
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
                                          this.txtMemo.Text, CreaterName, this.txtStaffInput.SelectedItem.Text, "", pinyin.GetChineseSpell(usernames), this.cboTechnicTitle.SelectedItem.Text,
                                          TimeConvert(this.dtfTitleAssess), this.cboBackboneCircs.SelectedItem.Text, imagid, this.txtCredithourPerYear.Text,
                                          "", this.cboIsOnGuard.SelectedItem.Text == "是" ? this.cboStation.SelectedItem.Value : "", cboGord.SelectedItem.Value, contractstart, contractend, ComboBox2.SelectedItem.Text, ComboBox3.SelectedItem.Text, this.txtBonusNum.Text);

            tdal.UpdateStaffInfo(HiddenId.Text, info, "1", "", "", this.cboBraid.SelectedItem.Value, theFrist ? usernames : "", theFrist ? this.txtStaffInput.SelectedItem.Value.ToString() : "", theFrist ? DB_USER : pinyin.GetChineseSpell(usernames), TimeConvert(this.DateField1));
            Goldnet.Ext.Web.ScriptManager scManager = Goldnet.Ext.Web.ScriptManager.GetInstance(this.Page);
            scManager.AddScript("Store1.reload();");
            scManager.AddScript("btn_EchoHandle.setDisabled(true);");
            scManager.AddScript("btn_Delete.setDisabled(true);");
            scManager.AddScript("btnCommand.setDisabled(true);");
            scManager.AddScript("arcEditWindow.hide();");
        }

        /// <summary>
        /// 添加的情况下直接提交按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void SaveUpInfo(object sender, AjaxEventArgs e)
        {
            int values = cboInptName.Checked == true ? this.txtStaffInput.SelectedItem.Value.Length : 0;
            string usernames = this.txtStaffInput.SelectedItem.Text;
            string Creater = ((User)Session["CURRENTSTAFF"]).UserId == null ? "NotUserId" : ((User)Session["CURRENTSTAFF"]).UserId;
            string CreaterName = ((User)Session["CURRENTSTAFF"]).UserName;
            string[] filename = this.photoimg.PostedFile.FileName.ToString().Split('\\');
            string imagid = "";
            if (filename.Length != 0 && !filename[0].Equals(""))
            {
                string fpath = @"/resources/UploadPicTemp/" + Creater + "temp" + filename[filename.Length - 1].Substring(filename[filename.Length - 1].LastIndexOf("."));
                //保存人员相应图片
                if (System.IO.File.Exists(Server.MapPath(fpath)))
                {
                    string staffid = OracleOledbBase.GetMaxID("STAFF_ID", DataUser.RLZY + ".NEW_STAFF_INFO").ToString();
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

            string index = e.ExtraParams["index"].ToString();
            if (this.cboInptName.Checked)
            {
                if (index == "-1")
                {
                    Goldnet.Ext.Web.Ext.Msg.Show(new Goldnet.Ext.Web.MessageBox.Config
                    {
                        Title = "信息提示",
                        Message = "请选择军卫帐户",
                        Buttons = MessageBox.Button.OK,
                        Icon = (MessageBox.Icon)Enum.Parse(typeof(MessageBox.Icon), "INFO")
                    });
                    return;

                }
            }

            bool theFrist = true;
            if (this.cboInptName.Checked)
            {
                if (tdal.isExUserInfoByUserId(this.txtStaffInput.SelectedItem.Value))
                {
                    Goldnet.Ext.Web.Ext.Msg.Show(new Goldnet.Ext.Web.MessageBox.Config
                    {
                        Title = "信息提示",
                        Message = "输入用户已存在信息",
                        Buttons = MessageBox.Button.OK,
                        Icon = (MessageBox.Icon)Enum.Parse(typeof(MessageBox.Icon), "INFO")
                    });
                    return;
                }
                else
                {
                    if (tdal.isExStaffInfoByName(this.DeptCodeCombo.SelectedItem.Value, this.txtStaffInput.SelectedItem.Text))
                    {
                        Goldnet.Ext.Web.Ext.Msg.Show(new Goldnet.Ext.Web.MessageBox.Config
                        {
                            Title = "信息提示",
                            Message = "本科人员姓名不能重复",
                            Buttons = MessageBox.Button.OK,
                            Icon = (MessageBox.Icon)Enum.Parse(typeof(MessageBox.Icon), "INFO")
                        });
                        return;
                    }
                }
            }
            else
            {

                if (tdal.isExStaffInfoByName(this.DeptCodeCombo.SelectedItem.Value, this.txtStaffInput.SelectedItem.Text))
                {
                    Goldnet.Ext.Web.Ext.Msg.Show(new Goldnet.Ext.Web.MessageBox.Config
                    {
                        Title = "信息提示",
                        Message = "本科人员姓名不能重复",
                        Buttons = MessageBox.Button.OK,
                        Icon = (MessageBox.Icon)Enum.Parse(typeof(MessageBox.Icon), "INFO")
                    });
                    return;
                }
                theFrist = false;
            }
            string isAmry = GetConfig.GetConfigString("isAmry");
            if (isAmry == "2")
            {
                this.cboIfarmy.SelectedItem.Value = "0";

            }
            string DB_USER = "";
            if (this.cboInptName.Checked)
            {
                DB_USER = tdal.getHisDataDbUserByUserid(this.txtStaffInput.SelectedItem.Value);
            }

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
                                            this.txtMemo.Text, PowerPageInfo == 2 ? "" : CreaterName, usernames, "", theFrist ? DB_USER : pinyin.GetChineseSpell(usernames), this.cboTechnicTitle.SelectedItem.Text,
                                            TimeConvert(this.dtfTitleAssess), this.cboBackboneCircs.SelectedItem.Text, imagid, this.txtCredithourPerYear.Text,
                                            "", this.cboIsOnGuard.SelectedItem.Text == "是" ? this.cboStation.SelectedItem.Value : "", cboGord.SelectedItem.Value, contractstart, contractend, ComboBox2.SelectedItem.Text, ComboBox3.SelectedItem.Text, this.txtBonusNum.Text);

            tdal.InsertStaffInfo(info, "0", "", "", this.cboBraid.SelectedItem.Value, theFrist ? usernames : "", theFrist ? this.txtStaffInput.SelectedItem.Value : "", TimeConvert(this.DateField1));


            Goldnet.Ext.Web.ScriptManager scManager = Goldnet.Ext.Web.ScriptManager.GetInstance(this.Page);
            scManager.AddScript("Store1.reload();");
            scManager.AddScript("btn_EchoHandle.setDisabled(true);");
            scManager.AddScript("btn_Delete.setDisabled(true);");
            scManager.AddScript("btnCommand.setDisabled(true);");
            scManager.AddScript("arcEditWindow.hide();");
        }


        /// <summary>
        /// 未提交时再保存按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void SetSave(object sender, AjaxEventArgs e)
        {
            int values = cboInptName.Checked == true ? this.txtStaffInput.SelectedItem.Value.Length : 0;
            string usernames = this.txtStaffInput.SelectedItem.Text;
            string Creater = ((User)Session["CURRENTSTAFF"]).UserId == null ? "NotUserId" : ((User)Session["CURRENTSTAFF"]).UserId;
            string CreaterName = ((User)Session["CURRENTSTAFF"]).UserName;
            string[] filename = this.photoimg.PostedFile.FileName.ToString().Split('\\');
            string imagid = "";
            if (filename.Length != 0 && !filename[0].Equals(""))
            {
                string fpath = @"/resources/UploadPicTemp/" + Creater + "temp" + filename[filename.Length - 1].Substring(filename[filename.Length - 1].LastIndexOf("."));
                //保存人员相应图片
                if (System.IO.File.Exists(Server.MapPath(fpath)))
                {
                    string staffid = OracleOledbBase.GetMaxID("STAFF_ID", DataUser.RLZY + ".NEW_STAFF_INFO").ToString();
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

            string index = e.ExtraParams["UserId"].ToString();
            //选择军卫，但是没有选择军卫数据的情况
            if (this.cboInptName.Checked)
            {
                if (index == "" || index == null)
                {
                    Goldnet.Ext.Web.Ext.Msg.Show(new Goldnet.Ext.Web.MessageBox.Config
                    {
                        Title = "信息提示",
                        Message = "请选择军卫帐户",
                        Buttons = MessageBox.Button.OK,
                        Icon = (MessageBox.Icon)Enum.Parse(typeof(MessageBox.Icon), "INFO")
                    });
                    return;

                }
            }

            string deptCode = e.ExtraParams["deptCode"].ToString();
            string deptName = e.ExtraParams["deptName"].ToString();
            bool theFrist = true;
            //选择军卫用户时，判断有没有人已使用，除被修改人以外 
            if (this.cboInptName.Checked)
            {
                if (tdal.isExUserInfoByUserId(this.txtStaffInput.SelectedItem.Value, HiddenId.Text))
                {
                    Goldnet.Ext.Web.Ext.Msg.Show(new Goldnet.Ext.Web.MessageBox.Config
                    {
                        Title = "信息提示",
                        Message = "输入用户已存在信息",
                        Buttons = MessageBox.Button.OK,
                        Icon = (MessageBox.Icon)Enum.Parse(typeof(MessageBox.Icon), "INFO")
                    });
                    return;
                }
                else
                {
                    //再判断本科室有没有同名的情况，除被修改人以外
                    if (tdal.isExStaffInfoByName(deptCode, this.txtStaffInput.SelectedItem.Text, HiddenId.Text))
                    {
                        Goldnet.Ext.Web.Ext.Msg.Show(new Goldnet.Ext.Web.MessageBox.Config
                        {
                            Title = "信息提示",
                            Message = "本科人员姓名不能重复",
                            Buttons = MessageBox.Button.OK,
                            Icon = (MessageBox.Icon)Enum.Parse(typeof(MessageBox.Icon), "INFO")
                        });
                        return;
                    }
                }
            }
            else
            {
                //不选择军卫判断本科室有没有同名的情况，除被修改人以外
                if (tdal.isExStaffInfoByName(deptCode, this.txtStaffInput.SelectedItem.Text, HiddenId.Text))
                {
                    Goldnet.Ext.Web.Ext.Msg.Show(new Goldnet.Ext.Web.MessageBox.Config
                    {
                        Title = "信息提示",
                        Message = "本科人员姓名不能重复",
                        Buttons = MessageBox.Button.OK,
                        Icon = (MessageBox.Icon)Enum.Parse(typeof(MessageBox.Icon), "INFO")
                    });
                    return;
                }
                theFrist = false;
            }
            string isAmry = GetConfig.GetConfigString("isAmry");
            if (isAmry == "2")
            {
                this.cboIfarmy.SelectedItem.Value = "0";
            }
            string DB_USER = "";
            if (this.cboInptName.Checked)
            {
                DB_USER = tdal.getHisDataDbUserByUserid(this.txtStaffInput.SelectedItem.Value);
            }

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
                                        deptCode, deptName, usernames, this.cboIsOnGuard.SelectedItem.Text,
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
                                        this.txtMemo.Text, "", usernames, "", pinyin.GetChineseSpell(usernames), this.cboTechnicTitle.SelectedItem.Text,
                                        TimeConvert(this.dtfTitleAssess), this.cboBackboneCircs.SelectedItem.Text, imagid, this.txtCredithourPerYear.Text,
                                        "", this.cboIsOnGuard.SelectedItem.Text == "是" ? this.cboStation.SelectedItem.Value : "", cboGord.SelectedItem.Value, contractstart, contractend, ComboBox2.SelectedItem.Text, ComboBox3.SelectedItem.Text, this.txtBonusNum.Text);

            tdal.UpdateStaffInfo(HiddenId.Text, info, "3", "", "", this.cboBraid.SelectedItem.Value, theFrist ? usernames : "", theFrist ? this.txtStaffInput.SelectedItem.Value : "", theFrist ? DB_USER : pinyin.GetChineseSpell(usernames), TimeConvert(this.DateField1));
            Goldnet.Ext.Web.ScriptManager scManager = Goldnet.Ext.Web.ScriptManager.GetInstance(this.Page);
            scManager.AddScript("Store1.reload();");
            scManager.AddScript("btn_EchoHandle.setDisabled(true);");
            scManager.AddScript("btn_Delete.setDisabled(true);");
            scManager.AddScript("btnCommand.setDisabled(true);");
            scManager.AddScript("arcEditWindow.hide();");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void DbRowClick(object sender, AjaxEventArgs e)
        {
            Dictionary<string, string>[] selectRow = GetSelectRow(e);
            if (selectRow == null || selectRow.Length != 1)
            {
                Goldnet.Ext.Web.Ext.Msg.Show(new Goldnet.Ext.Web.MessageBox.Config
                {
                    Title = "信息提示",
                    Message = "只能选择一条记录",
                    Buttons = MessageBox.Button.OK,
                    Icon = (MessageBox.Icon)Enum.Parse(typeof(MessageBox.Icon), "INFO")
                });
            }
            else
            {
                string staffid = selectRow[0]["STAFF_ID"];

                LoadConfig loadcfg = getLoadConfig("Staff_Document.aspx");
                loadcfg.Params.Add(new Goldnet.Ext.Web.Parameter("staffid", staffid));

                showCenterSet(this.jsda_Detail, loadcfg);
            }
        }

        /// <summary>
        /// 列表记录点击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void RowClick(object sender, AjaxEventArgs e)
        {
            Dictionary<string, string>[] selectRow = GetSelectRow(e);

            if (selectRow == null || selectRow.Length < 1)
            {
                Goldnet.Ext.Web.Ext.Msg.Show(new Goldnet.Ext.Web.MessageBox.Config
                {
                    Title = "信息提示",
                    Message = "只能选择一条记录",
                    Buttons = MessageBox.Button.OK,
                    Icon = (MessageBox.Icon)Enum.Parse(typeof(MessageBox.Icon), "INFO")
                });
            }
            else
            {
                string staffid = selectRow[0]["STAFF_ID"];
                string deptcode = selectRow[0]["DEPT_CODE"];

                LoadConfig loadcfg = getLoadConfig("StaffInfodetail.aspx");
                loadcfg.Params.Add(new Goldnet.Ext.Web.Parameter("staffid", staffid));
                loadcfg.Params.Add(new Goldnet.Ext.Web.Parameter("pageid", this.Pageid()));
                //loadcfg.Params.Add(new Goldnet.Ext.Web.Parameter("deptcode", deptcode));

                showCenterSet(this.staffinfodetail, loadcfg);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        private Dictionary<string, string>[] GetSelectRow(AjaxEventArgs e)
        {
            string row = e.ExtraParams["Values"].ToString();
            Dictionary<string, string>[] selectRow = JSON.Deserialize<Dictionary<string, string>[]>(row);
            if (selectRow.Length <= 0) { return null; } else { return selectRow; }
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

        /// <summary>
        /// 导出人员列表
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void OutExcel(object sender, EventArgs e)
        {
            ExportData ex = new ExportData();
            BaseInfoMaintainDal dal = new BaseInfoMaintainDal();
            DataTable l_dt2 = new DataTable();
            DataTable col_dt = new DataTable();

            InitAddMark();

            string sort = "";
            if (GetConfig.GetConfigString("Hospital").ToUpper() != "CA")
            {
                if (this.cboPersonType.SelectedItem.Text == "" || this.cboPersonType.SelectedItem.Text == "全部")
                {
                    DataTable l_dt = PersTypeFilter();
                    for (int i = 0; i < l_dt.Rows.Count; i++)
                    {
                        sort = sort + "'" + l_dt.Rows[i]["NAME"] + "',";
                    }
                    if (l_dt.Rows.Count == 0)
                    {
                        sort = "'-1'";
                    }
                }
                else
                {
                    sort = "'" + this.cboPersonType.SelectedItem.Text + "'";
                }
            }

            string contracttime = "";
            if (endtime.SelectedDate.ToString("yyyy") != "0001")
            {
                contracttime = endtime.SelectedDate.ToString("yyyyMM");
            }

            //DataTable dt = tdal.ViewStaffList(this.DeptCodeCombo.SelectedItem.Value, add_mark, this.DeptFilter(""), sort.TrimEnd(new char[] { ',' }), ComboBoxonguard.SelectedItem.Text, Combosex.SelectedItem.Text, contracttime).Tables[0];

            l_dt2 = dal.getStaffInfoList(this.DeptCodeCombo.SelectedItem.Value, add_mark, this.DeptFilter(""), sort.TrimEnd(new char[] { ',' }), ComboBoxonguard.SelectedItem.Text, Combosex.SelectedItem.Text, contracttime).Tables[0];

            col_dt = dal.getStaffInfo().Tables[0];

            for (int i = 0; i < col_dt.Rows.Count; i++)
            {
                l_dt2.Columns[i].ColumnName = col_dt.Rows[i]["COMMENTS"].ToString();
            }

            //ex.ExportToLocal(l_dt2, this.Page, "xls", "人员信息");

            // 创建工作簿
            IWorkbook workbook = new HSSFWorkbook();
            ISheet sheet = workbook.CreateSheet("人员信息");

            // 创建表头
            IRow headerRow = sheet.CreateRow(0);
            for (int i = 0; i < l_dt2.Columns.Count; i++)
            {
                ICell cell = headerRow.CreateCell(i);
                cell.SetCellValue(l_dt2.Columns[i].ColumnName);
            }

            // 创建样式
            ICellStyle numberStyle = workbook.CreateCellStyle();
            numberStyle.DataFormat = HSSFDataFormat.GetBuiltinFormat("0");// 数值格式

            ICellStyle textStyle = workbook.CreateCellStyle();
            textStyle.DataFormat = HSSFDataFormat.GetBuiltinFormat("@");// 文本格式

            ICellStyle currencyStyle = workbook.CreateCellStyle();
            currencyStyle.DataFormat = HSSFDataFormat.GetBuiltinFormat("#,##0.00"); // 金钱格式

            // 填充数据
            for (int i = 0; i < l_dt2.Rows.Count; i++)
            {
                IRow row = sheet.CreateRow(i + 1);
                for (int j = 0; j < l_dt2.Columns.Count; j++)
                {
                    ICell cell = row.CreateCell(j);
                    string cellValue = l_dt2.Rows[i][j].ToString();
                    double number;

                    if (double.TryParse(cellValue, out number))
                    {
                        cell.SetCellValue(number);

                        // 如果是金钱列，设置为金钱格式；否则设置为数值格式
                        //if (j == /* 你的金钱列索引，例如第5列 */)
                        //{
                        //    cell.CellStyle = numberStyle;
                        //}
                        //else
                        //{
                        //    cell.CellStyle = currencyStyle;
                        //}

                        cell.CellStyle = currencyStyle; // 金钱格式
                    }
                    else
                    {
                        cell.SetCellValue(cellValue);
                        cell.CellStyle = textStyle; // 文本格式
                    }
                }
            }

            // 自动调整列宽
            int maxColumnWidth = 30 * 256; // 最大宽度设置为30字符宽
            for (int i = 0; i < l_dt2.Columns.Count; i++)
            {
                sheet.AutoSizeColumn(i);
                int currentWidth = sheet.GetColumnWidth(i);
                if (currentWidth > maxColumnWidth)
                {
                    sheet.SetColumnWidth(i, maxColumnWidth);
                }
            }

            // 导出为Excel文件
            using (MemoryStream exportData = new MemoryStream())
            {
                workbook.Write(exportData);
                Response.Clear();
                Response.AddHeader("content-disposition", "attachment; filename=人员信息.xls");
                Response.ContentType = "application/vnd.ms-excel";
                Response.BinaryWrite(exportData.ToArray());
                Response.End();
            }
        }

    }
}
