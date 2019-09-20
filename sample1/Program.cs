using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
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

            //DES加密
            Console.WriteLine(Encrypt("admin"));
            //DES解密
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
            // string strTemp = "1F8B0800D9BB835D00FF3334320600D263488803000000";
            // byte[] bytesTemp3 = System.Text.Encoding.ASCII.GetBytes(strTemp);
            byte[] bytesTemp2 = DeCompress(bytesTemp);
            Console.WriteLine("输出解压的结果是:"); //输出解压的结果:
            Console.WriteLine(System.Text.Encoding.UTF8.GetString(bytesTemp2));

            //用GZip进行压缩解压缩和用AES加密解密 version2 2019-9-20 19:35:10
            Console.WriteLine("---------------------压缩解压缩和加密解密的工具---------------------" + "\n");
            //在这里定义要压缩的字符
            string strNeedToCompress2 = "123";
            Thread.Sleep(1000);
            Console.WriteLine("此要要压缩的字符为：" + strNeedToCompress2);

            byte[] bytesTemp22 = utils.utils.Compress(strNeedToCompress2);
            string bytesToHexStringTemp = utils.utils.BytesToHexString(bytesTemp22); //压缩成功的数组转成16进制字符串
            Console.WriteLine("压缩成功的数组转成16进制字符串为:" + bytesToHexStringTemp); //1F8B0800D9BB835D00FF3334320600D263488803000000

            //将压缩成功的数组转成的16进制字符串转成byte数组
            byte[] bytesFromHexStr = utils.utils.hexStrToHexByte(bytesToHexStringTemp);

            byte[] bytesTemp23 = utils.utils.DeCompress(bytesFromHexStr);
            Console.WriteLine("输出解压缩的结果是:"); //输出解压的结果:
            Console.WriteLine(utils.utils.Byte2Str(bytesTemp23)); //按照UTF8的编码方式从byte[]得到字符串

            //加密解密相关
            //首先加密 123
            string strAesTemp = utils.utils.StrAesEncrypt("0141190613225348414e474841495251", "0141190613225348414e474841495251");
            Console.WriteLine("加密成功的16进制字符串:" + strAesTemp); //8A48B3C11CE0FDE42EDB21E9B5423493A0EFE774929EA55518E8C6F1E4D667E7434B9A4C10BD9E970E04BB447AC51E83

            //解密 16进制字符串
            //首先 将16进制字符串转成byte[]
            byte[] bytesFromHexStrAes = utils.utils.hexStrToHexByte(strAesTemp);
            byte[] byteKeyAes = utils.utils.Str2Byte("0141190613225348414e474841495251");
            byte[] bytesOk = utils.utils.AesDecrypt(bytesFromHexStrAes, byteKeyAes);
            string strOk = utils.utils.Byte2Str(bytesOk);
            Console.WriteLine("解密的字符串为:" + strOk);

            Console.WriteLine("------------------------------------------" + "\n");
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
        /// DES加密
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
        /// DES解密
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