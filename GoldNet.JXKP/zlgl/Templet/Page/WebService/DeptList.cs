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
    /// <summary>
    /// 核算科室
    /// </summary>
    public class DeptList
    {
         public DeptList(string deptcode, string deptname, string inputcode)
        {
            this.DEPT_CODE = deptcode;
            this.DEPT_NAME = deptname;
            this.INPUT_CODE = inputcode;
        }

         public DeptList()
        {
        }

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
        public static Paging<DeptList> PlantsPaging(int start, int limit, string sort, string dir, string filter, string deptfilter,string deptcode)
        {
            List<DeptList> deptlist = new List<DeptList>();
            Goldnet.Dal.SYS_DEPT_DICT dal = new Goldnet.Dal.SYS_DEPT_DICT();
            string filters = "";
            if (!string.IsNullOrEmpty(filter) && filter.Trim() != "*")
            {
                filters = filter.Trim().ToUpper();
            }
            DataTable table = dal.GetAllDept(filters, deptfilter, deptcode).Tables[0];
            for (int i = 0; i < table.Rows.Count; i++)
            {
                DeptList dept = new DeptList();
                dept.DEPT_CODE = table.Rows[i]["dept_code"].ToString();
                dept.DEPT_NAME = table.Rows[i]["dept_name"].ToString();
                dept.INPUT_CODE = table.Rows[i]["input_code"].ToString();
                
               // if(!deptlist.Find(dept))
                    deptlist.Add(dept);
            }
            if (!string.IsNullOrEmpty(sort))
            {
                deptlist.Sort(delegate(DeptList x, DeptList y)
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

            List<DeptList> rangePlants = (start < 0 || limit < 0) ? deptlist : deptlist.GetRange(start, limit);

            return new Paging<DeptList>(rangePlants, deptlist.Count);
        }
    }
}
