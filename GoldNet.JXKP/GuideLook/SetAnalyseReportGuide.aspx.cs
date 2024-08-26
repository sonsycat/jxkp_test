using System;
using System.Collections;
using System.Configuration;
using System.Data;
using Goldnet.Ext.Web;
using System.Collections.Generic;
using GoldNet.Comm;
using Goldnet.Dal;

namespace GoldNet.JXKP.GuideLook
{
    public partial class SetAnalyseReportGuide : System.Web.UI.Page
    {
        
        GuideDalDict gdal = new GuideDalDict();

        private static string m_rptid = null;
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
                string rpt_id = Request.QueryString["Reportid"].ToString();
                if ((rpt_id == null) | (rpt_id.Equals("")))
                {
                    Response.End();
                    return;
                }

                m_rptid = rpt_id;
                //显示该指标集中已经包含的各项指标，以及指标权重
                DataTable table = gdal.getAnalyseReportGuideCodeByReportid(rpt_id).Tables[0];
                Store2.DataSource = table;
                Store2.DataBind();
                //绑定组织、科室下拉框列表
                //如果传入了指定的组织类别，则选中该类别，并将组织下拉框设为禁止下拉
                SetDictList();
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
           
            string depttype = this.Combo_DeptType.SelectedItem.Value.ToString();
            string organtype = this.Combo_Org.SelectedItem.Value.ToString();
            DataTable tablebsc = gdal.getBSCGuideTree(depttype, organtype).Tables[0];
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

        //填充字典信息
        protected void SetDictList()
        {
            DataTable dt = gdal.Getorg().Tables[0];
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                this.Combo_Org.Items.Insert(i, new Goldnet.Ext.Web.ListItem(dt.Rows[i]["ORGAN_CLASS_NAME"].ToString(), dt.Rows[i]["ORGAN_CLASS_CODE"].ToString()));
            }
            this.Combo_Org.SelectedIndex = 0;

            dt = gdal.GetDeptType().Tables[0];
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                this.Combo_DeptType.Items.Insert(i, new Goldnet.Ext.Web.ListItem(dt.Rows[i]["DEPT_CLASS_NAME"].ToString(), dt.Rows[i]["DEPT_CLASS_CODE"].ToString()));
            }
            this.Combo_DeptType.SelectedIndex = 0;
        }

        [AjaxMethod]
        public void TreeSelectedGuide(string bsc, string selectedid)
        {
            string depttype = this.Combo_DeptType.SelectedItem.Value.ToString();
            string organtype = this.Combo_Org.SelectedItem.Value.ToString();
            BindLeftSelector(bsc, organtype, depttype, "", selectedid);
        }
        /// <summary>
        /// 绑字左侧选择列表
        /// </summary>
        /// <param name="bsc"></param>
        /// <param name="organtype"></param>
        /// <param name="depttype"></param>
        /// <param name="tag"></param>
        /// <param name="selectedid"></param>
        protected void BindLeftSelector(string bsc, string organtype, string depttype, string tag, string selectedid)
        {

            Goldnet.Ext.Web.ListItem[] items1 = JSON.Deserialize<Goldnet.Ext.Web.ListItem[]>(selectedid);
            ArrayList array = new ArrayList();
            for (int i = 0; i < items1.Length; i++)
            {
                array.Add(items1[i].Value.ToString());
            }
            DataTable dt = gdal.GetAnalyseGuideDictListByBscOrgDept(bsc, organtype, depttype, tag).Tables[0];
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


        //查询指标
        protected void SearchGuide(object sender, AjaxEventArgs e)
        {
            string tag = this.txtTagName.Value.ToString().Trim();
            if (!tag.Equals(""))
            {
                string depttype = this.Combo_DeptType.SelectedItem.Value.ToString();
                string organtype = this.Combo_Org.SelectedItem.Value.ToString();
                string selectedid = e.ExtraParams["multi1"];
                BindLeftSelector("", organtype, depttype, tag, selectedid);
            }
        }

        //保存指标集定义中包含的指标及每项指标的权重
        protected void SaveGuide(object sender, AjaxEventArgs e)
        {
            string rpt_id = m_rptid;
            if ((rpt_id == null) | (rpt_id.Equals("")))
            {
                Response.End();
                return;
            }
            string selectedid = e.ExtraParams["multi2"];
            Goldnet.Ext.Web.ListItem[] selectedRow = JSON.Deserialize<Goldnet.Ext.Web.ListItem[]>(selectedid);
            DataTable l_dt = CreateTableForSelectRightItem(selectedRow);
            gdal.UpdateAnalyseReportGuideInfo(rpt_id, l_dt);
            string rtn = "";
            Goldnet.Ext.Web.Ext.Msg.Show(new Goldnet.Ext.Web.MessageBox.Config
            {
                Title = SystemMsg.msgtitle4,
                Message = (rtn.Equals("") ? "指标集保存成功！" : rtn),
                Buttons = MessageBox.Button.OK,
                Icon = (MessageBox.Icon)Enum.Parse(typeof(MessageBox.Icon), "INFO")
            });
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
    }
}
