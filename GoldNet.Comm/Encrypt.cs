using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace GoldNet.Comm
{
	/// <summary>
	/// 加密解密字符串类
	/// </summary>
	public class Encrypt
	{
		/// <summary>
		/// 密钥
		/// </summary>
		private string skey;

		/// <summary>
		/// 需要加密的字符串
		/// </summary>
		private string encryptStr;

		/// <summary>
		/// 需要解密的字符串
		/// </summary>
		private string unEncryptStr;

		/// <summary>
		/// 
		/// </summary>
		public Encrypt()
		{
			//密钥的默认值
			skey = "I am god";
			//加密的字符串的默认值
			encryptStr = "";
			//解密的字符串的默认值
			unEncryptStr = "";
		}

		/// <summary>
		/// 构造函数
		/// </summary>
		/// <param name="theKey">密钥参数</param>
		public Encrypt(string theKey)
		{
			//密钥的值
			skey = theKey;
			//加密的字符串的默认值
			encryptStr = "";
			//解密的字符串的默认值
			unEncryptStr = "";
		}

		/// <summary>
		/// 密钥
		/// </summary>
		public string Skey
		{
			get{return skey;}
			set{skey = value;}
		}

		/// <summary>
		/// 要加密的字符串
		/// </summary>
		public string EncryptStr
		{
			get{return encryptStr;}
			set{encryptStr = value;}
		}

		/// <summary>
		/// 要解密的字符串
		/// </summary>
		public string UnEncryptStr
		{
			get{return unEncryptStr;}
			set{unEncryptStr = value;}
		}

		/// <summary>
		/// 根据属性加密
		/// </summary>
		/// <returns></returns>
		private string EncryptTheStr()
		{
			string str = this.EncryptStr ;
			string key = this.Skey;
			return EncryptTheStr(key,str);
		}


		/// <summary>
		/// 根据参数加密参数字符串（从属性获得密钥）
		/// </summary>
		/// <param name="encryptStr">要加密的字符串</param>
		/// <returns>已加密的字符串</returns>
		public string EncryptTheStr(string encryptStr)
		{
			string str = encryptStr;
			return EncryptTheStr(this.Skey, str);
		}

		/// <summary>
		/// 根据密钥和需要加密的字符串2个参数加密
		/// </summary>
		/// <param name="theKey">key</param>
		/// <param name="encryptStr">要加密的字符串</param>
		/// <returns>已加密的字符串</returns>
		public string EncryptTheStr(string theKey, string encryptStr)
		{
			string skey = theKey;
			string SourceStr = encryptStr;
			DESCryptoServiceProvider des = new DESCryptoServiceProvider ();
			Byte[] inputByteArray ;
			inputByteArray = Encoding.Default.GetBytes(SourceStr);
			des.Key = ASCIIEncoding.ASCII.GetBytes(skey);
			des.IV = ASCIIEncoding.ASCII.GetBytes(skey);
			MemoryStream ms = new MemoryStream ();
			CryptoStream cs = new CryptoStream (ms, des.CreateEncryptor(), CryptoStreamMode.Write);
			StreamWriter sw = new StreamWriter (cs);
			sw.Write(SourceStr);
			sw.Flush ();
			cs.FlushFinalBlock();
			ms.Flush ();
			return (Convert.ToBase64String(ms.GetBuffer(), 0, (int)ms.Length));
		}

		/// <summary>
		/// 利用密钥直接根据参数加密，不用实例化类
		/// </summary>
		/// <param name="myKey">key</param>
		/// <param name="encryptStr">要加密的字符串</param>
		/// <returns>已加密的字符串</returns>
		public static string EncryptMyStr(string myKey,string encryptStr)
		{
            if (!encryptStr.ToString().Equals(string.Empty))
            {
                string skey = myKey;
                string SourceStr = encryptStr;
                DESCryptoServiceProvider des = new DESCryptoServiceProvider();
                Byte[] inputByteArray;
                inputByteArray = Encoding.Default.GetBytes(SourceStr);
                des.Key = ASCIIEncoding.ASCII.GetBytes(skey);
                des.IV = ASCIIEncoding.ASCII.GetBytes(skey);
                MemoryStream ms = new MemoryStream();
                CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(), CryptoStreamMode.Write);
                StreamWriter sw = new StreamWriter(cs);
                sw.Write(SourceStr);
                sw.Flush();
                cs.FlushFinalBlock();
                ms.Flush();
                return (Convert.ToBase64String(ms.GetBuffer(), 0, (int)ms.Length));
            }
            else
                return "";
		}


		/// <summary>
		/// 根据属性解密
		/// </summary>
		/// <returns></returns>
		private string UnEncryptTheStr()
		{
			string key = this.Skey;
			string str = this.UnEncryptStr;
			return UnEncryptTheStr(key,str);
		}

		/// <summary>
		/// 根据参数解密参数字符串（从属性获得密钥）
		/// </summary>
		/// <param name="encryptStr">要解密的字符串</param>
		/// <returns>已解密的字符串</returns>
		public string UnEncryptTheStr(string encryptStr)
		{
			string key = this.Skey;
			string str = encryptStr;
			return UnEncryptTheStr(key,str);
		}

		/// <summary>
		/// 根据参数解密参数字符串
		/// </summary>
		/// <param name="theKey">key</param>
		/// <param name="encryptStr">要解密的字符串</param>
		/// <returns>已解密的字符串</returns>
		public string UnEncryptTheStr(string theKey,string encryptStr)
		{
			string skey = theKey;
			DESCryptoServiceProvider des = new DESCryptoServiceProvider ();		
			des.Key = ASCIIEncoding.ASCII.GetBytes(skey);		
			des.IV = ASCIIEncoding.ASCII.GetBytes(skey);		
			Byte[] buffer = Convert.FromBase64String(encryptStr);	
			MemoryStream ms = new MemoryStream (buffer);	
			CryptoStream cs = new CryptoStream (ms, des.CreateDecryptor(), CryptoStreamMode.Read);	
			StreamReader sr = new StreamReader (cs);	
			return sr.ReadToEnd ();
		}

		/// <summary>
		/// 利用密钥直接根据参数解密，不用实例化类
		/// </summary>
		/// <param name="theKey">key</param>
		/// <param name="encryptStr">要解密的字符串</param>
		/// <returns>已解密的字符串</returns>
		public static string UnEncryptMyStr(string theKey,string encryptStr)
		{
			string skey = theKey;
			DESCryptoServiceProvider des = new DESCryptoServiceProvider ();		
			des.Key = ASCIIEncoding.ASCII.GetBytes(skey);		
			des.IV = ASCIIEncoding.ASCII.GetBytes(skey);		
			Byte[] buffer = Convert.FromBase64String(encryptStr);	
			MemoryStream ms = new MemoryStream (buffer);	
			CryptoStream cs = new CryptoStream (ms, des.CreateDecryptor(), CryptoStreamMode.Read);	
			StreamReader sr = new StreamReader (cs);	
			return sr.ReadToEnd ();
		}
	}
}
