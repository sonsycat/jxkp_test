<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PERFORMANCE_JIANGJIN.aspx.cs" Inherits="GoldNet.JXKP.Bonus.Input.PERFORMANCE_JIANGJIN" %>

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
                    <ext:RecordField Name="基础工资" />
                    <ext:RecordField Name="绩效" />
                    <ext:RecordField Name="小计" />
                    <ext:RecordField Name="绩效补" />
                    <ext:RecordField Name="独生补" />
                    <ext:RecordField Name="住基金" />
                    <ext:RecordField Name="医保" />
                    <ext:RecordField Name="扣大病" />
                    <ext:RecordField Name="扣帐面" />
                    <ext:RecordField Name="扣款" />
                    <ext:RecordField Name="质检扣" />
                    <ext:RecordField Name="税金" />
                    <ext:RecordField Name="合计" />
                    <ext:RecordField Name="实领工资" />
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
                                             <ext:Label ID="Label3" runat="server" Text="科室：" />
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
                                        <ext:Column Header="基础工资" Width="150" Align="Left" Sortable="true" MenuDisabled="true"
                                            ColumnID="基础工资" DataIndex="基础工资" Hidden="false">
                                        </ext:Column>
                                          <ext:Column Header="绩效" Width="150" Align="Left" Sortable="true" MenuDisabled="true"
                                            ColumnID="绩效" DataIndex="绩效" Hidden="false">
                                        </ext:Column>
                                        <ext:Column Header="小计" Width="150" Align="Left" Sortable="true" MenuDisabled="true"
                                            ColumnID="小计" DataIndex="小计" Hidden="false">
                                        </ext:Column>
                                           <ext:Column Header="绩效补" Width="150" Align="Left" Sortable="true" MenuDisabled="true"
                                            ColumnID="绩效补" DataIndex="绩效补" Hidden="false">
                                            <Renderer Fn="rmbMoney" />
                                        </ext:Column>
                                           <ext:Column Header="独生补" Width="100" Align="Left" Sortable="true" MenuDisabled="true"
                                            ColumnID="独生补" DataIndex="独生补">
                                            <Renderer Fn="rmbMoney" />
                                        </ext:Column>
                                        <ext:Column Header="住基金" Width="100" Align="Left" Sortable="true" MenuDisabled="true"
                                            ColumnID="住基金" DataIndex="住基金">
                                            <Renderer Fn="rmbMoney" />
                                        </ext:Column>
                                         <ext:Column Header="医保" Width="100" Align="Left" Sortable="true" MenuDisabled="true"
                                            ColumnID="医保" DataIndex="医保">
                                            <Renderer Fn="rmbMoney" />
                                        </ext:Column>
                                         <ext:Column Header="扣大病" Width="100" Align="Left" Sortable="true" MenuDisabled="true"
                                            ColumnID="扣大病" DataIndex="扣大病">
                                            <Renderer Fn="rmbMoney" />
                                        </ext:Column>
                                         <ext:Column Header="扣帐面" Width="100" Align="Left" Sortable="true" MenuDisabled="true"
                                            ColumnID="扣帐面" DataIndex="扣帐面">
                                            <Renderer Fn="rmbMoney" />
                                        </ext:Column>
                                         <ext:Column Header="扣款" Width="100" Align="Left" Sortable="true" MenuDisabled="true"
                                            ColumnID="扣款" DataIndex="扣款">
                                            <Renderer Fn="rmbMoney" />
                                        </ext:Column>
                                         <ext:Column Header="质检扣" Width="100" Align="Left" Sortable="true" MenuDisabled="true"
                                            ColumnID="质检扣" DataIndex="质检扣">
                                            <Renderer Fn="rmbMoney" />
                                        </ext:Column>
                                         <ext:Column Header="税金" Width="100" Align="Left" Sortable="true" MenuDisabled="true"
                                            ColumnID="税金" DataIndex="税金">
                                            <Renderer Fn="rmbMoney" />
                                        </ext:Column>
                                          <ext:Column Header="合计" Width="100" Align="Left" Sortable="true" MenuDisabled="true"
                                            ColumnID="合计" DataIndex="合计">
                                            <Renderer Fn="rmbMoney" />
                                        </ext:Column>
                                          <ext:Column Header="实领工资" Width="100" Align="Left" Sortable="true" MenuDisabled="true"
                                            ColumnID="实领工资" DataIndex="实领工资">
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



