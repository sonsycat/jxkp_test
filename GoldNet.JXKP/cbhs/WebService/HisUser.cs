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
    public class HisUser
    {
         public HisUser(string staff_id, string staff_name)
        {
            this.STAFF_ID = staff_id;
            this.STAFF_NAME = staff_name;
        }

        public HisUser()
        {

        }

         public string STAFF_ID { get; set; }

         public string STAFF_NAME { get; set; }

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
         public static Paging<HisUser> PlantsPaging(int start, int limit, string sort, string dir, string filter, string itemfilter)
        {
            List<HisUser> itemlist = new List<HisUser>();
            Goldnet.Dal.Cbhs_dict dal = new Goldnet.Dal.Cbhs_dict();
            string filters = "";
            if (!string.IsNullOrEmpty(filter) && filter.Trim() != "*")
            {
                filters = filter.Trim().ToUpper();
            }
            DataTable table = dal.GetStaffInfo(filters);
            for (int i = 0; i < table.Rows.Count; i++)
            {
                HisUser items = new HisUser();
                items.STAFF_ID = table.Rows[i]["STAFF_ID"].ToString();
                items.STAFF_NAME = table.Rows[i]["NAME"].ToString();
                itemlist.Add(items);
            }
            if (!string.IsNullOrEmpty(sort))
            {
                itemlist.Sort(delegate(HisUser x, HisUser y)
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

            List<HisUser> rangePlants = (start < 0 || limit < 0) ? itemlist : itemlist.GetRange(start, limit);

            return new Paging<HisUser>(rangePlants, itemlist.Count);
        }
    }
}
