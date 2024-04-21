
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Library.Security
{
    public interface IEncryptionService
    {
        string Encrypt(string PlainText);
        string DecryptPayload(string PlainText);
    }

    public class EncryptionService : IEncryptionService
    {
        public string DecryptPayload(string PlainText)
        {
            string key = ConfigurationManager.AppSettings["EmailAPIKey"];
            string Iv = ConfigurationManager.AppSettings["EmailAPIKey"];
            byte[] iv = new byte[16];
            byte[] buffer = Convert.FromBase64String(PlainText);
            using (Aes aes = Aes.Create())
            {
                aes.Key = Encoding.UTF8.GetBytes(key);
                aes.IV = Encoding.UTF8.GetBytes(Iv);
                ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);
                using (MemoryStream ms = new MemoryStream(buffer))
                {
                    using (CryptoStream cryptoStream = new CryptoStream((Stream)ms, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader sr = new StreamReader(cryptoStream))
                        {
                            return sr.ReadToEnd();
                        }
                    }
                };

            }
        }

        public string Encrypt(string PlainText)
        {
            string key = ConfigurationManager.AppSettings["EncryptionSecretKey"];
            string Iv = ConfigurationManager.AppSettings["EncryptionIV"];
            byte[] iv = new byte[16];
            byte[] array;
            using (Aes aes = Aes.Create())
            {
                aes.Key = Encoding.UTF8.GetBytes(key);
                aes.IV = Encoding.UTF8.GetBytes(Iv);
                ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cryptostream = new CryptoStream((Stream)ms, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter streamWriter = new StreamWriter((Stream)cryptostream))
                        {
                            streamWriter.Write(PlainText);
                        }

                        array = ms.ToArray();
                    }
                }
            }
            return Convert.ToBase64String(array);
        }
    }
}
