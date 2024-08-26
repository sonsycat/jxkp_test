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
    public partial class OutYBList : PageBase
    {
        BaseInfoManagerDal dal = new BaseInfoManagerDal();
        protected void Page_Load(object sender, EventArgs e)
        {
            //检查是否已经登录，否则停止
            if (Session["CURRENTSTAFF"] == null)
            {
                Response.End();
            }

            if (!Ext.IsAjaxRequest)
            {
                bool isEdit = this.IsEdit();
                if (!isEdit)
                {
                    ScriptManager1.AddScript("#{btn_Delete}.hide();#{btn_Add}.hide();#{btn_Modify}.hide();");
                }

                HttpProxy proxy = new HttpProxy();
                proxy.Method = HttpMethod.POST;
                proxy.Url = "/RLZY/WebService/DeptInfo.ashx?deptfilter=" + this.DeptFilter("");
                this.Store3.Proxy.Add(proxy);
                SetCombox();
                this.Store1.DataSource = dal.ViewOutYBList(DateTime.Now.Year.ToString() + "-01-01", DateTime.Now.ToString("yyyy-MM-dd"), "", this.DeptFilter(""));
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
        }

        /// <summary>
        /// 刷新数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void Data_RefreshData(object sender, StoreRefreshDataEventArgs e)
        {
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

            this.Store1.DataSource = dal.ViewOutYBList(dd1, dd2, this.DeptCodeCombo.SelectedItem.Value, this.DeptFilter(""));
            this.Store1.DataBind();
        }

        /// <summary>
        /// 保存按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void SaveInfo(object sender, AjaxEventArgs e)
        {
            string Creater = ((User)Session["CURRENTSTAFF"]).UserName == null ? "NotUserId" : ((User)Session["CURRENTSTAFF"]).UserName;
            dal.InsertOutYBList(DateTime.Now.ToString("yyyy-MM-dd"), this.DeptCodeCombo.SelectedItem.Value, this.DeptCodeCombo.SelectedItem.Text, Creater, this.txaWorkDetail.Text);
            Goldnet.Ext.Web.ScriptManager scManager = Goldnet.Ext.Web.ScriptManager.GetInstance(this.Page);
            scManager.AddScript("Store1.reload();");
            scManager.AddScript("btn_Modify.setDisabled(true);");
            scManager.AddScript("btn_Delete.setDisabled(true);");
            scManager.AddScript("arcEditWindow.hide();");
        }

        /// <summary>
        /// 修改按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void EditInfo(object sender, AjaxEventArgs e)
        {
            dal.UpdateOutYBList(this.txaWorkDetail.Text, HiddenId.Text);
            Goldnet.Ext.Web.ScriptManager scManager = Goldnet.Ext.Web.ScriptManager.GetInstance(this.Page);
            scManager.AddScript("Store1.reload();");
            scManager.AddScript("btn_Modify.setDisabled(true);");
            scManager.AddScript("btn_Delete.setDisabled(true);");
            scManager.AddScript("arcEditWindow.hide();");
        }

        [AjaxMethod]
        public void OutYBListAjaxOper(string Id)
        {
            dal.DeleteOutYBList(Id);
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