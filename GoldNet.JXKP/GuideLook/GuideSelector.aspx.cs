using System;
using System.Collections;
using System.Configuration;
using System.Data;
using Goldnet.Dal;
using Goldnet.Ext.Web;

namespace GoldNet.JXKP.GuideLook
{
    public partial class GuideSelector : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Ext.IsAjaxRequest)
            {
                BuildGuideTree("02");
            }
        }

        //构造指标树
        protected void BuildGuideTree(string organ)
        {
            GuideDalDict dal = new GuideDalDict();
            Goldnet.Ext.Web.TreeNode root = new Goldnet.Ext.Web.TreeNode();
            root.NodeID = "root";
            root.Text = "指标列表";
            root.Expanded = true;

            Goldnet.Ext.Web.TreeNode root1 = new Goldnet.Ext.Web.TreeNode();
            root1.NodeID = "rootbsc";
            root1.Text = "BSC指标分类";
            root1.Expanded = true;
            root1.Icon = Icon.Rgb;

            Goldnet.Ext.Web.TreeNode root2 = new Goldnet.Ext.Web.TreeNode();
            root2.NodeID = "rootdept";
            root2.Text = "科室指标分类";
            root2.Expanded = true;
            root2.Icon = Icon.UserHome;

            this.TreeCtrl.Root.Clear();
            this.TreeCtrl.Root.Add(root);

            root.Nodes.Add(root1);
            root.Nodes.Add(root2);

            //添加BSC类别树
            DataTable dt = dal.getBSCGuideTree(organ).Tables[0];
            Hashtable rootnode = new Hashtable();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                string nodekey = "";
                Goldnet.Ext.Web.TreeNode node = new Goldnet.Ext.Web.TreeNode();
                node.SingleClickExpand = true;
                node.NodeID = dt.Rows[i]["id"].ToString();
                node.Text = dt.Rows[i]["name"].ToString();
                node.Icon = Icon.Folder;
                //node.Icon = (Icon)Enum.Parse(typeof(Icon), "Folder");
                string pid = dt.Rows[i]["pid"].ToString();
                nodekey = pid;
                if (rootnode.Contains(nodekey))
                {
                    Goldnet.Ext.Web.TreeNode pnode = (Goldnet.Ext.Web.TreeNode)rootnode[nodekey];
                    pnode.Nodes.Add(node);
                }
                else
                {
                    root1.Nodes.Add(node);
                    rootnode.Add(node.NodeID, node);
                }
            }

            //添加科室类别树
            dt = dal.getDeptGuideTree().Tables[0];
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                Goldnet.Ext.Web.TreeNode node = new Goldnet.Ext.Web.TreeNode();
                node.SingleClickExpand = true;
                node.NodeID = dt.Rows[i]["DEPT_CLASS_CODE"].ToString();
                node.Text = dt.Rows[i]["DEPT_CLASS_NAME"].ToString();
                node.Icon = Icon.Folder;
                root2.Nodes.Add(node);
            }
        }

        //树节点选择，更新左侧列表
        protected void TreeNodeSelected(object sender, AjaxEventArgs e)
        {
            GuideDalDict dal = new GuideDalDict();
            string nodeid = e.ExtraParams["nodeid"].ToString();
            string selectedid = e.ExtraParams["multi1"].ToString();
            string bsc = "";
            string depttype = "";
            if (nodeid.Length.Equals(4))
            {
                bsc = nodeid;
            }
            else
            {
                depttype = nodeid.Substring(1);
            }

            Goldnet.Ext.Web.ListItem[] items1 = JSON.Deserialize<Goldnet.Ext.Web.ListItem[]>(selectedid);
            ArrayList array = new ArrayList();
            for (int i = 0; i < items1.Length; i++)
            {
                array.Add(items1[i].Value.ToString());
            }
            DataTable dt = dal.GetGuideDictListByBscOrgDept(bsc, depttype, "02").Tables[0];
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (array.Contains(dt.Rows[i]["GUIDE_CODE"].ToString()))
                {
                    dt.Rows.RemoveAt(i);
                    i--;
                }
            }
            Store1.DataSource = dt;
            Store1.DataBind();
        }
    }
}
