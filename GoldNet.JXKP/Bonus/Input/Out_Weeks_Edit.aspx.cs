using System;
using Goldnet.Ext.Web;
using System.Data;
using Goldnet.Dal;

namespace GoldNet.JXKP.Bonus.Input
{
    public partial class Out_Weeks_Edit : PageBase
    {
        private OutRestNonusDal dal = new OutRestNonusDal();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Ext.IsAjaxRequest)
            {
                Store2.DataSource = dal.GetWeeks();
                Store2.DataBind();
            }
        }


        protected void Buttonsave_Click(object sender, AjaxEventArgs e)
        {
            if (this.ccbtype.SelectedItem.Text == "")
            {
                this.ShowMessage("提示", "类别不能为空！");
            }
            else
            {
                string inputdate = Convert.ToDateTime(dfInputDate.Value.ToString()).ToString("yyyy-MM-dd");
                if (Convert.ToDateTime(inputdate) > Convert.ToDateTime("1000-01-01"))
                {
                    try
                    {

                        dal.SaveWeeks(inputdate, this.ccbtype.SelectedItem.Value);
                        Goldnet.Ext.Web.ScriptManager scManager = Goldnet.Ext.Web.ScriptManager.GetInstance(this.Page);
                        scManager.AddScript("parent.RefreshData('添加成功');");
                        scManager.AddScript("parent.DetailWin.hide();");

                    }

                    catch (Exception ex)
                    {
                        ShowDataError(ex.ToString(), Request.Url.LocalPath, "SavepatientInsert");
                    }
                }
            }

        }
        protected void btnCancle_Click(object sender, EventArgs e)
        {
            Goldnet.Ext.Web.ScriptManager scManager = Goldnet.Ext.Web.ScriptManager.GetInstance(this.Page);

            scManager.AddScript("parent.DetailWin.hide();");
        }
    }
}
