using System ;
using System.Text ;

namespace GoldNet.Comm
{
	/// <summary>
	/// ͨ���ַ�ת��
	/// </summary>
	public class CleanString
	{
		/// <summary>
		/// ת�������ַ���Web�ַ���������" ��&lt;��&gt;��'��
		/// </summary>
		/// <param name="inputString">Ҫת�����ַ���</param>
		/// <param name="maxLength">�ַ�����󳤶�</param>
		/// <returns></returns>
		public static string InputText( string inputString , int maxLength )
		{
			StringBuilder retVal = new StringBuilder( ) ;

			// �����������Ƿ�Ϊnull����ַ���.
			if ( ( inputString != null ) && ( inputString != String.Empty ) )
			{
				inputString = inputString.Trim( ) ;

				// ͨ�����������ַ������ض��ַ���,��ֹ���.
				if ( inputString.Length > maxLength )
					inputString = inputString.Substring( 0 , maxLength ) ;

				// ת�������ַ���WEB�ַ�.
				for ( int i = 0 ; i < inputString.Length ; i++ )
				{
					switch ( inputString[ i ] )
					{
						case '"':
							retVal.Append( "&quot;" ) ;
							break ;
						case '<':
							retVal.Append( "&lt;" ) ;
							break ;
						case '>':
							retVal.Append( "&gt;" ) ;
							break ;
                            
						default:
							retVal.Append( inputString[ i ] ) ;
							break ;

					}
				}

				// ת��������Ϊ�ո�.
				retVal.Replace( "'" , " " ) ;
			}

			return retVal.ToString( ) ;
		}

		/// <summary>
		/// ȥ���ַ����еĿո�
		/// </summary>
		/// <param name="input"></param>
		/// <returns></returns>
		public static string getstring(string input)
		{
			string[] Arrayfile=input.Split(new char[] {' '});
			string str2="";   
			for(int i=0;i<Arrayfile.Length;i++)   
			{   
				str2+=Arrayfile[i].ToString();   
			}   
			return str2;

		}


		/// <summary>
		/// ���һ�������ܵ���һ������
		/// </summary>
		/// <param name="date">����</param>
		/// <returns>����һ�������ܵ���һ������</returns>
		public static string GetMonDayByDate( DateTime date )
		{
			int monday = 0 ;
			switch ( date.DayOfWeek )
			{
				case DayOfWeek.Friday: //��ʾ�����塣
					{
						monday = -4 ;
						break ;
					}
				case DayOfWeek.Monday: //��ʾ����һ��
					{
						monday = 0 ;
						break ;
					}

				case DayOfWeek.Saturday: //��ʾ��������
					{
						monday = -5 ;
						break ;
					}

				case DayOfWeek.Sunday: //��ʾ�����ա�
					{
						monday = -6 ;
						break ;
					}

				case DayOfWeek.Thursday: //��ʾ�����ġ�
					{
						monday = -3 ;
						break ;
					}


				case DayOfWeek.Tuesday: //��ʾ���ڶ��� 
					{
						monday = -1 ;
						break ;
					}


				case DayOfWeek.Wednesday: //��ʾ�������� 
					{
						monday = -2 ;
						break ;
					}


			}
			return date.AddDays( monday ).ToShortDateString( ) ;

		}

		/// <summary>
		/// ���ϵͳ��ǰ���ڵ��·ݵ�һ��
		/// </summary>
		/// <returns>����ϵͳ��ǰ���ڵ��·ݵ�һ��</returns>
		public static string GetNowMonthFirstDay( )
		{
			string year = DateTime.Today.Year.ToString( ) ;
			string month = DateTime.Today.Month.ToString( ) ;
			if ( Convert.ToInt16( month ) <= 9 ) month = "0" + month ;
			return year + "-" + month + "-01" ;
		}
	}
}
