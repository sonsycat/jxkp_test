using System;
using System.Collections;
using System.Configuration;
using System.Data;
using Goldnet.Ext.Web;
using Goldnet.Dal;
using GoldNet.Comm;
using GoldNet.Model;

namespace GoldNet.JXKP.RLZY.BaseInfoMaintain
{
    public partial class SpecMediList : PageBase
    {
        BaseInfoMaintainDal dal = new BaseInfoMaintainDal();
        //1:具有审批权限的人登入,2:普通用户进入
        private static int PowerPageInfo = -1;
        private static string add_mark = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            //检查是否已经登录，否则停止
            if (Session["CURRENTSTAFF"] == null)
            {
                Response.End();
            }

            if (!Ext.IsAjaxRequest)
            {
                bool isPass = this.IsPass();
                bool isEdit = this.IsEdit();
                PowerPageInfo = isPass ? 1 : 2;
                this.hiddenEdit.Text = isEdit ? "1" : "2";
                if (PowerPageInfo == 1)
                {
                    if (!isEdit)
                    {
                        ScriptManager1.AddScript("#{btn_Delete}.hide();#{btn_Add}.hide();");
                        this.hiddenEdit.Text = "2";
                    }
                }
                else
                {
                    if (!isEdit)
                    {
                        ScriptManager1.AddScript("#{btn_Delete}.hide();#{btn_Add}.hide();#{btn_EchoHandle}.hide();#{cbxOpration}.hide();");
                    }
                }
                this.hiddenMeunUp.Text = isPass.ToString();
                this.PowerInfoHidden.Value = PowerPageInfo.ToString();

                string deptcode = this.DeptFilter("");
                HttpProxy proxy = new HttpProxy();
                proxy.Method = HttpMethod.POST;
                proxy.Url = "/RLZY/WebService/DeptInfo.ashx?deptfilter=" + deptcode;
                this.Store3.Proxy.Add(proxy);


                InitContrl();
                SetCombox();
                string year = "<="+"'" + System.DateTime.Now.Year+"'";
                add_mark = PowerPageInfo == 2 ? "1" : "0";
                this.Store1.DataSource = dal.ViewSpecMediList("", add_mark, deptcode, year).Tables[0];
                this.Store1.DataBind();
            }
        }

        /// <summary>
        /// 初始化Combox
        /// </summary>
        private void SetCombox() 
        {
            for (int i = 0; i < 10; i++)
            {
                int years = System.DateTime.Now.Year - i;
                this.cboTime.Items.Add(new Goldnet.Ext.Web.ListItem(years.ToString(), years.ToString()));
            }
            this.cboTime.SelectedIndex = 0;
            this.TimeOrgan.SelectedIndex = 0;

            DictMainTainDal dictdal = new DictMainTainDal();
            DataTable cboDt = null;

            //特殊诊疗项目
            cboDt = dictdal.getDictInfo("ESPE_DIAG_ITEM_DICT", "SERIAL_NO", "DIAG_NAME", false, "").Tables[0];
            SetCboData("SERIAL_NO", "DIAG_NAME", cboDt, cboSpeMediItem);
            cboSpeMediItem.SelectedIndex = 0;

            //患者身份字典
            cboDt = dictdal.getDictInfo("IDENTITYDICT", "SERIAL_NO", "IDENTITY_NAME", false, "").Tables[0];
            SetCboData("SERIAL_NO", "IDENTITY_NAME", cboDt, cboIdentity);
            cboIdentity.SelectedIndex = 0;
        }


        /// <summary>
        /// 初始化COMBOX控件
        /// </summary>
        /// <param name="ID">数据库ID名称</param>
        /// <param name="text">数据库NAME名称</param>
        /// <param name="dtSource">数据源</param>
        /// <param name="cbo">COMBOX控件</param>
        private void SetCboData(string ID, string text, DataTable dtSource, ComboBox cbo)
        {
            if (dtSource.Rows.Count < 1)
            {
                return;
            }
            for (int idx = 0; idx < dtSource.Rows.Count; idx++)
            {
                cbo.Items.Add(new Goldnet.Ext.Web.ListItem(dtSource.Rows[idx]["NAME"].ToString(), dtSource.Rows[idx]["ID"].ToString()));
            }
        }


