using System;
using System.Collections;
using System.Configuration;
using System.Data;
using Goldnet.Ext.Web;
using Goldnet.Dal;
using System.Collections.Generic;

namespace GoldNet.JXKP.GuideLook.Statement
{
    public partial class DeptBenefitInfo : System.Web.UI.Page
    {
        private StatementDal dal = new StatementDal();
        protected void Page_Load(object sender, EventArgs e)
        {
            //检查是否已经登录，否则停止
            if (Session["CURRENTSTAFF"] == null)
            {
                Response.End();
                return;
            }

            if (!Ext.IsAjaxRequest)
            {
                string fromdate = Request.QueryString["FromDate"].ToString();
                string todate = Request.QueryString["ToDate"].ToString();
                string type = Request.QueryString["Type"].ToString();
                string staffid = Request.QueryString["StaffId"].ToString();
                string deptCode = Request.QueryString["DeptCode"].ToString();

                this.Store1.DataSource = dal.getDeptBenefitInfo(fromdate, todate, deptCode, staffid, type).Tables[0];
                this.Store1.DataBind();
            }
        }


        /// <summary>
        /// 查询按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void GetQueryPer(object sender, AjaxEventArgs e)
        {
            Dictionary<string, string>[] selectRow = GetSelectRow(e);
            //判断如果selectRow里为null，则返回空
            if (selectRow == null)
            {
                return;
            }

            Goldnet.Ext.Web.ScriptManager scManager = Goldnet.Ext.Web.ScriptManager.GetInstance(this.Page);
            scManager.AddScript("parent.window.viewDetail('" + this.getPageResponseUrl("PerBenefit.aspx", selectRow[0]["FROMDATE"], selectRow[0]["TODATE"], selectRow[0]["TYPE"], selectRow[0]["DEPT_CODE"], selectRow[0]["STAFF_ID"], selectRow[0]["INCOM_TYPE_CODE"]) + "')");
        }


        //反序列化得到客户端提交的gridpanel数据行
        private Dictionary<string, string>[] GetSelectRow(AjaxEventArgs e)
        {
            string row = e.ExtraParams["Values"].ToString();
            Dictionary<string, string>[] selectRow = JSON.Deserialize<Dictionary<string, string>[]>(row);
            if (selectRow.Length <= 0)
            {
                return null;
            }
            else
            {
                return selectRow;
            }
        }

        /// <summary>
        /// 取得地址参数
        /// </summary>
        /// <param name="PageUrl">跳转地址</param>
        /// <returns>跳转地址以及参数</returns>
        private string getPageResponseUrl(string PageUrl, string fromDate, string toDate, string type, string deptCode,string staffId,string itemClass)
        {
            string ResponseUrl = "" + PageUrl + "?DeptCode=" + deptCode + "&FromDate=" + fromDate + "&ToDate=" + toDate + "&Type=" + type + "&Staffid=" + staffId + "&itemClass=" + itemClass;
            return ResponseUrl;
        }
    }
}
