using System;
using System;
using System.Data;
using Goldnet.Ext.Web;
using GoldNet.Comm.Pic;
using Goldnet.Dal.home;
using GoldNet.Model;

namespace GoldNet.JXKP.mainpage
{
    public partial class main_kpi_yzb : System.Web.UI.Page
    {
        /// <summary>
        /// 初始化处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            //检查是否已经登录，否则停止
            if (Session["CURRENTSTAFF"] == null)
            {
                Response.End();
            }

            if (!Ext.IsAjaxRequest)
            {
                this.Store1.DataSource = GetStoreData();
                this.Store1.DataBind();
            }
        }

        /// <summary>
        /// 数据刷新
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Store_RefreshData(object sender, StoreRefreshDataEventArgs e)
        {
            //绑定Store数据源
            Store1.DataSource = GetStoreData();
            Store1.DataBind();
        }

        /// <summary>
        /// 获取关键指标并显示图表
        /// </summary>
        /// <returns></returns>
        private DataTable GetStoreData()
        {
            //取得传入的选择的会话状态
            //获取当前登陆人员所在核算科室
            string deptcode = Session["curdeptcode"] == null ? ((User)Session["CURRENTSTAFF"]).AccountDeptCode : Session["curdeptcode"].ToString();
            string personid = Session["curpersonid"] == null ? ((User)Session["CURRENTSTAFF"]).UserId : Session["curpersonid"].ToString();
            //日期范围
            string yearstr = Session["curdateyear"] == null ? DateTime.Now.ToString("yyyy") : Session["curdateyear"].ToString();
            string months = Session["curmonths"] == null ? DateTime.Now.ToString("MM") : Session["curmonths"].ToString();
            //获取当前登陆人员岗位
            string stationcode = Session["curstationcode"] == null ? ((User)Session["CURRENTSTAFF"]).GetStationCode(((User)Session["CURRENTSTAFF"]).StaffId, yearstr) : Session["curstationcode"].ToString();
            //获取人员权限类别：院级；科级
            string meunid = System.Configuration.ConfigurationManager.AppSettings["PageMeunid"].ToString();
            string modid = System.Configuration.ConfigurationManager.AppSettings["modid"].ToString();
            string org = ((User)Session["CURRENTSTAFF"]).GetUserOrg(meunid, modid, personid);

            string curguide = "";
            if (org == "1")
            {
                //院级重点监控指标
                curguide = System.Configuration.ConfigurationManager.AppSettings["curguide"].ToString();
            }
            else
            {
                //科级重点监控指标
                curguide = System.Configuration.ConfigurationManager.AppSettings["deptcurguide"].ToString();
            }

            //人员ID
            //personid = ((User)Session["CURRENTSTAFF"]).GetStaffid(personid);
            personid = Session["curpersonid"] == null ? ((User)Session["CURRENTSTAFF"]).StaffId : ((User)Session["CURRENTSTAFF"]).GetStaffid(Session["curpersonid"].ToString());

            HomeDal dal = new HomeDal();
            DataTable dt = dal.GetGuidess_yzb(deptcode, stationcode, curguide, yearstr, personid, months).Tables[0];

            //获取指标数据后绘制图表
            GuagePicNew pic = new GuagePicNew();
            string temp = dt.Rows.Count == 0 ? "0" : dt.Rows[0]["WCBFB"].ToString();
            string mbz = dt.Rows.Count == 0 ? "0" : dt.Rows[0]["MBZ"].ToString();
            string wcz = dt.Rows.Count == 0 ? "0" : dt.Rows[0]["WCZ"].ToString();

            pic.BuideGuagePic(dt.Rows.Count > 0 ? Convert.ToString(Convert.ToDouble(temp)) : "0", yearstr + "年" + months + "月药占比", "目标值：" + mbz, "完成值：" + wcz, "3");
            string guidname = "0";
            string guidmbz = "0";
            string guidwcz = "0";
            string guidwcbfb = "0";
            if (dt.Rows.Count > 0)
            {
                guidname = dt.Rows[0]["ZBMC"].ToString();
                guidmbz = dt.Rows[0]["MBZ"].ToString();
                guidwcz = dt.Rows[0]["WCZ"].ToString();
                guidwcbfb = dt.Rows[0]["WCBFB"].ToString();
            }
            else
            {
                dt.Rows.Add("0", "0", "0", "0", "0");
            }
            return dt;
        }
    }
}
