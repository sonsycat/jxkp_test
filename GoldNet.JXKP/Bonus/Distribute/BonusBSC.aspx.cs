using System;
using Goldnet.Ext.Web;
using System.Data;
using Goldnet.Dal.Properties.Bound;
using GoldNet.Comm.ExportData;
using System;
using Goldnet.Dal;
using Goldnet.Ext.Web;
using System.Data;
using GoldNet.Comm.ExportData;
using System.IO;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;


namespace GoldNet.JXKP.Bonus.Distribute
{
    public partial class BonusBSC : PageBase
    {
        /// <summary>
        /// 初始化页面
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Ext.IsAjaxRequest)
            {
                BoundComm boundcomm = new BoundComm();
                //初始化年
                SYear.DataSource = boundcomm.getYears();
                SYear.DataBind();
                cbbYear.SetValue(DateTime.Now.Year);
                //初始化月
                SMonths.DataSource = boundcomm.getMonth();
                SMonths.DataBind();
                cbbmonth.SetValue(DateTime.Now.Month);
                //初始化条件大于，小于等
                StoreRelation.DataSource = boundcomm.dtBonusAmount();
                StoreRelation.DataBind();
            }
        }
        protected void OutExcel(object sender, EventArgs e)
        {
            if (Session["BonusBSC"] != null)
            {
                ExportData ex = new ExportData();
                DataTable dt = (DataTable)Session["BonusBSC"];

                //ex.ExportToLocal(dt, this.Page, "xls", "绩效汇总");
                //MHeaderTabletoExcel(dt, null, null, null, 0);
                //ex.ExportToLocal(l_dt, this.Page, "xls", "人员信息");

                string year = cbbYear.SelectedItem.Value.ToString();
                string month = cbbmonth.SelectedItem.Value.ToString();
                if (month.Length == 1)
                {
                    month = "0" + month;
                }
                string s_date = year + "-" + month;

                // 创建工作簿
                IWorkbook workbook = new HSSFWorkbook();
                ISheet sheet = workbook.CreateSheet("绩效汇总");

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

                            cell.CellStyle = currencyStyle; // 金钱格式
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
                    Response.AddHeader("content-disposition", "attachment; filename=" + s_date + "绩效汇总.xls");
                    Response.ContentType = "application/vnd.ms-excel";
                    Response.BinaryWrite(exportData.ToArray());
                    Response.End();
                }
            }

        }
        /// <summary>
        /// 根据页面输入的条件，查询人员奖金
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Btn_Search_Click(object sender, EventArgs e)
        {
            string years = cbbYear.SelectedItem.Value;
            string months = cbbmonth.SelectedItem.Value;
            if (months.Length == 1)
            {
                months = "0" + months;
            }
            string benginDate = years + "" + months;
            //string deptName = tfDeptName.Text;
            //string staffName = tfStaff.Text;
            //string bankCode = tfBankCode.Text;
            //string bonus = "";
            //if (cbbBonus.SelectedItem.Value != "")
            //{
            //    bonus = cbbBonus.SelectedItem.Value + nfBonus.Text;
            //}

            //查询人员奖金
            CalculateBonus calculateBonus_dal = new CalculateBonus();
            DataTable dt = calculateBonus_dal.SearchBonusBSC(benginDate);
            if (dt.Rows.Count > 0)
            {//找到时绑定数据源
                SSearch.DataSource = dt;
                SSearch.DataBind();
                Session.Remove("BonusBSC");
                Session["BonusBSC"] = calculateBonus_dal.SearchBonusBSC(benginDate);
            }
            else
            {
                Goldnet.Ext.Web.Ext.Msg.Show(new Goldnet.Ext.Web.MessageBox.Config
                {
                    Title = "提示",
                    Message = "未找到奖金",
                    Buttons = MessageBox.Button.OK,
                    Icon = (MessageBox.Icon)Enum.Parse(typeof(MessageBox.Icon), "INFO")
                });
            }
        }
    }
}