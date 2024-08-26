using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Goldnet.Dal
{
    public class StaffInfo
    {
        string _NATIONAL;//民族/
        string _BONUS_FACTOR;//奖金系数/
        string _GOVERNMENT_ALLOWANCE;//政府津贴/
        string _CADRES_CATEGORIES;//干部类别/
        string _STUDY_OVER_DATE;//毕业时间/
        string _DEPT_TYPE;//所在科室类/
        string _TOPEDUCATE;//文化程度/
        string _STUDY_SpecSort;//所学专业/

        string _DeptCode;//科室代码/
        string _DeptName;//科室名称/
        string _Name;//姓名/
        string _IsOnGuard;//是否在岗/
        string _Birthday;//生日/
        string _Sex;//性别/

        string _InHospitalDate;//来院时间/
        string _BasePay;//基本工资/
        string _RetainTerm;//受聘期限/
        string _ifArmy;//是否军人/

        string _Job;//技术职务/
        string _JobDate;//技术职务时间/
        string _StaffSort;//人员类别/
        string _BeEnrolledInDate;//入伍时间/
        string _WrokDate;//工作日期/
        string _Duty;//行政职务/
        string _DutyDate;//行政职务时间/
        string _TechnicClass;//技术级/
        string _TechnicClassDate;//技术级时间/
        string _CivilServiceClass;//文职级/
        string _CivilServiceClassDate;//文职级时间/
        string _SantSpecSort;//卫生专业分类/
        string _RootSpecSort;//从事专业
        string _MediCardMark;//医疗卡账号/
        string _MediCard;//医疗卡号/

        string _INPUT_USER;//提交人
        string _INPUT_DATE;//提交日期
        string _USER_DATE;//月份数据
        //string _deptgroup;//核算分组

        string _HOMEPLACE;//出生地点
        string _CERTIFICATE_NO;//证件号
        string _MARITAL_STATUS;//婚姻状况
        string _TITLE_LIST;//职称序列
        string _Edu1;//学位
        string _GRADUATE_ACADEMY;//毕业院校
        string _DATE_OF_GRADETITLE;//取得学历时间
        string _RANK;//军衔
        string _TITLE;//职称
        string _DEPTGROUP;//组
        string _MEMO;//备注
        string _MARK_USER;//审核人
        string _STAFF_INPUT;
        string _STAFF_ID;
        string _INPUT_CODE;//姓名首字母
        string _JOB_TITLE;
        string _TITLE_DATE;
        string _EXPERT;
        string _IMG_ID;

        string _Station_Code;//岗位编码
        string _Credithour_PerYear;

        string _Leadtecn;//学术任职

        string _Gord;
        string _emp_no;

        string _Contractstart;
        string _Contractend;

        string _Bonusflag;
        string _Checkflag;
        string _BonusNum;

        public string Emp_no
        {
            get { return _emp_no; }
            set { _emp_no = value; }
        }

        public StaffInfo(string emp_no, string national, string bonus_factor, string government_allowance, string cadres_categories, string study_over_date, string dept_type, string topeducate, string study_specsort, string deptCode, string deptName, string name, string isOnGuard, string birthday, string sex, string inHospitalDate, string basePay, string retainTerm, string ifArmy, string job, string jobDate, string staffSort, string beEnrolledInDate, string wrokDate, string duty, string dutyDate, string technicClass, string technicClassDate, string civilServiceClass, string civilServiceClassDate, string SantSpecSort, string RootSpecSort, string MediCardMark, string MediCard, string input_user, string input_date, string user_date, string homeplace, string certificateno, string maritalstatus, string titlelist, string edu1, string graduateacademy, string dateofgradetitle, string rank, string title, string deptgroup, string memo, string mark_user, string staff_input, string staff_id, string input_code, string job_title, string title_date, string expert, string img_id, string Credithour_PerYear, string Leadtecn, string station_Code, string gord, string contractstart, string contractend, string bonusflag, string checkflag, string bonusnum)
        {
            _emp_no = emp_no;
            _Credithour_PerYear = Credithour_PerYear;
            _JOB_TITLE = job_title;
            _TITLE_DATE = title_date;
            _EXPERT = expert;
            _IMG_ID = img_id;
            _INPUT_CODE = input_code;
            _STAFF_INPUT = staff_input;
            _STAFF_ID = staff_id;
            _DEPTGROUP = deptgroup;
            _MEMO = memo;
            _MARK_USER = mark_user;
            _TITLE = title;
            _NATIONAL = national;//
            _BONUS_FACTOR = bonus_factor;//
            _GOVERNMENT_ALLOWANCE = government_allowance;//
            _CADRES_CATEGORIES = cadres_categories;//
            _STUDY_OVER_DATE = study_over_date;//
            _DEPT_TYPE = dept_type;//
            _TOPEDUCATE = topeducate;//
            _STUDY_SpecSort = study_specsort;//

            _DeptCode = deptCode;
            _DeptName = deptName;
            _Name = name;
            _IsOnGuard = isOnGuard;
            _Birthday = birthday;
            _Sex = sex;

            _InHospitalDate = inHospitalDate;//来院时间
            _BasePay = basePay;//基本工资
            _RetainTerm = retainTerm;//受聘期限
            _ifArmy = ifArmy;//是否军人

            _Job = job;//技术职务
            _JobDate = jobDate;//职称时间
            _StaffSort = staffSort;//人员类别
            _BeEnrolledInDate = beEnrolledInDate;//入伍时间
            _WrokDate = wrokDate;//工作日期
            _Duty = duty;//行政职务
            _DutyDate = dutyDate;//职务时间
            _TechnicClass = technicClass;//技术级
            _TechnicClassDate = technicClassDate;//技术级时间
            _CivilServiceClass = civilServiceClass;//文职级
            _CivilServiceClassDate = civilServiceClassDate;//文职级时间
            _SantSpecSort = SantSpecSort;//卫生专业分类
            _RootSpecSort = RootSpecSort;//从事专业
            _MediCardMark = MediCardMark;//医疗卡账号
            _MediCard = MediCard;//医疗卡号

            _INPUT_USER = input_user;//提交人
            _INPUT_DATE = input_date;//提交日期
            _USER_DATE = user_date;//月份数据
            //_deptgroup=deptgroup;

            _HOMEPLACE = homeplace;
            _CERTIFICATE_NO = certificateno;
            _MARITAL_STATUS = maritalstatus;
            _TITLE_LIST = titlelist;
            _Edu1 = edu1;
            _GRADUATE_ACADEMY = graduateacademy;
            _DATE_OF_GRADETITLE = dateofgradetitle;
            _RANK = rank;

            _Leadtecn = Leadtecn;

            _Station_Code = station_Code;
            _Gord = gord;

            _Contractstart = contractstart;
            _Contractend = contractend;

            _Bonusflag = bonusflag;
            _Checkflag = checkflag;

            _BonusNum = bonusnum;
        }

        public string BONUSNUM
        {
            get { return _BonusNum; }
            set { _BonusNum = value; }
        }

        public string LEADTECN
        {
            get { return _Leadtecn; }
            set { _Leadtecn = value; }
        }
        public string CREDITHOUR_PERYEAR
        {
            get { return _Credithour_PerYear; }
            set { _Credithour_PerYear = value; }
        }
        public string JOB_TITLE
        {
            get { return _JOB_TITLE; }
            set { _JOB_TITLE = value; }
        }
        public string TITLE_DATE
        {
            get { return _TITLE_DATE; }
            set { _TITLE_DATE = value; }
        }
        public string EXPERT
        {
            get { return _EXPERT; }
            set { _EXPERT = value; }
        }
        public string IMG_ID
        {
            get { return _IMG_ID; }
            set { _IMG_ID = value; }
        }
        public string INPUT_CODE
        {
            get { return _INPUT_CODE; }
            set { _INPUT_CODE = value; }
        }
        public string STAFF_INPUT
        {
            get { return _STAFF_INPUT; }
            set { _STAFF_INPUT = value; }
        }
        public string STAFF_ID
        {
            get { return _STAFF_ID; }
            set { _STAFF_ID = value; }
        }


        public string DEPTGROUP
        {
            get { return _DEPTGROUP; }
            set { _DEPTGROUP = value; }
        }
        public string MEMO
        {
            get { return _MEMO; }
            set { _MEMO = value; }
        }
        public string MARK_USER
        {
            get { return _MARK_USER; }
            set { _MARK_USER = value; }
        }

        public string TITLE
        {
            get { return _TITLE; }
            set { _TITLE = value; }
        }
        public string RANK
        {
            get { return _RANK; }
            set { _RANK = value; }
        }
        public string DATE_OF_GRADETITLE
        {
            get { return _DATE_OF_GRADETITLE; }
            set { _DATE_OF_GRADETITLE = value; }
        }
        public string GRADUATE_ACADEMY
        {
            get { return _GRADUATE_ACADEMY; }
            set { _GRADUATE_ACADEMY = value; }
        }
        public string Edu1
        {
            get { return _Edu1; }
            set { _Edu1 = value; }
        }
        public string TITLE_LIST
        {
            get { return _TITLE_LIST; }
            set { _TITLE_LIST = value; }
        }
        public string MARITAL_STATUS
        {
            get { return _MARITAL_STATUS; }
            set { _MARITAL_STATUS = value; }
        }
        public string CERTIFICATE_NO
        {
            get { return _CERTIFICATE_NO; }
            set { _CERTIFICATE_NO = value; }
        }
        public string HOMEPLACE
        {
            get { return _HOMEPLACE; }
            set { _HOMEPLACE = value; }
        }

        public string INPUT_USER
        {
            get { return _INPUT_USER; }
            set { _INPUT_USER = value; }
        }
        public string INPUT_DATE
        {
            get { return _INPUT_DATE; }
            set { _INPUT_DATE = value; }
        }
        public string USER_DATE
        {
            get { return _USER_DATE; }
            set { _USER_DATE = value; }
        }

        //
        /// <summary>
        /// 所在科室的编号
        /// </summary>
        public string DeptCode
        {
            get
            { return _DeptCode; }
            set
            { _DeptCode = value; }
        }

        /// <summary>
        /// 所在科室名称
        /// </summary>
        public string DeptName
        {
            get
            { return _DeptName; }
            set
            { _DeptName = value; }
        }

        /// <summary>
        /// 名称
        /// </summary>
        public string Name
        {
            get
            { return _Name; }
            set
            { _Name = value; }
        }


        /// <summary>
        /// 是否在岗
        /// </summary>
        public string IsOnGuard
        {
            get
            { return _IsOnGuard; }
            set
            { _IsOnGuard = value; }
        }

        /// <summary>
        /// 性别
        /// </summary>
        public string Sex
        {
            get
            { return _Sex; }
            set
            { _Sex = value; }
        }

        /// <summary>
        /// 出生日期
        /// </summary>
        public string Birthday
        {
            get
            { return _Birthday; }
            set
            { _Birthday = value; }
        }

        /// <summary>
        /// 民族
        /// </summary>
        public string NATIONAL
        {
            get { return _NATIONAL; }
            set { _NATIONAL = value; }
        }

        /// <summary>
        /// 奖金系数
        /// </summary>
        public string BONUS_FACTOR
        {
            get { return _BONUS_FACTOR; }
            set { _BONUS_FACTOR = value; }
        }

        /// <summary>
        /// 政府津贴
        /// </summary>
        public string GOVERNMENT_ALLOWANCE
        {
            get { return _GOVERNMENT_ALLOWANCE; }
            set { _GOVERNMENT_ALLOWANCE = value; }
        }
        /// <summary>
        /// 干部类别
        /// </summary>
        public string CADRES_CATEGORIES
        {
            get { return _CADRES_CATEGORIES; }
            set { _CADRES_CATEGORIES = value; }
        }
        /// <summary>
        /// 毕业时间
        /// </summary>
        public string STUDY_OVER_DATE
        {
            get { return _STUDY_OVER_DATE; }
            set { _STUDY_OVER_DATE = value; }
        }
        /// <summary>
        /// 所在科室类
        /// </summary>
        public string DEPT_TYPE
        {
            get { return _DEPT_TYPE; }
            set { _DEPT_TYPE = value; }
        }
        /// <summary>
        /// 文化程度
        /// </summary>
        public string TOPEDUCATE
        {
            get { return _TOPEDUCATE; }
            set { _TOPEDUCATE = value; }
        }
        /// <summary>
        /// 所学专业
        /// </summary>
        public string STUDY_SpecSort
        {
            get { return _STUDY_SpecSort; }
            set { _STUDY_SpecSort = value; }
        }

        /// <summary>
        /// 来院时间
        /// </summary>
        public string InHospitalDate
        {
            get
            { return _InHospitalDate; }
            set
            { _InHospitalDate = value; }
        }

        /// <summary>
        /// 基本工资
        /// </summary>
        public string BasePay
        {
            get
            { return _BasePay; }
            set
            { _BasePay = value; }
        }

        /// <summary>
        ///受聘期限
        /// </summary>
        public string RetainTerm
        {
            get
            { return _RetainTerm; }
            set
            { _RetainTerm = value; }
        }
        /// <summary>
        /// 是否军人
        /// </summary>
        public string ifArmy
        {
            get
            { return _ifArmy; }
            set
            { _ifArmy = value; }
        }

        /// <summary>
        /// 技术职务
        /// </summary>
        public string Job
        {
            get
            { return _Job; }
            set
            { _Job = value; }
        }

        /// <summary>
        /// 职称时间
        /// </summary>
        public string JobDate
        {
            get
            { return _JobDate; }
            set
            { _JobDate = value; }
        }


        /// <summary>
        /// 人员类别
        /// </summary>
        public string StaffSort
        {
            get
            { return _StaffSort; }
            set
            { _StaffSort = value; }
        }



        /// <summary>
        /// 卫生专业分类
        /// </summary>
        public string SantSpecSort
        {
            get
            { return _SantSpecSort; }
            set
            { _SantSpecSort = value; }
        }

        /// <summary>
        /// 从事专业
        /// </summary>
        public string RootSpecSort
        {
            get
            { return _RootSpecSort; }
            set
            { _RootSpecSort = value; }
        }

        /// <summary>
        /// 医疗卡账号
        /// </summary>
        public string MediCardMark
        {
            get
            { return _MediCardMark; }
            set
            { _MediCardMark = value; }
        }

        /// <summary>
        /// 医疗卡号
        /// </summary>
        public string MediCard
        {
            get
            { return _MediCard; }
            set
            { _MediCard = value; }
        }

        /// <summary>
        /// 入伍时间
        /// </summary>
        public string BeEnrolledInDate
        {
            get
            {
                return _BeEnrolledInDate;
            }
            set
            { _BeEnrolledInDate = value; }
        }

        /// <summary>
        /// 工作日期
        /// </summary>
        public string WrokDate
        {
            get
            { return _WrokDate; }
            set
            { _WrokDate = value; }
        }

        /// <summary>
        /// 行政职务
        /// </summary>
        public string Duty
        {
            get
            { return _Duty; }
            set
            { _Duty = value; }
        }

        /// <summary>
        /// 职务时间
        /// </summary>
        public string DutyDate
        {
            get
            { return _DutyDate; }
            set
            { _DutyDate = value; }
        }

        /// <summary>
        /// 技术级
        /// </summary>
        public string TechnicClass
        {
            get
            { return _TechnicClass; }
            set
            { _TechnicClass = value; }
        }

        /// <summary>
        /// 技术级时间
        /// </summary>
        public string TechnicClassDate
        {
            get
            { return _TechnicClassDate; }
            set
            { _TechnicClassDate = value; }
        }

        /// <summary>
        /// 文职级
        /// </summary>
        public string CivilServiceClass
        {
            get
            { return _CivilServiceClass; }
            set
            { _CivilServiceClass = value; }
        }

        /// <summary>
        /// 文职级时间
        /// </summary>
        public string CivilServiceClassDate
        {
            get
            { return _CivilServiceClassDate; }
            set
            { _CivilServiceClassDate = value; }
        }

        public string Station_Code
        {
            get
            {
                return _Station_Code;
            }
            set
            {
                _Station_Code = value;
            }
        }

        public string Gord
        {
            get { return _Gord; }
            set { _Gord = value; }
        }

        public string Contractstart
        {
            get { return _Contractstart; }
            set { _Contractstart = value; }
        }

        public string Contractend
        {
            get { return _Contractend; }
            set { _Contractend = value; }
        }

        public string Bonusflag
        {
            get { return _Bonusflag; }
            set { _Bonusflag = value; }
        }

        public string Checkflag
        {
            get { return _Checkflag; }
            set { _Checkflag = value; }
        }
       
    }
}