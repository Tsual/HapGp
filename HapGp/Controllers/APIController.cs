using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using HapGp.APIModel;
using HapGp.Core;
using HapGp.Exceptions;
using HapGp.ModelInstance;
using System.Threading;

namespace HapGp.Controllers
{
    [Produces("application/json")]
    [Route("api/API")]
    public partial class APIController : Controller
    {


        [HttpGet]
        public PostResponseModel Get()
        {
            string token = "";
            using (var server = FrameCorex.GetService())
            {
                server.UserLogin("test1", "test1");
                token = server.Info.ToString();
                server.Info.EncryptToken = "test";
                server.Info.DisposeInfo = false;
            }
            using (var server = FrameCorex.RecoverService(token, (c) => { }))
            {
                server.Info.EncryptToken = "check";
            }
            return null;
        }

        // GET: api/API/5
        //[HttpGet("{id}", Name = "Get")]
        //public string Get(int id)
        //{
        //    return "value";
        //}

        // POST: api/API
        [HttpPost]
        public PostResponseModel Post([FromBody]PostInparamModel value)
        {
            if (value == null) return null;
            if (!value.InparamCheck())
                return new PostResponseModel()
                {
                    Message = "Missing value",
                    Result = Enums.APIResult.Error
                };
            try
            {
                return typeof(Utils).GetMethod("_" + value.Operation.ToString())?
                    .Invoke(null, new object[] { value }) as PostResponseModel;
            }
            catch (FPException ex)
            {
                return new PostResponseModel()
                {
                    Message = ex.Message,
                    Result = Enums.APIResult.Error
                };
            }
            catch (Exception)
            {
                return new PostResponseModel()
                {
                    Message = "Operation not support, APIOperation Enum:" + String.Join(",", Enum.GetNames(typeof(Enums.APIOperation))),
                    Result = Enums.APIResult.Error
                };
            }
        }



        // PUT: api/API/5
        //[HttpPut("{id}")]
        //public void Put(int id, [FromBody]string value)
        //{
        //}

        //// DELETE: api/ApiWithActions/5
        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //}
    }
}
