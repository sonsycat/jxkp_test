using System;
using System.Collections.Generic;
using Goldnet.Dal;
using Goldnet.Ext.Web;
using System.Data;
using System.IO;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;

namespace GoldNet.JXKP
{
    public partial class BonusSubmitEdit : PageBase
    {
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
                SMonths.DataSource = boundcomm.getMonth();
                SMonths.DataBind();
                cbbmonth.Value = month;
                
                data(DateTime.Now.AddMonths(-2).ToString("yyyyMM"));
            }
        }


        /// <summary>
        /// 删除按钮触发事件(返回按钮)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Btn_Del_Click(object sender, AjaxEventArgs e)
        {
            Dictionary<string, string>[] selectRow = GetSelectRow(e);
            if (selectRow != null)
            {
                string index_id = selectRow[0]["INDEX_ID"].ToString();
                string dept_code = selectRow[0]["DEPT_CODE"].ToString();
                string flags = selectRow[0]["FLAGS"].ToString();
                CalculateBonus calculateBons = new CalculateBonus();
                try
                {
                    if (calculateBons.DeleteGetSubmitBonus(GetBeginDate(), index_id, dept_code, flags))
                    {
                        Goldnet.Ext.Web.Ext.Msg.Show(new Goldnet.Ext.Web.MessageBox.Config
                        {
                            Title = "提示",
                            Message = "返回提交成功",
                            Buttons = MessageBox.Button.OK,
                            Icon = (MessageBox.Icon)Enum.Parse(typeof(MessageBox.Icon), "INFO")
                        });
                        Store_RefreshData(null, null);
                    }
                    else
                    {
                        this.ShowMessage("提示","返回失败");
                        Store_RefreshData(null, null);
                    }
                    
                }
                catch (Exception ex)
                {
                    ShowDataError(ex.ToString(), Request.Url.LocalPath, "DeleteEditSimoleEncourage");
                }
            }
        }

        protected void OutExcel(object sender, EventArgs e)
        {
            if (Session["BonusSubmitEdit"] != null)
            {
                CalculateBonus calculateBons = new CalculateBonus();
                DataTable dt = calculateBons.GetSubmitBonus(GetBeginDate(),1);

                string year = cbbYear.SelectedItem.Value.ToString();
                string month = cbbmonth.SelectedItem.Value.ToString();
                if (month.Length == 1)
                {
                    month = "0" + month;
                }
                string s_date = year + "-" + month;


                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    string columnName = dt.Columns[i].ColumnName;
                    if (columnName.Contains("ST_DATE"))
                    {
                        dt.Columns[i].ColumnName = "日期";
                    }
                    else if (columnName.Contains("INDEX_ID"))
                    {
                        dt.Columns[i].ColumnName = "奖金ID";
                    }
                    else if (columnName.Contains("DEPT_CODE"))
                    {
                        dt.Columns[i].ColumnName = "科室编码";
                    }
                    else if (columnName.Contains("DEPT_NAME"))
                    {
                        dt.Columns[i].ColumnName = "科室名称";
                    }
                    else if (columnName.Contains("FLAGS"))
                    {
                        dt.Columns[i].ColumnName = "奖金类型";
                    }
                    else if (columnName.Contains("ISCOMMIT"))
                    {
                        dt.Columns[i].ColumnName = "是否提交";
                    }
                }


                // 创建工作簿
                IWorkbook workbook = new HSSFWorkbook();
                ISheet sheet = workbook.CreateSheet("二次分配提交科室");

                // 创建表头
                IRow headerRow = sheet.CreateRow(0);
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    ICell cell = headerRow.CreateCell(i);
                    cell.SetCellValue(dt.Columns[i].ColumnName);
                }

                // 创建样式
                ICellStyle numberStyle = workbook.CreateCellStyle();
                numberStyle.DataFormat = HSSFDataFormat.GetBuiltinFormat("0");// 数值格式

                ICellStyle textStyle = workbook.CreateCellStyle();
                textStyle.DataFormat = HSSFDataFormat.GetBuiltinFormat("@");// 文本格式

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

                            cell.CellStyle = textStyle; // 金钱格式
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
                    Response.AddHeader("content-disposition", "attachment; filename=" + s_date + "二次分配提交科室.xls");
                    Response.ContentType = "application/vnd.ms-excel";
                    Response.BinaryWrite(exportData.ToArray());
                    Response.End();
                }
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Store_RefreshData(object sender, StoreRefreshDataEventArgs e)
        {
            //绑定Store数据源
            CalculateBonus calculateBons = new CalculateBonus();
            DataTable table = calculateBons.GetSubmitBonus(GetBeginDate());
            this.Store1.DataSource = table;
            this.Store1.DataBind();
        }

        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="stdate"></param>
        /// <param name="enddate"></param>
        private void data(string stdate)
        {

            CalculateBonus calculateBons = new CalculateBonus();
            DataTable table = calculateBons.GetSubmitBonus(stdate);
            this.Store1.DataSource = table;
            this.Store1.DataBind();
            this.GridPanel2.Reconfigure();  //清除列内容重新加载
            Session.Remove("BonusSubmitEdit");
            Session["BonusSubmitEdit"] = table;
        }

        protected void Btn_Query_Click(object sender, AjaxEventArgs e)
        {
            data(GetBeginDate());
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
            string benginDate = year + month;
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
