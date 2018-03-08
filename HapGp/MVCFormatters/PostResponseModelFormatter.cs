using HapGp.APIModel;
using Microsoft.AspNetCore.Mvc.Formatters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HapGp.MVCFormatters
{
    public class PostResponseModelOutFormatter : IOutputFormatter
    {
        public bool CanWriteResult(OutputFormatterCanWriteContext context) => context.ObjectType.Equals(typeof(PostResponseModel));

        public Task WriteAsync(OutputFormatterWriteContext context)
        {










            
            return Task.CompletedTask;
        }
    }

    public class PostResponseModelInFormatter : IInputFormatter
    {
        public bool CanRead(InputFormatterContext context)
        {
            throw new NotImplementedException();
        }

        public Task<InputFormatterResult> ReadAsync(InputFormatterContext context)
        {
            throw new NotImplementedException();
        }
    }
}
