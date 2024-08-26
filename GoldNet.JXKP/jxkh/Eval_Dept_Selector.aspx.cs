using System;
using Goldnet.Ext.Web;
using System.Data;
using System.Collections;

namespace GoldNet.JXKP.jxkh
{
    public partial class Eval_Dept_Selector : System.Web.UI.Page
    {
        Goldnet.Dal.Appraisal dal = new Goldnet.Dal.Appraisal();

        /// <summary>
        /// 初始化处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["CURRENTSTAFF"] == null)
            {
                Response.End();
            }
            if (!Ext.IsAjaxRequest)
            {
                BuildTree();
            }
        }

        //构造科室类别树
        protected void BuildTree()
        {
            Goldnet.Ext.Web.TreeNode root = new Goldnet.Ext.Web.TreeNode();
            root.NodeID = "root";
            root.Text = "科室列表";
            root.Expanded = true;
            this.TreeCtrl.Root.Clear();
            this.TreeCtrl.Root.Add(root);

            DataTable l_TreeDt = dal.GetDeptClassTree().Tables[0];
            Hashtable rootnode = new Hashtable();

            for (int i = 0; i < l_TreeDt.Rows.Count; i++)
            {
                string nodekey = "";
                Goldnet.Ext.Web.TreeNode node = new Goldnet.Ext.Web.TreeNode();
                node.NodeID = l_TreeDt.Rows[i]["id"].ToString();
                node.SingleClickExpand = true;
                node.Text = l_TreeDt.Rows[i]["name"].ToString();
                node.Icon = (Icon)Enum.Parse(typeof(Icon), "Folder");
                node.Checked = Goldnet.Ext.Web.ThreeStateBool.False;
                string pid = l_TreeDt.Rows[i]["pid"].ToString();
                nodekey = pid;
                if (rootnode.Contains(nodekey))
                {
                    Goldnet.Ext.Web.TreeNode pnode = (Goldnet.Ext.Web.TreeNode)rootnode[nodekey];
                    pnode.Nodes.Add(node);
                }
                else
                {
                    root.Nodes.Add(node);
                    rootnode.Add(node.NodeID, node);
                }

            }
        }

        //树选择改变时，填充左侧的选择列表
        protected void TreeChangeChecked(object sender, AjaxEventArgs e)
        {
            //科室权限过滤条件
            string deptFilter = Request.QueryString["deptFilter"].ToString();
            string classlist = e.ExtraParams["checkNodes"].ToString().TrimEnd(',');
            if (classlist.Trim().Equals(""))
            {
                this.Store1.RemoveAll();
                return;
            }
            DataTable l_dt = dal.GetDeptLeftSelector(classlist, deptFilter).Tables[0];
            string multi1 = e.ExtraParams["multi1"];
            Goldnet.Ext.Web.ListItem[] items1 = JSON.Deserialize<Goldnet.Ext.Web.ListItem[]>(multi1);
            ArrayList array = new ArrayList();
            for (int i = 0; i < items1.Length; i++)
            {
                array.Add(items1[i].Value.ToString());
            }
            for (int i = 0; i < l_dt.Rows.Count; i++)
            {
                if (array.Contains(l_dt.Rows[i]["DEPT_CODE"].ToString()))
                {
                    l_dt.Rows.RemoveAt(i);
                    i--;
                }

            }
            this.Store1.DataSource = l_dt;
            this.Store1.DataBind();
        }

    }
}
