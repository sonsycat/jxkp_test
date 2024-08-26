namespace GoldNet.JXKP.PowerManager
{
	/// <summary>
	/// 人员目标对象接口
	/// </summary>
	public interface IStaffTargetInfo : ITargetInfo
	{

		/// <summary>
		/// 获得接口对象的实象
		/// </summary>
		/// <param name="key">人员的序号</param>
		/// <returns>返回一个指定序号的人员对象</returns>
		IStaffTargetInfo GetObjByKey(string key);


	}
}
