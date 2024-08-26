<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="appor_prog_sqlexpress_edit.aspx.cs"
    Inherits="GoldNet.JXKP.cbhs.xyhs.xyhsdict.appor_prog_sqlexpress_edit" %>

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

    <script type="text/javascript">
        var CheckForm = function() {
        if (tfPROGCODE.validate() == false) {
                return false;
            }
            if (tfPROGNAME.validate() == false) {
                return false;
            }
            if (ccbtype.validate() == false) {
                return false;
            }
            if (taSQL.validate() == false) {
                return false;
            }
            return true;
        }
    </script>

    <ext:ScriptManager ID="ScriptManager1" runat="server">
    </ext:ScriptManager>
    <ext:Store ID="Store2" runat="server" AutoLoad="true">
        <Reader>
            <ext:JsonReader ReaderID="ID">
                <Fields>
                    <ext:RecordField Name="APPORTION_CODE" />
                    <ext:RecordField Name="APPORTION_NAME" />
                </Fields>
            </ext:JsonReader>
        </Reader>
    </ext:Store>
</head>
<body>
    <form id="form1" runat="server" style="background-color: Transparent">
    <div>
        <ext:FormPanel ID="FormPanel1" runat="server" Border="false" AutoScroll="false" ButtonAlign="Right"
            StyleSpec="background-color:Transparent" BodyStyle="background-color:Transparent;margin:10px,0,0,10px">
            <Body>
                <ext:Resizable ID="Resizable1" runat="server" Element="taSQL" Handles="South" Wrap="true"
                    Pinned="true" Height="96" MinHeight="60" Dynamic="true" />
                <ext:FormLayout ID="FormLayout1" runat="server">
                    <ext:Anchor Horizontal="95%">
                        <ext:TextField runat="server" ID="tfPROGCODE" CausesValidation="true" FieldLabel="方案代码"
                            AllowBlank="false" BlankText="请输入方案代码" />
                    </ext:Anchor>
                    <ext:Anchor Horizontal="95%">
                        <ext:TextField runat="server" ID="tfPROGNAME" CausesValidation="true" FieldLabel="方案名称"
                            AllowBlank="false" BlankText="请输入方案名称" />
                    </ext:Anchor>
                    <ext:Anchor Horizontal="95%">
                        <ext:ComboBox ID="ccbtype" runat="server" StoreID="Store2" DisplayField="APPORTION_NAME"
                            ValueField="APPORTION_CODE" Width="240" FieldLabel="方案类别" CausesValidation="true"
                            AllowBlank="false">
                        </ext:ComboBox>
                    </ext:Anchor>
                    <ext:Anchor Horizontal="95%">
                        <ext:TextArea runat="server" ID="taSQL" CausesValidation="true" FieldLabel="方案SQL"
                            AllowBlank="false" Height="200" />
                    </ext:Anchor>
                    <ext:Anchor Horizontal="95%">
                        <ext:TextArea runat="server" ID="taRemark" FieldLabel="备注" Height="100" />
                    </ext:Anchor>
                </ext:FormLayout>
            </Body>
            <Buttons>
                <ext:Button ID="BtnSave" runat="server" Text="保存" Icon="Disk">
                    <AjaxEvents>
                        <Click OnEvent="SaveProg_onClick" Before="if (CheckForm()== false){ Ext.Msg.alert('系统提示','请根据红线提示填写正确的信息！');return false;};">
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
