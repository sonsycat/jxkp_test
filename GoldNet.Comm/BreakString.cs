
using System;
using System.Text;

namespace GoldNet.Comm
{
	/// <summary>
	/// �ַ����ָ�
	/// </summary>
	public class BreakString
	{
		/// <summary>
		/// ���ַ����Զ���Ϊ�ָ�����зָ�
		/// </summary>
		/// <param name="showString">Ҫ�ָ����ַ���</param>
		/// <returns>����</returns>

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
            string showStringEach = "";    //��ʱ�ַ��������ڴ�ȡ�����е�ÿ���ַ�
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
