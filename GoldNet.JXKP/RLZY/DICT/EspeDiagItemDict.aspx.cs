using System;
using System.Collections;
using System.Configuration;
using System.Data;
using Goldnet.Ext.Web;
using Goldnet.Dal;

namespace GoldNet.JXKP.RLZY.DICT
{
    public partial class EspeDiagItemDict : System.Web.UI.Page
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
                this.Store1.DataSource = dal.ViewEspeDiagItemDictDict().Tables[0];
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
            Store1.DataSource = dal.ViewEspeDiagItemDictDict().Tables[0];
            Store1.DataBind();
        }

        [AjaxMethod]
        public void DictAjaxOper(string DictCode, string DictName, string OperType, string Code)
        {
            switch (OperType)
            {
                case "1":
                    dal.InsertEspeDiagItemDictDict(DictName, Code);
                    break;
                case "2":
                    dal.UpdataEspeDiagItemDictDict(DictCode, DictName, Code);
                    break;
                case "3":
                    dal.DelEspeDiagItemDictDict(DictCode);
                    break;
            }
        }
    }
}
