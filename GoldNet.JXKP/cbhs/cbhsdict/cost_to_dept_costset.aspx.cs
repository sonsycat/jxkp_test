using System;
using System.Drawing;
using System.Data;
using System.Data.OleDb;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using Goldnet.Ext.Web;
using GoldNet.Comm;
using GoldNet.Comm.Pic;
using GoldNet.Comm.DAL.Oracle;
using GoldNet.Model;
using Goldnet.Dal;

namespace GoldNet.JXKP.cbhs.cbhsdict
{
    public partial class cost_to_dept_costset : PageBase
    {
        private Appor_Prog_Dict dal = new Appor_Prog_Dict();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Ext.IsAjaxRequest)
            {
                if (Request["indexid"] != null)
                {
                    SetValue(Request["indexid"].ToString());
                }
            }

        }
        
        /// <summary>
        /// 成本列表
        /// </summary>
        /// <param name="func"></param>
        /// <returns></returns>
        public DataTable GetFunction()
        {
            return dal.GetNoCheckItembyindex(Request["indexid"].ToString()).Tables[0];
        }
        /// <summary>
        /// 分解方案项目
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Selectedrole(object sender, AjaxEventArgs e)
        {

            this.Store2.DataSource = dal.GetCostItembyindex(Request["indexid"].ToString()).Tables[0];
            this.Store2.DataBind();
            SelectedFuncType(null, null);

        }


        /// <summary>
        /// 保存项目
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void SubmitData(object sender, StoreSubmitDataEventArgs e)
        {

            List<PageModels.costselected> costlist = e.Object<PageModels.costselected>();

            try
            {
                dal.SavecosttodeptCostitem(costlist, Request["indexid"].ToString());
                this.SaveSucceed();

            }
            catch (Exception ex)
            {
                ShowDataError(ex, Request.Url.LocalPath, "SubmitData");

            }
        }

        protected void SelectedFuncType(object sender, AjaxEventArgs e)
        {
            this.Store1.DataSource = GetFunction();
            this.Store1.DataBind();
        }
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="model"></param>
        public void SetValue(string progcode)
        {
            Selectedrole(null, null);
        }
        protected void btnCancle_Click(object sender, EventArgs e)
        {
            Goldnet.Ext.Web.ScriptManager scManager = Goldnet.Ext.Web.ScriptManager.GetInstance(this.Page);

            scManager.AddScript("parent.itemset.hide();");
        }

    }
}