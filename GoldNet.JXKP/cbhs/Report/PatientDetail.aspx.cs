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

namespace GoldNet.JXKP.cbhs.Report
{
    public partial class PatientDetail : PageBase
    {
        private Goldnet.Dal.Report report_dal = new Goldnet.Dal.Report();

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
                SetStoreProxy();
            }
        }

        private void SetStoreProxy()
        {
            //查找科室信息
            HttpProxy pro = new HttpProxy();
            pro.Method = HttpMethod.POST;
            pro.Url = "../../cbhs/WebService/BonusDepts.ashx?deptfilter=" + this.DeptFilter("dept_code");
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

        protected void Btn_Query_Click(object sender, AjaxEventArgs e)
        {

            string deptcode = this.DeptFilter("");
            string dept = "";
            if (deptcode != "" && cbbdept.SelectedItem.Value == "")
            {
                dept = " and b.dept_code in (" + deptcode + ") ";
            }
            else
            {
                if (cbbdept.SelectedItem.Value != "")
                {
                    dept = " and b.dept_code in (select DEPT_CODE from comm.sys_dept_dict b where b.ACCOUNT_DEPT_CODE='" + cbbdept.SelectedItem.Value + "') ";
                }
            }


            DataTable dt = report_dal.GetPatientIncomeDetail(GetBeginDate(), GetEndDate(), dept, cbbType.SelectedItem.Value);
            if (dt != null)
            {
                SReport.DataSource = dt;
                SReport.DataBind();
                Session.Remove("ClinicDetail");
                Session["ClinicDetail"] = dt;
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
            if (Session["ClinicDetail"] != null)
            {
                ExportData ex = new ExportData();
                DataTable dt = (DataTable)Session["ClinicDetail"];



                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    if (dt.Columns[i].ColumnName == "DEPT_NAME")
                    {
                        dt.Columns[i].ColumnName = "科室";
                    }
                    else if (dt.Columns[i].ColumnName == "DOCTOR")
                    {
                        dt.Columns[i].ColumnName = "医生";
                    }
                    else if (dt.Columns[i].ColumnName == "CLINIC_TYPE")
                    {
                        dt.Columns[i].ColumnName = "费别";
                    }
                    else if (dt.Columns[i].ColumnName == "NUM")
                    {
                        dt.Columns[i].ColumnName = "工作量";
                    }


                }
                string dates = this.cbbYear.SelectedItem.Value + "年" + this.cbbmonth.SelectedItem.Value + "月-" + this.ccbYearTo.SelectedItem.Value + "年" + this.ccbMonthTo.SelectedItem.Value + "月";
                ex.ExportToLocal(dt, this.Page, "xls", "门诊工作量统计表(" + dates + ")");
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
