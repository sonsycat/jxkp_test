<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Persons_Change_List.aspx.cs" Inherits="GoldNet.JXKP.WebPage.SysManager.Persons_Change_List" %>
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
    <script type="text/javascript">
       
         var RefreshData = function() {
            Store1.reload();
        }   
    </script>

</head>
<body>
    <form id="form1" runat="server">
    <div>
        <ext:ScriptManager ID="ScriptManager1" runat="server" AjaxMethodNamespace="Goldnet" />
        <ext:Store runat="server" ID="Store1" AutoLoad="true" OnRefreshData="Store_RefreshData" WarningOnDirty="false">
            <Reader>
                <ext:JsonReader ReaderID="ID">
                    <Fields>
                       <ext:RecordField Name="ID" Type="String" Mapping="ID" />
                        <ext:RecordField Name="USER_ID" Type="String" Mapping="USER_ID" />
                        <ext:RecordField Name="USER_NAME" Type="String" Mapping="USER_NAME" />
                        <ext:RecordField Name="DEPT_NAME" Type="String" Mapping="DEPT_NAME" />
                        <ext:RecordField Name="ST_DATE" Type="String" Mapping="ST_DATE"  />
                        <ext:RecordField Name="END_DATE" Type="String" Mapping="END_DATE"/>
                        <ext:RecordField Name="OPERATOR_USERID" Type="String" Mapping="OPERATOR_USERID" />
                        <ext:RecordField Name="OPERATE_DATE" Type="String" Mapping="OPERATE_DATE"/>
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
                                TrackMouseOver="true" Height="480" AutoWidth="true"  AutoScroll="true">
                                <TopBar>
                                    <ext:Toolbar ID="Toolbar_detptype" runat="server" Visible="true" AutoWidth="true">
                                        <Items>
                                            
                                            <ext:Button ID="Buttonadd" runat="server" Text="添加" Icon="DatabaseAdd">
                                                <AjaxEvents>
                                                    <Click OnEvent="persons_add">
                                                        <EventMask Msg="载入中..." ShowMask="true" />
                                                    </Click>
                                                </AjaxEvents>
                                            </ext:Button>
                                            <ext:Button ID="Buttonedit" runat="server" Text="修改" Icon="DatabaseKey">
                                                <AjaxEvents>
                                                    <Click OnEvent="persons_edit">
                                                        <EventMask Msg="载入中..." ShowMask="true" />
                                                    </Click>
                                                </AjaxEvents>
                                            </ext:Button>
                                             <ext:Button ID="Buttondel" runat="server" Text="删除" Icon="DatabaseDelete">
                                                <AjaxEvents>
                                                    <Click OnEvent="persons_del">
                                                        <EventMask Msg="载入中..." ShowMask="true" />
                                                    </Click>
                                                </AjaxEvents>
                                            </ext:Button>
                                           <ext:Button ID="btnCancle" runat="server" Text="返回" Icon="ArrowUndo">
                                            <AjaxEvents>
                                                <Click OnEvent="btnCancle_Click">
                                                </Click>
                                            </AjaxEvents>
                                        </ext:Button>
                                            <ext:ToolbarSeparator ID="ToolbarSeparator4" runat="server" />
                                        </Items>
                                    </ext:Toolbar>
                                </TopBar>
                                <ColumnModel ID="ColumnModel1" runat="server">
                                    <Columns>
                                        <ext:RowNumbererColumn Width="32" Resizable="true">
                                        </ext:RowNumbererColumn>
                                        <ext:Column Header="ID" Width="66" Align="Left" Sortable="true" MenuDisabled="true"
                                            ColumnID="ID" DataIndex="ID" Hidden="true">
                                        </ext:Column>
                                        <ext:Column Header="人员编码" Width="66" Align="Left" Sortable="true" MenuDisabled="true"
                                            ColumnID="USER_ID" DataIndex="USER_ID">
                                        </ext:Column>
                                        <ext:Column Header="人员姓名" Width="66" Align="Left" Sortable="true" MenuDisabled="true"
                                            ColumnID="USER_NAME" DataIndex="USER_NAME">
                                        </ext:Column>
                                        <ext:Column Header="科室名称" Width="120" Align="left" Sortable="true" MenuDisabled="true"
                                            ColumnID="DEPT_NAME" DataIndex="DEPT_NAME">
                                        </ext:Column>
                                        <ext:Column Header="开始时间" Width="120" Align="left" Sortable="true" MenuDisabled="true"
                                            ColumnID="ST_DATE" DataIndex="ST_DATE">
                                        </ext:Column>
                                         <ext:Column Header="结束时间" Width="120" Align="left" Sortable="true" MenuDisabled="true"
                                            ColumnID="END_DATE" DataIndex="END_DATE">
                                        </ext:Column>
                                        <ext:Column Header="修改人" Width="120" Align="left" Sortable="true" MenuDisabled="true"
                                            ColumnID="OPERATOR_USERID" DataIndex="OPERATOR_USERID">
                                        </ext:Column>
                                        <ext:Column Header="修改时间" Width="120" Align="left" Sortable="true" MenuDisabled="true"
                                            ColumnID="OPERATE_DATE" DataIndex="OPERATE_DATE">
                                        </ext:Column>
                                    </Columns>
                                </ColumnModel>
                                <SelectionModel>
                                    <ext:RowSelectionModel ID="rowselection" SingleSelect="true">
                                    </ext:RowSelectionModel>
                                </SelectionModel>
                                
                                <LoadMask ShowMask="true" />
                            </ext:GridPanel>
                        </ext:LayoutColumn>
                    </Columns>
                </ext:ColumnLayout>
            </Body>
        </ext:ViewPort>
    </div>
    <ext:Window ID="add_persons" runat="server" Icon="Group" Title="添加转科记录" Width="400" Height="400"
        AutoShow="false" Modal="true" CenterOnLoad="true" AutoScroll="true" ShowOnLoad="false"
        Resizable="true" StyleSpec="background-color:Transparent;" BodyStyle="background-color:Transparent;">
    </ext:Window>
    
    </form>
</body>
</html>