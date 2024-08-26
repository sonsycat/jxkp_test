using System;
using System.Data;
using Goldnet.Ext.Web;
namespace GoldNet.JXKP.jxkh
{
    public partial class DeptGuideEdit : PageBase
    {
        /// <summary>
        /// 初始化处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Ext.IsAjaxRequest)
            {
                SetDict();
                HttpProxy proxy1 = new HttpProxy();
                proxy1.Method = HttpMethod.POST;
                proxy1.Url = "/jxkh/WebService/GuideList.ashx";
                this.Store1.Proxy.Add(proxy1);
                if (Request["guidecause"] != null)
                {
                    Edit();
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
            if (this.guide.SelectedItem.Value == string.Empty)
            {
                this.ShowMessage("系统提示", "指标不能为空！");
            }
            else if (this.Comboguide.SelectedItem.Value == string.Empty)
            {
                this.ShowMessage("系统提示", "关联指标不能为空！");
            }
            else if (this.guidecause.Text == string.Empty)
            {
                this.ShowMessage("系统提示", "达标值不能为空！");
            }
            else
            {
                try
                {
                    Goldnet.Dal.StationManager dal = new Goldnet.Dal.StationManager();
                    dal.SavedeptGuide(Request["deptcode"].ToString(), this.guide.SelectedItem.Value, this.Comboguide.SelectedItem.Value, this.guidecause.Text);
                    Goldnet.Ext.Web.ScriptManager scManager = Goldnet.Ext.Web.ScriptManager.GetInstance(this.Page);

                    scManager.AddScript("parent.RoleEdit.hide();");
                    scManager.AddScript("parent.RefreshData();");
                }
                catch (Exception ex)
                {
                    ShowDataError(ex, Request.Url.LocalPath, "Buttonsave_Click");

                }
            }
        }

        /// <summary>
        ///设置下拉框
        /// </summary>
        public void SetDict()
        {
            Goldnet.Dal.StationManager dal = new Goldnet.Dal.StationManager();
            DataTable table = dal.GetGuideDept(Request["deptcode"].ToString(), Request["years"].ToString());
            if (table.Rows.Count > 0)
            {
                for (int i = 0; i < table.Rows.Count; i++)
                {
                    this.guide.Items.Add(new Goldnet.Ext.Web.ListItem(table.Rows[i]["GUIDE_NAME"].ToString(), table.Rows[i]["GUIDE_CODE"].ToString()));
                }
            }
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="model"></param>
        public void Edit()
        {
            this.guide.SelectedItem.Value = Request["guidecode"].ToString();
            this.Comboguide.SelectedItem.Value = Request["vsguidecode"].ToString();
            this.guidecause.Text = Request["guidecause"].ToString();
            this.guide.Disabled = true;
            this.Comboguide.Disabled = true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnCancle_Click(object sender, EventArgs e)
        {
            Goldnet.Ext.Web.ScriptManager scManager = Goldnet.Ext.Web.ScriptManager.GetInstance(this.Page);

            scManager.AddScript("parent.RoleEdit.hide();");
        }
    }
}