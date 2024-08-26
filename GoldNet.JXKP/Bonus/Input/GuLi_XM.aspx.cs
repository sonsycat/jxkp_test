using System;
using System.Data;
using Goldnet.Ext.Web;
using Goldnet.Dal;
using GoldNet.Comm.ExportData;

namespace GoldNet.JXKP.Bonus.Input
{
    public partial class GuLi_XM : PageBase
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
                this.Store1.DataSource = dal.getGL_XL().Tables[0];
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
            Store1.DataSource = dal.getGL_XL().Tables[0];
            Store1.DataBind();
            Session.Remove("GuLi_XM");
            Session["GuLi_XM"] = dal.getGL_XL().Tables[0];
        }
        protected void OutExcel(object sender, EventArgs e)
        {
            if (Session["GuLi_XM"] != null)
            {
                ExportData ex = new ExportData();
                DataTable dt = (DataTable)Session["GuLi_XM"];

                ex.ExportToLocal(dt, this.Page, "xls", "鼓励项目");
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
        public void DictAjaxOper(string DictCode, string DictName,string BL, string OperType)
        {
            switch (OperType)
            {
                case "1":
                    dal.InserGL_XM(DictCode, DictName, BL);
                    ShowMessage("系统提示", "添加成功。");
                    break;
                case "3":
                    dal.DelGL_XM(DictCode, DictName);
                    break;
                case "4":
                    string name = dal.Cha_HS_GZL(DictCode).ToString();
                    if (name.Equals(""))
                    {
                        ShowMessage("系统提示", "项目编码输入错误。");
                        break;
                    }
                    string dept_code = dal.Cha_GL_XM(DictName).ToString();
                    if (dept_code.Equals(""))
                    {
                        ShowMessage("系统提示", "科室编码输入错误。");
                        break;
                    }
                   
                    break;
            }
        }
    }
}