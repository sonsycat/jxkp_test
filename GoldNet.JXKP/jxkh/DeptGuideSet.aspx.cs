using System;
using System.Data;
using System.Collections.Generic;
using Goldnet.Ext.Web;

namespace GoldNet.JXKP.jxkh
{
    public partial class DeptGuideSet : PageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Ext.IsAjaxRequest)
            {
                GetQueryPortalet(null, null);
            }
            //

        }
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Buttonadd_Click(object sender, AjaxEventArgs e)
        {
            string years = Request["stationyear"].ToString();
            string deptcode = Request["deptcode"].ToString();
           
            LoadConfig loadcfg = getLoadConfig("DeptGuideEdit.aspx");
            loadcfg.Params.Add(new Goldnet.Ext.Web.Parameter("deptcode", deptcode));
            loadcfg.Params.Add(new Goldnet.Ext.Web.Parameter("years", years));
            showCenterSet(this.RoleEdit, loadcfg);

        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Buttondel_Click(object sender, AjaxEventArgs e)
        {
            Dictionary<string, string>[] selectRow = GetSelectRow(e);
            if (selectRow != null)
            {
                Goldnet.Dal.StationManager dal = new Goldnet.Dal.StationManager();
                string deptcode = selectRow[0]["DEPT_CODE"];
                string guidecode = selectRow[0]["GUIDE_CODE"];
                string vsguidecode = selectRow[0]["VS_GUIDE_CODE"];
                //dal.DelDeptGuide(deptcode,guidecode,vsguidecode);
                if (dal.DelDeptGuide(deptcode, guidecode, vsguidecode))
                {
                    this.ShowMessage("系统提示", "删除成功");
                }
                else
                {
                    this.ShowMessage("系统提示", "删除失败");
                }
                Goldnet.Ext.Web.ScriptManager scManager = Goldnet.Ext.Web.ScriptManager.GetInstance(this.Page);
                scManager.AddScript("RefreshData();");
            }
            else
            {
                this.SelectRecord();
            }
        }
       
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Buttonedit_Click(object sender, AjaxEventArgs e)
        {
            Dictionary<string, string>[] selectRow = GetSelectRow(e);
            if (selectRow != null)
            {
                if (selectRow[0]["DEPT_CODE"] != null)
                {
                    string years = Request["stationyear"].ToString();

                    string deptcode = selectRow[0]["DEPT_CODE"];
                    string guidecode = selectRow[0]["GUIDE_CODE"];
                    string vsguidecode = selectRow[0]["VS_GUIDE_CODE"];
                    string guidecause = selectRow[0]["GUIDE_CAUSE"];
                    LoadConfig loadcfg = getLoadConfig("DeptGuideEdit.aspx");
                    loadcfg.Params.Add(new Goldnet.Ext.Web.Parameter("deptcode", deptcode));
                    loadcfg.Params.Add(new Goldnet.Ext.Web.Parameter("guidecode", guidecode));
                    loadcfg.Params.Add(new Goldnet.Ext.Web.Parameter("vsguidecode", vsguidecode));
                    loadcfg.Params.Add(new Goldnet.Ext.Web.Parameter("guidecause", guidecause));
                    loadcfg.Params.Add(new Goldnet.Ext.Web.Parameter("years", years));
                    showCenterSet(this.RoleEdit, loadcfg);
                }
            }
        }
     
        //反序列化得到客户端提交的gridpanel数据行      
        private Dictionary<string, string>[] GetSelectRow(AjaxEventArgs e)
        {
            string row = e.ExtraParams["Values"].ToString();
            Dictionary<string, string>[] selectRow = JSON.Deserialize<Dictionary<string, string>[]>(row);
            if (selectRow.Length <= 0) { return null; } else { return selectRow; }
        }
      

        protected void GetQueryPortalet(object sender, AjaxEventArgs e)
        {
            string deptcode = Request["deptcode"].ToString();
            DataTable table = GetStoreData(deptcode);

            this.Store1.DataSource = table;
            this.Store1.DataBind();
        }


        private DataTable GetStoreData(string deptcode)
        {
            Goldnet.Dal.StationManager dal = new Goldnet.Dal.StationManager();
            DataTable dt = dal.GetDeptGuide(deptcode);
            return dt;
        }
        protected void Store_RefreshData(object sender, StoreRefreshDataEventArgs e)
        {
            GetQueryPortalet(null, null);
        }
    }
}
