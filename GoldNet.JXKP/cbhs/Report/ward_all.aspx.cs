using System;
using System.Collections.Generic;
using Goldnet.Ext.Web;
using Goldnet.Dal;
using System.Data;
using GoldNet.Comm.ExportData;
using System.IO;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;

namespace GoldNet.JXKP
{
    public partial class ward_all : PageBase
    {
        DataTable table;
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
                ccbYearTo.Value = year;
                SMonth.DataSource = boundcomm.getMonth();
                SMonth.DataBind();
                cbbmonth.Value = month;
                ccbMonthTo.Value = month;
                costtype.Value = 1;
                SetDict();
                data(DateTime.Now.AddMonths(-1).ToString("yyyy-MM") + "-01", DateTime.Now.AddMonths(-1).ToString("yyyy-MM") + "-01");
            }
        }

        /// <summary>
        /// 字典设置
        /// </summary>
        public void SetDict()
        {
            Goldnet.Dal.SYS_DEPT_DICT dal = new Goldnet.Dal.SYS_DEPT_DICT();
            DataTable table = dal.GetDeptType().Tables[0];
            if (table.Rows.Count > 0)
            {
                for (int i = 0; i < table.Rows.Count; i++)
                {
                    this.deptType.Items.Add(new Goldnet.Ext.Web.ListItem(table.Rows[i]["ATTRIBUE"].ToString(), table.Rows[i]["id"].ToString()));
                }
            }
            //this.deptType.SelectedIndex = 1;
        }

        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="stdate"></param>
        /// <param name="enddate"></param>
        private void data(string stdate, string enddate)
        {
            //this.GridPanel2.Reconfigure();
            string deptcode = this.DeptFilter("");
            Report report_dal = new Report();

            string incomestype = this.incomestype.SelectedItem.Value == "" ? "0" : this.incomestype.SelectedItem.Value;
            string costtype = this.costtype.SelectedItem.Value == "" ? "1" : this.costtype.SelectedItem.Value;
            string depttype = this.deptType.SelectedItem.Value.ToString() == "" ? "" : this.deptType.SelectedItem.Value.ToString();
            string comestype = this.comestype.SelectedItem.Value == "" ? "0" : this.comestype.SelectedItem.Value;
            table = report_dal.GetWardall(stdate, enddate, deptcode, costtype, depttype, incomestype, comestype);
            for (int i = 0; i < table.Columns.Count; i++)
            {
                RecordField record = new RecordField();
                record = new RecordField(table.Columns[i].ColumnName, RecordFieldType.String);
                this.Store1.AddField(record);
                Column cl = new Column();
                cl.Header = table.Columns[i].ColumnName;
                cl.Sortable = false;
                cl.Align = Alignment.Right;
                cl.MenuDisabled = true;
                cl.DataIndex = table.Columns[i].ColumnName;
                TextField fils = new TextField();
                fils.ReadOnly = true;
                fils.ID = i.ToString();
                fils.SelectOnFocus = false;
                //fils.DecimalPrecision = 2;
                cl.Editor.Add(fils);
                if (cl.Header.Equals("DEPT_SNAP_DATE") || cl.Header.Equals("DEPT_CODE"))
                {
                    cl.Hidden = true;
                }
                else if (cl.Header.Equals("科室名称"))
                {
                    cl.Align = Alignment.Left;
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Btn_Query_Click(object sender, AjaxEventArgs e)
        {
            data(GetBeginDate(), GetEndDate());

            Session.Remove("wardall");
            Session["wardall"] = table;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void OutExcel(object sender, EventArgs e)
        {
            try
            {
                string deptcode = this.DeptFilter("");
                Report report_dal = new Report();

                string incomestype = this.incomestype.SelectedItem.Value == "" ? "0" : this.incomestype.SelectedItem.Value;
                string costtype = this.costtype.SelectedItem.Value == "" ? "1" : this.costtype.SelectedItem.Value;
                string comestype = this.comestype.SelectedItem.Value == "" ? "0" : this.comestype.SelectedItem.Value;
                DataTable dt = report_dal.GetWardall(GetBeginDate(), GetEndDate(), deptcode, costtype, this.deptType.SelectedItem.Value, incomestype, comestype);
                //ExportData ex = new ExportData();

                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    if (dt.Columns[i].ColumnName == "*")
                    {
                        dt.Columns[i].ColumnName = "未知项目";
                    }
                    else if (dt.Columns[i].ColumnName == "DEPT_NAME")
                    {
                        dt.Columns[i].ColumnName = "科室";
                    }
                }
                //this.outexcel(dt, "门诊收入统计表");

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
                ISheet sheet = workbook.CreateSheet("门诊收入统计表（核算前）");

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


                            if (j == 0)
                            {
                                cell.CellStyle = textStyle;
                            }
                            else
                            {
                                cell.CellStyle = currencyStyle;
                            }

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

                string excelName = "";

                if (comestype == "0")
                {
                    excelName = "全院收入统计表（核算前）.xls";
                }
                else if (comestype == "1")
                {
                    excelName = "门诊收入统计表（核算前）.xls";
                }
                else if (comestype == "2")
                {
                    excelName = "住院收入统计表（核算前）.xls";
                }


                // 导出为Excel文件
                using (MemoryStream exportData = new MemoryStream())
                {
                    workbook.Write(exportData);
                    Response.Clear();
                    if (s_date.Equals(e_date))
                    {
                        Response.AddHeader("content-disposition", "attachment; filename=" + s_date + excelName);
                    }
                    else
                    {
                        Response.AddHeader("content-disposition", "attachment; filename=" + s_date + "至" + e_date + excelName);
                    }
                    Response.ContentType = "application/vnd.ms-excel";
                    Response.BinaryWrite(exportData.ToArray());
                    Response.End();
                }
            }
            catch (Exception ex)
            {
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
            string endDate = year + "-" + month + "-01";
            return endDate;
        }

        //反序列化得到客户端提交的gridpanel数据行      
        private Dictionary<string, string>[] GetSelectRow(AjaxEventArgs e)
        {
            string row = e.ExtraParams["Values"].ToString();
            Dictionary<string, string>[] selectRow = JSON.Deserialize<Dictionary<string, string>[]>(row);
            if (selectRow.Length <= 0) { return null; } else { return selectRow; }
        }

        //双击设置
        //protected void DbRowClick(object sender, AjaxEventArgs e)
        //{

        //    RowSelectionModel sm = this.GridPanel2.SelectionModel.Primary as RowSelectionModel;
        //    if (sm.SelectedRows.Count < 1)
        //    {

        //        this.ShowMessage("提示", "请选择一条记录！");
        //    }
        //    else
        //    {
        //        string incomestype = this.incomestype.SelectedItem.Value == "" ? "0" : this.incomestype.SelectedItem.Value;
        //        string costtype = this.costtype.SelectedItem.Value == "" ? "1" : this.costtype.SelectedItem.Value;
        //        //
        //        string deptcode = sm.SelectedRow.RecordID;
        //        string deptname = "OUTP_CLASS2_INCOME";
        //        //string groupbydept = cbbDeptLevel.SelectedItem.Value == "0" ? "account_dept_code" : "DEPT_CODE_SECOND";

        //        //
        //        string groupbydept = "ORDERED_BY_DEPT";
        //        if (incomestype == "1")
        //        {
        //            groupbydept = "PERFORMED_BY";
        //        }
        //        if (incomestype == "2")
        //        {
        //            groupbydept = "WARD_CODE";
        //        }
        //        string incomescolums = "CHARGES";
        //        if (costtype == "0")
        //        {
        //            incomescolums = "COUNT_INCOME";
        //        }
        //        else if (costtype == "2")
        //        {
        //            incomescolums = "TRADE_PRICE";
        //        }
        //        LoadConfig loadcfg = getLoadConfig("DeptIncome_Detail.aspx");

        //        loadcfg.Params.Add(new Goldnet.Ext.Web.Parameter("stardate", GetBeginDate()));//开始时间
        //        loadcfg.Params.Add(new Goldnet.Ext.Web.Parameter("enddate", GetEndDate()));//结束时间
        //        loadcfg.Params.Add(new Goldnet.Ext.Web.Parameter("tablename", deptname));//表
        //        loadcfg.Params.Add(new Goldnet.Ext.Web.Parameter("deptcode", deptcode));//科室
        //        loadcfg.Params.Add(new Goldnet.Ext.Web.Parameter("groupbydept", groupbydept));//开单科室
        //        loadcfg.Params.Add(new Goldnet.Ext.Web.Parameter("incomescolums", incomescolums));//计价0/实收1
        //        loadcfg.Params.Add(new Goldnet.Ext.Web.Parameter("accountdept", "account_dept_code"));//二级科/三级科

        //        showCenterSet(this.Income_Detail, loadcfg);
        //    }
        //}

    }
}
