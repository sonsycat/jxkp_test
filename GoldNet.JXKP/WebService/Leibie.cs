using System.Data;
using Goldnet.Ext.Web;
using System.Collections;
using System.Collections.Generic;

namespace GoldNet.JXKP.WebService
{
    /// <summary>
    /// 科室
    /// </summary>
    public class Leibie
    {
        public Leibie(string itemcode, string itemname,string inputcode)
        {
            this.ITEM_CODE = itemcode;
            this.ITEM_NAME = itemname;
            this.INPUT_CODE = inputcode;
        }

        public Leibie()
        {
        }

        public string ITEM_CODE { get; set; }

        public string ITEM_NAME { get; set; }

        public string INPUT_CODE { get; set; }
        /// <summary>
        /// 查询类别
        /// </summary>
        /// <param name="start"></param>
        /// <param name="limit"></param>
        /// <param name="sort"></param>
        /// <param name="dir"></param>
        /// <param name="filter"></param>
        /// <param name="deptfilter"></param>
        /// <returns></returns>
        public static Paging<Leibie> PlantsPaging_Item(int start, int limit, string sort, string dir, string filter, string deptfilter)
        {
            List<Leibie> itemlist = new List<Leibie>();
            Goldnet.Dal.SYS_DEPT_DICT dal = new Goldnet.Dal.SYS_DEPT_DICT();
            string filters = "";
            if (!string.IsNullOrEmpty(filter) && filter.Trim() != "*")
            {
                filters = filter.Trim().ToUpper();
            }
            DataTable table = dal.GetAllLeibie(filters, deptfilter, "").Tables[0];
            for (int i = 0; i < table.Rows.Count; i++)
            {
                Leibie leibie = new Leibie();
                leibie.ITEM_CODE = table.Rows[i]["item_code"].ToString();
                leibie.ITEM_NAME = table.Rows[i]["item_name"].ToString();
                leibie.INPUT_CODE = table.Rows[i]["input_code"].ToString();

                // if(!deptlist.Find(dept))
                itemlist.Add(leibie);
            }
            if (!string.IsNullOrEmpty(sort))
            {
                itemlist.Sort(delegate(Leibie x, Leibie y)
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

            List<Leibie> rangePlants = (start < 0 || limit < 0) ? itemlist : itemlist.GetRange(start, limit);

            return new Paging<Leibie>(rangePlants, itemlist.Count);
        }
    }
}
