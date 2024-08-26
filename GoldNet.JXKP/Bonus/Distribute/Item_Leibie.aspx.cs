using System;
using System.Data;
using Goldnet.Ext.Web;
using Goldnet.Dal;
using System.Collections.Generic;
using Goldnet.Comm.ExportData;
using GoldNet.Comm;
using System.Threading;
using System.IO;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using System.Web;
using NPOI.XSSF;
using NPOI.XSSF.UserModel;
using NPOI.OpenXmlFormats;

namespace GoldNet.JXKP.Bonus.Distribute
{
    public partial class Item_Leibie : PageBase
    {
        DataTable table;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Ext.IsAjaxRequest)
            {

                SetStoreProxy();
                string item = this.ItemLeibie.SelectedItem.Value.ToString();
                //data(null);
            }
        }

        //下拉框初始化
        private void SetStoreProxy()
        {

            HttpProxy pro3 = new HttpProxy();
            pro3.Method = HttpMethod.POST;
            pro3.Url = "../../../WebService/ItemLeibie.ashx";
            this.Store3.Proxy.Add(pro3);
        }

        /// 获取绑定
        private void data(string item)
        {
            string deptcode = this.DeptFilter("");
            DeptPercent deptpercent = new DeptPercent();
            table = deptpercent.Item_Leibie_Select(item);

            this.Store1.DataSource = table;
            this.Store1.DataBind();
        }

        /// 查询
        protected void Query(object sender, AjaxEventArgs e)
        {
            string item = this.ItemLeibie.SelectedItem.Value.ToString();
            data(item);
            Session.Remove("itemleibie");
            Session["itemleibie"] = table;
        }

        /// 保存修改
        protected void Save(object sender, AjaxEventArgs e)
        {
            Dictionary<string, string>[] selectRow = GetSelectRow(e);
            if (selectRow != null)
            {
                DeptPercent deptpercent = new DeptPercent();
                if (deptpercent.Item_Leibie_Save(selectRow))
                {
                    this.ShowMessage("系统提示", "保存成功！");
                    string item = this.ItemLeibie.SelectedItem.Value.ToString();
                    data(item);
                }
                else
                {
                    ShowDataError("", Request.Url.LocalPath, "Save");
                }
            }
        }

        //导出
        protected void OutExcel(object sender, EventArgs e)
        {

            DataTable dt = (DataTable)Session["itemleibie"];

            
            for (int i = 0; i < dt.Columns.Count; i++)
            {
                if (dt.Columns[i].ColumnName == "ITEM_CODE")
                {
                    dt.Columns[i].ColumnName = "项目编码";
                }
                else if (dt.Columns[i].ColumnName == "ITEM_NAME")
                {
                    dt.Columns[i].ColumnName = "项目名称";
                }
                else if (dt.Columns[i].ColumnName == "CLASS_NAME")
                {
                    dt.Columns[i].ColumnName = "类型名称";
                }
                else if (dt.Columns[i].ColumnName == "ITEM_UNIT")
                {
                    dt.Columns[i].ColumnName = "规格";
                }
                else if (dt.Columns[i].ColumnName == "ITEM_PRICE")
                {
                    dt.Columns[i].ColumnName = "价格";
                }
                else if (dt.Columns[i].ColumnName == "PANDU")
                {
                    dt.Columns[i].ColumnName = "盘读";
                }
                else if (dt.Columns[i].ColumnName == "ZHIXING")
                {
                    dt.Columns[i].ColumnName = "执行";
                }
                else if (dt.Columns[i].ColumnName == "HULI")
                {
                    dt.Columns[i].ColumnName = "护理";
                }

            }


            // 创建工作簿
            IWorkbook workbook = new HSSFWorkbook();
            ISheet sheet = workbook.CreateSheet("当量积分设置");

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
                string fileName = HttpUtility.UrlEncode("当量积分设置.xls", System.Text.Encoding.UTF8);
                Response.AddHeader("content-disposition", "attachment; filename*=UTF-8''" + fileName + "");
                Response.ContentType = "application/vnd.ms-excel";
                Response.BinaryWrite(exportData.ToArray());
                Response.End();
            }
        }

        private bool IsNullOrWhiteSpace(string value)
        {
            return string.IsNullOrEmpty(value) || value.Trim().Length == 0;
        }

        //导入
        protected void ImportExcel(object sender, AjaxEventArgs e)
        {
            if (FileUploadField1.HasFile)
            {
                try
                {
                    // 读取上传的文件
                    using (Stream fileStream = FileUploadField1.PostedFile.InputStream)
                    {

                        // 获取存储路径
                        string customPath = "../../resources/ExportDataTemp/";

                        // 确保路径存在
                        string absolutePath = Server.MapPath(customPath);
                        if (!Directory.Exists(absolutePath))
                        {
                            Directory.CreateDirectory(absolutePath);
                        }

                        // 获取文件名并保存文件
                        string fileName = Path.GetFileName(FileUploadField1.PostedFile.FileName);
                        string filePath = Path.Combine(absolutePath, fileName);

                        // 保存文件到服务器
                        FileUploadField1.PostedFile.SaveAs(filePath);

                        // 创建工作簿
                        IWorkbook workbook = new XSSFWorkbook(fileStream);
                        ISheet sheet = workbook.GetSheetAt(0); // 获取第一个工作表

                        // 获取数据表结构
                        DataTable dataTable = new DataTable();
                        IRow headerRow = sheet.GetRow(0);

                        if (headerRow != null) // 检查表头行是否为 null
                        {
                            int cellCount = headerRow.LastCellNum;

                            for (int i = 0; i < cellCount; i++)
                            {
                                ICell headerCell = headerRow.GetCell(i);
                                if (headerCell != null)
                                {
                                    dataTable.Columns.Add(headerCell.ToString());
                                }
                                else
                                {
                                    dataTable.Columns.Add("Column{i}");
                                }
                            }

                            // 读取数据行
                            for (int i = 1; i <= sheet.LastRowNum; i++)
                            {
                                IRow row = sheet.GetRow(i);
                                if (row != null) // 检查行是否为 null
                                {
                                    DataRow dataRow = dataTable.NewRow();

                                    for (int j = 0; j < cellCount; j++)
                                    {
                                        ICell cell = row.GetCell(j);
                                        if (cell != null)
                                        {
                                            dataRow[j] = cell.ToString();
                                        }
                                        else
                                        {
                                            dataRow[j] = DBNull.Value; // 或者留空
                                        }
                                    }

                                    dataTable.Rows.Add(dataRow);
                                }
                            }
                        }

                        // 将 DataTable 数据插入到数据库中
                        Appended_income dal = new Appended_income();
                        if (dal.saveDljf_Dr(dataTable))
                        {
                            ShowMessage("系统提示", "导入成功！");
                            data(null); ; // 刷新显示数据
                        }
                        else
                        {
                            ShowMessage("系统提示", "导入失败！");
                        }
                    }
                }
                catch (Exception ex)
                {
                    ShowMessage("系统提示", "导入时出错：" + ex.Message);
                }
            }
            else
            {
                ShowMessage("系统提示", "请先选择要导入的文件。");
            }
        }



        // 刷新
        public void Data_RefreshData(object sender, StoreRefreshDataEventArgs e)
        {
            string item = this.ItemLeibie.SelectedItem.Value.ToString();
            data(item);
        }


        //获取行
        private Dictionary<string, string>[] GetSelectRow(AjaxEventArgs e)
        {
            string row = e.ExtraParams["Values"].ToString();
            Dictionary<string, string>[] selectRow = JSON.Deserialize<Dictionary<string, string>[]>(row);
            if (selectRow.Length <= 0) { return null; } else { return selectRow; }
        }

        //展示窗口
        private void showDetailWin(LoadConfig loadcfg)
        {
            DetailWin.ClearContent();
            DetailWin.Show();
            DetailWin.LoadContent(loadcfg);
        }

    }
}