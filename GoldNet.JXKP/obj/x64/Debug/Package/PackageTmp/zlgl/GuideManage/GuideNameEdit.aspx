<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="GuideNameEdit.aspx.cs" Inherits="GoldNet.JXKP.zlgl.SysManage.GuideNameEdit" %>
<%@ Register Assembly="Goldnet.Ext.Web" Namespace="Goldnet.Ext.Web" TagPrefix="ext" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title></title>
    <LINK href="../css/common.css" type="text/css" rel="stylesheet">
		<LINK href="../css/sps.css" type="text/css" rel="stylesheet">
		<style type="text/css">
        body
        {
            background-color:#DFE8F6;
            font-size: 12px;
        }
        td strong
        {
            color:Black;
        }
    </style>
    
</head>
<body>
<ext:ScriptManager ID="ScriptManager1" runat="server">
    </ext:ScriptManager>
    <form id="form1" runat="server">
    <div>
      <ext:FormPanel ID="FormPanel1" runat="server" Border="false" AutoHeight="true" AutoScroll="true"
        ButtonAlign="Right" StyleSpec="background-color:transparent" BodyStyle="background-color:transparent">
        <Body>
            <ext:Panel ID="Panel1" runat="server" Border="false" AutoHeight="true" AutoWidth="true"
                StyleSpec="background-color:transparent" BodyStyle="background-color:transparent">
                <TopBar>
                    <ext:Toolbar ID="Toolbar1" runat="server">
                        <Items>
                            <ext:ToolbarSeparator ID="ToolbarSeparator1" runat="server">
                            </ext:ToolbarSeparator>
                            <ext:Button ID="btnSave" runat="server" Text="保存" Icon="Disk" 
                                >
                                <AjaxEvents>
                                                    <Click OnEvent="BtnSave_Click" >
                                                    </Click>
                                                </AjaxEvents>
                            </ext:Button>
                            <ext:Button ID="btnCancle" runat="server" Text="返回" Icon="ArrowUndo">
                                <AjaxEvents>
                                    <Click OnEvent="btnCancle_Click">
                                    </Click>
                                </AjaxEvents>
                            </ext:Button>
                        </Items>
                    </ext:Toolbar>
                </TopBar>
                </ext:Panel>
                <ext:Panel ID="Panel2" runat="server" Border="false" Height="440" AutoWidth="true" AutoScroll="true"
                StyleSpec="background-color:transparent" BodyStyle="background-color:transparent">
                <Body>
                <table class="gs-maintab" cellSpacing="0" cellPadding="0" width="100%" border="0">
				
				<tr>
					<td>
						<table class="gs-input-tab" cellSpacing="0" cellPadding="0" width="100%" border="0">
							<tr>
								<td class="gs-input-separator" colSpan="2"></td>
							</tr>
							<tr>
								<td width="35%" valign=top>
								<table>
									<tr>
										<td class="gs-input-title"> 修改考评指标信息.</td>
									</tr>
									<tr>
										<td>注意，修改考评指标信息可能导致以下情况发生：
<UL>
	<LI>修改考评指标名称可能会导致已经使用此指标的模板无法找到指标。请修改指标名称后，再到相应模板下修改对应的指标。
	<LI>修改考评指标得分，可能会导致科室考评满分不等于100分。请修改多项考评得分，以使科室满分均为100分。
	<LI>修改考评指标对应模板，务必使考评指标均对应到一个可使用的模板，且此模板至少应包含一个时间字段、一个科室字段、一个指标字段(与此指标对应)、一个数字字段(用于保存扣分情况)。
