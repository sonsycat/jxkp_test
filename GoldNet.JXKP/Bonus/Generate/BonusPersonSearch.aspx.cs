using System;
using System.Collections.Generic;
using Goldnet.Dal;
using Goldnet.Ext.Web;
using System.Data;
using GoldNet.Model;
using GoldNet.Comm.ExportData;
using GoldNet.Comm;

namespace GoldNet.JXKP.Bonus.Generate
{
    public partial class BonusPersonSearch : PageBase
    {
        /// <summary>
        /// 初始化处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Ext.IsAjaxRequest)
            {
                //Encrypt aa = new Encrypt();
                //string cc = aa.EncryptTheStr("iloveyou", "1");
                //string bb = aa.EncryptTheStr("iloveyou", "2");
                //参数获取
                string tag = this.GetStringByQueryStr("tag");
                SetDict();
                data(tag);
            }
        }

        /// <summary>
        /// 下拉框设置
        /// </summary>
        public void SetDict()
        {
            //科室
            string deptcode = this.DeptFilter("");
            HttpProxy proxy = new HttpProxy();
            proxy.Method = HttpMethod.POST;
            proxy.Url = "/RLZY/WebService/DeptInfo.ashx?deptfilter=" + deptcode;
            this.Store3.Proxy.Add(proxy);

            //奖金列表
            CalculateBonus calculateBonus = new CalculateBonus();
            DataTable tblBonusIndex = calculateBonus.GetIndexAll();
            if (tblBonusIndex.Rows.Count > 0)
            {
                for (int i = 0; i < tblBonusIndex.Rows.Count; i++)
                {
                    this.comindex.Items.Add(new Goldnet.Ext.Web.ListItem(tblBonusIndex.Rows[i]["BONUSNAME"].ToString(), tblBonusIndex.Rows[i]["ID"].ToString()));
                }

                this.comindex.SelectedItem.Value = tblBonusIndex.Rows[0]["ID"].ToString();
            }

            //人员类别
            DataTable tblType = calculateBonus.GetPersonsType();
            if (tblType.Rows.Count > 0)
            {
                for (int i = 0; i < tblType.Rows.Count; i++)
                {
                    this.comtype.Items.Add(new Goldnet.Ext.Web.ListItem(tblType.Rows[i]["BONUS_PSERSONS_TYPE"].ToString(), tblType.Rows[i]["BONUS_PSERSONS_TYPE"].ToString()));
                }
            }
        }

        /// <summary>
        /// 查询处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Btn_View_Clicked(object sender, AjaxEventArgs e)
        {
            string tag = this.GetStringByQueryStr("tag");
            data(tag);
        }

        /// <summary>
        /// 获取二次分配数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Store_RefreshData(object sender, StoreRefreshDataEventArgs e)
        {
            string tag = this.GetStringByQueryStr("tag");
            data(tag);
        }

        /// <summary>
        /// 获取奖金明细列表数据
        /// </summary>
        /// <param name="bonusid"></param>
        private void data(string tag )
        {
            string bonusindex = this.comindex.SelectedItem.Value;
            string deptcode = this.Combodept.SelectedItem.Value;
            string typename = this.comtype.SelectedItem.Value;

            CalculateBonus calculateBouns = new CalculateBonus();

            //获取科室人员分配明细列表
            DataTable table = calculateBouns.GetBonusPersonsSearch(tag, bonusindex, deptcode, typename);
            
            DataRow dr = table.NewRow();
            for (int i = 0; i < table.Columns.Count; i++)
            {
                RecordField record = new RecordField();
                record = new RecordField(table.Columns[i].ColumnName, RecordFieldType.String);
                this.Store1.AddField(record);

                Column cl = new Column();
                cl.Header = table.Columns[i].ColumnName;
                cl.Sortable = false;
                cl.MenuDisabled = true;
                cl.DataIndex = table.Columns[i].ColumnName;
                cl.ColumnID = table.Columns[i].ColumnName;

                if (cl.Header.Equals("银行账号") || cl.Header.Equals("姓名") || cl.Header.Equals("科室名称") || cl.Header.Equals("ID") || cl.Header.Equals("DEPT_CODE") || cl.Header.Equals("人员类别"))
                {
                    if (cl.Header.Equals("ID") || cl.Header.Equals("DEPT_CODE"))
                    {
                        cl.Hidden = true;
                    }
                }
                else 
                {
                    cl.Renderer.Fn = "rmbMoney";
                }
                
                this.GridPanel1.ColumnModel.Columns.Add(cl);
            }
            table.Rows.Add(dr);

            this.Store1.DataSource = table;
            this.Store1.DataBind();
        }


        /// <summary>
        /// EXCEL导出
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void OutExcel(object sender, EventArgs e)
        {
            ExportData ex = new ExportData();
            string tag = this.GetStringByQueryStr("tag");
            string bonusindex = this.comindex.SelectedItem.Value;
            string deptcode = this.Combodept.SelectedItem.Value;
            string typename = this.comtype.SelectedItem.Value;

            CalculateBonus calculateBouns = new CalculateBonus();

            //获取科室人员分配明细列表
            DataTable table = calculateBouns.GetBonusPersonsSearch(tag, bonusindex, deptcode, typename);

            //this.outexcel(table,"奖金明细");
            ex.ExportToLocal(table, this.Page, "xls", "奖金明细");
        }
      

        //反序列化得到客户端提交的gridpanel数据行      
        private Dictionary<string, string>[] GetSelectRow(AjaxEventArgs e, string epValue)
        {
            string row = e.ExtraParams[epValue].ToString();
            Dictionary<string, string>[] selectRow = JSON.Deserialize<Dictionary<string, string>[]>(row);
            if (selectRow.Length <= 0) { return null; } else { return selectRow; }
        }

        //反序列化得到客户端提交的gridpanel数据行      
        private Dictionary<string, string>[] GetSelectRow(AjaxEventArgs e)
        {
            string row = e.ExtraParams["Values"].ToString();
            Dictionary<string, string>[] selectRow = JSON.Deserialize<Dictionary<string, string>[]>(row);
            if (selectRow.Length <= 0) { return null; } else { return selectRow; }
        }

    }
}
