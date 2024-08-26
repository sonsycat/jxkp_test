<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Quality_Submit.aspx.cs" Inherits="GoldNet.JXKP.zlgl.SysManage.Quality_Submit" %>
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
        <ext:ScriptManager ID="ScriptManager1" runat="server" AjaxMethodNamespace="Goldnet" />
        <ext:Store ID="Store1" runat="server" AutoLoad="true" OnRefreshData="Store_RefreshData">
            <Reader>
                <ext:JsonReader ReaderID="ID">
                    <Fields>
                        <ext:RecordField Name="TEMPLET_NAME" />
                        <ext:RecordField Name="DATES" />
                        <ext:RecordField Name="SUBMIT_USER" />
                        <ext:RecordField Name="SUBMIT_DATE">
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
                                            <ext:DateField ID="stardate" runat="server" FieldLabel="时间选择：" Width="100" EnableKeyEvents="true"
                                                ReadOnly="true" />
                                            <ext:KeyNav ID="stardate1" runat="server" Target="stardate">
                                                <Enter Handler="var str = document.getElementById('stardate').value ; var   reg=/^(\d{4})(\d{2})(\d{2})$/; document.getElementById('stardate').value   =   str.replace(reg, '$1-$2-$3');" />
                                            </ext:KeyNav>
                                           <ext:ToolbarSeparator ID="ToolbarSeparator1" runat="server" />
                                             <ext:Button ID="Buttonedit" runat="server" Text="查询" Icon="Zoom">
                                                <AjaxEvents>
                                                    <Click OnEvent="GetQueryPortalet">
                                                        <EventMask Msg="载入中..." ShowMask="true" />
                                                    </Click>
                                                </AjaxEvents>
                                            </ext:Button>
                                            
                                            <ext:ToolbarSeparator ID="ToolbarSeparator4" runat="server" />
                                            <ext:Button ID="Button1" runat="server" Text="审核" Icon="ApplicationKey">
                                                <AjaxEvents>
                                                    <Click OnEvent="SubmitQuality">
                                                     <Confirmation ConfirmRequest="true" Title="系统提示" Message="确定要审核该模板数据吗？,<br/>是否继续?" />
                                                        <EventMask Msg="载入中..." ShowMask="true" />
                                                    </Click>
                                                </AjaxEvents>
                                            </ext:Button>
                                            
                                            <ext:ToolbarSeparator ID="ToolbarSeparator2" runat="server" />
                                            <ext:Button ID="Button2" runat="server" Text="删除" Icon="ApplicationDelete">
                                                <AjaxEvents>
                                                    <Click OnEvent="Buttondel_Click">
                                                        <EventMask Msg="载入中..." ShowMask="true" />
                                                    </Click>
                                                </AjaxEvents>
                                            </ext:Button>
                                          
                                           
                                           
                                        </Items>
                                    </ext:Toolbar>
                                </TopBar>
                                <ColumnModel ID="ColumnModel1" runat="server">
                                    <Columns>
                                    <ext:Column ColumnID="ID" Header="模板编号" Width="50" Align="left" Sortable="true"
                                            DataIndex="ID" MenuDisabled="true" Hidden="true"/>
                                        <ext:Column ColumnID="TEMPLET_NAME" Header="模板名称" Width="200" Align="left" Sortable="true"
                                            DataIndex="TEMPLET_NAME" MenuDisabled="true" />
                              
                                        <ext:Column ColumnID="DATES" Header="时间" Width="100" Align="Center" Sortable="true"
                                            DataIndex="DATES" MenuDisabled="true" />
                                        <ext:Column ColumnID="SUBMIT_USER" Header="审核人" Width="100" Align="Center" Sortable="true"
                                            DataIndex="SUBMIT_USER" MenuDisabled="true" />
                                        <ext:Column ColumnID="SUBMIT_DATE" Header="审核时间" Width="100" Align="Center" Sortable="true"
                                            DataIndex="SUBMIT_DATE" MenuDisabled="true" />
                                       
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
