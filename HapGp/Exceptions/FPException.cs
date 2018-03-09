using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HapGp.Exceptions
{
    public class FPException:Exception
    {
        public FPException()
        {
            
        }

        bool msgoverride = false;
        string msg = "";

        public FPException(string msg)
        {
            msgoverride = true;
            this.msg = msg;
        }

        public override string Message => msgoverride?msg: base.Message;
    }
}
