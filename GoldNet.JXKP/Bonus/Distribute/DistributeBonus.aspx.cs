using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Goldnet.Dal;
using Goldnet.Ext.Web;
using System.Data;
using GoldNet.Comm;

namespace GoldNet.JXKP
{
    public partial class DistributeBonus : PageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Ext.IsAjaxRequest)
            {
                BoundComm boundcomm = new BoundComm();
                //初始化年
                SYear.DataSource = boundcomm.getYears();
                SYear.DataBind();
                cbbYear.SetValue(DateTime.Now.Year);
                //初始化月
                SMonths.DataSource = boundcomm.getMonth();
                SMonths.DataBind();
                cbbmonth.SetValue(DateTime.Now.Month);
                //初始化评价列表
                string typeid = Request["Type"].ToString();

                bool edit = this.IsEdit();
                if (!edit)
                {

                    ScriptManager1.AddScript("#{lTotalBouns}.hide();#{nfTotalBonus}.hide();#{BtnDistribute}.hide();#{BtnDel}.hide();");
                }


                Distribute distribute = new Distribute();
                SReport.DataSource = distribute.GetEvaluateName(typeid);
                SReport.DataBind();

            }
        }
        //查找已经或未分配的奖金
        protected void Search_Select(object sender, EventArgs e)
        {
            if (cbbReport.SelectedItem.Value == "")
            {
                return;
            }
            string reportID = cbbReport.SelectedItem.Value;
            string year = cbbYear.SelectedItem.Value;
            string month = cbbmonth.SelectedItem.Value;
            Distribute distribute = new Distribute();
            string conditin = this.DeptFilter("");
            DataTable dt = distribute.GetDistributeBonus(reportID, year, month, conditin);
            BuildControl bc = new BuildControl();
            bc.BuildDistributeBonus(dt, SSearch, GridPanel2);

            SSearch.DataSource = dt;
            SSearch.DataBind();
        }
        /// <summary>
        /// 奖金分配
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Btn_Distribute_Click(object sender, EventArgs e)
        {
            string reportID = cbbReport.SelectedItem.Value;
            string reportName = cbbReport.SelectedItem.Text;
            if (reportID == "" || reportName == "")
            {
                Goldnet.Ext.Web.Ext.Msg.Show(new Goldnet.Ext.Web.MessageBox.Config
                {
                    Title = "提示",
                    Message = "请选择要分配的报表",
                    Buttons = MessageBox.Button.OK,
                    Icon = (MessageBox.Icon)Enum.Parse(typeof(MessageBox.Icon), "INFO")
                });
                return;
            }
            string year = cbbYear.SelectedItem.Value;
            string month = cbbmonth.SelectedItem.Value;
            double totalbouns = Convert.ToDouble(nfTotalBonus.Text);
            string typeid = Request["Type"].ToString();
            //检查奖金是否分配
            Distribute distribute = new Distribute();
            if (distribute.CheckExist(reportID, year, month))
            {
                Goldnet.Ext.Web.Ext.Msg.Show(new Goldnet.Ext.Web.MessageBox.Config
                {
                    Title = "提示",
                    Message = "此报表的奖金已经分配完毕",
                    Buttons = MessageBox.Button.OK,
                    Icon = (MessageBox.Icon)Enum.Parse(typeof(MessageBox.Icon), "INFO")
                });
                return;
            }

            try
            {
                //分配奖金
                distribute.DistributeBonus(reportID, totalbouns, reportName, typeid, year, month);
                //查询奖金
                string conditin = this.DeptFilter("");
                DataTable dt = distribute.GetDistributeBonus(reportID, year, month, conditin);
                BuildControl bc = new BuildControl();
                bc.BuildDistributeBonus(dt, SSearch, GridPanel2);
                SSearch.DataSource = dt;
                SSearch.DataBind();
            }
            catch (Exception ex)
            {
                ShowDataError(ex.ToString(), Request.Url.LocalPath, "DistributeBonus");
            }

        }
        /// <summary>
        /// 删除分配过的奖金
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Btn_Delete_Click(object sender, EventArgs e)
        {
            string reportID = cbbReport.SelectedItem.Value;
            string year = cbbYear.SelectedItem.Value;
            string month = cbbmonth.SelectedItem.Value;
            try
            {
                Distribute distribute = new Distribute();
                //分配奖金
                distribute.DeleteDistributeBonus(reportID, year, month);
                //查询奖金
                string conditin = this.DeptFilter("");
                DataTable dt = distribute.GetDistributeBonus(reportID, year, month, conditin);
                BuildControl bc = new BuildControl();
                bc.BuildDistributeBonus(dt, SSearch, GridPanel2);
                SSearch.DataSource = dt;
                SSearch.DataBind();
            }
            catch (Exception ex)
            {
                ShowDataError(ex.ToString(), Request.Url.LocalPath, "DeleteDistribute");
            }
        }
    }
}
