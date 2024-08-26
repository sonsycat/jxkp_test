<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PerDeptIncome.aspx.cs" Inherits="GoldNet.JXKP.GuideLook.Statement.PerDeptIncome" %>

<%@ Register Assembly="Goldnet.Ext.Web" Namespace="Goldnet.Ext.Web" TagPrefix="ext" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
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
                </Fields>
            </ext:JsonReader>
        </Reader>
    </ext:Store>
    <div>
        <ext:ViewPort ID="ViewPort1" runat="server">
            <Body>
                <ext:BorderLayout ID="BorderLayout2" runat="server">
                    <North>
                        <ext:Toolbar runat="server" ID="ctl155" StyleSpec="border:0">
                            <Items>
                                <ext:Button ID="btnExcel" runat="server" Text=" 导出Excel " Icon="PageWhiteExcel" OnClick="OutExcel"
                                    AutoPostBack="true">
                                </ext:Button>
                            </Items>
                        </ext:Toolbar>
                    </North>
                    <Center>
                        <ext:GridPanel ID="GridPanel_Show" runat="server" Border="false" StoreID="Store1"
                            StripeRows="true" Height="480" Width="600" AutoScroll="true">
                            <ColumnModel ID="ColumnModel1" runat="server">
                                <Columns>
                                    <ext:RowNumbererColumn Width="32" Resizable="true">
                                    </ext:RowNumbererColumn>
                                </Columns>
                            </ColumnModel>
                            <SelectionModel>
                                <ext:RowSelectionModel SingleSelect="true">
                                </ext:RowSelectionModel>
                            </SelectionModel>
                            <LoadMask ShowMask="true" />
                        </ext:GridPanel>
                    </Center>
                </ext:BorderLayout>
            </Body>
        </ext:ViewPort>
    </div>
    </form>
</body>
</html>
