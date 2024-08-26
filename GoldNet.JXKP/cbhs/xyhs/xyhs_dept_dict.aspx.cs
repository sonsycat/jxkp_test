using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using Goldnet.Dal;
using Goldnet.Ext.Web;
using System.Collections.Generic;

namespace GoldNet.JXKP.cbhs.xyhs
{
    public partial class xyhs_dept_dict : PageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Ext.IsAjaxRequest)
            {
                //检查是否已经登录，否则停止
                if (Session["CURRENTSTAFF"] == null)
                {
                    Response.End();
                }
                setDict();
                Bindlist();

            }

        }
        //查询绑定数据到界面
        public void Bindlist()
        {
            XyhsDict dal = new XyhsDict();
            string deptType = this.Combo_DeptType.SelectedItem.Value.ToString();
            string showFlag = this.ComShowflag.SelectedItem.Value.ToString();
            DataTable dt = dal.GetDeptList(deptType, showFlag).Tables[0];

            this.Store1.DataSource = dt;
            this.Store1.DataBind();

        }
        //初始化科室类别查询条件下拉框
        private void setDict()
        {
            XyhsDict dal = new XyhsDict();
            DataTable dt = dal.GetDeptType().Tables[0];
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    this.Combo_DeptType.Items.Add(new Goldnet.Ext.Web.ListItem(dt.Rows[i]["XYHS_DEPT_TYPE"].ToString(), dt.Rows[i]["ID"].ToString()));
                }
            }
            HttpProxy pro3 = new HttpProxy();
            pro3.Method = HttpMethod.POST;
            pro3.Url = "WebService/xyhs_dept_dicts.ashx";
            this.Store3.Proxy.Add(pro3);
        }
        //查询条件改变
        protected void SelectedDepttype(object sender, EventArgs e)
        {
            Bindlist();
        }
        //查询事件
        protected void Button_look_click(object sender, EventArgs e)
        {
            Bindlist();
        }
        protected void Button_save_click(object sender, AjaxEventArgs e)
        {
             RowSelectionModel sm = this.GridPanel1.SelectionModel.Primary as RowSelectionModel;
             if (sm.SelectedRows.Count > 0)
             {
                 //
                 string deptcode = sm.SelectedRow.RecordID;
                 Dictionary<string, string>[] selectRow = GetSelectRow(e);
                 if (selectRow != null)
                 {
                     XyhsOperation dal = new XyhsOperation();

                     try
                     {
                         dal.Savetohisdept(selectRow, deptcode);

                         DataTable dt = dal.Select_dept_to_hisdept(deptcode).Tables[0];
                         DataRow dr = dt.NewRow();
                         dt.Rows.Add(dr);
                         this.Store2.DataSource = dt;
                         this.Store2.DataBind();

                     }
                     catch (Exception ex)
                     {
                         this.ShowDataError(ex.Message.ToString(), Request.Path, "Button_Save_click");
                     }
                 }
                 else
                 {
                     this.ShowMessage("提示","请选择要保存的行！");
                 }
             }

        }
        private Dictionary<string, string>[] GetSelectRow(AjaxEventArgs e)
        {
            string row = e.ExtraParams["Valuess"].ToString();
            Dictionary<string, string>[] selectRow = JSON.Deserialize<Dictionary<string, string>[]>(row);
            if (selectRow.Length <= 0) { return null; } else { return selectRow; }
        }
        protected void Button_delto_click(object sender, AjaxEventArgs e)
        {
            XyhsOperation dal = new XyhsOperation();

            Dictionary<string, string>[] selectRow = GetSelectRow(e);

            RowSelectionModel sm = this.GridPanel2.SelectionModel.Primary as RowSelectionModel;
            if (selectRow == null || selectRow.Length < 1)
            {
                Goldnet.Ext.Web.Ext.Msg.Show(new Goldnet.Ext.Web.MessageBox.Config
                {
                    Title = "信息提示",
                    Message = "请至少选择一条记录",
                    Buttons = MessageBox.Button.OK,
                    Icon = (MessageBox.Icon)Enum.Parse(typeof(MessageBox.Icon), "INFO")
                });
            }

            else
            {
                try
                {
                    for (int i = 0; i < selectRow.Length; i++)
                    {
                        string deptcode = selectRow[i]["DEPT_CODE"];
                        if (!deptcode.Equals(""))
                        {
                            dal.DeltohisDept(deptcode);
                        }
                    }
                    RowSelectionModel sm1 = this.GridPanel1.SelectionModel.Primary as RowSelectionModel;
                    string dept_code = sm1.SelectedRow.RecordID;
                    DataTable dt = dal.Select_dept_to_hisdept(dept_code).Tables[0];
                    DataRow dr = dt.NewRow();
                    dt.Rows.Add(dr);
                    this.Store2.DataSource = dt;
                    this.Store2.DataBind();
                }
                catch (Exception ex)
                {
                    this.ShowDataError(ex.Message.ToString(), Request.Path, "Button_del_click");
                }
            }
        }
        //设置事件
        protected void Button_edit_click(object sender, EventArgs e)
        {
            RowSelectionModel sm = this.GridPanel1.SelectionModel.Primary as RowSelectionModel;
            if (sm.SelectedRows.Count < 1)
            {
                this.ShowMessage("提示", "请选择一条记录！");
            }
            else
            {
                string dept_code = sm.SelectedRow.RecordID;
                LoadConfig loadcfg = getLoadConfig("xyhs_dept_set.aspx");
                loadcfg.Params.Add(new Goldnet.Ext.Web.Parameter("dept_code", dept_code));
                showDetailWin(loadcfg);
            }

           
        }
        protected void Button_add_click(object sender, EventArgs e)
        {
            LoadConfig loadcfg = getLoadConfig("xyhs_dept_set.aspx");
            loadcfg.Params.Add(new Goldnet.Ext.Web.Parameter("dept_code", "AAAAAAAA9"));
            showDetailWin(loadcfg);

        }
        //显示添加窗口
        private void showDetailWin(LoadConfig loadcfg)
        {
            DeptSetWin.ClearContent();
            DeptSetWin.Show();
            DeptSetWin.LoadContent(loadcfg);
        }
        //删除事件
        protected void Button_del_click(object sender, EventArgs e)
        {
            RowSelectionModel sm = this.GridPanel1.SelectionModel.Primary as RowSelectionModel;
            if (sm.SelectedRows.Count < 1)
            {
                this.ShowMessage("提示", "请选择一条记录！");
            }
            else
            {
                string dept_code = sm.SelectedRow.RecordID;
                XyhsDict dal = new XyhsDict();
                dal.DelDept(dept_code);
                Bindlist();

            }

        }
        //查找
        protected void select_dept(object sender, AjaxEventArgs e)
        {
            string deptType = this.Combo_DeptType.SelectedItem.Value.ToString();
            string showFlag = this.ComShowflag.SelectedItem.Value.ToString();
            string filter = this.txt_SearchTxt.Text;
            XyhsOperation dal = new XyhsOperation();
            DataSet ds = dal.GetDeptList(deptType, showFlag, filter);
            this.Store1.DataSource = ds;
            this.Store1.DataBind();
        }
        protected void select_hisdept(object sender, AjaxEventArgs e)
        {
            RowSelectionModel sm = this.GridPanel1.SelectionModel.Primary as RowSelectionModel;
            if (sm.SelectedRows.Count >0)
            {
                string dept_code = sm.SelectedRow.RecordID;
                
                XyhsOperation dal = new XyhsOperation();
                DataTable  dt = dal.Select_dept_to_hisdept(dept_code).Tables[0];
                DataRow dr = dt.NewRow();
                dt.Rows.Add(dr);
                this.Store2.DataSource = dt;
                this.Store2.DataBind();
            }
        }
        protected void Store_RefreshData(object sender, StoreRefreshDataEventArgs e)
        {
            Bindlist();
        }

        /// <summary>
        /// 同步his科室
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param> 
        protected void Button_sync_his_click(object sender, EventArgs e)
        {
            //string date_time = this.years.SelectedItem.Value + this.months.SelectedItem.Value;
            //string date_time = Convert.ToDateTime(this.stardate.Value.ToString()).ToString("yyyyMM");
            XyhsDetail dal = new XyhsDetail();

            string rtMsg = "";
            try
            {
                rtMsg = dal.Exec_sync_his_Deal();
                if (rtMsg == "")
                {
                    this.ShowMessage("系统提示", "同步his科室数据成功！");
                    //Bindlist();
                }
                else
                {
                    this.ShowDataError(rtMsg, Request.Path, "Button_sync_his_click");
                }

            }
            catch (Exception ex)
            {
                this.ShowDataError(ex.Message.ToString(), Request.Path, "Button_sync_his_click");
            }

        }






    }
}
