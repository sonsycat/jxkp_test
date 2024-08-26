using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using Goldnet.Ext.Web;
using Goldnet.Dal;
using System.Collections.Generic;

namespace GoldNet.JXKP.cbhs.xyhs
{
    public partial class xyhs_get_total_costs : PageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Ext.IsAjaxRequest)
            {
                //检查是否已经登录，否则停止
                if (Session["CURRENTSTAFF"] == null)
                {
                    Response.End();
                }
                SetInit();
                Bindlist();
            }
        }
        //界面初始化
        private void SetInit()
        {
            //时间
            for (int i = 0; i < 10; i++)
            {
                int years = System.DateTime.Now.Year - i;
                this.years.Items.Add(new Goldnet.Ext.Web.ListItem(years.ToString(), years.ToString()));
            }
            this.years.SelectedItem.Value = System.DateTime.Now.ToString("yyyy");
            this.months.SelectedItem.Value = System.DateTime.Now.ToString("MM");
        }
        //查询绑定主数据
        private void Bindlist()
        {
            string date_time = this.years.SelectedItem.Value + this.months.SelectedItem.Value;
            XyhsDetail dal = new XyhsDetail();
            DataTable dt = dal.GetTotalCosts(date_time).Tables[0];
            this.Store1.DataSource = dt;
            this.Store1.DataBind();

        }
        //时间条件改变
        protected void Date_SelectOnChange(object sender, EventArgs e)
        {
            Bindlist();
        }
        //查询
        protected void Button_look_click(object sender, EventArgs e)
        {
            Bindlist();
        }
        //添加
        protected void Button_add_click(object sender, EventArgs e)
        {
            string date_time = this.years.SelectedItem.Value + this.months.SelectedItem.Value;
            LoadConfig loadcfg = getLoadConfig("xyhs_get_total_costs_detail.aspx");
            loadcfg.Params.Add(new Goldnet.Ext.Web.Parameter("op", "add"));
            loadcfg.Params.Add(new Goldnet.Ext.Web.Parameter("date_time", date_time));
            loadcfg.Params.Add(new Goldnet.Ext.Web.Parameter("date_time", this.years.SelectedItem.Value + this.months.SelectedItem.Value));
            showDetailWin(loadcfg);
        }
        //删除
        protected void Button_del_click(object sender, AjaxEventArgs e)
        {
            string date_time = this.years.SelectedItem.Value + this.months.SelectedItem.Value;
            Dictionary<string, string>[] selectRow = GetSelectRow(e);
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
            else
            {
                try
                {
                    XyhsDetail dal = new XyhsDetail();
                    dal.DelTotalCosts(date_time, selectRow);

                    Goldnet.Ext.Web.Ext.Msg.Show(new Goldnet.Ext.Web.MessageBox.Config
                    {
                        Title = "信息提示",
                        Message = "删除成功",
                        Buttons = MessageBox.Button.OK,
                        Icon = (MessageBox.Icon)Enum.Parse(typeof(MessageBox.Icon), "INFO")
                    });
                }
                catch (Exception ex)
                {
                    this.ShowDataError(ex.Message.ToString(), Request.Path, "Button_del_click");
                }
            }


            Bindlist();
        }
        //彻底删除、删除本月数据同时删除基础数据
        protected void Button_del_true_click(object sender, AjaxEventArgs e)
        {
            string date_time = this.years.SelectedItem.Value + this.months.SelectedItem.Value;
            Dictionary<string, string>[] selectRow = GetSelectRow(e);
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
            else
            {
                try
                {
                    XyhsDetail dal = new XyhsDetail();
                    dal.DelTrueTotalCosts(date_time, selectRow);

                    Goldnet.Ext.Web.Ext.Msg.Show(new Goldnet.Ext.Web.MessageBox.Config
                    {
                        Title = "信息提示",
                        Message = "删除成功",
                        Buttons = MessageBox.Button.OK,
                        Icon = (MessageBox.Icon)Enum.Parse(typeof(MessageBox.Icon), "INFO")
                    });
                }
                catch (Exception ex)
                {
                    this.ShowDataError(ex.Message.ToString(), Request.Path, "Button_del_click");
                }
            }


            Bindlist();
        }
        //反序列化得到客户端提交的gridpanel数据行      
        private Dictionary<string, string>[] GetSelectRow(AjaxEventArgs e)
        {
            string row = e.ExtraParams["Values"].ToString();
            Dictionary<string, string>[] selectRow = JSON.Deserialize<Dictionary<string, string>[]>(row);
            if (selectRow.Length <= 0) { return null; } else { return selectRow; }
        }
        //修改记录
        [AjaxMethod]
        public void data_edit(string item_code)
        {
            string date_time = this.years.SelectedItem.Value + this.months.SelectedItem.Value;
            LoadConfig loadcfg = getLoadConfig("xyhs_get_total_costs_detail.aspx");
            loadcfg.Params.Add(new Goldnet.Ext.Web.Parameter("op", "edit"));
            loadcfg.Params.Add(new Goldnet.Ext.Web.Parameter("item_code", item_code));
            loadcfg.Params.Add(new Goldnet.Ext.Web.Parameter("date_time", date_time));
            loadcfg.Params.Add(new Goldnet.Ext.Web.Parameter("date_time", this.years.SelectedItem.Value + this.months.SelectedItem.Value));
            showDetailWin(loadcfg);

        }

        /// <summary>
        /// 获取院级总成本
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Button_get_click(object sender, EventArgs e)
        {
            string date_time = this.years.SelectedItem.Value + this.months.SelectedItem.Value;
            XyhsDetail dal = new XyhsDetail();
            try
            {
                DataTable dt = dal.GetTotalCostsFromViews(date_time).Tables[0];
                if (dt.Rows.Count < 1)
                {
                    Goldnet.Ext.Web.Ext.Msg.Show(new Goldnet.Ext.Web.MessageBox.Config
                    {
                        Title = "信息提示",
                        Message = "未提取到数据",
                        Buttons = MessageBox.Button.OK,
                        Icon = (MessageBox.Icon)Enum.Parse(typeof(MessageBox.Icon), "INFO")
                    });
                    return;
                }
                else
                {
                    //dal.SaveTatalCostsFromViews(date_time);
                    dal.SaveEXPORT_COSTS(date_time);
                    Goldnet.Ext.Web.Ext.Msg.Show(new Goldnet.Ext.Web.MessageBox.Config
                    {
                        Title = "信息提示",
                        Message = "提取成功",
                        Buttons = MessageBox.Button.OK,
                        Icon = (MessageBox.Icon)Enum.Parse(typeof(MessageBox.Icon), "INFO")
                    });
                    Bindlist();
                }
            }
            catch (Exception ex)
            {
                this.ShowDataError(ex.Message.ToString(), Request.Path, "Button_get_click");
            }
        }

        //获取物质科级总成本
        protected void Button_getwuzhi_click(object sender, EventArgs e)
        {
            string date_time = this.years.SelectedItem.Value + this.months.SelectedItem.Value;
            XyhsDetail dal = new XyhsDetail();
            try
            {
                DataTable dt = dal.GetTotalCostsFromViews(date_time).Tables[0];
                if (dt.Rows.Count < 1)
                {
                    Goldnet.Ext.Web.Ext.Msg.Show(new Goldnet.Ext.Web.MessageBox.Config
                    {
                        Title = "信息提示",
                        Message = "未提取到数据",
                        Buttons = MessageBox.Button.OK,
                        Icon = (MessageBox.Icon)Enum.Parse(typeof(MessageBox.Icon), "INFO")
                    });
                    return;
                }
                else
                {
                    try
                    {
                        dal.SaveWuzhiZhijieChengben(dt);
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
                        Goldnet.Ext.Web.Ext.Msg.Show(new Goldnet.Ext.Web.MessageBox.Config
                        {
                            Title = "信息提示",
                            Message = "提取失败" + ex.Message,
                            Buttons = MessageBox.Button.OK,
                            Icon = (MessageBox.Icon)Enum.Parse(typeof(MessageBox.Icon), "INFO")
                        });
                    }

                }
            }
            catch (Exception ex)
            {
                this.ShowDataError(ex.Message.ToString(), Request.Path, "Button_get_click");
            }
        }
        //显示添加窗口
        private void showDetailWin(LoadConfig loadcfg)
        {
            DetailWin.ClearContent();
            DetailWin.Show();
            DetailWin.LoadContent(loadcfg);
        }
        protected void Store_RefreshData(object sender, StoreRefreshDataEventArgs e)
        {
            Bindlist();
        }
    }
}
