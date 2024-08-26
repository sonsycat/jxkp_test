<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Out_Weeks_set.aspx.cs" Inherits="GoldNet.JXKP.Bonus.Input.Out_Weeks_set" %>
<%@ Register Assembly="Goldnet.Ext.Web" Namespace="Goldnet.Ext.Web" TagPrefix="ext" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
   <title></title>
    <link rel="stylesheet" type="text/css" href="../Orthers/Cbouns.css" />

    <script type="text/javascript">
          var RefreshData = function() {
              Store1.reload();
          }
          
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

         }           
    </script>

    <script type="text/javascript" src="../Orthers/Cbouns.js"></script>

</head>
<body>
    <ext:ScriptManager ID="ScriptManager1" runat="server" />
    <form id="form1" runat="server">
    <ext:Store ID="Store1" AutoLoad="true" runat="server" OnRefreshData="Store_RefreshData">
        <Reader>
            <ext:JsonReader>
                <Fields>
                    <ext:RecordField Name="ID">
                    </ext:RecordField>
                    <ext:RecordField Name="WEEKS">
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
                            <ext:GridPanel ID="GridPanel2" runat="server" Border="false" StoreID="Store1" StripeRows="true"
                                TrackMouseOver="true" Height="480">
                                <TopBar>
                                    <ext:Toolbar ID="Toolbar_detptype" runat="server" Visible="true" AutoWidth="true">
                                        <Items>
                                            
                                            <ext:Button ID="Btn_Edit" Text="编辑" Icon="NoteEdit" runat="server">
                                                <AjaxEvents>
                                                    <Click OnEvent="Btn_Edit_Click">
                                                        <ExtraParams>
                                                            <ext:Parameter Name="Values" Value="Ext.encode(#{GridPanel2}.getRowsValues())" Mode="Raw" />
                                                        </ExtraParams>
                                                    </Click>
                                                </AjaxEvents>
                                            </ext:Button>
                                           
                                            
                                        </Items>
                                    </ext:Toolbar>
                                </TopBar>
                                <ColumnModel ID="ColumnModel2" runat="server">
                                    <Columns>
                                        
                                        <ext:Column ColumnID="ID" Header="编号" Width="100" DataIndex="ID" MenuDisabled="true"
                                            Align="Center">
                                        </ext:Column>
                                        <ext:Column ColumnID="WEEKS" Header="<div style='text-align:center;'>分类</div>"
                                            Width="200" DataIndex="WEEKS" MenuDisabled="true" Align="Left">
                                        </ext:Column>
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
        <ext:Window ID="DetailWin" runat="server" Icon="Group" Title="奖金录入" Width="850" Height="500"
            AutoShow="false" Modal="true" CenterOnLoad="true" AutoScroll="true" ShowOnLoad="false"
            Resizable="false" StyleSpec="background-color:Transparent;" BodyStyle="background-color:Transparent;">
        </ext:Window>
    </div>
    </form>
</body>
</html>