using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using HapGp.APIModel;
using HapGp.Enums;
using HapGp.Exceptions;

namespace HapGp.Controllers
{
    [Produces("application/json")]
    [Route("api/AdminAPI")]
    public partial class AdminAPIController : Controller
    {
        [HttpPost]
        public PostResponseModel Post([FromBody]AdminPostInparamModel value)
        {
            if (value == null) return null;
            if (!value.InparamCheck())
                return new PostResponseModel()
                {
                    Message = "missing value" ,
                    Result = APIResult.Error
                };

            try
            {
                return typeof(Util).GetMethod("_" + value.Operation.ToString())?
                    .Invoke(null, new object[] { value }) as PostResponseModel;
            }
            catch (FPException ex)
            {
                return new PostResponseModel()
                {
                    Message = ex.Message,
                    Result = APIResult.Error
                };
            }
            catch (Exception)
            {
                return new PostResponseModel()
                {
                    Message = "Operation not support, APIOperation Enum:" + String.Join(",", Enum.GetNames(typeof(AdminAPIOperation))),
                    Result = APIResult.Error
                };
            }
        }
    }
}