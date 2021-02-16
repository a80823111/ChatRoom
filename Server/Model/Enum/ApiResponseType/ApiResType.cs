using System;
using System.Collections.Generic;
using System.Text;

namespace Model.Enum.ApiResponseType
{
    public enum ApiResType
    {
        /// <summary>
        /// 執行成功
        /// </summary>
        Success = 0,
        /// <summary>
        /// 執行失敗
        /// </summary>
        Failed = 1,
        /// <summary>
        /// 其他例外
        /// </summary>
        Exception = 2,

    }
}
