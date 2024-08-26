using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Goldnet.Ext.Web;
using Goldnet.Dal;
using System.Data;
using GoldNet.Comm;
using GoldNet.Comm.ExportData;


namespace GoldNet.JXKP
{
    public partial class SimpleIncome_Dept_Detail : PageBase
    {
        Report dal_report = new Report();
        protected void Page_Load(object sender, EventArgs e)
        {
            DateTime dtBegin = Convert.ToDateTime(Request["dtBegin"].ToString());
            string balance = Request["balance"].ToString();
            string deptcode = Request["deptcode"].ToString();
            string reck = Request["reck"].ToString();
            int months = Convert.ToInt32(Request["months"].ToString());
            string hospro = Request["hospro"].ToString();
            DataTable dt = dal_report.GetSimpledeptIncomeDetail(dtBegin, balance, deptcode, reck, months, hospro);
            if (dt != null)
            {
                SReport.RemoveFields();
                GridPanel_Show.Reconfigure();
                GridPanel_Show.ColumnModel.Columns.Clear();
                GoldNet.JXKP.cbhs.Report.BuildControl bc = new GoldNet.JXKP.cbhs.Report.BuildControl();
                for (int i = 0; i < dt.Columns.Count; i++)
                {

                    bc.AddRecord(dt.Columns[i].ColumnName, SReport);
                    ExtColumn Column = new ExtColumn();
                    Column.ColumnID = dt.Columns[i].ColumnName;
                    Column.Header = "<div style='text-align:center;'>" + dt.Columns[i].ColumnName + "</div>";
                    if (dt.Columns[i].ColumnName == "ITEM_NAME")
                    {
                        Column.Header = "<div style='text-align:center;'>" + "项目名称" + "</div>";
                    }
                    Column.Align = Alignment.Left;
                    if ( dt.Columns[i].ColumnName != "ITEM_NAME")
                    {
                        Column.Renderer.Fn = "rmbMoney";
                        Column.Align = Alignment.Right;
                    }
                    Column.DataIndex = dt.Columns[i].ColumnName;
                    Column.MenuDisabled = true;
                    Column.Width = 120;
                    GridPanel_Show.ColumnModel.Columns.Add(Column);
                    GridPanel_Show.AddColumn(Column);

                }

                SReport.DataSource = dt;
                SReport.DataBind();
                Session.Remove("SimpleIncomeDetail");
                Session["SimpleIncomeDetail"] = dt;
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
    }
}
