using ControlePonto.Core.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlePonto.Core.Dto
{
    public class ExtrairColaboradoresPendentesResponseMessage : ResponseMessage
    {
        public List<string> Errors { get; private set; }
        public ExtrairColaboradoresPendentesResponseMessage (bool success, List<string> errors, string message = null) : base(success, message)
        {
            Errors = errors;
        }
}
}
