namespace GoldNet.JXKP.PowerManager
{
	/// <summary>
	/// ��ԱĿ�����ӿ�
	/// </summary>
	public interface IStaffTargetInfo : ITargetInfo
	{

		/// <summary>
		/// ��ýӿڶ����ʵ��
		/// </summary>
		/// <param name="key">��Ա�����</param>
		/// <returns>����һ��ָ����ŵ���Ա����</returns>
		IStaffTargetInfo GetObjByKey(string key);


	}
}
