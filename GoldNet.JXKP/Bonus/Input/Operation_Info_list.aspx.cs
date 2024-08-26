using System;
using System.Data;
using System.IO;
using System.Threading;
using System.Web;
using System.Web.UI;
using Goldnet.Ext.Web;
using GoldNet.Model;

namespace GoldNet.JXKP.Bonus.Input
{
    public partial class Operation_Info_list : PageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// 角色设置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Buttonset_Click(object sender, EventArgs e)
        {
        }

        protected void Buttonadd_Click(object sender, EventArgs e)
        {
            LoadConfig loadcfg = getLoadConfig("Operation_Info_Edit.aspx");
            loadcfg.Params.Add(new Goldnet.Ext.Web.Parameter("id", ""));
            showCenterSet(this.RoleEdit, loadcfg);
        }


        protected void Buttondel_Click(object sender, EventArgs e)
        {
        }



        protected void Buttonedit_Click(object sender, EventArgs e)
        {
        }

        protected void GetQueryPortalet(object sender, AjaxEventArgs e)
        {
        }

        protected void Store_RefreshData(object sender, StoreRefreshDataEventArgs e)
        {
        }
        protected void RowSelect(object sender, AjaxEventArgs e)
        {
        }






    }
}