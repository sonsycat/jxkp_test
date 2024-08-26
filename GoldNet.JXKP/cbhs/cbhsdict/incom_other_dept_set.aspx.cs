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
namespace GoldNet.JXKP.cbhs.cbhsdict
{
    public partial class incom_other_dept_set : PageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Ext.IsAjaxRequest)
            {
                SetDict();
                if (Request["item_code"] != null)
                {
                    SetValue(Request["item_code"].ToString());
                    this.ComboBox1.SelectedIndex = 0;
                    Goldnet.Dal.Appor_Prog_Dict prodal = new Goldnet.Dal.Appor_Prog_Dict();
                    this.Store2.DataSource = prodal.GetDeptByotherdeptitem(this.ComboBox_Role.SelectedItem.Value, "0", Request["other_flags"].ToString(), Request["other_id"].ToString()).Tables[0];
                    this.Store2.DataBind();
                    this.Store1.DataSource = prodal.GetNoCheckDeptByitem(this.ComboBox_Role.SelectedItem.Value, this.Combo_SelectDept.SelectedItem.Value, "0", Request["other_flags"].ToString(), Request["other_id"].ToString());
                    this.Store1.DataBind();
                }
            }
        }
        /// <summary>
        /// 下拉框设置
        /// </summary>
        public void SetDict()
        {
            Goldnet.Dal.SYS_DEPT_DICT dal = new Goldnet.Dal.SYS_DEPT_DICT();
            DataRow[] depttyperow = dal.GetDeptType().Tables[0].Select();
            foreach (DataRow rw in depttyperow)
            {
                this.Combo_SelectDept.Items.Add(new Goldnet.Ext.Web.ListItem(rw["ATTRIBUE"].ToString(), rw["ID"].ToString()));
            }
            Goldnet.Dal.Appor_Prog_Dict prodal = new Goldnet.Dal.Appor_Prog_Dict();
            DataRow[] rolerow = prodal.GetIncomitem().Tables[0].Select();
            foreach (DataRow row in rolerow)
            {
                this.ComboBox_Role.Items.Add(new Goldnet.Ext.Web.ListItem(row["ITEM_NAME"].ToString(), row["ITEM_CLASS"].ToString()));
            }

        }



        /// <summary>
        /// 保存科室
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void SubmitData(object sender, StoreSubmitDataEventArgs e)
        {
            if (this.ComboBox_Role.SelectedItem.Value.Equals(string.Empty))
            {
                this.ShowMessage("提示", "项目不能为空！");
            }
            else
            {
                List<PageModels.deptselected> deptlist = e.Object<PageModels.deptselected>();
                Goldnet.Dal.Appor_Prog_Dict dal = new Goldnet.Dal.Appor_Prog_Dict();
                DataTable numtable = dal.GetdeptPercent(this.ComboBox_Role.SelectedItem.Value, Request["other_id"].ToString());
                double numbers = 0;
                foreach (GoldNet.Model.PageModels.deptselected dept in deptlist)
                {
                    numbers+=Convert.ToDouble(dept.RATIO)/100;
                }
                if (numtable.Rows.Count > 0)
                {
                    if (this.ComboBox1.SelectedItem.Value == "0")//住院
                    {
                        numbers += Convert.ToDouble(numtable.Rows[0]["IN_NUMBERS"].ToString());
                    }
                    else
                        numbers += Convert.ToDouble(numtable.Rows[0]["OUT_NUMBERS"].ToString());
                }
                //if (Math.Round(numbers,4) != 1)
                //{
                //    this.ShowMessage("系统提示", "分配比例设置等于" + numbers*100 + ",不等于100，请检查！");
                //}
                //else
                //{
                //    try
                //    {
                       dal.SaveItemotherDept(deptlist, this.ComboBox_Role.SelectedItem.Value, this.ComboBox1.SelectedItem.Value, Request["other_flags"].ToString(), Request["other_id"].ToString());
                       this.SaveSucceed();
                //    }
                //    catch (Exception ex)
                //    {
                //        ShowDataError(ex, Request.Url.LocalPath, "SubmitData");

                //    }
                //}
            }
        }
        /// <summary>
        /// 选择科室
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void SelectedDept(object sender, AjaxEventArgs e)
        {
            Goldnet.Dal.Appor_Prog_Dict prodal = new Goldnet.Dal.Appor_Prog_Dict();
            this.Store1.DataSource = prodal.GetNoCheckDeptByitem(this.ComboBox_Role.SelectedItem.Value, this.Combo_SelectDept.SelectedItem.Value, this.ComboBox1.SelectedItem.Value, Request["other_flags"].ToString(), Request["other_id"].ToString());
            this.Store1.DataBind();
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="model"></param>
        public void SetValue(string progcode)
        {
            this.ComboBox_Role.SelectedItem.Value = progcode;

            Selectedrole(null, null);
        }

        protected void btnCancle_Click(object sender, EventArgs e)
        {
            Goldnet.Ext.Web.ScriptManager scManager = Goldnet.Ext.Web.ScriptManager.GetInstance(this.Page);
            //scManager.AddScript("parent.RefreshData();");
            scManager.AddScript("parent.DeptWin.hide();");
        }

        /// <summary>
        /// 分解方案项目
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Selectedrole(object sender, AjaxEventArgs e)
        {
            Goldnet.Dal.Appor_Prog_Dict prodal = new Goldnet.Dal.Appor_Prog_Dict();
            this.Store2.DataSource = prodal.GetDeptByotherdeptitem(this.ComboBox_Role.SelectedItem.Value, this.ComboBox1.SelectedItem.Value, Request["other_flags"].ToString(), Request["other_id"].ToString()).Tables[0];
            this.Store2.DataBind();
            SelectedDept(null, null);

        }
    }
}