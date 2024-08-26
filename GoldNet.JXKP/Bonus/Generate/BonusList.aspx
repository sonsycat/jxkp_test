<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="BonusList.aspx.cs" Inherits="GoldNet.JXKP.BonusList" %>
<%@ Register Assembly="Goldnet.Ext.Web" Namespace="Goldnet.Ext.Web" TagPrefix="ext" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title></title>
    <style type="text/css">
        .x-grid3-cell-inner
        {
            border-right: 1px solid #eceff6;
        }
        .x-grid3-row td, .x-grid3-summary-row td
        {
            padding-right: 0px;
        }       
    </style>
      <script type="text/javascript">
        var RefreshData = function() {
            Store1.reload();
        }
        function selectRow() {
            Btn_Look.enable();
            var i = typeof (eval('document.all.Btn_Del'));
            if (i != 'undefined') {
                Btn_Del.enable();
            }
        }
        function deselectRow() {
            if (!GridPanel2.hasSelection()) {
                Btn_Look.disable();
                var i = typeof (eval('document.all.Btn_Del'));
                if (i != 'undefined') {
                    Btn_Del.disable();
                }
             }
         }
         var cellSelect = function() {
             var record = GridPanel2.getRowsValues();
             var value = record[0]['BONUSNAME'];
             return value;
         }

      </script>
       <link rel="stylesheet" type="text/css" href="../Orthers/Cbouns.css" />
</head>
<body>
    <ext:ScriptManager ID="ScriptManager1" runat="server" />
    <form id="form1" runat="server">
    <ext:Store ID="Store1" AutoLoad="true" runat="server"  OnRefreshData="Store_RefreshData" >
        <Reader>
            <ext:JsonReader ReaderID="ID">
                <Fields>
                     <ext:RecordField Name="ID">
                    </ext:RecordField>
                    <ext:RecordField Name="BONUSNAME">
                    </ext:RecordField>
                    <ext:RecordField Name="BONUSDATE">
                    </ext:RecordField>
                    <ext:RecordField Name="STATE">
                    </ext:RecordField>
                    <ext:RecordField Name="CREATEDATE">
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
                                            <ext:Button ID="Btn_Add" Text="生成新奖金" Icon="Add" runat="server"  >
                                            <AjaxEvents>
                                              <Click OnEvent="Btn_Add_Click">                                                        
                                                    </Click>
                                             </AjaxEvents>
                                            </ext:Button>
                                            <ext:Button ID="Btn_Look" Text="查看奖金" Icon="NoteEdit" runat="server" Disabled="true" >
                                                <AjaxEvents>
                                                    <Click OnEvent="Btn_Edit_Click">
                                                        <ExtraParams>
                                                            <ext:Parameter Name="Values" Value="Ext.encode(#{GridPanel2}.getRowsValues())" Mode="Raw" />
                                                        </ExtraParams>
                                                    </Click>
                                                </AjaxEvents>
                                            </ext:Button>
                                            <ext:Button ID="Btn_Del" Text="删除奖金" Icon="Delete" runat="server"  Disabled="true">
                                                <AjaxEvents>
                                                    <Click OnEvent="Btn_Del_Click">
                                                     <Confirmation  BeforeConfirm="config.confirmation.message = '你确定要删除奖金（'+cellSelect()+'）吗？';"  Title="系统提示"   ConfirmRequest="true" />
                                                        <ExtraParams>
                                                            <ext:Parameter Name="Values" Value="Ext.encode(#{GridPanel2}.getRowsValues())" Mode="Raw" />
                                                        </ExtraParams>
                                                    </Click>
                                                </AjaxEvents>
                                            </ext:Button>
                                            <ext:Button ID="Btn_Ref" Text="刷新" Icon="ArrowRefresh" runat="server">
                                                <Listeners>
                                                    <Click Handler="Store1.reload()" />
                                                </Listeners>
                                            </ext:Button>
                                            
                                        </Items>
                                    </ext:Toolbar>
                                </TopBar>
                                <ColumnModel ID="ColumnModel2" runat="server">
                                    <Columns>
                                        <ext:Column ColumnID="BONUSNAME" Header="奖金名称" Width="200" DataIndex="BONUSNAME" MenuDisabled="true" Align="Center">
                                        </ext:Column> 
                                         <ext:Column ColumnID="BONUSDATE" Header="时间" Width="100" DataIndex="BONUSDATE" MenuDisabled="true" Align="Center">
                                        </ext:Column>  
                                        <ext:Column ColumnID="STATE" Header="状态" Width="100" DataIndex="STATE" MenuDisabled="true"  Align="Center">
                                        </ext:Column>   
                                         <ext:Column ColumnID="CREATEDATE" Header="创建日期" Width="100" DataIndex="CREATEDATE" MenuDisabled="true" Align="Center">
                                        </ext:Column>                         
                                    </Columns>
                                </ColumnModel>
                                <SelectionModel>
                                    <ext:RowSelectionModel ID="rowselection" SingleSelect="true"> 
                                        <Listeners>
                                            <RowSelect Fn="selectRow" />
                                            <RowDeselect Fn="deselectRow" />
                                            <SelectionChange Handler="if (!#{GridPanel2}.hasSelection()){deselectRow();}else{selectRow();}" />
                                        </Listeners>
                                    </ext:RowSelectionModel>
                                </SelectionModel>
                                <LoadMask ShowMask="true" />
                                <AjaxEvents>
                                    <DblClick OnEvent="Btn_Edit_Click">
                                        <ExtraParams>
                                            <ext:Parameter Name="Values" Value="Ext.encode(#{GridPanel2}.getRowsValues())" Mode="Raw" />
                                        </ExtraParams>
                                    </DblClick>
                                </AjaxEvents>
                            </ext:GridPanel>
                        </ext:LayoutColumn>
                    </Columns>
                </ext:ColumnLayout>
            </Body>
        </ext:ViewPort>
         <ext:Window ID="DetailWin" runat="server" Icon="Add" Title="新测算奖金" Width="360"
            Height="200"  AutoShow="false" Modal="true"  CenterOnLoad="true" AutoScroll="false" ShowOnLoad="false" Closable="false" Resizable="false" StyleSpec="background-color:Transparent;" BodyStyle="background-color:Transparent;" CloseAction="Hide">
  </ext:Window>
    </div>
    </form>
</body>
</html>
