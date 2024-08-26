using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Goldnet.Dal;
using Goldnet.Dal.home;
using Goldnet.Ext.Web;
using GoldNet.Comm;
using GoldNet.Model;

namespace GoldNet.JXKP.jxkh
{
    public partial class GuideDeptSet : PageBase
    {
        HomeDal dal = new HomeDal();
        private BoundComm boundcomm = new BoundComm();
        public static string stationcode = "";
        public static string stationyear = "";
        public static string guidegathercode = "";

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
                //string stationcode = Session["curstationcode"] == null ? ((User)Session["CURRENTSTAFF"]).GetStationCode(((User)Session["CURRENTSTAFF"]).StaffId, DateTime.Now.Year.ToString()) : Session["curstationcode"].ToString();
                // 人员ID
                string personid = Session["curpersonid"] == null ? ((User)Session["CURRENTSTAFF"]).StaffId : ((User)Session["CURRENTSTAFF"]).GetStaffid(Session["curpersonid"].ToString());

                //SetDictList();

                //年度下拉列表初始化
                Store4.DataSource = boundcomm.getYears();
                Store4.DataBind();
                cbbYear.SetValue(DateTime.Now.Year);
            }
        }

        /// <summary>
        /// 设置已选择的指标列表
        /// </summary>
        /// <param name="organ"></param>
        protected void SetDictList()
        {
            //DataTable tables = dal.getBSCGuideTreeSelect().Tables[0];

            //if (!Convert.IsDBNull(tables))
            //{
            //    Store2.DataSource = tables;
            //    Store2.DataBind();
            //}
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
            //string stationcode = Session["curstationcode"] == null ? ((User)Session["CURRENTSTAFF"]).GetStationCode(((User)Session["CURRENTSTAFF"]).StaffId, DateTime.Now.Year.ToString()) : Session["curstationcode"].ToString();

            //获取指标树
            DataTable tablebsc = dal.getBSCGuideTreeAllGathers("").Tables[0];
            Hashtable rootnode = new Hashtable();
            for (int i = 0; i < tablebsc.Rows.Count; i++)
            {
                string nodekey = "";
                Goldnet.Ext.Web.TreeNode node = new Goldnet.Ext.Web.TreeNode();
                node.SingleClickExpand = true;
                node.NodeID = tablebsc.Rows[i]["id"].ToString();
                node.Text = tablebsc.Rows[i]["name"].ToString();
                node.Icon = Icon.Folder;
                //父节点
                string pid = tablebsc.Rows[i]["pid"].ToString();
                nodekey = pid;
                if (rootnode.Contains(nodekey))
                {
                    //在树中找到父节点并追加到父节点后，作为子节点
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
        /// 选择指标树处理
        /// </summary>
        /// <param name="bsc"></param>
        /// <param name="selectedid"></param>
        [AjaxMethod]
        public void TreeSelectedGuide(string bsc)
        {
            BindLeftSelector(bsc, "", "");
        }

        /// <summary>
        /// 绑字指标设置列表
        /// </summary>
        /// <param name="bsc"></param>
        /// <param name="organtype"></param>
        /// <param name="depttype"></param>
        /// <param name="tag"></param>
        /// <param name="selectedid"></param>
        protected void BindLeftSelector(string bsc, string stationcode, string selectedid)
        {
            stationyear = cbbYear.SelectedItem.Value;
            //获取当前节点下的所有指标
            DataTable dt = dal.getBSCGuideTreeByGathers(bsc, stationyear).Tables[0];

            Store3.DataSource = dt;
            Store3.DataBind();
        }

        /// <summary>
        /// 保存指标集定义中包含的指标及每项指标的权重
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void SaveGuide(object sender, AjaxEventArgs e)
        {
            stationyear = cbbYear.SelectedItem.Value;
            // 已选择的指标
            string guidecode = e.ExtraParams["Value2"];

           ArrayList selectRows = GetSelectRow(e);

            //更新保存
            if (selectRows == null && stationcode==null)
            {
                return;
            }
            else
            {
                Goldnet.Dal.StationManager dal1 = new Goldnet.Dal.StationManager();
                try
                {
                    if (!guidecode.Equals(""))
                    {
                        guidecode= guidecode.Replace("\"", "");
                    }
                    dal1.SaveDeptGuideTargetByDept(guidecode, stationyear, guidegathercode, selectRows);
                    Goldnet.Ext.Web.ScriptManager scManager = Goldnet.Ext.Web.ScriptManager.GetInstance(this.Page);

                    //获取当前节点下的所有指标
                    DataTable dt = dal.getBSCGuideTreeByGathers(guidecode, stationyear).Tables[0];

                    Store3.DataSource = dt;
                    Store3.DataBind();

                    this.showMsg("岗位指标量化保存成功!");
                }
                catch (Exception ee)
                {
                    showMsg("岗位指标量化保存失败!<br/>" + "原因:" + ee.Message);
                }
            }
        }

        /// <summary>
        /// 转化列表
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        private ArrayList GetSelectRow(AjaxEventArgs e)
        {
            ArrayList SelectRows = new ArrayList();

            string rows = e.ExtraParams["Value1"].ToString();
            Dictionary<string, string>[] selectRow = JSON.Deserialize<Dictionary<string, string>[]>(rows);
            if (selectRow != null)
            {
                for (int j = 0; j < selectRow.Length; j++)
                {
                    SelectRows.Add(selectRow.ToArray()[j]);
                }

            }
            return SelectRows;
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Button_look_click(object sender, EventArgs e)
        {
            //string year = cbbYear.SelectedItem.Value;
            //string deptcode = cbbdept.SelectedItem.Value;
            //Bindlist(year, deptcode);
        }
    }
}
