using System;
using System.Collections;
using System.Configuration;
using System.Data;
using Goldnet.Ext.Web;
using Goldnet.Dal;

namespace GoldNet.JXKP.RLZY.DICT
{
    public partial class DutyDict : System.Web.UI.Page
    {
        DictMainTainDal dal = new DictMainTainDal();
        protected void Page_Load(object sender, EventArgs e)
        {
            //检查是否已经登录，否则停止
            if (Session["CURRENTSTAFF"] == null)
            {
                Response.End();
            }

            if (!Ext.IsAjaxRequest)
            {
                this.Store1.DataSource = dal.getDutyDict().Tables[0];
                this.Store1.DataBind();
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
            Store1.DataSource = dal.getDutyDict().Tables[0];
            Store1.DataBind();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="DictCode"></param>
        /// <param name="DictName"></param>
        /// <param name="OperType"></param>
        /// <param name="Code"></param>
        [AjaxMethod]
        public void DictAjaxOper(string DictCode, string DictName, string OperType, string Subsidy, string Sortno)
        {
            switch (OperType)
            {
                case "1":
                    dal.InsertDutyDict(DictName, Subsidy, Sortno);
                    break;
                case "2":
                    dal.UpdataDutyDict(DictCode, DictName, Subsidy, Sortno);
                    break;
                case "3":
                    dal.DelDutyDict(DictCode);
                    break;
            }
        }
    }
}
