<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Manual_Decompose_List.aspx.cs" Inherits="GoldNet.JXKP.cbhs.datagather.Manual_Decompose_List" %>
<%@ Register Assembly="Goldnet.Ext.Web" Namespace="Goldnet.Ext.Web" TagPrefix="ext" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
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
    <style type="text/css">
        body
        {
            background-color: #DFE8F6;
            font-size: 12px;
        }
    </style>

    <script language="javascript" type="text/javascript">
        var rmbMoney = function(v) {
               if(v==null||v=="")
               {
               return "";
               }
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
       }
    </script>
    
    <style type="text/css">
        .x-grid3-cell-inner{ 
         border-right:1px solid #eceff6;  
       }
     
    </style>


<link href="../../resources/css/examples.css" rel="stylesheet" type="text/css" /> 
</head>
<body>
    <form id="form1" runat="server">
    <ext:ScriptManager ID="ScriptManager1" runat="server" AjaxMethodNamespace="CompanyX" />
    <ext:Store ID="Store1" runat="server"  WarningOnDirty="false">
        <Reader>
            <ext:JsonReader ReaderID="DEPT_CODE">
                <Fields>
                    <ext:RecordField Name="DEPT_CODE" Type="String" Mapping="DEPT_CODE" />
                    <ext:RecordField Name="DEPT_NAME" Type="String" Mapping="DEPT_NAME" />
                    <ext:RecordField Name="COSTS" Type="Float" Mapping="COSTS" />
                    <ext:RecordField Name="COSTS_ARMYFREE" Type="Float" Mapping="COSTS_ARMYFREE" />
                   
                </Fields>
            </ext:JsonReader>
        </Reader>
    </ext:Store>
    <ext:Store ID="Store2" runat="server" WarningOnDirty="false">
        <Reader>
             <ext:JsonReader ReaderID="DEPT_CODE">
                <Fields>
                    <ext:RecordField Name="DEPT_CODE" Type="String" Mapping="DEPT_CODE" />
                    <ext:RecordField Name="DEPT_NAME" Type="String" Mapping="DEPT_NAME" />
                    <ext:RecordField Name="COSTS" Type="Float" Mapping="COSTS" />
                    <ext:RecordField Name="COSTS_ARMYFREE" Type="Float" Mapping="COSTS_ARMYFREE" />
                    
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
                                        <ext:GridPanel ID="GridPanel1" runat="server" StoreID="Store1" BodyStyle="color:black"
                                            ClicksToEdit="1" TrackMouseOver="true" AutoWidth="true" Height="480" Border="true" BodyBorder="true" StripeRows="false">
                                            <TopBar>
                                                <ext:Toolbar ID="Toolbar_fjsr" runat="server" Visible="true" AutoWidth="true">
                                                    <Items>
                                                        <ext:ComboBox ID="years" runat="server" Width="60" AllowBlank="true" EmptyText="请选择年..."
                                                            FieldLabel="年">
                                                           
                                                        </ext:ComboBox>
                                                        <ext:ToolbarTextItem ID="dd1Name" runat="server" Text="年 " />
                                                        <ext:ComboBox ID="months" runat="server" Width="60" AllowBlank="true" EmptyText="请选择月..."
                                                            FieldLabel="月">
                                                            <Items>
                                                                <ext:ListItem Text="01" Value="01" />
                                                                <ext:ListItem Text="02" Value="02" />
                                                                <ext:ListItem Text="03" Value="03" />
                                                                <ext:ListItem Text="04" Value="04" />
                                                                <ext:ListItem Text="05" Value="05" />
                                                                <ext:ListItem Text="06" Value="06" />
                                                                <ext:ListItem Text="07" Value="07" />
                                                                <ext:ListItem Text="08" Value="08" />
                                                                <ext:ListItem Text="09" Value="09" />
                                                                <ext:ListItem Text="10" Value="10" />
                                                                <ext:ListItem Text="11" Value="11" />
                                                                <ext:ListItem Text="12" Value="12" />
                                                            </Items>
                                                           
                                                        </ext:ComboBox>
                                                        <ext:ToolbarTextItem ID="ToolbarTextItem1" runat="server" Text="月 " />
                                                        <ext:ToolbarSeparator ID="ToolbarSeparator1" runat="server" />
                                                       
                                                        <ext:Button ID="Button_look" runat="server" Text="查询" Icon="DatabaseGo">
                                                            <AjaxEvents>
                                                                <Click OnEvent="Button_look_click">
                                                                    <EventMask Msg="载入中..." ShowMask="true" />
                                                                </Click>
                                                            </AjaxEvents>
                                                        </ext:Button>
                                                        <ext:ToolbarSeparator ID="ToolbarSeparator3" runat="server" />
                                                        
                                                        
                                                        <ext:Button ID="costs" runat="server" Text="成本归集" Icon="Disk" >
                                                            <AjaxEvents>
                                                                <Click OnEvent="costs_click">
                                                                <Confirmation ConfirmRequest="true" Title="系统提示" Message="将进行成本归集,<br/>是否继续?" />
                                                                    <EventMask Msg="载入中..." ShowMask="true" />
                                                                </Click>
                                                            </AjaxEvents>
                                                        </ext:Button>

                                                    </Items>
                                                </ext:Toolbar>
                                            </TopBar>
                                            <ColumnModel ID="ColumnModel1" runat="server" >
                                                <Columns>
                                                    <ext:Column ColumnID="DEPT_CODE" Hidden="true" />
                                                    <ext:Column ColumnID="DEPT_NAME" Header="<div style='text-align:center;'>科室名称</div>"
                                                        Width="130" Align="left" Sortable="false" DataIndex="DEPT_NAME" MenuDisabled="true" />
                                                    <ext:Column ColumnID="COSTS" Header="<div style='text-align:center;'>实际成本</div>"
                                                        Width="130" Align="Right" Sortable="false" DataIndex="COSTS" MenuDisabled="true">
                                                        <Renderer Fn="rmbMoney" />
                                                    </ext:Column>
                                                    <ext:Column ColumnID="COSTS_ARMYFREE" Header="<div style='text-align:center;'>减免成本</div>"
                                                        Width="130" Align="Right" Sortable="false" DataIndex="COSTS_ARMYFREE" MenuDisabled="true">
                                                        <Renderer Fn="rmbMoney" />
                                                    </ext:Column>
                                                   
                                                </Columns>
                                            </ColumnModel>
                                            <SelectionModel>
                                                <ext:RowSelectionModel ID="RowSelectionModel1" runat="server">
                                               
                                                </ext:RowSelectionModel>
                                            </SelectionModel>
                                            
                                        </ext:GridPanel>
                                    </ext:LayoutColumn>
                                </Columns>
                            </ext:ColumnLayout>
                        </Body>
                    </ext:Panel>
                </Center>
                <East MinWidth="400" MaxWidth="800" SplitTip="成本归集后" Collapsible="true" Split="true" >
                    <ext:Panel ID="Panel1" runat="server" Border="false" Width="500" Title="成本归集后"
                        Collapsed="false" AutoScroll="true" TitleCollapse="True">
                      
                        <Body>
                            <ext:ColumnLayout ID="ColumnLayout2" runat="server" Split="true">
                                <Columns>
                                    <ext:LayoutColumn ColumnWidth="1">
                                        <ext:GridPanel ID="GridPanel2" runat="server" Border="false" StoreID="Store2" StripeRows="true"
                                            AutoHeight="true" AutoWidth="true" TrackMouseOver="true" AutoScroll="true">
                                            <ColumnModel ID="ColumnModel2" runat="server">
                                                <Columns>
                                                    <ext:Column ColumnID="DEPT_CODE" Hidden="true" />
                                                    <ext:Column ColumnID="DEPT_NAME" Header="<div style='text-align:center;'>科室名称</div>"
                                                        Width="100" Align="left" Sortable="false" DataIndex="DEPT_NAME" MenuDisabled="true" />
                                                    <ext:Column ColumnID="COSTS" Header="<div style='text-align:center;'>实际成本</div>"
                                                        Width="100" Align="Right" Sortable="false" DataIndex="COSTS" MenuDisabled="true">
                                                        <Renderer Fn="rmbMoney" />
                                                    </ext:Column>
                                                    <ext:Column ColumnID="COSTS_ARMYFREE" Header="<div style='text-align:center;'>减免成本</div>"
                                                        Width="100" Align="Right" Sortable="false" DataIndex="COSTS_ARMYFREE" MenuDisabled="true">
                                                        <Renderer Fn="rmbMoney" />
                                                    </ext:Column>
                                                   
                                                </Columns>
                                            </ColumnModel>
                                            <SelectionModel>
                                                <ext:RowSelectionModel ID="RowSelectionModel2" runat="server">
                                               
                                                </ext:RowSelectionModel>
                                            </SelectionModel>
                                            
                                            <Listeners>
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
   
    </form>
</body>
</html>
