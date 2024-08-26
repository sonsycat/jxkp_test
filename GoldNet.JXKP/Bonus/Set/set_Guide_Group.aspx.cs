using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI.WebControls;
using Goldnet.Ext.Web;
using GoldNet.Comm;
using Goldnet.Dal.Properties.Bound;

namespace GoldNet.JXKP.Bonus.Set
{
    public partial class set_Guide_Group : PageBase
    {
        bound_Guide_Group dal = new bound_Guide_Group();
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["CURRENTSTAFF"] == null)
            {
                Response.End();
                return;
            }
            if (!Ext.IsAjaxRequest)
            {
                BindGridPanel("");
            }
        }

        /// <summary>
        /// 绑定数据表格列表
        /// </summary>
        /// <param name="wherestr"></param>
        protected void BindGridPanel(string wherestr)
        {
            DataTable table = dal.GetGuideGroup(wherestr).Tables[0];
            this.Store1.DataSource = table;
            this.Store1.DataBind();
        }

        /// <summary>
        /// 获取指标集信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Store_RefreshData(object sender, StoreRefreshDataEventArgs e)
        {
            BindGridPanel("");
        }

        /// <summary>
        /// 增加按钮按下
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Btn_Add_Click(object sender, AjaxEventArgs e)
        {
            this.GuideGroupName.SetValue("");
            this.EditWin.SetTitle("添加指标集");
            this.EditWin.Show();
        }

        /// <summary>
        /// 编加按钮按下
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Btn_Edit_Click(object sender, AjaxEventArgs e)
        {
            string values = e.ExtraParams["Values"].ToString();
            Dictionary<string, string>[] selectedRow = JSON.Deserialize<Dictionary<string, string>[]>(values);
            this.SelectedID.SetValue(selectedRow[0]["GUIDE_GATHER_CODE"]);
            this.GuideGroupName.SetValue(selectedRow[0]["GUIDE_GATHER_NAME"]);
            this.EditWin.SetTitle("编辑指标集");
            this.EditWin.Show();
        }

        /// <summary>
        /// 删除指标集
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Btn_Del_Click(object sender, AjaxEventArgs e)
        {
            string codeStr = e.ExtraParams["Values"];
            string rtn = "";
            rtn = dal.DelGuideGroupByid(codeStr);
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
        /// 列表 指标编辑、权限维护
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Btn_Command_Click(object sender, AjaxEventArgs e)
        {
            string command = e.ExtraParams["command"].ToString();
            string guidegathercode = e.ExtraParams["Values"].ToString();
            string guidegathername = e.ExtraParams["Names"].ToString();
            string guidegatherorgan = e.ExtraParams["Organ"].ToString();
            //指标编辑
            if (command == "GuideDefine")
            {
                Session.Add("GUIDEGATHERDEFINE", guidegathercode + ',' + guidegatherorgan);
                showDetailWin(getLoadConfig("set_Guide_Selector.aspx"), "指标选择--" + guidegathername, "");
            }
        }

        /// <summary>
        /// 显示详细窗口
        /// </summary>
        /// <param name="loadcfg"></param>
        /// <param name="title"></param>
        /// <param name="width"></param>
        private void showDetailWin(LoadConfig loadcfg, string title, string width)
        {
            DetailWin.ClearContent();
            if (!title.Trim().Equals(""))
            {
                DetailWin.SetTitle(title);
            }
            if (!width.Trim().Equals(""))
            {
                DetailWin.Width = Unit.Pixel(Convert.ToInt16(width));
            }
            DetailWin.Center();
            DetailWin.Show();
            DetailWin.LoadContent(loadcfg);
        }

        /// <summary>
        /// 更新保存指标集
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Btn_Save_Click(object sender, AjaxEventArgs e)
        {

            string selectedid = SelectedID.Value.ToString();
            string guidegroupname = GuideGroupName.Value.ToString().Trim().Replace("'", "");

            string rtn = "";

            if (guidegroupname.Equals(""))
            {
                rtn = "请输入指标集名称！";
            }

            if (!rtn.Equals(""))
            {
                Goldnet.Ext.Web.Ext.Msg.Show(new Goldnet.Ext.Web.MessageBox.Config
                {
                    Title = SystemMsg.msgtitle4,
                    Message = rtn,
                    Buttons = MessageBox.Button.OK,
                    Icon = (MessageBox.Icon)Enum.Parse(typeof(MessageBox.Icon), "INFO")
                });
                return;
            }
            rtn = dal.UpdateGuideGroup(selectedid, guidegroupname);
            Goldnet.Ext.Web.Ext.Msg.Show(new Goldnet.Ext.Web.MessageBox.Config
            {
                Title = SystemMsg.msgtitle4,
                Message = (rtn.Equals("") ? "保存成功！" : rtn),
                Buttons = MessageBox.Button.OK,
                Icon = (MessageBox.Icon)Enum.Parse(typeof(MessageBox.Icon), "INFO")
            });
            if (rtn.Equals(""))
            {
                this.EditWin.Hide();
                Store_RefreshData(null, null);
            }
        }
        
    }
}
