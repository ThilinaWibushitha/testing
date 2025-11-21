using System.Net.NetworkInformation;
using System.Security.Cryptography;
using System.Text;

namespace my_pospointe
{
	public class CLSencryption
	{
		public static string Encrypt(string clearText)
		{
			string EncryptionKey = "Admin573184#";
			byte[] clearBytes = Encoding.Unicode.GetBytes(clearText);
			using (Aes encryptor = Aes.Create())
			{
				Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
				encryptor.Key = pdb.GetBytes(32);
				encryptor.IV = pdb.GetBytes(16);
				using (MemoryStream ms = new MemoryStream())
				{
					using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
					{
						cs.Write(clearBytes, 0, clearBytes.Length);
						cs.Close();
					}
					clearText = Convert.ToBase64String(ms.ToArray());
				}
			}
			return clearText;
		}

		public static string Decrypt(string cipherText)
		{
			string EncryptionKey = "Admin573184#";
			cipherText = cipherText.Replace(" ", "+");
			try
			{
				byte[] cipherBytes = Convert.FromBase64String(cipherText);
				using (Aes encryptor = Aes.Create())
				{
					Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
					encryptor.Key = pdb.GetBytes(32);
					encryptor.IV = pdb.GetBytes(16);
					using (MemoryStream ms = new MemoryStream())
					{
						using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
						{
							cs.Write(cipherBytes, 0, cipherBytes.Length);
							cs.Close();
						}
						cipherText = Encoding.Unicode.GetString(ms.ToArray());
					}
				}
			}
			catch (Exception)
			{

			}

			return cipherText;
		}

		public static string getMac()
		{
			var macAddr =
					(
						from nic in NetworkInterface.GetAllNetworkInterfaces()
						where nic.OperationalStatus == OperationalStatus.Up
						select nic.GetPhysicalAddress().ToString()
					).FirstOrDefault();
			return macAddr.ToString();
		}

		public static string EncryptDbString(string clearText)
		{

			string EncryptionKey = getMac();

			byte[] clearBytes = Encoding.Unicode.GetBytes(clearText);
			using (Aes encryptor = Aes.Create())
			{
				Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
				encryptor.Key = pdb.GetBytes(32);
				encryptor.IV = pdb.GetBytes(16);
				using (MemoryStream ms = new MemoryStream())
				{
					using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
					{
						cs.Write(clearBytes, 0, clearBytes.Length);
						cs.Close();
					}
					clearText = Convert.ToBase64String(ms.ToArray());
				}
			}

			return clearText;
		}


		public static string DecryptDbString(string cipherText)
		{
			string EncryptionKey = getMac();
			cipherText = cipherText.Replace(" ", "+");
			try
			{
				byte[] cipherBytes = Convert.FromBase64String(cipherText);
				using (Aes encryptor = Aes.Create())
				{
					Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
					encryptor.Key = pdb.GetBytes(32);
					encryptor.IV = pdb.GetBytes(16);
					using (MemoryStream ms = new MemoryStream())
					{
						using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
						{
							cs.Write(cipherBytes, 0, cipherBytes.Length);
							cs.Close();
						}
						cipherText = Encoding.Unicode.GetString(ms.ToArray());
					}
				}
			}
			catch (Exception)
			{

			}

			return cipherText;
		}

		public static string Encrypt(string prm_text_to_encrypt, string prm_key, string prm_iv)
		{
			var sToEncrypt = prm_text_to_encrypt;

			var rj = new RijndaelManaged()
			{
				Padding = PaddingMode.PKCS7,
				Mode = CipherMode.CBC,
				KeySize = 256,
				BlockSize = 256,
			};

			var key = Convert.FromBase64String(prm_key);
			var IV = Convert.FromBase64String(prm_iv);

			var encryptor = rj.CreateEncryptor(key, IV);

			var msEncrypt = new MemoryStream();
			var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write);

			var toEncrypt = Encoding.ASCII.GetBytes(sToEncrypt);

			csEncrypt.Write(toEncrypt, 0, toEncrypt.Length);
			csEncrypt.FlushFinalBlock();

			var encrypted = msEncrypt.ToArray();

			return (Convert.ToBase64String(encrypted));
		}

		public static string Decrypt(string prm_text_to_decrypt, string prm_key, string prm_iv)
		{

			var sEncryptedString = prm_text_to_decrypt;

			var rj = new RijndaelManaged()
			{
				Padding = PaddingMode.PKCS7,
				Mode = CipherMode.CBC,
				KeySize = 256,
				BlockSize = 256,
			};

			var key = Convert.FromBase64String(prm_key);
			var IV = Convert.FromBase64String(prm_iv);

			var decryptor = rj.CreateDecryptor(key, IV);

			var sEncrypted = Convert.FromBase64String(sEncryptedString);

			var fromEncrypt = new byte[sEncrypted.Length];

			var msDecrypt = new MemoryStream(sEncrypted);
			var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read);

			csDecrypt.Read(fromEncrypt, 0, fromEncrypt.Length);

			return (Encoding.ASCII.GetString(fromEncrypt));
		}

		public static void GenerateKeyIV(out string key, out string IV)
		{
			var rj = new RijndaelManaged()
			{
				Padding = PaddingMode.PKCS7,
				Mode = CipherMode.CBC,
				KeySize = 256,
				BlockSize = 256,
			};
			rj.GenerateKey();
			rj.GenerateIV();

			key = Convert.ToBase64String(rj.Key);
			IV = Convert.ToBase64String(rj.IV);
		}

	}
}
