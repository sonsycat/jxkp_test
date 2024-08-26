using System;
using Goldnet.Ext.Web;
using Goldnet.Dal;
using System.Data;
using GoldNet.Comm.ExportData;
using Goldnet.Comm.ExportData;

namespace GoldNet.JXKP
{
    public partial class Patient_Report : PageBase
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
                this.stardate.Value = System.DateTime.Now.ToString("yyyy-MM") + "-01";
                this.enddate.Value = System.DateTime.Now.ToString("yyyy-MM-dd");
                HttpProxy pro = new HttpProxy();
                pro.Method = HttpMethod.POST;
                //pro.Url = "WebService/AccountDepts.ashx?deptfilter=" + this.DeptFilter("dept_code");
                pro.Url = "../../../WebService/AccountDepts.ashx?deptfilter=" + this.DeptFilter("dept_code");
                this.Store2.Proxy.Add(pro);
                //data(System.DateTime.Now.ToString("yyyy-MM") + "-01", System.DateTime.Now.ToString("yyyy-MM-dd"));
                data("2000-01-01", "2000-01-01");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Btn_Query_Click(object sender, AjaxEventArgs e)
        {
            string stardate = Convert.ToDateTime(this.stardate.SelectedValue).ToString("yyyy-MM-dd");
            string enddate = Convert.ToDateTime(this.enddate.SelectedValue).ToString("yyyy-MM-dd");
            data(stardate, enddate);
            Session["pstdate"] = stardate;
            Session["peddate"] = enddate;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="stardate"></param>
        /// <param name="enddate"></param>
        private void data(string stardate, string enddate)
        {
            if (this.ComAccountdeptcode.SelectedItem.Value.ToString() == "" && stardate != "2000-01-01")
            {
                this.ShowMessage("提示", "请选择科室");
            }
            else
            {
                Report report_dal = new Report();

                string depttype = this.deptType.SelectedItem.Value.ToString();
                DataTable table = report_dal.GetPatientDetail(stardate, enddate, depttype, ComAccountdeptcode.SelectedItem.Value.ToString());

                for (int i = 0; i < table.Columns.Count; i++)
                {
                    RecordField record = new RecordField();
                    record = new RecordField(table.Columns[i].ColumnName, RecordFieldType.String);
                    this.Store1.AddField(record);
                    Column cl = new Column();
                    cl.Header = table.Columns[i].ColumnName;
                    cl.Sortable = false;
                    cl.MenuDisabled = true;
                    cl.DataIndex = table.Columns[i].ColumnName;
                    TextField fils = new TextField();
                    fils.ReadOnly = true;

                    fils.ID = i.ToString();
                    fils.SelectOnFocus = false;
                    //fils.DecimalPrecision = 2;
                    cl.Editor.Add(fils);
                    //if (cl.Header.Equals("PATIENT_ID") || cl.Header.Equals("DEPT_CODE"))
                    //{
                    //    cl.Hidden = true;
                    //}
                    if (cl.Header.Equals("DEPT_CODE") || cl.Header.Equals("DEPT_NAME"))
                    {
                        cl.Align = Alignment.Right;
                    }
                    else
                    {
                        cl.Renderer.Fn = "rmbMoney";
                    }


                    this.GridPanel2.ColumnModel.Columns.Add(cl);
                }

                this.Store1.DataSource = table;
                this.Store1.DataBind();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void OutExcel(object sender, EventArgs e)
        {
            Report report_dal = new Report();
            string stardate = Session["pstdate"].ToString();
            string enddate = Session["peddate"].ToString();
            string depttype = this.deptType.SelectedItem.Value.ToString();
            DataTable dt = report_dal.GetPatientDetail(stardate, enddate, depttype, ComAccountdeptcode.SelectedItem.Value.ToString());
            ExportData ex = new ExportData();
            // DataTable dt = (DataTable)Session["patientreport"];
            for (int i = 0; i < dt.Columns.Count; i++)
            {
                if (dt.Columns[i].ColumnName == "*")
                {
                    dt.Columns[i].ColumnName = "未知项目";
                }
                else if (dt.Columns[i].ColumnName == "PATIENT_ID")
                {
                    dt.Columns[i].ColumnName = "病人id";
                }
            }
            this.outexcel(dt, "病人收入统计表");

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void DbRowClick(object sender, AjaxEventArgs e)
        {
            RowSelectionModel sm = this.GridPanel2.SelectionModel.Primary as RowSelectionModel;
            if (sm.SelectedRows.Count < 1)
            {
                this.ShowMessage("提示", "请选择一条记录！");
            }
            else
            {
                string patientid = sm.SelectedRow.RecordID;
                string stardate = Convert.ToDateTime(this.stardate.SelectedValue).ToString("yyyy-MM-dd");
                string enddate = Convert.ToDateTime(this.enddate.SelectedValue).ToString("yyyy-MM-dd");


                string deptname = deptType.SelectedItem.Value == "1" ? "INP_BILL_DETAIL" : "OUTP_BILL_ITEMS";


                LoadConfig loadcfg = getLoadConfig("Doctor_Report_Detail.aspx");

                loadcfg.Params.Add(new Goldnet.Ext.Web.Parameter("stardate", stardate));//开始时间
                loadcfg.Params.Add(new Goldnet.Ext.Web.Parameter("enddate", enddate));//结束时间
                loadcfg.Params.Add(new Goldnet.Ext.Web.Parameter("tablename", deptname));//表
                loadcfg.Params.Add(new Goldnet.Ext.Web.Parameter("doctorname", ""));//
                loadcfg.Params.Add(new Goldnet.Ext.Web.Parameter("patientid", patientid));//
                loadcfg.Params.Add(new Goldnet.Ext.Web.Parameter("accountdeptcode", this.ComAccountdeptcode.SelectedItem.Value));//

                showCenterSet(this.Doctor_Detail, loadcfg);
            }
        }

        protected void Btn_Excel_Click(object sender, AjaxEventArgs e)
        {
            string file = "..\\..\\resources\\ExportDataTemp\\zl.xls";
            TestExcelRead(file);
        }

        private void TestExcelRead(string file)
        {
            try
            {
                using (ExcelHelper excelHelper = new ExcelHelper(file))
                {
                    DataTable table = excelHelper.ExcelToDataTable("MySheet", true);
                   // PrintData(dt);

                    for (int i = 0; i < table.Columns.Count; i++)
                    {
                        RecordField record = new RecordField();
                        record = new RecordField(table.Columns[i].ColumnName, RecordFieldType.String);
                        this.Store1.AddField(record);
                        Column cl = new Column();
                        cl.Header = table.Columns[i].ColumnName;
                        cl.Sortable = false;
                        cl.MenuDisabled = true;
                        cl.DataIndex = table.Columns[i].ColumnName;
                        TextField fils = new TextField();
                        fils.ReadOnly = true;

                        fils.ID = i.ToString();
                        fils.SelectOnFocus = false;
                        //fils.DecimalPrecision = 2;
                        cl.Editor.Add(fils);
                        //if (cl.Header.Equals("PATIENT_ID") || cl.Header.Equals("DEPT_CODE"))
                        //{
                        //    cl.Hidden = true;
                        //}
                        if (cl.Header.Equals("DEPT_CODE") || cl.Header.Equals("DEPT_NAME"))
                        {
                            cl.Align = Alignment.Right;
                        }
                        else
                        {
                            cl.Renderer.Fn = "rmbMoney";
                        }


                        this.GridPanel2.ColumnModel.Columns.Add(cl);
                    }

                    this.Store1.DataSource = table;
                    this.Store1.DataBind();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: " + ex.Message);
            }
        }


    }
}
