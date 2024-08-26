using System;
using Goldnet.Ext.Web;
using System.Data;
using Goldnet.Dal.cbhs;

namespace GoldNet.JXKP.cbhs.xyhs.xyhsdict
{
    public partial class xyhs_diagnosis_icd_dict : PageBase
    {
        private Xyhs_Appor_Prog_Dict dal_prog = new Xyhs_Appor_Prog_Dict();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Ext.IsAjaxRequest)
            {
                HttpProxy pro1 = new HttpProxy();
                pro1.Method = HttpMethod.POST;
                pro1.Url = "../../WebService/diagnosisList.ashx";
                this.Store3.Proxy.Add(pro1);

                Store2.DataSource = dal_prog.GetdiagnsosiDict();
                Store2.DataBind();



                if (Request.QueryString["ID"] != null && !Request.QueryString["ID"].Equals(""))
                {
                    string id = Request.QueryString["ID"].ToString();
                    Edit_Init(id);
                }
            }
        }

        public void Edit_Init(string id)
        {

            DataTable dt = dal_prog.GetDiagnosisIcdById(id);
            //绑定界面初始化值
            this.ccbtype.SelectedItem.Value = dt.Rows[0]["DIAGNOSIS_CODE"].ToString();
            this.ccbtype.SelectedItem.Text = dt.Rows[0]["DIAGNOSIS_NAME"].ToString();
            this.DIAGNOSIS_ITEM.SelectedItem.Value = dt.Rows[0]["ICD_CODE"].ToString();
            this.DIAGNOSIS_ITEM.SelectedItem.Text = dt.Rows[0]["ICD_NAME"].ToString();
        }

        /// <summary>
        /// 数据保存处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Buttonsave_Click(object sender, AjaxEventArgs e)
        {
            if (this.ccbtype.SelectedItem.Value == "" && this.DIAGNOSIS_ITEM.SelectedItem.Value == "")
            {
                this.ShowMessage("提示", "住院标识不能为空！");
            }
            else
            {
                
                if (Request.QueryString["ID"] != null && !Request.QueryString["ID"].Equals(""))
                {
                    string id = Request.QueryString["ID"].ToString();
                    dal_prog.UpdateDiagnosisIcd(this.ccbtype.SelectedItem.Value, this.ccbtype.SelectedItem.Text, this.DIAGNOSIS_ITEM.SelectedItem.Value, this.DIAGNOSIS_ITEM.SelectedItem.Text, id);
                    Goldnet.Ext.Web.ScriptManager scManager = Goldnet.Ext.Web.ScriptManager.GetInstance(this.Page);
                    scManager.AddScript("parent.RefreshData('修改成功');");
                    scManager.AddScript("parent.DetailWin.hide();");
                }
                else
                {
                    dal_prog.InsertDiagnosisIcd(this.ccbtype.SelectedItem.Value, this.ccbtype.SelectedItem.Text, this.DIAGNOSIS_ITEM.SelectedItem.Value, this.DIAGNOSIS_ITEM.SelectedItem.Text);
                    Goldnet.Ext.Web.ScriptManager scManager = Goldnet.Ext.Web.ScriptManager.GetInstance(this.Page);
                    scManager.AddScript("parent.RefreshData('添加成功');");
                    scManager.AddScript("parent.DetailWin.hide();");
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
