using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GoldNet.Model
{
    public class CostItem
    {
        public  CostItem()
        {
 
        }

        private string item_type;

        public string Item_type
        {
            get { return item_type; }
            set { item_type = value; }
        }

        private string item_class;

        public string Item_class
        {
            get { return item_class; }
            set { item_class = value; }
        }

        private string item_code;

        public string Item_code
        {
            get { return item_code; }
            set { item_code = value; }
        }

        private string item_name;

        public string Item_name
        {
            get { return item_name; }
            set { item_name = value; }
        }

        private string cost_property;

        public string Cost_property
        {
            get { return cost_property; }
            set { cost_property = value; }
        }

        private string input_code;

        public string Input_code
        {
            get { return input_code; }
            set { input_code = value; }
        }

        private string allot_for_jc;

        public string Allot_for_jc
        {
            get { return allot_for_jc; }
            set { allot_for_jc = value; }
        }

        private string allot_for_jd;

        public string Allot_for_jd
        {
            get { return allot_for_jd; }
            set { allot_for_jd = value; }
        }

        private string allot_for_ry;

        public string Allot_for_ry
        {
            get { return allot_for_ry; }
            set { allot_for_ry = value; }
        }

        private string gettype;

        public string Gettype
        {
            get { return gettype; }
            set { gettype = value; }
        }

        private string account_type;

        public string Account_type
        {
            get { return account_type; }
            set { account_type = value; }
        }

        private decimal compute_per;

        public decimal Compute_per
        {
            get { return compute_per; }
            set { compute_per = value; }
        }


        private string cost_direct;

        public string Cost_direct
        {
            get { return cost_direct; }
            set { cost_direct = value; }
        }
    }
}
