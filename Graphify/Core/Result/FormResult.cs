using MelofyAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Result
{
    public class FormResult<T>
    {
        public bool Success { get; set; }
        public string? Message { get; set; }
        public T? Data { get; set; }
        public List<MessageResult>? Responses { get; set; }

        public FormResult(bool success, T data, string message, List<MessageResult>? responses)
        {
            this.Success = success;
            this.Message = message;
            this.Data = data;
            this.Responses = responses;
        }
    }
}
