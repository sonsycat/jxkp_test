using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using Goldnet.Ext.Web;
using GoldNet.Comm;

namespace GoldNet.JXKP
{
    public partial class BonusGuideDict : Page
    {
        Goldnet.Dal.BonusGuideDict dal = new Goldnet.Dal.BonusGuideDict();

        /// <summary>
        /// 初始化处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Ext.IsAjaxRequest)
            {
                Store_RefreshData(null, null);
            }
        }

        /// <summary>
        /// 设置字典
        /// </summary>
        protected void Store_RefreshData(object sender, StoreRefreshDataEventArgs e)
        {
            DataTable table = dal.GetGuideDictList("", "", "", "").Tables[0];
            this.Store1.DataSource = table;
            this.Store1.DataBind();
        }

        /// <summary>
        /// 删除字典数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Btn_Del_Click(object sender, AjaxEventArgs e)
        {
            string codeStr = e.ExtraParams["Values"];
            string msg = dal.CheckGuidePrimary(codeStr);
            if (msg != "")
            {//检查删除的指标是否为奖金必备指标，如果是不可以删除
                Goldnet.Ext.Web.Ext.Msg.Show(new Goldnet.Ext.Web.MessageBox.Config
                {
                    Title = SystemMsg.msgtitle4,
                    Message = msg,
                    Buttons = MessageBox.Button.OK,
                    Icon = (MessageBox.Icon)Enum.Parse(typeof(MessageBox.Icon), "INFO")
                });
            }
            else
            {
                string rtn = dal.DelGuide(codeStr);
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
        }

        /// <summary>
        /// 增加新指标
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Btn_Add_Click(object sender, AjaxEventArgs e)
        {
            Session.Remove("SelectedGuide");
            showDetailWin(getLoadConfig("BonusGuideEdit.aspx"), "添加指标", "540");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Btn_Edit_Click(object sender, AjaxEventArgs e)
        {
            string values = e.ExtraParams["Values"];
            Dictionary<string, string>[] selectedRow = JSON.Deserialize<Dictionary<string, string>[]>(values);
            Session.Remove("SelectedGuide");
            Session.Add("SelectedGuide", selectedRow[0]);
            showDetailWin(getLoadConfig("BonusGuideEdit.aspx"), "编辑指标", "540");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Btn_Sql_Click(object sender, AjaxEventArgs e)
        {
            string values = e.ExtraParams["Values"];
            Dictionary<string, string>[] selectedRow = JSON.Deserialize<Dictionary<string, string>[]>(values);
            Session.Remove("SelectedGuide");
            Session.Add("SelectedGuide", selectedRow[0]);
            showDetailWin(getLoadConfig("BounsGuideSQLExpress.aspx"), "指标算法信息", "543");
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
        /// 载入参数设置
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        private LoadConfig getLoadConfig(string url)
        {
            LoadConfig loadcfg = new LoadConfig();
            loadcfg.Url = url;
            loadcfg.Mode = LoadMode.IFrame;
            loadcfg.MaskMsg = "载入中...";
            loadcfg.ShowMask = true;
            loadcfg.NoCache = true;
            return loadcfg;
        }

    }
}
