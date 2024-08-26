<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="QualitySearchPersonByData.aspx.cs"
    Inherits="GoldNet.JXKP.zlgl.SysManage.QualitySearchPersonByData" %>

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
            Store1.reload();
            Store2.reload();
            Store3.reload();
        }
        function edit(id,straction) {
            window.navigate("GuideTypeEdit.aspx?id="+id+"&straction="+straction);
        }
    </script>
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
              
               v = whole + sub;
               if (v.charAt(0) == '-') {
                   return '-' + v.substr(1);
               }
               return v;
       }
   </script>
    <script type="text/javascript">
        var CheckForm = function() {
            if (ComGuide.validate() == false) {
                return false;
            }
            if (commonguide.validate() == false) {
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
        <ext:Store ID="Store0" runat="server" AutoLoad="false">
            <Reader>
                <ext:JsonReader Root="list" TotalProperty="totalCount" ReaderID="STAFF_ID">
                    <Fields>
                        <ext:RecordField Name="STAFF_ID" />
                        <ext:RecordField Name="NAME" />
                        <ext:RecordField Name="DEPT_NAME" />
                    </Fields>
                </ext:JsonReader>
            </Reader>
        </ext:Store>
        <ext:Store ID="Store1" runat="server" AutoLoad="true" OnRefreshData="Store_RefreshData">
            <Reader>
                <ext:JsonReader>
                    <Fields>
                        <ext:RecordField Name="DEPTNAME" />
                        <ext:RecordField Name="GUIDETYPE" />
                        <ext:RecordField Name="COMMGUIDE" />
                        <ext:RecordField Name="PERSON" />
                        <ext:RecordField Name="SORNUM"/>
                        <ext:RecordField Name="ALLSORNUM">
                         
                        </ext:RecordField>
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
        <ext:Store ID="Store3" runat="server" AutoLoad="true" OnRefreshData="Storedate_RefreshData">
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
                                 AutoWidth="true">
                                <TopBar>
                                    <ext:Toolbar ID="Toolbar_guidetype" runat="server" Visible="true" AutoWidth="true">
                                        <Items>
                                            <ext:ToolbarSpacer ID="ToolbarSpacer5" runat="server" Width="6" />
                                            <ext:ToolbarSeparator ID="ToolbarSeparator3" runat="server" />
                                            <ext:DateField ID="stardate" runat="server" FieldLabel="开始时间：" Width="100"  EnableKeyEvents="true"
                                                ReadOnly="true" />
                                            <ext:KeyNav ID="stardate1" runat="server" Target="stardate">
                                                <Enter Handler="var str = document.getElementById('stardate').value ; var   reg=/^(\d{4})(\d{2})(\d{2})$/; document.getElementById('stardate').value   =   str.replace(reg, '$1-$2-$3');" />
                                            </ext:KeyNav>
                                            <ext:DateField ID="enddate" runat="server" FieldLabel="结束时间：" Width="100"   EnableKeyEvents="true"
                                                ReadOnly="true"  />
                                            <ext:KeyNav ID="enddate1" runat="server" Target="enddate">
                                                <Enter Handler="var str = document.getElementById('enddate').value ; var   reg=/^(\d{4})(\d{2})(\d{2})$/; document.getElementById('enddate').value   =   str.replace(reg, '$1-$2-$3');" />
                                            </ext:KeyNav>
                                            <ext:Label ID="Label4" runat="server" Text="科室：">
                                            </ext:Label>
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
                                            <ext:Label ID="Label3" runat="server" Text="责任人：">
                                            </ext:Label>
                                            <ext:ComboBox ID="Com_Director" runat="server" StoreID="Store0" DisplayField="NAME"
                                                ValueField="STAFF_ID" TypeAhead="false" LoadingText="Searching..." Width="180"
                                                PageSize="10" HideTrigger="false" ItemSelector="div.search-item" MinChars="1"
                                                 EmptyText ="选择人员" ListWidth="220">
                                                <Template ID="Template1" runat="server">
                                                   <tpl for=".">
                                                      <div class="search-item">
                                                         <h3><span>{DEPT_NAME}</span>{NAME}</h3>
                                                  
                                                      </div>
                                                   </tpl>
                                                </Template>
                                            </ext:ComboBox>
                                            <ext:Label ID="Label1" runat="server" Text="考核大项：">
                                            </ext:Label>
                                            <ext:ComboBox ID="ComGuide" runat="server" AllowBlank="false" Width="100" EmptyText="选择大项">
                                                <AjaxEvents>
                                                    <Select OnEvent="SelectedGuodeType">
                                                        <EventMask ShowMask="true" />
                                                    </Select>
                                                </AjaxEvents>
                                            </ext:ComboBox>
                                            <ext:Label ID="Label2" runat="server" Text="考核小项：">
                                            </ext:Label>
                                            <ext:ComboBox ID="commonguide" runat="server" AllowBlank="false" Width="100" EmptyText="选择小项">
                                            </ext:ComboBox>
                                            <ext:Button ID="Buttonedit" runat="server" Text="查询" Icon="Zoom">
                                                <AjaxEvents>
                                                    <Click OnEvent="GetQueryPortalet" Before="if (CheckForm()== false){ Ext.Msg.show({title:'系统提示',msg:'请根据红线提示填写正确的信息！',icon: 'ext-mb-info',buttons: { ok: true }});return false;};">
                                                     <EventMask Msg="载入中..." ShowMask="true" />
                                                    </Click>
                                                </AjaxEvents>
                                            </ext:Button>
                                            <ext:ToolbarSeparator ID="ToolbarSeparator4" runat="server" />
                                        </Items>
                                    </ext:Toolbar>
                                </TopBar>
                                <ColumnModel ID="ColumnModel1" runat="server">
                                    <Columns>
                                        <ext:Column ColumnID="DEPTNAME" Header="科室" Width="200" Align="left" Sortable="true"
                                            DataIndex="DEPTNAME" MenuDisabled="true" />
                                        <ext:Column ColumnID="GUIDETYPE" Header="大类" Width="300" Align="left" Sortable="true"
                                            DataIndex="GUIDETYPE" MenuDisabled="true" />
                                        <ext:Column ColumnID="COMMGUIDE" Header="小类" Width="100" Align="right" Sortable="true"
                                            DataIndex="COMMGUIDE" MenuDisabled="true" />
                                        <ext:Column ColumnID="PERSON" Header="责任人" Width="100" Align="right" Sortable="true"
                                            DataIndex="PERSON" MenuDisabled="true" />
                                        <ext:Column ColumnID="SORNUM" Header="考评次数" Width="100" Align="right" Sortable="true"
                                            DataIndex="SORNUM" MenuDisabled="true" />
                                        <ext:Column ColumnID="ALLSORNUM" Header="总分" Width="100" Align="right" Sortable="true"
                                            DataIndex="ALLSORNUM" MenuDisabled="true">
                                            <Renderer Fn="rmbMoney" />
                                            </ext:Column>
                                    </Columns>
                                </ColumnModel>
                            
                            </ext:GridPanel>
                        </ext:LayoutColumn>
                    </Columns>
                </ext:ColumnLayout>
            </Body>
        </ext:ViewPort>
    </div>
    </form>
</body>
</html>
