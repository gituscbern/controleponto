using ControlePonto.Core.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlePonto.Core.Dto
{
    public class EnviarPontoResponseMessage : ResponseMessage
    {
        public List<string> Errors { get; private set; }
        public EnviarPontoResponseMessage(bool success, List<string> errors, string message = null) : base(success, message)
        {
            Errors = errors;
        }
    }
}
