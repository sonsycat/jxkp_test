using System;
using System.Collections;
using System.Configuration;
using System.Data;
using Goldnet.Dal;
using Goldnet.Ext.Web;
using GoldNet.Comm;
using GoldNet.Model;

namespace GoldNet.JXKP.RLZY.BaseInfoManager
{
    public partial class MonographList : PageBase
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

                string deptCode = this.DeptFilter("");
                //liu.shh  2012.12.18
                if (deptCode.Equals("'-1'") && !this.IsEdit() == true && !this.IsPass() == true)
                {
                    deptCode = this.DeptCode();
                    PowerInfoHidden.Value = 0;
                }
                HttpProxy proxy = new HttpProxy();
                proxy.Method = HttpMethod.POST;
                proxy.Url = "/RLZY/WebService/DeptInfo.ashx?deptfilter=" + deptCode;
                this.Store3.Proxy.Add(proxy);

                InitContrl();
                SetCombox();
                add_mark = PowerPageInfo == 2 ? "1" : "0";
                //liu.shh  2012.12.18
                if (!IsEdit() == true && !IsPass() == true)
                {
                    this.Store1.DataSource = dal.ViewMonographListPerson("<='" + DateTime.Now.Year.ToString() + "'", add_mark, "", deptCode, ((User)Session["CURRENTSTAFF"]).UserId);
                }
                else
                {
                    this.Store1.DataSource = dal.ViewMonographList("<='" + DateTime.Now.Year.ToString() + "'", add_mark, "", deptCode);
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

            //加载开本字典
            cboDt = dictdal.getDictInfo("FORMAT_DICT", "ID", "FORMAT", true, "IS_DEL").Tables[0];
            SetCboData("ID", "FORMAT", cboDt, cboFormat);
            cboFormat.SelectedIndex = 0;

            //加载论文分级字典
            cboDt = dictdal.getDictInfo("TREAT_GRADE_DICT", "SERIAL_NO", "GRADE_NAME", false, "").Tables[0];
            SetCboData("SERIAL_NO", "GRADE_NAME", cboDt, cboDiscouLevel);
            cboDiscouLevel.SelectedIndex = 0;

            //加载担任职务字典
            cboDt = dictdal.getDictInfo("PAPER_DUTY_DICT", "SERIAL_NO", "PAPER_DUTY_NAME", false, "").Tables[0];
            SetCboData("SERIAL_NO", "PAPER_DUTY_NAME", cboDt, cboDuty);
            cboDuty.SelectedIndex = 0;


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
                    dal.EchoDeleteHandle("MONOGRAPH", "ID", Id);
                    break;
                case "4":
                    dal.EchoUpDataHandle("MONOGRAPH", "ADD_MARK", "ID", Id, "0");
                    break;
                case "6":
                    dal.EchoUpDataHandle("MONOGRAPH", "ADD_MARK", "ID", Id, "1");
                    break;
            }
        }



        [AjaxMethod]
        public void AuthorDataBaseAjaxOper(string OperCode, string staffid, string name, string AuthorRanking, string Remarks, string optype)
        {
            switch (optype)
            {
                case "1":
                    dal.InsertAuthor("MONOGRAPH_CODE", "MONOGRAPH_RANK", OperCode, staffid, name, Remarks, AuthorRanking);
                    break;
                case "2":
                    dal.DeleteAuthor("MONOGRAPH_RANK", OperCode);
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
            if (!IsEdit() == true && !IsPass() == true)
            {
                this.Store1.DataSource = dal.ViewMonographListPerson(this.TimeOrgan.SelectedItem.Value + "'" + DateTime.Now.Year.ToString() + "'", add_mark, this.DeptCodeCombo.SelectedItem.Value, this.DeptCode(), ((User)Session["CURRENTSTAFF"]).UserId);
            }
            else
            {
                this.Store1.DataSource = dal.ViewMonographList(this.TimeOrgan.SelectedItem.Value + "'" + DateTime.Now.Year.ToString() + "'", add_mark, this.DeptCodeCombo.SelectedItem.Value, this.DeptFilter(""));
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
            this.Store.DataSource = dal.ViewAuthor(this.HiddenId.Value.ToString(), "MONOGRAPH_CODE", this.HiddenId.Value.ToString(), "MONOGRAPH_RANK");
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
            dal.InsertMonographlist(this.DeptCodeCombo.SelectedItem.Value, this.DeptCodeCombo.SelectedItem.Text, this.txtMonographName.Text, this.txtPublish.Text,
                                    this.dtfPublishDate.SelectedDate.ToString("yyyy-MM-dd"), this.txtWordCount.Text, this.txtCallNumber.Text, this.cboFormat.SelectedItem.Text,
                                    this.txtAmount.Text, this.txtAuthor.Text, this.txrContent.Text, this.cboDiscouLevel.SelectedItem.Text,
                                    this.txtMagaNo.Text, this.cboDuty.SelectedItem.Text, this.txtMeetName.Text, this.dtfMeetDate.SelectedDate.ToString("yyyy-MM-dd"),
                                    this.txtMagaName.Text, DateTime.Now.ToString("yyyy-MM-dd"), Creater, mark, ((User)Session["CURRENTSTAFF"]).UserId);

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
            dal.UpdateMonographList(this.HiddenId.Text, deptcode, deptName, this.txtMonographName.Text, this.txtPublish.Text,
                                    this.dtfPublishDate.SelectedDate.ToString("yyyy-MM-dd"), this.txtWordCount.Text, this.txtCallNumber.Text, this.cboFormat.SelectedItem.Text,
                                    this.txtAmount.Text, this.txtAuthor.Text, this.txrContent.Text, this.cboDiscouLevel.SelectedItem.Text,
                                    this.txtMagaNo.Text, this.cboDuty.SelectedItem.Text, this.txtMeetName.Text, this.dtfMeetDate.SelectedDate.ToString("yyyy-MM-dd"),
                                    this.txtMagaName.Text, "0", this.txrSug.Text, this.txrSetSug.Text);
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
            dal.UpdateMonographList(this.HiddenId.Text, deptcode, deptName, this.txtMonographName.Text, this.txtPublish.Text,
                                    this.dtfPublishDate.SelectedDate.ToString("yyyy-MM-dd"), this.txtWordCount.Text, this.txtCallNumber.Text, this.cboFormat.SelectedItem.Text,
                                    this.txtAmount.Text, this.txtAuthor.Text, this.txrContent.Text, this.cboDiscouLevel.SelectedItem.Text,
                                    this.txtMagaNo.Text, this.cboDuty.SelectedItem.Text, this.txtMeetName.Text, this.dtfMeetDate.SelectedDate.ToString("yyyy-MM-dd"),
                                    this.txtMagaName.Text, "1", this.txrSug.Text, this.txrSetSug.Text);
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
            dal.UpdateMonographList(this.HiddenId.Text, deptcode, deptName, this.txtMonographName.Text, this.txtPublish.Text,
                                    this.dtfPublishDate.SelectedDate.ToString("yyyy-MM-dd"), this.txtWordCount.Text, this.txtCallNumber.Text, this.cboFormat.SelectedItem.Text,
                                    this.txtAmount.Text, this.txtAuthor.Text, this.txrContent.Text, this.cboDiscouLevel.SelectedItem.Text,
                                    this.txtMagaNo.Text, this.cboDuty.SelectedItem.Text, this.txtMeetName.Text, this.dtfMeetDate.SelectedDate.ToString("yyyy-MM-dd"),
                                    this.txtMagaName.Text, "-1", this.txrSug.Text, this.txrSetSug.Text);
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
            dal.UpdateMonographList(this.HiddenId.Text, deptcode, deptName, this.txtMonographName.Text, this.txtPublish.Text,
                                   this.dtfPublishDate.SelectedDate.ToString("yyyy-MM-dd"), this.txtWordCount.Text, this.txtCallNumber.Text, this.cboFormat.SelectedItem.Text,
                                   this.txtAmount.Text, this.txtAuthor.Text, this.txrContent.Text, this.cboDiscouLevel.SelectedItem.Text,
                                   this.txtMagaNo.Text, this.cboDuty.SelectedItem.Text, this.txtMeetName.Text, this.dtfMeetDate.SelectedDate.ToString("yyyy-MM-dd"),
                                   this.txtMagaName.Text, "3", this.txrSug.Text, this.txrSetSug.Text);
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
            dal.InsertMonographlist(this.DeptCodeCombo.SelectedItem.Value, this.DeptCodeCombo.SelectedItem.Text, this.txtMonographName.Text, this.txtPublish.Text,
                         this.dtfPublishDate.SelectedDate.ToString("yyyy-MM-dd"), this.txtWordCount.Text, this.txtCallNumber.Text, this.cboFormat.SelectedItem.Text,
                         this.txtAmount.Text, this.txtAuthor.Text, this.txrContent.Text, this.cboDiscouLevel.SelectedItem.Text,
                         this.txtMagaNo.Text, this.cboDuty.SelectedItem.Text, this.txtMeetName.Text, this.dtfMeetDate.SelectedDate.ToString("yyyy-MM-dd"),
                         this.txtMagaName.Text, DateTime.Now.ToString("yyyy-MM-dd"), Creater, "0", ((User)Session["CURRENTSTAFF"]).UserId);
            Goldnet.Ext.Web.ScriptManager scManager = Goldnet.Ext.Web.ScriptManager.GetInstance(this.Page);
            scManager.AddScript("Store1.reload();");
            scManager.AddScript("btn_EchoHandle.setDisabled(true);");
            scManager.AddScript("btn_Delete.setDisabled(true);");
            scManager.AddScript("arcEditWindow.hide();");
        }




        private ArrayList ContrlJuge()
        {
            ArrayList l_ar = new ArrayList();
            // 专著名称  期刊号  作者 出版日期
            return l_ar;
        }
    }
}
