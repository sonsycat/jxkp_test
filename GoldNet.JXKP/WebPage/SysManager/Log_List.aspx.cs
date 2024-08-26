using System;
using Goldnet.Ext.Web;

namespace GoldNet.JXKP.WebPage.SysManager
{
    public partial class Log_List : PageBase
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Ext.IsAjaxRequest)
            {

                for (int i = 0; i < 10; i++)
                {
                    int years = System.DateTime.Now.Year - i;
                    this.years.Items.Add(new Goldnet.Ext.Web.ListItem(years.ToString(), years.ToString()));
                }
                this.years.SelectedItem.Value = System.DateTime.Now.ToString("yyyy");
                this.months.SelectedItem.Value = System.DateTime.Now.ToString("MM");
                Goldnet.Dal.SYS_DEPT_DICT dal = new Goldnet.Dal.SYS_DEPT_DICT();

                BindData(System.DateTime.Now.ToString("yyyy-MM") + "-01");

            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="date"></param>
        private void BindData(string date)
        {
            Goldnet.Dal.SYS_DEPT_DICT dal = new Goldnet.Dal.SYS_DEPT_DICT();
            this.Store1.DataSource = dal.Get_WORK_LOG(date);
            this.Store1.DataBind();

            this.Store2.DataSource = dal.Get_EXEC_LOG(date);
            this.Store2.DataBind();

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Button_click(object sender, AjaxEventArgs e)
        {
            BindData(this.years.SelectedItem.Text + "-" + this.months.SelectedItem.Text + "-01");
        }
    }
}
