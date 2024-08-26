using System;
using System.Data;
using Goldnet.Ext.Web;
using GoldNet.JXKP.Templet.BLL;
using GoldNet.JXKP.PowerManager;
using System.IO;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using System.Linq;
using System.Web;

namespace GoldNet.JXKP.zlgl.Templet.Page
{
    public partial class PageList : PageBase
    {
        private string files = "";
        private int _templetID;
        private TempletBO _templet;
        private DataTable _dataTable;
        private System.Data.DataView _dataView;
        private GoldNet.JXKP.Templet.BLL.ListView _currentView;
        private const string FILTER_STRING = "FILTER_STRING";
        private const string SORT_FIELD = "SORT_FIELD";
        private const string SORT_DESC = "SORT_DESC";
        private const string CURRENT_PAGE = "CURRENT_PAGE";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Ext.IsAjaxRequest)
            {
                this.Buttonadd.Visible = this.IsEdit();
                this.Buttondel.Visible = this.IsEdit();
                this.Buttonedit.Visible = false;
                if (this.IsEdit() || this.IsPass())
                {
                    this.Buttonedit.Visible = true;
                }
                string str = this.DeptFilter("deptcode");
                data();
            }
            //if (Session["CURRENTSTAFF"] != null)
            //{
            //    this.Session.Remove("CURRENTSTAFF");
            //}
        }

        /// <summary>
        /// 
        /// </summary>
        private void data()
        {
            files="";
            // 得到当前用户
            IStaffTargetInfo currentStaff = Staff.GetStaff();
            string str11 = currentStaff.Dept;
            Goldnet.Dal.TempList dal = new Goldnet.Dal.TempList();
            string[] pageids = Request.QueryString["pageid"].ToString().Split('_');
            string menuid = pageids[0].ToString();
            _templetID = int.Parse(menuid.TrimStart('0'));
            _templet = new TempletBO(_templetID);
            _dataTable = _templet.GetAllRecord();
            if (Session["CURRENT_VIEW"] != null)
            {
                _currentView = (GoldNet.JXKP.Templet.BLL.ListView)Session["CURRENT_VIEW"];
                if (_currentView.TempletID != this._templetID)
                {
                    Session["CURRENT_VIEW"] = null;
                    _currentView = _templet.DefaultView;
                }
            }
            else
            {
                // 如果Session中没有视图信息，使用默认视图。
                _currentView = _templet.DefaultView;
                Session["CURRENT_VIEW"] = _currentView;
            }
            //if (this.ViewState[FILTER_STRING] == null)
            this.ViewState[FILTER_STRING] = _currentView.RowFilter;
            //if (this.ViewState[SORT_FIELD] == null)
            this.ViewState[SORT_FIELD] = _currentView.SortStr;
            string filterString, sortString;
            filterString = sortString = "";

            if (this.ViewState[FILTER_STRING] != null)
            {
                filterString = this.ViewState[FILTER_STRING].ToString();
            }
            string check = this.checks.Checked == true ? "1" : "0";
            string flags = this.flags.Checked == true ? "1" : "0";

            if (check != "0" | flags != "0")
            {
                string id = dal.GetCheckPass(_templetID.ToString(), check, flags);
                if (id != "" && filterString != "")
                {
                    filterString += string.Format(" and id in ({0})", id);
                }
                else if (id != "" && filterString == "")
                {
                    filterString += string.Format("  id in ({0})", id);
                }
            }
            if (this.ViewState[SORT_FIELD] != null)
                sortString = this.ViewState[SORT_FIELD].ToString();
            if (sortString == "")
                sortString = " sort_no";
            else
                sortString += " ,sort_no";

            DataRow[] datarows = _dataTable.Select(filterString, sortString, DataViewRowState.CurrentRows);

            DataTable vtable = datarows.Length == 0 ? _dataTable.Clone() : datarows.CopyToDataTable();

            DataRow rw = vtable.NewRow();
            string guidehead = "";
            string rowfilter = "";
            string deptfilter = "";
            // 添加显示绑定列
            foreach (GoldNet.JXKP.Templet.BLL.Field field in _currentView.DisplayFields)
            {
                //bool f = false;
                //for (int j = 0; j < vtable.Columns.Count; j++)
                //{
                //    if (field.ListDisplayDataName == vtable.Columns[j].ColumnName)
                //        f = true;
                //}
                //if (f == false)
                //{
                //    vtable.Columns.Remove();
                //}
                RecordField record = new RecordField();
                if (field.FieldTypeID == TempletBO.DATEFIELDTYPEID)
                {
                    record = new RecordField(field.ListDisplayDataName.ToUpper(), RecordFieldType.Date);
                }
                else record = new RecordField(field.ListDisplayDataName.ToUpper(), RecordFieldType.String);
                if (field.FieldTypeID == TempletBO.DEPTFIELDTYPEID)
                {
                    deptfilter = this.DeptFilter("DEPT_ID_" + field.ID);
                    rowfilter = deptfilter == "" ? "" : deptfilter + " or DEPT_ID_" + field.ID + " is null";
                }
                this.Store1.AddField(record);
                //
                if (_templet.exitdisplayfild(field))
                {
                    if(files=="")
                        files = field.ListDisplayDataName;
                    else
                    files += ","+field.ListDisplayDataName;
                    Column cl = new Column();
                    cl.Header = field.FieldName;
                    cl.Sortable = false;
                    cl.MenuDisabled = true;
                    cl.ColumnID = field.ListDisplayDataName.ToUpper();
                    cl.DataIndex = field.ListDisplayDataName.ToUpper();
                    if (field.FieldTypeID == TempletBO.DATEFIELDTYPEID)
                    {
                        cl.Renderer.Fn = "Ext.util.Format.dateRenderer('Y-m-d')";
                    }
                    //cl.Editor.Add();

                    //NumberField nf = new NumberField();
                    //nf.ID = System.DateTime.Now.ToString();
                    //cl.Editor.Add(nf);
                    this.GridPanel.AddColumn(cl);
                    if (field.FieldTypeID == TempletBO.GUIDEFIELDTYPEID)
                    {
                        guidehead = field.ListDisplayDataName.ToUpper();
                    }
                    CollectBO collect = _currentView.CollectFields.Contains(field);
                    if (collect != null)
                    {
                        // 如果汇总数据为空,不写汇总数据
                        string filterview = this.ViewState[FILTER_STRING].ToString() == "" ? deptfilter : " and " + deptfilter;
                        string deptaddfilter = deptfilter == "" ? this.ViewState[FILTER_STRING].ToString() : this.ViewState[FILTER_STRING].ToString() + filterview;
                        if (_dataTable.Compute(collect.CollectComputerString, deptaddfilter) != DBNull.Value)
                        {
                            rw[field.ListDisplayDataName.ToUpper()] = Decimal.Round(Convert.ToDecimal(_dataTable.Compute(collect.CollectComputerString, deptaddfilter)), 2).ToString();
                            if (guidehead != "")
                            {
                                rw[guidehead] = "合计：";
                            }
                        }
                    }
                }
            }
            if (!(_currentView.PageCount == 0))
            {
                PagingToolbar ptb = new PagingToolbar();
                ptb.ID = "PagingToolBar1";
                ptb.PageSize = _currentView.PageCount;
                ptb.StoreID = "Store1";
                ptb.AutoWidth = true;
                ptb.AutoDataBind = true;

                GridPanel.BottomBar.Add(ptb);

            }
            rw["ID"] = -1;
            vtable.Rows.Add(rw);
            _dataView = vtable.DefaultView;
            _dataView.RowFilter = rowfilter;
            this.Store1.DataSource = _dataView;
            this.Store1.DataBind();
            Session["zlglview"] = _dataView;

        }
        protected void OutExcel(object sender, EventArgs e)
        {
            
            if (Session["zlglview"] != null)
            {
                System.Data.DataView dv = (System.Data.DataView)(Session["zlglview"]);

                DataTable dt = dv.ToTable();

                //从第10列开始匹配列名，防止重复
                for (int i = 10; i < dt.Columns.Count; i++)
                {
                    string columnName = dt.Columns[i].ColumnName;
                    if (columnName.Contains("*"))
                    {
                        dt.Columns[i].ColumnName = "未知项目";
                    }
                    else if (columnName.Contains("DATE_"))
                    {
                        dt.Columns[i].ColumnName = "日期";
                    }
                    else if (columnName.Contains("DEPT_") && !columnName.Contains("DEPT_ID_"))
                    {
                        dt.Columns[i].ColumnName = "科室";
                    }
                    else if (columnName.Contains("DEPT_ID_"))
                    {
                        dt.Columns[i].ColumnName = "科室编码";
                    }
                    else if (columnName.Contains("NUMBER_"))
                    {
                        dt.Columns[i].ColumnName = "分值";
                    }
                    else if (columnName.Contains("GUIDE_") && !columnName.Contains("GUIDE_STANDARD_"))
                    {
                        dt.Columns[i].ColumnName = "指标";
                    }
                    else if (columnName.Contains("GUIDE_STANDARD_"))
                    {
                        dt.Columns[i].ColumnName = "指标标准";
                    }
                    else if (columnName.Contains("STAFF_") && !columnName.Contains("STAFF_ID_"))
                    {
                        dt.Columns[i].ColumnName = "责任人";
                    }
                    else if (columnName.Contains("STAFF_ID_"))
                    {
                        dt.Columns[i].ColumnName = "责任人编码";
                    }
                    else if (columnName.Contains("CHAR_"))
                    {
                        dt.Columns[i].ColumnName = "备注";
                    }
                }


               // 创建工作簿
               IWorkbook workbook = new HSSFWorkbook();
               ISheet sheet = workbook.CreateSheet("科室质量得分");

               // 指定要模糊匹配的列名关键词
               string[] keywordsToExport = { "日期", "科室", "科室编码", "分值", "指标", "指标标准", "责任人", "责任人编码", "备注" };

               // 使用模糊匹配找到需要导出的列名
               var columnsToExport = dt.Columns.Cast<DataColumn>()
                                                .Where(col => keywordsToExport.Any(keyword => col.ColumnName.Contains(keyword)))
                                                .Select(col => col.ColumnName)
                                                .ToArray();

               // 创建表头
               IRow headerRow = sheet.CreateRow(0);
               for (int i = 0; i < columnsToExport.Length; i++)
               {
                   ICell cell = headerRow.CreateCell(i);
                   cell.SetCellValue(columnsToExport[i]);
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
                   for (int j = 0; j < columnsToExport.Length; j++)
                   {
                       ICell cell = row.CreateCell(j);
                       string columnName = columnsToExport[j];
                       string cellValue = dt.Rows[i][columnName].ToString();
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
                   string fileName = HttpUtility.UrlEncode("科室质量得分.xls", System.Text.Encoding.UTF8);
                   Response.AddHeader("content-disposition", "attachment; filename*=UTF-8''" + fileName + " ");
                   Response.ContentType = "application/vnd.ms-excel";
                   Response.BinaryWrite(exportData.ToArray());
                   Response.End();
               }
            }
           
        }
        //双击查看详细信息
        protected void DbRowClick(object sender, AjaxEventArgs e)
        {
            string[] pageids = Request.QueryString["pageid"].ToString().Split('_');
            string menuid = pageids[0].ToString();
            _templetID = int.Parse(menuid.TrimStart('0'));

            RowSelectionModel sm = this.GridPanel.SelectionModel.Primary as RowSelectionModel;
            if (sm.SelectedRows.Count < 1 || sm.SelectedRow.RecordID == "-1")
            {
                this.SelectRecord();
            }
            else
            {
                string id = sm.SelectedRow.RecordID;

                LoadConfig loadcfg = getLoadConfig("View.aspx");
                loadcfg.Params.Add(new Goldnet.Ext.Web.Parameter("templetid", _templetID.ToString()));
                loadcfg.Params.Add(new Goldnet.Ext.Web.Parameter("recid", id));
                showCenterSet(this.ListDetail, loadcfg);
            }
        }
        //添加
        protected void Buttonadd_Click(object sender, EventArgs e)
        {
            string[] pageids = Request.QueryString["pageid"].ToString().Split('_');
            string menuid = pageids[0].ToString();
            _templetID = int.Parse(menuid.TrimStart('0'));

            LoadConfig loadcfg = getLoadConfig("PageInput.aspx");
            loadcfg.Params.Add(new Goldnet.Ext.Web.Parameter("templetid", _templetID.ToString()));
            loadcfg.Params.Add(new Goldnet.Ext.Web.Parameter("pass", this.IsPass().ToString()));
            showCenterSet(this.ListDetail, loadcfg);
        }
        //查询设置
        protected void SerchSet(object sender, EventArgs e)
        {
            string[] pageids = Request.QueryString["pageid"].ToString().Split('_');
            string menuid = pageids[0].ToString();
            _templetID = int.Parse(menuid.TrimStart('0'));

            LoadConfig loadcfg = getLoadConfig("SearchViewSet.aspx");
            loadcfg.Params.Add(new Goldnet.Ext.Web.Parameter("templetid", _templetID.ToString()));
            showCenterSet(this.searchset, loadcfg);
        }
        //修改
        protected void Buttonedit_Click(object sender, EventArgs e)
        {
            string[] pageids = Request.QueryString["pageid"].ToString().Split('_');
            string menuid = pageids[0].ToString();
            _templetID = int.Parse(menuid.TrimStart('0'));
            string sd = this.IsPass().ToString();
            RowSelectionModel sm = this.GridPanel.SelectionModel.Primary as RowSelectionModel;
            if (sm.SelectedRows.Count < 1 || sm.SelectedRow.RecordID == "-1")
            {
                this.SelectRecord();
            }
            else
            {
                string id = sm.SelectedRow.RecordID;
                LoadConfig loadcfg = getLoadConfig("PageEdit.aspx");
                loadcfg.Params.Add(new Goldnet.Ext.Web.Parameter("templetid", _templetID.ToString()));
                loadcfg.Params.Add(new Goldnet.Ext.Web.Parameter("recid", id));
                loadcfg.Params.Add(new Goldnet.Ext.Web.Parameter("pass", this.IsPass().ToString()));
                showCenterSet(this.ListDetail, loadcfg);
            }
        }
        //删除
        protected void Buttondel_Click(object sender, EventArgs e)
        {
            string[] pageids = Request.QueryString["pageid"].ToString().Split('_');
            string menuid = pageids[0].ToString();
            _templetID = int.Parse(menuid.TrimStart('0'));
            _templet = new TempletBO(_templetID);
            RowSelectionModel sm = this.GridPanel.SelectionModel.Primary as RowSelectionModel;

            if (sm.SelectedRows.Count < 1 || sm.SelectedRow.RecordID == "-1")
            {
                this.SelectRecord();
            }
            else
            {
                try
                {
                    string id = sm.SelectedRow.RecordID;
                    _templet.checkrecorddel(int.Parse(id));
                    Ext.Msg.Confirm("系统提示", "您确定要删除选中记录吗？", new MessageBox.ButtonsConfig
                    {
                        Yes = new MessageBox.ButtonConfig
                        {
                            Handler = "Goldnet.del()",
                            Text = "确定"

                        },
                        No = new MessageBox.ButtonConfig
                        {
                            Text = "取消"
                        }
                    }).Show();
                }
                catch (Exception ex)
                {
                    ShowDataError(ex, Request.Url.LocalPath, "Buttondel_Click");
                }
            }
        }
        [AjaxMethod]
        public void del()
        {
            IStaffTargetInfo currentStaff = Staff.GetStaff();
            string[] pageids = Request.QueryString["pageid"].ToString().Split('_');
            string menuid = pageids[0].ToString();
            _templetID = int.Parse(menuid.TrimStart('0'));
            _templet = new TempletBO(_templetID);
            Goldnet.Dal.Guide_Manager dal = new Goldnet.Dal.Guide_Manager();
            RowSelectionModel sm = this.GridPanel.SelectionModel.Primary as RowSelectionModel;
            string id = sm.SelectedRow.RecordID;
            _templet.deleteRecord(currentStaff, int.Parse(id));
            this.GridPanel.Reconfigure();
            data();
        }
        protected void GetQueryPortalet(object sender, AjaxEventArgs e)
        {
            this.GridPanel.Reconfigure();
            data();
        }
        protected void Store_RefreshData(object sender, StoreRefreshDataEventArgs e)
        {
            this.GridPanel.Reconfigure();
            data();
        }

    }
}
