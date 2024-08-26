using System;
using System.Collections;
using System.Configuration;
using System.Data;
using Goldnet.Dal;
using Goldnet.Ext.Web;

namespace GoldNet.JXKP.RLZY.BaseInfoMaintain
{
    public partial class StaffStafferStat : PageBase
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
                DataTable l_dt = PersTypeFilter();
                if (l_dt.Rows.Count > 0)
                {
                    cboPersonType.Items.Add(new Goldnet.Ext.Web.ListItem("全部", "全部"));
                }
                for (int i = 0; i < l_dt.Rows.Count; i++)
                {
                    this.cboPersonType.Items.Add(new Goldnet.Ext.Web.ListItem(l_dt.Rows[i]["NAME"].ToString(), l_dt.Rows[i]["NAME"].ToString()));
                }

                if (l_dt.Rows.Count > 0)
                {
                    cboPersonType.SelectedIndex = 0;
                }

                string sort = "";
                for (int i = 0; i < l_dt.Rows.Count; i++)
                {
                    sort = sort + "'" + l_dt.Rows[i]["NAME"] + "',";
                }
                if (l_dt.Rows.Count == 0)
                {
                    sort = "'-1'";
                }

                HttpProxy proxy = new HttpProxy();
                proxy.Method = HttpMethod.POST;
                proxy.Url = "/RLZY/WebService/DeptInfo.ashx?deptfilter=" + this.DeptFilter("");
                this.Store3.Proxy.Add(proxy);

                this.Store1.DataSource = dal.ViewStaffBaseInfo("", sort.TrimEnd(new char[] { ',' }), this.DeptFilter(""), "0").Tables[0];
                this.Store1.DataBind();
            }
        }

        /// <summary>
        /// 查询按钮事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void QueryStaff(object sender, AjaxEventArgs e)
        {
            this.Store1.RemoveAll();

            string sort = "";
            if (this.cboPersonType.SelectedItem.Text == "" || this.cboPersonType.SelectedItem.Text == "全部")
            {
                DataTable l_dt = PersTypeFilter();
                for (int i = 0; i < l_dt.Rows.Count; i++)
                {
                    sort = sort + "'" + l_dt.Rows[i]["NAME"] + "',";
                }
                if (l_dt.Rows.Count == 0)
                {
                    sort = "'-1'";
                }
            }
            else
            {
                sort = "'" + this.cboPersonType.SelectedItem.Text + "'";
            }

            DataTable l_dt1 = dal.ViewStaffBaseInfo(this.DeptCodeCombo.SelectedItem.Value.ToString(), sort.TrimEnd(new char[] { ',' }), this.DeptFilter(""), "0").Tables[0];

            if (l_dt1.Rows.Count > 0)
            {
                this.Store1.DataSource = l_dt1;
                this.Store1.DataBind();
            }
        }
    }
}
