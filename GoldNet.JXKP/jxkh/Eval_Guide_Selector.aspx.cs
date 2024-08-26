using System;
using System.Data;
using GoldNet.Model;
using Goldnet.Ext.Web;
using System.Collections;

namespace GoldNet.JXKP.jxkh
{
    public partial class Eval_Guide_Selector : PageBase
    {
        Goldnet.Dal.Appraisal dal = new Goldnet.Dal.Appraisal();
        string organ = "02";

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
            if (this.Request["organ"] != null)
            {
                organ = this.Request["organ"].ToString().Equals("R") ? "03" : "02";
            }
            if (!Ext.IsAjaxRequest)
            {
                BindOrganClass(organ);
                bool isEdit = Boolean.Parse(Request.QueryString["isEdit"]);
                BindGatherTypeCombox();
                if (isEdit)
                {
                    BuildGuideTree(organ);
                }
            }
        }

        //构造指标树
        protected void BuildGuideTree(string organ)
        {

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
            DataTable dt = dal.GetGuideDictListByBscOrgDept(bsc, depttype, organ).Tables[0];
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

        //评价对象类别绑定
        protected void BindOrganClass(string organ)
        {
            this.Combo_Org.Value = organ;
        }

        /// <summary>
        /// 绑定指标集类别绑定
        /// </summary>
        protected void BindGatherTypeCombox()
        {
            User user = (User)Session["CURRENTSTAFF"];
            string organ = this.Request["organ"].ToString().Equals("R") ? "03" : "02";
            DataTable dt = dal.GetEvalGuideGroupTypeClass(user.UserId, organ).Tables[0];
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                this.Combo_GuideGroupClass.Items.Add(new Goldnet.Ext.Web.ListItem(dt.Rows[i]["GUIDE_GROUP_TYPE_NAME"].ToString(), dt.Rows[i]["GUIDE_GROUP_TYPE"].ToString()));
            }
        }

        /// <summary>
        /// 指标集类别选择刷新
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void GuideGroupClass_Selected(object sender, AjaxEventArgs e)
        {
            User user = (User)Session["CURRENTSTAFF"];
            string typestr = this.Combo_GuideGroupClass.SelectedItem.Value.ToString();
            //绑定评价指标集
            DataTable dt = dal.GetEvalGuideGather(typestr, organ, user.UserId).Tables[0];
            this.Combo_GuideGather.Value = "";
            this.StoreCombo1.DataSource = dt;
            this.StoreCombo1.DataBind();
        }

        /// <summary>
        /// 指标集下拉选择事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void GuideGather_Selected(object sender, AjaxEventArgs e)
        {
            string guidecode = this.Combo_GuideGather.SelectedItem.Value.ToString();
            if (guidecode.Equals(""))
            {
                return;
            }
            this.Store1.RemoveAll();
            this.Store2.RemoveAll();
            DataTable dt = dal.GetGuideGroupList(guidecode).Tables[0];
            this.Store2.DataSource = dt;
            this.Store2.DataBind();
        }

    }
}
