using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using Goldnet.Ext.Web;
using Goldnet.Dal;
using GoldNet.Comm;
using GoldNet.Model;

namespace GoldNet.JXKP.RLZY.BaseInfoManager
{
    public partial class DeptExList : PageBase
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
                        ScriptManager1.AddScript("#{btn_Delete}.hide();#{btn_Add}.hide();");
                        this.hiddenEdit.Text = "2";
                    }
                }
                else
                {
                    if (!isEdit)
                    {
                        //ScriptManager1.AddScript("#{btn_Delete}.hide();#{btn_Add}.hide();#{cbxOpration}.hide();#{btn_EchoHandle}.hide();");
                        //liu.shh  2012.12.18
                        ScriptManager1.AddScript("#{btn_EchoHandle}.hide();");
                    }
                }
                this.hiddenMeunUp.Text = isPass.ToString();
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

                string fromDate = DateTime.Now.Year + "01";
                string toDay = (DateTime.Now.Month).ToString().Length == 1 ? "0" + (DateTime.Now.Month).ToString() : (DateTime.Now.Month).ToString();
                string toDate = DateTime.Now.Year.ToString() + toDay;
                //liu.shh  2012.12.18
                if (!this.IsEdit() == true && !this.IsPass() == true)
                {
                    this.Store1.DataSource = dal.ViewDeptExListPerson(fromDate, toDate, "", deptCode, add_mark, ((User)Session["CURRENTSTAFF"]).UserId).Tables[0];
                }
                else
                {
                    this.Store1.DataSource = dal.ViewDeptExList(fromDate, toDate, "", deptCode, add_mark).Tables[0];
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
        }

        [AjaxMethod]
        public void DeptExListAjaxOper(string Id, string ExTypes, string ExSorts, string ExPerson, string ExCount, string ExDates,
                                       string ExTimes, string ExMemert, string OperType, string DeptCode, string DeptName, string ExSug, string ExSetSug)
        {
            switch (OperType)
            {
                case "1":
                    string mark = PowerPageInfo == 2 ? "3" : "1";
                    dal.InsertDeptExList(DeptCode, this.DeptCodeCombo.SelectedItem.Text, ExDates, ExTypes, ExPerson, ExSorts, ExTimes, ExMemert, ExCount, mark, ((User)Session["CURRENTSTAFF"]).UserId);
                    break;
                case "2":
                    dal.UpdataDeptExList(Id, DeptCode, DeptName, ExTypes, ExPerson, ExSorts, ExTimes, ExMemert, ExCount, "0", ExDates, ExSug, ExSetSug);
                    break;
                case "3":
                    dal.EchoDeleteHandle("DEPT_EX", "ID", Id);
                    break;
                case "4":
                    dal.EchoUpDataHandle("DEPT_EX", "ADD_MARK", "ID", Id, "0");
                    break;
                case "5":
                    dal.UpdataDeptExList(Id, DeptCode, DeptName, ExTypes, ExPerson, ExSorts, ExTimes, ExMemert, ExCount, "1", ExDates, ExSug, ExSetSug);
                    break;
                case "6":
                    dal.EchoUpDataHandle("DEPT_EX", "ADD_MARK", "ID", Id, "1");
                    break;
                case "7":
                    dal.UpdataDeptExList(Id, DeptCode, DeptName, ExTypes, ExPerson, ExSorts, ExTimes, ExMemert, ExCount, "3", ExDates, ExSug, ExSetSug);
                    break;
                case "8":
                    dal.UpdataDeptExList(Id, DeptCode, this.DeptCodeCombo.SelectedItem.Text, ExTypes, ExPerson, ExSorts, ExTimes, ExMemert, ExCount, "3", ExDates, ExSug, ExSetSug);
                    break;
                case "-1":
                    dal.UpdataDeptExList(Id, DeptCode, DeptName, ExTypes, ExPerson, ExSorts, ExTimes, ExMemert, ExCount, "-1", ExDates, ExSug, ExSetSug);
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
            string fromDate = this.Comb_StartYear.SelectedItem.Value + (this.Comb_StartMonth.SelectedItem.Value.ToString().Length == 1 ? "0" + this.Comb_StartMonth.SelectedItem.Value.ToString() : this.Comb_StartMonth.Value.ToString());
            string toDate = this.Comb_EndYear.SelectedItem.Value + (this.Comb_EndMonth.SelectedItem.Value.ToString().Length == 1 ? "0" + this.Comb_EndMonth.SelectedItem.Value.ToString() : this.Comb_EndMonth.SelectedItem.Value.ToString());
            //liu.shh  2012.12.18
            if (!this.IsEdit() == true && !this.IsPass() == true)
            {
                this.Store1.DataSource = dal.ViewDeptExListPerson(fromDate, toDate, this.DeptCodeCombo.SelectedItem.Value, this.DeptCode(), add_mark, ((User)Session["CURRENTSTAFF"]).UserId).Tables[0];
                string ss = this.Store1.ToString();
            }
            else
            {
                this.Store1.DataSource = dal.ViewDeptExList(fromDate, toDate, this.DeptCodeCombo.SelectedItem.Value, this.DeptFilter(""), add_mark).Tables[0];
            }
            this.Store1.DataBind();
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
