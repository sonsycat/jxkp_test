using System;
using System.Data;
using System.Collections.Generic;
using Goldnet.Ext.Web;
using Goldnet.Dal;
using GoldNet.Comm.ExportData;

namespace GoldNet.JXKP.RLZY.BaseInfoMaintain
{
    public partial class Staffworkday_KQ_report : System.Web.UI.Page
    {
        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Ext.IsAjaxRequest)
            {
                if (Session["CURRENTSTAFF"] == null)
                {
                    //Response.End();
                }

                //开始日期
                this.stardate.Value = Convert.ToDateTime(DateTime.Now.Year.ToString() + "-" + DateTime.Now.Month.ToString() + "-1").ToString("yyyy-MM-dd");

            }
        }

        /// <summary>
        /// 查询数据并绑定
        /// </summary>
        /// <param name="stdate"></param>
        /// <param name="enddate"></param>
        private void data(string date_time)
        {
            BaseInfoMaintainDal tdal = new BaseInfoMaintainDal();
            //科室权限
            string deptcode = "";

            DataTable dt = tdal.SearchKQRY(date_time, deptcode).Tables[0];
            if (dt != null)
            {
                Store1.DataSource = dt;
                Store1.DataBind();
                Session.Remove("Staffworkday_KQ_report");
                Session["Staffworkday_KQ_report"] = dt;
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

        /// <summary>
        /// 查询处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Btn_Query_Click(object sender, AjaxEventArgs e)
        {
            string date_time = Convert.ToDateTime(this.stardate.Value.ToString()).ToString("yyyyMMdd") ;
            data(date_time);
        }

        /// <summary>
        /// EXCEL导出
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void OutExcel(object sender, EventArgs e)
        {

            if (Session["Staffworkday_KQ_report"] != null)
            {
                ExportData ex = new ExportData();
                DataTable dt = (DataTable)Session["Staffworkday_KQ_report"];

                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    if (dt.Columns[i].ColumnName == "DEPT_NAME")
                    {
                        dt.Columns[i].ColumnName = "科室";
                    }
                    else if (dt.Columns[i].ColumnName == "RS")
                    {
                        dt.Columns[i].ColumnName = "空勤人数";
                    }
                }
                ex.ExportToLocal(dt, this.Page, "xls", "科室空勤情况统计");
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
    }
}
