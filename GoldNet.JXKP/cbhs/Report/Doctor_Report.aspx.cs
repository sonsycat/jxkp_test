using System;
using Goldnet.Ext.Web;
using Goldnet.Dal;
using System.Data;
using GoldNet.Comm.ExportData;
using System.IO;
using System.Web;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;

namespace GoldNet.JXKP
{
    public partial class Doctor_Report : PageBase
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
                this.stardate.Value = System.DateTime.Now.ToString("yyyy-MM") + "-01";
                this.enddate.Value = System.DateTime.Now.ToString("yyyy-MM-dd");
                HttpProxy pro = new HttpProxy();
                pro.Method = HttpMethod.POST;
                //pro.Url = "WebService/AccountDepts.ashx?deptfilter=" + this.DeptFilter("dept_code");
                pro.Url = "../../../WebService/AccountDepts.ashx?deptfilter=" + this.DeptFilter("dept_code");
                this.Store2.Proxy.Add(pro);
                //data(System.DateTime.Now.ToString("yyyy-MM") + "-01", System.DateTime.Now.ToString("yyyy-MM-dd"));
                data("2000-01-01", "2000-01-01");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Btn_Query_Click(object sender, AjaxEventArgs e)
        {
            string stardate = Convert.ToDateTime(this.stardate.SelectedValue).ToString("yyyy-MM-dd");
            string enddate = Convert.ToDateTime(this.enddate.SelectedValue).ToString("yyyy-MM-dd");
            data(stardate, enddate);
            Session["dstdate"] = stardate;
            Session["deddate"] = enddate;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="stardate"></param>
        /// <param name="enddate"></param>
        private void data(string stardate, string enddate)
        {
            if (this.ComAccountdeptcode.SelectedItem.Value.ToString() == "" && stardate != "2000-01-01")
            {
                this.ShowMessage("提示", "请选择科室");
            }
            else
            {
                Report report_dal = new Report();

                string depttype = this.deptType.SelectedItem.Value.ToString();
                DataTable table = report_dal.GetDoctorDetail(stardate, enddate, depttype, ComAccountdeptcode.SelectedItem.Value.ToString(), cbbType.SelectedItem.Value);

                for (int i = 0; i < table.Columns.Count; i++)
                {
                    RecordField record = new RecordField();
                    record = new RecordField(table.Columns[i].ColumnName, RecordFieldType.String);
                    this.Store1.AddField(record);
                    Column cl = new Column();
                    cl.Header = table.Columns[i].ColumnName;
                    cl.Sortable = false;
                    cl.MenuDisabled = true;
                    cl.DataIndex = table.Columns[i].ColumnName;
                    TextField fils = new TextField();
                    fils.ReadOnly = true;

                    fils.ID = i.ToString();
                    fils.SelectOnFocus = false;
                    //fils.DecimalPrecision = 2;
                    cl.Editor.Add(fils);
                    if (cl.Header.Equals("ORDERED_BY_DOCTOR") || cl.Header.Equals("DEPT_CODE"))
                    {
                        cl.Hidden = true;
                    }
                    else if (cl.Header.Equals("ORDERED_BY_DOCTOR") || cl.Header.Equals("医生"))
                    {
                        cl.Align = Alignment.Right;
                    }
                    else
                    {
                        cl.Renderer.Fn = "rmbMoney";
                    }

                    this.GridPanel2.ColumnModel.Columns.Add(cl);
                }

                this.Store1.DataSource = table;
                this.Store1.DataBind();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void OutExcel(object sender, EventArgs e)
        {
            Report report_dal = new Report();
            string stardate = Session["dstdate"].ToString();
            string enddate = Session["deddate"].ToString();
            string depttype = this.deptType.SelectedItem.Value.ToString();
            DataTable dt = report_dal.GetDoctorDetail(stardate, enddate, depttype, ComAccountdeptcode.SelectedItem.Value.ToString(), cbbType.SelectedItem.Value);

            for (int i = 0; i < dt.Columns.Count; i++)
            {
                if (dt.Columns[i].ColumnName == "*")
                {
                    dt.Columns[i].ColumnName = "未知项目";
                }
                else if (dt.Columns[i].ColumnName == "dept_name")
                {
                    dt.Columns[i].ColumnName = "科室";
                }
            }

            // 创建工作簿
            IWorkbook workbook = new HSSFWorkbook();
            ISheet sheet = workbook.CreateSheet("医生收入统计表");

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
                Response.AddHeader("content-disposition", "attachment; filename=" + stardate + "至" + enddate + this.ComAccountdeptcode.SelectedItem.Text + "医生收入统计表.xls");
                Response.ContentType = "application/vnd.ms-excel";
                Response.BinaryWrite(exportData.ToArray());
                Response.End();
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void DbRowClick(object sender, AjaxEventArgs e)
        {
            RowSelectionModel sm = this.GridPanel2.SelectionModel.Primary as RowSelectionModel;
            if (sm.SelectedRows.Count < 1)
            {
                this.ShowMessage("提示", "请选择一条记录！");
            }
            else
            {
                string doctorname = sm.SelectedRow.RecordID;
                string stardate = Convert.ToDateTime(this.stardate.SelectedValue).ToString("yyyy-MM-dd");
                string enddate = Convert.ToDateTime(this.enddate.SelectedValue).ToString("yyyy-MM-dd");


                string deptname = deptType.SelectedItem.Value == "1" ? "INP_BILL_DETAIL" : "OUTP_BILL_ITEMS";


                LoadConfig loadcfg = getLoadConfig("Doctor_Report_Detail.aspx");

                loadcfg.Params.Add(new Goldnet.Ext.Web.Parameter("stardate", stardate));//开始时间
                loadcfg.Params.Add(new Goldnet.Ext.Web.Parameter("enddate", enddate));//结束时间
                loadcfg.Params.Add(new Goldnet.Ext.Web.Parameter("tablename", deptname));//表
                loadcfg.Params.Add(new Goldnet.Ext.Web.Parameter("doctorname", doctorname));//
                loadcfg.Params.Add(new Goldnet.Ext.Web.Parameter("patientid", ""));//
                loadcfg.Params.Add(new Goldnet.Ext.Web.Parameter("accountdeptcode", this.ComAccountdeptcode.SelectedItem.Value));//

                showCenterSet(this.Doctor_Detail, loadcfg);
            }
        }
    }
}
