using System;
using System.Collections;
using System.Configuration;
using System.Data;
using Goldnet.Ext.Web;
using GoldNet.Comm;
using Goldnet.Dal;
using System.Text;

namespace GoldNet.JXKP.GuideLook
{
    public partial class GuideByTime : PageBase
    {
        private static string m_GuideCode = null;

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
                InitComboxValue();
                this.DeptInfobtn.Disabled = true;
                this.empInfobtn.Disabled = true;
                DeptPower = this.DeptFilter("");
            }
        }

        [AjaxMethod(ClientProxy = ClientProxy.Ignore)]
        public string ChartsRefresh(string StationCode)
        {
            string organ = this.cbx_OrgType.SelectedItem.Value;

            string depttype = "";

            string deptattr = "";

            string chartstype = this.cbx_ChartsType.SelectedItem.Value;

            string GuideCode = m_GuideCode;
            string years = this.cbx_Years.SelectedItem.Value;

            string where = "";

            #region 查询条件

            StringBuilder str = new StringBuilder();

            if (!this.cbx_Ptype.SelectedItem.Value.Equals("全部"))
            {
                str.Append(" and staffsort" + Stringfilter(this.cbx_Ptype.SelectedItem.Text.Trim()) + " ");
            }

            if (this.cbx_PTechType.SelectedItem.Value != "全部")
            {
                str.Append(" and techincclass" + Stringfilter(this.cbx_PTechType.SelectedItem.Value) + " ");
            }
            if (this.cbx_PLevel.SelectedItem.Value != "全部")
            {
                str.Append(" and CivilServiceClass" + Stringfilter(this.cbx_PLevel.SelectedItem.Value) + " ");
            }
            if (this.cbx_PTech.SelectedItem.Value != "全部")
            {
                str.Append(" and title_list" + Stringfilter(this.cbx_PTech.SelectedItem.Text.Trim()) + " ");
            }
            if (this.cbx_PCollage.SelectedItem.Value != "全部")
            {
                str.Append(" and edu1" + Stringfilter(this.cbx_PCollage.SelectedItem.Text.Trim()) + " ");
            }

            if (this.cbx_TimeOrgan.SelectedItem.Value == ">=")
            {
                str.Append(" and TechnicClassDate is not null and ");

                str.Append(" substr(nvl(TechnicClassDate,'') ,1,4)|| lpad(replace(replace(replace(substr(nvl(TechnicClassDate,'') ,6,2),'.',''),'-',''),'/',''),2,'0')||'01'  >= '");

                str.Append(this.timer.SelectedDate.ToString("yyyyMM") + "01" + "' ");
            }
            else if (this.cbx_TimeOrgan.SelectedItem.Value == "<=")
            {
                str.Append(" and TechnicClassDate is not null and ");

                str.Append(" substr(nvl(TechnicClassDate,'') ,1,4)|| lpad(replace(replace(replace(substr(nvl(TechnicClassDate,'') ,6,2),'.',''),'-',''),'/',''),2,'0')||'01'  <= '");

                str.Append(this.timer.SelectedDate.ToString("yyyyMM") + "01" + "' ");
            }
            else if (this.cbx_TimeOrgan.SelectedItem.Value == "=")
            {
                str.Append(" and TechnicClassDate is not null and ");

                str.Append(" substr(nvl(TechnicClassDate,'') ,1,4)|| lpad(replace(replace(replace(substr(nvl(TechnicClassDate,'') ,6,2),'.',''),'-',''),'/',''),2,'0')||'01'  = '");

                str.Append(this.timer.SelectedDate.ToString("yyyyMM") + "01" + "' ");
            }


            where = str.ToString();

            #endregion

            string offset = "0";
            if (this.radTj.Checked)
            {
                offset = GetConfig.GetConfigString("dateoffset");
            }

            string ChartsTerms = chartstype + "*" + organ + "*" + depttype + "*" + deptattr + "*" + years + "*" + StationCode.TrimEnd(new char[] { ',' }) + "*" + where + "*" + GuideCode + "*" + offset;


            FromDate = this.cboFromMonth.SelectedItem.Value;

            ToDate = this.cboToMonth.SelectedItem.Value;

            return ChartsTerms;
        }

        [AjaxMethod(ClientProxy = ClientProxy.Ignore)]
        public string DeptChartsRefresh(string DeptCode)
        {
            string organ = this.cbx_OrgType.SelectedItem.Value;

            string depttype = "";

            string deptattr = "";

            string chartstype = this.cbx_ChartsType.SelectedItem.Value;

            string GuideCode = m_GuideCode;

            string years = this.cbx_Years.SelectedItem.Value;

            string where = "";

            string offset = "0";

            if (this.radTj.Checked)
            {
                offset = GetConfig.GetConfigString("dateoffset");
            }

            FromDate = this.cboFromMonth.SelectedItem.Value;

            ToDate = this.cboToMonth.SelectedItem.Value;

            string ChartsTerms = chartstype + "*" + organ + "*" + depttype + "*" + deptattr + "*" + years + "*" + DeptCode.TrimEnd(new char[] { ',' }) + "*" + where + "*" + GuideCode + "*" + offset;

            return ChartsTerms;
        }

        [AjaxMethod(ClientProxy = ClientProxy.Ignore)]
        public string GridPanelRefresh(string GuideCode)
        {
            string organ = this.cbx_OrgType.SelectedItem.Value;

            if (organ == "02")
            {
                this.DeptInfobtn.Disabled = false;
                this.empInfobtn.Disabled = true;
            }
            if (organ == "03")
            {
                this.empInfobtn.Disabled = false;
                this.DeptInfobtn.Disabled = true;
            }

            string depttype = this.cbx_DeptType.SelectedItem.Value;

            string deptattr = "";

            string chartstype = this.cbx_ChartsType.SelectedItem.Value;

            m_GuideCode = GuideCode;

            chartstype = "bar";
            
            deptattr = "";
            depttype = "";

            string years = this.cbx_Years.SelectedItem.Value;

            string station = "";

            string where = "";

            string offset = "0";

            FromDate = this.cboFromMonth.SelectedItem.Value;

            ToDate = this.cboToMonth.SelectedItem.Value;

            if (this.radTj.Checked)
            {
                offset = GetConfig.GetConfigString("dateoffset");
            }
            string ChartsTerms = chartstype + "*" + organ + "*" + depttype + "*" + deptattr + "*" + years + "*" + station + "*" + where + "*" + GuideCode + "*" + offset;
            return ChartsTerms;
        }

        
        
        
        
        
        
        
        
        
        private string Stringfilter(string item)
        {
            if (item == "为空")
            {
                item = " is null ";
            }
            else
            {
                item = " = '" + item + "'";
            }
            return item;
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

            string OrganType = this.cbx_OrgType.SelectedItem.Value;

            string DeptType = this.cbx_DeptType.SelectedItem.Value;

            TreeDalDict dal = new TreeDalDict();

            DataTable l_TreeDt = dal.getGuideForGuideLook(OrganType, DeptType).Tables[0];

            Hashtable rootnode = new Hashtable();

            for (int i = 0; i < l_TreeDt.Rows.Count; i++)
            {
                string nodekey = "";
                Goldnet.Ext.Web.TreeNode node = new Goldnet.Ext.Web.TreeNode();
                node.SingleClickExpand = true;
                node.NodeID = l_TreeDt.Rows[i]["code"].ToString();
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

        [AjaxMethod]
        public string RefreshDeptTree()
        {

            Goldnet.Ext.Web.TreeNode root = new Goldnet.Ext.Web.TreeNode();
            root.NodeID = "root";
            root.Text = "指标体系";
            root.Expanded = true;

            string DeptType = this.cbx_DeptType.SelectedItem.Value;

            string queryString = this.txtDeptCode.Text;

            TreeDalDict dal = new TreeDalDict();
            DataTable l_TreeDt = dal.getDeptForGuideLook(DeptType, queryString).Tables[0];

            Hashtable rootnode = new Hashtable();

            for (int i = 0; i < l_TreeDt.Rows.Count; i++)
            {
                string nodekey = "";
                Goldnet.Ext.Web.TreeNode node = new Goldnet.Ext.Web.TreeNode();
                node.SingleClickExpand = true;
                root.Checked = Goldnet.Ext.Web.ThreeStateBool.False;
                node.NodeID = l_TreeDt.Rows[i]["id"].ToString();
                node.Text = l_TreeDt.Rows[i]["name"].ToString();
                node.Icon = (Icon)Enum.Parse(typeof(Icon), "Folder");
                string pid = l_TreeDt.Rows[i]["pid"].ToString();
                nodekey = pid;
                if (rootnode.Contains(nodekey))
                {
                    Goldnet.Ext.Web.TreeNode pnode = (Goldnet.Ext.Web.TreeNode)rootnode[nodekey];
                    node.Checked = ThreeStateBool.False;
                    pnode.Nodes.Add(node);
                }
                else
                {
                    node.Checked = ThreeStateBool.False;
                    root.Nodes.Add(node);
                }
                rootnode.Add(node.NodeID, node);
            }

            Goldnet.Ext.Web.TreeNodeCollection tnodec = new Goldnet.Ext.Web.TreeNodeCollection();
            tnodec.Add(root);

            return tnodec.ToJson();
        }

        [AjaxMethod]
        public string RefreshStaffTree()
        {
            Goldnet.Ext.Web.TreeNode root = new Goldnet.Ext.Web.TreeNode();
            root.NodeID = "root";
            root.Text = "指标体系";
            root.Expanded = true;
            TreeDalDict dal = new TreeDalDict();
            string DeptType = this.cbx_DeptType.SelectedItem.Value.ToString();

            string year = DateTime.Now.Year.ToString();

            string queryString = "";

            DataTable l_TreeDt = dal.getSatffForGuideLook(DeptType, year, queryString).Tables[0];

            Hashtable rootnode = new Hashtable();

            for (int i = 0; i < l_TreeDt.Rows.Count; i++)
            {
                string nodekey = "";
                Goldnet.Ext.Web.TreeNode node = new Goldnet.Ext.Web.TreeNode();
                root.Checked = Goldnet.Ext.Web.ThreeStateBool.False;
                node.NodeID = l_TreeDt.Rows[i]["id"].ToString();
                node.Text = l_TreeDt.Rows[i]["name"].ToString();
                node.Icon = (Icon)Enum.Parse(typeof(Icon), "Folder");
                string pid = l_TreeDt.Rows[i]["pid"].ToString();
                nodekey = pid;
                if (rootnode.Contains(nodekey))
                {
                    Goldnet.Ext.Web.TreeNode pnode = (Goldnet.Ext.Web.TreeNode)rootnode[nodekey];
                    node.Checked = ThreeStateBool.False;
                    pnode.Nodes.Add(node);
                }
                else
                {
                    node.Checked = ThreeStateBool.False;
                    root.Nodes.Add(node);
                }
                rootnode.Add(node.NodeID, node);
            }

            Goldnet.Ext.Web.TreeNodeCollection tnodec = new Goldnet.Ext.Web.TreeNodeCollection();
            tnodec.Add(root);

            return tnodec.ToJson();
        }

        protected void QueryStationNode(object sender, AjaxEventArgs e)
        {
            string node = e.ExtraParams["nodeDept"].ToString();
            string deptCode = "";
            string[] nodeCode = node.Split(';');
            for (int i = 0; i < nodeCode.Length; i++)
            {
                if (nodeCode[i].IndexOf("C") < 0 && nodeCode[i] != "")
                {
                    deptCode = deptCode + nodeCode[i] + ",";
                }
            }

            GuideDalDict dal = new GuideDalDict();
            this.StoreCombo.DataSource = dal.getStationMain(deptCode.TrimEnd(new char[] { ',' }), DateTime.Now.Year.ToString()).Tables[0];
            this.StoreCombo.DataBind();
        }

        /// <summary>
        /// 初始化
        /// </summary>
        private void InitComboxValue()
        {
            for (int i = 0; i < 10; i++)
            {
                int years = System.DateTime.Now.Year - i;
                this.cbx_Years.Items.Add(new Goldnet.Ext.Web.ListItem(years.ToString() + "年", years.ToString()));
            }
            this.cbx_Years.SelectedIndex = 0;

            for (int i = 1; i <= 12; i++)
            {
                this.cboFromMonth.Items.Add(new Goldnet.Ext.Web.ListItem(i.ToString() + "月", i.ToString()));
            }
            this.cboFromMonth.SelectedIndex = 0;

            for (int i = 1; i <= 12; i++)
            {
                this.cboToMonth.Items.Add(new Goldnet.Ext.Web.ListItem(i.ToString() + "月", i.ToString()));
            }
            this.cboToMonth.SelectedIndex = 11;


            DeptDalDict Deptdal = new DeptDalDict();

            //组织类型
            DataTable l_OrganTypedt = this.GetPerRoleType();
            for (int i = 0; i < l_OrganTypedt.Rows.Count; i++)
            {
                this.cbx_OrgType.Items.Add(new Goldnet.Ext.Web.ListItem(l_OrganTypedt.Rows[i]["ROLE_TYPE"].ToString(), l_OrganTypedt.Rows[i]["ID"].ToString()));
            }
            DataRow[] l_dr = l_OrganTypedt.Select("ID=02");
            if (l_dr.Length > 0)
            {
                this.cbx_OrgType.Value = "02";
            }
            else
            {
                this.cbx_OrgType.SelectedIndex = 0;
            }

            //科室
            DataTable l_DeptTypedt = Deptdal.DeptStore().Tables[0];
            for (int i = 0; i < l_DeptTypedt.Rows.Count; i++)
            {
                this.cbx_DeptType.Items.Add(new Goldnet.Ext.Web.ListItem(l_DeptTypedt.Rows[i]["TEXT"].ToString(), l_DeptTypedt.Rows[i]["ID"].ToString()));

            }
            this.cbx_DeptType.Value = "20";

            //人员基本信息
            StaffDalDict dal = new StaffDalDict();
            DataTable l_Sortdt = dal.getPSort().Tables[0];

            this.cbx_Ptype.Items.Add(new Goldnet.Ext.Web.ListItem("全部", "全部"));
            for (int i = 0; i < l_Sortdt.Rows.Count; i++)
            {
                this.cbx_Ptype.Items.Add(new Goldnet.Ext.Web.ListItem(l_Sortdt.Rows[i]["ID"].ToString(), l_Sortdt.Rows[i]["ID"].ToString()));

            }
            this.cbx_Ptype.Value = "全部";


            DataTable l_Techdt = dal.getTechnicclass().Tables[0];

            this.cbx_PTechType.Items.Add(new Goldnet.Ext.Web.ListItem("全部", "全部"));
            for (int i = 0; i < l_Techdt.Rows.Count; i++)
            {
                this.cbx_PTechType.Items.Add(new Goldnet.Ext.Web.ListItem(l_Techdt.Rows[i]["techID"].ToString(), l_Techdt.Rows[i]["techID"].ToString()));

            }
            this.cbx_PTechType.Value = "全部";



            DataTable l_leveldt = dal.getCivilserviceclass().Tables[0];

            this.cbx_PLevel.Items.Add(new Goldnet.Ext.Web.ListItem("全部", "全部"));
            for (int i = 0; i < l_leveldt.Rows.Count; i++)
            {
                this.cbx_PLevel.Items.Add(new Goldnet.Ext.Web.ListItem(l_leveldt.Rows[i]["civID"].ToString(), l_leveldt.Rows[i]["civID"].ToString()));

            }

            this.cbx_PLevel.Value = "全部";



            DataTable l_Degeedt = dal.getDegee().Tables[0];

            this.cbx_PCollage.Items.Add(new Goldnet.Ext.Web.ListItem("全部", "全部"));
            for (int i = 0; i < l_Degeedt.Rows.Count; i++)
            {
                this.cbx_PCollage.Items.Add(new Goldnet.Ext.Web.ListItem(l_Degeedt.Rows[i]["eduID"].ToString(), l_Degeedt.Rows[i]["eduID"].ToString()));

            }

            this.cbx_PCollage.Value = "全部";

            DataTable l_Titlelistdt = dal.getTitlelist().Tables[0];

            this.cbx_PTech.Items.Add(new Goldnet.Ext.Web.ListItem("全部", "全部"));
            for (int i = 0; i < l_Titlelistdt.Rows.Count; i++)
            {
                this.cbx_PTech.Items.Add(new Goldnet.Ext.Web.ListItem(l_Titlelistdt.Rows[i]["ID"].ToString(), l_Titlelistdt.Rows[i]["ID"].ToString()));

            }
            this.cbx_PTech.Value = "全部";


        }

        public static string _DeptPower;
        public static string DeptPower
        {
            set { _DeptPower = value; }
            get { return _DeptPower; }
        }

        public static string _FromDate;
        public static string FromDate
        {
            set { _FromDate = value; }
            get { return _FromDate; }
        }

        public static string _ToDate;
        public static string ToDate
        {
            set { _ToDate = value; }
            get { return _ToDate; }
        }
    }
}
