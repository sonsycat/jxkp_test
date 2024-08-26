using System;
using System.Collections.Generic;
using System.Data;
using Goldnet.Dal;
using Goldnet.Ext.Web;
using GoldNet.Comm.ExportData;
using GoldNet.Model;

namespace GoldNet.JXKP
{
    public partial class dept_Assess_ResultInfo : PageBase
    {
        /// <summary>
        /// 初始化处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["CURRENTSTAFF"] == null)
            {
                Response.End();
            }

            if (!Ext.IsAjaxRequest)
            {
                //科室、指标类别
                string dept_code = Request.QueryString["dept_code"].ToString();
                string bsc_class = Request.QueryString["bsc_class"].ToString();
                string benginDate = Request.QueryString["benginDate"].ToString();
                if (Request.QueryString["assess_code"] == null)
                {
                    string counting = Request.QueryString["counting"].ToString();
                    //
                    BindList1(counting, dept_code, bsc_class, benginDate);
                }
                else
                {
                    string assess_code = Request.QueryString["assess_code"].ToString();
                    //
                    BindList2(assess_code, dept_code, bsc_class);
                }
                //是否做临时保存
                if (this.Session["saveflag"] == "0")
                {
                    //保存按钮设置
                    //this.Btn_Del.Disabled = true;
                }
                else
                {
                    //this.Btn_Del.Disabled = false;
                }
            }
        }

        /// <summary>
        /// 获取临时保存的数据并绑定
        /// </summary>
        /// <param name="counting"></param>
        /// <param name="person_id"></param>
        /// <param name="bsc_class"></param>
        protected void BindList1(string counting, string dept_code, string bsc_class, string benginDate)
        {
            Assess dal = new Assess();
            DataTable dt;
            string bsc_where = "";
            if (bsc_class != null && bsc_class != "")
            {
                bsc_where = "T.BSC_CLASS LIKE '" + bsc_class + "%' AND";
            }
            
            if (this.Session["saveflag"] =="0")
            {
                //获取临时保存的明细
                dt = dal.GetdeptResultInfoSave2(counting, dept_code, bsc_where, benginDate).Tables[0];
            }

            else if (this.Session["saveflag"] == "1")
            {
                //获取归档的明细
                dt = dal.GetdeptResultInfoSave(counting, dept_code, bsc_where).Tables[0];
            }
            else
            {
                //获取未临时保存的明细
                dt = dal.GetdeptResultInfoTemp(counting, dept_code, bsc_where).Tables[0];
            }
            this.Store1.DataSource = dt;
            this.Store1.DataBind();

            Session.Remove("dept_assess_resultinfo");
            Session["dept_assess_resultinfo"] = dt;
        }

        /// <summary>
        /// 获取已归档的数据并绑定
        /// </summary>
        /// <param name="assess_code"></param>
        /// <param name="person_id"></param>
        /// <param name="bsc_class"></param>
        protected void BindList2(string assess_code, string dept_code, string bsc_class)
        {
            Assess dal = new Assess();
            string bsc_where = "";
            if (bsc_class != null && bsc_class != "")
            {
                bsc_where = "T.BSC_CLASS LIKE '" + bsc_class + "%' AND";
            }
            DataTable dt = dal.GetdeptResultInfo(assess_code, dept_code, bsc_where).Tables[0];
            this.Store1.DataSource = dt;
            this.Store1.DataBind();

            Session.Remove("dept_assess_resultinfo");
            Session["dept_assess_resultinfo"] = dt;
        }

        /// <summary>
        /// 修改生成后的得分
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Btn_Del_Click(object sender, AjaxEventArgs e)
        {
            Dictionary<string, string>[] selectRow = GetSelectRow(e);

            string dept_code = Request.QueryString["dept_code"].ToString();
            string bsc_class = Request.QueryString["bsc_class"].ToString();
            string benginDate = Request.QueryString["benginDate"].ToString();
            string save_id = ((User)(Session["CURRENTSTAFF"])).UserId;

            if (selectRow != null)
            {
                Goldnet.Dal.Assess dal = new Goldnet.Dal.Assess();
                try
                {
                    //保存更改后绩效考核得分
                    dal.SaveTemAssessInformation(selectRow, dept_code, bsc_class, save_id);

                    this.ShowMessage("信息提示", "数据保存成功！");

                    //更新列表数据
                    string counting = Request.QueryString["counting"].ToString();
                    BindList1(counting, dept_code, bsc_class, benginDate);
                }
                catch (Exception ex)
                {
                    ShowDataError(ex.ToString(), Request.Url.LocalPath, "DeleteOtherAward");
                }
            }
        }

        /// <summary>
        /// 页面列表数据序列化
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
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
        protected void OutExcel(object sender, EventArgs e)
        {
            if (Session["dept_assess_resultinfo"] != null)
            {
                ExportData ex = new ExportData();
                DataTable dt = (DataTable)Session["dept_assess_resultinfo"];

                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    if (dt.Columns[i].ColumnName == "BSC_CLASS_NAME")
                    {
                        dt.Columns[i].ColumnName = "类别";
                    }
                    else if (dt.Columns[i].ColumnName == "GUIDE_NAME")
                    {
                        dt.Columns[i].ColumnName = "指标名";
                    }
                    else if (dt.Columns[i].ColumnName == "GUIDE_VALUE")
                    {
                        dt.Columns[i].ColumnName = "指标分值";
                    }
                    else if (dt.Columns[i].ColumnName == "GUIDE_CAUSE")
                    {
                        dt.Columns[i].ColumnName = "目标值";
                    }
                    else if (dt.Columns[i].ColumnName == "GUIDE_FACT")
                    {
                        dt.Columns[i].ColumnName = "实际完成值";
                    }
                    else if (dt.Columns[i].ColumnName == "GUIDE_I_VALUE")
                    {
                        dt.Columns[i].ColumnName = "加分";
                    }
                    else if (dt.Columns[i].ColumnName == "GUIDE_D_VALUE")
                    {
                        dt.Columns[i].ColumnName = "扣分";
                    }
                    else if (dt.Columns[i].ColumnName == "GUIDE_F_VALUE")
                    {
                        dt.Columns[i].ColumnName = "最后得分";
                    }
                    else if (dt.Columns[i].ColumnName == "GUIDE_CODE")
                    {
                        dt.Columns[i].ColumnName = "指标代码";
                    }
                    else if (dt.Columns[i].ColumnName == "EXPLAIN")
                    {
                        dt.Columns[i].ColumnName = "备注";
                    }
                    else if (dt.Columns[i].ColumnName == "BSC_CLASS")
                    {
                        dt.Columns[i].ColumnName = "分类";
                    }
                }
                string dept_code = Request.QueryString["dept_code"].ToString();
                //string dates = this.cbbYear.SelectedItem.Value + "年" + this.cbbmonth.SelectedItem.Value + "月";
                ex.ExportToLocal(dt, this.Page, "xls", dept_code+"绩效考核结果表");
            }
        }
    }
}
