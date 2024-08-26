using System;
using System.Collections;
using System.Data;
using Goldnet.Ext.Web;
using GoldNet.Comm;
using System.Collections.Generic;
using Goldnet.Dal;
using GoldNet.Model;

namespace GoldNet.JXKP.GuideLook
{
    public partial class SetReport : System.Web.UI.Page
    {

        #region --页面初始化--

        /// <summary>
        /// 树型结构节点
        /// </summary>
        public static Goldnet.Ext.Web.TreeNode root = null;

        /// <summary>
        /// 组织类别:院,科,人
        /// </summary>
        private static string OrganType = null;

        /// <summary>
        /// 根节点名称
        /// </summary>
        private string TreeRootName = "报表集";

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

            //主页面传入组织类别
            OrganType = Request.QueryString["OrganType"].ToString();

            Session["OrganType"] = OrganType;
            if (OrganType == "R")
            {
                TreeRootName = "人员" + TreeRootName;
            }
            if (OrganType == "K")
            {
                TreeRootName = "科室" + TreeRootName;

            }

            if (!Ext.IsAjaxRequest)
            {
                //初始化树结构
                root = new Goldnet.Ext.Web.TreeNode();

                root.NodeID = "root";

                root.Text = TreeRootName;

                root.Expanded = true;

                this.TreeCtrl.Root.Add(root);

                TreeBuild();

                this.btn_Add.Disabled = true;

                this.btn_Modify.Disabled = true;

                this.btn_Delete.Disabled = true;


            }
        }

        [AjaxMethod(ClientProxy = ClientProxy.Ignore)]
        public string TreeClick(string NodeCode)
        {
            //读取选择节点值
            string id = NodeCode;

            int Index = id.IndexOf('p');

            ReportDalDict dal = new ReportDalDict();
            if (Index >= 0)
            {
                string reportTypeId = id.Replace("p", "");
                this.Store1.DataSource = dal.getReportByPid(reportTypeId, OrganType).Tables[0];
                this.Store1.DataBind();
            }
            else
            {
                if (id != "")
                {
                    Session["WindowEditorClassId"] = id;
                    this.Store1.DataSource = dal.getDictBuild(id, OrganType).Tables[0];
                    this.Store1.DataBind();
                }
            }
            return "1";
        }

        /// <summary>
        /// 刷新数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void Data_RefreshData(object sender, StoreRefreshDataEventArgs e)
        {
            ReportDalDict dal = new ReportDalDict();
            Store1.RemoveAll();
            Store1.DataSource = dal.getDictBuild(Session["WindowEditorClassId"].ToString(), OrganType);
            Store1.DataBind();
        }

        /// <summary>
        /// 关闭窗体，清除SESSION
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void WindowsCloseSession(object sender, AjaxEventArgs e)
        {
            Session.Remove("SelectRightGuideItem");
            Session.Remove("ReportDeptCode");
            Session.Remove("ReportStaffCode");

        }

        /// <summary>
        /// 保存新增报表
        /// </summary>
        /// <param name="ReportName"></param>
        /// <param name="Oper"></param>
        /// <param name="ReportTypeId"></param>
        /// <param name="Reportid"></param>
        /// <param name="TempRoportCode"></param>
        /// <param name="TempReportRanking"></param>
        /// <param name="ReportSortNum"></param>
        /// <returns></returns>
        [AjaxMethod(ClientProxy = ClientProxy.Ignore)]
        public string DataBaseByOperaType(string ReportName, string Oper, string ReportTypeId, string Reportid, string TempRoportCode, string TempReportRanking, string ReportSortNum)
        {
            ReportDalDict dal = new ReportDalDict();

            switch (Oper)
            {
                case "1":
                    string SortNum = dal.getReportMaxRanking("1", Session["OrganType"].ToString(), ReportTypeId).ToString();
                    dal.AddReportCenterDict(ReportTypeId, ReportName, Session["OrganType"].ToString(), SortNum, ((User)Session["CURRENTSTAFF"]).UserId);
                    break;
                case "2":
                    dal.UpdateReportCenterDict(Reportid, ReportName, ReportSortNum);
                    break;
                case "3":
                    dal.DelReportListDictTran(Reportid);
                    break;
                case "4":
                    dal.ReportRankingTran(Reportid, TempRoportCode, ReportSortNum, TempReportRanking);
                    break;
                case "5":
                    dal.ReportRankingTran(Reportid, TempRoportCode, ReportSortNum, TempReportRanking);
                    break;
            }
            return "1";
        }
        #endregion

