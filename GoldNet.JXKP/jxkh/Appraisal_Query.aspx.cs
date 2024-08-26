using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Web.UI.WebControls;
using Goldnet.Ext.Web;

namespace GoldNet.JXKP.jxkh
{
    public partial class Appraisal_Query : PageBase
    {
        Goldnet.Dal.Appraisal dal = new Goldnet.Dal.Appraisal();

        /// <summary>
        /// 初始化处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["CURRENTSTAFF"] == null)
            {
                Response.End();
            }
            if (!Ext.IsAjaxRequest)
            {
                SetInitState();
                GridDataBind();
            }
        }

        //设置页面控件初始化状态
        protected void SetInitState()
        {
            //年份下拉框
            for (int i = 0; i < 10; i++)
            {
                int years = System.DateTime.Now.Year - i;
                this.CombEvalYear.Items.Add(new Goldnet.Ext.Web.ListItem(years.ToString(), years.ToString()));
            }
            this.CombEvalYear.SelectedItem.Value = System.DateTime.Now.ToString("yyyy");
            //评价类型列表
            DataTable dt = dal.GetEvalutetype("").Tables[0];
            this.Store2.DataSource = dt;
            this.Store2.DataBind();
            if (dt.Rows.Count > 0)
            {
                this.CombEvalType.SelectedIndex = 0;
            }
            this.CombEvalYear.SelectedIndex = 0;
            this.CombArchFlg.SelectedIndex = 0;
        }

        //数据查询绑定
        protected void GridDataBind()
        {
            string evalyear = this.CombEvalYear.SelectedItem.Value.Trim().Replace("'", "");
            string evalarch = this.CombArchFlg.SelectedItem.Value.Trim().Replace("'", "");
            string evaltype = this.CombEvalType.SelectedItem.Value.Trim().Replace("'", "");
            StringBuilder strwhere = new StringBuilder();
            if (!evalyear.Equals(""))
            {
                strwhere.Append(" AND SUBSTR(A.START_DATE,0,4)='" + evalyear + "' ");
            }

            if (!evaltype.Equals(""))
            {
                strwhere.Append(" AND A.EVALUATE_CLASS_CODE='" + evaltype + "' ");
            }
            if (!evalarch.Equals(""))
            {
                strwhere.Append(" AND A.ARCHIVE_TAGS='" + evalarch + "' ");
            }
            string deptFilter = DeptFilter("");
            DataTable dt = dal.GetAppraisalList(strwhere.ToString(), deptFilter).Tables[0];
            this.Store1.DataSource = dt;
            this.Store1.DataBind();
        }

        /// <summary>
        /// 查询处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Btn_Search_Click(object sender, AjaxEventArgs e)
        {
            GridDataBind();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Store_RefreshData(object sender, StoreRefreshDataEventArgs e)
        {
            GridDataBind();
        }

        //显示评价详情
        protected void Appraisal_Detail_Show(object sender, AjaxEventArgs e)
        {
            Session.Remove("APPRAISALDETAIL");
            string values = e.ExtraParams["Values"].ToString();
            Dictionary<string, string>[] selectedRow = JSON.Deserialize<Dictionary<string, string>[]>(values);
            if (selectedRow.Length == 0) return;
            Session.Add("APPRAISALDETAIL", selectedRow);

            LoadConfig loadcfg = getLoadConfig("Appraisal_Detail.aspx");
            string isEdit = IsEdit().ToString();
            loadcfg.Params.Add(new Goldnet.Ext.Web.Parameter("isEdit", isEdit));
            string deptFilter = DeptFilter("");
            loadcfg.Params.Add(new Goldnet.Ext.Web.Parameter("deptFilter", deptFilter));
            showDetailWin(loadcfg, "评价详情--" + selectedRow[0]["EVALUATE_NAME"], "");

        }

        //显示详细窗口
        private void showDetailWin(LoadConfig loadcfg, string title, string width)
        {
            DetailWin.ClearContent();
            if (!title.Trim().Equals(""))
            {
                DetailWin.SetTitle(title);
            }
            if (!width.Trim().Equals(""))
            {
                DetailWin.Width = Unit.Pixel(Convert.ToInt16(width));
            }
            DetailWin.Center();
            DetailWin.Show();
            DetailWin.LoadContent(loadcfg);
        }

    }
}
