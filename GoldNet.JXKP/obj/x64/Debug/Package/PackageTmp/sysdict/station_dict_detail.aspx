<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="station_dict_detail.aspx.cs"
    Inherits="GoldNet.JXKP.sysdict.station_dict_detail" %>

<%@ Register Assembly="Goldnet.Ext.Web" Namespace="Goldnet.Ext.Web" TagPrefix="ext" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link rel="stylesheet" type="text/css" href="/resources/css/main.css" />
    <style type="text/css">
        body
        {
            background-color: #DFE8F6;
            font-size: 12px;
        }
        td strong
        {
            color: Red;
        }
    </style>

    <script type="text/javascript">
        var CheckForm = function() {
            if (StationNameTxt.validate() == false) {
                return false;
            }
            if (StationTypeCombo.validate() == false) {
                return false;
            }
            if (SortNoNum.validate() == false) {
                return false;
            }
            if (DeptDutyCombo.validate() == false) {
                return false;
            }
            if (ScoreNum1.validate() == false) {
                return false;
            }
            if (ScoreNum2.validate() == false) {
                return false;
            }
            if (ScoreNum3.validate() == false) {
                return false;
            }
            if (ScoreNum4.validate() == false) {
                return false;
            }
            return true;
        }
    </script>

</head>
<body>
    <ext:ScriptManager ID="ScriptManager1" runat="server">
    </ext:ScriptManager>
    <ext:Store runat="server" ID="Store1">
        <Reader>
            <ext:JsonReader>
                <Fields>
                    <ext:RecordField Name="STATION_NAME" />
                    <ext:RecordField Name="SEQUENCE" />
                </Fields>
            </ext:JsonReader>
        </Reader>
    </ext:Store>
    <div class="x-hide-display">
        <ext:GridPanel ID="orderGridPanel" runat="server" AutoScroll="true" StoreID="Store1"
            Height="300">
            <ColumnModel runat="server" Width="100">
                <Columns>
                    <ext:RowNumbererColumn Sortable="false" Width="30">
                    </ext:RowNumbererColumn>
                    <ext:Column DataIndex="STATION_NAME" Width="200" ColumnID="STATION_NAME" MenuDisabled="true"
                        Fixed="true" Sortable="false">
                    </ext:Column>
                </Columns>
            </ColumnModel>
            <SelectionModel>
                <ext:RowSelectionModel runat="server">
                </ext:RowSelectionModel>
            </SelectionModel>
            <Listeners>
                <RowClick Handler="orderMenu.hide(); SortNoNum.setValue(this.store.getAt(rowIndex).get('SEQUENCE')); " />
            </Listeners>
        </ext:GridPanel>
    </div>
    <form id="form1" runat="server" style="background-color: Transparent">
    <div>
        <ext:FormPanel ID="FormPanel1" runat="server" Border="false" AutoScroll="true" ButtonAlign="Right"
            StyleSpec="background-color:transparent" BodyStyle="background-color:transparent">
            <Body>
                <ext:Panel ID="Panel1" runat="server" Border="false" AutoHeight="true" AutoWidth="true"
                    StyleSpec="background-color:transparent" BodyStyle="background-color:transparent">
                    <Body>
                        <ext:FieldSet ID="fieldset1" runat="server" Title="岗位基本信息" Collapsible="true" Collapsed="false"
                            StyleSpec="margin:2px" Width="510" BodyStyle="background-color:Transparent;">
                            <Body>
                                <table width="95%">
                                    <tbody>
                                        <tr>
                                            <td style="width: 25%;">
                                                岗位名称:<strong>*</strong><br />
                                                <ext:TextField ID="StationNameTxt" CausesValidation="true" runat="server" FieldLabel="岗位名称"
                                                    Width="100" AllowBlank="false" BlankText="岗位名称不能为空！" />
                                            </td>
                                            <td style="width: 25%;">
                                                科室类别:<strong>*</strong><br />
                                                <ext:ComboBox ID="StationTypeCombo" CausesValidation="true" runat="server" AllowBlank="false"
                                                    BlankText="请选择该岗位类别！" Width="100">
                                                </ext:ComboBox>
                                            </td>
                                            <td style="width: 25%;" colspan="2">
                                                显示顺序:<strong>*</strong><br />
                                                <ext:NumberField AllowNegative="false" ID="SortNoNum" runat="server" CausesValidation="true"
                                                    AllowBlank="false" FieldLabel="显示顺序" Width="100" StyleSpec="text-align:right" />
                                                <ext:Button runat="server" ID="Btn_ShowOrder" Icon="ArrowDown" StyleSpec="margin-left:105px;margin-top:-23px;">
                                                    <AjaxEvents>
                                                        <Click OnEvent="ShowSortNum">
                                                        </Click>
                                                    </AjaxEvents>
                                                    <Menu>
                                                        <ext:Menu ID="orderMenu" runat="server" Width="200">
                                                            <Items>
                                                                <ext:ElementMenuItem Target="#{orderGridPanel}" Shift="false" />
                                                            </Items>
                                                        </ext:Menu>
                                                    </Menu>
                                                </ext:Button>
                                                <ext:Hidden ID="SortNoNumHidden" runat="server">
                                                </ext:Hidden>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width: 25%;">
                                                组织:<strong>*</strong><br />
                                                <ext:ComboBox ID="DeptDutyCombo" runat="server" Width="100" AllowBlank="false" BlankText="请选择该岗位指标集组织类别！"
                                                    FieldLabel="组织类别" CausesValidation="true">
                                                </ext:ComboBox>
                                            </td>
                                            <td style="width: 25%;">
                                                指标集类别:<br />
                                                <ext:ComboBox ID="GatherTypeCombo" runat="server" Editable="true" EmptyText="请选择"
                                                    ForceSelection="false" AllowBlank="true" Width="100">
                                                    <AjaxEvents>
                                                        <Select OnEvent="SelectedGatherType">
                                                            <EventMask ShowMask="true" />
                                                        </Select>
                                                    </AjaxEvents>
                                                </ext:ComboBox>
                                            </td>
                                            <td style="width: 25%;">
                                                岗位指标集:<br />
                                                <ext:ComboBox ID="GuideGatherCombo" runat="server" Editable="true" EmptyText="请选择"
                                                    AllowBlank="true" ForceSelection="false" Width="100" ListWidth="220">
                                                </ext:ComboBox>
                                            </td>
                                            <td style="width: 25%;">
                                                是否考核:<strong>*</strong><br />
                                                <ext:RadioGroup ID="RadioGroup1" runat="server" ClearCls="x-form-radio-group" ItemCls="x-check-group-base"
                                                    FieldLabel="是否考核" StyleSpec="background-color: transparent;">
                                                    <Items>
                                                        <ext:Radio ID="Radio1" runat="server" BoxLabel="是" AutoWidth="true" StyleSpec="background-color: transparent;" />
                                                        <ext:Radio ID="Radio2" runat="server" BoxLabel="否" Checked="true" StyleSpec="background-color: transparent;"
                                                            AutoWidth="true" />
                                                    </Items>
                                                </ext:RadioGroup>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width: 25%;">
                                                内部管理总分:<strong>*</strong><br />
                                                <ext:NumberField ID="ScoreNum1" AllowNegative="false" AllowBlank="false" StyleSpec="text-align:right"
                                                    BlankText="该项分值不能为空！" SelectOnFocus="true" CausesValidation="true" runat="server"
                                                    FieldLabel="" Width="99" />
                                            </td>
                                            <td style="width: 25%;">
                                                经济财务总分:<strong>*</strong><br />
                                                <ext:NumberField ID="ScoreNum2" AllowNegative="false" AllowBlank="false" StyleSpec="text-align:right"
                                                    BlankText="该项分值不能为空！" SelectOnFocus="true" CausesValidation="true" runat="server"
                                                    FieldLabel="" Width="99" />
                                            </td>
                                            <td style="width: 25%;">
                                                客户满意度总分:<strong>*</strong><br />
                                                <ext:NumberField ID="ScoreNum3" AllowNegative="false" AllowBlank="false" StyleSpec="text-align:right"
                                                    BlankText="该项分值不能为空！" SelectOnFocus="true" CausesValidation="true" runat="server"
                                                    FieldLabel="" Width="99" />
                                            </td>
                                            <td style="width: 25%;">
                                                学习与成长总分:<strong>*</strong><br />
                                                <ext:NumberField ID="ScoreNum4" AllowNegative="false" AllowBlank="false" StyleSpec="text-align:right"
                                                    BlankText="该项分值不能为空！" SelectOnFocus="true" CausesValidation="true" runat="server"
                                                    FieldLabel="" Width="99" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width: 25%;">
                                                薪金标准:<br />
                                                <ext:TextField ID="SalaryTxt" runat="server" FieldLabel="薪金标准" Width="100" />
                                            </td>
                                            <td style="width: 25%;">
                                                核准人:<br />
                                                <ext:TextField ID="ApplyTxt" runat="server" FieldLabel=" 核准人" Width="100" />
                                            </td>
                                            <td style="width: 25%;">
                                                录入人员:<br />
                                                <ext:TextField ID="CreateTxt" runat="server" FieldLabel=" 录入人员" Width="100" />
                                            </td>
                                            <td style="width: 25%;">
                                                录入日期:<br />
                                                <ext:DateField ID="CreateDate" runat="server" FieldLabel="录入日期" Format="yyyy-m-dd"
                                                    Width="100" EnableKeyEvents="true" />
                                                <ext:KeyNav ID="KeyNav1" runat="server" Target="CreateDate">
                                                    <Enter Handler="var str = document.getElementById('CreateDate').value ; var   reg=/^(\d{4})(\d{2})(\d{2})$/; document.getElementById('CreateDate').value   =   str.replace(reg, '$1-$2-$3');" />
                                                </ext:KeyNav>
                                            </td>
                                        </tr>
                                    </tbody>
                                </table>
                            </Body>
                        </ext:FieldSet>
                        <ext:FieldSet ID="fieldset3" runat="server" Title="岗位说明" Collapsible="true" Collapsed="false"
                            StyleSpec="margin:2px" Width="510" BodyStyle="background-color:Transparent;">
                            <Body>
                                <ext:TextArea ID="StationTxt" runat="server" FieldLabel="岗位说明" MaxLength="500" Width="486"
                                    Height="96" StyleSpec="margin:2px" />
                                <ext:Resizable ID="Resizable1" runat="server" Element="StationTxt" Handles="South"
                                    Wrap="true" Pinned="true" Width="482" Height="96" MinWidth="200" MinHeight="60"
                                    Dynamic="true" />
                            </Body>
                        </ext:FieldSet>
                        <ext:FieldSet ID="fieldset2" runat="server" Title="工作内容" Collapsible="true" Collapsed="true"
                            StyleSpec="margin:2px" Width="510" BodyStyle="background-color:Transparent;">
                            <Body>
                                <ext:TextArea ID="WorkTxt" runat="server" FieldLabel="工作说明" MaxLength="500" Width="486"
                                    Height="96" StyleSpec="margin:2px" />
                                <ext:Resizable ID="Resizable2" runat="server" Element="WorkTxt" Handles="South" Wrap="true"
                                    Pinned="true" Width="482" Height="96" MinWidth="200" MinHeight="60" Dynamic="true" />
                            </Body>
                        </ext:FieldSet>
                        <ext:FieldSet ID="fieldset4" runat="server" Title="任职资格" Collapsible="true" Collapsed="true"
                            StyleSpec="margin:2px" Width="510" BodyStyle="background-color:Transparent;">
                            <Body>
                                <ext:TextArea ID="TitleTxt" runat="server" FieldLabel="任职资格" MaxLength="500" Width="486"
                                    Height="96" StyleSpec="margin:2px" />
                                <ext:Resizable ID="Resizable3" runat="server" Element="TitleTxt" Handles="South"
                                    Wrap="true" Pinned="true" Width="482" Height="96" MinWidth="200" MinHeight="60"
                                    Dynamic="true" />
                            </Body>
                        </ext:FieldSet>
                        <ext:FieldSet ID="fieldset5" runat="server" Title="工作环境" Collapsible="true" Collapsed="true"
                            StyleSpec="margin:2px" Width="510" BodyStyle="background-color:Transparent;">
                            <Body>
                                <ext:TextArea ID="JobTxt" runat="server" FieldLabel="工作环境" MaxLength="500" Width="486"
                                    Height="96" StyleSpec="margin:2px" />
                                <ext:Resizable ID="Resizable4" runat="server" Element="JobTxt" Handles="South" Wrap="true"
                                    Pinned="true" Width="482" Height="96" MinWidth="200" MinHeight="60" Dynamic="true" />
                            </Body>
                        </ext:FieldSet>
                    </Body>
                </ext:Panel>
            </Body>
            <Buttons>
                <ext:Button ID="BtnSave" runat="server" Text="确定" Icon="Disk">
                    <AjaxEvents>
                        <Click OnEvent="BtnSave_Click" Before="if (CheckForm()== false){ Ext.Msg.alert('系统提示','请根据红线提示填写正确的信息！');return false;};">
                            <ExtraParams>
                                <ext:Parameter Name="Values" Value="Ext.encode(#{FormPanel1}.getValue())" Mode="Raw">
                                </ext:Parameter>
                            </ExtraParams>
                            <EventMask ShowMask="true" Msg="请稍候..." />
                        </Click>
                    </AjaxEvents>
                </ext:Button>
                <ext:Button ID="CancelButton" runat="server" Text="取消" Icon="Cancel">
                    <Listeners>
                        <Click Handler="parent.DetailWin.hide(); parent.DetailWin.clearContent();" />
                    </Listeners>
                </ext:Button>
            </Buttons>
        </ext:FormPanel>
    </div>
    </form>
</body>
</html>
