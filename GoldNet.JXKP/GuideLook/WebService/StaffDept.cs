using System;
using System.Data;
using System.Configuration;
using Goldnet.Ext.Web;
using GoldNet.JXKP.WebService;
using System.Collections.Generic;
using System.Collections;

namespace GoldNet.JXKP.GuideLook.WebService
{
    public class StaffDept
    {
        public StaffDept(string deptcode, string deptname, string inputcode)
        {
            this.DEPT_CODE = deptcode;
            this.DEPT_NAME = deptname;
            this.INPUT_CODE = inputcode;
        }

        public StaffDept() { }

        public string DEPT_CODE { get; set; }

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
        public static Paging<StaffDept> PlantsPaging(int start, int limit, string sort, string dir, string filter, string deptfilter)
        {
            List<StaffDept> deptlist = new List<StaffDept>();
            
            Goldnet.Dal.GuideDalDict dal = new Goldnet.Dal.GuideDalDict();

            DataTable table = dal.GetStaffDept(filter,deptfilter).Tables[0];

            for (int i = 0; i < table.Rows.Count; i++)
            {
                StaffDept dept = new StaffDept();

                dept.DEPT_CODE = table.Rows[i]["dept_code"].ToString();
                dept.DEPT_NAME = table.Rows[i]["dept_name"].ToString();
                dept.INPUT_CODE = table.Rows[i]["input_code"].ToString();
                deptlist.Add(dept);
            }
            if (!string.IsNullOrEmpty(sort))
            {
                deptlist.Sort(delegate(StaffDept x, StaffDept y)
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

            List<StaffDept> rangePlants = (start < 0 || limit < 0) ? deptlist : deptlist.GetRange(start, limit);

            return new Paging<StaffDept>(rangePlants, deptlist.Count);
        }
    }
}
