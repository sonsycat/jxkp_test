using System;
using System.Security.Cryptography;  
using System.Text;
using System.Management;
using System.Runtime.InteropServices;
namespace GoldNet.Comm
{
	/// <summary>
	/// ��̬����/����
	/// </summary>
	public class DEncrypt
	{
		/// <summary>
		/// ���췽��
		/// </summary>
		public DEncrypt()  
		{  
		} 

		#region ʹ�� ȱʡ��Կ�ַ��� ����/����string

		/// <summary>
		/// ʹ��ȱʡ��Կ�ַ�������string
		/// </summary>
		/// <param name="original">����</param>
		/// <returns>����</returns>
		public static string Encrypt(string original)
		{
			return Encrypt(original,"GOLDNET");
		}
		/// <summary>
		/// ʹ��ȱʡ��Կ�ַ�������string
		/// </summary>
		/// <param name="original">����</param>
		/// <returns>����</returns>
		public static string Decrypt(string original)
		{
            return Decrypt(original, "GOLDNET", System.Text.Encoding.Default);
		}

		#endregion

		#region ʹ�� ������Կ�ַ��� ����/����string
		/// <summary>
		/// ʹ�ø�����Կ�ַ�������string
		/// </summary>
		/// <param name="original">ԭʼ����</param>
		/// <param name="key">��Կ</param>
		/// <param name="encoding">�ַ����뷽��</param>
		/// <returns>����</returns>
		public static string Encrypt(string original, string key)  
		{  
			byte[] buff = System.Text.Encoding.Default.GetBytes(original);  
			byte[] kb = System.Text.Encoding.Default.GetBytes(key);
			return Convert.ToBase64String(Encrypt(buff,kb));      
		}
		/// <summary>
		/// ʹ�ø�����Կ�ַ�������string
		/// </summary>
		/// <param name="original">����</param>
		/// <param name="key">��Կ</param>
		/// <returns>����</returns>
		public static string Decrypt(string original, string key)
		{
			return Decrypt(original,key,System.Text.Encoding.Default);
		}

		/// <summary>
		/// ʹ�ø�����Կ�ַ�������string,����ָ�����뷽ʽ����
		/// </summary>
		/// <param name="encrypted">����</param>
		/// <param name="key">��Կ</param>
		/// <param name="encoding">�ַ����뷽��</param>
		/// <returns>����</returns>
		public static string Decrypt(string encrypted, string key,Encoding encoding)  
		{       
			byte[] buff = Convert.FromBase64String(encrypted);  
			byte[] kb = System.Text.Encoding.Default.GetBytes(key);
			return encoding.GetString(Decrypt(buff,kb));      
		}  
		#endregion

		#region ʹ�� ȱʡ��Կ�ַ��� ����/����/byte[]
		/// <summary>
		/// ʹ��ȱʡ��Կ�ַ�������byte[]
		/// </summary>
		/// <param name="encrypted">����</param>
		/// <param name="key">��Կ</param>
		/// <returns>����</returns>
		public static byte[] Decrypt(byte[] encrypted)  
		{
            byte[] key = System.Text.Encoding.Default.GetBytes("GOLDNET"); 
			return Decrypt(encrypted,key);     
		}
		/// <summary>
		/// ʹ��ȱʡ��Կ�ַ�������
		/// </summary>
		/// <param name="original">ԭʼ����</param>
		/// <param name="key">��Կ</param>
		/// <returns>����</returns>
		public static byte[] Encrypt(byte[] original)  
		{
            byte[] key = System.Text.Encoding.Default.GetBytes("GOLDNET"); 
			return Encrypt(original,key);     
		}  
		#endregion

		#region  ʹ�� ������Կ ����/����/byte[]

		/// <summary>
		/// ����MD5ժҪ
		/// </summary>
		/// <param name="original">����Դ</param>
		/// <returns>ժҪ</returns>
		public static byte[] MakeMD5(byte[] original)
		{
			MD5CryptoServiceProvider hashmd5 = new MD5CryptoServiceProvider();   
			byte[] keyhash = hashmd5.ComputeHash(original);       
			hashmd5 = null;  
			return keyhash;
		}


