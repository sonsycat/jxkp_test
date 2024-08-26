using System;
using System.Data;
using Goldnet.Dal;
using Goldnet.Ext.Web;
using GoldNet.Comm;
using GoldNet.Comm.ExportData;
using GoldNet.Model;

namespace GoldNet.JXKP
{
    public partial class BonusDeptList_New : PageBase
    {

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Ext.IsAjaxRequest)
            {
                //SetDict();
                GetDeptBouns();
            }
        }

        /// <summary>
        /// EXCEL导出
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void OutExcel(object sender, EventArgs e)
        {
            //if (Session["BonusDeptList"] != null)
            //{
            //    //tag：0表示核算科室，1表示平均奖科室
            //    string tag = this.GetStringByQueryStr("tag");
            //    //表示奖金ID
            //    string bonusid = this.GetStringByQueryStr("tagID");
            //    ExportData ex = new ExportData();
            //    DataTable dt = (DataTable)Session["BonusDeptList"];
            //    CalculateBonus calculateBouns = new CalculateBonus();
            //    calculateBouns.SetBonusDeptListExcel(bonusid, tag, dt);
            //    MHeaderTabletoExcel(dt, null, "科室奖金", null, 0);
            //}
        }

        /// <summary>
        /// 返回处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void BackClick(object sender, AjaxEventArgs e)
        {
            //string tagMode = this.GetStringByQueryStr("tagMode");
            //string tagId = this.GetStringByQueryStr("tagID");
            //if (Request.QueryString["rMode"] != null)
            //{
            //    Response.Redirect("BonusShow.aspx?tag=" + this.EncryptTheQueryString(tagMode) + "&bonusid=" + this.EncryptTheQueryString(tagId) + "&pageid=" + Request.QueryString["pageid"].ToString());
            //}
            //else
            //{
            //    Response.Redirect("BonusList.aspx?tag=" + this.EncryptTheQueryString(tagMode) + "&pageid=" + Request.QueryString["pageid"].ToString());
            //}
        }

        /// <summary>
        /// 跳转到科室中人的列表页面
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void PersonClick(string UNITCODE, string UNITNAME)
        {
            //try
            //{
            //    if (!UNITCODE.Equals(""))
            //    {
            //        string tag = this.GetStringByQueryStr("tag");
            //        string bonusid = this.GetStringByQueryStr("tagID");
            //        string deptId = UNITCODE;
            //        string deptName = UNITNAME;
            //        string tagMode = this.GetStringByQueryStr("tagMode");
            //        Response.Redirect("BonusPersonList.aspx?tag=" + this.EncryptTheQueryString(tag) + "&bonusid=" + this.EncryptTheQueryString(bonusid) + "&deptid=" + this.EncryptTheQueryString(deptId) + "&tagMode=" + this.EncryptTheQueryString(tagMode) + "&deptname=" + this.EncryptTheQueryString(deptName) + "&pageid=" + Request.QueryString["pageid"].ToString());
            //    }
            //}
            //catch (Exception err)
            //{

            //}
        }

        /// <summary>
        /// 下拉框值改变处理事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Item_SelectOnChange(object sender, EventArgs e)
        {
            GetDeptBouns();
        }

        /// <summary>
        /// 设置查询控件初始值
        /// </summary>
        protected void SetDict()
        {
            //this.depttype.Items.Add(new Goldnet.Ext.Web.ListItem("全部", ""));
            //CalculateBonus dal = new CalculateBonus();
            //DataTable dt = new DataTable();
            //dt = dal.GetDeptType();
            //for (int i = 0; i < dt.Rows.Count; i++)
            //{
            //    this.depttype.Items.Add(new Goldnet.Ext.Web.ListItem(dt.Rows[i]["TYPE_NAME"].ToString(), dt.Rows[i]["TYPE_CODE"].ToString()));
            //}
        }

        /// <summary>
        /// 数据获取并显示
        /// </summary>
        private void GetDeptBouns()
        {
            //tag：0表示核算科室，1表示平均奖科室
            string tag = this.GetStringByQueryStr("tag");
            //获取奖金ID
            string bonusid = this.GetStringByQueryStr("tagID");
            string pgid = Request.QueryString["pageid"].ToString();
            string tagMode = this.GetStringByQueryStr("tagMode");

            CalculateBonus calculateBouns = new CalculateBonus();
            string conditin = this.DeptFilter("");
            //获得科室的奖金金额列表
            DataTable dtBonusValue = calculateBouns.GetBonusValue(bonusid, tag, conditin, "全部");

            //奖金指标为0不显示科室的处理
            System.Data.DataView dv = dtBonusValue.DefaultView;
            //if ((this.cbbType.SelectedItem.Value == "1" || this.cbbType.SelectedItem.Value == "") && tag == "1")
            //{
            string filters = "";
            string[] guides = GetConfig.GetConfigString("BonusGuide").Split(',');
            for (int i = 0; i < guides.Length; i++)
            {
                if (guides[i].ToString() != "")
                    filters += " A" + guides[i].ToString() + "<>0 or";
            }
            if (filters.Length > 2)
                dv.RowFilter = filters.Substring(0, filters.Length - 2);
            // }

            string org = GetUserOrg();

            BuildControl buildControl = new BuildControl();
            //根据Tag生成奖金的列
            string user_id = ((User)Session["CURRENTSTAFF"]).UserId;
            DataTable coltb = buildControl.BuildDeptBonusDetail_new(bonusid, tag, org, user_id, conditin);
            WelcomeLabel1.Columns = coltb;
            DataTable dt = dv.Table;
            WelcomeLabel1.DataViewSource = dt;
            WelcomeLabel1.Tag = tag;
            WelcomeLabel1.TagID = bonusid;
            WelcomeLabel1.Pageid = pgid;
            WelcomeLabel1.TagMode = tagMode;
            WelcomeLabel1.RMode = Request.QueryString["rMode"];

            Session.Remove("BonusDeptList");
            Session["BonusDeptList"] = dtBonusValue;
        }

    }
}
