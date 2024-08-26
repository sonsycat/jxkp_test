<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="BonusPersonSearch.aspx.cs"
    Inherits="GoldNet.JXKP.Bonus.Generate.BonusPersonSearch" %>

<%@ Register Assembly="Goldnet.Ext.Web" Namespace="Goldnet.Ext.Web" TagPrefix="ext" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <link rel="stylesheet" type="text/css" href="../Orthers/Cbouns.css" />
    
    <style type="text/css">
        .search-item
        {
            font: normal 11px tahoma, arial, helvetica, sans-serif;
            padding: 3px 10px 3px 10px;
            border: 1px solid #fff;
            border-bottom: 1px solid #eeeeee;
            white-space: normal;
            color: #555;
            width: 200px;
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
            width: 140px;
            display: block;
            clear: none;
        }
    </style>
    
    <script type="text/javascript">

         var RefreshData = function() {
            Store1.reload();
        };   

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
             };    
    </script>

</head>
<body>
    <ext:ScriptManager ID="ScriptManager1" runat="server" AjaxMethodNamespace="Goldnet" />
    <ext:Store ID="Store1" AutoLoad="true" runat="server" OnRefreshData="Store_RefreshData">
        <Reader>
            <ext:JsonReader ReaderID="ID">
                <Fields>
                </Fields>
            </ext:JsonReader>
        </Reader>
    </ext:Store>
    <ext:Store ID="Store3" runat="server" AutoLoad="true">
        <Reader>
            <ext:JsonReader Root="Staffdepts">
                <Fields>
                    <ext:RecordField Name="DEPT_NAME" />
                    <ext:RecordField Name="DEPT_CODE" />
                </Fields>
            </ext:JsonReader>
        </Reader>
    </ext:Store>
    <form id="form1" runat="server">
    <div>
        <ext:ViewPort ID="ViewPort1" runat="server" AutoWidth="true">
            <Body>
                <ext:BorderLayout ID="BorderLayout1" runat="server">
                    <Center>
                        <ext:Panel ID="Panel2" runat="server" BodyBorder="true" Border="false">
                            <Body>
                                <ext:ColumnLayout ID="ColumnLayout1" runat="server" Split="true" FitHeight="true">
                                    <Columns>
                                        <ext:LayoutColumn ColumnWidth="1">
                                            <ext:GridPanel ID="GridPanel1" runat="server" Border="false" StoreID="Store1" StripeRows="true"
                                                Width="2000" TrackMouseOver="true" Height="480" ClicksToEdit="1">
                                                <TopBar>
                                                    <ext:Toolbar ID="Toolbar1" runat="server" Visible="true" AutoWidth="true">
                                                        <Items>
                                                            <ext:ToolbarTextItem ID="ToolbarTextItem1" runat="server" Text="奖金选择：" />
                                                            <ext:ComboBox ID="comindex" runat="server" AllowBlank="true" EmptyText="请选择奖金" Width="200"
                                                                FieldLabel="奖金选择">
                                                            </ext:ComboBox>
                                                            <ext:ToolbarTextItem ID="ToolbarTextItem5" runat="server" Text="科室名称：" />
                                                            <ext:ComboBox runat="server" ID="Combodept" Width="140" ListWidth="240" StoreID="Store3"
                                                                DisplayField="DEPT_NAME" ValueField="DEPT_CODE" TypeAhead="false" LoadingText="Searching..."
                                                                PageSize="1000" ItemSelector="div.search-item" MinChars="1" FieldLabel="科室信息">
                                                                <Template ID="Template1" runat="server">
                                       <tpl for=".">
                                          <div class="search-item">
                                             <h3><span style="width:auto">{DEPT_CODE}</span>{DEPT_NAME}</h3>
                                          </div>
                                       </tpl>
                                                                </Template>
                                                            </ext:ComboBox>
                                                            <ext:ToolbarTextItem ID="ToolbarTextItem2" runat="server" Text="人员类别：" />
                                                            <ext:ComboBox ID="comtype" runat="server" AllowBlank="true" Width="200" FieldLabel="人员类别">
                                                            </ext:ComboBox>
                                                            <ext:ToolbarSpacer ID="ToolbarSpacer5" runat="server" Width="6" />
                                                            <ext:Button ID="Btn_View" runat="server" Icon="Zoom" Text="查询">
                                                                <AjaxEvents>
                                                                    <Click OnEvent="Btn_View_Clicked">
                                                                        <EventMask ShowMask="true" Msg="请稍候..." />
                                                                    </Click>
                                                                </AjaxEvents>
                                                            </ext:Button>
                                                            <ext:ToolbarSeparator ID="ToolbarSeparator3" runat="server" />
                                                            <ext:Button ID="Btn_Exl" Text="导出Excel" Icon="TextColumns" runat="server" OnClick="OutExcel"
                                                                AutoPostBack="true">
                                                            </ext:Button>
                                                        </Items>
                                                    </ext:Toolbar>
                                                </TopBar>
                                                <ColumnModel ID="extColumnModel2" runat="server">
                                                    <Columns>
                                                    </Columns>
                                                </ColumnModel>
                                                <SelectionModel>
                                                    <ext:RowSelectionModel SingleSelect="true">
                                                    </ext:RowSelectionModel>
                                                </SelectionModel>
                                                <LoadMask ShowMask="true" />
                                            </ext:GridPanel>
                                        </ext:LayoutColumn>
                                    </Columns>
                                </ext:ColumnLayout>
                            </Body>
                        </ext:Panel>
                    </Center>
                </ext:BorderLayout>
            </Body>
        </ext:ViewPort>
    </div>
    </form>
</body>
</html>
