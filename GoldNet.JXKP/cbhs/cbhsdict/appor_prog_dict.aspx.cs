using System;
using System.Collections.Generic;
using Goldnet.Dal;
using Goldnet.Ext.Web;
using System.Data;
using System.Collections;

namespace GoldNet.JXKP
{

    public partial class appor_prog_dict : PageBase
    {
        private Appor_Prog_Dict dal_appor = new Appor_Prog_Dict();

        /// <summary>
        /// 
        /// </summary>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Ext.IsAjaxRequest)
            {
                Store1.DataSource = dal_appor.GetProgDict();
                Store1.DataBind();
            }
        }

        /// <summary>
        /// 编辑按钮触发事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Btn_Edit_Click(object sender, AjaxEventArgs e)
        {
            Dictionary<string, string>[] selectRow = GetSelectRow(e);
            if (selectRow != null)
            {
                LoadConfig loadcfg = getLoadConfig("appor_prog_dict_edit.aspx");
                loadcfg.Params.Add(new Goldnet.Ext.Web.Parameter("ApporProgID", selectRow[0]["ID"]));
                loadcfg.Params.Add(new Goldnet.Ext.Web.Parameter("ApproProgEditMode", "Edit"));
                showDetailWin(loadcfg);
            }
        }

        /// <summary>
        /// 添加按钮触发事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Btn_Add_Click(object sender, AjaxEventArgs e)
        {
            LoadConfig loadcfg = getLoadConfig("appor_prog_dict_edit.aspx");
            loadcfg.Params.Add(new Goldnet.Ext.Web.Parameter("ApproProgEditMode", "Add"));
            showDetailWin(loadcfg);
        }

