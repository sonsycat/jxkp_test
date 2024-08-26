using System;
using System.Data;
using System.Text;
using Goldnet.Ext.Web;
using GoldNet.Comm;
using GoldNet.Model;


namespace GoldNet.JXKP.home
{
    public partial class header : System.Web.UI.Page
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            string str1 = "";
            string str2 = "";
            string str3 = "";
            string str4 = "";
            string str5 = "";
            string str6 = "";
            string str7 = "";
            string str8 = "";
            string str9 = "";
            string str10 = "";
            string url = "";
            if (this.Request.QueryString.Count > 0)
            {
                url = this.Request.QueryString[0];
            }
            string classtr = "class='current'";
            if (url.Equals("9"))
            {
                str9 = classtr;
            }
            else if (url.Equals("1"))
            {
                str1 = classtr;
            }
            else if (url.Equals("2"))
            {
                str2 = classtr;
            }
            else if (url.Equals("3"))
            {
                str3 = classtr;
            }
            else if (url.Equals("4"))
            {
                str4 = classtr;
            }
            else if (url.Equals("5"))
            {
                str5 = classtr;
            }
            else if (url.Equals("6"))
            {
                str6 = classtr;
            }
            else if (url.Equals("7"))
            {
                str7 = classtr;
            }
            else if (url.Equals("8"))
            {
                str8 = classtr;
            }
            else if (url.Equals("10"))
            {
                str10 = classtr;
            }
            int width = 0;
            int left = 50;
            int wi = 30;
            int le = 5;
            if (Session["CURRENTSTAFF"] != null)
            { 
                //DataTable powtable = ((User)Session["CURRENTSTAFF"]).GetUserPower;
                DataTable powtable = (DataTable)Session["menu"];
                StringBuilder powstr = new StringBuilder();
                powstr.Append("<ul>");
                
                if (powtable.Select("modid='9'").Length > 0)
                {
                    if (str9 == "")
                        powstr.Append("<li><a href='/home/default.aspx?9'><span>系统管理</span></a></li>");
                    else
                        powstr.AppendFormat("<li><a href='/home/default.aspx?9' {0}><span>系统管理</span></a></li>", str9);
                    width += wi;
                    left += le;
                }
                if (powtable.Select("modid='10'").Length > 0)
                {
                    if (str10 == "")
                        powstr.Append("<li><a href='/home/default.aspx?10'><span>全成本核算</span></a></li>");
                    else
                        powstr.AppendFormat("<li><a href='/home/default.aspx?10' {0}><span>全成本核算</span></a></li>", str10);
                    width += wi;
                    left += le;
                }
                if (powtable.Select("modid='1'").Length > 0)
                {
                    if (str1 == "")
                        powstr.Append("<li><a href='/home/default.aspx?1'><span>科级核算</span></a></li>");
                    else
                        powstr.AppendFormat("<li><a href='/home/default.aspx?1' {0}><span>科级核算</span></a></li>", str1);
                    width += wi;
                    left += le;
                }
                if (powtable.Select("modid='3'").Length > 0)
                {
                    if (str3 == "")
                        powstr.Append("<li><a href='/home/default.aspx?3'><span>质量管理</span></a></li>");
                    else
                        powstr.AppendFormat("<li><a href='/home/default.aspx?3' {0}><span>质量管理</span></a></li>", str3);
                    width += wi;
                    left += le;
                }
                if (powtable.Select("modid='2'").Length > 0)
                {
                    if (str2 == "")
                        powstr.Append("<li><a href='/home/default.aspx?2'><span>技术档案</span></a></li>");
                    else
                        powstr.AppendFormat("<li><a href='/home/default.aspx?2' {0}><span>技术档案</span></a></li>", str2);
                    width += wi;
                    left += le;
                }
                if (powtable.Select("modid='7'").Length > 0)
                {
                    if (str7 == "")
                        powstr.Append("<li><a href='/home/default.aspx?7'><span>超劳补贴</span></a></li>");
                    else
                        powstr.AppendFormat("<li><a href='/home/default.aspx?7' {0}><span>超劳补贴</span></a></li>", str7);
                    width += wi;
                    left += le;
                }
                if (powtable.Select("modid='6'").Length > 0)
                {
                    if (str6 == "")
                        powstr.Append("<li><a href='/home/default.aspx?6'><span>数字中心</span></a></li>");
                    else
                        powstr.AppendFormat("<li><a href='/home/default.aspx?6' {0}><span>数字中心</span></a></li>", str6);
                    width += wi;
                    left += le;
                }
                if (powtable.Select("modid='5'").Length > 0)
                {
                    if (str5 == "")
                        powstr.Append("<li><a href='/home/default.aspx?5'><span>绩效考评</span></a></li>");
                    else
                        powstr.AppendFormat("<li><a href='/home/default.aspx?5' {0}><span>绩效考评</span></a></li>", str5);
                    width += wi;
                    left += le;
                }
                
                if (powtable.Select("modid='8'").Length > 0)
                {
                    powstr.AppendFormat("<li><a href='/home/default_02.aspx' {0}><span>运行监控</span></a></li>", str8);
                }
               
                powstr.Append("</ul>");
                this.menu.InnerHtml = powstr.ToString();
                this.HosTitle.InnerText = Constant.HospitalName;
                StringBuilder names = new StringBuilder();
                names.Append("<ul>");
                names.AppendFormat("<li>『姓名：{0},科室：{1}』     <a href='../auth/login/login.aspx'<span>注销</span></a></li>", ((User)Session["CURRENTSTAFF"]).UserName, ((User)Session["CURRENTSTAFF"]).AccountDeptName);
                names.Append("</ul>");
                this.loginuser.InnerHtml = names.ToString();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        [AjaxMethod]
        public void btnCancle()
        {
            Session.RemoveAll();
            Session.Clear();
            
        }
    }
}
