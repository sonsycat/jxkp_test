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
using GoldNet.JXKP.cbhs.datagather;
using System.Collections.Generic;

namespace GoldNet.JXKP.Bonus.Input
{
    public partial class OperationInfoSearch : PageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Ext.IsAjaxRequest)
            {
          


            }
        }

        protected void Btn_Query_Click(object sender, AjaxEventArgs e)
        {
            string stardate = Convert.ToDateTime(this.stardate.SelectedValue).ToString("yyyyMMdd");
            string enddate = Convert.ToDateTime(this.enddate.SelectedValue).ToString("yyyyMMdd");

            OperationInfo report_dal = new OperationInfo();

            DataTable dt = report_dal.GetOperationInfo(stardate, enddate);
            if (dt != null)
            {
                SReport.DataSource = dt;
                SReport.DataBind();
                Session.Remove("OperationInfoSearch");
                Session["OperationInfoSearch"] = dt;
            }
            else
            {
                Goldnet.Ext.Web.Ext.Msg.Show(new Goldnet.Ext.Web.MessageBox.Config
                {
                    Title = "提示",
                    Message = "未找到数据",
                    Buttons = MessageBox.Button.OK,
                    Icon = (MessageBox.Icon)Enum.Parse(typeof(MessageBox.Icon), "INFO")
                });
            }
        }

        protected void OutExcel(object sender, EventArgs e)
        {
            if (Session["OperationInfoSearch"] != null)
            {
                GoldNet.Comm.ExportData.ExportData ex = new GoldNet.Comm.ExportData.ExportData();
                DataTable dt = (DataTable)Session["OperationInfoSearch"];



                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    if (dt.Columns[i].ColumnName == "ACCOUNT_DEPT_NAME")
                    {
                        dt.Columns[i].ColumnName = "科室";
                    }
                    else if (dt.Columns[i].ColumnName == "THENUMBERFO")
                    {
                        dt.Columns[i].ColumnName = "医生";
                    }
                    else if (dt.Columns[i].ColumnName == "MFS")
                    {
                        dt.Columns[i].ColumnName = "金额";
                    }
                   
                   

                }
                ex.ExportToLocal(dt, this.Page, "xls", "手术奖励表");
            }
        }



    }
}