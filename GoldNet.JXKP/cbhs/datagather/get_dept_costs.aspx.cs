using System;
using System.Data;
using Goldnet.Dal;
using GoldNet.Model;
using Goldnet.Ext.Web;
using GoldNet.Comm.ExportData;
using System.Collections.Generic;
using System.IO;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;

namespace GoldNet.JXKP.cbhs.datagather
{
    public partial class get_dept_costs : PageBase
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
                    //               Response.End();
                }
                HttpProxy pro = new HttpProxy();
                pro.Method = HttpMethod.POST;
                pro.Url = "../../../WebService/AccountDepts.ashx";
                this.Store2.Proxy.Add(pro);

                for (int i = 0; i < 10; i++)
                {
                    int year = System.DateTime.Now.Year - i;
                    this.years.Items.Add(new Goldnet.Ext.Web.ListItem(year.ToString(), year.ToString()));
                }
                this.years.SelectedItem.Value = System.DateTime.Now.ToString("yyyy");
                this.months.SelectedItem.Value = System.DateTime.Now.ToString("MM");
                // SetDict();

                //Bindlist(this.DEPT.SelectedItem.Value, System.DateTime.Now.ToString("yyyyMM"));


                this.Button1.Visible = this.IsEdit();
                this.Buttonfh.Visible = this.IsPass();
                if (this.IsEdit() || this.IsPass())
                {
                    this.Button_no.Visible = true;
                }
                else
                    this.Button_no.Visible = false;
                if (System.Configuration.ConfigurationSettings.AppSettings["yysqlbutton"] == "1")
                {
                    this.Button2.Hidden = false;
                }
            }
        }

        /// <summary>
        /// 查询绑定成本正式表数据
        /// </summary>
        /// <param name="dept_code"></param>
        /// <param name="date_time"></param>
        protected void Bindlist(string dept_code, string date_time)
        {
            Cost_detail dal = new Cost_detail();
            User user = (User)Session["CURRENTSTAFF"];
            DataTable dt = dal.GetDeptCosts(dept_code, date_time, user.UserId).Tables[0];
            this.Store1.DataSource = dt;
            this.Store1.DataBind();
            Session.Remove("DeptGeCost");
            Session["DeptGeCost"] = dt;
            Bindsubcost(date_time);
        }

        /// <summary>
        /// 
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
        /// 查询
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Button_look_click(object sender, EventArgs e)
        {
            string dept_code = this.DEPT.SelectedItem.Value;
            string date_time = this.years.SelectedItem.Value + this.months.SelectedItem.Value;
            Bindlist(dept_code, date_time);
        }

        /// <summary>
        /// 删除单项成本记录
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Button_del_click(object sender, AjaxEventArgs e)
        {
            Cbhs_dict dal_dict = new Cbhs_dict();
            Dictionary<string, string>[] selectRow = GetSelectRow(e);
            string dept_code = this.DEPT.SelectedItem.Value;
            string date_time = this.years.SelectedItem.Value + this.months.SelectedItem.Value;
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

            else
            {
                try
                {
                    Cost_detail dal = new Cost_detail();
                    string mess = "";
                    for (int i = 0; i < selectRow.Length; i++)
                    {
                        string item_code = selectRow[i]["ITEM_CODE"].ToString();
                        if (item_code != "")
                        {
                            if (dal.GetIsSubmit(date_time, item_code))
                            {
                                mess = "___有成本已经提交不能删除！";
                            }
                            else
                            {
                                dal.DelDeptCosts(dept_code, date_time, item_code);
                            }
                        }

                    }
                    Goldnet.Ext.Web.Ext.Msg.Show(new Goldnet.Ext.Web.MessageBox.Config
                    {
                        Title = "信息提示",
                        Message = "删除成功" + mess,
                        Buttons = MessageBox.Button.OK,
                        Icon = (MessageBox.Icon)Enum.Parse(typeof(MessageBox.Icon), "INFO")
                    });
                    Bindlist(dept_code, date_time);
                }
                catch (Exception ex)
                {
                    this.ShowDataError(ex.Message.ToString(), Request.Path, "Button_del_click");
                }
            }
        }

        /// <summary>
        /// 成本提取
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void costs_click(object sender, AjaxEventArgs e)
        {
            Cost_detail dal = new Cost_detail();
            string date = Convert.ToDateTime(this.years.SelectedItem.Value.ToString() + "-" + this.months.SelectedItem.Value.ToString() + "-01").ToString("yyyy-MM-dd");
            try
            {
                dal.getcacost(date);
                Button_look_click(null, null);
                this.ShowMessage("系统提示", "数据提取成功！");
            }
            catch (Exception ex)
            {
                this.ShowDataError(ex, Request.Path, "costs_click");
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
        /// SQL提取
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void SQL_get_due_click(object sender, EventArgs e)
        {
            string date_time = this.years.SelectedItem.Value.ToString() + this.months.SelectedItem.Value.ToString();
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
            string dept_code = this.DEPT.SelectedItem.Value;
            User user = (User)Session["CURRENTSTAFF"];
            string opuser = user.UserId;
            // string opno = this.Opno.SelectedItem.Value;
            string rtnmsg = "";
            try
            {
                Cost_detail dal = new Cost_detail();
                dal.getsqlcacost(date_time, user.UserName);
                // rtnmsg = dal.Exec_Sp_Extract_Cost(date_time, user.UserName, user.UserId, "1");
                Goldnet.Ext.Web.ScriptManager scManager = Goldnet.Ext.Web.ScriptManager.GetInstance(this.Page);
                scManager.AddScript("Store1.reload();");
                Goldnet.Ext.Web.Ext.Msg.Show(new Goldnet.Ext.Web.MessageBox.Config
                {
                    Title = "信息提示",
                    Message = "提取成功",
                    Buttons = MessageBox.Button.OK,
                    Icon = (MessageBox.Icon)Enum.Parse(typeof(MessageBox.Icon), "INFO")
                });
            }
            catch (Exception ex)
            {
                this.ShowDataError(ex.ToString(), Request.Path, "Button_get_due_click");
            }
        }

        /// <summary>
        /// 正式提取成本操作
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Button_get_due_click(object sender, EventArgs e)
        {
            string date_time = this.years.SelectedItem.Value.ToString() + this.months.SelectedItem.Value.ToString();
            Cbhs_dict dal_dict = new Cbhs_dict();
            AccountingData dal_acc = new AccountingData();
            if (dal_dict.IsBonusSave(date_time))
            {
                //验证奖金数据是否生成
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
            string dept_code = this.DEPT.SelectedItem.Value;
            User user = (User)Session["CURRENTSTAFF"];
            string opuser = user.UserId;
            // string opno = this.Opno.SelectedItem.Value;
            string rtnmsg = "";
            try
            {
                Cost_detail dal = new Cost_detail();
                //执行成本预提操作
                rtnmsg = dal.Exec_Sp_Extract_Cost_Pre(date_time, opuser, user.UserId);
                //执行正式提取操作（空总将成本提取和成本分解分开处理）
                rtnmsg = dal.Exec_Sp_Extract_Cost(date_time, user.UserName, user.UserId, "1");
                Goldnet.Ext.Web.ScriptManager scManager = Goldnet.Ext.Web.ScriptManager.GetInstance(this.Page);
                scManager.AddScript("Store1.reload();");
                if (rtnmsg == "")
                {
                    rtnmsg = "成本提取成功";
                }

                Goldnet.Ext.Web.Ext.Msg.Show(new Goldnet.Ext.Web.MessageBox.Config
                {
                    Title = "信息提示",
                    Message = rtnmsg,
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
        /// 成本分解操作(暂不使用)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void costs_click(object sender, EventArgs e)
        {
            string date_time = this.years.SelectedItem.Value.ToString() + this.months.SelectedItem.Value.ToString();
            Cbhs_dict dal_dict = new Cbhs_dict();
            AccountingData dal_acc = new AccountingData();
            if (dal_dict.IsBonusSave(date_time))
            {
                //验证奖金数据是否生成
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
            string dept_code = this.DEPT.SelectedItem.Value;
            User user = (User)Session["CURRENTSTAFF"];
            string opuser = user.UserId;
            // string opno = this.Opno.SelectedItem.Value;
            string rtnmsg = "";
            try
            {
                Cost_detail dal = new Cost_detail();
                //执行成本预提操作
                //rtnmsg = dal.Exec_Sp_Extract_Cost_Pre(date_time, opuser, user.UserId);
                //执行正式提取操作（空总将成本提取和成本分解分开处理）
                rtnmsg = dal.Exec_Sp_Extract_Cost(date_time, user.UserName, user.UserId, "1");
                Goldnet.Ext.Web.ScriptManager scManager = Goldnet.Ext.Web.ScriptManager.GetInstance(this.Page);
                scManager.AddScript("Store1.reload();");
                if (rtnmsg == "")
                {
                    rtnmsg = "成本分解成功";
                }

                Goldnet.Ext.Web.Ext.Msg.Show(new Goldnet.Ext.Web.MessageBox.Config
                {
                    Title = "信息提示",
                    Message = rtnmsg,
                    Buttons = MessageBox.Button.OK,
                    Icon = (MessageBox.Icon)Enum.Parse(typeof(MessageBox.Icon), "INFO")
                });
            }
            catch
            {
                this.ShowDataError(rtnmsg, Request.Path, "Button_get_due_click");
            }
        }

        //时间改变再去验证奖金是否生成
        protected void Date_SelectOnChange(object sender, AjaxEventArgs e)
        {
            Bindlist(this.DEPT.SelectedItem.Value, this.years.SelectedItem.Value + this.months.SelectedItem.Value);
            // ReSetDict();
        }

        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void SubmitData(object sender, StoreSubmitDataEventArgs e)
        {
            string date_time = this.years.SelectedItem.Value.ToString() + this.months.SelectedItem.Value.ToString();
            Cbhs_dict dal_dict = new Cbhs_dict();
            AccountingData dal_acc = new AccountingData();
            if (dal_dict.IsBonusSave(date_time))
            {
                Goldnet.Ext.Web.Ext.Msg.Show(new Goldnet.Ext.Web.MessageBox.Config
                {
                    Title = "信息提示",
                    Message = "该月奖金已经生成、不可以改变收入数据!",
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
                        Message = "该月奖金核算已经完成、不可以改变收入数据!",
                        Buttons = MessageBox.Button.OK,
                        Icon = (MessageBox.Icon)Enum.Parse(typeof(MessageBox.Icon), "INFO")
                    });
                    return;
                }
            }

            List<CostDetail> costdetail = e.Object<CostDetail>();
            Cost_detail dal = new Cost_detail();
            string dept_code = this.DEPT.SelectedItem.Value;
            try
            {
                dal.UpdateDepteCosts(date_time, costdetail);
                Goldnet.Ext.Web.ScriptManager scManager = Goldnet.Ext.Web.ScriptManager.GetInstance(this.Page);
                scManager.AddScript("Store1.reload();");
                // Bindlist(dept_code, date_time);
                Goldnet.Ext.Web.Ext.Msg.Show(new Goldnet.Ext.Web.MessageBox.Config
                {
                    Title = "信息提示",
                    Message = "保存成功",
                    Buttons = MessageBox.Button.OK,
                    Icon = (MessageBox.Icon)Enum.Parse(typeof(MessageBox.Icon), "INFO")
                });
            }
            catch (Exception ex)
            {
                this.ShowDataError(ex.Message.ToString(), Request.Path, "Button_create_click");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void Data_RefreshData(object sender, StoreRefreshDataEventArgs e)
        {
            User user = (User)Session["CURRENTSTAFF"];
            string dept_code = this.DEPT.SelectedItem.Value;
            string date_time = this.years.SelectedItem.Value + this.months.SelectedItem.Value;
            Cost_detail dal = new Cost_detail();
            DataTable dt = dal.GetDeptCosts(dept_code, date_time, user.UserId).Tables[0];
            this.Store1.DataSource = dt;
            this.Store1.DataBind();
        }

        /// <summary>
        /// 提交成本
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Button_ok_click(object sender, AjaxEventArgs e)
        {
            Cost_detail dal = new Cost_detail();
            User user = (User)Session["CURRENTSTAFF"];
            Dictionary<string, string>[] selectRow = GetSelectRow(e);
            string dept_code = this.DEPT.SelectedItem.Value;
            string date_time = this.years.SelectedItem.Value + this.months.SelectedItem.Value;
            if (selectRow == null || selectRow.Length < 1)
            {
                this.SelectRecord();
            }
            else
            {
                try
                {

                    for (int i = 0; i < selectRow.Length; i++)
                    {
                        string item_code = selectRow[i]["ITEM_CODE"];
                        if (!dal.GetIsSubmit(date_time, item_code))
                        {
                            dal.Submitcost(date_time, item_code, user.UserName);
                        }
                    }
                    this.SaveSucceed();
                    Bindlist(dept_code, date_time);
                }
                catch (Exception ex)
                {
                    this.ShowDataError(ex, Request.Path, "Button_ok_click");
                }
            }
        }

        /// <summary>
        /// 审核
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Button_sh_click(object sender, AjaxEventArgs e)
        {
            User user = (User)Session["CURRENTSTAFF"];
            Dictionary<string, string>[] selectRow = GetSelectRow(e);
            string dept_code = this.DEPT.SelectedItem.Value;
            string date_time = this.years.SelectedItem.Value + this.months.SelectedItem.Value;
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
                        string item_code = selectRow[i]["ITEM_CODE"];

                        dal.Checkcost(date_time, item_code, user.UserName);
                    }
                    this.SaveSucceed();
                    Bindlist(dept_code, date_time);
                }
                catch (Exception ex)
                {
                    this.ShowDataError(ex, Request.Path, "Button_sh_click");
                }
            }
        }

        /// <summary>
        /// 复合
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Button_fh_click(object sender, AjaxEventArgs e)
        {
            User user = (User)Session["CURRENTSTAFF"];
            Dictionary<string, string>[] selectRow = GetSelectRow(e);
            string dept_code = this.DEPT.SelectedItem.Value;
            string date_time = this.years.SelectedItem.Value + this.months.SelectedItem.Value;
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
                        string item_code = selectRow[i]["ITEM_CODE"];

                        dal.Compcost(date_time, item_code, user.UserName);
                    }
                    this.SaveSucceed();
                    Bindlist(dept_code, date_time);
                }
                catch (Exception ex)
                {
                    this.ShowDataError(ex, Request.Path, "Button_fh_click");
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Button_no_click(object sender, AjaxEventArgs e)
        {
            User user = (User)Session["CURRENTSTAFF"];
            Dictionary<string, string>[] selectRow = GetSelectRow(e);
            string dept_code = this.DEPT.SelectedItem.Value;
            string date_time = this.years.SelectedItem.Value + this.months.SelectedItem.Value;
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
                        string item_code = selectRow[i]["ITEM_CODE"];

                        dal.Canclecost(date_time, item_code, user.UserName);
                    }
                    this.SaveSucceed();
                    Bindlist(dept_code, date_time);
                }
                catch (Exception ex)
                {
                    this.ShowDataError(ex, Request.Path, "Button_no_click");
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void OutExcel(object sender, EventArgs e)
        {
            if (Session["DeptGeCost"] != null)
            {

                string dept_name = this.DEPT.SelectedItem.Text;
                string date_time = this.years.SelectedItem.Value + this.months.SelectedItem.Value;
                ExportData ex = new ExportData();
                DataTable dt = (DataTable)Session["DeptGeCost"];


                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    if (dt.Columns[i].ColumnName == "ITEM_CODE")
                    {
                        dt.Columns[i].ColumnName = "项目代码";
                    }
                    if (dt.Columns[i].ColumnName == "ITEM_NAME")
                    {
                        dt.Columns[i].ColumnName = "成本项目";
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

                }
                //ex.ExportToLocal(dt, this.Page, "xls", "提取成本报表");

                // 创建工作簿
                IWorkbook workbook = new HSSFWorkbook();
                ISheet sheet = workbook.CreateSheet("提取成本报表");

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
                    Response.AddHeader("content-disposition", "attachment; filename=" + date_time + dept_name + "提取成本报表.xls");
                    Response.ContentType = "application/vnd.ms-excel";
                    Response.BinaryWrite(exportData.ToArray());
                    Response.End();
                }
            }
        }

        /// <summary>
        /// 双击设置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void DbRowClick(object sender, AjaxEventArgs e)
        {

            Dictionary<string, string>[] selectRow = GetSelectRow(e);
            string dept_code = this.DEPT.SelectedItem.Value;
            string date_time = this.years.SelectedItem.Value + this.months.SelectedItem.Value;
            if (selectRow == null || selectRow.Length != 1)
            {
                Goldnet.Ext.Web.Ext.Msg.Show(new Goldnet.Ext.Web.MessageBox.Config
                {
                    Title = "信息提示",
                    Message = "只能选择一条记录",
                    Buttons = MessageBox.Button.OK,
                    Icon = (MessageBox.Icon)Enum.Parse(typeof(MessageBox.Icon), "INFO")
                });
            }
            else
            {
                string itemcode = selectRow[0]["ITEM_CODE"];
                string deptcode = this.DEPT.SelectedItem.Value.ToString() != "" ? "'" + this.DEPT.SelectedItem.Value.ToString() + "'" : this.DeptFilter("");

                LoadConfig loadcfg = getLoadConfig("DeptCostDetail.aspx");
                loadcfg.Params.Add(new Goldnet.Ext.Web.Parameter("itemcode", itemcode));
                loadcfg.Params.Add(new Goldnet.Ext.Web.Parameter("stardate", this.years.SelectedItem.Value + this.months.SelectedItem.Value + "01"));
                loadcfg.Params.Add(new Goldnet.Ext.Web.Parameter("enddate", this.years.SelectedItem.Value + this.months.SelectedItem.Value + "01"));
                loadcfg.Params.Add(new Goldnet.Ext.Web.Parameter("deptcode", deptcode));
                loadcfg.Params.Add(new Goldnet.Ext.Web.Parameter("balances", "0"));
                loadcfg.Params.Add(new Goldnet.Ext.Web.Parameter("gettype", "1"));
                showCenterSet(this.Cost_Detail, loadcfg);
            }
        }

    }
}
