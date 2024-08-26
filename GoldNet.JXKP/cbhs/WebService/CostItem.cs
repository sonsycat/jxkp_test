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

namespace GoldNet.JXKP.WebPage.Cbhs.WebService
{
    /// <summary>
    /// 核算项目
    /// </summary>
    public class CostItem
    {
        public CostItem(string item_code, string item_name, string input_code)
        {
            this.ITEM_CODE = item_code;
            this.ITEM_NAME = item_name;
            this.INPUT_CODE = input_code;
        }

        public CostItem()
        {
        }

         public string ITEM_CODE { get; set; }

         public string ITEM_NAME { get; set; }

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
         public static Paging<CostItem> PlantsPaging(int start, int limit, string sort, string dir, string filter, string itemfilter)
        {
            List<CostItem> itemlist = new List<CostItem>();
            Goldnet.Dal.Cbhs_dict dal = new Goldnet.Dal.Cbhs_dict();
            string filters = "";
            if (!string.IsNullOrEmpty(filter) && filter.Trim() != "*")
            {
                filters = filter.Trim().ToUpper();
            }
            DataTable table = dal.GetCbhs_Items(filters).Tables[0];
            for (int i = 0; i < table.Rows.Count; i++)
            {
                CostItem items = new CostItem();
                items.ITEM_CODE = table.Rows[i]["item_code"].ToString();
                items.ITEM_NAME = table.Rows[i]["item_name"].ToString();
                items.INPUT_CODE = table.Rows[i]["input_code"].ToString();
                itemlist.Add(items);
            }
            if (!string.IsNullOrEmpty(sort))
            {
                itemlist.Sort(delegate(CostItem x, CostItem y)
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

            List<CostItem> rangePlants = (start < 0 || limit < 0) ? itemlist : itemlist.GetRange(start, limit);

            return new Paging<CostItem>(rangePlants, itemlist.Count);
        }
    }
}
