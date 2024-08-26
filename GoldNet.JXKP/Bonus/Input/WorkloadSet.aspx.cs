using System;
using Goldnet.Ext.Web;
using Goldnet.Dal;
using System.Data;

namespace GoldNet.JXKP.Bonus.Input
{
    public partial class WorkloadSet : PageBase
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

                this.cbbType.Value = "3";
                //检查是否已经登录，否则停止
                if (Session["CURRENTSTAFF"] == null)
                {
                    //               Response.End();
                }
                Bindlist();
            }
        }

        /// <summary>
        /// 刷新store
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Store_RefreshData(object sender, StoreRefreshDataEventArgs e)
        {
            Bindlist();
        }

        /// <summary>
        /// 查询、绑定数据
        /// </summary>
        public void Bindlist()
        {
            this.GridPanel1.Dispose();
            this.Store1.Dispose();
            Cbhs_dict sd = new Cbhs_dict();
            DataTable dt = sd.GetleibieItemList(this.cbbType.SelectedItem.Value).Tables[0];
            this.Store1.DataSource = dt;
            this.Store1.DataBind();
            
        }

        /// <summary>
        /// 刷新
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Button_refresh_click(object sender, EventArgs e)
        {
            Bindlist();
        }

        /// <summary>
        /// 设置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Button_set_click(object sender, EventArgs e)
        {
            RowSelectionModel sm = this.GridPanel1.SelectionModel.Primary as RowSelectionModel;
            if (sm.SelectedRows.Count < 1)
            {
                Goldnet.Ext.Web.Ext.Msg.Show(new Goldnet.Ext.Web.MessageBox.Config
                {
                    Title = "提示",
                    Message = "请选择一条记录",
                    Buttons = MessageBox.Button.OK,
                    Icon = (MessageBox.Icon)Enum.Parse(typeof(MessageBox.Icon), "INFO")
                });
            }
            else
            {
                string ITEM_CLASS = sm.SelectedRow.RecordID;
                string TYPE_CODE = this.cbbType.SelectedItem.Value;
                LoadConfig loadcfg = getLoadConfig("WorkloadSet_manager.aspx");
                loadcfg.Params.Add(new Goldnet.Ext.Web.Parameter("class_code", ITEM_CLASS));
                loadcfg.Params.Add(new Goldnet.Ext.Web.Parameter("TYPE_CODE", TYPE_CODE));
                showDetailWin(loadcfg);
            }
        }

        /// <summary>
        /// 科室比例设置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Button_dept_click(object sender, EventArgs e)
        {
            RowSelectionModel sm = this.GridPanel1.SelectionModel.Primary as RowSelectionModel;
            if (sm.SelectedRows.Count < 1)
            {
                Goldnet.Ext.Web.Ext.Msg.Show(new Goldnet.Ext.Web.MessageBox.Config
                {
                    Title = "提示",
                    Message = "请选择一条记录",
                    Buttons = MessageBox.Button.OK,
                    Icon = (MessageBox.Icon)Enum.Parse(typeof(MessageBox.Icon), "INFO")
                });
            }
            else
            {
                string item_class = sm.SelectedRow.RecordID;
                Goldnet.Ext.Web.ScriptManager scManager = Goldnet.Ext.Web.ScriptManager.GetInstance(this.Page);
                scManager.AddScript("dbonclick('" + item_class + "');");
            }
        }

        /// <summary>
        /// 显示弹出窗口
        /// </summary>
        /// <param name="loadcfg"></param>
        private void showDetailWin(LoadConfig loadcfg)
        {
            DetailWin.ClearContent();
            DetailWin.Show();
            DetailWin.LoadContent(loadcfg);
        }

        /// <summary>
        /// 载入参数设置
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        private new LoadConfig getLoadConfig(string url)
        {
            LoadConfig loadcfg = new LoadConfig();
            loadcfg.Url = url;
            loadcfg.Mode = LoadMode.IFrame;
            loadcfg.MaskMsg = "载入中...";
            loadcfg.ShowMask = true;
            loadcfg.NoCache = true;
            return loadcfg;
        }
        //添加
        protected void Button_add_click(object sender, AjaxEventArgs e)
        {
            string TYPE_CODE = this.cbbType.SelectedItem.Value;
            LoadConfig loadcfg = getLoadConfig("WorkloadSet_ADD.aspx");
            loadcfg.Params.Add(new Goldnet.Ext.Web.Parameter("TYPE_CODE", TYPE_CODE));
            // loadcfg.Params.Add(new Goldnet.Ext.Web.Parameter("op", "add"));
            //loadcfg.Params.Add(new Goldnet.Ext.Web.Parameter("id", e.ExtraParams["Values"].ToString()));
            showDetailWin(loadcfg);


        }
        //删除
        protected void Button_del_click(object sender, EventArgs e)
        {
            RowSelectionModel sm = this.GridPanel1.SelectionModel.Primary as RowSelectionModel;
            if (sm.SelectedRows.Count < 1)
            {
                Ext.Msg.Alert("提示", "请选择一条记录！").Show();
            }
            else
            {
                string item_code = sm.SelectedRow.RecordID;
                Cbhs_dict dal = new Cbhs_dict();
                dal.DelleibieItem(item_code);

            }
            Bindlist();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="itemcode"></param>
        [AjaxMethod]
        public void SetDept(string itemcode)
        {
            LoadConfig loadcfg = getLoadConfig("WorkloadSet_Other.aspx");
            loadcfg.Params.Add(new Goldnet.Ext.Web.Parameter("item_code", itemcode));
            loadcfg.Params.Add(new Goldnet.Ext.Web.Parameter("other_flags", "0"));
            loadcfg.Params.Add(new Goldnet.Ext.Web.Parameter("other_id", "0"));
            loadcfg.Params.Add(new Goldnet.Ext.Web.Parameter("TYPE_CODE", this.cbbType.SelectedItem.Value));
            showCenterSet(this.DeptWin, loadcfg);
        }
    }
}