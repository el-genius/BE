using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;


namespace URCP.Web
{
    public class Encryption
    {
        private TripleDESCryptoServiceProvider t_des;
        private byte[] m_key;
        private byte[] m_iv;
        private UTF8Encoding m_utf8;
        private DESCryptoServiceProvider s_des;
        private string p;

        private const string KEY = "75A83D1962F9BDEB2282058F68EE4D2DBB73DB2EA560A455";


        public Encryption(string key = KEY)
        {
            this.t_des = new TripleDESCryptoServiceProvider();
            this.s_des = new DESCryptoServiceProvider();
            this.m_utf8 = new UTF8Encoding();
            this.s_des.Mode = CipherMode.ECB;
            this.t_des.Mode = CipherMode.ECB;
            this.m_key = this.HexStringToBytes(key);
            this.m_iv = this.m_utf8.GetBytes("");
        }

        private byte[] HexStringToBytes(string input)
        {
            byte[] ret = new byte[0];
            int i = 0;
            for (int x = 0; i < input.Length; x++)
            {
                ret = (byte[])CopyArray((Array)ret, new byte[x + 1]);
                ret[x] = byte.Parse(input.Substring(i, 2), System.Globalization.NumberStyles.HexNumber);
                i += 2;
            }
            return ret;
        }

        private byte[] Transform(byte[] input, ICryptoTransform CryptoTransform)
        {
            MemoryStream memStream = new MemoryStream();
            CryptoStream cryptStream = new CryptoStream(memStream, CryptoTransform, CryptoStreamMode.Write);
            cryptStream.Write(input, 0, input.Length);
            cryptStream.FlushFinalBlock();
            memStream.Position = 0L;
            byte[] result = new byte[((int)(memStream.Length - 1L)) + 1];
            memStream.Read(result, 0, result.Length);
            memStream.Close();
            cryptStream.Close();
            return result;
        }


        public string TDecrypt(string text)
        {
            byte[] input = (Convert.FromBase64String(Base64ForUrlDecode(text)));
            byte[] output = this.Transform(input, this.t_des.CreateDecryptor(this.m_key, this.m_iv));
            return this.m_utf8.GetString(output);
        }



        public string TEncrypt(string text)
        {
            byte[] bytes = this.m_utf8.GetBytes(text);
            return Base64ForUrlEncode(Convert.ToBase64String(this.Transform(bytes, this.t_des.CreateEncryptor(this.m_key, this.m_iv))));
        }

        public static Array CopyArray(Array arySrc, Array aryDest)
        {
            if (arySrc != null)
            {
                int length = arySrc.Length;
                if (length == 0)
                {
                    return aryDest;
                }
                if (aryDest.Rank != arySrc.Rank)
                {
                    // throw ExceptionUtils.VbMakeException(new InvalidCastException(GetResourceString("Array_RankMismatch")), 9);
                }
                int num8 = aryDest.Rank - 2;
                for (int i = 0; i <= num8; i++)
                {
                    if (aryDest.GetUpperBound(i) != arySrc.GetUpperBound(i))
                    {
                        //throw ExceptionUtils.VbMakeException(new ArrayTypeMismatchException(GetResourceString("Array_TypeMismatch")), 9);
                    }
                }
                if (length > aryDest.Length)
                {
                    length = aryDest.Length;
                }
                if (arySrc.Rank > 1)
                {
                    int rank = arySrc.Rank;
                    int num7 = arySrc.GetLength(rank - 1);
                    int num6 = aryDest.GetLength(rank - 1);
                    if (num6 != 0)
                    {
                        int num5 = Math.Min(num7, num6);
                        int num9 = (arySrc.Length / num7) - 1;
                        for (int j = 0; j <= num9; j++)
                        {
                            Array.Copy(arySrc, j * num7, aryDest, j * num6, num5);
                        }
                    }
                    return aryDest;
                }
                Array.Copy(arySrc, aryDest, length);
            }
            return aryDest;
        }

        private string Base64ForUrlEncode(string str)
        {
            byte[] encbuff = Encoding.UTF8.GetBytes(str);
            return System.Web.HttpServerUtility.UrlTokenEncode(encbuff);
        }

        private static string Base64ForUrlDecode(string str)
        {
            byte[] decbuff = System.Web.HttpServerUtility.UrlTokenDecode(str);
            return Encoding.UTF8.GetString(decbuff);
        }
    }
}
