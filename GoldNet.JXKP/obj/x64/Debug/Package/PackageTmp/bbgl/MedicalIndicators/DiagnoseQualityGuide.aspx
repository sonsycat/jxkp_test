<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DiagnoseQualityGuide.aspx.cs" Inherits="GoldNet.JXKP.bbgl.MedicalIndicators.DiagnoseQualityGuide" %>

<%@ Register Assembly="Goldnet.Ext.Web" Namespace="Goldnet.Ext.Web" TagPrefix="ext" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>无标题页</title>
</head>
<body>
    <form id="form1" runat="server">
   <ext:ScriptManager ID="ScriptManager1" runat="server">
    </ext:ScriptManager>
        <ext:Store ID="Store1" runat="server" OnRefreshData="Data_RefreshData">
        <Reader>
            <ext:JsonReader>
                <Fields>
                    <ext:RecordField Name="Columns1"   />
                    <ext:RecordField Name="Columns2"   />
                    <ext:RecordField Name="Columns3"   />
                    <ext:RecordField Name="Columns4"   />
                    <ext:RecordField Name="Columns5"  />
                    <ext:RecordField Name="Columns6"   />
                    <ext:RecordField Name="Columns7"  />
                    <ext:RecordField Name="Columns8"   />
                    <ext:RecordField Name="Columns9"  />
                    <ext:RecordField Name="Columns10"   />
                    <ext:RecordField Name="Columns11"  />
                    
 
                </Fields>
            </ext:JsonReader>
        </Reader>
    </ext:Store>
    
    <div>    
  
         
           
           
    <ext:ViewPort ID="ViewPort1" runat="server">
     <Body>
     <ext:ColumnLayout ID="ColumnLayout1" runat="server" Split="true" FitHeight="true">
     <Columns>
        <ext:LayoutColumn ColumnWidth="1">
        
        
        
                    <ext:GridPanel ID="GridPanel_Show" runat="server" StoreID="Store1" Border="false" AutoWidth="true" Height="591" Title="" MonitorResize="true" MonitorWindowResize="true" StripeRows="true" TrackMouseOver="true">
  <TopBar>
               <ext:Toolbar ID="Toolbar_ZLJK" runat="server" Visible="true">
                   <Items>
                       <ext:ToolbarSpacer ID="ToolbarSpacer2" runat="server" Width="10" />
                       <ext:Label ID="Label7" runat="server" Text="统计日期:   月份"></ext:Label>
                       <ext:DateField runat="server" ID="dd1" Vtype="daterange"   AllowBlank="false" Format="Ym"  MaxLength="6" Width="100">
                       </ext:DateField>
                        
                       <ext:Label ID="Label6" runat="server" Text="   至   "></ext:Label>
                       
                       <ext:DateField runat="server" ID="dd2" Vtype="daterange"   AllowBlank="false"  Format="Ym"  MaxLength="6" Width="100">
                        <Listeners>
                            <Render Handler="this.startDateField = '#{dd1}'" />
                        </Listeners> 
                       </ext:DateField>
                       <ext:ToolbarSpacer ID="ToolbarSpacer5" runat="server" Width="6" />
                       <ext:ToolbarSeparator ID="ToolbarSeparator3" runat="server" />
                       <ext:Button ID="Button1" runat="server" Text=" 查询 " Icon="DatabaseGo">
                        <AjaxEvents>
<%--                            <Click OnEvent="GetQueryPortalet">
                            <EventMask Msg="载入中..." ShowMask="true" />
                            </Click>--%>
                        </AjaxEvents>
                        </ext:Button>
                       <ext:ToolbarSeparator ID="ToolbarSeparator4" runat="server" />
                   </Items>
               </ext:Toolbar>
