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
using System.Diagnostics;
using HapGp.Helper;

namespace HapGp.Controllers
{
    public partial class APIController
    {


        private static class Utils
        {


            public static PostResponseModel _AddRecord(PostInparamModel value)
            {
                try
                {
                    using (var server = FrameCorex.RecoverService(value.Token, (c) => { Debug.WriteLine("Container Token not found Token: " + c); }))
                    {
                        if (!FrameCorex.ServiceInstanceInfo(server).IsLogin)
                        {
                            server.UserLogin(value.LID, value.PWD);
                            FrameCorex.ServiceInstanceInfo(server).DisposeInfo = false;
                        }
                        Userx User = FrameCorex.ServiceInstanceInfo(server).User;
                        foreach (var t in value.Params)
                            if (t.Value != null)
                                User.Records[t.Key] = t.Value;
                        return new PostResponseModel()
                        {
                            Message = "Record add successs",
                            Result = Enums.APIResult.Success,
                            UserLoginToken = FrameCorex.ServiceInstanceInfo(server).LoginHashToken
                        };
                    }

                }
                catch (UserNotfindException ex)
                {
                    return new PostResponseModel()
                    {
                        Message = ex.Message,
                        Result = Enums.APIResult.Error
                    };
                }
                catch (UserLoginPermissionException ex)
                {
                    return new PostResponseModel()
                    {
                        Message = ex.Message,
                        Result = Enums.APIResult.Error
                    };
                }
                catch (UserPwdErrorException ex)
                {
                    return new PostResponseModel()
                    {
                        Message = ex.Message,
                        Result = Enums.APIResult.Error
                    };
                }
                catch (FPException ex)
                {
                    return new PostResponseModel()
                    {
                        Message = ex.Message,
                        Result = Enums.APIResult.Error
                    };
                }


            }

            //
            public static PostResponseModel _Regist(PostInparamModel value)
            {
                try
                {
                    using (ServiceInstance server = FrameCorex.GetService())
                    {
                        if (server.UserRegist_CheckLIDNotExsist(value.LID))
                        {
                            server.UserRegist(value.LID, value.PWD, value.Params.ContainsKey("teacher")?Enums.UserRole.Teacher:Enums.UserRole.Student);
                        }
                        else
                        {
                            return new PostResponseModel()
                            {
                                Message = "User already exsist",
                                Result = Enums.APIResult.Error
                            };
                        }
                    }
                }
                catch (FPException ex)
                {
                    return new PostResponseModel()
                    {
                        Message = ex.Message,
                        Result = Enums.APIResult.Error
                    };
                }
                return new PostResponseModel()
                {
                    Message = "User regist success,welcome ",
                    Result = Enums.APIResult.Success
                };
            }

            public static PostResponseModel _GetRecord(PostInparamModel value)
            {
                try
                {
                    using (ServiceInstance server = FrameCorex.RecoverService(value.Token, (c) => { Debug.WriteLine("Container Token not found Token: " + c); }))
                    {
                        if (!FrameCorex.ServiceInstanceInfo(server).IsLogin)
                        {
                            server.UserLogin(value.LID, value.PWD);
                            FrameCorex.ServiceInstanceInfo(server).DisposeInfo = false;
                        }
                        var user = FrameCorex.ServiceInstanceInfo(server).User;
                        var tarres = new PostResponseModel()
                        {
                            Message = "Excute record query success",
                            Result = Enums.APIResult.Success,
                            UserLoginToken = FrameCorex.ServiceInstanceInfo(server).LoginHashToken,
                            ExtResult = { }
                        };
                        if (value.Params != null)
                            foreach (var t in value.Params.Keys)
                            {
                                tarres.ExtResult.Add(t, user.Records[t]);
                            }
                        return tarres;

                    }
                }
                catch (FPException ex)
                {
                    return new PostResponseModel()
                    {
                        Message = ex.Message,
                        Result = Enums.APIResult.Error
                    };
                }
            }

            public static PostResponseModel _DeleteRecord(PostInparamModel value)
            {
                try
                {
                    using ( ServiceInstance server = FrameCorex.RecoverService(value.Token, (c) => { Debug.WriteLine("Container Token not found Token: " + c); }))
                    {
                        if (!FrameCorex.ServiceInstanceInfo(server).IsLogin)
                        {
                            server.UserLogin(value.LID, value.PWD);
                            FrameCorex.ServiceInstanceInfo(server).DisposeInfo = false;
                        }
                        var user = FrameCorex.ServiceInstanceInfo(server).User;
                        foreach (var t in value.Params.Values)
                        {
                            user.Records.Delete(t);
                        }
                    }
                }
                catch (FPException ex)
                {
                    return new PostResponseModel()
                    {
                        Message = ex.Message,
                        Result = Enums.APIResult.Error
                    };
                }
                return new PostResponseModel()
                {
                    Message = "Delete record success",
                    Result = Enums.APIResult.Success
                };
            }

            //projectname subtitle starttime endtime
            public static PostResponseModel _AddProject(PostInparamModel value)
            {
                try
                {
                    using (ServiceInstance server = FrameCorex.RecoverService(value.Token, (c) => { Debug.WriteLine("Container Token not found Token: " + c); }))
                    {
                        if (!FrameCorex.ServiceInstanceInfo(server).IsLogin)
                        {
                            server.UserLogin(value.LID, value.PWD);
                            FrameCorex.ServiceInstanceInfo(server).DisposeInfo = false;
                        }
                        var user = FrameCorex.ServiceInstanceInfo(server).User;

                        user.SetProject(null,
                            value.Params["projectname"],
                            value.Params.Get("subtitle"),
                            value.Params.Get("starttime") == null ? null : (DateTime?)DateTime.Parse(value.Params.Get("starttime")),
                            value.Params.Get("endtime") == null ? null : (DateTime?)DateTime.Parse(value.Params.Get("endtime")));

                        return new PostResponseModel()
                        {
                            Message = "添加课程成功",
                            Result = Enums.APIResult.Success,
                            UserLoginToken = FrameCorex.ServiceInstanceInfo(server).LoginHashToken
                        };
                    }
                }
                catch (FPException ex)
                {
                    return new PostResponseModel()
                    {
                        Message = ex.Message,
                        Result = Enums.APIResult.Error
                    };
                }

            }

