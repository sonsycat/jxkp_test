<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="BonusSimpleAwardDetail.aspx.cs"
    Inherits="GoldNet.JXKP.BonusSimpleAwardDetail" %>

<%@ Register Assembly="Goldnet.Ext.Web" Namespace="Goldnet.Ext.Web" TagPrefix="ext" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <style type="text/css">
        .pageSize
        {
            font-size: 13px;
        }
        .captionStyle
        {
            margin-left: 25px;
            font-size: 18px;
            color: Blue;
        }
    </style>
     <link rel="stylesheet" type="text/css" href="../Orthers/Cbouns.css" />
</head>
<body>
    <ext:ScriptManager ID="ScriptManager1" runat="server" />
    <ext:Store ID="SSimpleAward" AutoLoad="true" runat="server">
        <Reader>
            <ext:JsonReader>
                <Fields>
                    <ext:RecordField Name="DEPT_NAME">
                    </ext:RecordField>
                    <ext:RecordField Name="TYPE_NAME">
                    </ext:RecordField>
                    <ext:RecordField Name="CHECKSTAN">
                    </ext:RecordField>
                    <ext:RecordField Name="REMARK">
                    </ext:RecordField>
                    <ext:RecordField Name="MONEY">
                    </ext:RecordField>
                    <ext:RecordField Name="AWARD_DATE">
                    </ext:RecordField>
                    <ext:RecordField Name="INPUTER">
                    </ext:RecordField>
                </Fields>
            </ext:JsonReader>
        </Reader>
    </ext:Store>
    <form id="form1" runat="server">
    <ext:ViewPort ID="ViewPort1" runat="server">
        <Body>
            <ext:ColumnLayout ID="ColumnLayout1" runat="server" Split="true" FitHeight="true">
                <Columns>
                    <ext:LayoutColumn ColumnWidth="1">
                        <ext:Panel runat="server" ID="panel1" AutoScroll="true" Border="false">
                            <Body>
                                <ext:FitLayout ID="FitLayout2" runat="server">
                                    <Items>
                                        <ext:GridPanel runat="server" ID="gpSimpleAward" StoreID="SSimpleAward" Border="false" >
                                            <ColumnModel>
                                                <Columns>
                                                    <ext:Column ColumnID="TYPE_NAME" Header="奖惩项目" Width="100" DataIndex="TYPE_NAME"
                                                        MenuDisabled="true">
                                                    </ext:Column>
                                                    <ext:Column ColumnID="MONEY" Header="奖惩金额" Width="89" DataIndex="MONEY" MenuDisabled="true">
                                                    </ext:Column>
                                                    <ext:Column ColumnID="AWARD_DATE" Header="奖惩时间" Width="76" DataIndex="AWARD_DATE"
                                                        MenuDisabled="true">
                                                    </ext:Column>
                                                    <ext:Column ColumnID="CHECKSTAN" Header="奖惩标准" Width="150" DataIndex="CHECKSTAN"
                                                        MenuDisabled="true">
                                                    </ext:Column>
                                                    <ext:Column ColumnID="REMARK" Header="备注" Width="100" DataIndex="REMARK" MenuDisabled="true">
                                                    </ext:Column>
                                                    <ext:Column ColumnID="INPUTER" Header="录入人" Width="78" DataIndex="INPUTER" MenuDisabled="true">
                                                    </ext:Column>
                                                </Columns>
                                            </ColumnModel>
                                            <SelectionModel>
                                                <ext:RowSelectionModel ID="RowSelectionModel1" SingleSelect="true">
                                                </ext:RowSelectionModel>
                                            </SelectionModel>
                                        </ext:GridPanel>
                                    </Items>
                                </ext:FitLayout>
                            </Body>
                        </ext:Panel>
                    </ext:LayoutColumn>
                </Columns>
            </ext:ColumnLayout>
        </Body>
    </ext:ViewPort>
    </form>
</body>
</html>
