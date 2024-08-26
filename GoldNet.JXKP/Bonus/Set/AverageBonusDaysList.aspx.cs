using System;
using System.Collections.Generic;
using Goldnet.Ext.Web;
using Goldnet.Dal;

namespace GoldNet.JXKP
{
    /// <summary>
    /// 统计已经录入的人每个年月的平均奖科室人的信息
    /// </summary>
	public partial class AverageBonusDaysList : PageBase
	{
        /// <summary>
        /// 初始化处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
		protected void Page_Load(object sender, EventArgs e)
		{
            if (!Ext.IsAjaxRequest)
            {
                //统计平均奖人的信息
                AverageBonusDays averagebonusdays = new AverageBonusDays();
                Store1.DataSource = averagebonusdays.GetAverageBonusList();
                Store1.DataBind();
            }
		}

        /// <summary>
        /// 刷新处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btn_Ref_Click(object sender, AjaxEventArgs e)
        {
            AverageBonusDays averagebonusdays = new AverageBonusDays();
            Store1.DataSource = averagebonusdays.GetAverageBonusList();
            Store1.DataBind();
        }

        /// <summary>
        /// 查看处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btn_Look_Click(object sender, AjaxEventArgs e)
        {
            //定义一个HashTable,将前台编辑按钮所选中的行数据复制到定义的HashTable对象selectRow中            
            Dictionary<string, string>[] selectRow = GetSelectRow(e);
            if (selectRow != null)
            {
                string year=selectRow[0]["YEARS"];
                string month = selectRow[0]["MONTHS"];
                Response.Redirect("AverageBonusDaysLook.aspx?AverageYear=" + year + "&AverageMonth="+month+"");
            }
        }

        /// <summary>
        /// 删除所在月份的平均奖的人的信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btn_Del_Click(object sender, AjaxEventArgs e)
        {
             Dictionary<string, string>[] selectRow = GetSelectRow(e);
            if (selectRow != null)
            {
                try
                {
                    AverageBonusDays averagebonusdays = new AverageBonusDays();
                    averagebonusdays.DeleteAvergeBonusDays(selectRow[0]["YEARS"], selectRow[0]["MONTHS"]);
                    Goldnet.Ext.Web.Ext.Msg.Show(new Goldnet.Ext.Web.MessageBox.Config
                    {
                        Title ="提示",
                        Message = "删除成功",
                        Buttons = MessageBox.Button.OK,
                        Icon = (MessageBox.Icon)Enum.Parse(typeof(MessageBox.Icon), "INFO")
                    });  
                    Store1.DataSource = averagebonusdays.GetAverageBonusList();
                    Store1.DataBind();
                }
                catch (Exception ex)
                {
                    ShowDataError(ex.ToString(), Request.Url.LocalPath, "SaveAverageBonusDaysList");
                }
            }
        }

        /// <summary>
        /// 反序列化得到客户端提交的gridpanel数据行  
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        private Dictionary<string, string>[] GetSelectRow(AjaxEventArgs e)
        {
            string row = e.ExtraParams["Values"].ToString();
            Dictionary<string, string>[] selectRow = JSON.Deserialize<Dictionary<string, string>[]>(row);
            if (selectRow.Length <= 0) { return null; } else { return selectRow; }
        }

        /// <summary>
        /// 获得要编辑的年月，跳转到人编辑界面
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btn_Edit_Click(object sender, AjaxEventArgs e)
        {
            //定义一个HashTable,将前台编辑按钮所选中的行数据复制到定义的HashTable对象selectRow中            
            Dictionary<string, string>[] selectRow = GetSelectRow(e);
            if (selectRow != null)
            {
                string year = selectRow[0]["YEARS"];
                string month = selectRow[0]["MONTHS"];
                Mask.Config msgconfig = new Mask.Config();
                msgconfig.Msg = "页面转向中...";
                msgconfig.MsgCls = "x-mask-loading";
                Goldnet.Ext.Web.Ext.Mask.Show(msgconfig);
                Goldnet.Ext.Web.Ext.Redirect("AverageBonusDaysDetail.aspx?AverageYear=" + year + "&AverageMonth=" + month + "");
            }
        }

        /// <summary>
        /// 新建一个新的平均奖科室的人的信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btn_Add_Click(object sender, AjaxEventArgs e)
        {
            Mask.Config msgconfig = new Mask.Config();
            msgconfig.Msg = "页面转向中...";
            msgconfig.MsgCls = "x-mask-loading";
            Goldnet.Ext.Web.Ext.Mask.Show(msgconfig);
            //Goldnet.Ext.Web.Ext.Redirect("AverageBonusDaysAdd.aspx");
            Goldnet.Ext.Web.Ext.Redirect("AverageBonusDaysDetail.aspx");
        }
	}
}
