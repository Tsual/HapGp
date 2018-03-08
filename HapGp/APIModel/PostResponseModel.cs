using HapGp.Enums;
using System.Collections.Generic;
using System.Runtime.Serialization;


namespace HapGp.APIModel
{
    public class PostResponseModel
    {
        public string ExcuteResult => Result.ToString();
        public string Message { get; set; }
        public string UserLoginToken { get; set; }
        public Dictionary<string, object> ExtResult { get => _ExtResult; set => _ExtResult = value; }

        [IgnoreDataMember]
        private Dictionary<string, object> _ExtResult = new Dictionary<string, object>();
        [IgnoreDataMember]
        public APIResult Result { get; set; }
    }

}
