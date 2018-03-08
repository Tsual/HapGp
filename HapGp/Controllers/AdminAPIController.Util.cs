using HapGp.APIModel;
using HapGp.Core;
using HapGp.ModelInstance;
using System;
using System.Collections.Generic;

namespace HapGp.Controllers
{
    public partial class AdminAPIController
    {
        private class Util
        {
            /// <summary>
            /// 
            /// </summary>
            /// <param name="value"></param>
            /// <returns></returns>
            public static PostResponseModel _GetServerState(AdminPostInparamModel value)
            {
                try
                {
                    using (ServiceInstance server = FrameCorex.GetService())
                    {
                        server.UserLogin(value.LID, value.PWD, Enums.Permission.Administor);
                        FrameCorex.ServiceInstanceInfo(server).DisposeInfo = false;
                        var qresult = FrameCorex.UserHelper.LocalServerState(server);
                        if (qresult == null) return new PostResponseModel() { Result = Enums.APIResult.Error };
                        return new PostResponseModel() {
                            Result = Enums.APIResult.Success,
                            ExtResult= qresult,
                            Message="query success"
                        } ;
                    }
                }
                catch (Exceptions.FPException ex)
                {
                    return new PostResponseModel()
                    {
                        Message = ex.Message,
                        Result = Enums.APIResult.Error
                    };
                }
            }


            /// <summary>
            /// 
            /// </summary>
            /// <param name="value"></param>
            /// <returns></returns>
            public static PostResponseModel _GetAllUserState(AdminPostInparamModel value)
            {
                try
                {
                    using (ServiceInstance server = FrameCorex.GetService())
                    {
                        server.UserLogin(value.LID, value.PWD, Enums.Permission.Administor);
                        FrameCorex.ServiceInstanceInfo(server).DisposeInfo = false;
                        var result = new PostResponseModel();
                        result.ExtResult.Add("Current users", Dealdct(FrameCorex.CurrentUsers(server)));
                        result.ExtResult.Add("Interrupt users", Dealdct(FrameCorex.InterruptUsers(server)));
                        return result;
                    }
                }
                catch (Exceptions.FPException ex)
                {
                    return new PostResponseModel()
                    {
                        Message = ex.Message,
                        Result = Enums.APIResult.Error
                    };
                }
            }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="list"></param>
            /// <returns></returns>
            private static List<Dictionary<string, string>> Dealdct(List<ServiceInstanceInfo> list) => list.ConvertAll((t) =>{
                     return new Dictionary<string, string>()
                     {
                        { "LID", t.User.Origin.LID },
                        { "IsLogin", t.IsLogin.ToString() },
                        { "DuoTime", t.DuoTime.ToString() },
                        { "DurTime", t.DurTime.ToString() }
                     };
            });

        }
    }
}