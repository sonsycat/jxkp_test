using System;
using System.Collections;
using System.Configuration;
using System.Data;
using Goldnet.Dal;
using Goldnet.Ext.Web;

namespace GoldNet.JXKP.GuideLook
{
    public partial class Disease_Analyse : System.Web.UI.Page
    {
        /// <summary>
        /// 树节点名称
        /// </summary>
        private string m_TreeName = "病种";

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
                this.diseasetype.Value = "DEPT_OPERATION_DICT";
            }
        }

        /// <summary>
        /// 刷新树 Json 获取
        /// </summary>
        /// <returns></returns>
        [AjaxMethod]
        public string RefreshMenu()
        {

            Goldnet.Ext.Web.TreeNode root = new Goldnet.Ext.Web.TreeNode();
            root.NodeID = "root";
            root.Text = m_TreeName;
            root.Expanded = true;

            string DeptType = this.diseasetype.SelectedItem.Value;

            TreeDalDict dal = new TreeDalDict();

            DataTable l_TreeDt = dal.getDiseaseAnalyseOperationTreeBuilder(DeptType).Tables[0];

            Hashtable rootnode = new Hashtable();

            for (int i = 0; i < l_TreeDt.Rows.Count; i++)
            {
                string nodekey = "";
                Goldnet.Ext.Web.TreeNode node = new Goldnet.Ext.Web.TreeNode();
                node.SingleClickExpand = true;
                node.NodeID = l_TreeDt.Rows[i]["ID"].ToString();
                node.Text = l_TreeDt.Rows[i]["NAME"].ToString();
                node.Icon = (Icon)Enum.Parse(typeof(Icon), "Folder");
                string pid = l_TreeDt.Rows[i]["P_ID"].ToString();
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
    }
}
