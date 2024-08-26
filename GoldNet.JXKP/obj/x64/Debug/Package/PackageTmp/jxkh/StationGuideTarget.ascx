<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="StationGuideTarget.ascx.cs"
    Inherits="GoldNet.JXKP.jxkh.StationGuideTarget" %>
<%@ Register Assembly="Goldnet.Ext.Web" Namespace="Goldnet.Ext.Web" TagPrefix="ext" %>
<ext:Store runat="server" ID="Store1">

    <Reader>
        <ext:JsonReader ReaderID="GUIDE_CODE">
            <Fields>
                <ext:RecordField Name="BSC_TYPE" />
                <ext:RecordField Name="BSC_TYPE_NAME" />
                <ext:RecordField Name="BSC" />
                <ext:RecordField Name="BSC_NAME" />
                <ext:RecordField Name="GUIDE_CODE" />
                <ext:RecordField Name="GUIDE_NAME" />
                <ext:RecordField Name="BSCPOINT" />
                <ext:RecordField Name="GUIDE_VALUE" />
                <ext:RecordField Name="GUIDE_CAUSE" />
                <ext:RecordField Name="GUIDE_UNIT">
                    <Convert Fn="ConvertGuideUnit" />
                </ext:RecordField>
                <ext:RecordField Name="INCREASE" />
                <ext:RecordField Name="INCREASE_ARITHMETIC" />
                <ext:RecordField Name="DECREASE" />
                <ext:RecordField Name="DECREASE_ARITHMETIC" />
                <ext:RecordField Name="MINUSFLAG">
                    <Convert Fn="ConvertMinusFlag" />
                </ext:RecordField>
                <ext:RecordField Name="PLUSFLAG">
                    <Convert Fn="ConvertMinusFlag" />
                </ext:RecordField>
                <ext:RecordField Name="FIXNUM">
                    <Convert Fn="ConvertMinusFlag" />
                </ext:RecordField>
                <ext:RecordField Name="PLUS_INCREASE" />
                <ext:RecordField Name="PLUS_ARITHMETIC" />
                <ext:RecordField Name="MINUS_INCREASE" />
                <ext:RecordField Name="MINUS_ARITHMETIC" />
                <ext:RecordField Name="THRESHOLD_VALUE" />
                <ext:RecordField Name="PLUS_LIMIT" />
                <ext:RecordField Name="MINUS_LIMIT" />
            </Fields>
        </ext:JsonReader>
    </Reader>