</UL>

										</td>
									</tr>
								</table>
								</td>
								<td  id="ContentTD" vAlign="top" align="left" width="65%">
									<table width="100%">
										<tr>
											<td align="right" width="20%"><FONT face="宋体">指标名称：</FONT></td>
											<td width="80%">
											<ext:TextField ID="TBGuideName" runat ="server" AllowBlank="false" Width="100"></ext:TextField>
											</td>
										</tr>
										<tr>
											<td width="20%">&nbsp;&nbsp;</td>
											<td width="80%">&nbsp;&nbsp;</td>
										</tr>
										<tr>
											<td align="right"><FONT face="宋体">指标类别：</FONT></td>
											<td>
											 <ext:ComboBox ID="DDLGuideType" runat="server" AllowBlank="false" Width="100" EmptyText="选择指标类别">
                                            </ext:ComboBox>
											
											</td>
										</tr>
										<tr>
											<td width="20%">&nbsp;&nbsp;</td>
											<td width="80%">&nbsp;&nbsp;</td>
										</tr>
										<tr>
											<td align="right"><FONT face="宋体">主管部门：</FONT></td>
											<td><ext:TextField ID="TBManaDept" runat ="server" AllowBlank="false" Width="100"></ext:TextField>
											</td>
										</tr>
										<tr>
											<td width="20%">&nbsp;&nbsp;</td>
											<td width="80%">&nbsp;&nbsp;</td>
										</tr>
										<tr>
											<td align="right"><FONT face="宋体">指标分值：</FONT></td>
											<td onpaste="return false">
											
											<ext:NumberField ID="TBGuideNum" runat ="server" AllowBlank="false" Width="100"></ext:NumberField>
											
										</tr>
										<tr>
											<td width="20%">&nbsp;&nbsp;</td>
											<td width="80%">&nbsp;&nbsp;</td>
										</tr>
										<tr>
											<td align="right" style="HEIGHT: 17px"><FONT face="宋体">模板：</FONT></td>
											<td style="HEIGHT: 17px">
											 <ext:ComboBox ID="DDLTemplet" runat="server" AllowBlank="false" Width="100" EmptyText="选择模版">
											 <AjaxEvents>
                                                <Select OnEvent="DDLTemplet_SelectedIndexChanged">
                                                    <EventMask ShowMask="true" />
                                                </Select>
                                            </AjaxEvents>
                                            </ext:ComboBox>
											
											</td>
										</tr>
										<tr>
											<td width="20%">&nbsp;&nbsp;</td>
											<td width="80%">&nbsp;&nbsp;</td>
										</tr>
										<tr>
											<td align="right"><FONT face="宋体">时间列：</FONT></td>
											<td><ext:ComboBox ID="DDLDateCol" runat="server" AllowBlank="false" Editable="false" ForceSelection="false"
                                                        Width="100">
                                                    </ext:ComboBox>
											</td>
										</tr>
										<tr>
											<td width="20%">&nbsp;&nbsp;</td>
											<td width="80%">&nbsp;&nbsp;</td>
										</tr>
										<tr>
											<td align="right"><FONT face="宋体">部门列：</FONT></td>
											<td><ext:ComboBox ID="DDLTargetCol" runat="server" AllowBlank="false" Editable="false"
                                                        ForceSelection="false" Width="100">
                                                    </ext:ComboBox></td>
										</tr>
										
										<tr>
											<td width="20%">&nbsp;&nbsp;</td>
											<td width="80%">&nbsp;&nbsp;</td>
										</tr>
										<tr>
											<td align="right">数值列：</td>
											<td><ext:ComboBox ID="DDLGuideNameColValue" runat="server" AllowBlank="false" Editable="false"
                                                        ForceSelection="false" Width="100">
                                                    </ext:ComboBox></td>
										</tr>
										<tr>
											<td width="20%">&nbsp;&nbsp;</td>
											<td width="80%">&nbsp;&nbsp;</td>
										</tr>
										<tr>
											<td align="right"></td>
											<td><ext:ComboBox ID="DDLArithmetic" runat="server" AllowBlank="true" Editable="false"
                                                        Visible="false" ForceSelection="false" Width="100">
                                                        <Items>
                                                            <ext:ListItem Value="1" Text="1" />
                                                        </Items>
                                                    </ext:ComboBox>
                                                     <ext:ComboBox ID="DDLGuideNameCol" runat="server" AllowBlank="true" Editable="false"
                                                        Visible="false" ForceSelection="false" Width="100">
                                                        <Items>
                                                            <ext:ListItem Value="1" Text="1" />
                                                        </Items>
                                                    </ext:ComboBox>
                                                    </td>
										</tr>
									</table>
								</td>
							</tr>
							<tr>
								<td class="gs-input-separator2" colSpan="2"></td>
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
