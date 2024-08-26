<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="BonusSearch.aspx.cs" Inherits="GoldNet.JXKP.BonusSearch" %>

<%@ Register Assembly="Goldnet.Ext.Web" Namespace="Goldnet.Ext.Web" TagPrefix="ext" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <link rel="stylesheet" type="text/css" href="../Orthers/Cbouns.css" />

    <script type="text/javascript">
          var applyFilter = function() {
            SSearch.filterBy(getRecordFilter());
          };
          var getRecordFilter = function() {
              var f = [];
              f.push({
                  filter: function(record) {
                  return filterString(txt_SearchTxt.getValue(), 'UNIT_NAME', record);
                  }
              });
              f.push({
                  filter: function(record) {
                  return filterString(txt_SearchTxt.getValue(), 'USER_NAME', record);
                  }
              });
              f.push({
                  filter: function(record) {
                  return filterNumber(txt_SearchTxt.getValue(), 'USER_TOTAL_BONUS', record);
                  }
              });
              f.push({
                  filter: function(record) {
                  return filterString(txt_SearchTxt.getValue(), 'BANK_CODE', record);
                  }
              });

              var len = f.length;
              return function(record) {
                  if (f[0].filter(record) || f[1].filter(record) || f[2].filter(record) || f[3].filter(record) ) {
                      return true;
                  }
                  return false;
              }
          };      
    </script>

    <script type="text/javascript" src="../Orthers/Cbouns.js"></script>

