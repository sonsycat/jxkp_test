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
using System.Xml.Linq;
using Goldnet.Ext.Web;
using Goldnet.Dal;
using System.Collections.Generic;


namespace GoldNet.JXKP
{
    public partial class cost_item_type : PageBase
    {
        private Cost_Item_Type dal_type = new Cost_Item_Type();
        protected void Page_Load(object sender, EventArgs e)
        {
            //检查是否已经登录，否则停止
            if (Session["CURRENTSTAFF"] == null)
            {
                //     Response.End();
            }
            if (!Ext.IsAjaxRequest)
            {
                TreeInitBuild();
            }
        }
        /// <summary>
        /// 初始化树结构
        /// </summary>
        private void TreeInitBuild()
        {
            Hashtable rootnode = new Hashtable();
            DataTable dt = dal_type.GetCostType().Tables[0];
            Goldnet.Ext.Web.TreeNode root = new Goldnet.Ext.Web.TreeNode();
            root.NodeID = "0";
            root.Text = "项目类别";
            root.Expanded = true;
            root.Leaf = false;
            root.IsTarget = true;
            this.TreeCtrl.Root.Add(root);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                string nodeid = dt.Rows[i]["CLASSID"].ToString();
                string nodetext = dt.Rows[i]["CLASSNAME"].ToString();
                string attr = dt.Rows[i]["LEV"].ToString();
                string pid = dt.Rows[i]["CLASSPID"].ToString();
                string isleaf = dt.Rows[i]["ISLEAF"].ToString();
                Goldnet.Ext.Web.TreeNode node1 = new Goldnet.Ext.Web.TreeNode();
                node1.NodeID = nodeid;
                node1.Text = nodetext;
                node1.IsTarget = true;
                node1.Leaf = isleaf.Equals("0") ? false : true;
                // node1.SingleClickExpand = true;
                if (isleaf.Equals("1"))
                {
                    node1.Icon = (Icon)Enum.Parse(typeof(Icon), "Folder");
                }
                if (rootnode.Contains(pid))
                {
                    Goldnet.Ext.Web.TreeNode pnode = (Goldnet.Ext.Web.TreeNode)rootnode[pid];
                    pnode.Nodes.Add(node1);
                }
                else
                {
                    root.Nodes.Add(node1);
                }
                if (isleaf.Equals("0"))
                {
                    rootnode.Add(nodeid, node1);
                }
            }
        }
        /// <summary>
        /// 删除节点方法，传入节点ID，判断是否可以删除
        /// </summary>
        /// <param name="nodeid"></param>
        /// <returns></returns>
        [AjaxMethod]
        public string DelTreeNode(string nodeid)
        {
            string result = "";

            if (dal_type.EstimateData(nodeid) && dal_type.EstimatePData(nodeid))
            {
                dal_type.DelCostType(nodeid);
            }
            else
            {
                result = "类别被使用,不可以被删除";
            }
            return result;
        }

        /// <summary>
        /// 传入树节点JSON, 更新树
        /// </summary>
        /// <returns></returns>
        [AjaxMethod]
        public string UpdateTree(string treeNodesJSON)
        {
            Dictionary<string, string>[] treeNodes = JSON.Deserialize<Dictionary<string, string>[]>(treeNodesJSON);
            dal_type.UpdateCostType(treeNodes);
            return "";
        }
    }
}
