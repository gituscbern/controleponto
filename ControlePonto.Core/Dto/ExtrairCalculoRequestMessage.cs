using ControlePonto.Core.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlePonto.Core.Dto
{
    public class ExtrairCalculoRequestMessage : IRequest<ExtrairCalculoResponseMessage>
    {
        public string CaminhoPlanilhaRelatorio;
        public int Mes;

        public ExtrairCalculoRequestMessage(string caminhoPlanilhaRelatorio)
        {
            CaminhoPlanilhaRelatorio = caminhoPlanilhaRelatorio;
        }

        public ExtrairCalculoRequestMessage(string caminhoPlanilhaRelatorio, int mes)
        {
            Mes = mes;
            CaminhoPlanilhaRelatorio = caminhoPlanilhaRelatorio;
        }
    }
}
