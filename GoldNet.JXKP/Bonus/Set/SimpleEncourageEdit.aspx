<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SimpleEncourageEdit.aspx.cs" Inherits="GoldNet.JXKP.SimpleEncourageEdit" %>
<%@ Register Assembly="Goldnet.Ext.Web" Namespace="Goldnet.Ext.Web" TagPrefix="ext" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title></title>
</head>
<style type="text/css">
        body{
         background-color: #DFE8F6;
         font-size:12px;
        }
    </style>
      <script type="text/javascript">
        var CheckForm = function() {
            if (tfITEMNAME.validate() == false) {
                return false;
            }
            if (taCHECKSTAN.validate() == false) {
                return false;
            }
            if (taREMARK.validate() == false) {
                return false;
            }
           
            return true;
        }
    </script>
 <ext:ScriptManager ID="ScriptManager1" runat="server">
 </ext:ScriptManager>
<body>
    <form id="form1" runat="server"  style="background-color:Transparent" >   
    <div>
        <ext:FormPanel ID="FormPanel1" runat="server" Border="false" AutoScroll="false" ButtonAlign="Right"
            StyleSpec="background-color:Transparent" BodyStyle="background-color:Transparent;margin:10px,0,0,10px">
            <Body>
                <ext:Resizable ID="Resizable2" runat="server" Element="taCHECKSTAN" Handles="South" Wrap="true" Pinned="true" Height="96"  MinHeight="60" Dynamic="true" />
                <ext:Resizable ID="Resizable1" runat="server" Element="taREMARK" Handles="South" Wrap="true" Pinned="true" Height="96"  MinHeight="60" Dynamic="true" />
                <ext:FormLayout ID="FormLayout1" runat="server">
                    <ext:Anchor Horizontal="95%">
                       <ext:TextField runat="server" ID="tfITEMNAME"  CausesValidation="true"  FieldLabel="奖惩名称"   AllowBlank="false" BlankText="请输入奖惩名称" />
                    </ext:Anchor>
                    <ext:Anchor Horizontal="95%">
                         <ext:TextArea runat="server" ID="taCHECKSTAN" CausesValidation="true"  FieldLabel="奖惩标准"   AllowBlank="false"  BlankText="请输入奖惩标准" Height="140" />
                    </ext:Anchor>
                    <ext:Anchor Horizontal="95%">
                        <ext:TextArea runat="server" ID="taREMARK"  CausesValidation="true"  FieldLabel="备注信息"   AllowBlank="false"  BlankText="请输入备注信息" Height="140" />
                    </ext:Anchor>
                </ext:FormLayout>
            </Body>
            <Buttons>
                <ext:Button ID="BtnSave" runat="server" Text="保存" Icon="Disk">
                    <AjaxEvents>
                        <Click OnEvent="SaveEditSimpleEncourage" Before="if (CheckForm()== false){ Ext.Msg.alert('系统提示','请根据红线提示填写正确的信息！');return false;};">
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
