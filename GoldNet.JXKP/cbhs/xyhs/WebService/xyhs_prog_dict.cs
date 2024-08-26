using System.Data;
using Goldnet.Ext.Web;
using System.Collections;
using System.Collections.Generic;

namespace GoldNet.JXKP.cbhs.xyhs.WebService
{
    public class xyhs_prog_dict
    {
public xyhs_prog_dict(string progcode, string progname, string inputcode)
        {
            this.PROG_CODE = progcode;
            this.PROG_NAME = progname;
            this.INPUT_CODE = inputcode;
        }

public xyhs_prog_dict()
        {
        }

 public string PROG_CODE { get; set; }

 public string PROG_NAME { get; set; }

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
 public static Paging<xyhs_prog_dict> PlantsPaging(int start, int limit, string sort, string dir, string filter, string costsfilter)
        {
            List<xyhs_prog_dict> deptlist = new List<xyhs_prog_dict>();
            Goldnet.Dal.XyhsOperation dal = new Goldnet.Dal.XyhsOperation();
            string filters = "";
            if (!string.IsNullOrEmpty(filter) && filter.Trim() != "*")
            {
                filters = filter.Trim().ToUpper();
            }
            DataTable table = dal.GetAllProg(filters, costsfilter).Tables[0];
            for (int i = 0; i < table.Rows.Count; i++)
            {
                xyhs_prog_dict dept = new xyhs_prog_dict();
                dept.PROG_CODE = table.Rows[i]["prog_code"].ToString();
                dept.PROG_NAME = table.Rows[i]["prog_name"].ToString();
                dept.INPUT_CODE = table.Rows[i]["input_code"].ToString();
                
               // if(!deptlist.Find(dept))
                    deptlist.Add(dept);
            }
            if (!string.IsNullOrEmpty(sort))
            {
                deptlist.Sort(delegate(xyhs_prog_dict x, xyhs_prog_dict y)
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

            List<xyhs_prog_dict> rangePlants = (start < 0 || limit < 0) ? deptlist : deptlist.GetRange(start, limit);

            return new Paging<xyhs_prog_dict>(rangePlants, deptlist.Count);
        }
    }
}