        /// <summary>
        /// 添加按钮触发事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Btn_Del_Click(object sender, AjaxEventArgs e)
        {
            Dictionary<string, string>[] selectRow = GetSelectRow(e);
            if (selectRow != null)
            {
                string id = selectRow[0]["ID"].ToString();
                try
                {
                    dal_appor.DeleteProgDict(id);
                    Goldnet.Ext.Web.Ext.Msg.Show(new Goldnet.Ext.Web.MessageBox.Config
                    {
                        Title = "提示",
                        Message = "删除成功",
                        Buttons = MessageBox.Button.OK,
                        Icon = (MessageBox.Icon)Enum.Parse(typeof(MessageBox.Icon), "INFO")
                    });
                    Store_RefreshData(null, null);
                }
                catch (Exception ex)
                {
                    ShowDataError(ex.ToString(), Request.Url.LocalPath, "DeleteApporProg");
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Store_RefreshData(object sender, StoreRefreshDataEventArgs e)
        {
            //绑定Store数据源
            Store1.DataSource = dal_appor.GetProgDict();
            Store1.DataBind();
        }

        //显示详细窗口
        private void showDetailWin(LoadConfig loadcfg)
        {
            DetailWin.ClearContent();
            DetailWin.Show();
            DetailWin.LoadContent(loadcfg);
        }

        //反序列化得到客户端提交的gridpanel数据行      
        private Dictionary<string, string>[] GetSelectRow(AjaxEventArgs e)
        {
            string row = e.ExtraParams["Values"].ToString();
            Dictionary<string, string>[] selectRow = JSON.Deserialize<Dictionary<string, string>[]>(row);
            if (selectRow.Length <= 0) { return null; } else { return selectRow; }
        }

        /// <summary>
        /// 
        /// </summary>
        [AjaxMethod]
        public void DefaultDeptShow()
        {
            DefaultDeptWin.Show();

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="faid"></param>
        [AjaxMethod]
        public void SQLExpressShow(string id, string faid)
        {
            DataTable dt = dal_appor.GetProgDict(id);
            if (dt != null && dt.Rows.Count > 0)
            {
                DataTable dtSQL = dal_appor.GetProgSqlList(faid);

                string express = dt.Rows[0]["PROG_EXPRESS"].ToString();
                if (express != "")
                {
                    string[] name = express.Split('+');
                    if (name.Length > 0)
                    {
                        for (int j = 0; j < name.Length; j++)
                        {
                            if (name[j] != "")
                            {
                                string[] namexpress = name[j].Split('*');
                                if (namexpress.Length > 0)
                                {
                                    for (int z = 0; z < dtSQL.Rows.Count; z++)
                                    {
                                        if (dtSQL.Rows[z]["PROG_ITEM"].ToString() == namexpress[0])
                                        {
                                            if (namexpress.Length > 1)
                                            {
                                                dtSQL.Rows[z]["PROG_VALUES"] = namexpress[1];
                                            }
                                            else
                                            {
                                                dtSQL.Rows[z]["PROG_VALUES"] = 1;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                PropertyGrid1.Source.Clear();
                for (int i = 0; i < dtSQL.Rows.Count; i++)
                {
                    NumberField nf = new NumberField();
                    nf.MaxValue = 1;
                    nf.MinValue = 0;
                    nf.DecimalPrecision = 6;

                    PropertyGridParameter pgp = new PropertyGridParameter(dtSQL.Rows[i]["PROG_NAME"].ToString(), dtSQL.Rows[i]["PROG_VALUES"].ToString());
                    pgp.Mode = ParameterMode.Raw;
                    pgp.Editor.Add(nf);
                    this.PropertyGrid1.AddScript("{0}.store.add(new Ext.grid.PropertyRecord({{name: {1}, value: {2} }}));",
                       PropertyGrid1.ClientID,
                       JSON.Serialize(pgp.Name),
                       pgp.Mode == ParameterMode.Value ? JSON.Serialize(pgp.Value) : pgp.Value);
                    PropertyGrid1.Source.Add(pgp);
                    PropertyGrid1.SetSource(PropertyGrid1.Source);


                }
                Session["PROG_SQLExpress_ID"] = id;
                DictWin.Show();

            }
            else
            {
                Goldnet.Ext.Web.Ext.Msg.Show(new Goldnet.Ext.Web.MessageBox.Config
                {
                    Title = "提示",
                    Message = "未找到相应的分摊比例字典",
                    Buttons = MessageBox.Button.OK,
                    Icon = (MessageBox.Icon)Enum.Parse(typeof(MessageBox.Icon), "INFO")
                });
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="str"></param>
        [AjaxMethod]
        public void Btn_Save(string str)
        {
            double total = 0;
            Hashtable hs = new Hashtable();
            if (str != "")
            {
                string[] name = str.Split(',');
                if (name.Length > 0)
                {
                    for (int j = 0; j < name.Length; j++)
                    {
                        if (name[j] != "")
                        {
                            string[] namexpress = name[j].Split(':');
                            if (namexpress.Length > 0)
                            {
                                if (Convert.ToDouble(namexpress[1]) > 0)
                                {
                                    total += Convert.ToDouble(namexpress[1]);
                                    hs.Add(namexpress[0], namexpress[1]);
                                }
                            }
                        }
                    }
                }
            }
            if (total != 1)
            {
                Goldnet.Ext.Web.Ext.Msg.Show(new Goldnet.Ext.Web.MessageBox.Config
                {
                    Title = "提示",
                    Message = "设置的值必须等于1",
                    Buttons = MessageBox.Button.OK,
                    Icon = (MessageBox.Icon)Enum.Parse(typeof(MessageBox.Icon), "INFO")
                });
                return;
            }
            string sqlExpress = "";
            DataTable dtSQL = dal_appor.GetProgSqlList();
            for (int i = 0; i < dtSQL.Rows.Count; i++)
            {
                foreach (DictionaryEntry de in hs)
                {
                    if (dtSQL.Rows[i]["PROG_NAME"].ToString() == de.Key.ToString())
                    {
                        sqlExpress += dtSQL.Rows[i]["PROG_ITEM"].ToString() + "*" + de.Value + "+";
                    }
                }
            }
            string id = Session["PROG_SQLExpress_ID"].ToString();
            sqlExpress = sqlExpress.Substring(0, sqlExpress.Length - 1);
            if (sqlExpress != "")
            {
                dal_appor.UpdateSQL(id, sqlExpress);
            }

            ScriptManager1.AddScript(" DictWin.hide(); ");
            ScriptManager1.AddScript(" Store1.reload();");
        }

        /// <summary>
        /// 建立科室树结构
        /// </summary>
        /// <returns></returns>
        [AjaxMethod]
        public string RefreshDeptTree(string row)
        {
            Dictionary<string, string>[] selectRow = JSON.Deserialize<Dictionary<string, string>[]>(row);
            string applDept = selectRow[0]["APPLY_DEPT"] == null ? "" : selectRow[0]["APPLY_DEPT"].ToString();

            Goldnet.Ext.Web.TreeNode root = new Goldnet.Ext.Web.TreeNode();
            root.NodeID = "root";
            root.Text = "指标体系";
            root.Expanded = true;



            TreeDalDict dal = new TreeDalDict();
            DataTable l_TreeDt = dal_appor.getDeptForGuideLook().Tables[0];

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
                bool contain = false;
                string[] aa = applDept.Split(',');
                for (int z = 0; z < aa.Length; z++)
                {
                    if (l_TreeDt.Rows[i]["id"].ToString() == aa[z])
                    {
                        contain = true;
                        break;
                    }
                }
                if (applDept != null && applDept != "" && contain)
                {
                    root.Checked = Goldnet.Ext.Web.ThreeStateBool.True;
                    node.Checked = Goldnet.Ext.Web.ThreeStateBool.True;
                }
                else
                {
                    node.Checked = Goldnet.Ext.Web.ThreeStateBool.False;
                }
                rootnode.Add(node.NodeID, node);
            }

            Goldnet.Ext.Web.TreeNodeCollection tnodec = new Goldnet.Ext.Web.TreeNodeCollection();
            tnodec.Add(root);

            return tnodec.ToJson();
        }

        /// <summary>
        /// 保存适用科室
        /// </summary>
        /// <param name="selectDept"></param>
        /// <param name="row"></param>
        [AjaxMethod]
        public void DefaultDept(string selectDept, string row)
        {
            try
            {
                Dictionary<string, string>[] selectRow = JSON.Deserialize<Dictionary<string, string>[]>(row);
                dal_appor.UpdateDefaultDept(selectRow[0]["ID"], selectDept);
                Goldnet.Ext.Web.Ext.Msg.Show(new Goldnet.Ext.Web.MessageBox.Config
                {
                    Title = "提示",
                    Message = "分摊方案设置科室成功",
                    Buttons = MessageBox.Button.OK,
                    Icon = (MessageBox.Icon)Enum.Parse(typeof(MessageBox.Icon), "INFO")
                });
                Goldnet.Ext.Web.ScriptManager scManager = Goldnet.Ext.Web.ScriptManager.GetInstance(this.Page);
                scManager.AddScript("DefaultDeptWin.hide();");
                scManager.AddScript("DefaultDeptWin.clearContent();");
                scManager.AddScript("Store1.reload();");
            }
            catch (Exception ex)
            {
                ShowDataError(ex.ToString(), Request.Url.LocalPath, "SaveProgDept");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="progcode"></param>
        [AjaxMethod]
        public void SetCost(string progcode)
        {
            LoadConfig loadcfg = getLoadConfig("prog_cost.aspx");
            loadcfg.Params.Add(new Goldnet.Ext.Web.Parameter("prog_code", progcode));
            showCenterSet(this.CostWin, loadcfg);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="progcode"></param>
        [AjaxMethod]
        public void SetDept(string progcode)
        {
            LoadConfig loadcfg = getLoadConfig("appor_prog_dept_set.aspx");
            loadcfg.Params.Add(new Goldnet.Ext.Web.Parameter("prog_code", progcode));
            showCenterSet(this.DeptWin, loadcfg);
        }
    }
}
