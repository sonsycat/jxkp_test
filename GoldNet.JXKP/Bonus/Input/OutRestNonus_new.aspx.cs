using System;
using Goldnet.Ext.Web;
using Goldnet.Dal;
using System.Data;
using GoldNet.Comm.ExportData;
using System.Collections.Generic;

namespace GoldNet.JXKP.Bonus.Input
{
    public partial class OutRestNonus_new : PageBase
    {
        /// <summary>
        /// 初始化处理
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
                //设置年度下拉列表
                SYear.DataSource = boundcomm.getYears();
                SYear.DataBind();
                cbbYear.Value = year;
                //设置月份下拉列表
                SMonth.DataSource = boundcomm.getMonth();
                SMonth.DataBind();
                cbbmonth.Value = month;


                if (month.Length == 1)
                {
                    month = "0" + month;
                }
                string benginDate = year + month + "01";

                Bindlist(benginDate);
            }
        }

        /// <summary>
        /// 查询操作处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Btn_Query_Click(object sender, EventArgs e)
        {
            Bindlist(GetBeginDate());
        }

        /// <summary>
        /// 数据刷新
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void Data_RefreshData(object sender, StoreRefreshDataEventArgs e)
        {
            //Bindlist();
        }

        /// <summary>
        /// 数据获取
        /// </summary>
        /// <param name="item_code"></param>
        /// <param name="date_time"></param>
        protected void Bindlist(string datetime)
        {
            OutRestNonusDal dal = new OutRestNonusDal();
            DataTable dt = dal.GetOutRestWeeks(datetime);

            this.GridPanel_Show.ExtColumnModel.HeadRows.Clear();

            this.GridPanel_Show.ExtColumnModel.Columns.Clear();

            //this.GridPanel_Show.Reconfigure();

            Goldnet.Ext.Web.ExtRows rows = new ExtRows();

            if (dt != null)
            {
                Column bonusColumnX = new Column();
                bonusColumnX.ColumnID = "DEPT_NAME";
                bonusColumnX.Header = "<div style='text-align:center;'>科室</div>";
                bonusColumnX.DataIndex = "DEPT_NAME";
                bonusColumnX.MenuDisabled = true;
                bonusColumnX.Tooltip = "科室";
                bonusColumnX.Align = Alignment.Left;
                bonusColumnX.Width = 120;
                bonusColumnX.Fixed = true;
                
                this.GridPanel_Show.ExtColumnModel.Columns.Add(bonusColumnX);

                Goldnet.Ext.Web.ExtRow row1 = new ExtRow();
                row1.Header = "";
                row1.ColSpan = 1;
                row1.Align = Alignment.Center;
                rows.Rows.Add(row1);

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    Column bonusColumn = new Column();
                    bonusColumn.ColumnID = dt.Rows[i]["ZIDUAN"].ToString() + "_1";
                    bonusColumn.Header = "<div style='text-align:center; font-weight:bold;'>正上</div>";
                    bonusColumn.DataIndex = dt.Rows[i]["ZIDUAN"].ToString() + "_1";
                    bonusColumn.MenuDisabled = true;
                    bonusColumn.Tooltip = "正常上午";
                    bonusColumn.Align = Alignment.Right;
                    bonusColumn.Width = 70;
                    bonusColumn.Renderer.Fn = "rmbMoney";
                    //录入文本控件
                    Goldnet.Ext.Web.NumberField luru0 = new NumberField();
                    luru0.ID = "aaa0"+i.ToString();
                    bonusColumn.Editor.Add(luru0);
                    this.GridPanel_Show.ExtColumnModel.Columns.Add(bonusColumn);

                    Column bonusColumn1 = new Column();
                    bonusColumn1.ColumnID = dt.Rows[i]["ZIDUAN"].ToString() + "_2";
                    bonusColumn1.Header = "<div style='text-align:center;font-weight:bold;'>正下</div>";
                    bonusColumn1.DataIndex = dt.Rows[i]["ZIDUAN"].ToString() + "_2";
                    bonusColumn1.MenuDisabled = true;
                    bonusColumn1.Tooltip = "正常下午";
                    bonusColumn1.Align = Alignment.Right;
                    bonusColumn1.Width = 70;
                    bonusColumn1.Renderer.Fn = "rmbMoney";
                    //录入文本控件
                    Goldnet.Ext.Web.NumberField luru1 = new NumberField();
                    luru1.ID = "aaa1"+i.ToString();
                    bonusColumn1.Editor.Add(luru1);
                    this.GridPanel_Show.ExtColumnModel.Columns.Add(bonusColumn1);

                    Column bonusColumn2 = new Column();
                    bonusColumn2.ColumnID = dt.Rows[i]["ZIDUAN"].ToString() + "_3";
                    bonusColumn2.Header = "<div style='text-align:center;'>临上</div>";
                    bonusColumn2.DataIndex = dt.Rows[i]["ZIDUAN"].ToString() + "_3";
                    bonusColumn2.MenuDisabled = true;
                    bonusColumn2.Tooltip = "临时上午";
                    bonusColumn2.Align = Alignment.Right;
                    bonusColumn2.Width = 70;
                    bonusColumn2.Renderer.Fn = "rmbMoney";
                    //录入文本控件
                    Goldnet.Ext.Web.NumberField luru2 = new NumberField();
                    luru2.ID = "aaa2" + i.ToString();
                    bonusColumn2.Editor.Add(luru2);
                    this.GridPanel_Show.ExtColumnModel.Columns.Add(bonusColumn2);

                    Column bonusColumn3 = new Column();
                    bonusColumn3.ColumnID = dt.Rows[i]["ZIDUAN"].ToString() + "_4";
                    bonusColumn3.Header = "<div style='text-align:center;'>临下</div>";
                    bonusColumn3.DataIndex = dt.Rows[i]["ZIDUAN"].ToString() + "_4";
                    bonusColumn3.MenuDisabled = true;
                    bonusColumn3.Tooltip = "临时下午";
                    bonusColumn3.Align = Alignment.Right;
                    bonusColumn3.Width = 70;
                    bonusColumn3.Renderer.Fn = "rmbMoney";
                    //录入文本控件
                    Goldnet.Ext.Web.NumberField luru3 = new NumberField();
                    luru3.ID = "aaa3" + i.ToString();
                    bonusColumn3.Editor.Add(luru3);
                    this.GridPanel_Show.ExtColumnModel.Columns.Add(bonusColumn3);


                    Goldnet.Ext.Web.ExtRow row2 = new ExtRow();
                    row2.Header = dt.Rows[i]["RIQI"].ToString()+"("+dt.Rows[i]["WEEKS"].ToString()+")";
                    row2.ColSpan = 4;
                    row2.Align = Alignment.Center;
                    rows.Rows.Add(row2);
                }

                Column bonusColumn4 = new Column();
                bonusColumn4.ColumnID = "OUT_TOTAL";
                bonusColumn4.Header = "<div style='text-align:center;'>合计</div>";
                bonusColumn4.DataIndex = "OUT_TOTAL";
                bonusColumn4.MenuDisabled = true;
                bonusColumn4.Tooltip = "合计";
                bonusColumn4.Align = Alignment.Right;
                bonusColumn4.Width = 70;
                this.GridPanel_Show.ExtColumnModel.Columns.Add(bonusColumn4);

                Goldnet.Ext.Web.ExtRow row3 = new ExtRow();
                row3.Header = "";
                row3.ColSpan = 1;
                row3.Align = Alignment.Center;
                rows.Rows.Add(row3);
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

            this.GridPanel_Show.ExtColumnModel.HeadRows.Add(rows);

            //获取双休日固定岗位补贴
            DataTable dt1 = dal.GetOutRestNonusAccount(datetime);

            this.SReport.DataSource = dt1;
            this.SReport.DataBind();
        }

        /// <summary>
        /// 保存处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Button_Save_click(object sender, AjaxEventArgs e)
        {
            Dictionary<string, string>[] selectRow = GetSelectRow(e);
            if (selectRow != null)
            {
                OutRestNonusDal dal = new OutRestNonusDal();
                string date_time = GetBeginDate();

                try
                {
                    dal.SaveOutRestNonusAccount(selectRow, date_time);

                    this.ShowMessage("信息提示", "保存成功！");
                    Data_RefreshData(null, null);

                }
                catch (Exception ex)
                {
                    this.ShowDataError(ex.Message.ToString(), Request.Path, "Button_create_click");
                }
            }
        }

        /// <summary>
        /// EXCEL导出处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void OutExcel(object sender, EventArgs e)
        {
            //if (Session["OutRestNonusAccount"] != null)
            //{
            //    ExportData ex = new ExportData();
            //    DataTable dt = (DataTable)Session["OutRestNonusAccount"];

            //    for (int i = 0; i < dt.Columns.Count; i++)
            //    {
            //        if (dt.Columns[i].ColumnName == "ST_DATE")
            //        {
            //            dt.Columns[i].ColumnName = "时间";
            //        }
            //        else if (dt.Columns[i].ColumnName == "DEPT_NAME")
            //        {
            //            dt.Columns[i].ColumnName = "科室";
            //        }
            //        else if (dt.Columns[i].ColumnName == "FORMAL_AM")
            //        {
            //            dt.Columns[i].ColumnName = "正式上午";
            //        }
            //        else if (dt.Columns[i].ColumnName == "FORMAL_PM")
            //        {
            //            dt.Columns[i].ColumnName = "正式下午";
            //        }
            //        else if (dt.Columns[i].ColumnName == "TEMPORARY_AM")
            //        {
            //            dt.Columns[i].ColumnName = "临时上午";
            //        }
            //        else if (dt.Columns[i].ColumnName == "TEMPORARY_PM")
            //        {
            //            dt.Columns[i].ColumnName = "临时下午";
            //        }
            //        else if (dt.Columns[i].ColumnName == "SUMNUB")
            //        {
            //            dt.Columns[i].ColumnName = "合计";
            //        }
            //    }
            //    string dates = this.cbbYear.SelectedItem.Value + "年" + this.cbbmonth.SelectedItem.Value + "月";// +this.ccbYearTo.SelectedItem.Value + "年" + this.ccbMonthTo.SelectedItem.Value + "月";
            //    ex.ExportToLocal(dt, this.Page, "xls", "双休日门诊固定岗位补贴表(" + dates + ")");
            //}
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
            string benginDate = year + month + "01";
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
