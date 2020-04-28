using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlePonto.Core.Contracts
{
    public abstract class ResponseMessage
    {
        public bool Success { get; private set; }
        public string Message { get; private set; }

        protected ResponseMessage(bool success, string message = null)
        {
            Success = success;
            Message = message;
        }
    }
}
