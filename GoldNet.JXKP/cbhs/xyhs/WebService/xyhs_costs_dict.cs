using System.Data;
using Goldnet.Ext.Web;
using System.Collections;
using System.Collections.Generic;

namespace GoldNet.JXKP.cbhs.xyhs.WebService
{
    public class xyhs_costs_dict
    {
 public xyhs_costs_dict(string itemcode, string itemname, string inputcode)
        {
            this.ITEM_CODE = itemcode;
            this.ITEM_NAME = itemname;
            this.INPUT_CODE = inputcode;
        }

 public xyhs_costs_dict()
        {
        }

 public string ITEM_CODE { get; set; }

 public string ITEM_NAME { get; set; }

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
 public static Paging<xyhs_costs_dict> PlantsPaging(int start, int limit, string sort, string dir, string filter, string costsfilter)
        {
            List<xyhs_costs_dict> deptlist = new List<xyhs_costs_dict>();
            Goldnet.Dal.XyhsOperation dal = new Goldnet.Dal.XyhsOperation();
            string filters = "";
            if (!string.IsNullOrEmpty(filter) && filter.Trim() != "*")
            {
                filters = filter.Trim().ToUpper();
            }
            DataTable table = dal.GetAllCost(filters, costsfilter, "").Tables[0];
            for (int i = 0; i < table.Rows.Count; i++)
            {
                xyhs_costs_dict dept = new xyhs_costs_dict();
                dept.ITEM_CODE = table.Rows[i]["item_code"].ToString();
                dept.ITEM_NAME = table.Rows[i]["item_name"].ToString();
                dept.INPUT_CODE = table.Rows[i]["input_code"].ToString();
                
               // if(!deptlist.Find(dept))
                    deptlist.Add(dept);
            }
            if (!string.IsNullOrEmpty(sort))
            {
                deptlist.Sort(delegate(xyhs_costs_dict x, xyhs_costs_dict y)
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

            List<xyhs_costs_dict> rangePlants = (start < 0 || limit < 0) ? deptlist : deptlist.GetRange(start, limit);

            return new Paging<xyhs_costs_dict>(rangePlants, deptlist.Count);
        }
    }
}
