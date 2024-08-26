using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Goldnet.Ext.Web;
using GoldNet.Comm;
using GoldNet.Comm.DAL.Oracle;
using System.Data;
using GoldNet.Model;

namespace GoldNet.JXKP
{
    public partial class Staff_Document :PageBase
    {
        private Goldnet.Dal.StationManager dal = new Goldnet.Dal.StationManager();
        protected void Page_Load(object sender, EventArgs e)
        {
            string staffid = Request["staffid"].ToString();
            GetStaffDocument(staffid);
        }
       
        protected void GetStaffDocument(string staffid)
        {
            DataTable table = dal.GetStaffDocument(staffid);
            this.xm.Text = table.Compute("Max(VALUE)", "ATTR='XM'").ToString();
            this.xb.Text = table.Compute("Max(VALUE)", "ATTR='XB'").ToString();
            this.csrq.Text = table.Compute("Max(VALUE)", "ATTR='CSRQ'").ToString();
            //this.bysj.Text = table.Compute("Max(VALUE)", "ATTR='BYSJ'").ToString();
            this.zy.Text = table.Compute("Max(VALUE)", "ATTR='ZY'").ToString();
            this.xw.Text = table.Compute("Max(VALUE)", "ATTR='XW'").ToString();
            //this.yx.Text = table.Compute("Max(VALUE)", "ATTR='YX'").ToString();
            //this.bzb.Text = table.Compute("Max(VALUE)", "ATTR='BZB'").ToString();
            this.jszc.Text = table.Compute("Max(VALUE)", "ATTR='JSZC'").ToString();
            this.xgwgzsj.Text = table.Compute("Max(VALUE)", "ATTR='XGWGZSJ'").ToString();

            this.mzrc.Text = table.Compute("Max(VALUE)", "ATTR='MZRC'").ToString() == "" ? "0" : table.Compute("Max(VALUE)", "ATTR='MZRC'").ToString();
            this.mzsscs.Text = table.Compute("Max(VALUE)", "ATTR='MZSSCS'").ToString() == "" ? "0" : table.Compute("Max(VALUE)", "ATTR='MZSSCS'").ToString();
            this.czrs.Text = table.Compute("Max(VALUE)", "ATTR='CZRS'").ToString() == "" ? "0" : table.Compute("Max(VALUE)", "ATTR='CZRS'").ToString();
            this.srl.Text = table.Compute("Max(VALUE)", "ATTR='SRL'").ToString() == "" ? "0" : table.Compute("Max(VALUE)", "ATTR='SRL'").ToString();
            this.rjgcs.Text = table.Compute("Max(VALUE)", "ATTR='RJGCS'").ToString() == "" ? "0" : table.Compute("Max(VALUE)", "ATTR='RJGCS'").ToString();
            this.qjl.Text = table.Compute("Max(VALUE)", "ATTR='QJL'").ToString() == "" ? "0" : table.Compute("Max(VALUE)", "ATTR='QJL'").ToString();
            this.cyrs.Text = table.Compute("Max(VALUE)", "ATTR='CYRS'").ToString() == "" ? "0" : table.Compute("Max(VALUE)", "ATTR='CYRS'").ToString();
            this.pjzyr.Text = table.Compute("Max(VALUE)", "ATTR='PJZYR'").ToString() == "" ? "0" : table.Compute("Max(VALUE)", "ATTR='PJZYR'").ToString();
            this.zyhzl.Text = table.Compute("Max(VALUE)", "ATTR='ZYHZL'").ToString() == "" ? "0" : table.Compute("Max(VALUE)", "ATTR='ZYHZL'").ToString();
            this.srqzl.Text = table.Compute("Max(VALUE)", "ATTR='SRQZL'").ToString() == "" ? "0" : table.Compute("Max(VALUE)", "ATTR='SRQZL'").ToString();
            this.hzrjfy.Text = table.Compute("Max(VALUE)", "ATTR='HZRJFY'").ToString() == "" ? "0" : table.Compute("Max(VALUE)", "ATTR='HZRJFY'").ToString();
            this.sstc.Text = table.Compute("Max(VALUE)", "ATTR='SSTC'").ToString() == "" ? "0" : table.Compute("Max(VALUE)", "ATTR='SSTC'").ToString();
            this.dssl.Text = table.Compute("Max(VALUE)", "ATTR='DSSL'").ToString() == "" ? "0" : table.Compute("Max(VALUE)", "ATTR='DSSL'").ToString();
            this.zssl.Text = table.Compute("Max(VALUE)", "ATTR='ZSSL'").ToString() == "" ? "0" : table.Compute("Max(VALUE)", "ATTR='ZSSL'").ToString();
            this.xssl.Text = table.Compute("Max(VALUE)", "ATTR='XSSL'").ToString() == "" ? "0" : table.Compute("Max(VALUE)", "ATTR='XSSL'").ToString();

            this.xssj.Text = table.Compute("Max(VALUE)", "ATTR='XSSJ'").ToString();
            this.kylw.Text = table.Compute("Max(VALUE)", "ATTR='KYLW'").ToString();
            this.jxlw.Text = table.Compute("Max(VALUE)", "ATTR='JXLW'").ToString();
            this.hxqk.Text = table.Compute("Max(VALUE)", "ATTR='HXQK'").ToString();
            this.dyzz.Text = table.Compute("Max(VALUE)", "ATTR='DYZZ'").ToString();
            this.xgkt.Text = table.Compute("Max(VALUE)", "ATTR='XGKT'").ToString();
            this.yjcg.Text = table.Compute("Max(VALUE)", "ATTR='YJCG'").ToString();
            this.gjcg.Text = table.Compute("Max(VALUE)", "ATTR='GJCG'").ToString();
            this.sbjcg.Text = table.Compute("Max(VALUE)", "ATTR='SBJCG'").ToString();
            this.ssttrzsl.Text = table.Compute("Max(VALUE)", "ATTR='SSTTRZSL'").ToString();
            this.xshy.Text = table.Compute("Max(VALUE)", "ATTR='XSHY'").ToString();
            this.byjq.Text = table.Compute("Max(VALUE)", "ATTR='BYJQ'").ToString();
            this.yljf.Text = table.Compute("Max(VALUE)", "ATTR='YLJF'").ToString();
            this.hzts.Text = table.Compute("Max(VALUE)", "ATTR='HZTS'").ToString();
            this.tbpp.Text = table.Compute("Max(VALUE)", "ATTR='TBPP'").ToString();
        }

    }
}
