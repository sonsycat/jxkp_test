using System;
using Goldnet.Ext.Web;
using Goldnet.Dal;
using System.Data;
using GoldNet.Comm.ExportData;

namespace GoldNet.JXKP.Bonus.Input
{
    public partial class GongZuoLiang_Select : PageBase
    {
        private Report report_dal = new Report();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Ext.IsAjaxRequest)
            {
                this.stardate.Value = System.DateTime.Now.ToString("yyyy-MM") + "-01";
                this.enddate.Value = System.DateTime.Now.ToString("yyyy-MM-dd");
             
                SetStoreProxy();
            }
        }

        private void SetStoreProxy()
        {
            //查找科室信息
            HttpProxy pro = new HttpProxy();
            pro.Method = HttpMethod.POST;
            pro.Url = "../../../cbhs/WebService/BonusDepts.ashx?deptfilter=" + this.DeptFilter("dept_code");
            this.SDept.Proxy.Add(pro);
            JsonReader jr = new JsonReader();
            jr.ReaderID = "DEPT_CODE";
            jr.Root = "Bonusdepts";
            jr.TotalProperty = "totalCount";
            RecordField rf = new RecordField();
            rf.Name = "DEPT_CODE";
            jr.Fields.Add(rf);
            RecordField rfn = new RecordField();
            rfn.Name = "DEPT_NAME";
            jr.Fields.Add(rfn);
            this.SDept.Reader.Add(jr);
        }

        protected void GetQueryPortalet(object sender, AjaxEventArgs e)
        {
            string stardate = Convert.ToDateTime(this.stardate.SelectedValue).ToString("yyyy-MM-dd");
            string enddate = Convert.ToDateTime(this.enddate.SelectedValue).ToString("yyyy-MM-dd");
            data(stardate, enddate);
            Session["dstdate"] = stardate;
            Session["deddate"] = enddate;
        }
        private void data(string stardate, string enddate)
        {

            string balance = cbbType.SelectedItem.Value;
            string deptcode = this.DeptFilter("");
            string dept = cbbdept.SelectedItem.Value;

            DataTable dt = report_dal.GetAccountDeptGZL(stardate, enddate,balance, dept);

            Store1.DataSource = dt;
            Store1.DataBind();
            Session.Remove("GongZuoLiang_Select");
            Session["GongZuoLiang_Select"] = dt;
            
        }
        protected void OutExcel(object sender, EventArgs e)
        {
            if (Session["GongZuoLiang_Select"] != null)
            {
                ExportData ex = new ExportData();
                DataTable dt = (DataTable)Session["GongZuoLiang_Select"];
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                   
                     if (dt.Columns[i].ColumnName == "DEPT_NAME")
                    {
                        dt.Columns[i].ColumnName = "科室名称";
                    }
                    else if (dt.Columns[i].ColumnName == "XMMC")
                    {
                        dt.Columns[i].ColumnName = "项目名称";
                    }
                    else if (dt.Columns[i].ColumnName == "JBMC")
                    {
                        dt.Columns[i].ColumnName = "级别名称";
                    }
                    else if (dt.Columns[i].ColumnName == "AMOUNT")
                    {
                        dt.Columns[i].ColumnName = "数量";
                    }
                    else if (dt.Columns[i].ColumnName == "JF")
                    {
                        dt.Columns[i].ColumnName = "积分";
                    }
                     else if (dt.Columns[i].ColumnName == "ZJF")
                     {
                         dt.Columns[i].ColumnName = "总积分";
                     }


                }
                ex.ExportToLocal(dt, this.Page, "xls", "科室工作量明细");

            }
        
        
        
        }
    }
}