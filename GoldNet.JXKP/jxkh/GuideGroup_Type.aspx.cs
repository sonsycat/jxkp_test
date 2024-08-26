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
    public partial class GuideGroup_Type : System.Web.UI.Page
    {
        Goldnet.Dal.Guide_Group dal = new Goldnet.Dal.Guide_Group();
        protected void Page_Load(object sender, EventArgs e)
        {
            //检查是否已经登录，否则停止
            if (Session["CURRENTSTAFF"] == null)
            {
                Response.End();
                return;
            }
            if (!Ext.IsAjaxRequest)
            {
                Store_RefreshData(null, null);
                BindTypeCombox();
            }
        }
        /// <summary>
        /// 读取字典信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Store_RefreshData(object sender, StoreRefreshDataEventArgs e)
        {
            DataTable table = dal.GetGuideGroupTypeDictList().Tables[0];
            this.Store1.DataSource = table;
            this.Store1.DataBind();
        }
        /// <summary>
        /// 获取指标集类别大分类
        /// </summary>
        protected void BindTypeCombox()
        {
            DataTable dt = dal.GetGuideGroupTypeClass().Tables[0];
            this.StoreCombo.DataSource = dt;
            this.StoreCombo.DataBind();
        }
        /// <summary>
        /// 删除字典数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Btn_Del_Click(object sender, AjaxEventArgs e)
        {
            string codeStr = e.ExtraParams["Values"];
            string rtn = dal.GuideGroupTypeDel(codeStr);
            if (rtn.Equals(""))
            {
                Store_RefreshData(null, null);
            }
            else
            {
                Goldnet.Ext.Web.Ext.Msg.Show(new Goldnet.Ext.Web.MessageBox.Config
                {
                    Title = SystemMsg.msgtitle4,
                    Message = rtn,
                    Buttons = MessageBox.Button.OK,
                    Icon = (MessageBox.Icon)Enum.Parse(typeof(MessageBox.Icon), "INFO")
                });
            }
        }
        /// <summary>
        /// 添加按钮，显示录入界面
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Btn_Add_Click(object sender, AjaxEventArgs e)
        {
            this.CodeField.Text="(新增项)";
            this.CodeFieldHide.SetValue("(新增项)");
            this.NameField.SetValue("");
            this.TypeField.SetValue("01");
            this.TypeFieldHide.SetValue("01");
            this.UseallField.SetValue(false);
            this.DetailWin.SetTitle("增加新类别");
            this.DetailWin.Show();
        }
        /// <summary>
        /// 编辑按钮，显示选择项以供编辑
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Btn_Edit_Click(object sender, AjaxEventArgs e)
        {
            string values = e.ExtraParams["Values"].ToString();
            Dictionary<string, string>[] selectedRow = JSON.Deserialize<Dictionary<string, string>[]>(values);
            this.CodeField.Text = selectedRow[0]["GUIDE_GROUP_TYPE"];
            this.CodeFieldHide.SetValue(selectedRow[0]["GUIDE_GROUP_TYPE"]);
            this.NameField.SetValue(selectedRow[0]["GUIDE_GROUP_TYPE_NAME"]);
            this.TypeField.SetValue(selectedRow[0]["P_TYPE"]);
            this.TypeFieldHide.SetValue(selectedRow[0]["P_TYPE"]);
            this.UseallField.SetValue(selectedRow[0]["ALLUSE"] == null ? false : true);
            this.DetailWin.SetTitle("编辑指标集类别");
            this.DetailWin.Show();
        }

        /// <summary>
        /// 保存按钮，输入项检验，更新数据库
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Btn_Save_Click(object sender, AjaxEventArgs e)
        {
            string code = this.CodeFieldHide.Value.ToString().Replace("'", "''");
            string name = this.NameField.Value.ToString().Trim();
            string type = this.TypeFieldHide.Value.ToString().Trim();
            string useall = this.UseallField.Checked?"1":"0";

            if (name.Equals("") || type.Equals(""))
            {
                Goldnet.Ext.Web.Ext.Msg.Show(new Goldnet.Ext.Web.MessageBox.Config
                {
                    Title = SystemMsg.msgtitle4,
                    Message = "请输入指标集类别名称及分类！",
                    Buttons = MessageBox.Button.OK,
                    Icon = (MessageBox.Icon)Enum.Parse(typeof(MessageBox.Icon), "INFO")
                });
            }
            else
            {
                dal.GuideGroupTypeUpdate(code, name, type, useall);
                this.DetailWin.Hide();
                Store_RefreshData(null,null);
            }

        }
        

    }
}
