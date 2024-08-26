using System;
using System.Data;
using System.Data.OleDb;
using System.Collections.Generic;
using System.Text;
using GoldNet.Comm.DAL.Oracle;
using GoldNet.Model;
using GoldNet.Comm;

namespace Goldnet.Dal
{
    public class SE_ROLE
    {
        /// <summary>
        /// 查询单项奖惩类别设置表数据
        /// </summary>
        /// <returns></returns>
        public DataTable GetSimpleEncourage()
        {
            string sql = "select ID,ITEMNAME,CHECKSTAN,REMARK from PERFORMANCE.SET_SINGLEAWARDDICT";
            DataSet ds = OracleOledbBase.ExecuteDataSet(sql.ToString());
            if (ds != null && ds.Tables.Count > 0)
            {
                return ds.Tables[0];
            }
            else
            {
                return BuildSimpleEncourage();
            }

        }
        /// <summary>
        /// 组合单项奖惩类别设置表结构
        /// </summary>
        /// <returns></returns>
        private DataTable BuildSimpleEncourage()
        {
            DataTable dt = new DataTable();
            DataColumn dcID = new DataColumn("ID");
            DataColumn dcITEMNAME = new DataColumn("ITEMNAME");
            DataColumn dcCHECKSTAN = new DataColumn("CHECKSTAN");
            DataColumn dcREMARK = new DataColumn("REMARK");
            dt.Columns.AddRange(new DataColumn[] { dcID, dcITEMNAME, dcCHECKSTAN, dcREMARK });
            return dt;
        }
        /// <summary>
        /// 没被特殊权限选择的角色
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public DataSet GetRoleListspe(string id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select a.ROLE_ID,a.ROLE_NAME");
            strSql.AppendFormat(" FROM {0}.SYS_ROLE_DICT a where a.ROLE_APP='-1' and a.role_id not in (select role_id from SYS_SPECPOWER_ROLE where id='{1}' and type={2})", DataUser.COMM, id, 3);
            strSql.Append(" order by a.role_id");
            return OracleOledbBase.ExecuteDataSet(strSql.ToString());

        }

        /// <summary>
        /// 选中的特殊角色
        /// </summary>
        /// <param name="id"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public DataSet GetSpeRole(string id)
        {
            string str = string.Format("select a.role_id,a.role_name from {0}.sys_role_dict a,{1}.SYS_SPECPOWER_ROLE b where a.role_id=b.role_id and b.id='{2}' and b.type={3}", DataUser.COMM, DataUser.COMM, id, 3);
            return OracleOledbBase.ExecuteDataSet(str);
        }

        /// <summary>
        /// 保存特殊角色
        /// </summary>
        /// <param name="rolelist"></param>
        /// <param name="id"></param>
        public void Saverolelist(List<GoldNet.Model.PageModels.roleselected> rolelist, string id)
        {
            MyLists listtable = new MyLists();
            string strdel = string.Format("delete {0}.SYS_SPECPOWER_ROLE where id=? and type={1}", DataUser.COMM, 3);
            List listdel = new List();
            listdel.StrSql = strdel;
            listdel.Parameters = new OleDbParameter[] { new OleDbParameter("", id) };
            listtable.Add(listdel);
            foreach (GoldNet.Model.PageModels.roleselected role in rolelist)
            {
                StringBuilder strSql = new StringBuilder();
                strSql.AppendFormat("insert into {0}.SYS_SPECPOWER_ROLE(", DataUser.COMM);
                strSql.Append("id,role_id,type)");
                strSql.Append(" values (");
                strSql.Append("?,?,?)");
                OleDbParameter[] parameteradd = {
											  new OleDbParameter("id",id),
											  new OleDbParameter("role_id", role.ROLE_ID),
	                                          new OleDbParameter("type",3)
										  };

                List listadd = new List();
                listadd.StrSql = strSql.ToString();
                listadd.Parameters = parameteradd;
                listtable.Add(listadd);
            }
            OracleOledbBase.ExecuteTranslist(listtable);
        }

