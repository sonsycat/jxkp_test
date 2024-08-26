<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="InpCostInfo.aspx.cs" Inherits="GoldNet.JXKP.GuideLook.Statement.InpCostInfo" %>

<%@ Register Assembly="Goldnet.Ext.Web" Namespace="Goldnet.Ext.Web" TagPrefix="ext" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>无标题页</title>
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
        h2
        {
            font-size: 24px;
            letter-spacing: 1px;
            margin: 10px 0 20px;
            padding: 0;
        }
    </style>

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
    <ext:ScriptManager ID="ScriptManager1" runat="server">
    </ext:ScriptManager>
    <ext:Store ID="Store1" runat="server">
        <Reader>
            <ext:JsonReader>
                <Fields>
                    <ext:RecordField Name="DATE_TIME" />
                    <ext:RecordField Name="NAME" />
                    <ext:RecordField Name="ITEM_NAME" />
                    <ext:RecordField Name="ITEM_SPEC" />
                    <ext:RecordField Name="AMOUNT" />
                    <ext:RecordField Name="UNITS" />
                    <ext:RecordField Name="ORDERED_BY" />
                    <ext:RecordField Name="PERFORMED_BY" />
                    <ext:RecordField Name="COSTS" />
                    <ext:RecordField Name="CHARGES" />
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
                                <ext:ToolbarSpacer ID="ToolbarSpacer2" runat="server" Width="10" />
                                <ext:Label ID="Label7" runat="server" Text="统计日期">
                                </ext:Label>
                                <ext:ToolbarSpacer ID="ToolbarSpacer1" runat="server" Width="10" />
                                <ext:ComboBox runat="server" ID="Comb_StartYear" Width="60" ListWidth="60" SelectedIndex="0">
                                </ext:ComboBox>
                                <ext:ToolbarTextItem ID="ToolbarTextItem2" runat="server" Text="年" />
                                <ext:ComboBox runat="server" ID="Comb_StartMonth" Width="40" ListWidth="40" SelectedIndex="0">
                                </ext:ComboBox>
                                <ext:ToolbarTextItem ID="ToolbarTextItem1" runat="server" Text="月" />
                                <ext:ComboBox runat="server" ID="Comb_StartDate" Width="40" ListWidth="40" SelectedIndex="0">
                                </ext:ComboBox>
                                <ext:ToolbarTextItem ID="dd1Name" runat="server" Text="日 " />
                                <ext:ToolbarTextItem ID="ToolbarTextItem7" runat="server" Text="   至   " />
                                <ext:ToolbarSpacer ID="ToolbarSpacer5" runat="server" Width="6" />
                                <ext:ComboBox runat="server" ID="Comb_EndYear" Width="60" ListWidth="60" SelectedIndex="0">
                                </ext:ComboBox>
                                <ext:ToolbarTextItem ID="ToolbarTextItem4" runat="server" Text="年" />
                                <ext:ComboBox runat="server" ID="Comb_EndMonth" Width="40" ListWidth="40" SelectedIndex="0">
                                </ext:ComboBox>
                                <ext:ToolbarTextItem ID="ToolbarTextItem5" runat="server" Text="月" />
                                <ext:ComboBox runat="server" ID="Comb_EndDate" Width="40" ListWidth="40" SelectedIndex="0">
                                </ext:ComboBox>
                                <ext:ToolbarTextItem ID="dd2Name" runat="server" Text="日 " />
                                <ext:Label ID="Label3" runat="server" Text="ID号：">
                                </ext:Label>
                                <ext:TextField runat="server" ID="txtId">
                                </ext:TextField>
                                <ext:Label ID="Label1" runat="server" Text="住院号：">
                                </ext:Label>
                                <ext:TextField runat="server" ID="txtInHospital">
                                </ext:TextField>
                                <ext:Button ID="btnQurey" runat="server" Text=" 查询 " Icon="DatabaseGo">
                                    <AjaxEvents>
                                        <Click OnEvent="GetQueryPortalet" Success="#{btnExcel}.enable()">
                                            <EventMask Msg="载入中..." ShowMask="true" />
                                        </Click>
                                    </AjaxEvents>
                                </ext:Button>
                                <ext:ToolbarSeparator ID="ToolbarSeparator4" runat="server" />
                                <ext:ToolbarFill>
                                </ext:ToolbarFill>
                                <ext:Button ID="btnExcel" runat="server" Text=" 导出Excel " Icon="PageWhiteExcel" OnClick="OutExcel"
                                    AutoPostBack="true" Disabled="true">
                                </ext:Button>
                            </Items>
                        </ext:Toolbar>
                    </North>
                    <Center>
                        <ext:GridPanel ID="GridPanel_Show" runat="server" Border="false" StoreID="Store1"
                            Height="800" Width="600" AutoScroll="true" Title="报表信息" Header="false">
                            <ColumnModel ID="ColumnModel1" runat="server">
                                <Columns>
                                    <ext:ExtColumn ColumnID="DATE_TIME" Header="日期" Sortable="true" DataIndex="DATE_TIME" />
                                    <ext:ExtColumn ColumnID="NAME" Header="姓名" Sortable="true" DataIndex="NAME" />
                                    <ext:ExtColumn ColumnID="ITEM_NAME" Header="项目名称" Sortable="true" DataIndex="ITEM_NAME" />
                                    <ext:ExtColumn ColumnID="ITEM_SPEC" Header="规格" Sortable="true" DataIndex="ITEM_SPEC" />
                                    <ext:ExtColumn ColumnID="AMOUNT" Header="数量" Sortable="true" DataIndex="AMOUNT" />
                                    <ext:ExtColumn ColumnID="UNITS" Header="单位" Sortable="true" DataIndex="UNITS" />
                                    <ext:ExtColumn ColumnID="ORDERED_BY" Header="开单科室" Sortable="true" DataIndex="ORDERED_BY" />
                                    <ext:ExtColumn ColumnID="PERFORMED_BY" Header="执行科室" Sortable="true" DataIndex="PERFORMED_BY" />
                                    <ext:ExtColumn ColumnID="COSTS" Header="应收" Sortable="true" DataIndex="COSTS" />
                                    <ext:ExtColumn ColumnID="CHARGES" Header="实收" Sortable="true" DataIndex="CHARGES" />
                                </Columns>
                            </ColumnModel>
                            <SelectionModel>
                                <ext:RowSelectionModel SingleSelect="true">
                                </ext:RowSelectionModel>
                            </SelectionModel>
                            <LoadMask ShowMask="true" />
                            <BottomBar>
                                <ext:PagingToolbar ID="PagingToolBar1" runat="server" PageSize="20" StoreID="Store1"
                                    AutoWidth="true" DisplayInfo="true" AutoDataBind="true">
                                </ext:PagingToolbar>
                            </BottomBar>
                        </ext:GridPanel>
                    </Center>
                </ext:BorderLayout>
            </Body>
        </ext:ViewPort>
    </div>
    </form>
</body>
</html>
