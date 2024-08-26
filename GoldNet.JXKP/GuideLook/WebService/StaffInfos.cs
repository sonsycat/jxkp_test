using System;
using System.Data;
using System.Configuration;
using System.Collections;
using Goldnet.Ext.Web;
using System.Collections.Generic;


namespace GoldNet.JXKP.GuideLook.WebService
{
    public class StaffInfos
    {
        public StaffInfos(string staffid, string staffname,string inputcode)
        {
            this.STAFF_ID = staffid;
            this.STAFF_NAME = staffname;
            this.INPUT_CODE = inputcode;
        }

        public StaffInfos() { }

        public string STAFF_ID { get; set; }

        public string STAFF_NAME { get; set; }

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
        public static Paging<StaffInfos> PlantsPaging(int start, int limit, string sort, string dir, string filter, string deptfilter)
        {
            List<StaffInfos> deptlist = new List<StaffInfos>();

            Goldnet.Dal.StatementDal dal = new Goldnet.Dal.StatementDal();

            DataTable table = dal.GetStaffInfo(filter, deptfilter).Tables[0];

            for (int i = 0; i < table.Rows.Count; i++)
            {
                StaffInfos dept = new StaffInfos();

                dept.STAFF_ID = table.Rows[i]["STAFF_ID"].ToString();
                dept.STAFF_NAME = table.Rows[i]["STAFF_NAME"].ToString();
                dept.DEPT_NAME = table.Rows[i]["DEPT_NAME"].ToString();
                dept.INPUT_CODE = table.Rows[i]["INPUT_CODE"].ToString();
                deptlist.Add(dept);
            }
            if (!string.IsNullOrEmpty(sort))
            {
                deptlist.Sort(delegate(StaffInfos x, StaffInfos y)
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

            List<StaffInfos> rangePlants = (start < 0 || limit < 0) ? deptlist : deptlist.GetRange(start, limit);

            return new Paging<StaffInfos>(rangePlants, deptlist.Count);
        }
    }
}
