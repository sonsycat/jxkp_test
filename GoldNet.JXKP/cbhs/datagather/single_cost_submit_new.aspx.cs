using System;
using System.Data;
using System.Web.UI.WebControls;
using Goldnet.Ext.Web;
using Goldnet.Dal;
using GoldNet.Model;
using System.Collections.Generic;

namespace GoldNet.JXKP.cbhs.datagather
{
    public partial class single_cost_submit_new : PageBase
    {
        /// <summary>
        /// 
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

                string a1 = Request.QueryString["startime"].ToString();
                string a2 = Request.QueryString["costitem"].ToString();

                //开始日期
                this.stardate.Value = Convert.ToDateTime(a1).ToString("yyyy-MM-dd");

                //设置成本项目字典列表
                SetDict(a2);
                string item_code = this.COST_ITEM.SelectedItem.Value.ToString();
                string date_time = System.DateTime.Now.ToString("yyyyMM");

                SetStoreProxy();

                //获取单项成本列表后绑定
                Bindlist(a2, Convert.ToDateTime(a1).ToString("yyyy-MM-dd"));

                //按钮权限控制
                this.Button1.Visible = this.IsEdit();
                this.Buttonfh.Visible = this.IsPass();

                if (this.IsEdit() || this.IsPass())
                {
                    this.Button_no.Visible = true;
                }
                else
                    this.Button_no.Visible = false;
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
        /// 
        /// </summary>
        protected void SetDict(string a2)
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
            //if (dt.Rows.Count > 0)
            //{
            //    this.COST_ITEM.SelectedItem.Value = dt.Rows[0]["ITEM_CODE"].ToString();
            //}

            if (a2 != "")
            {
                this.COST_ITEM.SelectedItem.Value = a2;
            }
            else if (dt.Rows.Count > 0)
            {
                this.COST_ITEM.SelectedItem.Value = dt.Rows[0]["ITEM_CODE"].ToString();
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
        /// 查询操作处理
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
        /// 数据刷新
        /// </summary>
        /// <param name="item_code"></param>
        /// <param name="date_time"></param>
        protected void Bindlist(string item_code, string date_time)
        {
            Bindsubcost(Convert.ToDateTime(date_time).ToString("yyyyMM"));
        }

        /// <summary>
        /// 获取提交信息
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
        /// 成本提交操作处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Button_ok_click(object sender, AjaxEventArgs e)
        {
            User user = (User)Session["CURRENTSTAFF"];
            Dictionary<string, string>[] selectRow = GetSelectRow(e);
            string item_code = this.COST_ITEM.SelectedItem.Value.ToString();
            string date_time = Convert.ToDateTime(this.stardate.Value.ToString()).ToString("yyyyMM");
            string kk = Convert.ToDateTime(this.stardate.Value.ToString()).ToString("yyyyMMdd");
            if (selectRow == null || selectRow.Length < 1)
            {
                this.SelectRecord();
            }
            else
            {
                try
                {
                    Cost_detail dal = new Cost_detail();
                    string ll = "1";
                    string ff = "";
                    for (int i = 0; i < selectRow.Length; i++)
                    {
                        string itemcode = selectRow[i]["ITEM_CODE"];
                        string itemname = selectRow[i]["ITEM_NAME"];
                        if (!dal.GetIsSubmit(date_time, item_code) && dal.GetIsSave(date_time, itemcode))
                        {
                            dal.Submitcost(date_time, itemcode, user.UserName);
                        }
                        else 
                        {
                            //this.ShowMessage("提示", itemname+"项目没有保存，不能提交！");
                            ff = itemname;
                            ll = "0";
                            break;
                        }
                    }
                    if (ll == "1")
                    {
                        this.SaveSucceed();
                        Bindlist(item_code, Convert.ToDateTime(this.stardate.Value.ToString()).ToString("yyyy-MM") + "-01");
                    }
                    else 
                    {
                        this.Saveshibai(ff);
                    }
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
    }
}
