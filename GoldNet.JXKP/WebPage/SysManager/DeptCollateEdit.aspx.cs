using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Goldnet.Dal;
using Goldnet.Ext.Web;
using System.Data;

namespace GoldNet.JXKP.WebPage.SysManager
{
    public partial class DeptCollateEdit : PageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Ext.IsAjaxRequest)
            {
               
                SetDict();
                if (Request["DeptCollateMode"] != null)
                {
                    string mode = Request["DeptCollateMode"].ToString();
                    if (mode == "Edit" && Request["DeptCollateID"] != null)
                    {
                        SetCollateEdit();
                    }
                    else if (mode == "Add")
                    {
                        SetCollateAdd();
                    }
                }
                InitStore();
            }
        }
        /// <summary>
        /// 初始化Store
        /// </summary>
        private void InitStore()
        {
            HttpProxy pro = new HttpProxy();
            pro.Method = HttpMethod.POST;
            pro.Url = "WebService/HisDepts.ashx";
            this.Store2.Proxy.Add(pro);

            JsonReader jr = new JsonReader();
            jr.ReaderID = "DEPT_CODE";
            jr.Root = "deptlist";
            jr.TotalProperty = "totalCount";
            RecordField rf = new RecordField();
            rf.Name = "DEPT_CODE";
            jr.Fields.Add(rf);
            RecordField rfn = new RecordField();
            rfn.Name = "DEPT_NAME";
            jr.Fields.Add(rfn);
            this.Store2.Reader.Add(jr);

        }
        private void SetCollateEdit()
        {
            Goldnet.Dal.SYS_DEPT_INFO sysdeptinfo = new Goldnet.Dal.SYS_DEPT_INFO();
            string id = Request["DeptCollateID"].ToString();
            DataTable dt = sysdeptinfo.GetDeptInfoById(id);
            if (dt != null && dt.Rows.Count > 0)
            {
                TextDeptcode.Text = dt.Rows[0]["DEPT_CODE"].ToString();
                TextDeptname.Text = dt.Rows[0]["DEPT_NAME"].ToString();
                Combo_DeptType.SetValue(dt.Rows[0]["DEPT_TYPE"].ToString());
                ComPdeptcode.SelectedItem.Value=dt.Rows[0]["P_DEPT_CODE"].ToString();
               // ComPdeptcode.SelectedItem.Text = dt.Rows[0]["P_DEPT_NAME"].ToString();
                ComAccountdeptcode.SelectedItem.Value =dt.Rows[0]["ACCOUNT_DEPT_CODE"].ToString();
               // ComAccountdeptcode.SelectedItem.Text = dt.Rows[0]["ACCOUNT_DEPT_NAME"].ToString();
                ComDeptcodesecond.SelectedItem.Value =dt.Rows[0]["DEPT_CODE_SECOND"].ToString();
              //  ComDeptcodesecond.SelectedItem.Text = dt.Rows[0]["DEPT_NAME_SECOND"].ToString();
                ComLcattr.SelectedItem.Value=dt.Rows[0]["DEPT_LCATTR"].ToString();
                ComIsaccount.SelectedItem.Value=dt.Rows[0]["ATTR"].ToString();
                NumSortid.Text = dt.Rows[0]["SORT_NO"].ToString();
                ComShowflag.SetValue(dt.Rows[0]["SHOW_FLAG"].ToString());

                TextDeptcode.ReadOnly = true;
                TextDeptname.ReadOnly = true;
            }
        
        }
        private void SetCollateAdd()
        {
            
                TextDeptcode.Text = "";
                TextDeptname.Text = "";
                Combo_DeptType.SetValue("");
                ComPdeptcode.SetValue("");
                ComAccountdeptcode.SetValue("");
                ComDeptcodesecond.SetValue("");
                ComLcattr.SetValue("");
                ComIsaccount.SetValue("");
                NumSortid.Text = "";
                ComShowflag.SetValue("");            

        }
        /// <summary>
        /// 字典设置
        /// </summary>
        public void SetDict()
        {
            Goldnet.Dal.SYS_DEPT_DICT dal = new Goldnet.Dal.SYS_DEPT_DICT();
            DataTable table = dal.GetDeptType().Tables[0];
            if (table.Rows.Count > 0)
            {
                for (int i = 0; i < table.Rows.Count; i++)
                {
                    this.Combo_DeptType.Items.Add(new Goldnet.Ext.Web.ListItem(table.Rows[i]["ATTRIBUE"].ToString(), table.Rows[i]["id"].ToString()));

                }

            }
            DataTable lctable = dal.GetDeptLcattr().Tables[0];
            if (lctable.Rows.Count > 0)
            {
                for (int i = 0; i < lctable.Rows.Count; i++)
                {
                    this.ComLcattr.Items.Add(new Goldnet.Ext.Web.ListItem(lctable.Rows[i]["ATTRIBUE"].ToString(), lctable.Rows[i]["id"].ToString()));

                }

            }
        }
        /// <summary>
        /// 保存添加或修改的科室对照
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void SaveEditDeptCollate(object sender, AjaxEventArgs e)
        {
            if (Request["DeptCollateMode"] != null)
            {
                Goldnet.Dal.SYS_DEPT_INFO sysdeptinfo = new Goldnet.Dal.SYS_DEPT_INFO();
                string mode = Request["DeptCollateMode"].ToString();
                string year = Request["Year"].ToString();
                string month = Request["Month"].ToString();
                GoldNet.Model.SYS_DEPT_DICT model_sysDept = GetModel();
                if (mode == "Add")
                {
                    try
                    {
                        //检查科室是否存在
                        if (sysdeptinfo.CheckDept(model_sysDept.DEPT_CODE, year, month))
                        {
                            Goldnet.Ext.Web.Ext.Msg.Show(new Goldnet.Ext.Web.MessageBox.Config
                            {
                                Title = "提示",
                                Message = "科室代码已经存在",
                                Buttons = MessageBox.Button.OK,
                                Icon = (MessageBox.Icon)Enum.Parse(typeof(MessageBox.Icon), "INFO")
                            });
                            return;
                        }
                        sysdeptinfo.Insert(model_sysDept,year,month);
                        Goldnet.Ext.Web.Ext.Msg.Show(new Goldnet.Ext.Web.MessageBox.Config
                        {
                            Title = "提示",
                            Message = "添加成功",
                            Buttons = MessageBox.Button.OK,
                            Icon = (MessageBox.Icon)Enum.Parse(typeof(MessageBox.Icon), "INFO")
                        });
                        Goldnet.Ext.Web.ScriptManager scManager = Goldnet.Ext.Web.ScriptManager.GetInstance(this.Page);
                        scManager.AddScript("parent.RefreshData();");
                        scManager.AddScript("parent.DetailWin.hide();");
                        scManager.AddScript("parent.DetailWin.clearContent();");
                    }
                    catch (Exception ex)
                    {
                        ShowDataError(ex.ToString(), Request.Url.LocalPath, "SaveEditDeptCollate");
                    }
                }
                else if (mode == "Edit")
                {
                    string id = Request["DeptCollateID"].ToString();
                    try
                    {
                        sysdeptinfo.Update(id, model_sysDept);
                        Goldnet.Ext.Web.Ext.Msg.Show(new Goldnet.Ext.Web.MessageBox.Config
                        {
                            Title = "提示",
                            Message = "修改成功",
                            Buttons = MessageBox.Button.OK,
                            Icon = (MessageBox.Icon)Enum.Parse(typeof(MessageBox.Icon), "INFO")
                        });
                        Goldnet.Ext.Web.ScriptManager scManager = Goldnet.Ext.Web.ScriptManager.GetInstance(this.Page);
                        scManager.AddScript("parent.RefreshData();");
                        scManager.AddScript("parent.DetailWin.hide();");
                        scManager.AddScript("parent.DetailWin.clearContent();");
                    }
                    catch (Exception ex)
                    {
                        ShowDataError(ex.ToString(), Request.Url.LocalPath, "SaveEditDeptCollate");
                    }
                }
            }


        }
        /// <summary>
        /// 页面上的数据转换成对象
        /// </summary>
        /// <returns></returns>
        private GoldNet.Model.SYS_DEPT_DICT GetModel()
        {
            GoldNet.Model.SYS_DEPT_DICT sysDept = new GoldNet.Model.SYS_DEPT_DICT();
           sysDept.DEPT_CODE=TextDeptcode.Text ;
           sysDept.DEPT_NAME=TextDeptname.Text ;
           sysDept.DEPT_TYPE=Combo_DeptType.SelectedItem.Value;
           sysDept.P_DEPT_CODE=ComPdeptcode.SelectedItem.Value ;
           sysDept.P_DEPT_Name=ComPdeptcode.SelectedItem.Text ;
           sysDept.ACCOUNT_DEPT_CODE=ComAccountdeptcode.SelectedItem.Value;
           sysDept.ACCOUNT_DEPT_NAME=ComAccountdeptcode.SelectedItem.Text;
           sysDept.DEPT_CODE_SECOND=ComDeptcodesecond.SelectedItem.Value ;
           sysDept.DEPT_NAME_SECOND=ComDeptcodesecond.SelectedItem.Text;
           sysDept.DEPT_LCATTR=ComLcattr.SelectedItem.Value;
           sysDept.ATTR=ComIsaccount.SelectedItem.Value;
           sysDept.SORT_NO=NumSortid.Text;
           sysDept.SHOW_FLAG = ComShowflag.SelectedItem.Value;
           return sysDept;
        }
    }
}
