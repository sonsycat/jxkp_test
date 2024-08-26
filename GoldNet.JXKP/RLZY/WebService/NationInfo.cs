using System;
using System.Data;
using System.Configuration;
using System.Collections;
using Goldnet.Ext.Web;
using System.Collections.Generic;

namespace GoldNet.JXKP.RLZY.WebService
{
    public class NationInfo
    {
        public NationInfo(string nationCode, string nationName,string inputcode)
        {
            this.NATION_CODE = nationCode;
            this.NATION_NAME = nationName;
            this.INPUT_CODE = inputcode;
        }

        public NationInfo() { }

        public string NATION_CODE { get; set; }

        public string NATION_NAME { get; set; }

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
        public static Paging<NationInfo> PlantsPaging(int start, int limit, string sort, string dir, string filter, string deptfilter)
        {
            List<NationInfo> nationlist = new List<NationInfo>();

            Goldnet.Dal.BaseInfoMaintainDal dal = new Goldnet.Dal.BaseInfoMaintainDal();

            DataTable table = dal.GetNationInfo(filter).Tables[0];

            for (int i = 0; i < table.Rows.Count; i++)
            {
                NationInfo nation = new NationInfo();

                nation.NATION_CODE = table.Rows[i]["SERIAL_NO"].ToString();
                nation.NATION_NAME = table.Rows[i]["NATION_NAME"].ToString();
                nationlist.Add(nation);
            }
            if (!string.IsNullOrEmpty(sort))
            {
                nationlist.Sort(delegate(NationInfo x, NationInfo y)
                {
                    object a;
                    object b;

                    int direction = dir == "DESC" ? -1 : 1;

                    a = x.GetType().GetProperty(sort).GetValue(x, null);
                    b = y.GetType().GetProperty(sort).GetValue(y, null);

                    return CaseInsensitiveComparer.Default.Compare(a, b) * direction;
                });
            }

            if ((start + limit) > nationlist.Count)
            {
                limit = nationlist.Count - start;
            }

            List<NationInfo> rangePlants = (start < 0 || limit < 0) ? nationlist : nationlist.GetRange(start, limit);

            return new Paging<NationInfo>(rangePlants, nationlist.Count);
        }
    }
}
