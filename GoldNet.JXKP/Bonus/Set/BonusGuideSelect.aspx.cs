using System;
using System.Collections.Generic;
using System.Data;
using Goldnet.Ext.Web;
using GoldNet.Comm;

namespace GoldNet.JXKP
{
    public partial class BonusGuideSelect : System.Web.UI.Page
    {
        Goldnet.Dal.BonusGuideDict dal = new Goldnet.Dal.BonusGuideDict();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            //检查是否已经登录，否则停止
            if (Session["CURRENTSTAFF"] == null)
            {
                Response.End();
                return;
            }
            if (!Ext.IsAjaxRequest)
            {
                //显示该指标集中已经包含的各项指标，以及指标权重
                getAllShow(Combo_DeptType.SelectedItem.Value);
            }
        }

        //保存指标集定义中包含的指标及每项指标的权重
        protected void SaveGuide(object sender, AjaxEventArgs e)
        {
            string reportID = Combo_DeptType.SelectedItem.Value;
            if (reportID == "" || reportID == null)
            {
                Goldnet.Ext.Web.Ext.Msg.Show(new Goldnet.Ext.Web.MessageBox.Config
                {
                    Title = SystemMsg.msgtitle4,
                    Message = "请选择科室",
                    Buttons = MessageBox.Button.OK,
                    Icon = (MessageBox.Icon)Enum.Parse(typeof(MessageBox.Icon), "INFO")
                });
                getAllShow(reportID);
            }
            else
            {

                string selectedid = e.ExtraParams["multi2"];
                Dictionary<string, string>[] selectedRow = JSON.Deserialize<Dictionary<string, string>[]>(selectedid);
                string msg = dal.CheckGuidePrimary(selectedRow, reportID);
                if (msg != "")
                {
                    Goldnet.Ext.Web.Ext.Msg.Show(new Goldnet.Ext.Web.MessageBox.Config
                    {
                        Title = SystemMsg.msgtitle4,
                        Message = msg + "未在指标集中设置",
                        Buttons = MessageBox.Button.OK,
                        Icon = (MessageBox.Icon)Enum.Parse(typeof(MessageBox.Icon), "INFO")
                    });
                }
                else
                {
                    string rtn = dal.UpdateGuideGatherDetail(selectedRow, reportID);
                    Goldnet.Ext.Web.Ext.Msg.Show(new Goldnet.Ext.Web.MessageBox.Config
                   {
                       Title = SystemMsg.msgtitle4,
                       Message = rtn + "设置成功",
                       Buttons = MessageBox.Button.OK,
                       Icon = (MessageBox.Icon)Enum.Parse(typeof(MessageBox.Icon), "INFO")
                   });
                    getAllShow(reportID);
                }
            }
        }

        /// <summary>
        /// 科室类别选择触发处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Search_Select(object sender, EventArgs e)
        {
            if (Combo_DeptType.SelectedItem.Value == "")
            {
                return;
            }
            string reportID = Combo_DeptType.SelectedItem.Value;
            getAllShow(reportID);
        }

        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="reportID"></param>
        private void getAllShow(string reportID)
        {
            if (reportID == "" || reportID == null)
            {
                //获取待选指标并绑定列表
                DataTable dt = dal.GetGuideDictListByBscOrgDept(reportID).Tables[0];
                Store1.DataSource = dt;
                Store1.DataBind();

                Store2.DataBind();
            }
            else
            {
                //获取已选指标并绑定列表
                DataTable table = dal.GetGuideGroupList(reportID).Tables[0];
                if (table.Rows.Count > 0)
                {
                    Store2.DataSource = table;
                    Store2.DataBind();
                }
                else
                {
                    Store2.DataBind();
                }

                //获取待选指标并绑定列表
                DataTable dt = dal.GetGuideDictListByBscOrgDept(reportID).Tables[0];
                if (dt.Rows.Count > 0)
                {
                    Store1.DataSource = dt;
                    Store1.DataBind();
                }
                else
                {
                    Store1.DataBind();
                }
            }
        }


    }
}
