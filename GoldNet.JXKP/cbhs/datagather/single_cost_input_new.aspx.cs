using System;
using System.Data;
using System.Web.UI.WebControls;
using Goldnet.Ext.Web;
using Goldnet.Dal;
using GoldNet.Model;
using System.Collections.Generic;
using GoldNet.Comm.ExportData;
using System.IO;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;

namespace GoldNet.JXKP.cbhs.datagather
{
    public partial class single_cost_input_new : PageBase
    {
        /// <summary>
        /// 初始化处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Ext.IsAjaxRequest)
            {
                //检查是否已经登录，否则停止
                if (Session["CURRENTSTAFF"] == null)
                {
                    //Response.End();
                }

                //开始日期
                this.stardate.Value = Convert.ToDateTime(DateTime.Now.Year.ToString() + "-" + DateTime.Now.Month.ToString() + "-1").ToString("yyyy-MM-dd");

                //设置成本项目字典列表
                SetDict();
                string item_code = this.COST_ITEM.SelectedItem.Value.ToString();
                string date_time = System.DateTime.Now.ToString("yyyyMM");

                SetStoreProxy();

                //获取单项成本列表后绑定
                //Bindlist(item_code, Convert.ToDateTime(DateTime.Now.Year.ToString() + "-" + DateTime.Now.Month.ToString() + "-1").ToString("yyyy-MM-dd"));

                //按钮权限控制
                this.Button1.Visible = this.IsEdit();
                this.Buttonfh.Visible = this.IsPass();
                this.costs.Visible = this.IsPass();
                if (this.IsEdit() || this.IsPass())
                {
                    this.Button_no.Visible = true;
                }
                else
                    this.Button_no.Visible = false;
            }
        }

