using System;
using System.Collections.Generic;
using Goldnet.Ext.Web;
using Goldnet.Dal;

namespace GoldNet.JXKP
{
    public partial class CheckPersonsList : PageBase
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Ext.IsAjaxRequest)
            {
                CheckPersons checkpersons = new CheckPersons();
                Store1.DataSource = checkpersons.GetCheckPersonsList();
                Store1.DataBind();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btn_Ref_Click(object sender, AjaxEventArgs e)
        {
            CheckPersons checkpersons = new CheckPersons();
            Store1.DataSource = checkpersons.GetCheckPersonsList();
            Store1.DataBind();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btn_Look_Click(object sender, AjaxEventArgs e)
        {
            //定义一个HashTable,将前台编辑按钮所选中的行数据复制到定义的HashTable对象selectRow中            
            Dictionary<string, string>[] selectRow = GetSelectRow(e);
            if (selectRow != null)
            {
                string year = selectRow[0]["YEARS"];
                string month = selectRow[0]["MONTHS"];
                Response.Redirect("CheckPersonsLook.aspx?CheckYear=" + year + "&CheckMonth=" + month + "&pageid=" + Request.QueryString["pageid"].ToString() + "");
            }
        }

        /// <summary>
        /// 
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
                    CheckPersons checkpersons = new CheckPersons();
                    checkpersons.DeleteCheckPerson(selectRow);
                    Goldnet.Ext.Web.Ext.Msg.Show(new Goldnet.Ext.Web.MessageBox.Config
                    {
                        Title = "提示",
                        Message = "删除成功",
                        Buttons = MessageBox.Button.OK,
                        Icon = (MessageBox.Icon)Enum.Parse(typeof(MessageBox.Icon), "INFO")
                    });
                    Store1.DataSource = checkpersons.GetCheckPersonsList();
                    Store1.DataBind();
                }
                catch (Exception ex)
                {
                    ShowDataError(ex.ToString(), Request.Url.LocalPath, "SaveCheckPersonsDetail");
                }
            }

        }

        //反序列化得到客户端提交的gridpanel数据行      
        private Dictionary<string, string>[] GetSelectRow(AjaxEventArgs e)
        {
            string row = e.ExtraParams["Values"].ToString();
            Dictionary<string, string>[] selectRow = JSON.Deserialize<Dictionary<string, string>[]>(row);
            if (selectRow.Length <= 0) { return null; } else { return selectRow; }
        }

        /// <summary>
        /// 
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
                Goldnet.Ext.Web.Ext.Redirect("CheckPersonsEdit.aspx?CheckYear=" + year + "&CheckMonth=" + month + "&pageid=" + Request.QueryString["pageid"].ToString()+"");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btn_Add_Click(object sender, AjaxEventArgs e)
        {
            Mask.Config msgconfig = new Mask.Config();
            msgconfig.Msg = "页面转向中...";
            msgconfig.MsgCls = "x-mask-loading";
            Goldnet.Ext.Web.Ext.Mask.Show(msgconfig);
            Goldnet.Ext.Web.Ext.Redirect("CheckPersonsAdd.aspx?pageid=" + Request.QueryString["pageid"].ToString() + "");
        }
    }
}
