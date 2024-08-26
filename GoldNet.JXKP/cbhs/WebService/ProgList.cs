using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Goldnet.Ext.Web;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;

namespace GoldNet.JXKP.cbhs.WebService
{
    public class ProgList
    {
 public ProgList(string progcode, string progname)
        {
            this.PROG_CODE = progcode;
            this.PROG_NAME = progname;
        }

 public ProgList()
        {

        }

 public string PROG_CODE { get; set; }

         public string PROG_NAME { get; set; }

        /// <summary>
        /// 查询核算项目
        /// </summary>
        /// <param name="start"></param>
        /// <param name="limit"></param>
        /// <param name="sort"></param>
        /// <param name="dir"></param>
        /// <param name="filter"></param>
        /// <param name="itemfilter"></param>
        /// <returns></returns>
         public static Paging<ProgList> PlantsPaging(int start, int limit, string sort, string dir, string filter, string itemfilter,string flags)
        {
            List<ProgList> itemlist = new List<ProgList>();
            Goldnet.Dal.Cbhs_dict dal = new Goldnet.Dal.Cbhs_dict();
            string filters = "";
            if (!string.IsNullOrEmpty(filter) && filter.Trim() != "*")
            {
                filters = filter.Trim().ToUpper();
            }
            DataTable table = dal.GetProgList(filters,flags).Tables[0];
            for (int i = 0; i < table.Rows.Count; i++)
            {
                ProgList items = new ProgList();
                items.PROG_CODE = table.Rows[i]["PROG_CODE"].ToString();
                items.PROG_NAME = table.Rows[i]["PROG_NAME"].ToString();
                itemlist.Add(items);
            }
            if (!string.IsNullOrEmpty(sort))
            {
                itemlist.Sort(delegate(ProgList x, ProgList y)
               { 
                    object a;
                    object b;

                    int direction = dir == "DESC" ? -1 : 1;

                    a = x.GetType().GetProperty(sort).GetValue(x, null);
                    b = y.GetType().GetProperty(sort).GetValue(y, null);

                    return CaseInsensitiveComparer.Default.Compare(a, b) * direction;
                });
            }

            if ((start + limit) > itemlist.Count)
            {
                limit = itemlist.Count - start;
            }

            List<ProgList> rangePlants = (start < 0 || limit < 0) ? itemlist : itemlist.GetRange(start, limit);

            return new Paging<ProgList>(rangePlants, itemlist.Count);
        }
    }
}
