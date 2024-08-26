using System;
using Goldnet.Ext.Web;
using Goldnet.Dal;
using System.Data;

namespace GoldNet.JXKP.cbhs.cbhsdict
{
    public partial class Item_Dict : System.Web.UI.Page
    {
        DictMainTainDal dal = new DictMainTainDal();
        protected void Page_Load(object sender, EventArgs e)
        {
            //检查是否已经登录，否则停止
            if (Session["CURRENTSTAFF"] == null)
            {
                Response.End();
            }

            if (!Ext.IsAjaxRequest)
            {
                this.Store1.DataSource = dal.getDutyItem_Dict().Tables[0];
                this.Store1.DataBind();
                DataRow[] deptrolerow1 = dal.GetAccoutCLASS_CODE().Tables[0].Select();

                foreach (DataRow roww in deptrolerow1)
                {
                    this.cost_SelectDept.Items.Add(new Goldnet.Ext.Web.ListItem(roww["CLASS_NAME"].ToString(), roww["CLASS_CODE"].ToString()));
                }

            }
        }

        /// <summary>
        /// 刷新数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void Data_RefreshData(object sender, StoreRefreshDataEventArgs e)
        {
            Store1.RemoveAll();
            Store1.DataSource = dal.getDutyItem_Dict().Tables[0];
            Store1.DataBind();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="DictCode"></param>
        /// <param name="DictName"></param>
        /// <param name="OperType"></param>
        /// <param name="Code"></param>
        [AjaxMethod]
        public void DictAjaxOper(string CLASS_CODE, string DEPT_NAME, string OperType, string ITEM_CODE, string ITEM_NAME)
        {
            switch (OperType)
            {
                case "1":
                     CalculateBonus cal_dal = new CalculateBonus();
                    if (!cal_dal.CheckLinShi(ITEM_CODE))
            {
                Goldnet.Ext.Web.Ext.Msg.Show(new Goldnet.Ext.Web.MessageBox.Config
                {
                    Title = "提示",
                    Message = "没有输入正确项目编码",
                    Buttons = MessageBox.Button.OK,
                    Icon = (MessageBox.Icon)Enum.Parse(typeof(MessageBox.Icon), "INFO")
                });
                return;
            }
                    dal.InsertItem_Dict(ITEM_CODE, DEPT_NAME, ITEM_NAME, CLASS_CODE);
                    break;
                case "2":
                    dal.UpdataIItem_Dict(ITEM_CODE, DEPT_NAME, ITEM_NAME, CLASS_CODE);
                    break;
                case "3":
                    dal.DelDutyItem_Dict(ITEM_CODE);
                    break;
                   
            }
          
        }
    }
}