using System;
using System.Data;
using System.Collections.Generic;
using Goldnet.Ext.Web;
using Goldnet.Dal;
using GoldNet.Comm.ExportData;

namespace GoldNet.JXKP.RLZY.BaseInfoMaintain
{
    public partial class Staffworkday_ZWCQ_report : PageBase
    {
        /// <summary>
        /// 初始化
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
                ComboBox1.Value = year;

                SMonth.DataSource = boundcomm.getMonth();
                SMonth.DataBind();
                cbbmonth.Value = month;
                ComboBox2.Value = month;

                //data(DateTime.Now.AddMonths(-1).ToString("yyyyMM"));
            }
        }

        /// <summary>
        /// 查询数据并绑定
        /// </summary>
        /// <param name="stdate"></param>
        /// <param name="enddate"></param>
        private void data(string stdate, string enddate)
        {
            BaseInfoMaintainDal tdal = new BaseInfoMaintainDal();
            //科室权限
            string deptcode = this.DeptFilter("");

            DataTable dt = tdal.SearchZWCQ(stdate, deptcode, enddate).Tables[0];
            if (dt != null)
            {
                //DataRow dr = dt.NewRow();
                //dr["DEPT_NAME"] = "合计";
                //dr["NAME"] = dt.Compute("count(NAME)", "").ToString();
                //dr["DGTS"] = dt.Compute("Sum(DGTS)", "");
                //dr["ZWBZ"] = dt.Compute("Sum(ZWBZ)", "");
                //dr["DZJT"] = dt.Compute("Sum(DZJT)", "");
                //dt.Rows.Add(dr);

                Store1.DataSource = dt;
                Store1.DataBind();
                Session.Remove("Staffworkday_ZWCQ_report");
                Session["Staffworkday_ZWCQ_report"] = dt;
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

        /// <summary>
        /// 查询处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Btn_Query_Click(object sender, AjaxEventArgs e)
        {
            data(GetBeginDate(), GetEndDate());
        }

        /// <summary>
        /// EXCEL导出
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void OutExcel(object sender, EventArgs e)
        {

            if (Session["Staffworkday_ZWCQ_report"] != null)
            {
                ExportData ex = new ExportData();
                DataTable dt = (DataTable)Session["Staffworkday_ZWCQ_report"];


                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    if (dt.Columns[i].ColumnName == "ACCOUNT_DEPT_NAME")
                    {
                        dt.Columns[i].ColumnName = "科室";
                    }
                    else if (dt.Columns[i].ColumnName == "NAME")
                    {
                        dt.Columns[i].ColumnName = "姓名";
                    }
                    else if (dt.Columns[i].ColumnName == "DUTY")
                    {
                        dt.Columns[i].ColumnName = "职务";
                    }
                    else if (dt.Columns[i].ColumnName == "YCQ")
                    {
                        dt.Columns[i].ColumnName = "应出勤";
                    }
                    else if (dt.Columns[i].ColumnName == "SJCQ")
                    {
                        dt.Columns[i].ColumnName = "实际出勤";
                    }
                    else if (dt.Columns[i].ColumnName == "QQTS")
                    {
                        dt.Columns[i].ColumnName = "缺勤天数";
                    }
                }
                ex.ExportToLocal(dt, this.Page, "xls", "职务出勤天数统计");
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
            string benginDate = year + "" + month;
            return benginDate;
        }

        private string GetEndDate()
        {
            string year = ComboBox1.SelectedItem.Value.ToString();
            string month = ComboBox2.SelectedItem.Value.ToString();
            if (month.Length == 1)
            {
                month = "0" + month;
            }
            string benginDate = year + "" + month;
            return benginDate;
        }

        /// <summary>
        /// 反序列化得到客户端提交的gridpanel数据行   
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        private Dictionary<string, string>[] GetSelectRow(AjaxEventArgs e)
        {
            string row = e.ExtraParams["Values"].ToString();
            Dictionary<string, string>[] selectRow = JSON.Deserialize<Dictionary<string, string>[]>(row);
            if (selectRow.Length <= 0) { return null; } else { return selectRow; }
        }
    }
}
