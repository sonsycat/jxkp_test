using System;
using System.Data;
using Goldnet.Ext.Web;
using Goldnet.Dal;
using GoldNet.Model;
using System.Collections.Generic;

namespace GoldNet.JXKP.cbhs.datagather
{
    public partial class single_cost_decompose : PageBase
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

                for (int i = 0; i < 10; i++)
                {
                    int years = System.DateTime.Now.Year - i;
                    this.years.Items.Add(new Goldnet.Ext.Web.ListItem(years.ToString(), years.ToString()));
                }
                this.years.SelectedItem.Value = System.DateTime.Now.ToString("yyyy");
                this.months.SelectedItem.Value = System.DateTime.Now.ToString("MM");
                SetDict();
                string item_code = this.COST_ITEM.SelectedItem.Value.ToString();
                string date_time = System.DateTime.Now.ToString("yyyy-MM-dd");
                //Bindlist(item_code, date_time);
            }
        }

        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="item_code"></param>
        /// <param name="date_time"></param>
        protected void Bindlist(string item_code, string date_time)
        {
            Cost_detail dal = new Cost_detail();
            DataTable dt = dal.GetSingleCostdecompose(item_code, date_time, "3", this.PROG_ITEM.SelectedItem.Value, "0").Tables[0];//成本分解3
            this.Store1.DataSource = dt;
            this.Store1.DataBind();
        }

        /// <summary>
        /// 
        /// </summary>
        protected void SetDict()
        {
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
                setprog(dt.Rows[0]["ITEM_CODE"].ToString());
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="itemcode"></param>
        protected void setprog(string itemcode)
        {
            Cost_detail dal = new Cost_detail();
            DataTable progdt = dal.GetProgByCostItem(itemcode).Tables[0];
            this.PROG_ITEM.Items.Add(new Goldnet.Ext.Web.ListItem("全部", ""));
            for (int i = 0; i < progdt.Rows.Count; i++)
            {
                this.PROG_ITEM.Items.Add(new Goldnet.Ext.Web.ListItem(progdt.Rows[i]["PROG_NAME"].ToString(), progdt.Rows[i]["PROG_CODE"].ToString()));

            }
            this.PROG_ITEM.SelectedIndex = 0;
        }

        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Button_look_click(object sender, EventArgs e)
        {
            string item_code = this.COST_ITEM.SelectedItem.Value.ToString();
            string date_time = this.years.SelectedItem.Value.ToString() + "-" + this.months.SelectedItem.Value.ToString() + "-01";
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
            string date_time = this.years.SelectedItem.Value.ToString() + this.months.SelectedItem.Value.ToString();
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
            else
            {

                try
                {
                    string datetime = this.years.SelectedItem.Value.ToString() + "-" + this.months.SelectedItem.Value.ToString() + "-01";
                    for (int i = 0; i < selectRow.Length; i++)
                    {
                        string dept_code = selectRow[i]["DEPT_CODE"];
                        dal.DelSingleCostsp(datetime, item_code, dept_code, "3", this.PROG_ITEM.SelectedItem.Value);
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

        //反序列化得到客户端提交的gridpanel数据行      
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
        protected void Prog_SelectOnChange(object sender, AjaxEventArgs e)
        {
            Goldnet.Ext.Web.ScriptManager scManager = Goldnet.Ext.Web.ScriptManager.GetInstance(this.Page);
            scManager.AddScript("Store1.reload();");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Item_SelectOnChange(object sender, AjaxEventArgs e)
        {
            string item_code = this.COST_ITEM.SelectedItem.Value.ToString();
            string date_time = this.years.SelectedItem.Value.ToString() + this.months.SelectedItem.Value.ToString();
            Goldnet.Ext.Web.ScriptManager scManager = Goldnet.Ext.Web.ScriptManager.GetInstance(this.Page);
            scManager.AddScript("Store1.reload();");
            scManager.AddScript(PROG_ITEM.ClientID + ".store.removeAll();");
            scManager.AddScript(PROG_ITEM.ClientID + ".clearValue();");
            Cost_detail dal = new Cost_detail();
            DataTable progdt = dal.GetProgByCostItem(item_code).Tables[0];
            this.PROG_ITEM.Items.Add(new Goldnet.Ext.Web.ListItem("全部", ""));
            for (int i = 0; i < progdt.Rows.Count; i++)
            {
                this.PROG_ITEM.AddItem(progdt.Rows[i]["PROG_NAME"].ToString(), progdt.Rows[i]["PROG_CODE"].ToString());
            }
            this.PROG_ITEM.SelectedIndex = 0;
        }

        /// <summary>
        /// 保存处理
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
                    Message = "该月奖金已经生成、不可以改变成本数据!",
                    Buttons = MessageBox.Button.OK,
                    Icon = (MessageBox.Icon)Enum.Parse(typeof(MessageBox.Icon), "INFO")
                });
                return;
            }
            else if (this.PROG_ITEM.SelectedItem.Value.Equals("") || this.PROG_ITEM.SelectedItem.Value.Equals("全部"))
            {
                this.ShowMessage("系统提示", "不能是全部分摊方案，请选择一个分摊方案！");
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

            List<CostDetail> costdetail = e.Object<CostDetail>();
            Cost_detail dal = new Cost_detail();
            string item_code = this.COST_ITEM.SelectedItem.Value.ToString();
            User user = (User)Session["CURRENTSTAFF"];
            string operators = user.UserName;
            try
            {
                string datetime = this.years.SelectedItem.Value.ToString() + "-" + this.months.SelectedItem.Value.ToString() + "-01";
                dal.SaveSingleCostsdecompose(costdetail, item_code, datetime, operators, "3", this.PROG_ITEM.SelectedItem.Value);
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
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void Data_RefreshData(object sender, StoreRefreshDataEventArgs e)
        {
            string date_time = this.years.SelectedItem.Value + "-" + this.months.SelectedItem.Value + "-01";
            string costitem = this.COST_ITEM.SelectedItem.Value.ToString();
            Cost_detail dal = new Cost_detail();
            this.Store1.DataSource = dal.GetSingleCostdecompose(costitem, date_time, "3", this.PROG_ITEM.SelectedItem.Value, "0").Tables[0];
            this.Store1.DataBind();
        }

        /// <summary>
        /// 成本分解处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Button_break_click(object sender, EventArgs e)
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
            else if (this.PROG_ITEM.SelectedItem.Value.Equals(""))
            {
                this.ShowMessage("系统提示", "不能是全部分摊方案，请选择一个分摊方案！");
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

            if (this.AllCosts.Value == null)
            {
                Goldnet.Ext.Web.Ext.Msg.Show(new Goldnet.Ext.Web.MessageBox.Config
                {
                    Title = "信息提示",
                    Message = "请输入成本总额！",
                    Buttons = MessageBox.Button.OK,
                    Icon = (MessageBox.Icon)Enum.Parse(typeof(MessageBox.Icon), "INFO")
                });
                return;
            }
            string costitem = this.COST_ITEM.SelectedItem.Value.ToString();
            double costvalue;
            string rtnmsg = "";
            try
            {
                costvalue = Convert.ToDouble(this.AllCosts.Value);
                try
                {
                    Cost_detail dal = new Cost_detail();
                    rtnmsg = dal.Exec_Sp_Extract_Cost_Input(this.PROG_ITEM.SelectedItem.Value, date_time, costitem, costvalue);
                    //查询绑定临时数据
                    DataTable dt = dal.GetSingleCostTmp().Tables[0];
                    this.Store1.DataSource = dt;
                    this.Store1.DataBind();
                    this.AllCosts.Clear();
                    Goldnet.Ext.Web.Ext.Msg.Show(new Goldnet.Ext.Web.MessageBox.Config
                    {
                        Title = "信息提示",
                        Message = "分解完成，注意保存！",
                        Buttons = MessageBox.Button.OK,
                        Icon = (MessageBox.Icon)Enum.Parse(typeof(MessageBox.Icon), "INFO")
                    });
                }
                catch (Exception ex)
                {
                    this.ShowDataError(ex.Message.ToString(), Request.Path, "Button_create_click");
                }
            }
            catch (Exception ex)
            {
                this.ShowDataError(ex.Message.ToString(), Request.Path, "Button_break_click");
            }
        }

    }
}
