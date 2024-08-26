<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="UnitDetail.aspx.cs" Inherits="GoldNet.JXKP.RLZY.BaseInfoMaintain.UnitDetail" %>

<%@ Register Assembly="Goldnet.Ext.Web" Namespace="Goldnet.Ext.Web" TagPrefix="ext" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>无标题页</title>
</head>
<body>
    <ext:ScriptManager ID="ScriptManager1" runat="server" />
    <form id="form1" runat="server">
    <div>
        <ext:ViewPort ID="ViewPort1" runat="server">
            <Body>
                <ext:BorderLayout ID="BorderLayout2" runat="server">
                    <North>
                        <ext:Toolbar ID="Toolbar1" runat="server">
                            <Items>
                                <ext:NumberField ID="NumYear" runat="server" MaxLength="4" Width="40">
                                </ext:NumberField>
                                <ext:ToolbarTextItem ID="ToolbarTextItem2" runat="server" Text="年" />
                                <ext:ComboBox runat="server" ID="Comb_StartMonth" Width="40" ListWidth="40" SelectedIndex="0">
                                </ext:ComboBox>
                                <ext:ToolbarTextItem ID="ToolbarTextItem1" runat="server" Text="月" />
                                <ext:Button ID="btnSearch" runat="server" Text="查询" Icon="DatabaseGo">
                                    <AjaxEvents>
                                            <Click OnEvent="QueryUnit">
                                                <EventMask  Msg="正在查询....."/>
                                            </Click>
                                    </AjaxEvents>
                                </ext:Button>
                                <ext:Button ID="btnSave" runat="server" Text="保存" Icon="Disk">
                                    <AjaxEvents>
                                        <Click OnEvent="SaveInfo">
                                            <EventMask  ShowMask="true" Msg="正在保存....."/>
                                        </Click>
                                    </AjaxEvents>
                                </ext:Button>
                            </Items>
                        </ext:Toolbar>
                    </North>
                    <Center>
                        <ext:Panel runat="server" Width="400">
                            <Body>
                                <ext:ColumnLayout ID="ColumnLayout1" runat="server">
                                    <ext:LayoutColumn ColumnWidth=".2">
                                        <ext:Panel ID="Panel1" runat="server" Border="false" Header="false" BodyStyle="margin:10px;">
                                            <Body>
                                                <ext:FormLayout ID="FormLayout1" runat="server" LabelAlign="Left">
                                                    <ext:Anchor Horizontal="92%">
                                                        <ext:TextField ID="txtUnitCode" runat="server" FieldLabel="单位代码" />
                                                    </ext:Anchor>
                                                    <ext:Anchor Horizontal="92%">
                                                        <ext:NumberField ID="txtAuthorizedPepleTotal" runat="server" FieldLabel="编制人数"> </ext:NumberField>
                                                    </ext:Anchor>
                                                    <ext:Anchor Horizontal="92%">
                                                        <ext:NumberField ID="txtFactCadreHigher" runat="server" FieldLabel="实有师以上干部">
                                                        </ext:NumberField>
                                                    </ext:Anchor>
                                                    <ext:Anchor Horizontal="92%">
                                                        <ext:TextField ID="txtPlacePhone" runat="server" FieldLabel="地方号码">
                                                        </ext:TextField>
                                                    </ext:Anchor>
                                                    <ext:Anchor Horizontal="92%">
                                                        <ext:TextField ID="txtUnitLevelCode" runat="server" FieldLabel="单位等级代码">
                                                        </ext:TextField>
                                                    </ext:Anchor>
                                                    <ext:Anchor Horizontal="92%">
                                                        <ext:NumberField ID="txtOperationArea" runat="server" FieldLabel="业务用房面积">
                                                        </ext:NumberField>
                                                    </ext:Anchor>
                                                    <ext:Anchor Horizontal="92%">
                                                        <ext:NumberField ID="txtLibrary" runat="server" FieldLabel="图书馆">
                                                        </ext:NumberField>
                                                    </ext:Anchor>
                                                    <ext:Anchor Horizontal="92%">
                                                        <ext:TextField ID="txtMagazineSortNum" runat="server" FieldLabel="订阅期刊数量">
                                                        </ext:TextField>
                                                    </ext:Anchor>
                                                    <ext:Anchor Horizontal="92%">
                                                        <ext:TextField ID="txtMailingAddress" runat="server" FieldLabel="通讯地址" Width="500" />
                                                    </ext:Anchor>
                                                    <ext:Anchor Horizontal="92%">
                                                        <ext:TextField ID="txtLocation" runat="server" FieldLabel="单位驻地" Width="500" />
                                                    </ext:Anchor>
                                                    <ext:Anchor Horizontal="92%">
                                                        <ext:TextField ID="txtUintName" runat="server" FieldLabel="单位名称" />
                                                    </ext:Anchor>
                                                    <ext:Anchor Horizontal="92%">
                                                        <ext:NumberField ID="txtAuthorizedCadreHighe" runat="server" FieldLabel="编制师以上干部" />
                                                    </ext:Anchor>
                                                    <ext:Anchor Horizontal="92%">
                                                        <ext:NumberField ID="txtFactRelation" runat="server" FieldLabel="实有包干家属"/>
                                                        
                                                    </ext:Anchor>
                                                    <ext:Anchor Horizontal="92%">
                                                        <ext:ComboBox ID="cboCharacter" runat="server" FieldLabel="单位性质">
                                                        </ext:ComboBox>
                                                    </ext:Anchor>
                                                    <ext:Anchor Horizontal="92%">
                                                        <ext:TextField ID="txtCondtion" runat="server" FieldLabel="驻地环境">
                                                        </ext:TextField>
                                                    </ext:Anchor>
                                                    <ext:Anchor Horizontal="92%">
                                                        <ext:NumberField ID="txtOfficeArea" runat="server" FieldLabel="办公用房面积">
                                                        </ext:NumberField>
                                                    </ext:Anchor>
                                                    <ext:Anchor Horizontal="92%">
                                                        <ext:NumberField ID="txtBookNum" runat="server" FieldLabel="图书馆图书数">
                                                        </ext:NumberField>
                                                    </ext:Anchor>
                                                    <ext:Anchor Horizontal="92%">
                                                        <ext:NumberField ID="txtForeignMagazine" runat="server" FieldLabel="其中外文图书数">
                                                        </ext:NumberField>
                                                    </ext:Anchor>
                                                </ext:FormLayout>
                                            </Body>
                                        </ext:Panel>
                                    </ext:LayoutColumn>
                                    <ext:LayoutColumn ColumnWidth=".2">
                                        <ext:Panel ID="Panel4" runat="server" Border="false" BodyStyle="margin:10px;">
                                            <Body>
                                                <ext:FormLayout ID="FormLayout9" runat="server" LabelAlign="Left">
                                                    <ext:Anchor Horizontal="92%">
                                                        <ext:TextField ID="txtDasNameCommissar" runat="server" FieldLabel="政委姓名">
                                                        </ext:TextField>
                                                    </ext:Anchor>
                                                    <ext:Anchor Horizontal="92%">
                                                        <ext:TextField ID="txtZipCode" runat="server" FieldLabel="邮政编码">
                                                        </ext:TextField>
                                                    </ext:Anchor>
                                                    <ext:Anchor Horizontal="92%">
                                                        <ext:TextField ID="txtSubUnit" runat="server" FieldLabel="隶属单位">
                                                        </ext:TextField>
                                                    </ext:Anchor>
                                                    <ext:Anchor Horizontal="92%">
                                                        <ext:ComboBox ID="cboUnitLevel" runat="server" FieldLabel="单位等级" />
                                                    </ext:Anchor>
                                                    <ext:Anchor Horizontal="92%">
                                                        <ext:NumberField ID="txtExistArea" runat="server" FieldLabel="辅助用房面积" />
                                                    </ext:Anchor>
                                                    <ext:Anchor Horizontal="92%">
                                                        <ext:NumberField ID="txtAmbulance" runat="server" FieldLabel="救护车数" />
                                                    </ext:Anchor>
                                                    <ext:Anchor Horizontal="92%">
                                                        <ext:NumberField ID="txtComputerNum" runat="server" FieldLabel="计算机数" />
                                                    </ext:Anchor>
                                                    <ext:Anchor Horizontal="92%">
                                                        <ext:NumberField ID="txtComputerOldNum" runat="server" FieldLabel="486以上" />
                                                    </ext:Anchor>
                                                    <ext:Anchor Horizontal="92%">
                                                        <ext:TextField ID="txtCareBilty" runat="server" FieldLabel="医务责任人" />
                                                    </ext:Anchor>
                                                    <ext:Anchor Horizontal="92%">
                                                        <ext:TextField ID="txtMedicalBility" runat="server" FieldLabel="护理责任人" />
                                                    </ext:Anchor>
                                                    <ext:Anchor Horizontal="92%">
                                                        <ext:TextField ID="txtDasNameDean" runat="server" FieldLabel="院长姓名">
                                                        </ext:TextField>
                                                    </ext:Anchor>
                                                    <ext:Anchor Horizontal="92%">
                                                        <ext:TextField ID="txtFactRetire" runat="server" FieldLabel="实有离退休干部">
                                                        </ext:TextField>
                                                    </ext:Anchor>
                                                    <ext:Anchor Horizontal="92%">
                                                        <ext:TextField ID="txtArmPhone" runat="server" FieldLabel="军线号码">
                                                        </ext:TextField>
                                                    </ext:Anchor>
                                                    <ext:Anchor Horizontal="92%">
                                                        <ext:TextField ID="txtCharacterCode" runat="server" FieldLabel="单位性质代码" />
                                                    </ext:Anchor>
                                                    <ext:Anchor Horizontal="92%">
                                                        <ext:NumberField ID="txtTotalArea" runat="server" FieldLabel="占地" />
                                                    </ext:Anchor>
                                                    <ext:Anchor Horizontal="92%">
                                                        <ext:NumberField ID="txtAssistantArea" runat="server" FieldLabel="生活用房面积" />
                                                    </ext:Anchor>
                                                    <ext:Anchor Horizontal="92%">
                                                        <ext:NumberField ID="txtForeignBookNum" runat="server" FieldLabel="图书馆图外文书数" />
                                                    </ext:Anchor>
                                                    <ext:Anchor Horizontal="92%">
                                                        <ext:NumberField ID="txtSortNum" runat="server" FieldLabel="订购期刊种类" />
                                                    </ext:Anchor>
                                                </ext:FormLayout>
                                            </Body>
                                        </ext:Panel>
                                    </ext:LayoutColumn>
                                     <ext:LayoutColumn ColumnWidth=".6">
                                        <ext:Panel ID="Panel2" runat="server" Border="false">
                                            <Body>
                                                <ext:FormLayout ID="FormLayout3" runat="server" LabelAlign="Left">
                                                </ext:FormLayout>
                                            </Body>
                                        </ext:Panel>
                                    </ext:LayoutColumn>
                                </ext:ColumnLayout>
                            </Body>
                        </ext:Panel>
                    </Center>
                </ext:BorderLayout>
            </Body>
        </ext:ViewPort>
    </div>
    </form>
</body>
</html>
