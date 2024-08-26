using System;
using System.Collections;
using System.Configuration;
using System.Data;
using Goldnet.Dal;
using Goldnet.Ext.Web;
using GoldNet.Comm;
using GoldNet.Model;

namespace GoldNet.JXKP.RLZY.BaseInfoMaintain
{
    public partial class StaffInfoDeptChange : PageBase
    {
        BaseInfoMaintainDal dal = new BaseInfoMaintainDal();
        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            //检查是否已经登录，否则停止
            if (Session["CURRENTSTAFF"] == null)
            {
                Response.End();
            }
            if (!Ext.IsAjaxRequest)
            {
                DictMainTainDal dictdal = new DictMainTainDal();
                this.Store1.DataSource = dal.ViewStaffDeptInfo("").Tables[0];
                this.Store1.DataBind();
            }
        }

        /// <summary>
        /// 刷新数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void Data_RefreshData(object sender, StoreRefreshDataEventArgs e)
        {
            this.Store1.RemoveAll();
            DataTable l_dt = dal.ViewStaffDeptInfo(this.DeptCodeCombo.SelectedItem.Value.ToString()).Tables[0];
            if (l_dt.Rows.Count > 0)
            {
                this.Store1.DataSource = l_dt;
                this.Store1.DataBind();
            }
        }

        /// <summary>
        /// 保存安钮
        /// </summary>
        protected void SaveInfo(object sender, AjaxEventArgs e)
        {
            string id = e.ExtraParams["Staffid"].ToString().Replace("\"", "");
            string oldDpetCode = e.ExtraParams["staffOldDeptCode"].ToString().Replace("\"", "");
            string oldDpetName = e.ExtraParams["staffOldDeptName"].ToString().Replace("\"", "");
            string Name = e.ExtraParams["StaffName"].ToString().Replace("\"", "");

            if (dal.isExStaffInfoByName(this.cboChangeDept.SelectedItem.Value, this.staffName.Text))
            {
                Goldnet.Ext.Web.Ext.Msg.Show(new Goldnet.Ext.Web.MessageBox.Config
                {
                    Title = "信息提示",
                    Message = "调动科室不能有重复姓名人员",
                    Buttons = MessageBox.Button.OK,
                    Icon = (MessageBox.Icon)Enum.Parse(typeof(MessageBox.Icon), "INFO")
                });
                return;
            }
            dal.InsertStaffChangeInfo(id, Name, oldDpetCode, oldDpetName, this.staffName.Text, this.cboChangeDept.SelectedItem.Value,
                this.cboChangeDept.SelectedItem.Text, ((User)Session["CURRENTSTAFF"]).UserName, id, "");
        }
    }
}
