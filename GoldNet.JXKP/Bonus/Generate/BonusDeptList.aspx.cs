using System;
using System.Collections.Generic;
using System.Data;
using Goldnet.Dal;
using Goldnet.Ext.Web;
using GoldNet.Comm;
using GoldNet.Comm.ExportData;

namespace GoldNet.JXKP
{
    public partial class BonusDeptList : PageBase
    {
        //----------------------------------------------------------
        //1、显示核算科室或平均奖科室的奖金列表
        //2、tag：0表示核算科室，1表示平均奖科室
        ///注意：在页面传递参数是有很多参数在本页面没有用到，但是在页面跳转（返回）时，有的参数是其他页面用到的，所以在此页面时参数只是传递作用。如TagMode
        //-------------------------------------------------------------
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Ext.IsAjaxRequest)
            {
                SetDict();
                GetDeptBouns();
            }
        }

        //返回到BonusShow.aspx页面
        protected void Back_Click(object sender, AjaxEventArgs e)
        {
            string tagMode = this.GetStringByQueryStr("tagMode");
            string tagId = this.GetStringByQueryStr("tagID");
            if (Request.QueryString["rMode"] != null)
            {
                Response.Redirect("BonusShow.aspx?tag=" + this.EncryptTheQueryString(tagMode) + "&bonusid=" + this.EncryptTheQueryString(tagId) + "&pageid=" + Request.QueryString["pageid"].ToString());
            }
            else
            {
                Response.Redirect("BonusList.aspx?tag=" + this.EncryptTheQueryString(tagMode) + "&pageid=" + Request.QueryString["pageid"].ToString());
            }
        }

        //跳转到科室中人的列表页面
        protected void Person_Click(object sender, AjaxEventArgs e)
        {
            Dictionary<string, string>[] selectRow = GetSelectRow(e);
            if (selectRow != null)
            {
                if (selectRow[0]["UNIT_CODE"] != null)
                {
                    string tag = this.GetStringByQueryStr("tag");
                    string bonusid = this.GetStringByQueryStr("tagID");
                    string deptId = selectRow[0]["UNIT_CODE"];
                    string deptName = selectRow[0]["UNIT_NAME"];
                    string tagMode = this.GetStringByQueryStr("tagMode");
                    Response.Redirect("BonusPersonList.aspx?tag=" + this.EncryptTheQueryString(tag) + "&bonusid=" + this.EncryptTheQueryString(bonusid) + "&deptid=" + this.EncryptTheQueryString(deptId) + "&tagMode=" + this.EncryptTheQueryString(tagMode) + "&deptname=" + this.EncryptTheQueryString(deptName) + "&pageid=" + Request.QueryString["pageid"].ToString());
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
        //----------------------------------------------------------
        //1、获得科室的奖金金额列表
        ///2、根据Tag生成奖金的列（平均奖或核算科室）
        //-------------------------------------------------------------
        private void GetDeptBouns()
        {
            //tag：0表示核算科室，1表示平均奖科室
            string tag = this.GetStringByQueryStr("tag");
            //获取奖金ID
            string bonusid = this.GetStringByQueryStr("tagID");

            CalculateBonus calculateBouns = new CalculateBonus();
            string conditin = this.DeptFilter("");
            //获得科室的奖金金额列表
            DataTable dtBonusValue = calculateBouns.GetBonusValue(bonusid, tag, conditin, this.depttype.SelectedItem.Value);

            //奖金指标为0不显示科室的处理
            System.Data.DataView dv = dtBonusValue.DefaultView;
            if ((this.cbbType.SelectedItem.Value == "1" || this.cbbType.SelectedItem.Value == "") && tag == "1")
            {
                string filters = "";
                string[] guides = GetConfig.GetConfigString("BonusGuide").Split(',');
                for (int i = 0; i < guides.Length; i++)
                {
                    if (guides[i].ToString() != "")
                        filters += " A" + guides[i].ToString() + "<>0 or";
                }
                if (filters.Length > 2)
                    dv.RowFilter = filters.Substring(0, filters.Length - 2);
            }

            string org = GetUserOrg();

            BuildControl buildControl = new BuildControl();
            //根据Tag生成奖金的列
            buildControl.BuildDeptBonusDetail(bonusid, tag, Store1, GridPanel11,org);

            Store1.DataSource = dv;
            Store1.DataBind();
            Session.Remove("BonusDeptList");
            Session["BonusDeptList"] = dtBonusValue;
        }

        /// <summary>
        /// 数据导出
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void OutExcel(object sender, EventArgs e)
        {
            if (Session["BonusDeptList"] != null)
            {
                //tag：0表示核算科室，1表示平均奖科室
                string tag = this.GetStringByQueryStr("tag");
                //表示奖金ID
                string bonusid = this.GetStringByQueryStr("tagID");
                ExportData ex = new ExportData();
                DataTable dt = (DataTable)Session["BonusDeptList"];
                CalculateBonus calculateBouns = new CalculateBonus();
                calculateBouns.SetBonusDeptListExcel(bonusid, tag, dt);
                //ex.ExportToLocal(dt, this.Page, "xls", "科室奖金");
                //dt.Columns.Remove("DEPT_NAMES");
                //dt.Columns.Remove("UNIT_TYPE_NAME");
                MHeaderTabletoExcel(dt, null, "科室奖金", null, 0);
            }
        }

        /// <summary>
        /// 初始化科室类型下拉列表
        /// </summary>
        protected void SetDict()
        {
            this.depttype.Items.Add(new Goldnet.Ext.Web.ListItem("全部", ""));
            CalculateBonus dal = new CalculateBonus();
            DataTable dt = new DataTable();
            dt = dal.GetDeptType();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                this.depttype.Items.Add(new Goldnet.Ext.Web.ListItem(dt.Rows[i]["TYPE_NAME"].ToString(), dt.Rows[i]["TYPE_CODE"].ToString()));
            }
        }

        /// <summary>
        /// 科室类型选择处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Item_SelectOnChange(object sender, EventArgs e)
        {
            GetDeptBouns();
        }


    }
}
