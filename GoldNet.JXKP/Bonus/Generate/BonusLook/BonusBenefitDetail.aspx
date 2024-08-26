<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="BonusBenefitDetail.aspx.cs"
    Inherits="GoldNet.JXKP.BonusBenefitDetail" %>

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
    <ext:Store ID="SIncomes" AutoLoad="true" runat="server">
        <Reader>
            <ext:JsonReader>
                <Fields>
                    <ext:RecordField Name="DEPT_NAME">
                    </ext:RecordField>
                    <ext:RecordField Name="RECK_ITEM">
                    </ext:RecordField>
                    <ext:RecordField Name="CLASS_NAME">
                    </ext:RecordField>
                    <ext:RecordField Name="INCOMES">
                    </ext:RecordField>
                    <ext:RecordField Name="INCOMES_CHARGES">
                    </ext:RecordField>
                    <ext:RecordField Name="TOTAL_INCOMES">
                    </ext:RecordField>
                </Fields>
            </ext:JsonReader>
        </Reader>
    </ext:Store>
    <ext:Store ID="SCost" AutoLoad="true" runat="server">
        <Reader>
            <ext:JsonReader>
                <Fields>
                    <ext:RecordField Name="DEPT_NAME">
                    </ext:RecordField>
                    <ext:RecordField Name="COST_ITEM">
                    </ext:RecordField>
                    <ext:RecordField Name="ITEM_NAME">
                    </ext:RecordField>
                    <ext:RecordField Name="COSTS">
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
                        <ext:TabPanel ID="TabPanel1" runat="server" Border="false">
                            <Tabs>
                                <ext:Tab ID="Tab1" runat="server" Title="科室收入信息" AutoScroll="false">
                                    <Body>
                                        <ext:FitLayout ID="FitLayout1" runat="server">
                                            <Items>
                                                <ext:GridPanel runat="server" ID="gpIncome" StoreID="SIncomes" Header="false" AutoScroll="true" Border="false"
                                                    Width="630">
                                                    <ColumnModel>
                                                        <Columns>
                                                            <ext:Column ColumnID="DEPT_NAME" Header="科室" Width="100" DataIndex="DEPT_NAME" MenuDisabled="true">
                                                            </ext:Column>
                                                            <ext:Column ColumnID="RECK_ITEM" Header="类别代码" Width="100" DataIndex="RECK_ITEM"
                                                                MenuDisabled="true">
                                                            </ext:Column>
                                                            <ext:Column ColumnID="CLASS_NAME" Header="类别名称" Width="100" DataIndex="CLASS_NAME"
                                                                MenuDisabled="true">
                                                            </ext:Column>
                                                            <ext:Column ColumnID="INCOMES" Header="实际收入" Width="100" DataIndex="INCOMES" MenuDisabled="true">
                                                            </ext:Column>
                                                            <ext:Column ColumnID="INCOMES_CHARGES" Header="计价收入" Width="100" DataIndex="INCOMES_CHARGES"
                                                                MenuDisabled="true">
                                                            </ext:Column>
                                                            <ext:Column ColumnID="TOTAL_INCOMES" Header="总收入" Width="100" DataIndex="TOTAL_INCOMES"
                                                                MenuDisabled="true">
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
                                </ext:Tab>
                                <ext:Tab ID="Tab2" runat="server" Title="科室成本信息">
                                    <Body>
                                        <ext:FitLayout ID="FitLayout2" runat="server">
                                            <Items>
                                                <ext:GridPanel runat="server" ID="gpzlgl" StoreID="SCost" Height="240" AutoWidth="true"
                                                    Header="false" Width="630" Border="false">
                                                    <ColumnModel>
                                                        <Columns>
                                                            <ext:Column ColumnID="DEPT_NAME" Header="科室" Width="100" DataIndex="DEPT_NAME" MenuDisabled="true">
                                                            </ext:Column>
                                                            <ext:Column ColumnID="COST_ITEM" Header="类别代码" Width="100" DataIndex="COST_ITEM"
                                                                MenuDisabled="true">
                                                            </ext:Column>
                                                            <ext:Column ColumnID="ITEM_NAME" Header="类别名称" Width="100" DataIndex="ITEM_NAME"
                                                                MenuDisabled="true">
                                                            </ext:Column>
                                                            <ext:Column ColumnID="COSTS" Header="成本" Width="100" DataIndex="COSTS" MenuDisabled="true">
                                                            </ext:Column>
                                                        </Columns>
                                                    </ColumnModel>
                                                    <SelectionModel>
                                                        <ext:RowSelectionModel ID="RowSelectionModel2" SingleSelect="true">
                                                        </ext:RowSelectionModel>
                                                    </SelectionModel>
                                                </ext:GridPanel>
                                            </Items>
                                        </ext:FitLayout>
                                    </Body>
                                </ext:Tab>
                            </Tabs>
                        </ext:TabPanel>
                    </ext:LayoutColumn>
                </Columns>
            </ext:ColumnLayout>
        </Body>
    </ext:ViewPort>
    </form>
</body>
</html>
