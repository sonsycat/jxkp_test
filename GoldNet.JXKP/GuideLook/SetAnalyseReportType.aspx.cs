using System;
using System.Collections;
using System.Configuration;
using System.Data;
using Goldnet.Ext.Web;
using Goldnet.Dal;
using System.Collections.Generic;

namespace GoldNet.JXKP.GuideLook
{
    public partial class SetAnalyseReportType : System.Web.UI.Page
    {
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
            TreeDalDict dal = new TreeDalDict();
            DataTable dt = dal.getAnalyseReportTypeTreeBuilder().Tables[0];
            Goldnet.Ext.Web.TreeNode root = new Goldnet.Ext.Web.TreeNode();
            root.NodeID = "0";
            root.Text = "分析报表类别";
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
            ReportDalDict dal = new ReportDalDict();
            if (dal.EstimateData(nodeid, "ANALYSISRPT_LIST_DICT"))
            {
                dal.DelReportTypeCenterDict(nodeid, "ANALYSISRPT_CLASS_DICT");
            }
            else
            {
                result = "该类别下已经设置了报表，请先删除报表后再删除该类别!";
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
            ReportDalDict dal = new ReportDalDict();
            dal.UpdateReportTypeCenterDict(treeNodes, "ANALYSISRPT_CLASS_DICT");
            return "";
        }
    }
}
