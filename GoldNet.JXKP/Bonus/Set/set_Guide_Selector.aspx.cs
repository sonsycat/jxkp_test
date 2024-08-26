using System;
using System.Collections.Generic;
using System.Data;
using System.Collections;
using Goldnet.Ext.Web;
using GoldNet.Comm;
using Goldnet.Dal.Properties.Bound;

namespace GoldNet.JXKP.Bonus.Set
{
    public partial class set_Guide_Selector : System.Web.UI.Page
    {
        Goldnet.Dal.Guide_Dict dal = new Goldnet.Dal.Guide_Dict();

        bound_Guide_Group dal_b = new bound_Guide_Group();

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
                string GuideGatherDefine = Session["GUIDEGATHERDEFINE"].ToString();
                if ((GuideGatherDefine == null) | (GuideGatherDefine.Equals("")))
                {
                    Response.End();
                    return;
                }
                string GuideGatherID = GuideGatherDefine.Split(',')[0];
                string GuideGatherOrgan = GuideGatherDefine.Split(',')[1];
                //显示该指标集中已经包含的各项指标，以及指标权重
                DataTable table = dal_b.GetGuideGroupListByGatherCode(GuideGatherID).Tables[0];
                Store2.DataSource = table;
                Store2.DataBind();

                DataTable dt = dal_b.GetGuideDictListByGatherCode(GuideGatherID).Tables[0];
                Store1.DataSource = dt;
                Store1.DataBind();
               
            }
        }


        

        //保存指标集定义中包含的指标及每项指标的权重
        protected void SaveGuide(object sender, AjaxEventArgs e)
        {
            string GuideGatherDefine = Session["GUIDEGATHERDEFINE"].ToString();
            if ((GuideGatherDefine == null) | (GuideGatherDefine.Equals("")))
            {
                Response.End();
                return;
            }
            string GuideGatherID = GuideGatherDefine.Split(',')[0];
            string selectedid = e.ExtraParams["multi2"];
            Dictionary<string, string>[] selectedRow = JSON.Deserialize<Dictionary<string, string>[]>(selectedid);

            string rtn = dal_b.UpdateGuideGatherDetail(GuideGatherID, selectedRow);
            Goldnet.Ext.Web.Ext.Msg.Show(new Goldnet.Ext.Web.MessageBox.Config
            {
                Title = SystemMsg.msgtitle4,
                Message = (rtn.Equals("") ? "指标集保存成功！" : rtn),
                Buttons = MessageBox.Button.OK,
                Icon = (MessageBox.Icon)Enum.Parse(typeof(MessageBox.Icon), "INFO")
            });
        }







    }
}
