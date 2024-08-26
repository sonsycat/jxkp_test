namespace GoldNet.JXKP.PowerManager
{
	/// <summary>
	/// 目标对象信息接口
	/// </summary>
	public interface ITargetInfo
	{

		/// <summary>
		/// 目标对象的主键
		/// </summary>
		string Key { get; }

		/// <summary>
		/// 目标对象的名称
		/// </summary>
		string Name { get; }
		/// <summary>
		/// 科室
		/// </summary>
		string Dept { get;}
	}
}
