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
    public class HisDept
    {
        public HisDept(string deptcode, string deptname, string inputcode)
        {
            this.DEPT_CODE = deptcode;
            this.DEPT_NAME = deptname;
            this.INPUT_CODE = inputcode;
        }

        public HisDept()
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
        public static Paging<HisDept> PlantsPaging(int start, int limit, string sort, string dir, string filter, string deptfilter)
        {
            List<HisDept> hisdepts = new List<HisDept>();
            Goldnet.Dal.SYS_ROLE_DICT dal = new Goldnet.Dal.SYS_ROLE_DICT();
            string filters = "";
            if (!string.IsNullOrEmpty(filter) && filter.Trim() != "*")
            {
                filters = filter.Trim().ToUpper();
            }
            DataTable table = dal.GetHisDept(filters, deptfilter).Tables[0];
            for (int i = 0; i < table.Rows.Count; i++)
            {
                HisDept hisdept = new HisDept();
                hisdept.DEPT_CODE = table.Rows[i]["dept_code"].ToString();
                hisdept.DEPT_NAME = table.Rows[i]["dept_name"].ToString();
                hisdept.INPUT_CODE = table.Rows[i]["input_code"].ToString();
                //if (hisdepts.Contains(hisdept))
                hisdepts.Add(hisdept);
            }
            if (!string.IsNullOrEmpty(sort))
            {
                hisdepts.Sort(delegate(HisDept x, HisDept y)
                {
                    object a;
                    object b;

                    int direction = dir == "DESC" ? -1 : 1;

                    a = x.GetType().GetProperty(sort).GetValue(x, null);
                    b = y.GetType().GetProperty(sort).GetValue(y, null);

                    return CaseInsensitiveComparer.Default.Compare(a, b) * direction;
                });
            }

            if ((start + limit) > hisdepts.Count)
            {
                limit = hisdepts.Count - start;
            }

            List<HisDept> rangePlants = (start < 0 || limit < 0) ? hisdepts : hisdepts.GetRange(start, limit);

            return new Paging<HisDept>(rangePlants, hisdepts.Count);
        }


    }
}
