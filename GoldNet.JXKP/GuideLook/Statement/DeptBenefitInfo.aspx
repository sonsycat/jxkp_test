<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DeptBenefitInfo.aspx.cs"
    Inherits="GoldNet.JXKP.GuideLook.Statement.DeptBenefitInfo" %>

<%@ Register Assembly="Goldnet.Ext.Web" Namespace="Goldnet.Ext.Web" TagPrefix="ext" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>无标题页</title>

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
                  if(v=='0') {
                    v = '0.00'
                  }
                  return v;
           }
    </script>

</head>
<body>
    <form id="form1" runat="server">
    <ext:ScriptManager ID="ScriptManager1" runat="server" />
    <ext:Store runat="server" ID="Store1">
        <Reader>
            <ext:JsonReader>
                <Fields>
                    <ext:RecordField Name="INCOM_TYPE_CODE" />
                    <ext:RecordField Name="INCOM_TYPE_NAME" />
                    <ext:RecordField Name="FY" />
                    <ext:RecordField Name="FROMDATE" />
                    <ext:RecordField Name="TODATE" />
                    <ext:RecordField Name="STAFF_ID" />
                    <ext:RecordField Name="DEPT_CODE" />
                    <ext:RecordField Name="TYPE" />
                </Fields>
            </ext:JsonReader>
        </Reader>
    </ext:Store>
    <div>
        <ext:ViewPort ID="ViewPort1" runat="server">
            <Body>
                <ext:BorderLayout ID="BorderLayout2" runat="server">
                    <Center>
                        <ext:GridPanel ID="GridPanel_Show" runat="server" Border="false" StoreID="Store1"
                            StripeRows="true" Height="480" Width="600" AutoScroll="true">
                            <ColumnModel ID="ColumnModel1" runat="server">
                                <Columns>
                                    <ext:ExtColumn ColumnID="INCOM_TYPE_CODE" Header="项目编码" Sortable="true" DataIndex="INCOM_TYPE_CODE" />
                                    <ext:ExtColumn ColumnID="INCOM_TYPE_NAME" Header="项目类别" Sortable="true" DataIndex="INCOM_TYPE_NAME" />
                                    <ext:ExtColumn ColumnID="FY" Header="费用" Sortable="true" DataIndex="FY">
                                        <Renderer Fn="rmbMoney" />
                                    </ext:ExtColumn>
                                    <ext:ExtColumn ColumnID="FROMDATE" Header="" Sortable="true" Hidden="true" DataIndex="FROMDATE" />
                                    <ext:ExtColumn ColumnID="TODATE" Header="" Sortable="true" Hidden="true" DataIndex="TODATE" />
                                    <ext:ExtColumn ColumnID="STAFF_ID" Header="" Sortable="true" Hidden="true" DataIndex="STAFF_ID" />
                                    <ext:ExtColumn ColumnID="DEPT_CODE" Header="" Sortable="true" Hidden="true" DataIndex="DEPT_CODE" />
                                    <ext:ExtColumn ColumnID="TYPE" Header="" Sortable="true" Hidden="true" DataIndex="TYPE" />
                                </Columns>
                            </ColumnModel>
                            <SelectionModel>
                                <ext:RowSelectionModel SingleSelect="true">
                                </ext:RowSelectionModel>
                            </SelectionModel>
                            <LoadMask ShowMask="true" />
                            <AjaxEvents>
                                <RowDblClick OnEvent="GetQueryPer">
                                    <EventMask Msg="载入中..." ShowMask="true" />
                                    <ExtraParams>
                                        <ext:Parameter Name="Values" Value="Ext.encode(#{GridPanel_Show}.getRowsValues())"
                                            Mode="Raw">
                                        </ext:Parameter>
                                    </ExtraParams>
                                </RowDblClick>
                            </AjaxEvents>
                        </ext:GridPanel>
                    </Center>
                </ext:BorderLayout>
            </Body>
        </ext:ViewPort>
    </div>
    </form>
</body>
</html>
