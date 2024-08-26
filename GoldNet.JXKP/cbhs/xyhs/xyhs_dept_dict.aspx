<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="xyhs_dept_dict.aspx.cs"
    Inherits="GoldNet.JXKP.cbhs.xyhs.xyhs_dept_dict" %>

<%@ Register Assembly="Goldnet.Ext.Web" Namespace="Goldnet.Ext.Web" TagPrefix="ext" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
      <script language="javascript" type="text/javascript">
      
        var RowIndex;
        var RefreshData = function() {
            Store1.reload();
        }
        function FormatRender(v, p, record, rowIndex) {
            var colors = ["red", "black"];
            if(record.data.ACCOUNT_DEPT_NAME=="")
            {
            var template = '<span style="color:{0};">{1}</span>';
            return String.format(template, colors[0], record.data.DEPT_NAME);
            }
            else
            {
            var templateb = '<span style="color:{0};">{1}</span>';
            return String.format(templateb, colors[1], record.data.DEPT_NAME);
            }
        }
          function selectOrderedProg(cc)
        {
            var record2 = Store2.getAt(RowIndex);
            record2.data['DEPT_CODE'] = cc;
            GridPanel2.getView().focusRow(RowIndex);
        };
        </script>
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
                    <ext:RecordField Name="DEPT_TYPE" Type="String" Mapping="DEPT_TYPE" />
                    <ext:RecordField Name="ATTR" Type="String" Mapping="ATTR" />
                    <ext:RecordField Name="INPUT_CODE" Type="String" Mapping="INPUT_CODE" />
                    <ext:RecordField Name="ACCOUNT_DEPT_CODE" Type="String" Mapping="ACCOUNT_DEPT_CODE" />
                    <ext:RecordField Name="ACCOUNT_DEPT_NAME" Type="String" Mapping="ACCOUNT_DEPT_NAME" />
                    <ext:RecordField Name="SORT_NO" Type="Float" Mapping="SORT_NO" />
                    <ext:RecordField Name="SHOW_FLAG" Type="String" Mapping="SHOW_FLAG" />
                </Fields>
            </ext:JsonReader>
        </Reader>
    </ext:Store>
     <ext:Store ID="Store2" runat="server" >
        <Reader>
            <ext:JsonReader>
                <Fields>
                    <ext:RecordField Name="DEPT_CODE" Type="String" Mapping="DEPT_CODE" />
                    <ext:RecordField Name="DEPT_NAME" Type="String" Mapping="DEPT_NAME" />
                   
                </Fields>
            </ext:JsonReader>
        </Reader>
    </ext:Store>
    <ext:Store ID="Store3" runat="server" AutoLoad="true">
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
    <ext:ViewPort ID="ViewPort1" runat="server">
            <Body>
                <ext:BorderLayout ID="BorderLayout1" runat="server">
                    <Center>
                        <ext:Panel ID="Panel2" runat="server" BodyBorder="true" Border="false">
                            <Body>
            <ext:ColumnLayout ID="ColumnLayout1" runat="server" Split="true" FitHeight="true">
                <Columns>
                    <ext:LayoutColumn ColumnWidth="1">
                        <ext:GridPanel ID="GridPanel1" runat="server" StoreID="Store1" StripeRows="true"
                            TrackMouseOver="true" AutoWidth="true" Height="480" Border="false">
                            <TopBar>
                                <ext:Toolbar ID="Toolbar_fjsr" runat="server" Visible="true" AutoWidth="true">
                                    <Items>
                                        <ext:Label ID="func" runat="server" Text="科室类别：" Width="40">
                                        </ext:Label>
                                        <ext:ComboBox ID="Combo_DeptType" runat="server" AllowBlank="true" EmptyText="请选择类别"
                                            Width="100" >
                                            <AjaxEvents>
                                                <Select OnEvent="SelectedDepttype">
                                                    <EventMask ShowMask="true" />
                                                </Select>
                                            </AjaxEvents>
                                        </ext:ComboBox>
                                        <ext:ComboBox ID="ComShowflag" runat="server" AllowBlank="false" Width="100" EmptyText="选择是否停用"
                                            FieldLabel="是否启用" SelectedIndex="0">
                                            <Items>
                                                <ext:ListItem Text="启用" Value="0" />
                                                <ext:ListItem Text="停用" Value="1" />
                                            </Items>
                                            <AjaxEvents>
                                                <Select OnEvent="SelectedDepttype">
                                                    <EventMask ShowMask="true" />
                                                </Select>
                                            </AjaxEvents>
                                        </ext:ComboBox>
                                        <ext:Button ID="Button_look" runat="server" Text="查询" Icon="DatabaseGo">
                                            <AjaxEvents>
                                                <Click OnEvent="Button_look_click">
                                                    <EventMask Msg="载入中..." ShowMask="true" />
                                                </Click>
                                            </AjaxEvents>
                                        </ext:Button>
                                          <ext:Button ID="Button2" runat="server" Text="添加" Icon="NoteEdit">
                                            <AjaxEvents>
                                                <Click OnEvent="Button_add_click">
                                                    <EventMask Msg="载入中..." ShowMask="true" />
                                                </Click>
                                            </AjaxEvents>
                                        </ext:Button>
                                        <ext:ToolbarSeparator ID="ToolbarSeparator2" runat="server" />
                                        <ext:Button ID="Button_edit" runat="server" Text="设置" Icon="NoteEdit" Disabled="true">
                                            <AjaxEvents>
                                                <Click OnEvent="Button_edit_click">
                                                    <EventMask Msg="载入中..." ShowMask="true" />
                                                </Click>
                                            </AjaxEvents>
                                        </ext:Button>
                                        <ext:ToolbarSeparator ID="ToolbarSeparator3" runat="server" />
                                        <ext:Button ID="Button_del" runat="server" Text="删除" Icon="Delete" Disabled="true">
                                            <AjaxEvents>
                                                <Click OnEvent="Button_del_click">
                                                    <Confirmation ConfirmRequest="true" Title="系统提示" Message="将删除选中数据,<br/>是否继续?" />
                                                    <EventMask Msg="载入中..." ShowMask="true" />
                                                </Click>
                                            </AjaxEvents>
                                        </ext:Button>
                                        
                                       
                                    </Items>
                                </ext:Toolbar>
                            </TopBar>
                            <ColumnModel ID="ColumnModel1" runat="server">
                                <Columns>
                                    <ext:RowNumbererColumn Width="32" Resizable="true"/>
                                    <ext:Column ColumnID="DEPT_CODE" Header="<div style='text-align:center;'>科室编号</div>" Width="80" Align="left" Sortable="true"
                                        DataIndex="DEPT_CODE" MenuDisabled="true" />
                                    <ext:Column ColumnID="DEPT_NAME" Header="<div style='text-align:center;'>科室名称</div>" Width="120" Align="left" Sortable="true"
                                        DataIndex="DEPT_NAME" MenuDisabled="true" >
                                        <Renderer Fn="FormatRender" />
                                    </ext:Column>
                                    <ext:Column ColumnID="DEPT_TYPE" Header="<div style='text-align:center;'>科室类别</div>" Width="120" Align="left" Sortable="true"
                                        DataIndex="DEPT_TYPE" MenuDisabled="true" />
                                    <ext:Column ColumnID="ACCOUNT_DEPT_NAME" Header="<div style='text-align:center;'>中心科室</div>" Width="120" Align="left" Sortable="true"
                                        DataIndex="ACCOUNT_DEPT_NAME" MenuDisabled="true" />
                                    <ext:Column ColumnID="ATTR" Header="<div style='text-align:center;'>是否是核算</div>" Width="120" Align="left" Sortable="true"
                                        DataIndex="ATTR" MenuDisabled="true" />
                                    <ext:Column ColumnID="SORT_NO" Header="<div style='text-align:center;'>排列顺序</div>" Width="120" Align="left" Sortable="true"
                                        DataIndex="SORT_NO" MenuDisabled="true" />
                                    <ext:Column ColumnID="INPUT_CODE" Header="<div style='text-align:center;'>输入码</div>" Width="120" Align="left" Sortable="true"
                                        DataIndex="INPUT_CODE" MenuDisabled="true" />
                                </Columns>
                            </ColumnModel>
                            <LoadMask ShowMask="true" />
                            <BottomBar>
                                <ext:Toolbar ID="Toolbar4" runat="server" Height="26">
                                    <Items>
                                        <ext:TextField ID="txt_SearchTxt" runat="server" EmptyText="查找信息">
                                            <ToolTips>
                                                <ext:ToolTip ID="ToolTip1" runat="server" Html="根据科室编码，科室名称，输入码模糊查找">
                                                </ext:ToolTip>
                                            </ToolTips>
                                        </ext:TextField>
                                        <ext:Button ID="btn_Search" Icon="Zoom" runat="server" Text="查询">
                                            <AjaxEvents>
                                                <Click OnEvent="select_dept">
                                                    <EventMask Msg="载入中..." ShowMask="true" />
                                                </Click>
                                            </AjaxEvents>
                                        </ext:Button>
                                        <ext:ToolbarFill>
                                        </ext:ToolbarFill>
                                    </Items>
                                </ext:Toolbar>
                            </BottomBar>
                                <SelectionModel>
                                    <ext:RowSelectionModel ID="rowselection" SingleSelect="true">
                                    <Listeners>
                                      <RowSelect Handler="RowIndex = rowIndex" />
                                                            <RowDeselect Handler="RowIndex = rowIndex" />
                                        <SelectionChange Handler="var tmpfl=#{GridPanel1}.hasSelection()?false:true; #{Button_edit}.setDisabled(tmpfl);#{Button_del}.setDisabled(tmpfl);" />
                                    </Listeners>
                                   
                                    </ext:RowSelectionModel>
                                </SelectionModel>
                                <AjaxEvents>
                                    <%--<DblClick OnEvent="Button_edit_click" />--%>
                                   
                                    <Click OnEvent="select_hisdept">
                                        <ExtraParams>
                                            <ext:Parameter Name="Values" Value="Ext.encode(#{GridPanel1}.getRowsValues())" Mode="Raw" />
                                        </ExtraParams>
                                    </Click>
                          
                                </AjaxEvents>
                        </ext:GridPanel>
                    </ext:LayoutColumn>
                </Columns>
          </ext:ColumnLayout>
                            </Body>
                        </ext:Panel>
                    </Center>
      <East MinWidth="200" MaxWidth="400" SplitTip="对照HIS科室" Collapsible="true" Split="true">
                        <ext:Panel ID="Panel1" runat="server" Border="false" Width="350" Title="对照HIS科室"
                            Collapsed="false" AutoScroll="true">
                            <TopBar>
                                <ext:Toolbar ID="Toolbar1" runat="server">
                                    <Items>
                                      
                                        <ext:Button ID="Button1" runat="server" Text="保存" Icon="Disk" >
                                            <AjaxEvents>
                                                <Click OnEvent="Button_save_click">
                                                   
                                                    <ExtraParams>
                                                    </ExtraParams>
                                                    <EventMask Msg="载入中..." ShowMask="true" />
                                                    <ExtraParams>
                                                        <ext:Parameter Name="Valuess" Value="Ext.encode(#{GridPanel2}.getRowsValues())" Mode="Raw">
                                                        </ext:Parameter>
                                                    </ExtraParams>
                                                </Click>
                                            </AjaxEvents>
                                        </ext:Button>
                                      
                                        <ext:Button ID="Button_no" runat="server" Text="删除" Icon="Disk" >
                                            <AjaxEvents>
                                                <Click OnEvent="Button_delto_click">
                                                    <Confirmation ConfirmRequest="true" Title="系统提示" Message="将取消选中的成本,<br/>是否继续?" />
                                                    <ExtraParams>
                                                    </ExtraParams>
                                                    <EventMask Msg="载入中..." ShowMask="true" />
                                                    <ExtraParams>
                                                        <ext:Parameter Name="Valuess" Value="Ext.encode(#{GridPanel2}.getRowsValues())" Mode="Raw">
                                                        </ext:Parameter>
                                                    </ExtraParams>
                                                </Click>
                                            </AjaxEvents>
                                        </ext:Button>
                                    </Items>
                                </ext:Toolbar>
                            </TopBar>
                            
                            <Body>
                                <ext:ColumnLayout ID="ColumnLayout2" runat="server" Split="true">
                                    <Columns>
                                        <ext:LayoutColumn ColumnWidth="1">
                                            <ext:GridPanel ID="GridPanel2" runat="server" Border="false" StoreID="Store2" StripeRows="true"
                                                AutoHeight="true" AutoWidth="true" TrackMouseOver="true" AutoScroll="true">
                                                <ColumnModel ID="ColumnModel2" runat="server">
                                                    <Columns>
                   
                                                       <ext:Column ColumnID="DEPT_NAME" Header="<div style='text-align:center;'>HIS科室名称</div>"
                                                            Width="130" Align="left" DataIndex="DEPT_NAME" MenuDisabled="true">
                                                            <Editor>
                                                                <ext:ComboBox ID="ComboBox1" runat="server" StoreID="Store3" DisplayField="DEPT_CODE"
                                                                    AllowBlank="true" ValueField="DEPT_NAME" TypeAhead="false" LoadingText="Searching..."
                                                                    Width="220" ListWidth="220" PageSize="10" ItemSelector="div.search-item" MinChars="1">
                                                                    <Template ID="Template1" runat="server">
                                                                  <tpl for=".">
                                                                   <div class="search-item">
                                                                     <h3><span>{DEPT_NAME}</span>{DEPT_CODE}</h3>
                                                                   </div>
                                                                  </tpl>
                                                                    </Template>
                                                                    <Listeners>
                                                                        <Select Handler="selectOrderedProg(this.getText());" />
                                                                    </Listeners>
                                                                </ext:ComboBox>
                                                            </Editor>
                                                        </ext:Column>
                                                        
                                                             <ext:Column ColumnID="DEPT_CODE" Header="<div style='text-align:center;'>HIS科室代码</div>" Width="100"
                                                            Align="left" Sortable="false" DataIndex="DEPT_CODE" MenuDisabled="true">
                                                           
                                                        </ext:Column>
                                                    </Columns>
                                                </ColumnModel>
                                                <SelectionModel>
                                                    <ext:CheckboxSelectionModel ID="RowSelectionModel2" runat="server">
                                                     <Listeners>
                                                            <RowSelect Handler="RowIndex = rowIndex" />
                                                            <RowDeselect Handler="RowIndex = rowIndex" />
                                                           
                                                        </Listeners>
                                                    </ext:CheckboxSelectionModel>
                                                </SelectionModel>
                                                 <Listeners>
                                                    <KeyDown Handler="if (e.getKey() == 40){ #{GridPanel2}.insertRecord(0, {});#{GridPanel2}.getView().focusRow(0);#{GridPanel2}.startEditing(0, 0);} ;" />
                                                </Listeners>
                                                <LoadMask ShowMask="true" />
                                            </ext:GridPanel>
                                        </ext:LayoutColumn>
                                    </Columns>
                                </ext:ColumnLayout>
                            </Body>
                        </ext:Panel>
                    </East>
                </ext:BorderLayout>
            </Body>
        </ext:ViewPort>
    <ext:Window ID="DeptSetWin" runat="server" Icon="Group" Title="科室设置" Width="370" Height="380"
        AutoShow="false" Modal="true" CenterOnLoad="true" AutoScroll="true" ShowOnLoad="false"
        Resizable="true" StyleSpec="background-color:Transparent;" BodyStyle="background-color:Transparent;">
    </ext:Window>
    </form>
</body>
</html>
