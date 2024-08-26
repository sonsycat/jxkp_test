
using System;
using System.Text;

namespace GoldNet.Comm
{
	/// <summary>
	/// 字符串分隔
	/// </summary>
	public class BreakString
	{
		/// <summary>
		/// 将字符串以逗号为分隔点进行分隔
		/// </summary>
		/// <param name="showString">要分隔的字符串</param>
		/// <returns>数组</returns>

		public static string[] ShowText(string showString)
		{
			int showStringLen = Convert.ToInt32(showString.Trim().ToString().Length);
            int arrayLen = 0;
			for(int i=0;i<showStringLen;i++)
			{
				if(showString.Substring(i,1).ToString() == ','.ToString())
				{
					arrayLen =arrayLen + 1;
				}
			}
            
			string[] showStringArray = new string[arrayLen];

			int j = 0;
            string showStringEach = "";    //临时字符串，用于存取数组中的每个字符
			for(int k=0;k<showStringLen;k++)
			{				
				if(showString.Substring(k,1).ToString() != ','.ToString())
				{
					showStringEach = showStringEach + showString.Substring(k,1);
				}
				else
				{
					showStringArray[j] = showStringEach;
					j=j+1;
					showStringEach = "";
				}
			}

			return showStringArray;

		}
	}
}
