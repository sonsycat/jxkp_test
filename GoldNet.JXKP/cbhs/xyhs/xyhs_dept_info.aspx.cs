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
using GoldNet.Model;
using System.Collections.Generic;

namespace GoldNet.JXKP.cbhs.xyhs
{
    public partial class xyhs_dept_info : PageBase
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
            //公摊方案
            XyhsDict dal = new XyhsDict();
            DataTable dt = dal.GetProgDict().Tables[0];
            this.Store2.DataSource = dt;
            this.Store2.DataBind();
        }
        //查询绑定主数据
        private void Bindlist()
        {
            string date_time = this.years.SelectedItem.Value + this.months.SelectedItem.Value;
            XyhsDict dal = new XyhsDict();
            DataTable dt = dal.GetDeptInfo(date_time).Tables[0];
            this.Store1.DataSource = dt;
            this.Store1.DataBind();
        }
        //保存
        protected void SubmitData(object sender, StoreSubmitDataEventArgs e)
        {
            string date_time= this.years.SelectedItem.Value+this.months.SelectedItem.Value;
            List<YxhsDeptInfo> deptinfos = e.Object<YxhsDeptInfo>();
            XyhsDict dal = new XyhsDict();
            try
            {
                dal.SaveDeptInfo(deptinfos, date_time);
                Goldnet.Ext.Web.Ext.Msg.Show(new Goldnet.Ext.Web.MessageBox.Config
                {
                    Title = "信息提示",
                    Message = "保存成功",
                    Buttons = MessageBox.Button.OK,
                    Icon = (MessageBox.Icon)Enum.Parse(typeof(MessageBox.Icon), "INFO")
                });
                Bindlist();
            }
            catch (Exception ex)
            {
                this.ShowDataError(ex.Message.ToString(), Request.Path, "SubmitData");
            }
        }
        //查询
        protected void Button_look_click(object sender, EventArgs e)
        {
            Bindlist();
        }
        
    }
}
