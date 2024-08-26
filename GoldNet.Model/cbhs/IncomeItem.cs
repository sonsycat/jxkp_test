using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GoldNet.Model
{
    public class IncomeItem
    {
        public IncomeItem()
        {

        }
        private string item_class;

        public string Item_class
        {
            get { return item_class; }
            set { item_class = value; }
        }
        private string item_name;

        public string Item_name
        {
            get { return item_name; }
            set { item_name = value; }
        }
        private string input_code;

        public string Input_code
        {
            get { return input_code; }
            set { input_code = value; }
        }
        private double inp_grade;

        public double INP_GRADE
        {
            get { return inp_grade; }
            set { inp_grade = value; }
        }
        private double oup_grade;

        public double OUP_GRADE
        {
            get { return oup_grade; }
            set { oup_grade = value; }
        }


        private double type_code;

        public double TYPE_CODE
        {
            get { return type_code; }
            set { type_code = value; }
        }
        private double _ORDER_DEPT_DISTRIBUT;

        public double Order_dept_distribut
        {
            get { return _ORDER_DEPT_DISTRIBUT; }
            set { _ORDER_DEPT_DISTRIBUT = value; }
        }
        private double _PERFORM_DEPT_DISTRIBUT;

        public double Perform_dept_distribut
        {
            get { return _PERFORM_DEPT_DISTRIBUT; }
            set { _PERFORM_DEPT_DISTRIBUT = value; }
        }
        private double _Nursing_percen;

        public double Nursing_percen
        {
            get { return _Nursing_percen; }
            set { _Nursing_percen = value; }
        }
        private double _Out_opdept_percen;

        public double Out_opdept_percen
        {
            get { return _Out_opdept_percen; }
            set { _Out_opdept_percen = value; }
        }
        private double _Out_exdept_percen;

        public double Out_exdept_percen
        {
            get { return _Out_exdept_percen; }
            set { _Out_exdept_percen = value; }
        }
        private double _Out_nursing_percen;

        public double Out_nursing_percen
        {
            get { return _Out_nursing_percen; }
            set { _Out_nursing_percen = value; }
        }
        private double cooperant_prercen;

        public double Cooperant_prercen
        {
            get { return cooperant_prercen; }
            set { cooperant_prercen = value; }
        }
        private string calculation_type;

        public string Calculation_type
        {
            get { return calculation_type; }
            set { calculation_type = value; }
        }
        private double fixed_percen;

        public double Fixed_percen
        {
            get { return fixed_percen; }
            set { fixed_percen = value; }
        }
        private string cost_code;

        public string Cost_code
        {
            get { return cost_code; }
            set { cost_code = value; }
        }

        private double profit_rate;

        public double Profit_rate
        {
            get { return profit_rate; }
            set { profit_rate = value; }
        }

        private string _PERFRO_DEPT;

        public string PERFRO_DEPT
        {
            get { return _PERFRO_DEPT; }
            set { _PERFRO_DEPT = value; }
        }
        private string _OTHER_DEPT;

        public string OTHER_DEPT
        {
            get { return _OTHER_DEPT; }
            set { _OTHER_DEPT = value; }
        }
        private double _OTHER_PERCEN;

        public double OTHER_PERCEN
        {
            get { return _OTHER_PERCEN; }
            set { _OTHER_PERCEN = value; }
        }
        //
        private string _OUT_OTHER_DEPT;

        public string OUT_OTHER_DEPT
        {
            get { return _OUT_OTHER_DEPT; }
            set { _OUT_OTHER_DEPT = value; }
        }
        private double _OUT_OTHER_PERCEN;

        public double OUT_OTHER_PERCEN
        {
            get { return _OUT_OTHER_PERCEN; }
            set { _OUT_OTHER_PERCEN = value; }
        }
        //
        private double _ZJCBBL;

        public double ZJCBBL
        {
            get { return _ZJCBBL; }
            set { _ZJCBBL = value; }
        }
        private double _JJCBBL;

        public double JJCBBL
        {
            get { return _JJCBBL; }
            set { _JJCBBL = value; }
        }
        private double _DCCB;

        public double DCCB
        {
            get { return _DCCB; }
            set { _DCCB = value; }
        }
        private string _CLASSTYPE;

        public string CLASSTYPE
        {
            get { return _CLASSTYPE; }
            set { _CLASSTYPE = value; }
        }
        private string _CLASSNAME;

        public string CLASSNAME
        {
            get { return _CLASSNAME; }
            set { _CLASSNAME = value; }
        }
        //平衡积分卡字段
        private string CARD_ID;

        public string _CARD_ID
        {
            get { return CARD_ID; }
            set { CARD_ID = value; }
        }
        private string CARD_NAME;

        public string _CARD_NAME
        {
            get { return CARD_NAME; }
            set { CARD_NAME = value; }
        }
        private double FIRST_INDEX;

        public double _FIRST_INDEX
        {
            get { return FIRST_INDEX; }
            set { FIRST_INDEX = value; }
        }
        private double FIRST_GRADE;

        public double _FIRST_GRADE
        {
            get { return FIRST_GRADE; }
            set { FIRST_GRADE = value; }
        }
        private double SECOND_INDEX;

        public double _SECOND_INDEX
        {
            get { return SECOND_INDEX; }
            set { SECOND_INDEX = value; }
        }
        private double SECOND_GRADE;

        public double _SECOND_GRADE
        {
            get { return SECOND_GRADE; }
            set { SECOND_GRADE = value; }
        }
        private double THIRD_INDEX;

        public double _THIRD_INDEX
        {
            get { return THIRD_INDEX; }
            set { THIRD_INDEX = value; }
        }
        private double THIRD_GRADE;

        public double _THIRD_GRADE
        {
            get { return THIRD_GRADE; }
            set { THIRD_GRADE = value; }
        }
        private double FOURTH_INDEX;

        public double _FOURTH_INDEX
        {
            get { return FOURTH_INDEX; }
            set { FOURTH_INDEX = value; }
        }
        private double FOURTH_GRADE;

        public double _FOURTH_GRADE
        {
            get { return FOURTH_GRADE; }
            set { FOURTH_GRADE = value; }
        }
        private double FIFTH_INDEX;

        public double _FIFTH_INDEX
        {
            get { return FIFTH_INDEX; }
            set { FIFTH_INDEX = value; }
        }
        private double FIFTH_GRADE;

        public double _FIFTH_GRADE
        {
            get { return FIFTH_GRADE; }
            set { FIFTH_GRADE = value; }
        }
        private string MEMO;

        public string _MEMO
        {
            get { return MEMO; }
            set { MEMO = value; }
        }
        private string DEPT_NAME;

        public string _DEPT_NAME
        {
            get { return DEPT_NAME; }
            set { DEPT_NAME = value; }
        }

    }
}
