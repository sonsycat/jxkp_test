using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.Text;
using Goldnet.Ext.Web;
using GoldNet.Comm;
using GoldNet.Comm.DAL.Oracle;
using System.Collections.Generic;
using GoldNet.Model;
using Goldnet.Dal;
namespace GoldNet.JXKP.cbhs.xyhs
{
    public partial class xyhs_cost_income_edit : PageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Ext.IsAjaxRequest)
            {
                if (Request["costcode"].ToString() != "")
                {
                    XyhsDetail dal = new XyhsDetail();
                    DataTable tb = dal.GetCost_incomeList(Request["itemcode"].ToString()).Tables[0];
                    if (tb.Rows.Count > 0)
                    {
                        this.Combo_ItemType.SelectedItem.Text = tb.Rows[0]["flags"].ToString();
                        this.Text_ItemName.Text = tb.Rows[0]["ITEM_NAME"].ToString();
                        
                    }
                }

            }

        }
        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Buttonsave_Click(object sender, EventArgs e)
        {
            try
            {
                XyhsDetail dal = new XyhsDetail();
               // dal.SaveXyhsincomesItem(Request["itemcode"].ToString(), this.Combo_ItemType.SelectedItem.Value, this.ratio.Text);
                Goldnet.Ext.Web.ScriptManager scManager = Goldnet.Ext.Web.ScriptManager.GetInstance(this.Page);
                scManager.AddScript("parent.DetailWin.hide();");
                scManager.AddScript("parent.RefreshData();");
            }
            catch (Exception ex)
            {
                ShowDataError(ex, Request.Url.LocalPath, "Buttonsave_Click");

            }
        }


        protected void btnCancle_Click(object sender, EventArgs e)
        {
            Goldnet.Ext.Web.ScriptManager scManager = Goldnet.Ext.Web.ScriptManager.GetInstance(this.Page);

            scManager.AddScript("parent.DetailWin.hide();");

        }
    }
}