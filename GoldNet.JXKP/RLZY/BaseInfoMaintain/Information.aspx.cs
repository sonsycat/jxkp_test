using System;
using System.Collections;
using System.Configuration;
using System.Data;
using Goldnet.Dal;
using Goldnet.Ext.Web;

namespace GoldNet.JXKP.RLZY.BaseInfoMaintain
{
    public partial class Information : PageBase
    {
        BaseInfoMaintainDal dal = new BaseInfoMaintainDal();
        protected void Page_Load(object sender, EventArgs e)
        {
            //检查是否已经登录，否则停止
            if (Session["CURRENTSTAFF"] == null)
            {
                Response.End();
            }

            if (!Ext.IsAjaxRequest)
            {
                for (int i = 0; i < 10; i++)
                {
                    int years = System.DateTime.Now.Year - i;
                    this.cboTime.Items.Add(new Goldnet.Ext.Web.ListItem(years.ToString(), years.ToString()));
                }
                this.cboTime.SelectedIndex = 0;
                this.TimeOrgan.SelectedIndex = 0;
            }
        }


        /// <summary>
        /// 刷新数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void Data_RefreshData(object sender, StoreRefreshDataEventArgs e)
        {
            Store1.RemoveAll();
            string organ = this.TimeOrgan.SelectedItem.Value.ToString();
            string year = "'" + this.cboTime.SelectedItem.Value.ToString() + "'";
            DataTable l_dt = dal.ViewInformation(organ + year).Tables[0];
            Store1.DataSource = l_dt;
            Store1.DataBind();
        }

        [AjaxMethod]
        public void InformatinAjaxOper(string Id, string statMonth, string openDate, string appSubsysNum, string netNum, string netCompNum, string serverNum, string investTotal, string HIStechPers, string planetMediCase, string planetLongCase, string planetLongPpers, string OperType)
        {
            switch (OperType)
            {
                case "1":
                    dal.InsertInformation(statMonth, openDate, appSubsysNum, netNum, netCompNum, serverNum, investTotal, HIStechPers, planetMediCase, planetLongCase, planetLongPpers);
                    break;
                case "2":
                    dal.UpdateInformation(Id,openDate,appSubsysNum,netNum,netCompNum,serverNum,investTotal,HIStechPers,planetMediCase,planetLongCase,planetLongPpers);
                    break;
                case "3":
                    dal.DelInformation(Id);
                    break;
            }
        }
    }
}