            public static PostResponseModel _ModifyProject(PostInparamModel value)
            {
                try
                {
                    using (ServiceInstance server = FrameCorex.RecoverService(value.Token, (c) => { Debug.WriteLine("Container Token not found Token: " + c); }))
                    {
                        if (!FrameCorex.ServiceInstanceInfo(server).IsLogin)
                        {
                            server.UserLogin(value.LID, value.PWD);
                            FrameCorex.ServiceInstanceInfo(server).DisposeInfo = false;
                        }
                        var user = FrameCorex.ServiceInstanceInfo(server).User;

                        user.SetProject(int.Parse(value.Params["projectid"]), value.Params["projectname"], value.Params["subtitle"], DateTime.Parse(value.Params["starttime"]), DateTime.Parse(value.Params["endtime"]));

                        return new PostResponseModel()
                        {
                            Message = "修改课程信息成功",
                            Result = Enums.APIResult.Success,
                            UserLoginToken = FrameCorex.ServiceInstanceInfo(server).LoginHashToken
                        };
                    }
                }
                catch (FPException ex)
                {
                    return new PostResponseModel()
                    {
                        Message = ex.Message,
                        Result = Enums.APIResult.Error
                    };
                }

            }

            public static PostResponseModel _SelectClass(PostInparamModel value)
            {
                try
                {
                    using (ServiceInstance server = FrameCorex.RecoverService(value.Token, (c) => { Debug.WriteLine("Container Token not found Token: " + c); }))
                    {
                        if (!FrameCorex.ServiceInstanceInfo(server).IsLogin)
                        {
                            server.UserLogin(value.LID, value.PWD);
                            FrameCorex.ServiceInstanceInfo(server).DisposeInfo = false;
                        }
                        var user = FrameCorex.ServiceInstanceInfo(server).User;

                        if (value.Params.ContainsKey("projectid")) user.SelectProject(int.Parse(value.Params["projectid"]));
                        else if (value.Params.ContainsKey("projectname")) user.SelectProject(value.Params["projectname"]);
                        else throw new FPException("未指定选择的课程");
                        return new PostResponseModel()
                        {
                            Message = "添加课程成功",
                            Result = Enums.APIResult.Success,
                            UserLoginToken = FrameCorex.ServiceInstanceInfo(server).LoginHashToken
                        };
                    }
                }   
                catch (FPException ex)
                {
                    return new PostResponseModel()
                    {
                        Message = ex.Message,
                        Result = Enums.APIResult.Error
                    };
                }

            }

            public static PostResponseModel _GetClass(PostInparamModel value)
            {
                try
                {
                    using (ServiceInstance server = FrameCorex.RecoverService(value.Token, (c) => { Debug.WriteLine("Container Token not found Token: " + c); }))
                    {
                        if (!FrameCorex.ServiceInstanceInfo(server).IsLogin)
                        {
                            server.UserLogin(value.LID, value.PWD);
                            FrameCorex.ServiceInstanceInfo(server).DisposeInfo = false;
                        }
                        var user = FrameCorex.ServiceInstanceInfo(server).User;
                        var res = user.QueryClass();
                        return new PostResponseModel()
                        {
                            Message = "获取课程",
                            Result = Enums.APIResult.Success,
                            ExtResult = { { "class", user.QueryClass() } },
                            UserLoginToken = FrameCorex.ServiceInstanceInfo(server).LoginHashToken
                        };
                    }
                }
                catch (FPException ex)
                {
                    return new PostResponseModel()
                    {
                        Message = ex.Message,
                        Result = Enums.APIResult.Error
                    };
                }
            }

            public static PostResponseModel _QueryClass(PostInparamModel value)
            {
                try
                {
                    using (ServiceInstance server = FrameCorex.RecoverService(value.Token, (c) => { Debug.WriteLine("Container Token not found Token: " + c); }))
                    {
                        if (!FrameCorex.ServiceInstanceInfo(server).IsLogin)
                        {
                            server.UserLogin(value.LID, value.PWD);
                            FrameCorex.ServiceInstanceInfo(server).DisposeInfo = false;
                        }
                        var user = FrameCorex.ServiceInstanceInfo(server).User;
                        var tarclass=user.GetProjectInfo(value.Params["projectname"]);
                        return new PostResponseModel()
                        {
                            Message = "获取课程",
                            Result = Enums.APIResult.Success,
                            ExtResult = { { "课程ID", tarclass.Key }, { "课程名称", tarclass.ProjectName }, { "课程描述", tarclass.Subtitle }, { "课程开始时间", tarclass.StartTime.Value.ToShortTimeString() }, { "课程结束时间", tarclass.EndTime.Value.ToShortTimeString() } },
                            UserLoginToken = FrameCorex.ServiceInstanceInfo(server).LoginHashToken
                        };
                    }
                }
                catch (FPException ex)
                {
                    return new PostResponseModel()
                    {
                        Message = ex.Message,
                        Result = Enums.APIResult.Error
                    };
                }
            }


        }
    }
}
