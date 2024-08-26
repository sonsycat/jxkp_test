using System;
using System.Data;
using System.Configuration;
using System.Linq;
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
namespace GoldNet.JXKP.WebPage.SysManager.WebService
{
    public class HisStaff
    {
 public HisStaff(string empno, string name)
        {
            this.EMP_NO = empno;
            this.NAME = name;
        }

 public HisStaff()
        {
        }

        public string EMP_NO { get; set; }

        public string NAME { get; set; }

        /// <summary>
        /// 查询his人员
        /// </summary>
        /// <param name="start"></param>
        /// <param name="limit"></param>
        /// <param name="sort"></param>
        /// <param name="dir"></param>
        /// <param name="filter"></param>
        /// <param name="deptfilter"></param>
        /// <returns></returns>
        public static Paging<HisStaff> PlantsPaging(int start, int limit, string sort, string dir, string filter,string deptfilter)
        {
            List<HisStaff> hisusers = new List<HisStaff>();
            Goldnet.Dal.SYS_ROLE_DICT dal = new Goldnet.Dal.SYS_ROLE_DICT();
            string filters = "";
            if (!string.IsNullOrEmpty(filter) && filter != "*")
            {
                filters = filter.Trim().ToUpper();
            }
            DataTable table = dal.GetHisStaff(filters.Trim(), deptfilter).Tables[0];
            
            for (int i = 0; i < table.Rows.Count; i++)
            {
                HisStaff user = new HisStaff();
                user.EMP_NO = table.Rows[i]["emp_no"].ToString();
                user.NAME = table.Rows[i]["name"].ToString();
                hisusers.Add(user);
            }
            if (!string.IsNullOrEmpty(sort))
            {
                hisusers.Sort(delegate(HisStaff x, HisStaff y)
                {
                    object a;
                    object b;

                    int direction = dir == "DESC" ? -1 : 1;

                    a = x.GetType().GetProperty(sort).GetValue(x, null);
                    b = y.GetType().GetProperty(sort).GetValue(y, null);

                    return CaseInsensitiveComparer.Default.Compare(a, b) * direction;
                });
            }

            if ((start + limit) > hisusers.Count)
            {
                limit = hisusers.Count - start;
            }

            List<HisStaff> rangeusers = (start < 0 || limit < 0) ? hisusers : hisusers.GetRange(start, limit);

            return new Paging<HisStaff>(rangeusers, hisusers.Count);
        }

        public static List<HisStaff> TestData
        {
            get
            {
                Goldnet.Dal.SYS_ROLE_DICT dal = new Goldnet.Dal.SYS_ROLE_DICT();
                DataTable table = dal.GetHisUsers().Tables[0];
                List<HisStaff> data = new List<HisStaff>();
                for (int i = 0; i < table.Rows.Count; i++)
                {
                    HisStaff plant = new HisStaff();
                    plant.EMP_NO = table.Rows[i]["emp_no"].ToString();
                    plant.NAME = table.Rows[i]["name"].ToString();
                    
                    data.Add(plant);
                }
                return data;
            }
        }
    }
}