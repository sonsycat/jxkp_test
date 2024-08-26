<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="login.aspx.cs" Inherits="Goldnet.JXKP.auth.login.login" %>
<%@ Register Assembly="Goldnet.Ext.Web" Namespace="Goldnet.Ext.Web" TagPrefix="ext" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
<title>吉大医院绩效考核管理系统</title>
      

<link rel="stylesheet" type="text/css" href="/resources/css/login.css" />
<link rel="shortcut icon" href="/favicon.ico"  />
<script type="text/javascript">
    function centerPanel(loginPanel) {
        var xy = loginPanel.getAlignToXY(document, 'c-c');
        positionPanel(loginPanel, xy[0], xy[1]);
    }
    function positionPanel(el, x, y) {
        if (x && typeof x[1] == 'number') {
            y = x[1];
            x = x[0];
        }
        el.pageX = x;
        el.pageY = y;
        if (x === undefined || y === undefined) {
            return;
        }
        if (y < 0) { y = 10; }
        var p = el.translatePoints(x, y);
        el.setLocation(p.left, p.top);
        return el;
    }
</script>
</head>
<body>
<form id="form1" runat="server" >
<ext:ScriptManager ID="ScriptManager1" runat="server"> 
<Listeners>
    <DocumentReady Handler="var loginPanel = Ext.get('qo-login-panel');centerPanel(loginPanel);" />
    <WindowResize Handler="var loginPanel = Ext.get('qo-login-panel');centerPanel(loginPanel);" />
</Listeners>
</ext:ScriptManager>
<div id="qo-login-panel">
	<span class="qo-login-logo qo-abs-position">
			<b id="hosname" runat="server">吉大医院绩效考核管理系统</b><span>v1.0</span><br/>
	</span>
		
	<div class="qo-login-benefits qo-abs-position">
		○ 提升医院核心竞争力<br/>
		○ 全方位的医院岗位目标管理<br/>
		○ 专业丰富的医疗评价体系<br/>
		○ 全新的用户操作体验<br/>
	</div>
	<%--<span class="qo-login-supported qo-abs-position">
		<b>支持以下浏览器</b><br />
		<a href="http://www.mozilla.org/download.html" target="_blank">Firefox 2+</a><br />
		<a href="http://www.microsoft.com/windows/downloads/ie/getitnow.mspx" target="_blank">Internet Explorer 6+</a><br />
		建议采用1280*800以上的分辨率<br />
		
	</span>
	--%>
	<span class="qo-login-forgot qo-abs-position">
		<b id="yz" runat="server"></b><br/>
	</span>
	<span class="qo-login-signup qo-abs-position">
		版权所有 &copy
		<b id="bq" runat="server">2009-2015 心医国际</b><br/>
	</span>
	
	
	
	<label id="field1-label" class="qo-abs-position" accesskey="e" for="UNAME">　用户名：</label>
	<ext:TextField  SelectOnFocus="true" ID="UNAME" Cls="field1 qo-abs-position"  MaxLength="20"  InputType="Text" runat="server" EmptyText="请输入用户登录名..."  > </ext:TextField>
	
	<label id="field2-label" class="qo-abs-position" accesskey="p" for="PASSWORD">密&nbsp&nbsp码：</label>

	<ext:TextField   SelectOnFocus="true" ID="PASSWORD" Cls="field2 qo-abs-position"  MaxLength="20" InputType="Password" runat="server" EnableKeyEvents="true"  >
	<AjaxEvents>
	<Change  OnEvent="Button1_Click" >
	      <EventMask ShowMask="true" Msg="认证中,请稍候..." MinDelay="1000"  />
	</Change>
	</AjaxEvents>
	</ext:TextField>
	<ext:KeyNav ID="KeyNav1" runat="server" Target="PASSWORD" >
	    <Enter Handler="var loginP = Ext.get('PASSWORD');   loginP.blur();" />
	</ext:KeyNav>
	<ext:KeyNav ID="KeyNav2" runat="server" Target="UNAME" >
	    <Enter Handler="var  loginP = Ext.get('PASSWORD');  loginP.focus();" />
	</ext:KeyNav>
	<ext:Button  ID="Button2"  runat="server" Text=" 登　录　"  Icon="Key" StyleSpec="position:absolute; left:660px; top:373px;" >
        <AjaxEvents >
                <Click OnEvent="Button1_Click"  >
                    
                </Click>
            </AjaxEvents>
            <ToolTips><ext:ToolTip ID="ToolTip2" runat="server" Title="点击登录系统"></ext:ToolTip></ToolTips> 
     </ext:Button>
</div>
</form>

</body>
</html>