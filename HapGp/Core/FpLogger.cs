using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HapGp.Core
{
    public class FpLogger:ILogger
    {
        private static readonly FpLogger currentInstance = new FpLogger();
        public static ILogger Current { get => currentInstance; }
        

        private ILogger innerLogger = null;
        public ILogger InnerLogger { set => innerLogger = value; }








        public IDisposable BeginScope<TState>(TState state)
        {
            return innerLogger?.BeginScope(state);
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            var res = innerLogger?.IsEnabled(logLevel);
            return res.HasValue ? false : res.Value;
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            innerLogger?.Log(logLevel, eventId, state, exception, formatter);
        }




    }
}
