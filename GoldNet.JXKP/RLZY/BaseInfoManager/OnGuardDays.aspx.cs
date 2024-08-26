using System;
using System.Collections;
using System.Configuration;
using System.Data;
using Goldnet.Dal;
using Goldnet.Ext.Web;
using GoldNet.Comm.ExportData;

namespace GoldNet.JXKP.RLZY.BaseInfoManager
{
    public partial class OnGuardDays : PageBase
    {
        BaseInfoMaintainDal dal = new BaseInfoMaintainDal();

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            //检查是否已经登录，否则停止
            if (Session["CURRENTSTAFF"] == null)
            {
                Response.End();
            }
            if (!Ext.IsAjaxRequest)
            {
                //this.NumYear.Text = DateTime.Now.Year.ToString();
                //for (int i = 1; i <= 12; i++)
                //{
                //    this.Comb_StartMonth.Items.Add(new Goldnet.Ext.Web.ListItem(i.ToString(), i.ToString()));
                //}
                //this.Comb_StartMonth.SelectedIndex = DateTime.Now.Month - 1;

                string year = DateTime.Now.AddMonths(-1).Year.ToString();
                string month = DateTime.Now.AddMonths(-1).Month.ToString();
                BoundComm boundcomm = new BoundComm();

                SYear.DataSource = boundcomm.getYears();
                SYear.DataBind();
                NumYear.Value = year;
                EndYear.Value = year;

                SMonth.DataSource = boundcomm.getMonth();
                SMonth.DataBind();
                Comb_StartMonth.Value = month;
                EndMonth.Value = month;

                //人员类别
                DataTable l_dt = PersTypeFilter();
                if (l_dt.Rows.Count > 0)
                {
                    cboPersonType.Items.Add(new Goldnet.Ext.Web.ListItem("全部", "全部"));
                }
                for (int i = 0; i < l_dt.Rows.Count; i++)
                {
                    this.cboPersonType.Items.Add(new Goldnet.Ext.Web.ListItem(l_dt.Rows[i]["NAME"].ToString(), l_dt.Rows[i]["NAME"].ToString()));
                }
                if (l_dt.Rows.Count > 0)
                {
                    cboPersonType.SelectedIndex = 0;
                }

                //科室
                string deptcode = this.DeptFilter("");

                //科室下拉列表初始化
                HttpProxy pro2 = new HttpProxy();
                pro2.Method = HttpMethod.POST;
                pro2.Url = "/RLZY/WebService/DeptDict.ashx?deptfilter=" + deptcode;
                this.Store2.Proxy.Add(pro2);

                //int year = Convert.ToInt32(DateTime.Now.Year.ToString());
                //int month = Convert.ToInt32(DateTime.Now.Month - 1);
                //string deptcode = "";
                data(Convert.ToInt32(year), Convert.ToInt32(month), Convert.ToInt32(year), Convert.ToInt32(month), "");
            }
        }

        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Btn_Query_Click(object sender, AjaxEventArgs e)
        {
            int year = Convert.ToInt32(this.NumYear.SelectedItem.Value);
            int month = Convert.ToInt32(this.Comb_StartMonth.SelectedItem.Value);

            int endyear = Convert.ToInt32(this.EndYear.SelectedItem.Value);
            int endmonth = Convert.ToInt32(this.EndMonth.SelectedItem.Value);

            string deptcode = this.cbbdept.SelectedItem.Value.ToString();
            data(year, month, endyear, endmonth, deptcode);
        }

