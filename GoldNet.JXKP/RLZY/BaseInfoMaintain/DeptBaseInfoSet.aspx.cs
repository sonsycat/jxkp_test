using System;
using System.Collections;
using System.Configuration;
using System.Data;
using Goldnet.Ext.Web;
using Goldnet.Dal;

namespace GoldNet.JXKP.RLZY.BaseInfoMaintain
{
    public partial class DeptBaseInfoSet : PageBase
    {
        BaseInfoMaintainDal dal = new BaseInfoMaintainDal();
        protected void Page_Load(object sender, EventArgs e)
        {
            //检查是否已经登录，否则停止
            if (Session["CURRENTSTAFF"] == null)
            {
                Response.End();
            }
            if (!Ext.IsAjaxRequest)
            {
                if (!this.IsEdit())
                {
                    ScriptManager1.AddScript("#{btnModifyDeptInfo}.hide();");
                }

                HttpProxy proxy = new HttpProxy();
                proxy.Method = HttpMethod.POST;
                proxy.Url = "/RLZY/WebService/DeptInfo.ashx?deptfilter=" + this.DeptFilter("");
                this.Store3.Proxy.Add(proxy);

                this.btnModifyDeptInfo.Disabled = true;
                SetComboxValue();
            }
        }

        private void SetComboxValue() 
        {
            DictMainTainDal dictdal = new DictMainTainDal();
            DataTable cboDt = null;

            // 科室专业
            cboDt = dictdal.getDictInfo("PERS_SPEC_SORT_DICT", "SERIAL_NO", "SPEC_SORT_NAME", false, "").Tables[0];
            SetCboData("SERIAL_NO", "SPEC_SORT_NAME", cboDt, cboDeptSpeciality);
            cboDeptSpeciality.SelectedIndex = 0;

            // 门诊住院标识
            cboDt = dictdal.getDictInfo("OUTP_OR_INP", "ID", "ATTRIBUE", false, "").Tables[0];
            SetCboData("ID", "ATTRIBUE", cboDt, cboOutpOrInp);
            cboOutpOrInp.SelectedIndex = 0;

            // 所属专科中心
            cboTraining.Items.Add(new Goldnet.Ext.Web.ListItem("", ""));
            cboDt = dictdal.getDictInfo("SPEC_CENTER_DICT", "CENTER_CODE", "CENTER_NAME", false, "").Tables[0];
            SetCboData("CENTER_CODE", "CENTER_NAME", cboDt, cboTraining);

            // 临床科室属性
            cboDt = dictdal.getDictInfo("LCDEPT_ATTRIBUTE_DICT", "ID", "ATTRIBUE", false, "").Tables[0];
            SetCboData("ID", "ATTRIBUE", cboDt, cboDeptAttrib);
            cboDeptAttrib.SelectedIndex = 0;

            // 内外科标识
            cboTernalOrSergery.Items.Add(new Goldnet.Ext.Web.ListItem("", ""));
            cboDt = dictdal.getDictInfo("INTERNAL_OR_SERGERY", "ID", "ATTRIBUE", false, "").Tables[0];
            SetCboData("ID", "ATTRIBUE", cboDt, cboTernalOrSergery);

            // 所属专科中心类别
            cboTrainingType.Items.Add(new Goldnet.Ext.Web.ListItem("", ""));
            cboDt = dictdal.getDictInfo("DEPT_SORT_DICT", "SEID", "SORT_NAME", false, "").Tables[0];
            SetCboData("SEID", "SORT_NAME", cboDt, cboTrainingType);


            // 科室属性
            cboDt = dictdal.getDictInfo("DEPT_ATTRIBUTE_DICT", "ID", "ATTRIBUE", false, "").Tables[0];
            SetCboData("ID", "ATTRIBUE", cboDt, cboDeptattr);
            cboDeptattr.SelectedIndex = 0;
        }

