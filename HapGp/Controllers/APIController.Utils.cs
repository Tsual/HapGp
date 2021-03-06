﻿using System;
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
using HapGp.BussinessProcessing;

namespace HapGp.Controllers
{
    public partial class APIController
    {


        private static class Utils
        {
            #region 登陆注册
            public static PostResponseModel _Login(PostInparamModel value)
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

                        return new PostResponseModel()
                        {
                            Message = "成功",
                            Result = Enums.APIResult.Success,
                            ExtResult = { { "Role", user.Infos.Role.ToString() }, { "Name", user.Infos.Name } },
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
            public static PostResponseModel _Regist(PostInparamModel value)
            {
                try
                {
                    using (ServiceInstance server = FrameCorex.GetService())
                    {
                        if (server.UserRegist_CheckLIDNotExsist(value.LID))
                        {
                            server.UserRegist(value.LID, value.PWD, value.Params.ContainsKey("teacher") ? Enums.UserRole.Teacher : Enums.UserRole.Student, value.Params.Get("name"));
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
            #endregion

            #region 原生功能
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
                    using (ServiceInstance server = FrameCorex.RecoverService(value.Token, (c) => { Debug.WriteLine("Container Token not found Token: " + c); }))
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
            #endregion

            #region 课程
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

                        try
                        {
                            user.SetProject(null,
                                value.Params["projectname"],
                                value.Params.Get("subtitle"),
                                value.Params.Get("starttime") == null ? null :
                                    (DateTime?)DateTime.Parse(value.Params.Get("starttime")),
                                value.Params.Get("endtime") == null ? null :
                                    (DateTime?)DateTime.Parse(value.Params.Get("endtime")),
                                (DayOfWeek)Enum.Parse(typeof(DayOfWeek), value.Params.Get("dayofweek")),
                                DataUtils.FromStringToDouble(value.Params.Get("west")),
                                DataUtils.FromStringToDouble(value.Params.Get("south")),
                                DataUtils.FromStringToDouble(value.Params.Get("north")),
                                DataUtils.FromStringToDouble(value.Params.Get("east")));
                        }
                        catch (FPException e)
                        {
                            throw e;
                        }
                        catch (Exception e)
                        {
                            return new PostResponseModel()
                            {
                                Result = Enums.APIResult.Error,
                                Message = "课程添加失败"
                            };
                        }


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

                        user.SetProject(int.Parse(value.Params["projectid"]),
                            value.Params["projectname"],
                            value.Params["subtitle"],
                            DateTime.Parse(value.Params["starttime"]),
                            DateTime.Parse(value.Params["endtime"]),
                            (DayOfWeek)Enum.Parse(typeof(DayOfWeek), value.Params.Get("dayofweek")),
                            DataUtils.FromStringToDouble(value.Params.Get("west")),
                            DataUtils.FromStringToDouble(value.Params.Get("south")),
                            DataUtils.FromStringToDouble(value.Params.Get("north")),
                            DataUtils.FromStringToDouble(value.Params.Get("east")));

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
                        return new PostResponseModel()
                        {
                            Message = "获取课程",
                            Result = Enums.APIResult.Success,
                            ExtResult = {
                                { "class", user.QueryClass().ConvertClassInstance() }
                            },
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
                        var tarclass = user.GetProjectInfo(value.Params["projectname"]);
                        if (tarclass == null) throw new FPException("获取课程不存在");
                        return new PostResponseModel()
                        {
                            Message = "获取课程",
                            Result = Enums.APIResult.Success,
                            ExtResult = {
                                { "课程ID", tarclass.Key },
                                { "课程日程", tarclass.DayofWeek.ToString() },
                                { "课程名称", tarclass.ProjectName },
                                { "课程描述", tarclass.Subtitle },
                                { "课程开始时间", tarclass.StartTime.Value.ToShortTimeString() },
                                { "课程结束时间", tarclass.EndTime.Value.ToShortTimeString() }
                            },
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
            public static PostResponseModel _TeacherQueryClass(PostInparamModel value)
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
                        return new PostResponseModel()
                        {
                            Message = "获取课程",
                            Result = Enums.APIResult.Success,
                            ExtResult = {
                                { "class", user.QueryClassTeacher().ConvertProject() }
                            },
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
            #endregion






            #region 签到
            public static PostResponseModel _SignIn(PostInparamModel value)
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
                        user.SignIn(DataUtils.FromStringToDouble(value.Params.Get("x")), DataUtils.FromStringToDouble(value.Params.Get("y")),user.Origin.ID);
                        return new PostResponseModel()
                        {
                            Message = "签到成功",
                            Result = Enums.APIResult.Success,
                            UserLoginToken = FrameCorex.ServiceInstanceInfo(server).LoginHashToken
                        };
                    }
                }
                catch (ClassLocationMissing ex)
                {
                    return new PostResponseModel()
                    {
                        Message = "签到成功",
                        Result = Enums.APIResult.Warning,
                        ExtResult = { { "警告信息", ex.Message } }
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
            public static PostResponseModel _QuerySignIn(PostInparamModel value)
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

                        int projectid = value.Params.Get("projectid") == null ? 0 : int.Parse(value.Params.Get("projectid"));

                        return new PostResponseModel()
                        {
                            Message = "签到情况查询成功",
                            Result = Enums.APIResult.Success,
                            ExtResult = { { "结果", user.querySignIn(0) } },
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
            public static PostResponseModel _TeacherSignIn(PostInparamModel value)
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
                        user.TeacherSignIn(value.Params.Get("names").Split(","));

                        return new PostResponseModel()
                        {
                            Message = "补签成功",
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
            #endregion


            #region 校车
            public static PostResponseModel _QueryBus(PostInparamModel value)
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
                        return new PostResponseModel()
                        {
                            Message = "获取校车",
                            Result = Enums.APIResult.Success,
                            ExtResult = {
                                { "class", user.QueryClassTeacher().ConvertClassInstance() }
                            },
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
            public static PostResponseModel _ModifyBus(PostInparamModel value)
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
                        return new PostResponseModel()
                        {
                            Message = "获取课程",
                            Result = Enums.APIResult.Success,
                            ExtResult = {
                                { "class", user.QueryClassTeacher().ConvertClassInstance() }
                            },
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
            public static PostResponseModel _DeleteBus(PostInparamModel value)
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
                        return new PostResponseModel()
                        {
                            Message = "获取课程",
                            Result = Enums.APIResult.Success,
                            ExtResult = {
                                { "class", user.QueryClassTeacher().ConvertClassInstance() }
                            },
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
            #endregion

            #region 任务 
            public static PostResponseModel _CreateMission(PostInparamModel value)
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
                        user.CreateMission(value.Params.Get("ClassID").toInt(), value.Params.Get("Name"));
                        return new PostResponseModel()
                        {
                            Message = "创建任务",
                            Result = Enums.APIResult.Success,
                            ExtResult = {
                                
                            },
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
            public static PostResponseModel _QueryMission(PostInparamModel value)
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
                        var missions = user.QueryMissions();
                        var result= new PostResponseModel()
                        {
                            Message = "获取任务集",
                            Result = Enums.APIResult.Success,
                            ExtResult = {
                                
                            },
                            UserLoginToken = FrameCorex.ServiceInstanceInfo(server).LoginHashToken
                        };

                        foreach(var t in missions)
                        {
                            result.ExtResult.Add(t.ID + "", t.ConvertMission());
                        }

                        return result;

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
            public static PostResponseModel _FinishMission(PostInparamModel value)
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

                        user.FinishMission(value.Params.Get("MissionID").toInt());
                        return new PostResponseModel()
                        {
                            Message = "任务完成",
                            Result = Enums.APIResult.Success,
                            ExtResult = {
                                
                            },
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
            #endregion

            #region 请假
            public static PostResponseModel _RequestLeave(PostInparamModel value)
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
                        user.RequestLeave(Convert.ToInt32(value.Params.Get("ClassSelectID")));
                        return new PostResponseModel()
                        {
                            Message = "请假成功",
                            Result = Enums.APIResult.Success,
                            ExtResult = {
                            },
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
            public static PostResponseModel _QueryLeave(PostInparamModel value)
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
                        var ret= new PostResponseModel()
                        {
                            Message = "获取请假单",
                            Result = Enums.APIResult.Success,
                            ExtResult = {
                            },
                            UserLoginToken = FrameCorex.ServiceInstanceInfo(server).LoginHashToken
                        };
                        foreach(var t in user.QueryLeave())
                        {
                            ret.ExtResult.Add(t.ID + "", t.ConvertLeave());
                        }
                        return ret;



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
            public static PostResponseModel _CancelLeave(PostInparamModel value)
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
                        user.CancelLeave(Convert.ToInt32(value.Params.Get("LeaveID")));
                        return new PostResponseModel()
                        {
                            Message = "撤销请假成功",
                            Result = Enums.APIResult.Success,
                            ExtResult = {
                            },
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
            public static PostResponseModel _ApproveLeave(PostInparamModel value)
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
                        user.ApproveLeave(Convert.ToInt32(value.Params.Get("LeaveID")));
                        return new PostResponseModel()
                        {
                            Message = "批准假单成功",
                            Result = Enums.APIResult.Success,
                            ExtResult = {
                            },
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
            #endregion

        }
    }
}
