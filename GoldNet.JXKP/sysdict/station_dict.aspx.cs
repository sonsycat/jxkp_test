using System;
using System.Data;
using System.Data.OleDb;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using Goldnet.Ext.Web;
using GoldNet.Comm;
using GoldNet.Comm.DAL.Oracle;
using System.Collections;


namespace GoldNet.JXKP.sysdict
{
    public partial class station_dict : System.Web.UI.Page
    {
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
                this.Store1.DataSource = GetStoreData();
                this.Store1.DataBind();

            }

        }

        protected void Store_RefreshData(object sender, StoreRefreshDataEventArgs e)
        {
            //绑定Store数据源
            Store1.DataSource = GetStoreData();
            Store1.DataBind();
        }

        /// <summary>
        /// 字典数据
        /// </summary>
        private DataTable GetStoreData()
        {
            Goldnet.Dal.Station_Dict dal = new Goldnet.Dal.Station_Dict();
            DataTable dt = dal.GetStationDictList().Tables[0];
            return dt;
        }



         //刷新按钮触发事件
        protected void Btn_Refresh_Click(object sender, AjaxEventArgs e)
        {
            Store1.DataSource = GetStoreData();
            Store1.DataBind();            
        }

         //预览按钮触发事件，构造岗位组织树
        [AjaxMethod]
        public string Btn_Preview_Click()
        {
            Goldnet.Dal.Station_Dict dal = new Goldnet.Dal.Station_Dict();
            DataTable dt = dal.GetStationDictTreeList().Tables[0];
            Hashtable rootnode = new Hashtable();

            TreePanel tree = new TreePanel();
            Goldnet.Ext.Web.TreeNode root = new Goldnet.Ext.Web.TreeNode();
            root.NodeID = "root";
            root.Text = "岗位体系";
            root.Expanded = true;
            tree.Root.Add(root);

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                string type_code = dt.Rows[i]["TYPECODE"].ToString();
                string type_name = dt.Rows[i]["TYPENAME"].ToString();
                string station_code = dt.Rows[i]["ID"].ToString();
                string station_name = dt.Rows[i]["STATION_NAME"].ToString();

                Goldnet.Ext.Web.TreeNode node1 = new Goldnet.Ext.Web.TreeNode();
                node1.NodeID = type_code;
                node1.Text =   type_name;
                node1.Icon = (Icon)Enum.Parse(typeof(Icon), "Folder");

                Goldnet.Ext.Web.TreeNode node2 = new Goldnet.Ext.Web.TreeNode();
                node2.NodeID = station_code;
                node2.Text = station_name;
                node2.Icon = (Icon)Enum.Parse(typeof(Icon), "Folder");

                string nodekey1 = type_code;
                string nodekey2 = station_code;

                if (rootnode.Contains(nodekey1))
                {
                    Goldnet.Ext.Web.TreeNode pnode = (Goldnet.Ext.Web.TreeNode)rootnode[nodekey1];
                    pnode.Nodes.Add(node2);
                }
                else
                {
                    if (!(station_code == null | station_code.Equals("")))
                    { 
                        node1.Nodes.Add(node2); 
                    }
                    rootnode.Add(nodekey1, node1);
                    root.Nodes.Add(node1);

                }
            }
            return tree.Root.ToJson();
        }



        #region 页面功能按钮事件######################################################################
       

        //添加按钮触发事件
        protected void Btn_Add_Click(object sender, AjaxEventArgs e)
        {
            //定义一个HashTable,将前台编辑按钮所选中的行数据复制到定义的HashTable对象selectRow中
            Dictionary<string, string>[] selectRow = GetSelectRow(e);
            string id = "";
            if (selectRow != null)
            {
                id = selectRow[0]["ID"];
            }
            LoadConfig loadcfg = getLoadConfig("station_dict_detail.aspx");
            loadcfg.Params.Add(new Goldnet.Ext.Web.Parameter("op", "add"));
            loadcfg.Params.Add(new Goldnet.Ext.Web.Parameter("id", id));
            showDetailWin(loadcfg, "添加岗位信息");
        }




        //编辑按钮触发事件
        protected void Btn_Edit_Click(object sender, AjaxEventArgs e)
        {

            //定义一个HashTable,将前台编辑按钮所选中的行数据复制到定义的HashTable对象selectRow中
            Dictionary<string, string>[] selectRow = GetSelectRow(e);
            //判断如果selectRow里为null，则返回空
            if (selectRow == null)
            {
                return;
            }

            LoadConfig loadcfg = getLoadConfig("station_dict_detail.aspx");
            loadcfg.Params.Add(new Goldnet.Ext.Web.Parameter("op", "edit"));
            loadcfg.Params.Add(new Goldnet.Ext.Web.Parameter("id", selectRow[0]["ID"]));
            showDetailWin(loadcfg, "编辑岗位信息");
            
           
          
        }
        //删除按钮触发事件
        protected void Btn_Del_Click(object sender, AjaxEventArgs e)
        {
            //定义一个HashTable,将前台编辑按钮所选中的行数据复制到定义的HashTable对象selectRow中
            Dictionary<string, string>[] selectRow = GetSelectRow(e);
            //判断如果selectRow里为null，则返回空
            if (selectRow == null)
            {
                return;
            }
            LoadConfig loadcfg = getLoadConfig("station_dict_detail.aspx");
            loadcfg.Params.Add(new Goldnet.Ext.Web.Parameter("op", "del"));
            loadcfg.Params.Add(new Goldnet.Ext.Web.Parameter("id", selectRow[0]["ID"]));
            showDetailWin(loadcfg,"删除岗位信息");
        }

        #endregion

        //显示详细窗口
        private void showDetailWin( LoadConfig loadcfg,string title)
        {
            DetailWin.ClearContent();
            if (!title.Trim().Equals("")) 
            {
                DetailWin.SetTitle(title);
            }
            DetailWin.Center();
            DetailWin.Show();
            DetailWin.LoadContent(loadcfg);
        }

        //载入参数设置
        private LoadConfig getLoadConfig(string url)
        {
            LoadConfig loadcfg = new LoadConfig();
            loadcfg.Url = url;
            loadcfg.Mode = LoadMode.IFrame;
            loadcfg.MaskMsg = "载入中...";
            loadcfg.ShowMask = true;
            loadcfg.NoCache = true;
            return loadcfg;
        }

        //反序列化得到客户端提交的gridpanel数据行
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






    }
}
