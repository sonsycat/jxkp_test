using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Text;
using GoldNet.Comm;
using Goldnet.Dal;
using Goldnet.Ext.Web;

namespace GoldNet.JXKP.WebPage.SysManager
{
    public partial class Sys_Menu_Guide :PageBase
    {
        #region --页面初始化--
        /// <summary>
        /// 选择报表ID
        /// </summary>
        private static string m_rptid = null;

        #endregion

        #region --页面事件--

        protected void Page_Load(object sender, EventArgs e)
        {
            //检查是否已经登录，否则停止
            if (Session["CURRENTSTAFF"] == null)
            {
                Response.End();
                return;
            }

            SE_ROLE dal = new SE_ROLE();

            if (!Ext.IsAjaxRequest)
            {
                //绑定科室类别
                DataTable l_DeptTypedt = dal.DeptStore().Tables[0];

                for (int i = 0; i < l_DeptTypedt.Rows.Count; i++)
                {
                    this.Combo_DeptType.Items.Add(new Goldnet.Ext.Web.ListItem(l_DeptTypedt.Rows[i]["TEXT"].ToString(), l_DeptTypedt.Rows[i]["ID"].ToString()));

                }

                //默认临床
                this.Combo_DeptType.Value = "20";


                //保存数据库的情况，显示已选择项
                string rptid = Request.QueryString["APP_MENU_ID"].ToString();

                m_rptid = rptid;

                //指标现有项
                DataTable l_ExRptCol = dal.getReportGuideExCol(rptid).Tables[0];

                if (l_ExRptCol.Rows.Count > 0)
                {
                    this.Store2.DataSource = l_ExRptCol;
                    this.Store2.DataBind();
                }

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
            root.Text = "指标体系";
            root.Expanded = true;

            string DeptType = this.Combo_DeptType.SelectedItem.Value;

            SE_ROLE dal = new SE_ROLE();

            DataTable l_TreeDt = dal.getSetReportGuideTreeBuilder(DeptType).Tables[0];

            Hashtable rootnode = new Hashtable();

            for (int i = 0; i < l_TreeDt.Rows.Count; i++)
            {
                string nodekey = "";
                Goldnet.Ext.Web.TreeNode node = new Goldnet.Ext.Web.TreeNode();
                node.SingleClickExpand = true;
                node.NodeID = l_TreeDt.Rows[i]["id"].ToString();
                node.Text = l_TreeDt.Rows[i]["name"].ToString();
                node.Icon = (Icon)Enum.Parse(typeof(Icon), "Folder");
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
                }
                rootnode.Add(node.NodeID, node);
            }

            Goldnet.Ext.Web.TreeNodeCollection tnodec = new Goldnet.Ext.Web.TreeNodeCollection();
            tnodec.Add(root);

            return tnodec.ToJson();
        }
        /// <summary>
        /// 点击树节点事件
        /// </summary>
        /// <param name="bsc">父节点ID</param>
        /// <param name="selectedid">右侧选择项目</param>
        [AjaxMethod]
        public void TreeClick(string bsc, string selectedid)
        {

            SE_ROLE dal = new SE_ROLE();

            this.Store1.RemoveAll();

            //树接点值
            BindLeftSelector(bsc, this.Combo_DeptType.SelectedItem.Value.ToString(), "", selectedid);
        }

        /// <summary>
        /// 查询指标
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void SearchGuide(object sender, AjaxEventArgs e)
        {

            if (this.txtTagName.Text == "")
            {
                Goldnet.Ext.Web.Ext.Msg.Show(new Goldnet.Ext.Web.MessageBox.Config
                {
                    Title = "提示信息",
                    Message = "请先输入查询内容",
                    Buttons = MessageBox.Button.OK,
                    Icon = (MessageBox.Icon)Enum.Parse(typeof(MessageBox.Icon), "INFO")
                });
                return;
            }

            this.Store1.RemoveAll();
            string selectedid = e.ExtraParams["multi1"];
            BindLeftSelector("", this.Combo_DeptType.SelectedItem.Value.ToString(), this.txtTagName.Text, selectedid);
        }
        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void SaveGuide(object sender, AjaxEventArgs e)
        {

            string multi1 = e.ExtraParams["multi1"];
            Goldnet.Ext.Web.ListItem[] items1 = JSON.Deserialize<Goldnet.Ext.Web.ListItem[]>(multi1);

            DataTable tb= CreateTableForSelectRightItem(items1);
            SE_ROLE dal = new SE_ROLE();
            string rptid = Request.QueryString["APP_MENU_ID"].ToString();
            try
            {
                dal.SaveSysmenuguide(tb, rptid);
                this.ShowMessage("提示", "保存成功！");
            }
            catch (Exception ex)
            {
                this.ShowMessage("提示","保存失败："+ex.Message);
            }
           

        }

       

        #endregion

        #region --页面逻辑--
        /// <summary>
        /// 绑字左侧选择列表
        /// </summary>
        /// <param name="bsc"></param>
        /// <param name="organtype"></param>
        /// <param name="depttype"></param>
        /// <param name="tag"></param>
        /// <param name="selectedid"></param>
        protected void BindLeftSelector(string bsc, string depttype, string tag, string selectedid)
        {
            SE_ROLE dal = new SE_ROLE();
            Goldnet.Ext.Web.ListItem[] items1 = JSON.Deserialize<Goldnet.Ext.Web.ListItem[]>(selectedid);
            ArrayList array = new ArrayList();
            for (int i = 0; i < items1.Length; i++)
            {
                array.Add(items1[i].Value.ToString());
            }
            DataTable dt = dal.getReportSearchGuide(this.Combo_DeptType.SelectedItem.Value.ToString(), bsc, tag).Tables[0];
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (array.Contains(dt.Rows[i]["VALUE"].ToString()))
                {
                    dt.Rows.RemoveAt(i);
                    i--;
                }
            }
            Store1.DataSource = dt;
            Store1.DataBind();
        }

        /// <summary>
        /// 创建选择指标集合ListItem变换成TABLE
        /// </summary>
        /// <param name="RightItems">选择指标集合</param>
        /// <returns>选择指标集合</returns>
        private DataTable CreateTableForSelectRightItem(Goldnet.Ext.Web.ListItem[] RightItems)
        {
            DataTable l_dt = new DataTable();
            l_dt.Columns.Add("TEXT");
            l_dt.Columns.Add("VALUE");
            l_dt.Columns.Add("INDEX");

            for (int i = 0; i < RightItems.Length; i++)
            {
                DataRow l_dr = l_dt.NewRow();

                l_dr[0] = RightItems[i].Text;
                l_dr[1] = RightItems[i].Value;
                l_dr[2] = i;

                l_dt.Rows.Add(l_dr);

            }
            return l_dt;

        }

        #endregion
    }
}