        /// <summary>
        /// 插入科室信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void InsertInfo(object sender, AjaxEventArgs e)
        {
            if (this.DeptCodeCombo.SelectedItem.Value == "")
            {
                Goldnet.Ext.Web.Ext.Msg.Show(new Goldnet.Ext.Web.MessageBox.Config
                {
                    Title = "信息提示",
                    Message = "没有符合条件的部门,请重新选择条件",
                    Buttons = MessageBox.Button.OK,
                    Icon = (MessageBox.Icon)Enum.Parse(typeof(MessageBox.Icon), "INFO")
                });
                return;
            }
            if (this.txtDeptDirector.Text == "")
            {
                Goldnet.Ext.Web.Ext.Msg.Show(new Goldnet.Ext.Web.MessageBox.Config
                {
                    Title = "信息提示",
                    Message = "请填写科主任(负责人)",
                    Buttons = MessageBox.Button.OK,
                    Icon = (MessageBox.Icon)Enum.Parse(typeof(MessageBox.Icon), "INFO")
                });
                return;
            }
            InsertDeptData();
            Goldnet.Ext.Web.Ext.Msg.Show(new Goldnet.Ext.Web.MessageBox.Config
            {
                Title = "信息提示",
                Message = "保存成功！",
                Buttons = MessageBox.Button.OK,
                Icon = (MessageBox.Icon)Enum.Parse(typeof(MessageBox.Icon), "INFO")
            });
        }

        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void QueryDept(object sender, AjaxEventArgs e)
        {
                this.btnModifyDeptInfo.Disabled = false;
                SelectDeptData();
        }

        #region --逻辑--

        /// <summary>
        /// 读取数据后页面设置
        /// </summary>
        private void SelectDeptData()
        {
            // 读取科室信息数据
            DataTable detpDt = dal.getDeptInfoByDeptCode(this.DeptCodeCombo.SelectedItem.Value.ToString()).Tables[0];

            // 科室数据不存在
            if (detpDt.Rows.Count < 1)
            {
                InitData();

                return;
            }

            // 设定科室数据
            DataRow row = detpDt.Rows[0];

            // 科主任
            txtDeptDirector.Text = row["DIRECTOR"].ToString();

            // 编制床位
            txtWeaveBed.Text = row["WEAVE_BED"].ToString();

            // 护士长
            txtChargeNurse.Text = row["CHARGE_NURSE"].ToString();

            // 干部床
            txtShouleNum.Text = row["SHOULE_NUM"].ToString();

            // 实有人数
            txtShouldNum.Text = row["SHOULD_NUM"].ToString();

            // 编制管理人数
            txtApproManager.Text = row["APPRO_MANAGER"].ToString();

            // 副主任
            txtViceDirector.Text = row["SUBDIRECOTR"].ToString();

            // 展开床位
            txtDeployBed.Text = row["DEPLOY_BED"].ToString();

            // 科室专业
            cboDeptSpeciality.Value = row["SPES_SORT_ID"].ToString();

            // 门诊住院标识
            cboOutpOrInp.Value = row["OUTP_OR_INP"].ToString();

            // 编制医疗专业人数
            txtApproDoctor.Text = row["APPRO_DOCTOR"].ToString();

            // 编制医技专业人数
            txtApproTech.Text = row["APPRO_TECH"].ToString();

            // 所属专科中心
            cboTraining.Value = row["CENTER_ID"].ToString();

            // 临床科室属性
            cboDeptAttrib.Value = row["DEPT_ATTR"].ToString();

            // 专业负责人
            txtSpesManager.Text = row["PRINCIPAL"].ToString();

            // 内外科标识
            cboTernalOrSergery.Value = row["INTERNAL_OR_SERGERY"].ToString();

            // 编制药剂专业人数
            txtApproDrug.Text = row["APPRO_DRUG"].ToString();

            // 编制工程专业人数
            txtApproProject.Text = row["APPRO_PROJECT"].ToString();

            // 所属专科中心类别
            cboTrainingType.Value = row["CENTER_SORT_ID"].ToString();

            // 重点科室
            cboPivotDept.Value = row["IS_PIVOT_DEPT"].ToString();

            // 科室属性
            cboDeptattr.Value = row["DEPT_ATTR"].ToString();

            // 编制人数
            txtApproNum.Text = row["APPRO_NUM"].ToString();

            // 编制护理专业人数
            txtApproNurse.Text = row["APPRO_NURSE"].ToString();

            // 编制其它专业人数
            txtApproOther.Text = row["APPRO_OTHER"].ToString();


        }

