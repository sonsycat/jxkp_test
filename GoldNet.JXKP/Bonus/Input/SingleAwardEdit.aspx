<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SingleAwardEdit.aspx.cs"
    Inherits="GoldNet.JXKP.SingleAwardEdit" %>

<%@ Register Assembly="Goldnet.Ext.Web" Namespace="Goldnet.Ext.Web" TagPrefix="ext" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
</head>
<style type="text/css">
    body
    {
        background-color: #DFE8F6;
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

<script type="text/javascript">
    var CheckForm = function() {
        if (cbbdept.validate() == false) {
            return false;
        }
        if (ccbtype.validate() == false) {
            return false;
        }
        if (dfInputDate.validate() == false) {
            return false;
        }
        if (nfNumber.validate() == false) {
            return false;
        }

        return true;
    }
    function KedDown(tagObject, keys) {
        if (keys.getKey() == 13) {
            var str = document.getElementById('dfInputDate').value;
            var reg = /^(\d{4})(\d{2})(\d{2})$/;
            document.getElementById('dfInputDate').value = str.replace(reg, '$1-$2-$3');
        }
    }
    function dept_date() {
        var str = document.getElementById('dfInputDate').value
        Store3.proxy.conn.url = '../WebService/BonusDepts.ashx?dept_date=' + str;
        Store3.reload();
    }
</script>

<ext:ScriptManager ID="ScriptManager1" runat="server">
</ext:ScriptManager>
<ext:Store ID="Store3" runat="server" AutoLoad="true">
</ext:Store>
<ext:Store ID="Store2" runat="server">
    <Reader>
        <ext:JsonReader ReaderID="TYPEID">
            <Fields>
                <ext:RecordField Name="TYPEID" />
                <ext:RecordField Name="TYPENAME" />
            </Fields>
        </ext:JsonReader>
    </Reader>
</ext:Store>
<body>
    <form id="form1" runat="server" style="background-color: Transparent">
    <div>
        <ext:FormPanel ID="FormPanel1" runat="server" Border="false" AutoScroll="false" ButtonAlign="Right"
            StyleSpec="background-color:Transparent" BodyStyle="background-color:Transparent;;margin:10px,0,0,10px">
            <Body>
                <ext:Resizable ID="Resizable3" runat="server" Element="taCheckStan" Handles="South" Wrap="true" Pinned="true" Height="96"  MinHeight="60" Dynamic="true" />
                <ext:Resizable ID="Resizable1" runat="server" Element="taRemark" Handles="South" Wrap="true" Pinned="true" Height="96"  MinHeight="60" Dynamic="true" />
                <ext:FormLayout ID="FormLayout1" runat="server" LabelWidth="80">
                    <ext:Anchor Horizontal="70%">
                        <ext:DateField runat="server" ID="dfInputDate" CausesValidation="true" FieldLabel="奖惩时间" 
                            AllowBlank="false" Format="yyyy-MM-dd" MaxLength="10" EnableKeyEvents="true">
                            <Listeners>
                                    <KeyDown Fn="KedDown" />
                                    <Change Fn="dept_date" /> 
                                </Listeners>
                        </ext:DateField>
                    </ext:Anchor>
                    <ext:Anchor Horizontal="70%">
                        <ext:ComboBox ID="cbbdept" runat="server" StoreID="Store3" DisplayField="DEPT_NAME"
                            ValueField="DEPT_CODE" TypeAhead="false" LoadingText="Searching..." Width="240"
                            PageSize="10" ItemSelector="div.search-item" MinChars="1" FieldLabel="科室信息" ListWidth="240"
                            CausesValidation="true" AllowBlank="false">
                            <Template ID="Template1" runat="server">
                                <tpl for=".">
                                    <div class="search-item">
                                         <h3>{DEPT_NAME}</h3>
                                         </div>
                                  </tpl>                                                                                                       
                            </Template>
                        </ext:ComboBox>
                    </ext:Anchor>                      
                    <ext:Anchor Horizontal="70%">
                        <ext:NumberField runat="server" ID="nfNumber" CausesValidation="true"  FieldLabel="奖惩金额"
                            Text="0" AllowBlank="false" SelectOnFocus="true" DecimalPrecision="2" StyleSpec="text-align:right" >
                        </ext:NumberField>
                    </ext:Anchor>                 
                    <ext:Anchor Horizontal="70%">
                        <ext:ComboBox ID="ccbtype" runat="server" StoreID="Store2" DisplayField="TYPENAME"
                            ValueField="TYPEID" Width="240" FieldLabel="奖惩项目"  CausesValidation="true" AllowBlank="false">
                            <AjaxEvents>
                                <Select OnEvent="TypeSelect">
                                </Select>
                            </AjaxEvents>
                        </ext:ComboBox>
                    </ext:Anchor>
                    <ext:Anchor Horizontal="95%">
                        <ext:TextArea runat="server" ID="taCheckStan" FieldLabel="奖惩标准"  Height="80" />
                    </ext:Anchor>
                    <ext:Anchor Horizontal="95%">
                        <ext:TextArea runat="server" ID="taRemark" FieldLabel="备注"  Height="80" />
                    </ext:Anchor>
                    <ext:Anchor Horizontal="70%">
                        <ext:TextField runat="server" ID="tfInputer" FieldLabel="录入人"  />
                    </ext:Anchor>
                    <ext:Anchor Horizontal="70%">
                        <ext:TextField runat="server" ID="tfInputDate" FieldLabel="录入时间" />
                    </ext:Anchor>
                    <ext:Anchor Horizontal="70%">
                        <ext:TextField runat="server" ID="tfModifier" FieldLabel="修改人"  />
                    </ext:Anchor>
                    <ext:Anchor Horizontal="70%">
                        <ext:TextField runat="server" ID="tfModifyDate" FieldLabel="修改时间"/>
                    </ext:Anchor>
                </ext:FormLayout>
            </Body>
            <Buttons>
                <ext:Button ID="BtnSave" runat="server" Text="保存" Icon="Disk">
                    <AjaxEvents>
                        <Click OnEvent="SaveEditSingleAward" Before="if (CheckForm()== false){ Ext.Msg.alert('系统提示','请根据红线提示填写正确的信息！');return false;};">
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
