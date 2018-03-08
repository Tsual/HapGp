using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.Diagnostics;

namespace HapGp.Helper
{
    public class AESProvider
    {
        Aes _aes = null;
        public AESProvider(byte[] iv, byte[] key)
        {
            if (iv.Length == 16 && key.Length == 32)
            {
                _aes = Aes.Create();
                _aes.IV = iv;
                _aes.Key = key;
            }
        }

        public string Encrypt(string metaStr)
        {
            byte[] metaStr_byte = StringByteHelper.GetBytesFromString(metaStr);
            int metaStr_byte_count = metaStr_byte.Count();
            int metaStr_byte_f_count = (metaStr_byte_count / 16 + 1) * 16;
            byte[] metaStr_byte_f = new byte[metaStr_byte_f_count];
            for (int i = 0; i < metaStr_byte_f_count; i++)
            {
                if (i < metaStr_byte_count) metaStr_byte_f[i] = metaStr_byte[i];
                else metaStr_byte_f[i] = 0;
            }
            return StringByteHelper.GetStringFromBytes(_aes.CreateEncryptor().TransformFinalBlock(metaStr_byte_f, 0, metaStr_byte_f.Length));
        }
        public string Decrypt(string metaStr)
        {
            var metaStr_b = StringByteHelper.GetBytesFromString(metaStr);
            var target_b = _aes.CreateDecryptor().TransformFinalBlock(metaStr_b, 0, metaStr_b.Length);
            int bcount = target_b.Length;
            while (target_b[--bcount] == 0) { }
            metaStr_b = new byte[bcount + 1];
            Array.Copy(target_b, metaStr_b, bcount + 1);
            return StringByteHelper.GetStringFromBytes(metaStr_b);
        }


        public string Encrypt(string metaStr,byte[] iv)
        {
            if (iv.Length != 16) return Encrypt(metaStr);
            byte[] orib = _aes.IV;
            _aes.IV = iv;
            byte[] metaStr_byte = StringByteHelper.GetBytesFromString(metaStr);
            int metaStr_byte_count = metaStr_byte.Count();
            int metaStr_byte_f_count = (metaStr_byte_count / 16 + 1) * 16;
            byte[] metaStr_byte_f = new byte[metaStr_byte_f_count];
            for (int i = 0; i < metaStr_byte_f_count; i++)
            {
                if (i < metaStr_byte_count) metaStr_byte_f[i] = metaStr_byte[i];
                else metaStr_byte_f[i] = 0;
            }
            var res = StringByteHelper.GetStringFromBytes(_aes.CreateEncryptor().TransformFinalBlock(metaStr_byte_f, 0, metaStr_byte_f.Length));
            _aes.IV = orib;
            return res;
        }
        public string Decrypt(string metaStr, byte[] iv)
        {
            if (iv.Length != 16) return Decrypt(metaStr);
            byte[] orib = _aes.IV;
            _aes.IV = iv;
            var metaStr_b = StringByteHelper.GetBytesFromString(metaStr);
            var target_b = _aes.CreateDecryptor().TransformFinalBlock(metaStr_b, 0, metaStr_b.Length);
            int bcount = target_b.Length;
            while (target_b[--bcount] == 0) { }
            metaStr_b = new byte[bcount + 1];
            Array.Copy(target_b, metaStr_b, bcount + 1);
            _aes.IV = orib;
            return StringByteHelper.GetStringFromBytes(metaStr_b);
        }


    }
}
