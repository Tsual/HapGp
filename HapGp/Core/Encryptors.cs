using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HapGp.Enums;
using HapGp.Helper;
using HapGp.Interfaces;

namespace HapGp.Core
{
    internal class AppEncryptor : IEncryptor
    {
        private AppEncryptor()
        {

        }

        private static AppEncryptor _Current = new AppEncryptor();
        internal static AppEncryptor Current { get => _Current; }


        private static byte[] _appiv;
        private static byte[] _appkey;

        static byte[] decodeappiv(string ivstr)
        {
            var slist = ivstr.Split('|');
            if (slist.Length != 16) throw new Exception("iv dec error");
            byte[] res = new byte[16];
            for (int i = 0; i < 16; i++)
            {
                res[i] = Convert.ToByte(slist[i]);
            }
            return res;

        }
        static string encodeappiv(byte[] iv)
        {
            if (iv == null || iv.Length != 16) throw new Exception("iv error");
            string res = "" + iv[0];
            for (int i = 1; i < 16; i++)
            {
                res += "|" + iv[i];
            }
            return res;
        }
        static byte[] createappiv()
        {
            _appiv = new byte[16];
            new Random().NextBytes(_appiv);
            return _appiv;
        }

        static byte[] decodeappkey(string keystr)
        {
            var slist = keystr.Split('|');
            if (slist.Length != 32) throw new Exception("iv dec error");
            byte[] res = new byte[32];
            for (int i = 0; i < 32; i++)
            {
                res[i] = Convert.ToByte(slist[i]);
            }
            return res;
        }
        static string encodeappkey(byte[] key)
        {
            if (key == null || key.Length != 32) throw new Exception("key error");
            string res = "" + key[0];
            for (int i = 1; i < 32; i++)
            {
                res += "|" + key[i];
            }
            return res;
        }
        static byte[] createappkey()
        {
            _appkey = new byte[32];
            new Random().NextBytes(_appkey);
            return _appkey;
        }

        static AppEncryptor()
        {
            if(FrameCorex.Config[AppConfigEnum.FirstInstallFlag]=="")
            {
                FrameCorex.Config[AppConfigEnum.FirstInstallFlag] = "1";
                FrameCorex.Config[AppConfigEnum.AppAesIV] = encodeappiv(createappiv());
                FrameCorex.Config[AppConfigEnum.AppAesKey] = encodeappkey(createappkey());
            }

            try
            {
                _appiv = decodeappiv(FrameCorex.Config[AppConfigEnum.AppAesIV]);
                _appkey = decodeappkey(FrameCorex.Config[AppConfigEnum.AppAesKey]);
            }
            catch (Exception)
            {
                FrameCorex.Config[AppConfigEnum.AppAesIV] = encodeappiv(createappiv());
                FrameCorex.Config[AppConfigEnum.AppAesKey] = encodeappkey(createappkey());
                _appiv = decodeappiv(FrameCorex.Config[AppConfigEnum.AppAesIV]);
                _appkey = decodeappkey(FrameCorex.Config[AppConfigEnum.AppAesKey]);
            }

        }

        public string Encrypt(string metaStr)
        {
            var aesobj = new AESProvider(_appiv, _appkey);
            return aesobj.Encrypt(metaStr);
        }

        public string Decrypt(string metaStr)
        {
            var aesobj = new AESProvider(_appiv, _appkey);
            return aesobj.Decrypt(metaStr);
        }

    }


}
