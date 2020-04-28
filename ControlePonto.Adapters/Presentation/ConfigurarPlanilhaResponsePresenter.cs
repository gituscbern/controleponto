using ControlePonto.Core.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlePonto.Adapters.Presentation
{
    public class ConfigurarPlanilhaResponsePresenter
    {
        public ConfigurarPlanilhaResponseViewModel Handle(ConfigurarPlanilhaResponseMessage responseMessage)
        {
            if (responseMessage.Success)
            {
                return new ConfigurarPlanilhaResponseViewModel(true, "Planilha configurada com sucesso!");
            }

            var sb = new StringBuilder();
            sb.AppendLine("Falha ao configurar planilha");
            foreach (var e in responseMessage.Errors)
            {
                sb.AppendLine(e);
            }

            return new ConfigurarPlanilhaResponseViewModel(false, sb.ToString());
        }
    }
}
