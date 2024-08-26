<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="single_cost_inputAdd.aspx.cs"
    Inherits="GoldNet.JXKP.cbhs.datagather.single_cost_inputAdd" %>

<%@ Register Assembly="Goldnet.Ext.Web" Namespace="Goldnet.Ext.Web" TagPrefix="ext" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
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
        if (tfBName.validate() == false) {
                return false;
            }                  
            return true;
        }
        function SelectChange(control) {
//            var beginYear = cbbBeginYear.getSelectedItem().text;
//            var beginMonth = cbbBeginMonth.getSelectedItem().text;         
//            var beginText=beginYear+'年'+beginMonth+'月';
//            labelBegin.setText(beginText);
//            labelEnd.setText(beginText);
        }
        function setHidden() {
            BtnSave.hide();
            BtnCancel.show();
            BtnClose.disable();
            progressTip.show();
        }
        function setCancelHidd() {
            BtnCancel.hide();
            BtnSave.show();
            BtnClose.enable();
            progressTip.hide();
        }
           function checkType(){
            //得到上传文件的值
            var fileName=document.getElementById("photoimg").value;
            //返回String对象中子字符串最后出现的位置.
            var seat=fileName.lastIndexOf(".");
            //返回位于String对象中指定位置的子字符串并转换为小写.
            var extension=fileName.substring(seat).toLowerCase();
            //判断允许上传的文件格式
            if(extension!=".xlsx"&&extension!=".xls"){
              Ext.Msg.show({ title: '信息提示', msg: '不支持'+extension+'文件的上传', icon: 'ext-mb-info', buttons: { ok: true }});
              return false;
            }else{
              return true;
            }
          }
          
    </script>

</head>
<body>
    <ext:ScriptManager ID="ScriptManager1" runat="server" />
    <form id="form1" runat="server">
    <ext:Store ID="Store1" runat="server">
        <Reader>
            <ext:JsonReader ReaderID="YEAR">
                <Fields>
                    <ext:RecordField Name="YEAR" />
                </Fields>
            </ext:JsonReader>
        </Reader>
    </ext:Store>
    <ext:Store ID="Store2" runat="server">
        <Reader>
            <ext:JsonReader ReaderID="MONTH">
                <Fields>
                    <ext:RecordField Name="MONTH" />
                </Fields>
            </ext:JsonReader>
        </Reader>
    </ext:Store>
    <div>
        <ext:FormPanel ID="FormPanel1" runat="server" Border="false" AutoScroll="false" ButtonAlign="Right"
            StyleSpec="background-color:Transparent" BodyStyle="background-color:Transparent">
            <Body>
                <table border="0">
                    <tr>
                        <td>
                            奖金名称：
                        </td>
                        <td>
                            <ext:TextField runat="server" Width="260" ID="tfBName" AllowBlank="false" CausesValidation="true">
                            </ext:TextField>
                            <ext:FileUploadField ID="photoimg" runat="server" ButtonOnly="true" ButtonText="上传图片"
                                Icon="ImageAdd" Width="150">
                                <AjaxEvents>
                                    <FileSelected OnEvent="UploadClick"><%--Before="return checkType();"
                                     --%>   
                                    </FileSelected>
                                </AjaxEvents>
                            </ext:FileUploadField>
                        </td>
                    </tr>
                    <tr>
                        <td>
                        </td>
                        <td>
                            如：2004年10月份绩效奖金
                        </td>
                    </tr>
                    <tr>
                        <td>
                            奖金月份：
                        </td>
                        <td>
                            <table>
                                <tr>
                                    <td>
                                        <ext:ComboBox ID="cbbBeginYear" runat="server" StoreID="Store1" Width="70" DisplayField="YEAR"
                                            ValueField="YEAR" ReadOnly="true">
                                            <Listeners>
                                                <Select Fn="SelectChange" />
                                            </Listeners>
                                        </ext:ComboBox>
                                    </td>
                                    <td>
                                        年
                                    </td>
                                    <td>
                                        <ext:ComboBox ID="cbbBeginMonth" runat="server" StoreID="Store2" Width="50" DisplayField="MONTH"
                                            ValueField="MONTH" ReadOnly="true">
                                            <Listeners>
                                                <Select Fn="SelectChange" />
                                            </Listeners>
                                        </ext:ComboBox>
                                    </td>
                                    <td>
                                        月
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <ext:Label runat="server" ID="progressTip" Text="" AutoWidth="true">
                            </ext:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <p>
                                生成进度：
                            </p>
                        </td>
                        <td>
                            <ext:ProgressBar ID="Progress1" runat="server" Width="260">
                            </ext:ProgressBar>
                        </td>
                    </tr>
                    <ext:TaskManager ID="TaskManager1" runat="server">
                        <Tasks>
                            <ext:Task TaskID="longactionprogress" Interval="1000" AutoRun="false" OnStart="#{BtnSave}.setDisabled(true); "
                                OnStop="#{BtnSave}.setDisabled(false);">
                                <AjaxEvents>
                                    <Update OnEvent="RefreshProgress" />
                                </AjaxEvents>
                            </ext:Task>
                        </Tasks>
                    </ext:TaskManager>
                </table>
            </Body>
            <Buttons>
                <ext:Button ID="BtnSave" runat="server" Text="生成" Icon="PlayBlue">
                    <AjaxEvents>
                        <Click OnEvent="Btn_New_Add" Before="if (CheckForm()== false){ Ext.Msg.alert('系统提示','请输入奖金名称！');return false;}">
                        </Click>
                    </AjaxEvents>
                </ext:Button>
                <ext:Button ID="BtnCancel" runat="server" Text="取消" Icon="StopBlue" Hidden="true">
                    <AjaxEvents>
                        <Click OnEvent="Btn_Cancel">
                            <Confirmation BeforeConfirm="config.confirmation.message = '您确定取消吗？';" Title="系统提示"
                                ConfirmRequest="true" />
                        </Click>
                    </AjaxEvents>
                </ext:Button>
                <ext:Button ID="BtnClose" runat="server" Text="关闭" Icon="Cancel">
                    <Listeners>
                        <Click Handler="parent.DetailWin.hide();parent.RefreshData();parent.DetailWin.clearContent();" />
                    </Listeners>
                </ext:Button>
            </Buttons>
        </ext:FormPanel>
    </div>
    </form>
</body>
</html>
