using System;
using System.Data;
using System.Web.UI.WebControls;
using Goldnet.Ext.Web;
using Goldnet.Dal;
using GoldNet.Model;
using System.Collections.Generic;
using GoldNet.Comm.ExportData;

namespace GoldNet.JXKP.cbhs.xyhs.Operation
{
    public partial class Dept_Cost_Ext : PageBase
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

                for (int i = 0; i < 10; i++)
                {
                    int years = System.DateTime.Now.Year - i;
                    this.years.Items.Add(new Goldnet.Ext.Web.ListItem(years.ToString(), years.ToString()));
                }
                this.years.SelectedItem.Value = System.DateTime.Now.ToString("yyyy");
                this.months.SelectedItem.Value = System.DateTime.Now.ToString("MM");

                SetStoreProxy();
                Bindlist();
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
            pro2.Url = "../WebService/xyhs_costs_dicts.ashx";
            this.Store2.Proxy.Add(pro2);
            HttpProxy pro3 = new HttpProxy();
            pro3.Method = HttpMethod.POST;
            pro3.Url = "../WebService/xyhs_dept_dicts.ashx";
            this.Store3.Proxy.Add(pro3);
        }

        /// <summary>
        /// 获取单项成本列表后绑定
        /// </summary>
        /// <param name="item_code"></param>
        /// <param name="date_time"></param>
        protected void Bindlist()
        {
            XyhsOperation dal = new XyhsOperation();
            string date = this.years.SelectedItem.Value + "-" + this.months.SelectedItem.Value + "-01";
            DataTable dt = dal.GetdDeptCostext(date,this.ComboBoxdept.SelectedItem.Value,this.ComboBoxitem.SelectedItem.Value, "0").Tables[0];
            this.Store1.DataSource = dt;
            this.Store1.DataBind();

            if (dt.Rows.Count > 0)
            {
                this.CB_SUM.Text = "合计：" + dt.Compute("Sum(COSTS)", "").ToString();

            }
            else
            {
                this.CB_SUM.Text = "";

            }


        }



        /// <summary>
        /// 查询处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Button_look_click(object sender, EventArgs e)
        {
            Bindlist();
        }

        /// <summary>
        /// 数据提取
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Button_ext_click(object sender, AjaxEventArgs e)
        {
            string date_time = this.years.SelectedItem.Value + this.months.SelectedItem.Value;
            XyhsOperation dal = new XyhsOperation();
            try
            {

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
            catch (Exception ex)
            {
                this.ShowDataError(ex.Message.ToString(), Request.Path, "Button_get_click");
            }
        }

     

        /// <summary>
        /// 时间选择触发
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Data_SelectOnChange(object sender, EventArgs e)
        {
            Bindlist();
        }
       

        /// <summary>
        /// 数据刷新
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void Data_RefreshData(object sender, StoreRefreshDataEventArgs e)
        {
            Bindlist();
        }



    }
}
