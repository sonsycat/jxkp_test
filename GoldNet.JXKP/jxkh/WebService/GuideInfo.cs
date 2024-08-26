using System;
using System.Data;
using System.Configuration;
using Goldnet.Ext.Web;
using System.Collections;
using System.Collections.Generic;

namespace GoldNet.JXKP.jxkh.WebService
{
    public class GuideInfo
    {

        public GuideInfo(string guidecode, string guidename, string inputcode)
        {
            this.GUIDE_CODE = guidecode;
            this.GUIDE_NAME = guidename;
            this.INPUT_CODE = inputcode;
        }

        public GuideInfo()
        {
        }
        public string GUIDE_CODE { get; set; }

        public string GUIDE_NAME { get; set; }

        public string INPUT_CODE { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="start"></param>
        /// <param name="limit"></param>
        /// <param name="sort"></param>
        /// <param name="dir"></param>
        /// <param name="filter"></param>
        /// <param name="deptfilter"></param>
        /// <returns></returns>
        public static Paging<GuideInfo> PlantsPaging(int start, int limit, string sort, string dir, string filter, string deptfilter)
        {
            List<GuideInfo> guidelist = new List<GuideInfo>();

            Goldnet.Dal.BaseInfoMaintainDal dal = new Goldnet.Dal.BaseInfoMaintainDal();

            DataTable table = dal.GetGuideInfo(filter, deptfilter).Tables[0];

            for (int i = 0; i < table.Rows.Count; i++)
            {
                GuideInfo guide = new GuideInfo();

                guide.GUIDE_CODE = table.Rows[i]["guide_code"].ToString();
                guide.GUIDE_NAME = table.Rows[i]["guide_name"].ToString();
                guide.INPUT_CODE = table.Rows[i]["guide_code"].ToString();
                guidelist.Add(guide);
            }

            if (!string.IsNullOrEmpty(sort))
            {
                guidelist.Sort(delegate(GuideInfo x, GuideInfo y)
                {
                    object a;
                    object b;

                    int direction = dir == "DESC" ? -1 : 1;

                    a = x.GetType().GetProperty(sort).GetValue(x, null);
                    b = y.GetType().GetProperty(sort).GetValue(y, null);

                    return CaseInsensitiveComparer.Default.Compare(a, b) * direction;
                });
            }

            if ((start + limit) > guidelist.Count)
            {
                limit = guidelist.Count - start;
            }

            List<GuideInfo> rangePlants = (start < 0 || limit < 0) ? guidelist : guidelist.GetRange(start, limit);

            return new Paging<GuideInfo>(rangePlants, guidelist.Count);
        }
    }


}
