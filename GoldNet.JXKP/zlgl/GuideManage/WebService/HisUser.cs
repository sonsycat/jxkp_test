using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using Goldnet.Ext.Web;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;

namespace GoldNet.JXKP.zlgl.SysManage.WebService
{
    public class HisUser
    {
        public HisUser(string userid, string dbuser, string username)
        {
            this.USER_ID = userid;
            this.DB_USER = dbuser;
            this.USER_NAME = username;
        }

        public HisUser()
        {
        }

        public string USER_ID { get; set; }

        public string DB_USER { get; set; }

        public string USER_NAME { get; set; }
        /// <summary>
        /// 查询his人员
        /// </summary>
        /// <param name="start"></param>
        /// <param name="limit"></param>
        /// <param name="sort"></param>
        /// <param name="dir"></param>
        /// <param name="filter"></param>
        /// <param name="deptfilter"></param>
        /// <returns></returns>
        public static Paging<HisUser> PlantsPaging(int start, int limit, string sort, string dir, string filter, string deptfilter)
        {
            List<HisUser> hisusers = new List<HisUser>();
            Goldnet.Dal.SYS_ROLE_DICT dal = new Goldnet.Dal.SYS_ROLE_DICT();
            string filters = "";
            if (!string.IsNullOrEmpty(filter) && filter != "*")
            {
                filters = filter.Trim().ToUpper();
            }
            DataTable table = dal.GetHisUsers(filters.Trim(), deptfilter).Tables[0];

            for (int i = 0; i < table.Rows.Count; i++)
            {
                HisUser user = new HisUser();
                user.USER_ID = table.Rows[i]["user_id"].ToString();
                user.DB_USER = table.Rows[i]["db_user"].ToString();
                user.USER_NAME = table.Rows[i]["user_name"].ToString();
                hisusers.Add(user);
            }
            if (!string.IsNullOrEmpty(sort))
            {
                hisusers.Sort(delegate(HisUser x, HisUser y)
                {
                    object a;
                    object b;

                    int direction = dir == "DESC" ? -1 : 1;

                    a = x.GetType().GetProperty(sort).GetValue(x, null);
                    b = y.GetType().GetProperty(sort).GetValue(y, null);

                    return CaseInsensitiveComparer.Default.Compare(a, b) * direction;
                });
            }

            if ((start + limit) > hisusers.Count)
            {
                limit = hisusers.Count - start;
            }

            List<HisUser> rangeusers = (start < 0 || limit < 0) ? hisusers : hisusers.GetRange(start, limit);

            return new Paging<HisUser>(rangeusers, hisusers.Count);
        }

        public static List<HisUser> TestData
        {
            get
            {
                Goldnet.Dal.SYS_ROLE_DICT dal = new Goldnet.Dal.SYS_ROLE_DICT();
                DataTable table = dal.GetHisUsers().Tables[0];
                List<HisUser> data = new List<HisUser>();
                for (int i = 0; i < table.Rows.Count; i++)
                {
                    HisUser plant = new HisUser();
                    plant.USER_ID = table.Rows[i]["user_id"].ToString();
                    plant.DB_USER = table.Rows[i]["db_user"].ToString();
                    plant.USER_NAME = table.Rows[i]["user_name"].ToString();
                    data.Add(plant);
                }
                return data;
            }
        }
    }
}
