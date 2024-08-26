using System;
using System.Collections.Generic;
using System.Data;
using System.Collections;
using Goldnet.Ext.Web;
using GoldNet.Comm;

namespace GoldNet.JXKP.jxkh
{
    public partial class Guide_Selector : System.Web.UI.Page
    {
        Goldnet.Dal.Guide_Dict dal = new Goldnet.Dal.Guide_Dict();
        /// <summary>
        /// 
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
                string GuideGatherDefine = Session["GUIDEGATHERDEFINE"].ToString();
                if ((GuideGatherDefine == null) | (GuideGatherDefine.Equals("")))
                {
                    Response.End();
                    return;
                }
                string GuideGatherID = GuideGatherDefine.Split(',')[0];
                string GuideGatherOrgan = GuideGatherDefine.Split(',')[1];
                //显示该指标集中已经包含的各项指标，以及指标权重
                DataTable table = dal.GetGuideGroupList(GuideGatherID).Tables[0];
                Store2.DataSource = table;
                Store2.DataBind();
                //绑定组织、科室下拉框列表
                //如果传入了指定的组织类别，则选中该类别，并将组织下拉框设为禁止下拉
                SetDictList(GuideGatherOrgan);
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
            DataTable tablebsc = dal.getBSCGuideTree(depttype, organtype).Tables[0];
            Hashtable rootnode = new Hashtable();
            for (int i = 0; i < tablebsc.Rows.Count; i++)
            {
                string nodekey = "";
                Goldnet.Ext.Web.TreeNode node = new Goldnet.Ext.Web.TreeNode();
                node.SingleClickExpand = true;
                node.NodeID = tablebsc.Rows[i]["id"].ToString();
                node.Text = tablebsc.Rows[i]["name"].ToString();
                node.Icon = Icon.Folder;
                //node.Icon = (Icon)Enum.Parse(typeof(Icon), "Folder");
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
        /// 填充字典信息
        /// </summary>
        /// <param name="organ"></param>
        protected void SetDictList(string organ)
        {
            DataTable dt = dal.Getorg().Tables[0];
            int j = 0;
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (dt.Rows[i]["ORGAN_CLASS_CODE"].ToString().Equals(organ))
                {
                    j = i;
                }
                this.Combo_Org.Items.Insert(i, new Goldnet.Ext.Web.ListItem(dt.Rows[i]["ORGAN_CLASS_NAME"].ToString(), dt.Rows[i]["ORGAN_CLASS_CODE"].ToString()));
            }
            this.Combo_Org.SelectedIndex = j;
            if (!organ.Equals(""))
            {
                this.Combo_Org.Disabled = true;
            }
            dt = dal.GetDeptType().Tables[0];
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                this.Combo_DeptType.Items.Insert(i, new Goldnet.Ext.Web.ListItem(dt.Rows[i]["DEPT_CLASS_NAME"].ToString(), dt.Rows[i]["DEPT_CLASS_CODE"].ToString()));
            }
            this.Combo_DeptType.SelectedIndex = 0;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bsc"></param>
        /// <param name="selectedid"></param>
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
            DataTable dt = dal.GetGuideDictListByBscOrgDept(bsc, organtype, depttype, tag).Tables[0];
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

        /// <summary>
        /// 查询指标
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

        /// <summary>
        /// 保存指标集定义中包含的指标及每项指标的权重
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void SaveGuide(object sender, AjaxEventArgs e)
        {
            string GuideGatherDefine = Session["GUIDEGATHERDEFINE"].ToString();
            if ((GuideGatherDefine == null) | (GuideGatherDefine.Equals("")))
            {
                Response.End();
                return;
            }
            string GuideGatherID = GuideGatherDefine.Split(',')[0];
            string selectedid = e.ExtraParams["multi2"];
            Dictionary<string, string>[] selectedRow = JSON.Deserialize<Dictionary<string, string>[]>(selectedid);

            string rtn = dal.UpdateGuideGatherDetail(GuideGatherID, selectedRow);
            Goldnet.Ext.Web.Ext.Msg.Show(new Goldnet.Ext.Web.MessageBox.Config
            {
                Title = SystemMsg.msgtitle4,
                Message = (rtn.Equals("") ? "指标集保存成功！<BR/>指标集内指标项目的变更，将会影响岗位绩效评测目标，<br/>请更新指标关联的岗位评测目标！" : rtn),
                Buttons = MessageBox.Button.OK,
                Icon = (MessageBox.Icon)Enum.Parse(typeof(MessageBox.Icon), "INFO")
            });
        }

    }
}
