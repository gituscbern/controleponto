using ControlePonto.Core.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlePonto.Core.Dto
{
    public class EnviarPlanilhasRequestMessage : IRequest<EnviarPlanilhasResponseMessage>
    {
        public string SheetName { get; set; }
        public EnviarPlanilhasRequestMessage(string sheetName)
        {
            SheetName = sheetName;
        }
    }
}
