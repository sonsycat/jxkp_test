<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DistributeBonus.aspx.cs"
    Inherits="GoldNet.JXKP.DistributeBonus" %>

<%@ Register Assembly="Goldnet.Ext.Web" Namespace="Goldnet.Ext.Web" TagPrefix="ext" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <link rel="stylesheet" type="text/css" href="../Orthers/Cbouns.css" />
      <script type="text/javascript">
          var rmbMoney = function(v) {
              v = (Math.round((v - 0) * 100)) / 100;
              v = (v == Math.floor(v)) ? v + ".00" : ((v * 10 == Math.floor(v * 10)) ? v + "0" : v);
              v = String(v);
              var ps = v.split('.');
              var whole = ps[0];
              var sub = ps[1] ? '.' + ps[1] : '.00';
              var r = /(\d+)(\d{3})/;
              while (r.test(whole)) {
                  whole = whole.replace(r, '$1' + ',' + '$2');
              }
              v = whole + sub;
              if (v.charAt(0) == '-') {
                  return '-' + v.substr(1);
              }
              return v;

          };
    </script>
</head>
<body>
    <ext:ScriptManager ID="ScriptManager1" runat="server" />
    <form id="form1" runat="server">
    <ext:Store ID="SSearch" AutoLoad="true" runat="server">
        <Reader>
            <ext:JsonReader>
                <Fields>                    
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
    <ext:Store ID="SReport" runat="server">
        <Reader>
            <ext:JsonReader ReaderID="ID">
                <Fields>
                    <ext:RecordField Name="ID" />
                    <ext:RecordField Name="NAME" />
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
                                            <ext:Label runat="server" ID="lReport" Text="报表名称：">
                                            </ext:Label>
                                            <ext:ComboBox ID="cbbReport" runat="server" ReadOnly="true" StoreID="SReport" Width="100"
                                                DisplayField="NAME" ValueField="ID">
                                                <AjaxEvents>
                                                    <Select OnEvent="Search_Select">
                                                    </Select>
                                                </AjaxEvents>
                                            </ext:ComboBox>
                                            <ext:ToolbarSpacer ID="ToolbarSpacer5" runat="server" Width="10" />
                                            <ext:ToolbarSeparator ID="ToolbarSeparator1" runat="server">
                                            </ext:ToolbarSeparator>
                                            <ext:ToolbarSpacer ID="ToolbarSpacer3" runat="server" Width="10" />
                                            <ext:Label ID="Label1" runat="server" Text="分配年月：">
                                            </ext:Label>
                                            <ext:ComboBox ID="cbbYear" runat="server" ReadOnly="true" StoreID="SYear" Width="60"
                                                DisplayField="YEAR" ValueField="YEAR">
                                                <AjaxEvents>
                                                    <Select OnEvent="Search_Select">
                                                    </Select>
                                                </AjaxEvents>
                                            </ext:ComboBox>
                                            <ext:Label ID="lYear" runat="server" Text="年">
                                            </ext:Label>
                                            <ext:ComboBox ID="cbbmonth" runat="server" ReadOnly="true" StoreID="SMonths" Width="40"
                                                DisplayField="MONTH" ValueField="MONTH">
                                                <AjaxEvents>
                                                    <Select OnEvent="Search_Select">
                                                    </Select>
                                                </AjaxEvents>
                                            </ext:ComboBox>
                                            <ext:Label ID="lMonth" runat="server" Text="月">
                                            </ext:Label>
                                            <ext:ToolbarSpacer ID="ToolbarSpacer1" runat="server" Width="10" />
                                            <ext:ToolbarSeparator runat="server">
                                            </ext:ToolbarSeparator>
                                            <ext:ToolbarSpacer ID="ToolbarSpacer2" runat="server" Width="10" />
                                            <ext:Label ID="lTotalBouns" runat="server" Text="效率奖总数：">
                                            </ext:Label>
                                            <ext:NumberField runat="server" Text="0" ID="nfTotalBonus" Width="100">
                                            </ext:NumberField>
                                            <ext:ToolbarSeparator ID="ToolbarSeparator2" runat="server">
                                            </ext:ToolbarSeparator>
                                            <ext:Button runat="server" ID="BtnDistribute" Icon="Box" Text="分配奖金">
                                                <AjaxEvents>
                                                    <Click OnEvent="Btn_Distribute_Click">
                                                    </Click>
                                                </AjaxEvents>
                                            </ext:Button>
                                            <ext:ToolbarSeparator ID="ToolbarSeparator3" runat="server">
                                            </ext:ToolbarSeparator>
                                            <ext:Button runat="server" ID="BtnDel" Text="删除" Icon="Delete">
                                                <AjaxEvents>
                                                    <Click OnEvent="Btn_Delete_Click">
                                                    </Click>
                                                </AjaxEvents>
                                            </ext:Button>
                                            <ext:ToolbarSeparator ID="ToolbarSeparator4" runat="server">
                                            </ext:ToolbarSeparator>
                                        </Items>
                                    </ext:Toolbar>
                                </TopBar>
                                <ColumnModel ID="ColumnModel2" runat="server">
                                    <Columns>                                                              
                                    </Columns>
                                </ColumnModel>
                                <SelectionModel>
                                    <ext:RowSelectionModel ID="rowselection" SingleSelect="true">
                                    </ext:RowSelectionModel>
                                </SelectionModel>
                                <LoadMask ShowMask="true" />
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
