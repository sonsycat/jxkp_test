using System;
using System.Collections.Generic;
using Goldnet.Ext.Web;
using System.Data;

namespace GoldNet.JXKP.jxkh
{
    public partial class Guide_Edit : System.Web.UI.Page
    {
        private Goldnet.Dal.Guide_Dict dal = new Goldnet.Dal.Guide_Dict();

        /// <summary>
        /// 初始化处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Ext.IsAjaxRequest)
            {
                //检查是否已经登录，否则停止
                if (Session["CURRENTSTAFF"] == null)
                {
                    Response.End();
                    return;
                }
                SetInitState(Session["SelectedGuide"]);
            }
        }

        //设置页面各控件状态
        private void SetInitState(object selectedRow)
        {
            //部门下拉列表
            DataTable dt = dal.GetDeptType().Tables[0];
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                this.DeptTypeComb.Items.Insert(i, new Goldnet.Ext.Web.ListItem(dt.Rows[i]["DEPT_CLASS_NAME"].ToString(), dt.Rows[i]["DEPT_CLASS_CODE"].ToString()));
            }

            //组织下拉列表
            dt = dal.Getorg().Tables[0];
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                this.OrganComb.Items.Insert(i, new Goldnet.Ext.Web.ListItem(dt.Rows[i]["ORGAN_CLASS_NAME"].ToString(), dt.Rows[i]["ORGAN_CLASS_CODE"].ToString()));
                this.OrganTypeComb.Items.Insert(i, new Goldnet.Ext.Web.ListItem(dt.Rows[i]["ORGAN_CLASS_NAME"].ToString(), dt.Rows[i]["ORGAN_CLASS_CODE"].ToString()));
            }
            //BSC1列表
            dt = dal.GetBSCOne().Tables[0];
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                this.BSC1Comb.Items.Insert(i, new Goldnet.Ext.Web.ListItem(dt.Rows[i]["BSC_CLASS_NAME"].ToString(), dt.Rows[i]["BSC_CLASS_CODE"].ToString()));
            }
            //BSC2列表
            string bsc1 = (selectedRow == null ? "01" : ((Dictionary<string, string>)selectedRow)["BSC"].Substring(0, 2));
            dt = dal.GetBSTwo(bsc1).Tables[0];
            this.BSCStore.DataSource = dt;
            this.BSCStore.DataBind();

            //适用年份下拉
            this.EvaluationYearComb.Items.Add(new Goldnet.Ext.Web.ListItem("通用", "all"));
            for (int i = 0; i < 10; i++)
            {
                int years = System.DateTime.Now.Year - i;
                this.EvaluationYearComb.Items.Add(new Goldnet.Ext.Web.ListItem(years.ToString(), years.ToString()));
            }

            //获取关联指标集的类别，年份，组织信息
            string guidegathercode = (selectedRow == null ? "" : (((Dictionary<string, string>)selectedRow)["GUIDE_GATHER_CODE"] == null ? "" : ((Dictionary<string, string>)selectedRow)["GUIDE_GATHER_CODE"]));
            string gathertype = "";
            string gatherorgan = "";
            string gatheryear = "";
            if (!guidegathercode.Equals(""))
            {
                dt = dal.GetGuideGatherInfo(guidegathercode).Tables[0];
                if (dt.Rows.Count > 0)
                {
                    gathertype = dt.Rows[0]["GUIDE_ATTR"].ToString();
                    gatherorgan = dt.Rows[0]["ORGAN_CLASS"].ToString();
                    gatheryear = dt.Rows[0]["EVALUATION_YEAR"].ToString();
                }
                else
                {
                    guidegathercode = "";
                }
            }
            gathertype = (gathertype.Equals("") ? "0101" : gathertype);
            gatherorgan = (gatherorgan.Equals("") ? "01" : gatherorgan);
            gatheryear = (gatheryear.Equals("") ? "all" : gatheryear);

            //指标集大类别下拉列表
            dt = dal.GetGuidetype().Tables[0];
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                this.GatherTypeComb.Items.Insert(i, new Goldnet.Ext.Web.ListItem(dt.Rows[i]["GUIDE_GROUP_TYPE_NAME"].ToString(), dt.Rows[i]["GUIDE_GROUP_TYPE"].ToString()));
            }

            //指标集小类别下拉列表
            dt = dal.GetGuideGroupType(gathertype.Substring(0, 2)).Tables[0];
            this.GuideClassStore.DataSource = dt;
            this.GuideClassStore.DataBind();

            //指标集下拉列表
            dt = dal.GetGuideGather(gathertype, gatherorgan, gatheryear).Tables[0];
            this.GuideGatherStore.DataSource = dt;
            this.GuideGatherStore.DataBind();

            //初始化各项目的值
            if (selectedRow == null)
            {
                this.DeptTypeComb.SelectedIndex = 0;
                this.OrganComb.SelectedIndex = 0;
                this.OrganTypeComb.SelectedIndex = 0;
                this.BSC1Comb.SelectedIndex = 0;
                this.BSC2Comb.SelectedIndex = 0;
                this.GuideCodeTxt.Value = dal.GetNextGuideCode("10101");
                this.IsExpressComb.SelectedIndex = 1;
                this.IsPageComb.SelectedIndex = 0;
                this.IsHighComb.SelectedIndex = 0;
                this.IsZhpjComb.SelectedIndex = 0;
                this.IsABSComb.SelectedIndex = 0;
                this.fixnum.SelectedIndex = 0;
            }
            else
            {
                Dictionary<string, string> row = (Dictionary<string, string>)selectedRow;
                this.DeptTypeComb.Value = row["DEPT"];
                this.OrganComb.Value = row["ORGAN"];
                this.BSC1Comb.Value = row["BSC"].Substring(0, 2);
                this.BSC2Comb.Value = row["BSC"];
                this.GuideCodeTxt.Value = row["GUIDE_CODE"];
                this.GuideNameTxt.Value = row["GUIDE_NAME"];
                this.IsExpressComb.Value = row["ISEXPRESS"].Equals("是") ? "1" : "0";
                this.GuideExpressTxt.Value = row["GUIDE_EXPRESS"];
                this.GuideExpressTxt.Disabled = row["ISEXPRESS"].Equals("是") ? false : true;
                this.IsPageComb.Value = row["ISPAGE"].Equals("停用") ? "0" : (row["ISPAGE"].Equals("启用") ? "1" : "2");
                this.IsHighComb.Value = row["ISHIGHGUIDE"].Equals("是") ? "1" : (row["ISHIGHGUIDE"].Equals("否") ? "0" : "2");
                this.IsZhpjComb.Value = row["ISSEL"].Equals("是") ? "1" : "0";
                this.IsABSComb.Value = row["ISABS"].Equals("是") ? "1" : "0";
                this.GuideCodeOriginal.Value = row["GUIDE_CODE"];
                this.CorrelationCHK.Checked = guidegathercode.Equals("") ? false : true;
                this.THRESHOLD_RATIO.Value = Convert.ToDouble(row["THRESHOLD_RATIO"].ToString());
                this.fixnum.Value = row["FIXNUM"].Equals("是") ? "1" : "0";
                this.TextAreaexplain.Value = row["EXPLAIN"];
                this.NumberField1.Value = Convert.ToDouble(row["SORT_NO"].ToString());

                if (!guidegathercode.Equals(""))
                {
                    this.EvaluationYearComb.Disabled = false;
                    this.GatherTypeComb.Disabled = false;
                    this.GatherClassComb.Disabled = false;
                    this.OrganTypeComb.Disabled = false;
                    this.GuideGatherCombo.Disabled = false;
                }
            }

            if (guidegathercode.Equals(""))
            {
                this.OrganTypeComb.SelectedIndex = 0;
                this.GatherTypeComb.SelectedIndex = 0;
                this.GatherClassComb.SelectedIndex = 0;
            }
            else
            {
                this.EvaluationYearComb.Value = gatheryear;
                this.OrganTypeComb.Value = gatherorgan;
                this.GatherTypeComb.Value = gathertype.Substring(0, 2);
                this.GatherClassComb.Value = gathertype;
                this.GuideGatherCombo.Value = guidegathercode;
            }
        }

        //指标集类别选择事件
        protected void SelectedGatherClass(object sender, AjaxEventArgs e)
        {
            string gathertype = "";
            string gatherorgan = "";
            string gatheryear = "";
            gatherorgan = this.OrganTypeComb.SelectedItem.Value;
            gatherorgan = gatherorgan.Equals("") ? "01" : gatherorgan;
            gatheryear = this.EvaluationYearComb.SelectedItem.Value;
            gatheryear = gatheryear.Equals("") ? "all" : gatheryear;
            gathertype = this.GatherClassComb.SelectedItem.Value;
            gathertype = gathertype.Equals("") ? "0101" : gathertype;
            DataTable dt = dal.GetGuideGather(gathertype, gatherorgan, gatheryear).Tables[0];
            this.GuideGatherStore.DataSource = dt;
            this.GuideGatherStore.DataBind();
            if (dt.Rows.Count > 0)
            {
                this.GuideGatherCombo.SelectedIndex = 0;
            }
        }

        //指标集大类别选择下拉事件
        protected void SelectedGatherType(object sender, AjaxEventArgs e)
        {
            string gathertype = this.GatherTypeComb.SelectedItem.Value;
            DataTable dt = dal.GetGuideGroupType(gathertype).Tables[0];
            this.GuideClassStore.DataSource = dt;
            this.GuideClassStore.DataBind();
            if (dt.Rows.Count > 0)
            {
                this.GatherClassComb.SelectedIndex = 0;
            }
        }

        //BSC1类别选择下拉事件
        protected void SelectedBSC1(object sender, AjaxEventArgs e)
        {
            string organ = "";
            string bsc2 = "";
            string bsc1 = this.BSC1Comb.SelectedItem.Value;
            bsc1 = bsc1.Equals("") ? "01" : bsc1;
            DataTable dt = dal.GetBSTwo(bsc1).Tables[0];
            this.BSCStore.DataSource = dt;
            this.BSCStore.DataBind();
            if (dt.Rows.Count > 0)
            {
                this.BSC2Comb.SelectedIndex = 0;
                bsc2 = dt.Rows[0]["BSC_CLASS_CODE"].ToString();
            }
            bsc2 = bsc2.Equals("") ? "0101" : bsc2;
            organ = this.OrganComb.SelectedItem.Value;
            organ = organ.Equals("") ? "01" : organ;
            string code = organ.Substring(1, 1) + bsc2;
            if (Session["SelectedGuide"] == null)
            {
                this.GuideCodeTxt.Value = dal.GetNextGuideCode(code);
            }
        }

        //BSC2类别选择下拉事件
        protected void SelectedBSC2(object sender, AjaxEventArgs e)
        {
            string organ = "";
            string bsc2 = "";
            bsc2 = this.BSC2Comb.SelectedItem.Value;
            organ = this.OrganComb.SelectedItem.Value;
            bsc2 = bsc2.Equals("") ? "0101" : bsc2;
            organ = organ.Equals("") ? "01" : organ;
            string code = organ.Substring(1, 1) + bsc2;
            if (Session["SelectedGuide"] == null)
            {
                this.GuideCodeTxt.Value = dal.GetNextGuideCode(code);
            }
        }

        /// <summary>
        /// 确定按钮事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void BtnSave_Click(object sender, AjaxEventArgs e)
        {
            string Values = e.ExtraParams["Values"].ToString();
            Dictionary<string, string> FormValues = JSON.Deserialize<Dictionary<string, string>>(Values);

            //检验指标表达式
            if (CheckExpress(this.IsExpressComb.SelectedItem.Value, this.GuideExpressTxt.Text))
            {
                Goldnet.Ext.Web.Ext.Msg.Show(new Goldnet.Ext.Web.MessageBox.Config
                {
                    Title = "信息提示",
                    Message = "指标公式不正确!",
                    Buttons = MessageBox.Button.OK,
                    Icon = (MessageBox.Icon)Enum.Parse(typeof(MessageBox.Icon), "WARNING")
                });
                return;
            }

            string rtn = dal.UpdateGuideInfo(FormValues);
            if (rtn.Equals(""))
            {
                Goldnet.Ext.Web.ScriptManager scManager = Goldnet.Ext.Web.ScriptManager.GetInstance(this.Page);
                scManager.AddScript("parent.Ext.Msg.show({ title: '系统提示', msg:  '指标保存成功!',icon: 'ext-mb-info',  buttons: { ok: true }  });");
                scManager.AddScript("parent.Store1.reload();");
                scManager.AddScript("parent.DetailWin.hide();");
            }
            else
            {
                Goldnet.Ext.Web.ScriptManager scManager = Goldnet.Ext.Web.ScriptManager.GetInstance(this.Page);
                scManager.AddScript("parent.Ext.Msg.show({ title: '系统提示', msg:  '" + rtn + "',icon: 'ext-mb-info',  buttons: { ok: true }  });");
            }
        }

        /// <summary>
        /// 检查公式录入是否正确
        /// </summary>
        /// <param name="isexpress"></param>
        /// <param name="expresstxt"></param>
        /// <returns></returns>
        private Boolean CheckExpress(string isexpress, string expresstxt)
        {
            Boolean retExpress = false;
            if (isexpress.Equals("1"))
            {
                //选择公式
                //指标公式空检查
                if (string.IsNullOrEmpty(expresstxt))
                {
                    retExpress = true;
                }

                //指标正确性检查
                if (expresstxt.ToUpper().Contains("IF"))
                {
                    if (expresstxt.ToUpper().Contains("THEN") && expresstxt.ToUpper().Contains("END"))
                    {
                        retExpress = false;
                    }
                    else
                    {
                        retExpress = true;
                    }
                }
            }
            return retExpress;
        }

    }
}
