using ControlePonto.Core.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlePonto.Adapters.Presentation
{
    public class Presenter
    {
        public ResponseViewModel Handle(ResponseMessage responseMessage)
        {
            if (responseMessage.Success)
            {
                return new ResponseViewModel(true, "Planilha configurada com sucesso!");
            }

            var sb = new StringBuilder();
            sb.AppendLine("Falha ao configurar planilha");
            //foreach (var e in responseMessage.Errors)
            //{
            //    sb.AppendLine(e);
            //}

            return new ResponseViewModel(false, sb.ToString());
        }
    }
}
