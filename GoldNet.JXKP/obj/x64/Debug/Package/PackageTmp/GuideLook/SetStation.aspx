<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SetStation.aspx.cs" Inherits="GoldNet.JXKP.GuideLook.SetStation" %>

<%@ Register Assembly="Goldnet.Ext.Web" Namespace="Goldnet.Ext.Web" TagPrefix="ext" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>无标题页</title>

    <script type="text/javascript">      
         var SelectorLayout = function() {
             SelectorLeft.setHeight(Ext.lib.Dom.getViewHeight() - SelectorLeft.getPosition()[1]- 5);
             SelectorRight.setHeight(Ext.lib.Dom.getViewHeight() - SelectorRight.getPosition()[1]- 5);
         }
         var TwoSideSelector = {
             add: function(source, destination) {
                 source = source || SelectorLeft;
                 destination = destination || SelectorRight;
                 var selectionsArray = source.view.getSelectedIndexes();
                 var records = [];
                 if (selectionsArray.length > 0) {
                     for (var i = 0; i < selectionsArray.length; i++) {
                         var rec = source.view.store.getAt(selectionsArray[i]);
                         destination.store.add(rec);
                         records.push(rec);
                     }
                     for (var i = 0; i < selectionsArray.length; i++) {
                         source.store.remove(records[i]);
                     }
                 }
             },
             addAll: function(source, destination) {
                 source = source || SelectorLeft;
                 destination = destination || SelectorRight;
                 destination.store.add(source.store.getRange());
                 source.store.removeAll();
             },
             remove: function() {
                 var source = SelectorLeft;
                 var destination = SelectorRight;
                 this.add(destination, source);
             },
             removeAll: function() {
                 var source = SelectorLeft;
                 var destination = SelectorRight;
                 this.addAll(destination, source);
             }
         };
         
        
        function getStaffCheckedNode() {
            var result = "";
            var checkeds = Ext.getCmp('TreePanel1').getChecked();
            for (var i = 0; i < checkeds.length; i++) {
                result = result + checkeds[i].id + ";";
            }
            return result;
        }
        
        var nodeState = function(node) {
            var box = node.getUI().checkbox;
            if (typeof box == 'undefined') return;
            if (box.checked) {
                return 1;
            } else if (box.indeterminate) {
                return 2;
            } else {
                return 3;
            }
        }
        var siblState = function(node) {
            var state = new Array();
            var firstNode = node.parentNode.firstChild;
            if (!firstNode) {
                return false;
            }
            do {
                state.push(nodeState(firstNode));
                firstNode = firstNode.nextSibling;
            } while (firstNode != null)
            return state;
        }
        var parentState = function(node) {
            var state = siblState(node).join();
            if (state.indexOf("3") == -1 && state.indexOf("2") == -1) {
                return 1;
            } else if (state.indexOf("1") == -1 && state.indexOf("2") == -1) {
                return -1;
            } else {
                return 0;
            }
        }
        var parentChecked = function(node) {
            var parentNode = node.parentNode;
            if (parentNode == null)  return false;
            var checkbox = parentNode.getUI().checkbox;
            if (typeof checkbox == 'undefined')  return false;
            var check = parentState(node);
            if (check == 1) {
                checkbox.indeterminate = false;
                checkbox.checked = true;
            } else if (check == -1) {
                checkbox.checked = false;
                checkbox.indeterminate = false;
            } else {
                checkbox.checked = false;
                checkbox.indeterminate = true;
            }
            parentChecked(parentNode);
        }

        //选择改变事件
        function ToCheckChange(node, checked) {
            if (checked) {
                node.expand();
                node.eachChild(function(child) {
                //toggleCheck将嵌套触发checkchange事件
                child.ui.toggleCheck(checked);
//                child.ui.checkbox.checked = checked;
//                child.ui.node.attributes.checked = checked;
                });
                parentChecked(node);
            }
            else {
                node.collapse();
                node.eachChild(function(child) {
                //toggleCheck将嵌套触发checkchange事件
                child.ui.toggleCheck(checked);
//                child.ui.checkbox.checked = checked;
//                child.ui.node.attributes.checked = checked;
                });
                parentChecked(node);
            }
        }
        
        
                     
    </script>

    <style type="text/css">
        .icon-expand-all
        {
            background-image: url(/resources/images/expand-all.gif) !important;
        }
        .icon-collapse-all
        {
            background-image: url(/resources/images/collapse-all.gif) !important;
        }
        table
        {
            font-size: 12px;
            background-color: #dfe8f6;
        }
        body
        {
            background-color: #DFE8F6;
            font-size: 12px;
        }
        .x-form-group .x-form-group-header-text
        {
            background-color: #dfe8f6;
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
</head>
<body>
    <form id="form2" runat="server">
    <ext:ScriptManager ID="ScriptManager1" runat="server">
        <Listeners>
            <DocumentReady Handler=" Ext.EventManager.onWindowResize(SelectorLayout);" />
        </Listeners>
    </ext:ScriptManager>
    <ext:Hidden ID="hideCode" runat="server">
    </ext:Hidden>
    <ext:Store runat="server" ID="Store1">
        <Reader>
            <ext:JsonReader>
                <Fields>
                    <ext:RecordField Name="TEXT" />
                    <ext:RecordField Name="VALUE" />
                </Fields>
            </ext:JsonReader>
        </Reader>
    </ext:Store>
    <ext:Store runat="server" ID="Store2">
        <Reader>
            <ext:JsonReader>
                <Fields>
                    <ext:RecordField Name="TEXT" />
                    <ext:RecordField Name="VALUE" />
                </Fields>
            </ext:JsonReader>
        </Reader>
    </ext:Store>
    <ext:Store ID="Store3" runat="server" AutoLoad="true">
        <Proxy>
            <ext:HttpProxy Method="POST" Url="WebService/StaffDepts.ashx" />
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
    <ext:Store ID="StoreCombo" runat="server">
        <Reader>
            <ext:JsonReader>
                <Fields>
                    <ext:RecordField Name="VALUE" />
                    <ext:RecordField Name="TEXT" />
                </Fields>
            </ext:JsonReader>
        </Reader>
    </ext:Store>
   <ext:Store ID="StoreStation" runat="server">
        <Reader>
            <ext:JsonReader>
                <Fields>
                    <ext:RecordField Name="VALUE" />
                    <ext:RecordField Name="TEXT" />
                </Fields>
            </ext:JsonReader>
        </Reader>
    </ext:Store>
    <div>
        <ext:Panel ID="Panel1" runat="server" Border="false" Header="false" AutoShow="false"
            Width="760" Height="350" BodyStyle="background-color:transparent">
            <Body>
                <ext:Panel ID="SFM_DefineDetailsWindow" runat="server" Height="350" Width="760" Border="false"
                    BodyStyle="background-color:transparent">
                    <Body>
                        <ext:ColumnLayout ID="ColumnLayout1" runat="server" FitHeight="true">
                            <ext:LayoutColumn ColumnWidth="0.33">
                                <ext:Panel ID="panel2" runat="server" Border="false" Width="300" BodyStyle="background-color:transparent">
                                    <Body>
                                        <ext:FieldSet ID="fieldset2" runat="server" Title="人员信息" Width="230" Collapsed="false"
                                            StyleSpec="margin:2px;padding-left:2px;" BodyStyle="background-color:Transparent;">
                                            <Body>
                                                <table width="100%">
                                                    <tr>
                                                        <td style="width: 45%">
                                                            人员类别
                                                        </td>
                                                        <td>
                                                            <ext:ComboBox ID="cbx_Ptype" runat="server" Width="50" FieldLabel="人员类别">
                                                            </ext:ComboBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            技术级
                                                        </td>
                                                        <td>
                                                            <ext:ComboBox ID="cbx_PTechType" runat="server" Width="50" FieldLabel="技术级">
                                                            </ext:ComboBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            技职类别
                                                        </td>
                                                        <td>
                                                            <ext:ComboBox ID="cbx_PTech" runat="server" Width="50" FieldLabel="技职类别">
                                                            </ext:ComboBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            技术职务时间:
                                                        </td>
                                                        <td>
                                                            <table cellpadding="0" cellspacing="0">
                                                                <tr>
                                                                    <td>
                                                                        <ext:ComboBox ID="cbx_TimeOrgan" runat="server" Width="50" ListWidth="50">
                                                                            <Items>
                                                                                <ext:ListItem Text="全部" Value="" />
                                                                                <ext:ListItem Text="晚于" Value=">=" />
                                                                                <ext:ListItem Text="早于" Value="<=" />
                                                                                <ext:ListItem Text="为" Value="=" />
                                                                            </Items>
                                                                        </ext:ComboBox>
                                                                    </td>
                                                                    <td>
                                                                        <ext:DateField ID="timer" runat="server" Width="85" Format="yyyy-MM-dd">
                                                                        </ext:DateField>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            学位
                                                        </td>
                                                        <td>
                                                            <ext:ComboBox ID="cbx_PCollage" runat="server" Width="70" FieldLabel="学位">
                                                            </ext:ComboBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            技术系列
                                                        </td>
                                                        <td>
                                                            <ext:ComboBox ID="cbx_PLevel" runat="server" Width="50" FieldLabel="技术系列">
                                                            </ext:ComboBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            科室信息
                                                        </td>
                                                        <td>
                                                            <ext:ComboBox ID="DeptCodeCombo" runat="server" StoreID="Store3" DisplayField="DEPT_NAME"
                                                                Width="120" ValueField="DEPT_CODE" TypeAhead="false" LoadingText="Searching..."
                                                                PageSize="1000" ItemSelector="div.search-item" MinChars="1" FieldLabel="科室信息"
                                                                ListWidth="240">
                                                                <Template ID="Template1" runat="server">
                                                                   <tpl for=".">
                                                                      <div class="search-item">
                                                                         <h3><span style="width:auto">{DEPT_CODE}</span>{DEPT_NAME}</h3>
                                                                      </div>
                                                                   </tpl>
                                                                </Template>
                                                                <AjaxEvents>
                                                                    <Select OnEvent="QueryStation"></Select>
                                                                </AjaxEvents>
                                                            </ext:ComboBox>
                                                        </td>
                                                    </tr>
                                                     <tr>
                                                        <td>
                                                            岗位信息
                                                        </td>
                                                        <td>
                                                            <ext:ComboBox ID="cboStation" runat="server" StoreID="StoreStation" DisplayField="TEXT"
                                                                Width="120" ValueField="VALUE">
                                                            </ext:ComboBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                        </td>
                                                        <td>
                                                            <ext:Button ID="btn_Query" runat="server" Icon="DatabaseGo" Text="查询">
                                                                <AjaxEvents>
                                                                    <Click OnEvent="QueryStaff">
                                                                        <ExtraParams>
                                                                            <ext:Parameter Name="multi1" Value="Ext.encode(#{SelectorRight}.getValues())" Mode="Raw" />
                                                                        </ExtraParams>
                                                                        <EventMask ShowMask="true" Msg="请稍候..." />
                                                                    </Click>
                                                                </AjaxEvents>
                                                            </ext:Button>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </Body>
                                        </ext:FieldSet>
                                    </Body>
                                </ext:Panel>
                            </ext:LayoutColumn>
                            <ext:LayoutColumn ColumnWidth="0.33">
                                <ext:Panel ID="Panel59" runat="server" Border="false" MonitorResize="true" Width="200"
                                    BodyStyle="background-color: transparent;">
                                    <Body>
                                        <ext:MultiSelect ID="SelectorLeft" runat="server" Legend="待选信息" DragGroup="grp1"
                                            DropGroup="grp2,grp1" StoreID="Store1" DisplayField="TEXT" ValueField="VALUE"
                                            EnableViewState="true" Stateful="true" AutoWidth="true" Height="250" KeepSelectionOnClick="WithCtrlKey">
                                        </ext:MultiSelect>
                                    </Body>
                                </ext:Panel>
                            </ext:LayoutColumn>
                            <ext:LayoutColumn>
                                <ext:Panel ID="Panel4" runat="server" Width="35" BodyStyle="background-color: transparent;"
                                    Border="false">
                                    <Body>
                                        <ext:AnchorLayout ID="AnchorLayout1" runat="server">
                                            <ext:Anchor Vertical="20%">
                                                <ext:Panel ID="Panel5" runat="server" Border="false" BodyStyle="background-color: transparent;" />
                                            </ext:Anchor>
                                            <ext:Anchor>
                                                <ext:Panel ID="Panel6" runat="server" Border="false" BodyStyle="padding:5px;background-color: transparent;">
                                                    <Body>
                                                        <ext:Button ID="Button1" runat="server" Icon="ResultsetNext" StyleSpec="margin-bottom:2px;">
                                                            <Listeners>
                                                                <Click Handler="TwoSideSelector.add();" />
                                                            </Listeners>
                                                            <ToolTips>
                                                                <ext:ToolTip ID="ToolTip1" runat="server" Title="添加" Html="添加左侧选中行" />
                                                            </ToolTips>
                                                        </ext:Button>
                                                        <ext:Button ID="Button2" runat="server" Icon="ResultsetLast" StyleSpec="margin-bottom:2px;">
                                                            <Listeners>
                                                                <Click Handler="TwoSideSelector.addAll();" />
                                                            </Listeners>
                                                            <ToolTips>
                                                                <ext:ToolTip ID="ToolTip2" runat="server" Title="添加全部" Html="添加左侧全部" />
                                                            </ToolTips>
                                                        </ext:Button>
                                                        <ext:Button ID="Button3" runat="server" Icon="ResultsetPrevious" StyleSpec="margin-bottom:2px;">
                                                            <Listeners>
                                                                <Click Handler="TwoSideSelector.remove();" />
                                                            </Listeners>
                                                            <ToolTips>
                                                                <ext:ToolTip ID="ToolTip3" runat="server" Title="移除" Html="移除右侧选中行" />
                                                            </ToolTips>
                                                        </ext:Button>
                                                        <ext:Button ID="Button4" runat="server" Icon="ResultsetFirst" StyleSpec="margin-bottom:2px;">
                                                            <Listeners>
                                                                <Click Handler="TwoSideSelector.removeAll();" />
                                                            </Listeners>
                                                            <ToolTips>
                                                                <ext:ToolTip ID="ToolTip4" runat="server" Title="移除全部" Html="移除右侧全部" />
                                                            </ToolTips>
                                                        </ext:Button>
                                                    </Body>
                                                </ext:Panel>
                                            </ext:Anchor>
                                        </ext:AnchorLayout>
                                    </Body>
                                </ext:Panel>
                            </ext:LayoutColumn>
                            <ext:LayoutColumn ColumnWidth="0.33">
                                <ext:Panel ID="Panel7" runat="server" Border="false" Width="200" BodyStyle="background-color: transparent;">
                                    <Body>
                                        <ext:MultiSelect ID="SelectorRight" runat="server" Legend="已选信息" DragGroup="grp2"
                                            DropGroup="grp1,grp2" StoreID="Store2" DisplayField="TEXT" ValueField="VALUE"
                                            AutoWidth="true" Height="250" KeepSelectionOnClick="WithCtrlKey">
                                        </ext:MultiSelect>
                                        <table>
                                            <tr>
                                                <td>
                                                    <ext:Checkbox ID="ckx_Sort" runat="server" Checked="true">
                                                    </ext:Checkbox>
                                                </td>
                                                <td>
                                                    <ext:Label ID="Label2" Text="不包含已选信息(反向选择)" ForID="ckx_Sort" runat="server">
                                                    </ext:Label>
                                                </td>
                                            </tr>
                                        </table>
                                    </Body>
                                </ext:Panel>
                            </ext:LayoutColumn>
                        </ext:ColumnLayout>
                    </Body>
                    <BottomBar>
                        <ext:Toolbar ID="ToolBar2" runat="server" Height="25">
                            <Items>
                                <ext:ToolbarButton ID="btnPrev" runat="server" Text="上一步" Icon="PreviousGreen" MenuAlign="right">
                                    <AjaxEvents>
                                        <Click OnEvent="Prev_Click">
                                            <ExtraParams>
                                                <ext:Parameter Name="multi1" Value="Ext.encode(#{SelectorRight}.getValues())" Mode="Raw" />
                                            </ExtraParams>
                                        </Click>
                                    </AjaxEvents>
                                </ext:ToolbarButton>
                                <ext:ToolbarFill ID="Toolba" runat="server" />
                                <ext:ToolbarButton ID="btnNext" runat="server" Text="保存" Icon="Disk" MenuAlign="right">
                                    <AjaxEvents>
                                        <Click OnEvent="Next_Click">
                                            <ExtraParams>
                                                <ext:Parameter Name="multi1" Value="Ext.encode(#{SelectorRight}.getValues())" Mode="Raw" />
                                                <ext:Parameter Name="multi2" Value="Ext.encode(#{SelectorLeft}.getValues())" Mode="Raw" />
                                            </ExtraParams>
                                            <EventMask ShowMask="true" Msg="请稍候..." />
                                        </Click>
                                    </AjaxEvents>
                                </ext:ToolbarButton>
                            </Items>
                        </ext:Toolbar>
                    </BottomBar>
                </ext:Panel>
            </Body>
        </ext:Panel>
    </div>
    </form>
</body>
</html>