</TopBar>
                                <ColumnModel ID="ColumnModel1" runat="server">
                                    <Columns>
                                        
                                        <ext:Column Header="<center>科室</center>" Sortable="true" ColumnID="Columns1" DataIndex="Columns1"
                                            Align="center" Width="116">
                                            <PrepareCommand Handler="" Args="grid,command,record,row,col,value" FormatHandler="False">
                                            </PrepareCommand>
                                        </ext:Column>
                                        <ext:Column Header="<center>门诊_出院诊断<br>符合率</center>" Sortable="true" ColumnID="Columns2" DataIndex="Columns2"
                                            Align="center" Width="98">
                                            <PrepareCommand Handler="" Args="grid,command,record,row,col,value" FormatHandler="False">
                                            </PrepareCommand>
                                        </ext:Column>
                                        <ext:Column Header="<center>入院_出院诊断<br>符合率</center>" Sortable="true" ColumnID="Columns3" DataIndex="Columns3"
                                            Align="center" Width="98">
                                            
                                        </ext:Column>
                                        <ext:Column Header="<center>术前_术后诊断<br>符合率</center>" Sortable="true" ColumnID="Columns4" DataIndex="Columns4" 
                                            Align="center" Width="98">
                                            <PrepareCommand Handler="" Args="grid,command,record,row,col,value" FormatHandler="False">
                                            </PrepareCommand>
                                        </ext:Column>
                                        <ext:Column Header="<center>放射_术后诊断<br>符合率</center>" Sortable="true" ColumnID="Columns5" DataIndex="Columns5"
                                            Align="center" Width="98">
                                            <PrepareCommand Handler="" Args="grid,command,record,row,col,value" FormatHandler="False">
                                            </PrepareCommand>
                                        </ext:Column>
                                       
                                        
                                        
                                        <ext:Column Header="<center>临床_病理诊断<br>符合率</center>" Sortable="true" ColumnID="Columns2" DataIndex="Columns2"
                                            Align="center" Width="98">
                                            <PrepareCommand Handler="" Args="grid,command,record,row,col,value" FormatHandler="False">
                                            </PrepareCommand>
                                        </ext:Column>
                                        <ext:Column Header="<center>放射_病理诊断<br>符合率</center>" Sortable="true" ColumnID="Columns3" DataIndex="Columns3"
                                            Align="center" Width="98">
                                            
                                        </ext:Column>
     
                                        
                                        
                                  
                                        
                                        <ext:Column Header="<center>临床_尸检诊断<br>符合率</center>" Sortable="true" ColumnID="Columns6" DataIndex="Columns6" Align="center" Width="98">
                                            <PrepareCommand Handler="" Args="grid,command,record,row,col,value" FormatHandler="False">
                                            </PrepareCommand>
                                        </ext:Column>
                                        
                                        
                                        <ext:Column Header="<center>三日确诊率</center>" Sortable="true" ColumnID="Columns7" DataIndex="Columns7"
                                            Align="center" Width="151">
                                            <PrepareCommand Handler="" Args="grid,command,record,row,col,value" FormatHandler="False">
                                            </PrepareCommand>
                                        </ext:Column>
                                        
                                         <ext:Column Header="<center>住院病人门诊诊断待诊率</center>" Sortable="true" ColumnID="Columns8" DataIndex="Columns8"
                                            Align="center" Width="151">
                                            <PrepareCommand Handler="" Args="grid,command,record,row,col,value" FormatHandler="False">
                                            </PrepareCommand>
                                        </ext:Column>
                                        
                                         <ext:Column Header="<center>住院诊断待诊率</center>" Sortable="true" ColumnID="Columns8" DataIndex="Columns8"
                                            Align="center" Width="134">
                                            <PrepareCommand Handler="" Args="grid,command,record,row,col,value" FormatHandler="False">
                                            </PrepareCommand>
                                        </ext:Column>

                                        
                                        
                                        
                                    </Columns>
                                </ColumnModel>




                        <SelectionModel>
                            <ext:RowSelectionModel ID="rowselection" SingleSelect="true" />
                        </SelectionModel>
    

                        <LoadMask ShowMask="true" />
                        
                        
                        
                         <BottomBar>
                                    <ext:PagingToolbar ID="PagingToolBar1" runat="server" PageSize="25" StoreID="Store1"
                                        AutoWidth="true" DisplayInfo="false" AutoDataBind="true">

                                    </ext:PagingToolbar>
                         </BottomBar>
                    </ext:GridPanel>
               </ext:LayoutColumn>
      </Columns>
     </ext:ColumnLayout>
    </body> 
    </ext:ViewPort>       
    </div>
    </form>
</body>
</html>