</head>
<body>
    <ext:ScriptManager ID="ScriptManager1" runat="server" />
    <form id="form1" runat="server">
    <ext:Store ID="SSearch" AutoLoad="true" runat="server">
        <Reader>
            <ext:JsonReader>
                <Fields>
                    <ext:RecordField Name="UNIT_NAME">
                    </ext:RecordField>
                    <ext:RecordField Name="USER_NAME">
                    </ext:RecordField>
                    <ext:RecordField Name="USER_TOTAL_BONUS">
                    </ext:RecordField>
                    <ext:RecordField Name="STAFF_ID">
                    </ext:RecordField>
                    <ext:RecordField Name="STAFFSORT">
                    </ext:RecordField>
                     <ext:RecordField Name="BANK_CODE">
                    </ext:RecordField>
                </Fields>
            </ext:JsonReader>
        </Reader>
    </ext:Store>
    <ext:Store ID="SYear" runat="server">
        <Reader>
            <ext:JsonReader ReaderID="YEAR">
                <Fields>
                    <ext:RecordField Name="YEAR" />
                </Fields>
            </ext:JsonReader>
        </Reader>
    </ext:Store>
    <ext:Store ID="SMonths" runat="server">
        <Reader>
            <ext:JsonReader ReaderID="MONTH">
                <Fields>
                    <ext:RecordField Name="MONTH" />
                </Fields>
            </ext:JsonReader>
        </Reader>
    </ext:Store>
    <ext:Store ID="StoreRelation" runat="server">
        <Reader>
            <ext:JsonReader>
                <Fields>
                    <ext:RecordField Name="Key">
                    </ext:RecordField>
                    <ext:RecordField Name="Value">
                    </ext:RecordField>
                </Fields>
            </ext:JsonReader>
        </Reader>
    </ext:Store>
    <div>
        <ext:ViewPort ID="ViewPort1" runat="server">
            <Body>
                <ext:ColumnLayout ID="ColumnLayout1" runat="server" Split="true" FitHeight="true">
                    <Columns>
                        <ext:LayoutColumn ColumnWidth="1">
                            <ext:GridPanel ID="GridPanel2" runat="server" Border="false" StoreID="SSearch" StripeRows="true"
                                TrackMouseOver="true" Height="480">
                                <TopBar>
                                    <ext:Toolbar ID="Toolbar_detptype" runat="server" Visible="true" AutoWidth="true">
                                        <Items>
                                            <ext:ToolbarSpacer ID="ToolbarSpacer5" runat="server" Width="10" />
                                            <ext:ComboBox ID="cbbYear" runat="server" ReadOnly="true" StoreID="SYear" Width="80"
                                                DisplayField="YEAR" ValueField="YEAR">
                                            </ext:ComboBox>
                                            <ext:Label ID="lYear" runat="server" Text="年">
                                            </ext:Label>
                                            <ext:ComboBox ID="cbbmonth" runat="server" ReadOnly="true" StoreID="SMonths" Width="60"
                                                DisplayField="MONTH" ValueField="MONTH">
                                            </ext:ComboBox>
                                            <ext:Label ID="lMonth" runat="server" Text="月">
                                            </ext:Label>
                                            <ext:ToolbarSeparator ID="ToolbarSeparator4" runat="server" />
                                            <ext:Button runat="server" ID="BtnSearcd" Text="查询" Icon="FolderMagnify">
                                                <AjaxEvents>
                                                    <Click OnEvent="Btn_Search_Click">
                                                    </Click>
                                                </AjaxEvents>
                                            </ext:Button>
                                             <ext:Button ID="btn_Excel" runat="server" OnClick="OutExcel" AutoPostBack="true"
                                                Text="导出Excel" Icon="PageWhiteExcel" CausesValidation="false">
                                            </ext:Button>
                                        </Items>
                                    </ext:Toolbar>
                                </TopBar>
                                <ColumnModel ID="ColumnModel2" runat="server">
                                    <Columns>
                                        <ext:Column ColumnID="UNITNAME" Header="科室" Width="100" DataIndex="UNIT_NAME" MenuDisabled="true"
                                            Align="Center">
                                        </ext:Column>
                                        <ext:Column ColumnID="USERNAME" Header="人员" Width="100" DataIndex="USER_NAME" MenuDisabled="true"
                                            Align="Center">
                                        </ext:Column>
                                        <ext:Column ColumnID="USERBONUS" Header="<div style='text-align:center;'>奖金</div>"
                                            Width="100" DataIndex="USER_TOTAL_BONUS" MenuDisabled="true" Align="Right">
                                        </ext:Column>
                                        <ext:Column ColumnID="STAFF_ID" Header="标识号" Width="200" DataIndex="STAFF_ID" MenuDisabled="true"
                                            Align="Center">
                                        </ext:Column>
                                        <ext:Column ColumnID="STAFFSORT" Header="人员类别" Width="200" DataIndex="STAFFSORT" MenuDisabled="true"
                                            Align="Center">
                                        </ext:Column>
                                          <ext:Column ColumnID="BANK_CODE" Header="银行卡号" Width="200" DataIndex="BANK_CODE" MenuDisabled="true"
                                            Align="Center">
                                        </ext:Column>
                                    </Columns>
                                </ColumnModel>
                                <SelectionModel>
                                    <ext:RowSelectionModel ID="rowselection" SingleSelect="true">
                                    </ext:RowSelectionModel>
                                </SelectionModel>
                                <LoadMask ShowMask="true" />
                                <BottomBar>
                                    <ext:Toolbar ID="Toolbar1" runat="server">
                                        <Items>
                                            <ext:Label runat="server" ID="lCondition" Text="条件查询">
                                            </ext:Label>
                                            <ext:TextField ID="txt_SearchTxt" runat="server" EmptyText="查找">
                                            </ext:TextField>
                                            <ext:Button ID="Button1" Icon="FolderMagnify" runat="server" Text="查询">
                                                <Listeners>
                                                    <Click Fn="applyFilter" />
                                                </Listeners>
                                            </ext:Button>
                                        </Items>
                                    </ext:Toolbar>
                                </BottomBar>
                            </ext:GridPanel>
                        </ext:LayoutColumn>
                    </Columns>
                </ext:ColumnLayout>
            </Body>
        </ext:ViewPort>
    </div>
    </form>
</body>
</html>
