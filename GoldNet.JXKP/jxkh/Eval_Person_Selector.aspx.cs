using System;
using Goldnet.Ext.Web;
using System.Data;
using System.Collections;
using Goldnet.Dal;
using System.Text;

namespace GoldNet.JXKP.jxkh
{
    public partial class Eval_Person_Selector : System.Web.UI.Page
    {
        Goldnet.Dal.Appraisal dal = new Goldnet.Dal.Appraisal();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["CURRENTSTAFF"] == null)
            {
                Response.End();
            }
            if (!Ext.IsAjaxRequest)
            {
                HttpProxy pro1 = new HttpProxy();
                pro1.Method = HttpMethod.POST;
                string deptfilter = Request.QueryString["deptFilter"].ToString();
                pro1.Url = "WebService/StaffDepts.ashx?deptfilter=" + deptfilter;
                this.Store3.Proxy.Add(pro1);

                InitComboxValue();
            }
        }

        /// <summary>
        /// 初始化人员属性选项
        /// </summary>
        private void InitComboxValue()
        {
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
            this.cbx_TimeOrgan.Value = "全部";
        }

        /// <summary>
        /// 查询按钮事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void QueryStaff(object sender, AjaxEventArgs e)
        {
            StringBuilder str = new StringBuilder();

            #region //------根据下拉框组织查询条件------//

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
            if (this.DeptCodeCombo.SelectedItem.Value != "")
            {
                str.Append(" and dept_code='" + this.DeptCodeCombo.SelectedItem.Value + "' ");
            }
            else
            {
                string deptfilter = Request.QueryString["deptFilter"].ToString();
                if (deptfilter != "")
                {
                    str.Append(" and " + deptfilter);
                }
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

            #endregion

            this.Store1.RemoveAll();
            Goldnet.Dal.Appraisal dal = new Goldnet.Dal.Appraisal();

            DataTable l_dt = dal.GetStaff(str.ToString()).Tables[0];
            string multi1 = e.ExtraParams["multi1"];
            Goldnet.Ext.Web.ListItem[] items1 = JSON.Deserialize<Goldnet.Ext.Web.ListItem[]>(multi1);
            ArrayList array = new ArrayList();
            for (int i = 0; i < items1.Length; i++)
            {
                array.Add(items1[i].Value.ToString());
            }
            for (int i = 0; i < l_dt.Rows.Count; i++)
            {
                if (array.Contains(l_dt.Rows[i]["PERSON_CODE"].ToString()))
                {
                    l_dt.Rows.RemoveAt(i);
                    i--;
                }
            }
            this.Store1.DataSource = l_dt;
            this.Store1.DataBind();
        }

        /// <summary>
        /// 组建条件字符
        /// </summary>
        /// <param name="item">元素</param>
        /// <returns></returns>
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
    }
}
