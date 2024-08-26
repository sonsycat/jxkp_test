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
    public partial class DoctorDetail : PageBase
    {
        Report dal_report = new Report();
        
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!Ext.IsAjaxRequest)
            {
                string str = Session.SessionID;
                HttpProxy prodept = new HttpProxy();
                prodept.Method = HttpMethod.POST;
                string deptcode = this.DeptFilter("");
                prodept.Url = "../../WebService/Depts.ashx";
                this.SDept.Proxy.Add(prodept);
                JsonReader jrdept = new JsonReader();
                jrdept.ReaderID = "DEPT_CODE";
                jrdept.Root = "deptlist";
                jrdept.TotalProperty = "totalCount";
                RecordField rfdept = new RecordField();
                rfdept.Name = "DEPT_CODE";
                jrdept.Fields.Add(rfdept);
                RecordField rfndept = new RecordField();
                rfndept.Name = "DEPT_NAME";
                jrdept.Fields.Add(rfndept);
                this.SDept.Reader.Add(jrdept);

                HttpProxy pro = new HttpProxy();
                pro.Method = HttpMethod.POST;
                pro.Url = "../WebService/ReckItems.ashx";
                this.SCostitem.Proxy.Add(pro);
                JsonReader jr = new JsonReader();
                jr.ReaderID = "CLASS_CODE";
                jr.Root = "itemlist";
                jr.TotalProperty = "totalitems";
                RecordField rf = new RecordField();
                rf.Name = "CLASS_CODE";
                jr.Fields.Add(rf);
                RecordField rfn = new RecordField();
                rfn.Name = "CLASS_NAME";
                jr.Fields.Add(rfn);
                this.SCostitem.Reader.Add(jr);
                this.stardate.Value = Convert.ToDateTime(DateTime.Now.Year.ToString()+"-"+DateTime.Now.Month.ToString()+"-1").ToString("yyyy-MM-dd");
                this.enddate.Value = DateTime.Now.ToString("yyyy-MM-dd");
            }
        }
        protected void Btn_Query_Click(object sender, AjaxEventArgs e)
        {
            //string deptcode = this.DeptFilter("");
            int date = Convert.ToDateTime(this.enddate.Value.ToString()).Subtract(Convert.ToDateTime(stardate.Value.ToString())).Days;
            if (date > 31)
            {
                this.ShowMessage("系统提示","时间范围太大，不能超过31天");
            }
            else if (this.cbbdept.SelectedItem.Text.Equals(string.Empty) | this.cbb_ReckItem.SelectedItem.Text.Equals(string.Empty))
            {
                this.ShowMessage("系统提示","科室和收入项目不能为空！");
            }
            else
            {
                DataTable dt = dal_report.GetDoctorDeatil(stardate.Value.ToString(), this.enddate.Value.ToString(), this.cbbType.SelectedItem.Value, this.cbbdept.SelectedItem.Value, this.cbb_ReckItem.SelectedItem.Value);
                if (dt != null)
                {
                    if (dt.Rows.Count > 10000)
                    {
                        this.ShowMessage("系统提示","数据量过大，请重新确定时间范围");
                    }
                    else
                    {
                    SReport.RemoveFields();
                    GridPanel_Show.Reconfigure();
                    GridPanel_Show.ColumnModel.Columns.Clear();
                    GoldNet.JXKP.cbhs.Report.BuildControl bc = new GoldNet.JXKP.cbhs.Report.BuildControl();
                    for (int i = 0; i < dt.Columns.Count; i++)
                    {
                        if (dt.Columns[i].ColumnName != "DEPT_CODE")
                        {
                            bc.AddRecord(dt.Columns[i].ColumnName, SReport);
                            ExtColumn Column = new ExtColumn();
                            Column.ColumnID = dt.Columns[i].ColumnName;
                            Column.Header = "<div style='text-align:center;'>" + dt.Columns[i].ColumnName + "</div>";
                            Column.Align = Alignment.Center;
                            Column.DataIndex = dt.Columns[i].ColumnName;
                            Column.MenuDisabled = true;
                            Column.Width = 120;
                            GridPanel_Show.ColumnModel.Columns.Add(Column);
                            GridPanel_Show.AddColumn(Column);
                        }
                    }

                    SReport.DataSource = dt;
                    SReport.DataBind();
                    Session.Remove("DoctorDetail");
                    Session["DoctorDetail"] = dt;
                        }
                    
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
        protected void Store_RefreshData(object sender, StoreRefreshDataEventArgs e)
        {
            Btn_Query_Click(null,null);
        }
        protected void OutExcel(object sender, EventArgs e)
        {
            if (Session["DoctorDetail"] != null)
            {
                ExportData ex = new ExportData();
                DataTable dt = (DataTable)Session["DoctorDetail"];
                //ex.ExportToLocal(dt, this.Page, "xls", "科室收入明细报表");
                outexcel(dt, "科室收入明细报表");
            }
        }
        

    }
}
