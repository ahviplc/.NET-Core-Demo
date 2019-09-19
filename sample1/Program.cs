using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using ICSharpCode.SharpZipLib.GZip;
using sample1.pojo;

namespace sample1
{
    class Program
    {
        public static string md4j = "admin";

        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            Console.Beep(1000, 1000);
            Console.WriteLine(1 + 1);
            MyClass c1 = new MyClass();
            Console.WriteLine($"Hello World! {c1.ReturnMessage()}");
            Console.WriteLine(2 + 2);

            //加密
            Console.WriteLine(Encrypt("admin"));
            //解密
            Console.WriteLine(Decrypt(md4j));
            // Encrypt("admin");
            // Decrypt(md4j);

            //md5
            Console.WriteLine(Md5Hash("admin"));

            //用GZip进行压缩和解压缩
            //在这里定义要压缩的字符
            string strNeedToCompress = "123456789";
            Console.WriteLine("此要要压缩的字符为：" + strNeedToCompress);
            byte[] bytesTemp = Compress(strNeedToCompress);
            //             string strTemp = "1F8B0800D9BB835D00FF3334320600D263488803000000";
            //             byte[] bytesTemp3 = System.Text.Encoding.ASCII.GetBytes(strTemp);
            byte[] bytesTemp2 = DeCompress(bytesTemp);
            Console.WriteLine("输出解压的结果是:"); //输出解压的结果:
            Console.WriteLine(System.Text.Encoding.UTF8.GetString(bytesTemp2));
        }


        /// <summary>
        /// GZip压缩
        /// </summary>
        /// <param name="rawData"></param>
        /// <returns></returns>
        public static byte[] Compress(string input)
        {
            Byte[] byteInput = System.Text.Encoding.Default.GetBytes(input); //string类型转成byte[]：

            //            MemoryStream ms = new MemoryStream();
            //            GZipStream compressedzipStream = new GZipStream(ms, CompressionMode.Compress, true);
            //            compressedzipStream.Write(byteInput, 0, input.Length);
            //            compressedzipStream.Close();
            //            Console.WriteLine(ToHexString(ms.ToArray()));
            //            return ToHexString(ms.ToArray());//123 ---> 1F8B08000000000004003334320600D263488803000000

            using (MemoryStream ms = new MemoryStream())
            {
                GZipOutputStream zipFile = new GZipOutputStream(ms);
                zipFile.Write(byteInput, 0, input.Length);
                zipFile.Close();
                Console.WriteLine("压缩成功的数组转成16进制字符串为:" + ToHexString(ms.ToArray())); //1F8B0800D9BB835D00FF3334320600D263488803000000
                // return ToHexString(ms.ToArray());
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

                //Console.WriteLine(ms1.ToArray()[0]); 49
                //Console.WriteLine(BitConverter.ToString(ms1.ToArray())); //31-32-33
                //Console.WriteLine(System.Text.Encoding.UTF8.GetString(ms1.ToArray()));

                ms1.Close();
                return ms1.ToArray();
            }
            catch
            {
                throw;
            }
        }

        // byte[]转16进制格式string：

        //new byte[]{ 0x30, 0x31}转成"3031":

        public static string ToHexString(byte[] bytes) // 0xae00cf => "AE00CF "
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


        /// <summary>
        /// 加密
        /// </summary>
        /// <param name="Text">要加密的文本</param>
        /// <param name="sKey">秘钥</param>
        /// <returns></returns>
        public static string Encrypt(string Text, string sKey = "test")
        {
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();
            byte[] inputByteArray;
            inputByteArray = Encoding.Default.GetBytes(Text);
            des.Key = ASCIIEncoding.ASCII.GetBytes(Md5Hash(sKey).Substring(0, 8));
            des.IV = ASCIIEncoding.ASCII.GetBytes(Md5Hash(sKey).Substring(0, 8));
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(), CryptoStreamMode.Write);
            cs.Write(inputByteArray, 0, inputByteArray.Length);
            cs.FlushFinalBlock();
            StringBuilder ret = new StringBuilder();
            foreach (byte b in ms.ToArray())
            {
                ret.AppendFormat("{0:X2}", b);
            }

            md4j = ret.ToString();
            return ret.ToString();
        }

        /// <summary>
        /// 解密
        /// </summary>
        /// <param name="Text"></param>
        /// <param name="sKey"></param>
        /// <returns></returns>
        public static string Decrypt(string Text, string sKey = "test")
        {
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();
            int len;
            len = Text.Length / 2;
            byte[] inputByteArray = new byte[len];
            int x, i;
            for (x = 0; x < len; x++)
            {
                i = Convert.ToInt32(Text.Substring(x * 2, 2), 16);
                inputByteArray[x] = (byte) i;
            }

            des.Key = ASCIIEncoding.ASCII.GetBytes(Md5Hash(sKey).Substring(0, 8));
            des.IV = ASCIIEncoding.ASCII.GetBytes(Md5Hash(sKey).Substring(0, 8));
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(), CryptoStreamMode.Write);
            cs.Write(inputByteArray, 0, inputByteArray.Length);
            cs.FlushFinalBlock();
            return Encoding.Default.GetString(ms.ToArray());
        }

        /// <summary>
        /// 32位MD5加密
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        private static string Md5Hash(string input)
        {
            MD5CryptoServiceProvider md5Hasher = new MD5CryptoServiceProvider();
            byte[] data = md5Hasher.ComputeHash(Encoding.Default.GetBytes(input));
            StringBuilder sBuilder = new StringBuilder();
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }

            return sBuilder.ToString();
        }
    }
}