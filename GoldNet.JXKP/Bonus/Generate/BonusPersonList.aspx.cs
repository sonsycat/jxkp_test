using System;
using System.Collections.Generic;
using Goldnet.Dal;
using Goldnet.Ext.Web;
using System.Data;
using GoldNet.Model;
using GoldNet.Comm.ExportData;

namespace GoldNet.JXKP
{
    public partial class BonusPersonList : PageBase
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
                //参数获取
                string tag = this.GetStringByQueryStr("tag");
                string bonusid = this.GetStringByQueryStr("bonusid");
                string deptid = this.GetStringByQueryStr("deptid");
                
                //InitDeptPerson(bonusid, deptid,tag);
                
                //if (this.IsEdit())
                //{
                //有编辑权限
                //提交按钮
                this.Button_ok.Visible = true;
                //添加人员按钮
                this.AddPersons.Visible = true;
                //保存按钮
                this.Btn_Save.Visible = true;
                //删除按钮
                this.Btn_Del.Visible = true;
                //全部删除按钮
                this.Btn_DelAll.Visible = true;

                
                // }
                // else
                // {
                //无编辑权限
                //提交按钮
                // this.Button_ok.Visible = false;
                //添加人员按钮
                //this.AddPersons.Visible = false;
                //保存按钮
                // this.Btn_Save.Visible = false;
                //删除按钮
                //this.Btn_Del.Visible = false;
                //全部删除按钮
                //this.Btn_DelAll.Visible = false;
                // }
                if (this.IsPass())
                {
                    //有审核权限
                    this.Button_no.Visible = true;
                }
                else
                {
                    //无审核权限
                    this.Button_no.Visible = false;
                }

                //SetDict();
                
