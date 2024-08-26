using System;
using System.Collections;
using System.Configuration;
using System.Data;
using Goldnet.Dal;
using Goldnet.Ext.Web;

namespace GoldNet.JXKP.RLZY.DICT
{
    public partial class CivilDict : System.Web.UI.Page
    {

        private static string TableName = "CIVILSERVICECLASS_DICT";
        private static string fieldId = "ID";
        private static string fieldName = "CIVILSERVICECLASS";
        private static bool flgDel = true;
        private static string flg = "IS_DEL";

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
                this.Store1.DataSource = dal.getDictInfo(TableName, fieldId, fieldName, flgDel, flg).Tables[0];
                this.Store1.DataBind();
            }
        }

        [AjaxMethod]
        public void DictAjaxOper(string DictCode, string DictName, string OperType)
        {
            switch (OperType)
            {
                case "1":
                    dal.InsertDictInfo(TableName, fieldId, fieldName, DictCode, DictName);
                    break;
                case "2":
                    dal.UpdatedictInfo(TableName, fieldId, fieldName, flgDel, flg, DictCode, DictName);
                    break;
                case "3":
                    dal.DelDictInfo(TableName, fieldId, flgDel, flg, DictCode);
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
            Store1.RemoveAll();
            Store1.DataSource = dal.getDictInfo(TableName, fieldId, fieldName, flgDel, flg).Tables[0];
            Store1.DataBind();
        }

    }
}
