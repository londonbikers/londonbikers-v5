using System;
using System.IO;
using System.Text;
using System.Configuration;
using System.Security.Cryptography;

namespace Apollo.Utilities
{
	public class SecurityHelpers
	{
		public static string DesEncrypt(string toEncrypt) 
		{
			byte[] iv = {18, 52, 86, 120, 144, 171, 205, 239};

			//-- Convert input into byte 
			var input = Encoding.UTF8.GetBytes(toEncrypt); 
			var des = new DESCryptoServiceProvider(); 
			var ms = new MemoryStream(); 
			
			//-- Get key and IV (generated on application startup) using des.GenerateKey(); 
            des.Key = Encoding.UTF8.GetBytes(ConfigurationManager.AppSettings["Helpers.EncryptionKey"]);
			des.IV = iv;
			
			//-- Encrypt 
			var cs = new CryptoStream(ms, des.CreateEncryptor(des.Key, des.IV), CryptoStreamMode.Write); 
			cs.Write(input, 0, input.Length); 
			cs.FlushFinalBlock(); 
			
			//-- Convert to string
			var encrypt = Convert.ToBase64String(ms.ToArray()); 
			
			//-- Url encode 
			encrypt = System.Web.HttpContext.Current.Server.UrlEncode(encrypt); 
            ms.Close();

			return encrypt; 
		} 

		public static string DesDecrypt(string toDecrypt) 
		{ 
			byte[] iv = {18, 52, 86, 120, 144, 171, 205, 239};

			//-- Convert string to byte 
			var input = Convert.FromBase64String(toDecrypt); 
			var des = new DESCryptoServiceProvider(); 
			var ms = new MemoryStream(); 
			
			//-- Get key and IV 
            des.Key = Encoding.UTF8.GetBytes(ConfigurationManager.AppSettings["Helpers.EncryptionKey"]);
			des.IV = iv;
			var cs = new CryptoStream(ms, des.CreateDecryptor(des.Key, des.IV), CryptoStreamMode.Write); 
			cs.Write(input, 0, input.Length); 
			cs.FlushFinalBlock(); 
			
			//-- Get string 
			Encoding encoding = new UTF8Encoding(); 
			var decrypted = encoding.GetString(ms.ToArray()); 
            ms.Close();

			return decrypted; 
		}
	}
}