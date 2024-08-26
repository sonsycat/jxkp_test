using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Text;
using Goldnet.Ext.Web;
using GoldNet.Comm;
using GoldNet.Comm.DAL.Oracle;
using GoldNet.Model;

namespace GoldNet.JXKP.jxkh
{
    public partial class Evaluate_Type : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["CURRENTSTAFF"] == null)
            {
                Response.End();
                return;
            }
            if (!Ext.IsAjaxRequest)
            {
                  Store_RefreshData(null, null);
            }
        }
        /// <summary>
        /// 字典数据刷新
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Store_RefreshData(object sender, StoreRefreshDataEventArgs e)
        {
            Goldnet.Dal.Guide_Group dal = new Goldnet.Dal.Guide_Group();
            DataTable table = dal.GetEvaluateTypeList("").Tables[0];
            this.Store1.DataSource = table;
            this.Store1.DataBind();
            DataTable dt = dal.GetEvaluateTypeTarget().Tables[0];
            this.StoreCombo.DataSource = dt;
            this.StoreCombo.DataBind();
        }
        /// <summary>
        /// 删除字典数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Btn_Del_Click(object sender, AjaxEventArgs e)
        {
            string codeStr = e.ExtraParams["Values"];
            Goldnet.Dal.Guide_Group dal = new Goldnet.Dal.Guide_Group();
            string rtn = dal.EvaluateTypeDel(codeStr);
            if (rtn.Equals(""))
            {
                Store_RefreshData(null, null);
            }
            else
            {
                Goldnet.Ext.Web.Ext.Msg.Show(new Goldnet.Ext.Web.MessageBox.Config
                {
                    Title = SystemMsg.msgtitle4,
                    Message = rtn,
                    Buttons = MessageBox.Button.OK,
                    Icon = (MessageBox.Icon)Enum.Parse(typeof(MessageBox.Icon), "INFO")
                });
            }            
        }
        /// <summary>
        /// 保存字典数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Btn_Save_Click(object sender, AjaxEventArgs e)
        {
            string values = e.ExtraParams["Values"].ToString();
            Dictionary<string, string>[] dataRows = JSON.Deserialize<Dictionary<string, string>[]>(values);
            if (dataRows.Length <= 0) 
            {
                Store_RefreshData(null, null);
                return; 
            }
            else 
            {
                Goldnet.Dal.Guide_Group dal = new Goldnet.Dal.Guide_Group();
                string rtn = dal.EvaluateTypeUpdate(dataRows);
                if (rtn.Equals(""))
                {
                    Store_RefreshData(null, null);
                    Btn_Add.Disabled = false;
                    Btn_Refresh.Disabled = false;
                    Btn_Save.Disabled = true;
                    Btn_Cancel.Disabled = true;
                }
                else
                {
                    Goldnet.Ext.Web.Ext.Msg.Show(new Goldnet.Ext.Web.MessageBox.Config
                    {
                        Title = SystemMsg.msgtitle4,
                        Message = rtn,
                        Buttons = MessageBox.Button.OK,
                        Icon = (MessageBox.Icon)Enum.Parse(typeof(MessageBox.Icon), "INFO")
                    });
                } 
            }
        }

    }
}
