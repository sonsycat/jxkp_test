using System;
using System.Drawing;
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
using GoldNet.Comm.Pic;
using GoldNet.Comm.DAL.Oracle;
using GoldNet.Model;
using GoldNet.JXKP.BLL.Guide;

namespace GoldNet.JXKP.zlgl.SysManage
{
    public partial class DeptQualityView :PageBase
    {
        public DataTable dt;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Ext.IsAjaxRequest)
            {
                Bindlist();
            }
        }

        //查询、绑定数据
        public void Bindlist()
        {
            Goldnet.Dal.Guide_Manager dal = new Goldnet.Dal.Guide_Manager();
            string Deptcode = Request["Deptcode"].ToString();
            string DateDesc = Request["year"].ToString() + "-" + Request["month"].ToString();
            dt = dal.DeptScoreTable(Deptcode, DateDesc);
            this.Store1.DataSource = dt;
            this.Store1.DataBind();
        }
        //刷新
        protected void Store_RefreshData(object sender, StoreRefreshDataEventArgs e)
        {
            Bindlist();
        }
        protected void btnCancle_Click(object sender, EventArgs e)
        {
            Goldnet.Ext.Web.ScriptManager scManager = Goldnet.Ext.Web.ScriptManager.GetInstance(this.Page);
            scManager.AddScript("parent.guideDetail.hide();");
        }
        //设置
        protected void Button_set_click(object sender, AjaxEventArgs e)
        {
            Dictionary<string, string>[] selectRow = GetSelectRow(e);
            if (selectRow != null)
            {
                string templetid =this.EncryptTheQueryString(selectRow[0]["TEMPLETID"]);
                string datefieldid =this.EncryptTheQueryString(selectRow[0]["DATECOL"]);
                string deptfieldid = this.EncryptTheQueryString(selectRow[0]["TARGETCOL"]);
                string startdate = this.EncryptTheQueryString(selectRow[0]["STARTDATE"]);
                string enddate = this.EncryptTheQueryString(selectRow[0]["ENDDATE"]);
                string deptcode = this.EncryptTheQueryString(selectRow[0]["DEPTNAME"]);
                Response.Redirect("templet_view.aspx?templetid=" + templetid + "&datefieldid=" + datefieldid + "&deptfieldid=" + deptfieldid + "&startdate=" + startdate + "&enddate=" + enddate + "&deptcode=" + deptcode);
            }
        }
        //反序列化得到客户端提交的gridpanel数据行      
        private Dictionary<string, string>[] GetSelectRow(AjaxEventArgs e)
        {
            string row = e.ExtraParams["Values"].ToString();
            Dictionary<string, string>[] selectRow = JSON.Deserialize<Dictionary<string, string>[]>(row);
            if (selectRow.Length <= 0) { return null; } else { return selectRow; }
        }
    }
}
