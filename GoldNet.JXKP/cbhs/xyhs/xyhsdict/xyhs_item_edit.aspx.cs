using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.Text;
using Goldnet.Ext.Web;
using GoldNet.Comm;
using GoldNet.Comm.DAL.Oracle;
using System.Collections.Generic;
using GoldNet.Model;
using Goldnet.Dal;

namespace GoldNet.JXKP.cbhs.xyhs.xyhsdict
{
    public partial class xyhs_item_edit : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Ext.IsAjaxRequest)
            {
                SetDict();
                this.Combo_ItemType.SelectedItem.Value = Request["id"].ToString();

                if (Request["op"].ToString() != "add")
                {
                    XyhsDetail dal = new XyhsDetail();
                    DataTable tb = dal.GetCostitem(Request["id"].ToString());


                    if (tb.Rows.Count > 0)
                    {
                        Text_ItemCode.Text = Request["id"].ToString();
                        this.Combo_ItemType.SelectedItem.Value = tb.Rows[0]["ITEM_TYPE_CODE"].ToString();
                        this.Text_ItemName.Text = tb.Rows[0]["ITEM_NAME"].ToString();
                        Text_FINANCE_ITEM.Text = tb.Rows[0]["FINANCE_ITEM"].ToString();
                        Text_FINANCE_ITEM_GL.Text = tb.Rows[0]["FINANCE_ITEM_GL"].ToString();
                    }

                }
                else
                {
                    XyhsDetail dal = new XyhsDetail();
                    Text_ItemCode.Text = this.Combo_ItemType.SelectedItem.Value + dal.GetItemCode(this.Combo_ItemType.SelectedItem.Value);
                }

                

            }
        }

        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Buttonsave_Click(object sender, EventArgs e)
        {
            if (this.Combo_ItemType.SelectedItem.Value == string.Empty)
            {
                //this.ShowMessage("系统提示", "成本类别不能为空！");
            }

            else
            {
                try
                {
                    XyhsDetail dal = new XyhsDetail();
                    dal.SaveXyhsItem(Text_ItemCode.Text, this.Combo_ItemType.SelectedItem.Value, Text_ItemName.Text, this.Text_FINANCE_ITEM.Text,this.Text_FINANCE_ITEM_GL.Text);
                    Goldnet.Ext.Web.ScriptManager scManager = Goldnet.Ext.Web.ScriptManager.GetInstance(this.Page);
                    scManager.AddScript("parent.RefreshData('保存成功');");
                    scManager.AddScript("parent.DetailWin.hide();");
                    
                }
                catch (Exception ex)
                {
                    //ShowDataError(ex, Request.Url.LocalPath, "Buttonsave_Click");

                }
            }

        }
        /// <summary>
        ///设置下拉框
        /// </summary>
        public void SetDict()
        {
            XyhsDetail dal = new XyhsDetail();
            DataTable table = dal.GetCostType().Tables[0];
            if (table.Rows.Count > 0)
            {
                for (int i = 0; i < table.Rows.Count; i++)
                {
                    this.Combo_ItemType.Items.Add(new Goldnet.Ext.Web.ListItem(table.Rows[i]["item_name"].ToString(), table.Rows[i]["item_code"].ToString()));
                }

            }

        }

        protected void btnCancle_Click(object sender, EventArgs e)
        {
            Goldnet.Ext.Web.ScriptManager scManager = Goldnet.Ext.Web.ScriptManager.GetInstance(this.Page);

            scManager.AddScript("parent.DetailWin.hide();");

        }

        
    }
}
