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
using System.IO;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;

namespace GoldNet.JXKP.zlgl
{
    public partial class BSC_Quality_Detail : PageBase
    {
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

        private void GetPageData()
        {
            string dept_code = this.DeptFilter("dept_code").ToString();
            Goldnet.Dal.Guide_Manager dal = new Goldnet.Dal.Guide_Manager();
            DataTable dt = dal.GetQualityDetail(Convert.ToDateTime(this.stardate.SelectedValue).ToString("yyyy-MM-dd"), Convert.ToDateTime(this.enddate.SelectedValue).ToString("yyyy-MM-dd"), "1",dept_code);
            this.Store1.DataSource = dt;
            this.Store1.DataBind();
        }
        protected void OutExcel(object sender, EventArgs e)
        {
            string dept_code = this.DeptFilter("dept_code").ToString();
            Goldnet.Dal.Guide_Manager dal = new Goldnet.Dal.Guide_Manager();
            DataTable dt = dal.GetQualityDetail(Convert.ToDateTime(this.stardate.SelectedValue).ToString("yyyy-MM-dd"), Convert.ToDateTime(this.enddate.SelectedValue).ToString("yyyy-MM-dd"), "1",dept_code);
            for (int i = 0; i < dt.Columns.Count; i++)
            {
                if (dt.Columns[i].ColumnName == "ID")
                    dt.Columns[i].ColumnName = "编号";
                if (dt.Columns[i].ColumnName == "DUTY_DEPT_ID")
                    dt.Columns[i].ColumnName = "科室代码";
                if (dt.Columns[i].ColumnName == "DUTY_DEPT_NAME")
                    dt.Columns[i].ColumnName = "科室名称";
                if (dt.Columns[i].ColumnName == "DUTY_USER_NAME")
                    dt.Columns[i].ColumnName = "责任人";
                if (dt.Columns[i].ColumnName == "CHECKCONT")
                    dt.Columns[i].ColumnName = "考核内容";
                if (dt.Columns[i].ColumnName == "CHECKSTAN")
                    dt.Columns[i].ColumnName = "考核标准";
                if (dt.Columns[i].ColumnName == "DATE_TIME")
                    dt.Columns[i].ColumnName = "考评时间";
                if (dt.Columns[i].ColumnName == "NUMBERS")
                    dt.Columns[i].ColumnName = "扣分";
                if (dt.Columns[i].ColumnName == "MEMO")
                    dt.Columns[i].ColumnName = "备注";
                if (dt.Columns[i].ColumnName == "INPUT_USER")
                    dt.Columns[i].ColumnName = "创建者";
                if (dt.Columns[i].ColumnName == "CREATEDATE")
                    dt.Columns[i].ColumnName = "创建时间";
            }
            //ExportData ex = new ExportData();

            //ex.ExportToLocal(dt, this.Page, "xls", "质量扣分明细");

            string s_date = Convert.ToDateTime(this.stardate.SelectedValue).ToString("yyyy-MM");
            string e_date = Convert.ToDateTime(this.enddate.SelectedValue).ToString("yyyy-MM");

            // 创建工作簿
            IWorkbook workbook = new HSSFWorkbook();
            ISheet sheet = workbook.CreateSheet("质量扣分明细");

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

                        cell.CellStyle = numberStyle; // 数值格式
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
                    Response.AddHeader("content-disposition", "attachment; filename=" + s_date + "质量扣分明细.xls");
                }
                else
                {
                    Response.AddHeader("content-disposition", "attachment; filename=" + s_date + "至" + e_date + "质量扣分明细.xls");
                }
                Response.ContentType = "application/vnd.ms-excel";
                Response.BinaryWrite(exportData.ToArray());
                Response.End();
            }
        }

    }
}
