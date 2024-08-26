using System;
using System.Data;
using Goldnet.Ext.Web;
using Goldnet.Dal;
using GoldNet.Comm.ExportData;


namespace GoldNet.JXKP.Bonus.Input
{
    public partial class INP_ZCF : PageBase
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
                this.Store1.DataSource = dal.getINP_ZCF().Tables[0];
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
            Store1.DataSource = dal.getINP_ZCF().Tables[0];
            Store1.DataBind();
            Session.Remove("INP_ZCF");
            Session["INP_ZCF"] = dal.getINP_ZCF().Tables[0];
        }
        protected void OutExcel(object sender, EventArgs e)
        {
            if (Session["INP_ZCF"] != null)
            {
                ExportData ex = new ExportData();
                DataTable dt = (DataTable)Session["INP_ZCF"];

                ex.ExportToLocal(dt, this.Page, "xls", "住院诊查费人名");
                //MHeaderTabletoExcel(dt, null, null, null, 0);
                //ex.ExportToLocal(l_dt, this.Page, "xls", "人员信息");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="DictCode"></param>
        /// <param name="DictName"></param>
        /// <param name="OperType"></param>
        /// <param name="Code"></param>
        [AjaxMethod]
        public void DictAjaxOper(string DictCode, string DictName, string OperType)
        {
            switch (OperType)
            {
                case "1":
                    dal.InsertINP_ZCF(DictCode, DictName);
                    ShowMessage("系统提示", "添加成功。");
                    break;
                case "3":
                    dal.DelINP_ZCF(DictCode);
                    break;
                case "4":
                    string name = dal.Cha_INP_ZCF(DictCode).ToString();
                    if (name.Equals(""))
                    {
                        ShowMessage("系统提示", "项目编码输入错误。");
                    }
                    else
                    {
                        txtName.Text = name;
                        //dal.InsertHS_GZL(DictCode, DictName);
                    }
                    break;
            }
        }
    }
}