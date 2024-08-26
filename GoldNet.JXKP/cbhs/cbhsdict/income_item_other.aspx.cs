using System;
using Goldnet.Ext.Web;
using Goldnet.Dal;
using System.Data;

namespace GoldNet.JXKP.cbhs.cbhsdict
{
    public partial class income_item_other : System.Web.UI.Page
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
                this.Store1.DataSource = dal.getDutyItem_Ohter().Tables[0];
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
            Store1.DataSource = dal.getDutyItem_Ohter().Tables[0];
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
        public void DictAjaxOper(string CLASS_CODE, string DEPT_NAME, string OperType, string CLASS_NAME, string INPUT_CODE)
        {
            switch (OperType)
            {
                case "1":
                    dal.InsertItem_Ohter(CLASS_CODE, DEPT_NAME, CLASS_NAME, INPUT_CODE);
                    break;
                case "2":
                    dal.UpdataItem_Ohter(CLASS_CODE, DEPT_NAME, CLASS_NAME, INPUT_CODE);
                    break;
                case "3":
                    dal.DelDutyItem_Ohter(CLASS_CODE);
                    break;
            }
        }
    }
}