using System;
using Goldnet.Ext.Web;
using GoldNet.Comm;
using GoldNet.Comm.ExportData;
using System.Data;
using GoldNet.Model;

namespace GoldNet.JXKP.jxkh
{
    public partial class Dept_Assess_Guid_Result :  PageBase
    {
        Goldnet.Dal.Assess dal = new Goldnet.Dal.Assess();

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
                string deptcode = this.DeptFilter("");
                HttpProxy proxy = new HttpProxy();
                proxy.Method = HttpMethod.POST;
                proxy.Url = "/RLZY/WebService/DeptInfo.ashx?deptfilter=" + deptcode;
                this.Store3.Proxy.Add(proxy);

                HttpProxy proxy2 = new HttpProxy();
                proxy2.Method = HttpMethod.POST;
                proxy2.Url = "/jxkh/WebService/GuideList.ashx?deptfilter=" + deptcode;
                this.Store2.Proxy.Add(proxy2);

                SetInitState();
             }

        }

        //设置页面控件初始化状态
        protected void SetInitState()
        {
            //年份、月份下拉初始化
            for (int i = 0; i < 10; i++)
            {
                int years = System.DateTime.Now.Year - i;
                this.Comb_StartYear.Items.Add(new Goldnet.Ext.Web.ListItem(years.ToString(), years.ToString()));

            }
            string stationyear = DateTime.Now.Year.ToString();
            //获取考核名称，并初始化考核名称下拉
            DataTable dt = dal.GetDeptArchAssess(stationyear, "").Tables[0];
            if (dt.Rows.Count.Equals(0))
            {
                this.Btn_View.Disabled = true;
            }
            else
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    this.Comb_AssessName.Items.Add(new Goldnet.Ext.Web.ListItem(dt.Rows[i]["ASSESS_NAME"].ToString(), dt.Rows[i]["ASSESS_CODE"].ToString()));
                }
                this.Comb_AssessName.SelectedIndex = 0;
            }
            //科室类别下拉初始化
            Goldnet.Dal.SYS_DEPT_DICT daldept = new Goldnet.Dal.SYS_DEPT_DICT();
            DataTable table = daldept.GetDeptType().Tables[0];
            if (table.Rows.Count > 0)
            {
                for (int i = 0; i < table.Rows.Count; i++)
                {
                    this.ComboBoxdepttype.Items.Add(new Goldnet.Ext.Web.ListItem(table.Rows[i]["ATTRIBUE"].ToString(), table.Rows[i]["id"].ToString()));
                }
            }

        }

        /// <summary>
        /// 考核年度选择
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Comb_Year_Selected(object sender, AjaxEventArgs e)
        {
            string stationyear = this.Comb_StartYear.SelectedItem.Value;
            string incount = ((User)(Session["CURRENTSTAFF"])).UserId;
            DataTable dt = dal.GetDeptArchAssess(stationyear, incount).Tables[0];

            Goldnet.Ext.Web.ScriptManager scManager = Goldnet.Ext.Web.ScriptManager.GetInstance(this.Page);
            scManager.AddScript(Comb_AssessName.ClientID + ".store.removeAll();");
            scManager.AddScript(Comb_AssessName.ClientID + ".clearValue();");

            if (dt.Rows.Count.Equals(0))
            {
                this.Btn_View.Disabled = true;
            }
            else
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    this.Comb_AssessName.AddItem(dt.Rows[i]["ASSESS_NAME"].ToString(), dt.Rows[i]["ASSESS_CODE"].ToString());
                }
                this.Comb_AssessName.SelectedIndex = 0;
                this.Btn_View.Disabled = false;
            }
        }

        /// <summary>
        /// 查询处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Btn_View_Clicked(object sender, AjaxEventArgs e)
        {
            string stationyear = this.Comb_StartYear.SelectedItem.Value;
            string assesscode = this.Comb_AssessName.SelectedItem.Value;
            stationyear = stationyear.PadLeft(4, '0');
            string deptFilter = DeptFilter("");

            DataTable dt = dal.GetDeptAssessSavedArchByGuide(assesscode, stationyear, deptFilter, this.ComboBoxdepttype.SelectedItem.Value, this.Combodept.SelectedItem.Value, this.Comboguide.SelectedItem.Value).Tables[0];

            this.Store1.DataSource = dt;
            this.Store1.DataBind();
        }
        
        /// <summary>
        /// 导出EXCEL
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void OutExcel(object sender, EventArgs e)
        {
            string filename = this.Comb_AssessName.SelectedItem.Text;
            ExportData ex = new ExportData();
            string stationyear = this.Comb_StartYear.SelectedItem.Value;
            string assesscode = this.Comb_AssessName.SelectedItem.Value;
            stationyear = stationyear.PadLeft(4, '0');
            string deptFilter = DeptFilter("");
            
            DataTable dt = dal.GetDeptAssessSavedArchByGuide(assesscode, stationyear, deptFilter, this.ComboBoxdepttype.SelectedItem.Value, this.Combodept.SelectedItem.Value, this.Comboguide.SelectedItem.Value).Tables[0];
            
            ex.ExportToLocal(dt, this.Page, "xls", filename);
        }

        //显示提示信息
        public void showMsg(string msg)
        {
            Goldnet.Ext.Web.Ext.Msg.Show(new Goldnet.Ext.Web.MessageBox.Config
            {
                Title = SystemMsg.msgtitle4,
                Message = msg,
                Buttons = MessageBox.Button.OK,
                Icon = (MessageBox.Icon)Enum.Parse(typeof(MessageBox.Icon), "INFO")
            });
        }



    }
}