        /// <summary>
        /// 刷新数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void data(int year, int month, int endyear, int endmonth, string deptcode)
        {
            if (year > DateTime.Now.Year || (year == DateTime.Now.Year && month >= DateTime.Now.Month))
            {
                Goldnet.Ext.Web.Ext.Msg.Show(new Goldnet.Ext.Web.MessageBox.Config
                {
                    Title = "信息提示",
                    Message = "请选择当前时间之前的年份或月份",
                    Buttons = MessageBox.Button.OK,
                    Icon = (MessageBox.Icon)Enum.Parse(typeof(MessageBox.Icon), "INFO")
                });
                return;
            }

            string sort = "";
            if (this.cboPersonType.SelectedItem.Text == "" || this.cboPersonType.SelectedItem.Text == "全部")
            {
                DataTable l_dt = PersTypeFilter();
                for (int i = 0; i < l_dt.Rows.Count; i++)
                {
                    sort = sort + "'" + l_dt.Rows[i]["NAME"] + "',";
                }
                if (l_dt.Rows.Count == 0)
                {
                    sort = "'-1'";
                }
            }
            else
            {
                sort = "'" + this.cboPersonType.SelectedItem.Text + "'";
            }
            string days = DateTime.DaysInMonth(year, month).ToString();
            //string fromDate = Convert.ToString(year) + "-" + Convert.ToString(month) + "-01";
            //string ToDate = Convert.ToString(year) + "-" + Convert.ToString(month) + "-" + days;
            //string datetime = GetBeginDate();

            string mon = Convert.ToString(month);
            string endmon = Convert.ToString(endmonth);
            if (mon.Length == 1)
            {
                mon = "0" + mon;
            }
            if (endmon.Length == 1)
            {
                endmon = "0" + endmon;
            }
            string datetime = Convert.ToString(year) + "" + mon;
            string enddatetime = Convert.ToString(endyear) + "" + endmon;

            DataTable table = dal.ViewOnGuardDays2(datetime, enddatetime,sort.TrimEnd(new char[] { ',' }), days, this.DeptFilter(""), "", deptcode).Tables[0];

            DataRow dr = table.NewRow();
            dr["科室"] = "合计";

            for (int i = 0; i < table.Columns.Count; i++)
            {
                //定义字段
                RecordField record = new RecordField();
                record = new RecordField(table.Columns[i].ColumnName, RecordFieldType.String);
                this.Store1.AddField(record);
                //定义列
                Column cl = new Column();
                cl.Header = table.Columns[i].ColumnName;
                cl.Sortable = true;
                cl.Align = Alignment.Right;
                cl.MenuDisabled = true;
                cl.DataIndex = table.Columns[i].ColumnName;

                if (cl.Header.Equals("STAFF_ID"))
                {
                    cl.Hidden = true;
                }
                else if (cl.Header.Equals("科室") || cl.Header.Equals("姓名") || cl.Header.Equals("行政职务") || cl.Header.Equals("类别") || cl.Header.Equals("技术职务") || cl.Header.Equals("职称序列"))
                {
                    cl.Align = Alignment.Left;
                }
                else
                {
                }

                this.GridPanel_Show.ColumnModel.Columns.Add(cl);
                if (!cl.Header.Equals("科室") && !cl.Header.Equals("姓名") && !cl.Header.Equals("行政职务") && !cl.Header.Equals("类别") && !cl.Header.Equals("STAFF_ID") && !cl.Header.Equals("技术职务") && !cl.Header.Equals("职称序列"))
                {
                    string aa = table.Columns[i].ColumnName;
                    string bb = "Sum(" + aa + ")";
                    dr[aa] = table.Compute(bb, "");
                }

                if (cl.Header.Equals("姓名"))
                {
                    string aa = table.Columns[i].ColumnName;
                    string bb = "count(" + aa + ")";
                    dr[aa] = table.Compute(bb, "").ToString() + "人";
                }
            }

            table.Rows.Add(dr);
            this.Store1.DataSource = table;
            this.Store1.DataBind();

            Session.Remove("OnGuardDays");
            Session["OnGuardDays"] = table;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void SearchDetail(object sender, AjaxEventArgs e)
        {
            //string name = e.ExtraParams["name"].ToString().Replace("\"", "");
            //string deptCode = e.ExtraParams["deptCode"].ToString().Replace("\"", "");
            //string months = this.Comb_StartMonth.SelectedItem.Value.Length == 1 ? "0" + this.Comb_StartMonth.SelectedItem.Value : this.Comb_StartMonth.SelectedItem.Value;
            //string days = DateTime.DaysInMonth(Convert.ToInt32(this.NumYear.Text), Convert.ToInt32(this.Comb_StartMonth.SelectedItem.Value)).ToString();
            //string fromDate = this.NumYear.Text + "-" + months + "-01";
            //string ToDate = this.NumYear.Text + "-" + months + "-" + days;
            //this.Store.DataSource = dal.ViewOnGuardDetail(fromDate, ToDate, deptCode, name);
            //this.Store.DataBind();
        }

        /// <summary>
        /// 获取开始时间
        /// </summary>
        /// <returns></returns>
        private string GetBeginDate()
        {
            string year = NumYear.SelectedItem.Value.ToString();
            string month = Comb_StartMonth.SelectedItem.Value.ToString();
            if (month.Length == 1)
            {
                month = "0" + month;
            }
            string benginDate = year + "" + month;
            return benginDate;
        }

        /// <summary>
        /// EXCEL导出
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void OutExcel(object sender, EventArgs e)
        {
            int year = Convert.ToInt32(this.NumYear.SelectedItem.Value);
            int month = Convert.ToInt32(this.Comb_StartMonth.SelectedItem.Value);
            int endyear = Convert.ToInt32(this.EndYear.SelectedItem.Value);
            int endmonth = Convert.ToInt32(this.EndMonth.SelectedItem.Value);
            string deptcode = this.cbbdept.SelectedItem.Value.ToString();

            string sort = "";
            if (this.cboPersonType.SelectedItem.Text == "" || this.cboPersonType.SelectedItem.Text == "全部")
            {
                DataTable l_dt = PersTypeFilter();
                for (int i = 0; i < l_dt.Rows.Count; i++)
                {
                    sort = sort + "'" + l_dt.Rows[i]["NAME"] + "',";
                }
                if (l_dt.Rows.Count == 0)
                {
                    sort = "'-1'";
                }
            }
            else
            {
                sort = "'" + this.cboPersonType.SelectedItem.Text + "'";
            }
            string days = DateTime.DaysInMonth(year, month).ToString();
            //string fromDate = Convert.ToString(year) + "-" + Convert.ToString(month) + "-01";
            //string ToDate = Convert.ToString(year) + "-" + Convert.ToString(month) + "-" + days;
            //string datetime = GetBeginDate();

            string mon = Convert.ToString(month);
            string endmon = Convert.ToString(endmonth);
            if (mon.Length == 1)
            {
                mon = "0" + mon;
            }
            if (endmon.Length == 1)
            {
                endmon = "0" + endmon;
            }
            string datetime = Convert.ToString(year) + "" + mon;
            string enddatetime = Convert.ToString(endyear) + "" + endmon;

            DataTable dt = dal.ViewOnGuardDays2(datetime, enddatetime,sort.TrimEnd(new char[] { ',' }), days, this.DeptFilter(""), "", deptcode).Tables[0];

            //if (Session["OnGuardDays"] != null)
            //{
            ExportData ex = new ExportData();
            //DataTable dt = (DataTable)Session["OnGuardDays"];

            ex.ExportToLocal(dt, this.Page, "xls", "月在岗天数");
            //}
        }

    }
}
