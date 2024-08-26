using System;
using System.Drawing;
using System.Data;
using System.Data.OleDb;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using Goldnet.Ext.Web;
using GoldNet.Comm;
using GoldNet.Comm.Pic;
using GoldNet.Comm.DAL.Oracle;
using GoldNet.Model;
using GoldNet.JXKP.WebService;
using System.Xml;


namespace GoldNet.JXKP.WebPage.SysManager
{
    public partial class FunctionSearch : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Ext.IsAjaxRequest)
            {
                SetDict();
                //
//                UserService us = new UserService();
//                string strxml = @"<?xml version=""1.0"" encoding=""utf-8"" ?>  
//                <userinfo> 
//                  <user><db_user>DM</db_user><user_id>0997</user_id> <user_name>杜明(大)</user_name><user_dept>1101</user_dept><user_pswd>qqqqq</user_pswd><input_code>XW</input_code></user> 
//                </userinfo>";
//                string strxmlA = us.SyncUser(strxml);
//                string strxmlB = us.SyncUser(strxm);

            }
        }
        /// <summary>
        /// 下拉框设置
        /// </summary>
        public void SetDict()
        {
            Goldnet.Dal.SYS_ROLE_DICT dal = new Goldnet.Dal.SYS_ROLE_DICT();
            DataTable table = dal.GetFunction("").Tables[0];
            if (table.Rows.Count > 0)
            {
                for (int i = 0; i < table.Rows.Count; i++)
                {
                    this.Combo_Func.Items.Add(new Goldnet.Ext.Web.ListItem(table.Rows[i]["function_name"].ToString(), table.Rows[i]["function_id"].ToString()));
                }

            }
           
        }
        /// <summary>
        /// 人员查询
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void SelectedFunc(object sender, AjaxEventArgs e)
        {
            Goldnet.Dal.SYS_ROLE_DICT dal = new Goldnet.Dal.SYS_ROLE_DICT();
            DataSet ds = dal.GetUserbyFunction(this.Combo_Func.SelectedItem.Value);
            this.Store1.DataSource = ds.Tables[0];
            this.Store1.DataBind();
        }
        protected void SelectFunc(object sender, AjaxEventArgs e)
        {
        }
    }
}
