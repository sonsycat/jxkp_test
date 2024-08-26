<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="BonusGuideEdit.aspx.cs"
    Inherits="GoldNet.JXKP.BonusGuideEdit" %>

<%@ Register Assembly="Goldnet.Ext.Web" Namespace="Goldnet.Ext.Web" TagPrefix="ext" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>指标信息</title>
    <link rel="stylesheet" type="text/css" href="/resources/css/main.css" />
    <style type="text/css">
        body
        {
            background-color: #DFE8F6;
        }
    </style>

    <script type="text/javascript">
        var CheckForm = function() {
            if (GuideNameTxt.validate() == false) {
                return false;
            }
            return true;
        }
        var SetCorrelationCombState = function(flg) {
            flg = !flg;
            EvaluationYearComb.setDisabled(flg);
            GatherTypeComb.setDisabled(flg);
            GatherClassComb.setDisabled(flg);
            OrganTypeComb.setDisabled(flg);
            GuideGatherCombo.setDisabled(flg);
        }
    </script>

</head>
<body>
    <ext:ScriptManager ID="ScriptManager1" runat="server">
    </ext:ScriptManager>
    <ext:Store ID="BSCStore" runat="server" AutoLoad="true">
        <Reader>
            <ext:JsonReader ReaderID="BSC_CLASS_CODE">
                <Fields>
                    <ext:RecordField Name="BSC_CLASS_CODE" />
                    <ext:RecordField Name="BSC_CLASS_NAME" />
                </Fields>
            </ext:JsonReader>
        </Reader>
    </ext:Store>
    <ext:Store ID="GuideClassStore" runat="server" AutoLoad="true">
        <Reader>
            <ext:JsonReader ReaderID="GUIDE_GROUP_TYPE">
                <Fields>
                    <ext:RecordField Name="GUIDE_GROUP_TYPE" />
                    <ext:RecordField Name="GUIDE_GROUP_TYPE_NAME" />
                </Fields>
            </ext:JsonReader>
        </Reader>
    </ext:Store>
    <ext:Store ID="GuideGatherStore" runat="server" AutoLoad="true">
        <Reader>
            <ext:JsonReader ReaderID="GUIDE_GATHER_CODE">
                <Fields>
                    <ext:RecordField Name="GUIDE_GATHER_CODE" />
                    <ext:RecordField Name="GUIDE_GATHER_NAME" />
                </Fields>
            </ext:JsonReader>
        </Reader>
    </ext:Store>
    <form id="form1" runat="server" style="background-color: Transparent">
    <div>
        <ext:FormPanel ID="FormPanel1" runat="server" Border="false" AutoScroll="true" Width="515"
            ButtonAlign="Right" BodyStyle="background-color:transparent">
            <Body>
                <ext:FieldSet ID="fieldset2" runat="server" Title="指标类别信息" Collapsible="true" Collapsed="false"
                    StyleSpec="margin:2px" Width="510" BodyStyle="background-color:Transparent;">
                    <Body>
                        <table>
                            <tr>
                                <td>
                                    BSC分类:
                                </td>
                                <td>
                                    <ext:ComboBox ID="BSC1Comb" runat="server" Editable="false" Width="90" ListWidth="100"
                                        HideTrigger="true" Disabled="true">
                                        <AjaxEvents>
                                            <Select OnEvent="SelectedBSC1">
                                                <EventMask ShowMask="true" />
                                            </Select>
                                        </AjaxEvents>
                                    </ext:ComboBox>
                                </td>
                                <td>
                                    <ext:ComboBox ID="BSC2Comb" runat="server" Editable="false" Width="100" ListWidth="100"
                                        StoreID="BSCStore" DisplayField="BSC_CLASS_NAME" ValueField="BSC_CLASS_CODE"
                                        HideTrigger="true" Disabled="true">
                                        <AjaxEvents>
                                            <Select OnEvent="SelectedBSC2">
                                                <EventMask ShowMask="true" />
                                            </Select>
                                        </AjaxEvents>
                                    </ext:ComboBox>
                                </td>
                                <td>
                                    部门:
                                </td>
                                <td>
                                    <ext:ComboBox ID="DeptTypeComb" runat="server" AllowBlank="false" Width="80" ListWidth="90">
                                    </ext:ComboBox>
                                </td>
                                <td>
                                    组织类别:
                                </td>
                                <td>
                                    <ext:ComboBox ID="OrganComb" runat="server" AllowBlank="false" Width="40" ListWidth="50">
                                        <AjaxEvents>
                                            <Select OnEvent="SelectedBSC2">
                                                <EventMask ShowMask="true" />
                                            </Select>
                                        </AjaxEvents>
                                    </ext:ComboBox>
                                </td>
                            </tr>
                        </table>
                    </Body>
                </ext:FieldSet>
                <ext:FieldSet ID="fieldset1" runat="server" Title="指标基本信息" Collapsible="true" Collapsed="false"
                    StyleSpec="margin:2px" Width="510" BodyStyle="background-color:Transparent;">
                    <Body>
                        <table width="95%">
                            <tr>
                                <td style="width: 25%;">
                                    指标代码:
                                </td>
                                <td>
                                    <ext:TextField ID="GuideCodeTxt" runat="server" ReadOnly="true" FieldLabel="岗位编码"
                                        Width="100" />
                                    <ext:Hidden ID="GuideCodeOriginal" runat="server">
                                    </ext:Hidden>
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 25%;">
                                    指标名称:
                                </td>
                                <td>
                                    <ext:TextField ID="GuideNameTxt" runat="server" AllowBlank="false" BlankText="请输入指标名称!"
                                        Width="200" MaxLength="50" />
                                </td>
                              
                            </tr>
                            <tr>
                                <td style="width: 25%;">
                                    指标表达式:
                                </td>
                                <td>
                                    <table cellspacing="0" cellpadding="0">
                                        <tr>
                                            <td>
                                                <ext:ComboBox ID="IsExpressComb" runat="server" Width="40" ListWidth="40" Editable="false">
                                                    <Items>
                                                        <ext:ListItem Text="是" Value="1" />
                                                        <ext:ListItem Text="否" Value="0" />
                                                    </Items>
                                                    <Listeners>
                                                        <Select Handler="GuideExpressTxt.setDisabled(this.value==0?true:false);if (this.value==0) GuideExpressTxt.setValue('');" />
                                                    </Listeners>
                                                </ext:ComboBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="padding: 1px">
                                                <ext:TextArea ID="GuideExpressTxt" Disabled="true" runat="server" Width="350" Height="50"
                                                    MaxLength="450" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 25%;">
                                    显示停用状态:
                                </td>
                                <td>
                                    <ext:ComboBox ID="IsPageComb" runat="server" Width="60" Editable="false">
                                        <Items>
                                            <ext:ListItem Text="启用" Value="1" />
                                            <ext:ListItem Text="停用" Value="0" />
                                            <ext:ListItem Text="不显示" Value="2" />
                                        </Items>
                                    </ext:ComboBox>
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 25%;">
                                    奖金类型:
                                </td>
                                <td>
                                    <ext:ComboBox ID="cbbType" runat="server" Width="60">
                                        <Items>
                                            <ext:ListItem Text="空" Value="" />
                                            <ext:ListItem Text="核算" Value="5" />
                                            <ext:ListItem Text="平均奖" Value="6" />
                                        </Items>
                                    </ext:ComboBox>
                                </td>
                            </tr>
                            <tr>
                             <td style="width: 25%;">
                                    说明:
                                </td>
                                            <td style="padding: 1px">
                                                <ext:TextArea ID="TextAreaexplain"  runat="server" Width="350" Height="50"
                                                    MaxLength="450" />
                                            </td>
                                        </tr>
                                        <tr>
                                <td style="width: 25%;">
                                    奖金指标显示宽度:
                                </td>
                                <td>
                                    <ext:TextField ID="TextField2" runat="server" AllowBlank="false" BlankText="请输入指标名称!"
                                        Width="50" MaxLength="50" />
                                </td>
                            </tr>
                            <tr style="visibility: hidden">
                                <td style="width: 25%;">
                                    是否高优指标:
                                </td>
                                <td>
                                    <ext:ComboBox ID="IsHighComb" runat="server" Width="40" ListWidth="40" Editable="false">
                                        <Items>
                                            <ext:ListItem Text="是" Value="1" />
                                            <ext:ListItem Text="否" Value="0" />
                                        </Items>
                                    </ext:ComboBox>
                                </td>
                            </tr>
                            <tr style="visibility: hidden">
                                <td style="width: 25%;">
                                    是否综合评价指标:
                                </td>
                                <td>
                                    <ext:ComboBox ID="IsZhpjComb" runat="server" Width="40" ListWidth="40" Editable="false">
                                        <Items>
                                            <ext:ListItem Text="是" Value="1" />
                                            <ext:ListItem Text="否" Value="0" />
                                        </Items>
                                    </ext:ComboBox>
                                </td>
                            </tr>
                            <tr style="visibility: hidden">
                                <td style="width: 25%;">
                                    指标类型:
                                </td>
                                <td>
                                    <ext:ComboBox ID="IsABSComb" runat="server" Width="90" Editable="false">
                                        <Items>
                                            <ext:ListItem Text="绝对值指标" Value="1" />
                                            <ext:ListItem Text="相对值指标" Value="0" />
                                        </Items>
                                    </ext:ComboBox>
                                </td>
                            </tr>
                             <tr style="visibility: hidden">
                                <td style="width: 25%;">
                                    指标类型:
                                </td>
                                <td>
                                    <ext:ComboBox ID="ComboBox1" runat="server" Width="90" Editable="false">
                                        <Items>
                                            <ext:ListItem Text="绝对值指标" Value="1" />
                                            <ext:ListItem Text="相对值指标" Value="0" />
                                        </Items>
                                    </ext:ComboBox>
                                </td>
                            </tr>
                             
 


                        </table>
                    </Body>
                </ext:FieldSet>
            </Body>
            <Buttons>
                <ext:Button ID="BtnSave" runat="server" Text="保存" Icon="Disk">
                    <AjaxEvents>
                        <Click OnEvent="BtnSave_Click" Before="if (CheckForm()== false){ Ext.Msg.alert('系统提示','请根据红线提示填写正确的信息！');return false;};">
                            <Confirmation BeforeConfirm="if ((GuideCodeOriginal.value !='' )&& (GuideCodeTxt.value != GuideCodeOriginal.value )){config.confirmation.message = '注意:该指标的指标代码已经发生变化，这将更新<br/> 该指标相关的所有信息，确定要继续吗？'; }else{ return false;}"
                                Title="系统提示" ConfirmRequest="true" />
                            <ExtraParams>
                                <ext:Parameter Name="Values" Value="Ext.encode(#{FormPanel1}.getValue())" Mode="Raw">
                                </ext:Parameter>
                            </ExtraParams>
                            <EventMask ShowMask="true" Msg="请稍候..." />
                        </Click>
                    </AjaxEvents>
                </ext:Button>
                <ext:Button ID="CancelButton" runat="server" Text="取消" Icon="ArrowUndo">
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
