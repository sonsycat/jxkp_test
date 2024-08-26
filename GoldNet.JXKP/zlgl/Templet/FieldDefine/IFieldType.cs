using System;
using System.Data.OleDb;

namespace GoldNet.JXKP.Templet.BLL
{
	/// <summary>
	/// �ֶ����ͽӿ� 
	/// </summary>
	public interface IFieldType
	{
		/// <summary>
		/// �����ֶ���������
		/// </summary>
		string FieldTypeName{get;}

		/// <summary>
		/// ���ع��˵Ŀ�ѡ�������ַ���
		/// </summary>
		string[] FilterOperators{get;}

		/// <summary>
		/// ���ػ��ܵĿ�ѡ�������ַ���
		/// </summary>
		string[] CollectModes{get;}

		/// <summary>
		/// �����б���ʾʱ,Ҫ�󶨵������е�����.
		/// </summary>
		string ListDisplayDataName{get;}

		/// <summary>
		/// �����б���ʾʱ��Ҫ�󶨵������еĸ�ʽ���ַ�����
		/// </summary>
		string ListDataFormatString{get;}

		/// <summary>
		/// ��ָ���ؼ�����ʾһ���ֶ����Զ����ҳ��
		/// </summary>
		/// <param name="curCtrl">Ҫ��ʾ�Ŀؼ�</param>
		void ShowSpecialProperty(System.Web.UI.Control curCtrl);

		/// <summary>
		/// ����ֶζ�����������Զ�����Ϣ
		/// </summary>
		/// <param name="curPage">��ʾ��ҳ��</param>
		/// <param name="fieldDefineSql">���ڶ��������ֶε�SQL���</param>
		/// <returns>���ɵ��ֶζ�����</returns>
		int InsertSpecialPropertyFormPage(System.Web.UI.Page curPage, ref string fieldDefineSql);

		/// <summary>
		/// �����ֶζ�����������Զ�����Ϣ
		/// ����Ҳ�������һ���µ��ֶζ��壬������Ƚ��鷳��
		/// </summary>
		/// <param name="curPage">��ʾ��ҳ��</param>
		/// <param name="fieldDefineSql">���ڶ��������ֶε�SQL���</param>
		void UpdateSpecialPropertyFormPage(System.Web.UI.Page curPage, ref string fieldDefineSql);

		/// <summary>
		/// ��ָ���ؼ�����ʾһ��¼��ؼ���
		/// </summary>
		/// <param name="curCtrl">ָ����ҳ��ؼ�</param>
		void ShowInputControl(System.Web.UI.Control curCtrl,bool pass);


		/// <summary>
		/// ����һ����¼
		/// </summary>
		/// <param name="tabName">��ʵ���ݱ�</param>
		/// <param name="recId">��¼��</param>
		/// <param name="curPage">�������ݵ�ҳ��</param>
		void UpdateRecord(OleDbTransaction myTrans,string tabName, int recId, System.Web.UI.Page curPage,int id);

		/// <summary>
		/// ��ʾ�鿴����
		/// </summary>
		/// <param name="tabName">��ʵ���ݱ�</param>
		/// <param name="recID">��¼��</param>
		/// <param name="curCtrl">ָ����ҳ��ؼ�</param>
		void ShowViewData(string tabName, int recID, System.Web.UI.Control curCtrl);

		/// <summary>
		/// ��ָ���ؼ�����ʾһ���޸Ŀؼ���
		/// </summary>
		/// <param name="tabName">��ʵ���ݱ�</param>
		/// <param name="recID">��¼��</param>
		/// <param name="curCtrl">ָ����ҳ��ؼ�</param>
		void ShowEditControl(string tabName, int recID, System.Web.UI.Control curCtrl,bool pass);

		/// <summary>
		/// �õ�������Ϣ
		/// </summary>
		/// <param name="compOperator">�ȽϷ�</param>
		/// <param name="values">ֵ</param>
		/// <returns>�����ַ���</returns>
		string GetFilter(string compOperator, string values);

		/// <summary>
		/// �õ����ܼ��㷽���ַ���
		/// ����:"count(name)"-ͳ��name�еļ�¼����
		/// </summary>
		/// <param name="collectMode">ͳ�Ʒ����ַ���</param>
		/// <returns>���㷽���ַ���</returns>
		string ListCollectComputerString(string collectMode);

	}
}