        #region --逻辑处理--
        /// <summary>
        /// 获取菜单树
        /// </summary>
        private void TreeBuild()
        {
            TreeDalDict dal = new TreeDalDict();

            DataTable l_TreeDt = dal.getSetReportTreeBuilder().Tables[0];

            Hashtable rootnode = new Hashtable();

            for (int i = 0; i < l_TreeDt.Rows.Count; i++)
            {
                string nodekey = "";
                Goldnet.Ext.Web.TreeNode node = new Goldnet.Ext.Web.TreeNode();
                node.NodeID = l_TreeDt.Rows[i]["CLASSID"].ToString();
                node.SingleClickExpand = true;
                node.Text = l_TreeDt.Rows[i]["CLASSNAME"].ToString();
                node.Icon = (Icon)Enum.Parse(typeof(Icon), "Folder");
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
        /// 显示细节
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void ShowDetail(Object sender, AjaxEventArgs e)
        {
            Dictionary<string, string>[] selectRow = GetSelectRow(e);
            if (selectRow == null)
            {
                return;
            }

            LoadConfig loadcfg = new LoadConfig();

            string PageType = this.hideBtn.Text;
            //Edit 定义报表,Show 预览报表
            if (PageType == "Edit")
            {
                loadcfg.Url = "ReportDetail.aspx?Reportid=" + selectRow[0]["ID"] + "";
                UrlPageResponse(arcEditWindow, loadcfg);

            }
            if (PageType == "Show")
            {
                ReportDalDict dal = new ReportDalDict();

                DataTable l_dt = dal.getReportTerms(selectRow[0]["ID"].ToString()).Tables[0];
                //导入ViewReportStructure页面
                string DeptTreeType = l_dt.Rows[0]["RPT_DPETTREETYPE"].ToString();
                string terms = DeptTreeType == "0" ? l_dt.Rows[0]["RPT_TERMS"].ToString() : l_dt.Rows[0]["PRT_SECOND_TERMS"].ToString();
                if (!terms.Equals(""))
                {
                    loadcfg.Url = "ViewReportStructure.aspx?Reportid=" + selectRow[0]["ID"] + "&OrganType=" + OrganType + "&DeptTreeType=" + DeptTreeType;
                    this.ViewWindow.Title = "报表信息--统计区间" + DateTime.Now.AddMonths(-1).ToString("yyyy-MM-dd") + "至" + DateTime.Now.ToString("yyyy-MM-dd");
                    UrlPageResponse(ViewWindow, loadcfg);
                }
                else
                {
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

        /// <summary>
        /// 前台出入的选择行
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        private Dictionary<string, string>[] GetSelectRow(AjaxEventArgs e)
        {
            string row = e.ExtraParams["Values"].ToString();
            Dictionary<string, string>[] selectRow = JSON.Deserialize<Dictionary<string, string>[]>(row);
            if (selectRow.Length <= 0)
            {
                return null;
            }
            else
            {
                return selectRow;
            }
        }

        /// <summary>
        /// 跳转页面
        /// </summary>
        /// <param name="win">windows窗体</param>
        /// <param name="loadcfg">url配置</param>
        private void UrlPageResponse(Window win, LoadConfig loadcfg)
        {
            loadcfg.Mode = LoadMode.IFrame;
            loadcfg.MaskMsg = "载入中...";
            loadcfg.ShowMask = true;
            loadcfg.NoCache = true;
            win.ClearContent();
            win.Show();
            win.LoadContent(loadcfg);
        }

        #endregion

    }
}
