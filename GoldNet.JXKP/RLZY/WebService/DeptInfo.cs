using System.Collections;
using System.Collections.Generic;
using System.Data;
using Goldnet.Ext.Web;

namespace GoldNet.JXKP.RLZY.WebService
{
    public class DeptInfo
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="deptcode"></param>
        /// <param name="deptname"></param>
        /// <param name="inputcode"></param>
        public DeptInfo(string deptcode, string deptname, string inputcode)
        {
            this.DEPT_CODE = deptcode;
            this.DEPT_NAME = deptname;
            this.INPUT_CODE = inputcode;
        }

        public DeptInfo() { }

        public string DEPT_CODE { get; set; }

        public string DEPT_NAME { get; set; }

        public string INPUT_CODE { get; set; }

        /// <summary>
        /// 查询全部科室
        /// </summary>
        /// <param name="start"></param>
        /// <param name="limit"></param>
        /// <param name="sort"></param>
        /// <param name="dir"></param>
        /// <param name="filter"></param>
        /// <param name="deptfilter"></param>
        /// <returns></returns>
        public static Paging<DeptInfo> PlantsPaging(int start, int limit, string sort, string dir, string filter, string deptfilter)
        {
            List<DeptInfo> deptlist = new List<DeptInfo>();

            Goldnet.Dal.BaseInfoMaintainDal dal = new Goldnet.Dal.BaseInfoMaintainDal();

            DataTable table = dal.GetDeptInfo(filter, deptfilter).Tables[0];

            for (int i = 0; i < table.Rows.Count; i++)
            {
                DeptInfo dept = new DeptInfo();

                dept.DEPT_CODE = table.Rows[i]["dept_code"].ToString();
                dept.DEPT_NAME = table.Rows[i]["dept_name"].ToString();
                dept.INPUT_CODE = table.Rows[i]["input_code"].ToString();
                deptlist.Add(dept);
            }
            if (!string.IsNullOrEmpty(sort))
            {
                deptlist.Sort(delegate(DeptInfo x, DeptInfo y)
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

            List<DeptInfo> rangePlants = (start < 0 || limit < 0) ? deptlist : deptlist.GetRange(start, limit);

            return new Paging<DeptInfo>(rangePlants, deptlist.Count);
        }

        /// <summary>
        /// 获取核算科室
        /// </summary>
        /// <param name="start"></param>
        /// <param name="limit"></param>
        /// <param name="sort"></param>
        /// <param name="dir"></param>
        /// <param name="filter"></param>
        /// <param name="deptfilter"></param>
        /// <returns></returns>
        public static Paging<DeptInfo> PlantsPaging2(int start, int limit, string sort, string dir, string filter, string deptfilter)
        {
            List<DeptInfo> deptlist = new List<DeptInfo>();

            Goldnet.Dal.BaseInfoMaintainDal dal = new Goldnet.Dal.BaseInfoMaintainDal();

            DataTable table = dal.GetDeptAccont(filter, deptfilter).Tables[0];

            for (int i = 0; i < table.Rows.Count; i++)
            {
                DeptInfo dept = new DeptInfo();

                dept.DEPT_CODE = table.Rows[i]["dept_code"].ToString();
                dept.DEPT_NAME = table.Rows[i]["dept_name"].ToString();
                dept.INPUT_CODE = table.Rows[i]["input_code"].ToString();
                deptlist.Add(dept);
            }
            if (!string.IsNullOrEmpty(sort))
            {
                deptlist.Sort(delegate(DeptInfo x, DeptInfo y)
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

            List<DeptInfo> rangePlants = (start < 0 || limit < 0) ? deptlist : deptlist.GetRange(start, limit);

            return new Paging<DeptInfo>(rangePlants, deptlist.Count);
        }

        /// <summary>
        /// 获取全部科室字典信息
        /// </summary>
        /// <param name="start"></param>
        /// <param name="limit"></param>
        /// <param name="sort"></param>
        /// <param name="dir"></param>
        /// <param name="filter"></param>
        /// <param name="deptfilter"></param>
        /// <returns></returns>
        public static Paging<DeptInfo> GetDedptDict(int start, int limit, string sort, string dir, string filter, string deptfilter)
        {
            List<DeptInfo> deptlist = new List<DeptInfo>();

            Goldnet.Dal.BaseInfoMaintainDal dal = new Goldnet.Dal.BaseInfoMaintainDal();

            DataTable table = dal.GetDeptInfo(filter, deptfilter).Tables[0];

            for (int i = 0; i < table.Rows.Count; i++)
            {
                DeptInfo dept = new DeptInfo();

                dept.DEPT_CODE = table.Rows[i]["dept_code"].ToString();
                dept.DEPT_NAME = table.Rows[i]["dept_name"].ToString();
                dept.INPUT_CODE = table.Rows[i]["input_code"].ToString();
                deptlist.Add(dept);
            }
            if (!string.IsNullOrEmpty(sort))
            {
                deptlist.Sort(delegate(DeptInfo x, DeptInfo y)
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

            List<DeptInfo> rangePlants = (start < 0 || limit < 0) ? deptlist : deptlist.GetRange(start, limit);

            return new Paging<DeptInfo>(rangePlants, deptlist.Count);
        }
    }
}
