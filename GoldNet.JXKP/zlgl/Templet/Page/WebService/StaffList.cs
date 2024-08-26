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

namespace GoldNet.JXKP.zlgl.Templet.Page.WebService
{
    public class StaffList
    {
        public StaffList(string staffid, string name, string deptname)
        {
            this.STAFF_ID = staffid;
            this.NAME = name;
            this.DEPT_NAME = deptname;
        }

        public StaffList()
        {
        }

        public string STAFF_ID { get; set; }

        public string NAME { get; set; }

        public string DEPT_NAME { get; set; }
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
        public static Paging<StaffList> PlantsPaging(int start, int limit, string sort, string dir, string filter, string staffid)
        {
            List<StaffList> users = new List<StaffList>();
            Goldnet.Dal.SYS_ROLE_DICT dal = new Goldnet.Dal.SYS_ROLE_DICT();
            string filters = "";
            if (!string.IsNullOrEmpty(filter) && filter != "*")
            {
                filters = filter.Trim().ToUpper();
            }
            DataTable table = dal.GetStaffUsers(filters.Trim(), staffid).Tables[0];

            for (int i = 0; i < table.Rows.Count; i++)
            {
                StaffList user = new StaffList();
                user.STAFF_ID = table.Rows[i]["staff_id"].ToString();
                user.NAME = table.Rows[i]["name"].ToString();
                user.DEPT_NAME = table.Rows[i]["dept_name"].ToString();
                users.Add(user);
            }
            if (!string.IsNullOrEmpty(sort))
            {
                users.Sort(delegate(StaffList x, StaffList y)
                {
                    object a;
                    object b;

                    int direction = dir == "DESC" ? -1 : 1;

                    a = x.GetType().GetProperty(sort).GetValue(x, null);
                    b = y.GetType().GetProperty(sort).GetValue(y, null);

                    return CaseInsensitiveComparer.Default.Compare(a, b) * direction;
                });
            }

            if ((start + limit) > users.Count)
            {
                limit = users.Count - start;
            }

            List<StaffList> rangeusers = (start < 0 || limit < 0) ? users : users.GetRange(start, limit);

            return new Paging<StaffList>(rangeusers, users.Count);
        }

    }
}