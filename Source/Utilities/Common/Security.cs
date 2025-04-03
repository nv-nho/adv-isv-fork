using System;
using System.Text;
using System.Security.Cryptography;
using System.IO;
using System.Configuration;

namespace OMS.Utilities
{
    public interface ISecurity
    {
        string Encrypt(string plainText);
        string Decrypt(string cipherText);
        string HashString(string text);
    }
    /// <summary>
    /// Security Utility class
    /// </summary>
    public sealed class Security : ISecurity
    {
        static readonly ISecurity _instance = new Security();
        public static ISecurity Instance
        {
            get { return _instance; }
        }

        // This constant string is used as a "salt" value for the PasswordDeriveBytes function calls.
        // This size of the IV (in bytes) must = (keysize / 8).  Default keysize is 256, so the IV must be
        // 32 bytes long.  Using a 16 character string here gives us 32 bytes when converted to a byte array.
        private const string INIT_VECTOR = "tu89geji340t89u2";

        // This constant is used to determine the keysize of the encryption algorithm.
        private const int KEY_SIZE = 256;

        /// <summary>
        /// Secret phrase
        /// </summary>
        private string SECRET_PHRASE 
        {
            get
            {
                //return ConfigurationManager.AppSettings["OMS.SECRET_PHRASE"];
                return "B77A5C561934E089";
            }
        }

        /// <summary>
        /// Encrypt
        /// </summary>
        /// <param name="plainText">plain plainTextText</param>
        /// <returns>Encrypt text</returns>
        public string Encrypt(string plainText)
        {
            //Init
            byte[] initVectorBytes = Encoding.UTF8.GetBytes(INIT_VECTOR);
            byte[] plainTextBytes = Encoding.UTF8.GetBytes(plainText);
            PasswordDeriveBytes password = new PasswordDeriveBytes(SECRET_PHRASE, null);
            byte[] keyBytes = password.GetBytes(KEY_SIZE / 8);

            //Create Rijndael Managed
            RijndaelManaged symmetricKey = new RijndaelManaged();
            symmetricKey.Mode = CipherMode.CBC;

            //Encrypt
            ICryptoTransform encryptor = symmetricKey.CreateEncryptor(keyBytes, initVectorBytes);
            MemoryStream memoryStream = new MemoryStream();
            CryptoStream cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write);
            cryptoStream.Write(plainTextBytes, 0, plainTextBytes.Length);
            cryptoStream.FlushFinalBlock();
            byte[] cipherTextBytes = memoryStream.ToArray();
            memoryStream.Close();
            cryptoStream.Close();

            return Convert.ToBase64String(cipherTextBytes);
        }

        /// <summary>
        /// Decrypt
        /// </summary>
        /// <param name="cipherText">cipher Text</param>
        /// <returns>plain Text</returns>
        public string Decrypt(string cipherText)
        {
            //Init
            byte[] initVectorBytes = Encoding.ASCII.GetBytes(INIT_VECTOR);
            byte[] cipherTextBytes = Convert.FromBase64String(cipherText);
            PasswordDeriveBytes password = new PasswordDeriveBytes(SECRET_PHRASE, null);
            byte[] keyBytes = password.GetBytes(KEY_SIZE / 8);

            //Create Rijndael Managed
            RijndaelManaged symmetricKey = new RijndaelManaged();
            symmetricKey.Mode = CipherMode.CBC;

            //Decrypt
            ICryptoTransform decryptor = symmetricKey.CreateDecryptor(keyBytes, initVectorBytes);
            MemoryStream memoryStream = new MemoryStream(cipherTextBytes);
            CryptoStream cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read);
            byte[] plainTextBytes = new byte[cipherTextBytes.Length];
            int decryptedByteCount = cryptoStream.Read(plainTextBytes, 0, plainTextBytes.Length);
            memoryStream.Close();
            cryptoStream.Close();

            return Encoding.UTF8.GetString(plainTextBytes, 0, decryptedByteCount);
        }

        /// <summary>
        /// Hash String 
        /// </summary>
        /// <param name="text"></param>
        /// <returns>Hex String hashed</returns>
        public string HashString(string text)
        {
            MD5 sha = MD5.Create();
            byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(text);
            byte[] hash = sha.ComputeHash(inputBytes);
            hash = sha.ComputeHash(hash);
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            for (int i = 0; i < hash.Length; i++)
            {
                sb.Append(hash[i].ToString("X2"));
            }
            return sb.ToString();
        }
    }
}