                data(bonusid, deptid, tag);
            }
        }

        /// <summary>
        /// 下拉框设置
        /// </summary>
        //public void SetDict()
        //{
        //    string bonusid = this.GetStringByQueryStr("bonusid");
        //    CalculateBonus calculateBonus = new CalculateBonus();
        //    DataTable table = calculateBonus.GetIndex();
        //    if (table.Rows.Count > 0)
        //    {
        //        for (int i = 0; i < table.Rows.Count; i++)
        //        {
        //            if (table.Rows[i]["ID"].ToString() != bonusid)
        //            {
        //                this.comindex.Items.Add(new Goldnet.Ext.Web.ListItem(table.Rows[i]["BONUSNAME"].ToString(), table.Rows[i]["ID"].ToString()));
        //            }
        //        }
        //    }
        //}

        /// <summary>
        /// 提取历史奖金人员并且保存
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void SelectedFunc(object sender, AjaxEventArgs e)
        {
            //CalculateBonus calculateBonus = new CalculateBonus();
            //string bonusid = this.GetStringByQueryStr("bonusid");
            //string deptid = this.GetStringByQueryStr("deptid");
            //string tag = this.GetStringByQueryStr("tag");
            //if (calculateBonus.GetDeptPersonsIsSubmit(bonusid, deptid))
            //{
            //    this.ShowMessage("系统提示", "科室人员奖金已提交，不能再修改！");
            //}
            //else
            //{
            //    if (this.comindex.SelectedItem.Value != "")
            //    {
            //        calculateBonus.GetBonusP(bonusid, deptid);
            //        data(bonusid, deptid, tag);
            //    }
            //}
        }

        /// <summary>
        /// 获取二次分配数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Store_RefreshData(object sender, StoreRefreshDataEventArgs e)
        {
            string bonusid = this.GetStringByQueryStr("bonusid");
            string deptid = this.GetStringByQueryStr("deptid");
            string tag = this.GetStringByQueryStr("tag");
            data(bonusid, deptid, tag);
        }

        /// <summary>
        /// 返回处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Back_Click(object sender, AjaxEventArgs e)
        {
            string tag = this.GetStringByQueryStr("tag");
            string bonusid = this.GetStringByQueryStr("bonusid");
            string tagMode = this.GetStringByQueryStr("tagMode");
            Response.Redirect("BonusDeptList_New.aspx?tag=" + this.EncryptTheQueryString(tag) + "&tagID=" + this.EncryptTheQueryString(bonusid) + "&tagMode=" + this.EncryptTheQueryString(tagMode) + "&pageid=" + Request.QueryString["pageid"].ToString());
        }

        /// <summary>
        /// 初始化平均奖/核算科室人的列表
        /// </summary>
        /// <param name="bonusid"></param>
        /// <param name="deptid"></param>
        private void InitDeptPerson(string bonusid, string deptid, string tag)
        {
            CalculateBonus calculateBouns = new CalculateBonus();
            BuildControl buildcontrol = new BuildControl();
            DataTable dtBonusValue = null;
            if (tag == "1")
            {
                dtBonusValue = calculateBouns.GetAccountPersonDetail(bonusid, deptid);
                buildcontrol.BuildAccountPerson(Store1, GridPanel1);
            }
            else
            {
                dtBonusValue = calculateBouns.GetAvgPersonDetail(bonusid, deptid);
                buildcontrol.BuildAvgPerson(Store1, GridPanel1);
            }
            Session.Remove("BonusPersonList");
            Session["BonusPersonList"] = dtBonusValue;
            Store1.DataSource = dtBonusValue;
            Store1.DataBind();

            //
        }

        /// <summary>
        /// 获取数据并绑定
        /// </summary>
        /// <param name="bonusid"></param>
        /// <param name="deptid"></param>
        private void data(string bonusid, string deptid, string tagflag)
        {
            string tag = this.GetStringByQueryStr("tag");
            string deptname = this.GetStringByQueryStr("deptname");
            CalculateBonus calculateBouns = new CalculateBonus();

            if (calculateBouns.GetBonusIsSubmit(bonusid, deptid, tag))
            {
                this.Commit.Disabled = true;
                this.Btn_Save.Disabled = true;
                this.Btn_Del.Disabled = true;
                this.Btn_DelAll.Disabled = true;
                this.AddPersons.Disabled = true;
            }

            //获取科室总奖金及人员分配总奖金
            string deptbonus = calculateBouns.GetDeptBonus_New(bonusid, deptid,tag);
            string deptpersonsbonus = calculateBouns.GetDeptPersonsBonus(bonusid, deptid, tagflag);
            this.memu.Text = deptname + "：科室总奖金：" + deptbonus + "，人员奖金合计：" + deptpersonsbonus;

            DataTable dt = calculateBouns.GetSubmitBonusPersons(bonusid, this.DeptFilter(""));
            this.Store3.DataSource = dt;
            this.Store3.DataBind();

            //获取科室人员分配明细列表
            DataTable table = calculateBouns.GetBonusPersons(bonusid, deptid);
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

                if (cl.Header.Equals("银行账号") || cl.Header.Equals("名称") || cl.Header.Equals("科室名称") || cl.Header.Equals("ID") || cl.Header.Equals("DEPT_CODE") || cl.Header.Equals("STAFFID"))
                {
                    TextField fils = new TextField();
                    fils.ID = i.ToString();
                    fils.SelectOnFocus = true;
                    if (cl.Header.Equals("名称") || cl.Header.Equals("科室名称"))
                    {
                        fils.ReadOnly = true;
                    }
                    if (cl.Header.Equals("ID") || cl.Header.Equals("DEPT_CODE") || cl.Header.Equals("STAFFID"))
                    {
                        cl.Hidden = true;
                    }
                    if (cl.Header.Equals("科室名称"))
                    {
                        dr[cl.Header] = "合计";
                    }
                    cl.Editor.Add(fils);
                }
                else if (cl.Header.Equals("人员类别"))
                {
                    ComboBox fils = new ComboBox();
                    fils.ID = i.ToString();
                    DataTable tb = calculateBouns.GetBonusPersonType();
                    for (int j = 0; j < tb.Rows.Count; j++)
                    {
                        fils.Items.Insert(j, new Goldnet.Ext.Web.ListItem(tb.Rows[j]["BONUS_PSERSONS_TYPE"].ToString(), tb.Rows[j]["BONUS_PSERSONS_TYPE"].ToString()));
                    }
                    fils.ReadOnly = false;
                    cl.Editor.Add(fils);
                }
                else
                {
                    dr[cl.Header] = table.Compute(string.Format("Sum({0})", cl.Header), "");
                    NumberField fils = new NumberField();
                    fils.ID = i.ToString();
                    fils.SelectOnFocus = true;
                    fils.DecimalPrecision = 2;
                    if (cl.Header.Equals("合计"))
                    {
                        fils.ReadOnly = false;
                    }
                    cl.Editor.Add(fils);
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
            string bonusid = this.GetStringByQueryStr("bonusid");
            string deptid = this.GetStringByQueryStr("deptid");
            CalculateBonus calculateBouns = new CalculateBonus();
            //DataTable table = calculateBouns.GetBonusPersons(bonusid, deptid);
            DataTable table = calculateBouns.GetBonusPersons_DC(bonusid, deptid);
            //this.outexcel(table,"奖金明细");
            ex.ExportToLocal(table, this.Page, "xls", "奖金明细");
        }

        ///全部删除平均奖、核算科室人
        protected void Btn_DelAll_Click(object sender, AjaxEventArgs e)
        {
            if (Store1.Reader.Count > 0)
            {
                CalculateBonus calculateBonus = new CalculateBonus();
                BuildControl buildcontrol = new BuildControl();
                string bonusid = this.GetStringByQueryStr("bonusid");
                string deptid = this.GetStringByQueryStr("deptid");
                string tag = this.GetStringByQueryStr("tag");
                if (calculateBonus.GetDeptPersonsIsSubmit(bonusid, deptid))
                {
                    this.ShowMessage("系统提示", "科室人员奖金已提交，不能再修改！");
                }
                else
                {
                    try
                    {
                        //string tag = this.GetStringByQueryStr("tag");
                        calculateBonus.DelAllAccountPerson(bonusid, deptid);
                        data(bonusid, deptid, tag);
                        Goldnet.Ext.Web.Ext.Msg.Show(new Goldnet.Ext.Web.MessageBox.Config
                        {
                            Title = "提示",
                            Message = "删除成功",
                            Buttons = MessageBox.Button.OK,
                            Icon = (MessageBox.Icon)Enum.Parse(typeof(MessageBox.Icon), "INFO")
                        });
                    }
                    catch (Exception ex)
                    {
                        ShowDataError(ex.ToString(), Request.Url.LocalPath, "SaveBonusPersonList");
                    }
                }
            }
        }

        /// <summary>
        /// 保存处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Btn_Save_Click(object sender, AjaxEventArgs e)
        {
            Dictionary<string, string>[] selectRow = GetSelectRow(e, "Values");
            if (selectRow != null)
            {
                CalculateBonus calculateBonus = new CalculateBonus();
                string bonusid = this.GetStringByQueryStr("bonusid");
                string deptcode = this.GetStringByQueryStr("deptid");
                string deptname = this.GetStringByQueryStr("deptname");
                string tag = this.GetStringByQueryStr("tag");
                if (calculateBonus.GetDeptPersonsIsSubmit(bonusid, deptcode))
                {
                    data(bonusid, deptcode, tag);
                    this.ShowMessage("系统提示", "科室人员奖金已提交，不能再修改！");
                }
                if (calculateBonus.SaveAccountPersonBouns_JY(bonusid, deptcode, deptname, selectRow))
                {
                    this.ShowMessage("系统提示", "科室人员奖金总数不对，不能保存");
                }
                else
                {
                    try
                    {
                        calculateBonus.SaveAccountPersonBouns(bonusid, deptcode, deptname, selectRow);
                        Goldnet.Ext.Web.Ext.Msg.Show(new Goldnet.Ext.Web.MessageBox.Config
                        {
                            Title = "提示",
                            Message = "保存成功",
                            Buttons = MessageBox.Button.OK,
                            Icon = (MessageBox.Icon)Enum.Parse(typeof(MessageBox.Icon), "INFO")
                        });
                        GridPanel1.RefreshView();
                        data(bonusid, deptcode, tag);
                    }
                    catch (Exception ex)
                    {
                        ShowDataError(ex.ToString(), Request.Url.LocalPath, "SaveBonusPersonList");
                    }
                }
            }
        }

        /// <summary>
        /// 删除处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Btn_Del_Click(object sender, AjaxEventArgs e)
        {
            CalculateBonus calculateBonus = new CalculateBonus();
            string bonusid = this.GetStringByQueryStr("bonusid");
            string deptcode = this.GetStringByQueryStr("deptid");
            if (calculateBonus.GetDeptPersonsIsSubmit(bonusid, deptcode))
            {
                this.ShowMessage("系统提示", "科室人员奖金已提交，不能再修改！");
            }
            else
            {
                Dictionary<string, string>[] selectRow = GetSelectRow(e, "Selecct");
                if (selectRow != null)
                {
                    try
                    {
                        string id = selectRow[0]["ID"];

                        calculateBonus.DelPersonBonus(id);
                        GridPanel1.DeleteSelected();

                        Goldnet.Ext.Web.Ext.Msg.Show(new Goldnet.Ext.Web.MessageBox.Config
                            {
                                Title = "提示",
                                Message = "删除成功",
                                Buttons = MessageBox.Button.OK,
                                Icon = (MessageBox.Icon)Enum.Parse(typeof(MessageBox.Icon), "INFO")
                            });
                    }
                    catch (Exception ex)
                    {
                        ShowDataError(ex.ToString(), Request.Url.LocalPath, "DelBonusPersonList");
                    }
                }
            }

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

        /// <summary>
        /// 添加奖金人员
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void AddPersons_Click(object sender, EventArgs e)
        {
            CalculateBonus calculateBonus = new CalculateBonus();
            string bonusid = this.GetStringByQueryStr("bonusid");
            string deptcode = this.GetStringByQueryStr("deptid");
            if (calculateBonus.GetDeptPersonsIsSubmit(bonusid, deptcode))
            {
                this.ShowMessage("系统提示", "科室人员奖金已提交，不能再修改！");
            }
            else
            {
                LoadConfig loadcfg = getLoadConfig("AddPersons.aspx");
                loadcfg.Params.Add(new Goldnet.Ext.Web.Parameter("bonusid", bonusid));
                loadcfg.Params.Add(new Goldnet.Ext.Web.Parameter("deptcode", deptcode));
                showCenterSet(this.addpersonsWin, loadcfg);
            }
        }

        /// <summary>
        /// 提交科室人员奖金
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Button_ok_click(object sender, AjaxEventArgs e)
        {
            string bonusid = this.GetStringByQueryStr("bonusid");
            string deptid = this.GetStringByQueryStr("deptid");
            string tag = this.GetStringByQueryStr("tag");
            User user = (User)Session["CURRENTSTAFF"];
            Dictionary<string, string>[] selectRow = GetSelectRow(e);
            if (selectRow == null || selectRow.Length < 1)
            {
                this.SelectRecord();
            }
            else
            {
                try
                {
                    CalculateBonus dal = new CalculateBonus();
                    for (int i = 0; i < selectRow.Length; i++)
                    {
                        string deptcode = selectRow[i]["DEPT_CODE"];
                        string deptname = selectRow[i]["DEPT_NAME"];
                        string deptbonus = dal.GetDeptBonus_New(bonusid, deptcode,tag);
                        string deptpersonsbonus = dal.GetDeptPersonsBonus(bonusid, deptcode, tag);
                        if (deptbonus == deptpersonsbonus)
                        {
                            if (!dal.GetDeptPersonsIsSubmit(bonusid, deptcode))
                            {
                                dal.SubmitBonusPersons(bonusid, deptcode, user.UserName);
                            }
                        }
                        else
                        {
                            Ext.Msg.Confirm("系统提示", deptname + "：科室总奖金:" + deptbonus + ",科室人员奖金和:" + deptpersonsbonus + ",相差" + (double.Parse(deptbonus) - double.Parse(deptpersonsbonus)).ToString() + ",确认进行提交吗？", new MessageBox.ButtonsConfig
                            {
                                Yes = new MessageBox.ButtonConfig
                                {
                                    Handler = "Goldnet.SaveBonusPerson('" + bonusid + "','" + deptcode + "','" + tag + "')",
                                    Text = "确定"

                                },
                                No = new MessageBox.ButtonConfig
                                {
                                    Text = "取消"
                                }
                            }).Show();
                        }
                    }
                    data(bonusid, deptid, tag);
                }
                catch (Exception ex)
                {
                    this.ShowDataError(ex, Request.Path, "Button_ok_click");
                }
            }
        }

        /// <summary>
        /// 提交科室人员奖金
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Btn_Commit_Click(object sender, AjaxEventArgs e)
        {
            string bonusid = this.GetStringByQueryStr("bonusid");
            string deptid = this.GetStringByQueryStr("deptid");
            string tag = this.GetStringByQueryStr("tag");
            User user = (User)Session["CURRENTSTAFF"];

            try
            {
                CalculateBonus dal = new CalculateBonus();

                string deptcode = this.GetStringByQueryStr("deptid");
                string deptname = this.GetStringByQueryStr("deptname");
                string deptbonus = dal.GetDeptBonus_New(bonusid, deptcode,tag);
                string deptpersonsbonus = dal.GetDeptPersonsBonus(bonusid, deptcode, tag);
                if (deptbonus == deptpersonsbonus)
                {
                    if (!dal.GetDeptPersonsIsCommit(bonusid, deptcode))
                    {
                        dal.CommitBonusPersons(bonusid, deptcode, user.UserName,tag);
                    }
                }
                else
                {
                    Ext.Msg.Confirm("系统提示", deptname + "：科室总奖金:" + deptbonus + ",科室人员奖金和:" + deptpersonsbonus + ",相差" + (double.Parse(deptbonus) - double.Parse(deptpersonsbonus)).ToString() + ",确认进行提交吗？", new MessageBox.ButtonsConfig
                    {
                        Yes = new MessageBox.ButtonConfig
                        {
                            Handler = "Goldnet.SaveBonusPerson('" + bonusid + "','" + deptcode + "','" + tag + "')",
                            Text = "确定"

                        },
                        No = new MessageBox.ButtonConfig
                        {
                            Text = "取消"
                        }
                    }).Show();
                }

                data(bonusid, deptid, tag);
            }
            catch (Exception ex)
            {
                this.ShowDataError(ex, Request.Path, "Btn_Commit_Click");
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bonusid"></param>
        /// <param name="deptcode"></param>
        [AjaxMethod]
        public void SaveBonusPerson(string bonusid, string deptcode, string tag)
        {
            Goldnet.Ext.Web.ScriptManager scManager = Goldnet.Ext.Web.ScriptManager.GetInstance(this.Page);
            User user = (User)Session["CURRENTSTAFF"];
            CalculateBonus dal = new CalculateBonus();
            dal.CommitBonusPersons(bonusid, deptcode, user.UserName,tag);
            data(bonusid, deptcode, tag);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Button_no_click(object sender, AjaxEventArgs e)
        {
            string bonusid = this.GetStringByQueryStr("bonusid");
            string deptid = this.GetStringByQueryStr("deptid");
            string tag = this.GetStringByQueryStr("tag");
            Dictionary<string, string>[] selectRow = GetSelectRow(e);
            if (selectRow == null || selectRow.Length < 1)
            {
                this.SelectRecord();
            }
            else
            {
                try
                {
                    CalculateBonus dal = new CalculateBonus();
                    for (int i = 0; i < selectRow.Length; i++)
                    {
                        string deptcode = selectRow[i]["DEPT_CODE"];

                        dal.CancleBonusPersons(bonusid, deptcode);
                    }
                    this.SaveSucceed();
                    data(bonusid, deptid, tag);
                }
                catch (Exception ex)
                {
                    this.ShowDataError(ex, Request.Path, "Button_no_click");
                }
            }
        }

    }
}
