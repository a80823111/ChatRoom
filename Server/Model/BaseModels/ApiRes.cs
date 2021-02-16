using Model.Enum.ApiResponseType;
using System;
using System.Collections.Generic;
using System.Text;

namespace Model.BaseModels
{
    public class ApiRes<T>
    {
        /// <summary>
        /// 操作狀態碼 ApiStatusType
        /// </summary>
        public T Status { get; set; }

        /// <summary>
        /// 描述訊息
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// 錯誤訊息
        /// </summary>
        public string Errors { get; set; }

        /// <summary>
        /// 結果
        /// </summary>
        public object Result { get; set; }
    }

    public class ApiRes
    {
        /// <summary>
        /// 操作狀態碼 ApiStatusType
        /// </summary>
        public ApiResType Status { get; set; }

        /// <summary>
        /// 描述訊息
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// 錯誤訊息
        /// </summary>
        public string Errors { get; set; }

        /// <summary>
        /// 結果
        /// </summary>
        public object Result { get; set; }
    }
}
