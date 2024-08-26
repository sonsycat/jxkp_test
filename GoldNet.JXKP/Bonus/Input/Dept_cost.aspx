<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Dept_cost.aspx.cs" Inherits="GoldNet.JXKP.Bonus.Input.Dept_cost" %>

<%@ Register Assembly="Goldnet.Ext.Web" Namespace="Goldnet.Ext.Web" TagPrefix="ext" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
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
        var rmbMoney = function (v) {
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
    <form id="form2" runat="server">
    <ext:ScriptManager ID="ScriptManager1" runat="server" />
    <ext:Store ID="Store1" runat="server">
        <Reader>
            <ext:JsonReader>
                <Fields>
                      <ext:RecordField Name="科室名称" />
                    <ext:RecordField Name="项目次数" />
                    <ext:RecordField Name="分值" />
                    <ext:RecordField Name="项目名称" />
                    <ext:RecordField Name="项目明细" />
                </Fields>
            </ext:JsonReader>
        </Reader>
    </ext:Store>
     <ext:Store ID="SYear" runat="server">
        <Reader>
            <ext:JsonReader ReaderID="YEAR">
                <Fields>
                    <ext:RecordField Name="YEAR" />
                </Fields>
            </ext:JsonReader>
        </Reader>
    </ext:Store>
    <ext:Store ID="SMonth" runat="server">
        <Reader>
            <ext:JsonReader ReaderID="MONTH">
                <Fields>
                    <ext:RecordField Name="MONTH" />
                </Fields>
            </ext:JsonReader>
        </Reader>
    </ext:Store>

     <ext:Store ID="SDept" runat="server" AutoLoad="true">
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
                                    <ext:Toolbar ID="Toolbar_detptype" runat="server" StyleSpec="border:0">
                                        <Items>
                                             <ext:Label ID="Label1" runat="server" Text="查询时间：" />
                                            <ext:DateField ID="stardate" runat="server" FieldLabel="查询时间：" Width="80" EnableKeyEvents="true" />                                           
                                             <ext:Label ID="Label3" runat="server" Text="科室：" Hidden="true" />
                                            <ext:ComboBox ID="cbbdept" runat="server" StoreID="SDept" DisplayField="DEPT_NAME"
                                                ValueField="DEPT_CODE" TypeAhead="false" LoadingText="Searching..." Width="150"
                                                PageSize="10" ItemSelector="div.search-item" MinChars="1" ListWidth="200" >
                                                <Template ID="Template1" runat="server">
                                                    <tpl for=".">
                                                        <div class="search-item">
                                                             <h3>{DEPT_NAME}</h3>
                                                             </div>
                                                      </tpl>                                                                                                       
                                                </Template>
                                            </ext:ComboBox>
                                         <%--   <ext:ComboBox ID="cbbType" runat="server" ReadOnly="true" ForceSelection="true"
                                                SelectOnFocus="true" Width="200">
                                                <Items>
                                                    <ext:ListItem Text="门诊平日诊查费" Value="1" />
                                                    <ext:ListItem Text="无假日门诊诊查费" Value="2" />
                                                    <ext:ListItem Text="住院平日诊查费" Value="3" />
                                                </Items>
                                            </ext:ComboBox>                  --%>                                             
                                            <ext:ToolbarSpacer ID="ToolbarSpacer2" runat="server" Width="20">
                                            </ext:ToolbarSpacer>
                                            <ext:Button ID="Buttonlist" runat="server" Text="查询" Icon="ArrowRefresh">
                                                <AjaxEvents>
                                                    <Click OnEvent="GetQueryPortalet" Timeout="9999999">
                                                        <EventMask Msg="载入中..." ShowMask="true" />
                                                    </Click>
                                                </AjaxEvents>
                                            </ext:Button>                                      
                                            <ext:ToolbarSpacer ID="ToolbarSpacer3" runat="server" Width="20">
                                            </ext:ToolbarSpacer>
                                            <ext:Button ID="btn_Excel" runat="server" OnClick="OutExcel" AutoPostBack="true"
                                                Text="导出Excel" Icon="PageWhiteExcel" CausesValidation="false">
                                            </ext:Button>
                                        </Items>
                                    </ext:Toolbar>
                                </TopBar>
                                <ColumnModel ID="ColumnModel2" runat="server">
                                 
                                        <Columns>
                                        <ext:Column Header="科室名称" Width="150" Align="Left" Sortable="true" MenuDisabled="true"
                                            ColumnID="科室名称" DataIndex="科室名称" Hidden="false">
                                        </ext:Column>
                                          <ext:Column Header="项目名称" Width="150" Align="Left" Sortable="true" MenuDisabled="true"
                                            ColumnID="项目名称" DataIndex="项目名称" Hidden="false">
                                        </ext:Column>
                                        <ext:Column Header="项目明细" Width="150" Align="Left" Sortable="true" MenuDisabled="true"
                                            ColumnID="项目明细" DataIndex="项目明细" Hidden="false">
                                        </ext:Column>
                                           <ext:Column Header="项目次数" Width="150" Align="Left" Sortable="true" MenuDisabled="true"
                                            ColumnID="项目次数" DataIndex="项目次数" Hidden="false">
                                            <Renderer Fn="rmbMoney" />
                                        </ext:Column>
                                         <%--  <ext:Column Header="单价" Width="100" Align="Left" Sortable="true" MenuDisabled="true"
                                            ColumnID="单价" DataIndex="单价">
                                            <Renderer Fn="rmbMoney" />
                                        </ext:Column>--%>
                                        <ext:Column Header="分值" Width="100" Align="Left" Sortable="true" MenuDisabled="true"
                                            ColumnID="分值" DataIndex="分值">
                                            <Renderer Fn="rmbMoney" />
                                        </ext:Column>
                                      
                                  
                                    </Columns>
                                </ColumnModel>
                                <SelectionModel>
                                    <ext:RowSelectionModel ID="rowselection" SingleSelect="true">
                                    </ext:RowSelectionModel>
                                </SelectionModel>
                                <LoadMask ShowMask="true" />
                              
                                <BottomBar>
                                    <ext:PagingToolbar ID="PagingToolBar2" runat="server" PageSize="20" StoreID="Store1"
                                        AutoWidth="true" DisplayInfo="false" AutoDataBind="true">
                                    </ext:PagingToolbar>
                                </BottomBar>
                            </ext:GridPanel>
                        </ext:LayoutColumn>
                    </Columns>
                </ext:ColumnLayout>
            </Body>
        </ext:ViewPort>
    </div>
    <%--  <ext:Window ID="Doctor_Detail" runat="server" Icon="Group" Title="收入明细" Width="880"
        Height="420" AutoShow="false" Modal="true" CenterOnLoad="true" AutoScroll="true"
        ShowOnLoad="false" Resizable="true" StyleSpec="background-color:Transparent;"
        BodyStyle="background-color:Transparent;">
    </ext:Window>--%>
    </form>
</body>
</html>


