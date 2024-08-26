<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="QualityBuide.aspx.cs" Inherits="GoldNet.JXKP.zlgl.SysManage.QualityBuide" %>

<%@ Register Assembly="Goldnet.Ext.Web" Namespace="Goldnet.Ext.Web" TagPrefix="ext" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
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
        function backToList() {
            window.navigate("GuideType.aspx");
        }
         var RefreshData = function() {
            Store3.reload();
            Store2.reload();
            Store1.reload();
        }
        function edit(id,straction) {
            window.navigate("GuideTypeEdit.aspx?id="+id+"&straction="+straction);
        }
    </script>

</head>
<body>
    <form id="form1" runat="server">
    <div>
        <ext:ScriptManager ID="ScriptManager1" runat="server" AjaxMethodNamespace="CompanyX" />
        <ext:Store ID="Store1" runat="server" AutoLoad="true" OnRefreshData="Store_RefreshData">
            <Reader>
                <ext:JsonReader ReaderID="科室">
                    <Fields>
                    </Fields>
                </ext:JsonReader>
            </Reader>
        </ext:Store>
        <ext:Store ID="Store2" runat="server" AutoLoad="false">
            <Proxy>
            </Proxy>
            <Reader>
            </Reader>
        </ext:Store>
        <ext:Store ID="Store3" runat="server" AutoLoad="true" OnRefreshData="Storedate_RefreshData"
            WarningOnDirty="false">
            <Reader>
                <ext:JsonReader ReaderID="ID">
                    <Fields>
                        <ext:RecordField Name="ID" />
                        <ext:RecordField Name="DATEDESC" />
                    </Fields>
                </ext:JsonReader>
            </Reader>
        </ext:Store>
        <ext:ViewPort ID="ViewPort1" runat="server">
            <Body>
                <ext:ColumnLayout ID="ColumnLayout1" runat="server" Split="true" FitHeight="true">
                    <Columns>
                        <ext:LayoutColumn ColumnWidth="1">
                            <ext:GridPanel ID="GridPanel1" runat="server" Border="false" StoreID="Store1" StripeRows="true"
                                Height="480" AutoWidth="true">
                                <TopBar>
                                    <ext:Toolbar ID="Toolbar_guidetype" runat="server" Visible="true" AutoWidth="true">
                                        <Items>
                                            <ext:ToolbarSpacer ID="ToolbarSpacer5" runat="server" Width="6" />
                                            <ext:ToolbarSeparator ID="ToolbarSeparator3" runat="server" />
                                            <ext:ComboBox ID="date" runat="server" AllowBlank="false" StoreID="Store3" Width="100"
                                                DisplayField="DATEDESC" ValueField="ID" ReadOnly="true" EmptyText="选择时间" AutoDataBind="true">
                                            </ext:ComboBox>
                                            <ext:ComboBox ID="ComAccountdeptcode" runat="server" StoreID="Store2" DisplayField="DEPT_NAME"
                                                ValueField="DEPT_CODE" TypeAhead="false" LoadingText="Searching..." Width="100"
                                                PageSize="10" HideTrigger="false" ItemSelector="div.search-item" MinChars="1"
                                                ListWidth="300" EmptyText="选择科室">
                                                <Template ID="Template2" runat="server">
                                                   <tpl for=".">
                                                      <div class="search-item">
                                                         <h3><span>
                                                          {DEPT_CODE}</span>{DEPT_NAME}</h3>
                                                      </div>
                                                   </tpl>
                                                </Template>
                                            </ext:ComboBox>
                                            <ext:ComboBox ID="ComGuide" runat="server" AllowBlank="true" Width="100" EmptyText="选择指标">
                                            </ext:ComboBox>
                                            <ext:Button ID="Buttonedit" runat="server" Text="查询" Icon="Zoom">
                                                <AjaxEvents>
                                                    <Click OnEvent="GetQueryPortalet">
                                                    </Click>
                                                </AjaxEvents>
                                            </ext:Button>
                                            <ext:Button ID="buidequality" runat="server" Text="生成质量数据" Icon="Build">
                                                <AjaxEvents>
                                                    <Click OnEvent="buidequality_Click">
                                                    </Click>
                                                </AjaxEvents>
                                            </ext:Button>
                                            <ext:FileUploadField ID="photoimg" runat="server" ButtonOnly="true" ButtonText="上传附件"
                                                Icon="ImageAdd" Hidden="true">
                                                <AjaxEvents>
                                                    <FileSelected OnEvent="UploadClick">
                                                    </FileSelected>
                                                </AjaxEvents>
                                            </ext:FileUploadField>
                                            <ext:Button ID="ddd" runat="server" Text="下载附件" Icon="DateLink">
                                            <AjaxEvents>
                                            <Click OnEvent="DownClick"></Click>
                                            </AjaxEvents>
                                            </ext:Button>
                                            <ext:ToolbarSeparator ID="ToolbarSeparator1" runat="server" />
                                        </Items>
                                    </ext:Toolbar>
                                </TopBar>
                                <ColumnModel ID="ColumnModel1" runat="server">
                                    <Columns>
                                    </Columns>
                                </ColumnModel>
                                <SelectionModel>
                                    <ext:RowSelectionModel ID="rowselection" SingleSelect="true">
                                        <AjaxEvents>
                                            <RowSelect OnEvent="RowSelect" Buffer="250">
                                                <EventMask ShowMask="true" Target="CustomTarget" CustomTarget="#{Store1}" />
                                                <ExtraParams>
                                                    <ext:Parameter Name="ID" Value="this.getSelected().科室" Mode="Raw" />
                                                </ExtraParams>
                                            </RowSelect>
                                        </AjaxEvents>
                                    </ext:RowSelectionModel>
                                </SelectionModel>
                                <AjaxEvents>
                                    <DblClick OnEvent="DbRowClick" />
                                </AjaxEvents>
                                <Listeners>
                                    <Command Handler="edit(record.data.科室,'edit');" />
                                </Listeners>
                                <LoadMask ShowMask="true" />
                            </ext:GridPanel>
                        </ext:LayoutColumn>
                    </Columns>
                </ext:ColumnLayout>
            </Body>
        </ext:ViewPort>
    </div>
    <ext:Window ID="guideDetail" runat="server" Icon="Group" Title="科室分项得分" Width="800"
        Height="500" AutoShow="false" Modal="true" CenterOnLoad="true" AutoScroll="true"
        ShowOnLoad="false" Resizable="true" StyleSpec="background-color:Transparent;"
        BodyStyle="background-color:Transparent;">
    </ext:Window>
    <ext:Window ID="BuideWin" runat="server" Icon="Add" Title="生成质量数据" Width="600" Height="400"
        AutoShow="false" Modal="true" CenterOnLoad="true" AutoScroll="false" ShowOnLoad="false"
        Closable="false" Resizable="false" StyleSpec="background-color:Transparent;"
        BodyStyle="background-color:Transparent;" CloseAction="Hide">
    </ext:Window>
    </form>
</body>
</html>
