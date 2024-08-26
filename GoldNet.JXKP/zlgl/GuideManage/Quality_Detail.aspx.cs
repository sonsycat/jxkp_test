using System;
using System.Data;
using Goldnet.Ext.Web;
using GoldNet.Comm.ExportData;
using System.IO;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;

namespace GoldNet.JXKP.zlgl.SysManage
{
    public partial class Quality_Detail : PageBase
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
                HttpProxy pro = new HttpProxy();
                pro.Method = HttpMethod.POST;
                pro.Url = "../../../WebService/Depts.ashx";
                this.Store2.Proxy.Add(pro);

                this.stardate.Value = this.stardate.Value.ToString().Substring(0, 1) == "0" ? System.DateTime.Now.ToString("yyyy-MM") + "-01" : this.stardate.Value;
                this.enddate.Value = this.enddate.Value.ToString().Substring(0, 1) == "0" ? System.DateTime.Now.ToString("yyyy-MM-dd") : this.enddate.Value;

                Goldnet.Dal.Guide_Manager dal = new Goldnet.Dal.Guide_Manager();
                DataTable dt = dal.GetCheckcont();
                this.Store3.DataSource = dt;
                this.Store3.DataBind();
                this.ComboBox1.SelectedIndex = 0;

                this.GridPanel1.ColumnModel.RegisterCommandStyleRules();
                GetPageData();
            }
        }

        /// <summary>
        /// 查询处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void GetQueryPortalet(object sender, EventArgs e)
        {
            GetPageData();
        }

        /// <summary>
        /// 获取质量扣分明细
        /// </summary>
        private void GetPageData()
        {
            //string dept_code = this.DeptFilter("dept_code").ToString();
            string dept_code = this.DEPT.SelectedItem.Value.ToString();
            string checkont = this.ComboBox1.SelectedItem.Value.ToString();

            Goldnet.Dal.Guide_Manager dal = new Goldnet.Dal.Guide_Manager();
            DataTable dt = dal.GetQualityDetailbycheckont(Convert.ToDateTime(this.stardate.SelectedValue).ToString("yyyy-MM-dd"), Convert.ToDateTime(this.enddate.SelectedValue).ToString("yyyy-MM-dd"), "0", dept_code, checkont);
            this.Store1.DataSource = dt;
            this.Store1.DataBind();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void OutExcel(object sender, EventArgs e)
        {
            string dept_code = this.DeptFilter("dept_code").ToString();
            string checkont = this.ComboBox1.SelectedItem.Value.ToString();
            Goldnet.Dal.Guide_Manager dal = new Goldnet.Dal.Guide_Manager();
            DataTable dt = dal.GetQualityDetailbycheckont(Convert.ToDateTime(this.stardate.SelectedValue).ToString("yyyy-MM-dd"), Convert.ToDateTime(this.enddate.SelectedValue).ToString("yyyy-MM-dd"), "0", dept_code, checkont);
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
