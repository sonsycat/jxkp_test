using System;
using System.Data.OleDb;

namespace GoldNet.JXKP.Templet.BLL
{
	/// <summary>
	/// 字段类型接口 
	/// </summary>
	public interface IFieldType
	{
		/// <summary>
		/// 返回字段类型名称
		/// </summary>
		string FieldTypeName{get;}

		/// <summary>
		/// 返回过滤的可选方法的字符串
		/// </summary>
		string[] FilterOperators{get;}

		/// <summary>
		/// 返回汇总的可选方法的字符串
		/// </summary>
		string[] CollectModes{get;}

		/// <summary>
		/// 返回列表显示时,要绑定的数据列的名称.
		/// </summary>
		string ListDisplayDataName{get;}

		/// <summary>
		/// 返回列表显示时，要绑定的数据列的格式化字符串。
		/// </summary>
		string ListDataFormatString{get;}

		/// <summary>
		/// 在指定控件中显示一个字段属性定义的页面
		/// </summary>
		/// <param name="curCtrl">要显示的控件</param>
		void ShowSpecialProperty(System.Web.UI.Control curCtrl);

		/// <summary>
		/// 添加字段定义的特殊属性定义信息
		/// </summary>
		/// <param name="curPage">显示的页面</param>
		/// <param name="fieldDefineSql">用于定义数据字段的SQL语句</param>
		/// <returns>生成的字段定义编号</returns>
		int InsertSpecialPropertyFormPage(System.Web.UI.Page curPage, ref string fieldDefineSql);

		/// <summary>
		/// 更新字段定义的特殊属性定义信息
		/// 这里也可以添加一个新的字段定义，但好像比较麻烦。
		/// </summary>
		/// <param name="curPage">显示的页面</param>
		/// <param name="fieldDefineSql">用于定义数据字段的SQL语句</param>
		void UpdateSpecialPropertyFormPage(System.Web.UI.Page curPage, ref string fieldDefineSql);

		/// <summary>
		/// 在指定控件中显示一个录入控件。
		/// </summary>
		/// <param name="curCtrl">指定的页面控件</param>
		void ShowInputControl(System.Web.UI.Control curCtrl,bool pass);


		/// <summary>
		/// 更新一条记录
		/// </summary>
		/// <param name="tabName">事实数据表</param>
		/// <param name="recId">记录号</param>
		/// <param name="curPage">包含数据的页面</param>
		void UpdateRecord(OleDbTransaction myTrans,string tabName, int recId, System.Web.UI.Page curPage,int id);

		/// <summary>
		/// 显示查看数据
		/// </summary>
		/// <param name="tabName">事实数据表</param>
		/// <param name="recID">记录号</param>
		/// <param name="curCtrl">指定的页面控件</param>
		void ShowViewData(string tabName, int recID, System.Web.UI.Control curCtrl);

		/// <summary>
		/// 在指定控件中显示一个修改控件。
		/// </summary>
		/// <param name="tabName">事实数据表</param>
		/// <param name="recID">记录号</param>
		/// <param name="curCtrl">指定的页面控件</param>
		void ShowEditControl(string tabName, int recID, System.Web.UI.Control curCtrl,bool pass);

		/// <summary>
		/// 得到过滤信息
		/// </summary>
		/// <param name="compOperator">比较符</param>
		/// <param name="values">值</param>
		/// <returns>过滤字符串</returns>
		string GetFilter(string compOperator, string values);

		/// <summary>
		/// 得到汇总计算方法字符串
		/// 类似:"count(name)"-统计name列的记录个数
		/// </summary>
		/// <param name="collectMode">统计方法字符串</param>
		/// <returns>计算方法字符串</returns>
		string ListCollectComputerString(string collectMode);

	}
}
