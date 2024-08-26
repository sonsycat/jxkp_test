using System;
using System.Web;
using GoldNet.Comm;

namespace GoldNet.JXKP
{
    public class AjaxSystem
    {

        public HttpContext Context;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        public AjaxSystem(HttpContext context)
        {
            this.Context = context;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="acctyear"></param>
        /// <returns></returns>
        public static AjaxResult PersonClick(string UNITCODE, string UNITNAME, string tag, string bonusid, string pageid, string tagMode)
        {
            try
            {
                string aa = "";
                if (!UNITCODE.Equals(""))
                {
                    //string tag = this.GetStringByQueryStr("tag");
                    //string bonusid = this.GetStringByQueryStr("tagID");
                    //string tagMode = this.GetStringByQueryStr("tagMode");
                    string deptId = UNITCODE;
                    string deptName = UNITNAME;
                    //HttpContext.Current.Response.Redirect("BonusPersonList.aspx?tag=" + EncryptTheQueryString(tag) + "&bonusid=" + EncryptTheQueryString(bonusid) + "&deptid=" + EncryptTheQueryString(deptId) + "&tagMode=" + EncryptTheQueryString(tagMode) + "&deptname=" + EncryptTheQueryString(deptName) + "&pageid=" + pageid, false);
                    aa = "BonusPersonList.aspx?tag=" + EncryptTheQueryString(tag) + "&bonusid=" + EncryptTheQueryString(bonusid) + "&deptid=" + EncryptTheQueryString(deptId) + "&tagMode=" + EncryptTheQueryString(tagMode) + "&deptname=" + EncryptTheQueryString(deptName) + "&pageid=" + pageid;
                }
                return AjaxResult.Success(aa, aa);
            }
            catch (Exception err)
            {
                return AjaxResult.Error(err.Message);
            }
        }


        public static AjaxResult BackClick(string tag, string bonusid, string pageid, string tagMode, string rMode)
        {
            try
            {
                string aa = "";
                //string tagMode = this.GetStringByQueryStr("tagMode");
                //string tagId = this.GetStringByQueryStr("tagID");
                if (rMode !="")
                {
                    aa="BonusShow.aspx?tag=" + EncryptTheQueryString(tagMode) + "&bonusid=" + EncryptTheQueryString(bonusid) + "&pageid=" + pageid;
                }
                else
                {
                    aa="BonusList.aspx?tag=" + EncryptTheQueryString(tagMode) + "&pageid=" + pageid;
                }
                return AjaxResult.Success(aa, aa);
            }
            catch (Exception err)
            {
                return AjaxResult.Error(err.Message);
            }
        }

        protected static string EncryptTheQueryString(string theStr)
        {
            return HttpUtility.UrlEncode(Encrypt.EncryptMyStr("iloveyou", theStr));
        }

    }
}