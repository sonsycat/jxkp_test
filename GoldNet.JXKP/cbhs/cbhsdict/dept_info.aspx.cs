using System;
using System.Data;
using Goldnet.Dal;
using Goldnet.Ext.Web;
using System.Collections.Generic;
using GoldNet.Model;

namespace GoldNet.JXKP.cbhs.cbhsdict
{
    public partial class dept_info : PageBase
    {
        private BoundComm boundcomm = new BoundComm();
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Ext.IsAjaxRequest)
            {

                Store3.DataSource = boundcomm.getYears();
                Store3.DataBind();
                cbbYear.SetValue(DateTime.Now.Year);
                Store4.DataSource = boundcomm.getMonth();
                Store4.DataBind();
                cbbmonth.SetValue(DateTime.Now.Month);
                //---------------------------------
                Bindlist(DateTime.Now.Year.ToString(), DateTime.Now.Month.ToString());
            }
        }
        //查询科室信息表Dept_info
        protected void Bindlist(string year, string month)
        {
            Cbhs_dict dal = new Cbhs_dict();
            DataTable dt = dal.GetDeptInfo(year, month).Tables[0];
            if (dt.Rows.Count == 0)
            {
                Goldnet.Ext.Web.Ext.Msg.Show(new Goldnet.Ext.Web.MessageBox.Config
                {
                    Title = "提示",
                    Message = year + "年" + month + "月未设置科室快照",
                    Buttons = MessageBox.Button.OK,
                    Icon = (MessageBox.Icon)Enum.Parse(typeof(MessageBox.Icon), "INFO")
                });
            }
            this.Store1.DataSource = dt;
            this.Store1.DataBind();
        }
        //查询
        protected void Button_look_click(object sender, EventArgs e)
        {
            string year = cbbYear.SelectedItem.Value;
            string month = cbbmonth.SelectedItem.Value;
            Bindlist(year, month);
        }
        //保存
        protected void SubmitData(object sender, StoreSubmitDataEventArgs e)
        {
            List<DeptInfo> deptinfos = e.Object<DeptInfo>();
            Cbhs_dict dal = new Cbhs_dict();
            try
            {
                if (deptinfos == null || deptinfos.Count == 0)
                {
                    return;
                }
                string year = cbbYear.SelectedItem.Value;
                string month = cbbmonth.SelectedItem.Value;
                dal.SaveDeptInfo(deptinfos, year, month);
                Ext.Msg.Alert("提示", "保存成功").Show();
                Bindlist(year, month);
            }
            catch
            {
                Ext.Msg.Alert("提示", "保存失败").Show();
            }
        }
    }
}
