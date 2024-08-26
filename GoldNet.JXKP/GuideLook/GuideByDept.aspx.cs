using System;
using System.Collections;
using System.Configuration;
using System.Data;
using GoldNet.Model;
using Goldnet.Ext.Web;
using Goldnet.Dal;
using System.Web.UI.WebControls;

namespace GoldNet.JXKP.GuideLook
{
    public partial class GuideByDept : System.Web.UI.Page
    {
        #region --页面初始化--

        public static Goldnet.Ext.Web.TreeNode root = null;

        private static string m_incount = null;

        #endregion

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
                setTimeContrl();
                m_incount = ((User)Session["CURRENTSTAFF"]).StaffId;

                if (m_incount == "" || m_incount == null) m_incount = "NotUserid";

                root = new Goldnet.Ext.Web.TreeNode();

                root.NodeID = "root";

                root.Text = "科室结构";

                root.Expanded = true;

                this.TreeCtrl.Root.Add(root);

                TreeBuild();
            }
        }

        /// <summary>
        /// 初始化时间空件
        /// </summary>
        private void setTimeContrl()
        {
            //年度下拉框内容
            for (int i = 0; i < 10; i++)
            {
                int years = System.DateTime.Now.Year - i;
                this.Combo_StationYear.Items.Add(new Goldnet.Ext.Web.ListItem(years.ToString() + "年", years.ToString()));
            }
            this.Combo_StationYear.Value = System.DateTime.Now.Year.ToString();
        }

        /// <summary>
        /// 树结构初始化
        /// </summary>
        private void TreeBuild()
        {
            TreeDalDict dal = new TreeDalDict();

            DataTable l_TreeDt = dal.getSatffForGuideLook("","","").Tables[0];

            Hashtable rootnode = new Hashtable();

            for (int i = 0; i < l_TreeDt.Rows.Count; i++)
            {
                string nodekey = "";
                Goldnet.Ext.Web.TreeNode node = new Goldnet.Ext.Web.TreeNode();
                node.NodeID = l_TreeDt.Rows[i]["ID"].ToString();

                node.Text = l_TreeDt.Rows[i]["NAME"].ToString();
                node.Icon = (Icon)Enum.Parse(typeof(Icon), "Folder");
                node.SingleClickExpand = true;
                string pid = l_TreeDt.Rows[i]["PID"].ToString();
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
        }

        //选择评价指标
        protected void Btn_SelectGuide_Click(object sender, AjaxEventArgs e)
        {
            showDetailWin(getLoadConfig("GuideSelector.aspx"), "选择科室指标", "760");
        }

        //显示详细窗口
        private void showDetailWin(LoadConfig loadcfg, string title, string width)
        {
            DetailWin.ClearContent();
            if (!title.Trim().Equals(""))
            {
                DetailWin.SetTitle(title);
            }
            if (!width.Trim().Equals(""))
            {
                DetailWin.Width = Unit.Pixel(Convert.ToInt16(width));
            }
            DetailWin.Center();
            DetailWin.Show();
            DetailWin.LoadContent(loadcfg);
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
    }
}