        /// <summary>
        /// 设置成本项目字典列表
        /// </summary>
        protected void SetDict()
        {
            Cbhs_dict dal = new Cbhs_dict();
            DataTable dt = new DataTable();
            //成本项目下拉框
            //用户所具有权限的成本项目
            dt = this.CostItemFilter();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                this.COST_ITEM.Items.Add(new Goldnet.Ext.Web.ListItem(dt.Rows[i]["ITEM_NAME"].ToString(), dt.Rows[i]["ITEM_CODE"].ToString()));
            }
            if (dt.Rows.Count > 0)
            {
                this.COST_ITEM.SelectedItem.Value = dt.Rows[0]["ITEM_CODE"].ToString();
            }
        }

        /// <summary>
        /// 设置科室列表
        /// </summary>
        private void SetStoreProxy()
        {
            //科室下拉列表初始化
            HttpProxy pro2 = new HttpProxy();
            pro2.Method = HttpMethod.POST;
            //pro2.Url = "../WebService/BonusDepts.ashx";
            pro2.Url = "../../../WebService/Depts.ashx";
            this.Store2.Proxy.Add(pro2);
        }

        /// <summary>
        /// 获取单项成本列表后绑定
        /// </summary>
        /// <param name="item_code"></param>
        /// <param name="date_time"></param>
        protected void Bindlist(string item_code, string date_time)
        {
            Cost_detail dal = new Cost_detail();
            //参数：成本录入"0",分解前、分解后
            DataTable dt = dal.GetSingleCost(item_code, date_time, "0", "", this.cbbType.SelectedItem.Value).Tables[0];

            DataRow rw = dt.NewRow();
            dt.Rows.Add(rw);
            this.Store1.DataSource = dt;
            this.Store1.DataBind();

            Session.Remove("single_cost_input_new");
            Session["single_cost_input_new"] = dt;

            if (dt.Rows.Count > 0)
            {
                this.CB_SUM.Text = "合计：" + dt.Compute("Sum(TOTAL_COSTS)", "").ToString();
                this.SJ_SUM.Text = "合计：" + dt.Compute("Sum(COSTS)", "").ToString();
                this.JM_SUM.Text = "合计：" + dt.Compute("Sum(COSTS_ARMYFREE)", "").ToString();
            }
            else
            {
                this.CB_SUM.Text = "";
                this.SJ_SUM.Text = "";
                this.JM_SUM.Text = "";
            }

            Bindsubcost(Convert.ToDateTime(date_time).ToString("yyyyMM"));
        }

        /// <summary>
        /// 获取成本提交状态信息后绑定
        /// </summary>
        /// <param name="months"></param>
        protected void Bindsubcost(string months)
        {
            Cost_detail dal = new Cost_detail();
            User user = (User)Session["CURRENTSTAFF"];
            DataTable dt = dal.GetSubmitcost(months, user.UserId);
            this.Store3.DataSource = dt;
            this.Store3.DataBind();
        }

        /// <summary>
        /// 查询处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Button_look_click(object sender, EventArgs e)
        {
            //成本项目选择
            string item_code = this.COST_ITEM.SelectedItem.Value.ToString();
            //日期
            string date_time = Convert.ToDateTime(this.stardate.Value.ToString()).ToString("yyyy-MM") + "-01";
            Bindlist(item_code, date_time);
        }

        /// <summary>
        /// 删除操作处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Button_del_click(object sender, AjaxEventArgs e)
        {
            Cost_detail dal = new Cost_detail();
            Cbhs_dict dal_dict = new Cbhs_dict();
            Dictionary<string, string>[] selectRow = GetSelectRow(e);
            string item_code = this.COST_ITEM.SelectedItem.Value.ToString();
            string date_time = Convert.ToDateTime(this.stardate.Value.ToString()).ToString("yyyyMM");
            RowSelectionModel sm = this.GridPanel1.SelectionModel.Primary as RowSelectionModel;
            if (selectRow == null || selectRow.Length < 1)
            {
                Goldnet.Ext.Web.Ext.Msg.Show(new Goldnet.Ext.Web.MessageBox.Config
                {
                    Title = "信息提示",
                    Message = "请至少选择一条记录",
                    Buttons = MessageBox.Button.OK,
                    Icon = (MessageBox.Icon)Enum.Parse(typeof(MessageBox.Icon), "INFO")
                });
            }
            else if (dal_dict.IsBonusSave(date_time))
            {
                this.ShowMessage("信息提示", "奖金已经生成，不能再修改数据!");
            }
            else if (dal.GetIsSubmit(date_time, item_code))
            {
                this.ShowMessage("系统提示", "该成本已经提交，不能删除！");
            }
            else
            {
                try
                {
                    for (int i = 0; i < selectRow.Length; i++)
                    {
                        string dept_code = selectRow[i]["DEPT_CODE"];
                        if (!dept_code.Equals(""))
                        {
                            if (cbbType.SelectedItem.Value == "0")
                            {
                                dal.DelSingleCosts(Convert.ToDateTime(this.stardate.Value.ToString()).ToString("yyyy-MM") + "-01", item_code, dept_code, "0", "");
                            }
                            else
                            {
                                dal.DelSingleCosts_hou(Convert.ToDateTime(this.stardate.Value.ToString()).ToString("yyyy-MM") + "-01", item_code, dept_code, "0", "");
                            }
                        }
                    }

                    this.ShowMessage("信息提示", "删除成功！");
                    Data_RefreshData(null, null);
                    //scManager.AddScript("Store1.reload();");
                }
                catch (Exception ex)
                {
                    this.ShowDataError(ex.Message.ToString(), Request.Path, "Button_del_click");
                }
            }
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

        /// <summary>
        /// 成本项目选择触发
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Item_SelectOnChange(object sender, EventArgs e)
        {
            string item_code = this.COST_ITEM.SelectedItem.Value.ToString();

            Goldnet.Ext.Web.ScriptManager scManager = Goldnet.Ext.Web.ScriptManager.GetInstance(this.Page);
            scManager.AddScript("Store1.reload();");
        }

        /// <summary>
        /// 保存数据处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Button_Save_click(object sender, AjaxEventArgs e)
        {
            Dictionary<string, string>[] selectRow = GetSelectRow(e);
            if (selectRow != null)
            {
                Cost_detail dal = new Cost_detail();
                string item_code = this.COST_ITEM.SelectedItem.Value.ToString();
                string date_time = Convert.ToDateTime(this.stardate.Value.ToString()).ToString("yyyyMM");
                Cbhs_dict dal_dict = new Cbhs_dict();
                AccountingData dal_acc = new AccountingData();
                if (dal_dict.IsBonusSave(date_time))
                {
                    Goldnet.Ext.Web.Ext.Msg.Show(new Goldnet.Ext.Web.MessageBox.Config
                    {
                        Title = "信息提示",
                        Message = "该月奖金已经生成、不可以改变成本数据!",
                        Buttons = MessageBox.Button.OK,
                        Icon = (MessageBox.Icon)Enum.Parse(typeof(MessageBox.Icon), "INFO")
                    });
                    return;
                }
                else
                {
                    //验证核算数据是否生成
                    if (dal_acc.IsAccount(date_time))
                    {
                        Goldnet.Ext.Web.Ext.Msg.Show(new Goldnet.Ext.Web.MessageBox.Config
                        {
                            Title = "信息提示",
                            Message = "该月奖金核算已经完成、不可以改变成本数据!",
                            Buttons = MessageBox.Button.OK,
                            Icon = (MessageBox.Icon)Enum.Parse(typeof(MessageBox.Icon), "INFO")
                        });
                        return;
                    }
                }
                if (dal.GetIsSubmit(date_time, item_code))
                {
                    this.ShowMessage("系统提示", "该成本已经提交，不能再保存了！");
                    return;
                }

                //List<CostDetail> costdetail = e.Object<CostDetail>();

                User user = (User)Session["CURRENTSTAFF"];
                string operators = user.UserName;
                try
                {
                    dal.SaveSingleCosts(selectRow, item_code, Convert.ToDateTime(this.stardate.Value.ToString()).ToString("yyyy-MM") + "-01", operators, "0", "");

                    this.ShowMessage("信息提示", Convert.ToDateTime(this.stardate.Value.ToString()).ToString("yyyy-MM")+"月成本数据保存成功！");
                    Data_RefreshData(null, null);

                }
                catch (Exception ex)
                {
                    this.ShowDataError(ex.Message.ToString(), Request.Path, "Button_create_click");
                }
            }
        }

        /// <summary>
        /// 数据刷新
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void Data_RefreshData(object sender, StoreRefreshDataEventArgs e)
        {
            //string deptcode = cbbdept.SelectedItem.Value;
            string date_time = Convert.ToDateTime(this.stardate.Value.ToString()).ToString("yyyy-MM") + "-01";
            string costitem = this.COST_ITEM.SelectedItem.Value.ToString();
            Cost_detail dal = new Cost_detail();

            DataTable dt = dal.GetSingleCost(costitem, date_time, "0", "", this.cbbType.SelectedItem.Value).Tables[0];
            DataRow rw = dt.NewRow();
            dt.Rows.Add(rw);
            this.Store1.DataSource = dt;
            this.Store1.DataBind();
            if (dt.Rows.Count > 0)
            {
                this.CB_SUM.Text = "合计：" + dt.Compute("Sum(TOTAL_COSTS)", "").ToString();
                this.SJ_SUM.Text = "合计：" + dt.Compute("Sum(COSTS)", "").ToString();
                this.JM_SUM.Text = "合计：" + dt.Compute("Sum(COSTS_ARMYFREE)", "").ToString();
            }
            else
            {
                this.CB_SUM.Text = "";
                this.SJ_SUM.Text = "";
                this.JM_SUM.Text = "";
            }
        }

        /// <summary>
        /// 提交成本操作处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Button_ok_click(object sender, AjaxEventArgs e)
        {
            User user = (User)Session["CURRENTSTAFF"];
            Dictionary<string, string>[] selectRow = GetSelectRow(e);
            string item_code = this.COST_ITEM.SelectedItem.Value.ToString();
            string date_time = Convert.ToDateTime(this.stardate.Value.ToString()).ToString("yyyyMM");
            if (selectRow == null || selectRow.Length < 1)
            {
                this.SelectRecord();
            }
            else
            {
                try
                {
                    Cost_detail dal = new Cost_detail();
                    for (int i = 0; i < selectRow.Length; i++)
                    {
                        string itemcode = selectRow[i]["ITEM_CODE"];
                        if (!dal.GetIsSubmit(date_time, item_code))
                        {
                            dal.Submitcost(date_time, itemcode, user.UserName);
                        }
                    }
                    this.SaveSucceed();
                    Bindlist(item_code, Convert.ToDateTime(this.stardate.Value.ToString()).ToString("yyyy-MM") + "-01");
                }
                catch (Exception ex)
                {
                    this.ShowDataError(ex, Request.Path, "Button_ok_click");
                }
            }
        }

        /// <summary>
        /// 成本审核操作处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Button_sh_click(object sender, AjaxEventArgs e)
        {
            User user = (User)Session["CURRENTSTAFF"];
            Dictionary<string, string>[] selectRow = GetSelectRow(e);
            string item_code = this.COST_ITEM.SelectedItem.Value.ToString();
            string date_time = Convert.ToDateTime(this.stardate.Value.ToString()).ToString("yyyyMM");
            if (selectRow == null || selectRow.Length < 1)
            {
                this.SelectRecord();
            }
            else
            {
                try
                {
                    Cost_detail dal = new Cost_detail();
                    for (int i = 0; i < selectRow.Length; i++)
                    {
                        string itemcode = selectRow[i]["ITEM_CODE"];

                        dal.Checkcost(date_time, itemcode, user.UserName);
                    }
                    this.SaveSucceed();
                    Bindlist(item_code, Convert.ToDateTime(this.stardate.Value.ToString()).ToString("yyyy-MM") + "-01");
                }
                catch (Exception ex)
                {
                    this.ShowDataError(ex, Request.Path, "Button_sh_click");
                }
            }
        }

        /// <summary>
        /// 成本复核操作处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Button_fh_click(object sender, AjaxEventArgs e)
        {
            User user = (User)Session["CURRENTSTAFF"];
            Dictionary<string, string>[] selectRow = GetSelectRow(e);
            string item_code = this.COST_ITEM.SelectedItem.Value.ToString();
            string date_time = Convert.ToDateTime(this.stardate.Value.ToString()).ToString("yyyyMM");
            if (selectRow == null || selectRow.Length < 1)
            {
                this.SelectRecord();
            }
            else
            {
                try
                {
                    Cost_detail dal = new Cost_detail();
                    for (int i = 0; i < selectRow.Length; i++)
                    {
                        string itemcode = selectRow[i]["ITEM_CODE"];

                        dal.Compcost(date_time, itemcode, user.UserName);
                    }
                    this.SaveSucceed();
                    Bindlist(item_code, Convert.ToDateTime(this.stardate.Value.ToString()).ToString("yyyy-MM") + "-01");
                }
                catch (Exception ex)
                {
                    this.ShowDataError(ex, Request.Path, "Button_fh_click");
                }
            }
        }

        /// <summary>
        /// 导出操作处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void OutExcel(object sender, EventArgs e)
        {
            if (Session["single_cost_input_new"] != null)
            {
                ExportData ex = new ExportData();
                DataTable dt = (DataTable)Session["single_cost_input_new"];
                ////string deptcode = cbbdept.SelectedItem.Value;
                //string item_code = this.COST_ITEM.SelectedItem.Value.ToString();
                //string date_time = Convert.ToDateTime(this.stardate.Value.ToString()).ToString("yyyy-MM") + "-01";
                //Cost_detail dal = new Cost_detail();
                ////成本录入0
                //DataTable dt = dal.GetSingleCost(item_code, date_time, "0", "", this.cbbType.SelectedItem.Value).Tables[0];
                //TableCell[] header = new TableCell[6];

                string item_name = this.COST_ITEM.SelectedItem.Text;
                string date_time = Convert.ToDateTime(this.stardate.Value.ToString()).ToString("yyyy-MM");
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    if (dt.Columns[i].ColumnName == "DEPT_CODE")
                    {
                        dt.Columns[i].ColumnName = "科室代码";
                    }
                    else if (dt.Columns[i].ColumnName == "DEPT_NAME")
                    {
                        dt.Columns[i].ColumnName = "科室名称";
                    }
                    else if (dt.Columns[i].ColumnName == "TOTAL_COSTS")
                    {
                        dt.Columns[i].ColumnName = "成本额";
                    }
                    else if (dt.Columns[i].ColumnName == "COSTS")
                    {
                        dt.Columns[i].ColumnName = "实际成本";
                    }
                    else if (dt.Columns[i].ColumnName == "COSTS_ARMYFREE")
                    {
                        dt.Columns[i].ColumnName = "减免成本";
                    }
                    else if (dt.Columns[i].ColumnName == "MEMO")
                    {
                        dt.Columns[i].ColumnName = "备注";
                    }
                    else if (dt.Columns[i].ColumnName == "GET_NAME")
                    {
                        dt.Columns[i].ColumnName = "类型";
                    }
                    //if (dt.Columns[i].ColumnName == "DEPT_CODE")
                    //    header[i].Text = "科室代码";
                    //if (dt.Columns[i].ColumnName == "DEPT_NAME")
                    //    header[i].Text = "科室名称";
                    //if (dt.Columns[i].ColumnName == "TOTAL_COSTS")
                    //    header[i].Text = "成本额";
                    //if (dt.Columns[i].ColumnName == "COSTS")
                    //    header[i].Text = "实际成本";
                    //if (dt.Columns[i].ColumnName == "COSTS_ARMYFREE")
                    //    header[i].Text = "减免成本";
                    //if (dt.Columns[i].ColumnName == "MEMO")
                    //    header[i].Text = "备注";
                }
                //Dictionary<int, int> mergeCellNums = null;
                //MHeaderTabletoExcel(dt, header, "单项成本录入", mergeCellNums, 0);
                //ex.ExportToLocal(dt, this.Page, "xls", "单项成本录入");

                // 创建工作簿
                IWorkbook workbook = new HSSFWorkbook();
                ISheet sheet = workbook.CreateSheet("单项成本录入");

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
                            if (j == 1)
                            {
                                cell.CellStyle = numberStyle;
                            }
                            else
                            {
                                cell.CellStyle = currencyStyle;
                            }

                            //cell.CellStyle = currencyStyle; // 金钱格式
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
                    Response.AddHeader("content-disposition", "attachment; filename=" + date_time + item_name + "单项成本录入.xls");
                    Response.ContentType = "application/vnd.ms-excel";
                    Response.BinaryWrite(exportData.ToArray());
                    Response.End();
                }
            }
        }

        /// <summary>
        /// 取消操作处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Button_no_click(object sender, AjaxEventArgs e)
        {
            User user = (User)Session["CURRENTSTAFF"];
            Dictionary<string, string>[] selectRow = GetSelectRow(e);
            string item_code = this.COST_ITEM.SelectedItem.Value.ToString();
            string date_time = Convert.ToDateTime(this.stardate.Value.ToString()).ToString("yyyyMM");
            if (selectRow == null || selectRow.Length < 1)
            {
                this.SelectRecord();
            }
            else
            {
                try
                {
                    Cost_detail dal = new Cost_detail();
                    for (int i = 0; i < selectRow.Length; i++)
                    {
                        string itemcode = selectRow[i]["ITEM_CODE"];

                        dal.Canclecost(date_time, itemcode, user.UserName);
                    }
                    this.SaveSucceed();
                    Bindlist(item_code, Convert.ToDateTime(this.stardate.Value.ToString()).ToString("yyyy-MM") + "-01");
                }
                catch (Exception ex)
                {
                    this.ShowDataError(ex, Request.Path, "Button_no_click");
                }
            }
        }

        /// <summary>
        /// 添加按钮触发事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Btn_Add_Click(object sender, AjaxEventArgs e)
        {
            LoadConfig loadcfg = getLoadConfig("single_cost_inputAdd.aspx");
            //loadcfg.Params.Add(new Goldnet.Ext.Web.Parameter("Mode", "NewAdd"));
            showDetailWin(loadcfg);
        }

        /// <summary>
        /// 显示详细窗口
        /// </summary>
        /// <param name="loadcfg"></param>
        private void showDetailWin(LoadConfig loadcfg)
        {
            DetailWin.ClearContent();
            DetailWin.Show();
            DetailWin.LoadContent(loadcfg);
        }

        /// <summary>
        /// 成本分解
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void costs_click(object sender, EventArgs e)
        {
            string date_time = Convert.ToDateTime(this.stardate.Value.ToString()).ToString("yyyyMM");
            Cbhs_dict dal_dict = new Cbhs_dict();
            AccountingData dal_acc = new AccountingData();
            if (dal_dict.IsBonusSave(date_time))
            {
                Goldnet.Ext.Web.Ext.Msg.Show(new Goldnet.Ext.Web.MessageBox.Config
                {
                    Title = "信息提示",
                    Message = "该月奖金已经生成、不可以改变成本数据!",
                    Buttons = MessageBox.Button.OK,
                    Icon = (MessageBox.Icon)Enum.Parse(typeof(MessageBox.Icon), "INFO")
                });
                return;
            }
            else
            {
                //验证核算数据是否生成
                if (dal_acc.IsAccount(date_time))
                {
                    Goldnet.Ext.Web.Ext.Msg.Show(new Goldnet.Ext.Web.MessageBox.Config
                    {
                        Title = "信息提示",
                        Message = "该月奖金核算已经完成、不可以改变成本数据!",
                        Buttons = MessageBox.Button.OK,
                        Icon = (MessageBox.Icon)Enum.Parse(typeof(MessageBox.Icon), "INFO")
                    });
                    return;
                }
            }
            //string dept_code = this.DEPT.SelectedItem.Value;
            User user = (User)Session["CURRENTSTAFF"];
            string opuser = user.UserId;
            // string opno = this.Opno.SelectedItem.Value;
            string rtnmsg = "";
            try
            {
                Cost_detail dal = new Cost_detail();

                rtnmsg = dal.Exec_Sp_Extract_Cost_commit(date_time, user.UserName, user.UserId, "1");

                Goldnet.Ext.Web.ScriptManager scManager = Goldnet.Ext.Web.ScriptManager.GetInstance(this.Page);
                scManager.AddScript("Store1.reload();");
                Goldnet.Ext.Web.Ext.Msg.Show(new Goldnet.Ext.Web.MessageBox.Config
                {
                    Title = "信息提示",
                    Message = "分解成功",
                    Buttons = MessageBox.Button.OK,
                    Icon = (MessageBox.Icon)Enum.Parse(typeof(MessageBox.Icon), "INFO")
                });
            }
            catch
            {
                this.ShowDataError(rtnmsg, Request.Path, "Button_get_due_click");
            }
        }

        /// <summary>
        /// 显示信息
        /// </summary>
        /// <param name="title"></param>
        /// <param name="message"></param>
        protected void ShowMessage(string title, string message)
        {
            Goldnet.Ext.Web.Ext.Msg.Show(new Goldnet.Ext.Web.MessageBox.Config
            {
                Title = title,
                Message = message + "   ",
                Buttons = MessageBox.Button.OK,
                Icon = (MessageBox.Icon)Enum.Parse(typeof(MessageBox.Icon), "INFO"),
                Modal = true
            });
        }

    }
}
