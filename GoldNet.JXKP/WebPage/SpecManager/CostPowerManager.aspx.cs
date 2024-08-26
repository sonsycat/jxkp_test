using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using Goldnet.Dal;
using Goldnet.Ext.Web;
using System.Text;

namespace GoldNet.JXKP.WebPage.SpecManager
{
    public partial class CostPowerManager : PageBase
    {
        private SYS_ROLE_DICT dal_type = new SYS_ROLE_DICT();
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
            DataTable dt = dal_type.GetCostItem("").Tables[0];
            DataTable item_power = new DataTable();
            this.Store1.DataSource = dt;
            this.Store1.DataBind();
        }
        //刷新store
        protected void Store_RefreshData(object sender, StoreRefreshDataEventArgs e)
        {
            Bindlist();
        }
       
       
        //刷新
        protected void Button_refresh_click(object sender, EventArgs e)
        {
            Bindlist();
        }
     
        //显示弹出权限窗口
        private void showPowerWin(LoadConfig loadcfg)
        {
            PowerWin.ClearContent();
            PowerWin.Show();
            PowerWin.LoadContent(loadcfg);
        }

       
        //设置权限
        protected void SetPower(object sender, AjaxEventArgs e)
        {
            string item_code = e.ExtraParams["Values"].ToString();
            LoadConfig loadcfg = getLoadConfig("cost_item_power.aspx");
            loadcfg.Params.Add(new Goldnet.Ext.Web.Parameter("item_code", item_code));
            showPowerWin(loadcfg);
        }
    }
}
