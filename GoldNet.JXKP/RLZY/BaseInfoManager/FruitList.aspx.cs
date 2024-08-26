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
    public partial class FruitList : PageBase
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
                        //this.hiddenEdit.Text = "1";
                    }
                }

                this.hiddenMeunUp.Text = isPass.ToString();
                this.hiddenAuthor.Text = isEdit.ToString();

                this.PowerInfoHidden.Value = PowerPageInfo.ToString();

                //liu.shh  2012.12.18
                string deptcode = this.DeptFilter("");
                if (deptcode.Equals("'-1'") && !this.IsEdit() == true && !this.IsPass() == true)
                {
                    deptcode = this.DeptCode();
                    PowerInfoHidden.Value = 0;
                }

                HttpProxy proxy = new HttpProxy();
                proxy.Method = HttpMethod.POST;
                if (!this.IsEdit() == true && !this.IsPass() == true)
                {
                    proxy.Url = "/RLZY/WebService/DeptInfo.ashx?deptfilter=" + deptcode;
                }
                else
                {
                    proxy.Url = "/RLZY/WebService/DeptInfo.ashx?deptfilter=" + this.DeptFilter("");
                }
                this.Store3.Proxy.Add(proxy);

                InitContrl();
                SetCombox();
                add_mark = PowerPageInfo == 2 ? "1" : "0";
                //liu.shh  2012.12.18
                if (!this.IsEdit() == true && !this.IsPass() == true)
                {
                    this.Store1.DataSource = dal.ViewFruitListPerson("<='" + DateTime.Now.Year.ToString() + "'", add_mark, "", "全部", deptcode, ((User)Session["CURRENTSTAFF"]).UserId);
                }
                else
                {
                    this.Store1.DataSource = dal.ViewFruitList("<='" + DateTime.Now.Year.ToString() + "'", add_mark, "", "全部", deptcode);
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

            //获奖类别等级字典
            cboDt = dictdal.getDictInfo("GRADE_DICT", "ID", "GRADE", true, "IS_DEL").Tables[0];
            cboThePalmGrade.Items.Add(new Goldnet.Ext.Web.ListItem("全部", "全部"));
            SetCboData("ID", "GRADE", cboDt, cboThePalmGrade);
            cboThePalmGrade.SelectedIndex = 0;

            SetCboData("ID", "GRADE", cboDt, cboBearThePalmGrade);
            cboBearThePalmGrade.SelectedIndex = 0;

            //加载成果性质字典
            cboDt = dictdal.getDictInfo("FRUIT_QUALITY_DICT", "ID", "FRUIT_QUALITY", true, "IS_DEL").Tables[0];
            SetCboData("ID", "FRUIT_QUALITY", cboDt, cboFruitKind);
            cboFruitKind.SelectedIndex = 0;

            //加载职称字典
            cboDt = dictdal.getDictInfo("JOB_DICT", "ID", "JOB", true, "IS_DEL").Tables[0];
            SetCboData("ID", "JOB", cboDt, cboMostlyPersonsJob);
            cboMostlyPersonsJob.SelectedIndex = 0;

            //加载学历字典
            cboDt = dictdal.getDictInfo("LEARNSUFFER_DICT", "ID", "LEARNSUFFER", true, "IS_DEL").Tables[0];
            SetCboData("SERIAL_NO", "IDENTITY_NAME", cboDt, cboMostlyPersonsSchoolAge);
            cboMostlyPersonsSchoolAge.SelectedIndex = 0;

            //加载推广应用范围字典
            cboDt = dictdal.getDictInfo("EXTEND_APPL_DICT", "SERIAL_NO", "APPL_NAME", false, "").Tables[0];
            SetCboData("SERIAL_NO", "APPL_NAME", cboDt, cboExtendAppBound);
            cboExtendAppBound.SelectedIndex = 0;

            //加载批准单位字典
            cboDt = dictdal.getDictInfo("SEAL_UNIT_DICT", "SERIAL_NO", "UNIT_NAME", false, "").Tables[0];
            SetCboData("SERIAL_NO", "UNIT_NAME", cboDt, cboAuthUnit);
            cboAuthUnit.SelectedIndex = 0;

            //加载任务来源字典
            cboDt = dictdal.getDictInfo("TASK_SOURCE_DICT", "ID", "TASK_SOURCE", true, "IS_DEL").Tables[0];
            SetCboData("ID", "TASK_SOURCE", cboDt, cboTaskSource);
            cboTaskSource.SelectedIndex = 0;


            //加载专利类别字典
            cboDt = dictdal.getDictInfo("PATENT_TYPE_DICT", "SERIAL_NO", "TYPE_NAME", false, "").Tables[0];
            SetCboData("SERIAL_NO", "TYPE_NAME", cboDt, cboPatentSort);
            cboPatentSort.SelectedIndex = 0;


            //作者排名字典
            cboDt = dictdal.getDictInfo("RANK_DICT", "ID", "RANK", false, "").Tables[0];
            SetCboData("ID", "RANK", cboDt, cboRanking);
            cboRanking.SelectedIndex = 0;

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
        public void FruitListAjaxOper(string Id, string optype)
        {
            switch (optype)
            {
                case "3":
                    dal.EchoDeleteHandle("FRUIT", "ID", Id);
                    break;
                case "4":
                    dal.EchoUpDataHandle("FRUIT", "ADD_MARK", "ID", Id, "0");
                    break;
                case "6":
                    dal.EchoUpDataHandle("FRUIT", "ADD_MARK", "ID", Id, "1");
                    break;
            }
        }



        [AjaxMethod]
        public void AuthorDataBaseAjaxOper(string OperCode, string staffid, string name, string AuthorRanking, string Remarks, string optype)
        {
            switch (optype)
            {
                case "1":
                    dal.InsertAuthor("FRUIT_CODE", "FUIT_RANK", OperCode, staffid, name, Remarks, AuthorRanking);
                    break;
                case "2":
                    dal.DeleteAuthor("FUIT_RANK", OperCode);
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
                this.Store1.DataSource = dal.ViewFruitListPerson(this.TimeOrgan.SelectedItem.Value + "'" + DateTime.Now.Year.ToString() + "'", add_mark, this.DeptCodeCombo.SelectedItem.Value, this.cboThePalmGrade.SelectedItem.Text, this.DeptCode(), ((User)Session["CURRENTSTAFF"]).UserId);
            }
            else
            {
                this.Store1.DataSource = dal.ViewFruitList(this.TimeOrgan.SelectedItem.Value + "'" + DateTime.Now.Year.ToString() + "'", add_mark, this.DeptCodeCombo.SelectedItem.Value, this.cboThePalmGrade.SelectedItem.Text, this.DeptFilter(""));
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
            this.Store.DataSource = dal.ViewAuthor(this.HiddenId.Value.ToString(), "FRUIT_CODE", this.HiddenId.Value.ToString(), "FUIT_RANK");
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

        /// <summary>
        /// 保存按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void SaveInfo(object sender, AjaxEventArgs e)
        {
            string mark = PowerPageInfo == 2 ? "3" : "1";
            string Creater = ((User)Session["CURRENTSTAFF"]).UserId == null ? "NotUserId" : ((User)Session["CURRENTSTAFF"]).UserId;
            dal.InsertFruitList(this.DeptCodeCombo.SelectedItem.Value, this.DeptCodeCombo.SelectedItem.Text, this.txtFruitName.Text, this.txtMostlyUnit.Text,
                                this.txtMostlyPersons.Text, this.cboMostlyPersonsSchoolAge.SelectedItem.Text, cboMostlyPersonsJob.SelectedItem.Text,
                                "", this.dtfBearThePalmDate.SelectedDate.ToString("yyyy-MM-dd"), this.cboBearThePalmGrade.SelectedItem.Text, this.cboFruitKind.SelectedItem.Text,
                                this.cboTaskSource.SelectedItem.Text, this.txtPatent.Text, this.txtNewReadNumber.Text, this.txtThematic.Text,
                                this.txtSummary.Text, this.txtFruitCode.Text, this.cboAuthUnit.SelectedItem.Text, this.cboIsextendApp.SelectedItem.Text,
                                this.cboExtendAppBound.SelectedItem.Text, this.cboPatentSort.SelectedItem.Text, this.txtExtendIncome.Text, this.txtPatentIncome.Text,
                                DateTime.Now.ToString("yyyy-MM-dd"), Creater, mark, ((User)Session["CURRENTSTAFF"]).UserId);

            Goldnet.Ext.Web.ScriptManager scManager = Goldnet.Ext.Web.ScriptManager.GetInstance(this.Page);
            scManager.AddScript("Store1.reload();");
            scManager.AddScript("btn_EchoHandle.setDisabled(true);");
            scManager.AddScript("btn_Delete.setDisabled(true);");
            scManager.AddScript("arcEditWindow.hide();");
        }

        /// <summary>
        /// 提交按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void SetUpInfo(object sender, AjaxEventArgs e)
        {
            string deptcode = e.ExtraParams["deptCode"].ToString();
            string deptName = e.ExtraParams["deptName"].ToString();
            dal.UpdateFruitList(this.HiddenId.Text, deptcode, deptName, this.txtFruitName.Text, this.txtMostlyUnit.Text,
                                this.txtMostlyPersons.Text, this.cboMostlyPersonsSchoolAge.SelectedItem.Text, cboMostlyPersonsJob.SelectedItem.Text, "",
                                this.dtfBearThePalmDate.SelectedDate.ToString("yyyy-MM-dd"), this.cboBearThePalmGrade.SelectedItem.Text, this.cboFruitKind.SelectedItem.Text,
                                this.cboTaskSource.SelectedItem.Text, this.txtPatent.Text, this.txtNewReadNumber.Text, this.txtThematic.Text,
                                this.txtSummary.Text, this.txtFruitCode.Text, this.cboAuthUnit.SelectedItem.Text, this.cboIsextendApp.SelectedItem.Text,
                                this.cboExtendAppBound.SelectedItem.Text, this.cboPatentSort.SelectedItem.Text, this.txtExtendIncome.Text, this.txtPatentIncome.Text,
                                "0", this.txrSug.Text, this.txrSetSug.Text);

            Goldnet.Ext.Web.ScriptManager scManager = Goldnet.Ext.Web.ScriptManager.GetInstance(this.Page);
            scManager.AddScript("Store1.reload();");
            scManager.AddScript("btn_EchoHandle.setDisabled(true);");
            scManager.AddScript("btn_Delete.setDisabled(true);");
            scManager.AddScript("arcEditWindow.hide();");
        }

        /// <summary>
        /// 审批按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ApproveInfo(object sender, AjaxEventArgs e)
        {
            string deptcode = e.ExtraParams["deptCode"].ToString();
            string deptName = e.ExtraParams["deptName"].ToString();
            dal.UpdateFruitList(this.HiddenId.Text, deptcode, deptName, this.txtFruitName.Text, this.txtMostlyUnit.Text,
                                this.txtMostlyPersons.Text, this.cboMostlyPersonsSchoolAge.SelectedItem.Text, cboMostlyPersonsJob.SelectedItem.Text, "",
                                this.dtfBearThePalmDate.SelectedDate.ToString("yyyy-MM-dd"), this.cboBearThePalmGrade.SelectedItem.Text, this.cboFruitKind.SelectedItem.Text,
                                this.cboTaskSource.SelectedItem.Text, this.txtPatent.Text, this.txtNewReadNumber.Text, this.txtThematic.Text,
                                this.txtSummary.Text, this.txtFruitCode.Text, this.cboAuthUnit.SelectedItem.Text, this.cboIsextendApp.SelectedItem.Text,
                                this.cboExtendAppBound.SelectedItem.Text, this.cboPatentSort.SelectedItem.Text, this.txtExtendIncome.Text, this.txtPatentIncome.Text,
                                "1", this.txrSug.Text, this.txrSetSug.Text);

            Goldnet.Ext.Web.ScriptManager scManager = Goldnet.Ext.Web.ScriptManager.GetInstance(this.Page);
            scManager.AddScript("Store1.reload();");
            scManager.AddScript("btn_EchoHandle.setDisabled(true);");
            scManager.AddScript("btn_Delete.setDisabled(true);");
            scManager.AddScript("arcEditWindow.hide();");
        }

        /// <summary>
        /// 审批不通过按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void NotApproveInfo(object sender, AjaxEventArgs e)
        {
            string deptcode = e.ExtraParams["deptCode"].ToString();
            string deptName = e.ExtraParams["deptName"].ToString();
            dal.UpdateFruitList(this.HiddenId.Text, deptcode, deptName, this.txtFruitName.Text, this.txtMostlyUnit.Text,
                                this.txtMostlyPersons.Text, this.cboMostlyPersonsSchoolAge.SelectedItem.Text, cboMostlyPersonsJob.SelectedItem.Text, "",
                                this.dtfBearThePalmDate.SelectedDate.ToString("yyyy-MM-dd"), this.cboBearThePalmGrade.SelectedItem.Text, this.cboFruitKind.SelectedItem.Text,
                                this.cboTaskSource.SelectedItem.Text, this.txtPatent.Text, this.txtNewReadNumber.Text, this.txtThematic.Text,
                                this.txtSummary.Text, this.txtFruitCode.Text, this.cboAuthUnit.SelectedItem.Text, this.cboIsextendApp.SelectedItem.Text,
                                this.cboExtendAppBound.SelectedItem.Text, this.cboPatentSort.SelectedItem.Text, this.txtExtendIncome.Text, this.txtPatentIncome.Text,
                                "-1", this.txrSug.Text, this.txrSetSug.Text);

            Goldnet.Ext.Web.ScriptManager scManager = Goldnet.Ext.Web.ScriptManager.GetInstance(this.Page);
            scManager.AddScript("Store1.reload();");
            scManager.AddScript("btn_EchoHandle.setDisabled(true);");
            scManager.AddScript("btn_Delete.setDisabled(true);");
            scManager.AddScript("arcEditWindow.hide();");
        }

        /// <summary>
        /// 未提交时保存
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void SetSave(object sender, AjaxEventArgs e)
        {
            string deptcode = e.ExtraParams["deptCode"].ToString();
            string deptName = e.ExtraParams["deptName"].ToString();
            dal.UpdateFruitList(this.HiddenId.Text, deptcode, deptName, this.txtFruitName.Text, this.txtMostlyUnit.Text,
                                this.txtMostlyPersons.Text, this.cboMostlyPersonsSchoolAge.SelectedItem.Text, cboMostlyPersonsJob.SelectedItem.Text, "",
                                this.dtfBearThePalmDate.SelectedDate.ToString("yyyy-MM-dd"), this.cboBearThePalmGrade.SelectedItem.Text, this.cboFruitKind.SelectedItem.Text,
                                this.cboTaskSource.SelectedItem.Text, this.txtPatent.Text, this.txtNewReadNumber.Text, this.txtThematic.Text,
                                this.txtSummary.Text, this.txtFruitCode.Text, this.cboAuthUnit.SelectedItem.Text, this.cboIsextendApp.SelectedItem.Text,
                                this.cboExtendAppBound.SelectedItem.Text, this.cboPatentSort.SelectedItem.Text, this.txtExtendIncome.Text, this.txtPatentIncome.Text,
                                "3", this.txrSug.Text, this.txrSetSug.Text);

            Goldnet.Ext.Web.ScriptManager scManager = Goldnet.Ext.Web.ScriptManager.GetInstance(this.Page);
            scManager.AddScript("Store1.reload();");
            scManager.AddScript("btn_EchoHandle.setDisabled(true);");
            scManager.AddScript("btn_Delete.setDisabled(true);");
            scManager.AddScript("arcEditWindow.hide();");
        }

        /// <summary>
        /// 添加时直接提交
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void SaveSet(object sender, AjaxEventArgs e)
        {
            string Creater = ((User)Session["CURRENTSTAFF"]).UserId == null ? "NotUserId" : ((User)Session["CURRENTSTAFF"]).UserId;
            dal.InsertFruitList(this.DeptCodeCombo.SelectedItem.Value, this.DeptCodeCombo.SelectedItem.Text, this.txtFruitName.Text, this.txtMostlyUnit.Text,
                                this.txtMostlyPersons.Text, this.cboMostlyPersonsSchoolAge.SelectedItem.Text, cboMostlyPersonsJob.SelectedItem.Text,
                                "", this.dtfBearThePalmDate.SelectedDate.ToString("yyyy-MM-dd"), this.cboBearThePalmGrade.SelectedItem.Text, this.cboFruitKind.SelectedItem.Text,
                                this.cboTaskSource.SelectedItem.Text, this.txtPatent.Text, this.txtNewReadNumber.Text, this.txtThematic.Text,
                                this.txtSummary.Text, this.txtFruitCode.Text, this.cboAuthUnit.SelectedItem.Text, this.cboIsextendApp.SelectedItem.Text,
                                this.cboExtendAppBound.SelectedItem.Text, this.cboPatentSort.SelectedItem.Text, this.txtExtendIncome.Text, this.txtPatentIncome.Text,
                                DateTime.Now.ToString("yyyy-MM-dd"), Creater, "0", ((User)Session["CURRENTSTAFF"]).UserId);

            Goldnet.Ext.Web.ScriptManager scManager = Goldnet.Ext.Web.ScriptManager.GetInstance(this.Page);
            scManager.AddScript("Store1.reload();");
            scManager.AddScript("btn_EchoHandle.setDisabled(true);");
            scManager.AddScript("btn_Delete.setDisabled(true);");
            scManager.AddScript("arcEditWindow.hide();");
        }

    }
}
