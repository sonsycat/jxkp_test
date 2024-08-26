using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Goldnet.Dal;
using Goldnet.Ext.Web;
using System.Data;
using GoldNet.Comm;
using GoldNet.Model;
using Goldnet.Dal.Properties.Bound;

namespace GoldNet.JXKP.Bonus.Input
{
    public partial class Operation_Info_Edit : PageBase
    {
        OperationDal dal = new OperationDal();
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void Buttonsave_Click(object sender, EventArgs e)
        {
        }

        protected void btnCancle_Click(object sender, EventArgs e)
        {
        }

        /// <summary>
        /// 查询按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Buttonselect_Click(object sender, EventArgs e)
        {
            if (text_PATIENT_ID.Text.Equals(""))
            {
                ShowMessage("系统提示", "请输入住院编号。");
                return;

            }

            DataTable dt = dal.GetOerationByPid(text_PATIENT_ID.Text);
            date_SCHEDULED_DATE_TIME.Value = dt.Rows[0]["SCHEDULED_DATE_TIME"].ToString();
            text_OPERATION_NAME.Text = dt.Rows[0]["OPERATION_NAME"].ToString();
            text_SURGEON.Text = dt.Rows[0]["SURGEON"].ToString();
            text_FIRST_ASSISTANT.Text = dt.Rows[0]["FIRST_ASSISTANT"].ToString();
            text_SECOND_ASSISTANT.Text = dt.Rows[0]["SECOND_ASSISTANT"].ToString();
            text_THIRD_ASSISTANT.Text = dt.Rows[0]["THIRD_ASSISTANT"].ToString();
            text_FOURTH_ASSISTANT.Text = dt.Rows[0]["FOURTH_ASSISTANT"].ToString();
            text_ANESTHESIA_DOCTOR.Text = dt.Rows[0]["ANESTHESIA_DOCTOR"].ToString();
            text_ANESTHESIA_ASSISTANT.Text = dt.Rows[0]["ANESTHESIA_ASSISTANT"].ToString();
            text_FIRST_OPERATION_NURSE.Text = dt.Rows[0]["FIRST_OPERATION_NURSE"].ToString();
            text_SECOND_OPERATION_NURSE.Text = dt.Rows[0]["SECOND_OPERATION_NURSE"].ToString();
            text_FIRST_SUPPLY_NURSE.Text = dt.Rows[0]["FIRST_SUPPLY_NURSE"].ToString();
            text_SECOND_SUPPLY_NURSE.Text = dt.Rows[0]["SECOND_SUPPLY_NURSE"].ToString();


        }
    }
}