</ext:Store>
<ext:GridPanel ID="GridPanel1" runat="server" StoreID="Store1" StripeRows="true"
    TrackMouseOver="true" MaskDisabled="false" EnableHdMenu="false" AutoHeight="true" AutoWidth="true" AutoScroll="true"
    ClicksToEdit="1">
    <TopBar>
        <ext:Toolbar ID="Toolbar1" runat="server" Cls="grouptoolbar" StyleSpec="border-top:0;height: 25px;padding-top:5px;">
            <Items>
                <ext:Label runat="server" ID="lbl_groupname" Text="    总分：" Icon="ApplicationViewColumns">
                </ext:Label>
                <ext:NumberField runat="server" ID="num_bscpoint" Width="60" ReadOnly="true">
                </ext:NumberField>
                <%--<ext:Label runat="server" Text="当为反向数据时，请将降低量或增加量设为负值" Cls="tipslabel">
                </ext:Label>--%>
            </Items>
        </ext:Toolbar>
    </TopBar>
    <ColumnModel runat="server" ID="ColumnModel1">
        <Columns>
            <ext:Column ColumnID="BSC_NAME" Header="指标分类" Sortable="false" DataIndex="BSC_NAME"
                Width="80" Hidden="true" />
            <ext:Column ColumnID="GUIDE_NAME" Width="80" Header="指标名称" DataIndex="GUIDE_NAME" />
            <ext:Column ColumnID="INCREASE" Width="60" Header="降低量" Sortable="true" DataIndex="INCREASE"
                Align="Right">
                <Editor>
                    <ext:NumberField runat="server" ID="INCREASE_TXT" SelectOnFocus="true" AllowNegative="false"
                        AllowDecimals="true" DecimalPrecision="2">
                    </ext:NumberField>
                </Editor>
            </ext:Column>
            <ext:Column Width="60" ColumnID="INCREASE_ARITHMETIC" Header="扣分" Sortable="true"
                DataIndex="INCREASE_ARITHMETIC" Align="Right">
                <Editor>
                    <ext:NumberField ID="INCREASE_ARITHMETIC_TXT" runat="server" SelectOnFocus="true"
                        AllowNegative="false" AllowDecimals="true" DecimalPrecision="2">
                    </ext:NumberField>
                </Editor>
            </ext:Column>
            <ext:Column Width="60" ColumnID="DECREASE" Header="增加量" Sortable="true" DataIndex="DECREASE"
                Align="Right">
                <Editor>
                    <ext:NumberField ID="DECREASE_TXT" runat="server" SelectOnFocus="true" AllowNegative="false"
                        AllowDecimals="true" DecimalPrecision="2">
                    </ext:NumberField>
                </Editor>
            </ext:Column>
            <ext:Column Width="60" ColumnID="DECREASE_ARITHMETIC" Header="加分" Sortable="false"
                DataIndex="DECREASE_ARITHMETIC" Align="Right">
                <Editor>
                    <ext:NumberField ID="DECREASE_ARITHMETIC_TXT" runat="server" SelectOnFocus="true"
                        AllowNegative="false" AllowDecimals="true" DecimalPrecision="2">
                    </ext:NumberField>
                </Editor>
            </ext:Column>
            <%--<ext:Column Width="60" ColumnID="PLUS_INCREASE" Header="超增加量" Sortable="true" DataIndex="PLUS_INCREASE"
                Align="Right">
                <Editor>
                    <ext:NumberField ID="NumberField1" runat="server" SelectOnFocus="true" AllowNegative="false"
                        AllowDecimals="true" DecimalPrecision="2">
                    </ext:NumberField>
                </Editor>
            </ext:Column>
            <ext:Column Width="60" ColumnID="PLUS_ARITHMETIC" Header="超加分值" Sortable="false"
                DataIndex="PLUS_ARITHMETIC" Align="Right">
                <Editor>
                    <ext:NumberField ID="NumberField2" runat="server" SelectOnFocus="true" AllowNegative="false"
                        AllowDecimals="true" DecimalPrecision="2">
                    </ext:NumberField>
                </Editor>
            </ext:Column>
            <ext:Column Width="60" ColumnID="MINUS_INCREASE" Header="超降低量" Sortable="true" DataIndex="MINUS_INCREASE"
                Align="Right">
                <Editor>
                    <ext:NumberField ID="NumberField3" runat="server" SelectOnFocus="true" AllowNegative="false"
                        AllowDecimals="true" DecimalPrecision="2">
                    </ext:NumberField>
                </Editor>
            </ext:Column>
            <ext:Column Width="60" ColumnID="MINUS_ARITHMETIC" Header="超扣分值" Sortable="false"
                DataIndex="MINUS_ARITHMETIC" Align="Right">
                <Editor>
                    <ext:NumberField ID="NumberField4" runat="server" SelectOnFocus="true" AllowNegative="false"
                        AllowDecimals="true" DecimalPrecision="2">
                    </ext:NumberField>
                </Editor>
            </ext:Column>--%>
            <ext:CheckColumn Width="50" ColumnID="GUIDE_UNIT" Header="单位(%)" Sortable="true"
                Editable="true" DataIndex="GUIDE_UNIT">
            </ext:CheckColumn>
            <ext:Column Width="60" ColumnID="GUIDE_CAUSE" Header="目标值" Sortable="true" DataIndex="GUIDE_CAUSE"
                Align="Right">
                <Editor>
                    <ext:NumberField ID="GUIDE_CAUSE_TXT" runat="server" SelectOnFocus="true" AllowNegative="false"
                        AllowDecimals="true" DecimalPrecision="2">
                    </ext:NumberField>
                </Editor>
            </ext:Column>
            <ext:Column Width="60" ColumnID="GUIDE_VALUE" Header="指标分值" Sortable="true" DataIndex="GUIDE_VALUE"
                Align="Right">
                <Editor>
                    <ext:NumberField ID="GUIDE_VALUE_TXT" runat="server" SelectOnFocus="true" AllowNegative="false"
                        AllowDecimals="true" DecimalPrecision="2">
                    </ext:NumberField>
                </Editor>
            </ext:Column>
            <ext:CheckColumn Width="60" ColumnID="PLUSFLAG" Header="允许超分" Sortable="true" Editable="true"
                DataIndex="PLUSFLAG">
            </ext:CheckColumn>
            <ext:CheckColumn Width="60" ColumnID="MINUSFLAG" Header="允许负分" Sortable="true" Editable="true"
                DataIndex="MINUSFLAG">
            </ext:CheckColumn>
            <ext:CheckColumn Width="60" ColumnID="FIXNUM" Header="固定分值" Sortable="true" Editable="true"
                DataIndex="FIXNUM">
            </ext:CheckColumn>
            <ext:Column Width="60" ColumnID="THRESHOLD_VALUE" Header="阀值" Sortable="true" DataIndex="THRESHOLD_VALUE"
                Align="Right">
                <Editor>
                    <ext:NumberField ID="NumberField5" runat="server" SelectOnFocus="true" AllowNegative="false"
                        AllowDecimals="true" DecimalPrecision="2" AutoDataBind="true">
                    </ext:NumberField>
                </Editor>
            </ext:Column>
            <ext:Column Width="60" ColumnID="PLUS_LIMIT" Header="超分限制" Sortable="false"
                DataIndex="PLUS_LIMIT" Align="Right">
                <Editor>
                    <ext:NumberField ID="NumberField6" runat="server" SelectOnFocus="true" AllowNegative="false"
                        AllowDecimals="true" DecimalPrecision="2">
                    </ext:NumberField>
                </Editor>
            </ext:Column>
            <ext:Column Width="60" ColumnID="MINUS_LIMIT" Header="减分限制" Sortable="false"
                DataIndex="MINUS_LIMIT" Align="Right">
                <Editor>
                    <ext:NumberField ID="NumberField7" runat="server" SelectOnFocus="true" AllowNegative="false"
                        AllowDecimals="true" DecimalPrecision="2">
                    </ext:NumberField>
                </Editor>
            </ext:Column>
        </Columns>
    </ColumnModel>
    <View>
        <ext:GroupingView runat="server" ID="GroupingView1" ForceFit="false" ShowGroupName="false"
            EnableNoGroups="false" HideGroupedColumn="true" GroupTextTpl="" EnableRowBody="true">
        </ext:GroupingView>
    </View>
    <SelectionModel>
        <ext:RowSelectionModel ID="RowSelectionModel1" runat="server" SingleSelect="true">
        </ext:RowSelectionModel>
    </SelectionModel>
    <Listeners>
        <BeforeRender Handler="Ext.EventManager.onWindowResize(function(){ this.setWidth( Ext.getBody().getViewSize().width -18) }, this)" />
    </Listeners>
</ext:GridPanel>
