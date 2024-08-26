using System;
using System.Collections;
using System.Configuration;
using System.Data;
using Goldnet.Ext.Web;
using System.Collections.Generic;
using Goldnet.Dal;
using GoldNet.Comm;
using GoldNet.Model;

namespace GoldNet.JXKP.GuideLook
{
    public partial class SetAnalyseReport : System.Web.UI.Page
    {

        #region --页面初始化--

        /// <summary>
        /// 树型结构节点
        /// </summary>
        public static Goldnet.Ext.Web.TreeNode root = null;

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
                //this.btn_Modify.Disabled = true;

                //this.btn_Delete.Disabled = true;

                

                string reportTypeId = id.Replace("p", "");

                this.Store1.DataSource = dal.getAnalyseReportByPid(reportTypeId).Tables[0];
                this.Store1.DataBind();
            }
            else
            {
                if (id != "")
                {
                    //this.btn_Modify.Disabled = true;

                    //this.btn_Delete.Disabled = true;

                    

                    Session["AnalyseReportWindowEditorId"] = id;

                    this.Store1.DataSource = dal.getAnalyseDictBuild(id).Tables[0];
                    this.Store1.DataBind();
                }
            }
            return "1";
        }


        [AjaxMethod(ClientProxy = ClientProxy.Ignore)]
        public string DataBaseByOperaType(string ReportName, string Oper, string ReportTypeId, string Reportid, string TempRoportCode, string TempReportRanking, string ReportSortNum)
        {
            ReportDalDict dal = new ReportDalDict();

            switch (Oper)
            {
                case "1":
                    string SortNum = dal.getReportMaxRanking("2", "", ReportTypeId).ToString();
                    dal.AddAnalyseReportCenterDict(ReportTypeId, ReportName, SortNum, ((User)Session["CURRENTSTAFF"]).UserId);
                    break;
                case "2":
                    dal.UpdateAnalyseReportCenterDict(Reportid, ReportName, ReportSortNum);
                    break;
                case "3":
                    dal.DelAnalyseReportListDictTran(Reportid);
                    break;
                case "4":
                    dal.ReportAnalyseRankingTran(Reportid, TempRoportCode, ReportSortNum, TempReportRanking);
                    break;
                case "5":
                    dal.ReportAnalyseRankingTran(Reportid, TempRoportCode, ReportSortNum, TempReportRanking);
                    break;
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
            Store1.DataSource = dal.getAnalyseDictBuild(Session["AnalyseReportWindowEditorId"].ToString());
            Store1.DataBind();
        }
        #endregion

        #region --逻辑处理--

        private void TreeBuild()
        {
            TreeDalDict dal = new TreeDalDict();

            DataTable l_TreeDt = dal.getSetAnalyseReportTreeBuilder().Tables[0];

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
                loadcfg.Url = "SetAnalyseReportGuide.aspx?Reportid=" + selectRow[0]["ID"] + "";
                UrlPageResponse(arcEditWindow,loadcfg);
            }
            if (PageType == "Show")
            {
                ReportDalDict dal = new ReportDalDict();
                DataTable l_dt = dal.getAnalyseReportGuideByRptId(selectRow[0]["ID"].ToString()).Tables[0];
                //导入ViewReportStructure页面
                if (l_dt.Rows.Count > 0)
                {
                    loadcfg.Url = "ViewAnalyseReportStructure.aspx?Reportid=" + selectRow[0]["ID"] + "";
                    this.ViewWindow.Title = "分析报表" + DateTime.Now.Year + "年" + DateTime.Now.Month+"月";
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
