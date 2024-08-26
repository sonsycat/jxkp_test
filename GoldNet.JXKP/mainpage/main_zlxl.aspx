<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="main_zlxl.aspx.cs" Inherits="GoldNet.JXKP.mainpage.main_zlxl" %>

<%@ Register Assembly="Goldnet.Ext.Web" Namespace="Goldnet.Ext.Web" TagPrefix="ext" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>

    <script type="text/javascript">

         var rmbMoney = function(v) {
                  if(v != '0') {
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
                  }
                  return v;
           }
    </script>

</head>
<body>
    <form id="form1" runat="server">
    <ext:ScriptManager ID="ScriptManager1" runat="server">
    </ext:ScriptManager>
    <ext:Store runat="server" ID="Store1" >
        <Reader>
            <ext:JsonReader>
                <Fields>
                    <ext:RecordField Name="ZBL" />
                    <ext:RecordField Name="ZBDM" />
                    <ext:RecordField Name="ZBMC" />
                    <ext:RecordField Name="MBZ" />
                    <ext:RecordField Name="WCZ" />
                    <ext:RecordField Name="WCBFB" />
                    <ext:RecordField Name="ORGAN" />
                    <ext:RecordField Name="NEXTGUIDE" />
                    <ext:RecordField Name="PERSONID" />
                    <ext:RecordField Name="TQWCZ" />
                    <ext:RecordField Name="ZZL" />
                </Fields>
            </ext:JsonReader>
        </Reader>
    </ext:Store>
    <div>
        <ext:Panel ID="Panel1" runat="server" Border="false" MonitorResize="true">
            <Body>
                <ext:FitLayout ID="FitLayout1" runat="server">
                    <ext:GridPanel ID="GridPanel_Show" runat="server" StoreID="Store1" Border="false"
                        AutoWidth="true" AutoHeight="true" Title="" MonitorResize="true" MonitorWindowResize="true"
                        StripeRows="true" TrackMouseOver="true" Collapsible="false">
                        <ColumnModel ID="ColumnModel1" runat="server">
                            <Columns>
                                <ext:Column Header="指标名称" Width="120" Align="left" MenuDisabled="true" Sortable="false"
                                    ColumnID="zbmc" DataIndex="ZBMC" />
                                <ext:Column Header="<div style='text-align:center;'>本期值</div>" Width="96" Align="Center" MenuDisabled="true" Sortable="false"
                                    ColumnID="wcz" DataIndex="WCZ">

                                </ext:Column>
                                <ext:Column Header="<div style='text-align:center;'>同期值</div>" Width="96" Align="Center" MenuDisabled="true" Sortable="false"
                                    ColumnID="wcz" DataIndex="TQWCZ">

                                </ext:Column>
                                <ext:Column Header="<div style='text-align:center;'>上期值</div>" Width="70" Align="Center" MenuDisabled="true" Sortable="false"
                                    ColumnID="zzl" DataIndex="ZZL">

                                </ext:Column>
                            </Columns>
                        </ColumnModel>
                        <SelectionModel>
                            <ext:RowSelectionModel ID="rowselection" SingleSelect="true" />
                        </SelectionModel>
                        <LoadMask ShowMask="true" />
                    </ext:GridPanel>
                </ext:FitLayout>
            </Body>
        </ext:Panel>
    </div>
    </form>
</body>
</html>