<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Singleaward_Detail.aspx.cs" Inherits="GoldNet.JXKP.zlgl.SysManage.Singleaward_Detail" %>
<%@ Register Assembly="Goldnet.Ext.Web" Namespace="Goldnet.Ext.Web" TagPrefix="ext" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title></title>
    <style type="text/css">
        .search-item
        {
            font: normal 11px tahoma, arial, helvetica, sans-serif;
            padding: 3px 10px 3px 10px;
            border: 1px solid #fff;
            border-bottom: 1px solid #eeeeee;
            white-space: normal;
            color: #555;
        }
        .search-item h3
        {
            display: block;
            font: inherit;
            font-weight: bold;
            color: #222;
        }
        .search-item h3 span
        {
            float: right;
            font-weight: normal;
            margin: 0 0 5px 5px;
            width: 100px;
            display: block;
            clear: none;
        }
        p
        {
            width: 650px;
        }
        .ext-ie .x-form-text
        {
            position: static !important;
        }
    </style>

    <script type="text/javascript">
        function backToList() {
            window.navigate("GuideType.aspx");
        }
         var RefreshData = function() {
            Store1.reload();
            Store2.reload();
            Store3.reload();
        }
        function edit(id,straction) {
            window.navigate("GuideTypeEdit.aspx?id="+id+"&straction="+straction);
        }
    </script>

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
              
               v = whole + sub;
               if (v.charAt(0) == '-') {
                   return '-' + v.substr(1);
               }
               return v;
       }
    </script>

    <script type="text/javascript">
        var CheckForm = function() {
            if (ComGuide.validate() == false) {
                return false;
            }
            if (commonguide.validate() == false) {
                return false;
            }
            
            return true;
        }
    </script>

</head>
<body>
    <form id="form1" runat="server">
    <div>
        <ext:ScriptManager ID="ScriptManager1" runat="server" AjaxMethodNamespace="CompanyX" />
        <ext:Store ID="Store1" runat="server" AutoLoad="true">
            <Reader>
                <ext:JsonReader>
                    <Fields>
                        <ext:RecordField Name="ID" />
                        <ext:RecordField Name="DEPT_CODE" />
                        <ext:RecordField Name="DEPT_NAME" />
                        <ext:RecordField Name="AWARD_DATE" />
                        <ext:RecordField Name="MONEY" />
                        <ext:RecordField Name="TYPE_NAME" />
                        <ext:RecordField Name="CHECKSTAN" />
                        <ext:RecordField Name="REMARK" >
                        
                        </ext:RecordField>
                    </Fields>
                </ext:JsonReader>
            </Reader>
        </ext:Store>
        <ext:ViewPort ID="ViewPort1" runat="server">
            <Body>
                <ext:ColumnLayout ID="ColumnLayout1" runat="server" Split="true" FitHeight="true">
                    <Columns>
                        <ext:LayoutColumn ColumnWidth="1">
                            <ext:GridPanel ID="GridPanel1" runat="server" Border="false" StoreID="Store1" StripeRows="true"
                                AutoWidth="true">
                                <TopBar>
                                    <ext:Toolbar ID="Toolbar_guidetype" runat="server" Visible="true" AutoWidth="true">
                                        <Items>
                                            <ext:ToolbarSpacer ID="ToolbarSpacer5" runat="server" Width="6" />
                                            <ext:ToolbarSeparator ID="ToolbarSeparator3" runat="server" />
                                            <ext:DateField ID="stardate" runat="server" FieldLabel="开始时间：" Width="100" EnableKeyEvents="true"
                                                ReadOnly="true" />
                                            <ext:KeyNav ID="stardate1" runat="server" Target="stardate">
                                                <Enter Handler="var str = document.getElementById('stardate').value ; var   reg=/^(\d{4})(\d{2})(\d{2})$/; document.getElementById('stardate').value   =   str.replace(reg, '$1-$2-$3');" />
                                            </ext:KeyNav>
                                            <ext:DateField ID="enddate" runat="server" FieldLabel="结束时间：" Width="100" EnableKeyEvents="true"
                                                ReadOnly="true" />
                                            <ext:KeyNav ID="enddate1" runat="server" Target="enddate">
                                                <Enter Handler="var str = document.getElementById('enddate').value ; var   reg=/^(\d{4})(\d{2})(\d{2})$/; document.getElementById('enddate').value   =   str.replace(reg, '$1-$2-$3');" />
                                            </ext:KeyNav>
                                            <ext:Button ID="Buttonedit" runat="server" Text="查询" Icon="Zoom">
                                                <AjaxEvents>
                                                    <Click OnEvent="GetQueryPortalet">
                                                        <EventMask Msg="载入中..." ShowMask="true" />
                                                    </Click>
                                                </AjaxEvents>
                                            </ext:Button>
                                            <ext:Button ID="Btn_Export" runat="server" Text="导出Excel" Icon="PageWhiteExcel"  OnClick="OutExcel" AutoPostBack="true">
                        </ext:Button>
                                            <ext:ToolbarSeparator ID="ToolbarSeparator4" runat="server" />
                                           
                                        </Items>
                                    </ext:Toolbar>
                                </TopBar>
                                <ColumnModel ID="ColumnModel1" runat="server">
                                    <Columns>
                                        <ext:Column ColumnID="DEPT_NAME" Header="奖惩科室" Width="100" Align="left" Sortable="true"
                                            DataIndex="DEPT_NAME" MenuDisabled="true" />
                                        <ext:Column ColumnID="AWARD_DATE" Header="奖惩时间" Width="100" Align="left" Sortable="true"
                                            DataIndex="AWARD_DATE" MenuDisabled="true" />
                                       <ext:Column ColumnID="MONEY" Header="奖惩金额" Width="100" Align="right" Sortable="true"
                                            DataIndex="MONEY" MenuDisabled="true">
                                            <Renderer Fn="rmbMoney" />
                                        </ext:Column>
                                        <ext:Column ColumnID="TYPE_NAME" Header="奖惩项目" Width="200" Align="left" Sortable="true"
                                            DataIndex="TYPE_NAME" MenuDisabled="true" />
                                        <ext:Column ColumnID="CHECKSTAN" Header="奖惩标准" Width="300" Align="left" Sortable="true"
                                            DataIndex="CHECKSTAN" MenuDisabled="true" />
                                       
                                        <ext:Column ColumnID="REMARK" Header="备注" Width="300" Align="left" Sortable="true"
                                            DataIndex="REMARK" MenuDisabled="true" />
                                        
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
