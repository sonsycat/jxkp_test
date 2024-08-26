using System;
using System.Collections.Generic;
using System.Data;
using Goldnet.Dal.cbhs;
using Goldnet.Ext.Web;
using GoldNet.Comm.DAL.Oracle;
using GoldNet.Model;

namespace GoldNet.JXKP.cbhs.xyhs.xyhsdict
{
    public partial class prog_cost : PageBase
    {
        private Xyhs_Appor_Prog_Dict dal = new Xyhs_Appor_Prog_Dict();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Ext.IsAjaxRequest)
            {
                SetDict();
                if (Request["prog_code"] != null)
                {
                    SetValue(Request["prog_code"].ToString());
                }
            }
        }

        /// <summary>
        /// 下拉框设置
        /// </summary>
        public void SetDict()
        {

            DataRow[] rolerow = dal.GetFJFA().Tables[0].Select();
            foreach (DataRow row in rolerow)
            {
                this.ComboBox_Role.Items.Add(new Goldnet.Ext.Web.ListItem(row["PROG_NAME"].ToString(), row["PROG_CODE"].ToString()));
            }

        }
        /// <summary>
        /// 成本列表
        /// </summary>
        /// <param name="func"></param>
        /// <returns></returns>
        public DataTable GetFunction()
        {
            return dal.GetNoCheckItem(this.ComboBox_Role.SelectedItem.Value).Tables[0];
        }
        /// <summary>
        /// 分解方案项目
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Selectedrole(object sender, AjaxEventArgs e)
        {

            this.Store2.DataSource = dal.GetCostItembyProg(this.ComboBox_Role.SelectedItem.Value).Tables[0];
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
            if (this.ComboBox_Role.SelectedItem.Value.Equals(string.Empty))
            {
                this.ShowMessage("系统提示", "方案不能为空！");
            }
            else
            {
                List<PageModels.costselected> costlist = e.Object<PageModels.costselected>();

                try
                {
                    dal.SavefjfaCostitem(costlist, this.ComboBox_Role.SelectedItem.Value);
                    this.SaveSucceed();

                }
                catch (Exception ex)
                {
                    ShowDataError(ex, Request.Url.LocalPath, "SubmitData");

                }
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
            this.ComboBox_Role.SelectedItem.Value = progcode;

            Selectedrole(null, null);
        }
        protected void btnCancle_Click(object sender, EventArgs e)
        {
            Goldnet.Ext.Web.ScriptManager scManager = Goldnet.Ext.Web.ScriptManager.GetInstance(this.Page);

            scManager.AddScript("parent.CostWin.hide();");
        }






    }
}
