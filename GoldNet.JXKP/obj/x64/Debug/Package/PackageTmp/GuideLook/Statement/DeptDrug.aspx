<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DeptDrug.aspx.cs" Inherits="GoldNet.JXKP.GuideLook.Statement.DeptDrug" %>

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
           var ViewDeptBenefitInfo = function(grid,rowIndex,colIndex) {
                ColIndex.value = colIndex;
           }
        var viewDetail = function(url) {
            arcEditDetailWindow.show();
            arcEditDetailWindow.clearContent();
            arcEditDetailWindow.loadContent({
            url: url,
                mode: "iframe",
                showMask: true,
                maskMsg: "载入中..."
            });
        }
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
    <ext:Hidden ID="ColIndex" runat="server">
    </ext:Hidden>
    <ext:Store ID="Store1" runat="server" OnRefreshData="Data_RefreshData">
        <Reader>
            <ext:JsonReader>
                <Fields>
                    <ext:RecordField Name="DEPT_CODE" />
                    <ext:RecordField Name="DEPT_NAME" />
                    <ext:RecordField Name="ZJ" SortType="AsFloat" />
                    <ext:RecordField Name="KSS" SortType="AsFloat" />
                    <ext:RecordField Name="XYXJ" SortType="AsFloat" />
                    <ext:RecordField Name="XYDF" SortType="AsFloat" />
                    <ext:RecordField Name="XYJM" SortType="AsFloat" />
                    <ext:RecordField Name="ZYXJ" SortType="AsFloat" />
                    <ext:RecordField Name="ZYDF" SortType="AsFloat" />
                    <ext:RecordField Name="ZYJM" SortType="AsFloat" />
                    <ext:RecordField Name="ZKYYXJ" SortType="AsFloat" />
                    <ext:RecordField Name="ZKYYDF" SortType="AsFloat" />
                    <ext:RecordField Name="ZKYYJM" SortType="AsFloat" />
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
    <div>
        <ext:ViewPort ID="ViewPort1" runat="server">
            <Body>
                <ext:ColumnLayout ID="ColumnLayout1" runat="server" Split="true" FitHeight="true">
                    <Columns>
                        <ext:LayoutColumn ColumnWidth="1">
                            <ext:Panel runat="server" ID="p11" AutoScroll="true" Border="false">
                                <TopBar>
                                    <ext:Toolbar ID="Toolbar_detptype" runat="server">
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
                                            <ext:Label ID="Label3" runat="server" Text="科室：">
                                            </ext:Label>
                                            <ext:ComboBox ID="DeptCodeCombo" runat="server" StoreID="Store3" DisplayField="DEPT_NAME"
                                                Width="120" ValueField="DEPT_CODE" TypeAhead="false" LoadingText="Searching..."
                                                PageSize="1000" ItemSelector="div.search-item" MinChars="1" FieldLabel="科室信息"
                                                ListWidth="240">
                                                <Template ID="Template1" runat="server">
                                                   <tpl for=".">
                                                      <div class="search-item">
                                                         <h3><span style="width:auto">{DEPT_CODE}</span>{DEPT_NAME}</h3>
                                                      </div>
                                                   </tpl>
                                                </Template>
                                            </ext:ComboBox>
                                            <ext:Label ID="Label1" runat="server" Text="类别：">
                                            </ext:Label>
                                            <ext:ComboBox runat="server" ID="cboType" Width="60" ListWidth="60">
                                                <Items>
                                                    <ext:ListItem Text="全部" Value="qb" />
                                                    <ext:ListItem Text="门诊" Value="mz" />
                                                    <ext:ListItem Text="住院" Value="zy" />
                                                </Items>
                                            </ext:ComboBox>
                                            <ext:Button ID="btnSearch" runat="server" Text="查询" Icon="DatabaseGo">
                                                <Listeners>
                                                    <Click Handler="#{Store1}.reload();" />
                                                </Listeners>
                                            </ext:Button>
                                            <ext:ToolbarFill>
                                            </ext:ToolbarFill>
                                            <ext:Button ID="btnExcel" runat="server" Text=" 导出Excel " Icon="PageWhiteExcel" OnClick="OutExcel"
                                                AutoPostBack="true" Disabled="true">
                                            </ext:Button>
                                        </Items>
                                    </ext:Toolbar>
                                </TopBar>
                                <Body>
                                    <center>
                                        <h2>
                                            用药管理
                                        </h2>
                                    </center>
                                    <ext:ExtGridPanel ID="GridPanel_Show" runat="server" StoreID="Store1" Width="800"
                                        Height="400" AutoScroll="true" StyleSpec="margin:10px" Border="true">
                                        <ExtColumnModel ID="ColumnModel1" runat="server">
                                            <Columns>
                                                <ext:ExtColumn ColumnID="DEPT_NAME" Header="科室" Sortable="true" DataIndex="DEPT_NAME" />
                                                <ext:ExtColumn ColumnID="DEPT_CODE" Header="本科" Sortable="true" DataIndex="DEPT_CODE"
                                                    Hidden="true" />
                                                <ext:ExtColumn ColumnID="ZJ" Header="药品总收入" Sortable="true" DataIndex="ZJ">
                                                    <Renderer Fn="rmbMoney" />
                                                </ext:ExtColumn>
                                                <ext:ExtColumn ColumnID="KSS" Header="抗生素" Sortable="true" DataIndex="KSS">
                                                    <Renderer Fn="rmbMoney" />
                                                </ext:ExtColumn>
                                                <ext:ExtColumn ColumnID="XYXJ" Header="小计" Sortable="true" DataIndex="XYXJ">
                                                    <Renderer Fn="rmbMoney" />
                                                </ext:ExtColumn>
                                                <ext:ExtColumn ColumnID="XYDF" Header="地方" Sortable="true" DataIndex="XYDF">
                                                    <Renderer Fn="rmbMoney" />
                                                </ext:ExtColumn>
                                                <ext:ExtColumn ColumnID="XYJM" Header="军免" Sortable="true" DataIndex="XYJM">
                                                    <Renderer Fn="rmbMoney" />
                                                </ext:ExtColumn>
                                                <ext:ExtColumn ColumnID="ZYXJ" Header="小计" Sortable="true" DataIndex="ZYXJ">
                                                    <Renderer Fn="rmbMoney" />
                                                </ext:ExtColumn>
                                                <ext:ExtColumn ColumnID="ZYDF" Header="地方" Sortable="true" DataIndex="ZYDF">
                                                    <Renderer Fn="rmbMoney" />
                                                </ext:ExtColumn>
                                                <ext:ExtColumn ColumnID="ZYJM" Header="军免" Sortable="true" DataIndex="ZYJM">
                                                    <Renderer Fn="rmbMoney" />
                                                </ext:ExtColumn>
                                                <ext:ExtColumn ColumnID="ZKYYXJ" Header="小计" Sortable="true" DataIndex="ZKYYXJ">
                                                    <Renderer Fn="rmbMoney" />
                                                </ext:ExtColumn>
                                                <ext:ExtColumn ColumnID="ZKYYDF" Header="地方" Sortable="true" DataIndex="ZKYYDF">
                                                    <Renderer Fn="rmbMoney" />
                                                </ext:ExtColumn>
                                                <ext:ExtColumn ColumnID="ZKYYJM" Header="军免" Sortable="true" DataIndex="ZKYYJM">
                                                    <Renderer Fn="rmbMoney" />
                                                </ext:ExtColumn>
                                            </Columns>
                                            <HeadRows>
                                                <ext:ExtRows>
                                                    <Rows>
                                                        <ext:ExtRow Header="" ColSpan="4" Align="Center" />
                                                        <ext:ExtRow Header="西药" ColSpan="3" Align="Center" />
                                                        <ext:ExtRow Header="中药" ColSpan="3" Align="Center" />
                                                        <ext:ExtRow Header="专科用药" ColSpan="3" Align="Center" />
                                                    </Rows>
                                                </ext:ExtRows>
                                            </HeadRows>
                                        </ExtColumnModel>
                                        <Plugins>
                                            <ext:ExtGroupHeaderGrid ID="ExtGroupHeaderGrid2" runat="server">
                                            </ext:ExtGroupHeaderGrid>
                                        </Plugins>
                                        <SelectionModel>
                                            <ext:RowSelectionModel SingleSelect="true" ID="selectRow">
                                            </ext:RowSelectionModel>
                                        </SelectionModel>
                                        <Listeners>
                                            <BeforeRender Handler="Ext.EventManager.onWindowResize(function(){ if(Ext.getBody().getViewSize().width>850){this.setWidth( Ext.getBody().getViewSize().width -20);}; this.setHeight( Ext.getBody().getViewSize().height -130); }, this);" />
                                            <Render Handler="if(Ext.getBody().getViewSize().width>850){this.setWidth( Ext.getBody().getViewSize().width -20);}; this.setHeight( Ext.getBody().getViewSize().height -130);" />
                                            <CellClick Handler="ViewDeptBenefitInfo(this,rowIndex,columnIndex)" />
                                        </Listeners>
                                        <AjaxEvents>
                                            <DblClick OnEvent="QueryPerDrug">
                                                <ExtraParams>
                                                    <ext:Parameter Name="value" Value="Ext.encode(#{GridPanel_Show}.getRowsValues())"
                                                        Mode="Raw">
                                                    </ext:Parameter>
                                                    <ext:Parameter Name="col" Value="#{ColIndex}.value" Mode="Raw">
                                                    </ext:Parameter>
                                                </ExtraParams>
                                            </DblClick>
                                        </AjaxEvents>
                                        <LoadMask ShowMask="true" Msg="查询中....." />
                                    </ext:ExtGridPanel>
                                </Body>
                            </ext:Panel>
                        </ext:LayoutColumn>
                    </Columns>
                </ext:ColumnLayout>
            </Body>
        </ext:ViewPort>
    </div>
    <ext:Window ID="arcEditWindow" runat="server" Icon="Group" Width="580" Height="384"
        AutoShow="false" Modal="true" CenterOnLoad="true" AutoScroll="true" ShowOnLoad="false"
        Resizable="true" StyleSpec="background-color:Transparent;" BodyStyle="background-color:Transparent;"
        Maximizable="true">
    </ext:Window>
    </form>
</body>
</html>
