using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.Text;
using Goldnet.Dal;
using GoldNet.Comm;
using GoldNet.Model;
using Goldnet.Ext.Web;
using GoldNet.Comm.ExportData;
using System.Collections.Generic;

namespace GoldNet.JXKP.cbhs.datagather
{
    public partial class Manual_Decompose_List : PageBase
    {
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
                    int year = System.DateTime.Now.Year - i;
                    this.years.Items.Add(new Goldnet.Ext.Web.ListItem(year.ToString(), year.ToString()));
                }
                this.years.SelectedItem.Value = System.DateTime.Now.ToString("yyyy");
                this.months.SelectedItem.Value = System.DateTime.Now.ToString("MM");
            }
        }
        
        //查询绑定成本正式表数据
        protected void Bindlist( string date_time)
        {
            Cost_detail dal = new Cost_detail();
            DataTable dt = dal.GetManualCostList(date_time,"1");
            this.Store1.DataSource = dt;
            this.Store1.DataBind();

            DataTable dtm = dal.GetManualCostList(date_time, "0");
            this.Store2.DataSource = dtm;
            this.Store2.DataBind();
        }
       
        //查询
        protected void Button_look_click(object sender, EventArgs e)
        {
            string date_time = this.years.SelectedItem.Value + this.months.SelectedItem.Value;
            Bindlist(date_time);

        }

        protected void costs_click(object sender, AjaxEventArgs e)
        {
            Cost_detail dal = new Cost_detail();
            string date_time = this.years.SelectedItem.Value + this.months.SelectedItem.Value;
            try
            {
                dal.Exec_Sp_Manual_Cost_Pre(date_time);
                Button_look_click(null, null);
                this.ShowMessage("系统提示", "科室成本归集成功！");
            }
            catch (Exception ex)
            {
                this.ShowDataError(ex, Request.Path, "costs_click");
            }

        }
    }
}
