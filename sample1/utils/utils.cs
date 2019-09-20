using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using ICSharpCode.SharpZipLib.GZip;

namespace sample1.utils
{
    public class utils
    {
        /// <summary>
        /// AES解密
        /// </summary>
        /// <param name="src">解密数据</param>
        /// <param name="aesKey">密钥字符串(需要和加密时相同)</param>
        /// <returns></returns>
        public static byte[] AesDecrypt(byte[] src, byte[] aesKey)
        {
            try
            {
                AesCryptoServiceProvider aes = new AesCryptoServiceProvider();
                aes.BlockSize = 128;
                aes.KeySize = 128;
                aes.Key = aesKey;
                aes.Mode = CipherMode.ECB;
                aes.Padding = PaddingMode.PKCS7;

                using (ICryptoTransform decrypt = aes.CreateDecryptor())
                {
                    byte[] dest = decrypt.TransformFinalBlock(src, 0, src.Length);
                    return dest;
                }
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// AES加密
        /// </summary>
        /// <param name="data">加密数据</param>
        /// <param name="key">密钥字符串</param>
        /// <returns></returns>
        public static string StrAesEncrypt(string data, string key)
        {
            try
            {
                AesCryptoServiceProvider aes = new AesCryptoServiceProvider();
                aes.KeySize = 128;
                aes.Key = Str2Byte(key);
                aes.Mode = CipherMode.ECB;
                aes.Padding = PaddingMode.PKCS7;

                using (ICryptoTransform encrypt = aes.CreateEncryptor())
                {
                    byte[] src = Str2Byte(data);
                    byte[] dest = encrypt.TransformFinalBlock(src, 0, src.Length);
                    return BytesToHexString(dest);
                }
            }
            catch
            {
                throw;
            }
        }

        //Str2Byte()
        public static byte[] Str2Byte(string input)
        {
            Byte[] byteInput = System.Text.Encoding.UTF8.GetBytes(input); //按照UTF8的编码方式将string类型转成byte[]
            return byteInput;
        }

        //Byte2Str()
        public static string Byte2Str(byte[] byteInput)
        {
            String strInput = System.Text.Encoding.UTF8.GetString(byteInput); //按照UTF8的编码方式从byte[]得到字符串
            return strInput;
        }

        /// <summary>
        /// GZip压缩
        /// </summary>
        /// <param name="rawData"></param>
        /// <returns></returns>
        public static byte[] Compress(string input)
        {
            Byte[] byteInput = Str2Byte(input); //按照UTF8的编码方式将string类型转成byte[]

            using (MemoryStream ms = new MemoryStream())
            {
                GZipOutputStream zipFile = new GZipOutputStream(ms);
                zipFile.Write(byteInput, 0, input.Length);
                zipFile.Close();
                //return ToHexString(ms.ToArray());
                return ms.ToArray();
            }
        }

        /// <summary>
        /// GZip解压缩
        /// </summary>
        /// <param name="rawData"></param>
        /// <returns></returns>
        public static byte[] DeCompress(byte[] input)
        {
            int bufferSize = 2048;
            try
            {
                MemoryStream ms = new MemoryStream(input);
                MemoryStream ms1 = new MemoryStream();
                GZipInputStream zipFile = new GZipInputStream(ms);
                byte[] output = new byte[2048]; //指定缓冲区的大小
                while (bufferSize > 0)
                {
                    bufferSize = zipFile.Read(output, 0, bufferSize);
                    ms1.Write(output, 0, bufferSize);
                }

                ms1.Close();
                return ms1.ToArray();
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// 16进制字符串转16进制字节数组
        /// 1F=16+15=31-->[0] = {byte} 31
        /// </summary>
        /// <param name="hexString"></param>
        /// <returns></returns>
        public static byte[] hexStrToHexByte(string hexString)
        {
            hexString = hexString.Replace(" ", "");
            if ((hexString.Length % 2) != 0)
                hexString += " ";
            byte[] returnBytes = new byte[hexString.Length / 2];
            for (int i = 0; i < returnBytes.Length; i++)
                returnBytes[i] = Convert.ToByte(hexString.Substring(i * 2, 2), 16);
            return returnBytes;
        }

        // byte转16进制字符串
        //[0] = {byte} 31 // 31->1F
        //[1] = {byte} 139 // 139->8B
        //8->08
        public static string BytesToHexString(byte[] bytes)
        {
            string hexString = string.Empty;
            if (bytes != null)
            {
                StringBuilder strB = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    strB.Append(bytes[i].ToString("X2"));
                }

                hexString = strB.ToString();
            }

            return hexString;
        }

        /*
         * byte转16进制字符串
         * BytesToHexString2
         */
        public static string BytesToHexString2(byte[] source)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < source.Length; i++)
            {
                sb.AppendFormat("{0:X2}", source[i]);
            }

            string hex = sb.ToString();
            return hex;
        }
    }
}