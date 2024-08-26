<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AverageBonusDaysAddOnePerson.aspx.cs"
    Inherits="GoldNet.JXKP.AverageBonusDaysAddOnePerson" %>

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
        if (cbbdept.validate() == false) {
                return false;
            }
            if (tfStaffName.validate() == false) {
                return false;
            }           
            return true;
        }
    </script>

</head>
<body>
    <ext:ScriptManager ID="ScriptManager1" runat="server" />
    <form id="form1" runat="server">
    <ext:Store ID="Store2" runat="server">
        <Reader>
            <ext:JsonReader ReaderID="DEPTID">
                <Fields>
                    <ext:RecordField Name="DEPTNAME" />
                    <ext:RecordField Name="DEPTID" />
                </Fields>
            </ext:JsonReader>
        </Reader>
    </ext:Store>
    <div>
        <ext:FormPanel ID="FormPanel1" runat="server" Border="false" AutoScroll="false" ButtonAlign="Right"
            StyleSpec="background-color:Transparent" BodyStyle="background-color:Transparent;margin:10px,0,0,10px">
            <Body>
                <ext:FormLayout ID="FormLayout1" runat="server">
                    <ext:Anchor Horizontal="95%">
                        <ext:ComboBox runat="server" FieldLabel="科室" ID="cbbdept" ValueField="DEPTID" DisplayField="DEPTNAME"
                            StoreID="Store2" ReadOnly="true" BlankText="请选择科室..." CausesValidation="true"
                            AllowBlank="false">
                        </ext:ComboBox>
                    </ext:Anchor>
                    <ext:Anchor Horizontal="95%">
                        <ext:TextField runat="server" ID="tfStaffName" CausesValidation="true" FieldLabel="人员姓名"
                            AllowBlank="false" />
                    </ext:Anchor>
                    <ext:Anchor Horizontal="95%">
                        <ext:Checkbox runat="server" ID="cbisbouns" Checked="true" FieldLabel="是否发放奖金">
                        </ext:Checkbox>
                    </ext:Anchor>
                    <ext:Anchor Horizontal="95%">
                        <ext:NumberField runat="server" ID="nfDAYS" CausesValidation="true" FieldLabel="工作日数"
                            Text="0" AllowBlank="false" SelectOnFocus="true" DecimalPrecision="2" StyleSpec="text-align:right">
                        </ext:NumberField>
                    </ext:Anchor>
                    <ext:Anchor Horizontal="95%">
                        <ext:NumberField runat="server" ID="nfBONUSMODULUS" CausesValidation="true" FieldLabel="岗位系数"
                            Text="0.00" AllowBlank="false" SelectOnFocus="true" DecimalPrecision="2" StyleSpec="text-align:right">
                        </ext:NumberField>
                    </ext:Anchor>
                </ext:FormLayout>
            </Body>
            <Buttons>
                <ext:Button ID="BtnSave" runat="server" Text="保存" Icon="Disk">
                    <AjaxEvents>
                        <Click OnEvent="SaveAddPerson" Before="if (CheckForm()== false){ Ext.Msg.alert('系统提示','请根据红线提示填写正确的信息！');return false;};">
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
