<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="register_detail.aspx.cs"
    Inherits="GoldNet.JXKP.auth.login.register_detail" %>

<%@ Register Assembly="Goldnet.Ext.Web" Namespace="Goldnet.Ext.Web" TagPrefix="ext" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style type="text/css">
        body
        {
            background-color: #DFE8F6;
            font-size: 12px;
        }
    </style>
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
    </style>
     <script language="javascript" type="text/javascript">
       function validate_id(data_id)
       {
           Goldnet.AjaxMethod.request( 
           'GetDbuser',
            {
                params: 
                {
                    dbuser:DB_USER.getValue()
                },
                success: function(result) 
                {
                    if(result == "0")
                    {
                        save.setDisabled(false);
                    }
                    else
                    {
                        save.setDisabled(true);
                        Ext.Msg.alert('提示','登录名被占用');
                    }
                    
                },
                failure: function(msg) 
                {
                    Ext.Msg.alert('提示',msg);
                }
             });
       }
       
       var CheckForm = function() {
        if (DeptCodeCombo.validate() == false) {
            return false;
        }
        if (USER_NAME.validate() == false) {
            return false;
        }
        if (DB_USER.validate() == false) {
            return false;
        }
        if (PASSWORD.validate() == false) {
            return false;
        }

        return true;
    }
    </script>
</head>
<body>
    <br />
    <ext:ScriptManager ID="ScriptManager1" runat="server" />
    <ext:Store ID="Store3" runat="server" AutoLoad="true">
        <Proxy>
            <ext:HttpProxy Method="POST" Url="/RLZY/WebService/DeptInfo.ashx?deptfilter=" />
        </Proxy>
        <Reader>
            <ext:JsonReader Root="Staffdepts">
                <Fields>
                    <ext:RecordField Name="DEPT_NAME" />
                    <ext:RecordField Name="DEPT_CODE" />
                </Fields>
            </ext:JsonReader>
        </Reader>
    </ext:Store>
    <form id="form1" runat="server">
    <div>
        <ext:FormPanel ID="FormPanel1" runat="server" Border="false" MonitorValid="true"
            ButtonAlign="Right" BodyStyle="background-color:transparent;">
            <Body>
                <ext:FormLayout ID="FormLayout1" runat="server" LabelWidth="80" StyleSpec="margin:10px">
                    <ext:Anchor>
                        <ext:TextField runat="server" ID="USER_ID" Hidden="true" />
                    </ext:Anchor>
                    <ext:Anchor>
                        <ext:MultiField ID="MultiField2" runat="server" FieldLabel="科室">
                            <Fields>
                                <ext:ComboBox ID="DeptCodeCombo" runat="server" StoreID="Store3" DisplayField="DEPT_NAME"
                                    Width="220" ValueField="DEPT_CODE" TypeAhead="false" LoadingText="Searching..."
                                    PageSize="1000" ItemSelector="div.search-item" MinChars="1" FieldLabel="科室信息"
                                    ListWidth="240"  CausesValidation="true" AllowBlank="false">
                                    <Template ID="Template1" runat="server">
                                       <tpl for=".">
                                          <div class="search-item">
                                             <h3><span style="width:auto">{DEPT_CODE}</span>{DEPT_NAME}</h3>
                                          </div>
                                       </tpl>
                                    </Template>
                                </ext:ComboBox>
                            </Fields>
                        </ext:MultiField>
                    </ext:Anchor>
                    <ext:Anchor>
                        <ext:MultiField ID="MultiField1" runat="server" FieldLabel="人员名称">
                            <Fields>
                                <ext:TextField ID="USER_NAME" runat="server" DataIndex="USER_NAME" MsgTarget="Side"
                                    Width="220" MaxLength="10"  CausesValidation="true" AllowBlank="false" />
                            </Fields>
                        </ext:MultiField>
                    </ext:Anchor>
                    <ext:Anchor>
                        <ext:MultiField ID="MultiField9" runat="server" FieldLabel="登录名">
                            <Fields>
                                <ext:TextField ID="DB_USER" runat="server" DataIndex="DB_USER" MsgTarget="Side" Width="220"
                                    MaxLength="10"  CausesValidation="true" AllowBlank="false">
                                    <Listeners >
                                        <Blur Handler="validate_id();" />
                                    </Listeners>
                                </ext:TextField>
                            </Fields>
                        </ext:MultiField>
                    </ext:Anchor>
                    <ext:Anchor>
                        <ext:MultiField ID="MultiField3" runat="server" FieldLabel="密码">
                            <Fields>
                                <ext:TextField ID="PASSWORD" runat="server" DataIndex="PASSWORD" MsgTarget="Side"
                                    Width="220" MaxLength="10"  CausesValidation="true" AllowBlank="false" />
                            </Fields>
                        </ext:MultiField>
                    </ext:Anchor>
                </ext:FormLayout>
            </Body>
            <Buttons>
                <ext:Button ID="save" runat="server" Text="保存" Icon="Disk">
                    <AjaxEvents>
                        <Click OnEvent="Buttonsave_Click" Before="if (CheckForm()== false){ Ext.Msg.alert('系统提示','请根据红线提示填写正确的信息！');return false;};">
                            <EventMask Msg="保存中..." ShowMask="true" />
                        </Click>
                    </AjaxEvents>
                </ext:Button>
                <ext:Button ID="cancel" runat="server" Text="取消" Icon="Cancel">
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
