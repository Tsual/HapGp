using HapGp.Attribute;
using System;

namespace HapGp.Enums
{
    public enum AppConfigEnum {
        /// <summary>
        /// 应用默认加密对象
        /// </summary>
        AppAesObj,

        FirstInstallFlag,

        AppAesIV,

        AppAesKey,

        AppDBex,

        [AppConfigDefault("0")]
        RandomStringCount,

        [AppConfigDefault("45")]
        ServiceDropTime,

        /// <summary>
        /// Ava>cur * ServiceInstanceObjectDestroylimit 将不再回收对象
        /// </summary>
        [AppConfigDefault("8")]
        ServiceInstanceObjectDestroylimit


    }
}
