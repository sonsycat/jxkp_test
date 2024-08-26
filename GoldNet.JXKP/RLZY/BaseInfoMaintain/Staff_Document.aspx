<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Staff_Document.aspx.cs" Inherits="GoldNet.JXKP.Staff_Document" %>
<%@ Register Assembly="Goldnet.Ext.Web" Namespace="Goldnet.Ext.Web" TagPrefix="ext" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
   <title></title>
    <link rel="stylesheet" type="text/css" href="/resources/css/main.css" />
    <style type="text/css">
        body
        {
         background-color: #DFE8F6;
         font-size:12px;
        }
       td strong
       {
       	color:Red;
       }
    </style>
    <script type="text/javascript">
        var CheckForm = function() {
            if (StationNameTxt.validate() == false) {
                return false;
            }
            if (DeptCombo.validate() == false) {
                return false;
            }
            if (DeptDutyCombo.validate() == false) {
                return false;
            }
            if (ScoreNum1.validate() == false) {
                return false;
            }
            if (ScoreNum2.validate() == false) {
                return false;
            }
            if (ScoreNum3.validate() == false) {
                return false;
            }
            if (ScoreNum4.validate() == false) {
                return false;
            }
            return true;
        }
    </script>
</head>
<body>
    <ext:ScriptManager ID="ScriptManager1" runat="server">
    </ext:ScriptManager>
     
    <form id="form1" runat="server" style="background-color:Transparent">
    <div>
        <ext:FormPanel ID="FormPanel1" runat="server" Border="false"  AutoScroll="true"  ButtonAlign="Right" StyleSpec="background-color:transparent" BodyStyle="background-color:transparent" >
            <Body>
            <ext:Panel ID ="Panel1" runat ="server" Border ="false" AutoHeight="true" AutoWidth = "true" StyleSpec="background-color:transparent" BodyStyle="background-color:transparent">
            <TopBar>
                <ext:Toolbar ID="Toolbar1" runat="server">
                    <Items>

                        <ext:ToolbarSeparator ID="ToolbarSeparator1" runat="server"></ext:ToolbarSeparator>
                        <ext:Button ID="CancelButton" runat="server" Text="返回" Icon="ArrowUndo">
                            <Listeners>
                                <Click Handler="parent.jsda_Detail.hide();" />
                            </Listeners>
                        </ext:Button>
                    
                    </Items>
                </ext:Toolbar>
            </TopBar>
            <Body>
                    <ext:FieldSet ID="fieldset1" runat="server" Title="基本情况"  Collapsible="true" Collapsed="false" StyleSpec="margin:2px" Width="510" BodyStyle="background-color:Transparent;">
                        <Body>
                            <table width="95%">
                                <tbody>
                                    <tr>
                                        <td style="width: 33%;">
                                            姓名:<ext:Label ID="xm" runat="server"></ext:Label>
                                        </td>
                                        <td style="width: 33%;">
                                            性别:<ext:Label ID="xb" runat="server"></ext:Label>
                                             
                                        </td>
                                        <td style="width: 34%;" >
                                            出生日期:<ext:Label ID="csrq" runat="server"></ext:Label>
                                            
                                        </td>                                        
                                        
                                    </tr>
                                    <tr>
                                    <%-- <td style="width: 33%;">
                                            毕业时间:<ext:Label ID="bysj" runat="server"></ext:Label>
                                            
                                        </td>      --%>                     
                                          <td style="width: 33%;">
                                            专业:<ext:Label ID="zy" runat="server"></ext:Label>
                                        </td>
                                        <td style="width: 34%;">
                                            学位:<ext:Label ID="xw" runat="server"></ext:Label>
                                             
                                        </td>
                                      <td style="width: 33%;">
                                           
                                        </td>
                                        
                                    </tr>
                                   <%-- <tr> <td style="width: 100%;" colspan="3">
                                            院校:<ext:Label ID="yx" runat="server" Text="石家庄炮兵学院"></ext:Label>
                                             
                                        </td></tr>
                                    <tr>--%>
                                       
                                        <td style="width: 33%;">
                                            技术职称:<ext:Label ID="jszc" runat="server"></ext:Label>
                                            
                                        </td>
                                        <td style="width: 34%;" >
                                            工作时间:<ext:Label ID="xgwgzsj" runat="server"></ext:Label>
                                           
                                        </td>
                                        <td style="width: 33%;">
                                            <%--部职别:<ext:Label ID="bzb" runat="server"></ext:Label>--%>
                                             
                                        </td>
                                    </tr>
                                    
                                </tbody>
                            </table>
                        </Body>
                    </ext:FieldSet>
                     <ext:FieldSet ID="fieldset2" runat="server" Title="医疗管理"  Collapsible="true" Collapsed="false" StyleSpec="margin:2px" Width="510" BodyStyle="background-color:Transparent;">
                        <Body>
                            <table width="95%">
                                <tbody>
                                    <tr>
                                        <td style="width: 20%;" colspan="3">
                                           <strong> 门诊工作量</strong>
                                        </td>
                                        
                                    </tr>
                                    <tr>
                                     <td style="width: 10%;">
                                           
                                        </td>                           
                                          <td style="width: 40%;">
                                            门诊人次:
                                             <ext:LinkButton ID="mzrc" runat="server" Text="0"></ext:LinkButton>
                                        </td>
                                        <td style="width: 50%;">
                                            门诊手术次数:
                                             <ext:LinkButton ID="mzsscs" runat="server" Text="0"></ext:LinkButton>
                                        </td>
                                       
                                        
                                    </tr>
                                    <tr>
                                       <td style="width: 10%;">
                                           
                                        </td>
                                        <td style="width: 40%;">
                                            出诊人数:
                                             <ext:LinkButton ID="czrs" runat="server" Text="0"></ext:LinkButton>
                                        </td>
                                        <td style="width: 50%;">
                                           
                                        </td>
                                    </tr>
                                    
                                </tbody>
                            </table>
                             <table width="95%">
                                <tbody>
                                    <tr>
                                        <td style="width: 20%;" colspan="3">
                                           <strong> 住院工作量</strong>
                                        </td>
                                        
                                    </tr>
                                    <tr>
                                     <td style="width: 10%;">
                                           
                                        </td>                           
                                          <td style="width: 40%;">
                                            收 容 量:
                                             <ext:LinkButton ID="srl" runat="server" Text="0"></ext:LinkButton>
                                        </td>
                                        <td style="width: 50%;">
                                            日均管床数:
                                            <ext:LinkButton ID="rjgcs" runat="server" Text="0"></ext:LinkButton>
                                        </td>
                                       
                                        
                                    </tr>
                                    <tr>
                                       <td style="width: 10%;">
                                           
                                        </td>
                                        <td style="width: 40%;">
                                            抢 救 量:
                                             <ext:LinkButton ID="qjl" runat="server" Text="0"></ext:LinkButton>
                                        </td>
                                         <td style="width: 50%;">
                                            出 院 人 数:
                                             <ext:LinkButton ID="cyrs" runat="server" Text="0"></ext:LinkButton>
                                        </td>
                                    </tr>
                                    
                                </tbody>
                            </table>
                            <table width="95%">
                                <tbody>
                                    <tr>
                                        <td style="width: 20%;" colspan="3">
                                           <strong> 质量、效率</strong>
                                        </td>
                                        
                                    </tr>
                                    <tr>
                                     <td style="width: 10%;">
                                           
                                        </td>                           
                                          <td style="width: 40%;">
                                            平均住院日:
                                             <ext:LinkButton ID="pjzyr" runat="server" Text="0"></ext:LinkButton>
                                        </td>
                                        <td style="width: 50%;">
                                            治愈好转率:
                                            <ext:LinkButton ID="zyhzl" runat="server" Text="0"></ext:LinkButton>
                                        </td>
                                       
                                        
                                    </tr>
                                    <tr>
                                       <td style="width: 10%;">
                                           
                                        </td>
                                        <td style="width: 40%;">
                                            三日确诊率:
                                             <ext:LinkButton ID="srqzl" runat="server" Text="0"></ext:LinkButton>
                                        </td>
                                         <td style="width: 50%;">
                                            患者人均费用:
                                            <ext:LinkButton ID="hzrjfy" runat="server" Text="0"></ext:LinkButton>
                                        </td>
                                    </tr>
                                    
                                </tbody>
                            </table>
                             <table width="95%">
                                <tbody>
                                    <tr>
                                        <td style="width: 20%;" colspan="3">
                                           <strong> 手术工作量</strong>
                                        </td>
                                        
                                    </tr>
                                    <tr>
                                     <td style="width: 10%;">
                                           
                                        </td>                           
                                          <td style="width: 40%;">
                                            手术台次:
                                             <ext:LinkButton ID="sstc" runat="server" Text="0"></ext:LinkButton>
                                        </td>
                                        <td style="width: 50%;">
                                            大手术量:
                                             <ext:LinkButton ID="dssl" runat="server" Text="0"></ext:LinkButton>
                                        </td>
                                       
                                        
                                    </tr>
                                    <tr>
                                       <td style="width: 10%;">
                                           
                                        </td>
                                        <td style="width: 40%;">
                                            中手术量:
                                             <ext:LinkButton ID="zssl" runat="server" Text="0"></ext:LinkButton>
                                        </td>
                                         <td style="width: 50%;">
                                            小手术量:
                                            <ext:LinkButton ID="xssl" runat="server" Text="0"></ext:LinkButton>
                                             
                                        </td>
                                    </tr>
                                    
                                </tbody>
                            </table>
                        </Body>
                    </ext:FieldSet> 
                    <ext:FieldSet ID="fieldset3" runat="server" Title="科教研管理"  Collapsible="true" Collapsed="false" StyleSpec="margin:2px" Width="510" BodyStyle="background-color:Transparent;">
                        <Body>
                            <table width="95%">
                                <tbody>
                                    <tr>
                                        <td style="width: 100%;">
                                            1、参与编撰出版学术书籍 <ext:Label ID="xssj" runat="server" Text="0"></ext:Label> 本
                                        </td>
                                        
                                        
                                    </tr>
                                    <tr>
                                     <td style="width: 100%;">
                                           2、发表科研论文 <ext:Label ID="kylw" runat="server" Text="0"></ext:Label> 篇，
                                           教学论文 <ext:Label ID="jxlw" runat="server" Text="0"></ext:Label> 篇，
                                           其中核心期刊 <ext:Label ID="hxqk" runat="server" Text="0"></ext:Label> 篇，
                                           第一作者 <ext:Label ID="dyzz" runat="server" Text="0"></ext:Label> 篇
                                        </td>                           
                                         
                                      
                                        
                                    </tr>
                                    <tr> <td style="width: 100%;" >
                                           3、承担相关课题 <ext:Label ID="xgkt" runat="server" Text="0"></ext:Label> 项
                                             
                                        </td>
                                    </tr>
                                    <tr>
                                       <td style="width: 33%;">
                                           4、研究成果 <ext:Label ID="yjcg" runat="server" Text="0"></ext:Label> 项，
                                          其中：国家级成果 <ext:Label ID="gjcg" runat="server" Text="0"/> 项，
                                          省部级（军区）成果 <ext:Label ID="sbjcg" runat="server" Text="0"/> 项   
                                        </td>
                                       
                                    </tr>
                                    <tr> <td style="width: 100%;">
                                           5、在学术团体任职数量 <ext:Label ID="ssttrzsl" runat="server" Text="0"></ext:Label> 项
                                             
                                        </td>
                                    </tr>
                                    <tr> <td style="width: 100%;">
                                           6、参加学术会议 <ext:Label ID="xshy" runat="server" Text="0"></ext:Label> 次数
                                             
                                        </td>
                                    </tr>
                                </tbody>
                            </table>
                        </Body>
                    </ext:FieldSet>       
                    <ext:FieldSet ID="fieldset4" runat="server" Title="医德医风"  Collapsible="true" Collapsed="false" StyleSpec="margin:2px" Width="510" BodyStyle="background-color:Transparent;">
                        <Body>
                            <table width="95%">
                                <tbody>
                                    <tr>
                                        <td style="width: 100%;">
                                            1、接到表扬、锦旗 <ext:Label ID="byjq" runat="server" Text="0"></ext:Label> 次数
                                        </td>
                                        
                                        
                                    </tr>
                                    <tr>
                                     <td style="width: 100%;">
                                           2、遭受医疗纠纷 <ext:Label ID="yljf" runat="server" Text="0"></ext:Label> 次，
                                           患者投诉 <ext:Label ID="hzts" runat="server" Text="0"></ext:Label> 次
                                        </td>                           
                                    </tr>
                                    <tr> <td style="width: 100%;" >
                                           3、工作期间迟到早退或其他违纪行为被医院通报批评 <ext:Label ID="tbpp" runat="server" Text="0"></ext:Label> 次
                                             
                                        </td>
                                    </tr>
                                  
                                </tbody>
                            </table>
                        </Body>
                    </ext:FieldSet>       
                       </Body>
            </ext:Panel>
            
                    
            </Body>

            
       </ext:FormPanel>
    </div>
    </form>
</body>
</html>