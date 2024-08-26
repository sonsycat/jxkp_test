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
    public partial class DiscourseList : PageBase
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
                //this.btnDelAuthor.Visible = this.IsEdit();
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
                    this.Store1.DataSource = dal.ViewDiscourseListPerson("<=" + DateTime.Now.Year.ToString() + "", add_mark, "", "全部", deptcode, "全部", ((User)Session["CURRENTSTAFF"]).UserId);
                }
                else
                {
                    this.Store1.DataSource = dal.ViewDiscourseList("<=" + DateTime.Now.Year.ToString() + "", add_mark, "", "全部", deptcode, "全部");
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

            //加载刊物等级字典信息
            cboDt = dictdal.getDictInfo("PUBLICATION_GRADE_DICT", "ID", "PUBLICATION_GRADE", true, "IS_DEL").Tables[0];
            cboSearchPublicationGrade.Items.Add(new Goldnet.Ext.Web.ListItem("全部", "全部"));
            SetCboData("ID", "PUBLICATION_GRADE", cboDt, cboSearchPublicationGrade);
            cboSearchPublicationGrade.SelectedIndex = 0;

            SetCboData("ID", "PUBLICATION_GRADE", cboDt, cboPublicationGrade);
            cboPublicationGrade.SelectedIndex = 0;

            //加载论文分级字典
            cboDt = dictdal.getDictInfo("TREAT_GRADE_DICT", "SERIAL_NO", "GRADE_NAME", false, "IS_DEL").Tables[0];
            SetCboData("ID", "FRUIT_QUALITY", cboDt, cboDiscouLevel);
            cboDiscouLevel.SelectedIndex = 0;

            //加载担任职务字典
            cboDt = dictdal.getDictInfo("PAPER_DUTY_DICT", "SERIAL_NO", "PAPER_DUTY_NAME", false, "IS_DEL").Tables[0];
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
        public void DiscourseListAjaxOper(string Id, string optype)
        {
            switch (optype)
            {
                case "3":
                    dal.EchoDeleteHandle("DISCOURSE", "ID", Id);
                    break;
                case "4":
                    dal.EchoUpDataHandle("DISCOURSE", "ADD_MARK", "ID", Id, "0");
                    break;
                case "6":
                    dal.EchoUpDataHandle("DISCOURSE", "ADD_MARK", "ID", Id, "1");
                    break;
            }
        }



        [AjaxMethod]
        public void AuthorDataBaseAjaxOper(string OperCode, string staffid, string name, string AuthorRanking, string Remarks, string optype)
        {
            switch (optype)
            {
                case "1":
                    dal.InsertAuthor("DISCOURSE_CODE", "DISCOURSE_RANK", OperCode, staffid, name, Remarks, AuthorRanking);
                    break;
                case "2":
                    dal.DeleteAuthor("DISCOURSE_RANK", OperCode);
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
                this.Store1.DataSource = dal.ViewDiscourseListPerson(this.TimeOrgan.SelectedItem.Value + "" + DateTime.Now.Year.ToString() + "", add_mark, this.DeptCodeCombo.SelectedItem.Value, this.cboSearchPublicationGrade.SelectedItem.Text, this.DeptCode(), this.cboSearchIsSource.SelectedItem.Text, ((User)Session["CURRENTSTAFF"]).UserId);
            }
            else
            {
                this.Store1.DataSource = dal.ViewDiscourseList(this.TimeOrgan.SelectedItem.Value + "" + DateTime.Now.Year.ToString() + "", add_mark, this.DeptCodeCombo.SelectedItem.Value, this.cboSearchPublicationGrade.SelectedItem.Text, this.DeptFilter(""), this.cboSearchIsSource.SelectedItem.Text);
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
            this.Store.DataSource = dal.ViewAuthor(this.HiddenId.Value.ToString(), "DISCOURSE_CODE", this.HiddenId.Value.ToString(), "DISCOURSE_RANK");
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
            dal.InsertDiscourseList(this.DeptCodeCombo.SelectedItem.Value, this.DeptCodeCombo.SelectedItem.Text, this.NumYears.Text, this.txtPublicationName.Text,
                                    this.txtDiscourseTitle.Text, this.txtDiscourseSubject.Text, this.cboPublicationGrade.SelectedItem.Text, this.NumYear.Text,
                                    this.txtBook.Text, this.txtExpect.Text, this.txtAuthor.Text, this.cboIsSource.SelectedItem.Text, this.cboDiscouLevel.SelectedItem.Text,
                                    this.txtMagaNo.Text, this.cboDuty.SelectedItem.Text, this.txtMeetName.Text, this.dtfMeetDate.SelectedDate.ToString("yyyy-MM-dd"),
                                    this.txtPublish.Text, this.dtfPublishDate.SelectedDate.ToString("yyyy-MM-dd"), Creater, mark, ((User)Session["CURRENTSTAFF"]).UserId);

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
            dal.UpdateDiscourseList(HiddenId.Text, deptcode, deptName, this.NumYears.Text, this.txtPublicationName.Text,
                                        this.txtDiscourseTitle.Text, this.txtDiscourseSubject.Text, this.cboPublicationGrade.SelectedItem.Text, this.NumYear.Text,
                                        this.txtBook.Text, this.txtExpect.Text, this.txtAuthor.Text, this.cboIsSource.SelectedItem.Text, this.cboDiscouLevel.SelectedItem.Text,
                                        this.txtMagaNo.Text, this.cboDuty.SelectedItem.Text, this.txtMeetName.Text, this.dtfMeetDate.SelectedDate.ToString("yyyy-MM-dd"),
                                        this.txtPublish.Text, this.dtfPublishDate.SelectedDate.ToString("yyyy-MM-dd"), "0", this.txrSug.Text, this.txrSetSug.Text);

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
            dal.UpdateDiscourseList(HiddenId.Text, deptcode, deptName, this.NumYears.Text, this.txtPublicationName.Text,
                                     this.txtDiscourseTitle.Text, this.txtDiscourseSubject.Text, this.cboPublicationGrade.SelectedItem.Text, this.NumYear.Text,
                                     this.txtBook.Text, this.txtExpect.Text, this.txtAuthor.Text, this.cboIsSource.SelectedItem.Text, this.cboDiscouLevel.SelectedItem.Text,
                                     this.txtMagaNo.Text, this.cboDuty.SelectedItem.Text, this.txtMeetName.Text, this.dtfMeetDate.SelectedDate.ToString("yyyy-MM-dd"),
                                     this.txtPublish.Text, this.dtfPublishDate.SelectedDate.ToString("yyyy-MM-dd"), "1", this.txrSug.Text, this.txrSetSug.Text);

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
            dal.UpdateDiscourseList(HiddenId.Text, deptcode, deptName, this.NumYears.Text, this.txtPublicationName.Text,
                                     this.txtDiscourseTitle.Text, this.txtDiscourseSubject.Text, this.cboPublicationGrade.SelectedItem.Text, this.NumYear.Text,
                                     this.txtBook.Text, this.txtExpect.Text, this.txtAuthor.Text, this.cboIsSource.SelectedItem.Text, this.cboDiscouLevel.SelectedItem.Text,
                                     this.txtMagaNo.Text, this.cboDuty.SelectedItem.Text, this.txtMeetName.Text, this.dtfMeetDate.SelectedDate.ToString("yyyy-MM-dd"),
                                     this.txtPublish.Text, this.dtfPublishDate.SelectedDate.ToString("yyyy-MM-dd"), "-1", this.txrSug.Text, this.txrSetSug.Text);

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
            dal.UpdateDiscourseList(HiddenId.Text, deptcode, deptName, this.NumYears.Text, this.txtPublicationName.Text,
                            this.txtDiscourseTitle.Text, this.txtDiscourseSubject.Text, this.cboPublicationGrade.SelectedItem.Text, this.NumYear.Text,
                            this.txtBook.Text, this.txtExpect.Text, this.txtAuthor.Text, this.cboIsSource.SelectedItem.Text, this.cboDiscouLevel.SelectedItem.Text,
                            this.txtMagaNo.Text, this.cboDuty.SelectedItem.Text, this.txtMeetName.Text, this.dtfMeetDate.SelectedDate.ToString("yyyy-MM-dd"),
                            this.txtPublish.Text, this.dtfPublishDate.SelectedDate.ToString("yyyy-MM-dd"), "3", this.txrSug.Text, this.txrSetSug.Text);

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
            dal.InsertDiscourseList(this.DeptCodeCombo.SelectedItem.Value, this.DeptCodeCombo.SelectedItem.Text, this.NumYears.Text, this.txtPublicationName.Text,
                                    this.txtDiscourseTitle.Text, this.txtDiscourseSubject.Text, this.cboPublicationGrade.SelectedItem.Text, this.NumYear.Text,
                                    this.txtBook.Text, this.txtExpect.Text, this.txtAuthor.Text, this.cboIsSource.SelectedItem.Text, this.cboDiscouLevel.SelectedItem.Text,
                                    this.txtMagaNo.Text, this.cboDuty.SelectedItem.Text, this.txtMeetName.Text, this.dtfMeetDate.SelectedDate.ToString("yyyy-MM-dd"),
                                    this.txtPublish.Text, this.dtfPublishDate.SelectedDate.ToString("yyyy-MM-dd"), Creater, "0", ((User)Session["CURRENTSTAFF"]).UserId);

            Goldnet.Ext.Web.ScriptManager scManager = Goldnet.Ext.Web.ScriptManager.GetInstance(this.Page);
            scManager.AddScript("Store1.reload();");
            scManager.AddScript("btn_EchoHandle.setDisabled(true);");
            scManager.AddScript("btn_Delete.setDisabled(true);");
            scManager.AddScript("arcEditWindow.hide();");
        }




        private ArrayList ContrlJuge()
        {
            ArrayList l_ar = new ArrayList();

            //txtAuthor" //负责人
            //txtPublicationName" //刊物名称
            //txtDiscourseTitle" //论文题目
            //txtDiscourseSubject" //论文栏目
            //cboPublicationGrade" //刊物等级
            //txtMeetName" //会议名称
            //txtPublish" //出版社
            //cboIsSource" //是否统计源期刊">cboYears" //年度
            //cboDiscouLevel" //论文级别
            //txtMagaNo" //刊期号
            //cboYear" //年
            //txtExpect" //期
            //txtBook" //卷
            //cboDuty" //担任职务
            //dtfPublishDate" //出版时间
            //dtfMeetDate" //会议时间
            return l_ar;
        }
    }
}
