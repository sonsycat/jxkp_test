using System;
using Goldnet.Ext.Web;
using System.Data;
using Goldnet.Dal.Properties.Bound;

namespace GoldNet.JXKP.Bonus.Input
{
    public partial class QuanZhong : PageBase
    {
        OperationDal dal = new OperationDal();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Ext.IsAjaxRequest)
            {

              
            }
        }

        /// <summary>
        /// 计算按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Buttonsave_Click(object sender, EventArgs e)
        {
            if (!dal.COSTS(Convert.ToDateTime(this.stardate.SelectedValue).ToString("yyyyMM")))
            {
                Goldnet.Ext.Web.Ext.Msg.Show(new Goldnet.Ext.Web.MessageBox.Config
                {
                    Title = "提示",
                    Message = "当月成本没有录入",
                    Buttons = MessageBox.Button.OK,
                    Icon = (MessageBox.Icon)Enum.Parse(typeof(MessageBox.Icon), "INFO")
                });
                return;
            }
            dal.deleteQUANZHONG(Convert.ToDateTime(this.stardate.SelectedValue).ToString("yyyyMM"));
            dal.UpdateQUANZHONG(Convert.ToDouble(JXBL.Text), Convert.ToDouble(YSRS.Text), Convert.ToDouble(HSRS.Text), Convert.ToDouble(YJRS.Text), Convert.ToDouble(YAOJURS.Text), Convert.ToDouble(CKRS.Text),
                Convert.ToDouble(XZRS.Text), Convert.ToDouble(YSXS.Text), Convert.ToDouble(HSXS.Text), Convert.ToDouble(YJXS.Text), Convert.ToDouble(YAOJUXS.Text), Convert.ToDouble(CKXS.Text), Convert.ToDouble(XZXS.Text), Convert.ToDouble(YZJCXS.Text), Convert.ToDouble(YZZXS.Text),
                Convert.ToDateTime(this.stardate.SelectedValue).ToString("yyyyMM"));

            ShowMessage("系统提示", "计算完成。");
            SetDict(Convert.ToDateTime(this.stardate.SelectedValue).ToString("yyyyMM"));
        }



        /// <summary>
        /// 查询按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Buttonselect_Click(object sender, EventArgs e)
        {

            DataTable dt = dal.GetQUANZHONG(Convert.ToDateTime(this.stardate.SelectedValue).ToString("yyyyMM"));
            INCOME.Text = dt.Rows[0]["INCOME"].ToString();
            COST.Text = dt.Rows[0]["COST"].ToString();
            INCOME_COST.Text = dt.Rows[0]["INCOME_COST"].ToString();
            JXBL.Text = dt.Rows[0]["JXBL"].ToString();
            GFJX.Text = dt.Rows[0]["GFJX"].ToString();
            YSRS.Text = dt.Rows[0]["YSRS"].ToString();
            HSRS.Text = dt.Rows[0]["HSRS"].ToString();
            YJRS.Text = dt.Rows[0]["YJRS"].ToString();
            YAOJURS.Text = dt.Rows[0]["YAOJURS"].ToString();
            CKRS.Text = dt.Rows[0]["CKRS"].ToString();
            XZRS.Text = dt.Rows[0]["XZRS"].ToString();
            YSXS.Text = dt.Rows[0]["YSXS"].ToString();
            HSXS.Text = dt.Rows[0]["HSXS"].ToString();
            YJXS.Text = dt.Rows[0]["YJXS"].ToString();
            YAOJUXS.Text = dt.Rows[0]["YAOJUXS"].ToString();
            CKXS.Text = dt.Rows[0]["CKXS"].ToString();
            XZXS.Text = dt.Rows[0]["XZXS"].ToString();
            YSBL.Text = dt.Rows[0]["YSBL"].ToString();
            HSBL.Text = dt.Rows[0]["HSBL"].ToString();
            YJBL.Text = dt.Rows[0]["YJBL"].ToString();
            YAOJUBL.Text = dt.Rows[0]["YAOJUBL"].ToString();
            CKBL.Text = dt.Rows[0]["CKBL"].ToString();
            XZBL.Text = dt.Rows[0]["XZBL"].ToString();
            YSZE.Text = dt.Rows[0]["YSZE"].ToString();
            HSZE.Text = dt.Rows[0]["HSZE"].ToString();
            YJZE.Text = dt.Rows[0]["YJZE"].ToString();
            YAOJUZE.Text = dt.Rows[0]["YAOJUZE"].ToString();
            CKZE.Text = dt.Rows[0]["CKZE"].ToString();
            XZZE.Text = dt.Rows[0]["XZZE"].ToString();

            YLDZE.Text = dt.Rows[0]["YLDZE"].ToString();
            YZJCXS.Text = dt.Rows[0]["YZJCXS"].ToString();
            YZZXS.Text = dt.Rows[0]["YZZXS"].ToString();
            YLDBL.Text = dt.Rows[0]["YLDBL"].ToString();
            //YSXYBL.Text = dt.Rows[0]["YSXYBL"].ToString();
            //YSYJBL.Text = dt.Rows[0]["YSYJBL"].ToString();
            //HSXYBL.Text = dt.Rows[0]["HSXYBL"].ToString();
            //HSYJBL.Text = dt.Rows[0]["HSYJBL"].ToString();
            //YJXYBL.Text = dt.Rows[0]["YJXYBL"].ToString();
            //YJYJBL.Text = dt.Rows[0]["YJYJBL"].ToString();
            //YSXYJE.Text = dt.Rows[0]["YSXYJE"].ToString();
            //YSYJEJ.Text = dt.Rows[0]["YSYJEJ"].ToString();
            //HSXYJE.Text = dt.Rows[0]["HSXYJE"].ToString();
            //HSYJJE.Text = dt.Rows[0]["HSYJJE"].ToString();
            //YJXYJE.Text = dt.Rows[0]["YJXYJE"].ToString();
            //YJYJJE.Text = dt.Rows[0]["YJYJJE"].ToString();
            //HSZCEJ.Text = dt.Rows[0]["HSZCEJ"].ToString();
            //HSZCBL.Text = dt.Rows[0]["HSZCBL"].ToString();
            //MZHSJE.Text = dt.Rows[0]["MZHSJE"].ToString();
            //HLLSBL.Text = dt.Rows[0]["HLLSBL"].ToString();
            //BLLSJE.Text = dt.Rows[0]["BLLSJE"].ToString();

        }

        /// <summary>
        /// 初始化控件值
        /// </summary>
        protected void SetDict( string startdate)
        {
            DataTable dt = dal.GetQUANZHONG(Convert.ToDateTime(this.stardate.SelectedValue).ToString("yyyyMM"));
            INCOME.Text = dt.Rows[0]["INCOME"].ToString();
            COST.Text = dt.Rows[0]["COST"].ToString();
            INCOME_COST.Text = dt.Rows[0]["INCOME_COST"].ToString();
            JXBL.Text = dt.Rows[0]["JXBL"].ToString();
            GFJX.Text = dt.Rows[0]["GFJX"].ToString();
            YSRS.Text = dt.Rows[0]["YSRS"].ToString();
            HSRS.Text = dt.Rows[0]["HSRS"].ToString();
            YJRS.Text = dt.Rows[0]["YJRS"].ToString();
            YAOJURS.Text = dt.Rows[0]["YAOJURS"].ToString();
            CKRS.Text = dt.Rows[0]["CKRS"].ToString();
            XZRS.Text = dt.Rows[0]["XZRS"].ToString();
            YSXS.Text = dt.Rows[0]["YSXS"].ToString();
            HSXS.Text = dt.Rows[0]["HSXS"].ToString();
            YJXS.Text = dt.Rows[0]["YJXS"].ToString();
            YAOJUXS.Text = dt.Rows[0]["YAOJUXS"].ToString();
            CKXS.Text = dt.Rows[0]["CKXS"].ToString();
            XZXS.Text = dt.Rows[0]["XZXS"].ToString();
            YSBL.Text = dt.Rows[0]["YSBL"].ToString();
            HSBL.Text = dt.Rows[0]["HSBL"].ToString();
            YJBL.Text = dt.Rows[0]["YJBL"].ToString();
            YAOJUBL.Text = dt.Rows[0]["YAOJUBL"].ToString();
            CKBL.Text = dt.Rows[0]["CKBL"].ToString();
            XZBL.Text = dt.Rows[0]["XZBL"].ToString();
            YSZE.Text = dt.Rows[0]["YSZE"].ToString();
            HSZE.Text = dt.Rows[0]["HSZE"].ToString();
            YJZE.Text = dt.Rows[0]["YJZE"].ToString();
            YAOJUZE.Text = dt.Rows[0]["YAOJUZE"].ToString();
            CKZE.Text = dt.Rows[0]["CKZE"].ToString();
            XZZE.Text = dt.Rows[0]["XZZE"].ToString();

            YLDZE.Text = dt.Rows[0]["YLDZE"].ToString();
            YZJCXS.Text = dt.Rows[0]["YZJCXS"].ToString();
            YZZXS.Text = dt.Rows[0]["YZZXS"].ToString();
            YLDBL.Text = dt.Rows[0]["YLDBL"].ToString();
            //YSXYBL.Text = dt.Rows[0]["YSXYBL"].ToString();
            //YSYJBL.Text = dt.Rows[0]["YSYJBL"].ToString();
            //HSXYBL.Text = dt.Rows[0]["HSXYBL"].ToString();
            //HSYJBL.Text = dt.Rows[0]["HSYJBL"].ToString();
            //YJXYBL.Text = dt.Rows[0]["YJXYBL"].ToString();
            //YJYJBL.Text = dt.Rows[0]["YJYJBL"].ToString();
            //YSXYJE.Text = dt.Rows[0]["YSXYJE"].ToString();
            //YSYJEJ.Text = dt.Rows[0]["YSYJEJ"].ToString();
            //HSXYJE.Text = dt.Rows[0]["HSXYJE"].ToString();
            //HSYJJE.Text = dt.Rows[0]["HSYJJE"].ToString();
            //YJXYJE.Text = dt.Rows[0]["YJXYJE"].ToString();
            //YJYJJE.Text = dt.Rows[0]["YJYJJE"].ToString();
            //HSZCEJ.Text = dt.Rows[0]["HSZCEJ"].ToString();
            //HSZCBL.Text = dt.Rows[0]["HSZCBL"].ToString();
            //MZHSJE.Text = dt.Rows[0]["MZHSJE"].ToString();
            //HLLSBL.Text = dt.Rows[0]["HLLSBL"].ToString();
            //BLLSJE.Text = dt.Rows[0]["BLLSJE"].ToString();

        }
    }
}