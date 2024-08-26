using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using Goldnet.Ext.Web;
using Goldnet.Dal;
using GoldNet.Comm;
using GoldNet.Model;

namespace GoldNet.JXKP.RLZY.BaseInfoManager
{
    public partial class DevelopNewTechnic : PageBase
    {
        BaseInfoManagerDal dal = new BaseInfoManagerDal();
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
                        //ScriptManager1.AddScript("#{btn_Delete}.hide();#{btn_Add}.hide();#{btn_EchoHandle}.hide();");
                        //以个人身份登陆时，可以录入及删除自己录入的科目   liu.shh  2012.12.18
                        ScriptManager1.AddScript("#{btn_EchoHandle}.hide();");
                    }
                }

                this.hiddenMeunUp.Text = isPass.ToString();
                this.PowerInfoHidden.Value = PowerPageInfo.ToString();

                string deptCode = this.DeptFilter("");
                //liu.shh  2012.12.18
                if (deptCode.Equals("'-1'") && !this.IsEdit() == true && !this.IsPass() == true)
                {
                    deptCode = this.DeptCode();
                    PowerInfoHidden.Value = 0; //个人身登陆，只能添加、删除、修改、不能提交
                }
                HttpProxy proxy = new HttpProxy();
                proxy.Method = HttpMethod.POST;
                proxy.Url = "/RLZY/WebService/DeptInfo.ashx?deptfilter=" + deptCode;
                this.Store3.Proxy.Add(proxy);

                InitContrl();
                SetCombox();
                add_mark = PowerPageInfo == 2 ? "1" : "0";
                //如果以个人身份登陆，只能查看自己  liu.shh 2012.12.18
                if (!this.IsEdit() == true && !this.IsPass() == true)
                {
                    this.Store1.DataSource = dal.ViewDevelopNewTecListPerson("<='" + DateTime.Now.Year.ToString() + "'", add_mark, "", deptCode, ((User)Session["CURRENTSTAFF"]).UserId);
                }
                else
                {
                    this.Store1.DataSource = dal.ViewDevelopNewTecList("<='" + DateTime.Now.Year.ToString() + "'", add_mark, "", deptCode);
                }
                this.Store1.DataBind();
            }
        }

        /// <summary>
        /// 初始化Combox
        /// </summary>
        private void SetCombox()
        {
            //开始时间
            for (int i = 0; i < 10; i++)
            {
                int years = System.DateTime.Now.Year - i;
                this.cboTime.Items.Add(new Goldnet.Ext.Web.ListItem(years.ToString(), years.ToString()));
            }
            this.cboTime.SelectedIndex = 0;
            this.TimeOrgan.SelectedIndex = 0;

            DictMainTainDal dictdal = new DictMainTainDal();
            DataTable cboDt = null;

            //加载新技术新业务字典
            cboDt = dictdal.getDictInfo("NEW_TECHNIC_DICT", "ID", "NEW_TECHNIC", true, "IS_DEL").Tables[0];
            SetCboData("ID", "NEW_TECHNIC", cboDt, cboNewTechnic);
            cboNewTechnic.SelectedIndex = 0;

            //加载职称字典
            cboDt = dictdal.getDictInfo("JOB_DICT", "ID", "JOB", true, "IS_DEL").Tables[0];
            SetCboData("ID", "JOB", cboDt, cboPrincipalJob);
            cboPrincipalJob.SelectedIndex = 0;

            //加载学历字典
            cboDt = dictdal.getDictInfo("LEARNSUFFER_DICT", "ID", "LEARNSUFFER", true, "IS_DEL").Tables[0];
            SetCboData("SERIAL_NO", "IDENTITY_NAME", cboDt, cboPrincipalSchoolAge);
            cboPrincipalSchoolAge.SelectedIndex = 0;


            //加载水平字典
            cboDt = dictdal.getDictInfo("LEVEL_DICT", "SERIAL_NO", "LEVEL_NAME", false, "").Tables[0];
            SetCboData("SERIAL_NO", "LEVEL_NAME", cboDt, cboLevelCol);
            cboLevelCol.SelectedIndex = 0;


            //效果字典
            cboDt = dictdal.getDictInfo("EFFE_DICT", "SERIAL_NO", "EFFE_NAME", false, "").Tables[0];
            SetCboData("SERIAL_NO", "EFFE_NAME", cboDt, cboEffect);
            cboEffect.SelectedIndex = 0;

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
        public void FruitListAjaxOper(string Id, string optype)
        {
            switch (optype)
            {
                case "3":
                    dal.EchoDeleteHandle("DEVELOP_NEW_TECHNIC", "ID", Id);
                    break;
                case "4":
                    dal.EchoUpDataHandle("DEVELOP_NEW_TECHNIC", "ADD_MARK", "ID", Id, "0");
                    break;
                case "6":
                    dal.EchoUpDataHandle("DEVELOP_NEW_TECHNIC", "ADD_MARK", "ID", Id, "1");
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
            //如果以个人身份登陆，只能查看自己  liu.shh 2012.12.18
            if (!this.IsEdit() == true && !this.IsPass() == true)
            {
                this.Store1.DataSource = dal.ViewDevelopNewTecListPerson(this.TimeOrgan.SelectedItem.Value + "'" + DateTime.Now.Year.ToString() + "'", add_mark, this.DeptCodeCombo.SelectedItem.Value, this.DeptCode(), ((User)Session["CURRENTSTAFF"]).UserId);
            }
            else
            {
                this.Store1.DataSource = dal.ViewDevelopNewTecList(this.TimeOrgan.SelectedItem.Value + "'" + DateTime.Now.Year.ToString() + "'", add_mark, this.DeptCodeCombo.SelectedItem.Value, this.DeptFilter(""));
            }
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

        /// <summary>
        /// 保存按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void SaveInfo(object sender, AjaxEventArgs e)
        {
            string mark = PowerPageInfo == 2 ? "3" : "1";
            string Creater = ((User)Session["CURRENTSTAFF"]).UserId == null ? "NotUserId" : ((User)Session["CURRENTSTAFF"]).UserId;
            dal.InsertDevelopNewTechnic(this.DeptCodeCombo.SelectedItem.Value, this.DeptCodeCombo.SelectedItem.Text, this.cboNewTechnic.SelectedItem.Text, this.txtName.Text,
                this.cboPrincipal.SelectedItem.Text, this.cboPrincipalSchoolAge.SelectedItem.Text, this.cboPrincipalJob.SelectedItem.Text, this.txrJoinPersons.Text,
                this.dtfDates.SelectedDate.ToString("yyyy-MM-dd"), this.txtBrief.Text, this.txtClubDept.Text, this.txtCompCase.Text, this.cboLevelCol.SelectedItem.Text,
                this.cboEffect.SelectedItem.Text, DateTime.Now.ToString("yyyy-MM-dd"), Creater, mark, this.cboPrincipal.SelectedItem.Value, ((User)Session["CURRENTSTAFF"]).UserId);

            Goldnet.Ext.Web.ScriptManager scManager = Goldnet.Ext.Web.ScriptManager.GetInstance(this.Page);
            scManager.AddScript("Store1.reload();");
            scManager.AddScript("btn_EchoHandle.setDisabled(true);");
            scManager.AddScript("btn_Delete.setDisabled(true);");
            scManager.AddScript("arcEditWindow.hide();");
        }

        /// <summary>
        /// 提交按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void SetUpInfo(object sender, AjaxEventArgs e)
        {
            string deptcode = e.ExtraParams["deptCode"].ToString();
            string deptName = e.ExtraParams["deptName"].ToString();
            dal.UpdateDevelopNewTechnic(this.HiddenId.Text, deptcode, deptName, this.cboNewTechnic.SelectedItem.Text, this.txtName.Text,
                                        this.cboPrincipal.SelectedItem.Text, this.cboPrincipalSchoolAge.SelectedItem.Text, this.cboPrincipalJob.SelectedItem.Text, this.txrJoinPersons.Text,
                                        this.dtfDates.SelectedDate.ToString("yyyy-MM-dd"), this.txtBrief.Text, this.txtClubDept.Text, this.txtCompCase.Text, this.cboLevelCol.SelectedItem.Text,
                                        this.cboEffect.SelectedItem.Text, "0", this.cboPrincipal.SelectedItem.Value, this.txrSug.Text, this.txrSetSug.Text);
            Goldnet.Ext.Web.ScriptManager scManager = Goldnet.Ext.Web.ScriptManager.GetInstance(this.Page);
            scManager.AddScript("Store1.reload();");
            scManager.AddScript("btn_EchoHandle.setDisabled(true);");
            scManager.AddScript("btn_Delete.setDisabled(true);");
            scManager.AddScript("arcEditWindow.hide();");
        }

        /// <summary>
        /// 审批按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ApproveInfo(object sender, AjaxEventArgs e)
        {
            string deptcode = e.ExtraParams["deptCode"].ToString();
            string deptName = e.ExtraParams["deptName"].ToString();
            dal.UpdateDevelopNewTechnic(this.HiddenId.Text, deptcode, deptName, this.cboNewTechnic.SelectedItem.Text, this.txtName.Text,
                             this.cboPrincipal.SelectedItem.Text, this.cboPrincipalSchoolAge.SelectedItem.Text, this.cboPrincipalJob.SelectedItem.Text, this.txrJoinPersons.Text,
                             this.dtfDates.SelectedDate.ToString("yyyy-MM-dd"), this.txtBrief.Text, this.txtClubDept.Text, this.txtCompCase.Text, this.cboLevelCol.SelectedItem.Text,
                             this.cboEffect.SelectedItem.Text, "1", this.cboPrincipal.SelectedItem.Value, this.txrSug.Text, this.txrSetSug.Text);
            Goldnet.Ext.Web.ScriptManager scManager = Goldnet.Ext.Web.ScriptManager.GetInstance(this.Page);
            scManager.AddScript("Store1.reload();");
            scManager.AddScript("btn_EchoHandle.setDisabled(true);");
            scManager.AddScript("btn_Delete.setDisabled(true);");
            scManager.AddScript("arcEditWindow.hide();");
        }

        /// <summary>
        /// 审批不通过按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void NotApproveInfo(object sender, AjaxEventArgs e)
        {
            string deptcode = e.ExtraParams["deptCode"].ToString();
            string deptName = e.ExtraParams["deptName"].ToString();
            dal.UpdateDevelopNewTechnic(this.HiddenId.Text, deptcode, deptName, this.cboNewTechnic.SelectedItem.Text, this.txtName.Text,
                             this.cboPrincipal.SelectedItem.Text, this.cboPrincipalSchoolAge.SelectedItem.Text, this.cboPrincipalJob.SelectedItem.Text, this.txrJoinPersons.Text,
                             this.dtfDates.SelectedDate.ToString("yyyy-MM-dd"), this.txtBrief.Text, this.txtClubDept.Text, this.txtCompCase.Text, this.cboLevelCol.SelectedItem.Text,
                             this.cboEffect.SelectedItem.Text, "-1", this.cboPrincipal.SelectedItem.Value, this.txrSug.Text, this.txrSetSug.Text);
            Goldnet.Ext.Web.ScriptManager scManager = Goldnet.Ext.Web.ScriptManager.GetInstance(this.Page);
            scManager.AddScript("Store1.reload();");
            scManager.AddScript("btn_EchoHandle.setDisabled(true);");
            scManager.AddScript("btn_Delete.setDisabled(true);");
            scManager.AddScript("arcEditWindow.hide();");
        }



        /// <summary>
        /// 未提交时保存
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void SetSave(object sender, AjaxEventArgs e)
        {
            string deptcode = e.ExtraParams["deptCode"].ToString();
            string deptName = e.ExtraParams["deptName"].ToString();
            dal.UpdateDevelopNewTechnic(this.HiddenId.Text, deptcode, deptName, this.cboNewTechnic.SelectedItem.Text, this.txtName.Text,
                            this.cboPrincipal.SelectedItem.Text, this.cboPrincipalSchoolAge.SelectedItem.Text, this.cboPrincipalJob.SelectedItem.Text, this.txrJoinPersons.Text,
                            this.dtfDates.SelectedDate.ToString("yyyy-MM-dd"), this.txtBrief.Text, this.txtClubDept.Text, this.txtCompCase.Text, this.cboLevelCol.SelectedItem.Text,
                            this.cboEffect.SelectedItem.Text, "3", this.cboPrincipal.SelectedItem.Value, this.txrSug.Text, this.txrSetSug.Text);
            Goldnet.Ext.Web.ScriptManager scManager = Goldnet.Ext.Web.ScriptManager.GetInstance(this.Page);
            scManager.AddScript("Store1.reload();");
            scManager.AddScript("btn_EchoHandle.setDisabled(true);");
            scManager.AddScript("btn_Delete.setDisabled(true);");
            scManager.AddScript("arcEditWindow.hide();");
        }

        /// <summary>
        /// 添加时直接提交
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void SaveSet(object sender, AjaxEventArgs e)
        {
            string Creater = ((User)Session["CURRENTSTAFF"]).UserId == null ? "NotUserId" : ((User)Session["CURRENTSTAFF"]).UserId;
            dal.InsertDevelopNewTechnic(this.DeptCodeCombo.SelectedItem.Value, this.DeptCodeCombo.SelectedItem.Text, this.cboNewTechnic.SelectedItem.Text, this.txtName.Text,
               this.cboPrincipal.SelectedItem.Text, this.cboPrincipalSchoolAge.SelectedItem.Text, this.cboPrincipalJob.SelectedItem.Text, this.txrJoinPersons.Text,
               this.dtfDates.SelectedDate.ToString("yyyy-MM-dd"), this.txtBrief.Text, this.txtClubDept.Text, this.txtCompCase.Text, this.cboLevelCol.SelectedItem.Text,
               this.cboEffect.SelectedItem.Text, DateTime.Now.ToString("yyyy-MM-dd"), Creater, "0", this.cboPrincipal.SelectedItem.Value, ((User)Session["CURRENTSTAFF"]).UserId);
            Goldnet.Ext.Web.ScriptManager scManager = Goldnet.Ext.Web.ScriptManager.GetInstance(this.Page);
            scManager.AddScript("Store1.reload();");
            scManager.AddScript("btn_EchoHandle.setDisabled(true);");
            scManager.AddScript("btn_Delete.setDisabled(true);");
            scManager.AddScript("arcEditWindow.hide();");
        }



        private ArrayList ContrlJuge()
        {
            ArrayList l_ar = new ArrayList();
            return l_ar;
        }
    }
}
