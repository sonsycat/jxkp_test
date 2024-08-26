using System;
using System.Drawing;
using System.Data;
using System.Data.OleDb;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.UI;
using System.Web.UI.WebControls;
using Goldnet.Ext.Web;
using GoldNet.Comm;
using GoldNet.Comm.Pic;
using GoldNet.Comm.DAL.Oracle;
using GoldNet.Comm.ExportData;
using GoldNet.Model;
using GoldNet.JXKP.BLL.Guide;
namespace GoldNet.JXKP.zlgl.SysManage
{
    public partial class Singleaward_Detail : PageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Ext.IsAjaxRequest)
            {
                this.stardate.Value = this.stardate.Value.ToString().Substring(0,1)=="0"?System.DateTime.Now.ToString("yyyy-MM") + "-01":this.stardate.Value;
                this.enddate.Value = this.enddate.Value.ToString().Substring(0,1) == "0" ? System.DateTime.Now.ToString("yyyy-MM-dd") : this.enddate.Value ;
                this.GridPanel1.ColumnModel.RegisterCommandStyleRules();
                GetPageData();
            }

        }

        protected void GetQueryPortalet(object sender, EventArgs e)
        {
            GetPageData();
        }

        private void GetPageData()
        {
            Goldnet.Dal.Guide_Manager dal = new Goldnet.Dal.Guide_Manager();
            DataTable dt = dal.GetSingleawardDetail(Convert.ToDateTime(this.stardate.SelectedValue).ToString("yyyy-MM-dd"), Convert.ToDateTime(this.enddate.SelectedValue).ToString("yyyy-MM-dd"));
            this.Store1.DataSource = dt;
            this.Store1.DataBind();
        }
        protected void OutExcel(object sender, EventArgs e)
        {
            List<string> lists = new List<string>();
            Goldnet.Dal.Guide_Manager dal = new Goldnet.Dal.Guide_Manager();
            DataTable dt = dal.GetSingleawardDetail(Convert.ToDateTime(this.stardate.SelectedValue).ToString("yyyy-MM-dd"), Convert.ToDateTime(this.enddate.SelectedValue).ToString("yyyy-MM-dd"));
            for (int i = 0; i < dt.Columns.Count; i++)
            {
                if (dt.Columns[i].ColumnName == "ID")
                    dt.Columns[i].ColumnName = "编号";
                else if (dt.Columns[i].ColumnName == "DEPT_CODE")
                    dt.Columns[i].ColumnName = "科室代码";
                else if (dt.Columns[i].ColumnName == "DEPT_NAME")
                    dt.Columns[i].ColumnName = "科室名称";

                else if (dt.Columns[i].ColumnName == "AWARD_DATE")
                    dt.Columns[i].ColumnName = "奖惩时间";
                else if (dt.Columns[i].ColumnName == "MONEY")
                    dt.Columns[i].ColumnName = "奖惩金额";
                else if (dt.Columns[i].ColumnName == "TYPE_NAME")
                    dt.Columns[i].ColumnName = "奖惩项目";
                else if (dt.Columns[i].ColumnName == "CHECKSTAN")
                    dt.Columns[i].ColumnName = "奖惩标准";
                else if (dt.Columns[i].ColumnName == "REMARK")
                    dt.Columns[i].ColumnName = "备注";
                else
                    lists.Add(dt.Columns[i].ColumnName);
            }
            for (int j = 0; j < lists.Count; j++)
            {
                dt.Columns.Remove(lists[j]);
            }
            ExportData ex = new ExportData();

            ex.ExportToLocal(dt, this.Page, "xls", "单项奖惩明细");
        }

    }
}
