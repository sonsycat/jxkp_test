<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Dept_Guide_Set.aspx.cs"
    Inherits="GoldNet.JXKP.jxkh.Dept_Guide_Set" %>

<%@ Register Assembly="Goldnet.Ext.Web" Namespace="Goldnet.Ext.Web" TagPrefix="ext" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link rel="stylesheet" type="text/css" href="../../Bonus/Orthers/Cbouns.css" />
    <style type="text/css">
        h2
        {
            font-size: 24px;
            letter-spacing: 1px;
            margin: 10px 0 20px;
            padding: 0;
        }
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
        }   
    </script>

</head>
<body>
    <form id="form1" runat="server">
    <ext:ScriptManager ID="ScriptManager1" runat="server" AjaxMethodNamespace="CompanyX" />
    <ext:Store ID="Store1" runat="server" OnRefreshData="Store_RefreshData">
        <Reader>
            <ext:JsonReader ReaderID="DEPT_CODE">
                <Fields>
                    <ext:RecordField Name="DEPT_CODE" Type="String" Mapping="DEPT_CODE" />
                    <ext:RecordField Name="DEPT_NAME" Type="String" Mapping="DEPT_NAME" />
                    <ext:RecordField Name="GATHER_CODE" Type="String" Mapping="GATHER_CODE" />
                    <ext:RecordField Name="GATHER_NAME" Type="String" Mapping="GATHER_NAME" />
                    <ext:RecordField Name="STATION_BSC_CLASS_01" Type="String" Mapping="STATION_BSC_CLASS_01" />
                    <ext:RecordField Name="STATION_BSC_CLASS_02" Type="String" Mapping="STATION_BSC_CLASS_02" />
                    <ext:RecordField Name="STATION_BSC_CLASS_03" Type="String" Mapping="STATION_BSC_CLASS_03" />
                    <ext:RecordField Name="STATION_BSC_CLASS_04" Type="String" Mapping="STATION_BSC_CLASS_04" />
                </Fields>
            </ext:JsonReader>
        </Reader>
    </ext:Store>
    <ext:Store ID="Store3" runat="server">
        <Reader>
            <ext:JsonReader ReaderID="YEAR">
                <Fields>
                    <ext:RecordField Name="YEAR" />
                </Fields>
            </ext:JsonReader>
        </Reader>
    </ext:Store>
    <ext:Store ID="SDept" runat="server" AutoLoad="true">
        <Proxy>
        </Proxy>
        <Reader>
            <ext:JsonReader Root="deptlist" TotalProperty="totalCount">
                <Fields>
                    <ext:RecordField Name="DEPT_CODE" />
                    <ext:RecordField Name="DEPT_NAME" />
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
                            <ext:GridPanel ID="GridPanel1" runat="server" StoreID="Store1" StripeRows="true"
                                ClicksToEdit="1" TrackMouseOver="true" AutoWidth="true" Height="480" Border="false">
                                <TopBar>
                                    <ext:Toolbar ID="Toolbar_fjsr" runat="server" Visible="true" AutoWidth="true">
                                        <Items>
                                            <ext:ComboBox ID="cbbYear" runat="server" ReadOnly="true" StoreID="Store3" Width="70"
                                                DisplayField="YEAR" ValueField="YEAR" ForceSelection="true" SelectOnFocus="true">
                                                <AjaxEvents>
                                                    <Select OnEvent="Button_look_click">
                                                    </Select>
                                                </AjaxEvents>
                                            </ext:ComboBox>
                                            <ext:Label ID="lYear" runat="server" Text="年">
                                            </ext:Label>
                                            <ext:ToolbarSpacer ID="ToolbarSpacer2" runat="server">
                                            </ext:ToolbarSpacer>
                                            <ext:ToolbarTextItem ID="ToolbarTextItem4" runat="server" Text="科室类别：" />
                                            <ext:ComboBox runat="server" ID="ComboBoxdepttype" Width="140" ListWidth="140">
                                            </ext:ComboBox>
                                            <ext:ToolbarSpacer ID="ToolbarSpacer1" runat="server">
                                            </ext:ToolbarSpacer>
                                            <ext:Label ID="Label3" runat="server" Text="科室：" />
                                            <ext:ComboBox ID="cbbdept" runat="server" StoreID="SDept" DisplayField="DEPT_NAME"
                                                ValueField="DEPT_CODE" TypeAhead="false" LoadingText="Searching..." Width="150"
                                                PageSize="10" ItemSelector="div.search-item" MinChars="1" ListWidth="200">
                                                <Template ID="Template1" runat="server">
                                                    <tpl for=".">
                                                        <div class="search-item">
                                                             <h3>{DEPT_NAME}</h3>
                                                             </div>
                                                      </tpl>                                                                                                       
                                                </Template>
                                            </ext:ComboBox>
                                            <ext:Button ID="Button_look" runat="server" Text="查询" Icon="DatabaseGo">
                                                <AjaxEvents>
                                                    <Click OnEvent="Button_look_click">
                                                        <EventMask Msg="载入中..." ShowMask="true" />
                                                    </Click>
                                                </AjaxEvents>
                                            </ext:Button>
                                            <ext:Button ID="edit" runat="server" Text="编辑" Icon="NoteEdit">
                                                <AjaxEvents>
                                                    <Click OnEvent="Button_edit_click">
                                                        <EventMask Msg="载入中..." ShowMask="true" />
                                                    </Click>
                                                </AjaxEvents>
                                            </ext:Button>
                                            <ext:Button ID="Btn_BatInit" runat="server" Text="批量指标量化" Icon="TagPink">
                                                <AjaxEvents>
                                                    <Click OnEvent="Btn_BatInit_Click">
                                                    </Click>
                                                </AjaxEvents>
                                            </ext:Button>
                                            <ext:Button ID="Button1" runat="server" Text="科室指标设置" Icon="ApplicationOsxLink">
                                                <AjaxEvents>
                                                    <Click OnEvent="Button_Set_click">
                                                        <ExtraParams>
                                                            <ext:Parameter Name="Values" Value="Ext.encode(#{GridPanel1}.getRowsValues())" Mode="Raw" />
                                                        </ExtraParams>
                                                    </Click>
                                                </AjaxEvents>
                                            </ext:Button>
                                            <ext:Button ID="Button2" runat="server" Text="解除关联" Icon="ApplicationOsxLink">
                                                <AjaxEvents>
                                                    <Click OnEvent="Button_unlock_click">
                                                        <Confirmation ConfirmRequest="true" Title="系统提示" Message="确定解除科室与指标集的关联?" />
                                                        <ExtraParams>
                                                        </ExtraParams>
                                                        <EventMask Msg="载入中..." ShowMask="true" />
                                                        <ExtraParams>
                                                            <ext:Parameter Name="Values" Value="Ext.encode(#{GridPanel1}.getRowsValues())" Mode="Raw" />
                                                        </ExtraParams>
                                                    </Click>
                                                </AjaxEvents>
                                            </ext:Button>
                                        </Items>
                                    </ext:Toolbar>
                                </TopBar>
                                <ColumnModel ID="ColumnModel1" runat="server">
                                    <Columns>
                                        <ext:Column ColumnID="DEPT_NAME" Header="<div style='text-align:center;'>科室名称</div>"
                                            Width="120" Align="Left" Sortable="true" DataIndex="DEPT_NAME" MenuDisabled="false" />
                                        <ext:Column ColumnID="DEPT_CODE" Header="<div style='text-align:center;'>科室代码</div>"
                                            Width="90" Align="Left" Sortable="true" DataIndex="DEPT_CODE" MenuDisabled="true" />
                                        <ext:Column ColumnID="GATHER_NAME" Header="<div style='text-align:center;'>指标集名称</div>"
                                            Width="200" Align="Left" Sortable="true" DataIndex="GATHER_NAME" MenuDisabled="true">
                                        </ext:Column>
                                        <ext:Column ColumnID="GATHER_CODE" Header="<div style='text-align:center;'>指标集代码</div>"
                                            Width="200" Align="Left" Sortable="true" DataIndex="GATHER_CODE" MenuDisabled="true"
                                            Hidden="true">
                                        </ext:Column>
                                        <ext:Column ColumnID="STATION_BSC_CLASS_01" Header="<div style='text-align:center;'>内部管理总分</div>"
                                            Width="100" Align="Right" Sortable="true" DataIndex="STATION_BSC_CLASS_01" MenuDisabled="true">
                                        </ext:Column>
                                        <ext:Column ColumnID="STATION_BSC_CLASS_02" Header="<div style='text-align:center;'>经济财务总分</div>"
                                            Width="100" Align="Right" Sortable="true" DataIndex="STATION_BSC_CLASS_02" MenuDisabled="true">
                                        </ext:Column>
                                        <ext:Column ColumnID="STATION_BSC_CLASS_03" Header="<div style='text-align:center;'>客户满意度总分</div>"
                                            Width="100" Align="Right" Sortable="true" DataIndex="STATION_BSC_CLASS_03" MenuDisabled="true">
                                        </ext:Column>
                                        <ext:Column ColumnID="STATION_BSC_CLASS_04" Header="<div style='text-align:center;'>学习与成长总分</div>"
                                            Width="100" Align="Right" Sortable="true" DataIndex="STATION_BSC_CLASS_04" MenuDisabled="true">
                                        </ext:Column>
                                    </Columns>
                                </ColumnModel>
                                <Plugins>
                                    <ext:GridFilters ID="GridFilters1" runat="server" Local="true" FiltersText="过滤" ShowMenu="true">
                                        <Filters>
                                            <ext:StringFilter DataIndex="DEPT_NAME" />
                                        </Filters>
                                    </ext:GridFilters>
                                </Plugins>
                                <SelectionModel>
                                    <ext:RowSelectionModel ID="RowSelectionModel1" runat="server" SingleSelect="true">
                                    </ext:RowSelectionModel>
                                </SelectionModel>
                            </ext:GridPanel>
                        </ext:LayoutColumn>
                    </Columns>
                </ext:ColumnLayout>
            </Body>
        </ext:ViewPort>
        <ext:Window ID="DeptGuide_Edit" runat="server" Icon="Group" Title="科室指标设置" Width="320"
            Height="280" AutoShow="false" Modal="true" CenterOnLoad="true" AutoScroll="true"
            ShowOnLoad="false" Resizable="true" StyleSpec="background-color:Transparent;"
            BodyStyle="background-color:Transparent;">
        </ext:Window>
        <ext:Window ID="DeptGuideSet" runat="server" Icon="Group" Title="科室指标设置" Width="600"
            Height="480" AutoShow="false" Modal="false" CenterOnLoad="true" AutoScroll="true" 
            ShowOnLoad="false" Resizable="true" StyleSpec="background-color:Transparent;"
            BodyStyle="background-color:Transparent;">
        </ext:Window>
        <ext:Window runat="server" ID="Win_BatchInit" AutoShow="false" ShowOnLoad="false"
            Modal="true" Resizable="false" Title="年度批量指标量化" CenterOnLoad="true" AutoScroll="false"
            Width="280" Height="180" CloseAction="Hide" AnimateTarget="Btn_BatInit" Icon="TagPink"
            BodyStyle="padding:2px;">
            <Body>
                <table>
                    <tr>
                        <td colspan="2" align="left">
                            <p>
                                注意：生成数据需要大约2分钟时间，在此时间内请不要关闭您的浏览器或者刷新页面。</p>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            目标值参照年份:
                        </td>
                        <td align="left">
                            <ext:ComboBox runat="server" ID="Combo_TargetYear" FieldLabel="目标值参照年份" Width="90"
                                AllowBlank="false" Editable="false">
                            </ext:ComboBox>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <p>
                                <ext:Label runat="server" ID="progressTip" Text="进度" AutoWidth="true">
                                </ext:Label>
                            </p>
                            <ext:ProgressBar ID="Progress1" runat="server" Width="255">
                            </ext:ProgressBar>
                        </td>
                    </tr>
                </table>
                <ext:TaskManager ID="TaskManager1" runat="server">
                    <Tasks>
                        <ext:Task TaskID="longactionprogress" Interval="1000" AutoRun="false" OnStart="#{Btn_BatStart}.setDisabled(true); "
                            OnStop="#{Btn_BatStart}.setDisabled(false);">
                            <AjaxEvents>
                                <Update OnEvent="RefreshProgress" />
                            </AjaxEvents>
                        </ext:Task>
                    </Tasks>
                </ext:TaskManager>
            </Body>
            <BottomBar>
                <ext:Toolbar ID="Toolbar1" runat="server">
                    <Items>
                        <ext:ToolbarFill ID="ToolbarFill2" runat="server" />
                        <ext:ToolbarButton ID="Btn_BatStart" runat="server" Icon="PlayGreen" Text="开始生成">
                            <AjaxEvents>
                                <Click OnEvent="Btn_BatStart_Click" Timeout="1200000">
                                </Click>
                            </AjaxEvents>
                        </ext:ToolbarButton>
                        <ext:ToolbarSeparator ID="ToolbarSeparator3" runat="server" />
                        <ext:ToolbarButton ID="Btn_BatCancel" runat="server" Icon="Cancel" Text="退出">
                            <Listeners>
                                <Click Handler="Win_BatchInit.hide();" />
                            </Listeners>
                        </ext:ToolbarButton>
                    </Items>
                </ext:Toolbar>
            </BottomBar>
            <Listeners>
                <Show Handler="this.dirty = false;" />
                <BeforeHide Handler="
                    if ((this.dirty==false)&&(Btn_BatCancel.text=='取消') ){
                        Ext.Msg.confirm('系统提示', '注意:任务正在运行，确定取消任务并退出吗？', function (btn) { 
                            if(btn == 'yes') { 
                                this.dirty = true;
                                this.hide(); 
                            } 
                        }, this);
                        return false;    
                    }" />
                <Hide Handler="TaskManager1.stopAll();" />
            </Listeners>
            <AjaxEvents>
                <Hide OnEvent="CloseBatInit" />
            </AjaxEvents>
        </ext:Window>
    </div>
    </form>
</body>
</html>
