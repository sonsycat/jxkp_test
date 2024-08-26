using System;
using System.Collections;
using System.Configuration;
using System.Data;
using Goldnet.Dal;
using Goldnet.Ext.Web;
using GoldNet.Comm;
using GoldNet.Model;

namespace GoldNet.JXKP.GuideLook
{
    public partial class ViewAnalyseReport : System.Web.UI.Page
    {
        #region --页面初始化--

        public static Goldnet.Ext.Web.TreeNode root = null;

        #endregion


        #region --页面事件--
        protected void Page_Load(object sender, EventArgs e)
        {

            //检查是否已经登录，否则停止
            //if (Session["CURRENTSTAFF"] == null)
            //{
            //    Response.End();
            //    return;
            //}

            if (!Ext.IsAjaxRequest)
            {
                setTimeContrl();
                root = new Goldnet.Ext.Web.TreeNode();

                root.NodeID = "root";

                root.Text = "分析报表";

                root.Expanded = true;

                this.TreeCtrl.Root.Add(root);

                TreeBuild();
            }
        }


        /// <summary>
        /// 点击查询按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void GetQueryPortalet(object sender, AjaxEventArgs e)
        {
            string id = e.ExtraParams["NodeId"].ToString().Replace("\"", "");

            string name = e.ExtraParams["NodeName"].ToString().Replace("\"", "");

            this.Store1.RemoveAll();

            if (id != "" && id.IndexOf("CLASS") != 0)
            {
                DataTable kdept = new DataTable();

                string incount = ((User)Session["CURRENTSTAFF"]).StaffId;

                if (incount == "" || incount == null) incount = "NotUserid";

                string year = this.Comb_StartYear.SelectedItem.Value.ToString();

                string Month = this.Comb_StartMonth.SelectedItem.Value.ToString();

                ReportDalDict dal = new ReportDalDict();

                DataTable l_dt = dal.getAnalyseReportGuideByRptId(id).Tables[0];

                if (l_dt.Rows.Count > 0)
                {
                    //运行报表存储过程
                    DataTable Rptdt = dal.getAnalyseReport(incount, id).Tables[0];
                    this.GridPanel_Show.Title = "报表信息----" + name + "日期：" + year + "年" + Month+"月";
                    this.Store1.DataSource = Rptdt;
                    this.Store1.DataBind();
                }
                else
                {
                    //Ext.Msg.Alert("系统提示", "请先定义报表！",MessageBox.Icon.INFO).Show();
                    Goldnet.Ext.Web.Ext.Msg.Show(new Goldnet.Ext.Web.MessageBox.Config
                    {
                        Title = SystemMsg.msgtitle4,
                        Message = "请先定义报表！",
                        Buttons = MessageBox.Button.OK,
                        Icon = (MessageBox.Icon)Enum.Parse(typeof(MessageBox.Icon), "INFO")
                    });
                    return;
                }
            }
        }

        #endregion


        #region --页面逻辑--

        /// <summary>
        /// 树结构初始化
        /// </summary>
        private void TreeBuild()
        {
            TreeDalDict dal = new TreeDalDict();
            DataTable l_TreeDt = dal.getAnalyseReportViewTree().Tables[0];
            Hashtable rootnode = new Hashtable();
            for (int i = 0; i < l_TreeDt.Rows.Count; i++)
            {
                string nodekey = "";
                Goldnet.Ext.Web.TreeNode node = new Goldnet.Ext.Web.TreeNode();
                node.NodeID = l_TreeDt.Rows[i]["ID"].ToString();

                node.Text = l_TreeDt.Rows[i]["CLASSNAME"].ToString();
                node.Icon = (Icon)Enum.Parse(typeof(Icon), "Folder");
                node.SingleClickExpand = true;
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

        }


        /// <summary>
        /// 时间转化
        /// </summary>
        /// <param name="time">190001</param>
        /// <param name="split">'-'/'.'/'/'</param>
        /// <returns></returns>
        private string ConvertStringToTime(string time, string split)
        {
            string y = time.Substring(0, 4);

            string m = time.Substring(4);

            time = y + split + m;

            return time;

        }

        /// <summary>
        /// 初始化时间空件
        /// </summary>
        private void setTimeContrl() 
        {
            for (int i = 0; i < 10; i++)
            {
                int years = System.DateTime.Now.Year - i;
                this.Comb_StartYear.Items.Add(new Goldnet.Ext.Web.ListItem(years.ToString(), years.ToString()));
            }
            for (int i = 1; i <= 12; i++)
            {
                this.Comb_StartMonth.Items.Add(new Goldnet.Ext.Web.ListItem(i.ToString(), i.ToString()));
            }
            this.Comb_StartMonth.SelectedIndex = DateTime.Now.Month - 1;
        }
        #endregion

    }
}
