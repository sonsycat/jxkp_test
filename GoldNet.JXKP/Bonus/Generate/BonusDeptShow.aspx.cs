using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Goldnet.Dal;
using Goldnet.Ext.Web;

namespace GoldNet.JXKP
{
    public partial class BonusDeptShow :PageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Ext.IsAjaxRequest)
            {
                SetDict();
                
            }
        }

        public void Bindlist()
        {

            CalculateBonus calculateBonus = new CalculateBonus();
            DataTable dt=calculateBonus.GetDeptBonusList(this.comindex.SelectedItem.Value, this.DeptFilter(""));
            DataRow dr = dt.NewRow();
            //dr["SEC_UNIT_NAME"] = "ALL";
            //dr["UNIT_NAME"] = "合计";
            //dr["UNIT_BONUS"] = dt.Compute("Sum(UNIT_BONUS)/2", "");
            //dr["BONUS_PERSONS_VALUE"] = dt.Compute("Sum(BONUS_PERSONS_VALUE)/2", "");
            //dt.Rows.Add(dr);
            Store1.DataSource = dt;
            Store1.DataBind();
            this.Store1.DataSource = dt;
            this.Store1.DataBind();
            this.yf_bonus.Text = dt.Compute("Sum(UNIT_BONUS)/2", "").ToString();
            this.sf_bonus.Text = dt.Compute("Sum(BONUS_PERSONS_VALUE)/2", "").ToString();
        }
        /// <summary>
        /// 下拉框设置
        /// </summary>
        public void SetDict()
        {
            CalculateBonus calculateBonus = new CalculateBonus();
            DataTable table = calculateBonus.GetIndex();
            if (table.Rows.Count > 0)
            {
                for (int i = 0; i < table.Rows.Count; i++)
                {
                    this.comindex.Items.Add(new Goldnet.Ext.Web.ListItem(table.Rows[i]["BONUSNAME"].ToString(), table.Rows[i]["ID"].ToString()));
                }

            }

        }
        protected void SelectedFunc(object sender, AjaxEventArgs e)
        {
            Bindlist();
        }
    }
}
