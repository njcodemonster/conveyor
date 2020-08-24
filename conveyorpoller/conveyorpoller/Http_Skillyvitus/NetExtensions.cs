namespace conveyorpoller.Http_Skillyvitus
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using System.Text;

    public static class NetExtensions
    {
        public static bool IsAlphaNum(this string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return false;
            }
            return str.ToCharArray().All<char>(delegate (char c) {
                if (!char.IsLetter(c))
                {
                    return char.IsNumber(c);
                }
                return true;
            });
        }

        public static byte[] ReadToNull(this Stream s)
        {
            int num;
            List<byte> list = new List<byte>();
            while ((num = s.ReadByte()) != -1)
            {
                byte item = (byte)num;
                if (item != 0)
                {
                    list.Add(item);
                }
                else
                {
                    return list.ToArray();
                }
            }
            return list.ToArray();
        }

        public static string ToHex(this byte[] ext)
        {
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < ext.Length; i += 0x10)
            {
                for (int j = 0; j < 0x10; j++)
                {
                    if ((j + i) < ext.Length)
                    {
                        builder.Append(ext[j + i].ToString("X2")).Append(" ");
                    }
                }
                builder.Append('\n');
            }
            return builder.ToString();
        }

        public static string ToHex(this byte[] ext, int length)
        {
            int num = ext.Length;
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < (length - 1); i++)
            {
                if (i > num)
                {
                    builder.Append("00 ");
                }
                else
                {
                    builder.Append(ext[i].ToString("X2")).Append(" ");
                }
            }
            builder.Append('\n');
            return builder.ToString();
        }

        public static string Truncate(this string source, int length)
        {
            if (source.Length > length)
            {
                source = source.Substring(0, length);
            }
            return source;
        }
    }
}

