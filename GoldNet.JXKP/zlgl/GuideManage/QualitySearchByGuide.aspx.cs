using System;
using System.Drawing;
using System.Data;
using System.Data.OleDb;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using Goldnet.Ext.Web;
using GoldNet.Comm;
using GoldNet.Comm.ExportData;
using GoldNet.Comm.Pic;
using GoldNet.Comm.DAL.Oracle;
using GoldNet.Model;
using GoldNet.JXKP.BLL.Guide;
using System.IO;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;

namespace GoldNet.JXKP.zlgl.SysManage
{
    public partial class QualitySearchByGuide : PageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Ext.IsAjaxRequest)
            {
                this.GridPanel1.ColumnModel.RegisterCommandStyleRules();
                ScriptManager1.RegisterIcon(Goldnet.Ext.Web.Icon.Accept);
                HttpProxy pro = new HttpProxy();
                pro.Method = HttpMethod.POST;

                pro.Url = "WebService/AccountDepts.ashx?deptfilter=" + this.DeptFilter("dept_code");
                this.Store2.Proxy.Add(pro);


                JsonReader jr = new JsonReader();
                jr.ReaderID = "DEPT_CODE";
                jr.Root = "deptlist";
                jr.TotalProperty = "totalCount";
                RecordField rf = new RecordField();
                rf.Name = "DEPT_CODE";
                jr.Fields.Add(rf);
                RecordField rfn = new RecordField();
                rfn.Name = "DEPT_NAME";
                jr.Fields.Add(rfn);
                this.Store2.Reader.Add(jr);
                //
                DataTable daterow = QualityGuide.GetAllDateDesc().Tables[0];
                this.Store3.DataSource = daterow;
                this.Store3.DataBind();
                this.stardate.SelectedIndex = 0;
                this.enddate.SelectedIndex = 0;
                //
                SetDict();
                //

                //GetPageData();

            }

        }

        public void SetDict()
        {
            Goldnet.Dal.Guide_Manager dal = new Goldnet.Dal.Guide_Manager();

            DataTable depttyperow = dal.Guide_Type_Dict().Tables[0];
            for (int i = 0; i < depttyperow.Rows.Count; i++)
            {
                this.ComGuide.Items.Add(new Goldnet.Ext.Web.ListItem(depttyperow.Rows[i]["GuideType"].ToString(), depttyperow.Rows[i]["ID"].ToString()));
            }
            if (this.stardate.SelectedItem.Value != "" && this.enddate.SelectedItem.Value != "")
            {
                this.GridPanel1.Reconfigure();


                int intStartDate = Convert.ToInt32(this.stardate.SelectedItem.Value.ToString());
                int intEndDate = Convert.ToInt32(this.enddate.SelectedItem.Value.ToString());

                string strDept = this.ComAccountdeptcode.SelectedItem.Value.Trim();
                int intGuide = Convert.ToInt32(this.ComGuide.SelectedItem.Value.ToString() == "" ? "0" : this.ComGuide.SelectedItem.Value.ToString());

                DataTable dt = QualityGuide.QualitySearchByGuide(intStartDate, intEndDate, strDept, intGuide, this.DeptFilter("dept_code"));

                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    RecordField field = new RecordField();
                    if (dt.Columns[i].ColumnName.Equals("科室"))
                    {
                        field = new RecordField(dt.Columns[i].ColumnName, RecordFieldType.String);
                    }
                    else
                    {
                        field = new RecordField(dt.Columns[i].ColumnName, RecordFieldType.Float);
                    }
                    this.Store1.AddField(field, i);
                    Column cl = new Column();
                    cl.Header = dt.Columns[i].ColumnName;
                    cl.Sortable = true;
                    cl.MenuDisabled = true;
                    cl.ColumnID = dt.Columns[i].ColumnName;
                    cl.DataIndex = dt.Columns[i].ColumnName;

                    this.GridPanel1.AddColumn(cl);
                }

                this.Store1.DataSource = dt;
                this.Store1.DataBind();
            }
        }


        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void GetQueryPortalet(object sender, EventArgs e)
        {
            GetPageData();
        }
        protected void Store_RefreshData(object sender, StoreRefreshDataEventArgs e)
        {
            GetPageData();
        }
        protected void Storedate_RefreshData(object sender, StoreRefreshDataEventArgs e)
        {
            DataTable daterow = QualityGuide.GetAllDateDesc().Tables[0];
            this.Store3.DataSource = daterow;
            this.Store3.DataBind();
            this.stardate.SelectedIndex = 0;
            this.enddate.SelectedIndex = 0;
        }
        /// <summary>
        /// 绑定数据
        /// </summary>
        private void GetPageData()
        {
            if (this.stardate.SelectedItem.Value.Equals(string.Empty))
            {
                this.ShowMessage("系统提示", "没有可以查询的数据！");
            }
            else
            {
                this.GridPanel1.Reconfigure();
                int intStartDate = Convert.ToInt32(this.stardate.SelectedItem.Value.ToString());
                int intEndDate = Convert.ToInt32(this.enddate.SelectedItem.Value.ToString());

                string strDept = this.ComAccountdeptcode.SelectedItem.Value.Trim();
                int intGuide = this.ComGuide.SelectedItem.Value.ToString() == "" ? 0 : Convert.ToInt32(this.ComGuide.SelectedItem.Value.ToString());

                DataTable dt = QualityGuide.QualitySearchByGuide(intStartDate, intEndDate, strDept, intGuide, this.DeptFilter("dept_code"));
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    RecordField field = new RecordField();
                    if (dt.Columns[i].ColumnName.Equals("科室"))
                    {
                        field = new RecordField(dt.Columns[i].ColumnName, RecordFieldType.String);
                    }
                    else
                    {
                        field = new RecordField(dt.Columns[i].ColumnName, RecordFieldType.Float);
                    }
                    this.Store1.AddField(field, i);
                    Column cl = new Column();
                    cl.Header = dt.Columns[i].ColumnName;
                    cl.Sortable = true;
                    cl.MenuDisabled = true;
                    cl.ColumnID = dt.Columns[i].ColumnName;
                    cl.DataIndex = dt.Columns[i].ColumnName;

                    this.GridPanel1.AddColumn(cl);
                }
                this.Store1.DataSource = dt;
                this.Store1.DataBind();
            }
        }
        protected void OutExcel(object sender, EventArgs e)
        {
            int intStartDate = Convert.ToInt32(this.stardate.SelectedItem.Value.ToString());
            int intEndDate = Convert.ToInt32(this.enddate.SelectedItem.Value.ToString());
            string strDept = this.ComAccountdeptcode.SelectedItem.Value.Trim();
            int intGuide = this.ComGuide.SelectedItem.Value.ToString() == "" ? 0 : Convert.ToInt32(this.ComGuide.SelectedItem.Value.ToString());
            DataTable dt = QualityGuide.QualitySearchByGuide(intStartDate, intEndDate, strDept, intGuide, this.DeptFilter("dept_code"));
            //string filename = this.stardate.SelectedItem.Text+"到" + this.enddate.SelectedItem.Text+this.ComAccountdeptcode.SelectedItem.Text + this.ComGuide.SelectedItem.Text + "质量数据";
            //ExportData ex = new ExportData();
            //ex.ExportToLocal(dt, this.Page, "xls", filename);

            // 创建工作簿
            IWorkbook workbook = new HSSFWorkbook();
            ISheet sheet = workbook.CreateSheet("质量数据");

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

            string s_date = this.stardate.SelectedItem.Text;
            string e_date = this.enddate.SelectedItem.Text;

            // 导出为Excel文件
            using (MemoryStream exportData = new MemoryStream())
            {
                workbook.Write(exportData);
                Response.Clear();
                if (s_date.Equals(e_date))
                {
                    Response.AddHeader("content-disposition", "attachment; filename=" + s_date + "质量数据.xls");
                }
                else
                {
                    Response.AddHeader("content-disposition", "attachment; filename=" + s_date + "至" + e_date + this.ComAccountdeptcode.SelectedItem.Text + this.ComGuide.SelectedItem.Text + "质量数据.xls");
                }
                Response.ContentType = "application/vnd.ms-excel";
                Response.BinaryWrite(exportData.ToArray());
                Response.End();
            }
        }

    }
}
