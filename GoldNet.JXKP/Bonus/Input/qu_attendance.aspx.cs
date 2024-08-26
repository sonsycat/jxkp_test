using System;
using Goldnet.Ext.Web;
using Goldnet.Dal;
using System.Data;
using GoldNet.Comm.ExportData;

namespace GoldNet.JXKP.Bonus.Input
{
    public partial class qu_attendance : PageBase
    {
        /// <summary>
        /// 初始化处理
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

                SMonth.DataSource = boundcomm.getMonth();
                SMonth.DataBind();
                cbbmonth.Value = month;

                SetStoreProxy();
            }
        }

        /// <summary>
        /// 科室下拉列表初始化
        /// </summary>
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

        /// <summary>
        /// 查询处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Btn_Query_Click(object sender, AjaxEventArgs e)
        {
            string deptcode = this.DeptFilter("");
            string dept = "";
            if (deptcode != "" && cbbdept.SelectedItem.Value == "")
            {
                dept = " and k.dept_code in (" + deptcode + ") ";
            }
            else
            {
                if (cbbdept.SelectedItem.Value != "")
                {
                    dept = " and k.dept_code in (select DEPT_CODE from comm.sys_dept_dict b where b.ACCOUNT_DEPT_CODE='" + cbbdept.SelectedItem.Value + "') ";
                }
            }

            OutRestNonusDal dal = new OutRestNonusDal();

            DataTable dt = dal.Getquattendance(GetBeginDate(), dept);
            if (dt != null)
            {
                SReport.DataSource = dt;
                SReport.DataBind();
                Session.Remove("OutRestNonusAccount1");
                Session["OutRestNonusAccount1"] = dt;
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
        /// EXCEL导出
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void OutExcel(object sender, EventArgs e)
        {
            if (Session["OutRestNonusAccount1"] != null)
            {
                ExportData ex = new ExportData();
                DataTable dt = (DataTable)Session["OutRestNonusAccount1"];

                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    if (dt.Columns[i].ColumnName == "DATETIME")
                    {
                        dt.Columns[i].ColumnName = "时间";
                    }
                    else if (dt.Columns[i].ColumnName == "DEPT_NAME")
                    {
                        dt.Columns[i].ColumnName = "科室";
                    }
                    else if (dt.Columns[i].ColumnName == "DEPT_CODE")
                    {
                        dt.Columns[i].ColumnName = "科室代码";
                    }
                    else if (dt.Columns[i].ColumnName == "NAME")
                    {
                        dt.Columns[i].ColumnName = "姓名";
                    }
                    else if (dt.Columns[i].ColumnName == "JOB")
                    {
                        dt.Columns[i].ColumnName = "职务";
                    }
                    else if (dt.Columns[i].ColumnName == "ZWTS")
                    {
                        dt.Columns[i].ColumnName = "在岗天数";
                    }
                    else if (dt.Columns[i].ColumnName == "SUBSIDY")
                    {
                        dt.Columns[i].ColumnName = "职务标准";
                    }
                    else if (dt.Columns[i].ColumnName == "MONEYS")
                    {
                        dt.Columns[i].ColumnName = "职务津贴";
                    }

                }
                string dates = this.cbbYear.SelectedItem.Value + "年" + this.cbbmonth.SelectedItem.Value + "月";
                ex.ExportToLocal(dt, this.Page, "xls", "职务岗位津贴统计表(" + dates + ")");
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
            string benginDate = year + "-" + month + "-01";
            return benginDate;
        }

    }
}
