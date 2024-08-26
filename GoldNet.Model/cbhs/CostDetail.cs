using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GoldNet.Model
{
    public class CostDetail
    {
        public CostDetail()
        {

        }
        private string dept_code;

        public string Dept_code
        {
            get { return dept_code; }
            set { dept_code = value; }
        }
        private string item_code;

        public string Item_code
        {
            get { return item_code; }
            set { item_code = value; }
        }
        private string accounting_date;

        public string Accounting_date
        {
            get { return accounting_date; }
            set { accounting_date = value; }
        }
        private string total_costs;

        public string Total_costs
        {
            get { return total_costs; }
            set { total_costs = value; }
        }

        private string costs;

        public string Costs
        {
            get { return costs; }
            set { costs = value; }
        }
        private string operators;

        public string Operators
        {
            get { return operators; }
            set { operators = value; }
        }
        private string operator_date;

        public string Operator_date
        {
            get { return operator_date; }
            set { operator_date = value; }
        }
        private string get_type;

        public string Get_type
        {
            get { return get_type; }
            set { get_type = value; }
        }
        private string costs_armyfree;

        public string Costs_armyfree
        {
            get { return costs_armyfree; }
            set { costs_armyfree = value; }
        }
        private string cost_flag;

        public string Cost_flag
        {
            get { return cost_flag; }
            set { cost_flag = value; }
        }
        private string balance_tag;

        public string Balance_tag
        {
            get { return balance_tag; }
            set { balance_tag = value; }
        }
        private string memo;

        public string Memo
        {
            get { return memo; }
            set { memo = value; }
        }
        private string dept_type_flag;

        public string Dept_type_flag
        {
            get { return dept_type_flag; }
            set { dept_type_flag = value; }
        }

        private string _MEDICINE_COSTS;

        public string MEDICINE_COSTS
        {
            get { return _MEDICINE_COSTS; }
            set { _MEDICINE_COSTS = value; }
        }
        private string _MANAGE_COSTS;

        public string MANAGE_COSTS
        {
            get { return _MANAGE_COSTS; }
            set { _MANAGE_COSTS = value; }
        }

        private string _FINANCE_ITEM;

        public string FINANCE_ITEM
        {
            get { return _FINANCE_ITEM; }
            set { _FINANCE_ITEM = value; }
        }

        private string _FINANCE_ITEM_GL;

        public string FINANCE_ITEM_GL
        {
            get { return _FINANCE_ITEM_GL; }
            set { _FINANCE_ITEM_GL = value; }
        }
    }
}
