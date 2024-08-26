<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Eval_Person_Selector.aspx.cs"
    Inherits="GoldNet.JXKP.jxkh.Eval_Person_Selector" %>

<%@ Register Assembly="Goldnet.Ext.Web" Namespace="Goldnet.Ext.Web" TagPrefix="ext" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>选择评价人员</title>

    <script type="text/javascript">
        var SelectorLayout = function() {
            SelectorLeft.setHeight(Ext.lib.Dom.getViewHeight() - SelectorLeft.getPosition()[1] - 10);
            SelectorRight.setHeight(Ext.lib.Dom.getViewHeight() - SelectorRight.getPosition()[1] - 10);
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
                var records = source.store.getRange();
                destination.store.add(records);
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
       

        var loadSelected = function() {
            if (parent.DetailWin == null) return;
            var records = parent.SelectorLeft.store.getRange();
            SelectorRight.store.removeAll();
            for (var i = 0; i < records.length; i++) {
                SelectorRight.store.add(new Ext.data.Record({ PERSON_NAME: records[i].data.text, PERSON_CODE: records[i].data.value }));
            }
        };

        var selectDone = function() {
            if (parent.DetailWin == null) return;
            var records = [];
            var records1 = SelectorRight.store.getRange();
            if (records1.length > 0) {
                for (var i = 0; i < records1.length; i++) {
                    var rec = [];
                    rec.push(records1[i].data.PERSON_NAME);
                    rec.push(records1[i].data.PERSON_CODE);
                    records.push(rec);
                }
            }
            parent.addItems(parent.SelectorLeft, records)
            parent.DetailWin.hide();
        };
    </script>

    <style type="text/css">
        table
        {
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
    <form id="form1" runat="server">
    <ext:ScriptManager ID="ScriptManager1" runat="server">
        <Listeners>
            <DocumentReady Handler=" Ext.EventManager.onWindowResize(SelectorLayout); loadSelected();" />
        </Listeners>
    </ext:ScriptManager>
    <ext:Store runat="server" ID="Store1" AutoLoad="false">
        <Reader>
            <ext:JsonReader>
                <Fields>
                    <ext:RecordField Name="PERSON_NAME" />
                    <ext:RecordField Name="PERSON_CODE" />
                </Fields>
            </ext:JsonReader>
        </Reader>
    </ext:Store>
    <ext:Store ID="Store3" runat="server" AutoLoad="true">
        <Reader>
            <ext:JsonReader Root="Staffdepts" TotalProperty="totalCount">
                <Fields>
                    <ext:RecordField Name="DEPT_NAME" />
                    <ext:RecordField Name="DEPT_CODE" />
                </Fields>
            </ext:JsonReader>
        </Reader>
    </ext:Store>
    <div>
        <ext:ViewPort ID="ViewPort1" runat="server">
            <Body>
                <ext:ColumnLayout ID="ColumnLayout2" runat="server" Split="true" FitHeight="true">
                    <Columns>
                        <ext:LayoutColumn ColumnWidth="1">
                            <ext:Panel ID="Panel11" runat="server" Width="400" Height="300" BodyBorder="false">
                                <TopBar>
                                    <ext:Toolbar runat="server" ID="ctl155">
                                        <Items>
                                            <ext:ToolbarSeparator ID="ToolbarSeparator3" runat="server">
                                            </ext:ToolbarSeparator>
                                            <ext:ToolbarButton ID="btn_Query" runat="server" Icon="DatabaseGo" Text="按条件查询人员">
                                                <AjaxEvents>
                                                    <Click OnEvent="QueryStaff">
                                                        <ExtraParams>
                                                            <ext:Parameter Name="multi1" Value="Ext.encode(#{SelectorRight}.getValues())" Mode="Raw" />
                                                        </ExtraParams>
                                                        <EventMask ShowMask="true" Msg="请稍候..." Target="CustomTarget" CustomTarget="#{SelectorLeft}" />
                                                    </Click>
                                                </AjaxEvents>
                                            </ext:ToolbarButton>
                                            <ext:ToolbarSeparator ID="ToolbarSeparator4" runat="server">
                                            </ext:ToolbarSeparator>
                                            <ext:ToolbarFill ID="ToolbarFill1" runat="server">
                                            </ext:ToolbarFill>
                                            <ext:ToolbarSeparator ID="ToolbarSeparator1" runat="server">
                                            </ext:ToolbarSeparator>
                                            <ext:ToolbarButton runat="server" ID="Btn_OK" Text="确定选择" Icon="UserAdd">
                                                <Listeners>
                                                    <Click Handler="selectDone();" />
                                                </Listeners>
                                            </ext:ToolbarButton>
                                            <ext:ToolbarSeparator ID="ToolbarSeparator2" runat="server">
                                            </ext:ToolbarSeparator>
                                            <ext:ToolbarButton runat="server" ID="Btn_Cancel" Text="返回" Icon="ArrowUndo">
                                                <Listeners>
                                                    <Click Handler="if (parent.DetailWin != null) parent.DetailWin.hide();" />
                                                </Listeners>
                                            </ext:ToolbarButton>
                                        </Items>
                                    </ext:Toolbar>
                                </TopBar>
                                <Body>
                                    <ext:ColumnLayout ID="ColumnLayout1" runat="server" FitHeight="true">
                                        <ext:LayoutColumn ColumnWidth="0.34">
                                            <ext:Panel ID="panel3" runat="server" Border="false" Width="330" BodyStyle="background-color:transparent">
                                                <Body>
                                                    <ext:FieldSet ID="fieldset2" runat="server" Title="人员条件" Collapsed="false" StyleSpec="margin:5px;padding-left:2px;"
                                                        BodyStyle="background-color:Transparent;">
                                                        <Body>
                                                            <table width="100%">
                                                                <tr>
                                                                    <td style="width: 45%">
                                                                        人员类别
                                                                    </td>
                                                                    <td>
                                                                        <ext:ComboBox ID="cbx_Ptype" runat="server" Width="80">
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
                                                                        技术职务时间
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
                                                                            Width="120" ValueField="DEPT_CODE" TypeAhead="false" LoadingText="载入中..." PageSize="10"
                                                                            ItemSelector="div.search-item" MinChars="1" FieldLabel="所属科室" ListWidth="240">
                                                                            <Template ID="Template1" runat="server">
                                           <tpl for=".">
                                              <div class="search-item">
                                                 <h3><span style="width:auto">{DEPT_CODE}</span>{DEPT_NAME}</h3>
                                              </div>
                                           </tpl>
                                                                            </Template>
                                                                        </ext:ComboBox>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                    </td>
                                                                    <td>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </Body>
                                                    </ext:FieldSet>
                                                </Body>
                                            </ext:Panel>
                                        </ext:LayoutColumn>
                                        <ext:LayoutColumn ColumnWidth="0.34">
                                            <ext:Panel ID="Panel5" runat="server" Border="false" MonitorResize="true">
                                                <Body>
                                                    <ext:MultiSelect ID="SelectorLeft" runat="server" Legend="待选人员" DragGroup="grp1"
                                                        DropGroup="grp2,grp1" StoreID="Store1" DisplayField="PERSON_NAME" ValueField="PERSON_CODE"
                                                        AutoWidth="true" Height="250" KeepSelectionOnClick="WithCtrlKey" StyleSpec="margin:5px; ">
                                                        <Listeners>
                                                            <Render Handler=" this.setHeight(Ext.lib.Dom.getViewHeight() - this.getPosition()[1] - 10 );" />
                                                        </Listeners>
                                                    </ext:MultiSelect>
                                                </Body>
                                            </ext:Panel>
                                        </ext:LayoutColumn>
                                        <ext:LayoutColumn>
                                            <ext:Panel ID="Panel2" runat="server" Width="35" BodyStyle="background-color: transparent;"
                                                Border="false">
                                                <Body>
                                                    <ext:AnchorLayout ID="AnchorLayout1" runat="server">
                                                        <ext:Anchor Vertical="20%">
                                                            <ext:Panel ID="Panel1" runat="server" Border="false" BodyStyle="background-color: transparent;" />
                                                        </ext:Anchor>
                                                        <ext:Anchor>
                                                            <ext:Panel ID="Panel4" runat="server" Border="false" BodyStyle="padding:5px;background-color: transparent;">
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
                                        <ext:LayoutColumn ColumnWidth="0.28">
                                            <ext:Panel ID="Panel6" runat="server" Border="false">
                                                <Body>
                                                    <ext:MultiSelect ID="SelectorRight" runat="server" Legend="已选人员" DragGroup="grp2"
                                                        DropGroup="grp1,grp2" DisplayField="PERSON_NAME" ValueField="PERSON_CODE" AutoWidth="true"
                                                        Height="250" KeepSelectionOnClick="WithCtrlKey" StyleSpec="margin:5px; ">
                                                        <Listeners>
                                                            <Render Handler=" this.setHeight(Ext.lib.Dom.getViewHeight()  - this.getPosition()[1] - 10 );" />
                                                        </Listeners>
                                                    </ext:MultiSelect>
                                                </Body>
                                            </ext:Panel>
                                        </ext:LayoutColumn>
                                    </ext:ColumnLayout>
                                </Body>
                            </ext:Panel>
                        </ext:LayoutColumn>
                    </Columns>
                </ext:ColumnLayout>
            </Body>
        </ext:ViewPort>
    </div>
    </form>
</body>
</html>
