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

namespace GoldNet.JXKP.WebPage.SysManager
{
    public partial class RoleSet : PageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Ext.IsAjaxRequest)
            {
                SetDict();
                if (Request["roleid"] != null)
                {
                    GoldNet.Model.SYS_ROLE_DICT model = new GoldNet.Model.SYS_ROLE_DICT(int.Parse(Request["roleid"].ToString()));
                    SetValue(model);
                }
            }

        }
        /// <summary>
        /// 下拉框设置
        /// </summary>
        public void SetDict()
        {
            Goldnet.Dal.SYS_ROLE_DICT dal = new Goldnet.Dal.SYS_ROLE_DICT();
            User user = (User)Session["CURRENTSTAFF"];
            string filter = user.GetRoleFilter("app_id");
            DataRow[] funrow = dal.GetFunctionType().Tables[0].Select();
            if (filter != "app_id in ('-1')")
            {
                funrow = dal.GetFunctionType().Tables[0].Select(filter);
            }
            else
            {
                funrow = dal.GetFunctionType().Tables[0].Select("POWER_TYPE='0'");
            }

            foreach (DataRow rw in funrow)
            {
                this.Combo_FuncType.Items.Add(new Goldnet.Ext.Web.ListItem(rw["app_name"].ToString(), rw["app_id"].ToString()));
            }

            DataRow[] rolerow = dal.GetList("").Tables[0].Select(user.GetRoleFilter("role_app"));
            foreach (DataRow row in rolerow)
            {
                this.ComboBox_Role.Items.Add(new Goldnet.Ext.Web.ListItem(row["role_name"].ToString(), row["role_id"].ToString()));
            }

        }
        /// <summary>
        /// 功能列表
        /// </summary>
        /// <param name="func"></param>
        /// <returns></returns>
        public DataTable GetFunction(string func)
        {
            Goldnet.Dal.SYS_ROLE_DICT roledal = new Goldnet.Dal.SYS_ROLE_DICT();
            return roledal.GetFunction(func, this.ComboBox_Role.SelectedItem.Value).Tables[0];
        }
        /// <summary>
        /// 角色权限
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Selectedrole(object sender, AjaxEventArgs e)
        {
            Goldnet.Dal.SYS_ROLE_DICT roledal = new Goldnet.Dal.SYS_ROLE_DICT();
            this.Store2.DataSource = roledal.GetFunctionByRole(this.ComboBox_Role.SelectedItem.Value).Tables[0];
            this.Store2.DataBind();
            SelectedFuncType(null, null);

        }


        /// <summary>
        /// 保存角色
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void SubmitData(object sender, StoreSubmitDataEventArgs e)
        {
            if (this.ComboBox_Role.SelectedItem.Value.Equals(string.Empty))
            {
                this.ShowMessage("系统提示", "角色不能为空！");
            }
            else
            {
                List<PageModels.functionselected> functions = e.Object<PageModels.functionselected>();
                Goldnet.Dal.SYS_ROLE_DICT dal = new Goldnet.Dal.SYS_ROLE_DICT();
                try
                {
                    dal.SaveRoleFunction(functions, int.Parse(this.ComboBox_Role.SelectedItem.Value));
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
            this.Store1.DataSource = GetFunction(this.Combo_FuncType.SelectedItem.Value);
            this.Store1.DataBind();
        }
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="model"></param>
        public void SetValue(GoldNet.Model.SYS_ROLE_DICT model)
        {
            this.ComboBox_Role.SelectedItem.Value = model.ROLE_ID.ToString();
            this.ComboBox_Role.SelectedItem.Text = model.ROLE_NAME;
            Selectedrole(null, null);
        }
        protected void btnCancle_Click(object sender, EventArgs e)
        {
            Goldnet.Ext.Web.ScriptManager scManager = Goldnet.Ext.Web.ScriptManager.GetInstance(this.Page);

            scManager.AddScript("parent.Rle_Set.hide();");
        }
       
    }
}