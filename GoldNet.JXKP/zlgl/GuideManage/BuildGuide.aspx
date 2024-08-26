<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="BuildGuide.aspx.cs" Inherits="GoldNet.JXKP.zlgl.GuideSys.BuildGuide" %>

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
        if (tfBName.validate() == false) {
                return false;
            }                  
            return true;
        }
        function setHidden() {
            BtnSave.hide();
            BtnCancel.show();
            Buttondel.disable();
            BtnClose.disable();
            progressTip.show();
        }
        function setCancelHidd() {
            BtnCancel.hide();
            BtnSave.show();
            Buttondel.enable();
            BtnClose.enable();
            progressTip.hide();
        }
    </script>

</head>
<body>
    <ext:ScriptManager ID="ScriptManager1" runat="server" AjaxMethodNamespace="Goldnet"/>
    <form id="form1" runat="server">
    <div>
        <ext:FormPanel ID="FormPanel1" runat="server" Border="false" AutoScroll="false" ButtonAlign="Right"
            StyleSpec="background-color:Transparent" BodyStyle="background-color:Transparent">
            <Body>
                <table border="0" align="center">
                    <tr>
                        <td>
                        </td>
                        <td>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            质量月份：
                        </td>
                        <td>
                            <table>
                                <tr>
                                    <td>
                                        <ext:ComboBox ID="cbbBeginYear" runat="server" Width="70" ReadOnly="true">
                                        </ext:ComboBox>
                                    </td>
                                    <td>
                                        年
                                    </td>
                                    <td>
                                        <ext:ComboBox ID="cbbBeginMonth" runat="server" Width="50" ReadOnly="true">
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
                    
                    <tr>
                    <td></td>
                    
                        <td>
                        <table>
                        <tr>
                        <td>
                            <ext:Button ID="BtnSave" runat="server" Text="生成" Icon="PlayBlue">
                                 <AjaxEvents>
                                <Click OnEvent="Btn_New_Add"   Timeout="1200000" >
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
                        </td>
                        <td>
                         <ext:Button ID="Buttondel" runat="server" Text="删除" Icon="StopBlue">
                                <AjaxEvents>
                                    <Click OnEvent="Buttondeldata">
                                        <Confirmation BeforeConfirm="config.confirmation.message = '您确定删除选中的月份生成的质量数据吗？';" Title="系统提示"
                                            ConfirmRequest="true" />
                                    </Click>
                                </AjaxEvents>
                            </ext:Button>
                        </td>
                        <td>
                            
                            <ext:Button ID="BtnClose" runat="server" Text="关闭" Icon="Cancel">
                                <AjaxEvents>
                                    <Click OnEvent="Buttonclose">
                                        
                                    </Click>
                                </AjaxEvents>
                            </ext:Button>
                        </td>
                        </tr>
                        </table>
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
        </ext:FormPanel>
    </div>
    </form>
</body>
</html>
