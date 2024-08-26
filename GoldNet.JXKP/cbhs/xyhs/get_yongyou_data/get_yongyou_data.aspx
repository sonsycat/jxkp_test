<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="get_yongyou_data.aspx.cs" Inherits="GoldNet.JXKP.cbhs.xyhs.get_yongyou_data.get_yongyou_data" %>
<%@ Register Assembly="Goldnet.Ext.Web" Namespace="Goldnet.Ext.Web" TagPrefix="ext" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
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
       function edit(data_id)
       {
           Goldnet.AjaxMethod.request( 'data_edit', {params: {item_code:data_id}});
       }
       var RefreshData = function() {
            Store1.reload();
        }
    </script>
</head>
<body>
<ext:ScriptManager ID="ScriptManager1" runat="server" AjaxMethodNamespace="CompanyX" />
    <ext:Store ID="Store1" runat="server" WarningOnDirty="false">
        <Reader>
            <ext:JsonReader ReaderID="ITEM_CODE">
                <Fields>
                    <ext:RecordField Name="ITEM_CODE" Type="String" Mapping="ITEM_CODE" />
                    <ext:RecordField Name="ITEM_NAME" Type="String" Mapping="ITEM_NAME" />
                    <ext:RecordField Name="COSTS" Type="Float" Mapping="COSTS" />
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
                        <ext:GridPanel ID="GridPanel1" runat="server" StoreID="Store1" StripeRows="true"
                            ClicksToEdit="1" TrackMouseOver="true" AutoWidth="true" Height="480" Border="false">
                            <TopBar>
                                <ext:Toolbar ID="Toolbar_fjsr" runat="server" Visible="true" AutoWidth="true">
                                    <Items>                                
                                        <ext:ToolbarSeparator ID="ToolbarSeparator2" runat="server" />
                                        <ext:Button ID="Button_get" runat="server" Text="提取用友基础数据" Icon="Disk">
                                            <AjaxEvents>
                                                <Click OnEvent="Get_Yongyou_Data_Click">
                                                <Confirmation ConfirmRequest="true" Title="系统提示" Message="提取将覆盖原有数据,<br/>是否继续?" />
                                                    <EventMask Msg="载入中..." ShowMask="true" />
                                                </Click>
                                            </AjaxEvents>
                                        </ext:Button>
                                        <ext:ToolbarSeparator ID="ToolbarSeparator4" runat="server" />
                                        
                                    </Items>
                                </ext:Toolbar>
                            </TopBar>
                            <ColumnModel ID="ColumnModel1" runat="server">
                               <%-- <Columns>
                                    <ext:Column ColumnID="ITEM_CODE" Header="<div style='text-align:center;'>项目编码</div>" Width="130" Align="left" Sortable="true"
                                        DataIndex="ITEM_CODE" MenuDisabled="true" />
                                    <ext:Column ColumnID="ITEM_NAME" Header="<div style='text-align:center;'>项目名称</div>" Width="130" Align="left" Sortable="true"
                                        DataIndex="ITEM_NAME" MenuDisabled="true" />
                                    <ext:Column ColumnID="COSTS" Header="<div style='text-align:center;'>成本额</div>" Width="130" Align="Right" Sortable="true"
                                        DataIndex="COSTS" MenuDisabled="true">
                                        <Renderer Fn="rmbMoney" />
                                    </ext:Column>
                                    
                                </Columns>--%>
                            </ColumnModel>
                            <SelectionModel>
                                <ext:CheckboxSelectionModel ID="RowSelectionModel1" runat="server">
                                     <Listeners>
                                        <SelectionChange Handler="var tmpflg=  #{GridPanel1}.hasSelection()?false:true;   #{Button_del}.setDisabled(tmpflg);  #{Button_del_true}.setDisabled(tmpflg);" />
                                    </Listeners>
                                </ext:CheckboxSelectionModel>
                            </SelectionModel>
                            <Listeners>
                                <Command Handler="edit(record.data.ITEM_CODE);" />
                            </Listeners>
                        </ext:GridPanel>
                    </ext:LayoutColumn>
                </Columns>
            </ext:ColumnLayout>
        </Body>
    </ext:ViewPort>
    <ext:Window ID="DetailWin" runat="server" Icon="Group" Title="" Width="370" Height="240"
        AutoShow="false" Modal="true" CenterOnLoad="true" AutoScroll="true" ShowOnLoad="false"
        Resizable="true" StyleSpec="background-color:Transparent;" BodyStyle="background-color:Transparent;">
    </ext:Window>
    </form>
</body>
</html>


