using System;
using System.Data;
using System.Collections.Generic;
using Goldnet.Ext.Web;
using Goldnet.Dal;
using GoldNet.Comm.ExportData;

namespace GoldNet.JXKP.RLZY.BaseInfoMaintain
{
    public partial class StaffWorkday_KSCQ_report : PageBase
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
                string year = DateTime.Now.AddMonths(-1).Year.ToString();
                string month = DateTime.Now.AddMonths(-1).Month.ToString();
                BoundComm boundcomm = new BoundComm();

                SYear.DataSource = boundcomm.getYears();
                SYear.DataBind();
                cbbYear.Value = year;
                ComboBox1.Value = year;

                SMonth.DataSource = boundcomm.getMonth();
                SMonth.DataBind();
                cbbmonth.Value = month;
                ComboBox2.Value = month;

                string mon = month;
                if (mon.Length == 1)
                {
                    mon = "0" + mon;
                }
                string benginDate = year + "" + mon;

                data(benginDate, benginDate);
            }
        }

        /// <summary>
        /// 查询数据并绑定
        /// </summary>
        /// <param name="stdate"></param>
        /// <param name="enddate"></param>
        private void data(string stdate, string enddate)
        {
            BaseInfoMaintainDal tdal = new BaseInfoMaintainDal();
            //科室权限
            string deptcode = this.DeptFilter("");

            DataTable table = tdal.SearchKSCQ(stdate, deptcode, enddate).Tables[0];
            if (table != null)
            {
                for (int i = 0; i < table.Columns.Count; i++)
                {
                    //定义字段
                    RecordField record = new RecordField();
                    record = new RecordField(table.Columns[i].ColumnName, RecordFieldType.String);
                    this.Store1.AddField(record);
                    //定义列
                    Column cl = new Column();
                    cl.Header = table.Columns[i].ColumnName;
                    cl.Sortable = false;
                    cl.Align = Alignment.Right;
                    cl.MenuDisabled = true;
                    cl.DataIndex = table.Columns[i].ColumnName;
                    //TextField fils = new TextField();
                    //fils.ReadOnly = true;
                    //fils.ID = i.ToString();
                    //fils.SelectOnFocus = false;
                    ////fils.DecimalPrecision = 2;
                    //cl.Editor.Add(fils);
                    if (cl.Header.Equals("ACCOUNT_DEPT_CODE"))
                    {
                        cl.Hidden = true;
                    }
                    else if (cl.Header.Equals("科室") )
                    {
                        cl.Align = Alignment.Left;
                    }
                    else
                    {
                        //cl.Renderer.Fn = "rmbMoney";
                    }

                    this.GridPanel2.ColumnModel.Columns.Add(cl);
                }

                Store1.DataSource = table;
                Store1.DataBind();
                Session.Remove("StaffWorkday_KSCQ_report");
                Session["StaffWorkday_KSCQ_report"] = table;
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
            data(GetBeginDate(), GetEndDate());
        }

        /// <summary>
        /// EXCEL导出
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void OutExcel(object sender, EventArgs e)
        {

            if (Session["StaffWorkday_KSCQ_report"] != null)
            {
                ExportData ex = new ExportData();
                DataTable dt = (DataTable)Session["StaffWorkday_KSCQ_report"];

                //for (int i = 0; i < dt.Columns.Count; i++)
                //{
                //    if (dt.Columns[i].ColumnName == "DEPT_NAME")
                //    {
                //        dt.Columns[i].ColumnName = "科室";
                //    }
                //    else if (dt.Columns[i].ColumnName == "YCQTS")
                //    {
                //        dt.Columns[i].ColumnName = "应出勤天数";
                //    }
                //    else if (dt.Columns[i].ColumnName == "SJCQTS")
                //    {
                //        dt.Columns[i].ColumnName = "实际出勤";
                //    }
                //    else if (dt.Columns[i].ColumnName == "QJHZ")
                //    {
                //        dt.Columns[i].ColumnName = "抢救会诊";
                //    }
                //    else if (dt.Columns[i].ColumnName == "JX")
                //    {
                //        dt.Columns[i].ColumnName = "讲学";
                //    }
                //    else if (dt.Columns[i].ColumnName == "XSHD")
                //    {
                //        dt.Columns[i].ColumnName = "学术活动";
                //    }
                //    else if (dt.Columns[i].ColumnName == "XXJX")
                //    {
                //        dt.Columns[i].ColumnName = "学习进修";
                //    }
                //    else if (dt.Columns[i].ColumnName == "XHYL")
                //    {
                //        dt.Columns[i].ColumnName = "巡回医疗";
                //    }
                //    else if (dt.Columns[i].ColumnName == "QTRW")
                //    {
                //        dt.Columns[i].ColumnName = "其它任务";
                //    }
                //    else if (dt.Columns[i].ColumnName == "XJ")
                //    {
                //        dt.Columns[i].ColumnName = "休假";
                //    }
                //    else if (dt.Columns[i].ColumnName == "TQJ")
                //    {
                //        dt.Columns[i].ColumnName = "探亲假";
                //    }
                //    else if (dt.Columns[i].ColumnName == "CJ")
                //    {
                //        dt.Columns[i].ColumnName = "产假";
                //    }
                //    else if (dt.Columns[i].ColumnName == "SJ")
                //    {
                //        dt.Columns[i].ColumnName = "事假";
                //    }
                //    else if (dt.Columns[i].ColumnName == "BJ")
                //    {
                //        dt.Columns[i].ColumnName = "病假";
                //    }
                //    else if (dt.Columns[i].ColumnName == "WGQQ")
                //    {
                //        dt.Columns[i].ColumnName = "无故缺勤";
                //    }
                //}
                ex.ExportToLocal(dt, this.Page, "xls", "科室出勤情况统计");
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
            string benginDate = year + "" + month;
            return benginDate;
        }

        private string GetEndDate()
        {
            string year = ComboBox1.SelectedItem.Value.ToString();
            string month = ComboBox2.SelectedItem.Value.ToString();
            if (month.Length == 1)
            {
                month = "0" + month;
            }
            string benginDate = year + "" + month;
            return benginDate;
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
