using System;
using System.Collections;
using System.Data;
using System.Text;
using Goldnet.Ext.Web;
using Goldnet.Dal;

namespace GoldNet.JXKP.GuideLook
{
    public partial class SetDept : System.Web.UI.Page
    {

        #region --页面初始化--

        /// <summary>
        /// 报表ID
        /// </summary>
        private static string m_rptid = null;

        #endregion

        #region --页面事件--

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Ext.IsAjaxRequest)
            {
                string rptid = Request.QueryString["rptid"].ToString();

                m_rptid = rptid;

                string terms = "";

                string DeptTreeType = "0";

                //回显的情况
                if (Session["ReportDeptCode"] != null)
                {
                    terms = Session["ReportDeptCode"].ToString();
                    DeptTreeType = Session["DeptTreeType"].ToString();
                    Session.Remove("ReportDeptCode");
                }
                else
                {
                    //数据库读取的情况
                    ReportDalDict dal = new ReportDalDict();
                    DataTable l_dt = dal.getReportTerms(m_rptid).Tables[0];
                    if (l_dt.Rows.Count > 0)
                    {
                        DeptTreeType = l_dt.Rows[0]["RPT_DPETTREETYPE"].ToString();
                        if (DeptTreeType == "0")
                        {
                            terms = l_dt.Rows[0]["RPT_TERMS"].ToString();
                        }
                        else
                        {
                            terms = l_dt.Rows[0]["PRT_SECOND_TERMS"].ToString();
                        }
                    }
                }
                if (terms != "")
                {

                    //显示右边已有项
                    DataTable l_ExRptRow = AnalyzeStringToRightSelect(terms);

                    if (l_ExRptRow.Rows.Count > 0)
                    {
                        this.Store2.DataSource = l_ExRptRow;
                        this.Store2.DataBind();
                    }

                    //绑定左边项和搜索条件
                    AnalyzeStringToLeft(terms, DeptTreeType);
                }

                this.Combo_DeptType.SelectedItem.Value = DeptTreeType;
                if (DeptTreeType == "0")
                {
                    TreeBuild(terms);
                }
                else
                {
                    DeptTreeBuild(terms);
                }
            }
        }

        /// <summary>
        /// 树节点选择变化事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void TreeChangeChecked(object sender, AjaxEventArgs e)
        {
            DeptDalDict dal = new DeptDalDict();

            string temp = e.ExtraParams["checkNodes"];
            string deptTreeType = this.Combo_DeptType.SelectedItem.Value;
            this.Store1.RemoveAll();
            //0为科室类别，1为2级科室
            if ("0".Equals(deptTreeType))
            {
                if (temp != "")
                {
                    //ArrayList l_Arr = this.ReportTerms(temp);
                    temp = temp.TrimEnd(new char[] { ';' }).Replace(";", "','").Insert(0, "'") + "'";
                    DataTable l_dt = dal.GetDeptLeftSelector(temp).Tables[0];
                    l_dt = LeftSelectorFilter(l_dt, e);
                    if (l_dt.Rows.Count > 0)
                    {
                        this.Store1.DataSource = l_dt;
                        this.Store1.DataBind();
                    }
                }
            }
            else
            {
                if (temp != "")
                {
                    DataTable l_dt = dal.AnalyzeTermsToDeptBySecond(getFilterDeptCode(temp)).Tables[0];
                    l_dt = LeftSelectorFilter(l_dt, e);
                    if (l_dt.Rows.Count > 0)
                    {
                        this.Store1.DataSource = l_dt;
                        this.Store1.DataBind();
                    }
                }
            }
        }

        /// <summary>
        /// 已选择的指标，科室保存数据库
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Next_Click(object sender, AjaxEventArgs e)
        {
            ReportDalDict dal = new ReportDalDict();

            GuideDalDict GuideDal = new GuideDalDict();

            string DeptTreeType = this.Combo_DeptType.SelectedItem.Value;

            string multi1 = e.ExtraParams["multi1"];

            Goldnet.Ext.Web.ListItem[] items1 = JSON.Deserialize<Goldnet.Ext.Web.ListItem[]>(multi1);

            ArrayList array = new ArrayList();
            for (int i = 0; i < items1.Length; i++)
            {
                array.Add(items1[i].Value.ToString());
            }

            string temp = e.ExtraParams["checkNodes"];


            if (this.ckx_Sort.Checked)
            {
                if (temp.Equals(""))
                {
                    Goldnet.Ext.Web.Ext.Msg.Show(new Goldnet.Ext.Web.MessageBox.Config
                    {
                        Title = "信息提示",
                        Message = "没有符合条件的部门,请重新选择条件",
                        Buttons = MessageBox.Button.OK,
                        Icon = (MessageBox.Icon)Enum.Parse(typeof(MessageBox.Icon), "INFO")
                    });
                    return;
                }
            }
            else
            {
                if (items1.Length == 0)
                {
                    Goldnet.Ext.Web.Ext.Msg.Show(new Goldnet.Ext.Web.MessageBox.Config
                    {
                        Title = "信息提示",
                        Message = "请选择科室",
                        Buttons = MessageBox.Button.OK,
                        Icon = (MessageBox.Icon)Enum.Parse(typeof(MessageBox.Icon), "INFO")
                    });
                    return;
                }
            }

            string Terms = "";
            //1:反向,0:正向
            if (this.ckx_Sort.Checked)
            {
                if (DeptTreeType == "0")
                {
                    //ArrayList l_Arr = this.ReportTerms(temp);
                    //反向的条件字符串
                    Terms = getDeptRowString(temp, array);
                }
                else
                {
                    string tmp1 = "";
                    for (int i = 0; i < array.Count; i++)
                    {
                        tmp1 = tmp1 + array[i].ToString() + ",";
                    }
                    if (tmp1 == "")
                    {
                        tmp1 = "*";
                    }
                    Terms = getFilterDeptCode(temp).TrimEnd(new char[] { ';' }).Replace(';', ',') + ";" + tmp1.TrimEnd(new char[] { ',' });
                }
                Terms = Terms + ";1";
            }
            else
            {
                if (DeptTreeType == "0")
                {
                    ArrayList l_Arr = new ArrayList();

                    //正向字符串
                    Terms = getDeptRowString("", array);
                }
                else
                {
                    string tmp1 = "";
                    for (int i = 0; i < array.Count; i++)
                    {
                        tmp1 = tmp1 + array[i].ToString() + ",";
                    }
                    if (tmp1 == "")
                    {
                        tmp1 = "*";
                    }
                    Terms = "*;" + tmp1.TrimEnd(new char[] { ',' });
                }
                Terms = Terms + ";0";
            }

            if (Session["SelectRightGuideItem"] != null)
            {
                DataTable l_dt = ((DataTable)Session["SelectRightGuideItem"]);

                GuideDal.UpdateReportGuideInfo(m_rptid, l_dt);

                Session.Remove("SelectRightGuideItem");
            }

            dal.UpdateReportTerms(m_rptid, Terms, DeptTreeType);

            Goldnet.Ext.Web.ScriptManager scManager = Goldnet.Ext.Web.ScriptManager.GetInstance(this.Page);
            scManager.AddScript("parent.Ext.Msg.show({ title: '系统提示', msg:  '报表保存成功!',icon: 'ext-mb-info',  buttons: { ok: true }  });");
            scManager.AddScript("parent.window.arcEditWindow.hide()");
        }

        /// <summary>
        /// 上一步
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Prev_Click(object sender, AjaxEventArgs e)
        {
            string multi1 = e.ExtraParams["multi1"];

            Goldnet.Ext.Web.ListItem[] items1 = JSON.Deserialize<Goldnet.Ext.Web.ListItem[]>(multi1);

            ArrayList array = new ArrayList();
            for (int i = 0; i < items1.Length; i++)
            {
                array.Add(items1[i].Value.ToString());
            }

            string Terms = "";

            string temp = e.ExtraParams["checkNodes"];

            string deptTreeType = this.Combo_DeptType.SelectedItem.Value;

            if ("0".Equals(deptTreeType))
            {
                //1:反向,0:正向
                if (this.ckx_Sort.Checked)
                {
                    //ArrayList l_Arr = this.ReportTerms(temp);
                    //反向的条件字符串
                    Terms = getDeptRowString(temp, array);
                    Terms = Terms + ";1";
                }
                else
                {
                    ArrayList l_Arr = new ArrayList();
                    //正向字符串
                    Terms = getDeptRowString("", array);
                    Terms = Terms + ";0";
                }
            }
            else
            {
                //1:反向,0:正向
                if (this.ckx_Sort.Checked)
                {
                    //反向的条件字符串,123
                    string tmp1 = "";
                    for (int i = 0; i < array.Count; i++)
                    {
                        tmp1 = tmp1 + array[i].ToString() + ",";
                    }
                    if (tmp1 == "")
                    {
                        tmp1 = "*";
                    }
                    Terms = temp.TrimEnd(new char[] { ';' }).Replace(';', ',') + ";" + tmp1.TrimEnd(new char[] { ',' });

                    Terms = Terms + ";1";
                }
                else
                {
                    //正向字符串
                    string tmp1 = "";
                    for (int i = 0; i < array.Count; i++)
                    {
                        tmp1 = tmp1 + array[i].ToString() + ",";
                    }
                    if (tmp1 == "")
                    {
                        tmp1 = "*";
                    }
                    Terms = "*;" + tmp1.TrimEnd(new char[] { ',' });

                    Terms = Terms + ";0";
                }
            }
            //保存条件
            Session["ReportDeptCode"] = Terms;

            Session["DeptTreeType"] = deptTreeType;

            Response.Redirect("ReportDetail.aspx?rptid=" + m_rptid + "");
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
            root.Text = "科室树型结构";
            root.Checked = Goldnet.Ext.Web.ThreeStateBool.False;
            root.Expanded = true;

            string DeptType = this.Combo_DeptType.SelectedItem.Value;

            TreeDalDict dal = new TreeDalDict();
            DataTable l_TreeDt = new DataTable();
            if (DeptType.Equals("0"))
            {
                l_TreeDt = dal.getSetReportDeptTreeBuilder().Tables[0];
            }
            else
            {
                l_TreeDt = dal.getSetReportDeptSecondTreeBuilder().Tables[0];
            }


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
                node.SingleClickExpand = true;
                node.Checked = Goldnet.Ext.Web.ThreeStateBool.False;
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

        #endregion

        #region --页面逻辑--

        /// <summary>
        /// 读取条件字符，查询出已选中科室
        /// </summary>
        /// <param name="terms">条件字符</param>
        /// <returns>已选中科室集合</returns>
        private ArrayList AnalyzeStringToTreeCheckBox(string terms)
        {
            ArrayList l_ar = new ArrayList();

            if (terms != "")
            {
                string[] AnalyzeString = terms.Split(';');

                if (AnalyzeString[0] != "*")
                {
                    string[] LcdString = AnalyzeString[0].Split(',');

                    for (int i = 0; i < LcdString.Length; i++)
                    {
                        l_ar.Add("l" + LcdString[i].ToString());
                    }
                }
                if (AnalyzeString[1] != "*")
                {
                    string[] DeptType = AnalyzeString[1].Split(',');

                    for (int j = 0; j < DeptType.Length; j++)
                    {
                        l_ar.Add(DeptType[j].ToString());
                    }
                }

            }
            return l_ar;
        }

        private ArrayList AnalyzeStringToDeptSecondTreeCheckBox(string terms)
        {
            ArrayList l_ar = new ArrayList();

            if (terms != "")
            {
                string[] AnalyzeString = terms.Split(';');

                if (AnalyzeString[0] != "*")
                {
                    string[] LcdString = AnalyzeString[0].Split(',');

                    for (int i = 0; i < LcdString.Length; i++)
                    {
                        l_ar.Add(LcdString[i].ToString());
                    }
                }
            }
            return l_ar;
        }

        /// <summary>
        /// 解析字符显示左面SELETOR项目
        /// </summary>
        /// <param name="terms">条件字符</param>
        private void AnalyzeStringToLeft(string terms, string DeptTreeType)
        {
            DeptDalDict dal = new DeptDalDict();

            string[] AnalyzeString = terms.Split(';');

            string SortMethod = AnalyzeString[AnalyzeString.Length - 1].ToString();

            if (SortMethod == "1")
            {
                if (DeptTreeType == "0")
                {
                    this.ckx_Sort.Checked = true;
                    this.Store1.DataSource = dal.AnalyzeStringLeftSelector(terms);
                    this.Store1.DataBind();
                }
                else
                {
                    this.ckx_Sort.Checked = true;
                    this.Store1.DataSource = dal.StringLeftSelector(terms);
                    this.Store1.DataBind();
                }
            }
            else
            {
                this.ckx_Sort.Checked = false;
            }

        }

        /// <summary>
        /// 创建树结构
        /// </summary>
        /// <param name="terms">条件字符</param>
        private void TreeBuild(string terms)
        {
            Goldnet.Ext.Web.TreeNode root = new Goldnet.Ext.Web.TreeNode();
            root.NodeID = "root";
            root.Text = "科室结构";
            root.Checked = Goldnet.Ext.Web.ThreeStateBool.False;
            root.Expanded = true;

            this.TreeCtrlDept.Root.Add(root);

            TreeDalDict dal = new TreeDalDict();

            DataTable l_TreeDt = dal.getSetReportDeptTreeBuilder().Tables[0];

            Hashtable rootnode = new Hashtable();

            ArrayList l_ar = AnalyzeStringToTreeCheckBox(terms);

            for (int i = 0; i < l_TreeDt.Rows.Count; i++)
            {
                string nodekey = "";
                Goldnet.Ext.Web.TreeNode node = new Goldnet.Ext.Web.TreeNode();
                node.NodeID = l_TreeDt.Rows[i]["id"].ToString();
                node.SingleClickExpand = true;

                string id = l_TreeDt.Rows[i]["id"].ToString();

                if (l_ar.Contains(id))
                {
                    node.Checked = Goldnet.Ext.Web.ThreeStateBool.True;
                }
                else
                {
                    node.Checked = Goldnet.Ext.Web.ThreeStateBool.False;
                }

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
                rootnode.Add(id, node);
            }

        }

        /// <summary>
        /// 创建树结构
        /// </summary>
        /// <param name="terms">条件字符</param>
        private void DeptTreeBuild(string terms)
        {
            Goldnet.Ext.Web.TreeNode root = new Goldnet.Ext.Web.TreeNode();
            root.NodeID = "root";
            root.Text = "科室结构";
            root.Checked = Goldnet.Ext.Web.ThreeStateBool.False;
            root.Expanded = true;

            this.TreeCtrlDept.Root.Add(root);

            TreeDalDict dal = new TreeDalDict();

            DataTable l_TreeDt = dal.getSetReportDeptSecondTreeBuilder().Tables[0];

            Hashtable rootnode = new Hashtable();

            ArrayList l_ar = AnalyzeStringToDeptSecondTreeCheckBox(terms);

            for (int i = 0; i < l_TreeDt.Rows.Count; i++)
            {
                string nodekey = "";
                Goldnet.Ext.Web.TreeNode node = new Goldnet.Ext.Web.TreeNode();
                node.NodeID = l_TreeDt.Rows[i]["id"].ToString();
                node.SingleClickExpand = true;

                string id = l_TreeDt.Rows[i]["id"].ToString();

                if (l_ar.Contains(id))
                {
                    node.Checked = Goldnet.Ext.Web.ThreeStateBool.True;
                }
                else
                {
                    node.Checked = Goldnet.Ext.Web.ThreeStateBool.False;
                }

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
                rootnode.Add(id, node);
            }

        }


        /// <summary>
        /// 过滤已选择项
        /// </summary>
        /// <param name="table"></param>
        /// <param name="e"></param>
        /// <returns></returns>
        private DataTable LeftSelectorFilter(DataTable table, AjaxEventArgs e)
        {
            string multi1 = e.ExtraParams["multi1"];
            Goldnet.Ext.Web.ListItem[] items1 = JSON.Deserialize<Goldnet.Ext.Web.ListItem[]>(multi1);


            ArrayList array = new ArrayList();
            for (int i = 0; i < items1.Length; i++)
            {
                array.Add(items1[i].Value.ToString());
            }

            try
            {

                for (int i = 0; i < table.Rows.Count; i++)
                {
                    if (array.Contains(table.Rows[i]["VALUE"].ToString()))
                    {
                        table.Rows.RemoveAt(i);
                        i--;
                    }

                }

            }
            catch
            {
                Ext.Msg.Alert("系统提示", "输入有误！").Show();
            }


            return table;

        }

        /// <summary>
        /// 选择项目，组成自定义字符条件
        /// </summary>
        /// <param name="list">选择项目</param>
        /// <param name="NotINList">排除项目</param>
        /// <returns>条件字符</returns>
        private string getDeptRowString(string list, ArrayList NotINList)
        {
            //临床的情况下记录临床内科室的编号
            ArrayList SubidArr = new ArrayList();

            //记录除临床以外的编号
            ArrayList ArrtId = new ArrayList();

            list = list.TrimEnd(new char[] { ';' });

            StringBuilder str = new StringBuilder();
            if (list != "")
            {
                for (int i = 0; i < list.Split(';').Length; i++)
                {
                    string id = list.Split(';')[i];

                    if (id.Substring(0, 1) == "l")
                    {
                        SubidArr.Add(id.Substring(1));
                    }
                    else
                    {
                        ArrtId.Add(id);
                    }
                }
            }


            if (SubidArr.Count > 0)
            {

                for (int j = 0; j < SubidArr.Count; j++)
                {
                    if (j == SubidArr.Count - 1)
                    {
                        str.Append(SubidArr[j].ToString());
                        str.Append(";");
                    }
                    else
                    {
                        str.Append(SubidArr[j].ToString());
                        str.Append(",");
                    }
                }
            }
            else
            {
                str.Append("*;");
            }
            if (ArrtId.Count > 0)
            {
                for (int x = 0; x < ArrtId.Count; x++)
                {
                    if (x == ArrtId.Count - 1)
                    {
                        str.Append(ArrtId[x].ToString());
                        str.Append(";");
                    }
                    else
                    {
                        str.Append(ArrtId[x].ToString());
                        str.Append(",");
                    }
                }
            }
            else
            {
                str.Append("*;");
            }
            if (NotINList.Count > 0)
            {

                for (int i = 0; i < NotINList.Count; i++)
                {
                    if (i == NotINList.Count - 1)
                    {
                        str.Append(NotINList[i].ToString());
                    }
                    else
                    {
                        str.Append(NotINList[i].ToString());
                        str.Append(",");
                    }
                }
            }
            else
            {
                str.Append("*");
            }

            return str.ToString();

        }

        /// <summary>
        /// 解析字符显示右边SELETOR项目
        /// </summary>
        /// <param name="terms">条件字符</param>
        /// <returns>项目数据集</returns>
        private DataTable AnalyzeStringToRightSelect(string terms)
        {
            DeptDalDict dal = new DeptDalDict();
            string[] AnalyzeStr = terms.Split(';');
            string RightItems = AnalyzeStr[AnalyzeStr.Length - 2].ToString();
            string deptCode = "*";
            if (RightItems != "*")
            {
                deptCode = RightItems.TrimEnd(new char[] { ',' }).Replace(",", "','").Insert(0, "'") + "'";
            }

            //显示已有部门
            return dal.getReportExDept(deptCode).Tables[0];
        }

        private string getFilterDeptCode(string temp)
        {
            string[] temp1 = temp.TrimEnd(new char[] { ';' }).Split(';');

            string tmp = "";

            for (int i = 0; i < temp1.Length; i++)
            {
                if (temp1[i].IndexOf("C") < 0)
                {
                    tmp += temp1[i] + ";";
                }
            }

            return tmp;
        }

        #endregion
    }
}
