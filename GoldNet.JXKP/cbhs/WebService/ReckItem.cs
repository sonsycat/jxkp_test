using System;
using System.Data;
using System.Configuration;
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

namespace GoldNet.JXKP.cbhs.WebService
{
    /// <summary>
    /// 核算项目
    /// </summary>
    public class ReckItem
    {
         public ReckItem(string class_code, string class_name, string input_code)
        {
            this.CLASS_CODE = class_code;
            this.CLASS_NAME = class_name;
            this.INPUT_CODE = input_code;
        }

         public ReckItem()
        {
        }

         public string CLASS_CODE { get; set; }

         public string CLASS_NAME { get; set; }

         public string INPUT_CODE { get; set; }
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
         public static Paging<ReckItem> PlantsPaging(int start, int limit, string sort, string dir, string filter, string itemfilter)
        {
            List<ReckItem> itemlist = new List<ReckItem>();
            Goldnet.Dal.Cbhs_dict dal = new Goldnet.Dal.Cbhs_dict();
            string filters = "";
            if (!string.IsNullOrEmpty(filter) && filter.Trim() != "*")
            {
                filters = filter.Trim().ToUpper();
            }
            DataTable table = dal.GetReck_Items(filters).Tables[0];
            for (int i = 0; i < table.Rows.Count; i++)
            {
                ReckItem items = new ReckItem();
                items.CLASS_CODE = table.Rows[i]["class_code"].ToString();
                items.CLASS_NAME = table.Rows[i]["class_name"].ToString();
                items.INPUT_CODE = table.Rows[i]["input_code"].ToString();
                itemlist.Add(items);
            }
            if (!string.IsNullOrEmpty(sort))
            {
                itemlist.Sort(delegate(ReckItem x, ReckItem y)
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

            List<ReckItem> rangePlants = (start < 0 || limit < 0) ? itemlist : itemlist.GetRange(start, limit);

            return new Paging<ReckItem>(rangePlants, itemlist.Count);
        }
    }
}
