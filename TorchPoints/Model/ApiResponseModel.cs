using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TorchPoints.Model
{
    /// <summary>
    /// api返回模型
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ApiResponseModel<T>
    {
        /// <summary>
        /// 業務數據
        /// </summary>
        public T Data { get; protected set; }

        /// <summary>
        /// 接口調用標識
        /// </summary>
        public int Success { get; protected set; }

        /// <summary>
        /// 列表查詢時返回總條數
        /// </summary>
        public int TotalCount { get; protected set; }

        /// <summary>
        /// 接口狀態，僅異常或處理失敗是賦值
        /// </summary>
        public ResponseStatus Status { get; protected set; }

        private ApiResponseModel() { }

        public ApiResponseModel(StatusCode statusCode, string message)
            : this(new ResponseStatus(statusCode, message))
        {
        }

        public ApiResponseModel(T data, StatusCode statusCode, string message)
            : this(new ResponseStatus(statusCode, message))
        {
            this.Data = data;
            this.Success = 1;
            this.TotalCount = 0;
        }

        public ApiResponseModel(ResponseStatus status)
        {
            this.Status = status;
            this.Success = 0;
        }

        public ApiResponseModel(T data, int count = 0)
        {
            this.Data = data;
            this.Success = 1;
            this.TotalCount = count;
        }
        /// <summary>
        /// 请求失败时使用
        /// </summary>
        /// <param name="data"></param>
        /// <param name="statusCode"></param>
        /// <param name="message"></param>
        /// <param name="success">调用是否成功</param>
        public ApiResponseModel(T data, StatusCode statusCode, string message, int success = 0)
          : this(new ResponseStatus(statusCode, message))
        {
            this.Data = data;
            this.Success = success;
            this.TotalCount = 0;
        }
    }

    public class ResponseStatus
    {
        private ResponseStatus()
        {
        }

        public ResponseStatus(StatusCode statusCode, string message)
        {
            StatusCode = (int)statusCode;
            Message = message;
        }

        public int StatusCode { get; set; }

        public string Message { get; set; }
    }


    public enum StatusCode
    {
        #region 異常碼，10000至19999之間

        /// <summary>
        /// 運行期異常
        /// </summary>
        RuntimeError = 10000,

        /// <summary>
        /// 連接超時
        /// </summary>
        TimeoutError = 10001,

        /// <summary>
        /// 未受支持的文檔類型
        /// </summary>
        UnsupportedMediaTypeError = 10002,

        #endregion

        #region   业务警告码 20000开始
        /// <summary>
        /// 操作未授權
        /// </summary>
        NoPermissionAlert = 20001,

        /// <summary>
        /// 未做任何處理
        /// </summary>
        UnhandledAlert = 20011,

        /// <summary>
        /// 部分請求被處理
        /// </summary>
        PartialHandled = 20012,

        /// <summary>
        /// 請求處理失敗
        /// </summary>
        HandledFaild = 20013,

        /// <summary>
        /// 參數為空
        /// </summary>
        NullParameterAlert = 20021,

        /// <summary>
        /// 參數無效
        /// </summary>
        InvalidParameterAlert = 20022
        #endregion
    }
}
