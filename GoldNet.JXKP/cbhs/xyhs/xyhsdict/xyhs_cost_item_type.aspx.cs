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

namespace GoldNet.JXKP.cbhs.xyhs.xyhsdict
{
    public partial class xyhs_cost_item_type : System.Web.UI.Page
    {
        private Cost_Item_Type dal_type = new Cost_Item_Type();

        protected void Page_Load(object sender, EventArgs e)
        {
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
            XyhsDict dal = new XyhsDict();
            DataSet ds = dal.GetCostType();
            ds.Relations.Add("TreeRelation", ds.Tables[0].Columns["ITEM_CODE"], ds.Tables[0].Columns["CLASSPID"]);

            foreach (DataRow row in ds.Tables[0].Rows)
            {
                if (row.IsNull("CLASSPID") )
                {
                    string nodeid = row["ITEM_CODE"].ToString();
                    string nodetext = row["ITEM_NAME"].ToString();
                    string attr = row["LEV"].ToString();
                    string pid = row["CLASSPID"].ToString();
                    string isleaf = row["ISLEAF"].ToString();
                    Goldnet.Ext.Web.TreeNode node1 = new Goldnet.Ext.Web.TreeNode();
                    node1.NodeID = nodeid;
                    node1.Text = nodetext;
                    node1.IsTarget = true;
                    node1.Leaf = isleaf.Equals("0") ? false : true;
                    if (isleaf.Equals("1"))
                    {
                        node1.Icon = (Icon)Enum.Parse(typeof(Icon), "Folder");
                    }

                    this.TreeCtrl.Root.Add(node1);

                    ResolveSubTree(row, node1);
                }
            }


        }

        private void ResolveSubTree(DataRow dataRow, Goldnet.Ext.Web.TreeNode treeNode)
        {
            DataRow[] rows = dataRow.GetChildRows("TreeRelation");
            if (rows.Length > 0)
            {
                treeNode.Expanded = true;
                foreach (DataRow row in rows)
                {
                    string nodeid = row["ITEM_CODE"].ToString();
                    string nodetext = row["ITEM_NAME"].ToString();
                    string attr = row["LEV"].ToString();
                    string pid = row["CLASSPID"].ToString();
                    string isleaf = row["ISLEAF"].ToString();
                    Goldnet.Ext.Web.TreeNode node1 = new Goldnet.Ext.Web.TreeNode();
                    node1.NodeID = nodeid;
                    node1.Text = nodetext;
                    node1.IsTarget = true;
                    node1.Leaf = isleaf.Equals("0") ? false : true;
                    if (isleaf.Equals("1"))
                    {
                        node1.Icon = (Icon)Enum.Parse(typeof(Icon), "Folder");
                    }

                    treeNode.Nodes.Add(node1);

                    ResolveSubTree(row, node1);
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
            XyhsDict dal = new XyhsDict();
            string result = "";

            if (dal.EstimateData(nodeid) && dal.EstimatePData(nodeid))
            {
                dal.DelCostType(nodeid);
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
            XyhsDict dal = new XyhsDict();
            Dictionary<string, string>[] treeNodes = JSON.Deserialize<Dictionary<string, string>[]>(treeNodesJSON);
            dal.UpdateCostType(treeNodes);
            return "";
        }





    }
}
