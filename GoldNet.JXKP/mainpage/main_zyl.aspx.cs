using System;
using System.Drawing;
using System.Data;
using System.Data.OleDb;
using System.Collections.Generic;
using System.Text;
using Goldnet.Ext.Web;
using GoldNet.Comm;
using GoldNet.Comm.Pic;
using GoldNet.Comm.DAL.Oracle;
using dotnetCHARTING;
using GoldNet.Model;
using Goldnet.Dal.home;

namespace GoldNet.JXKP.mainpage
{
    public partial class main_zyl : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //检查是否已经登录，否则停止
            if (Session["CURRENTSTAFF"] == null)
            {
                Response.End();
            }

            if (!Ext.IsAjaxRequest)
            {
                GetStoreData();
            }
        }


        /// <summary>
        /// 住院量数据及绘图
        /// </summary>
        private DataTable GetStoreData()
        {
            string meunid = System.Configuration.ConfigurationManager.AppSettings["PageMeunid"].ToString();
            string modid = System.Configuration.ConfigurationManager.AppSettings["modid"].ToString();
            string power = ((User)Session["CURRENTSTAFF"]).GetUserDeptFilter("", meunid, modid);
            // 科室
            string DeptCode = power == "" ? "" : ((User)Session["CURRENTSTAFF"]).AccountDeptCode;
            string deptcode = Session["curdeptcode"] == null ? DeptCode : Session["curdeptcode"].ToString();
            // 年度
            string yearstr = Session["curdateyear"] == null ? DateTime.Now.ToString("yyyy") : Session["curdateyear"].ToString();
            // 岗位
            string stationcode = Session["curstationcode"] == null ? ((User)Session["CURRENTSTAFF"]).GetStationCode(((User)Session["CURRENTSTAFF"]).StaffId, DateTime.Now.Year.ToString()) : Session["curstationcode"].ToString();
            // 人员ID
            string personid = Session["curpersonid"] == null ? ((User)Session["CURRENTSTAFF"]).StaffId : ((User)Session["CURRENTSTAFF"]).GetStaffid(Session["curpersonid"].ToString());
            
            DataTable dt = GetZYL(deptcode, stationcode, personid,yearstr);
            
            //double[] values = { Convert.ToDouble(dt.Rows[0]["CYRS"].ToString()), Convert.ToDouble(dt.Rows[0]["TQCYRS"].ToString()), Convert.ToDouble(dt.Rows[0]["JDCYRS"].ToString()), Convert.ToDouble(dt.Rows[0]["TQJDCYRS"].ToString()) };
            //string[] names = { "出院人数", "同期出院人数", "军队出院病人", "同期军队出院病人" };
            if (dt.Rows.Count > 0)
            {
                Charting c = new Charting();
                c.Title = dt.Rows[0]["BT"].ToString();
                c.XTitle = "";
                c.YTitle = "";
                c.PicHight = 270;
                c.PicWidth = 360;
                c.PhaysicalImagePath = "TempImages";
                c.FileName = "imagezyl";
                c.Type = SeriesType.Column;
                c.Use3D = true;
                c.DataSource = GetDataSource1(dt);
                c.CreateStatisticPic(this.Chartzyl);

                if (dt.Rows[0]["BS"].Equals("0"))
                {
                    string js = "<script type='text/javascript'>";
                    js += "parent.window.showChart('02'); </script> ";
                    ClientScript.RegisterStartupScript(GetType(), "registerJS", js);
                }
            }
            return dt;

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        private SeriesCollection GetDataSource1(DataTable dt)
        {
            dotnetCHARTING.SeriesCollection SC = new SeriesCollection();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                Series s = new Series();
                s.Name = dt.Rows[i]["ZBMC"].ToString();
                Element e = new Element();
                e.Name = "";
                e.YValue = Convert.ToDouble(dt.Rows[i]["WCZ"].ToString());
                s.Elements.Add(e);
                SC.Add(s);
            }
            return SC;
        }

        /// <summary>
        /// 住院量
        /// </summary>
        /// <param name="deptcode"></param>
        /// <param name="datetime"></param>
        /// <returns></returns>
        public DataTable GetZYL(string deptcode, string stationcode, String personid, String yearst)
        {
            HomeDal dal = new HomeDal();
            DataTable dt = dal.GetMzl(deptcode, stationcode, personid, yearst, "03").Tables[0];
            return dt;
        }
    }
}
