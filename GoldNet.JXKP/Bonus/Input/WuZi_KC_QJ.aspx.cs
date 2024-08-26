using System;
using System.Data;
using Goldnet.Ext.Web;
using Goldnet.Dal.Properties.Bound;
using System.Collections.Generic;
using GoldNet.Comm.ExportData;
using GoldNet.Model;

namespace GoldNet.JXKP.Bonus.Input
{
    public partial class WuZi_KC_QJ : System.Web.UI.Page
    {
        OperationDal dal = new OperationDal();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Ext.IsAjaxRequest)
            {
                Goldnet.Dal.SYS_DEPT_DICT dal = new Goldnet.Dal.SYS_DEPT_DICT();
                string user_id = ((User)Session["CURRENTSTAFF"]).UserId;

                DataRow[] deptrolerow1 = dal.GetAccoutDeptCode111(user_id).Tables[0].Select();

                foreach (DataRow roww in deptrolerow1)
                {
                    this.dept_SelectDept.Items.Add(new Goldnet.Ext.Web.ListItem(roww["dept_name"].ToString(), roww["dept_name"].ToString()));
                }


            }


        }

        private void BindDate(string date, string end, string dept_code)
        {
            DataTable dt = dal.GetWZ_KC_QJ(date, end,dept_code);
            Store1.DataSource = dt;
            Store1.DataBind();
            Session.Remove("WuZi_KC_QJ");
            Session["WuZi_KC_QJ"] = dt;

        }

        /// <summary>
        /// EXCEL导出
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void OutExcel(object sender, EventArgs e)
        {
            if (Session["WuZi_KC_QJ"] != null)
            {
                ExportData ex = new ExportData();
                DataTable dt = (DataTable)Session["WuZi_KC_QJ"];
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    if (dt.Columns[i].ColumnName == "GEGIN_DATE")
                    {
                        dt.Columns[i].ColumnName = "开始时间";
                    }
                    else if (dt.Columns[i].ColumnName == "END_DATE")
                    {
                        dt.Columns[i].ColumnName = "结束时间";
                    }
                    else if (dt.Columns[i].ColumnName == "DEPT_NAME")
                    {
                        dt.Columns[i].ColumnName = "科室名称";
                    }
                    else if (dt.Columns[i].ColumnName == "ITEM_CODE")
                    {
                        dt.Columns[i].ColumnName = "项目编码";
                    }
                    else if (dt.Columns[i].ColumnName == "ITEM_NAME")
                    {
                        dt.Columns[i].ColumnName = "项目名称";
                    }
                    else if (dt.Columns[i].ColumnName == "ITEM_SPEC")
                    {
                        dt.Columns[i].ColumnName = "规格型号";
                    }
                    else if (dt.Columns[i].ColumnName == "UNITS")
                    {
                        dt.Columns[i].ColumnName = "单位";
                    }
                    else if (dt.Columns[i].ColumnName == "QC_AMOUNT")
                    {
                        dt.Columns[i].ColumnName = "区间期初数量";
                    }
                    else if (dt.Columns[i].ColumnName == "INP_AMOUNT")
                    {
                        dt.Columns[i].ColumnName = "区间申请数量";
                    }
                    else if (dt.Columns[i].ColumnName == "OUT_AMOUNT")
                    {
                        dt.Columns[i].ColumnName = "区间使用数量";
                    }
                    else if (dt.Columns[i].ColumnName == "AMOUNT")
                    {
                        dt.Columns[i].ColumnName = "区间库存数量";
                    }
                    else if (dt.Columns[i].ColumnName == "CLASS_NAME")
                    {
                        dt.Columns[i].ColumnName = "类别";
                    }


                }
                ex.ExportToLocal(dt, this.Page, "xls", "区间库存");

            }
        }

        /// <summary>
        /// 查询按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void GetQueryPortalet(object sender, AjaxEventArgs e)
        {

            BindDate(Convert.ToDateTime(this.stardate.SelectedValue).ToString("yyyyMMdd"), Convert.ToDateTime(this.enddate.SelectedValue).ToString("yyyyMMdd"), this.dept_SelectDept.SelectedItem.Value);
        }

        protected void Store_RefreshData(object sender, StoreRefreshDataEventArgs e)
        {

            BindDate(Convert.ToDateTime(this.stardate.SelectedValue).ToString("yyyyMMdd"), Convert.ToDateTime(this.enddate.SelectedValue).ToString("yyyyMMdd"), this.dept_SelectDept.SelectedItem.Value);
        }
        protected void RowSelect(object sender, AjaxEventArgs e)
        {
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