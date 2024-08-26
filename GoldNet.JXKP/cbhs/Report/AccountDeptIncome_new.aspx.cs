using System;
using Goldnet.Ext.Web;
using Goldnet.Dal;
using System.Data;
using GoldNet.Comm.ExportData;
using System.IO;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;

namespace GoldNet.JXKP
{
    public partial class AccountDeptIncome_new : PageBase
    {
        private Report report_dal = new Report();

        /// <summary>
        /// 初始化处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Ext.IsAjaxRequest)
            {
                //初始化查询日期
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

                //初始化类型、科室列表
                SetStoreProxy();
                cbbType.SelectedItem.Value = "0";

                //包含中心收入
                ComboBox1.Value = 1;

                string hospro = System.Configuration.ConfigurationManager.AppSettings["HOSPRO"];
                if (hospro == "1")
                {
                    this.GridPanel_Show.ColumnModel.Columns[5].Hidden = true;
                    this.GridPanel_Show.ColumnModel.Columns[6].Hidden = true;
                    this.GridPanel_Show.ColumnModel.Columns[7].Hidden = true;
                }
            }
        }

        /// <summary>
        /// 初始化类型、科室列表
        /// </summary>
        private void SetStoreProxy()
        {
            //查找科室信息
            HttpProxy pro = new HttpProxy();
            pro.Method = HttpMethod.POST;
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

        /// <summary>
        /// 查询处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Btn_Query_Click(object sender, AjaxEventArgs e)
        {
            string balance = cbbType.SelectedItem.Value;
            string deptcode = this.DeptFilter("");
            string dept = cbbdept.SelectedItem.Value;

            //默认不包含中心收入
            string centerincome = this.ComboBox1.SelectedItem.Value == "" ? "1" : this.ComboBox1.SelectedItem.Value;

            DataTable dt = report_dal.GetAccountDeptIncome_new(GetBeginDate(), GetEndDate(), balance, dept, centerincome);
            if (dt != null)
            {
                DataRow dr = dt.NewRow();
                dr["DEPT_NAME"] = "合计";
                dr["HJ"] = dt.Compute("Sum(HJ)", "");
                dr["ZYHJ"] = dt.Compute("Sum(ZYHJ)", "");
                dr["ZYSS"] = dt.Compute("Sum(ZYSS)", "");
                dr["ZYJD"] = dt.Compute("Sum(ZYJD)", "");
                dr["MZHJ"] = dt.Compute("Sum(MZHJ)", "");
                dr["MZSS"] = dt.Compute("Sum(MZSS)", "");
                dr["MZJD"] = dt.Compute("Sum(MZJD)", "");
                dt.Rows.Add(dr);

                SReport.DataSource = dt;
                SReport.DataBind();
                Session.Remove("Account_Dept_Income");
                Session["Account_Dept_Income"] = dt;
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
            if (Session["Account_Dept_Income"] != null)
            {
                ExportData ex = new ExportData();
                DataTable dt = (DataTable)Session["Account_Dept_Income"];

                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    if (dt.Columns[i].ColumnName == "DEPT_NAME")
                    {
                        dt.Columns[i].ColumnName = "科室";
                    }
                    else if (dt.Columns[i].ColumnName == "HJ")
                    {
                        dt.Columns[i].ColumnName = "总收入";
                    }
                    else if (dt.Columns[i].ColumnName == "ZYHJ")
                    {
                        dt.Columns[i].ColumnName = "住院小计";
                    }
                    else if (dt.Columns[i].ColumnName == "ZYSS")
                    {
                        dt.Columns[i].ColumnName = "住院实收";
                    }
                    else if (dt.Columns[i].ColumnName == "ZYJD")
                    {
                        dt.Columns[i].ColumnName = "住院计价";
                    }
                    else if (dt.Columns[i].ColumnName == "MZHJ")
                    {
                        dt.Columns[i].ColumnName = "门诊小计";
                    }
                    else if (dt.Columns[i].ColumnName == "MZSS")
                    {
                        dt.Columns[i].ColumnName = "门诊实收";
                    }
                    else if (dt.Columns[i].ColumnName == "MZJD")
                    {
                        dt.Columns[i].ColumnName = "门诊计价";
                    }
                }
                //string dates = this.cbbYear.SelectedItem.Value + "年" + this.cbbmonth.SelectedItem.Value + "月-" + this.ccbYearTo.SelectedItem.Value + "年" + this.ccbMonthTo.SelectedItem.Value + "月";
                //ex.ExportToLocal(dt, this.Page, "xls", "核算单位医疗收入统计报表(" + dates + ")");

                string year = cbbYear.SelectedItem.Value.ToString();
                string month = cbbmonth.SelectedItem.Value.ToString();
                if (month.Length == 1)
                {
                    month = "0" + month;
                }
                string s_date = year + "-" + month;

                string yearTo = ccbYearTo.SelectedItem.Value.ToString();
                string monthTo = ccbMonthTo.SelectedItem.Value.ToString();
                if (monthTo.Length == 1)
                {
                    monthTo = "0" + monthTo;
                }
                string e_date = yearTo + "-" + monthTo;

                // 创建工作簿
                IWorkbook workbook = new HSSFWorkbook();
                ISheet sheet = workbook.CreateSheet("核算单位医疗收入统计报表（核算后）");

                // 创建表头
                IRow headerRow = sheet.CreateRow(0);
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    ICell cell = headerRow.CreateCell(i);
                    cell.SetCellValue(dt.Columns[i].ColumnName);
                }

                // 创建样式
                ICellStyle numberStyle = workbook.CreateCellStyle();
                numberStyle.DataFormat = HSSFDataFormat.GetBuiltinFormat("0");

                ICellStyle textStyle = workbook.CreateCellStyle();
                textStyle.DataFormat = HSSFDataFormat.GetBuiltinFormat("@");

                ICellStyle currencyStyle = workbook.CreateCellStyle();
                currencyStyle.DataFormat = HSSFDataFormat.GetBuiltinFormat("#,##0.00"); // 金钱格式

                // 填充数据
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    IRow row = sheet.CreateRow(i + 1);
                    for (int j = 0; j < dt.Columns.Count; j++)
                    {
                        ICell cell = row.CreateCell(j);
                        string cellValue = dt.Rows[i][j].ToString();
                        double number;

                        if (double.TryParse(cellValue, out number))
                        {
                            cell.SetCellValue(number);

                            // 如果是金钱列，设置为金钱格式；否则设置为数值格式
                            //if (j == /* 你的金钱列索引，例如第5列 */)
                            //{
                            //    cell.CellStyle = numberStyle;
                            //}
                            //else
                            //{
                            //    cell.CellStyle = currencyStyle;
                            //}

                            cell.CellStyle = currencyStyle; // 数值格式
                        }
                        else
                        {
                            cell.SetCellValue(cellValue);
                            cell.CellStyle = textStyle; // 文本格式
                        }
                    }
                }

                // 自动调整列宽
                int maxColumnWidth = 30 * 256; // 最大宽度设置为30字符宽
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    sheet.AutoSizeColumn(i);
                    int currentWidth = sheet.GetColumnWidth(i);
                    if (currentWidth > maxColumnWidth)
                    {
                        sheet.SetColumnWidth(i, maxColumnWidth);
                    }
                }

                // 导出为Excel文件
                using (MemoryStream exportData = new MemoryStream())
                {
                    workbook.Write(exportData);
                    Response.Clear();
                    if (s_date.Equals(e_date))
                    {
                        Response.AddHeader("content-disposition", "attachment; filename=" + s_date + "核算单位医疗收入统计报表（核算后）.xls");
                    }
                    else
                    {
                        Response.AddHeader("content-disposition", "attachment; filename=" + s_date + "至" + e_date + "核算单位医疗收入统计报表（核算后）.xls");
                    }
                    Response.ContentType = "application/vnd.ms-excel";
                    Response.BinaryWrite(exportData.ToArray());
                    Response.End();
                }
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
