<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="dept_income_item.aspx.cs"
    Inherits="GoldNet.JXKP.cbhs.cbhsdict.dept_income_item" %>

<%@ Register Assembly="Goldnet.Ext.Web" Namespace="Goldnet.Ext.Web" TagPrefix="ext" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style type="text/css">
        body
        {
            background-color: #DFE8F6;
            font-size: 12px;
        }
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
    <link rel="stylesheet" type="text/css" href="../../Bonus/Orthers/Cbouns.css" />

    <script language="javascript" type="text/javascript">
        var RefreshData = function(msg) {
            Ext.Msg.show({ title: '提示', msg: msg, icon: 'ext-mb-info', buttons: { ok: true} });
            Store1.reload();
        }
    </script>

</head>
<body>
    <form id="form1" runat="server">
    <div>
        <ext:ScriptManager ID="ScriptManager1" runat="server" />
        <ext:Store ID="Store1" runat="server" OnRefreshData="Store_RefreshData">
            <Reader>
                <ext:JsonReader ReaderID="ID">
                    <Fields>
                        <ext:RecordField Name="ID" Type="String" Mapping="ID" />
                        <ext:RecordField Name="ITEM_CLASS" Type="String" Mapping="ITEM_CLASS" />
                        <ext:RecordField Name="ORDER_DEPT" Type="String" Mapping="ORDER_DEPT" />
                        
                        <ext:RecordField Name="PERFORM_DEPT" Type="String" Mapping="PERFORM_DEPT" />
                        
                        <ext:RecordField Name="CLASSNAME" Type="String" Mapping="CLASSNAME" />
                       
                        <ext:RecordField Name="ORDER_DEPT_DISTRIBUT" Type="String" Mapping="ORDER_DEPT_DISTRIBUT" />
                        <ext:RecordField Name="PERFORM_DEPT_DISTRIBUT" Type="String" Mapping="PERFORM_DEPT_DISTRIBUT" />
                        <ext:RecordField Name="NURSING_PERCEN" Type="String" Mapping="NURSING_PERCEN" />
                        <ext:RecordField Name="OUT_OPDEPT_PERCEN" Type="String" Mapping="OUT_OPDEPT_PERCEN" />
                        <ext:RecordField Name="OUT_EXDEPT_PERCEN" Type="String" Mapping="OUT_EXDEPT_PERCEN" />
                        <ext:RecordField Name="OUT_NURSING_PERCEN" Type="String" Mapping="OUT_NURSING_PERCEN" />
                        <ext:RecordField Name="COOPERANT_PERCEN" Type="String" Mapping="COOPERANT_PERCEN" />
                        <ext:RecordField Name="FIXED_PERCEN" Type="String" Mapping="FIXED_PERCEN" />
                        <ext:RecordField Name="COST_CODE" Type="String" Mapping="COST_CODE" />
                        <ext:RecordField Name="PROFIT_RATE" Type="String" Mapping="PROFIT_RATE" />
                        
                        <ext:RecordField Name="PERFRO_DEPT" Type="String" Mapping="PERFRO_DEPT" />
                        
                    </Fields>
                </ext:JsonReader>
            </Reader>
        </ext:Store>
        <ext:Store ID="Store2" runat="server" AutoLoad="true">
            <Proxy>
            </Proxy>
            <Reader>
                <ext:JsonReader Root="costsitemlist" TotalProperty="totalitems">
                    <Fields>
                        <ext:RecordField Name="ITEM_CODE" />
                        <ext:RecordField Name="ITEM_NAME" />
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
        <ext:Store ID="Store4" runat="server" AutoLoad="true">
            <Proxy>
            </Proxy>
            <Reader>
                <ext:JsonReader Root="itemlist" TotalProperty="totalitems">
                    <Fields>
                        <ext:RecordField Name="CLASS_CODE" />
                        <ext:RecordField Name="CLASS_NAME" />
                    </Fields>
                </ext:JsonReader>
            </Reader>
        </ext:Store>
        <ext:ViewPort ID="ViewPort1" runat="server">
            <Body>
                <ext:ColumnLayout ID="ColumnLayout1" runat="server" Split="true" FitHeight="true">
                    <Columns>
                        <ext:LayoutColumn ColumnWidth="1">
                            <ext:GridPanel ID="GridPanel1" runat="server" StoreID="Store1" StripeRows="true"
                                TrackMouseOver="true" AutoWidth="true" Height="480" Border="false" AutoScroll="true"
                                ClicksToEdit="1">
                                <TopBar>
                                    <ext:Toolbar ID="Toolbar1" runat="server" Visible="true" AutoWidth="true">
                                        <Items>
                                            <ext:Button ID="Button_set" runat="server" Text="设置" Icon="DatabaseGo" Visible="false">
                                                <AjaxEvents>
                                                    <Click OnEvent="Button_set_click">
                                                        <EventMask Msg="载入中..." ShowMask="true" />
                                                    </Click>
                                                </AjaxEvents>
                                            </ext:Button>
                                            <ext:Button ID="Button1" runat="server" Text="保存" Icon="Disk">
                                                <AjaxEvents>
                                                    <Click OnEvent="Button_Save_click">
                                                        <EventMask Msg="保存中..." ShowMask="true" />
                                                        <ExtraParams>
                                                            <ext:Parameter Name="Values" Value="Ext.encode(#{GridPanel1}.getRowsValues(false))"
                                                                Mode="Raw">
                                                            </ext:Parameter>
                                                        </ExtraParams>
                                                    </Click>
                                                </AjaxEvents>
                                            </ext:Button>
                                             <ext:Button ID="Button2" runat="server" Text="刷新" Icon="DatabaseGo" >
                                                <AjaxEvents>
                                                    <Click OnEvent="Button_refresh_click">
                                                        <EventMask Msg="载入中..." ShowMask="true" />
                                                    </Click>
                                                </AjaxEvents>
                                            </ext:Button>
                                            
                                        </Items>
                                    </ext:Toolbar>
                                </TopBar>
                                <ColumnModel ID="ColumnModel1" runat="server">
                                    <Columns>
                                    <ext:Column ColumnID="ID" Header="<div style='text-align:center;'>编号</div>"
                                            Width="100" Align="Center" Sortable="true" DataIndex="ID" MenuDisabled="true"/>
                                         <ext:Column ColumnID="ITEM_CLASS" Header="<div style='text-align:center;'>项目代码</div>"
                                            Width="100" Align="Center" Sortable="true" DataIndex="ITEM_CLASS" MenuDisabled="true" Hidden="true"/>   
                                           
                                            
                                        <ext:Column ColumnID="CLASSNAME" Header="<div style='text-align:center;'>核算项目</div>"
                                            Width="100" Align="Center" Sortable="true" DataIndex="CLASSNAME" MenuDisabled="true">
                                            <Editor>
                                                <ext:ComboBox ID="ComboBox6" runat="server" StoreID="Store4" DisplayField="CLASS_NAME"
                                                    AllowBlank="true" ValueField="CLASS_NAME" TypeAhead="false" LoadingText="Searching..."
                                                    Width="220" ListWidth="220" PageSize="10" ItemSelector="div.search-item" MinChars="1">
                                                    <Template ID="Template7" runat="server">
                                                      <tpl for=".">
                                                       <div class="search-item">
                                                         
                                                         <h3><span>{CLASS_NAME}</span>{CLASS_CODE}</h3>
                                                       </div>
                                                      </tpl>
                                                    </Template>
                                                </ext:ComboBox>
                                            </Editor>
                                        </ext:Column>
                                        <ext:Column ColumnID="ORDER_DEPT" Header="<div style='text-align:center;'>开单科室</div>"
                                            Width="100" Align="Center" Sortable="true" DataIndex="ORDER_DEPT" MenuDisabled="true">
                                            <Editor>
                                                <ext:ComboBox ID="ComboBox4" runat="server" StoreID="Store3" DisplayField="DEPT_NAME"
                                                    AllowBlank="true" ValueField="DEPT_NAME" TypeAhead="false" LoadingText="Searching..."
                                                    Width="220" ListWidth="220" PageSize="10" ItemSelector="div.search-item" MinChars="1">
                                                    <Template ID="Template5" runat="server">
                                                      <tpl for=".">
                                                       <div class="search-item">
                                                         
                                                         <h3><span>{DEPT_NAME}</span>{DEPT_CODE}</h3>
                                                       </div>
                                                      </tpl>
                                                    </Template>
                                                </ext:ComboBox>
                                            </Editor>
                                        </ext:Column>
                                        <ext:Column ColumnID="PERFORM_DEPT" Header="<div style='text-align:center;'>执行科室</div>"
                                            Width="100" Align="Center" Sortable="true" DataIndex="PERFORM_DEPT" MenuDisabled="true">
                                            <Editor>
                                                <ext:ComboBox ID="ComboBox5" runat="server" StoreID="Store3" DisplayField="DEPT_NAME"
                                                    AllowBlank="true" ValueField="DEPT_NAME" TypeAhead="false" LoadingText="Searching..."
                                                    Width="220" ListWidth="220" PageSize="10" ItemSelector="div.search-item" MinChars="1">
                                                    <Template ID="Template6" runat="server">
                                                      <tpl for=".">
                                                       <div class="search-item">
                                                         <h3><span>{DEPT_NAME}</span>{DEPT_CODE}</h3>
                                                       </div>
                                                      </tpl>
                                                    </Template>
                                                </ext:ComboBox>
                                            </Editor>
                                        </ext:Column>
                                        <ext:Column ColumnID="ORDER_DEPT_DISTRIBUT" Header="住院开单" Width="60" Align="right"
                                            Sortable="true" DataIndex="ORDER_DEPT_DISTRIBUT" MenuDisabled="true">
                                            <Editor>
                                                <ext:NumberField ID="nfpercent" runat="server" SelectOnFocus="true" DecimalPrecision="2"
                                                    MaxValue="100" MinValue="0">
                                                </ext:NumberField>
                                            </Editor>
                                        </ext:Column>
                                        <ext:Column ColumnID="PERFORM_DEPT_DISTRIBUT" Header="住院执行" Width="60" Align="right"
                                            Sortable="true" DataIndex="PERFORM_DEPT_DISTRIBUT" MenuDisabled="true">
                                            <Editor>
                                                <ext:NumberField ID="NumberField1" runat="server" SelectOnFocus="true" DecimalPrecision="2"
                                                    MaxValue="100" MinValue="0">
                                                </ext:NumberField>
                                            </Editor>
                                        </ext:Column>
                                        <ext:Column ColumnID="NURSING_PERCEN" Header="住院护理" Width="60" Align="right" Sortable="true"
                                            DataIndex="NURSING_PERCEN" MenuDisabled="true">
                                            <Editor>
                                                <ext:NumberField ID="NumberField2" runat="server" SelectOnFocus="true" DecimalPrecision="2"
                                                    MaxValue="100" MinValue="0">
                                                </ext:NumberField>
                                            </Editor>
                                        </ext:Column>
                                        
                                        
                                        <ext:Column ColumnID="OUT_OPDEPT_PERCEN" Header="门诊开单" Width="60" Align="right" Sortable="true"
                                            DataIndex="OUT_OPDEPT_PERCEN" MenuDisabled="true">
                                            <Editor>
                                                <ext:NumberField ID="NumberField3" runat="server" SelectOnFocus="true" DecimalPrecision="2"
                                                    MaxValue="100" MinValue="0">
                                                </ext:NumberField>
                                            </Editor>
                                        </ext:Column>
                                        <ext:Column ColumnID="OUT_EXDEPT_PERCEN" Header="门诊执行" Width="60" Align="right" Sortable="true"
                                            DataIndex="OUT_EXDEPT_PERCEN" MenuDisabled="true">
                                            <Editor>
                                                <ext:NumberField ID="NumberField4" runat="server" SelectOnFocus="true" DecimalPrecision="2"
                                                    MaxValue="100" MinValue="0">
                                                </ext:NumberField>
                                            </Editor>
                                        </ext:Column>
                                        <ext:Column ColumnID="OUT_NURSING_PERCEN" Header="门诊护理" Width="60" Align="right"
                                            Sortable="true" DataIndex="OUT_NURSING_PERCEN" MenuDisabled="true">
                                            <Editor>
                                                <ext:NumberField ID="NumberField6" runat="server" SelectOnFocus="true" DecimalPrecision="2"
                                                    MaxValue="100" MinValue="0">
                                                </ext:NumberField>
                                            </Editor>
                                        </ext:Column>
                                         
                                       
                                         <ext:Column ColumnID="PERFRO_DEPT" Header="<div style='text-align:center;'>分配科室</div>"
                                            Width="80" Align="Center" Sortable="true" DataIndex="PERFRO_DEPT" MenuDisabled="true">
                                            <Editor>
                                                <ext:ComboBox ID="ComboBox3" runat="server" StoreID="Store3" DisplayField="DEPT_NAME"
                                                    AllowBlank="true" ValueField="DEPT_NAME" TypeAhead="false" LoadingText="Searching..."
                                                    Width="220" ListWidth="220" PageSize="10" ItemSelector="div.search-item" MinChars="1">
                                                    <Template ID="Template3" runat="server">
                                                      <tpl for=".">
                                                       <div class="search-item">
                                                         <h3><span>{DEPT_NAME}</span>{DEPT_CODE}</h3>
                                                       </div>
                                                      </tpl>
                                                    </Template>
                                                </ext:ComboBox>
                                            </Editor>
                                        </ext:Column>
                                        <ext:Column ColumnID="COST_CODE" Header="<div style='text-align:center;'>成本对照</div>"
                                            Width="80" Align="Center" Sortable="true" DataIndex="COST_CODE" MenuDisabled="true">
                                            <Editor>
                                                <ext:ComboBox ID="COST_CODE" runat="server" StoreID="Store2" DisplayField="ITEM_NAME"
                                                    AllowBlank="true" ValueField="ITEM_NAME" TypeAhead="false" LoadingText="Searching..."
                                                    Width="220" ListWidth="220" PageSize="10" ItemSelector="div.search-item" MinChars="1">
                                                    <Template ID="Template4" runat="server">
                                                      <tpl for=".">
                                                       <div class="search-item">
                                                         <h3><span>{ITEM_NAME}</span>{ITEM_CODE}</h3>
                                                       </div>
                                                      </tpl>
                                                    </Template>
                                                </ext:ComboBox>
                                            </Editor>
                                        </ext:Column>
                                        <ext:Column ColumnID="COOPERANT_PERCEN" Header="合作医疗" Width="60" Align="right" Sortable="true"
                                            DataIndex="COOPERANT_PERCEN" MenuDisabled="true">
                                            <Editor>
                                                <ext:NumberField ID="NumberField5" runat="server" SelectOnFocus="true" DecimalPrecision="2"
                                                    MaxValue="100" MinValue="0">
                                                </ext:NumberField>
                                            </Editor>
                                        </ext:Column>
                                        <ext:Column ColumnID="PROFIT_RATE" Header="利润率" Width="60" Align="right" Sortable="true"
                                            DataIndex="PROFIT_RATE" MenuDisabled="true" Hidden="true">
                                            <Editor>
                                                <ext:NumberField ID="NumberField7" runat="server" SelectOnFocus="true" DecimalPrecision="2"
                                                    MaxValue="100" MinValue="0">
                                                </ext:NumberField>
                                            </Editor>
                                        </ext:Column>
                                        <ext:Column ColumnID="FIXED_PERCEN" Header="固定折算比" Width="70" Align="right" Sortable="true"
                                            DataIndex="FIXED_PERCEN" MenuDisabled="true">
                                            <Editor>
                                                <ext:NumberField ID="NumberField8" runat="server" SelectOnFocus="true" DecimalPrecision="2"
                                                    MaxValue="100" MinValue="0">
                                                </ext:NumberField>
                                            </Editor>
                                        </ext:Column>
                                        <ext:CommandColumn Width="80" Align="Center" Header="<div style='text-align:center;'>第三方科室</div>">
                                        <Commands>
                                            <ext:GridCommand Icon="Outline" Text="<div style='text-align:center;'>第三方科室</div>"  CommandName="OtherDept" ToolTip-Text="设置第三方科室">
                          
                                            </ext:GridCommand>                        
                                        </Commands>                                       
                                    </ext:CommandColumn>
                                    </Columns>
                                </ColumnModel>
                               <%-- <AjaxEvents>
                                    <RowDblClick OnEvent="Button_set_click" />
                                </AjaxEvents>--%>
                                <SelectionModel>
                                    <ext:RowSelectionModel ID="RowSelectionModel1" runat="server" SingleSelect="true">
                                      <%--  <Listeners>
                                            <RowSelect Handler="#{Button_set}.enable()" />
                                            <RowDeselect Handler="if (!#{GridPanel1}.hasSelection()) {#{Button_set}.disable()}" />
                                        </Listeners>--%>
                                    </ext:RowSelectionModel>
                                </SelectionModel>
                                <Listeners>
                                <Command Handler="if(command=='OtherDept'){ Goldnet.AjaxMethods.SetDept(record.data.ITEM_CLASS,record.data.ID)}" />
                            </Listeners>
                            </ext:GridPanel>
                        </ext:LayoutColumn>
                    </Columns>
                </ext:ColumnLayout>
            </Body>
        </ext:ViewPort>
    </div>
    <ext:Window ID="DetailWin" runat="server" Icon="Group" Title="科室明细比例设置" Width="410"
        Height="470" AutoShow="false" Modal="true" CenterOnLoad="true" AutoScroll="false"
        ShowOnLoad="false" Resizable="false" StyleSpec="background-color:Transparent;"
        BodyStyle="background-color:Transparent;">
    </ext:Window>
    <ext:Window ID="DeptWin" runat="server" Icon="Group" Title="设置第三方科室" Width="550" Height="400"
            AutoShow="false" Modal="true" CenterOnLoad="true" AutoScroll="true" ShowOnLoad="false"
            Resizable="true" StyleSpec="background-color:Transparent;" BodyStyle="background-color:Transparent;">
        </ext:Window>
    </form>
</body>
</html>
