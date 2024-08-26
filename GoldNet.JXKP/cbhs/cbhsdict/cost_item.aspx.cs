using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Goldnet.Dal;
using Goldnet.Ext.Web;
using System.Text;
using System.Collections.Generic;

namespace GoldNet.JXKP.cbhs.cbhsdict
{
    public partial class cost_item : System.Web.UI.Page
    {
        private Cost_Item_Type dal_type = new Cost_Item_Type();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Ext.IsAjaxRequest)
            {
                //检查是否已经登录，否则停止
                if (Session["CURRENTSTAFF"] == null)
                {
                    //               Response.End();
                }
                TreeBuild();
                Bindlist();
            }

        }
        [AjaxMethod]
        public string TreeBuild()
        {
            this.TreeCtrl.Root.Clear();

            Goldnet.Ext.Web.TreeNode root = null;
            //初始化树结构
            root = new Goldnet.Ext.Web.TreeNode();

            root.NodeID = "root";

            root.Text = "成本项目";

            root.Expanded = true;

            this.TreeCtrl.Root.Add(root);

            DataTable l_TreeDt = dal_type.GetCostType().Tables[0];

            Hashtable rootnode = new Hashtable();

            for (int i = 0; i < l_TreeDt.Rows.Count; i++)
            {
                string nodekey = "";
                Goldnet.Ext.Web.TreeNode node = new Goldnet.Ext.Web.TreeNode();
                node.NodeID = l_TreeDt.Rows[i]["CLASSID"].ToString();
                node.SingleClickExpand = true;
                node.Text = l_TreeDt.Rows[i]["CLASSNAME"].ToString();
                node.Icon = (Icon)Enum.Parse(typeof(Icon), "Folder");
                string pid = l_TreeDt.Rows[i]["CLASSPID"].ToString();
                nodekey = pid;
                if (rootnode.Contains(nodekey))
                {
                    Goldnet.Ext.Web.TreeNode pnode = (Goldnet.Ext.Web.TreeNode)rootnode[nodekey];
                    pnode.Nodes.Add(node);
                }
                else
                {
                    root.Nodes.Add(node);
                }
                rootnode.Add(node.NodeID, node);
            }
            Goldnet.Ext.Web.TreeNodeCollection tnodec = new Goldnet.Ext.Web.TreeNodeCollection();
            tnodec.Add(root);

            return tnodec.ToJson();
        }
        //查询绑定数据
        protected void Bindlist()
        {
            this.GridPanel1.Dispose();
            this.Store1.Dispose();
            Cbhs_dict dal = new Cbhs_dict();
            DataTable dt = dal.GetCostItem().Tables[0];
            DataTable item_power = new DataTable();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                item_power = dal.GetCostItemPower(dt.Rows[i]["item_code"].ToString()).Tables[0];
                StringBuilder user_power = new StringBuilder();
                //补所属用户列
                for (int j = 0; j < item_power.Rows.Count; j++)
                {
                    if (j == 0)
                    {
                        user_power.Append(item_power.Rows[j]["user_name"]);
                    }
                    else
                    {
                        user_power.Append(","+item_power.Rows[j]["user_name"]);
                    }
                }
                dt.Rows[i]["item_power"]=user_power.ToString();
            }
            this.Store1.DataSource = dt;
            this.Store1.DataBind();
        }
        //刷新store
        protected void Store_RefreshData(object sender, StoreRefreshDataEventArgs e)
        {
            Bindlist();
        }
        //添加
        protected void Button_add_click(object sender, AjaxEventArgs e)
        {
           
            LoadConfig loadcfg = getLoadConfig("cost_item_set.aspx");
            loadcfg.Params.Add(new Goldnet.Ext.Web.Parameter("op", "add"));
            loadcfg.Params.Add(new Goldnet.Ext.Web.Parameter("id",  e.ExtraParams["Values"].ToString()));
            showDetailWin(loadcfg);

        }
        //修改
        protected void Button_edit_click(object sender, EventArgs e)
        {
            RowSelectionModel sm = this.GridPanel1.SelectionModel.Primary as RowSelectionModel;
            if (sm.SelectedRows.Count < 1)
            {
                Ext.Msg.Alert("提示", "请选择一条记录！").Show();
            }
            else
            {
                string item_code = sm.SelectedRow.RecordID;
                LoadConfig loadcfg = getLoadConfig("cost_item_set.aspx");
                loadcfg.Params.Add(new Goldnet.Ext.Web.Parameter("op", "edit"));
                loadcfg.Params.Add(new Goldnet.Ext.Web.Parameter("item_code", item_code));
                showDetailWin(loadcfg);
            }
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
                LoadConfig loadcfg = getLoadConfig("cost_item_set.aspx");
                loadcfg.Params.Add(new Goldnet.Ext.Web.Parameter("op", "del"));
                loadcfg.Params.Add(new Goldnet.Ext.Web.Parameter("item_code", item_code));
                showDetailWin(loadcfg);
            }
        }
        //修改类别
        protected void Button_editType_click(object sender, EventArgs e)
        {
            LoadConfig loadcfg = getLoadConfig("cost_item_type.aspx");
            TypeWin.Center();
            TypeWin.ClearContent();
            TypeWin.Show();
            TypeWin.LoadContent(loadcfg);
        }
        //刷新
        protected void Button_refresh_click(object sender, EventArgs e)
        {
            Bindlist();
        }
        //显示弹出设置窗口
        private void showDetailWin(LoadConfig loadcfg)
        {
        	DetailWin.Center();
            DetailWin.ClearContent();
            DetailWin.Show();
            DetailWin.LoadContent(loadcfg);
        }
        //显示弹出权限窗口
        private void showPowerWin(LoadConfig loadcfg)
        {
            PowerWin.ClearContent();
            PowerWin.Show();
            PowerWin.LoadContent(loadcfg);
        }

        //载入参数设置
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
        //设置权限
        protected void SetPower(object sender,  AjaxEventArgs e)
        {
            string item_code = e.ExtraParams["Values"].ToString();
            LoadConfig loadcfg = getLoadConfig("cost_item_power.aspx");
            loadcfg.Params.Add(new Goldnet.Ext.Web.Parameter("item_code", item_code));
            showPowerWin(loadcfg);
        }
    }
}
