using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlePonto.Adapters.Presentation
{
    public class ResponseViewModel
    {
        public bool Success { get; private set; }
        public string ResultMessage { get; private set; }

        public ResponseViewModel(bool success, string resultMessage)
        {
            Success = success;
            ResultMessage = resultMessage;
        }
    }
}