		/// <summary>
		/// ʹ�ø�����Կ����
		/// </summary>
		/// <param name="original">����</param>
		/// <param name="key">��Կ</param>
		/// <returns>����</returns>
		public static byte[] Encrypt(byte[] original, byte[] key)  
		{  
			TripleDESCryptoServiceProvider des = new TripleDESCryptoServiceProvider();       
			des.Key =  MakeMD5(key);
			des.Mode = CipherMode.ECB;  
     
			return des.CreateEncryptor().TransformFinalBlock(original, 0, original.Length);     
		}  

		/// <summary>
		/// ʹ�ø�����Կ��������
		/// </summary>
		/// <param name="encrypted">����</param>
		/// <param name="key">��Կ</param>
		/// <returns>����</returns>
		public static byte[] Decrypt(byte[] encrypted, byte[] key)  
		{  
			TripleDESCryptoServiceProvider des = new TripleDESCryptoServiceProvider();  
			des.Key =  MakeMD5(key);    
			des.Mode = CipherMode.ECB;  

			return des.CreateDecryptor().TransformFinalBlock(encrypted, 0, encrypted.Length);
		}  
  
		#endregion

         ///   <summary>    
         ///   ��ȡ����Ӳ����ַ    
         ///   </summary>    
         ///   <returns> string </returns>    
         //public string GetMoAddress()   
         //{   
         //    string MoAddress = " ";   
         //    using (ManagementClass mc = new ManagementClass("Win32_NetworkAdapterConfiguration"))   
         //    {   
         //        ManagementObjectCollection moc2 = mc.GetInstances();   
         //        foreach (ManagementObject mo in moc2)   
         //        {   
         //            if ((bool)mo["IPEnabled"] == true)   
         //                MoAddress = mo["MacAddress"].ToString();   
         //            mo.Dispose();   
         //        }   
         //    }   
         //    return MoAddress.ToString();   
         //}

        public static string GetMacAddress()
         {
             string addr = "";
             try
             {
                 int cb;
                 ASTAT adapter;
                 NCB Ncb = new NCB();
                 char uRetCode;
                 LANA_ENUM lenum;

                 Ncb.ncb_command = (byte)NCBCONST.NCBENUM;
                 cb = Marshal.SizeOf(typeof(LANA_ENUM));
                 Ncb.ncb_buffer = Marshal.AllocHGlobal(cb);
                 Ncb.ncb_length = (ushort)cb;
                 uRetCode = Win32API.Netbios(ref Ncb);
                 lenum = (LANA_ENUM)Marshal.PtrToStructure(Ncb.ncb_buffer, typeof(LANA_ENUM));
                 Marshal.FreeHGlobal(Ncb.ncb_buffer);
                 if (uRetCode != (short)NCBCONST.NRC_GOODRET)
                     return "";

                 for (int i = 0; i < lenum.length; i++)
                 {
                     Ncb.ncb_command = (byte)NCBCONST.NCBRESET;
                     Ncb.ncb_lana_num = lenum.lana[i];
                     uRetCode = Win32API.Netbios(ref Ncb);
                     if (uRetCode != (short)NCBCONST.NRC_GOODRET)
                         return "";

                     Ncb.ncb_command = (byte)NCBCONST.NCBASTAT;
                     Ncb.ncb_lana_num = lenum.lana[i];
                     Ncb.ncb_callname[0] = (byte)'*';
                     cb = Marshal.SizeOf(typeof(ADAPTER_STATUS)) + Marshal.SizeOf(typeof(NAME_BUFFER)) * (int)NCBCONST.NUM_NAMEBUF;
                     Ncb.ncb_buffer = Marshal.AllocHGlobal(cb);
                     Ncb.ncb_length = (ushort)cb;
                     uRetCode = Win32API.Netbios(ref Ncb);
                     adapter.adapt = (ADAPTER_STATUS)Marshal.PtrToStructure(Ncb.ncb_buffer, typeof(ADAPTER_STATUS));
                     Marshal.FreeHGlobal(Ncb.ncb_buffer);

                     if (uRetCode == (short)NCBCONST.NRC_GOODRET)
                     {
                         if (i > 0)
                             addr += ":";
                         addr = string.Format("{0,2:X}{1,2:X}{2,2:X}{3,2:X}{4,2:X}{5,2:X}",
                         adapter.adapt.adapter_address[0],
                         adapter.adapt.adapter_address[1],
                         adapter.adapt.adapter_address[2],
                         adapter.adapt.adapter_address[3],
                         adapter.adapt.adapter_address[4],
                         adapter.adapt.adapter_address[5]);
                     }
                 }
             }
             catch
             {
             }
             return addr.Replace(' ', '0');
         }

