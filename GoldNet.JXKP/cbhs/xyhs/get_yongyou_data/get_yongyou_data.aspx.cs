using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Data.SqlClient;
using Goldnet.Ext.Web;

namespace GoldNet.JXKP.cbhs.xyhs.get_yongyou_data
{
    public partial class get_yongyou_data : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void Get_Yongyou_Data_Click(object sender, EventArgs e)
        {


            try
            {


                //清空科室表数据
                string dept_sql = "delete from  CBHS.yongyou_dept_dict";
                get_yongyou.DAL.OracleBase.ExecuteSql(dept_sql);

                ArrayList alist = new ArrayList();
                string esql = string.Empty;
                //开始时间
                //string sd = this.sdate.Value + " 00:00:00";

                //结束时间
                //string ed = this.edate.Value + " 23:59:59";

                // 查询SQL 
                string sql = string.Format(@"select 年度, 部门代码, 部门名称, 是否末级 from 部门档案
                                                 where   datediff(year,年度,getdate())=0");
                using (SqlDataReader dr = get_yongyou.DAL.SQLDB.ExecuteReader(sql))
                {
                    //从数据库循环读取机构信息
                    while (dr.Read())
                    {
                        esql = string.Format(@"insert into CBHS.yongyou_dept_dict(年度, 部门代码, 部门名称, 是否末级)
                             values('{0}', '{1}', '{2}', '{3}')", dr["年度"].ToString(), dr["部门代码"].ToString(), dr["部门名称"].ToString(), dr["是否末级"].ToString());
                        alist.Add(esql);
                    }
                    dr.Close();
                }
                //往oracle中插入数据
                get_yongyou.DAL.OracleBase.ExecuteSqlTran(alist);






                //清空薪资表数据
                string salary_sql = "delete from  CBHS.yongyou_salary_information";
                get_yongyou.DAL.OracleBase.ExecuteSql(salary_sql);

                ArrayList salary_alist = new ArrayList();
                string salary_esql = string.Empty;

                // 查询SQL 
                string sql_salary = string.Format(@"select 期间,
                                               部门代码,
                                               部门名称,
                                               职员代码,
                                               职员姓名,
                                               类别代码,
                                               类别名称,
                                               类型代码,
                                               类型名称,
                                               职称,
                                               职务,
                                               身份证号,
                                               工资合计
                                          from 工资信息
                                          where 期间+'01' >=  CONVERT(varchar(100), DATEADD (MONTH , -3, getdate() ), 112)-03
                                          order by 期间");
                using (SqlDataReader dr = get_yongyou.DAL.SQLDB.ExecuteReader(sql_salary))
                {
                    //从数据库循环读取机构信息
                    while (dr.Read())
                    {
                        salary_esql = string.Format(@"insert into CBHS.yongyou_salary_information
                                   (期间,
                                   部门代码,
                                   部门名称,
                                   职员代码,
                                   职员姓名,
                                   类别代码,
                                   类别名称,
                                   类型代码,
                                   类型名称,
                                   职称,
                                   职务,
                                   身份证号,
                                   工资合计)
                             values('{0}', 
                                    '{1}', 
                                    '{2}',
                                    '{3}', 
                                    '{4}', 
                                    '{5}', 
                                    '{6}', 
                                    '{7}', 
                                    '{8}', 
                                    '{9}', 
                                    '{10}', 
                                    '{11}', 
                                    '{12}'
                                   )",
                                       dr["期间"].ToString(),
                                       dr["部门代码"].ToString(),
                                       dr["部门名称"].ToString(),
                                       dr["职员代码"].ToString(),
                                       dr["职员姓名"].ToString(),
                                       dr["类别代码"].ToString(),
                                       dr["类别名称"].ToString(),
                                       dr["类型代码"].ToString(),
                                       dr["类型名称"].ToString(),
                                       dr["职称"].ToString(),
                                       dr["职务"].ToString(),
                                       dr["身份证号"].ToString(),
                                       dr["工资合计"].ToString()
                                       );
                        salary_alist.Add(salary_esql);
                    }
                    dr.Close();
                }
                //往oracle中插入数据
                get_yongyou.DAL.OracleBase.ExecuteSqlTran(salary_alist);




                //清空人力信息表数据
                string hr_sql = "delete from  CBHS.yongyou_hr_information";
                get_yongyou.DAL.OracleBase.ExecuteSql(hr_sql);

                ArrayList hr_alist = new ArrayList();
                string hr_esql = string.Empty;


                // 查询SQL 
                string sql_hr = string.Format(@"select 期间,
                                                       部门代码,
                                                       部门名称,
                                                       职员代码,
                                                       职员姓名,
                                                       类别代码,
                                                       类别名称,
                                                       类型代码,
                                                       类型名称,
                                                       职称,
                                                       职务,
                                                       身份证号
                                                       from 人力信息
                                                     where 期间+'01'>= CONVERT(varchar(100), DATEADD (MONTH , -3, getdate() ), 112)-03
                                                    order by 期间");
                using (SqlDataReader dr = get_yongyou.DAL.SQLDB.ExecuteReader(sql_hr))
                {
                    //从数据库循环读取机构信息
                    while (dr.Read())
                    {
                        hr_esql = string.Format(@"insert into CBHS.yongyou_hr_information
                                               (期间,
                                                部门代码,
                                                部门名称,
                                                职员代码,
                                                职员姓名,
                                                类别代码,
                                                类别名称,
                                                类型代码,
                                                类型名称,
                                                职称,
                                                职务,
                                                身份证号)
                                         values('{0}', 
                                                '{1}', 
                                                '{2}',
                                                '{3}', 
                                                '{4}', 
                                                '{5}', 
                                                '{6}', 
                                                '{7}', 
                                                '{8}', 
                                                '{9}', 
                                                '{10}', 
                                                '{11}'
                                               )",
                                       dr["期间"].ToString(),
                                       dr["部门代码"].ToString(),
                                       dr["部门名称"].ToString(),
                                       dr["职员代码"].ToString(),
                                       dr["职员姓名"].ToString(),
                                       dr["类别代码"].ToString(),
                                       dr["类别名称"].ToString(),
                                       dr["类型代码"].ToString(),
                                       dr["类型名称"].ToString(),
                                       dr["职称"].ToString(),
                                       dr["职务"].ToString(),
                                       dr["身份证号"].ToString()
                                       );
                        hr_alist.Add(hr_esql);
                    }
                    dr.Close();
                }
                //往oracle中插入数据
                get_yongyou.DAL.OracleBase.ExecuteSqlTran(hr_alist);





                //清空成本信息表数据
                string costs_sql = "delete from  CBHS.yongyou_costs_information";
                get_yongyou.DAL.OracleBase.ExecuteSql(costs_sql);

                ArrayList costs_alist = new ArrayList();
                string costs_esql = string.Empty;


                // 查询SQL 
                string sql_costs = string.Format(@"select 单位代码,
                                                   帐套号,
                                                   期间,
                                                   凭证日期,
                                                   摘要,
                                                   凭证号,
                                                   部门代码,
                                                   部门名称,
                                                   科目代码,
                                                   科目名称,
                                                   金额,
                                                   经济科目代码,
                                                   经济科目名称,
                                                   功能科目代码,
                                                   功能科目名称
                                              from 成本信息
                                              where  期间+'01'>= CONVERT(varchar(100), DATEADD (MONTH , -3, getdate() ), 112)-03
                                                     order by 期间");
                using (SqlDataReader dr = get_yongyou.DAL.SQLDB.ExecuteReader(sql_costs))
                {
                    //从数据库循环读取机构信息
                    while (dr.Read())
                    {
                        costs_esql = string.Format(@"insert into CBHS.yongyou_costs_information
                                                  (单位代码,
                                                   帐套号,
                                                   期间,
                                                   凭证日期,
                                                   摘要,
                                                   凭证号,
                                                   部门代码,
                                                   部门名称,
                                                   科目代码,
                                                   科目名称,
                                                   金额,
                                                   经济科目代码,
                                                   经济科目名称,
                                                   功能科目代码,
                                                   功能科目名称)
                                                values
                                                ('{0}', 
                                                '{1}', 
                                                '{2}',
                                                '{3}', 
                                                '{4}', 
                                                '{5}', 
                                                '{6}', 
                                                '{7}', 
                                                '{8}', 
                                                '{9}', 
                                                '{10}', 
                                                '{11}', 
                                                '{12}', 
                                                '{13}', 
                                                '{14}'
                                               )",
                                                   dr["单位代码"].ToString(),
                                                   dr["帐套号"].ToString(),
                                                   dr["期间"].ToString(),
                                                   dr["凭证日期"].ToString(),
                                                   dr["摘要"].ToString(),
                                                   dr["凭证号"].ToString(),
                                                   dr["部门代码"].ToString(),
                                                   dr["部门名称"].ToString(),
                                                   dr["科目代码"].ToString(),
                                                   dr["科目名称"].ToString(),
                                                   dr["金额"].ToString(),
                                                   dr["经济科目代码"].ToString(),
                                                   dr["经济科目名称"].ToString(),
                                                   dr["功能科目代码"].ToString(),
                                                   dr["功能科目名称"].ToString()
                                       );
                        costs_alist.Add(costs_esql);
                    }
                    dr.Close();
                }
                //往oracle中插入数据
                get_yongyou.DAL.OracleBase.ExecuteSqlTran(costs_alist);


                string update_sql = "update CBHS.yongyou_dept_dict set 部门代码 = trim(' ' from 部门代码)";
                int n = 0;
                n = get_yongyou.DAL.OracleBase.ExecuteSql(update_sql);
                if (n > 0)
                {
                    //Response.Write("<script language='javascript'>alert('提取成功！');</script>");
                    Goldnet.Ext.Web.Ext.Msg.Show(new Goldnet.Ext.Web.MessageBox.Config
                    {
                        Title = "信息提示",
                        Message = "提取成功",
                        Buttons = MessageBox.Button.OK,
                        Icon = (MessageBox.Icon)Enum.Parse(typeof(MessageBox.Icon), "INFO")
                    });
                }
            }
            catch (Exception ex)
            {

                //Response.Write("<script language='javascript'>alert(\"" + ex.Message + "\");</script>");
                Goldnet.Ext.Web.Ext.Msg.Show(new Goldnet.Ext.Web.MessageBox.Config
                {
                    Title = "信息提示",
                    Message = "提取失败",
                    Buttons = MessageBox.Button.OK,
                    Icon = (MessageBox.Icon)Enum.Parse(typeof(MessageBox.Icon), "INFO")
                });
            }



        }
    }
}
