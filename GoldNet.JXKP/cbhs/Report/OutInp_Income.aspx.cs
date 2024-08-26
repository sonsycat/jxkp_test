using System;
using Goldnet.Ext.Web;
using Goldnet.Dal;
using System.Data;
using GoldNet.Comm.ExportData;


namespace GoldNet.JXKP.cbhs.Report
{
    public partial class OutInp_Income : PageBase
    {
        private Goldnet.Dal.Report report_dal = new Goldnet.Dal.Report();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Ext.IsAjaxRequest)
            {
                string year = DateTime.Now.AddMonths(-1).Year.ToString();
                string month = DateTime.Now.AddMonths(-1).Month.ToString();
                BoundComm boundcomm = new BoundComm();
                SYear.DataSource = boundcomm.getYears();
                SYear.DataBind();
                cbbYear.Value = year;
                ccbYearTo.Value = year;
                SMonth.DataSource = boundcomm.getMonth();
                SMonth.DataBind();
                cbbmonth.Value = month;
                ccbMonthTo.Value = month;
                
                cbbType.SelectedItem.Value = "0";
                //cbbType.SelectedItem.Text = "收付实现";

               
            }
        }

        

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Btn_Query_Click(object sender, AjaxEventArgs e)
        {
            string balance = cbbType.SelectedItem.Value;
            string deptcode = this.DeptFilter("");
            string dept = "";


            DataTable dt = report_dal.GetOutInp_Income(GetBeginDate(), GetEndDate(), balance, dept, incomestype.SelectedItem.Value);
            if (dt != null)
            {
                SReport.DataSource = dt;
                SReport.DataBind();
                Session.Remove("OutInp_Income");
                Session["OutInp_Income"] = dt;
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
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void OutExcel(object sender, EventArgs e)
        {
            if (Session["OutInp_Income"] != null)
            {
                ExportData ex = new ExportData();
                DataTable dt = (DataTable)Session["OutInp_Income"];

                

                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    if (dt.Columns[i].ColumnName == "DEPT_NAME")
                    {
                        dt.Columns[i].ColumnName = "科室";
                    }
                    else if (dt.Columns[i].ColumnName == "ZL")
                    {
                        dt.Columns[i].ColumnName = "治疗费";
                    }
                    else if (dt.Columns[i].ColumnName == "GYY")
                    {
                        dt.Columns[i].ColumnName = "高压氧";
                    }
                    else if (dt.Columns[i].ColumnName == "YQ")
                    {
                        dt.Columns[i].ColumnName = "氧气费";
                    }
                    else if (dt.Columns[i].ColumnName == "HJ")
                    {
                        dt.Columns[i].ColumnName = "合计";
                    }
                    else if (dt.Columns[i].ColumnName == "ICUZL")
                    {
                        dt.Columns[i].ColumnName = "ICU治疗";
                    }
                    else if (dt.Columns[i].ColumnName == "LCUYQ")
                    {
                        dt.Columns[i].ColumnName = "ICU氧气";
                    }
                    else if (dt.Columns[i].ColumnName == "ICUHJ")
                    {
                        dt.Columns[i].ColumnName = "合计1";
                    }
                    else if (dt.Columns[i].ColumnName == "SSF")
                    {
                        dt.Columns[i].ColumnName = "手术费";
                    }
                    else if (dt.Columns[i].ColumnName == "SSYQ")
                    {
                        dt.Columns[i].ColumnName = "手术氧气";
                    }
                    else if (dt.Columns[i].ColumnName == "SSHJ")
                    {
                        dt.Columns[i].ColumnName = "合计2";
                    }
                    else if (dt.Columns[i].ColumnName == "FSF")
                    {
                        dt.Columns[i].ColumnName = "放射";
                    }
                    else if (dt.Columns[i].ColumnName == "CTF")
                    {
                        dt.Columns[i].ColumnName = "CT费";
                    }
                    else if (dt.Columns[i].ColumnName == "DZF")
                    {
                        dt.Columns[i].ColumnName = "电诊费";
                    }
                    else if (dt.Columns[i].ColumnName == "JYF")
                    {
                        dt.Columns[i].ColumnName = "检验费";
                    }
                    else if (dt.Columns[i].ColumnName == "QJF")
                    {
                        dt.Columns[i].ColumnName = "腔镜费";
                    }
                    else if (dt.Columns[i].ColumnName == "BLF")
                    {
                        dt.Columns[i].ColumnName = "病理费";
                    }
                    else if (dt.Columns[i].ColumnName == "FCDJQBHJ")
                    {
                        dt.Columns[i].ColumnName = "合计3";
                    }
                    else if (dt.Columns[i].ColumnName == "Y")
                    {
                        dt.Columns[i].ColumnName = "药费";
                    }
                    else if (dt.Columns[i].ColumnName == "WC")
                    {
                        dt.Columns[i].ColumnName = "卫材费";
                    }
                    else if (dt.Columns[i].ColumnName == "XF")
                    {
                        dt.Columns[i].ColumnName = "血费";
                    }
                    else if (dt.Columns[i].ColumnName == "ZJ")
                    {
                        dt.Columns[i].ColumnName = "总计";
                    }
                    
                }
                string dates = this.cbbYear.SelectedItem.Value + "年" + this.cbbmonth.SelectedItem.Value + "月-" + this.ccbYearTo.SelectedItem.Value + "年" + this.ccbMonthTo.SelectedItem.Value + "月";
                ex.ExportToLocal(dt, this.Page, "xls", "门诊及住院收入报表(" + dates + ")");
            }
        }

        /// <summary>
        /// 开始时间
        /// </summary>
        /// <returns></returns>
        private string GetBeginDate()
        {
            string year = cbbYear.SelectedItem.Value.ToString();
            string month = cbbmonth.SelectedItem.Value.ToString();
            if (month.Length == 1)
            {
                month = "0" + month;
            }
            string benginDate = year + month;
            return benginDate;
        }

        /// <summary>
        /// 结束时间
        /// </summary>
        /// <returns></returns>
        private string GetEndDate()
        {
            string year = ccbYearTo.SelectedItem.Value.ToString();
            string month = ccbMonthTo.SelectedItem.Value.ToString();
            if (month.Length == 1)
            {
                month = "0" + month;
            }
            string endDate = year + month;
            return endDate;
        }









    }
}