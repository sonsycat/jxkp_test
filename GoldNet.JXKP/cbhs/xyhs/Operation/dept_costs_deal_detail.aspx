<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="dept_costs_deal_detail.aspx.cs" Inherits="GoldNet.JXKP.cbhs.xyhs.Operation.dept_costs_deal_detail" %>
<%@ Register Assembly="Goldnet.Ext.Web" Namespace="Goldnet.Ext.Web" TagPrefix="ext" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
     <title></title>

    <script language="javascript" type="text/javascript">
        var rmbMoney = function(v) {
               if(v==null||v=="")
               {
               return "";
               }
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
       }
    </script>

</head>
<body>
    <form id="form1" runat="server">
    <div>
        <ext:scriptmanager id="ScriptManager1" runat="server" ajaxmethodnamespace="CompanyX" />
        <ext:store id="Store1" runat="server">
            <Reader>
                <ext:JsonReader>
                    <Fields>
                        <ext:RecordField Name="ITEM_CODE" Type="String" Mapping="ITEM_CODE" />
                        <ext:RecordField Name="ITEM_NAME" Type="String" Mapping="ITEM_NAME" />
                        <ext:RecordField Name="COSTS" Type="String" Mapping="COSTS" />
                        <ext:RecordField Name="COST_TYPE" Type="String" Mapping="COST_TYPE" />
                    </Fields>
                </ext:JsonReader>
            </Reader>
        </ext:store>
        <ext:viewport id="ViewPort1" runat="server">
            <Body>
                <ext:ColumnLayout ID="ColumnLayout1" runat="server" Split="true" FitHeight="true">
                    <Columns>
                        <ext:LayoutColumn ColumnWidth="1">
                            <ext:GridPanel ID="GridPanel1" runat="server" StoreID="Store1" StripeRows="true"
                                ClicksToEdit="1" TrackMouseOver="true" AutoWidth="true" Height="480" Border="false">
                                <TopBar>
                                    <ext:Toolbar ID="Toolbar1" runat="server">
                                        <Items>
                                             <ext:ToolbarTextItem ID="dd1Name" runat="server"  />
                                        </Items>
                                    </ext:Toolbar>
                                </TopBar>
                                <ColumnModel ID="ColumnModel1" runat="server">
                                    <Columns>
                                        <ext:Column ColumnID="ITEM_CODE" Header="<div style='text-align:center;'>成本编码</div>" Width="90" Align="left" Sortable="true"
                                            DataIndex="ITEM_CODE" MenuDisabled="true" />
                                        <ext:Column ColumnID="ITEM_NAME" Header="<div style='text-align:center;'>成本名称</div>" Width="90" Align="left" Sortable="true"
                                            DataIndex="ITEM_NAME" MenuDisabled="true" />
                                        <ext:Column ColumnID="COSTS" Header="<div style='text-align:center;'>成本额</div>" Width="90" Align="left" Sortable="true"
                                            DataIndex="COSTS" MenuDisabled="true" >
                                            <Renderer Fn="rmbMoney" />
                                        </ext:Column>
                                        <ext:Column ColumnID="COST_TYPE" Header="<div style='text-align:center;'>成本属性</div>" Width="90" Align="left" Sortable="true"
                                            DataIndex="COST_TYPE" MenuDisabled="true" />
                                    </Columns>
                                </ColumnModel>
                                <SelectionModel>
                                    <ext:RowSelectionModel ID="RowSelectionModel1" runat="server" SingleSelect="true">
                                    </ext:RowSelectionModel>
                                </SelectionModel>
                            </ext:GridPanel>
                        </ext:LayoutColumn>
                    </Columns>
                </ext:ColumnLayout>
            </Body>
        </ext:viewport>
    </div>
    </form>
</body>
</html>
