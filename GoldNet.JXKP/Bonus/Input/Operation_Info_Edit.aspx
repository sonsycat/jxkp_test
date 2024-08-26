<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Operation_Info_Edit.aspx.cs"
    Inherits="GoldNet.JXKP.Bonus.Input.Operation_Info_Edit" %>

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
</head>
<body>
    <ext:ScriptManager ID="ScriptManager1" runat="server">
    </ext:ScriptManager>
    <form id="form1" runat="server">
    <div>
        <ext:FormPanel ID="FormPanel1" runat="server" Border="false" AutoHeight="true" AutoWidth="true"
            AutoScroll="true" ButtonAlign="Right" StyleSpec="background-color:transparent"
            BodyStyle="background-color:transparent">
            <Body>
                <ext:Panel ID="Panel1" runat="server" Border="false" AutoHeight="true" AutoWidth="true"
                    StyleSpec="background-color:transparent" BodyStyle="background-color:transparent">
                    <TopBar>
                        <ext:Toolbar ID="Toolbar1" runat="server">
                            <Items>
                                <ext:ToolbarSeparator ID="ToolbarSeparator1" runat="server">
                                </ext:ToolbarSeparator>
                                <ext:Button ID="select" runat="server" Text="查询" Icon="Zoom">
                                    <AjaxEvents>
                                        <Click OnEvent="Buttonselect_Click">
                                        </Click>
                                    </AjaxEvents>
                                </ext:Button>
                                <ext:ToolbarSeparator ID="ToolbarSeparator2" runat="server">
                                </ext:ToolbarSeparator>
                                <ext:Button ID="save" runat="server" Text="保存" Icon="Disk">
                                    <AjaxEvents>
                                        <Click OnEvent="Buttonsave_Click">
                                        </Click>
                                    </AjaxEvents>
                                </ext:Button>
                                <ext:ToolbarSeparator ID="ToolbarSeparator3" runat="server">
                                </ext:ToolbarSeparator>
                                <ext:Button ID="btnCancle" runat="server" Text="返回" Icon="ArrowUndo">
                                    <AjaxEvents>
                                        <Click OnEvent="btnCancle_Click">
                                        </Click>
                                    </AjaxEvents>
                                </ext:Button>
                            </Items>
                        </ext:Toolbar>
                    </TopBar>
                </ext:Panel>
                <ext:ColumnLayout ID="ColumnLayout2" runat="server">
                    <ext:LayoutColumn ColumnWidth=".5">
                        <ext:Panel ID="Panel2" runat="server" Border="false" Header="false" BodyStyle="background-color:Transparent;margin:10px;">
                            <Body>
                                <ext:FormLayout ID="FormLayout2" runat="server" LabelAlign="Left">
                                    <ext:Anchor>
                                        <ext:ComboBox ID="months" runat="server" Width="150" AllowBlank="true" EmptyText="请选择..."
                                            FieldLabel="手术类别" SelectedIndex="0">
                                            <Items>
                                                <ext:ListItem Text="住院手术" Value="1" />
                                                <ext:ListItem Text="门诊手术" Value="0" />
                                            </Items>
                                        </ext:ComboBox>
                                    </ext:Anchor>
                                    <ext:Anchor>
                                        <ext:TextField ID="text_PATIENT_ID" runat="server" DataIndex="PATIENT_ID" MsgTarget="Side"
                                            AllowBlank="false" FieldLabel="病人标识号" Width="150" />
                                    </ext:Anchor>
                                    <ext:Anchor>
                                        <ext:DateField ID="date_SCHEDULED_DATE_TIME" runat="server" FieldLabel="手术时间：" Width="150"
                                            EnableKeyEvents="true" DataIndex="SCHEDULED_DATE_TIME" ReadOnly="true" />
                                    </ext:Anchor>
                                    <ext:Anchor>
                                        <ext:TextField ID="text_OPERATION_NAME" runat="server" DataIndex="OPERATION_NAME"
                                            MsgTarget="Side" AllowBlank="false" FieldLabel="手术名称" Width="150" />
                                    </ext:Anchor>
                                    <ext:Anchor>
                                        <ext:TextField ID="text_SURGEON" runat="server" DataIndex="SURGEON" MsgTarget="Side"
                                            AllowBlank="false" FieldLabel="手术者" Width="150" />
                                    </ext:Anchor>
                                    <ext:Anchor>
                                        <ext:TextField ID="text_FIRST_ASSISTANT" runat="server" DataIndex="FIRST_ASSISTANT"
                                            FieldLabel="第一手术助手" Width="150" />
                                    </ext:Anchor>
                                    <ext:Anchor>
                                        <ext:TextField ID="text_SECOND_ASSISTANT" runat="server" DataIndex="SECOND_ASSISTANT"
                                            FieldLabel="第二手术助手" Width="150" />
                                    </ext:Anchor>
                                    <ext:Anchor>
                                        <ext:TextField ID="text_THIRD_ASSISTANT" runat="server" DataIndex="THIRD_ASSISTANT"
                                            FieldLabel="第三手术助手" Width="150" />
                                    </ext:Anchor>
                                    <ext:Anchor>
                                        <ext:TextField ID="text_FOURTH_ASSISTANT" runat="server" DataIndex="FOURTH_ASSISTANT"
                                            FieldLabel="第四手术助手" Width="150" />
                                    </ext:Anchor>
                                    <ext:Anchor>
                                        <ext:TextField ID="text_ANESTHESIA_DOCTOR" runat="server" DataIndex="ANESTHESIA_DOCTOR"
                                            MsgTarget="Side" AllowBlank="false" FieldLabel="麻醉医师" Width="150" />
                                    </ext:Anchor>
                                </ext:FormLayout>
                            </Body>
                        </ext:Panel>
                    </ext:LayoutColumn>
                    <ext:LayoutColumn ColumnWidth=".5">
                        <ext:Panel ID="Panel3" runat="server" Border="false" Header="false" BodyStyle="background-color:Transparent;margin:10px;">
                            <Body>
                                <ext:FormLayout ID="FormLayout1" runat="server" LabelAlign="Left">
                                    <ext:Anchor>
                                        <ext:TextField ID="text_ANESTHESIA_ASSISTANT" runat="server" DataIndex="ANESTHESIA_ASSISTANT"
                                            FieldLabel="麻醉助手" Width="150" />
                                    </ext:Anchor>
                                    <ext:Anchor>
                                        <ext:TextField ID="text_FIRST_OPERATION_NURSE" runat="server" DataIndex="FIRST_OPERATION_NURSE"
                                            MsgTarget="Side" AllowBlank="false" FieldLabel="第一台上护士" Width="150" />
                                    </ext:Anchor>
                                    <ext:Anchor>
                                        <ext:TextField ID="text_FIRST_OPERATION_DATE" runat="server" DataIndex="FIRST_OPERATION_DATE"
                                            MsgTarget="Side" AllowBlank="false" FieldLabel="第一台上护士时间" Width="150" />
                                    </ext:Anchor>
                                    <ext:Anchor>
                                        <ext:TextField ID="text_SECOND_OPERATION_NURSE" runat="server" DataIndex="SECOND_OPERATION_NURSE"
                                            FieldLabel="第二台上护士" Width="150" />
                                    </ext:Anchor>
                                    <ext:Anchor>
                                        <ext:TextField ID="text_SECOND_OPERATION_DATE" runat="server" DataIndex="SECOND_OPERATION_DATE"
                                            FieldLabel="第二台上护士时间" Width="150" />
                                    </ext:Anchor>
                                    <ext:Anchor>
                                        <ext:TextField ID="text_FIRST_SUPPLY_NURSE" runat="server" DataIndex="FIRST_SUPPLY_NURSE"
                                            FieldLabel="第一供应护士" Width="150" />
                                    </ext:Anchor>
                                    <ext:Anchor>
                                        <ext:TextField ID="text_FIRST_SUPPLY_DATE" runat="server" DataIndex="FIRST_SUPPLY_DATE"
                                            FieldLabel="第一供应护士时间" Width="150" />
                                    </ext:Anchor>
                                    <ext:Anchor>
                                        <ext:TextField ID="text_SECOND_SUPPLY_NURSE" runat="server" DataIndex="SECOND_SUPPLY_NURSE"
                                            FieldLabel="第二供应护士" Width="150" />
                                    </ext:Anchor>
                                    <ext:Anchor>
                                        <ext:TextField ID="text_SECOND_SUPPLY_DATE" runat="server" DataIndex="SECOND_SUPPLY_DATE"
                                            FieldLabel="第二供应护士时间" Width="150" />
                                    </ext:Anchor>
                                </ext:FormLayout>
                            </Body>
                        </ext:Panel>
                    </ext:LayoutColumn>
                </ext:ColumnLayout>
            </Body>
        </ext:FormPanel>
    </div>
    </form>
</body>
</html>
