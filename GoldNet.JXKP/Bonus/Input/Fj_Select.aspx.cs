using System;
using Goldnet.Ext.Web;
using Goldnet.Dal;
using System.Data;
using GoldNet.Comm.ExportData;

namespace GoldNet.JXKP.Bonus.Input
{
    public partial class Fj_Select : PageBase
    {
        private Report report_dal = new Report();

        protected void Page_Load(object sender, EventArgs e)
        {

            if (!Ext.IsAjaxRequest)
            {

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
            BindDate(Convert.ToDateTime(this.stardate.SelectedValue).ToString("yyyyMM"));
        }

        protected void Store_RefreshData(object sender, StoreRefreshDataEventArgs e)
        {
            BindDate(Convert.ToDateTime(this.stardate.SelectedValue).ToString("yyyyMM"));
        }

        private void BindDate(string date)
        {
            //string balance = cbbType.SelectedItem.Value;
            string deptcode = this.DeptFilter("");
            string dept = cbbdept.SelectedItem.Value;
            string balance = cbbType.SelectedItem.Value;
            DataTable dt = report_dal.GetAccountDeptZCFCount(date, dept, balance);

            Store1.DataSource = dt;
            Store1.DataBind();
            Session.Remove("Fj_Select");
            Session["Fj_Select"] = dt;

        }

        protected void OutExcel(object sender, EventArgs e)
        {
            if (Session["Fj_Select"] != null)
            {
                ExportData ex = new ExportData();
                DataTable dt = (DataTable)Session["Fj_Select"];

                ex.ExportToLocal(dt, this.Page, "xls", "明细");
                //MHeaderTabletoExcel(dt, null, null, null, 0);
                //ex.ExportToLocal(l_dt, this.Page, "xls", "人员信息");
            }
        }
    }
}