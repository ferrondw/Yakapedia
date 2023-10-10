using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;

namespace Yakapedia
{
    public static class Encryption
    {
        /// <summary>
        /// Encrypts data into a byte[] using Rijndeal.
        /// </summary>
        /// <param name="plain">Data to encrypt.</param>
        /// <param name="password">Password to use for encryption.</param>
        /// <returns></returns>
        public static byte[] RijndealEncrypt(byte[] plain, string password)
        {
            byte[] encrypted;
            RijndaelManaged rijndael = SetupRijndaelManaged;
            Rfc2898DeriveBytes deriveBytes = new(password, 32);
            byte[] salt = new byte[32];
            salt = deriveBytes.Salt;
            byte[] bufferKey = deriveBytes.GetBytes(32);

            rijndael.Key = bufferKey;
            rijndael.GenerateIV();

            using (ICryptoTransform encrypt = rijndael.CreateEncryptor(rijndael.Key, rijndael.IV))
            {
                byte[] dest = encrypt.TransformFinalBlock(plain, 0, plain.Length);
                List<byte> compile = new(salt);
                compile.AddRange(rijndael.IV);
                compile.AddRange(dest);
                encrypted = compile.ToArray();
            }

            return encrypted;
        }

        /// <summary>
        /// Decrypts data into a byte[] using Rijndeal.
        /// </summary>
        /// <param name="encrypted">Data to decrypt.</param>
        /// <param name="password">Password to use for decryption.</param>
        /// <returns></returns>
        public static byte[] RijndealDecrypt(byte[] encrypted, string password)
        {
            byte[] decrypted;

            RijndaelManaged rijndael = SetupRijndaelManaged;

            List<byte> compile = encrypted.ToList();

            List<byte> salt = compile.GetRange(0, 32);
            List<byte> iv = compile.GetRange(32, 32);
            rijndael.IV = iv.ToArray();

            Rfc2898DeriveBytes deriveBytes = new(password, salt.ToArray());
            byte[] bufferKey = deriveBytes.GetBytes(32);
            rijndael.Key = bufferKey;

            byte[] plain = compile.GetRange(32 * 2, compile.Count - (32 * 2)).ToArray();

            using (ICryptoTransform decrypt = rijndael.CreateDecryptor(rijndael.Key, rijndael.IV))
            {
                byte[] dest = decrypt.TransformFinalBlock(plain, 0, plain.Length);
                decrypted = dest;
            }

            return decrypted;
        }

        private static RijndaelManaged SetupRijndaelManaged
        {
            get
            {
                RijndaelManaged rijndael = new()
                {
                    BlockSize = 256,
                    KeySize = 256,
                    Mode = CipherMode.CBC,
                    Padding = PaddingMode.PKCS7
                };
                return rijndael;
            }
        }
    }
}