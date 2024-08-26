using System;
using System.Drawing;
using System.Data;
using System.Data.OleDb;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using Goldnet.Ext.Web;
using GoldNet.Comm;
using GoldNet.Comm.ExportData;
using GoldNet.Comm.Pic;
using GoldNet.Comm.DAL.Oracle;
using GoldNet.Model;
using GoldNet.JXKP.BLL.Guide;

namespace GoldNet.JXKP.zlgl.GuideManage
{
    public partial class Quality_SearchList : PageBase
    {
        DataTable dt = new DataTable();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Ext.IsAjaxRequest)
            {
                this.stardate.Value = this.stardate.Value.ToString().Substring(0, 1) == "0" ? System.DateTime.Now.ToString("yyyy-MM") + "-01" : this.stardate.Value;
                this.enddate.Value = this.enddate.Value.ToString().Substring(0, 1) == "0" ? System.DateTime.Now.ToString("yyyy-MM-dd") : this.enddate.Value;
                this.GridPanel1.ColumnModel.RegisterCommandStyleRules();
                GetPageData();
            }
        }
        protected void GetQueryPortalet(object sender, EventArgs e)
        {
            GetPageData();
        }
        protected void OutExcel(object sender, EventArgs e)
        {
            dt = QualityGuide.QualitySearchlist(Convert.ToDateTime(this.stardate.SelectedValue).ToString("yyyy-MM-dd"), Convert.ToDateTime(this.enddate.SelectedValue).ToString("yyyy-MM-dd"));
            this.MHeaderTabletoExcel(dt, null, "科室质量得分", null, 1);
        }
        private void GetPageData()
        {
            dt.Clear();

            this.GridPanel1.Reconfigure();

            dt = QualityGuide.QualitySearchlist(Convert.ToDateTime(this.stardate.SelectedValue).ToString("yyyy-MM-dd"), Convert.ToDateTime(this.enddate.SelectedValue).ToString("yyyy-MM-dd"));
            for (int i = 0; i < dt.Columns.Count; i++)
            {
                RecordField field = new RecordField();
                if (dt.Columns[i].ColumnName.Equals("科室") || dt.Columns[i].ColumnName.Equals("DEPT_CODE"))
                {
                    field = new RecordField(dt.Columns[i].ColumnName, RecordFieldType.String);
                }
                else
                {
                    field = new RecordField(dt.Columns[i].ColumnName, RecordFieldType.Float);
                }
                this.Store1.AddField(field, i);
                Column cl = new Column();
                cl.Header = dt.Columns[i].ColumnName;
                cl.Sortable = true;
                cl.MenuDisabled = true;
                cl.ColumnID = dt.Columns[i].ColumnName;
                cl.DataIndex = dt.Columns[i].ColumnName;

                this.GridPanel1.AddColumn(cl);
            }
           

            this.Store1.DataSource = dt;
            this.Store1.DataBind();
        }

        
    }
}
