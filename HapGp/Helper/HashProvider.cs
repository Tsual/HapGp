using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace HapGp.Helper
{
    public class HashProvider
    {
        HashAlgorithm _Hash = null;

        public enum HashAlgorithms { MD5, SHA256, SHA512 ,SHA128}

        public HashProvider()
        {
            _Hash = SHA256.Create();
        }
        public HashProvider(HashAlgorithms HashAlgorithm)
        {
            switch (HashAlgorithm)
            {
                case HashAlgorithms.MD5:
                    _Hash = MD5.Create();
                    break;
                case HashAlgorithms.SHA512:
                    _Hash = SHA512.Create();
                    break;
                case HashAlgorithms.SHA256:
                    _Hash = SHA256.Create();
                    break;
            }

        }

        public string Hash(string str)
        {
            return StringByteHelper.GetStringFromBytes(_Hash.ComputeHash(Encoding.UTF8.GetBytes(str)));
        }

        public byte[] Hashbytes(string str)
        {
            return _Hash.ComputeHash(StringByteHelper.GetBytesFromString(str));
        }

        
    }
}
