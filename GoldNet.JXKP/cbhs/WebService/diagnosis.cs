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
    public class diagnosis
    {
        public diagnosis(string diagnosis_code, string diagnosis_name)
        {
            this.DIAGNOSIS_CODE = diagnosis_code;
            this.DIAGNOSIS_NAME = diagnosis_name;
        }

        public diagnosis()
        {
        }

        public string DIAGNOSIS_CODE { get; set; }

        public string DIAGNOSIS_NAME { get; set; }
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
        public static Paging<diagnosis> PlantsPaging(int start, int limit, string sort, string dir, string filter, string itemfilter)
        {
            List<diagnosis> itemlist = new List<diagnosis>();
            Goldnet.Dal.Cbhs_dict dal = new Goldnet.Dal.Cbhs_dict();
            string filters = "";
            if (!string.IsNullOrEmpty(filter) && filter.Trim() != "*")
            {
                filters = filter.Trim().ToUpper();
            }
            DataTable table = dal.Getdiagnosis(filters).Tables[0];
            for (int i = 0; i < table.Rows.Count; i++)
            {
                diagnosis items = new diagnosis();
                items.DIAGNOSIS_CODE = table.Rows[i]["DIAGNOSIS_CODE"].ToString();
                items.DIAGNOSIS_NAME = table.Rows[i]["DIAGNOSIS_NAME"].ToString();
                itemlist.Add(items);
            }
            if (!string.IsNullOrEmpty(sort))
            {
                itemlist.Sort(delegate(diagnosis x, diagnosis y)
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

            List<diagnosis> rangePlants = (start < 0 || limit < 0) ? itemlist : itemlist.GetRange(start, limit);

            return new Paging<diagnosis>(rangePlants, itemlist.Count);
        }
    }
}
