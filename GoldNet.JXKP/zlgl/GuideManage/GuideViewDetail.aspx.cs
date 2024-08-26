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
    public partial class GuideViewDetail : PageBase
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
            dt = dal.GetGuideContent().Tables[0];
            this.Store1.DataSource = dt;
            this.Store1.DataBind();
        }
        //刷新
        protected void Store_RefreshData(object sender, StoreRefreshDataEventArgs e)
        {
            Bindlist();
        }
        
     
        //反序列化得到客户端提交的gridpanel数据行      
        private Dictionary<string, string>[] GetSelectRow(AjaxEventArgs e)
        {
            string row = e.ExtraParams["Values"].ToString();
            Dictionary<string, string>[] selectRow = JSON.Deserialize<Dictionary<string, string>[]>(row);
            if (selectRow.Length <= 0) { return null; } else { return selectRow; }
        }

        protected void BtnGuideName_Click(object sender, System.EventArgs e)
        {
            Response.Redirect("GuideName.aspx");
        }

        protected void BtnGuideType_Click(object sender, System.EventArgs e)
        {
            Response.Redirect("GuideType.aspx");
        }
    }
}
