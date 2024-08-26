<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="BenefitAdjustSearch.aspx.cs"
    Inherits="GoldNet.JXKP.BenefitAdjustSearch" %>

<%@ Register Assembly="Goldnet.Ext.Web" Namespace="Goldnet.Ext.Web" TagPrefix="ext" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style type="text/css">
        body
        {
            background-color: #DFE8F6;
            font-size: 12px;
        }
        .cwidth
        {
             width:130px;            
              padding:0 0 0 20px;
            }
         .vwidth
         {
             width:150px;            
              padding:0 0 0 10px; 
             }
         .twidth
         {
             padding:0 0 0 10px;
             }
        .cheigh
        {
            height:40px;
            }
    </style>
</head>
<body>
    <ext:ScriptManager ID="ScriptManager1" runat="server">
    </ext:ScriptManager>
    <ext:Store ID="StoreDate" runat="server">
        <Reader>
            <ext:JsonReader>
                <Fields>
                    <ext:RecordField Name="Key"></ext:RecordField>
                    <ext:RecordField Name="Value"></ext:RecordField>
                </Fields>
            </ext:JsonReader>
        </Reader>
    </ext:Store>
    <ext:Store ID="StoreCondition" runat="server">
     <Reader>
            <ext:JsonReader>
                <Fields>
                    <ext:RecordField Name="Key"></ext:RecordField>
                    <ext:RecordField Name="Value"></ext:RecordField>
                </Fields>
            </ext:JsonReader>
        </Reader>
    </ext:Store>
    <ext:Store ID="StoreRelation" runat="server">
     <Reader>
            <ext:JsonReader>
                <Fields>
                    <ext:RecordField Name="Key"></ext:RecordField>
                    <ext:RecordField Name="Value"></ext:RecordField>
                </Fields>
            </ext:JsonReader>
        </Reader>
    </ext:Store>
    <form id="form1" runat="server">
    <ext:FormPanel ID="FormPanel1" runat="server" Border="false" AutoScroll="false" ButtonAlign="Right"
        StyleSpec="background-color:Transparent" BodyStyle="background-color:Transparent">
        <Body>
            <table >            
                 <tr style="height:20px;">
                    <td>                      
                    </td>
                    <td  class="cwidth" >
                        <div style="padding:0 0 0 30px;">条件</div>
                    </td>
                    <td class="vwidth">
                         <div style="padding:0 0 0 40px;">值</div>
                    </td>
                    <td>
                         <div style="padding:0 0 0 10px;">关系</div>
                    </td>
                </tr>
                <tr  class="cheigh">
                    <td class="twidth">
                       <span>调整时间</span>
                    </td>
                    <td class="cwidth">
                        <ext:ComboBox runat="server"  ID="cbbCTime" Width="100" StoreID="StoreDate" DisplayField="Value" ValueField="Key"></ext:ComboBox>
                    </td>
                    <td class="vwidth">
                        <ext:DateField runat="server" ID="dfDate" Width="130"></ext:DateField>
                    </td>
                    <td>
                         <ext:ComboBox runat="server"  ID="cbbRTime" Width="60" StoreID="StoreRelation" DisplayField="Value" ValueField="Key"></ext:ComboBox>
                    </td>
                </tr>
                <tr  class="cheigh">
                    <td class="twidth">
                        <span>调整科室</span>
                    </td>
                    <td class="cwidth">
                        <ext:ComboBox runat="server"  ID="cbbCDept" Width="100" StoreID="StoreCondition" DisplayField="Value" ValueField="Key"></ext:ComboBox>
                    </td>
                    <td class="vwidth">
                        <ext:TextField runat="server" ID="tfDept"  Width="130"></ext:TextField>
                    </td>
                    <td>
                         <ext:ComboBox runat="server"  ID="cbbRDept"  Width="60"   StoreID="StoreRelation" DisplayField="Value" ValueField="Key"></ext:ComboBox>
                    </td>
                </tr>
                <tr  class="cheigh">
                    <td class="twidth">
                        <span>调整类别</span>
                    </td>
                    <td class="cwidth">
                        <ext:ComboBox runat="server"  ID="cbbCType" Width="100" StoreID="StoreCondition" DisplayField="Value" ValueField="Key"></ext:ComboBox>
                    </td>
                    <td class="vwidth">
                        <ext:RadioGroup ID="rgType" runat="server" ColumnsNumber="2"  Width="130">
                            <Items>
                                <ext:Radio ID="rIncome" runat="server" BoxLabel="收入"  Checked="true"/>
                                <ext:Radio ID="rCost" runat="server" BoxLabel="成本"  />
                            </Items>
                        </ext:RadioGroup>
                    </td>
                    <td>
                        <ext:ComboBox runat="server"  ID="cbbRType"  Width="60"  StoreID="StoreRelation" DisplayField="Value" ValueField="Key"></ext:ComboBox>
                    </td>
                </tr>
                <tr  class="cheigh">
                    <td class="twidth">
                        <span>调整金额</span>
                    </td>
                    <td class="cwidth">
                        <ext:ComboBox runat="server"  ID="cbbCValue"  Width="100" StoreID="StoreDate" DisplayField="Value" ValueField="Key"></ext:ComboBox>
                    </td>
                    <td class="vwidth">
                        <ext:TextField runat="server" ID="tfNumber" Width="130"></ext:TextField>
                    </td>
                    <td>
                      <ext:ComboBox runat="server"  ID="cbbRNumber" Width="60" StoreID="StoreRelation" DisplayField="Value" ValueField="Key"></ext:ComboBox>
                    </td>
                </tr>
                <tr  class="cheigh">
                    <td class="twidth">
                        <span>调整方向</span>
                    </td>
                    <td class="cwidth">
                        <ext:ComboBox runat="server"  ID="cbbCDirection"  Width="100" StoreID="StoreCondition" DisplayField="Value" ValueField="Key"></ext:ComboBox>
                    </td>
                    <td class="vwidth">
                        <ext:RadioGroup ID="rgDirection" runat="server"  ColumnsNumber="2" Width="130">
                            <Items>
                                <ext:Radio ID="rLocal" runat="server" BoxLabel="地方" Checked="true"/>
                                <ext:Radio ID="rArmy" runat="server" BoxLabel="军免"  />
                            </Items>
                        </ext:RadioGroup>
                    </td>
                </tr>
            </table>
        </Body>
        <Buttons>
            <ext:Button ID="BtnSave" runat="server" Text="查找" Icon="Disk">
                <AjaxEvents>
                    <Click OnEvent="Search_Click">
                    </Click>
                </AjaxEvents>
            </ext:Button>
            <ext:Button ID="CancelButton" runat="server" Text="取消" Icon="Cancel">
                <Listeners>
                    <Click Handler="parent.Search.hide();" />
                </Listeners>
            </ext:Button>
        </Buttons>
    </ext:FormPanel>
    </form>
</body>
</html>
