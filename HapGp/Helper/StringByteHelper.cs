using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HapGp.Helper
{
    public class StringByteHelper
    {
        public static string GetStringFromBytes(byte[] ba)=>
                 Convert.ToBase64String(ba, Base64FormattingOptions.InsertLineBreaks);

        
        //StringBuilder sb = new StringBuilder();
        //for (int i = 0; i < ba.Length; i++)
        //{
        //    sb.Append(Convert.ToChar(ba[i]));
        //}
        //return sb.ToString();

        public static byte[] GetBytesFromString(string str)
        {
            try
            {
                return Convert.FromBase64String(str);
            }
            catch (Exception)
            {
                List<byte> lb = new List<byte>();
                for (int i = 0; i < str.Length; i++)
                {
                    lb.Add(Convert.ToByte(str[i]));
                }
                return Convert.FromBase64String(Convert.ToBase64String(lb.ToArray(), Base64FormattingOptions.InsertLineBreaks));
            }

        }
        //List<byte> lb = new List<byte>();
        //for (int i = 0; i < str.Length; i++)
        //{
        //    lb.Add(Convert.ToByte(str[i]));
        //}
        //return lb.ToArray();

    }

}
