using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Goldnet.Ext.Web;
using Goldnet.Dal;
using System.Data;
using GoldNet.Comm;
using GoldNet.Model;
using GoldNet.Comm.ExportData;
namespace GoldNet.JXKP
{
    /// <summary>
    /// 成本明细查询
    /// </summary>
    public partial class DeptCostDetail : PageBase
    {
        private Report report_dal = new Report();
        /// <summary>
        /// 科室成本明细查询
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Ext.IsAjaxRequest)
            {
                string year = DateTime.Now.AddMonths(-1).Year.ToString();
                string month = DateTime.Now.AddMonths(-1).Month.ToString();
                BoundComm boundcomm = new BoundComm();
                SYear.DataSource = boundcomm.getYears();
                SYear.DataBind();
                cbbYear.Value = year;
                ccbYearTo.Value = year;
                SMonth.DataSource = boundcomm.getMonth();
                SMonth.DataBind();
                cbbmonth.Value = month;
                ccbMonthTo.Value = month;
                SetDeptStoreProxy();
                SetStoreProxy();

                cbbType.SelectedItem.Value = "0";
                //cbbType.SelectedItem.Text = "收付实现";

                //string deptcode = this.DeptFilter("");
                //if (deptcode != "")
                //{
                //    User user = (User)Session["CURRENTSTAFF"];
                //    cbbdept.SelectedItem.Value = user.HisDeptCode;
                //    cbbdept.SelectedItem.Text = user.HisDeptName;
                //    cbbdept.Disabled = true;
                //}


                string hospro = System.Configuration.ConfigurationManager.AppSettings["HOSPRO"];
                if (hospro == "1")
                {
                    this.GridPanel_Show.ColumnModel.Columns[4].Hidden = true;
                    this.GridPanel_Show.ColumnModel.Columns[5].Hidden = true;
                }
            }

        }
        private void SetDeptStoreProxy()
        {

            //查找科室信息
            HttpProxy pro = new HttpProxy();
            pro.Method = HttpMethod.POST;
            //pro.Url = "../WebService/BonusDepts.ashx";
            pro.Url = "../WebService/BonusDepts.ashx?deptfilter=" + this.DeptFilter("dept_code");
            this.SDept.Proxy.Add(pro);
            JsonReader jr = new JsonReader();
            jr.ReaderID = "DEPT_CODE";
            jr.Root = "Bonusdepts";
            jr.TotalProperty = "totalCount";
            RecordField rf = new RecordField();
            rf.Name = "DEPT_CODE";
            jr.Fields.Add(rf);
            RecordField rfn = new RecordField();
            rfn.Name = "DEPT_NAME";
            jr.Fields.Add(rfn);
            this.SDept.Reader.Add(jr);
        }
        private void SetStoreProxy()
        {

            //查找科室信息
            HttpProxy pro = new HttpProxy();
            pro.Method = HttpMethod.POST;
            pro.Url = "../WebService/CostItems.ashx";
            this.SCostitem.Proxy.Add(pro);
            JsonReader jr = new JsonReader();
            jr.ReaderID = "ITEM_CODE";
            jr.Root = "costsitemlist";
            jr.TotalProperty = "totalitems";
            RecordField rf = new RecordField();
            rf.Name = "ITEM_CODE";
            jr.Fields.Add(rf);
            RecordField rfn = new RecordField();
            rfn.Name = "ITEM_NAME";
            jr.Fields.Add(rfn);
            this.SCostitem.Reader.Add(jr);
        }

        protected void Btn_Query_Click(object sender, AjaxEventArgs e)
        {
            string balance = cbbType.SelectedItem.Value;
            string deptcode = this.DeptFilter("");
            if (cbbdept.SelectedItem.Value.ToString()!="")
            {
                deptcode = "'"+cbbdept.SelectedItem.Value.ToString()+"'";
            }
           string reck = COST_CODE.SelectedItem.Value;
            string hospro = System.Configuration.ConfigurationManager.AppSettings["HOSPRO"];
            DataTable dt = report_dal.GetDeptCostDetail(GetBeginDate(), GetEndDate(), balance, deptcode, reck, "");
            if (dt != null)
            {
                SReport.DataSource = dt;
                SReport.DataBind();
                Session.Remove("DeptCostDetail");
                Session["DeptCostDetail"] = dt;
            }
            else
            {
                Goldnet.Ext.Web.Ext.Msg.Show(new Goldnet.Ext.Web.MessageBox.Config
                {
                    Title = "提示",
                    Message = "未找到数据",
                    Buttons = MessageBox.Button.OK,
                    Icon = (MessageBox.Icon)Enum.Parse(typeof(MessageBox.Icon), "INFO")
                });
            }
        }
        protected void OutExcel(object sender, EventArgs e)
        {
            if (Session["DeptCostDetail"] != null)
            {
                ExportData ex = new ExportData();
                DataTable dt = (DataTable)Session["DeptCostDetail"];

                string hospro = System.Configuration.ConfigurationManager.AppSettings["HOSPRO"];
                if (hospro == "1")
                {
                    dt.Columns.Remove("COSTS_ARMYFREE");
                    dt.Columns.Remove("TOTALCOST");
                }
                dt.Columns.Remove("DEPT_CODE");
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    if (dt.Columns[i].ColumnName == "DEPT_NAME")
                    {
                        dt.Columns[i].ColumnName = "科室";
                    }
                    else if (dt.Columns[i].ColumnName == "ITEM_NAME")
                    {
                        dt.Columns[i].ColumnName = "项目";
                    }
                    else if (dt.Columns[i].ColumnName == "COST_DATE")
                    {
                        dt.Columns[i].ColumnName = "发生日期";
                    }
                    else if (dt.Columns[i].ColumnName == "COSTS")
                    {
                        dt.Columns[i].ColumnName = "实际成本";
                    }
                    else if (dt.Columns[i].ColumnName == "COSTS_ARMYFREE")
                    {
                        dt.Columns[i].ColumnName = "计价成本";
                    }
                    else if (dt.Columns[i].ColumnName == "TOTALCOST")
                    {
                        dt.Columns[i].ColumnName = "总金额";
                    }
                    else if (dt.Columns[i].ColumnName == "OPERATORNAME")
                    {
                        dt.Columns[i].ColumnName = "录入员";
                    }
                }
                ex.ExportToLocal(dt, this.Page, "xls", "各单位月份成本报表");
            }
        }
        /// <summary>
        /// 开始时间
        /// </summary>
        /// <returns></returns>
        private string GetBeginDate()
        {
            string year = cbbYear.SelectedItem.Value.ToString();
            string month = cbbmonth.SelectedItem.Value.ToString();
            if (month.Length == 1)
            {
                month = "0" + month;
            }
            string benginDate = year + month + "01";
            return benginDate;
        }
        /// <summary>
        /// 结束时间
        /// </summary>
        /// <returns></returns>
        private string GetEndDate()
        {
            string year = ccbYearTo.SelectedItem.Value.ToString();
            string month = ccbMonthTo.SelectedItem.Value.ToString();
            if (month.Length == 1)
            {
                month = "0" + month;
            }
            string endDate = year + month + "01";
            return endDate;
        }
    }
}
