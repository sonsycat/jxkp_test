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


namespace GoldNet.JXKP.zlgl.Templet.Page.WebService
{
    public class PatientList
    {
        public PatientList(string patientid, string patientname, string deptname)
        {
            this.PATIENT_ID = patientid;
            this.PATIENT_NAME = patientname;
            this.DEPT_NAME = deptname;
        }
        public PatientList() { }
        public string PATIENT_ID { get; set; }
        public string PATIENT_NAME { get; set; }
        public string DEPT_NAME { get; set; }
        /// <summary>
        /// 查询在院病人
        /// </summary>
        /// <param name="start"></param>
        /// <param name="limit"></param>
        /// <param name="sort"></param>
        /// <param name="dir"></param>
        /// <param name="filter"></param>
        /// <param name="deptfilter"></param>
        /// <returns></returns>
        public static Paging<PatientList> PlantsPaging(int start, int limit, string sort, string dir, string filter, string patientid,string tablename,string fieldid)
        {
            List<PatientList> patients = new List<PatientList>();
            Goldnet.Dal.SYS_ROLE_DICT dal = new Goldnet.Dal.SYS_ROLE_DICT();
            string filters = "";
            if (!string.IsNullOrEmpty(filter) && filter != "*")
            {
                filters = filter.Trim().ToUpper();
            }
            DataTable table = dal.GetPatient(filters.Trim(),patientid,tablename,fieldid).Tables[0];

            for (int i = 0; i < table.Rows.Count; i++)
            {
                PatientList patient = new PatientList();
                patient.PATIENT_ID = table.Rows[i]["patient_id"].ToString();
                patient.PATIENT_NAME = table.Rows[i]["patient_name"].ToString();
                patient.DEPT_NAME = table.Rows[i]["dept_name"].ToString();
                patients.Add(patient);
            }
            if (!string.IsNullOrEmpty(sort))
            {
                patients.Sort(delegate(PatientList x, PatientList y)
                {
                    object a;
                    object b;

                    int direction = dir == "DESC" ? -1 : 1;

                    a = x.GetType().GetProperty(sort).GetValue(x, null);
                    b = y.GetType().GetProperty(sort).GetValue(y, null);

                    return CaseInsensitiveComparer.Default.Compare(a, b) * direction;
                });
            }

            if ((start + limit) > patients.Count)
            {
                limit = patients.Count - start;
            }

            List<PatientList> rangeusers = (start < 0 || limit < 0) ? patients : patients.GetRange(start, limit);

            return new Paging<PatientList>(rangeusers, patients.Count);
        }

    }
}