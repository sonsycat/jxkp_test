using System;
using System.Data;
using System.Collections.Generic;
using Goldnet.Ext.Web;
using Goldnet.Dal;
using GoldNet.Comm.ExportData;

namespace GoldNet.JXKP.RLZY.BaseInfoMaintain
{
    public partial class StaffWorkday_ZK_report : PageBase
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

            DataTable dt = tdal.SearchZK(stdate, deptcode, enddate).Tables[0];
            if (dt != null)
            {
                Store1.DataSource = dt;
                Store1.DataBind();
                Session.Remove("StaffWorkday_ZK_report");
                Session["StaffWorkday_ZK_report"] = dt;
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

            if (Session["StaffWorkday_ZK_report"] != null)
            {
                ExportData ex = new ExportData();
                DataTable dt = (DataTable)Session["StaffWorkday_ZK_report"];


                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    if (dt.Columns[i].ColumnName == "CHANGE_DATE")
                    {
                        dt.Columns[i].ColumnName = "考勤月份";
                    }
                    else if (dt.Columns[i].ColumnName == "FROM_DEPT_NAME")
                    {
                        dt.Columns[i].ColumnName = "调出科室";
                    }
                    else if (dt.Columns[i].ColumnName == "DEPT_NAME")
                    {
                        dt.Columns[i].ColumnName = "调入科室";
                    }
                    else if (dt.Columns[i].ColumnName == "NAME")
                    {
                        dt.Columns[i].ColumnName = "姓名";
                    }
                    else if (dt.Columns[i].ColumnName == "SEX")
                    {
                        dt.Columns[i].ColumnName = "性别";
                    }
                    else if (dt.Columns[i].ColumnName == "DAYS")
                    {
                        dt.Columns[i].ColumnName = "实际出勤";
                    }
                    else if (dt.Columns[i].ColumnName == "MEMO")
                    {
                        dt.Columns[i].ColumnName = "备注";
                    }
                    else if (dt.Columns[i].ColumnName == "INPUT_USER")
                    {
                        dt.Columns[i].ColumnName = "考勤员";
                    }
                }
                ex.ExportToLocal(dt, this.Page, "xls", "人员调动报表");
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
            string benginDate = year + "" + month+"01";
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
            string benginDate = year + "" + month+"01";
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
