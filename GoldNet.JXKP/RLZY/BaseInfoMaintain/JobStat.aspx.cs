using System;
using System.Data;
using Goldnet.Ext.Web;
using System.Web.UI.WebControls;
using Goldnet.Dal;

namespace GoldNet.JXKP.RLZY.BaseInfoMaintain
{
    public partial class JobStat : PageBase
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
                //等级字典信息
                DataTable l_dt = PersTypeFilter();
                if (l_dt.Rows.Count > 0)
                {
                    cboPersonType.Items.Add(new Goldnet.Ext.Web.ListItem("全部", "全部"));
                }
                SetCboData("", "", l_dt, cboPersonType);
                if (l_dt.Rows.Count > 0)
                {
                    cboPersonType.SelectedIndex = 0;
                }
                string sort = "";
                for (int i = 0; i < l_dt.Rows.Count; i++)
                {
                    sort = sort + "'" + l_dt.Rows[i]["NAME"] + "',";
                }
                if (l_dt.Rows.Count == 0)
                {
                    sort = "'-1'";
                }
                string deptcode = this.DeptFilter("");

                HttpProxy proxy = new HttpProxy();
                proxy.Method = HttpMethod.POST;
                proxy.Url = "/RLZY/WebService/DeptInfo.ashx?deptfilter=" + deptcode;
                this.Store3.Proxy.Add(proxy);
                //DataTable tb = dal.ViewJobStat("", sort.TrimEnd(new char[] { ',' }), deptcode).Tables[0];
                //Session["jobstattb"] = tb;
                //this.Store1.DataSource = tb;
                //this.Store1.DataBind();
            }
        }
        /// <summary>
        /// 刷新数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void Data_RefreshData(object sender, StoreRefreshDataEventArgs e)
        {
            string sort = "";
            if (this.cboPersonType.SelectedItem.Text == "" || this.cboPersonType.SelectedItem.Text == "全部")
            {
                DataTable l_dt = PersTypeFilter();
                for (int i = 0; i < l_dt.Rows.Count; i++)
                {
                    sort = sort + "'" + l_dt.Rows[i]["NAME"] + "',";
                }
                if (l_dt.Rows.Count == 0)
                {
                    sort = "'-1'";
                }
            }
            else
            {
                sort = "'" + this.cboPersonType.SelectedItem.Text + "'";
            }
            Store1.RemoveAll();
            DataTable tb = dal.ViewJobStat(this.DeptCodeCombo.SelectedItem.Value, sort.TrimEnd(new char[] { ',' }), this.DeptFilter("")).Tables[0];
            Session["jobstattb"] = tb;
            Store1.DataSource = tb;
            Store1.DataBind();
        }
        protected void OutExcel(object sender, EventArgs e)
        {
            TableCell[] header = new TableCell[31];

            header[0] = new TableHeaderCell();
            header[0].Text = "科室名称";
            header[0].RowSpan = 2;
            header[1] = new TableHeaderCell();
            header[1].Text = "合计";
            header[1].RowSpan = 2;
            header[2] = new TableHeaderCell();
            header[2].Text = "医疗";
            header[2].ColumnSpan = 4;
            header[3] = new TableHeaderCell();
            header[3].Text = "护理";
            header[3].ColumnSpan = 5;
            header[4] = new TableHeaderCell();
            header[4].Text = "医技";
            header[4].ColumnSpan = 5;
            header[5] = new TableHeaderCell();
            header[5].Text = "药剂";
            header[5].ColumnSpan = 5;
            header[6] = new TableHeaderCell();
            header[6].Text = "工程";
            header[6].ColumnSpan = 3;
            header[7] = new TableHeaderCell();
            header[7].Text = "会计</th></tr><tr>";
            header[8] = new TableHeaderCell();
            header[8].Text = "主任医师";
            header[9] = new TableHeaderCell();
            header[9].Text = "副主任医师";
            header[10] = new TableHeaderCell();
            header[10].Text = "主治医师";
            header[11] = new TableHeaderCell();
            header[11].Text = "医师";
            header[12] = new TableHeaderCell();
            header[12].Text = "见习医师";
            header[13] = new TableHeaderCell();
            header[13].Text = "主任护师";
            header[14] = new TableHeaderCell();
            header[14].Text = "副主任护师";
            header[15] = new TableHeaderCell();
            header[15].Text = "主管护师";
            header[16] = new TableHeaderCell();
            header[16].Text = "护师";
            header[17] = new TableHeaderCell();
            header[17].Text = "护士";
            header[18] = new TableHeaderCell();
            header[18].Text = "见习护士";
            header[19] = new TableHeaderCell();
            header[19].Text = "主任技师";
            header[20] = new TableHeaderCell();
            header[20].Text = "副主任技师";
            header[21] = new TableHeaderCell();
            header[21].Text = "主管技师";
            header[22] = new TableHeaderCell();
            header[22].Text = "技师";
            header[23] = new TableHeaderCell();
            header[23].Text = "技士";
            header[24] = new TableHeaderCell();
            header[24].Text = "主任药师";
            header[25] = new TableHeaderCell();
            header[25].Text = "副主任药师";
            header[26] = new TableHeaderCell();
            header[26].Text = "主管药师";
            header[27] = new TableHeaderCell();
            header[27].Text = "药师";
            header[28] = new TableHeaderCell();
            header[28].Text = "药士";
            header[29] = new TableHeaderCell();
            header[29].Text = "高级工程师";
            header[30] = new TableHeaderCell();
            header[30].Text = "工程师";
            header[31] = new TableHeaderCell();
            header[31].Text = "助理工程师";
            header[32] = new TableHeaderCell();
            header[32].Text = "会计师</th>";

            //
            if (Session["jobstattb"] != null)
            {
                DataTable dt = (DataTable)Session["jobstattb"];
                if (dt.Rows.Count > 0)
                    MHeaderTabletoExcel(dt, header, "职称结构分布", null, 0);
            }
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

    }
}
