using System;
using System.Data;
using System.Configuration;
using System.Collections;
using Goldnet.Ext.Web;
using System.Collections.Generic;

namespace GoldNet.JXKP.RLZY.WebService
{
    public class UserInfo
    {
        public UserInfo(string userid, string username,string inputcode,string deptname,string id)
        {
            this.USER_ID = userid;
            this.ID = id;
            this.USER_NAME = username;
            this.INPUT_CODE = inputcode;
            this.DEPT_NAME = deptname;
        }

        public UserInfo() { }

        public string USER_ID { get; set; }

        public string USER_NAME { get; set; }

        public string ID { get; set; }

        public string DEPT_NAME { get; set; }

        public string INPUT_CODE { get; set; }

        /// <summary>
        /// 查询his科室
        /// </summary>
        /// <param name="start"></param>
        /// <param name="limit"></param>
        /// <param name="sort"></param>
        /// <param name="dir"></param>
        /// <param name="filter"></param>
        /// <param name="deptfilter"></param>
        /// <returns></returns>
        public static Paging<UserInfo> PlantsPaging(int start, int limit, string sort, string dir, string filter, string staffid)
        {
            List<UserInfo> deptlist = new List<UserInfo>();

            Goldnet.Dal.BaseInfoMaintainDal dal = new Goldnet.Dal.BaseInfoMaintainDal();
            DataTable table = dal.GetUserInfo(filter,staffid).Tables[0];

            for (int i = 0; i < table.Rows.Count; i++)
            {
                
                UserInfo dept = new UserInfo();
                //dept.USER_ID = table.Rows[i]["USER_ID"].ToString() + "&" + table.Rows[i]["DB_USER"].ToString();
                dept.USER_ID = table.Rows[i]["USER_ID"].ToString();
                dept.ID = table.Rows[i]["USER_ID"].ToString();
                dept.USER_NAME = table.Rows[i]["USER_NAME"].ToString();
                dept.DEPT_NAME = table.Rows[i]["DEPT_NAME"].ToString();
                dept.INPUT_CODE = table.Rows[i]["DB_USER"].ToString();
                deptlist.Add(dept);
            }
            if (!string.IsNullOrEmpty(sort))
            {
                deptlist.Sort(delegate(UserInfo x, UserInfo y)
                {
                    object a;
                    object b;

                    int direction = dir == "DESC" ? -1 : 1;

                    a = x.GetType().GetProperty(sort).GetValue(x, null);
                    b = y.GetType().GetProperty(sort).GetValue(y, null);

                    return CaseInsensitiveComparer.Default.Compare(a, b) * direction;
                });
            }

            if ((start + limit) > deptlist.Count)
            {
                limit = deptlist.Count - start;
            }

            List<UserInfo> rangePlants = (start < 0 || limit < 0) ? deptlist : deptlist.GetRange(start, limit);

            return new Paging<UserInfo>(rangePlants, deptlist.Count);
        }
    }
}
