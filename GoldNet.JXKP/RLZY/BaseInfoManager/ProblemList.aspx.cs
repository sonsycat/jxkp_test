using System;
using System.Collections;
using System.Configuration;
using System.Data;
using Goldnet.Ext.Web;
using Goldnet.Dal;
using GoldNet.Comm;
using GoldNet.Model;

namespace GoldNet.JXKP.RLZY.BaseInfoManager
{
    public partial class ProblemList : PageBase
    {
        BaseInfoManagerDal dal = new BaseInfoManagerDal();
        //1:具有审批权限的人登入,2:普通用户进入
        private static int PowerPageInfo = -1;
        private static string add_mark = null;

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
                        ScriptManager1.AddScript("#{btn_Delete}.hide();#{btn_Add}.hide();#{btnDelAuthor}.hide();");
                        this.hiddenEdit.Text = "2";
                    }
                }
                else
                {
                    if (!isEdit)
                    {
                        //ScriptManager1.AddScript("#{btn_Delete}.hide();#{btn_Add}.hide();#{btnDelAuthor}.hide();#{btn_EchoHandle}.hide();");
                        //liu.shh  2012.12.18
                        ScriptManager1.AddScript("#{btn_EchoHandle}.hide();");
                    }
                }

                this.hiddenMeunUp.Text = isPass.ToString();
                this.hiddenAuthor.Text = isEdit.ToString();
                this.PowerInfoHidden.Value = PowerPageInfo.ToString();

                string deptcode = this.DeptFilter("");
                //liu.shh  2012.12.18
                if (deptcode.Equals("'-1'") && !this.IsEdit() == true && !this.IsPass() == true)
                {
                    deptcode = this.DeptCode();

                    PowerInfoHidden.Value = 0;
                }
                HttpProxy proxy = new HttpProxy();
                proxy.Method = HttpMethod.POST;
                proxy.Url = "/RLZY/WebService/DeptInfo.ashx?deptfilter=" + deptcode;
                this.Store3.Proxy.Add(proxy);

                InitContrl();
                SetCombox();
                add_mark = PowerPageInfo == 2 ? "1" : "0";
                //liu.shh  2012.12.18
                if (!this.IsEdit() == true && !this.IsPass() == true)
                {
                    this.Store1.DataSource = dal.ViewProblemListPerson("<=TO_NUMBER('" + DateTime.Now.Year.ToString() + "')", add_mark, "", "", deptcode, ((User)Session["CURRENTSTAFF"]).UserId);
                }
                else
                {
                    this.Store1.DataSource = dal.ViewProblemList("<=TO_NUMBER('" + DateTime.Now.Year.ToString() + "')", add_mark, "", "", deptcode);
                }
                this.Store1.DataBind();
            }
        }

        /// <summary>
        /// 初始化Combox
        /// </summary>
        private void SetCombox()
        {
            //开始时间
            for (int i = 0; i < 10; i++)
            {
                int years = System.DateTime.Now.Year - i;
                this.cboTime.Items.Add(new Goldnet.Ext.Web.ListItem(years.ToString(), years.ToString()));
            }
            this.cboTime.SelectedIndex = 0;
            this.TimeOrgan.SelectedIndex = 0;

            DictMainTainDal dictdal = new DictMainTainDal();
            DataTable cboDt = null;

            //作者排名字典
            cboDt = dictdal.getDictInfo("RANK_DICT", "ID", "RANK", false, "").Tables[0];
            SetCboData("ID", "RANK", cboDt, cboRanking);
            cboRanking.SelectedIndex = 0;

            //加载课题类别字典
            cboDt = dictdal.getDictInfo("PROBLEM_SORT_DICT", "ID", "PROBLEM_SORT", true, "IS_DEL").Tables[0];
            SetCboData("ID", "PROBLEM_SORT", cboDt, cboProblemSort);
            cboProblemSort.SelectedIndex = 0;

            //加载职称字典
            cboDt = dictdal.getDictInfo("JOB_DICT", "ID", "JOB", true, "IS_DEL").Tables[0];
            SetCboData("ID", "JOB", cboDt, cboPrincipalJob);
            cboPrincipalJob.SelectedIndex = 0;

            //加载学历字典
            cboDt = dictdal.getDictInfo("LEARNSUFFER_DICT", "ID", "LEARNSUFFER", true, "IS_DEL").Tables[0];
            SetCboData("SERIAL_NO", "IDENTITY_NAME", cboDt, cboPrincipalSchoolAge);
            cboPrincipalSchoolAge.SelectedIndex = 0;

            //加载经费类别字典
            cboDt = dictdal.getDictInfo("OUTLAY_SORT_DICT", "ID", "OUTLAY_SORT", true, "IS_DEL").Tables[0];
            SetCboData("ID", "OUTLAY_SORT", cboDt, cboOutlayType);
            cboOutlayType.SelectedIndex = 0;

            //加载批准单位字典
            cboDt = dictdal.getDictInfo("PASS_UNITS", "ID", "PASSED_UNIT", false, "").Tables[0];
            SetCboData("ID", "PASSED_UNIT", cboDt, cboPassedUnit);
            cboPassedUnit.SelectedIndex = 0;

            //加载等级字典
            cboDt = dictdal.getDictInfo("PROBLEM_LEVER", "ID", "LERVER", false, "").Tables[0];
            SetCboData("ID", "LERVER", cboDt, cboLerver);
            cboLerver.SelectedIndex = 0;
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

        [AjaxMethod]
        public void ProblemListAjaxOper(string Id, string deptCode, string ProblemCode, string ProblemSort, string StartDate, string OutlayType, string Principal, string
                                                                    PrincipalSpeciality, string PassedUnit, string Unit, string Conent, string ProblemName, string Years, string EndDate, string OutlayNum, string
                                                                    PrincipalSchoolAge, string PrincipalJob, string Lerver, string optype, string deptName, string mark_sug, string setup_sug)
        {
            switch (optype)
            {
                case "1":
                    string mark = PowerPageInfo == 2 ? "3" : "1";
                    string Creater = ((User)Session["CURRENTSTAFF"]).UserId == null ? "NotUserId" : ((User)Session["CURRENTSTAFF"]).UserId;
                    dal.InsertProblem(deptCode, this.DeptCodeCombo.SelectedItem.Text, ProblemName, ProblemSort, Years, Principal, PrincipalSpeciality,
                                      PrincipalSchoolAge, PrincipalJob, Unit, "", Conent, ProblemCode, OutlayType, StartDate, EndDate,
                                      OutlayNum, DateTime.Now.ToString("yyyy-MM-dd"), Creater, mark, PassedUnit, Lerver, ((User)Session["CURRENTSTAFF"]).UserId);
                    break;
                case "2":
                    dal.UpdataProblemList(Id, deptCode, deptName, ProblemName, ProblemSort, Years, Principal,
                                          PrincipalSpeciality, PrincipalSchoolAge, PrincipalJob, Unit, Conent, ProblemCode, OutlayType, StartDate,
                                          EndDate, OutlayNum, "0", PassedUnit, Lerver, mark_sug, setup_sug);
                    break;
                case "3":
                    dal.EchoDeleteHandle("PROBLEM_INFO", "ID", Id);
                    break;
                case "4":
                    dal.EchoUpDataHandle("PROBLEM_INFO", "ADD_MARK", "ID", Id, "0");
                    break;
                case "5":
                    dal.UpdataProblemList(Id, deptCode, deptName, ProblemName, ProblemSort, Years, Principal,
                                          PrincipalSpeciality, PrincipalSchoolAge, PrincipalJob, Unit, Conent, ProblemCode, OutlayType, StartDate,
                                          EndDate, OutlayNum, "1", PassedUnit, Lerver, mark_sug, setup_sug);
                    break;
                case "6":
                    dal.EchoUpDataHandle("PROBLEM_INFO", "ADD_MARK", "ID", Id, "1");
                    break;
                case "7":
                    dal.UpdataProblemList(Id, deptCode, deptName, ProblemName, ProblemSort, Years, Principal,
                                          PrincipalSpeciality, PrincipalSchoolAge, PrincipalJob, Unit, Conent, ProblemCode, OutlayType, StartDate,
                                          EndDate, OutlayNum, "3", PassedUnit, Lerver, mark_sug, setup_sug);
                    break;
                case "8":
                    dal.UpdataProblemList(Id, deptCode, this.DeptCodeCombo.SelectedItem.Text, ProblemName, ProblemSort, Years, Principal,
                                          PrincipalSpeciality, PrincipalSchoolAge, PrincipalJob, Unit, Conent, ProblemCode, OutlayType, StartDate,
                                          EndDate, OutlayNum, "3", PassedUnit, Lerver, mark_sug, setup_sug);
                    break;
                case "-1":
                    dal.UpdataProblemList(Id, deptCode, deptName, ProblemName, ProblemSort, Years, Principal,
                                          PrincipalSpeciality, PrincipalSchoolAge, PrincipalJob, Unit, Conent, ProblemCode, OutlayType, StartDate,
                                          EndDate, OutlayNum, "-1", PassedUnit, Lerver, mark_sug, setup_sug);
                    break;
            }
        }



        [AjaxMethod]
        public void AuthorDataBaseAjaxOper(string OperCode, string staffid, string name, string AuthorRanking, string Remarks, string optype)
        {
            switch (optype)
            {
                case "1":
                    dal.InsertAuthor("PROBLEM_CODE", "PROBLEM_RANK", OperCode, staffid, name, Remarks, AuthorRanking);
                    break;
                case "2":
                    dal.DeleteAuthor("PROBLEM_RANK", OperCode);
                    break;
            }
        }

        /// <summary>
        /// 刷新数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void Data_RefreshData(object sender, StoreRefreshDataEventArgs e)
        {
            InitAddMark();
            //liu.shh  2012.12.18
            if (!this.IsEdit() == true && !this.IsPass() == true)
            {
                this.Store1.DataSource = dal.ViewProblemListPerson(this.TimeOrgan.SelectedItem.Value + "TO_NUMBER('" + DateTime.Now.Year.ToString() + "')", add_mark, this.DeptCodeCombo.SelectedItem.Value, this.txtPrName.Text, this.DeptCode(), ((User)Session["CURRENTSTAFF"]).UserId);
            }
            else
            {
                this.Store1.DataSource = dal.ViewProblemList(this.TimeOrgan.SelectedItem.Value + "TO_NUMBER('" + DateTime.Now.Year.ToString() + "')", add_mark, this.DeptCodeCombo.SelectedItem.Value, this.txtPrName.Text, this.DeptFilter(""));
            }
            this.Store1.DataBind();
        }



        /// <summary>
        /// 刷新数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void RefreshData(object sender, StoreRefreshDataEventArgs e)
        {
            this.Store.DataSource = dal.ViewAuthor(this.HiddenId.Value.ToString(), "PROBLEM_CODE", this.HiddenId.Value.ToString(), "PROBLEM_RANK");
            this.Store.DataBind();
        }

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
            //string mark = PowerPageInfo == 2 ? "3" : "0";
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
    }
}
