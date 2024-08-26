<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Other_dept_cbhs_sqlexpress_edit.aspx.cs" Inherits="GoldNet.JXKP.Other_dept_cbhs_sqlexpress_edit" %>
<%@ Register Assembly="Goldnet.Ext.Web" Namespace="Goldnet.Ext.Web" TagPrefix="ext" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title></title>
       <style type="text/css">
        body{
         background-color: #DFE8F6;
         font-size:12px;
        }
    </style>
     <ext:ScriptManager ID="ScriptManager1" runat="server">
 </ext:ScriptManager>
 
</head>
<body>
 <form id="form1" runat="server"  style="background-color:Transparent" >   
    <div>
        <ext:FormPanel ID="FormPanel1" runat="server" Border="false" AutoScroll="false" ButtonAlign="Right"
            StyleSpec="background-color:Transparent" BodyStyle="background-color:Transparent;margin:10px,0,0,10px">
            <Body>               
               
                <ext:FormLayout ID="FormLayout1" runat="server">
                   
                    <ext:Anchor Horizontal="95%">
                         <ext:TextField runat="server" ID="SQL_NAME"  FieldLabel="语句名称"  BlankText="请输入语句名称" />
                    </ext:Anchor>
                    <ext:Anchor>
                    <ext:ComboBox ID="flags" runat="server" ReadOnly="true" ForceSelection="true" SelectOnFocus="true" Width="100" FieldLabel="是否启用">
                                                <Items>
                                                     <ext:ListItem Text="启用" Value="0" />
                                                    <ext:ListItem Text="停用" Value="1" />
                                                </Items>
                                            </ext:ComboBox>
                    </ext:Anchor>
                    <ext:Anchor Horizontal="95%">
                        <ext:TextArea runat="server" ID="SQLEXPRESS"  CausesValidation="true"  FieldLabel="语句"   AllowBlank="false"  Height="200" />
                    </ext:Anchor>
                    <ext:Anchor Horizontal="95%">
                        <ext:TextArea runat="server" ID="MEMO"  FieldLabel="备注" Height="100" />
                    </ext:Anchor>
                    
                </ext:FormLayout>
                
            </Body>
            <Buttons>
                <ext:Button ID="BtnSave" runat="server" Text="保存" Icon="Disk">
                    <AjaxEvents>
                        <Click OnEvent="SaveExpress_onClick" >
                        </Click>
                    </AjaxEvents>
                </ext:Button>
                <ext:Button ID="CancelButton" runat="server" Text="取消" Icon="Cancel">
                    <Listeners>
                        <Click Handler="parent.DetailWin.hide();" />
                    </Listeners>
                </ext:Button>
            </Buttons>
            
       </ext:FormPanel>
    </div>
    </form>
</body>
</html>

