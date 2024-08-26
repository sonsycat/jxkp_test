using System;
using System.Data;
using System.Web.UI.WebControls;
using Goldnet.Ext.Web;
using Goldnet.Dal;
using GoldNet.Model;
using System.Collections.Generic;

namespace GoldNet.JXKP.cbhs.datagather
{
    public partial class single_cost_input : PageBase
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
                //for (int i = 0; i < 10; i++)
                //{
                //    int years = System.DateTime.Now.Year - i;
                //    this.years.Items.Add(new Goldnet.Ext.Web.ListItem(years.ToString(), years.ToString()));
                //}
                //this.years.SelectedItem.Value = System.DateTime.Now.ToString("yyyy");
                //this.months.SelectedItem.Value = System.DateTime.Now.ToString("MM");

                //设置成本项目字典列表
                SetDict();
                string item_code = this.COST_ITEM.SelectedItem.Value.ToString();
                string date_time = System.DateTime.Now.ToString("yyyyMM");

                SetStoreProxy();

                //获取单项成本列表后绑定
                Bindlist(item_code, Convert.ToDateTime(DateTime.Now.Year.ToString() + "-" + DateTime.Now.Month.ToString() + "-1").ToString("yyyy-MM-dd"));

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

        private void SetStoreProxy()
        {
            //查找科室信息
            HttpProxy pro = new HttpProxy();
            pro.Method = HttpMethod.POST;
            pro.Url = "../../WebService/Depts.ashx?deptfilter=" + this.DeptFilter("dept_code");
            this.SDept.Proxy.Add(pro);
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
            this.SDept.Reader.Add(jr);
        }

        /// <summary>
        /// 获取单项成本列表后绑定
        /// </summary>
        /// <param name="item_code"></param>
        /// <param name="date_time"></param>
        protected void Bindlist(string item_code, string date_time)
        {
            string deptcode = cbbdept.SelectedItem.Value;

            Cost_detail dal = new Cost_detail();
            DataTable dt = dal.GetSingleCost(item_code, date_time, "0", "", this.cbbType.SelectedItem.Value, deptcode).Tables[0];//成本录入0
            this.Store1.DataSource = dt;
            this.Store1.DataBind();
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
        /// 查询处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Button_look_click(object sender, EventArgs e)
        {
            string item_code = this.COST_ITEM.SelectedItem.Value.ToString();
            string date_time = Convert.ToDateTime(this.stardate.Value.ToString()).ToString("yyyy-MM") + "-01";
            Bindlist(item_code, date_time);
        }

        /// <summary>
        /// 
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
                        dal.DelSingleCosts(Convert.ToDateTime(this.stardate.Value.ToString()).ToString("yyyy-MM") + "-01", item_code, dept_code, "0", "");
                    }
                    Goldnet.Ext.Web.Ext.Msg.Show(new Goldnet.Ext.Web.MessageBox.Config
                    {
                        Title = "信息提示",
                        Message = "删除成功",
                        Buttons = MessageBox.Button.OK,
                        Icon = (MessageBox.Icon)Enum.Parse(typeof(MessageBox.Icon), "INFO")
                    });
                    Goldnet.Ext.Web.ScriptManager scManager = Goldnet.Ext.Web.ScriptManager.GetInstance(this.Page);
                    scManager.AddScript("Store1.reload();");
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
        /// 
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
        protected void SubmitData(object sender, StoreSubmitDataEventArgs e)
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

            List<CostDetail> costdetail = e.Object<CostDetail>();

            User user = (User)Session["CURRENTSTAFF"];
            string operators = user.UserName;
            try
            {
                dal.SaveSingleCosts(costdetail, item_code, Convert.ToDateTime(this.stardate.Value.ToString()).ToString("yyyy-MM") + "-01", operators, "0", "");
                Goldnet.Ext.Web.ScriptManager scManager = Goldnet.Ext.Web.ScriptManager.GetInstance(this.Page);
                scManager.AddScript("Store1.reload();");
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
        /// 数据刷新
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void Data_RefreshData(object sender, StoreRefreshDataEventArgs e)
        {
            string deptcode = cbbdept.SelectedItem.Value;
            string date_time = Convert.ToDateTime(this.stardate.Value.ToString()).ToString("yyyy-MM") + "-01";
            string costitem = this.COST_ITEM.SelectedItem.Value.ToString();
            Cost_detail dal = new Cost_detail();
            this.Store1.DataSource = dal.GetSingleCost(costitem, date_time, "0", "", this.cbbType.SelectedItem.Value, deptcode).Tables[0];
            this.Store1.DataBind();
        }

        /// <summary>
        /// 提交成本
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
        /// 审核
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
        /// 复合
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
        /// 导出
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void OutExcel(object sender, EventArgs e)
        {
            string deptcode = cbbdept.SelectedItem.Value;
            string item_code = this.COST_ITEM.SelectedItem.Value.ToString();
            string date_time = Convert.ToDateTime(this.stardate.Value.ToString()).ToString("yyyy-MM") + "-01";
            Cost_detail dal = new Cost_detail();
            DataTable dt = dal.GetSingleCost(item_code, date_time, "0", "", this.cbbType.SelectedItem.Value, deptcode).Tables[0];//成本录入0
            TableCell[] header = new TableCell[6];
            for (int i = 0; i < dt.Columns.Count; i++)
            {
                header[i] = new TableHeaderCell();
                if (dt.Columns[i].ColumnName == "DEPT_CODE")
                    header[i].Text = "科室代码";
                if (dt.Columns[i].ColumnName == "DEPT_NAME")
                    header[i].Text = "科室名称";
                if (dt.Columns[i].ColumnName == "TOTAL_COSTS")
                    header[i].Text = "成本额";
                if (dt.Columns[i].ColumnName == "COSTS")
                    header[i].Text = "实际成本";
                if (dt.Columns[i].ColumnName == "COSTS_ARMYFREE")
                    header[i].Text = "减免成本";
                if (dt.Columns[i].ColumnName == "MEMO")
                    header[i].Text = "备注";
            }
            Dictionary<int, int> mergeCellNums = null;
            MHeaderTabletoExcel(dt, header, "单项成本录入", mergeCellNums, 0);
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

        //添加按钮触发事件
        protected void Btn_Add_Click(object sender, AjaxEventArgs e)
        {
            LoadConfig loadcfg = getLoadConfig("single_cost_inputAdd.aspx");
            //loadcfg.Params.Add(new Goldnet.Ext.Web.Parameter("Mode", "NewAdd"));
            showDetailWin(loadcfg);
        }

        //显示详细窗口
        private void showDetailWin(LoadConfig loadcfg)
        {
            DetailWin.ClearContent();
            DetailWin.Show();
            DetailWin.LoadContent(loadcfg);
        }

        /// <summary>
        /// 
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

    }
}
