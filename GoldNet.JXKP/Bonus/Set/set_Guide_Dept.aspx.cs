using System;
using System.Collections.Generic;
using Goldnet.Dal;
using Goldnet.Ext.Web;
using Goldnet.Dal.Properties.Bound;
using System.Data;

namespace GoldNet.JXKP.Bonus.Set
{
    public partial class set_Guide_Dept : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Ext.IsAjaxRequest)
            {
                BoundComm boundcomm = new BoundComm();

                bound_Guide_Group deptpercent = new bound_Guide_Group();
                Store3.DataSource = boundcomm.getYears();
                Store3.DataBind();
                cbbYear.SetValue(DateTime.Now.Year);
                Store4.DataSource = boundcomm.getMonth();
                Store4.DataBind();
                cbbmonth.SetValue(DateTime.Now.Month);

                DataTable prog = deptpercent.GetGuideGroup("").Tables[0];
                Store2.DataSource = prog;
                Store2.DataBind();

                if (boundcomm.GetAccountTypeByGroup(DateTime.Now.Year.ToString(), DateTime.Now.Month.ToString(), "01"))
                {
                    Store1.DataSource = deptpercent.GetGuideDept(DateTime.Now.Year.ToString(), DateTime.Now.Month.ToString());
                    Store1.DataBind();
                }
                else
                {
                    Goldnet.Ext.Web.Ext.Msg.Show(new Goldnet.Ext.Web.MessageBox.Config
                    {
                        Title = "提示",
                        Message = DateTime.Now.Year.ToString() + "年" + DateTime.Now.Month.ToString() + "月核算科室还未设置",
                        Buttons = MessageBox.Button.OK,
                        Icon = (MessageBox.Icon)Enum.Parse(typeof(MessageBox.Icon), "INFO")
                    });
                }

                
                
            }
        }

        /// <summary>
        /// 查找选择年月的科室核算属性设置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Query(object sender, AjaxEventArgs e)
        {
            BoundComm boundcomm = new BoundComm();
            if (boundcomm.GetAccountType(cbbYear.SelectedItem.Value.ToString(), cbbmonth.SelectedItem.Value.ToString(), "10001"))
            {
                bound_Guide_Group deptpercent = new bound_Guide_Group();
                Store1.DataSource = deptpercent.GetGuideDept(cbbYear.SelectedItem.Value.ToString(), cbbmonth.SelectedItem.Value.ToString());
                Store1.DataBind();

                //DataTable prog = deptpercent.GetGuideGroup("").Tables[0];
                //Store2.DataSource = prog;
                //Store2.DataBind();
            }
            else
            {

                Goldnet.Ext.Web.Ext.Msg.Show(new Goldnet.Ext.Web.MessageBox.Config
                {
                    Title = "提示",
                    Message = cbbYear.SelectedItem.Value.ToString() + "年" + cbbmonth.SelectedItem.Value.ToString() + "月核算科室还未设置",
                    Buttons = MessageBox.Button.OK,
                    Icon = (MessageBox.Icon)Enum.Parse(typeof(MessageBox.Icon), "INFO")
                });
            }
        }


        /// <summary>
        /// 保存按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Save(object sender, AjaxEventArgs e)
        {
            //定义一个HashTable,将前台编辑按钮所选中的行数据复制到定义的HashTable对象selectRow中            
            Dictionary<string, string>[] selectRow = GetSelectRow(e);
            if (selectRow != null)
            {
                bound_Guide_Group deptpercent = new bound_Guide_Group();
                if (deptpercent.SaveGuideDept(selectRow, cbbYear.SelectedItem.Value.ToString(), cbbmonth.SelectedItem.Value.ToString()))
                {
                    Goldnet.Ext.Web.Ext.Msg.Show(new Goldnet.Ext.Web.MessageBox.Config
                    {
                        Title = "提示",
                        Message = "设置成功",
                        Buttons = MessageBox.Button.OK,
                        Icon = (MessageBox.Icon)Enum.Parse(typeof(MessageBox.Icon), "INFO")
                    });
                    Store1.DataSource = deptpercent.GetGuideDept(cbbYear.SelectedItem.Value.ToString(), cbbmonth.SelectedItem.Value.ToString());
                    Store1.DataBind();
                }
                else
                {
                    //ShowDataError("", Request.Url.LocalPath, "SaveDeptPercent");
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
