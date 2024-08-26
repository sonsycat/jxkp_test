using System;
using System.Collections;
using System.Configuration;
using System.Data;
using Goldnet.Ext.Web;
using Goldnet.Dal;

namespace GoldNet.JXKP.RLZY.DICT
{
    public partial class TrainingListDict : System.Web.UI.Page
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

                DataTable l_dt = dal.getDictInfo("TITLE_LIST_DICT", "SERIAL_NO", "TITLE_LIST_NAME",false,"").Tables[0];
                for (int i = 0; i < l_dt.Rows.Count; i++)
                {
                    this.Combo_PersonType.Items.Add(new Goldnet.Ext.Web.ListItem(l_dt.Rows[i]["NAME"].ToString(), l_dt.Rows[i]["NAME"].ToString()));

                }
                this.Combo_PersonType.SelectedIndex = 0;

                this.Store1.DataSource = dal.ViewTrainingListDict().Tables[0];
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
            Store1.DataSource = dal.ViewTrainingListDict().Tables[0];
            Store1.DataBind();
        }
        
        [AjaxMethod]
        public void DictAjaxOper(string DictCode, string DictName, string OperType,string PersonType)
        {
            switch (OperType)
            {
                case "1":
                    dal.InsertTrainListDict(DictName, PersonType);
                    break;
                case "2":
                    dal.UpdataTrainListDict(DictCode, DictName, PersonType);
                    break;
                case "3":
                    dal.DelTrainListDict(DictCode);
                    break;
            }
        }
    }
}
