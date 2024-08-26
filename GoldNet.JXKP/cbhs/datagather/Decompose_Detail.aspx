<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Decompose_Detail.aspx.cs" Inherits="GoldNet.JXKP.cbhs.datagather.Decompose_Detail" %>
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
    <style type="text/css">
    
     .x-grid3-row td, .x-grid3-summary-row td{  
     padding-right: 0px;   
       /*显示竖线*/  
     border-right:1px solid #eceff6;  
     /*显示底线*/  
     border-bottom:1px solid #eceff6;  
 } 
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <ext:ScriptManager ID="ScriptManager1" runat="server" AjaxMethodNamespace="CompanyX" />
        <ext:Store ID="Store1" runat="server" OnSubmitData="SubmitData"
            WarningOnDirty="false">
            <Reader>
                <ext:JsonReader ReaderID="DEPT_CODE">
                    <Fields>
                        <ext:RecordField Name="DEPT_CODE" Type="String" Mapping="DEPT_CODE" />
                        <ext:RecordField Name="DEPT_NAME" Type="String" Mapping="DEPT_NAME" />
                        <ext:RecordField Name="ITEM_CODE" Type="String" Mapping="ITEM_CODE" />
                        <ext:RecordField Name="ACCOUNTING_DATE" Type="String" Mapping="ACCOUNTING_DATE" />
                        <ext:RecordField Name="TOTAL_COSTS" Type="Float" Mapping="TOTAL_COSTS" />
                        <ext:RecordField Name="COSTS" Type="Float" Mapping="COSTS" />
                        <ext:RecordField Name="COSTS_ARMYFREE" Type="Float" Mapping="COSTS_ARMYFREE" />
                        <ext:RecordField Name="OPERATOR" Type="String" Mapping="OPERATOR" />
                        <ext:RecordField Name="OPERATOR_DATE" Type="String" Mapping="OPERATOR_DATE" />
                        <ext:RecordField Name="GET_TYPE" Type="String" Mapping="GET_TYPE" />
                        <ext:RecordField Name="COST_FLAG" Type="String" Mapping="COST_FLAG" />
                        <ext:RecordField Name="BALANCE_TAG" Type="String" Mapping="BALANCE_TAG" />
                        <ext:RecordField Name="DEPT_TYPE_FLAG" Type="String" Mapping="DEPT_TYPE_FLAG" />
                        <ext:RecordField Name="MEMO" Type="String" Mapping="MEMO" />
                    </Fields>
                </ext:JsonReader>
            </Reader>
        </ext:Store>
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
                                       
                                            <ext:Button ID="Button_save" runat="server" Text="保存" Icon="Disk">
                                                <Listeners>
                                                    <Click Handler="#{GridPanel1}.submitData();" />
                                                </Listeners>
                                            </ext:Button>
                                               <ext:ToolbarSeparator ID="ToolbarSeparator2" runat="server" />
                                            <ext:Button ID="Button_refresh" runat="server" Text="返回" Icon="ArrowUndo">
                                                <AjaxEvents>
                                                    <Click OnEvent="btnCancle_Click">
                                                        <EventMask Msg="载入中..." ShowMask="true" />
                                                    </Click>
                                                </AjaxEvents>
                                            </ext:Button>

                                        </Items>
                                    </ext:Toolbar>
                                </TopBar>
                                <ColumnModel ID="ColumnModel1" runat="server">
                                    <Columns>
                                        <ext:Column ColumnID="DEPT_CODE" Header="<div style='text-align:center;'>科室代码</div>" Width="130" Align="left" Sortable="false"
                                            Hidden="true" DataIndex="DEPT_CODE" MenuDisabled="true" />
                                        <ext:Column ColumnID="DEPT_NAME" Header="<div style='text-align:center;'>科室</div>" Width="130" Align="left" Sortable="false"
                                            DataIndex="DEPT_NAME" MenuDisabled="true" />
                                        <ext:Column ColumnID="TOTAL_COSTS" Header="<div style='text-align:center;'>成本额</div>" Width="130" Align="Right" Sortable="false"
                                            DataIndex="TOTAL_COSTS" MenuDisabled="true">
                                           
                                        </ext:Column>
                                        <ext:Column ColumnID="COSTS" Header="<div style='text-align:center;'>实际成本</div>" Width="130" Align="Right" Sortable="false"
                                            DataIndex="COSTS" MenuDisabled="true" >
                                            <Renderer Fn="rmbMoney" />
                                        </ext:Column>
                                        <ext:Column ColumnID="COSTS_ARMYFREE" Header="<div style='text-align:center;'>减免成本</div>" Width="130" Align="Right" Sortable="false"
                                            DataIndex="COSTS_ARMYFREE" MenuDisabled="true" >
                                            <Renderer Fn="rmbMoney" />
                                        </ext:Column>
                                        <ext:Column ColumnID="MEMO" Header="<div style='text-align:center;'>备注</div>" Width="150" Align="left" Sortable="false"
                                            DataIndex="MEMO" MenuDisabled="true">
                                            <Editor>
                                                <ext:NumberField ID="NumberField2" runat="server" />
                                            </Editor>
                                        </ext:Column>
                                    </Columns>
                                </ColumnModel>
                                <SelectionModel>
                                    <ext:CheckboxSelectionModel ID="RowSelectionModel1" runat="server">
                                        
                                    </ext:CheckboxSelectionModel>
                                </SelectionModel>
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

