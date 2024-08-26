using System;
using System.Collections;
using System.Configuration;
using System.Data;
using Goldnet.Ext.Web;
using Goldnet.Dal;
using GoldNet.Comm;
using GoldNet.Model;

namespace GoldNet.JXKP.RLZY.BaseInfoManager
{
    public partial class RewardList : PageBase
    {
        BaseInfoManagerDal dal = new BaseInfoManagerDal();
        private static string add_mark = null;

        /// <summary>
        /// 初始化处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

                HttpProxy proxy = new HttpProxy();
                proxy.Method = HttpMethod.POST;
                proxy.Url = "/RLZY/WebService/DeptInfo.ashx?deptfilter=" + "";
                this.Store3.Proxy.Add(proxy);

                SetCombox();

                this.Store1.DataSource = dal.ViewRewardList(DateTime.Now.Year.ToString());
                this.Store1.DataBind();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="rewardDesc"></param>
        /// <param name="deptCode"></param>
        /// <param name="deptName"></param>
        /// <param name="StartDate"></param>
        /// <param name="manageDept"></param>
        /// <param name="managePrs"></param>
        /// <param name="optype"></param>
        [AjaxMethod]
        public void ProblemListAjaxOper(string Id, string rewardDesc, string deptCode, string deptName, string StartDate, string manageDept, string managePrs, string optype, string managePrsName, string manageDeptName)
        {
            switch (optype)
            {
                case "2":
                    // 新增保存
                    string Creater = ((User)Session["CURRENTSTAFF"]).UserId == null ? "NotUserId" : ((User)Session["CURRENTSTAFF"]).UserId;
                    dal.InsertReward(rewardDesc, deptCode, deptName, StartDate, manageDept, managePrs, managePrsName, manageDeptName);
                    break;
                case "7":
                    // 修改保存
                    dal.UpdataReward(Id, rewardDesc, deptCode, deptName, StartDate, manageDept, managePrs, managePrsName, manageDeptName);
                    break;
                case "3":
                    // 删除处理
                    dal.EchoDeleteHandle("REWARD_INFO", "ID", Id);
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
            this.Store1.DataSource = dal.ViewRewardList(this.TimeOrgan.SelectedItem.Value, this.cboTime.SelectedItem.Value, this.DeptCodeCombo.SelectedItem.Value, this.txtPrName.Text);

            this.Store1.DataBind();
        }

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

            //作者排名字典
            cboDt = dictdal.getDictInfo("REWARD_DEPT_DICT", "ID", "DEPT_NAME", false, "").Tables[0];
            SetCboData("ID", "DEPT_NAME", cboDt, ComboBox2);
            ComboBox2.SelectedIndex = 0;

            //加载课题类别字典
            cboDt = dictdal.getDictInfo("REWARD_DESC_DICT", "ID", "DESC_NAME", false, "").Tables[0];
            SetCboData("ID", "DESC_NAME", cboDt, txtrewarddesc);
            txtrewarddesc.SelectedIndex = 0;

         }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="text"></param>
        /// <param name="dtSource"></param>
        /// <param name="cbo"></param>
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

    }
}
