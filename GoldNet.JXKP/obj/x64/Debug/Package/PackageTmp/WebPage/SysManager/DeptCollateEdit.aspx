<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DeptCollateEdit.aspx.cs"
    Inherits="GoldNet.JXKP.WebPage.SysManager.DeptCollateEdit" %>

<%@ Register Assembly="Goldnet.Ext.Web" Namespace="Goldnet.Ext.Web" TagPrefix="ext" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style type="text/css">
        .search-item
        {
            font: normal 11px tahoma, arial, helvetica, sans-serif;
            padding: 3px 10px 3px 10px;
            border: 1px solid #fff;
            border-bottom: 1px solid #eeeeee;
            white-space: normal;
            color: #555;
        }
        .search-item h3
        {
            display: block;
            font: inherit;
            font-weight: bold;
            color: #222;
        }
        .search-item h3 span
        {
            float: right;
            font-weight: normal;
            margin: 0 0 5px 5px;
            width: 100px;
            display: block;
            clear: none;
        }
        p
        {
            width: 650px;
        }
        .ext-ie .x-form-text
        {
            position: static !important;
        }
    body{
         background-color: #DFE8F6;
         font-size:12px;
        }
    </style>
    <script type="text/jscript">
        var CheckForm = function() {
        if (TextDeptcode.validate() == false) {
                return false;
            }
            if (TextDeptname.validate() == false) {
                return false;
            }
            if (Combo_DeptType.validate() == false) {
                return false;
            }

            return true;
        }
    </script>
</head>
<ext:ScriptManager ID="ScriptManager1" runat="server">
</ext:ScriptManager>
<ext:Store ID="Store2" runat="server" AutoLoad="true">       
    </ext:Store>
<body>
    <form id="form1" runat="server">
    <div>
        <ext:FormPanel ID="FormPanel1" runat="server" Border="false" MonitorValid="true" ButtonAlign="Right"
            BodyStyle="background-color:transparent;margin:10px,0,0,10px">
            <Body>
                <ext:FormLayout ID="FormLayout2" runat="server">
                    <ext:Anchor Horizontal="85%">
                        <ext:TextField ID="TextDeptcode" runat="server" DataIndex="dept_code" MsgTarget="Side"
                            AllowBlank="false" FieldLabel="科室代码" Width="220" />
                    </ext:Anchor>
                    <ext:Anchor Horizontal="85%">
                        <ext:TextField ID="TextDeptname" runat="server" DataIndex="dept_name" MsgTarget="Side"
                            AllowBlank="false" FieldLabel="科室名称" Width="220"  />
                    </ext:Anchor>
                    <ext:Anchor Horizontal="85%">
                        <ext:ComboBox ID="Combo_DeptType" runat="server" AllowBlank="false" Width="220" EmptyText="请选择科室类别"
                            FieldLabel="科室类别">
                        </ext:ComboBox>
                    </ext:Anchor>
                    <ext:Anchor Horizontal="85%">
                        <ext:ComboBox ID="ComPdeptcode" runat="server" StoreID="Store2" DisplayField="DEPT_NAME"
                            ValueField="DEPT_CODE" TypeAhead="false" LoadingText="Searching..." Width="220"
                            PageSize="10" HideTrigger="false" ItemSelector="div.search-item" MinChars="1"
                            FieldLabel="上级科室" ListWidth="240">
                            <Template ID="Template1" runat="server">
                               <tpl for=".">
                                  <div class="search-item">
                                     <h3><span>{DEPT_NAME}</span>{DEPT_CODE}</h3>                              
                                  </div>
                               </tpl>
                            </Template>
                        </ext:ComboBox>
                    </ext:Anchor>
                    <ext:Anchor Horizontal="85%">
                        <ext:ComboBox ID="ComAccountdeptcode" runat="server" StoreID="Store2" DisplayField="DEPT_NAME"
                            ValueField="DEPT_CODE" TypeAhead="false" LoadingText="Searching..." Width="220"
                            PageSize="10" HideTrigger="false" ItemSelector="div.search-item" MinChars="1"
                            FieldLabel="核算科室" ListWidth="240">
                            <Template ID="Template2" runat="server">
                               <tpl for=".">
                                  <div class="search-item">
                                     <h3><span>{DEPT_CODE}</span>{DEPT_NAME}</h3>                              
                                  </div>
                               </tpl>
                            </Template>
                        </ext:ComboBox>
                    </ext:Anchor>
                    <ext:Anchor Horizontal="85%">
                        <ext:ComboBox ID="ComDeptcodesecond" runat="server" StoreID="Store2" DisplayField="DEPT_NAME"
                            ValueField="DEPT_CODE" TypeAhead="false" LoadingText="Searching..." Width="220"
                            PageSize="10" HideTrigger="false" ItemSelector="div.search-item" MinChars="1"
                            FieldLabel="二级科室" ListWidth="240">
                            <Template ID="Template3" runat="server">
                               <tpl for=".">
                                  <div class="search-item">
                                     <h3><span>{DEPT_CODE}</span>{DEPT_NAME}</h3>                              
                                  </div>
                               </tpl>
                            </Template>
                        </ext:ComboBox>
                    </ext:Anchor>
                    <ext:Anchor Horizontal="85%">
                        <ext:ComboBox ID="ComLcattr" runat="server" AllowBlank="false" Width="220" EmptyText="请选择临床属性"
                            FieldLabel="临床属性">
                        </ext:ComboBox>
                    </ext:Anchor>
                    <ext:Anchor Horizontal="85%">
                        <ext:ComboBox ID="ComIsaccount" runat="server" AllowBlank="false" Width="220" EmptyText="选择是否核算"
                            FieldLabel="是否核算">
                            <Items>
                                <ext:ListItem Text="是" Value="1" />
                                <ext:ListItem Text="不是" Value="0" />
                            </Items>
                        </ext:ComboBox>
                    </ext:Anchor>
                    <ext:Anchor Horizontal="85%">
                        <ext:NumberField ID="NumSortid" runat="server" DataIndex="SORT_NO" MsgTarget="Side"
                            AllowBlank="true" FieldLabel="排列顺序" Width="220" />
                    </ext:Anchor>
                    <ext:Anchor Horizontal="85%">
                        <ext:ComboBox ID="ComShowflag" runat="server" AllowBlank="false" Width="220" EmptyText="选择是否停用"
                            FieldLabel="是否停用">
                            <Items>
                                <ext:ListItem Text="启用" Value="0" />
                                <ext:ListItem Text="停用" Value="1" />
                            </Items>
                        </ext:ComboBox>
                    </ext:Anchor>
                </ext:FormLayout>
            </Body>
            <Buttons >
                <ext:Button ID="BtnSave" runat="server" Text="保存" Icon="Disk" >
                    <AjaxEvents>
                        <Click OnEvent="SaveEditDeptCollate" Before="if (CheckForm()== false){ Ext.Msg.alert('系统提示','请根据红线提示填写正确的信息！');return false;};">
                        </Click>
                    </AjaxEvents>
                </ext:Button>
                <ext:Button ID="CancelButton" runat="server" Text="取消" Icon="Cancel">
                    <Listeners>
                        <Click Handler="parent.DetailWin.hide();" />
                    </Listeners>
                </ext:Button>
            </Buttons>
        </ext:FormPanel>
    </div>
    </form>
</body>
</html>
