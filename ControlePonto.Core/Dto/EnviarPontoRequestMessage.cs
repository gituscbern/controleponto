using ControlePonto.Core.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlePonto.Core.Dto
{
    public class EnviarPontoRequestMessage : IRequest<EnviarPontoResponseMessage>
    {
        public string SheetName { get; set; }
        public string Email { get; set; }

        public EnviarPontoRequestMessage(string sheetName, string email)
        {
            SheetName = sheetName;
            Email = email;
        }
    }
}
