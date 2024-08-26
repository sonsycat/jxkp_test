using System;
using System.Data;
using System.Configuration;
using Goldnet.Ext.Web;
using System.Collections;
using System.Collections.Generic;

namespace GoldNet.JXKP.GuideLook.WebService
{
    public class DeptInfos
    {
        public DeptInfos(string deptcode, string deptname, string inputcode)
        {
            this.DEPT_CODE = deptcode;
            this.DEPT_NAME = deptname;
            this.INPUT_CODE = inputcode;
        }

        public DeptInfos() { }

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
        public static Paging<DeptInfos> PlantsPaging(int start, int limit, string sort, string dir, string filter, string deptfilter)
        {
            List<DeptInfos> deptlist = new List<DeptInfos>();

            Goldnet.Dal.StatementDal dal = new Goldnet.Dal.StatementDal();

            DataTable table = dal.GetDeptInfo(filter, deptfilter).Tables[0];

            for (int i = 0; i < table.Rows.Count; i++)
            {
                DeptInfos dept = new DeptInfos();

                dept.DEPT_CODE = table.Rows[i]["dept_code"].ToString();
                dept.DEPT_NAME = table.Rows[i]["dept_name"].ToString();
                dept.INPUT_CODE = table.Rows[i]["input_code"].ToString();
                deptlist.Add(dept);
            }
            if (!string.IsNullOrEmpty(sort))
            {
                deptlist.Sort(delegate(DeptInfos x, DeptInfos y)
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

            List<DeptInfos> rangePlants = (start < 0 || limit < 0) ? deptlist : deptlist.GetRange(start, limit);

            return new Paging<DeptInfos>(rangePlants, deptlist.Count);
        }
    }
}
