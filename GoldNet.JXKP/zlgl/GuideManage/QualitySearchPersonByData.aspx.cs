using System;
using System.Drawing;
using System.Data;
using System.Data.OleDb;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.Text.RegularExpressions;
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
    public partial class QualitySearchPersonByData : PageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Ext.IsAjaxRequest)
            {
                this.stardate.Value = System.DateTime.Now.ToString("yyyy-MM") + "-01";
                this.enddate.Value = System.DateTime.Now.ToString("yyyy-MM-dd");
                this.GridPanel1.ColumnModel.RegisterCommandStyleRules();
                ScriptManager1.RegisterIcon(Goldnet.Ext.Web.Icon.Accept);
                HttpProxy pro = new HttpProxy();
                pro.Method = HttpMethod.POST;
                //pro.Url = "WebService/AccountDepts.ashx?deptfilter=" + this.DeptFilter("dept_code");
                pro.Url = "WebService/HisDepts.ashx?deptfilter=" + this.DeptFilter("dept_code");
                this.Store2.Proxy.Add(pro);

                HttpProxy pro0 = new HttpProxy();
                pro0.Method = HttpMethod.POST;
                pro0.Url = "WebService/StaffLists.ashx";
                this.Store0.Proxy.Add(pro0);

                JsonReader jr = new JsonReader();
                jr.ReaderID = "DEPT_CODE";
                jr.Root = "deptlist";
                jr.TotalProperty = "totalCount";
                RecordField rf = new RecordField();
                rf.Name = "DEPT_CODE";
                jr.Fields.Add(rf);
                RecordField rfn = new RecordField();
                rfn.Name = "DEPT_NAME";
                jr.Fields.Add(rfn);
                this.Store2.Reader.Add(jr);
                //
                DataTable daterow = QualityGuide.GetAllDateDesc().Tables[0];
                this.Store3.DataSource = daterow;
                this.Store3.DataBind();
                SetDict();
            }

        }

        public void SetDict()
        {
            Goldnet.Dal.Guide_Manager dal = new Goldnet.Dal.Guide_Manager();

            DataTable guidetype = dal.GetDDLguidetype().Tables[0];
            for (int i = 0; i < guidetype.Rows.Count; i++)
            {
                this.ComGuide.Items.Add(new Goldnet.Ext.Web.ListItem(guidetype.Rows[i]["GuideType"].ToString(), guidetype.Rows[i]["ID"].ToString()));
            }
           
        }
       
        

        protected void GetQueryPortalet(object sender, EventArgs e)
        {
            GetPageData();
        }
        protected void Store_RefreshData(object sender, StoreRefreshDataEventArgs e)
        {
            GetPageData();
        }
        protected void Storedate_RefreshData(object sender, StoreRefreshDataEventArgs e)
        {
            DataTable daterow = QualityGuide.GetAllDateDesc().Tables[0];
            this.Store3.DataSource = daterow;
            this.Store3.DataBind();
            
        }
        private void GetPageData()
        {
            Goldnet.Dal.Guide_Manager dal = new Goldnet.Dal.Guide_Manager();
            string tablename = dal.GetTableName(this.commonguide.SelectedItem.Value);
            GetCname(tablename);
            

        }
        private void GetCname(string tablename)
        {
            Goldnet.Dal.Guide_Manager dal = new Goldnet.Dal.Guide_Manager();
            DataSet ds = dal.GetColumnName(tablename);

            string db_dept_code = string.Empty;//科室编码
            string db_staff_id = string.Empty;//人员编码
            string db_date = string.Empty;//时间
            string db_numer = string.Empty;//得分

            string startdate = Convert.ToDateTime(this.stardate.SelectedValue).ToString("yyyyMM");
            string enddate = Convert.ToDateTime(this.enddate.SelectedValue).ToString("yyyyMM");

            for (int i = 0; i < ds.Tables[0].Columns.Count; i++)
            {
                //科室编码
                Regex r_dept_id = new Regex("DEPT_ID_");
                Match m_dept_id = r_dept_id.Match(ds.Tables[0].Columns[i].ColumnName);
                if (m_dept_id.Success)
                {
                    if (db_dept_code == string.Empty)
                    {
                        db_dept_code = ds.Tables[0].Columns[i].ColumnName;
                    }
                }

                //人员编码
                Regex r_staff_id = new Regex("STAFF_ID_");
                Match m_staff_id = r_staff_id.Match(ds.Tables[0].Columns[i].ColumnName);
                if (m_staff_id.Success)
                {
                    if (db_staff_id == string.Empty)
                    {
                        db_staff_id = ds.Tables[0].Columns[i].ColumnName;
                    }
                }

                //时间
                Regex r_date = new Regex("DATE_");
                Match m_date = r_date.Match(ds.Tables[0].Columns[i].ColumnName);
                if (m_date.Success)
                {
                    if (db_date == string.Empty)
                    {
                        db_date = ds.Tables[0].Columns[i].ColumnName;
                    }
                }

                //得分
                Regex r_numer = new Regex("NUMBER_");
                Match m_numer = r_numer.Match(ds.Tables[0].Columns[i].ColumnName);
                if (m_numer.Success)
                {
                    if (db_numer == string.Empty)
                    {
                        db_numer = ds.Tables[0].Columns[i].ColumnName;
                    }
                }
            }

            DataSet ds_res = dal.GetSelectData(tablename, db_dept_code, db_staff_id, db_date, db_numer, startdate, enddate, ComAccountdeptcode.SelectedItem.Value, Com_Director.SelectedItem.Value, this.ComGuide.SelectedItem.Text, this.commonguide.SelectedItem.Text);
            DataTable dt = ds_res.Tables[0];
            int n = dt.Rows.Count;
            this.Store1.DataSource = dt;
            this.Store1.DataBind();

        }
        protected void SelectedGuodeType(object sender, AjaxEventArgs e)
        {
            Goldnet.Dal.Guide_Manager dal = new Goldnet.Dal.Guide_Manager();
            Goldnet.Ext.Web.ScriptManager scManager = Goldnet.Ext.Web.ScriptManager.GetInstance(this.Page);
            scManager.AddScript(commonguide.ClientID + ".store.removeAll();");
            scManager.AddScript(commonguide.ClientID + ".clearValue();");

            DataTable dt = dal.Getddl_commonguide(this.ComGuide.SelectedItem.Value.ToString()).Tables[0];
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                this.commonguide.AddItem(dt.Rows[i]["COMMGUIDENAME"].ToString(), dt.Rows[i]["TEMPLETID"].ToString());
            }
            this.commonguide.SelectedIndex = 0;

        }


    }
}