        [AjaxMethod]
        public void SpecMediListAjaxOper(string Id, string StatMonth, string Name, string Ages, string InpNo, string Unit, string DiagDesc, string Armcar, string SpeMediItem, string Birth, string ETimes, string Sex, string Identity, string InsuranceNo, string OperType, string DeptCode)
        {
            switch (OperType)
            {
                case "1":
                    string mark = PowerPageInfo == 2 ? "3" : "1";
                    dal.InsertSpecMediList(StatMonth, Name, Ages, InpNo, Unit, DiagDesc, Armcar, SpeMediItem, Birth, ETimes, Sex, Identity, InsuranceNo, DeptCode, mark, DateTime.Now.ToString("yyyy-MM-dd"), ((User)Session["CURRENTSTAFF"]).StaffId);
                    break;
                case "2":
                    dal.UpdataSpecMediList(Id,Name, Ages, InpNo, Unit, DiagDesc, Armcar, SpeMediItem, Birth, ETimes, Sex, Identity, InsuranceNo, "0");
                    break;
                case "3":
                    dal.EchoDelSpecMediList(Id);
                    break;
                case "4":
                    dal.EchoUpdataSpecMediList(Id,"0");
                    break;
                case "5":
                    dal.UpdataSpecMediList(Id, Name, Ages, InpNo, Unit, DiagDesc, Armcar, SpeMediItem, Birth, ETimes, Sex, Identity, InsuranceNo, "1");
                    break;
                case "6":
                    dal.EchoUpdataSpecMediList(Id, "1");
                    break;
                case "7":
                    dal.UpdataSpecMediList(Id, Name, Ages, InpNo, Unit, DiagDesc, Armcar, SpeMediItem, Birth, ETimes, Sex, Identity, InsuranceNo, "3");
                    break;
                case "8":
                    dal.InsertSpecMediList(StatMonth, Name, Ages, InpNo, Unit, DiagDesc, Armcar, SpeMediItem, Birth, ETimes, Sex, Identity, InsuranceNo, DeptCode, "0", DateTime.Now.ToString("yyyy-MM-dd"), ((User)Session["CURRENTSTAFF"]).StaffId);
                    break;
            }
        }

        /// <summary>
        /// 刷新数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void Data_RefreshData(object sender, StoreRefreshDataEventArgs e)
        {
            InitAddMark();
            string year = this.TimeOrgan.SelectedItem.Value+"'" + this.cboTime.SelectedItem.Value+"'";
            this.Store1.DataSource = dal.ViewSpecMediList(this.DeptCodeCombo.SelectedItem.Value.ToString(), add_mark, this.DeptFilter(""),year).Tables[0];
            this.Store1.DataBind();
        }

        private void InitContrl() 
        {
            switch (PowerPageInfo) 
            {
                case 1:
                    this.cbxOpration.BoxLabel = "显示已审批";
                    this.btn_EchoHandle.Text = "批量审批";
                    break;
                case 2:
                    this.cbxOpration.BoxLabel = "显示未提交";
                    this.btn_EchoHandle.Text = "批量提交";
                    break;
            }        
        }

        /// <summary>
        /// 判断标价值
        /// </summary>
        private void InitAddMark() 
        {
            //string mark = PowerPageInfo == 2 ? "3" : "0";
            switch (PowerPageInfo)
            {
                case 1:
                    add_mark = (this.cbxOpration.Checked) ? "1" : "0";
                    break;
                case 2:
                    add_mark = (this.cbxOpration.Checked) ? "3" : "1";
                    break;
            } 
        }
    }
}
