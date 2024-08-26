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
    public partial class TeciMeetList : PageBase
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
                    this.Store1.DataSource = dal.ViewTeciMeetPerson(DateTime.Now.Year.ToString() + "-01-01", DateTime.Now.ToString("yyyy-MM-dd"), add_mark, "", deptcode, ((User)Session["CURRENTSTAFF"]).UserId);
                }
                else
                {
                    this.Store1.DataSource = dal.ViewTeciMeet(DateTime.Now.Year.ToString() + "-01-01", DateTime.Now.ToString("yyyy-MM-dd"), add_mark, "", deptcode);
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
                this.Comb_StartYear.Items.Add(new Goldnet.Ext.Web.ListItem(years.ToString(), years.ToString()));
            }
            for (int i = 1; i <= 12; i++)
            {
                this.Comb_StartMonth.Items.Add(new Goldnet.Ext.Web.ListItem(i.ToString(), i.ToString()));
            }
            this.Comb_StartMonth.SelectedIndex = 0;

            for (int i = 1; i <= 31; i++)
            {
                this.Comb_StartDate.Items.Add(new Goldnet.Ext.Web.ListItem(i.ToString(), i.ToString()));
            }
            this.Comb_StartDate.SelectedIndex = 0;


            //结束时间
            for (int i = 0; i < 10; i++)
            {
                int years = System.DateTime.Now.Year - i;
                this.Comb_EndYear.Items.Add(new Goldnet.Ext.Web.ListItem(years.ToString(), years.ToString()));
            }
            for (int i = 1; i <= 12; i++)
            {
                this.Comb_EndMonth.Items.Add(new Goldnet.Ext.Web.ListItem(i.ToString(), i.ToString()));
            }
            this.Comb_EndMonth.SelectedIndex = DateTime.Now.Month - 1;


            for (int i = 1; i <= 31; i++)
            {
                this.Comb_EndDate.Items.Add(new Goldnet.Ext.Web.ListItem(i.ToString(), i.ToString()));
            }
            this.Comb_EndDate.SelectedIndex = DateTime.Now.Day - 1;

            DictMainTainDal dictdal = new DictMainTainDal();
            DataTable cboDt = null;

            //获奖类别等级字典
            cboDt = dictdal.getDictInfo("TECH_GRADE_DICT", "ID", "GRADE_NAME", false, "").Tables[0];
            SetCboData("ID", "GRADE_NAME", cboDt, cboGrade);
            cboGrade.SelectedIndex = 0;
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
                    dal.EchoDeleteHandle("TECI_MEET", "ID", Id);
                    break;
                case "4":
                    dal.EchoUpDataHandle("TECI_MEET", "ADD_MARK", "ID", Id, "0");
                    break;
                case "6":
                    dal.EchoUpDataHandle("TECI_MEET", "ADD_MARK", "ID", Id, "1");
                    break;
            }
        }



        [AjaxMethod]
        public void AuthorDataBaseAjaxOper(string OperCode, string staffid, string name, string Remarks, string optype, string MeetName)
        {
            switch (optype)
            {
                case "1":
                    dal.InsertTeciMeetJoinPerson(OperCode, MeetName, name, staffid, Remarks);
                    break;
                case "2":
                    dal.DeleteAuthor("TECI_MEET_PERSONNEL", OperCode);
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
            string dd1 = DateTimeConvert(this.Comb_StartYear.SelectedItem.Value, this.Comb_StartMonth.SelectedItem.Value, this.Comb_StartDate);
            string dd2 = DateTimeConvert(this.Comb_EndYear.SelectedItem.Value, this.Comb_EndMonth.SelectedItem.Value, this.Comb_EndDate);
            DateTime time1 = default(DateTime);
            DateTime time2 = default(DateTime);
            if ((!DateTime.TryParse(dd1, out time1)) || (!DateTime.TryParse(dd2, out time2)))
            {
                Goldnet.Ext.Web.Ext.Msg.Show(new Goldnet.Ext.Web.MessageBox.Config
                {
                    Title = "信息提示",
                    Message = "日期不正确,请重新选择条件",
                    Buttons = MessageBox.Button.OK,
                    Icon = (MessageBox.Icon)Enum.Parse(typeof(MessageBox.Icon), "INFO")
                });
                return;
            }
            if (dd1.CompareTo(dd2) > 0)
            {
                Goldnet.Ext.Web.Ext.Msg.Show(new Goldnet.Ext.Web.MessageBox.Config
                {
                    Title = "信息提示",
                    Message = "日期范围不正确,请重新选择条件",
                    Buttons = MessageBox.Button.OK,
                    Icon = (MessageBox.Icon)Enum.Parse(typeof(MessageBox.Icon), "INFO")
                });
                return;
            }
            //liu.shh  2012.12.18
            if (!this.IsEdit() == true && !this.IsPass() == true)
            {
                this.Store1.DataSource = dal.ViewTeciMeetPerson(dd1, dd2, add_mark, this.DeptCodeCombo.SelectedItem.Value, this.DeptCode(), ((User)Session["CURRENTSTAFF"]).UserId);
            }
            else
            {
                this.Store1.DataSource = dal.ViewTeciMeet(dd1, dd2, add_mark, this.DeptCodeCombo.SelectedItem.Value, this.DeptFilter(""));
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
            this.Store.DataSource = dal.ViewTeciMeetJoinPerson(this.HiddenId.Value.ToString());
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
            dal.InsertTeciMeet(this.DeptCodeCombo.SelectedItem.Value, this.txtMeetName.Text, this.cboGrade.SelectedItem.Text, this.dtfMeetDate.SelectedDate.ToString("yyyy-MM-dd"), Creater,
                               DateTime.Now.ToString("yyyy-MM-dd"), this.txtScienceMeetingPlace.Text, "", this.txrContent.Text, mark, ((User)Session["CURRENTSTAFF"]).UserId);
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
            dal.UpdateTeciMeet(HiddenId.Text, this.txtMeetName.Text, this.cboGrade.SelectedItem.Text, this.dtfMeetDate.SelectedDate.ToString("yyyy-MM-dd"),
                                this.txtScienceMeetingPlace.Text, "", this.txrContent.Text, "0", this.txrSug.Text, this.txrSetSug.Text);

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

            dal.UpdateTeciMeet(HiddenId.Text, this.txtMeetName.Text, this.cboGrade.SelectedItem.Text, this.dtfMeetDate.SelectedDate.ToString("yyyy-MM-dd"),
                                this.txtScienceMeetingPlace.Text, "", this.txrContent.Text, "1", this.txrSug.Text, this.txrSetSug.Text);
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

            dal.UpdateTeciMeet(HiddenId.Text, this.txtMeetName.Text, this.cboGrade.SelectedItem.Text, this.dtfMeetDate.SelectedDate.ToString("yyyy-MM-dd"),
                                this.txtScienceMeetingPlace.Text, "", this.txrContent.Text, "-1", this.txrSug.Text, this.txrSetSug.Text);
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
            dal.UpdateTeciMeet(HiddenId.Text, this.txtMeetName.Text, this.cboGrade.SelectedItem.Text, this.dtfMeetDate.SelectedDate.ToString("yyyy-MM-dd"),
                                this.txtScienceMeetingPlace.Text, "", this.txrContent.Text, "3", this.txrSug.Text, this.txrSetSug.Text);
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
            dal.InsertTeciMeet(this.DeptCodeCombo.SelectedItem.Value, this.txtMeetName.Text, this.cboGrade.SelectedItem.Text, this.dtfMeetDate.SelectedDate.ToString("yyyy-MM-dd"), Creater,
                              DateTime.Now.ToString("yyyy-MM-dd"), this.txtScienceMeetingPlace.Text, "", this.txrContent.Text, "0", ((User)Session["CURRENTSTAFF"]).UserId);
            Goldnet.Ext.Web.ScriptManager scManager = Goldnet.Ext.Web.ScriptManager.GetInstance(this.Page);
            scManager.AddScript("Store1.reload();");
            scManager.AddScript("btn_EchoHandle.setDisabled(true);");
            scManager.AddScript("btn_Delete.setDisabled(true);");
            scManager.AddScript("arcEditWindow.hide();");
        }

        private ArrayList ContrlJuge()
        {
            ArrayList l_ar = new ArrayList();
            return l_ar;
        }


        /// <summary>
        /// 时间转化格式
        /// </summary>
        /// <param name="year">yyyy</param>
        /// <param name="month">m or mm</param>
        /// <param name="date">日期选项</param>
        /// <returns>yyyyMMdd</returns>
        private string DateTimeConvert(string year, string month, ComboBox cbo)
        {
            string l_month = month;
            string l_date = cbo.SelectedItem.Value;
            if (month.Length != 2)
            {
                l_month = "0" + l_month;
            }
            if (cbo.Hidden)
            {
                l_date = "01";
            }
            else
            {
                if (l_date.Length != 2)
                {
                    l_date = "0" + l_date;
                }
            }
            return year + "-" + l_month + "-" + l_date;
        }

    }
}
