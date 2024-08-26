using System.Data;
using Goldnet.Ext.Web;
using System.Collections;
using System.Collections.Generic;

namespace GoldNet.JXKP.WebService
{
    /// <summary>
    /// 科室
    /// </summary>
    public class Dept
    {
         public Dept(string deptcode, string deptname, string inputcode)
        {
            this.DEPT_CODE = deptcode;
            this.DEPT_NAME = deptname;
            this.INPUT_CODE = inputcode;
        }

        public Dept()
        {
        }

        public string DEPT_CODE { get; set; }

        public string DEPT_NAME { get; set; }

        public string INPUT_CODE { get; set; }
        /// <summary>
        /// 查询科室
        /// </summary>
        /// <param name="start"></param>
        /// <param name="limit"></param>
        /// <param name="sort"></param>
        /// <param name="dir"></param>
        /// <param name="filter"></param>
        /// <param name="deptfilter"></param>
        /// <returns></returns>
        public static Paging<Dept> PlantsPaging(int start, int limit, string sort, string dir, string filter, string deptfilter)
        {
            List<Dept> deptlist = new List<Dept>();
            Goldnet.Dal.SYS_DEPT_DICT dal = new Goldnet.Dal.SYS_DEPT_DICT();
            string filters = "";
            if (!string.IsNullOrEmpty(filter) && filter.Trim() != "*")
            {
                filters = filter.Trim().ToUpper();
            }
            DataTable table = dal.GetAllDept(filters, deptfilter, "").Tables[0];
            for (int i = 0; i < table.Rows.Count; i++)
            {
                Dept dept = new Dept();
                dept.DEPT_CODE = table.Rows[i]["dept_code"].ToString();
                dept.DEPT_NAME = table.Rows[i]["dept_name"].ToString();
                dept.INPUT_CODE = table.Rows[i]["input_code"].ToString();
                
               // if(!deptlist.Find(dept))
                    deptlist.Add(dept);
            }
            if (!string.IsNullOrEmpty(sort))
            {
                deptlist.Sort(delegate(Dept x, Dept y)
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

            List<Dept> rangePlants = (start < 0 || limit < 0) ? deptlist : deptlist.GetRange(start, limit);

            return new Paging<Dept>(rangePlants, deptlist.Count);
        }
    }
}
