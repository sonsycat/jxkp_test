using System;
using System.Collections;
using System.Data;
using Goldnet.Ext.Web;
using System.Collections.Generic;
using GoldNet.Comm;
using Goldnet.Dal.home;
using GoldNet.Model;

namespace GoldNet.JXKP.GuideLook
{
    public partial class SetPlanGuideForYear : PageBase
    {
        HomeDal dal = new HomeDal();

        /// <summary>
        /// 初始化处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
                // 岗位代码
                string stationcode = Session["curstationcode"] == null ? ((User)Session["CURRENTSTAFF"]).GetStationCode(((User)Session["CURRENTSTAFF"]).StaffId, DateTime.Now.Year.ToString()) : Session["curstationcode"].ToString();
                // 人员ID
                string personid = Session["curpersonid"] == null ? ((User)Session["CURRENTSTAFF"]).StaffId : ((User)Session["CURRENTSTAFF"]).GetStaffid(Session["curpersonid"].ToString());
                
                SetDictList();
            }
        }

        /// <summary>
        /// 设置已选择的指标列表
        /// </summary>
        /// <param name="organ"></param>
        protected void SetDictList()
        {
            DataTable tables = dal.getBSCGuideTreeSelect().Tables[0];

            if (!Convert.IsDBNull(tables))
            {
                Store2.DataSource = tables;
                Store2.DataBind();
            }
        }

        /// <summary>
        /// 在组织、科室下拉选择改变时，刷新树 , 获取Json
        /// </summary>
        /// <returns></returns>
        [AjaxMethod]
        public string RefreshTree()
        {
            Goldnet.Ext.Web.TreeNode root = new Goldnet.Ext.Web.TreeNode();
            root.NodeID = "root";
            root.Text = "指标体系";
            root.Expanded = true;

            // 岗位代码
            string stationcode = Session["curstationcode"] == null ? ((User)Session["CURRENTSTAFF"]).GetStationCode(((User)Session["CURRENTSTAFF"]).StaffId, DateTime.Now.Year.ToString()) : Session["curstationcode"].ToString();

            //获取指标树
            DataTable tablebsc = dal.getBSCGuideTree(stationcode).Tables[0];
            Hashtable rootnode = new Hashtable();
            for (int i = 0; i < tablebsc.Rows.Count; i++)
            {
                string nodekey = "";
                Goldnet.Ext.Web.TreeNode node = new Goldnet.Ext.Web.TreeNode();
                node.SingleClickExpand = true;
                node.NodeID = tablebsc.Rows[i]["id"].ToString();
                node.Text = tablebsc.Rows[i]["name"].ToString();
                node.Icon = Icon.Folder;
                string pid = tablebsc.Rows[i]["pid"].ToString();
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
            Goldnet.Ext.Web.TreeNodeCollection tnodec = new Goldnet.Ext.Web.TreeNodeCollection();
            tnodec.Add(root);
            return tnodec.ToJson();
        }

        /// <summary>
        /// 选择指标树
        /// </summary>
        /// <param name="bsc"></param>
        /// <param name="selectedid"></param>
        [AjaxMethod]
        public void TreeSelectedGuide(string bsc, string selectedid)
        {
            // 岗位代码
            //string stationcode = Session["curstationcode"] == null ? ((User)Session["CURRENTSTAFF"]).GetStationCode(((User)Session["CURRENTSTAFF"]).StaffId, DateTime.Now.Year.ToString()) : Session["curstationcode"].ToString();
            string stationcode = "";
            BindLeftSelector(bsc, stationcode, selectedid);
        }

        /// <summary>
        /// 绑字左侧选择列表
        /// </summary>
        /// <param name="bsc"></param>
        /// <param name="organtype"></param>
        /// <param name="depttype"></param>
        /// <param name="tag"></param>
        /// <param name="selectedid"></param>
        protected void BindLeftSelector(string bsc, string stationcode, string selectedid)
        {
            Dictionary<string, string>[] selectRow = JSON.Deserialize<Dictionary<string, string>[]>(selectedid);
            ArrayList array = new ArrayList();
            for (int i = 0; i < selectRow.Length; i++)
            {
                array.Add(selectRow[i]["ID"].ToString());
            }
            DataTable dt = dal.getBSCGuideTreeAll(bsc).Tables[0];
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (array.Contains(dt.Rows[i]["ID"].ToString()))
                {
                    dt.Rows.RemoveAt(i);
                    i--;
                }
            }
            Store1.DataSource = dt;
            Store1.DataBind();
        }

        /// <summary>
        /// 保存指标集定义中包含的指标及每项指标的权重
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void SaveGuide(object sender, AjaxEventArgs e)
        {
            // 已选择的指标
            string selectedid = e.ExtraParams["multi2"];
            Dictionary<string, string>[] selectedRow = JSON.Deserialize<Dictionary<string, string>[]>(selectedid);

            //更新保存
            string rtn = dal.updateBSCGuide(selectedRow);
            Goldnet.Ext.Web.Ext.Msg.Show(new Goldnet.Ext.Web.MessageBox.Config
            {
                Title = SystemMsg.msgtitle4,
                Message = (rtn.Equals("") ? "所选指标保存成功！" : rtn),
                Buttons = MessageBox.Button.OK,
                Icon = (MessageBox.Icon)Enum.Parse(typeof(MessageBox.Icon), "INFO")
            });
        }


        /// <summary>
        /// 信息提示
        /// </summary>
        /// <param name="msg"></param>
        public void showMsg(string msg)
        {
            Goldnet.Ext.Web.Ext.Msg.Show(new Goldnet.Ext.Web.MessageBox.Config
            {
                Title = SystemMsg.msgtitle4,
                Message = msg,
                Buttons = MessageBox.Button.OK,
                Icon = (MessageBox.Icon)Enum.Parse(typeof(MessageBox.Icon), "INFO")
            });
        }
    }
}
