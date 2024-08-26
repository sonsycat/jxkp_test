using System;
using System.Data;
using System.Data.OleDb;
using System.Collections.Generic;
using System.Web;
using System.Text;
using GoldNet.Comm;
using GoldNet.Comm.DAL.Oracle;
using Goldnet.Ext.Web;
using System.Collections;



namespace GoldNet.JXKP.jxkh
{
    public partial class StationPersonnel : System.Web.UI.Page
    {
        public static string stationcode = "";
        public static string stationyear = "";
        public static string deptcode = "";
        Goldnet.Dal.StationManager dal = new Goldnet.Dal.StationManager();
        protected void Page_Load(object sender, EventArgs e)
        {

            //检查是否已经登录，否则停止
            if (Session["CURRENTSTAFF"] == null)
            {
                Response.End();
                return;
            }

            if (!Ext.IsAjaxRequest)
            {
                //数据有效性验证(待加)
                stationcode = Request.QueryString["id"].ToString();
                stationyear = Request.QueryString["sy"].ToString();
                deptcode = Request.QueryString["dc"].ToString();

                DataTable table = dal.GetStationPersonnelOnJob(stationcode, stationyear);
                Store1.DataSource = table;
                Store1.DataBind();
               
            }

        }

       

        //在岗离岗下拉框选择事件
        protected void Combo_zgflg_Select(object sender, AjaxEventArgs e)
        {
            string flag = this.Combo_zgflg.SelectedItem.Value;
            DataTable dt = new DataTable();

            if (flag.Equals("1"))
            {
               dt = dal.GetStationPersonnelOnJob(stationcode, stationyear);  
            }
            else
            {
                dt = dal.GetStationPersonByDept(deptcode, stationyear);
            }
            Store1.DataSource = dt;
            Store1.DataBind();

           
        }

        //保存按钮，处理进岗离岗
        protected void btn_save_Click(object sender, AjaxEventArgs e)
        {
            string flag = e.ExtraParams["flag"];
            string multi2 = e.ExtraParams["multi2"];
            Dictionary<string,string>[] Lists = JSON.Deserialize<Dictionary<string,string>[]>(multi2);

            if (Lists.Length.Equals(0))
            {
                Ext.Msg.Alert("系统提示", "请将人员信息加入已选列表后再作" + (flag.Equals("1") ? "【离岗】" : "【进岗】") + "操作！").Show();
                return;
            }

            dal.StationPersonUpdate(flag, Lists,stationcode,stationyear); 
            Ext.Msg.Alert("系统提示", "人员" + (flag.Equals("1") ? "【离岗】" : "【进岗】") + "操作成功！").Show();
            Store2.RemoveAll();
            return;
        }

    }
}
