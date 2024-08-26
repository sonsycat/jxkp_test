<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="QuanZhong.aspx.cs" Inherits="GoldNet.JXKP.Bonus.Input.QuanZhong" %>
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
    </style>
</head>
<body>
    <ext:ScriptManager ID="ScriptManager1" runat="server">
    </ext:ScriptManager>
    <form id="form1" runat="server">
    <div>
        <ext:FormPanel ID="FormPanel1" runat="server" Border="false" AutoHeight="true" AutoWidth="true"
            AutoScroll="true" ButtonAlign="Right" StyleSpec="background-color:transparent"
            BodyStyle="background-color:transparent">
            <Body>
                <ext:Panel ID="Panel1" runat="server" Border="false" AutoHeight="true" AutoWidth="true"
                    StyleSpec="background-color:transparent" BodyStyle="background-color:transparent">
                    <TopBar>
                        <ext:Toolbar ID="Toolbar1" runat="server">
                            <Items>
                                <ext:Label ID="Label1" runat="server" Text="查询时间：" />
                                <ext:DateField ID="stardate" runat="server" FieldLabel="查询时间：" Width="80" EnableKeyEvents="true" />
                                <ext:ToolbarSeparator ID="ToolbarSeparator1" runat="server">
                                </ext:ToolbarSeparator>
                                <ext:Button ID="select" runat="server" Text="查询" Icon="Zoom">
                                    <AjaxEvents>
                                        <Click OnEvent="Buttonselect_Click">
                                        </Click>
                                    </AjaxEvents>
                                </ext:Button>
                                <ext:ToolbarSeparator ID="ToolbarSeparator2" runat="server">
                                </ext:ToolbarSeparator>
                                <ext:Button ID="save" runat="server" Text="计算" Icon="Disk">
                                    <AjaxEvents>
                                        <Click OnEvent="Buttonsave_Click">
                                        </Click>
                                    </AjaxEvents>
                                </ext:Button>
                                <ext:ToolbarSeparator ID="ToolbarSeparator3" runat="server">
                                </ext:ToolbarSeparator>
                            </Items>
                        </ext:Toolbar>
                    </TopBar>
                </ext:Panel>
                <ext:ColumnLayout ID="ColumnLayout2" runat="server">
                    <ext:LayoutColumn ColumnWidth="0.25">
                        <ext:Panel ID="Panel2" runat="server" Border="false" Header="false" BodyStyle="background-color:Transparent;margin:10px;">
                            <Body>
                                <ext:FormLayout ID="FormLayout2" runat="server" LabelAlign="Left">
                                    <ext:Anchor>
                                        <ext:TextField ID="INCOME" runat="server" DataIndex="INCOME"  FieldLabel="收入" Width="150" />
                                    </ext:Anchor>
                                    <ext:Anchor>
                                        <ext:TextField ID="COST" runat="server" DataIndex="COST"  FieldLabel="成本" Width="150" />
                                    </ext:Anchor>
                                    <ext:Anchor>
                                        <ext:TextField ID="INCOME_COST" runat="server" DataIndex="INCOME_COST"  FieldLabel="结余" Width="150" />
                                    </ext:Anchor>
                                    <ext:Anchor>
                                        <ext:TextField ID="JXBL" runat="server" DataIndex="JXBL"  FieldLabel="绩效比例" Width="150" />
                                    </ext:Anchor>
                                     <ext:Anchor>
                                        <ext:TextField ID="GFJX" runat="server" DataIndex="GFJX"  FieldLabel="共发绩效" Width="150" />
                                    </ext:Anchor>
                                   
                                    <ext:Anchor>
                                         <ext:TextField ID="YSZE" runat="server" DataIndex="YSZE"  FieldLabel="医生总额" Width="150" />
                                    </ext:Anchor>
                                    <ext:Anchor>
                                        <ext:TextField ID="HSZE" runat="server" DataIndex="HSZE"  FieldLabel="护士总额" Width="150" />
                                    </ext:Anchor>
                                    <ext:Anchor>
                                         <ext:TextField ID="YJZE" runat="server" DataIndex="YJZE"  FieldLabel="医技总额" Width="150" />
                                    </ext:Anchor>
                                    <ext:Anchor>
                                         <ext:TextField ID="YAOJUZE" runat="server" DataIndex="YAOJUZE"  FieldLabel="药局总额" Width="150" />
                                    </ext:Anchor>
                                    <ext:Anchor>
                                        <ext:TextField ID="CKZE" runat="server" DataIndex="CKZE"  FieldLabel="窗口总额" Width="150"  />
                                    </ext:Anchor>
                                     <ext:Anchor>
                                        <ext:TextField ID="XZZE" runat="server" DataIndex="XZZE"  FieldLabel="行政总额" Width="150" />
                                    </ext:Anchor>
                                       <ext:Anchor>
                                        <ext:TextField ID="YLDZE" runat="server" DataIndex="YLDZE"  FieldLabel="院领导总额" Width="150" />
                                    </ext:Anchor>
                                   
                                </ext:FormLayout>
                            </Body>
                        </ext:Panel>
                    </ext:LayoutColumn>
                      <ext:LayoutColumn ColumnWidth=".25">
                        <ext:Panel ID="Panel3" runat="server" Border="false" Header="false" BodyStyle="background-color:Transparent;margin:10px;">
                            <Body>
                                <ext:FormLayout ID="FormLayout1" runat="server" LabelAlign="Left">
                                 <ext:Anchor>
                                        <ext:TextField ID="YSRS" runat="server" DataIndex="YSRS"  FieldLabel="医生人数" Width="150" AllowBlank="false"/>
                                    </ext:Anchor>
                                    <ext:Anchor>
                                        <ext:TextField ID="HSRS" runat="server" DataIndex="HSRS"  FieldLabel="护士人数" Width="150" AllowBlank="false"/>
                                    </ext:Anchor>
                                    <ext:Anchor>
                                         <ext:TextField ID="YJRS" runat="server" DataIndex="YJRS"  FieldLabel="医技人数" Width="150" AllowBlank="false"/>
                                    </ext:Anchor>
                                    <ext:Anchor>
                                         <ext:TextField ID="YAOJURS" runat="server" DataIndex="YAOJURS"  FieldLabel="药局人数" Width="150" AllowBlank="false"/>
                                    </ext:Anchor>
                                    <ext:Anchor>
                                        <ext:TextField ID="CKRS" runat="server" DataIndex="CKRS"  FieldLabel="窗口科室人数" Width="150" AllowBlank="false"/>
                                    </ext:Anchor>
                                     <ext:Anchor>
                                        <ext:TextField ID="XZRS" runat="server" DataIndex="XZRS"  FieldLabel="行政人数" Width="150" AllowBlank="false"/>
                                    </ext:Anchor>
                                    <ext:Anchor>
                                        <ext:TextField ID="YZJCXS" runat="server" DataIndex="YZJCXS"  FieldLabel="院领导基础系数" Width="150" AllowBlank="false"/>
                                    </ext:Anchor>
                                    <ext:Anchor>
                                    <ext:TextField ID="YZZXS" runat="server" DataIndex="YZZXS"  FieldLabel="院领导总系数" Width="150" AllowBlank="false" />
                                    </ext:Anchor>
                                     <%--<ext:Anchor>
                                        <ext:TextField ID="YSXYBL" runat="server" DataIndex="YSXYBL"  FieldLabel="医生效益比例" Width="150" />
                                    </ext:Anchor>
                                     <ext:Anchor>
                                        <ext:TextField ID="YSYJBL" runat="server" DataIndex="YSYJBL"  FieldLabel="医生业绩比例" Width="150" />
                                    </ext:Anchor>
                                     <ext:Anchor>
                                        <ext:TextField ID="HSXYBL" runat="server" DataIndex="HSXYBL"  FieldLabel="护士效益比例" Width="150" />
                                    </ext:Anchor>
                                     <ext:Anchor>
                                        <ext:TextField ID="HSYJBL" runat="server" DataIndex="HSYJBL"  FieldLabel="护士业绩比例" Width="150" />
                                    </ext:Anchor>
                                     <ext:Anchor>
                                        <ext:TextField ID="HSZCBL" runat="server" DataIndex="HSGZBL"  FieldLabel="护士职称比例" Width="150" />
                                    </ext:Anchor>
                                     <ext:Anchor>
                                        <ext:TextField ID="HLLSBL" runat="server" DataIndex="HLLSBL"  FieldLabel="护士临时比例" Width="150" />
                                    </ext:Anchor>
                                     <ext:Anchor>
                                        <ext:TextField ID="YJXYBL" runat="server" DataIndex="YJXYBL"  FieldLabel="医技效益比例" Width="150" />
                                    </ext:Anchor>
                                    <ext:Anchor>
                                        <ext:TextField ID="YJYJBL" runat="server" DataIndex="YJYJBL"  FieldLabel="医技业绩比例" Width="150" />
                                    </ext:Anchor>                                     --%>
                                </ext:FormLayout>
                            </Body>
                        </ext:Panel>
                    </ext:LayoutColumn>
                    <ext:LayoutColumn ColumnWidth=".25">
                        <ext:Panel ID="Panel4" runat="server" Border="false" Header="false" BodyStyle="background-color:Transparent;margin:10px;">
                            <Body>
                                <ext:FormLayout ID="FormLayout3" runat="server" LabelAlign="Left">
                                    <ext:Anchor>
                                        <ext:TextField ID="YSXS" runat="server" DataIndex="YSXS"  FieldLabel="医生系数" Width="150" AllowBlank="false"/>
                                    </ext:Anchor>
                                    <ext:Anchor>
                                        <ext:TextField ID="HSXS" runat="server" DataIndex="HSXS"  FieldLabel="护士系数" Width="150" AllowBlank="false"/>
                                    </ext:Anchor>
                                    <ext:Anchor>
                                         <ext:TextField ID="YJXS" runat="server" DataIndex="YJXS"  FieldLabel="医技系数" Width="150" AllowBlank="false"/>
                                    </ext:Anchor>
                                    <ext:Anchor>
                                        <ext:TextField ID="YAOJUXS" runat="server" DataIndex="YAOJUXS"  FieldLabel="药局系数" Width="150" AllowBlank="false"/>
                                    </ext:Anchor>
                                     <ext:Anchor>
                                        <ext:TextField ID="CKXS" runat="server" DataIndex="CKXS"  FieldLabel="窗口系数" Width="150" AllowBlank="false"/>
                                    </ext:Anchor>
                                     <ext:Anchor>
                                        <ext:TextField ID="XZXS" runat="server" DataIndex="XZXS"  FieldLabel="行政系数" Width="150" AllowBlank="false"/>
                                    </ext:Anchor>
                                    
                                    <%--<ext:Anchor>
                                        <ext:TextField ID="YSXYJE" runat="server" DataIndex="YSXYJE"  FieldLabel="医生效益金额" Width="150" />
                                    </ext:Anchor>
                                    <ext:Anchor>
                                        <ext:TextField ID="YSYJEJ" runat="server" DataIndex="YSYJEJ"  FieldLabel="医生业绩金额" Width="150" />
                                    </ext:Anchor>
                                    <ext:Anchor>
                                        <ext:TextField ID="HSXYJE" runat="server" DataIndex="HSXYJE"  FieldLabel="护士效益金额" Width="150" />
                                    </ext:Anchor>
                                    <ext:Anchor>
                                        <ext:TextField ID="HSYJJE" runat="server" DataIndex="HSYJJE"  FieldLabel="护士业绩金额" Width="150" />
                                    </ext:Anchor>
                                     <ext:Anchor>
                                        <ext:TextField ID="HSZCEJ" runat="server" DataIndex="HSGZEJ"  FieldLabel="护士职称金额" Width="150" />
                                    </ext:Anchor>
                                     <ext:Anchor>
                                        <ext:TextField ID="BLLSJE" runat="server" DataIndex="BLLSJE"  FieldLabel="护士临时金额" Width="150" />
                                    </ext:Anchor>
                                    <ext:Anchor>
                                        <ext:TextField ID="YJXYJE" runat="server" DataIndex="YJXYJE"  FieldLabel="医技效益金额" Width="150" />
                                    </ext:Anchor>
                                    <ext:Anchor>
                                        <ext:TextField ID="YJYJJE" runat="server" DataIndex="YJYJJE"  FieldLabel="医技业绩金额" Width="150" />
                                    </ext:Anchor>          --%>                          
                                </ext:FormLayout>
                            </Body>
                        </ext:Panel>
                    </ext:LayoutColumn>
                    <ext:LayoutColumn ColumnWidth=".25">
                        <ext:Panel ID="Panel5" runat="server" Border="false" Header="false" BodyStyle="background-color:Transparent;margin:10px;">
                            <Body>
                                <ext:FormLayout ID="FormLayout4" runat="server" LabelAlign="Left">
                                    <ext:Anchor>
                                        <ext:TextField ID="YSBL" runat="server" DataIndex="YSBL"  FieldLabel="医生比例" Width="150"  />
                                    </ext:Anchor>
                                    <ext:Anchor>
                                         <ext:TextField ID="HSBL" runat="server" DataIndex="HSBL"  FieldLabel="护士比例" Width="150" />
                                    </ext:Anchor>
                                    <ext:Anchor>
                                        <ext:TextField ID="YJBL" runat="server" DataIndex="YJBL"  FieldLabel="医技比例" Width="150" />
                                    </ext:Anchor>
                                    <ext:Anchor>
                                         <ext:TextField ID="YAOJUBL" runat="server" DataIndex="YAOJUBL"  FieldLabel="药局比例" Width="150" />
                                    </ext:Anchor>
                                    <ext:Anchor>
                                        <ext:TextField ID="CKBL" runat="server" DataIndex="CKBL"  FieldLabel="窗口比例" Width="150" />
                                    </ext:Anchor>
                                    <ext:Anchor>
                                        <ext:TextField ID="XZBL" runat="server" DataIndex="XZBL"  FieldLabel="行政比例" Width="150" />
                                    </ext:Anchor>
                                     <ext:Anchor>
                                        <ext:TextField ID="YLDBL" runat="server" DataIndex="YLDBL"  FieldLabel="院领导比例" Width="150" />
                                    </ext:Anchor>
                                   <%-- <ext:Anchor>
                                        <ext:TextField ID="MZHSJE" runat="server" DataIndex="MZHSJE"  FieldLabel="门诊护士金额" Width="150" />
                                    </ext:Anchor>--%>
                                </ext:FormLayout>
                            </Body>
                        </ext:Panel>
                    </ext:LayoutColumn>
                </ext:ColumnLayout>
            </Body>
        </ext:FormPanel>
    </div>
    </form>
</body>
</html>
