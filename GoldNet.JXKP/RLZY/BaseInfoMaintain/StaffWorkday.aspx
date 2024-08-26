<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="StaffWorkday.aspx.cs" Inherits="GoldNet.JXKP.RLZY.BaseInfoMaintain.StaffWorkday" %>

<%@ Register Assembly="Goldnet.Ext.Web" Namespace="Goldnet.Ext.Web" TagPrefix="ext" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
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
    <link rel="stylesheet" type="text/css" href="../../Bonus/Orthers/Cbouns.css" />

    <script language="javascript" type="text/javascript">
        
        var RowIndex;
        
        //列表刷新
        var RefreshData = function() {
            Store1.reload();
        };
        
        //列表单元格格式化（金额单元）
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
        
        //列表单元格
        function FormatRender(v, p, record, rowIndex) {
            var colors = ["red", "black","blue"];
            if(record.data.FLAG=="1")
            {
                var template = '<span style="color:{0};font-weight:bold;">{1}</span>';
                return String.format(template, colors[0], record.data.DEPT_NAME);
            }
            else
            {
                var templateb = '<span style="color:{0};">{1}</span>';
                return String.format(templateb, colors[1], record.data.DEPT_NAME);
            }
        };
        
        //科室代码赋值
        function selectOrderedDept(cc)
        {
            var record2 = Store1.getAt(RowIndex);
            record2.data['DEPT_CODE'] = cc;
            GridPanel1.getView().focusRow(RowIndex);
        };
        
        //人员代码赋值
        function selectOrderedPerson(cc)
        {
            var record2 = Store1.getAt(RowIndex);
            record2.data['STAFF_ID'] = cc;
            GridPanel1.getView().focusRow(RowIndex);
        };
        
        //考勤项目赋值
        function selectOrderedItem(cc)
        {
            var record2 = Store1.getAt(RowIndex);
            record2.data['ATTENDANCE_CODE'] = cc;
            
             if(record2.data.QJTS > 0)
             {
                alert(record2.data.STAFF_NAME+"已累计休假"+record2.data.QJTS+"天，请考勤员注意！");
             }
                       
            GridPanel1.getView().focusRow(RowIndex);
        };
        
        //科室调动
        function TreeOpration() {
            cboChangeDept.setValue("");
            staffName.setValue(this.Store1.getAt(RowIndex).get('STAFF_NAME'));
            arcEditWindow.show();
        };
        
        //检查变更科室数据
        var CheckForm = function() {
            if (cboChangeDept.validate() == false) {
                return false;
            }
            if (staffName.validate() == false) {
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
                <ext:JsonReader ReaderID="ROW_ID">
                    <Fields>
                        <ext:RecordField Name="ROW_ID" Type="String" Mapping="ROW_ID" />
                        <ext:RecordField Name="DEPT_CODE" Type="String" Mapping="DEPT_CODE" />
                        <ext:RecordField Name="DEPT_NAME" Type="String" Mapping="DEPT_NAME" />
                        <ext:RecordField Name="EMP_NO" Type="String" Mapping="EMP_NO" />
                        <ext:RecordField Name="STAFF_ID" Type="String" Mapping="STAFF_ID" />
                        <ext:RecordField Name="STAFF_NAME" Type="String" Mapping="STAFF_NAME" />
                        <ext:RecordField Name="ATTENDANCE_CODE" Type="String" Mapping="ATTENDANCE_CODE" />
                        <ext:RecordField Name="ATTENDANCE_VALUE" Type="Float" Mapping="ATTENDANCE_VALUE" />
                        <ext:RecordField Name="MEMO" Type="String" Mapping="MEMO" />
                        <ext:RecordField Name="REPORTER" Type="String" Mapping="REPORTER" />
                        <ext:RecordField Name="ATTENDANCE_NAME" Type="String" Mapping="ATTENDANCE_NAME" />
                        <ext:RecordField Name="YEAR_MONTH" Type="String" Mapping="YEAR_MONTH" />
                        <ext:RecordField Name="CHECK_TAG" Type="String" Mapping="CHECK_TAG" />
                        <ext:RecordField Name="EDIT_TAG" Type="String" Mapping="EDIT_TAG" />
                        <ext:RecordField Name="JZ" Type="Float" Mapping="JZ" />
                        <ext:RecordField Name="QJTS" Type="Float" Mapping="QJTS" />
                    </Fields>
                </ext:JsonReader>
            </Reader>
        </ext:Store>
        <ext:Store ID="Store2" runat="server" AutoLoad="true">
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
        <ext:Store ID="Store3" runat="server" AutoLoad="true">
            <Proxy>
            </Proxy>
            <Reader>
                <ext:JsonReader Root="StaffInfos" TotalProperty="totalCount">
                    <Fields>
                        <ext:RecordField Name="STAFF_ID" />
                        <ext:RecordField Name="STAFF_NAME" />
                        <ext:RecordField Name="DEPT_NAME" />
                    </Fields>
                </ext:JsonReader>
            </Reader>
        </ext:Store>
        <ext:Store ID="Store4" runat="server" AutoLoad="true">
            <Proxy>
            </Proxy>
            <Reader>
                <ext:JsonReader>
                    <Fields>
                        <ext:RecordField Name="ATTENDANCE_CODE" />
                        <ext:RecordField Name="ATTENDANCE_NAME" />
                    </Fields>
                </ext:JsonReader>
            </Reader>
        </ext:Store>
        <ext:Store ID="Store5" runat="server" AutoLoad="true">
            <Proxy>
            </Proxy>
            <Reader>
                <ext:JsonReader>
                    <Fields>
                        <ext:RecordField Name="MEMO" />
                    </Fields>
                </ext:JsonReader>
            </Reader>
        </ext:Store>
        <ext:Store ID="Store6" runat="server" AutoLoad="true">
            <Proxy>
                <ext:HttpProxy Method="POST" Url="/RLZY/WebService/DeptAccount.ashx" />
            </Proxy>
            <Reader>
                <ext:JsonReader Root="Staffdepts">
                    <Fields>
                        <ext:RecordField Name="DEPT_NAME" />
                        <ext:RecordField Name="DEPT_CODE" />
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
                                                ClicksToEdit="1" TrackMouseOver="true" AutoWidth="true" Height="480" Border="true">
                                                <TopBar>
                                                    <ext:Toolbar ID="Toolbar_fjsr" runat="server" Visible="true" AutoWidth="true">
                                                        <Items>
                                                            <ext:Label ID="lcaption" runat="server" Text="年月：">
                                                            </ext:Label>
                                                            <ext:ComboBox ID="cbbYear" runat="server" ReadOnly="true" StoreID="SYear" Width="60"
                                                                DisplayField="YEAR" ValueField="YEAR" ForceSelection="true" SelectOnFocus="true">
                                                            </ext:ComboBox>
                                                            <ext:Label ID="lYear" runat="server" Text="年">
                                                            </ext:Label>
                                                            <ext:ComboBox ID="cbbmonth" runat="server" ReadOnly="true" StoreID="SMonth" Width="50"
                                                                DisplayField="MONTH" ValueField="MONTH">
                                                            </ext:ComboBox>
                                                            <ext:Label ID="lmonth" runat="server" Text="月">
                                                            </ext:Label>
                                                            <ext:ToolbarSeparator ID="ToolbarSeparator5" runat="server" />
                                                            <ext:Label ID="Label3" runat="server" Text="科室：" />
                                                            <ext:ComboBox ID="cbbdept" runat="server" StoreID="Store2" DisplayField="DEPT_NAME"
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
                                                            <ext:ToolbarSeparator ID="ToolbarSeparator2" runat="server" />
                                                            <ext:Button ID="Button_look" runat="server" Text="查询" Icon="Zoom">
                                                                <AjaxEvents>
                                                                    <Click OnEvent="Button_look_click" Timeout="99999999">
                                                                        <EventMask Msg="载入中..." ShowMask="true" />
                                                                    </Click>
                                                                </AjaxEvents>
                                                            </ext:Button>
                                                            <ext:ToolbarSeparator ID="ToolbarSeparator3" runat="server" />
                                                            <ext:Button ID="Button_del" runat="server" Text="删除" Icon="Delete" Disabled="true">
                                                                <AjaxEvents>
                                                                    <Click OnEvent="Button_del_click">
                                                                        <Confirmation ConfirmRequest="true" Title="系统提示" Message="将删除选中数据,<br/>是否继续?" />
                                                                        <ExtraParams>
                                                                        </ExtraParams>
                                                                        <EventMask Msg="载入中..." ShowMask="true" />
                                                                        <ExtraParams>
                                                                            <ext:Parameter Name="Values" Value="Ext.encode(#{GridPanel1}.getRowsValues())" Mode="Raw">
                                                                            </ext:Parameter>
                                                                        </ExtraParams>
                                                                    </Click>
                                                                </AjaxEvents>
                                                            </ext:Button>
                                                            <ext:ToolbarSeparator ID="ToolbarSeparator4" runat="server" />
                                                            <ext:Button ID="Button_save" runat="server" Text="保存" Icon="Disk">
                                                                <AjaxEvents>
                                                                    <Click OnEvent="Button_Save_click" Timeout="99999999">
                                                                        <EventMask Msg="保存中..." ShowMask="true" />
                                                                        <ExtraParams>
                                                                            <ext:Parameter Name="Values" Value="Ext.encode(#{GridPanel1}.getRowsValues(false))"
                                                                                Mode="Raw">
                                                                            </ext:Parameter>
                                                                        </ExtraParams>
                                                                    </Click>
                                                                </AjaxEvents>
                                                            </ext:Button>
                                                            <ext:ToolbarSeparator ID="ToolbarSeparator7" runat="server" />
                                                            <ext:Button ID="btn_Excel" runat="server" OnClick="OutExcel" AutoPostBack="true"
                                                                Text="导出Excel" Icon="PageWhiteExcel" CausesValidation="false">
                                                            </ext:Button>
                                                            <ext:Button ID="Button1" runat="server" Text="提交" Icon="ArrowOut">
                                                                <AjaxEvents>
                                                                    <Click OnEvent="commit_click" Timeout="99999999">
                                                                        <EventMask Msg="提交中..." ShowMask="true" />
                                                                        <ExtraParams>
                                                                            <ext:Parameter Name="Values" Value="Ext.encode(#{GridPanel1}.getRowsValues(false))"
                                                                                Mode="Raw">
                                                                            </ext:Parameter>
                                                                        </ExtraParams>
                                                                    </Click>
                                                                </AjaxEvents>
                                                            </ext:Button>
                                                            <ext:ToolbarSeparator ID="ToolbarSeparator6" runat="server" />
                                                            <ext:Button ID="Button_check" runat="server" Text="审核" Icon="ArrowOut">
                                                                <AjaxEvents>
                                                                    <Click OnEvent="check_click" Timeout="99999999">
                                                                        <EventMask Msg="审核中..." ShowMask="true" />
                                                                        <ExtraParams>
                                                                            <ext:Parameter Name="Values" Value="Ext.encode(#{GridPanel1}.getRowsValues(false))"
                                                                                Mode="Raw">
                                                                            </ext:Parameter>
                                                                        </ExtraParams>
                                                                    </Click>
                                                                </AjaxEvents>
                                                            </ext:Button>
                                                            <ext:ToolbarSeparator ID="ToolbarSeparator9" runat="server" />
                                                            <ext:Button ID="btnDeptChange" runat="server" Text="科室调动" Icon="LinkGo" Disabled="true">
                                                                <Listeners>
                                                                    <Click Handler="TreeOpration();" />
                                                                </Listeners>
                                                            </ext:Button>
                                                            <ext:ToolbarSeparator ID="ToolbarSeparator1" runat="server" />
                                                            <ext:Button ID="Button2" runat="server" Text="新增" Icon="DatabaseAdd">
                                                                <Listeners>
                                                                    <Click Handler="var record2 = Store1.getAt(RowIndex); var cc=record2.data['DEPT_CODE'];var bb=record2.data['DEPT_NAME'];var dd=record2.data['EMP_NO'];var ee=record2.data['STAFF_ID'];var ff=record2.data['STAFF_NAME'];var gg=record2.data['QJTS'];#{GridPanel1}.insertRecord(0, {}); var record3 = Store1.getAt(0); record3.data['DEPT_CODE'] = cc; record3.data['DEPT_NAME'] = bb;record3.data['EMP_NO'] = dd; record3.data['STAFF_ID'] = ee;record3.data['STAFF_NAME'] = ff;record3.data['QJTS'] = gg;#{GridPanel1}.getView().refresh();#{GridPanel1}.getView().focusRow(0);#{GridPanel1}.startEditing(0, 0);" />
                                                                </Listeners>
                                                            </ext:Button>
                                                        </Items>
                                                    </ext:Toolbar>
                                                </TopBar>
                                                <ColumnModel ID="ColumnModel1" runat="server">
                                                    <Columns>
                                                        <ext:Column ColumnID="ROW_ID" Hidden="true" />
                                                        <ext:Column ColumnID="DEPT_NAME" Header="<div style='text-align:center;'>科室</div>"
                                                            Width="130" Align="left" DataIndex="DEPT_NAME" MenuDisabled="true">
                                                            <Editor>
                                                                <ext:ComboBox ID="ComboBox4" runat="server" StoreID="Store2" DisplayField="DEPT_CODE"
                                                                    AllowBlank="true" ValueField="DEPT_NAME" TypeAhead="false" LoadingText="Searching..."
                                                                    Width="220" ListWidth="220" PageSize="10" ItemSelector="div.search-item" MinChars="1">
                                                                    <Template ID="Template5" runat="server">
                                                                  <tpl for=".">
                                                                   <div class="search-item">
                                                                     <h3><span>{DEPT_NAME}</span>{DEPT_CODE}</h3>
                                                                   </div>
                                                                  </tpl>
                                                                    </Template>
                                                                    <Listeners>
                                                                        <Select Handler="selectOrderedDept(this.getText());" />
                                                                    </Listeners>
                                                                </ext:ComboBox>
                                                            </Editor>
                                                        </ext:Column>
                                                        <ext:Column ColumnID="STAFF_NAME" Header="<div style='text-align:center;'>人员</div>"
                                                            Width="100" Align="left" DataIndex="STAFF_NAME" MenuDisabled="true">
                                                            <Editor>
                                                                <ext:ComboBox ID="ComboBox1" runat="server" StoreID="Store3" DisplayField="STAFF_ID"
                                                                    AllowBlank="true" ValueField="STAFF_NAME" TypeAhead="false" LoadingText="Searching..."
                                                                    Width="220" ListWidth="220" PageSize="10" ItemSelector="div.search-item" MinChars="1">
                                                                    <Template ID="Template2" runat="server">
                                                                  <tpl for=".">
                                                                   <div class="search-item">
                                                                     <h3><span>{DEPT_NAME}</span>{STAFF_NAME}</h3>
                                                                   </div>
                                                                  </tpl>
                                                                    </Template>
                                                                    <Listeners>
                                                                        <Select Handler="selectOrderedPerson(this.getText());" />
                                                                    </Listeners>
                                                                </ext:ComboBox>
                                                            </Editor>
                                                        </ext:Column>
                                                        <ext:Column ColumnID="ATTENDANCE_NAME" Header="<div style='text-align:center;'>考勤项目</div>"
                                                            Width="100" Align="left" DataIndex="ATTENDANCE_NAME" MenuDisabled="true">
                                                            <Editor>
                                                                <ext:ComboBox ID="ComboBox2" runat="server" StoreID="Store4" DisplayField="ATTENDANCE_CODE"
                                                                    AllowBlank="true" ValueField="ATTENDANCE_NAME" TypeAhead="false" LoadingText="Searching..."
                                                                    Width="200" ListWidth="200" PageSize="15" ItemSelector="div.search-item" MinChars="1">
                                                                    <Template ID="Template3" runat="server">
                                                                  <tpl for=".">
                                                                   <div class="search-item">
                                                                     <h3><span>{ATTENDANCE_CODE}</span>{ATTENDANCE_NAME}</h3>
                                                                   </div>
                                                                  </tpl>
                                                                    </Template>
                                                                    <Listeners>
                                                                        <Select Handler="selectOrderedItem(this.getText());" />
                                                                    </Listeners>
                                                                </ext:ComboBox>
                                                            </Editor>
                                                        </ext:Column>
                                                        <ext:Column ColumnID="ATTENDANCE_VALUE" Header="<div style='text-align:center;'>天数</div>"
                                                            Width="90" Align="Right" Sortable="false" DataIndex="ATTENDANCE_VALUE" MenuDisabled="true">
                                                            <Editor>
                                                                <ext:NumberField ID="NumberField1" runat="server" />
                                                            </Editor>
                                                            <Renderer Fn="rmbMoney" />
                                                        </ext:Column>
                                                        <ext:Column ColumnID="MEMO" Header="<div style='text-align:center;'>备注</div>" Width="150"
                                                            Align="left" Sortable="false" DataIndex="MEMO" MenuDisabled="true">
                                                            <Editor>
                                                                <ext:ComboBox ID="ComboBox3" runat="server" StoreID="Store5" DisplayField="MEMO"
                                                                    AllowBlank="true" ValueField="MEMO" TypeAhead="false" LoadingText="Searching..."
                                                                    Width="220" ListWidth="220" PageSize="15" ItemSelector="div.search-item" MinChars="1">
                                                                    <Template ID="Template4" runat="server">
                                                                  <tpl for=".">
                                                                   <div class="search-item">
                                                                     <h3>{MEMO}</h3>
                                                                   </div>
                                                                  </tpl>
                                                                    </Template>
                                                                </ext:ComboBox>
                                                            </Editor>
                                                        </ext:Column>
                                                        <ext:Column ColumnID="REPORTER" Header="<div style='text-align:center;'>考勤员</div>"
                                                            Width="80" Align="Center" Sortable="false" DataIndex="REPORTER" MenuDisabled="true">
                                                        </ext:Column>
                                                        <ext:Column ColumnID="CHECK_TAG" Header="<div style='text-align:center;'>状态</div>"
                                                            Width="100" Align="Center" Sortable="false" DataIndex="CHECK_TAG" MenuDisabled="true">
                                                        </ext:Column>
                                                        <ext:Column ColumnID="JZ" Hidden="true" DataIndex="JZ" MenuDisabled="true"/>
                                                    </Columns>
                                                </ColumnModel>
                                                <SelectionModel>
                                                    <ext:CheckboxSelectionModel ID="RowSelectionModel1" runat="server">
                                                        <Listeners>
                                                            <RowSelect Handler="#{btnDeptChange}.enable();RowIndex = rowIndex" />
                                                            <RowDeselect Handler="#{btnDeptChange}.enable();RowIndex = rowIndex" />
                                                            <SelectionChange Handler="#{GridPanel1}.hasSelection()? #{Button_del}.setDisabled(false): #{Button_del}.setDisabled(true);" />
                                                        </Listeners>
                                                    </ext:CheckboxSelectionModel>
                                                </SelectionModel>
                                                <LoadMask ShowMask="true" />
                                                <Listeners>
                                                    <KeyDown Handler="if (e.getKey() == 40){#{GridPanel1}.insertRecord(0, {});#{GridPanel1}.getView().focusRow(0);#{GridPanel1}.startEditing(0, 0);} ;" />
                                                    <%-- <BeforeEdit Fn="beforeEdit" />--%>
                                                </Listeners>
                                                <View>
                                    <ext:GridView ID="GridView1" runat="server">
                                        <HeaderRows>
                                            <ext:HeaderRow>
                                                <Columns>
                                                    <ext:HeaderColumn>
                                                    </ext:HeaderColumn>
                                                    <ext:HeaderColumn>
                                                    </ext:HeaderColumn>
                                                    <ext:HeaderColumn>
                                                    </ext:HeaderColumn>
                                                    <ext:HeaderColumn>
                                                        <Component>
                                                            <ext:TextField runat="server" ID="summoney" ReadOnly="true" StyleSpec="text-align:right">
                                                            </ext:TextField>
                                                        </Component>
                                                    </ext:HeaderColumn>
                                                    <ext:HeaderColumn>
                                                    </ext:HeaderColumn>
                                                    <ext:HeaderColumn>
                                                    </ext:HeaderColumn>
                                                    <ext:HeaderColumn>
                                                    </ext:HeaderColumn>
                                                    <ext:HeaderColumn>
                                                    </ext:HeaderColumn>
                                                    <ext:HeaderColumn>
                                                    </ext:HeaderColumn>
                                                    <ext:HeaderColumn>
                                                    </ext:HeaderColumn>
                                                </Columns>
                                            </ext:HeaderRow>
                                        </HeaderRows>
                                    </ext:GridView>
                                </View>
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
        <ext:Window ID="DetailWin" runat="server" Icon="Add" Title="导入execl数据" Width="360"
            Height="200" AutoShow="false" Modal="true" CenterOnLoad="true" AutoScroll="false"
            ShowOnLoad="false" Closable="false" Resizable="false" StyleSpec="background-color:Transparent;"
            BodyStyle="background-color:Transparent;" CloseAction="Hide">
        </ext:Window>
        <ext:Window ID="arcEditWindow" runat="server" Icon="Group" Title="科室调动" Width="250"
            Height="130" AutoShow="false" Modal="true" CenterOnLoad="true" ShowOnLoad="false"
            Resizable="false" StyleSpec="background-color:Transparent;" BodyStyle="background-color:Transparent;">
            <Body>
                <ext:ColumnLayout ID="ColumnLayout2" runat="server">
                    <ext:LayoutColumn ColumnWidth="1">
                        <ext:Panel ID="Panel1" runat="server" Border="false" Header="false" BodyStyle="background-color:Transparent;margin:10px;">
                            <Body>
                                <ext:FormLayout ID="FormLayout1" runat="server" LabelAlign="Left">
                                    <ext:Anchor Horizontal="92%">
                                        <ext:TextField ID="staffName" CausesValidation="true" AllowBlank="false" runat="server"
                                            FieldLabel="调动人员名称" MaxLength="20">
                                        </ext:TextField>
                                    </ext:Anchor>
                                    <ext:Anchor Horizontal="92%">
                                        <ext:ComboBox ID="cboChangeDept" runat="server" StoreID="Store6" DisplayField="DEPT_NAME"
                                            Width="120" ValueField="DEPT_CODE" TypeAhead="false" LoadingText="Searching..."
                                            PageSize="1000" ItemSelector="div.search-item" MinChars="1" FieldLabel="调动人员科室"
                                            ListWidth="240" CausesValidation="true" AllowBlank="false">
                                            <Template ID="Template6" runat="server">
                                       <tpl for=".">
                                          <div class="search-item">
                                             <h3><span style="width:auto">{DEPT_CODE}</span>{DEPT_NAME}</h3>
                                          </div>
                                       </tpl>
                                            </Template>
                                        </ext:ComboBox>
                                    </ext:Anchor>
                                </ext:FormLayout>
                            </Body>
                        </ext:Panel>
                    </ext:LayoutColumn>
                </ext:ColumnLayout>
            </Body>
            <BottomBar>
                <ext:Toolbar ID="Toolbar1" runat="server">
                    <Items>
                        <ext:ToolbarFill ID="ToolbarFill2" runat="server" />
                        <ext:ToolbarButton ID="Btn_BatStart" runat="server" Icon="Disk" Text="保存">
                            <AjaxEvents>
                                <Click OnEvent="SaveInfo" Before="if (CheckForm()== false){ Ext.Msg.alert('系统提示','请根据红线提示填写正确的信息！');return false;};"
                                    Success=" btnDeptChange.setDisabled(true); arcEditWindow.hide();">
                                    <ExtraParams>
                                        <ext:Parameter Name="Staffid" Value="Ext.encode(#{Store1}.getAt(RowIndex).get('STAFF_ID'))"
                                            Mode="Raw">
                                        </ext:Parameter>
                                        <ext:Parameter Name="staffOldDeptCode" Value="Ext.encode(#{Store1}.getAt(RowIndex).get('DEPT_CODE'))"
                                            Mode="Raw">
                                        </ext:Parameter>
                                        <ext:Parameter Name="staffOldDeptName" Value="Ext.encode(#{Store1}.getAt(RowIndex).get('DEPT_NAME'))"
                                            Mode="Raw">
                                        </ext:Parameter>
                                        <ext:Parameter Name="StaffName" Value="Ext.encode(#{Store1}.getAt(RowIndex).get('STAFF_NAME'))"
                                            Mode="Raw">
                                        </ext:Parameter>
                                    </ExtraParams>
                                </Click>
                            </AjaxEvents>
                        </ext:ToolbarButton>
                        <ext:ToolbarSeparator ID="ToolbarSeparator8" runat="server" />
                        <ext:ToolbarButton ID="Btn_BatCancel" runat="server" Icon="Cancel" Text="退出">
                            <Listeners>
                                <Click Handler="arcEditWindow.hide();" />
                            </Listeners>
                        </ext:ToolbarButton>
                    </Items>
                </ext:Toolbar>
            </BottomBar>
        </ext:Window>
    </div>
    </form>
</body>
</html>
