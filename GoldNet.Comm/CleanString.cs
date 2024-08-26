using System ;
using System.Text ;

namespace GoldNet.Comm
{
	/// <summary>
	/// 通用字符转换
	/// </summary>
	public class CleanString
	{
		/// <summary>
		/// 转换输入字符到Web字符。包括：" 、&lt;、&gt;、'等
		/// </summary>
		/// <param name="inputString">要转换的字符串</param>
		/// <param name="maxLength">字符的最大长度</param>
		/// <returns></returns>
		public static string InputText( string inputString , int maxLength )
		{
			StringBuilder retVal = new StringBuilder( ) ;

			// 检查输入参数是否为null或空字符串.
			if ( ( inputString != null ) && ( inputString != String.Empty ) )
			{
				inputString = inputString.Trim( ) ;

				// 通过传入的最大字符参数截断字符串,防止溢出.
				if ( inputString.Length > maxLength )
					inputString = inputString.Substring( 0 , maxLength ) ;

				// 转换特殊字符到WEB字符.
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

				// 转换单引号为空格.
				retVal.Replace( "'" , " " ) ;
			}

			return retVal.ToString( ) ;
		}

		/// <summary>
		/// 去掉字符串中的空格
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
		/// 获得一天所在周的周一的日期
		/// </summary>
		/// <param name="date">日期</param>
		/// <returns>返回一天所在周的周一的日期</returns>
		public static string GetMonDayByDate( DateTime date )
		{
			int monday = 0 ;
			switch ( date.DayOfWeek )
			{
				case DayOfWeek.Friday: //表示星期五。
					{
						monday = -4 ;
						break ;
					}
				case DayOfWeek.Monday: //表示星期一。
					{
						monday = 0 ;
						break ;
					}

				case DayOfWeek.Saturday: //表示星期六。
					{
						monday = -5 ;
						break ;
					}

				case DayOfWeek.Sunday: //表示星期日。
					{
						monday = -6 ;
						break ;
					}

				case DayOfWeek.Thursday: //表示星期四。
					{
						monday = -3 ;
						break ;
					}


				case DayOfWeek.Tuesday: //表示星期二。 
					{
						monday = -1 ;
						break ;
					}


				case DayOfWeek.Wednesday: //表示星期三。 
					{
						monday = -2 ;
						break ;
					}


			}
			return date.AddDays( monday ).ToShortDateString( ) ;

		}

		/// <summary>
		/// 获得系统当前日期的月份第一天
		/// </summary>
		/// <returns>返回系统当前日期的月份第一天</returns>
		public static string GetNowMonthFirstDay( )
		{
			string year = DateTime.Today.Year.ToString( ) ;
			string month = DateTime.Today.Month.ToString( ) ;
			if ( Convert.ToInt16( month ) <= 9 ) month = "0" + month ;
			return year + "-" + month + "-01" ;
		}
	}
}
