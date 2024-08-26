<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="xyhs_diagnosis_set.aspx.cs" Inherits="GoldNet.JXKP.cbhs.xyhs.xyhs_diagnosis_set" %>
<%@ Register Assembly="Goldnet.Ext.Web" Namespace="Goldnet.Ext.Web" TagPrefix="ext" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
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
                    <ext:RecordField Name="DIAGNOSIS_CODE"  />
                    <ext:RecordField Name="DIAGNOSIS_NAME"   />
                    
                   
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
                        <ext:RecordField Name="DIAGNOSIS_CODE" />
                        <ext:RecordField Name="DIAGNOSIS_NAME" />
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
                                <ext:Column ColumnID="DIAGNOSIS_CODE" Hidden="true" />       
                                    <ext:Column ColumnID="DIAGNOSIS_NAME" Header="病种名称" Width="120" Align="left" Sortable="true"
                                        DataIndex="DIAGNOSIS_NAME" MenuDisabled="true">
                                        <Editor>
                                               <ext:TextField ID="sss" runat="server"></ext:TextField>
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
