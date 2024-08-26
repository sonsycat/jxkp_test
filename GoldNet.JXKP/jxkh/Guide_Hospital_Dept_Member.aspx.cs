using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Goldnet.Ext.Web;
using GoldNet.Comm;
using GoldNet.Comm.DAL.Oracle;
using GoldNet.Model;

namespace GoldNet.JXKP.jxkh
{
    public partial class Guide_Hospital_Dept_Member : System.Web.UI.Page
    {
        Goldnet.Dal.Guide_Dict dal = new Goldnet.Dal.Guide_Dict();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["CURRENTSTAFF"] == null)
            {
                Response.End();
                return;
            }
            if (!Ext.IsAjaxRequest)
            {
                  Store_RefreshData(null, null);
            }
        }
        /// <summary>
        /// 获取指标关联对照
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Store_RefreshData(object sender, StoreRefreshDataEventArgs e)
        {
            
            DataTable table = dal.GetGuideHospitalDeptMemberList("").Tables[0];
            this.Store1.DataSource = table;
            this.Store1.DataBind();
        }
        /// <summary>
        /// 绑定院、科、人指标下拉列表
        /// </summary>
        protected void BindCombox()
        {
            DataTable table = dal.GetGuideByOrgan("01").Tables[0];
            this.StoreComboY.DataSource = table;
            this.StoreComboY.DataBind();
            table = dal.GetGuideByOrgan("02").Tables[0];
            this.StoreComboK.DataSource = table;
            this.StoreComboK.DataBind();
            table = dal.GetGuideByOrgan("03").Tables[0];
            this.StoreComboR.DataSource = table;
            this.StoreComboR.DataBind();
        }
        /// <summary>
        /// 添加指标关联对照
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Btn_Add_Click(object sender, AjaxEventArgs e)
        {
            BindCombox();
            this.GuideYField.SetValue("");
            this.GuideKField.SetValue("");
            this.GuideRField.SetValue("");
            this.SerialNoHidden.SetValue("");
            this.GuideYFieldHide.SetValue("");
            this.GuideKFieldHide.SetValue("");
            this.GuideRFieldHide.SetValue("");
            this.DetailWin.SetTitle("添加指标关联对照");
            this.DetailWin.Show();
        }
        /// <summary>
        /// 编辑指标关联对照
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Btn_Edit_Click(object sender, AjaxEventArgs e)
        {
            string values = e.ExtraParams["Values"].ToString();
            Dictionary<string, string>[] selectedRow = JSON.Deserialize<Dictionary<string, string>[]>(values);

            BindCombox();
            this.GuideYField.AddItem(selectedRow[0]["HOSPITAL_GUIDE_NAME"], selectedRow[0]["HOSPITAL_GUIDE_CODE"]);
            this.GuideKField.AddItem(selectedRow[0]["DEPT_GUIDE_NAME"], selectedRow[0]["DEPT_GUIDE_CODE"]);
            this.GuideRField.AddItem(selectedRow[0]["MEMBER_GUIDE_NAME"], selectedRow[0]["MEMBER_GUIDE_CODE"]);
            this.GuideYField.SetValue(selectedRow[0]["HOSPITAL_GUIDE_CODE"]);
            this.GuideKField.SetValue(selectedRow[0]["DEPT_GUIDE_CODE"]);
            this.GuideRField.SetValue(selectedRow[0]["MEMBER_GUIDE_CODE"]);

            this.SerialNoHidden.SetValue(selectedRow[0]["SERIAL_NO"]);
            this.GuideYFieldHide.SetValue(selectedRow[0]["HOSPITAL_GUIDE_CODE"]);
            this.GuideKFieldHide.SetValue(selectedRow[0]["DEPT_GUIDE_CODE"]);
            this.GuideRFieldHide.SetValue(selectedRow[0]["MEMBER_GUIDE_CODE"]);
            this.DetailWin.SetTitle("编辑指标关联对照");
            this.DetailWin.Show();
        }
        /// <summary>
        /// 删除指标关联对照
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Btn_Del_Click(object sender, AjaxEventArgs e)
        {
            string codeStr = e.ExtraParams["Values"];
            string rtn = "";
            rtn = dal.DelGuideHospitalDeptMember(codeStr);
            if (rtn.Equals(""))
            {
                Goldnet.Ext.Web.ScriptManager scManager = Goldnet.Ext.Web.ScriptManager.GetInstance(this.Page);
                scManager.AddScript(this.GridPanel_List.ClientID + ".deleteSelected();");
            }
            else
            {
                Goldnet.Ext.Web.Ext.Msg.Show(new Goldnet.Ext.Web.MessageBox.Config
                {
                    Title = SystemMsg.msgtitle4,
                    Message = "删除操作失败！",
                    Buttons = MessageBox.Button.OK,
                    Icon = (MessageBox.Icon)Enum.Parse(typeof(MessageBox.Icon), "INFO")
                });
            }
        }
        /// <summary>
        /// 保存指标关联对照关系
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Btn_Save_Click(object sender, AjaxEventArgs e)
        {
            string serialno = this.SerialNoHidden.Value.ToString();
            string guidey = this.GuideYFieldHide.Value.ToString() == null ? "" : this.GuideYFieldHide.Value.ToString();
            string guidek = this.GuideKFieldHide.Value.ToString() == null ? "" : this.GuideKFieldHide.Value.ToString();
            string guider = this.GuideRFieldHide.Value.ToString() == null ? "" : this.GuideRFieldHide.Value.ToString();

            int cnt = (guidey.Length > 0 ? 1 : 0) + (guidek.Length > 0 ? 1 : 0) + (guider.Length > 0 ? 1 : 0);
            if (cnt < 2)
            {
                Goldnet.Ext.Web.Ext.Msg.Show(new Goldnet.Ext.Web.MessageBox.Config
                {
                    Title = SystemMsg.msgtitle4,
                    Message = "请选择关联的指标!",
                    Buttons = MessageBox.Button.OK,
                    Icon = (MessageBox.Icon)Enum.Parse(typeof(MessageBox.Icon), "INFO")
                });
                return;
            }
            else
            {
                dal.SaveGuideHospitalDeptMember(serialno, guidey, guidek, guider);
                this.DetailWin.Hide();
                Store_RefreshData(null, null);
            }


        }
    }
}
