using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace HapGp.ModelInstance
{
    public class ServiceInstanceInfo
    {
        public ServiceInstanceInfo()
        {
            _DuoTime = DateTime.Now;
        }

        [XmlIgnore]
        private DateTime _DuoTime ;

        [XmlIgnore]
        private bool _IsLogin = false;

        [XmlIgnore]
        private Userx _User = null;

        [XmlIgnore]
        private string _EncryptToken = null;

        [XmlIgnore]
        private bool _DisposeInfo = true;

        [XmlIgnore]
        private string _HashToken = "";

        public bool IsLogin { get => _IsLogin; set => _IsLogin = value; }
        public Userx User { get => _User; set => _User = value; }
        public DateTime DuoTime { get => _DuoTime; set => _DuoTime = value; }
        public TimeSpan DurTime { get => DateTime.Now - _DuoTime; }
        public string EncryptToken { get => _EncryptToken; set => _EncryptToken = value; }
        public bool DisposeInfo { get => _DisposeInfo; set => _DisposeInfo = value; }
        public string LoginHashToken { get => _HashToken; set => _HashToken = value; }

        public override string ToString()
        {
            return "User: "+User?.Origin?.LID+ ",HashToken: "+ LoginHashToken?.ToString()+ ",DisposeInfo: "+ DisposeInfo;
        }
    }
}
