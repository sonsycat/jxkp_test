<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EditTemplet_Info.aspx.cs"
    Inherits="GoldNet.JXKP.zlgl.Templet.Config.EditTemplet_Info" %>

<%@ Register TagPrefix="uc1" TagName="title" Src="title.ascx" %>
<%@ Register Assembly="Goldnet.Ext.Web" Namespace="Goldnet.Ext.Web" TagPrefix="ext" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<uc1:title ID="Title1" runat="server"></uc1:title>
 <script type="text/javascript">
function KeyDown(){    
  if ((window.event.ctrlKey)&& 
      ((window.event.keyCode==99)||  
       (window.event.keyCode==118))){ 
     alert("禁止复制粘贴！"); 
     event.returnValue=false; 
     } 
  }
</script>

<body>
    <ext:ScriptManager ID="ScriptManager1" runat="server">
    </ext:ScriptManager>
    <form id="form1" runat="server">
    <div>
    <ext:FormPanel ID="FormPanel1" runat="server" Border="false" Height="500" AutoScroll="true"
        ButtonAlign="Right" StyleSpec="background-color:transparent" BodyStyle="background-color:transparent">
        <Body>
        <ext:Panel ID="Panel1" runat="server" Border="false" AutoHeight="true" AutoWidth="true"
                StyleSpec="background-color:transparent" BodyStyle="background-color:transparent">
                <TopBar>
                    <ext:Toolbar ID="Toolbar1" runat="server">
                        <Items>
                            <ext:ToolbarSeparator ID="ToolbarSeparator1" runat="server">
                            </ext:ToolbarSeparator>
                            <ext:Button ID="save" runat="server" Text="保存" Icon="Disk" 
                               >
                                 <AjaxEvents>
                                                    <Click OnEvent="btnSave_Click" >
                                                    </Click>
                                                </AjaxEvents>
                            </ext:Button>
                            <ext:Button ID="CancelButton" runat="server" Text="返回" Icon="ArrowUndo">
                                <AjaxEvents>
                                    <Click OnEvent="btnCancle_Click">
                                    </Click>
                                </AjaxEvents>
                            </ext:Button>
                        </Items>
                    </ext:Toolbar>
                </TopBar>
                <Body>
            <table cellspacing="0" cellpadding="0" border="0" width="100%" height="100%">
                <tr>
                    <td>
                        <table class="gs-input-tab" cellspacing="0" cellpadding="0" border="0" width="100%" height="100%">
                            <tr>
                                <td class="gs-input-desc">
                                    使用此页面修改模板常规属性。
                                </td>
                            </tr>
                            <tr>
                                <td class="gs-input-separator">
                                </td>
                            </tr>
                            <tr>
                                <td id="ContentTD" valign="top">
                                    <table width="100%" border="0" cellpadding="0" cellspacing="0" class="gs-input-tab">
                                        <tr>
                                            <td class="gs-input-title" width="50%" valign="top">
                                                名称、标题和说明
                                                <table width="100%" border="0" cellpadding="0" cellspacing="0" class="gs-input-desc">
                                                    <tr>
                                                        <td>
                                                            名称用于唯一标识一个模板。标题显示于模板上方。说明将显示在模板列表的底部。
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                            <td width="50%">
                                                <table width="100%" border="0" cellpadding="0" cellspacing="0">
                                                    <tr>
                                                        <td class="gs-input-section">
                                                            名称：<br>
                                                            &nbsp;
                                                            <ext:TextField ID="textTempletName" runat="server" Width="180" AllowBlank="false"></ext:TextField>
                                                            
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="gs-input-section">
                                                            标题：<br>
                                                            &nbsp;
                                                            <ext:TextField ID="textTempletTitle" runat="server" Width="180" AllowBlank="false"></ext:TextField>
                                                           
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="gs-input-section" onpaste="return   false ">
                                                            顺序：<br>
                                                            &nbsp;
                                                            <ext:NumberField ID="showorder" runat="server" Width="180" AllowBlank="false" ></ext:NumberField><br>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="gs-input-section" onpaste="return   false ">
                                                            是否生成数据：<br>
                                                            &nbsp;
                                                            <ext:ComboBox ID="iscount" runat="server" ReadOnly="true" ForceSelection="true" SelectOnFocus="true" Width="60">
                                            <Items>
                                                    <ext:ListItem Text="是" Value="0"/>
                                                    <ext:ListItem Text="否" Value="1" />
                                                   
                                                </Items>
                                            </ext:ComboBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="gs-input-section">
                                                            说明：<br>
                                                            &nbsp;
                                                            <asp:TextBox ID="textTempletCommon" runat="server" TextMode="MultiLine" Rows="5"
                                                                CssClass="gs-input-text" MaxLength="200"></asp:TextBox><br>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td class="gs-input-separator">
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
               
            </table>
            </Body>
			</ext:Panel>
        </Body>
      
    </ext:FormPanel>
    </div>
    </form>
</body>
</html>