        /// <summary>
        /// 页面数据初始化
        /// </summary>
        private void InitData()
        {
            // 科主任
            txtDeptDirector.Text = "";

            // 编制床位
            txtWeaveBed.Text = "0";

            // 护士长
            txtChargeNurse.Text = "";

            // 干部床
            txtShouleNum.Text = "0";

            // 实有人数
            txtShouldNum.Text = "0";

            // 编制管理人数
            txtApproManager.Text = "0";

            // 副主任
            txtViceDirector.Text = "";

            // 展开床位
            txtDeployBed.Text = "0";

            // 科室专业
            cboDeptSpeciality.SelectedIndex = 0;

            // 门诊住院标识
            cboOutpOrInp.SelectedIndex = 0;

            // 编制医疗专业人数
            txtApproDoctor.Text = "0";

            // 编制医技专业人数
            txtApproTech.Text = "0";

            // 所属专科中心
            cboTraining.SelectedIndex = 0;

            // 临床科室属性
            cboDeptAttrib.SelectedIndex = 0;

            // 专业负责人
            txtSpesManager.Text = "";

            // 内外科标识
            cboTernalOrSergery.SelectedIndex = 0;

            // 编制药剂专业人数
            txtApproDrug.Text = "0";

            // 编制工程专业人数
            txtApproProject.Text = "0";

            // 所属专科中心类别
            cboTrainingType.SelectedIndex = 0;

            // 重点科室
            cboPivotDept.SelectedIndex = 0;

            // 科室属性
            cboDeptattr.SelectedIndex = 0;

            // 编制人数
            txtApproNum.Text = "0";

            // 编制护理专业人数
            txtApproNurse.Text = "0";

            // 编制其它专业人数
            txtApproOther.Text = "0";

        }

        private  void SetCboData(string ID, string text, DataTable dtSource, ComboBox cbo)
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

        /// <summary>
        /// 数据库更新
        /// </summary>
        public void InsertDeptData()
        {
            // 读取科室信息数据
            String[] updateData = new String[26];

            // 科主任
            updateData[0] = txtDeptDirector.Text;

            // 编制床位
            updateData[1] = txtWeaveBed.Text;

            // 护士长
            updateData[2] = txtChargeNurse.Text;

            // 干部床
            updateData[3] = txtShouleNum.Text;

            // 实有人数
            updateData[4] = txtShouldNum.Text;

            // 编制管理人数
            updateData[5] = txtApproManager.Text;

            // 副主任
            updateData[6] = txtViceDirector.Text;

            // 展开床位
            updateData[7] = txtDeployBed.Text;

            // 科室专业
            updateData[8] = cboDeptSpeciality.SelectedItem.Value.ToString();

            // 门诊住院标识
            updateData[9] = cboOutpOrInp.SelectedItem.Value.ToString();

            // 编制医疗专业人数
            updateData[10] = txtApproDoctor.Text;

            // 编制医技专业人数
            updateData[11] = txtApproTech.Text;

            // 所属专科中心
            updateData[12] = cboTraining.SelectedItem.Value.ToString();

            // 临床科室属性
            updateData[13] = cboDeptAttrib.SelectedItem.Value.ToString();

            // 专业负责人
            updateData[14] = txtSpesManager.Text;

            // 内外科标识
            updateData[15] = cboTernalOrSergery.SelectedItem.Value.ToString();

            // 编制药剂专业人数
            updateData[16] = txtApproDrug.Text;

            // 编制工程专业人数
            updateData[17] = txtApproProject.Text;

            // 所属专科中心类别
            updateData[18] = cboTrainingType.SelectedItem.Value.ToString();

            // 重点科室
            updateData[19] = cboPivotDept.SelectedItem.Value.ToString();

            // 科室属性
            updateData[20] = cboDeptattr.SelectedItem.Value.ToString();

            // 编制人数
            updateData[21] = txtApproNum.Text;

            // 编制护理专业人数
            updateData[22] = txtApproNurse.Text;

            // 编制其它专业人数
            updateData[23] = txtApproOther.Text;

            // 科室CODE
            updateData[24] = DeptCodeCombo.SelectedItem.Value.ToString();

            //科室名称
            updateData[25] = DeptCodeCombo.SelectedItem.Text.ToString();

            dal.InsertDeptInfo(updateData);
        }

#endregion
    }
}
