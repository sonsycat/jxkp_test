using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GoldNet.Model
{
    [Serializable]
    public class AppendIncome
    {
        public AppendIncome()
        {
 
        }
        private string row_id;
        private string reck_item;
        private double incomes;
        private double incomes_charges;
        private string ordered_by;
        private string performed_by;
        private string ward_code;
        private string order_doctor;
        private string incom_type;
        private string account_type;
        private string accounting_date;
        private string remarks;
        private string operator_date;


        public string Row_id
        {
            get { return row_id; }
            set { row_id = value; }
        }

        public string Reck_item
        {
            get { return reck_item; }
            set { reck_item = value; }
        }

        public double Incomes
        {
            get { return incomes; }
            set { incomes = value; }
        }

        public double Incomes_charges
        {
            get { return incomes_charges; }
            set { incomes_charges = value; }
        }

        public string Ordered_by
        {
            get { return ordered_by; }
            set { ordered_by = value; }
        }

        public string Performed_by
        {
            get { return performed_by; }
            set { performed_by = value; }
        }


        public string Ward_code
        {
            get { return ward_code; }
            set { ward_code = value; }
        }


        public string Order_doctor
        {
            get { return order_doctor; }
            set { order_doctor = value; }
        }

        public string Incom_type
        {
            get { return incom_type; }
            set { incom_type = value; }
        }

        public string Account_type
        {
            get { return account_type; }
            set { account_type = value; }
        }

        public string Accounting_date
        {
            get { return accounting_date; }
            set { accounting_date = value; }
        }

        public string Remarks
        {
            get { return remarks; }
            set { remarks = value; }
        }

        public string Operator_date
        {
            get { return operator_date; }
            set { operator_date = value; }
        }
    }
}