        /// <summary>
        /// 单项奖惩
        /// </summary>
        /// <returns></returns>
        public DataTable GetSEFilter(User user)
        {
            string str = string.Format(@"SELECT   a.id, a.ITEMNAME
                        FROM PERFORMANCE.SET_SINGLEAWARDDICT a,
                             comm.sys_power_detail b,
                             comm.sys_specpower_role c
                       WHERE a.id = c.ID
                         AND c.TYPE = 3
                         AND c.role_id = b.power_id
                         AND UPPER (b.target_id) = '{0}'
                    ORDER BY a.id", user.UserId.ToUpper());
            return OracleOledbBase.ExecuteDataSet(str).Tables[0];
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        public string GetSEID(string userid)
        {
            string str = string.Format(@"SELECT   a.id, a.ITEMNAME
                        FROM PERFORMANCE.SET_SINGLEAWARDDICT a,
                             comm.sys_power_detail b,
                             comm.sys_specpower_role c
                       WHERE a.id = c.ID
                         AND c.TYPE = 3
                         AND c.role_id = b.power_id
                         AND UPPER (b.target_id) = '{0}'
                    ORDER BY a.id", userid.ToUpper());
            DataTable dt = OracleOledbBase.ExecuteDataSet(str).Tables[0];
            string id = "  in ( ";
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                id += dt.Rows[i]["id"].ToString() + ",";
            }
            id = id.Substring(0, id.Length - 1);
            id += ")";
            return id;
        }

        /// <summary>
        /// 核算科室菜单
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public DataTable GetSysMenulist(string id)
        {
            StringBuilder str = new StringBuilder();
            str.AppendFormat(@"  SELECT   A.APP_MENU_ID,
                                           A.APP_MENU_NAME,
                                           A.MODID,
                                           A.MENUID,
                                           B.APP_NAME,
                                           A.GROUPTEXT,
                                           A.FUNCTION_ID,
                                           C.TYPE_ID,
                                           C.TYPE_NAME,
                                           D.ATTR_ID,
                                           D.ATTR_NAME
                                    FROM   comm.SYS_APP_MENU_DICT a,
                                           comm.SYS_APPLICATION_SUBSYS b,
                                           COMM.SYS_APP_MENU_TYPE C,
                                           COMM.SYS_APP_MENU_ATTR D
                                   WHERE       A.MODID = B.APP_ID
                                           AND A.MENU_TYPE = C.TYPE_ID
                                           AND A.MENU_ATTR = D.ATTR_ID
                                           AND A.MENU_CLASS='0'
                                ");
            if (id != "")
            {
                str.AppendFormat(" and A.APP_MENU_ID={0}", id);
            }
            str.Append(" order by A.APP_MENU_ID");
            return OracleOledbBase.ExecuteDataSet(str.ToString()).Tables[0];
        }

        /// <summary>
        /// 组菜单
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public DataTable GetSysMenulistByGroup(string id)
        {
            StringBuilder str = new StringBuilder();
            str.AppendFormat(@"  SELECT   A.APP_MENU_ID,
                                           A.APP_MENU_NAME,
                                           A.MODID,
                                           A.MENUID,
                                           B.APP_NAME,
                                           A.GROUPTEXT,
                                           A.FUNCTION_ID,
                                           C.TYPE_ID,
                                           C.TYPE_NAME,
                                           D.ATTR_ID,
                                           D.ATTR_NAME
                                    FROM   comm.SYS_APP_MENU_DICT a,
                                           comm.SYS_APPLICATION_SUBSYS b,
                                           COMM.SYS_APP_MENU_TYPE C,
                                           COMM.SYS_APP_MENU_ATTR D
                                   WHERE       A.MODID = B.APP_ID
                                           AND A.MENU_TYPE = C.TYPE_ID
                                           AND A.MENU_ATTR = D.ATTR_ID
                                           AND A.MENU_CLASS='1'
                                ");
            if (id != "")
            {
                str.AppendFormat(" and A.APP_MENU_ID={0}", id);
            }
            str.Append(" order by A.APP_MENU_ID");
            return OracleOledbBase.ExecuteDataSet(str.ToString()).Tables[0];
        }

        /// <summary>
        /// 获取核算科室
        /// </summary>
        /// <param name="menuid"></param>
        /// <param name="depttype"></param>
        /// <returns></returns>
        public DataSet GetNoCheckDeptBymenuid(string menuid, string depttype)
        {
            StringBuilder str = new StringBuilder();

            str.AppendFormat(@"SELECT a.dept_code,a.dept_name 
                                 FROM {0}.sys_dept_dict a 
                                WHERE a.SHOW_FLAG='0' and attr='是' 
                                  AND a.dept_code  NOT IN (SELECT APP_MENU_DEPT FROM {0}.SYS_APP_MENU_DEPT b WHERE b.APP_MENU_ID={1})", DataUser.COMM, menuid);
            if (!depttype.Equals(string.Empty))
            {
                str.AppendFormat(" and a.dept_type ='{0}'", depttype);
            }
            str.Append(" order by a.dept_code");
            return OracleOledbBase.ExecuteDataSet(str.ToString());
        }

        /// <summary>
        /// 获取组
        /// </summary>
        /// <param name="menuid"></param>
        /// <param name="depttype"></param>
        /// <returns></returns>
        public DataSet GetNoCheckDeptBymenuidByGroup(string menuid, string depttype)
        {
            StringBuilder str = new StringBuilder();

            str.AppendFormat(@"SELECT a.dept_code,a.dept_name 
                                 FROM {0}.sys_dept_dict a 
                                WHERE a.SHOW_FLAG='0' AND dept_group='1'
                                  AND a.dept_code  NOT IN (SELECT APP_MENU_DEPT FROM {0}.SYS_APP_MENU_DEPT b WHERE b.APP_MENU_ID={1})", DataUser.COMM, menuid);
            if (!depttype.Equals(string.Empty))
            {
                str.AppendFormat(" and a.dept_type ='{0}'", depttype);
            }
            str.Append(" order by a.dept_code");
            return OracleOledbBase.ExecuteDataSet(str.ToString());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="menuid"></param>
        /// <returns></returns>
        public DataSet GetDeptBymenuid(string menuid)
        {
            string str = string.Format(@"SELECT a.dept_code,
                                                a.dept_name 
                                           FROM {0}.sys_dept_dict a,
                                                {0}.SYS_APP_MENU_DEPT b
                                        WHERE  a.dept_code=b.APP_MENU_DEPT  
                                          AND  b.APP_MENU_ID={1} 
                                        order by a.dept_code", DataUser.COMM, menuid);
            return OracleOledbBase.ExecuteDataSet(str);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="deptlist"></param>
        /// <param name="menuid"></param>
        public void SaveMenuDept(List<GoldNet.Model.PageModels.deptselected> deptlist, string menuid)
        {
            MyLists listttrans = new MyLists();
            OleDbParameter[] Parmsdel = new OleDbParameter[] { };
            //
            string strdel = string.Format(@"delete {0}.SYS_APP_MENU_DEPT a where a.APP_MENU_ID={1}", DataUser.COMM, menuid);

            List listdel = new List();
            listdel.StrSql = strdel;
            listdel.Parameters = Parmsdel;
            listttrans.Add(listdel);
            foreach (GoldNet.Model.PageModels.deptselected dept in deptlist)
            {
                string str = string.Format("insert into {0}.SYS_APP_MENU_DEPT (APP_MENU_ID,APP_MENU_DEPT) values (?,?)", DataUser.COMM);
                OleDbParameter[] cmdPara = new OleDbParameter[]{
															   new OleDbParameter("PROG_CODE",menuid),
															   new OleDbParameter("DEPT_CODE",dept.DEPT_CODE)
														   };
                List listadd = new List();
                listadd.StrSql = str;
                listadd.Parameters = cmdPara;
                listttrans.Add(listadd);
            }

            OracleOledbBase.ExecuteTranslist(listttrans);
        }

        /// <summary>
        /// 删除菜单
        /// </summary>
        /// <param name="appid"></param>
        /// <param name="modid"></param>
        /// <param name="menuid"></param>
        /// <param name="funcid"></param>
        public void DeleteAppMenu(string appid, string modid, string menuid, string funcid)
        {
            MyLists listttrans = new MyLists();
            //删除菜单字典
            string str = string.Format("delete comm.SYS_APP_MENU_DICT where APP_MENU_ID=?");
            OleDbParameter[] cmdPara = new OleDbParameter[]{
                                                                new OleDbParameter("APP_MENU_ID",appid)
														   };
            List listdict = new List();
            listdict.StrSql = str;
            listdict.Parameters = cmdPara;
            listttrans.Add(listdict);

            //删除科室
            string strdept = string.Format("delete comm.SYS_APP_MENU_DEPT where APP_MENU_ID=?");
            OleDbParameter[] deptPara = new OleDbParameter[]{
                                                                new OleDbParameter("APP_MENU_ID",appid)
														   };
            List listdept = new List();
            listdept.StrSql = strdept;
            listdept.Parameters = deptPara;
            listttrans.Add(listdept);

            //删除指标
            string strguide = string.Format("delete comm.SYS_APP_MENU_GUIDE where APP_MENU_ID=?");
            OleDbParameter[] guidePara = new OleDbParameter[]{
                                                                new OleDbParameter("APP_MENU_ID",appid)
														   };
            List listguide = new List();
            listguide.StrSql = strguide;
            listguide.Parameters = guidePara;
            listttrans.Add(listguide);

            //删除指标对照
            string strguidevs = string.Format("delete comm.SYS_APP_MENU_VS_GUIDE where APP_MENU_ID=?");
            OleDbParameter[] guidevsPara = new OleDbParameter[]{
                                                                new OleDbParameter("APP_MENU_ID",appid)
														   };
            List listguidevs = new List();
            listguidevs.StrSql = strguidevs;
            listguidevs.Parameters = guidevsPara;
            listttrans.Add(listguidevs);

            //删除菜单
            string strmenu = string.Format("delete comm.SYS_APPLICATION_MENU where MODID=? and MENUID=?");
            OleDbParameter[] menuPara = new OleDbParameter[]{
                                                                new OleDbParameter("MODID",modid),
                                                                new OleDbParameter("MENUID",menuid)
														   };
            List listmenu = new List();
            listmenu.StrSql = strmenu;
            listmenu.Parameters = menuPara;
            listttrans.Add(listmenu);

            //删除功能
            string strfun = string.Format("delete comm.SYS_FUNCTION_DICT where FUNCTION_ID=? and FUNCTION_TYPE=?");
            OleDbParameter[] funPara = new OleDbParameter[]{
                                                                new OleDbParameter("FUNCTION_ID",funcid),
                                                                new OleDbParameter("FUNCTION_TYPE",modid)
														   };
            List listfun = new List();
            listfun.StrSql = strfun;
            listfun.Parameters = funPara;
            listttrans.Add(listfun);

            //删除权限
            string strrole = string.Format("delete comm.SYS_ROLE_FUNCTION where FUNCTION_ID=? ");
            OleDbParameter[] rolePara = new OleDbParameter[]{
                                                                new OleDbParameter("FUNCTION_ID",funcid)
														   };
            List listrole = new List();
            listrole.StrSql = strrole;
            listrole.Parameters = rolePara;
            listttrans.Add(listrole);

            //删除下拉
            string stritemlist = string.Format("delete comm.SYS_APP_MENU_GUIDE_ITEM where APP_MENU_ID=? ");
            OleDbParameter[] itemlistPara = new OleDbParameter[]{
                                                                new OleDbParameter("APP_MENU_ID",appid)
														   };
            List listitemlist = new List();
            listitemlist.StrSql = stritemlist;
            listitemlist.Parameters = itemlistPara;
            listttrans.Add(listitemlist);

            //删除数据
            string strdata = string.Format("delete hospitalsys.SYS_MENU_DETAIL where APP_MENU_ID=? ");
            OleDbParameter[] dataPara = new OleDbParameter[]{
                                                                new OleDbParameter("APP_MENU_ID",appid)
														   };
            List listdata = new List();
            listdata.StrSql = strdata;
            listdata.Parameters = dataPara;
            listttrans.Add(listdata);
            OracleOledbBase.ExecuteTranslist(listttrans);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="appid"></param>
        /// <param name="menuid"></param>
        /// <returns></returns>
        public DataTable GetSysMenu(string appid, string menuid)
        {
            StringBuilder str = new StringBuilder();
            str.AppendFormat(@"select a.*,b.TYPE_NAME,c.TYPES ACCOUNT_TYPE_NAME,d.MENU_ATTR_NAME from (select m.*,n.guide_name from  comm.SYS_APP_MENU_GUIDE m,hospitalsys.guide_name_dict n where m.LINK_GUIDE_CODE=n.guide_code(+)) a,
             comm.SYS_APP_MENU_FILED_TYPE b,comm.SYS_APP_MENU_FILED_ACCOUNT c,COMM.SYS_APP_MENU_FILED_ATTR D
              where a.FIELD_TYPE=b.TYPE_ID and A.ACCOUNT_FLAGS=c.id AND A.MENU_ATTR=d.MENU_ATTR_ID");
            if (appid != "")
            {
                str.AppendFormat(" and a.APP_MENU_ID={0}", appid);
            }
            if (menuid != "")
            {
                str.AppendFormat(" and a.MENU_GUIDE_ID={0}", menuid);
            }
            str.Append(" order by a.APP_MENU_ID,a.MENU_SORT,a.MENU_GUIDE_ID");
            return OracleOledbBase.ExecuteDataSet(str.ToString()).Tables[0];
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="appid"></param>
        /// <returns></returns>
        public DataTable GetSysMenuGuide(string appid)
        {
            string strdict = string.Format(@"select b.guide_code,b.guide_name,A.ACCOUNT_FLAGS,A.DETAIL_FLAGS from comm.SYS_APP_MENU_VS_GUIDE a,hospitalsys.GUIDE_NAME_DICT b 
            where a.guide_code=b.guide_code and a.APP_MENU_ID={0} order by a.GUIDE_SORT", appid);
            DataTable tabledict = OracleOledbBase.ExecuteDataSet(strdict).Tables[0];

            return tabledict;
        }

        /// <summary>
        /// 字段下拉
        /// </summary>
        /// <param name="appid"></param>
        /// <param name="menuid"></param>
        /// <param name="name"></param>
        /// <param name="values"></param>
        /// <returns></returns>
        public DataTable GetMenuItem(string appid, string menuid, string name, string values)
        {
            StringBuilder str = new StringBuilder();
            str.AppendFormat("select * from comm.SYS_APP_MENU_GUIDE_ITEM where APP_MENU_ID={0} and MENU_GUIDE_ID={1} ", appid, menuid);
            if (name != "")
            {
                str.AppendFormat(" and ITEM_NAME='{0}'", name);
            }
            if (values != "")
            {
                str.AppendFormat(" and ITEM_VALUE={0}", values);
            }
            str.Append(" order by ITEM_ID");
            return OracleOledbBase.ExecuteDataSet(str.ToString()).Tables[0];
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="appid"></param>
        /// <param name="menuid"></param>
        public void DeleteMenu(string appid, string menuid)
        {
            MyLists listttrans = new MyLists();
            string stritemlist = string.Format("delete comm.SYS_APP_MENU_GUIDE_ITEM where APP_MENU_ID=? and MENU_GUIDE_ID=?");
            OleDbParameter[] itemlistPara = new OleDbParameter[]{
                                                                new OleDbParameter("APP_MENU_ID",appid),
                                                                 new OleDbParameter("MENU_GUIDE_ID",menuid)
														   };
            List listitemlist = new List();
            listitemlist.StrSql = stritemlist;
            listitemlist.Parameters = itemlistPara;
            listttrans.Add(listitemlist);

            string str = string.Format("delete comm.SYS_APP_MENU_GUIDE where APP_MENU_ID=? and  MENU_GUIDE_ID=?");
            OleDbParameter[] delPara = new OleDbParameter[]{
                                                                new OleDbParameter("APP_MENU_ID",appid),
                                                                 new OleDbParameter("MENU_GUIDE_ID",menuid)
														   };
            List listdel = new List();
            listdel.StrSql = str;
            listdel.Parameters = delPara;
            listttrans.Add(listdel);
            OracleOledbBase.ExecuteTranslist(listttrans);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="appid"></param>
        /// <param name="menuname"></param>
        /// <returns></returns>
        public DataTable GetSystemMenuByName(string appid, string menuname)
        {
            string str = string.Format("select * from SYS_APP_MENU_GUIDE where APP_MENU_ID={0} and MENU_GUIDE_NAME='{1}'", appid, menuname);
            return OracleOledbBase.ExecuteDataSet(str).Tables[0];
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="appid"></param>
        /// <param name="menuname"></param>
        /// <param name="shownum"></param>
        /// <param name="filedtype"></param>
        /// <param name="accounttype"></param>
        /// <param name="menuattr"></param>
        public void SaveSysMenu(string appid, string menuname, string shownum, string filedtype, string accounttype, string menuattr,string guidecode)
        {
            string selmenuid = string.Format("select nvl(max(MENU_GUIDE_ID)+1,'1') menuid from comm.SYS_APP_MENU_GUIDE where APP_MENU_ID={0}", appid);
            string menuid = OracleOledbBase.ExecuteScalar(selmenuid).ToString();

            string str = string.Format("insert into {0}.SYS_APP_MENU_GUIDE (APP_MENU_ID,MENU_GUIDE_ID,MENU_GUIDE_NAME,SHOW_WIDTH,FIELD_TYPE,ACCOUNT_FLAGS,MENU_ATTR,LINK_GUIDE_CODE) values (?,?,?,?,?,?,?,?)", DataUser.COMM);
            OleDbParameter[] cmdPara = new OleDbParameter[]{
															   new OleDbParameter("APP_MENU_ID",appid),
															   new OleDbParameter("MENU_GUIDE_ID",menuid),
                                                               new OleDbParameter("MENU_GUIDE_NAME",menuname),
                                                                new OleDbParameter("SHOW_WIDTH",shownum),
                                                                new OleDbParameter("FIELD_TYPE",filedtype),
                                                                new OleDbParameter("ACCOUNT_FLAGS",accounttype),
                                                                new OleDbParameter("MENU_ATTR",menuattr),
                                                                new OleDbParameter("LINK_GUIDE_CODE",guidecode)
														   };
            OracleOledbBase.ExecuteNonQuery(str, cmdPara);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="menuid"></param>
        /// <param name="menuname"></param>
        /// <param name="shownum"></param>
        /// <param name="filedtype"></param>
        /// <param name="accounttype"></param>
        /// <param name="menuattr"></param>
        /// <param name="appid"></param>
        public void UpdateSysMenu(string menuid, string menuname, string shownum, string filedtype, string accounttype, string menuattr, string appid,string guidecode)
        {
            string str = string.Format("update comm.SYS_APP_MENU_GUIDE set MENU_GUIDE_NAME=?,SHOW_WIDTH=?,FIELD_TYPE=?,ACCOUNT_FLAGS=?,MENU_ATTR=?,LINK_GUIDE_CODE=? where APP_MENU_ID=? and  MENU_GUIDE_ID=?");
            OleDbParameter[] cmdPara = new OleDbParameter[]{
                                                               new OleDbParameter("MENU_GUIDE_NAME",menuname),
                                                                new OleDbParameter("SHOW_WIDTH",shownum),
                                                                new OleDbParameter("FIELD_TYPE",filedtype),
                                                                 new OleDbParameter("ACCOUNT_FLAGS",accounttype),
                                                                 new OleDbParameter("MENU_ATTR",menuattr),
                                                                 new OleDbParameter("LINK_GUIDE_CODE",guidecode),
                                                                 new OleDbParameter("APP_MENU_ID",appid),
                                                                new OleDbParameter("MENU_GUIDE_ID",menuid)
														   };
            OracleOledbBase.ExecuteNonQuery(str, cmdPara);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="appid"></param>
        /// <param name="mendid"></param>
        /// <returns></returns>
        public DataTable GetMenuItem(string appid, string mendid)
        {
            string str = string.Format(@"select * from comm.SYS_APP_MENU_GUIDE_ITEM where APP_MENU_ID={0} and MENU_GUIDE_ID={1} order by ITEM_ID", appid, mendid);
            return OracleOledbBase.ExecuteDataSet(str).Tables[0];
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="selectRow"></param>
        /// <param name="appid"></param>
        /// <param name="menuid"></param>
        public void SaveMenuItem(Dictionary<string, string>[] selectRow, string appid, string menuid)
        {
            Goldnet.Dal.SYS_DEPT_DICT deptdal = new SYS_DEPT_DICT();
            string delsql = string.Format("delete  comm.SYS_APP_MENU_GUIDE_ITEM where APP_MENU_ID={0} and MENU_GUIDE_ID={1}", appid, menuid);
            MyLists listtable = new MyLists();
            List listcenterdict = new List();
            listcenterdict.StrSql = delsql;
            listcenterdict.Parameters = new OleDbParameter[] { };
            listtable.Add(listcenterdict);

            for (int i = 0; i < selectRow.Length; i++)
            {
                if (selectRow[i]["ITEM_NAME"].ToString() != "" && selectRow[i]["ITEM_VALUE"].ToString() != "")
                {
                    StringBuilder isql = new StringBuilder();
                    isql.Append(" insert into comm.SYS_APP_MENU_GUIDE_ITEM ( ");
                    isql.Append("APP_MENU_ID,MENU_GUIDE_ID,ITEM_ID,ITEM_NAME,ITEM_VALUE");
                    isql.Append(") values (");
                    isql.Append("" + appid + ",");
                    isql.Append("" + menuid + ",");

                    isql.Append("" + selectRow[i]["ITEM_ID"].ToString() + ",");
                    isql.Append("'" + selectRow[i]["ITEM_NAME"].ToString() + "',");
                    isql.Append("" + selectRow[i]["ITEM_VALUE"].ToString() + "");
                    isql.Append(") ");
                    List listcenterdetail = new List();
                    listcenterdetail.StrSql = isql.ToString();
                    listcenterdetail.Parameters = new OleDbParameter[] { };
                    listtable.Add(listcenterdetail);
                }
            }
            OracleOledbBase.ExecuteTranslist(listtable);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="selectRow"></param>
        public void SaveMenuAttr(Dictionary<string, string>[] selectRow)
        {
            Goldnet.Dal.SYS_DEPT_DICT deptdal = new SYS_DEPT_DICT();
            string delsql = string.Format("delete  comm.SYS_APP_MENU_ATTR");
            MyLists listtable = new MyLists();
            List listcenterdict = new List();
            listcenterdict.StrSql = delsql;
            listcenterdict.Parameters = new OleDbParameter[] { };
            listtable.Add(listcenterdict);

            for (int i = 0; i < selectRow.Length; i++)
            {
                if (selectRow[i]["ATTR_NAME"].ToString() != "" && selectRow[i]["ATTR_NAME"].ToString() != "")
                {
                    StringBuilder isql = new StringBuilder();
                    isql.Append(" insert into comm.SYS_APP_MENU_ATTR ( ");
                    isql.Append("ATTR_ID,ATTR_NAME");
                    isql.Append(") values (");
                    isql.Append("" + selectRow[i]["ATTR_ID"].ToString() + ",");
                    isql.Append("'" + selectRow[i]["ATTR_NAME"].ToString() + "'");
                    isql.Append(") ");
                    List listcenterdetail = new List();
                    listcenterdetail.StrSql = isql.ToString();
                    listcenterdetail.Parameters = new OleDbParameter[] { };
                    listtable.Add(listcenterdetail);
                }
            }
            OracleOledbBase.ExecuteTranslist(listtable);

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="apptype"></param>
        /// <param name="mentype"></param>
        /// <param name="menuname"></param>
        /// <param name="menutype"></param>
        /// <param name="menuattr"></param>
        /// <param name="menutypename"></param>
        public void Saveappmenu(string apptype, string mentype, string menuname, string menutype, string menuattr, string menutypename, string menuclass)
        {
            MyLists listttrans = new MyLists();
            int appid = OracleOledbBase.GetMaxID("APP_MENU_ID", "Comm.SYS_APP_MENU_DICT");
            string seloldmenu = string.Format("select nvl(max(MENUID),'000000') menuid from comm.SYS_APPLICATION_MENU where MODID='{0}' and GROUPTEXT='{1}'", apptype, mentype);
            string oldmenuid = OracleOledbBase.ExecuteScalar(seloldmenu).ToString();
            string menuid = (Convert.ToInt32(oldmenuid) + 1).ToString().PadLeft(6, '0');
            string funcid = OracleOledbBase.ExecuteScalar(string.Format("select max(FUNCTION_ID)+1 from comm.SYS_FUNCTION_DICT where FUNCTION_TYPE='{0}'", apptype)).ToString();
            //插入菜单字典
            string str = string.Format("insert into {0}.SYS_APP_MENU_DICT (APP_MENU_ID,APP_MENU_NAME,MODID,MENUID,GROUPTEXT,FUNCTION_ID,MENU_TYPE,MENU_ATTR,MENU_CLASS) values (?,?,?,?,?,?,?,?,?)", DataUser.COMM);
            OleDbParameter[] cmdPara = new OleDbParameter[]{
															   new OleDbParameter("APP_MENU_ID",appid),
															   new OleDbParameter("APP_MENU_NAME",menuname),
                                                               new OleDbParameter("MODID",apptype),
                                                                new OleDbParameter("MENUID",menuid),
                                                                new OleDbParameter("GROUPTEXT",mentype),
                                                                new OleDbParameter("FUNCTION_ID",funcid),
                                                                new OleDbParameter("MENU_TYPE",menutype),
                                                                new OleDbParameter("MENU_ATTR",menuattr),
                                                                new OleDbParameter("MENU_CLASS",menuclass)
														   };
            List listadd = new List();
            listadd.StrSql = str;
            listadd.Parameters = cmdPara;
            listttrans.Add(listadd);

            string sortid = string.Format("select nvl(max(SORTNO)+1,'0') SORTNO from comm.SYS_APPLICATION_MENU where MODID='{0}' and GROUPTEXT='{1}'", apptype, mentype);
            string sort = OracleOledbBase.ExecuteScalar(sortid).ToString();
            //插入菜单
            string pageurl = "/WebPage/SysManager/Sys_Menu_Detail.aspx?appmenuid=" + appid.ToString();
            if (menutype == "2")
                pageurl = "/WebPage/SysManager/Sys_Menu_Detail_Search.aspx?appmenuid=" + appid.ToString();
            string strmenu = string.Format(@"Insert into COMM.SYS_APPLICATION_MENU
                                   (MENUID, MENUNAME, MODID, GROUPTEXT, PAGENAME, PAGETITLE, PAGEURL, OPTIONRULE, SORTNO, OPENSTATUS, OPENMODE, ICO, VISIBLE, CREATE_DATE, DEL_F, FUNCTION_ID, PERSONS_FLAGS) values 
                                    (?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?)");
            OleDbParameter[] menuPara = new OleDbParameter[]{
															   new OleDbParameter("MENUID",menuid),
															   new OleDbParameter("MENUNAME",menuname),
                                                               new OleDbParameter("MODID",apptype),
                                                                new OleDbParameter("GROUPTEXT",mentype),
                                                                new OleDbParameter("PAGENAME","Sys_Menu_Detail"),
                                                                new OleDbParameter("PAGETITLE",menuname),
                                                                 new OleDbParameter("PAGEURL",pageurl),
                                                                 new OleDbParameter("OPTIONRULE","1111111111"),
                                                                 new OleDbParameter("SORTNO",sort),
                                                                 new OleDbParameter("OPENSTATUS","0"),
                                                                 new OleDbParameter("OPENMODE","0"),
                                                                 new OleDbParameter("ICO","Clipboard"),
                                                                 new OleDbParameter("VISIBLE","1"),
                                                                 new OleDbParameter("CREATE_DATE",System.DateTime.Now),
                                                                 new OleDbParameter("DEL_F","0"),
                                                                 new OleDbParameter("FUNCTION_ID",funcid),
                                                                 new OleDbParameter("PERSONS_FLAGS","0")
														   };
            List listaddmenu = new List();
            listaddmenu.StrSql = strmenu;
            listaddmenu.Parameters = menuPara;
            listttrans.Add(listaddmenu);
            //插入功能
            string strfun = string.Format(@"insert into comm.SYS_FUNCTION_DICT (FUNCTION_ID,FUNCTION_NAME,FUNCTION_TYPE,DEL_F) values (?,?,?,?)");
            OleDbParameter[] funPara = new OleDbParameter[]{
															   new OleDbParameter("FUNCTION_ID",funcid),
															   new OleDbParameter("FUNCTION_NAME",menutypename+"："+menuname),
                                                               new OleDbParameter("FUNCTION_TYPE",apptype),
                                                                new OleDbParameter("DEL_F","0")
														   };
            List listaddfun = new List();
            listaddfun.StrSql = strfun;
            listaddfun.Parameters = funPara;
            listttrans.Add(listaddfun);
            OracleOledbBase.ExecuteTranslist(listttrans);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="appid"></param>
        /// <param name="apptype"></param>
        /// <param name="mentype"></param>
        /// <param name="menuname"></param>
        /// <param name="funcid"></param>
        /// <param name="menuid"></param>
        /// <param name="menuattr"></param>
        /// <param name="menutypename"></param>
        public void Updateappmenu(string appid, string apptype, string mentype, string menuname, string funcid, string menuid, string menuattr, string menutypename)
        {
            MyLists listttrans = new MyLists();
            //更新字典
            string str = string.Format("update comm.SYS_APP_MENU_DICT set APP_MENU_NAME=?,MENU_ATTR=? where APP_MENU_ID=?");
            OleDbParameter[] cmdPara = new OleDbParameter[]{
                                                               new OleDbParameter("APP_MENU_NAME",menuname),
                                                               new OleDbParameter("MENU_ATTR",menuattr),
                                                                new OleDbParameter("APP_MENU_ID",appid)
														   };
            List listdict = new List();
            listdict.StrSql = str;
            listdict.Parameters = cmdPara;
            listttrans.Add(listdict);
            //更新菜单
            string strmenu = string.Format("update comm.SYS_APPLICATION_MENU set MENUNAME=?,PAGETITLE=? where MENUID=? and MODID=?");
            OleDbParameter[] menuPara = new OleDbParameter[]{
                                                               new OleDbParameter("MENUNAME",menuname),
                                                                new OleDbParameter("PAGETITLE",menuname),
                                                                new OleDbParameter("MENUID",menuid),
                                                                new OleDbParameter("MODID",apptype)
														   };
            List listmenu = new List();
            listmenu.StrSql = strmenu;
            listmenu.Parameters = menuPara;
            listttrans.Add(listmenu);
            //更新功能
            string strfun = string.Format("update comm.SYS_FUNCTION_DICT set FUNCTION_NAME=? where FUNCTION_ID=? and FUNCTION_TYPE=?");
            OleDbParameter[] funPara = new OleDbParameter[]{
                                                               new OleDbParameter("FUNCTION_NAME",menutypename+"："+menuname),
                                                                new OleDbParameter("FUNCTION_ID",funcid),
                                                                new OleDbParameter("FUNCTION_TYPE",apptype)
														   };
            List listfun = new List();
            listfun.StrSql = strfun;
            listfun.Parameters = funPara;
            listttrans.Add(listfun);
            OracleOledbBase.ExecuteTranslist(listttrans);
        }

        /// <summary>
        /// 科室关系SQL
        /// </summary>
        /// <returns></returns>
        public DataSet DeptStore()
        {
            return OracleOledbBase.ExecuteDataSet(string.Format("SELECT '00' AS ID, '公共部分' AS TEXT FROM DUAL  UNION SELECT DEPT_CLASS_CODE ID,DEPT_CLASS_NAME TEXT FROM {0}.JXGL_GUIDE_DEPT_CLASS_DICT", DataUser.HOSPITALSYS));
        }

        /// <summary>
        /// 设置报表已选指标
        /// </summary>
        /// <returns></returns>
        public DataSet getReportGuideExCol(string rptid)
        {

            StringBuilder sql = new StringBuilder();

            sql.Append("SELECT b.guide_code as VALUE, b.guide_name as TEXT ");
            sql.Append("FROM comm.SYS_APP_MENU_VS_GUIDE a, hospitalsys.guide_name_dict b ");
            sql.Append("WHERE a.guide_code = b.guide_code and  APP_MENU_ID=");
            sql.Append(rptid);
            sql.Append(" order by a.GUIDE_SORT");
            return OracleOledbBase.ExecuteDataSet(sql.ToString());
        }
        public DataSet getReportGuidebyappid(string appid, string guidecode)
        {

            StringBuilder sql = new StringBuilder();

            sql.Append("SELECT b.guide_code as VALUE, b.guide_name as TEXT ");
            sql.Append("FROM comm.SYS_APP_MENU_VS_GUIDE_DETAIL a, hospitalsys.guide_name_dict b ");
            sql.AppendFormat("WHERE a.GUIDE_DETAIL = b.guide_code and a.GUIDE_CODE='{0}' and  APP_MENU_ID={1} ", guidecode, appid);
            return OracleOledbBase.ExecuteDataSet(sql.ToString());
        }
        public string getGuidebyappid(string appid, string guidecode)
        {

            StringBuilder sql = new StringBuilder();
            sql.AppendFormat("select ACCOUNT_FLAGS from comm.SYS_APP_MENU_VS_GUIDE a where   a.GUIDE_CODE='{0}' and  a.APP_MENU_ID={1} ", guidecode, appid);
            return OracleOledbBase.ExecuteScalar(sql.ToString()).ToString();
        }
        /// <summary>
        /// 设置报表指标初始化树结构
        /// </summary>
        /// <param name="depttype">部门类别</param>
        /// <param name="OrganType">组织类别</param>
        /// <returns></returns>
        public DataSet getSetReportGuideTreeBuilder(string depttype)
        {
            StringBuilder str = new StringBuilder();
            str.AppendFormat(@" SELECT A.name,a.id,a.pid FROM 
                                    (
                                    select  c.NAME,C.ID,LEVEL glevel,connect_by_isleaf LEAF ,c.pid
                                    from ( select BSC_CLASS_NAME NAME, BSC_CLASS_CODE ID ,  
                                    case when length(B.BSC_CLASS_CODE) =2 then '0'  else substr(B.BSC_CLASS_CODE,1,length(B.BSC_CLASS_CODE)- 2)    end as pid,     '' dept,'' organ,'' isexpress,'' ispage,'' issame,'' ishighguide,'' issel,'' isabs,'' isszpj   from hospitalsys.JXGL_GUIDE_BSC_CLASS_DICT   b   
                                    union all  
                                    select guide_name name , guide_code code ,bsc pid , dept,organ,isexpress,ispage,issame,ishighguide,issel,isabs,isszpj 
                                    from  hospitalsys.guide_name_dict a where a.ispage = '1' and case when a.DEPT = '00' THEN '{0}' ELSE a.DEPT END = '{0}' ) c    
                                    start with pid= '0'  connect by prior ID = pid 
                                     )
                                     A
                                     WHERE 
                                           A.glevel <> 3
                                          and  (A.glevel <> 2 or A.LEAF <> 1)   ", depttype);
            return OracleOledbBase.ExecuteDataSet(str.ToString());
        }
        /// <summary>
        /// 根据科室查询指标
        /// </summary>
        /// <param name="organ">组织类别</param>
        /// <param name="dept">部门</param>
        /// <param name="bsc">父部门</param>
        /// <param name="Search">查询条件</param>
        /// <returns></returns>
        public DataSet getReportSearchGuide(string dept, string bsc, string Search)
        {

            StringBuilder sql = new StringBuilder();
            sql.Append("select guide_name as TEXT , guide_code as VALUE ,bsc,dept,organ ");
            sql.Append("from hospitalsys.guide_name_dict ");

            sql.Append(" where ( dept =");
            sql.Append(dept);
            sql.Append(" or dept = '00')");
            if (Search.Equals(""))
            {
                if (bsc != "")
                {
                    sql.Append(" AND bsc =");
                    sql.Append(bsc);
                }
            }
            else
            {
                sql.Append(" AND guide_name like '%");
                sql.Append(Search);
                sql.Append("%'");
            }
            sql.Append(" AND ispage = '1' and ORGAN='02' ORDER BY GUIDE_NAME");
            return OracleOledbBase.ExecuteDataSet(sql.ToString());
        }

        public void SaveSysmenuguide(DataTable tb, string appid)
        {
            MyLists listttrans = new MyLists();
            OleDbParameter[] Parmsdel = new OleDbParameter[] { };
            OleDbParameter[] Parmsdeldetail = new OleDbParameter[] { };
            //
            string strdel = string.Format(@"delete {0}.SYS_APP_MENU_VS_GUIDE a where a.APP_MENU_ID={1}", DataUser.COMM, appid);
            List listdel = new List();
            listdel.StrSql = strdel;
            listdel.Parameters = Parmsdel;
            listttrans.Add(listdel);

            string strdeldetail = string.Format(@"delete {0}.SYS_APP_MENU_VS_GUIDE_DETAIL a where a.APP_MENU_ID={1}", DataUser.COMM, appid);
            List listdeldetail = new List();
            listdeldetail.StrSql = strdeldetail;
            listdeldetail.Parameters = Parmsdeldetail;
            listttrans.Add(listdeldetail);
            string strguidedetail = string.Format(@"select * from comm.SYS_APP_MENU_VS_GUIDE_DETAIL where APP_MENU_ID={0}", appid);
            DataTable guidedetailtb = OracleOledbBase.ExecuteDataSet(strguidedetail).Tables[0];
            for (int i = 0; i < tb.Rows.Count; i++)
            {
                string accountflags = "0";
                string detailflags = "0";
                string strguide = string.Format(@"select * from comm.SYS_APP_MENU_VS_GUIDE where APP_MENU_ID={0} and GUIDE_CODE='{1}'", appid, tb.Rows[i]["VALUE"].ToString());
                DataTable guidb = OracleOledbBase.ExecuteDataSet(strguide).Tables[0];
                if (guidb.Rows.Count > 0)
                {
                    accountflags = guidb.Rows[0]["ACCOUNT_FLAGS"].ToString();
                    detailflags = guidb.Rows[0]["DETAIL_FLAGS"].ToString();
                }
                string str = string.Format("insert into {0}.SYS_APP_MENU_VS_GUIDE (APP_MENU_ID,GUIDE_CODE,ACCOUNT_FLAGS,DETAIL_FLAGS,GUIDE_SORT) values (?,?,?,?,?)", DataUser.COMM);
                OleDbParameter[] cmdPara = new OleDbParameter[]{
															   new OleDbParameter("APP_MENU_ID",appid),
															   new OleDbParameter("GUIDE_CODE",tb.Rows[i]["VALUE"].ToString()),
                                                                new OleDbParameter("ACCOUNT_FLAGS",accountflags),
                                                                 new OleDbParameter("DETAIL_FLAGS",detailflags),
                                                               new OleDbParameter("GUIDE_SORT",tb.Rows[i]["INDEX"].ToString())
														   };
                List listadd = new List();
                listadd.StrSql = str;
                listadd.Parameters = cmdPara;
                listttrans.Add(listadd);
                for (int j = 0; j < guidedetailtb.Rows.Count; j++)
                {
                    if (tb.Rows[i]["VALUE"].ToString().Equals(guidedetailtb.Rows[j]["GUIDE_CODE"].ToString()))
                    {
                        string strdetails = string.Format("insert into {0}.SYS_APP_MENU_VS_GUIDE_DETAIL (APP_MENU_ID,GUIDE_CODE,GUIDE_DETAIL) values (?,?,?)", DataUser.COMM);
                        OleDbParameter[] cmdParadetail = new OleDbParameter[]{
															   new OleDbParameter("APP_MENU_ID",appid),
															   new OleDbParameter("GUIDE_CODE",tb.Rows[i]["VALUE"].ToString()),
                                                                new OleDbParameter("GUIDE_DETAIL",guidedetailtb.Rows[j]["GUIDE_DETAIL"].ToString())
                                                                
														   };
                        List listadddetail = new List();
                        listadddetail.StrSql = strdetails;
                        listadddetail.Parameters = cmdParadetail;
                        listttrans.Add(listadddetail);
                    }
                }
            }

            OracleOledbBase.ExecuteTranslist(listttrans);
        }

        public void SaveSysmenuguidedetail(DataTable tb, string appid, string guidecode, string accountflags)
        {
            MyLists listttrans = new MyLists();
            OleDbParameter[] Parmsdel = new OleDbParameter[] { };
            OleDbParameter[] Parmsup = new OleDbParameter[] { };

            //
            string strdel = string.Format(@"delete {0}.SYS_APP_MENU_VS_GUIDE_DETAIL a where a.APP_MENU_ID={1} and a.GUIDE_CODE='{2}'", DataUser.COMM, appid, guidecode);
            List listdel = new List();
            listdel.StrSql = strdel;
            listdel.Parameters = Parmsdel;
            listttrans.Add(listdel);
            string detailflag = "0";
            if (tb.Rows.Count > 0)
                detailflag = "1";
            string strup = string.Format(@"update comm.SYS_APP_MENU_VS_GUIDE set ACCOUNT_FLAGS={0},DETAIL_FLAGS={1} where APP_MENU_ID={2} and GUIDE_CODE='{3}'", accountflags, detailflag, appid, guidecode);
            List listup = new List();
            listup.StrSql = strup;
            listup.Parameters = Parmsup;
            listttrans.Add(listup);
            for (int i = 0; i < tb.Rows.Count; i++)
            {

                string strdetails = string.Format("insert into {0}.SYS_APP_MENU_VS_GUIDE_DETAIL (APP_MENU_ID,GUIDE_CODE,GUIDE_DETAIL) values (?,?,?)", DataUser.COMM);
                OleDbParameter[] cmdParadetail = new OleDbParameter[]{
															   new OleDbParameter("APP_MENU_ID",appid),
															   new OleDbParameter("GUIDE_CODE",guidecode),
                                                                new OleDbParameter("GUIDE_DETAIL",tb.Rows[i]["VALUE"].ToString())
                                                                
														   };
                List listadddetail = new List();
                listadddetail.StrSql = strdetails;
                listadddetail.Parameters = cmdParadetail;
                listttrans.Add(listadddetail);

            }

            OracleOledbBase.ExecuteTranslist(listttrans);
        }

        /// <summary>
        /// 获取主菜单
        /// </summary>
        /// <returns></returns>
        public DataTable GetApptype()
        {
            string str = string.Format("select APP_ID,APP_NAME from comm.SYS_APPLICATION_SUBSYS where DEL_F='0'");
            return OracleOledbBase.ExecuteDataSet(str).Tables[0];
        }

        /// <summary>
        /// 菜单类型
        /// </summary>
        /// <returns></returns>
        public DataTable GetMenutype()
        {
            string str = string.Format("select TYPE_ID,TYPE_NAME from comm.SYS_APP_MENU_TYPE");
            return OracleOledbBase.ExecuteDataSet(str).Tables[0];
        }

        /// <summary>
        /// 获取菜单属性
        /// </summary>
        /// <returns></returns>
        public DataTable GetMenuattr()
        {
            string str = string.Format("select ATTR_ID,ATTR_NAME from comm.SYS_APP_MENU_ATTR order by ATTR_ID");
            return OracleOledbBase.ExecuteDataSet(str).Tables[0];
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public DataTable GetFiledtype()
        {
            string str = string.Format("select TYPE_ID,TYPE_NAME from comm.SYS_APP_MENU_FILED_TYPE order by type_id");
            return OracleOledbBase.ExecuteDataSet(str).Tables[0];
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public DataTable GetFiledaccount()
        {
            string str = string.Format("select ID,TYPES from comm.SYS_APP_MENU_FILED_ACCOUNT order by ID");
            return OracleOledbBase.ExecuteDataSet(str).Tables[0];
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public DataTable GetFiledattr()
        {
            string str = string.Format("select MENU_ATTR_ID,MENU_ATTR_NAME from comm.SYS_APP_MENU_FILED_ATTR order by MENU_ATTR_ID");
            return OracleOledbBase.ExecuteDataSet(str).Tables[0];
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="appid"></param>
        /// <returns></returns>
        public DataTable GetMenutype(string appid)
        {
            string str = string.Format("select distinct GROUPTEXT from comm.SYS_APPLICATION_MENU where MODID='{0}'", appid);
            return OracleOledbBase.ExecuteDataSet(str).Tables[0];
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="appid"></param>
        /// <returns></returns>
        public DataTable GetGuideByappid(string appid)
        {
            string str = string.Format("select b.guide_code,b.guide_name from comm.SYS_APP_MENU_VS_GUIDE a,hospitalsys.GUIDE_NAME_DICT b where a.guide_code=b.guide_code and a.APP_MENU_ID={0} order by a.GUIDE_SORT", appid);
            return OracleOledbBase.ExecuteDataSet(str).Tables[0];
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <returns></returns>
        private string BuildDate(string year, string month)
        {
            if (month.Length == 1)
            {
                month = "0" + month;
            }
            return year + "" + month + "" + "01";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <param name="appid"></param>
        /// <param name="deptcode"></param>
        /// <returns></returns>
        public DataTable GetMenuDetail(string year, string month, string appid, string deptcode)
        {
            string date = BuildDate(year, month);
            string strdict = string.Format("select MENU_GUIDE_ID,MENU_GUIDE_NAME,FIELD_TYPE,ACCOUNT_FLAGS from comm.SYS_APP_MENU_GUIDE where APP_MENU_ID={0} order by MENU_SORT,MENU_GUIDE_ID", appid);
            DataTable tabledict = OracleOledbBase.ExecuteDataSet(strdict).Tables[0];

            StringBuilder str = new StringBuilder();
            str.AppendFormat("SELECT A.APP_MENU_DEPT dept_code,a.dept_name \"科室名称\"");
            for (int i = 0; i < tabledict.Rows.Count; i++)
            {
                if (tabledict.Rows[i]["FIELD_TYPE"].ToString().Equals("0") && tabledict.Rows[i]["ACCOUNT_FLAGS"].ToString().Equals("1"))
                {
                    str.AppendFormat(" ,sum(DECODE (MENU_GUIDE_ID, {0}, b.MENU_GUIDE_VALUE,0)) \"{1}\"", tabledict.Rows[i]["MENU_GUIDE_ID"].ToString(), tabledict.Rows[i]["MENU_GUIDE_NAME"].ToString());
                }
                else
                {
                    str.AppendFormat(" ,to_char(sum(DECODE (MENU_GUIDE_ID, {0}, b.MENU_GUIDE_VALUE,0))) \"{1}\"", tabledict.Rows[i]["MENU_GUIDE_ID"].ToString(), tabledict.Rows[i]["MENU_GUIDE_NAME"].ToString());
                }
            }
            str.AppendFormat(",B.GUIDE_MEMO \"备注\" ");
            str.AppendFormat(@" FROM  (select m.*,n.dept_name from  comm.SYS_APP_MENU_DEPT m,comm.sys_dept_dict n where m.APP_MENU_DEPT=n.dept_code and m.APP_MENU_ID={0}) a LEFT JOIN
            HOSPITALSYS.SYS_MENU_DETAIL b
         ON A.APP_MENU_ID = B.APP_MENU_ID AND A.APP_MENU_DEPT = B.DEPT_CODE and to_char(b.ST_DATE,'yyyymmdd')='{1}'", appid, date);

            if (deptcode != "")
            {
                str.AppendFormat(" where A.APP_MENU_DEPT IN ({0})", deptcode);
            }
            str.Append("  group by a.APP_MENU_DEPT,a.dept_name,B.GUIDE_MEMO order by a.dept_name");
            return OracleOledbBase.ExecuteDataSet(str.ToString()).Tables[0];
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <param name="appid"></param>
        /// <param name="deptcode"></param>
        /// <returns></returns>
        public DataTable GetMenuDetailSearch(string year, string month, string appid, string deptcode)
        {
            string date = BuildDate(year, month);
            string strdict = string.Format(@"select b.guide_code,b.guide_name,A.ACCOUNT_FLAGS from comm.SYS_APP_MENU_VS_GUIDE a,hospitalsys.GUIDE_NAME_DICT b 
            where a.guide_code=b.guide_code and a.APP_MENU_ID={0} order by a.GUIDE_SORT", appid);
            DataTable tabledict = OracleOledbBase.ExecuteDataSet(strdict).Tables[0];
            StringBuilder str = new StringBuilder();
            str.AppendFormat("SELECT A.APP_MENU_DEPT dept_code,a.dept_name \"科室名称\"");
            for (int i = 0; i < tabledict.Rows.Count; i++)
            {
                if (tabledict.Rows[i]["ACCOUNT_FLAGS"].ToString().Equals("1"))
                {
                    str.AppendFormat(" ,sum(DECODE (b.GUIDE_CODE, '{0}', b.GUIDE_VALUE,0)) \"{1}\"", tabledict.Rows[i]["GUIDE_CODE"].ToString(), "a" + tabledict.Rows[i]["GUIDE_CODE"].ToString());
                }
                else
                {
                    str.AppendFormat(" ,to_char(sum(DECODE (b.GUIDE_CODE, '{0}', b.GUIDE_VALUE,0))) \"{1}\"", tabledict.Rows[i]["GUIDE_CODE"].ToString(), "a" + tabledict.Rows[i]["GUIDE_CODE"].ToString());
                }
            }
            str.AppendFormat(@" FROM  (select m.*,n.dept_name from  comm.SYS_APP_MENU_DEPT m,comm.sys_dept_dict n where m.APP_MENU_DEPT=n.dept_code and m.APP_MENU_ID={0}) a LEFT JOIN
            HOSPITALSYS.GUIDE_VALUE b
         ON  A.APP_MENU_DEPT = B.UNIT_CODE and b.TJYF||'01'='{1}'", appid, date);
            if (deptcode != "")
            {
                str.AppendFormat(" where  A.APP_MENU_DEPT IN ({0})", deptcode);
            }
            str.Append("  group by a.APP_MENU_DEPT,a.dept_name order by a.app_menu_dept");
            return OracleOledbBase.ExecuteDataSet(str.ToString()).Tables[0];
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="appid"></param>
        /// <param name="deptcode"></param>
        /// <param name="guidecode"></param>
        /// <param name="tjyf"></param>
        /// <returns></returns>
        public DataTable GetMenuGuideDetail(string appid, string deptcode, string guidecode, string tjyf)
        {
            StringBuilder str = new StringBuilder();
            str.AppendFormat(@"select a.*,nvl(b.guide_value,0) guide_value from 
(select A.APP_MENU_ID,b.guide_code,b.guide_name,C.ACCOUNT_FLAGS from comm.SYS_APP_MENU_VS_GUIDE_DETAIL a,HOSPITALSYS.GUIDE_NAME_DICT b,comm.SYS_APP_MENU_VS_GUIDE c  
where a.app_menu_id={0} and a.guide_code='{1}' and a.APP_MENU_ID=c.APP_MENU_ID and A.GUIDE_CODE=C.GUIDE_CODE
 and a.guide_detail=b.guide_code) a left join hospitalsys.guide_value b
 on A.GUIDE_CODE=b.guide_code  and B.UNIT_CODE='{2}' and B.TJYF='{3}' ", appid, guidecode, deptcode, tjyf);
            return OracleOledbBase.ExecuteDataSet(str.ToString()).Tables[0];
        }

        /// <summary>
        /// 保存绩效考核录入指标数据
        /// </summary>
        /// <param name="rows"></param>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <param name="appid"></param>
        /// <returns></returns>
        public bool SaveMenuDetail(Dictionary<string, string>[] rows, string year, string month, string appid)
        {
            if (year == "" || month == "")
            {
                return false;
            }
            string date = BuildDate(year, month);
            MyLists listtable = new MyLists();
            string strdict = string.Format(@"select a.*,b.ORGAN_CLASS_KEY from comm.SYS_APP_MENU_GUIDE a,(select m.*,n.ORGAN_CLASS_KEY from  hospitalsys.GUIDE_NAME_DICT m,hospitalsys.JXGL_GUIDE_ORGAN_CLASS_DICT n 
                             where m.ORGAN=n.ORGAN_CLASS_CODE) b
                             where a.APP_MENU_ID={0} and a.link_guide_code=b.guide_code(+) order by a.MENU_GUIDE_ID", appid);
            DataTable tabledict = OracleOledbBase.ExecuteDataSet(strdict).Tables[0];

            string guidesqldel = string.Format("delete from  hospitalsys.GUIDE_VALUE  where  TJYF ='" + date.Substring(0, 6) + "' and APP_MENU_ID={0} ",appid);
            List delguidesql = new List();
            delguidesql.StrSql = guidesqldel;
            delguidesql.Parameters = new OleDbParameter[] { };
            listtable.Add(delguidesql);

            for (int i = 0; i < rows.Length; i++)
            {
                if (rows[i]["DEPT_CODE"].ToString() != "" && rows[i]["DEPT_CODE"].ToString() != null)
                {
                    string delsql = string.Format("delete from  {0}.SYS_MENU_DETAIL  where  st_date =TO_DATE ('" + date + "', 'yyyymmdd') and APP_MENU_ID={1} and DEPT_CODE='{2}' ", DataUser.HOSPITALSYS, appid, rows[i]["DEPT_CODE"].ToString());
                    List listcenterdict = new List();
                    listcenterdict.StrSql = delsql;
                    listcenterdict.Parameters = new OleDbParameter[] { };
                    listtable.Add(listcenterdict);

                    
                    //
                    for (int j = 0; j < tabledict.Rows.Count; j++)
                    {
                        string values = rows[i][tabledict.Rows[j]["MENU_GUIDE_NAME"].ToString()].ToString();
                        //
                        if (tabledict.Rows[j]["FIELD_TYPE"].ToString().Equals("1"))
                        {
                            DataTable tb = GetMenuItem(tabledict.Rows[j]["APP_MENU_ID"].ToString(), tabledict.Rows[j]["MENU_GUIDE_ID"].ToString(), rows[i][tabledict.Rows[j]["MENU_GUIDE_NAME"].ToString()].ToString(), "");
                            if (tb.Rows.Count > 0)
                            {
                                if (values.Equals(tb.Rows[0]["ITEM_NAME"].ToString()))
                                {
                                    values = tb.Rows[0]["ITEM_ID"].ToString();
                                }
                            }
                            else values = "0";
                        }
                        StringBuilder isql = new StringBuilder();
                        isql.AppendFormat(" insert into {0}.SYS_MENU_DETAIL (ST_DATE,DEPT_CODE,APP_MENU_ID,MENU_GUIDE_ID,MENU_GUIDE_VALUE,GUIDE_MEMO) values (", DataUser.HOSPITALSYS);
                        isql.Append("to_date('" + date + "','yyyymmdd')");
                        isql.Append(",");
                        isql.Append("'" + rows[i]["DEPT_CODE"].ToString() + "'");
                        isql.Append(",");
                        isql.Append("" + appid + "");
                        isql.Append(",");
                        isql.Append("" + int.Parse(tabledict.Rows[j]["MENU_GUIDE_ID"].ToString()) + "");
                        isql.Append(",");
                        isql.Append("" + values + "");
                        isql.Append(",");
                        isql.Append("'" + rows[i]["备注"].ToString() + "'");
                        isql.Append(")");


                        //添加科室类别
                        List listcenterdetail = new List();
                        listcenterdetail.StrSql = isql.ToString();
                        listcenterdetail.Parameters = new OleDbParameter[] { };
                        listtable.Add(listcenterdetail);
                        if (tabledict.Rows[j]["LINK_GUIDE_CODE"].ToString() != "")
                        {
                            StringBuilder guidesql = new StringBuilder();
                            guidesql.AppendFormat(@"insert into hospitalsys.GUIDE_VALUE (TJYF,UNIT_CODE,GUIDE_CODE,GUIDE_VALUE,GUIDE_TYPE,APP_MENU_ID) values (");
                            guidesql.Append("'" + date.Substring(0, 6) + "'");
                            guidesql.Append(",");
                            guidesql.Append("'" + rows[i]["DEPT_CODE"].ToString() + "'");
                            guidesql.Append(",");
                            guidesql.Append("'" + tabledict.Rows[j]["LINK_GUIDE_CODE"].ToString() + "'");
                            guidesql.Append(",");
                            guidesql.Append("" + values + "");
                            guidesql.Append(",");
                            guidesql.Append("'" + tabledict.Rows[j]["ORGAN_CLASS_KEY"].ToString() + "'");
                            guidesql.Append(",");
                            guidesql.Append("" + appid + "");
                            guidesql.Append(")");
                            List listgidevalue = new List();
                            listgidevalue.StrSql = guidesql.ToString();
                            listgidevalue.Parameters = new OleDbParameter[] { };
                            listtable.Add(listgidevalue);
                        }
                    }
                }
            }


            try
            {
                OracleOledbBase.ExecuteTranslist(listtable);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
