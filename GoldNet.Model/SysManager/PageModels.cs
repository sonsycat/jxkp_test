using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GoldNet.Model
{
    public class PageModels
    {
        //选中功能
        public class functionselected
        {
            public string FUNCTION_ID { get; set; }
            public string FUNCTION_NAME { get; set; }
            public string EDIT { get; set; }
            public string PASS { get; set; }
        }

        //选中人员
        public class userselected
        {
            public string USER_ID { get; set; }
        }
        public class userseaccount
        {
            public string ACCOUNT { get; set; }
        }
        //选中角色
        public class roleselected
        {
            public string ROLE_ID { get; set; }
        }

       //选中科室
        public class deptselected
        {
            public string DEPT_CODE { get; set; }
            public string DEPT_NAME { get; set; }
            public string RATIO { get;set;}
        }
        //选中的成本项目
        public class costselected
        {
            public string ITEM_CODE { get; set; }
            public string ITEM_NAME { get; set; }
        }
        //人力资源人员
        public class rlzylected
        {
            public string USER_NAME { get; set; }
            public string DEPT_NAME { get; set; }
            public string STAFF_ID { get; set; }
            public string BANK_CODE { get; set; }
        }
        //选中医生
        public class doctorselected
        {
            public string USER_ID { get; set; }
            public string USER_NAME { get; set; }
        }
        //月目标值
        public class stationmonthinfo
        {
            public string STATION_CODE { get; set; }
            public string GUIDE_CODE { get; set; }
            public string GUIDE_NAME { get; set; }
            public string STATION_YEAR { get; set; }

            public string MONTHS1 { get; set; }
            public string MONTHS2 { get; set; }
            public string MONTHS3 { get; set; }
            public string MONTHS4 { get; set; }
            public string MONTHS5 { get; set; }
            public string MONTHS6 { get; set; }
            public string MONTHS7 { get; set; }
            public string MONTHS8 { get; set; }
            public string MONTHS9 { get; set; }
            public string MONTHS10 { get; set; }
            public string MONTHS11 { get; set; }
            public string MONTHS12 { get; set; }
        }

        //选中工作量类别
        public class liebieselected
        {
            public string ITEM_CODE { get; set; }
            public string ITEM_NAME { get; set; }
            public string PRICE { get; set; }
            public string ITEM_SPEC { get; set; }
        }
    }
}
