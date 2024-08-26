using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace GoldNet.Comm
{
	/// <summary>
	/// ���ܽ����ַ�����
	/// </summary>
	public class Encrypt
	{
		/// <summary>
		/// ��Կ
		/// </summary>
		private string skey;

		/// <summary>
		/// ��Ҫ���ܵ��ַ���
		/// </summary>
		private string encryptStr;

		/// <summary>
		/// ��Ҫ���ܵ��ַ���
		/// </summary>
		private string unEncryptStr;

		/// <summary>
		/// 
		/// </summary>
		public Encrypt()
		{
			//��Կ��Ĭ��ֵ
			skey = "I am god";
			//���ܵ��ַ�����Ĭ��ֵ
			encryptStr = "";
			//���ܵ��ַ�����Ĭ��ֵ
			unEncryptStr = "";
		}

		/// <summary>
		/// ���캯��
		/// </summary>
		/// <param name="theKey">��Կ����</param>
		public Encrypt(string theKey)
		{
			//��Կ��ֵ
			skey = theKey;
			//���ܵ��ַ�����Ĭ��ֵ
			encryptStr = "";
			//���ܵ��ַ�����Ĭ��ֵ
			unEncryptStr = "";
		}

		/// <summary>
		/// ��Կ
		/// </summary>
		public string Skey
		{
			get{return skey;}
			set{skey = value;}
		}

		/// <summary>
		/// Ҫ���ܵ��ַ���
		/// </summary>
		public string EncryptStr
		{
			get{return encryptStr;}
			set{encryptStr = value;}
		}

		/// <summary>
		/// Ҫ���ܵ��ַ���
		/// </summary>
		public string UnEncryptStr
		{
			get{return unEncryptStr;}
			set{unEncryptStr = value;}
		}

		/// <summary>
		/// �������Լ���
		/// </summary>
		/// <returns></returns>
		private string EncryptTheStr()
		{
			string str = this.EncryptStr ;
			string key = this.Skey;
			return EncryptTheStr(key,str);
		}


		/// <summary>
		/// ���ݲ������ܲ����ַ����������Ի����Կ��
		/// </summary>
		/// <param name="encryptStr">Ҫ���ܵ��ַ���</param>
		/// <returns>�Ѽ��ܵ��ַ���</returns>
		public string EncryptTheStr(string encryptStr)
		{
			string str = encryptStr;
			return EncryptTheStr(this.Skey, str);
		}

		/// <summary>
		/// ������Կ����Ҫ���ܵ��ַ���2����������
		/// </summary>
		/// <param name="theKey">key</param>
		/// <param name="encryptStr">Ҫ���ܵ��ַ���</param>
		/// <returns>�Ѽ��ܵ��ַ���</returns>
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
		/// ������Կֱ�Ӹ��ݲ������ܣ�����ʵ������
		/// </summary>
		/// <param name="myKey">key</param>
		/// <param name="encryptStr">Ҫ���ܵ��ַ���</param>
		/// <returns>�Ѽ��ܵ��ַ���</returns>
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
		/// �������Խ���
		/// </summary>
		/// <returns></returns>
		private string UnEncryptTheStr()
		{
			string key = this.Skey;
			string str = this.UnEncryptStr;
			return UnEncryptTheStr(key,str);
		}

		/// <summary>
		/// ���ݲ������ܲ����ַ����������Ի����Կ��
		/// </summary>
		/// <param name="encryptStr">Ҫ���ܵ��ַ���</param>
		/// <returns>�ѽ��ܵ��ַ���</returns>
		public string UnEncryptTheStr(string encryptStr)
		{
			string key = this.Skey;
			string str = encryptStr;
			return UnEncryptTheStr(key,str);
		}

		/// <summary>
		/// ���ݲ������ܲ����ַ���
		/// </summary>
		/// <param name="theKey">key</param>
		/// <param name="encryptStr">Ҫ���ܵ��ַ���</param>
		/// <returns>�ѽ��ܵ��ַ���</returns>
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
		/// ������Կֱ�Ӹ��ݲ������ܣ�����ʵ������
		/// </summary>
		/// <param name="theKey">key</param>
		/// <param name="encryptStr">Ҫ���ܵ��ַ���</param>
		/// <returns>�ѽ��ܵ��ַ���</returns>
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
