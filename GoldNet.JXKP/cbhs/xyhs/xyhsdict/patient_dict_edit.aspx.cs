using System;
using Goldnet.Ext.Web;
using System.Data;
using Goldnet.Dal.cbhs;
namespace GoldNet.JXKP.cbhs.xyhs.xyhsdict
{
    public partial class patient_dict_edit : PageBase
    {
        private Xyhs_Appor_Prog_Dict dal_prog = new Xyhs_Appor_Prog_Dict();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Ext.IsAjaxRequest)
            {
                Store2.DataSource = dal_prog.GetdiagnsosiDict();
                Store2.DataBind();
            }
        }

        /// <summary>
        /// 数据保存处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Buttonsave_Click(object sender, AjaxEventArgs e)
        {
            if (this.ComShowflag.SelectedItem.Value == "0" && this.TextField1.Value.ToString() == "")
            {
                this.ShowMessage("提示", "住院标识不能为空！");
            }
            else
            {
                if (this.patient_id.Text != "" && this.ccbtype.SelectedItem.Value != "")
                {
                    try
                    {
                        string inputdate = Convert.ToDateTime(dfInputDate.Value.ToString()).ToString("yyyy-MM-dd");
                        if (Convert.ToDateTime(inputdate) > Convert.ToDateTime("1000-01-01"))
                        {
                            string strdate = dal_prog.getpatientdate(this.patient_id.Text, this.TextField1.Value.ToString(), this.ComShowflag.SelectedItem.Value);
                            if (Convert.ToDateTime(inputdate) < Convert.ToDateTime(strdate))
                            {
                                this.ShowMessage("提示", "不能早于病人确诊时间！");
                                return;
                            }
                        }
                        if (Convert.ToDateTime(inputdate) < Convert.ToDateTime("1000-01-01"))
                        {
                            inputdate = "";
                        }
                        dal_prog.insertpatientdict(this.patient_id.Text, inputdate, this.ccbtype.SelectedItem.Value, this.TextField1.Value.ToString(), this.ComShowflag.SelectedItem.Value);
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