         public enum NCBCONST
         {
             NCBNAMSZ = 16, /**//**//**//* absolute length of a net name */
             MAX_LANA = 254, /**//**//**//* lana's in range 0 to MAX_LANA inclusive */
             NCBENUM = 0x37, /**//**//**//* NCB ENUMERATE LANA NUMBERS */
             NRC_GOODRET = 0x00, /**//**//**//* good return */
             NCBRESET = 0x32, /**//**//**//* NCB RESET */
             NCBASTAT = 0x33, /**//**//**//* NCB ADAPTER STATUS */
             NUM_NAMEBUF = 30, /**//**//**//* Number of NAME's BUFFER */
         }

         [StructLayout(LayoutKind.Sequential)]
         public struct ADAPTER_STATUS
         {
             [MarshalAs(UnmanagedType.ByValArray, SizeConst = 6)]
             public byte[] adapter_address;
             public byte rev_major;
             public byte reserved0;
             public byte adapter_type;
             public byte rev_minor;
             public ushort duration;
             public ushort frmr_recv;
             public ushort frmr_xmit;
             public ushort iframe_recv_err;
             public ushort xmit_aborts;
             public uint xmit_success;
             public uint recv_success;
             public ushort iframe_xmit_err;
             public ushort recv_buff_unavail;
             public ushort t1_timeouts;
             public ushort ti_timeouts;
             public uint reserved1;
             public ushort free_ncbs;
             public ushort max_cfg_ncbs;
             public ushort max_ncbs;
             public ushort xmit_buf_unavail;
             public ushort max_dgram_size;
             public ushort pending_sess;
             public ushort max_cfg_sess;
             public ushort max_sess;
             public ushort max_sess_pkt_size;
             public ushort name_count;
         }

         [StructLayout(LayoutKind.Sequential)]
         public struct NAME_BUFFER
         {
             [MarshalAs(UnmanagedType.ByValArray, SizeConst = (int)NCBCONST.NCBNAMSZ)]
             public byte[] name;
             public byte name_num;
             public byte name_flags;
         }

         [StructLayout(LayoutKind.Sequential)]
         public struct NCB
         {
             public byte ncb_command;
             public byte ncb_retcode;
             public byte ncb_lsn;
             public byte ncb_num;
             public IntPtr ncb_buffer;
             public ushort ncb_length;
             [MarshalAs(UnmanagedType.ByValArray, SizeConst = (int)NCBCONST.NCBNAMSZ)]
             public byte[] ncb_callname;
             [MarshalAs(UnmanagedType.ByValArray, SizeConst = (int)NCBCONST.NCBNAMSZ)]
             public byte[] ncb_name;
             public byte ncb_rto;
             public byte ncb_sto;
             public IntPtr ncb_post;
             public byte ncb_lana_num;
             public byte ncb_cmd_cplt;
             [MarshalAs(UnmanagedType.ByValArray, SizeConst = 10)]
             public byte[] ncb_reserve;
             public IntPtr ncb_event;
         }

         [StructLayout(LayoutKind.Sequential)]
         public struct LANA_ENUM
         {
             public byte length;
             [MarshalAs(UnmanagedType.ByValArray, SizeConst = (int)NCBCONST.MAX_LANA)]
             public byte[] lana;
         }

         [StructLayout(LayoutKind.Auto)]
         public struct ASTAT
         {
             public ADAPTER_STATUS adapt;
             [MarshalAs(UnmanagedType.ByValArray, SizeConst = (int)NCBCONST.NUM_NAMEBUF)]
             public NAME_BUFFER[] NameBuff;
         }
         public class Win32API
         {
             [DllImport("NETAPI32.DLL")]
             public static extern char Netbios(ref NCB ncb);
         }



		
	}
}
