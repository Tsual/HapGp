using System;
using System.Collections.Generic;
using System.Linq;
using HapGp.Core;
using System.Threading.Tasks;

namespace HapGp.Exceptions
{
    public class ServiceException: FPException
    {
        private ServiceInstance _Instance = null;

        public ServiceInstance Instance { get => _Instance; set => _Instance = value; }
    }


    public class ServiceNotfindException: ServiceException
    {

    }

    public class ServiceTimeoutException : ServiceException
    {

    }
}
