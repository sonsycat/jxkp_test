using System;
using System.Collections;
using System.Configuration;
using System.Data;
using Goldnet.Ext.Web;
using Goldnet.Dal;

namespace GoldNet.JXKP.RLZY.BaseInfoMaintain
{
    public partial class DeptBaseInfoStat : PageBase
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

                HttpProxy proxy = new HttpProxy();
                proxy.Method = HttpMethod.POST;
                proxy.Url = "/RLZY/WebService/DeptInfo.ashx?deptfilter=" + this.DeptFilter("");
                this.Store3.Proxy.Add(proxy);

                this.Store1.DataSource = dal.ViewDeptBaseInfoStat("",this.DeptFilter("")).Tables[0];
                this.Store1.DataBind();
            }
        }
        /// <summary>
        /// 查询按钮事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void QueryDept(object sender, AjaxEventArgs e)
        {
            this.Store1.RemoveAll();

            DataTable l_dt = dal.ViewDeptBaseInfoStat(this.DeptCodeCombo.SelectedItem.Value.ToString(), this.DeptFilter("")).Tables[0];

            if (l_dt.Rows.Count > 0)
            {
                this.Store1.DataSource = l_dt;
                this.Store1.DataBind();
            }
        }
    }
}
