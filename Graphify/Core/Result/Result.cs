using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Result
{
    public class Result<T>
    {
        public bool Success { get; set; }
        public string? Message { get; set; }
        public T? Data { get; set; }

        public Result(bool success, T data, string message)
        {
            this.Success = success;
            this.Message = message;
            this.Data = data;
        }
    }
}
