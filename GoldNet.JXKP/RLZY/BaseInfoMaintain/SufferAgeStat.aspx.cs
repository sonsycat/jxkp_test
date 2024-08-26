using System;
using System.Collections;
using System.Configuration;
using System.Data;
using Goldnet.Ext.Web;
using Goldnet.Dal;

namespace GoldNet.JXKP.RLZY.BaseInfoMaintain
{
    public partial class SufferAgeStat : PageBase
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
                //等级字典信息
                DataTable l_dt = PersTypeFilter();
                if (l_dt.Rows.Count > 0)
                {
                    cboPersonType.Items.Add(new Goldnet.Ext.Web.ListItem("全部", "全部"));
                }
                SetCboData("", "", l_dt, cboPersonType);
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
                this.Store1.DataSource = dal.ViewSufferAgeStat("", sort.TrimEnd(new char[] { ',' }), this.DeptFilter("")).Tables[0];
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
            Store1.RemoveAll();
            Store1.DataSource = dal.ViewSufferAgeStat(this.DeptCodeCombo.SelectedItem.Value, sort.TrimEnd(new char[] { ',' }), this.DeptFilter("")).Tables[0];
            Store1.DataBind();
        }


        /// <summary>
        /// 初始化COMBOX控件
        /// </summary>
        /// <param name="ID">数据库ID名称</param>
        /// <param name="text">数据库NAME名称</param>
        /// <param name="dtSource">数据源</param>
        /// <param name="cbo">COMBOX控件</param>
        private void SetCboData(string ID, string text, DataTable dtSource, ComboBox cbo)
        {
            if (dtSource.Rows.Count < 1)
            {
                return;
            }
            for (int idx = 0; idx < dtSource.Rows.Count; idx++)
            {
                cbo.Items.Add(new Goldnet.Ext.Web.ListItem(dtSource.Rows[idx]["NAME"].ToString(), dtSource.Rows[idx]["ID"].ToString()));
            }
        }

    }
}
