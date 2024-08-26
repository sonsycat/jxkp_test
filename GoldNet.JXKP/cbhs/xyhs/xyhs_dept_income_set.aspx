<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="xyhs_dept_income_set.aspx.cs" Inherits="GoldNet.JXKP.cbhs.xyhs.xyhs_dept_income_set" %>
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
    </style>
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
            
        function dbonclick(item_class)
		{
		   document.location.href="dept_income_item.aspx?item_class="+item_class;  
		}
    </script>

</head>
<body>
    <form id="form1" runat="server">
    <ext:ScriptManager ID="ScriptManager1" runat="server"/>
    <ext:Store ID="Store1" runat="server" AutoLoad="true"  OnRefreshData="Store_RefreshData"  >
        <Reader>
            <ext:JsonReader>
                <Fields>
                    <%--<ext:RecordField Name="CLASS_CODE"  />
                    <ext:RecordField Name="CLASS_NAME"   />--%>
                    <ext:RecordField Name="DEPT_CODE"  />
                    <ext:RecordField Name="DEPT_NAME"   />
                    <ext:RecordField Name="INCOME_CODE"  />
                    <ext:RecordField Name="INCOME_NAME" />
                   
                </Fields>
            </ext:JsonReader>
        </Reader>
    </ext:Store>
     <ext:Store ID="Store2" runat="server" AutoLoad="true">
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
                            TrackMouseOver="true" AutoWidth="true" Height="480" Border="false">
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
                                       
                                        <ext:Button ID="Button_refresh" runat="server" Text="刷新" Icon="ArrowRefresh">
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
                                    <ext:Column ColumnID="DEPT_NAME" Header="科室名称" Width="120" Align="left" Sortable="true"
                                        DataIndex="DEPT_NAME" MenuDisabled="true">
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
                                  
                                    <ext:Column ColumnID="INCOME_NAME" Header="收入项目" Width="80" Align="left"
                                        Sortable="true" DataIndex="INCOME_NAME" MenuDisabled="true">
                                        <Editor>
                                                <ext:ComboBox ID="ComboBox6" runat="server" StoreID="Store2" DisplayField="CLASS_NAME"
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
                        
                                </Columns>
                            </ColumnModel>
                            
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
    <ext:Window ID="DetailWin" runat="server" Icon="Group" Title="全成本收入项目设置" Width="400"
        Height="200" AutoShow="false" Modal="true" CenterOnLoad="true" AutoScroll="false"
        ShowOnLoad="false" Resizable="false" StyleSpec="background-color:Transparent;"
        BodyStyle="background-color:Transparent;">
    </ext:Window>
     
    </form>
</body>
</html>
