<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="main_zrylgl.aspx.cs" Inherits="GoldNet.JXKP.mainpage.main_zrylgl" %>

<%@ Register Assembly="Goldnet.Ext.Web" Namespace="Goldnet.Ext.Web" TagPrefix="ext" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>

    <script type="text/javascript">  
         var rmbMoney = function(v) {
//                  if(v != '0') {
//                       v = (Math.round((v - 0) * 100)) / 100;
//                       v = (v == Math.floor(v)) ? v  : ((v * 10 == Math.floor(v * 10)) ? v + "0" : v);
//                       v = String(v);
//                       var ps = v.split('.');
//                       var whole = ps[0];
//                       var sub = ps[1] ? '.' + ps[1] : '.00';
//                       var r = /(\d+)(\d{3})/;
//                       while (r.test(whole)) {
//                           whole = whole.replace(r, '$1' + ',' + '$2');
//                           //alert("xx"+whole);
//                       }
//                       v = whole + sub;
//                       v=String(Math.round(v));
//                       if (v.charAt(0) == '-') {
//                           return '-' + v.substr(1);
//                       }
//                  }
                  v=Math.floor(v);
                  if(isNaN(v))
                  {
                    v="0";
                  }
                  else
                  {
                    v=String(Math.round(v));
                  }

                  return v;
           }
    </script>

</head>
<body>
    <form id="form1" runat="server">
    <ext:ScriptManager ID="ScriptManager1" runat="server">
    </ext:ScriptManager>
    <ext:Store ID="Store1" runat="server">
        <Reader>
            <ext:JsonReader>
                <Fields>
                    <ext:RecordField Name="ZBMC" />
                    <ext:RecordField Name="MONTHS" />
                    <ext:RecordField Name="SZ" />
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
                            <ext:GridPanel ID="GridPanel_Show" runat="server" StoreID="Store1" Border="false"
                                AutoWidth="true" AutoScroll="true" Title="" MonitorResize="true" MonitorWindowResize="true"
                                StripeRows="true" TrackMouseOver="true" AutoExpandColumn="ZBMC">
                                <TopBar>
                                    <ext:Toolbar ID="Toolbar1" runat="server">
                                        <Items>
                                            <ext:ToolbarSpacer ID="ToolbarSpacer11" runat="server" Width="15" />
                                            <ext:Label ID="Label1" runat="server" Text="请选择概览天数：" />
                                            <ext:ComboBox runat="server" ID="Combo_daydate" Width="40" AllowBlank="false" />
                                            <ext:ToolbarSpacer ID="ToolbarSpacer1" runat="server" Width="15" />
                                            <ext:ToolbarSeparator ID="ToolbarSeparator1" runat="server" />
                                            <ext:Button ID="Button1" runat="server" Text="查询" Icon="DatabaseGo">
                                                <AjaxEvents>
                                                    <Click OnEvent="GetQueryPortalet">
                                                        <EventMask Msg="载入中..." ShowMask="true" />
                                                    </Click>
                                                </AjaxEvents>
                                            </ext:Button>
                                            <ext:ToolbarSeparator ID="ToolbarSeparator2" runat="server" />
                                        </Items>
                                    </ext:Toolbar>
                                </TopBar>
                                <ColumnModel ID="ColumnModel1" runat="server">
                                    <Columns>
                                        <ext:RowNumbererColumn>
                                        </ext:RowNumbererColumn>
                                        <ext:Column Header="指标名称" Width="66" Align="Left" Sortable="false" MenuDisabled="true"
                                            ColumnID="ZBMC" DataIndex="ZBMC" />
                                        <ext:Column Header="昨日数值" Width="90" Align="Center" Sortable="false" MenuDisabled="true"
                                            ColumnID="months" DataIndex="MONTHS">
                                            <Renderer Fn="rmbMoney" />
                                        </ext:Column>
                                        <ext:Column Header="本月数值" Width="90" Align="Center" Sortable="false" MenuDisabled="true"
                                            ColumnID="sz" DataIndex="SZ">
                                            <Renderer Fn="rmbMoney" />
                                        </ext:Column>
                                    </Columns>
                                </ColumnModel>
                                <SelectionModel>
                                    <ext:RowSelectionModel ID="rowselection" SingleSelect="true" />
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
