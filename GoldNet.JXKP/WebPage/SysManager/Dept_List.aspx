<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Dept_List.aspx.cs" Inherits="GoldNet.JXKP.WebPage.SysManager.Dept_List" %>

<%@ Register Assembly="Goldnet.Ext.Web" Namespace="Goldnet.Ext.Web" TagPrefix="ext" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>

    <script type="text/javascript">
        function backToList() {
            window.navigate("Dept_List.aspx");
        }
         var RefreshData = function() {
            Store1.reload();
        }   
    </script>

    <script type="text/javascript">
        function FormatRender(v, p, record, rowIndex) {
             var colors = ["red", "black","blue"];
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
        
        function FormatRendercode(v, p, record, rowIndex) {
            var colors = ["red", "black","blue"];
            if(record.data.HIS_DEPT_CODE==record.data.JX_DEPT_CODE)
            {
                var template = '<span style="color:{0};">{1}</span>';
                return String.format(template, colors[1], record.data.HIS_DEPT_CODE);
            }
            else
            {
                var templateb = '<span style="color:{0};">{1}</span>';
                return String.format(templateb, colors[2], record.data.HIS_DEPT_CODE);
            }
        }
             function FormatRendername(v, p, record, rowIndex) {
             var colors = ["red", "black","blue"];
            if(record.data.HIS_DEPT_NAME==record.data.JX_DEPT_NAME)
            {
            var template = '<span style="color:{0};">{1}</span>';
            return String.format(template, colors[1], record.data.HIS_DEPT_NAME);
            }
            else
            {
            var templateb = '<span style="color:{0};">{1}</span>';
            return String.format(templateb, colors[2], record.data.HIS_DEPT_NAME);
            }
           
        }
           function FormatRenderjxcode(v, p, record, rowIndex) {
             var colors = ["red", "black","blue"];
            if(record.data.HIS_DEPT_CODE==record.data.JX_DEPT_CODE)
            {
            var template = '<span style="color:{0};">{1}</span>';
            return String.format(template, colors[1], record.data.JX_DEPT_CODE);
            }
            else
            {
            var templateb = '<span style="color:{0};">{1}</span>';
            return String.format(templateb, colors[2], record.data.JX_DEPT_CODE);
            }
            }
             function FormatRenderjxname(v, p, record, rowIndex) {
             var colors = ["red", "black","blue"];
            if(record.data.HIS_DEPT_NAME==record.data.JX_DEPT_NAME)
            {
            var template = '<span style="color:{0};">{1}</span>';
            return String.format(template, colors[1], record.data.JX_DEPT_NAME);
            }
            else
            {
            var templateb = '<span style="color:{0};">{1}</span>';
            return String.format(templateb, colors[2], record.data.JX_DEPT_NAME);
            }
           
        }
        function ieSearch()
{
var wsh = new ActiveXObject("WScript.Shell");
wsh.sendKeys("^{f}");
}

    </script>

    <script type="text/javascript">
function encode(s){
  return s.replace(/&/g,"&").replace(/</g,"<").replace(/>/g,">").replace(/([\\\.\*\[\]\(\)\$\^])/g,"\\$1");
}
function decode(s){
  return s.replace(/\\([\\\.\*\[\]\(\)\$\^])/g,"$1").replace(/>/g,">").replace(/</g,"<").replace(/&/g,"&");
}
function highlight(){
  var s=document.getElementById("Fields").value;
  if (s.length==0){
    alert('搜索关键词未填写！');
    return false;
  }
  s=encode(s);
  var obj=document.getElementsByTagName("body")[0];
  var t=obj.innerHTML.replace(/<span\s+class=.?highlight.?>([^<>]*)<\/span>/gi,"$1");
  obj.innerHTML=t;
  var cnt=loopSearch(s,obj);
  t=obj.innerHTML
  var r=/{searchHL}(({(?!\/searchHL})|[^{])*){\/searchHL}/g
  t=t.replace(r,"<span class='highlight'>$1</span>");
  obj.innerHTML=t;
  alert("搜索到关键词"+cnt+"处");
   return false;
}
function loopSearch(s,obj){
  var cnt=0;
  if (obj.nodeType==3){
    cnt=replace(s,obj);
    return cnt;
  }
  for (var i=0,c;c=obj.childNodes[i];i++){
    if (!c.className||c.className!="highlight")
      cnt+=loopSearch(s,c);
  }
  return cnt;
}
function replace(s,dest){
  var r=new RegExp(s,"g");
  var tm=null;
  var t=dest.nodeValue;
  var cnt=0;
  if (tm=t.match(r)){
    cnt=tm.length;
    t=t.replace(r,"{searchHL}"+decode(s)+"{/searchHL}")
    dest.nodeValue=t;
  }
  return cnt;
}
    </script>

    <style type="text/css">
        .highlight
        {
            background: green;
            font-weight: bold;
            color: white;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <ext:ScriptManager ID="ScriptManager1" runat="server" AjaxMethodNamespace="Goldnet" />
        <ext:Store runat="server" ID="Store1" AutoLoad="true" OnRefreshData="Store_RefreshData"
            WarningOnDirty="false" GroupField="ACCOUNT_DEPT_NAME">
            <Reader>
                <ext:JsonReader ReaderID="DEPT_CODE">
                    <Fields>
                        <ext:RecordField Name="DEPT_CODE" Type="String" Mapping="DEPT_CODE" />
                        <ext:RecordField Name="DEPT_NAME" Type="String" Mapping="DEPT_NAME" />
                        <ext:RecordField Name="HIS_DEPT_CODE" Type="String" Mapping="HIS_DEPT_CODE" />
                        <ext:RecordField Name="JX_DEPT_CODE" Type="String" Mapping="JX_DEPT_CODE" />
                        <ext:RecordField Name="HIS_DEPT_NAME" Type="String" Mapping="HIS_DEPT_NAME" />
                        <ext:RecordField Name="JX_DEPT_NAME" Type="String" Mapping="JX_DEPT_NAME" />
                        <ext:RecordField Name="INPUT_CODE" Type="String" Mapping="INPUT_CODE" />
                        <ext:RecordField Name="P_DEPT_NAME" Type="String" Mapping="P_DEPT_NAME" />
                        <ext:RecordField Name="DEPT_TYPE" Type="String" Mapping="DEPT_TYPE" />
                        <ext:RecordField Name="DEPT_LCATTR" Type="String" Mapping="DEPT_LCATTR" />
                        <ext:RecordField Name="SORT_NO" Type="String" Mapping="SORT_NO" />
                        <ext:RecordField Name="SHOW_FLAG" Type="String" Mapping="SHOW_FLAG" />
                        <ext:RecordField Name="ATTR" Type="String" Mapping="ATTR" />
                        <ext:RecordField Name="ACCOUNT_DEPT_NAME" Type="String" Mapping="ACCOUNT_DEPT_NAME" />
                        <ext:RecordField Name="DEPT_NAME_SECOND" Type="String" Mapping="DEPT_NAME_SECOND" />
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
                                TrackMouseOver="true" Height="480" AutoWidth="true" AutoExpandColumn="INPUT_CODE"
                                AutoScroll="true">
                                <TopBar>
                                    <ext:Toolbar ID="Toolbar_detptype" runat="server" Visible="true" AutoWidth="true">
                                        <Items>
                                            <ext:Label ID="func" runat="server" Text="选择科室类别：" Width="40">
                                            </ext:Label>
                                            <ext:ComboBox ID="Combo_DeptType" runat="server" AllowBlank="true" EmptyText="请选择科室类别"
                                                Width="200" FieldLabel="科室类别">
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
                                            <ext:Button ID="Button1" runat="server" Text="查询" Icon="Zoom">
                                                <AjaxEvents>
                                                    <Click OnEvent="SelectedDepttype">
                                                        <EventMask Msg="载入中..." ShowMask="true" />
                                                    </Click>
                                                </AjaxEvents>
                                            </ext:Button>
                                            <ext:Button ID="Buttonset" runat="server" Text="设置" Icon="DatabaseKey">
                                                <AjaxEvents>
                                                    <Click OnEvent="Buttonset_Click">
                                                        <EventMask Msg="载入中..." ShowMask="true" />
                                                    </Click>
                                                </AjaxEvents>
                                            </ext:Button>
                                            <ext:Button ID="Buttonadd" runat="server" Text="添加" Icon="DatabaseAdd">
                                                <AjaxEvents>
                                                    <Click OnEvent="Buttonadd_Click">
                                                        <EventMask Msg="载入中..." ShowMask="true" />
                                                    </Click>
                                                </AjaxEvents>
                                            </ext:Button>
                                            <ext:Button ID="Buttondel" runat="server" Text="删除" Icon="DatabaseDelete">
                                                <AjaxEvents>
                                                    <Click OnEvent="Buttondel_Click">
                                                        <EventMask Msg="载入中..." ShowMask="true" />
                                                    </Click>
                                                </AjaxEvents>
                                            </ext:Button>
                                            <ext:TextField ID="Fields" runat="server" Width="30" Visible="false">
                                            </ext:TextField>
                                            <ext:Button ID="ButtonFind" runat="server" Text="查找" Icon="DatabaseKey" Visible="false">
                                                <Listeners>
                                                </Listeners>
                                            </ext:Button>
                                            <ext:ToolbarSeparator ID="ToolbarSeparator4" runat="server" />
                                        </Items>
                                    </ext:Toolbar>
                                </TopBar>
                                <ColumnModel ID="ColumnModel1" runat="server">
                                    <Columns>
                                        <ext:RowNumbererColumn Width="32" Resizable="true">
                                        </ext:RowNumbererColumn>
                                        <ext:Column Header="科室编码" Width="66" Align="Left" Sortable="true" MenuDisabled="true"
                                            ColumnID="DEPT_CODE" DataIndex="DEPT_CODE">
                                        </ext:Column>
                                        <ext:Column Header="科室名称" Width="120" Align="left" Sortable="true" MenuDisabled="true"
                                            ColumnID="DEPT_NAME" DataIndex="DEPT_NAME" Tooltip="科室名称">
                                            <Renderer Fn="FormatRender" />
                                        </ext:Column>
                                        <ext:Column Header="上级科室" Width="120" Align="left" Sortable="true" MenuDisabled="true"
                                            ColumnID="P_DEPT_NAME" DataIndex="P_DEPT_NAME">
                                        </ext:Column>
                                        <ext:Column Header="核算科室名称" Width="120" Align="left" Sortable="true" MenuDisabled="true"
                                            ColumnID="ACCOUNT_DEPT_NAME" DataIndex="ACCOUNT_DEPT_NAME">
                                        </ext:Column>
                                        <ext:Column Header="二级科室名称" Width="120" Align="left" Sortable="true" MenuDisabled="true"
                                            ColumnID="DEPT_NAME_SECOND" DataIndex="DEPT_NAME_SECOND">
                                        </ext:Column>
                                        <ext:Column Header="是否核算" Width="80" Align="left" Sortable="true" MenuDisabled="true"
                                            ColumnID="ATTR" DataIndex="ATTR">
                                        </ext:Column>
                                        <ext:Column Header="排列顺序" Width="80" Align="left" Sortable="true" MenuDisabled="true"
                                            ColumnID="SORT_NO" DataIndex="SORT_NO">
                                        </ext:Column>
                                        <ext:Column Header="输入码" Width="120" Align="left" Sortable="true" MenuDisabled="true"
                                            ColumnID="INPUT_CODE" DataIndex="INPUT_CODE">
                                        </ext:Column>
                                        <ext:Column Header="HIS科室编码" Width="100" Align="Left" Sortable="true" MenuDisabled="true"
                                            ColumnID="HIS_DEPT_CODE" DataIndex="HIS_DEPT_CODE">
                                            <Renderer Fn="FormatRendercode" />
                                        </ext:Column>
                                        <ext:Column Header="绩效科室编码" Width="100" Align="Left" Sortable="true" MenuDisabled="true"
                                            ColumnID="JX_DEPT_CODE" DataIndex="JX_DEPT_CODE">
                                            <Renderer Fn="FormatRenderjxcode" />
                                        </ext:Column>
                                        <ext:Column Header="HIS科室名称" Width="120" Align="left" Sortable="true" MenuDisabled="true"
                                            ColumnID="HIS_DEPT_NAME" DataIndex="HIS_DEPT_NAME">
                                            <Renderer Fn="FormatRendername" />
                                        </ext:Column>
                                        <ext:Column Header="绩效科室名称" Width="120" Align="left" Sortable="true" MenuDisabled="true"
                                            ColumnID="JX_DEPT_NAME" DataIndex="JX_DEPT_NAME">
                                            <Renderer Fn="FormatRenderjxname" />
                                        </ext:Column>
                                    </Columns>
                                </ColumnModel>
                                <SelectionModel>
                                    <ext:RowSelectionModel ID="rowselection" SingleSelect="true">
                                    </ext:RowSelectionModel>
                                </SelectionModel>
                                <AjaxEvents>
                                    <DblClick OnEvent="DbRowClick" />
                                </AjaxEvents>
                                <View>
                                    <ext:GroupingView ID="GroupingView1" HideGroupedColumn="true" runat="server" GroupTextTpl='{text} ({[values.rs.length]})'
                                        EnableRowBody="false">
                                    </ext:GroupingView>
                                </View>
                                <LoadMask ShowMask="true" />
                                <BottomBar>
                                    <ext:PagingToolbar ID="PagingToolBar2" runat="server" PageSize="20" StoreID="Store1"
                                        AutoWidth="true" DisplayInfo="false" AutoDataBind="true">
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
                                    </ext:PagingToolbar>
                                </BottomBar>
                            </ext:GridPanel>
                        </ext:LayoutColumn>
                    </Columns>
                </ext:ColumnLayout>
            </Body>
        </ext:ViewPort>
    </div>
    <ext:Window ID="Dept_Set" runat="server" Icon="Group" Title="科室设置" Width="400" Height="400"
        AutoShow="false" Modal="true" CenterOnLoad="true" AutoScroll="true" ShowOnLoad="false"
        Resizable="true" StyleSpec="background-color:Transparent;" BodyStyle="background-color:Transparent;">
    </ext:Window>
    </form>
</body>
</html>
