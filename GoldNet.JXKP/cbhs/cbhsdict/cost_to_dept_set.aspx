<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="cost_to_dept_set.aspx.cs" Inherits="GoldNet.JXKP.cbhs.cbhsdict.cost_to_dept_set" %>
<%@ Register Assembly="Goldnet.Ext.Web" Namespace="Goldnet.Ext.Web" TagPrefix="ext" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
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
                        <ext:RecordField Name="DEPT_CODE" Type="String" Mapping="DEPT_CODE" />
                        <ext:RecordField Name="DEPT_NAME" Type="String" Mapping="DEPT_NAME" />
                        
                        <ext:RecordField Name="PROG_CODE" Type="String" Mapping="PROG_CODE" />
                        <ext:RecordField Name="PROG_NAME" Type="String" Mapping="PROG_NAME" />
                        <ext:RecordField Name="FLAGS" Type="String" Mapping="FLAGS" />                        
                    </Fields>
                </ext:JsonReader>
            </Reader>
        </ext:Store>
        <ext:Store ID="Store2" runat="server" AutoLoad="true">
            <Proxy>
            </Proxy>
            <Reader>
                <ext:JsonReader Root="proglist" TotalProperty="progcount">
                    <Fields>
                        <ext:RecordField Name="PROG_CODE" />
                        <ext:RecordField Name="PROG_NAME" />
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
                <ext:ColumnLayout ID="ColumnLayout1" runat="server" Split="true" FitHeight="true">
                    <Columns>
                        <ext:LayoutColumn ColumnWidth="1">
                            <ext:GridPanel ID="GridPanel1" runat="server" StoreID="Store1" StripeRows="true"
                                TrackMouseOver="true" AutoWidth="true" Height="480" Border="false" AutoScroll="true"
                                ClicksToEdit="1">
                                <TopBar>
                                    <ext:Toolbar ID="Toolbar1" runat="server" Visible="true" AutoWidth="true">
                                        <Items>
                                           
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
                                          
                                             <ext:Button ID="Button_set" runat="server" Text="刷新" Icon="DatabaseGo" >
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
                                        
                                        <ext:Column ColumnID="DEPT_NAME" Header="<div style='text-align:center;'>科室名称</div>"
                                            Width="100" Align="Center" Sortable="true" DataIndex="DEPT_NAME" MenuDisabled="true">
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
                                        <ext:Column ColumnID="PROG_NAME" Header="<div style='text-align:center;'>分摊方案</div>"
                                            Width="100" Align="Center" Sortable="true" DataIndex="PROG_NAME" MenuDisabled="true">
                                            <Editor>
                                                <ext:ComboBox ID="ComboBox5" runat="server" StoreID="Store2" DisplayField="PROG_NAME"
                                                    AllowBlank="true" ValueField="PROG_NAME" TypeAhead="false" LoadingText="Searching..."
                                                    Width="220" ListWidth="220" PageSize="10" ItemSelector="div.search-item" MinChars="1">
                                                    <Template ID="Template6" runat="server">
                                                      <tpl for=".">
                                                       <div class="search-item">
                                                         <h3><span>{PROG_NAME}</span>{PROG_CODE}</h3>
                                                       </div>
                                                      </tpl>
                                                    </Template>
                                                </ext:ComboBox>
                                            </Editor>
                                        </ext:Column>
                                        
                                        <ext:CommandColumn Width="80" Align="Center" Header="<div style='text-align:center;'>选择科室</div>">
                                        <Commands>
                                            <ext:GridCommand Icon="Outline" Text="<div style='text-align:center;'>科室</div>" CommandName="ToDept" ToolTip-Text="选择科室">
                          
                                            </ext:GridCommand>                        
                                        </Commands>                                       
                                    </ext:CommandColumn>
                                    <ext:CommandColumn Width="80" Align="Center" Header="<div style='text-align:center;'>选择项目</div>">
                                        <Commands>
                                            <ext:GridCommand Icon="Outline" Text="<div style='text-align:center;'>项目</div>" CommandName="ForItem" ToolTip-Text="选择项目">
                          
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
                                        <Listeners>
                                            <RowSelect Handler="#{Button_set}.enable()" />
                                            <RowDeselect Handler="if (!#{GridPanel1}.hasSelection()) {#{Button_set}.disable()}" />
                                        </Listeners>
                                    </ext:RowSelectionModel>
                                </SelectionModel>
                                <Listeners>
                                <Command Handler="if(command=='ToDept'){ Goldnet.AjaxMethods.SetDept(record.data.ID)} else if(command=='ForItem'){ Goldnet.AjaxMethods.SetItem(record.data.ID)}" />
                            </Listeners>
                            </ext:GridPanel>
                        </ext:LayoutColumn>
                    </Columns>
                </ext:ColumnLayout>
            </Body>
        </ext:ViewPort>
    </div>
    <ext:Window ID="itemset" runat="server" Icon="Group" Title="选择项目" Width="550" Height="400"
            AutoShow="false" Modal="true" CenterOnLoad="true" AutoScroll="true" ShowOnLoad="false"
            Resizable="true" StyleSpec="background-color:Transparent;" BodyStyle="background-color:Transparent;">
        </ext:Window>
    <ext:Window ID="deptset" runat="server" Icon="Group" Title="选择科室" Width="550" Height="400"
            AutoShow="false" Modal="true" CenterOnLoad="true" AutoScroll="true" ShowOnLoad="false"
            Resizable="true" StyleSpec="background-color:Transparent;" BodyStyle="background-color:Transparent;">
        </ext:Window>
    </form>
</body>
</html>
