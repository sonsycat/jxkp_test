using System;
using System.Collections.Generic;
using Goldnet.Dal;
using Goldnet.Ext.Web;

namespace GoldNet.JXKP
{
    public partial class SimpleEncourageList : PageBase
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Ext.IsAjaxRequest)
            {
                SimpleEncourage simpleencourage = new SimpleEncourage();
                Store1.DataSource = simpleencourage.GetSimpleEncourage();
                Store1.DataBind();
            }
        }

        /// <summary>
        /// 编辑按钮触发事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Btn_Edit_Click(object sender, AjaxEventArgs e)
        {
            Dictionary<string, string>[] selectRow = GetSelectRow(e);
            if (selectRow != null)
            {
                LoadConfig loadcfg = getLoadConfig("SimpleEncourageEdit.aspx");
                loadcfg.Params.Add(new Goldnet.Ext.Web.Parameter("SimpleEncourageID", selectRow[0]["ID"]));
                loadcfg.Params.Add(new Goldnet.Ext.Web.Parameter("SimpleEncourageMode", "Edit"));
                showDetailWin(loadcfg);
            }
        }

        /// <summary>
        /// 添加按钮触发事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Btn_Add_Click(object sender, AjaxEventArgs e)
        {
            LoadConfig loadcfg = getLoadConfig("SimpleEncourageEdit.aspx");
            loadcfg.Params.Add(new Goldnet.Ext.Web.Parameter("SimpleEncourageMode", "Add"));
            showDetailWin(loadcfg);
        }

        /// <summary>
        /// 添加按钮触发事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Btn_Del_Click(object sender, AjaxEventArgs e)
        {
            Dictionary<string, string>[] selectRow = GetSelectRow(e);
            if (selectRow != null)
            {
                string id = selectRow[0]["ID"].ToString();
                SimpleEncourage simpleencourage = new SimpleEncourage();
                try
                {
                    simpleencourage.DeleteSimpleEncourage(id);
                    Goldnet.Ext.Web.Ext.Msg.Show(new Goldnet.Ext.Web.MessageBox.Config
                    {
                        Title = "提示",
                        Message = "删除成功",
                        Buttons = MessageBox.Button.OK,
                        Icon = (MessageBox.Icon)Enum.Parse(typeof(MessageBox.Icon), "INFO")
                    });
                    Store_RefreshData(null, null);
                }
                catch (Exception ex)
                {
                    ShowDataError(ex.ToString(), Request.Url.LocalPath, "DeleteEditSimoleEncourage");
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Store_RefreshData(object sender, StoreRefreshDataEventArgs e)
        {
            //绑定Store数据源
            SimpleEncourage simpleencourage = new SimpleEncourage();
            Store1.DataSource = simpleencourage.GetSimpleEncourage();
            Store1.DataBind();
        }

        /// <summary>
        /// 显示详细窗口
        /// </summary>
        /// <param name="loadcfg"></param>
        private void showDetailWin(LoadConfig loadcfg)
        {
            DetailWin.ClearContent();
            DetailWin.Show();
            DetailWin.LoadContent(loadcfg);
        }

        ////载入参数设置
        //private LoadConfig getLoadConfig(string url)
        //{
        //    LoadConfig loadcfg = new LoadConfig();
        //    loadcfg.Url = url;
        //    loadcfg.Mode = LoadMode.IFrame;
        //    loadcfg.MaskMsg = "载入中...";
        //    loadcfg.ShowMask = true;
        //    loadcfg.NoCache = true;
        //    return loadcfg;
        //}

        /// <summary>
        /// 反序列化得到客户端提交的gridpanel数据行
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        private Dictionary<string, string>[] GetSelectRow(AjaxEventArgs e)
        {
            string row = e.ExtraParams["Values"].ToString();
            Dictionary<string, string>[] selectRow = JSON.Deserialize<Dictionary<string, string>[]>(row);
            if (selectRow.Length <= 0) { return null; } else { return selectRow; }
        }

    }
}
