using System;
using System.Data;
using Goldnet.Ext.Web;
using System.Collections.Generic;
using GoldNet.Model;

namespace GoldNet.JXKP.jxkh
{
    public partial class StationsMonthsSet : PageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Ext.IsAjaxRequest)
            {
                Bindlist(Request["stationcode"].ToString(), Request["stationyear"].ToString());
            }
        }

        //查询科室信息表Dept_info
        protected void Bindlist(string stationcode, string stationyear)
        {
            Goldnet.Dal.StationManager dal = new Goldnet.Dal.StationManager();

            DataTable dt = dal.GetStationMonthsSet(stationcode, stationyear);

            this.Store1.DataSource = dt;
            this.Store1.DataBind();
        }

        //
        protected void Store_RefreshData(object sender, StoreRefreshDataEventArgs e)
        {
            Bindlist(Request["stationcode"].ToString(), Request["stationyear"].ToString());
        }

        //刷新
        protected void Button_refresh_click(object sender, EventArgs e)
        {
            Bindlist(Request["stationcode"].ToString(), Request["stationyear"].ToString());
        }

        //保存
        protected void SubmitData(object sender, StoreSubmitDataEventArgs e)
        {
            List<PageModels.stationmonthinfo> deptinfos = e.Object<PageModels.stationmonthinfo>();
            Goldnet.Dal.StationManager dal = new Goldnet.Dal.StationManager();
            try
            {
                if (deptinfos == null || deptinfos.Count == 0)
                {
                    return;
                }
                string stationcode = Request["stationcode"].ToString();
                string stationyear = Request["stationyear"].ToString();
                dal.StationMonths(deptinfos, stationcode, stationyear);
                Ext.Msg.Alert("提示", "保存成功").Show();
                Store_RefreshData(null, null);
            }
            catch
            {
                Ext.Msg.Alert("提示", "保存失败").Show();
            }
        }

    }
}
