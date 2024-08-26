<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="StationPersonnel.aspx.cs"   Inherits="GoldNet.JXKP.jxkh.StationPersonnel" %>
<%@ Register Assembly="Goldnet.Ext.Web" Namespace="Goldnet.Ext.Web" TagPrefix="ext" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>岗位下属人员</title>

    <script type="text/javascript">
         var SelectorLayout = function() {
             SelectorLeft.setHeight(Ext.lib.Dom.getViewHeight() - SelectorLeft.getPosition()[1]- 30);
             SelectorRight.setHeight(Ext.lib.Dom.getViewHeight() - SelectorRight.getPosition()[1]- 30);
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
    </script>
</head>
<body>
   <form id="form1" runat="server">
   <ext:ScriptManager ID="ScriptManager1" runat="server">
       <Listeners>
       <DocumentReady Handler= " Ext.EventManager.onWindowResize(SelectorLayout);" />
       </Listeners>
   </ext:ScriptManager>
    <ext:Store runat="server" ID="Store1" >
        <Reader>
            <ext:JsonReader>
                <Fields>
                    <ext:RecordField Name="STAFF_ID" />
                    <ext:RecordField Name="STAFF_NAME" />
                </Fields>
            </ext:JsonReader>
        </Reader>
    </ext:Store>

    <ext:Store runat="server" ID="Store2">
        <Reader>
            <ext:JsonReader>
                <Fields>
                    <ext:RecordField Name="STAFF_ID" />
                    <ext:RecordField Name="STAFF_NAME" />                
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
        

<ext:Panel ID="Panel9" runat="server"  Width="400" Height="300"  BodyStyle="border-left:0px;border-right:0px;" >
    <TopBar >
        <ext:Toolbar runat="server" ID="ctl155" StyleSpec="border:0" >
            <Items>
                <ext:Label runat="server" Text="请选择：" ID="Label1" ></ext:Label>
                <ext:ComboBox  ID="Combo_zgflg" runat="server"    Editable="false" ForceSelection="true" SelectOnFocus="true">
                    <SelectedItem Value="1" />
                    <Items >
                        <ext:ListItem Text="本岗位在岗人员" Value="1" />
                        <ext:ListItem Text="本部门未在岗人员" Value="0" />
                    </Items>
                    <AjaxEvents>
                        <Select OnEvent="Combo_zgflg_Select">
                           <EventMask Msg="请稍候..." ShowMask="true" />
                        </Select>
                    </AjaxEvents>
                    <Listeners>
                        <Select Handler="btn_save.setText(this.value==0 ? '进岗' : '离岗');SelectorRight.store.removeAll()" />
                    </Listeners>
                </ext:ComboBox>

                <ext:ToolbarFill runat="server"></ext:ToolbarFill>
                <ext:Button ID="btn_save" runat="server" Icon="Disk" Text="离岗">
                     <AjaxEvents>
                        <Click  OnEvent="btn_save_Click" >
                            <EventMask Msg="请稍候..." ShowMask="true" />
                            <ExtraParams>
                                <ext:Parameter Name="multi2" Value="Ext.encode(#{SelectorRight}.getValues(true))" Mode="Raw" />
                                <ext:Parameter Name="flag" Value="#{Combo_zgflg}.getValue()" Mode="Raw" />
                            </ExtraParams>
                        </Click>
                    </AjaxEvents>
                </ext:Button>
               <ext:ToolbarSpacer runat="server" Width="20"></ext:ToolbarSpacer>
               <ext:ToolbarSeparator ID="ToolbarSeparator6" runat="server"></ext:ToolbarSeparator>
                <ext:Button ID="CancelButton" runat="server" Text="返回" Icon="ArrowUndo">
                    <Listeners>
                        <Click Handler="parent.DetailWin.hide();" />
                    </Listeners>
                </ext:Button>
            </Items>
        </ext:Toolbar>
    </TopBar>
    <BottomBar>
        <ext:StatusBar runat="server"  StyleSpec="border:0"  DefaultText="注：1.选择本岗位在岗人员时，功能按钮变为【离岗】  2.选择本部门未在岗人员时，功能按钮变为【进岗】" />
    </BottomBar>
    <Body>
        <ext:ColumnLayout ID="ColumnLayout1" runat="server" FitHeight="true" >
            <ext:LayoutColumn ColumnWidth="0.48">
                 <ext:Panel ID="Panel5" runat="server" Border="false"  MonitorResize="true" >
                        <Body>  
                            <ext:MultiSelect ID="SelectorLeft" runat="server"  Legend="待选列表" DragGroup="grp1" DropGroup="grp2,grp1" 
                                StoreID="Store1"
                                DisplayField="STAFF_NAME"
                                ValueField="STAFF_ID"
                                EnableViewState ="true"
                                 Stateful ="true"
                                AutoWidth="true" Height="250" KeepSelectionOnClick="WithCtrlKey" StyleSpec="margin:5px; ">
                                <Listeners>  
                                    <Render Handler=" this.setHeight(Ext.lib.Dom.getViewHeight() - this.getPosition()[1]-30 );" />
                                </Listeners>
                            </ext:MultiSelect>      
                            </Body>
                     </ext:Panel>        
                
            </ext:LayoutColumn>
            
            <ext:LayoutColumn>
            
                <ext:Panel ID="Panel2" runat="server" Width="35" BodyStyle="background-color: transparent;"  Border="false">
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
            
            <ext:LayoutColumn ColumnWidth="0.48">
            
               <ext:Panel ID="Panel6" runat="server" Border="false" >
                    <Body>
                        <ext:MultiSelect ID="SelectorRight" runat="server" Legend="已选列表" DragGroup="grp2" DropGroup="grp1,grp2" 
                                StoreID="Store2"
                                DisplayField="STAFF_NAME"
                                ValueField="STAFF_ID"
                                AutoWidth="true" Height="250" KeepSelectionOnClick="WithCtrlKey"  StyleSpec="margin:5px; ">
                            <Listeners>
                                <Render Handler=" this.setHeight(Ext.lib.Dom.getViewHeight()  - this.getPosition()[1]-30 );" />
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
    </body> 
    </ext:ViewPort>
    </div>
    </form>

</body>
</html>