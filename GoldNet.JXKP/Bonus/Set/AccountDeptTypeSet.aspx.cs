using System;
using System.Collections.Generic;
using Goldnet.Ext.Web;
using Goldnet.Dal;
using System.Data;

namespace GoldNet.JXKP
{
    public partial class AccountDeptTypeSet : PageBase
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
                //初始化年，月列表
                BoundComm boundcomm = new BoundComm();
                AccountDeptType accountDeptType = new AccountDeptType();
                Store2.DataSource = accountDeptType.getType();
                Store2.DataBind();
                Store3.DataSource = boundcomm.getYears();
                Store3.DataBind();
                cbbYear.SetValue(DateTime.Now.Year);
                Store4.DataSource = boundcomm.getMonth();
                Store4.DataBind();
                cbbmonth.SetValue(DateTime.Now.Month);
                //读取科室类别
                DataTable dt = accountDeptType.getDeptType(DateTime.Now.Year.ToString(), DateTime.Now.Month.ToString());
                if (dt == null)
                {
                    Goldnet.Ext.Web.Ext.Msg.Show(new Goldnet.Ext.Web.MessageBox.Config
                   {
                       Title = "提示",
                       Message = "科室快照未设置,请联系管理员",
                       Buttons = MessageBox.Button.OK,
                       Icon = (MessageBox.Icon)Enum.Parse(typeof(MessageBox.Icon), "INFO")
                   });
                }
                else
                {
                    Store1.DataSource = dt;
                    Store1.DataBind();
                }

                //查找科室信息
                HttpProxy pro = new HttpProxy();
                pro.Method = HttpMethod.POST;
                pro.Url = "../../cbhs/WebService/BonusDepts.ashx";
                this.SDept.Proxy.Add(pro);
                JsonReader jr = new JsonReader();
                jr.ReaderID = "DEPT_CODE";
                jr.Root = "Bonusdepts";
                jr.TotalProperty = "totalCount";
                RecordField rf = new RecordField();
                rf.Name = "DEPT_CODE";
                jr.Fields.Add(rf);
                RecordField rfn = new RecordField();
                rfn.Name = "DEPT_NAME";
                jr.Fields.Add(rfn);
                this.SDept.Reader.Add(jr);
            }
        }

        /// <summary>
        /// 查询科室类别
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void CBBSelect_Query(object sender, AjaxEventArgs e)
        {
            AccountDeptType accountDeptType = new AccountDeptType();
            DataTable dt = accountDeptType.getDeptType(cbbYear.SelectedItem.Value.ToString(), cbbmonth.SelectedItem.Value.ToString());
            if (dt == null)
            {
                Store1.DataSource = accountDeptType.BuildDeptType();
                Store1.DataBind();
                Goldnet.Ext.Web.Ext.Msg.Show(new Goldnet.Ext.Web.MessageBox.Config
               {
                   Title = "提示",
                   Message = "科室快照未设置,请联系管理员",
                   Buttons = MessageBox.Button.OK,
                   Icon = (MessageBox.Icon)Enum.Parse(typeof(MessageBox.Icon), "INFO")
               });
            }
            else
            {
                Store1.DataSource = dt;
                Store1.DataBind();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Save(object sender, AjaxEventArgs e)
        {
            //定义一个HashTable,将前台编辑按钮所选中的行数据复制到定义的HashTable对象selectRow中            
            Dictionary<string, string>[] selectRow = GetSelectRow(e);
            if (selectRow != null)
            {
                AccountDeptType accountDeptType = new AccountDeptType();
                if (accountDeptType.SaveDeptType(selectRow, cbbYear.SelectedItem.Value.ToString(), cbbmonth.SelectedItem.Value.ToString()))
                {
                    Goldnet.Ext.Web.Ext.Msg.Show(new Goldnet.Ext.Web.MessageBox.Config
                    {
                        Title = "提示",
                        Message = "核算单位类别设置成功",
                        Buttons = MessageBox.Button.OK,
                        Icon = (MessageBox.Icon)Enum.Parse(typeof(MessageBox.Icon), "INFO")
                    });
                    //Store1.DataSource = accountDeptType.getDeptType(cbbYear.SelectedItem.Value.ToString(), cbbmonth.SelectedItem.Value.ToString());
                    //Store1.DataBind();
                }
                else
                {
                    ShowDataError("", Request.Url.LocalPath, "SaveDeptType");
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


    }
}
