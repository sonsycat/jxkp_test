using System;
using System.Data;
using Goldnet.Ext.Web;
using System.Collections.Generic;
using Goldnet.Dal;
using GoldNet.Model;
using GoldNet.Comm;
using GoldNet.Comm.ExportData;

namespace GoldNet.JXKP.GuideLook.Statement
{
    public partial class PlanGuideStats : PageBase
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Ext.IsAjaxRequest)
            {
                StatementDal dal = new StatementDal();
                for (int i = 0; i < 10; i++)
                {
                    int years = System.DateTime.Now.Year - i;
                    this.years.Items.Add(new Goldnet.Ext.Web.ListItem(years.ToString(), years.ToString()));
                    this.years1.Items.Add(new Goldnet.Ext.Web.ListItem(years.ToString(), years.ToString()));
                }
                this.years.SelectedItem.Value = System.DateTime.Now.ToString("yyyy");
                this.months.SelectedItem.Value = System.DateTime.Now.ToString("MM");
                this.years1.SelectedItem.Value = System.DateTime.Now.ToString("yyyy");
                this.months1.SelectedItem.Value = System.DateTime.Now.ToString("MM");

                ///////设置权限/////////
                bool isEdit = this.IsEdit();
                if (isEdit)
                {
                    //ScriptManager1.AddScript("#{Button_save}.show();");
                }
                ///////处理DeptFilter 未-1的情况////////
                string DeptFilter = this.DeptFilter("");
                if (DeptFilter == "'-1'")
                    DeptFilter = "";
                ////////////////////////

                SetDict();
                Store1.DataSource = dal.GetSecondDept(DeptFilter);
                Store1.DataBind();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        protected void SetDict()
        {
            StatementDal dal = new StatementDal();
            DataTable dt = new DataTable();

            dt = dal.GetGuideDict(GetConfig.GetConfigString("singledeptcode"));
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                this.GUIDE_ITEM.Items.Add(new Goldnet.Ext.Web.ListItem(dt.Rows[i]["GUIDE_NAME"].ToString(), dt.Rows[i]["GUIDE_CODE"].ToString()));

            }
            if (dt.Rows.Count > 0)
            {
                this.GUIDE_ITEM.SelectedItem.Value = dt.Rows[0]["GUIDE_CODE"].ToString();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Button_look_click(object sender, EventArgs e)
        {
            StatementDal dal = new StatementDal();
            string tjny = GetBeginDate() + "," + GetEndDate();
            ///////////////
            string DeptFilter = this.DeptFilter("");
            if (DeptFilter == "'-1'")
                DeptFilter = "";
            Currdt = dal.getPlanGuideStatsInfo(tjny, (Convert.ToInt32(months1.SelectedItem.Value) - Convert.ToInt32(months.SelectedItem.Value) + 1), GUIDE_ITEM.SelectedItem.Value, GUIDE_ITEM.SelectedItem.Text, ((User)Session["CURRENTSTAFF"]).StaffId, DeptFilter);
            Store1.DataSource = Currdt;
            Store1.DataBind();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Button_del_click(object sender, AjaxEventArgs e)
        {
        }

        //反序列化得到客户端提交的gridpanel数据行      
        private Dictionary<string, string>[] GetSelectRow(AjaxEventArgs e, string epValue)
        {
            string row = e.ExtraParams[epValue].ToString();
            Dictionary<string, string>[] selectRow = JSON.Deserialize<Dictionary<string, string>[]>(row);
            if (selectRow.Length <= 0) { return null; } else { return selectRow; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Item_SelectOnChange(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Btn_Save_Click(object sender, AjaxEventArgs e)
        {
            Dictionary<string, string>[] selectRow = GetSelectRow(e, "Values");

            if (selectRow != null)
            {
                try
                {
                    StatementDal dal = new StatementDal();

                    dal.SavePlanGuideStats(selectRow, GUIDE_ITEM.SelectedItem.Value);

                    Goldnet.Ext.Web.Ext.Msg.Show(new Goldnet.Ext.Web.MessageBox.Config
                    {
                        Title = "提示",
                        Message = "保存成功",
                        Buttons = MessageBox.Button.OK,
                        Icon = (MessageBox.Icon)Enum.Parse(typeof(MessageBox.Icon), "INFO")
                    });
                    GridPanel1.RefreshView();
                }
                catch (Exception ex)
                {
                    ShowDataError(ex.ToString(), Request.Url.LocalPath, "PlanGuideStats");
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void Data_RefreshData(object sender, StoreRefreshDataEventArgs e)
        {
            StatementDal dal = new StatementDal();
            string tjny = GetBeginDate() + "," + GetEndDate();
            ///////////////
            string DeptFilter = this.DeptFilter("");
            if (DeptFilter == "'-1'")
                DeptFilter = "";
            Currdt = dal.getPlanGuideStatsInfo(tjny, (Convert.ToInt32(months1.SelectedItem.Value) - Convert.ToInt32(months.SelectedItem.Value) + 1), GUIDE_ITEM.SelectedItem.Value, GUIDE_ITEM.SelectedItem.Text, ((User)Session["CURRENTSTAFF"]).StaffId, DeptFilter);
            Store1.DataSource = Currdt;
            Store1.DataBind();
        }

        /// <summary>
        /// 开始时间
        /// </summary>
        /// <returns></returns>
        private string GetBeginDate()
        {
            string year = years.SelectedItem.Value.ToString();
            string month = months.SelectedItem.Value.ToString();
            if (month.Length == 1)
            {
                month = "0" + month;
            }
            string benginDate = year + "" + month + "01";
            return benginDate;
        }

        /// <summary>
        /// 结束时间
        /// </summary>
        /// <returns></returns>
        private string GetEndDate()
        {
            string year = years1.SelectedItem.Value.ToString();
            string month = months1.SelectedItem.Value.ToString();
            if (month.Length == 1)
            {
                month = "0" + month;
            }
            string endDate = year + "-" + month + "-01";
            endDate = Convert.ToDateTime(endDate).AddMonths(1).AddDays(-1).ToString("yyyyMMdd");
            return endDate;
        }

        /// <summary>
        /// 
        /// </summary>
        private static DataTable Currdt = new DataTable();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void OutExcel(object sender, EventArgs e)
        {
            if (Currdt.Rows.Count < 1) return;
            ExportData ex = new ExportData();
            DataTable ExcelTable = new DataTable();
            Currdt.Columns.Remove("DEPT_CODE");
            Currdt.Columns[0].ColumnName = "科室";
            Currdt.Columns[1].ColumnName = "指标";
            Currdt.Columns[3].ColumnName = "前期值";
            Currdt.Columns[3].ColumnName = "后期值";
            Currdt.Columns[4].ColumnName = "增长量";
            ExcelTable = Currdt;
            ex.ExportToLocal(ExcelTable, this.Page, "xls", "计划指标查询");
        }
    }
}
